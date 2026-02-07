using System;
using System.Collections.Generic;
using System.Threading;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;
using Scythe.Multiplayer.Messages;
using Scythe.Utilities;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x0200020E RID: 526
	public class ChatLogic : SingletonMono<ChatLogic>
	{
		// Token: 0x06000FAE RID: 4014 RVA: 0x0003224B File Offset: 0x0003044B
		private void OnEnable()
		{
			ChatLogic.restoreChatReady = true;
			MultiplayerController.OnChatMessageReceived += this.ChatMessageRecived;
			MultiplayerController.OnChatMessageRestored += this.ChatMessageRestored;
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x00032275 File Offset: 0x00030475
		private void OnDisable()
		{
			MultiplayerController.OnChatMessageReceived -= this.ChatMessageRecived;
			MultiplayerController.OnChatMessageRestored -= this.ChatMessageRestored;
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00032299 File Offset: 0x00030499
		private void Update()
		{
			this.UpdateCensoring();
			this.UpdateSending();
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0008E328 File Offset: 0x0008C528
		public void EnqueueChatMessageToCensoringAndSending(ChatMessage chatMessage, ChatLogic.ChatType chatType)
		{
			chatMessage.SetPlayerName(PlayerInfo.me.PlayerStats.Name);
			chatMessage.SetIsGlobalChat(chatType == ChatLogic.ChatType.Global);
			chatMessage.SetIsLobbyChat(this.chatLocation == ChatLogic.ChatLocation.Lobby);
			if (this.chatLocation == ChatLogic.ChatLocation.Ingame)
			{
				chatMessage.SetFacionPrefix("^" + ((Faction)PlayerInfo.me.Faction).ToString()[0].ToString());
			}
			this.createdMessages.Enqueue(chatMessage);
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0008E3B0 File Offset: 0x0008C5B0
		public bool HasUnreadMessages(ChatLogic.ChatType chatType)
		{
			if (chatType == ChatLogic.ChatType.Global)
			{
				return this.globalMessages.Count > 0;
			}
			if (chatType == ChatLogic.ChatType.Room)
			{
				return this.roomMessages.Count > 0;
			}
			Debug.LogError("Unrecognized chat type " + chatType.ToString());
			return false;
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0008E400 File Offset: 0x0008C600
		public ChatMessage GetUnreadChatMessage(ChatLogic.ChatType chatType)
		{
			if (chatType == ChatLogic.ChatType.Global)
			{
				if (this.globalMessages.Count > 0)
				{
					return this.globalMessages.Dequeue();
				}
				return null;
			}
			else
			{
				if (chatType != ChatLogic.ChatType.Room)
				{
					Debug.LogError("Unrecognized chat type " + chatType.ToString());
					return null;
				}
				if (this.roomMessages.Count > 0)
				{
					return this.roomMessages.Dequeue();
				}
				return null;
			}
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x000322A7 File Offset: 0x000304A7
		public bool CanRestoreMessages()
		{
			return ChatLogic.restoredMessages.Count > 0 && ChatLogic.restoreChatReady;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x000322BD File Offset: 0x000304BD
		public ChatMessage GetRestoredChatMessage()
		{
			if (ChatLogic.restoredMessages.Count > 0)
			{
				return ChatLogic.restoredMessages.Dequeue();
			}
			return null;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0008E46C File Offset: 0x0008C66C
		private void UpdateCensoring()
		{
			if (!this.censorThreadWorking && this.createdMessages.Count > 0)
			{
				ChatMessage messageToCensor = this.createdMessages.Dequeue();
				new Thread(delegate
				{
					this.CensorChatMessage(messageToCensor);
				}).Start();
			}
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0008E4C4 File Offset: 0x0008C6C4
		private void CensorChatMessage(ChatMessage chatMessage)
		{
			this.censorThreadWorking = true;
			string text = Censor.Process(chatMessage.GetMessage());
			chatMessage.SetMessage(text);
			this.censoredMessages.Enqueue(chatMessage);
			this.censorThreadWorking = false;
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0008E500 File Offset: 0x0008C700
		private void UpdateSending()
		{
			while (this.censoredMessages.Count > 0)
			{
				ChatMessage chatMessage = this.censoredMessages.Dequeue();
				this.SendMessage(chatMessage);
				this.ChatMessageRecived(chatMessage);
			}
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0008E538 File Offset: 0x0008C738
		private void SendMessage(ChatMessage chatMessage)
		{
			if (this.chatLocation == ChatLogic.ChatLocation.Ingame)
			{
				MultiplayerController.Instance.SendMessageToOtherPlayers(chatMessage);
				return;
			}
			RequestController.RequestPostCall("Room/Message", RequestController.GenerateStringFromMessage<Scythe.Multiplayer.Data.Message>(new Scythe.Multiplayer.Data.Message(GameSerializer.JsonMessageSerializer<ChatMessage>(chatMessage).Replace("Assembly-CSharp", "ScytheWebRole"))), false, delegate(string response)
			{
			}, null);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x000322D8 File Offset: 0x000304D8
		private void ChatMessageRecived(ChatMessage chatMessage)
		{
			if (chatMessage.IsGlobalChat())
			{
				this.globalMessages.Enqueue(chatMessage);
				return;
			}
			this.roomMessages.Enqueue(chatMessage);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0008E5A8 File Offset: 0x0008C7A8
		private void ChatMessageRestored(List<ChatMessage> chatMessages)
		{
			ChatLogic.restoredMessages.Clear();
			foreach (ChatMessage chatMessage in chatMessages)
			{
				ChatLogic.restoredMessages.Enqueue(chatMessage);
			}
			ChatLogic.restoreChatReady = false;
		}

		// Token: 0x04000C22 RID: 3106
		[SerializeField]
		private ChatLogic.ChatLocation chatLocation;

		// Token: 0x04000C23 RID: 3107
		private Queue<ChatMessage> createdMessages = new Queue<ChatMessage>();

		// Token: 0x04000C24 RID: 3108
		private Queue<ChatMessage> censoredMessages = new Queue<ChatMessage>();

		// Token: 0x04000C25 RID: 3109
		private bool censorThreadWorking;

		// Token: 0x04000C26 RID: 3110
		private Queue<ChatMessage> globalMessages = new Queue<ChatMessage>();

		// Token: 0x04000C27 RID: 3111
		private Queue<ChatMessage> roomMessages = new Queue<ChatMessage>();

		// Token: 0x04000C28 RID: 3112
		private static Queue<ChatMessage> restoredMessages = new Queue<ChatMessage>();

		// Token: 0x04000C29 RID: 3113
		private static bool restoreChatReady = false;

		// Token: 0x0200020F RID: 527
		public enum ChatType
		{
			// Token: 0x04000C2B RID: 3115
			Global,
			// Token: 0x04000C2C RID: 3116
			Room
		}

		// Token: 0x02000210 RID: 528
		public enum ChatLocation
		{
			// Token: 0x04000C2E RID: 3118
			Lobby,
			// Token: 0x04000C2F RID: 3119
			Ingame
		}
	}
}

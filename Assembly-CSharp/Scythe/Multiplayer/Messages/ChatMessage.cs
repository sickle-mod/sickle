using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002C3 RID: 707
	public class ChatMessage : Message, IExecutableMessage
	{
		// Token: 0x060015D0 RID: 5584 RVA: 0x00036DCC File Offset: 0x00034FCC
		public string GetMessage()
		{
			return this.message;
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x00036DD4 File Offset: 0x00034FD4
		public string GetPlayerName()
		{
			return this.name;
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x00036DDC File Offset: 0x00034FDC
		public string GetFactionPrefix()
		{
			return this.factionPrefix;
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x00036DE4 File Offset: 0x00034FE4
		public bool IsLobbyChat()
		{
			return this.isLobbyChat;
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x00036DEC File Offset: 0x00034FEC
		public bool IsGlobalChat()
		{
			return this.isGlobalChat;
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x00036DF4 File Offset: 0x00034FF4
		public void SetMessage(string newMessage)
		{
			this.message = newMessage;
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x00036DFD File Offset: 0x00034FFD
		public void SetPlayerName(string newPlayerName)
		{
			this.name = newPlayerName;
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x00036E06 File Offset: 0x00035006
		public void SetFacionPrefix(string factionPrefix)
		{
			this.factionPrefix = factionPrefix;
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x00036E0F File Offset: 0x0003500F
		public void SetIsLobbyChat(bool isLobbyChat)
		{
			this.isLobbyChat = isLobbyChat;
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00036E18 File Offset: 0x00035018
		public void SetIsGlobalChat(bool isGlobalChat)
		{
			this.isGlobalChat = isGlobalChat;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x00036E21 File Offset: 0x00035021
		public ChatMessage(bool isLobbyChat, bool isGlobalChat, string factionPrefix, string playerName, string message)
		{
			this.isLobbyChat = isLobbyChat;
			this.isGlobalChat = isGlobalChat;
			this.factionPrefix = factionPrefix;
			this.name = playerName;
			this.message = message;
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x00036E4E File Offset: 0x0003504E
		public ChatMessage(string message)
		{
			this.isLobbyChat = false;
			this.isGlobalChat = false;
			this.factionPrefix = string.Empty;
			this.name = string.Empty;
			this.message = message;
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x00036E81 File Offset: 0x00035081
		public void Execute(GameManager gameManager)
		{
			MultiplayerController.Instance.ReceivedChatMessage(this);
		}

		// Token: 0x04001013 RID: 4115
		private bool isLobbyChat;

		// Token: 0x04001014 RID: 4116
		private bool isGlobalChat;

		// Token: 0x04001015 RID: 4117
		private string factionPrefix;

		// Token: 0x04001016 RID: 4118
		private string name;

		// Token: 0x04001017 RID: 4119
		private string message;
	}
}

using System;
using System.Collections.Generic;
using System.Threading;
using HoneyFramework;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;
using Scythe.Multiplayer.Messages;
using Scythe.UI;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x0200020B RID: 523
	public class Chat : MonoBehaviour
	{
		// Token: 0x06000F8C RID: 3980 RVA: 0x0008D580 File Offset: 0x0008B780
		private void OnEnable()
		{
			this.Init();
			Chat.restoreChatReady = true;
			MultiplayerController.OnChatMessageReceived += this.ChatMessegeRecieved;
			MultiplayerController.OnChatMessageRestored += this.ChatMessageRestored;
			if (this.chatSpacePlaceholder != null && !this.isLobbyChat)
			{
				this.chatSpacePlaceholder.SetActive(false);
			}
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0008D5E0 File Offset: 0x0008B7E0
		private void OnDisable()
		{
			MultiplayerController.OnChatMessageReceived -= this.ChatMessegeRecieved;
			MultiplayerController.OnChatMessageRestored -= this.ChatMessageRestored;
			if (this.chatSpacePlaceholder != null && !this.isLobbyChat)
			{
				this.chatSpacePlaceholder.SetActive(false);
			}
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0008D634 File Offset: 0x0008B834
		private void Update()
		{
			if (this.refreshScrollingSystem)
			{
				this.ResetScroll();
			}
			if (this.awaitingMessagesToCensor.Count > 0 && !this.calculatingCensor)
			{
				KeyValuePair<ChatMessage, Transform> message = this.awaitingMessagesToCensor.Dequeue();
				new Thread(delegate
				{
					this.CalculateCensor(message.Key, message.Value);
				}).Start();
			}
			while (this.awaitingCensuredMessages.Count > 0)
			{
				KeyValuePair<ChatMessage, Transform> keyValuePair = this.awaitingCensuredMessages.Dequeue();
				this.SendMessage(keyValuePair.Key);
				if (this.isLobbyChat)
				{
					ChatMessage key = keyValuePair.Key;
					if (key.GetPlayerName() == PlayerInfo.me.PlayerStats.Name)
					{
						keyValuePair.Value.gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("<color=#F2C94C>{0}: {1}</color>", key.GetPlayerName(), key.GetMessage());
					}
					else
					{
						keyValuePair.Value.gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("<color=#888785>{0}</color>: {1}", key.GetPlayerName(), key.GetMessage());
					}
				}
				else
				{
					keyValuePair.Value.gameObject.GetComponent<TextMeshProUGUI>().text = "<b>" + keyValuePair.Key.GetPlayerName() + "</b>: " + keyValuePair.Key.GetMessage();
				}
			}
			if (Chat.restoreChatReady && !this.isLobbyChat && !this.isLobbyGlobalChat)
			{
				while (Chat.restoredMessages.Count > 0)
				{
					this.AddMessage(Chat.restoredMessages.Dequeue());
				}
				Chat.restoreChatReady = false;
			}
			if (this.awaitingMessages.Count > 0)
			{
				while (this.awaitingMessages.Count != 0)
				{
					this.AddMessage(this.awaitingMessages.Dequeue());
				}
			}
			this.KeyEventsHandling();
			if (this.chatDragged && !this.isLobbyChat)
			{
				this.MoveChat();
			}
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0008D820 File Offset: 0x0008BA20
		private void Init()
		{
			this.chatRectTransform = base.gameObject.GetComponent<RectTransform>();
			Vector3[] array = new Vector3[4];
			this.chatRectTransform.GetWorldCorners(array);
			this.chatSpace.GetWorldCorners(array);
			this.awaitingMessages = new Queue<ChatMessage>();
			this.awaitingCensuredMessages = new Queue<KeyValuePair<ChatMessage, Transform>>();
			this.awaitingMessagesToCensor = new Queue<KeyValuePair<ChatMessage, Transform>>();
			this.calculatingCensor = false;
			for (int i = 0; i < this.maxEntries; i++)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.chatEntryPrefab, Vector3.zero, Quaternion.identity, this.chatEntries.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.SetActive(false);
			}
			if (!this.isLobbyChat)
			{
				this.CreateInfoMessageAboutPlayers();
				if (!MultiplayerController.Instance.IsMultiplayer || MultiplayerController.Instance.SpectatorMode)
				{
					this.inputField.interactable = false;
					this.inputField.gameObject.SetActive(false);
				}
			}
			this.antiSpamTimer = new AntiSpamTimer(5, 2f, 5f);
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0008D928 File Offset: 0x0008BB28
		public void ChatMessegeRecieved(ChatMessage message)
		{
			if (message.IsLobbyChat() == this.isLobbyChat && this.isLobbyGlobalChat == message.IsGlobalChat())
			{
				if (message.GetPlayerName() != PlayerInfo.me.PlayerStats.Name)
				{
					this.awaitingMessages.Enqueue(message);
					return;
				}
			}
			else if (!this.isLobbyChat)
			{
				this.awaitingMessages.Enqueue(message);
			}
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0008D990 File Offset: 0x0008BB90
		private void ChatMessageRestored(List<ChatMessage> messages)
		{
			Chat.restoredMessages.Clear();
			foreach (ChatMessage chatMessage in messages)
			{
				Chat.restoredMessages.Enqueue(chatMessage);
			}
			Chat.restoreChatReady = false;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0008D9F4 File Offset: 0x0008BBF4
		private void CreateInfoMessageAboutPlayers()
		{
			foreach (PlayerData playerData in MultiplayerController.Instance.GetPlayersInGame())
			{
				string text = "^" + ((Faction)playerData.Faction).ToString()[0].ToString();
				this.AddMessage(new ChatMessage(false, false, text, playerData.Name, ((Faction)playerData.Faction).ToString()));
			}
			this.AddMessage(new ChatMessage(false, false, string.Empty, string.Empty, string.Empty));
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0008DAB4 File Offset: 0x0008BCB4
		public Transform AddMessage(ChatMessage message)
		{
			Transform child = this.chatEntries.transform.GetChild(0);
			if (!this.isLobbyChat)
			{
				if (message.GetFactionPrefix().StartsWith("^"))
				{
					char c = message.GetFactionPrefix()[1];
					if (c != 'A')
					{
						if (c != 'C')
						{
							switch (c)
							{
							case 'N':
								child.GetChild(0).GetComponent<Image>().sprite = GameController.factionInfo[Faction.Nordic].logo;
								break;
							case 'P':
								child.GetChild(0).GetComponent<Image>().sprite = GameController.factionInfo[Faction.Polania].logo;
								break;
							case 'R':
								child.GetChild(0).GetComponent<Image>().sprite = GameController.factionInfo[Faction.Rusviet].logo;
								break;
							case 'S':
								child.GetChild(0).GetComponent<Image>().sprite = GameController.factionInfo[Faction.Saxony].logo;
								break;
							case 'T':
								child.GetChild(0).GetComponent<Image>().sprite = GameController.factionInfo[Faction.Togawa].logo;
								break;
							}
						}
						else
						{
							child.GetChild(0).GetComponent<Image>().sprite = GameController.factionInfo[Faction.Crimea].logo;
						}
					}
					else
					{
						child.GetChild(0).GetComponent<Image>().sprite = GameController.factionInfo[Faction.Albion].logo;
					}
					child.GetChild(0).GetComponent<Image>().enabled = true;
				}
				else if (child.childCount > 0)
				{
					child.GetChild(0).GetComponent<Image>().enabled = false;
				}
			}
			if (!this.isLobbyChat)
			{
				if (message.GetPlayerName() != string.Empty)
				{
					child.gameObject.GetComponent<TextMeshProUGUI>().text = "<b>" + message.GetPlayerName() + "</b>: " + message.GetMessage();
				}
				else
				{
					child.gameObject.GetComponent<TextMeshProUGUI>().text = message.GetMessage();
				}
			}
			else if (message.GetPlayerName() != string.Empty)
			{
				if (message.GetPlayerName() == PlayerInfo.me.PlayerStats.Name)
				{
					child.gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("<color=#F2C94C>{0}: {1}</color>", message.GetPlayerName(), message.GetMessage());
				}
				else
				{
					child.gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("<color=#888785>{0}</color>: {1}", message.GetPlayerName(), message.GetMessage());
				}
			}
			else
			{
				child.gameObject.GetComponent<TextMeshProUGUI>().text = message.GetMessage();
			}
			child.SetAsLastSibling();
			child.gameObject.SetActive(true);
			if (!this.isLobbyChat && !this.chatElements.activeInHierarchy && !MultiplayerController.Instance.ReturningToStartedGame && message.GetPlayerName() != string.Empty)
			{
				this.HandleNotSeenMessage();
			}
			this.refreshScrollingSystem = true;
			return child;
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0008DDB0 File Offset: 0x0008BFB0
		private void HandleNotSeenMessage()
		{
			if (!this.unseenMessage)
			{
				this.unseenMessage = true;
				this.newMessageIndicator.SetActive(true);
			}
			this.unseenMessagesCount++;
			this.newMessageCount.text = ((this.unseenMessagesCount >= 10) ? "9<size=6.54>+</size>" : this.unseenMessagesCount.ToString());
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0003212D File Offset: 0x0003032D
		public void ResetScroll()
		{
			if (!this.scrollbarDragged)
			{
				Canvas.ForceUpdateCanvases();
				this.messageArea.verticalNormalizedPosition = 0f;
			}
			this.refreshScrollingSystem = false;
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0008DE10 File Offset: 0x0008C010
		public void ClearChat()
		{
			foreach (object obj in this.chatEntries.transform)
			{
				((Transform)obj).gameObject.SetActive(false);
			}
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0008DE74 File Offset: 0x0008C074
		private void KeyEventsHandling()
		{
			if (this.isFocused)
			{
				if ((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) && this.inputField.IsActive())
				{
					this.SendChatMessage();
					this.inputField.ActivateInputField();
					if (SceneManager.GetActiveScene().name == "main")
					{
						KeyboardShortcuts.Instance.isSendingChatMsg = false;
					}
				}
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					this.isFocused = false;
					CameraControler.CameraMovementBlocked = false;
					this.inputField.DeactivateInputField(false);
					if (PlatformManager.IsStandalone)
					{
						KeyboardShortcuts.Instance.isCloseChatWindow = false;
					}
				}
			}
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00032153 File Offset: 0x00030353
		private void ResetNewMessageIndicator()
		{
			if (this.unseenMessage)
			{
				this.unseenMessage = false;
				this.newMessageIndicator.SetActive(false);
				this.unseenMessagesCount = 0;
			}
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0008DF18 File Offset: 0x0008C118
		public void SendChatMessage()
		{
			if (this.inputField.text.Length != 0)
			{
				string text = string.Empty;
				if (!this.isLobbyChat)
				{
					text = text + "^" + ((Faction)PlayerInfo.me.Faction).ToString()[0].ToString();
				}
				if (this.antiSpamTimer.MakeACall())
				{
					ChatMessage chatMessage = new ChatMessage(this.isLobbyChat, this.isLobbyGlobalChat, text, PlayerInfo.me.PlayerStats.Name, this.inputField.text);
					Transform transform = this.AddMessage(chatMessage);
					this.inputField.text = "";
					this.awaitingMessagesToCensor.Enqueue(new KeyValuePair<ChatMessage, Transform>(chatMessage, transform));
					return;
				}
				this.CreateAntiSpamMessage();
				this.inputField.text = "";
			}
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0008DFF8 File Offset: 0x0008C1F8
		private void CalculateCensor(ChatMessage message, Transform messageObject)
		{
			this.calculatingCensor = true;
			string text = Censor.Process(message.GetMessage());
			message.SetMessage(text);
			this.awaitingCensuredMessages.Enqueue(new KeyValuePair<ChatMessage, Transform>(message, messageObject));
			this.calculatingCensor = false;
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0008E038 File Offset: 0x0008C238
		private void SendMessage(ChatMessage message)
		{
			if (!this.isLobbyChat)
			{
				MultiplayerController.Instance.SendMessageToOtherPlayers(message);
				return;
			}
			RequestController.RequestPostCall("Room/Message", RequestController.GenerateStringFromMessage<Scythe.Multiplayer.Data.Message>(new Scythe.Multiplayer.Data.Message(GameSerializer.JsonMessageSerializer<ChatMessage>(message).Replace("Assembly-CSharp", "ScytheWebRole"))), false, delegate(string response)
			{
			}, null);
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x0008E0A4 File Offset: 0x0008C2A4
		private void CreateAntiSpamMessage()
		{
			ChatMessage chatMessage = new ChatMessage(this.isLobbyChat, this.isLobbyGlobalChat, string.Empty, string.Empty, "<color=red>" + ScriptLocalization.Get("Lobby/ChatSpamWarning") + "</color>");
			this.AddMessage(chatMessage);
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00032177 File Offset: 0x00030377
		private void ChangeCameraMovementState(bool canMove)
		{
			CameraControler.CameraMovementBlocked = canMove;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0003217F File Offset: 0x0003037F
		public void OnScrollbarBeginDrag(BaseEventData data)
		{
			this.ChangeCameraMovementState(true);
			this.scrollbarDragged = true;
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0003218F File Offset: 0x0003038F
		public void OnScrollbarEndingDrag(BaseEventData data)
		{
			this.ChangeCameraMovementState(false);
			this.scrollbarDragged = false;
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0003219F File Offset: 0x0003039F
		public void OnChatBeginMoved()
		{
			if (!this.isLobbyChat)
			{
				this.chatDragged = true;
				this.lastMousePosition = Input.mousePosition;
				this.ChangeCameraMovementState(true);
			}
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x000321C2 File Offset: 0x000303C2
		public void OnChatEndedMove()
		{
			if (!this.isLobbyChat)
			{
				this.chatDragged = false;
				this.ChangeCameraMovementState(false);
			}
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0008E0F0 File Offset: 0x0008C2F0
		private void MoveChat()
		{
			Vector3 vector = this.lastMousePosition - Input.mousePosition;
			Vector3 vector2 = this.chatRectTransform.position - vector;
			this.chatRectTransform.position = this.AdjustChatPosition(vector2, vector);
			this.lastMousePosition = Input.mousePosition;
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0008E140 File Offset: 0x0008C340
		private Vector3 AdjustChatPosition(Vector3 chatNewPosition, Vector3 offset)
		{
			Vector3[] array = new Vector3[4];
			Vector3[] array2 = new Vector3[4];
			this.chatHeaderRectTransform.GetWorldCorners(array);
			this.chatSpace.GetWorldCorners(array2);
			for (int i = 0; i < 4; i++)
			{
				array[i] -= offset;
			}
			if (array[0].y < array2[0].y)
			{
				chatNewPosition.y += array2[0].y - array[0].y;
			}
			else if (array[2].y > array2[2].y)
			{
				chatNewPosition.y += array2[2].y - array[2].y;
			}
			if (array[0].x < array2[0].x)
			{
				chatNewPosition.x += array2[0].x - array[0].x;
			}
			else if (array[2].x > array2[2].x)
			{
				chatNewPosition.x += array2[2].x - array[2].x;
			}
			return chatNewPosition;
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0008E294 File Offset: 0x0008C494
		public void ChangeChatVisibility()
		{
			WorldSFXManager.PlaySound(SoundEnum.GuiChatButton, AudioSourceType.Buttons);
			if (this.chatElements.activeInHierarchy)
			{
				this.chatElements.SetActive(false);
				if (this.chatSpacePlaceholder != null)
				{
					this.chatSpacePlaceholder.SetActive(false);
					return;
				}
				Debug.LogError("Placeholder not connected. Pointer area will be not affected by chat space.");
				return;
			}
			else
			{
				this.chatElements.SetActive(true);
				this.ResetNewMessageIndicator();
				this.refreshScrollingSystem = true;
				if (this.chatSpacePlaceholder != null)
				{
					this.chatSpacePlaceholder.SetActive(true);
					return;
				}
				Debug.LogError("Placeholder not connected. Pointer area will be not affected by chat space.");
				return;
			}
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x000321DA File Offset: 0x000303DA
		public void OnInputFieldEditBegin()
		{
			this.ChangeCameraMovementState(true);
			this.isFocused = true;
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x000321EA File Offset: 0x000303EA
		public void OnInputFieldEditEnd()
		{
			this.ChangeCameraMovementState(false);
			this.isFocused = false;
		}

		// Token: 0x04000BFF RID: 3071
		public GameObject chatSpacePlaceholder;

		// Token: 0x04000C00 RID: 3072
		[Range(5f, 100f)]
		public int maxEntries = 20;

		// Token: 0x04000C01 RID: 3073
		public bool isLobbyChat;

		// Token: 0x04000C02 RID: 3074
		public bool isLobbyGlobalChat;

		// Token: 0x04000C03 RID: 3075
		public RectTransform chatSpace;

		// Token: 0x04000C04 RID: 3076
		public RectTransform chatHeaderRectTransform;

		// Token: 0x04000C05 RID: 3077
		public TMP_InputField inputField;

		// Token: 0x04000C06 RID: 3078
		public GameObject chatElements;

		// Token: 0x04000C07 RID: 3079
		public GameObject chatEntryPrefab;

		// Token: 0x04000C08 RID: 3080
		public GameObject chatEntries;

		// Token: 0x04000C09 RID: 3081
		public ScrollRect messageArea;

		// Token: 0x04000C0A RID: 3082
		public GameObject newMessageIndicator;

		// Token: 0x04000C0B RID: 3083
		public TextMeshProUGUI newMessageCount;

		// Token: 0x04000C0C RID: 3084
		private Queue<KeyValuePair<ChatMessage, Transform>> awaitingCensuredMessages;

		// Token: 0x04000C0D RID: 3085
		private Queue<KeyValuePair<ChatMessage, Transform>> awaitingMessagesToCensor;

		// Token: 0x04000C0E RID: 3086
		private bool calculatingCensor;

		// Token: 0x04000C0F RID: 3087
		private Queue<ChatMessage> awaitingMessages;

		// Token: 0x04000C10 RID: 3088
		private static Queue<ChatMessage> restoredMessages = new Queue<ChatMessage>();

		// Token: 0x04000C11 RID: 3089
		private static bool restoreChatReady = false;

		// Token: 0x04000C12 RID: 3090
		private bool scrollbarDragged;

		// Token: 0x04000C13 RID: 3091
		private bool chatDragged;

		// Token: 0x04000C14 RID: 3092
		private Vector3 lastMousePosition;

		// Token: 0x04000C15 RID: 3093
		private RectTransform chatRectTransform;

		// Token: 0x04000C16 RID: 3094
		private bool unseenMessage;

		// Token: 0x04000C17 RID: 3095
		private int unseenMessagesCount;

		// Token: 0x04000C18 RID: 3096
		private bool refreshScrollingSystem;

		// Token: 0x04000C19 RID: 3097
		private AntiSpamTimer antiSpamTimer;

		// Token: 0x04000C1A RID: 3098
		private const int NUMBER_OF_MESSAGES_ALLOWED = 5;

		// Token: 0x04000C1B RID: 3099
		private const float REQUIRED_MESSAGES_INTERVAL = 2f;

		// Token: 0x04000C1C RID: 3100
		private const float SPAM_PENALTY_DURATION = 5f;

		// Token: 0x04000C1D RID: 3101
		public bool isFocused;
	}
}

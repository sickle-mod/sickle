using System;
using System.Collections.Generic;
using System.Threading;
using HoneyFramework;
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
	// Token: 0x0200027F RID: 639
	public class MobileChat : SingletonMono<MobileChat>
	{
		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x000358CF File Offset: 0x00033ACF
		public static bool IsSupported
		{
			get
			{
				return SingletonMono<MobileChat>.Instance != null;
			}
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x000358DC File Offset: 0x00033ADC
		private void OnEnable()
		{
			this.Init();
			MultiplayerController.OnChatMessageReceived += this.ChatMessageReceived;
			if (this.chatSpacePlaceholder != null && !this.isLobbyChat)
			{
				this.chatSpacePlaceholder.SetActive(false);
			}
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x00035917 File Offset: 0x00033B17
		private void OnDisable()
		{
			MultiplayerController.OnChatMessageReceived -= this.ChatMessageReceived;
			if (this.chatSpacePlaceholder != null && !this.isLobbyChat)
			{
				this.chatSpacePlaceholder.SetActive(false);
			}
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0009A330 File Offset: 0x00098530
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
					keyValuePair.Value.gameObject.GetComponent<Text>().text = "<b>" + keyValuePair.Key.GetPlayerName() + "</b>: " + keyValuePair.Key.GetMessage();
				}
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

		// Token: 0x06001425 RID: 5157 RVA: 0x0009A4E0 File Offset: 0x000986E0
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
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0009A5D0 File Offset: 0x000987D0
		public void ChatMessageReceived(ChatMessage message)
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

		// Token: 0x06001427 RID: 5159 RVA: 0x0009A638 File Offset: 0x00098838
		private void CreateInfoMessageAboutPlayers()
		{
			foreach (PlayerData playerData in MultiplayerController.Instance.GetPlayersInGame())
			{
				string text = "^" + ((Faction)playerData.Faction).ToString()[0].ToString();
				this.AddMessage(new ChatMessage(false, false, text, playerData.Name, ((Faction)playerData.Faction).ToString()));
			}
			this.AddMessage(new ChatMessage(false, false, string.Empty, string.Empty, string.Empty));
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0009A6F8 File Offset: 0x000988F8
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
					child.gameObject.GetComponent<Text>().text = "<b>" + message.GetPlayerName() + "</b>: " + message.GetMessage();
				}
				else
				{
					child.gameObject.GetComponent<Text>().text = message.GetMessage();
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

		// Token: 0x06001429 RID: 5161 RVA: 0x0009A9F4 File Offset: 0x00098BF4
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

		// Token: 0x0600142A RID: 5162 RVA: 0x0003594C File Offset: 0x00033B4C
		public void ResetScroll()
		{
			if (!this.scrollbarDragged)
			{
				Canvas.ForceUpdateCanvases();
				this.messageArea.verticalNormalizedPosition = 0f;
			}
			this.refreshScrollingSystem = false;
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0009AA54 File Offset: 0x00098C54
		public void ClearChat()
		{
			foreach (object obj in this.chatEntries.transform)
			{
				((Transform)obj).gameObject.SetActive(false);
			}
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0009AAB8 File Offset: 0x00098CB8
		private void KeyEventsHandling()
		{
			if (this.isFocused)
			{
				if ((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) && this.inputField.IsActive())
				{
					this.SendChatMessage();
					this.inputField.ActivateInputField();
					if (PlatformManager.IsStandalone && SceneManager.GetActiveScene().name == "main")
					{
						KeyboardShortcuts.Instance.isSendingChatMsg = false;
					}
				}
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					this.isFocused = false;
					CameraControler.CameraMovementBlocked = false;
					this.inputField.DeactivateInputField();
					if (PlatformManager.IsStandalone)
					{
						KeyboardShortcuts.Instance.isCloseChatWindow = false;
					}
				}
			}
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x00035972 File Offset: 0x00033B72
		private void ResetNewMessageIndicator()
		{
			if (this.unseenMessage)
			{
				this.unseenMessage = false;
				this.newMessageIndicator.SetActive(false);
				this.unseenMessagesCount = 0;
			}
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0009AB64 File Offset: 0x00098D64
		public void SendChatMessage()
		{
			if (this.inputField.text.Length != 0)
			{
				string text = string.Empty;
				if (!this.isLobbyChat)
				{
					text = text + "^" + ((Faction)PlayerInfo.me.Faction).ToString()[0].ToString();
				}
				ChatMessage chatMessage = new ChatMessage(this.isLobbyChat, this.isLobbyGlobalChat, text, PlayerInfo.me.PlayerStats.Name, this.inputField.text);
				Transform transform = this.AddMessage(chatMessage);
				this.inputField.text = "";
				this.awaitingMessagesToCensor.Enqueue(new KeyValuePair<ChatMessage, Transform>(chatMessage, transform));
			}
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0009AC20 File Offset: 0x00098E20
		private void CalculateCensor(ChatMessage message, Transform messageObject)
		{
			this.calculatingCensor = true;
			string text = Censor.Process(message.GetMessage());
			message.SetMessage(text);
			this.awaitingCensuredMessages.Enqueue(new KeyValuePair<ChatMessage, Transform>(message, messageObject));
			this.calculatingCensor = false;
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0009AC60 File Offset: 0x00098E60
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

		// Token: 0x06001431 RID: 5169 RVA: 0x00032177 File Offset: 0x00030377
		private void ChangeCameraMovementState(bool canMove)
		{
			CameraControler.CameraMovementBlocked = canMove;
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x00035996 File Offset: 0x00033B96
		public void OnScrollbarBeginDrag(BaseEventData data)
		{
			this.ChangeCameraMovementState(true);
			this.scrollbarDragged = true;
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x000359A6 File Offset: 0x00033BA6
		public void OnScrollbarEndingDrag(BaseEventData data)
		{
			this.ChangeCameraMovementState(false);
			this.scrollbarDragged = false;
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x000359B6 File Offset: 0x00033BB6
		public void OnChatBeginMoved()
		{
			if (!this.isLobbyChat)
			{
				this.chatDragged = true;
				this.lastMousePosition = Input.mousePosition;
				this.ChangeCameraMovementState(true);
			}
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x000359D9 File Offset: 0x00033BD9
		public void OnChatEndedMove()
		{
			if (!this.isLobbyChat)
			{
				this.chatDragged = false;
				this.ChangeCameraMovementState(false);
			}
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0009ACCC File Offset: 0x00098ECC
		private void MoveChat()
		{
			Vector3 vector = this.lastMousePosition - Input.mousePosition;
			Vector3 vector2 = this.chatRectTransform.position - vector;
			this.chatRectTransform.position = this.AdjustChatPosition(vector2, vector);
			this.lastMousePosition = Input.mousePosition;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0009AD1C File Offset: 0x00098F1C
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

		// Token: 0x06001438 RID: 5176 RVA: 0x0009AE70 File Offset: 0x00099070
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

		// Token: 0x06001439 RID: 5177 RVA: 0x000359F1 File Offset: 0x00033BF1
		public void OnInputFieldEditBegin()
		{
			this.ChangeCameraMovementState(true);
			this.isFocused = true;
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x00035A01 File Offset: 0x00033C01
		public void OnInputFieldEditEnd()
		{
			this.ChangeCameraMovementState(false);
			this.isFocused = false;
		}

		// Token: 0x04000ED2 RID: 3794
		public GameObject chatSpacePlaceholder;

		// Token: 0x04000ED3 RID: 3795
		[Range(5f, 100f)]
		public int maxEntries = 20;

		// Token: 0x04000ED4 RID: 3796
		public bool isLobbyChat;

		// Token: 0x04000ED5 RID: 3797
		public bool isLobbyGlobalChat;

		// Token: 0x04000ED6 RID: 3798
		public RectTransform chatSpace;

		// Token: 0x04000ED7 RID: 3799
		public RectTransform chatHeaderRectTransform;

		// Token: 0x04000ED8 RID: 3800
		public InputField inputField;

		// Token: 0x04000ED9 RID: 3801
		public GameObject chatElements;

		// Token: 0x04000EDA RID: 3802
		public GameObject chatEntryPrefab;

		// Token: 0x04000EDB RID: 3803
		public GameObject chatEntries;

		// Token: 0x04000EDC RID: 3804
		public ScrollRect messageArea;

		// Token: 0x04000EDD RID: 3805
		public GameObject newMessageIndicator;

		// Token: 0x04000EDE RID: 3806
		public TextMeshProUGUI newMessageCount;

		// Token: 0x04000EDF RID: 3807
		private Queue<KeyValuePair<ChatMessage, Transform>> awaitingCensuredMessages;

		// Token: 0x04000EE0 RID: 3808
		private Queue<KeyValuePair<ChatMessage, Transform>> awaitingMessagesToCensor;

		// Token: 0x04000EE1 RID: 3809
		private bool calculatingCensor;

		// Token: 0x04000EE2 RID: 3810
		private Queue<ChatMessage> awaitingMessages;

		// Token: 0x04000EE3 RID: 3811
		private bool scrollbarDragged;

		// Token: 0x04000EE4 RID: 3812
		private bool chatDragged;

		// Token: 0x04000EE5 RID: 3813
		private Vector3 lastMousePosition;

		// Token: 0x04000EE6 RID: 3814
		private RectTransform chatRectTransform;

		// Token: 0x04000EE7 RID: 3815
		private bool unseenMessage;

		// Token: 0x04000EE8 RID: 3816
		private int unseenMessagesCount;

		// Token: 0x04000EE9 RID: 3817
		private bool refreshScrollingSystem;

		// Token: 0x04000EEA RID: 3818
		public bool isFocused;
	}
}

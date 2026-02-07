using System;
using System.Text;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using Scythe.Multiplayer.Messages;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x0200002B RID: 43
public class ChatUIMobile : MonoBehaviour
{
	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060000E0 RID: 224 RVA: 0x000287BA File Offset: 0x000269BA
	public ChatLogic.ChatType ChatType
	{
		get
		{
			return this.chatType;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060000E1 RID: 225 RVA: 0x000287C2 File Offset: 0x000269C2
	public ChatLogic.ChatLocation ChatLocation
	{
		get
		{
			return this.chatLocation;
		}
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x000287CA File Offset: 0x000269CA
	private void Start()
	{
		this.snapToBottomButton.gameObject.SetActive(false);
		this.antiSpamTimer = new AntiSpamTimer(5, 2f, 5f);
		if (this.chatLocation == ChatLogic.ChatLocation.Ingame)
		{
			this.CreateInfoMessageAboutPlayers();
		}
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00028802 File Offset: 0x00026A02
	private void OnEnable()
	{
		this.messageInputField.onValueChanged.AddListener(new UnityAction<string>(this.MessageInputField_OnValueChanged));
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00028820 File Offset: 0x00026A20
	private void OnDisable()
	{
		this.messageInputField.onValueChanged.RemoveListener(new UnityAction<string>(this.MessageInputField_OnValueChanged));
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00055D38 File Offset: 0x00053F38
	private void Update()
	{
		if (SingletonMono<ChatLogic>.Instance != null)
		{
			if (this.ChatLocation == ChatLogic.ChatLocation.Ingame)
			{
				while (SingletonMono<ChatLogic>.Instance.CanRestoreMessages())
				{
					this.chatSize = this.messageContainer.GetComponent<RectTransform>().sizeDelta.y;
					ChatMessage restoredChatMessage = SingletonMono<ChatLogic>.Instance.GetRestoredChatMessage();
					this.InstantiateMessage(restoredChatMessage);
				}
			}
			while (SingletonMono<ChatLogic>.Instance.HasUnreadMessages(this.chatType))
			{
				this.chatSize = this.messageContainer.GetComponent<RectTransform>().sizeDelta.y;
				ChatMessage unreadChatMessage = SingletonMono<ChatLogic>.Instance.GetUnreadChatMessage(this.chatType);
				this.InstantiateMessage(unreadChatMessage);
			}
		}
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00055DE0 File Offset: 0x00053FE0
	private void InstantiateMessage(ChatMessage chatMessage)
	{
		this.oldScrollPos = this.scrollRect.normalizedPosition.y;
		TextMeshProUGUI textMeshProUGUI = global::UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.messagePrefab, this.content);
		this.newMsg = textMeshProUGUI.gameObject;
		StringBuilder stringBuilder = new StringBuilder();
		string playerName = chatMessage.GetPlayerName();
		Color color = ((playerName == PlayerInfo.me.PlayerStats.Name) ? this.selfColor : this.foreignColor);
		if (!string.IsNullOrEmpty(playerName))
		{
			stringBuilder.Append(string.Concat(new string[]
			{
				"<color=#",
				ColorUtility.ToHtmlStringRGB(color),
				">",
				chatMessage.GetPlayerName(),
				":</color>"
			}));
		}
		stringBuilder.Append(" " + chatMessage.GetMessage());
		textMeshProUGUI.text = stringBuilder.ToString();
		Canvas.ForceUpdateCanvases();
		this.CounterChatMovementImmediately();
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00055EC4 File Offset: 0x000540C4
	private void InstantiateSpamMessage()
	{
		this.oldScrollPos = this.scrollRect.normalizedPosition.y;
		TextMeshProUGUI textMeshProUGUI = global::UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.messagePrefab, this.content);
		this.newMsg = textMeshProUGUI.gameObject;
		textMeshProUGUI.text = "<color=red>" + ScriptLocalization.Get("Lobby/ChatSpamWarning") + "</color>";
		Canvas.ForceUpdateCanvases();
		this.CounterChatMovementImmediately();
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0002883E File Offset: 0x00026A3E
	private void SnapToBottom()
	{
		this.scrollRect.normalizedPosition = new Vector2(0f, 0f);
		this.snapped = true;
		this.RefreshSnapToBottomButton();
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00028867 File Offset: 0x00026A67
	private void SnapToTop()
	{
		this.scrollRect.normalizedPosition = new Vector2(0f, 1f);
		this.RefreshSnapToBottomButton();
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00028889 File Offset: 0x00026A89
	private void DetermineSnapLock()
	{
		if (this.scrollRect.normalizedPosition.y <= this.snapTreshold)
		{
			this.snapped = true;
		}
		else
		{
			this.snapped = false;
		}
		this.RefreshSnapToBottomButton();
	}

	// Token: 0x060000EB RID: 235 RVA: 0x000288B9 File Offset: 0x00026AB9
	private void RefreshSnapToBottomButton()
	{
		if (this.IsChatScrollable())
		{
			this.snapToBottomButton.gameObject.SetActive(!this.snapped);
		}
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00055F30 File Offset: 0x00054130
	private void CounterChatMovementImmediately()
	{
		this.newChatSize = this.messageContainer.GetComponent<RectTransform>().sizeDelta.y;
		float y = this.newMsg.GetComponent<RectTransform>().sizeDelta.y;
		float spacing = this.newMsg.transform.parent.GetComponent<VerticalLayoutGroup>().spacing;
		float height = this.messageContainer.GetComponentInParent<ScrollRect>().transform.GetChild(0).GetComponent<RectTransform>().rect.height;
		this.chatSize -= height;
		this.newChatSize -= height;
		float num = (this.chatSize * this.oldScrollPos + (y + spacing)) / this.newChatSize;
		if (this.scrollRect.normalizedPosition != this.chatScrollPosition && !this.snapped)
		{
			this.scrollRect.normalizedPosition = new Vector2(this.scrollRect.normalizedPosition.x, num);
			this.chatScrollPosition = this.scrollRect.normalizedPosition;
			return;
		}
		this.SnapToBottom();
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00056044 File Offset: 0x00054244
	private void CreateInfoMessageAboutPlayers()
	{
		foreach (PlayerData playerData in MultiplayerController.Instance.GetPlayersInGame())
		{
			string text = "^" + ((Faction)playerData.Faction).ToString()[0].ToString();
			this.InstantiateMessage(new ChatMessage(false, false, text, playerData.Name, ((Faction)playerData.Faction).ToString()));
		}
		this.InstantiateMessage(new ChatMessage(false, false, string.Empty, string.Empty, string.Empty));
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00056100 File Offset: 0x00054300
	public void SendMessage_OnClick()
	{
		string text = this.messageInputField.text;
		if (text.Length == 0)
		{
			return;
		}
		this.messageInputField.text = "";
		if (this.antiSpamTimer.MakeACall())
		{
			ChatMessage chatMessage = new ChatMessage(text);
			SingletonMono<ChatLogic>.Instance.EnqueueChatMessageToCensoringAndSending(chatMessage, this.chatType);
		}
		else
		{
			this.InstantiateSpamMessage();
		}
		this.DetermineSnapLock();
		if (this.snapped)
		{
			this.SnapToBottom();
		}
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00056174 File Offset: 0x00054374
	public void ScrollRect_OnValueChanged()
	{
		this.DetermineSnapLock();
		if (this.snapToBottomButton.gameObject.activeSelf && this.scrollRect.normalizedPosition.y <= this.snapTreshold)
		{
			this.snapToBottomButton.gameObject.SetActive(false);
		}
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x000288DC File Offset: 0x00026ADC
	public void SnapToBottom_OnClick()
	{
		this.SnapToBottom();
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x000288E4 File Offset: 0x00026AE4
	public void SnapToTop_OnClick()
	{
		this.SnapToTop();
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x000288EC File Offset: 0x00026AEC
	public void MessageInputField_OnValueChanged(string text)
	{
		this.messageInputField.UpdateTextPositionForMobileInput();
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x000561C4 File Offset: 0x000543C4
	private bool IsChatScrollable()
	{
		return this.messageContainer.GetComponent<RectTransform>().sizeDelta.y >= this.messageContainer.parent.GetComponent<RectTransform>().rect.height;
	}

	// Token: 0x040000DA RID: 218
	[SerializeField]
	private ChatLogic.ChatType chatType;

	// Token: 0x040000DB RID: 219
	[SerializeField]
	private ChatLogic.ChatLocation chatLocation;

	// Token: 0x040000DC RID: 220
	[SerializeField]
	private TextMeshProUGUI messagePrefab;

	// Token: 0x040000DD RID: 221
	[SerializeField]
	private RectTransform content;

	// Token: 0x040000DE RID: 222
	[SerializeField]
	private TMP_InputField messageInputField;

	// Token: 0x040000DF RID: 223
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x040000E0 RID: 224
	[SerializeField]
	private Button snapToBottomButton;

	// Token: 0x040000E1 RID: 225
	[SerializeField]
	private Color selfColor;

	// Token: 0x040000E2 RID: 226
	[SerializeField]
	private Color foreignColor;

	// Token: 0x040000E3 RID: 227
	private bool snapped = true;

	// Token: 0x040000E4 RID: 228
	public Transform messageContainer;

	// Token: 0x040000E5 RID: 229
	private Vector2 chatScrollPosition;

	// Token: 0x040000E6 RID: 230
	private float oldScrollPos;

	// Token: 0x040000E7 RID: 231
	private float chatSize;

	// Token: 0x040000E8 RID: 232
	private float newChatSize;

	// Token: 0x040000E9 RID: 233
	private GameObject newMsg;

	// Token: 0x040000EA RID: 234
	private float snapTreshold = 0.05f;

	// Token: 0x040000EB RID: 235
	private AntiSpamTimer antiSpamTimer;

	// Token: 0x040000EC RID: 236
	private const int NUMBER_OF_MESSAGES_ALLOWED = 5;

	// Token: 0x040000ED RID: 237
	private const float REQUIRED_MESSAGES_INTERVAL = 2f;

	// Token: 0x040000EE RID: 238
	private const float SPAM_PENALTY_DURATION = 5f;
}

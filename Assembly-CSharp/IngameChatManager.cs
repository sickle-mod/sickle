using System;
using Scythe.Multiplayer;
using Scythe.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000DE RID: 222
public class IngameChatManager : MonoBehaviour
{
	// Token: 0x0600068A RID: 1674 RVA: 0x0002C292 File Offset: 0x0002A492
	private void Awake()
	{
		this.ChangeChatState(false);
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x0002C29B File Offset: 0x0002A49B
	private void Start()
	{
		if (MultiplayerController.Instance.ReturningToStartedGame)
		{
			this.ChatVisibility();
			return;
		}
		CameraMovementEffects.Instance.OnFactionPresentationEnd += this.ChatVisibility;
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x0002C2C6 File Offset: 0x0002A4C6
	private void OnDisable()
	{
		if (!MultiplayerController.Instance.ReturningToStartedGame)
		{
			CameraMovementEffects.Instance.OnFactionPresentationEnd -= this.ChatVisibility;
		}
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x0002C2EA File Offset: 0x0002A4EA
	private void ChatVisibility()
	{
		if (!GameController.GameManager.IsMultiplayer || GameController.GameManager.SpectatorMode)
		{
			this.ChangeChatState(false);
			return;
		}
		this.ChangeChatState(true);
		this.hasUnreadChatMessages = false;
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x000706FC File Offset: 0x0006E8FC
	private void Update()
	{
		if (this.hasUnreadChatMessages || !this.openChatButton.gameObject.activeSelf)
		{
			return;
		}
		if (this.chatLogic.HasUnreadMessages(this.chatUI.ChatType))
		{
			this.hasUnreadChatMessages = true;
			this.RefreshOpenChatButton();
		}
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x0002C31A File Offset: 0x0002A51A
	private void RefreshOpenChatButton()
	{
		if (this.hasUnreadChatMessages)
		{
			this.openChatButton.GetComponent<Image>().sprite = this.newMessagesChatButtonSprite;
			return;
		}
		this.openChatButton.GetComponent<Image>().sprite = this.normalChatButtonSprite;
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0002C351 File Offset: 0x0002A551
	private void ChangeChatState(bool chatState)
	{
		this.chatLogic.enabled = chatState;
		if (!chatState)
		{
			this.chatObject.SetActive(chatState);
		}
		this.openChatButton.gameObject.SetActive(chatState);
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x0007074C File Offset: 0x0006E94C
	public void ShowChat_OnClick()
	{
		this.chatObject.SetActive(true);
		this.openActionLogButton.gameObject.SetActive(false);
		this.openChatButton.gameObject.SetActive(false);
		this.hasUnreadChatMessages = false;
		this.RefreshOpenChatButton();
		if (!PlatformManager.IsStandalone && GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsMyTurn() && GameController.Instance.encounterCardPresenter.gameObject.activeInHierarchy)
		{
			GameController.Instance.encounterCardPresenter.MinimizeButton_OnClick();
		}
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x000707D8 File Offset: 0x0006E9D8
	public void HideChat_OnClick()
	{
		this.chatObject.SetActive(false);
		this.openChatButton.gameObject.SetActive(true);
		this.openActionLogButton.gameObject.SetActive(true);
		if (!PlatformManager.IsStandalone && GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsMyTurn() && GameController.GameManager.LastEncounterCard != null)
		{
			GameController.Instance.encounterCardPresenter.MinimizeButton_OnClick();
		}
	}

	// Token: 0x040005B4 RID: 1460
	[SerializeField]
	private ChatUIMobile chatUI;

	// Token: 0x040005B5 RID: 1461
	[SerializeField]
	private GameObject chatObject;

	// Token: 0x040005B6 RID: 1462
	[SerializeField]
	private Button openChatButton;

	// Token: 0x040005B7 RID: 1463
	[SerializeField]
	private Button openActionLogButton;

	// Token: 0x040005B8 RID: 1464
	[SerializeField]
	private Sprite normalChatButtonSprite;

	// Token: 0x040005B9 RID: 1465
	[SerializeField]
	private Sprite newMessagesChatButtonSprite;

	// Token: 0x040005BA RID: 1466
	[SerializeField]
	private ChatLogic chatLogic;

	// Token: 0x040005BB RID: 1467
	private bool hasUnreadChatMessages;
}

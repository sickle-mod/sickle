using System;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003D RID: 61
public class InvitationToBuddiesEntryMobile : MonoBehaviour
{
	// Token: 0x1700001F RID: 31
	// (get) Token: 0x060001DD RID: 477 RVA: 0x000290F6 File Offset: 0x000272F6
	// (set) Token: 0x060001DE RID: 478 RVA: 0x000290FE File Offset: 0x000272FE
	public PlayerInfo PlayerData { get; private set; }

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x060001DF RID: 479 RVA: 0x00029107 File Offset: 0x00027307
	public Toggle2 DropdownToggle
	{
		get
		{
			return this.dropdownToggle;
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x060001E0 RID: 480 RVA: 0x0002910F File Offset: 0x0002730F
	public Button AcceptButton
	{
		get
		{
			return this.acceptButton;
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x060001E1 RID: 481 RVA: 0x00029117 File Offset: 0x00027317
	public Button RejectButton
	{
		get
		{
			return this.rejectButton;
		}
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060001E2 RID: 482 RVA: 0x0002911F File Offset: 0x0002731F
	public Button CancelButton
	{
		get
		{
			return this.cancelButton;
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060001E3 RID: 483 RVA: 0x00029127 File Offset: 0x00027327
	public Button SeeStatisticsButton
	{
		get
		{
			return this.seeStatisticsButton;
		}
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x0005A068 File Offset: 0x00058268
	public void Init(Lobby lobby, PlayerInfo playerData, bool showCancelButton)
	{
		this.lobby = lobby;
		this.PlayerData = playerData;
		if (this.PlayerData != null)
		{
			if (this.playerNameLabel)
			{
				this.playerNameLabel.text = this.PlayerData.PlayerStats.Name;
			}
			if (this.AcceptButton)
			{
				this.AcceptButton.gameObject.SetActive(!showCancelButton);
			}
			if (this.RejectButton)
			{
				this.RejectButton.gameObject.SetActive(!showCancelButton);
			}
			if (this.CancelButton)
			{
				this.CancelButton.gameObject.SetActive(showCancelButton);
			}
		}
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x0005A118 File Offset: 0x00058318
	public void AcceptButton_OnClick()
	{
		LobbyLogic.Instance.FriendsLogic.AcceptInvitationSuccess += this.FriendsLogic_AcceptInvitationSuccess;
		LobbyLogic.Instance.FriendsLogic.AcceptInvitationError += this.FriendsLogic_AcceptInvitationError;
		LobbyLogic.Instance.FriendsLogic.AcceptInvitation(this.PlayerData.PlayerStats.Id);
		this.AcceptButton.interactable = false;
		this.RejectButton.interactable = false;
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x0005A194 File Offset: 0x00058394
	public void RejectButton_OnClick()
	{
		LobbyLogic.Instance.FriendsLogic.RejectInvitationSuccess += this.FriendsLogic_DeclineInvitationSuccess;
		LobbyLogic.Instance.FriendsLogic.RejectInvitationError += this.FriendsLogic_DeclineInvitationError;
		LobbyLogic.Instance.FriendsLogic.RejectInvitation(this.PlayerData.PlayerStats.Id);
		this.AcceptButton.interactable = false;
		this.RejectButton.interactable = false;
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x0005A210 File Offset: 0x00058410
	public void CancelButton_OnClick()
	{
		LobbyLogic.Instance.FriendsLogic.CancelInvitationSuccess += this.FriendsLogic_CancelInvitationSuccess;
		LobbyLogic.Instance.FriendsLogic.CancelInvitationError += this.FriendsLogic_CancelInvitationError;
		LobbyLogic.Instance.FriendsLogic.CancelInvitation(this.PlayerData.PlayerStats.Id);
		this.CancelButton.interactable = false;
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0002912F File Offset: 0x0002732F
	public void SeeStatisticsButton_OnClick()
	{
		this.SeeStatisticsButton.interactable = false;
		LobbyRestAPI.GetPlayerStats(this.PlayerData.PlayerStats.Id, delegate(string data)
		{
			PlayerStats playerStats = PlayerStats.FromJson(data);
			playerStats.Name = this.PlayerData.PlayerStats.Name;
			this.lobby.ShowStats(playerStats);
			this.SeeStatisticsButton.interactable = true;
		});
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x0005A280 File Offset: 0x00058480
	private void FriendsLogic_AcceptInvitationSuccess()
	{
		LobbyLogic.Instance.FriendsLogic.AcceptInvitationSuccess -= this.FriendsLogic_AcceptInvitationSuccess;
		LobbyLogic.Instance.FriendsLogic.AcceptInvitationError -= this.FriendsLogic_AcceptInvitationError;
		this.AcceptButton.interactable = true;
		this.RejectButton.interactable = true;
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0005A280 File Offset: 0x00058480
	private void FriendsLogic_AcceptInvitationError(AcceptInvitationErrorStatus errorCode)
	{
		LobbyLogic.Instance.FriendsLogic.AcceptInvitationSuccess -= this.FriendsLogic_AcceptInvitationSuccess;
		LobbyLogic.Instance.FriendsLogic.AcceptInvitationError -= this.FriendsLogic_AcceptInvitationError;
		this.AcceptButton.interactable = true;
		this.RejectButton.interactable = true;
	}

	// Token: 0x060001EB RID: 491 RVA: 0x0005A2DC File Offset: 0x000584DC
	private void FriendsLogic_DeclineInvitationSuccess()
	{
		LobbyLogic.Instance.FriendsLogic.RejectInvitationSuccess -= this.FriendsLogic_DeclineInvitationSuccess;
		LobbyLogic.Instance.FriendsLogic.RejectInvitationError -= this.FriendsLogic_DeclineInvitationError;
		this.AcceptButton.interactable = true;
		this.RejectButton.interactable = true;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x0005A2DC File Offset: 0x000584DC
	private void FriendsLogic_DeclineInvitationError(RejectInvitationErrorStatus errorCode)
	{
		LobbyLogic.Instance.FriendsLogic.RejectInvitationSuccess -= this.FriendsLogic_DeclineInvitationSuccess;
		LobbyLogic.Instance.FriendsLogic.RejectInvitationError -= this.FriendsLogic_DeclineInvitationError;
		this.AcceptButton.interactable = true;
		this.RejectButton.interactable = true;
	}

	// Token: 0x060001ED RID: 493 RVA: 0x0005A338 File Offset: 0x00058538
	private void FriendsLogic_CancelInvitationSuccess()
	{
		LobbyLogic.Instance.FriendsLogic.CancelInvitationSuccess -= this.FriendsLogic_CancelInvitationSuccess;
		LobbyLogic.Instance.FriendsLogic.CancelInvitationError -= this.FriendsLogic_CancelInvitationError;
		this.CancelButton.interactable = true;
	}

	// Token: 0x060001EE RID: 494 RVA: 0x0005A338 File Offset: 0x00058538
	private void FriendsLogic_CancelInvitationError(CancelInvitationErrorStatus errorCode)
	{
		LobbyLogic.Instance.FriendsLogic.CancelInvitationSuccess -= this.FriendsLogic_CancelInvitationSuccess;
		LobbyLogic.Instance.FriendsLogic.CancelInvitationError -= this.FriendsLogic_CancelInvitationError;
		this.CancelButton.interactable = true;
	}

	// Token: 0x04000175 RID: 373
	[SerializeField]
	private TextMeshProUGUI playerNameLabel;

	// Token: 0x04000176 RID: 374
	private Lobby lobby;

	// Token: 0x04000178 RID: 376
	[SerializeField]
	private Toggle2 dropdownToggle;

	// Token: 0x04000179 RID: 377
	[SerializeField]
	private Button acceptButton;

	// Token: 0x0400017A RID: 378
	[SerializeField]
	private Button rejectButton;

	// Token: 0x0400017B RID: 379
	[SerializeField]
	private Button cancelButton;

	// Token: 0x0400017C RID: 380
	[SerializeField]
	private Button seeStatisticsButton;
}

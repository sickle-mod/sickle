using System;
using Scythe.Analytics;
using Scythe.Multiplayer.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x020001FA RID: 506
	public class PlayerListEntryMobile : PlayerListEntry
	{
		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x00031BC9 File Offset: 0x0002FDC9
		// (set) Token: 0x06000EE4 RID: 3812 RVA: 0x00031BD1 File Offset: 0x0002FDD1
		public Toggle2 DropdownToggle
		{
			get
			{
				return this.dropdownToggle;
			}
			private set
			{
				this.dropdownToggle = value;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x00031BDA File Offset: 0x0002FDDA
		public Button InviteToGameButton
		{
			get
			{
				return this.inviteToGameButton;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x00031BE2 File Offset: 0x0002FDE2
		public Button SeeStatisticsButton
		{
			get
			{
				return this.seeStatisticsButton;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x00031BEA File Offset: 0x0002FDEA
		public Button SpectateButton
		{
			get
			{
				return this.spectateButton;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000EE8 RID: 3816 RVA: 0x00031BF2 File Offset: 0x0002FDF2
		public Button AddBuddyButton
		{
			get
			{
				return this.addBuddyButton;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x00031BFA File Offset: 0x0002FDFA
		public Button RemoveBuddyButton
		{
			get
			{
				return this.removeBuddyButton;
			}
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0008B724 File Offset: 0x00089924
		public override void Init(PlayerInfo player, Lobby lobby, bool friend, bool closeDropdown = true)
		{
			base.Init(player, lobby, friend, closeDropdown);
			if (this.dropdownToggle && this.dropdownPanel)
			{
				if (closeDropdown && player.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id)
				{
					this.dropdownToggle.gameObject.SetActive(false);
					this.dropdownPanel.SetActive(false);
				}
				if (this.AddBuddyButton)
				{
					if (player.RelationshipType == RelationshipType.None)
					{
						this.AddBuddyButton.interactable = true;
					}
					else
					{
						this.AddBuddyButton.interactable = false;
					}
				}
				if (this.RemoveBuddyButton)
				{
					if (player.RelationshipType == RelationshipType.Friends)
					{
						this.RemoveBuddyButton.interactable = true;
						return;
					}
					this.RemoveBuddyButton.interactable = false;
				}
			}
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00031C02 File Offset: 0x0002FE02
		public override void RemoveEntry()
		{
			if (this.dropdownToggle && this.dropdownPanel && this.dropdownToggle.isOn)
			{
				this.dropdownToggle.isOn = false;
			}
			base.RemoveEntry();
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00031C3D File Offset: 0x0002FE3D
		public void InviteToGameButton_OnClick()
		{
			this.InviteToGameButton.interactable = false;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00031C4B File Offset: 0x0002FE4B
		public void SeeStatisticsButton_OnClick()
		{
			this.SeeStatisticsButton.interactable = false;
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_show_player_stats);
			LobbyRestAPI.GetPlayerStats(this.playerData.PlayerStats.Id, delegate(string data)
			{
				PlayerStats playerStats = PlayerStats.FromJson(data);
				playerStats.Name = this.playerData.PlayerStats.Name;
				this.lobby.ShowStats(playerStats);
				this.SeeStatisticsButton.interactable = true;
			});
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00031C81 File Offset: 0x0002FE81
		public void SpectateButton_OnClick()
		{
			this.SpectateButton.interactable = false;
			this.lobby.SpectateGame(-1, this.playerData.RoomId);
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0008B7FC File Offset: 0x000899FC
		public void AddBuddyButton_OnClick()
		{
			LobbyLogic.Instance.FriendsLogic.SendInvitationSuccess += this.FriendsLogic_SendInvitationSuccess;
			LobbyLogic.Instance.FriendsLogic.SendInvitationError += this.FriendsLogic_SendInvitationError;
			LobbyLogic.Instance.FriendsLogic.FriendAddedSuccess += this.FriendsLogic_AddFriendSuccess;
			LobbyLogic.Instance.FriendsLogic.SendInvitation(this.playerData.PlayerStats.Id);
			this.AddBuddyButton.interactable = false;
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0008B888 File Offset: 0x00089A88
		private void FriendsLogic_SendInvitationSuccess()
		{
			LobbyLogic.Instance.FriendsLogic.SendInvitationSuccess -= this.FriendsLogic_SendInvitationSuccess;
			LobbyLogic.Instance.FriendsLogic.SendInvitationError -= this.FriendsLogic_SendInvitationError;
			LobbyLogic.Instance.FriendsLogic.FriendAddedSuccess -= this.FriendsLogic_AddFriendSuccess;
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0008B8E8 File Offset: 0x00089AE8
		private void FriendsLogic_AddFriendSuccess(int playerId)
		{
			LobbyLogic.Instance.FriendsLogic.SendInvitationSuccess -= this.FriendsLogic_SendInvitationSuccess;
			LobbyLogic.Instance.FriendsLogic.SendInvitationError -= this.FriendsLogic_SendInvitationError;
			LobbyLogic.Instance.FriendsLogic.FriendAddedSuccess -= this.FriendsLogic_AddFriendSuccess;
			AnalyticsEventLogger.Instance.LogFriendManagement(ActionsOnFriend.add, playerId.ToString(), LobbyLogic.Instance.FriendsLogic.Friends.Count + 1);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0008B970 File Offset: 0x00089B70
		private void FriendsLogic_SendInvitationError(SendInvitationErrorStatus errorCode)
		{
			LobbyLogic.Instance.FriendsLogic.SendInvitationSuccess -= this.FriendsLogic_SendInvitationSuccess;
			LobbyLogic.Instance.FriendsLogic.SendInvitationError -= this.FriendsLogic_SendInvitationError;
			LobbyLogic.Instance.FriendsLogic.FriendAddedSuccess -= this.FriendsLogic_AddFriendSuccess;
			this.AddBuddyButton.interactable = true;
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00031CA6 File Offset: 0x0002FEA6
		public void RemoveBuddyButton_OnClick()
		{
			this.lobby.ShowRemoveFriendPanel(this);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0008B9DC File Offset: 0x00089BDC
		public void RemoveBuddy()
		{
			LobbyLogic.Instance.FriendsLogic.FriendRemoveSuccess += this.FriendsLogic_FriendRemoveSuccess;
			LobbyLogic.Instance.FriendsLogic.FriendRemoveError += this.FriendsLogic_FriendRemoveError;
			LobbyLogic.Instance.FriendsLogic.RemoveFriend(this.playerData.PlayerStats.Id);
			this.RemoveBuddyButton.interactable = false;
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x0008BA4C File Offset: 0x00089C4C
		private void FriendsLogic_FriendRemoveSuccess()
		{
			LobbyLogic.Instance.FriendsLogic.FriendRemoveSuccess -= this.FriendsLogic_FriendRemoveSuccess;
			LobbyLogic.Instance.FriendsLogic.FriendRemoveError -= this.FriendsLogic_FriendRemoveError;
			Guid id = this.playerData.PlayerStats.Id;
			AnalyticsEventLogger.Instance.LogFriendManagement(ActionsOnFriend.remove, id.ToString(), LobbyLogic.Instance.FriendsLogic.Friends.Count);
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0008BACC File Offset: 0x00089CCC
		private void FriendsLogic_FriendRemoveError(RemoveFriendErrorStatus errorCode)
		{
			LobbyLogic.Instance.FriendsLogic.FriendRemoveSuccess -= this.FriendsLogic_FriendRemoveSuccess;
			LobbyLogic.Instance.FriendsLogic.FriendRemoveError -= this.FriendsLogic_FriendRemoveError;
			this.RemoveBuddyButton.interactable = true;
		}

		// Token: 0x04000BA2 RID: 2978
		[SerializeField]
		private Toggle2 dropdownToggle;

		// Token: 0x04000BA3 RID: 2979
		[SerializeField]
		private GameObject dropdownPanel;

		// Token: 0x04000BA4 RID: 2980
		[SerializeField]
		private Button inviteToGameButton;

		// Token: 0x04000BA5 RID: 2981
		[SerializeField]
		private Button seeStatisticsButton;

		// Token: 0x04000BA6 RID: 2982
		[SerializeField]
		private Button spectateButton;

		// Token: 0x04000BA7 RID: 2983
		[SerializeField]
		private Button addBuddyButton;

		// Token: 0x04000BA8 RID: 2984
		[SerializeField]
		private Button removeBuddyButton;
	}
}

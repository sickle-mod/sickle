using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x020001FB RID: 507
	public class PlayerListPanelMobile : PlayerListPanel
	{
		// Token: 0x06000EF9 RID: 3833 RVA: 0x0008BB60 File Offset: 0x00089D60
		protected override void Start()
		{
			base.Start();
			LobbyLogic.Instance.FriendsLogic.FriendsRefreshed += this.FriendsLogic_FriendsRefreshed;
			LobbyLogic.Instance.FriendsLogic.InvitationsSentRefreshed += this.FriendsLogic_InvitationsSentRefreshed;
			LobbyLogic.Instance.FriendsLogic.InvitationsReceivedRefreshed += this.FriendsLogic_InvitationsReceivedRefreshed;
			LobbyLogic.Instance.FriendsLogic.SendInvitationSuccess += this.FriendsLogic_SentInvitationSuccess;
			LobbyLogic.Instance.FriendsLogic.SendInvitationError += this.FriendsLogic_SentInvitationError;
			LobbyLogic.Instance.FriendsLogic.AcceptInvitationSuccess += this.FriendsLogic_AcceptInvitationSuccess;
			LobbyLogic.Instance.FriendsLogic.AcceptInvitationError += this.FriendsLogic_AcceptInvitationError;
			LobbyLogic.Instance.FriendsLogic.RejectInvitationSuccess += this.FriendsLogic_DeclineInvitationSuccess;
			LobbyLogic.Instance.FriendsLogic.RejectInvitationError += this.FriendsLogic_DeclineInvitationError;
			LobbyLogic.Instance.FriendsLogic.CancelInvitationSuccess += this.FriendsLogic_CancelInvitationSuccess;
			LobbyLogic.Instance.FriendsLogic.CancelInvitationError += this.FriendsLogic_CancelInvitationError;
			LobbyLogic.Instance.FriendsLogic.FriendRemoveSuccess += this.FriendsLogic_FriendRemoveSuccess;
			LobbyLogic.Instance.FriendsLogic.FriendRemoveError += this.FriendsLogic_FriendRemoveError;
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0008BCD4 File Offset: 0x00089ED4
		public override void AddFriend()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			string text = this.addFriendName.text;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			this.addFriendName.text = string.Empty;
			LobbyLogic.Instance.FriendsLogic.SendInvitation(text);
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0008BD24 File Offset: 0x00089F24
		private void RefreshFriends(List<PlayerInfo> friends)
		{
			for (int i = 0; i < this.friendEntries.Count; i++)
			{
				bool flag = true;
				for (int j = 0; j < friends.Count; j++)
				{
					if (this.friendEntries[i].playerData.PlayerStats.Id == friends[j].PlayerStats.Id)
					{
						flag = false;
					}
				}
				if (flag)
				{
					global::UnityEngine.Object.Destroy(this.friendEntries[i].gameObject);
					this.friendEntries.RemoveAt(i);
					i--;
				}
			}
			for (int k = 0; k < friends.Count; k++)
			{
				bool flag2 = true;
				for (int l = 0; l < this.friendEntries.Count; l++)
				{
					if (friends[k].PlayerStats.Id == this.friendEntries[l].playerData.PlayerStats.Id)
					{
						this.friendEntries[l].Init(friends[k], this.lobby, true, false);
						flag2 = false;
					}
				}
				if (flag2)
				{
					PlayerListEntryMobile playerListEntryMobile = (PlayerListEntryMobile)global::UnityEngine.Object.Instantiate<PlayerListEntry>(this.friendPrefab, this.friendsContent);
					playerListEntryMobile.transform.SetSiblingIndex(k);
					playerListEntryMobile.DropdownToggle.group = this.friendsContentToggleGroup;
					playerListEntryMobile.Init(friends[k], this.lobby, true, true);
					this.friendEntries.Insert(k, playerListEntryMobile);
				}
			}
			this.noBuddiesLabel.SetActive(this.friendEntries.Count == 0);
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0008BEC0 File Offset: 0x0008A0C0
		private void RefreshInvitationsReceived(List<PlayerInfo> invitationsReceived)
		{
			for (int i = 0; i < this.invitationsReceivedEntries.Count; i++)
			{
				bool flag = true;
				for (int j = 0; j < invitationsReceived.Count; j++)
				{
					if (this.invitationsReceivedEntries[i].PlayerData.PlayerStats.Id == invitationsReceived[j].PlayerStats.Id)
					{
						flag = false;
					}
				}
				if (flag)
				{
					global::UnityEngine.Object.Destroy(this.invitationsReceivedEntries[i].gameObject);
					this.invitationsReceivedEntries.RemoveAt(i);
					i--;
				}
			}
			for (int k = 0; k < invitationsReceived.Count; k++)
			{
				bool flag2 = true;
				for (int l = 0; l < this.invitationsReceivedEntries.Count; l++)
				{
					if (invitationsReceived[k].PlayerStats.Id == this.invitationsReceivedEntries[l].PlayerData.PlayerStats.Id)
					{
						flag2 = false;
					}
				}
				if (flag2)
				{
					InvitationToBuddiesEntryMobile invitationToBuddiesEntryMobile = global::UnityEngine.Object.Instantiate<InvitationToBuddiesEntryMobile>(this.invitationReceivedPrefab, this.invitationsReceivedContent);
					invitationToBuddiesEntryMobile.transform.SetSiblingIndex(k);
					invitationToBuddiesEntryMobile.DropdownToggle.group = this.invitationsContentToggleGroup;
					invitationToBuddiesEntryMobile.Init(this.lobby, invitationsReceived[k], false);
					this.invitationsReceivedEntries.Insert(k, invitationToBuddiesEntryMobile);
				}
			}
			this.invitationsNotificationBadge.SetActive(this.invitationsReceivedEntries.Count != 0);
			this.invitationsNotificationBadgeLabel.text = this.invitationsReceivedEntries.Count.ToString();
			this.noInvitationsReceivedLabel.SetActive(this.invitationsReceivedEntries.Count == 0);
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0008C06C File Offset: 0x0008A26C
		private void RefreshInvitationsSent(List<PlayerInfo> invitationsSent)
		{
			for (int i = 0; i < this.invitationsSentEntries.Count; i++)
			{
				bool flag = true;
				for (int j = 0; j < invitationsSent.Count; j++)
				{
					if (this.invitationsSentEntries[i].PlayerData.PlayerStats.Id == invitationsSent[j].PlayerStats.Id)
					{
						flag = false;
					}
				}
				if (flag)
				{
					global::UnityEngine.Object.Destroy(this.invitationsSentEntries[i].gameObject);
					this.invitationsSentEntries.RemoveAt(i);
					i--;
				}
			}
			for (int k = 0; k < invitationsSent.Count; k++)
			{
				bool flag2 = true;
				for (int l = 0; l < this.invitationsSentEntries.Count; l++)
				{
					if (invitationsSent[k].PlayerStats.Id == this.invitationsSentEntries[l].PlayerData.PlayerStats.Id)
					{
						flag2 = false;
					}
				}
				if (flag2)
				{
					InvitationToBuddiesEntryMobile invitationToBuddiesEntryMobile = global::UnityEngine.Object.Instantiate<InvitationToBuddiesEntryMobile>(this.invitationSentPrefab, this.invitationsSentContent);
					invitationToBuddiesEntryMobile.transform.SetSiblingIndex(k);
					invitationToBuddiesEntryMobile.DropdownToggle.group = this.invitationsContentToggleGroup;
					invitationToBuddiesEntryMobile.Init(this.lobby, invitationsSent[k], true);
					this.invitationsSentEntries.Insert(k, invitationToBuddiesEntryMobile);
				}
			}
			this.noInvitationsSentLabel.SetActive(this.invitationsSentEntries.Count == 0);
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0008C1E0 File Offset: 0x0008A3E0
		protected override void CreatePlayerEntry(PlayerInfo playerInfo)
		{
			PlayerListEntryMobile playerListEntryMobile = (PlayerListEntryMobile)global::UnityEngine.Object.Instantiate<PlayerListEntry>(this.playerPrefab, this.playersContent);
			playerListEntryMobile.DropdownToggle.group = this.playerContentToggleGroup;
			playerListEntryMobile.Init(playerInfo, this.lobby, false, true);
			this.playersInLobby.Add(playerInfo.PlayerStats.Id, playerListEntryMobile);
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0008C23C File Offset: 0x0008A43C
		protected override void CreateFriendEntry(PlayerInfo friend)
		{
			PlayerListEntryMobile playerListEntryMobile = (PlayerListEntryMobile)global::UnityEngine.Object.Instantiate<PlayerListEntry>(this.friendPrefab, this.friendsContent);
			playerListEntryMobile.DropdownToggle.group = this.friendsContentToggleGroup;
			playerListEntryMobile.Init(friend, this.lobby, true, true);
			this.playersInLobby.Add(friend.PlayerStats.Id, playerListEntryMobile);
			PlayerListEntryMobile playerListEntryMobile2 = playerListEntryMobile;
			playerListEntryMobile2.OnBuddyRemove = (Action<Guid>)Delegate.Combine(playerListEntryMobile2.OnBuddyRemove, new Action<Guid>(base.PlayerListEntry_OnBuddyRemove));
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00031CBC File Offset: 0x0002FEBC
		private void FriendsLogic_FriendsRefreshed()
		{
			this.RefreshFriends(LobbyLogic.Instance.FriendsLogic.Friends);
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00031CD3 File Offset: 0x0002FED3
		private void FriendsLogic_InvitationsSentRefreshed()
		{
			this.RefreshInvitationsSent(LobbyLogic.Instance.FriendsLogic.InvitationsSent);
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00031CEA File Offset: 0x0002FEEA
		private void FriendsLogic_InvitationsReceivedRefreshed()
		{
			this.RefreshInvitationsReceived(LobbyLogic.Instance.FriendsLogic.InvitationsReceived);
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0008C2BC File Offset: 0x0008A4BC
		private void FriendsLogic_SentInvitationSuccess()
		{
			this.RefreshFriends(LobbyLogic.Instance.FriendsLogic.Friends);
			this.RefreshInvitationsSent(LobbyLogic.Instance.FriendsLogic.InvitationsSent);
			this.RefreshInvitationsReceived(LobbyLogic.Instance.FriendsLogic.InvitationsReceived);
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00031D01 File Offset: 0x0002FF01
		private void FriendsLogic_SentInvitationError(SendInvitationErrorStatus errorStatus)
		{
			this.ShowErrorPopup(errorStatus.ToString());
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0008C2BC File Offset: 0x0008A4BC
		private void FriendsLogic_AcceptInvitationSuccess()
		{
			this.RefreshFriends(LobbyLogic.Instance.FriendsLogic.Friends);
			this.RefreshInvitationsSent(LobbyLogic.Instance.FriendsLogic.InvitationsSent);
			this.RefreshInvitationsReceived(LobbyLogic.Instance.FriendsLogic.InvitationsReceived);
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00031D16 File Offset: 0x0002FF16
		private void FriendsLogic_AcceptInvitationError(AcceptInvitationErrorStatus errorStatus)
		{
			this.ShowErrorPopup(errorStatus.ToString());
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0008C2BC File Offset: 0x0008A4BC
		private void FriendsLogic_DeclineInvitationSuccess()
		{
			this.RefreshFriends(LobbyLogic.Instance.FriendsLogic.Friends);
			this.RefreshInvitationsSent(LobbyLogic.Instance.FriendsLogic.InvitationsSent);
			this.RefreshInvitationsReceived(LobbyLogic.Instance.FriendsLogic.InvitationsReceived);
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00031D2B File Offset: 0x0002FF2B
		private void FriendsLogic_DeclineInvitationError(RejectInvitationErrorStatus errorStatus)
		{
			this.ShowErrorPopup(errorStatus.ToString());
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0008C2BC File Offset: 0x0008A4BC
		private void FriendsLogic_CancelInvitationSuccess()
		{
			this.RefreshFriends(LobbyLogic.Instance.FriendsLogic.Friends);
			this.RefreshInvitationsSent(LobbyLogic.Instance.FriendsLogic.InvitationsSent);
			this.RefreshInvitationsReceived(LobbyLogic.Instance.FriendsLogic.InvitationsReceived);
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x00031D40 File Offset: 0x0002FF40
		private void FriendsLogic_CancelInvitationError(CancelInvitationErrorStatus errorStatus)
		{
			this.ShowErrorPopup(errorStatus.ToString());
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0008C2BC File Offset: 0x0008A4BC
		private void FriendsLogic_FriendRemoveSuccess()
		{
			this.RefreshFriends(LobbyLogic.Instance.FriendsLogic.Friends);
			this.RefreshInvitationsSent(LobbyLogic.Instance.FriendsLogic.InvitationsSent);
			this.RefreshInvitationsReceived(LobbyLogic.Instance.FriendsLogic.InvitationsReceived);
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x00031D55 File Offset: 0x0002FF55
		private void FriendsLogic_FriendRemoveError(RemoveFriendErrorStatus errorStatus)
		{
			this.ShowErrorPopup(errorStatus.ToString());
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0008C308 File Offset: 0x0008A508
		private void ShowErrorPopup(string errorStatus)
		{
			string text = ScriptLocalization.Get("Lobby/" + errorStatus + "Title");
			string text2 = ScriptLocalization.Get("Lobby/" + errorStatus + "Content");
			this.basicPopup.Initialize(text, text2, 1);
			this.basicPopup.InitializeButton(0, ScriptLocalization.Get("Lobby/OkButton"), MenuPopupUI.ButtonColor.Green);
			this.basicPopup.OnClickButton0 += this.ClosePopup;
			this.basicPopup.Show();
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x00031D6A File Offset: 0x0002FF6A
		private void ClosePopup()
		{
			this.basicPopup.Hide();
		}

		// Token: 0x04000BA9 RID: 2985
		[Header("PlayerListPanelMobile")]
		[SerializeField]
		private Transform playersContent;

		// Token: 0x04000BAA RID: 2986
		[SerializeField]
		private Toggle2Group playerContentToggleGroup;

		// Token: 0x04000BAB RID: 2987
		[SerializeField]
		private Transform friendsContent;

		// Token: 0x04000BAC RID: 2988
		[SerializeField]
		private Toggle2Group friendsContentToggleGroup;

		// Token: 0x04000BAD RID: 2989
		[SerializeField]
		private InvitationToBuddiesEntryMobile invitationReceivedPrefab;

		// Token: 0x04000BAE RID: 2990
		[SerializeField]
		private InvitationToBuddiesEntryMobile invitationSentPrefab;

		// Token: 0x04000BAF RID: 2991
		[SerializeField]
		private Transform invitationsReceivedContent;

		// Token: 0x04000BB0 RID: 2992
		[SerializeField]
		private Transform invitationsSentContent;

		// Token: 0x04000BB1 RID: 2993
		[SerializeField]
		private Toggle2Group invitationsContentToggleGroup;

		// Token: 0x04000BB2 RID: 2994
		[SerializeField]
		private GameObject noBuddiesLabel;

		// Token: 0x04000BB3 RID: 2995
		[SerializeField]
		private GameObject noInvitationsReceivedLabel;

		// Token: 0x04000BB4 RID: 2996
		[SerializeField]
		private GameObject noInvitationsSentLabel;

		// Token: 0x04000BB5 RID: 2997
		[SerializeField]
		private GameObject invitationsNotificationBadge;

		// Token: 0x04000BB6 RID: 2998
		[SerializeField]
		private TextMeshProUGUI invitationsNotificationBadgeLabel;

		// Token: 0x04000BB7 RID: 2999
		[SerializeField]
		private List<PlayerListEntryMobile> friendEntries;

		// Token: 0x04000BB8 RID: 3000
		[SerializeField]
		private List<InvitationToBuddiesEntryMobile> invitationsReceivedEntries;

		// Token: 0x04000BB9 RID: 3001
		[SerializeField]
		private List<InvitationToBuddiesEntryMobile> invitationsSentEntries;

		// Token: 0x04000BBA RID: 3002
		[SerializeField]
		private MenuPopupUI basicPopup;
	}
}

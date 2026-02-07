using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Analytics;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x0200026A RID: 618
	public class PlayerListEntry : MonoBehaviour
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x00034864 File Offset: 0x00032A64
		// (set) Token: 0x060012E0 RID: 4832 RVA: 0x0003486C File Offset: 0x00032A6C
		public Action<Guid> OnBuddyRemove { get; set; }

		// Token: 0x060012E1 RID: 4833 RVA: 0x00097650 File Offset: 0x00095850
		public virtual void Init(PlayerInfo player, Lobby lobby, bool friend, bool closeDropdown = true)
		{
			bool flag = player.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id;
			this.lobby = lobby;
			this.isFriend = friend;
			this.playerData = player;
			this.name.text = player.PlayerStats.Name;
			this.name.color = (flag ? new Color(0.8352941f, 0.69411767f, 0.2627451f) : Color.white);
			if (this.dropdown)
			{
				if (closeDropdown && flag)
				{
					this.dropdown.gameObject.SetActive(false);
				}
				else
				{
					this.RefreshDropdowOptions(player);
				}
			}
			if (this.status)
			{
				this.status.gameObject.SetActive(false);
			}
			if (this.isFriend)
			{
				this.UpdateFriendStatus(player);
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x00034875 File Offset: 0x00032A75
		public void RefreshDropdowOptions(PlayerInfo player)
		{
			this.dropdown.options.Clear();
			if (this.isFriend)
			{
				this.AddFriendOptions();
				return;
			}
			this.AddPlayerOptions();
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00097738 File Offset: 0x00095938
		private void AddFriendOptions()
		{
			this.AddDropdownOption(this.optionsData[0]);
			this.AddDropdownOption(this.optionsData[1]);
			this.AddDropdownOption(this.optionsData[3]);
			this.AddDropdownOption(this.optionsData[4]);
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0003489C File Offset: 0x00032A9C
		private void AddPlayerOptions()
		{
			this.AddDropdownOption(this.optionsData[0]);
			this.AddDropdownOption(this.optionsData[1]);
			this.AddDropdownOption(this.optionsData[2]);
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00097790 File Offset: 0x00095990
		private void AddDropdownOption(TMP_Dropdown.OptionData option)
		{
			string text = ScriptLocalization.Get(option.text);
			this.dropdown.options.Add(new TMP_Dropdown.OptionData(text, option.image));
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x000348D4 File Offset: 0x00032AD4
		private void UpdateFriendStatus(PlayerInfo friend)
		{
			if (friend.PlayerStatus == PlayerStatus.Offline)
			{
				this.SetOfflineStatusColor();
			}
			else
			{
				this.SetNormalStatusColor();
			}
			this.status.text = this.PlayerStatusToLocalizedString(friend.PlayerStatus);
			this.status.gameObject.SetActive(true);
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x000977C8 File Offset: 0x000959C8
		private string PlayerStatusToLocalizedString(PlayerStatus playerStatus)
		{
			switch (playerStatus)
			{
			case PlayerStatus.Offline:
				return ScriptLocalization.Get("Lobby/Offline");
			case PlayerStatus.InLobby:
				return ScriptLocalization.Get("Lobby/InLobby");
			case PlayerStatus.InRoom:
				return ScriptLocalization.Get("Lobby/InRoom");
			case PlayerStatus.InGame:
				return ScriptLocalization.Get("Lobby/InGame");
			default:
				return ScriptLocalization.Get("Lobby/InLobby");
			}
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x00097824 File Offset: 0x00095A24
		private void SetOfflineStatusColor()
		{
			Color color;
			ColorUtility.TryParseHtmlString("#EB5757", out color);
			color.a = 0.7f;
			this.status.color = color;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x00097858 File Offset: 0x00095A58
		private void SetNormalStatusColor()
		{
			Color color;
			ColorUtility.TryParseHtmlString("#FFFFFF", out color);
			color.a = 0.7f;
			this.status.color = color;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0009788C File Offset: 0x00095A8C
		public virtual bool IsOptionAvailable(int option)
		{
			switch (option)
			{
			case 0:
				return this.CanInvite();
			case 1:
				return this.CanSeeStats();
			case 2:
				if (this.isFriend)
				{
					return this.CanSpectate();
				}
				return this.CanAddToFriends();
			case 3:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00034914 File Offset: 0x00032B14
		protected bool CanInvite()
		{
			return this.CanInvite(this.playerData);
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x000283F8 File Offset: 0x000265F8
		protected bool CanSeeStats()
		{
			return true;
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00034922 File Offset: 0x00032B22
		private bool CanInvite(PlayerInfo player)
		{
			return PlayerInfo.me.CurrentLobbyRoom != null && (player.PlayerStatus == PlayerStatus.InLobby || player.PlayerStatus == PlayerStatus.InRoom);
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x00034947 File Offset: 0x00032B47
		protected bool CanAddToFriends()
		{
			return !LobbyLogic.Instance.FriendsLogic.Friends.Exists((PlayerInfo friend) => friend.PlayerStats.Id == this.playerData.PlayerStats.Id);
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x0003496C File Offset: 0x00032B6C
		protected bool CanSpectate()
		{
			return this.playerData.PlayerStatus == PlayerStatus.InGame;
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x000978D8 File Offset: 0x00095AD8
		public void OnOptionChose(int option)
		{
			switch (option)
			{
			case 0:
				this.InvitePlayer();
				return;
			case 1:
				this.SeeStatistics();
				return;
			case 2:
				if (this.isFriend)
				{
					this.Spectate();
					return;
				}
				this.AddToFriends();
				return;
			case 3:
				this.RemoveFromFriends();
				return;
			default:
				return;
			}
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0003497C File Offset: 0x00032B7C
		public void InvitePlayer()
		{
			Guid id = this.playerData.PlayerStats.Id;
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.inviteSent);
			LobbyRestAPI.InvitePlayer(id, delegate(string response)
			{
			});
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x00097928 File Offset: 0x00095B28
		public void SeeStatistics()
		{
			if (this.playerStats != null)
			{
				this.lobby.ShowStats(this.playerStats);
				return;
			}
			Guid id = this.playerData.PlayerStats.Id;
			string playerName = this.playerData.PlayerStats.Name;
			LobbyRestAPI.GetPlayerStats(id, delegate(string data)
			{
				this.playerStats = PlayerStats.FromJson(data);
				this.playerStats.Name = playerName;
				this.lobby.ShowStats(this.playerStats);
			});
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x000349B8 File Offset: 0x00032BB8
		public void Spectate()
		{
			this.lobby.SpectateGame(-1, this.playerData.RoomId);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00097994 File Offset: 0x00095B94
		public void AddToFriends()
		{
			Guid id = this.playerData.PlayerStats.Id;
			LobbyLogic.Instance.FriendsLogic.SendInvitation(id);
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x00031CA6 File Offset: 0x0002FEA6
		public void ShowRemoveFriendPanel()
		{
			this.lobby.ShowRemoveFriendPanel(this);
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x000979C4 File Offset: 0x00095BC4
		public void RemoveFromFriends()
		{
			Guid id = this.playerData.PlayerStats.Id;
			AnalyticsEventLogger.Instance.LogFriendManagement(ActionsOnFriend.remove, id.ToString(), LobbyLogic.Instance.FriendsLogic.Friends.Count);
			Action<Guid> onBuddyRemove = this.OnBuddyRemove;
			if (onBuddyRemove == null)
			{
				return;
			}
			onBuddyRemove(id);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x000349D1 File Offset: 0x00032BD1
		public virtual void RemoveEntry()
		{
			if (this.dropdown && this.dropdown.IsExpanded)
			{
				this.dropdown.Hide();
			}
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04000E3F RID: 3647
		private const int InviteIndex = 0;

		// Token: 0x04000E40 RID: 3648
		private const int SeeStatisticsIndex = 1;

		// Token: 0x04000E41 RID: 3649
		private const int AddToFriendIndex = 2;

		// Token: 0x04000E42 RID: 3650
		private const int SpectateIndex = 3;

		// Token: 0x04000E43 RID: 3651
		private const int RemoveFromFriendsIndex = 4;

		// Token: 0x04000E44 RID: 3652
		[SerializeField]
		private new TextMeshProUGUI name;

		// Token: 0x04000E45 RID: 3653
		[SerializeField]
		private TextMeshProUGUI status;

		// Token: 0x04000E46 RID: 3654
		[SerializeField]
		private TMP_Dropdown dropdown;

		// Token: 0x04000E47 RID: 3655
		[SerializeField]
		private List<TMP_Dropdown.OptionData> optionsData;

		// Token: 0x04000E48 RID: 3656
		public Lobby lobby;

		// Token: 0x04000E49 RID: 3657
		private bool isFriend;

		// Token: 0x04000E4A RID: 3658
		public PlayerInfo playerData;

		// Token: 0x04000E4B RID: 3659
		private PlayerStats playerStats;

		// Token: 0x04000E4C RID: 3660
		public Action<Guid> BuddyRemoved;
	}
}

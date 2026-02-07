using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Multiplayer.AuthApi;
using Scythe.Analytics;
using Scythe.Multiplayer.AuthApi.Models;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x0200026F RID: 623
	public class PlayerListPanel : MonoBehaviour
	{
		// Token: 0x06001316 RID: 4886 RVA: 0x00034B5D File Offset: 0x00032D5D
		protected virtual void Start()
		{
			this.playersInLobbyAmount.text = "";
			this.friendsLogic = LobbyLogic.Instance.FriendsLogic;
			this.AddDelegates();
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00097C78 File Offset: 0x00095E78
		private void AddDelegates()
		{
			LobbyLogic.Instance.PlayersListLogic.PlayersListDownloaded += this.RefreshPlayersList;
			LobbyLogic.Instance.FriendsLogic.FriendsRefreshed += this.RefreshFriendsList;
			PersistentSingleton<BuddiesController>.Instance.OnAddBuddyByNameSuccess += this.BuddiesController_OnAddBuddyByNameSuccess;
			PersistentSingleton<BuddiesController>.Instance.OnAddBuddyByNameFailure += this.BuddiesController_OnAddBuddyFailure;
			PersistentSingleton<BuddiesController>.Instance.OnAddBuddyByNameError += this.BuddiesController_OnAddBuddyError;
			PersistentSingleton<BuddiesController>.Instance.OnAddBuddySuccess += this.BuddiesController_OnAddBuddySuccess;
			PersistentSingleton<BuddiesController>.Instance.OnAddBuddyFailure += this.BuddiesController_OnAddBuddyFailure;
			PersistentSingleton<BuddiesController>.Instance.OnAddBuddyError += this.BuddiesController_OnAddBuddyError;
			PersistentSingleton<BuddiesController>.Instance.OnRemoveBuddySuccess += this.BuddiesController_OnRemoveBuddySuccess;
			PersistentSingleton<BuddiesController>.Instance.OnRemoveBuddyFailure += this.BuddiesController_OnRemoveBuddyFailure;
			PersistentSingleton<BuddiesController>.Instance.OnRemoveBuddyError += this.BuddiesController_OnRemoveBuddyError;
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00097D84 File Offset: 0x00095F84
		public void RemoveDelegates()
		{
			LobbyLogic.Instance.PlayersListLogic.PlayersListDownloaded -= this.RefreshPlayersList;
			LobbyLogic.Instance.FriendsLogic.FriendsRefreshed -= this.RefreshFriendsList;
			PersistentSingleton<BuddiesController>.Instance.OnAddBuddyByNameSuccess -= this.BuddiesController_OnAddBuddyByNameSuccess;
			PersistentSingleton<BuddiesController>.Instance.OnAddBuddyByNameFailure -= this.BuddiesController_OnAddBuddyFailure;
			PersistentSingleton<BuddiesController>.Instance.OnAddBuddyByNameError -= this.BuddiesController_OnAddBuddyError;
			PersistentSingleton<BuddiesController>.Instance.OnAddBuddySuccess -= this.BuddiesController_OnAddBuddySuccess;
			PersistentSingleton<BuddiesController>.Instance.OnAddBuddyFailure -= this.BuddiesController_OnAddBuddyFailure;
			PersistentSingleton<BuddiesController>.Instance.OnAddBuddyError -= this.BuddiesController_OnAddBuddyError;
			PersistentSingleton<BuddiesController>.Instance.OnRemoveBuddySuccess -= this.BuddiesController_OnRemoveBuddySuccess;
			PersistentSingleton<BuddiesController>.Instance.OnRemoveBuddyFailure -= this.BuddiesController_OnRemoveBuddyFailure;
			PersistentSingleton<BuddiesController>.Instance.OnRemoveBuddyError -= this.BuddiesController_OnRemoveBuddyError;
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00034B85 File Offset: 0x00032D85
		protected virtual void UpdatePlayersInLobbyText(int amount)
		{
			this.playersInLobbyAmount.text = string.Format("{0} {1}", ScriptLocalization.Get("Lobby/PlayersInLobby"), amount);
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void UpdateFriendsOnlineText(int amount)
		{
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x00097E90 File Offset: 0x00096090
		public void OnPlayersTabStateChanged(bool state)
		{
			if (!state)
			{
				return;
			}
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			if (this.addFriendError)
			{
				this.addFriendError.gameObject.SetActive(false);
			}
			if (this.addFriendPanel)
			{
				this.addFriendPanel.SetActive(false);
			}
			this.friendsPanelActive = false;
			this.SetDefaultAlpha(this.playersTabImage);
			this.SetInvisible(this.playersTab);
			this.SetDefaultAlpha(this.friendsTab);
			this.SetInactiveAlpha(this.friendsTabImage);
			this.RefreshPlayersList();
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x00097F24 File Offset: 0x00096124
		public void OnFriendsTabStateChanged(bool state)
		{
			if (!state)
			{
				return;
			}
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			if (this.addFriendError)
			{
				this.addFriendError.gameObject.SetActive(true);
			}
			if (this.addFriendPanel)
			{
				this.addFriendPanel.SetActive(true);
			}
			this.addFriendName.text = string.Empty;
			if (this.addFriendError)
			{
				this.addFriendError.text = string.Empty;
			}
			this.friendsPanelActive = true;
			this.SetDefaultAlpha(this.friendsTabImage);
			this.SetInvisible(this.friendsTab);
			this.SetDefaultAlpha(this.playersTab);
			this.SetInactiveAlpha(this.playersTabImage);
			this.RefreshFriendsList();
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x00097FE4 File Offset: 0x000961E4
		private void SetDefaultAlpha(Image image)
		{
			if (image)
			{
				Color color = image.color;
				color.a = 1f;
				image.color = color;
			}
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00098014 File Offset: 0x00096214
		private void SetInactiveAlpha(Image image)
		{
			if (image)
			{
				Color color = image.color;
				color.a = 0.5f;
				image.color = color;
			}
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x00098044 File Offset: 0x00096244
		private void SetInvisible(Image image)
		{
			if (image)
			{
				Color color = image.color;
				color.a = 0f;
				image.color = color;
			}
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00098074 File Offset: 0x00096274
		private void RefreshPlayersList()
		{
			foreach (Guid guid in this.playersInLobby.Keys)
			{
				this.RemoveEntry(this.playersInLobby[guid]);
			}
			this.playersInLobby.Clear();
			foreach (PlayerInfo playerInfo in LobbyLogic.Instance.PlayersListLogic.Players.Values)
			{
				this.CreatePlayerEntry(playerInfo);
			}
			this.UpdatePlayersInLobbyText(this.playersInLobby.Count);
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00098144 File Offset: 0x00096344
		protected virtual void CreatePlayerEntry(PlayerInfo playerInfo)
		{
			PlayerListEntry playerListEntry = global::UnityEngine.Object.Instantiate<PlayerListEntry>(this.playerPrefab, this.content);
			playerListEntry.Init(playerInfo, this.lobby, false, true);
			this.playersInLobby.Add(playerInfo.PlayerStats.Id, playerListEntry);
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0009818C File Offset: 0x0009638C
		private void RefreshFriendsList()
		{
			if (this.addFriendPanel == null || !this.addFriendPanel.activeInHierarchy)
			{
				return;
			}
			foreach (Guid guid in this.playersInLobby.Keys)
			{
				this.RemoveEntry(this.playersInLobby[guid]);
			}
			this.playersInLobby.Clear();
			foreach (PlayerInfo playerInfo in this.friendsLogic.Friends)
			{
				this.CreateFriendEntry(playerInfo);
			}
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00098260 File Offset: 0x00096460
		public virtual void AddFriend()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			string text = this.addFriendName.text;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			this.addFriendName.text = string.Empty;
			if (this.addFriendError)
			{
				this.addFriendError.text = string.Empty;
			}
			PersistentSingleton<BuddiesController>.Instance.AddBuddy(text);
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x00034BAC File Offset: 0x00032DAC
		public void StartCoroutines()
		{
			base.StartCoroutine(this.DownloadFriendsList(5f));
			base.StartCoroutine(LobbyLogic.Instance.PlayersListLogic.DownloadPlayerList());
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x00034BD6 File Offset: 0x00032DD6
		public void StopCoroutines()
		{
			base.StopCoroutine(this.DownloadFriendsList(5f));
			base.StopCoroutine(LobbyLogic.Instance.PlayersListLogic.DownloadPlayerList());
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x00034BFE File Offset: 0x00032DFE
		private IEnumerator DownloadFriendsList(float interval)
		{
			while (!MultiplayerController.Instance.Disconnected)
			{
				LobbyLogic.Instance.FriendsLogic.GetAllRelationships();
				yield return new WaitForSeconds(interval);
			}
			yield break;
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x00034C0D File Offset: 0x00032E0D
		public void AddNewPlayer(PlayerInfo playerInfo)
		{
			LobbyLogic.Instance.PlayersListLogic.AddNewPlayer(playerInfo);
			this.OnPlayerDownloaded(playerInfo);
			this.UpdatePlayersInLobbyText(LobbyLogic.Instance.PlayersListLogic.Players.Count);
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x000982C8 File Offset: 0x000964C8
		private void OnPlayerDownloaded(PlayerInfo playerInfo)
		{
			if (!this.friendsPanelActive)
			{
				if (this.playersInLobby.ContainsKey(playerInfo.PlayerStats.Id))
				{
					this.playersInLobby[playerInfo.PlayerStats.Id].Init(playerInfo, this.lobby, false, true);
				}
				else
				{
					this.CreatePlayerEntry(playerInfo);
				}
			}
			this.UpdatePlayersInLobbyText(LobbyLogic.Instance.PlayersListLogic.Players.Count);
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0009833C File Offset: 0x0009653C
		public void RemovePlayer(Guid id)
		{
			LobbyLogic.Instance.PlayersListLogic.RemovePlayer(id);
			if (!this.friendsPanelActive && this.playersInLobby.ContainsKey(id))
			{
				this.RemoveEntry(this.playersInLobby[id]);
				this.playersInLobby.Remove(id);
			}
			this.UpdatePlayersInLobbyText(LobbyLogic.Instance.PlayersListLogic.Players.Count);
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x000983A8 File Offset: 0x000965A8
		protected virtual void CreateFriendEntry(PlayerInfo friend)
		{
			PlayerListEntry playerListEntry = global::UnityEngine.Object.Instantiate<PlayerListEntry>(this.friendPrefab, this.content);
			playerListEntry.Init(friend, this.lobby, true, true);
			this.playersInLobby.Add(friend.PlayerStats.Id, playerListEntry);
			PlayerListEntry playerListEntry2 = playerListEntry;
			playerListEntry2.OnBuddyRemove = (Action<Guid>)Delegate.Combine(playerListEntry2.OnBuddyRemove, new Action<Guid>(this.PlayerListEntry_OnBuddyRemove));
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00034C40 File Offset: 0x00032E40
		protected void PlayerListEntry_OnBuddyRemove(Guid id)
		{
			PersistentSingleton<BuddiesController>.Instance.RemoveBuddy(id);
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00034C4D File Offset: 0x00032E4D
		private void BuddiesController_OnRemoveBuddySuccess(RemoveBuddyResponse response)
		{
			this.RemoveEntry(this.playersInLobby[response.BuddyId]);
			this.playersInLobby.Remove(response.BuddyId);
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00034C78 File Offset: 0x00032E78
		private void BuddiesController_OnRemoveBuddyFailure(FailureResponse response)
		{
			this.BuddiesMessageHandle(response.GetErrorsString());
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x00034C86 File Offset: 0x00032E86
		private void BuddiesController_OnRemoveBuddyError(Exception exception)
		{
			this.BuddiesMessageHandle(exception.ToString());
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x00034C94 File Offset: 0x00032E94
		private void RemoveEntry(PlayerListEntry entry)
		{
			entry.OnBuddyRemove = (Action<Guid>)Delegate.Remove(entry.OnBuddyRemove, new Action<Guid>(this.PlayerListEntry_OnBuddyRemove));
			entry.RemoveEntry();
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x00034CBE File Offset: 0x00032EBE
		private void BuddiesController_OnAddBuddyByNameSuccess(AddBuddyByNameResponse response)
		{
			this.OnAddBuddySuccess(response.BuddyId);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x00034CCC File Offset: 0x00032ECC
		private void BuddiesController_OnAddBuddySuccess(AddBuddyResponse response)
		{
			this.OnAddBuddySuccess(response.BuddyId);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00034CDA File Offset: 0x00032EDA
		private void OnAddBuddySuccess(Guid? id)
		{
			this.BuddiesMessageHandle(ScriptLocalization.Get("Common/Ok"));
			AnalyticsEventLogger.Instance.LogFriendManagement(ActionsOnFriend.add, id.ToString(), this.friendsLogic.Friends.Count + 1);
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00034C78 File Offset: 0x00032E78
		private void BuddiesController_OnAddBuddyFailure(FailureResponse response)
		{
			this.BuddiesMessageHandle(response.GetErrorsString());
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x00034C86 File Offset: 0x00032E86
		private void BuddiesController_OnAddBuddyError(Exception e)
		{
			this.BuddiesMessageHandle(e.ToString());
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00034D16 File Offset: 0x00032F16
		private void BuddiesMessageHandle(string message)
		{
			if (this.friendsPanelActive && this.addFriendError)
			{
				this.addFriendError.text = message;
			}
		}

		// Token: 0x04000E5B RID: 3675
		[SerializeField]
		protected Lobby lobby;

		// Token: 0x04000E5C RID: 3676
		[SerializeField]
		protected PlayerListEntry playerPrefab;

		// Token: 0x04000E5D RID: 3677
		[SerializeField]
		protected PlayerListEntry friendPrefab;

		// Token: 0x04000E5E RID: 3678
		[SerializeField]
		private Transform content;

		// Token: 0x04000E5F RID: 3679
		[SerializeField]
		private Image playersTab;

		// Token: 0x04000E60 RID: 3680
		[SerializeField]
		private Image friendsTab;

		// Token: 0x04000E61 RID: 3681
		[SerializeField]
		private Image playersTabImage;

		// Token: 0x04000E62 RID: 3682
		[SerializeField]
		private Image friendsTabImage;

		// Token: 0x04000E63 RID: 3683
		[SerializeField]
		private GameObject addFriendPanel;

		// Token: 0x04000E64 RID: 3684
		[SerializeField]
		protected TMP_InputField addFriendName;

		// Token: 0x04000E65 RID: 3685
		[SerializeField]
		protected TextMeshProUGUI addFriendError;

		// Token: 0x04000E66 RID: 3686
		[SerializeField]
		protected TextMeshProUGUI playersInLobbyAmount;

		// Token: 0x04000E67 RID: 3687
		[SerializeField]
		protected TextMeshProUGUI friendsOnlineAmount;

		// Token: 0x04000E68 RID: 3688
		protected Dictionary<Guid, PlayerListEntry> playersInLobby = new Dictionary<Guid, PlayerListEntry>();

		// Token: 0x04000E69 RID: 3689
		protected FriendsLogic friendsLogic;

		// Token: 0x04000E6A RID: 3690
		private bool friendsPanelActive;
	}
}

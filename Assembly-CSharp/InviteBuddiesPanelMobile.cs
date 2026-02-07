using System;
using System.Collections.Generic;
using Scythe.Analytics;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class InviteBuddiesPanelMobile : MonoBehaviour
{
	// Token: 0x060001F1 RID: 497 RVA: 0x0002915E File Offset: 0x0002735E
	public void Show()
	{
		this.Init();
		base.gameObject.SetActive(true);
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x00029172 File Offset: 0x00027372
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x00029180 File Offset: 0x00027380
	public void Init()
	{
		this.ClearPlayers();
		this.AddPlayers();
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0005A3CC File Offset: 0x000585CC
	public void AddPlayers()
	{
		foreach (KeyValuePair<Guid, PlayerInfo> keyValuePair in LobbyLogic.Instance.PlayersListLogic.Players)
		{
			if (!(keyValuePair.Value.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id))
			{
				this.AddPlayer(keyValuePair.Value);
			}
		}
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0005A458 File Offset: 0x00058658
	public bool AddPlayer(PlayerInfo playerInfo)
	{
		if (this.players == null)
		{
			this.players = new List<InviteBuddiesListEntryMobile>();
		}
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].PlayerInfo.PlayerStats.Id == playerInfo.PlayerStats.Id)
			{
				return false;
			}
		}
		InviteBuddiesListEntryMobile component = global::UnityEngine.Object.Instantiate<GameObject>(this.prefab, this.listParent.transform).GetComponent<InviteBuddiesListEntryMobile>();
		this.players.Add(component);
		component.PlayerInfo = playerInfo;
		return true;
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0005A4F0 File Offset: 0x000586F0
	public bool RemovePlayer(PlayerInfo playerInfo)
	{
		if (this.players == null)
		{
			this.players = new List<InviteBuddiesListEntryMobile>();
		}
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].PlayerInfo.PlayerStats.Id == playerInfo.PlayerStats.Id)
			{
				global::UnityEngine.Object.Destroy(this.players[i].gameObject);
				this.players.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0005A578 File Offset: 0x00058778
	public bool RemovePlayer(Guid id)
	{
		if (this.players == null)
		{
			this.players = new List<InviteBuddiesListEntryMobile>();
		}
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].PlayerInfo.PlayerStats.Id == id)
			{
				global::UnityEngine.Object.Destroy(this.players[i].gameObject);
				this.players.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0005A5F8 File Offset: 0x000587F8
	public void InviteButtonClick()
	{
		List<PlayerInfo> list = new List<PlayerInfo>();
		if (this.players != null)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				if (this.players[i].IsSelected)
				{
					list.Add(this.players[i].PlayerInfo);
				}
			}
			Debug.Log("INVITE: " + list.Count.ToString());
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.inviteSent);
			for (int j = 0; j < list.Count; j++)
			{
				AnalyticsEventLogger.Instance.LogFriendManagement(ActionsOnFriend.invite, list[j].PlayerStats.Id.ToString(), LobbyLogic.Instance.FriendsLogic.Friends.Count);
				LobbyRestAPI.InvitePlayer(list[j].PlayerStats.Id, new Action<string>(this.PlayerInvitationResponseSuccess));
			}
			this.Hide();
		}
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0005A6F0 File Offset: 0x000588F0
	private void ClearPlayers()
	{
		if (this.players == null)
		{
			this.players = new List<InviteBuddiesListEntryMobile>();
		}
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.RemovePlayer(this.players[i].PlayerInfo))
			{
				i--;
			}
		}
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0002918E File Offset: 0x0002738E
	private void PlayerInvitationResponseSuccess(string message)
	{
		Debug.Log("PlayerInvitationResponseSuccess: " + message);
	}

	// Token: 0x0400017D RID: 381
	[SerializeField]
	private GameObject prefab;

	// Token: 0x0400017E RID: 382
	[SerializeField]
	private GameObject listParent;

	// Token: 0x0400017F RID: 383
	[SerializeField]
	private GameObject loadingPanel;

	// Token: 0x04000180 RID: 384
	[SerializeField]
	private List<InviteBuddiesListEntryMobile> players;
}

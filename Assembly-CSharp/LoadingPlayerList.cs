using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class LoadingPlayerList : MonoBehaviour
{
	// Token: 0x060003E5 RID: 997 RVA: 0x00062AC8 File Offset: 0x00060CC8
	public void FillPlayerInfoList(List<Player> players, Sprite[] logos)
	{
		LoadingPlayerInfo[] array = this.playerInfoList;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < players.Count; j++)
		{
			this.playerInfoList[j].FillPlayerInfo(ScriptLocalization.Get("FactionMat/" + players[j].matFaction.faction.ToString()), players[j].Name, logos[(int)players[j].matFaction.faction]);
			this.playerInfoList[j].gameObject.SetActive(true);
		}
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x00062B74 File Offset: 0x00060D74
	public void FillPlayerInfoList(List<PlayerData> players, Sprite[] logos)
	{
		LoadingPlayerInfo[] array = this.playerInfoList;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < players.Count; j++)
		{
			this.playerInfoList[j].FillPlayerInfo(ScriptLocalization.Get("FactionMat/" + ((Faction)players[j].Faction).ToString()), players[j].Name, logos[players[j].Faction]);
			this.playerInfoList[j].gameObject.SetActive(true);
		}
	}

	// Token: 0x04000359 RID: 857
	[SerializeField]
	private LoadingPlayerInfo[] playerInfoList;
}

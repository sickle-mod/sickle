using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Scythe.Multiplayer.Data;
using Scythe.Utilities;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x0200026D RID: 621
	public class PlayerListLogic
	{
		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x00034A31 File Offset: 0x00032C31
		// (set) Token: 0x06001300 RID: 4864 RVA: 0x00034A39 File Offset: 0x00032C39
		public Dictionary<Guid, PlayerInfo> Players { get; private set; }

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06001301 RID: 4865 RVA: 0x00097A70 File Offset: 0x00095C70
		// (remove) Token: 0x06001302 RID: 4866 RVA: 0x00097AA8 File Offset: 0x00095CA8
		public event Action<PlayerInfo> PlayerDownloaded;

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06001303 RID: 4867 RVA: 0x00097AE0 File Offset: 0x00095CE0
		// (remove) Token: 0x06001304 RID: 4868 RVA: 0x00097B18 File Offset: 0x00095D18
		public event global::System.Action PlayersListDownloaded;

		// Token: 0x06001305 RID: 4869 RVA: 0x00034A42 File Offset: 0x00032C42
		public PlayerListLogic()
		{
			this.Players = new Dictionary<Guid, PlayerInfo>();
			this.morePlayersToDownload = true;
			this.waitingForResponse = false;
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x00034A63 File Offset: 0x00032C63
		public bool IsPlayersListDownloaded()
		{
			return !this.morePlayersToDownload;
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x00034A6E File Offset: 0x00032C6E
		public void Clear()
		{
			this.morePlayersToDownload = true;
			this.waitingForResponse = false;
			this.ClearData();
			this.ClearEvents();
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x00034A8A File Offset: 0x00032C8A
		public IEnumerator DownloadPlayerList()
		{
			this.morePlayersToDownload = true;
			while (this.morePlayersToDownload)
			{
				this.DownloadBunchOfPlayers();
				while (this.waitingForResponse)
				{
					yield return new WaitForSeconds(1f);
				}
			}
			if (this.PlayersListDownloaded != null)
			{
				this.PlayersListDownloaded();
			}
			yield break;
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x00034A99 File Offset: 0x00032C99
		private void DownloadBunchOfPlayers()
		{
			this.waitingForResponse = true;
			LobbyRestAPI.GetPlayersInLobby(delegate(string response)
			{
				this.waitingForResponse = false;
				List<PlayerInfo> list = GameSerializer.DeserializeObject<List<PlayerInfo>>(response);
				int num = 0;
				foreach (PlayerInfo playerInfo in list)
				{
					if (!this.Players.ContainsKey(playerInfo.PlayerStats.Id))
					{
						this.Players.Add(playerInfo.PlayerStats.Id, playerInfo);
						if (this.PlayerDownloaded != null)
						{
							this.PlayerDownloaded(playerInfo);
						}
						num++;
					}
				}
				if (num < 3)
				{
					this.morePlayersToDownload = false;
				}
			}, delegate(Exception exception)
			{
				this.waitingForResponse = false;
			});
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x00034ABF File Offset: 0x00032CBF
		public void AddNewPlayer(PlayerInfo player)
		{
			if (!this.Players.ContainsKey(player.PlayerStats.Id))
			{
				this.Players.Add(player.PlayerStats.Id, player);
			}
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x00034AF0 File Offset: 0x00032CF0
		public void RemovePlayer(Guid id)
		{
			if (this.Players.ContainsKey(id))
			{
				this.Players.Remove(id);
			}
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x00034B0D File Offset: 0x00032D0D
		private void ClearData()
		{
			this.Players.Clear();
			if (this.downloadListTimer != null)
			{
				this.downloadListTimer.Stop();
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x00034B2D File Offset: 0x00032D2D
		private void ClearEvents()
		{
			this.PlayerDownloaded = null;
			this.PlayersListDownloaded = null;
		}

		// Token: 0x04000E55 RID: 3669
		private bool morePlayersToDownload;

		// Token: 0x04000E56 RID: 3670
		private bool waitingForResponse;

		// Token: 0x04000E57 RID: 3671
		private Timer downloadListTimer;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using Scythe.Analytics;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x0200029A RID: 666
	public class PlayerStatsScreen : MonoBehaviour
	{
		// Token: 0x0600150D RID: 5389 RVA: 0x0002920A File Offset: 0x0002740A
		public void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0009CA44 File Offset: 0x0009AC44
		public void Activate(PlayerStats playerStats, Lobby lobby)
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.statistics_multiplayer, Contexts.outgame);
			this.lobby = lobby;
			this.playerStats = playerStats;
			this.lastGamesText.text = ScriptLocalization.Get("Statistics/RecentGames") + " " + ScriptLocalization.Get("Statistics/10Games");
			this.numberOfNormalGames = 0;
			this.numberOfWonNormalGames = 0;
			this.numberOfRankedGames = 0;
			this.numberOfWonRankedGames = 0;
			this.ClearRecentGamesList();
			this.IterateOverFactionAndMats();
			this.UpdateLeftPanel();
			this.UpdateMiddlePanel();
			this.UpdateRightPanel();
			this.SetActive(true);
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x000363FF File Offset: 0x000345FF
		public void Deactivate()
		{
			this.SetActive(false);
			this.ClearRecentGamesList();
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x0003640E File Offset: 0x0003460E
		public void DeactivatePlayerStatsPresenter()
		{
			this.playerStatsPresenter.HideStats();
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x0009CAD8 File Offset: 0x0009ACD8
		private void ClearRecentGamesList()
		{
			foreach (RecentGameEntry recentGameEntry in this.recentGamesEntries)
			{
				global::UnityEngine.Object.Destroy(recentGameEntry.gameObject);
			}
			this.recentGamesEntries.Clear();
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0009CB38 File Offset: 0x0009AD38
		private void UpdateLeftPanel()
		{
			this.playerName.text = this.playerStats.Name;
			this.eloPoints.text = this.playerStats.ELO.ToString();
			this.rankingPosition.text = this.playerStats.RankingPosition.ToString();
			this.UpdateKarma();
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x0009CB98 File Offset: 0x0009AD98
		public void UpdateKarma()
		{
			int num = ((this.playerStats.Karma < 100) ? (this.playerStats.Karma / 20 + 1) : this.karmaSigns.Length);
			for (int i = 0; i < num; i++)
			{
				this.karmaSigns[i].sprite = this.karmaLevels[num - 1];
				this.karmaSigns[i].gameObject.SetActive(true);
			}
			for (int j = num; j < this.karmaSigns.Length; j++)
			{
				this.karmaSigns[j].gameObject.SetActive(false);
			}
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x0009CC2C File Offset: 0x0009AE2C
		private void UpdateMiddlePanel()
		{
			List<GameHistoryEntry> gamesHistory = this.playerStats.GetGamesHistory();
			if (gamesHistory.Count == 0)
			{
				this.noRecentGamesText.gameObject.SetActive(true);
				return;
			}
			this.noRecentGamesText.gameObject.SetActive(false);
			for (int i = gamesHistory.Count - 1; i >= 0; i--)
			{
				RecentGameEntry recentGameEntry = global::UnityEngine.Object.Instantiate<RecentGameEntry>(this.recentGameEntryPrefab, this.recentGamesList.content);
				this.recentGamesEntries.Add(recentGameEntry);
				recentGameEntry.Initialize(this.playerStats, gamesHistory[i], this);
			}
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x0003641B File Offset: 0x0003461B
		public Sprite[] FactionSprites()
		{
			return this.factionLogos;
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x00036423 File Offset: 0x00034623
		public void RedrawList()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.recentGamesList.content);
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0009CCBC File Offset: 0x0009AEBC
		public void ScrollToRecentGameEntry(string gameId)
		{
			int num = this.playerStats.GetGamesHistory().FindIndex((GameHistoryEntry item) => item.GameId == gameId);
			int num2 = this.recentGamesEntries.Count - num - 1;
			for (int i = 0; i < this.recentGamesEntries.Count; i++)
			{
				if (i == num2)
				{
					this.recentGamesEntries[i].ShowPlayersList();
				}
				else
				{
					this.recentGamesEntries[i].HidePlayersList();
				}
			}
			if (num == -1)
			{
				return;
			}
			this.recentGamesList.content.anchoredPosition = new Vector2(this.recentGamesList.content.anchoredPosition.x, (this.recentGamesList.transform.InverseTransformPoint(this.recentGamesList.content.position) - this.recentGamesList.transform.InverseTransformPoint(this.recentGamesEntries[num2].GetComponent<RectTransform>().position)).y);
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x0009CDCC File Offset: 0x0009AFCC
		private void UpdateRightPanel()
		{
			if (this.gamesPlayed != null)
			{
				this.gamesPlayed.text = (this.numberOfNormalGames + this.numberOfRankedGames).ToString();
			}
			this.wonNormalGames.text = this.numberOfWonNormalGames.ToString();
			TextMeshProUGUI textMeshProUGUI = this.wonNormalGames;
			textMeshProUGUI.text = string.Concat(new string[]
			{
				textMeshProUGUI.text,
				"<size=",
				this.totalNumberOfNormalGamesFontSize.ToString(),
				">/",
				this.numberOfNormalGames.ToString()
			});
			this.wonRankedGames.text = this.numberOfWonRankedGames.ToString();
			textMeshProUGUI = this.wonRankedGames;
			textMeshProUGUI.text = string.Concat(new string[]
			{
				textMeshProUGUI.text,
				"<size=",
				this.totalNumberOfRankedGamesFontSize.ToString(),
				">/",
				this.numberOfRankedGames.ToString()
			});
			if (this.playerStats.GetGamesHistory().Count > 0 && this.recentlyPlayedFaction != null)
			{
				this.recentlyPlayedFaction.sprite = this.factionLogos[this.playerStats.GetGamesHistory().Last<GameHistoryEntry>().Faction];
			}
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0009CF18 File Offset: 0x0009B118
		protected void IterateOverFactionAndMats()
		{
			for (int i = 0; i < 7; i++)
			{
				for (int j = 0; j < 7; j++)
				{
					this.numberOfNormalGames += this.playerStats.PlayerFactionStats[i][j].GamesAmount;
					this.numberOfWonNormalGames += this.playerStats.PlayerFactionStats[i][j].PlacesTwoPlayersGames[0];
					this.numberOfWonNormalGames += this.playerStats.PlayerFactionStats[i][j].PlacesThreePlayersGames[0];
					this.numberOfWonNormalGames += this.playerStats.PlayerFactionStats[i][j].PlacesFourPlayersGames[0];
					this.numberOfWonNormalGames += this.playerStats.PlayerFactionStats[i][j].PlacesFivePlayersGames[0];
					this.numberOfWonNormalGames += this.playerStats.PlayerFactionStats[i][j].PlacesSixPlayersGames[0];
					this.numberOfWonNormalGames += this.playerStats.PlayerFactionStats[i][j].PlacesSevenPlayersGames[0];
					this.numberOfRankedGames += this.playerStats.PlayerFactionStats[i][j].RankedGamesAmount;
					this.numberOfWonRankedGames += this.playerStats.PlayerFactionStats[i][j].RankedPlacesTwoPlayersGames[0];
					this.numberOfWonRankedGames += this.playerStats.PlayerFactionStats[i][j].RankedPlacesThreePlayersGames[0];
					this.numberOfWonRankedGames += this.playerStats.PlayerFactionStats[i][j].RankedPlacesFourPlayersGames[0];
					this.numberOfWonRankedGames += this.playerStats.PlayerFactionStats[i][j].RankedPlacesFivePlayersGames[0];
					this.numberOfWonRankedGames += this.playerStats.PlayerFactionStats[i][j].RankedPlacesSixPlayersGames[0];
					this.numberOfWonRankedGames += this.playerStats.PlayerFactionStats[i][j].RankedPlacesSevenPlayersGames[0];
				}
			}
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x00036435 File Offset: 0x00034635
		public void OnShowMoreStatsClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_more_stats_button);
			this.playerStatsPresenter.ShowStats(this.playerStats, Screens.statistics_multiplayer, false);
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x0003645E File Offset: 0x0003465E
		public void OnBackToLobbyClicked()
		{
			this.Deactivate();
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_back_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.lobby, Contexts.outgame);
			this.lobby.ReturnedToMainLobbyFromPlayerStats();
		}

		// Token: 0x04000F59 RID: 3929
		private const int KARMA_PER_SIGN = 20;

		// Token: 0x04000F5A RID: 3930
		[SerializeField]
		private TextMeshProUGUI playerName;

		// Token: 0x04000F5B RID: 3931
		[SerializeField]
		private TextMeshProUGUI eloPoints;

		// Token: 0x04000F5C RID: 3932
		[SerializeField]
		private TextMeshProUGUI rankingPosition;

		// Token: 0x04000F5D RID: 3933
		[SerializeField]
		private Image[] karmaSigns;

		// Token: 0x04000F5E RID: 3934
		[SerializeField]
		private Sprite[] karmaLevels;

		// Token: 0x04000F5F RID: 3935
		[SerializeField]
		private TextMeshProUGUI lastGamesText;

		// Token: 0x04000F60 RID: 3936
		[SerializeField]
		private ScrollRect recentGamesList;

		// Token: 0x04000F61 RID: 3937
		[SerializeField]
		private TextMeshProUGUI noRecentGamesText;

		// Token: 0x04000F62 RID: 3938
		[SerializeField]
		private RecentGameEntry recentGameEntryPrefab;

		// Token: 0x04000F63 RID: 3939
		[SerializeField]
		private Sprite[] factionLogos;

		// Token: 0x04000F64 RID: 3940
		[SerializeField]
		private TextMeshProUGUI gamesPlayed;

		// Token: 0x04000F65 RID: 3941
		[SerializeField]
		private TextMeshProUGUI wonNormalGames;

		// Token: 0x04000F66 RID: 3942
		[SerializeField]
		private int totalNumberOfNormalGamesFontSize = 12;

		// Token: 0x04000F67 RID: 3943
		[SerializeField]
		private TextMeshProUGUI wonRankedGames;

		// Token: 0x04000F68 RID: 3944
		[SerializeField]
		private int totalNumberOfRankedGamesFontSize = 12;

		// Token: 0x04000F69 RID: 3945
		[SerializeField]
		private Image recentlyPlayedFaction;

		// Token: 0x04000F6A RID: 3946
		[SerializeField]
		private PlayerStatsPresenter playerStatsPresenter;

		// Token: 0x04000F6B RID: 3947
		private Lobby lobby;

		// Token: 0x04000F6C RID: 3948
		private PlayerStats playerStats;

		// Token: 0x04000F6D RID: 3949
		private int numberOfNormalGames;

		// Token: 0x04000F6E RID: 3950
		private int numberOfWonNormalGames;

		// Token: 0x04000F6F RID: 3951
		private int numberOfRankedGames;

		// Token: 0x04000F70 RID: 3952
		private int numberOfWonRankedGames;

		// Token: 0x04000F71 RID: 3953
		private List<RecentGameEntry> recentGamesEntries = new List<RecentGameEntry>();
	}
}

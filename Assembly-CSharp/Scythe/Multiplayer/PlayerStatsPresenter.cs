using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Analytics;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000299 RID: 665
	public class PlayerStatsPresenter : MonoBehaviour
	{
		// Token: 0x060014EE RID: 5358 RVA: 0x0009C348 File Offset: 0x0009A548
		private void Start()
		{
			if (PlatformManager.IsMobile)
			{
				this.rankedFilterToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnRankedToggleValueChanged));
				this.normalFilterToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnNormalToggleValueChanged));
				this.allFilterToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnAllToggleValueChanged));
			}
			else
			{
				this.UpdateRankedFilterTexts();
				this.UpdateScreenToggles();
			}
			for (int i = 0; i < this.screens.Length; i++)
			{
				if (this.screens[i].activeInHierarchy)
				{
					this.activePanel = i;
				}
			}
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x00036293 File Offset: 0x00034493
		public RankedFilter GetCurrentFilter()
		{
			return this.actualRankedFilter;
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0003629B File Offset: 0x0003449B
		public Toggle[] GetFactionToggles()
		{
			return this.logoToggles;
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x000362A3 File Offset: 0x000344A3
		public Toggle[] GetPlayerMatsToggles()
		{
			return this.matToggles;
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x0002920A File Offset: 0x0002740A
		private void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x000362AB File Offset: 0x000344AB
		public void OnTabClicked(int tabId)
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
			this.ShowStatsWindow(tabId);
			if (PlatformManager.IsStandalone)
			{
				this.UpdateScreenToggles();
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0009C3E8 File Offset: 0x0009A5E8
		private void UpdateScreenToggles()
		{
			for (int i = 0; i < this.screenListToggles.Length; i++)
			{
				this.screenListToggles[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().font = (this.screenListToggles[i].isOn ? this.selectedScreenToggle : this.inactiveScreenToggle);
			}
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x0009C444 File Offset: 0x0009A644
		private void UpdateRankedFilterTexts()
		{
			this.filter.options = new List<TMP_Dropdown.OptionData>
			{
				new TMP_Dropdown.OptionData(ScriptLocalization.Get("Statistics/All")),
				new TMP_Dropdown.OptionData(ScriptLocalization.Get("Statistics/Ranked")),
				new TMP_Dropdown.OptionData(ScriptLocalization.Get("Statistics/Normal"))
			};
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0009C4A0 File Offset: 0x0009A6A0
		private void ShowStatsWindow(int id)
		{
			if (id >= 0 && id < this.statsTabWindows.Length && this.screenListToggles[id].isOn)
			{
				if (this.currentWindow != null)
				{
					this.currentWindow.Hide();
				}
				if (this.statsTabWindows[id] != null)
				{
					this.statsTabWindows[id].Show(this, this.actualPlayerStats);
				}
				if (this.currentWindow != this.statsTabWindows[id] || this.currentWindow == null)
				{
					if (!this.initializing)
					{
						AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_stats_overview_button + id);
					}
					AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.statistics_overview + id, Contexts.outgame);
				}
				this.currentWindow = this.statsTabWindows[id];
			}
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x0009C564 File Offset: 0x0009A764
		private void UpdateWindowForDLCs()
		{
			bool flag = GameServiceController.Instance.InvadersFromAfarUnlocked();
			this.logoToggles[1].gameObject.SetActive(flag);
			this.logoToggles[4].gameObject.SetActive(flag);
			this.matToggles[5].gameObject.SetActive(flag);
			this.matToggles[6].gameObject.SetActive(flag);
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x0009C5C8 File Offset: 0x0009A7C8
		public void ShowStats(PlayerStats playerStats, Screens previousScreen, bool localStats = false)
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.statistics_overview, Contexts.outgame);
			this.previousMainScreen = previousScreen;
			this.actualPlayerStats = playerStats;
			this.localStats = localStats;
			this.SetActive(true);
			this.initializing = true;
			if (localStats)
			{
				this.UpdateForLocalStats();
			}
			else if (PlatformManager.IsMobile)
			{
				this.UpdateForMultiplayerStats();
			}
			if (PlatformManager.IsStandalone && playerStats.PlayerFactionStats != null)
			{
				this.ShowMoreStats(playerStats);
			}
			this.UpdateWindowForDLCs();
			this.initializing = false;
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x0009C640 File Offset: 0x0009A840
		private void UpdateForLocalStats()
		{
			if (PlatformManager.IsMobile)
			{
				GameObject[] array = this.screens;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(false);
				}
				Toggle[] array2 = this.screenListToggles;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].isOn = false;
				}
				this.rightPanel.SetActive(false);
				this.screenListToggles[this.activePanel].isOn = true;
			}
			else
			{
				this.filter.gameObject.SetActive(false);
			}
			this.actualRankedFilter = RankedFilter.All;
			this.screenListToggles[0].gameObject.SetActive(false);
			this.statsTabWindows[0].Hide();
			this.activePanel = 1;
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0009C6F0 File Offset: 0x0009A8F0
		private void UpdateForMultiplayerStats()
		{
			GameObject[] array = this.screens;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			Toggle[] array2 = this.screenListToggles;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].isOn = false;
			}
			this.screenListToggles[0].isOn = true;
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0009C748 File Offset: 0x0009A948
		private void ShowMoreStats(PlayerStats playerStats)
		{
			if (playerStats.Name != null && this.playerName != null)
			{
				this.playerName.text = playerStats.Name.ToString();
			}
			this.screenListToggles[this.activePanel].isOn = false;
			this.screenListToggles[this.activePanel].isOn = true;
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x000362CD File Offset: 0x000344CD
		public void XButtonClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(this.previousMainScreen, Contexts.outgame);
			this.HideStats();
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x000362F8 File Offset: 0x000344F8
		public void HideStats()
		{
			if (this.currentWindow != null)
			{
				this.currentWindow.Hide();
			}
			this.currentWindow = null;
			this.SetActive(false);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0009C7A8 File Offset: 0x0009A9A8
		public void IndexButtonSelect()
		{
			for (int i = 0; i < this.screenListToggles.Length; i++)
			{
				if (this.screenListToggles[i].isOn && i != this.activePanel)
				{
					if (PlatformManager.IsStandalone)
					{
						this.screens[this.activePanel].SetActive(false);
					}
					this.activePanel = i;
					if (PlatformManager.IsStandalone)
					{
						this.screens[this.activePanel].SetActive(true);
					}
					AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_stats_overview_button + i);
					AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.statistics_overview + i, Contexts.outgame);
				}
			}
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00036321 File Offset: 0x00034521
		public void UpdateMatToogle()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
			if (this.currentWindow != null)
			{
				this.currentWindow.PlayerMatSelectionChanged();
			}
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x00036348 File Offset: 0x00034548
		public void UpdateFactionToogle()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
			if (this.currentWindow != null)
			{
				this.currentWindow.FactionSelectionChanged();
			}
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0003636F File Offset: 0x0003456F
		public void UpdateRankedFilter(int selectedFilter)
		{
			if (PlatformManager.IsStandalone)
			{
				ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
			}
			this.actualRankedFilter = (RankedFilter)selectedFilter;
			if (this.currentWindow != null)
			{
				this.currentWindow.RankedFilterChanged();
			}
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x000363A4 File Offset: 0x000345A4
		public void SaveRankedFiltr()
		{
			if (this.currentWindow != null)
			{
				this.currentWindow.RankedFilterChanged();
			}
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0009C840 File Offset: 0x0009AA40
		public bool AllFactionTogglesOff()
		{
			int num = 0;
			foreach (Toggle toggle in this.logoToggles)
			{
				if (toggle != null && !toggle.isOn)
				{
					num++;
				}
			}
			return num == this.logoToggles.Length;
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x0009C88C File Offset: 0x0009AA8C
		public bool AllMatsTogglesOff()
		{
			int num = 0;
			foreach (Toggle toggle in this.matToggles)
			{
				if (toggle != null && !toggle.isOn)
				{
					num++;
				}
			}
			return num == this.logoToggles.Length;
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x000363BF File Offset: 0x000345BF
		public bool LocalStats()
		{
			return this.localStats;
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x0009C8D8 File Offset: 0x0009AAD8
		private int GetNormalGamesAmount(PlayerStats playerStats)
		{
			int num = 0;
			for (int i = 0; i < 7; i++)
			{
				if (this.logoToggles[i] != null)
				{
					for (int j = 0; j < 5; j++)
					{
						num += playerStats.PlayerFactionStats[i][j].GamesAmount;
					}
				}
			}
			return num;
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x0009C924 File Offset: 0x0009AB24
		private int GetNormalGamesWonAll(PlayerStats playerStats)
		{
			int num = 0;
			for (int i = 0; i < 7; i++)
			{
				if (this.logoToggles[i] != null)
				{
					for (int j = 0; j < 5; j++)
					{
						num += playerStats.PlayerFactionStats[i][j].PlacesTwoPlayersGames[0];
						num += playerStats.PlayerFactionStats[i][j].PlacesThreePlayersGames[0];
						num += playerStats.PlayerFactionStats[i][j].PlacesFourPlayersGames[0];
						num += playerStats.PlayerFactionStats[i][j].PlacesFivePlayersGames[0];
					}
				}
			}
			return num;
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x0009C9AC File Offset: 0x0009ABAC
		private double GetAverageScoreAll(PlayerStats playerStats, RankedFilter rankedFiltr)
		{
			double num = 0.0;
			for (int i = 0; i < 7; i++)
			{
				if (this.logoToggles[i] != null)
				{
					for (int j = 0; j < 5; j++)
					{
						if (rankedFiltr == RankedFilter.All)
						{
							num += playerStats.PlayerFactionStats[i][j].AverageScore;
							num += playerStats.PlayerFactionStats[i][j].RankedAverageScore;
						}
						if (rankedFiltr == RankedFilter.Ranked)
						{
							num += playerStats.PlayerFactionStats[i][j].RankedAverageScore;
						}
						else if (rankedFiltr == RankedFilter.Normal)
						{
							num += playerStats.PlayerFactionStats[i][j].AverageScore;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x000363C7 File Offset: 0x000345C7
		private void OnRankedToggleValueChanged(bool isOn)
		{
			if (isOn)
			{
				this.UpdateRankedFilter(1);
			}
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x000363D3 File Offset: 0x000345D3
		private void OnNormalToggleValueChanged(bool isOn)
		{
			if (isOn)
			{
				this.UpdateRankedFilter(2);
			}
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x000363DF File Offset: 0x000345DF
		private void OnAllToggleValueChanged(bool isOn)
		{
			if (isOn)
			{
				this.UpdateRankedFilter(0);
			}
		}

		// Token: 0x04000F3F RID: 3903
		[SerializeField]
		private TextMeshProUGUI playerName;

		// Token: 0x04000F40 RID: 3904
		[SerializeField]
		private TMP_FontAsset selectedScreenToggle;

		// Token: 0x04000F41 RID: 3905
		[SerializeField]
		private TMP_FontAsset inactiveScreenToggle;

		// Token: 0x04000F42 RID: 3906
		[SerializeField]
		private Toggle[] logoToggles;

		// Token: 0x04000F43 RID: 3907
		[SerializeField]
		private Toggle[] matToggles;

		// Token: 0x04000F44 RID: 3908
		[SerializeField]
		private GameObject[] screens;

		// Token: 0x04000F45 RID: 3909
		private int activePanel;

		// Token: 0x04000F46 RID: 3910
		private bool initializing;

		// Token: 0x04000F47 RID: 3911
		private bool localStats;

		// Token: 0x04000F48 RID: 3912
		private Screens previousMainScreen;

		// Token: 0x04000F49 RID: 3913
		private PlayerStats actualPlayerStats;

		// Token: 0x04000F4A RID: 3914
		[SerializeField]
		private RankedFilter actualRankedFilter;

		// Token: 0x04000F4B RID: 3915
		[SerializeField]
		private TMP_Dropdown filter;

		// Token: 0x04000F4C RID: 3916
		[SerializeField]
		private GameObject rightPanel;

		// Token: 0x04000F4D RID: 3917
		[SerializeField]
		private Toggle rankedFilterToggle;

		// Token: 0x04000F4E RID: 3918
		[SerializeField]
		private Toggle normalFilterToggle;

		// Token: 0x04000F4F RID: 3919
		[SerializeField]
		private Toggle allFilterToggle;

		// Token: 0x04000F50 RID: 3920
		[SerializeField]
		private Toggle[] screenListToggles;

		// Token: 0x04000F51 RID: 3921
		[SerializeField]
		private StatsWindow[] statsTabWindows = new StatsWindow[0];

		// Token: 0x04000F52 RID: 3922
		[SerializeField]
		private StatsWindow overviewStatsPresenter;

		// Token: 0x04000F53 RID: 3923
		[SerializeField]
		private StatsWindow scoreStatsPresenter;

		// Token: 0x04000F54 RID: 3924
		[SerializeField]
		private StatsWindow placesStatsPresenter;

		// Token: 0x04000F55 RID: 3925
		[SerializeField]
		private StatsWindow combatStatsPresenter;

		// Token: 0x04000F56 RID: 3926
		[SerializeField]
		private StatsWindow othersStatsPresenter;

		// Token: 0x04000F57 RID: 3927
		private StatsWindow currentWindow;

		// Token: 0x04000F58 RID: 3928
		[SerializeField]
		private Color[] factionColors;
	}
}

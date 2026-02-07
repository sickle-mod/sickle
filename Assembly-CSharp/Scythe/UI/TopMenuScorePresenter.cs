using System;
using System.Collections.Generic;
using Scythe.Analytics;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004A5 RID: 1189
	public class TopMenuScorePresenter : MonoBehaviour
	{
		// Token: 0x060025C4 RID: 9668 RVA: 0x0004013F File Offset: 0x0003E33F
		private void Awake()
		{
			if (!this.isWinnerPanel)
			{
				this.InitScoreEntries();
				this.UpdateLegend();
			}
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x00040155 File Offset: 0x0003E355
		public void OnToggleClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.tab_score_toggle);
			base.gameObject.SetActive(true);
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000E0A70 File Offset: 0x000DEC70
		private void OnEnable()
		{
			if (!this.isWinnerPanel)
			{
				List<PlayerEndGameStats> list = GameController.GameManager.CalculateStats();
				this.ShowStats(list);
			}
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.tab_score, Contexts.ingame);
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x0004016A File Offset: 0x0003E36A
		public void ShowStats(List<PlayerEndGameStats> playerStats)
		{
			this.stats = playerStats;
			this.UpdateLegend();
			this.UpdateEntries(!this.detailsToggle.isOn);
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x0004018D File Offset: 0x0003E38D
		public List<PlayerEndGameStats> GetPlayerEndGameStats()
		{
			return this.stats;
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x00040195 File Offset: 0x0003E395
		public void DetailsToggle_OnValueChanged()
		{
			WorldSFXManager.PlaySound(SoundEnum.GuiChatButton, AudioSourceType.Buttons);
			this.UpdateLegend();
			this.UpdateEntries(!this.detailsToggle.isOn);
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x000401B9 File Offset: 0x0003E3B9
		private void UpdateLegend()
		{
			this.topMenuScoreLegend.UpdateLegend();
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x000E0AA4 File Offset: 0x000DECA4
		public void UpdateEntries(bool basic)
		{
			using (List<TopMenuScoreEntry>.Enumerator enumerator = this.topMenuScoreEntries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TopMenuScoreEntry scoreEntry = enumerator.Current;
					PlayerEndGameStats playerEndGameStats = this.stats.Find((PlayerEndGameStats stat) => stat.player.matFaction.faction == scoreEntry.Faction);
					scoreEntry.UpdateEntry(playerEndGameStats, basic);
				}
			}
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x000E0B20 File Offset: 0x000DED20
		public void UpdateEntriesLanguage()
		{
			using (List<TopMenuScoreEntry>.Enumerator enumerator = this.topMenuScoreEntries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TopMenuScoreEntry scoreEntry = enumerator.Current;
					PlayerEndGameStats playerEndGameStats = this.stats.Find((PlayerEndGameStats stat) => stat.player.matFaction.faction == scoreEntry.Faction);
					scoreEntry.UpdateEntryLanguage(playerEndGameStats, !this.detailsToggle.isOn);
				}
			}
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000E0BAC File Offset: 0x000DEDAC
		private void InitScoreEntries()
		{
			this.topMenuScoreEntries = new List<TopMenuScoreEntry>();
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.scoreEntryPrefab);
				gameObject.transform.SetParent(this.scoreEntryParent.transform, false);
				TopMenuScoreEntry component = gameObject.GetComponent<TopMenuScoreEntry>();
				component.InitializeEntry(player, this.GetPlayerBackgroundColor(player.matFaction.faction));
				this.topMenuScoreEntries.Add(component);
			}
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x000E0C54 File Offset: 0x000DEE54
		public void InitScoreEntries(List<PlayerEndGameStats> playerStats)
		{
			this.topMenuScoreEntries = new List<TopMenuScoreEntry>();
			foreach (PlayerEndGameStats playerEndGameStats in playerStats)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.scoreEntryPrefab);
				gameObject.transform.SetParent(this.scoreEntryParent.transform, false);
				TopMenuScoreEntry component = gameObject.GetComponent<TopMenuScoreEntry>();
				component.InitializeEntry(playerEndGameStats.player, this.GetPlayerBackgroundColor(playerEndGameStats.player.matFaction.faction));
				this.topMenuScoreEntries.Add(component);
			}
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x000401C6 File Offset: 0x0003E3C6
		public void SetPlaceOnPodium(int playerIndex, int playerPlace)
		{
			this.topMenuScoreEntries[playerIndex].SetPlaceOnPodium(new int?(playerPlace));
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x000E0CFC File Offset: 0x000DEEFC
		public void Reset()
		{
			foreach (TopMenuScoreEntry topMenuScoreEntry in this.topMenuScoreEntries)
			{
				global::UnityEngine.Object.Destroy(topMenuScoreEntry.gameObject);
			}
			this.topMenuScoreEntries.Clear();
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x000E0D5C File Offset: 0x000DEF5C
		private Color GetPlayerBackgroundColor(Faction faction)
		{
			switch (faction)
			{
			case Faction.Polania:
				return this.backgroundEntryColor[0];
			case Faction.Albion:
				return this.backgroundEntryColor[1];
			case Faction.Nordic:
				return this.backgroundEntryColor[2];
			case Faction.Rusviet:
				return this.backgroundEntryColor[3];
			case Faction.Togawa:
				return this.backgroundEntryColor[4];
			case Faction.Crimea:
				return this.backgroundEntryColor[5];
			case Faction.Saxony:
				return this.backgroundEntryColor[6];
			default:
				return this.backgroundEntryColor[7];
			}
		}

		// Token: 0x04001AA6 RID: 6822
		[SerializeField]
		private GameObject scoreEntryPrefab;

		// Token: 0x04001AA7 RID: 6823
		[SerializeField]
		private Transform scoreEntryParent;

		// Token: 0x04001AA8 RID: 6824
		[SerializeField]
		private TopMenuScoreLegend topMenuScoreLegend;

		// Token: 0x04001AA9 RID: 6825
		[SerializeField]
		private List<TopMenuScoreEntry> topMenuScoreEntries;

		// Token: 0x04001AAA RID: 6826
		[SerializeField]
		private Color[] backgroundEntryColor = new Color[7];

		// Token: 0x04001AAB RID: 6827
		[SerializeField]
		private Toggle detailsToggle;

		// Token: 0x04001AAC RID: 6828
		[SerializeField]
		private bool isWinnerPanel;

		// Token: 0x04001AAD RID: 6829
		private List<PlayerEndGameStats> stats;
	}
}

using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004AF RID: 1199
	public class TopMenuTracksPresenter : MonoBehaviour
	{
		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06002604 RID: 9732 RVA: 0x000404C2 File Offset: 0x0003E6C2
		// (set) Token: 0x06002605 RID: 9733 RVA: 0x000404CA File Offset: 0x0003E6CA
		private Dictionary<Faction, GameObject> playerFactionPowerMarks { get; set; }

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06002606 RID: 9734 RVA: 0x000404D3 File Offset: 0x0003E6D3
		// (set) Token: 0x06002607 RID: 9735 RVA: 0x000404DB File Offset: 0x0003E6DB
		private Dictionary<Faction, GameObject> playerFactionPopularityMarks { get; set; }

		// Token: 0x06002608 RID: 9736 RVA: 0x000E1DAC File Offset: 0x000DFFAC
		private void Awake()
		{
			if (this.playerFactionPowerMarks == null)
			{
				this.playerFactionPowerMarks = new Dictionary<Faction, GameObject>();
			}
			else
			{
				this.playerFactionPowerMarks.Clear();
			}
			if (this.playerFactionPopularityMarks == null)
			{
				this.playerFactionPopularityMarks = new Dictionary<Faction, GameObject>();
			}
			else
			{
				this.playerFactionPopularityMarks.Clear();
			}
			this.InitTracks();
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x000404E4 File Offset: 0x0003E6E4
		public void OnToggleClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.tab_tracks_toggle);
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x000404F9 File Offset: 0x0003E6F9
		private void OnEnable()
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.tab_tracks, Contexts.ingame);
			this.UpdateTracks();
			this.UpdateTierLabels();
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000E1E00 File Offset: 0x000E0000
		private void InitTracks()
		{
			int length = Enum.GetValues(typeof(Faction)).Length;
			int length2 = Enum.GetValues(typeof(StarType)).Length;
			for (int i = 0; i < length; i++)
			{
				Player playerByFaction = GameController.GameManager.GetPlayerByFaction((Faction)i);
				if (playerByFaction != null && this.colorMarkPrefab != null)
				{
					GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.colorMarkPrefab);
					gameObject.transform.SetParent(this.powerTrackSlots[playerByFaction.Power].transform, false);
					gameObject.GetComponent<Image>().sprite = ColorFactionMarks.colorFactionMarks[playerByFaction.matFaction.faction].powerMark;
					this.playerFactionPowerMarks.Add(playerByFaction.matFaction.faction, gameObject);
					GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(this.colorMarkPrefab);
					gameObject2.transform.SetParent(this.popularityTrackSlots[playerByFaction.Popularity].transform, false);
					gameObject2.GetComponent<Image>().sprite = ColorFactionMarks.colorFactionMarks[playerByFaction.matFaction.faction].popularityMark;
					this.playerFactionPopularityMarks.Add(playerByFaction.matFaction.faction, gameObject2);
				}
			}
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x000E1F3C File Offset: 0x000E013C
		public void UpdateTracks()
		{
			int length = Enum.GetValues(typeof(Faction)).Length;
			int length2 = Enum.GetValues(typeof(StarType)).Length;
			for (int i = 0; i < length; i++)
			{
				Player playerByFaction = GameController.GameManager.GetPlayerByFaction((Faction)i);
				if (playerByFaction != null)
				{
					if (this.playerFactionPowerMarks != null)
					{
						this.playerFactionPowerMarks[playerByFaction.matFaction.faction].transform.SetParent(this.powerTrackSlots[playerByFaction.Power].transform, false);
					}
					if (this.playerFactionPopularityMarks != null)
					{
						this.playerFactionPopularityMarks[playerByFaction.matFaction.faction].transform.SetParent(this.popularityTrackSlots[playerByFaction.Popularity].transform, false);
					}
				}
			}
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x000E200C File Offset: 0x000E020C
		public void UpdateTierLabels()
		{
			string text = ScriptLocalization.Get("Statistics/Tier");
			this.tierLabels[0].text = text + " I";
			this.tierLabels[1].text = text + " II";
			this.tierLabels[2].text = text + " III";
		}

		// Token: 0x04001AF6 RID: 6902
		[SerializeField]
		private Transform[] powerTrackSlots;

		// Token: 0x04001AF7 RID: 6903
		[SerializeField]
		private Transform[] popularityTrackSlots;

		// Token: 0x04001AF8 RID: 6904
		[SerializeField]
		private TextMeshProUGUI[] tierLabels;

		// Token: 0x04001AF9 RID: 6905
		[SerializeField]
		private GameObject colorMarkPrefab;
	}
}

using System;
using System.Collections.Generic;
using Scythe.Analytics;
using Scythe.GameLogic;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020004A9 RID: 1193
	public class TopMenuStarsPresenter : MonoBehaviour
	{
		// Token: 0x060025DB RID: 9691 RVA: 0x000E11F8 File Offset: 0x000DF3F8
		private void Awake()
		{
			this.topMenuStarsEntries = new List<TopMenuStarsEntry>();
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.starsEntry);
				gameObject.transform.SetParent(this.starsEntriesParent.transform, false);
				TopMenuStarsEntry component = gameObject.GetComponent<TopMenuStarsEntry>();
				component.SetEntryPlayerOwner(player);
				this.topMenuStarsEntries.Add(component);
				component.SetStars();
			}
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x0004029B File Offset: 0x0003E49B
		public void OnToggleClicked()
		{
			if (!GameController.GameManager.IsCampaign)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.tab_stars_toggle);
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x000402BC File Offset: 0x0003E4BC
		private void OnEnable()
		{
			this.UpdatePlayersEntries();
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.tab_stars, Contexts.ingame);
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x000E1294 File Offset: 0x000DF494
		public void UpdatePlayersEntries()
		{
			foreach (TopMenuStarsEntry topMenuStarsEntry in this.topMenuStarsEntries)
			{
				topMenuStarsEntry.SetStars();
			}
		}

		// Token: 0x04001ABC RID: 6844
		[SerializeField]
		private List<TopMenuStarsEntry> topMenuStarsEntries;

		// Token: 0x04001ABD RID: 6845
		[SerializeField]
		private GameObject starsEntry;

		// Token: 0x04001ABE RID: 6846
		[SerializeField]
		private Transform starsEntriesParent;
	}
}

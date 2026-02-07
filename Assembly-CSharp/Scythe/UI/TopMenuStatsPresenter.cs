using System;
using System.Collections.Generic;
using Scythe.Analytics;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004AB RID: 1195
	public class TopMenuStatsPresenter : MonoBehaviour
	{
		// Token: 0x060025E8 RID: 9704 RVA: 0x00040346 File Offset: 0x0003E546
		private void Awake()
		{
			this.topMenuStatsEntries = new List<TopMenuStatsEntry>();
			this.InitStatsEntries();
			this.CalculateVisiblity();
			this.UpdateVisibility();
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x00040365 File Offset: 0x0003E565
		public void OnToggleClicked()
		{
			if (!GameController.GameManager.IsCampaign)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.tab_stats_toggle);
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x00040386 File Offset: 0x0003E586
		public void OnScrollToLeftClicked()
		{
			WorldSFXManager.PlaySound(SoundEnum.GuiChatButton, AudioSourceType.Buttons);
			this.visibility = TopMenuStatsPresenter.Visibility.Left;
			this.UpdateVisibility();
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x0004039D File Offset: 0x0003E59D
		public void OnScrollToRightClicked()
		{
			WorldSFXManager.PlaySound(SoundEnum.GuiChatButton, AudioSourceType.Buttons);
			this.visibility = TopMenuStatsPresenter.Visibility.Right;
			this.UpdateVisibility();
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x000403B4 File Offset: 0x0003E5B4
		private void OnEnable()
		{
			this.UpdateAllEntries();
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.tab_stats, Contexts.ingame);
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000E1580 File Offset: 0x000DF780
		private void InitStatsEntries()
		{
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.statsEntry);
				gameObject.transform.SetParent(this.statsEntriesParent.transform, false);
				TopMenuStatsEntry component = gameObject.GetComponent<TopMenuStatsEntry>();
				component.InitializeEntry(player, this.GetPlayerBackgroundColor(player.matFaction.faction));
				component.UpdateEntry(player);
				this.topMenuStatsEntries.Add(component);
			}
			this.buttonsRow.transform.SetAsLastSibling();
			this.isInitialized = true;
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000E163C File Offset: 0x000DF83C
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

		// Token: 0x060025EF RID: 9711 RVA: 0x000E16D4 File Offset: 0x000DF8D4
		public void UpdateSingleEntry(Player player)
		{
			if (!this.isInitialized)
			{
				return;
			}
			this.topMenuStatsEntries.Find((TopMenuStatsEntry entry) => entry.Faction == player.matFaction.faction).UpdateEntry(player);
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x000E171C File Offset: 0x000DF91C
		public void UpdateAllEntries()
		{
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				this.UpdateSingleEntry(player);
			}
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000E1774 File Offset: 0x000DF974
		private void CalculateVisiblity()
		{
			int num = 0;
			if ((PlatformManager.DeviceSize == PlatformManager.DeviceSizeType.XL && (double)PlatformManager.ScreenAspectRatio > 1.2) || (PlatformManager.DeviceSize == PlatformManager.DeviceSizeType.L && (double)PlatformManager.ScreenAspectRatio > 1.5) || (PlatformManager.DeviceSize == PlatformManager.DeviceSizeType.M && (double)PlatformManager.ScreenAspectRatio > 1.8))
			{
				this.visibility = TopMenuStatsPresenter.Visibility.All;
			}
			else if ((PlatformManager.DeviceSize == PlatformManager.DeviceSizeType.XS && (double)PlatformManager.ScreenAspectRatio < 2.0) || (PlatformManager.DeviceSize == PlatformManager.DeviceSizeType.S && (double)PlatformManager.ScreenAspectRatio < 1.8) || (PlatformManager.DeviceSize == PlatformManager.DeviceSizeType.M && (double)PlatformManager.ScreenAspectRatio < 1.5))
			{
				this.visibility = TopMenuStatsPresenter.Visibility.Left;
				num = 10;
			}
			else
			{
				this.visibility = TopMenuStatsPresenter.Visibility.Left;
				num = 12;
			}
			if (this.visibility == TopMenuStatsPresenter.Visibility.All)
			{
				this.visibilityAll = new bool[17];
				for (int i = 0; i < 17; i++)
				{
					this.visibilityAll[i] = true;
				}
				return;
			}
			this.visibilityLeft = new bool[17];
			this.visibilityRight = new bool[17];
			for (int j = 0; j < 17; j++)
			{
				this.visibilityLeft[j] = j < num;
			}
			this.visibilityRight[0] = true;
			for (int k = 1; k < 17; k++)
			{
				this.visibilityRight[k] = k + num - 1 >= 17;
			}
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000E18C4 File Offset: 0x000DFAC4
		private void UpdateVisibility()
		{
			switch (this.visibility)
			{
			case TopMenuStatsPresenter.Visibility.All:
				foreach (TopMenuStatsEntry topMenuStatsEntry in this.topMenuStatsEntries)
				{
					topMenuStatsEntry.UpdateVisibility(this.visibilityAll);
				}
				this.topMenuStatsLegend.UpdateVisibility(this.visibilityAll);
				this.buttonsRow.SetActive(false);
				return;
			case TopMenuStatsPresenter.Visibility.Left:
				foreach (TopMenuStatsEntry topMenuStatsEntry2 in this.topMenuStatsEntries)
				{
					topMenuStatsEntry2.UpdateVisibility(this.visibilityLeft);
				}
				this.topMenuStatsLegend.UpdateVisibility(this.visibilityLeft);
				this.buttonsRow.SetActive(true);
				this.buttonLeft.interactable = false;
				this.buttonLeft.transform.GetChild(0).GetComponent<Image>().enabled = false;
				this.buttonRight.interactable = true;
				this.buttonRight.transform.GetChild(0).GetComponent<Image>().enabled = true;
				return;
			case TopMenuStatsPresenter.Visibility.Right:
				foreach (TopMenuStatsEntry topMenuStatsEntry3 in this.topMenuStatsEntries)
				{
					topMenuStatsEntry3.UpdateVisibility(this.visibilityRight);
				}
				this.topMenuStatsLegend.UpdateVisibility(this.visibilityRight);
				this.buttonsRow.SetActive(true);
				this.buttonLeft.interactable = true;
				this.buttonLeft.transform.GetChild(0).GetComponent<Image>().enabled = true;
				this.buttonRight.interactable = false;
				this.buttonRight.transform.GetChild(0).GetComponent<Image>().enabled = false;
				return;
			default:
				return;
			}
		}

		// Token: 0x04001AD3 RID: 6867
		[SerializeField]
		private List<TopMenuStatsEntry> topMenuStatsEntries;

		// Token: 0x04001AD4 RID: 6868
		[SerializeField]
		private TopMenuStatsLegend topMenuStatsLegend;

		// Token: 0x04001AD5 RID: 6869
		[SerializeField]
		private GameObject statsEntry;

		// Token: 0x04001AD6 RID: 6870
		[SerializeField]
		private Transform statsEntriesParent;

		// Token: 0x04001AD7 RID: 6871
		[SerializeField]
		private Color[] backgroundEntryColor = new Color[7];

		// Token: 0x04001AD8 RID: 6872
		[SerializeField]
		private GameObject buttonsRow;

		// Token: 0x04001AD9 RID: 6873
		[SerializeField]
		private Button buttonLeft;

		// Token: 0x04001ADA RID: 6874
		[SerializeField]
		private Button buttonRight;

		// Token: 0x04001ADB RID: 6875
		private const int CELLS_COUNT = 17;

		// Token: 0x04001ADC RID: 6876
		private TopMenuStatsPresenter.Visibility visibility;

		// Token: 0x04001ADD RID: 6877
		private bool[] visibilityAll;

		// Token: 0x04001ADE RID: 6878
		private bool[] visibilityLeft;

		// Token: 0x04001ADF RID: 6879
		private bool[] visibilityRight;

		// Token: 0x04001AE0 RID: 6880
		private bool isInitialized;

		// Token: 0x020004AC RID: 1196
		private enum Visibility
		{
			// Token: 0x04001AE2 RID: 6882
			All,
			// Token: 0x04001AE3 RID: 6883
			Left,
			// Token: 0x04001AE4 RID: 6884
			Right
		}
	}
}

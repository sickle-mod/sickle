using System;
using System.Linq;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x0200050E RID: 1294
	public class Tutorial02 : Tutorial
	{
		// Token: 0x0600296B RID: 10603 RVA: 0x00042F0D File Offset: 0x0004110D
		protected override void OnStart()
		{
			AnalyticsEventData.TutorialStart(StepIDs.tut3_00_introduction);
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x000EC878 File Offset: 0x000EAA78
		protected override void RegisterCustomCallbacks()
		{
			base.SetNextButton(3, this.hamburgerButton, HighlightController.HighlightStyle.Circle, 1);
			base.SetNextButton(4, this.statsTabButton, HighlightController.HighlightStyle.Circle, 1);
			base.SetShow(5, new Action(this.OnShow05));
			base.SetShow(6, new Action(this.OnShow06));
			base.SetHide(6, new Action(this.OnHide06));
			base.SetNextButton(7, GameController.Instance.matPlayer.matSection[1].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetEndTurn(8);
			base.SetNextButton(9, GameController.Instance.matPlayer.matSection[0].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			int num = 10;
			Unit[] array = new Unit[1];
			array[0] = GameController.GameManager.PlayerCurrent.GetAllUnits().First((Unit u) => u is Mech);
			base.SetUnitSelection(num, array);
			base.SetHexSelection(11, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[2, 3] });
			base.SetUnitSelection(12, new Unit[] { GameController.GameManager.PlayerCurrent.character });
			base.SetHexSelection(13, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[2, 3] });
			base.SetShow(14, new Action(this.OnShow14));
			base.SetNextButton(15, this.morePowerButton, 3);
			base.SetNextButton(16, this.combatCardButton, 2);
			base.SetNextButton(17, this.fightButton, HighlightController.HighlightStyle.Rectangle, 1);
			base.SetNextButton(18, this.closeCombatButton, HighlightController.HighlightStyle.Rectangle, 1);
			base.SetShow(20, new Action(this.OnShow20));
			base.SetHide(20, new Action(this.OnHide20));
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x00042F16 File Offset: 0x00041116
		private void OnShow05()
		{
			this.HighlightOpponentCombatStats();
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x00042F1E File Offset: 0x0004111E
		private void OnShow06()
		{
			this.HighlightPlayerCombatStats();
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x00042F26 File Offset: 0x00041126
		private void OnHide06()
		{
			SingletonMono<TopMenuPanelsManager>.Instance.Hide();
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x00042F32 File Offset: 0x00041132
		private void OnShow20()
		{
			SingletonMono<TopMenuPanelsManager>.Instance.ShowPanel(2);
			this.HighlightOpponentCombatCards();
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x00042F26 File Offset: 0x00041126
		private void OnHide20()
		{
			SingletonMono<TopMenuPanelsManager>.Instance.Hide();
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x00042F45 File Offset: 0x00041145
		private void OnShow14()
		{
			GameController.GameManager.combatManager.OnCombatStageChanged += this.OnCombatStageChanged14;
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x00042F62 File Offset: 0x00041162
		private void OnCombatStageChanged14(CombatStage stage)
		{
			if (stage == CombatStage.Preparation)
			{
				GameController.GameManager.combatManager.OnCombatStageChanged -= this.OnCombatStageChanged14;
				base.Next();
			}
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x000ECA68 File Offset: 0x000EAC68
		private void HighlightOpponentCombatStats()
		{
			TopMenuStatsEntry topMenuStatsEntry = SingletonMono<TopMenuPanelsManager>.Instance.GetComponentsInChildren<TopMenuStatsEntry>().Last<TopMenuStatsEntry>();
			SingletonMono<HighlightController>.Instance.HighlightUI(topMenuStatsEntry.TextPower, HighlightController.HighlightStyle.Circle);
			SingletonMono<HighlightController>.Instance.HighlightUI(topMenuStatsEntry.TextCombatCards, HighlightController.HighlightStyle.Circle);
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x000ECAA8 File Offset: 0x000EACA8
		private void HighlightPlayerCombatStats()
		{
			TopMenuStatsEntry topMenuStatsEntry = SingletonMono<TopMenuPanelsManager>.Instance.GetComponentsInChildren<TopMenuStatsEntry>().First<TopMenuStatsEntry>();
			SingletonMono<HighlightController>.Instance.HighlightUI(topMenuStatsEntry.TextPower, HighlightController.HighlightStyle.Circle);
			SingletonMono<HighlightController>.Instance.HighlightUI(topMenuStatsEntry.TextCombatCards, HighlightController.HighlightStyle.Circle);
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x000ECAE8 File Offset: 0x000EACE8
		private void HighlightOpponentCombatCards()
		{
			TopMenuStatsEntry topMenuStatsEntry = SingletonMono<TopMenuPanelsManager>.Instance.GetComponentsInChildren<TopMenuStatsEntry>().Last<TopMenuStatsEntry>();
			SingletonMono<HighlightController>.Instance.HighlightUI(topMenuStatsEntry.TextCombatCards, HighlightController.HighlightStyle.Circle);
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x00042F89 File Offset: 0x00041189
		protected override bool SkipCurrentState(StepIDs step)
		{
			return base.IsDesktopBuild() && step == StepIDs.tut3_02_b_open_left_top_menu;
		}

		// Token: 0x04001D88 RID: 7560
		[SerializeField]
		private Button hamburgerButton;

		// Token: 0x04001D89 RID: 7561
		[SerializeField]
		private Button statsTabButton;

		// Token: 0x04001D8A RID: 7562
		[SerializeField]
		private Button morePowerButton;

		// Token: 0x04001D8B RID: 7563
		[SerializeField]
		private Button combatCardButton;

		// Token: 0x04001D8C RID: 7564
		[SerializeField]
		private Button fightButton;

		// Token: 0x04001D8D RID: 7565
		[SerializeField]
		private Button closeCombatButton;
	}
}

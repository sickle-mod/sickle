using System;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x02000517 RID: 1303
	public class Tutorial08 : Tutorial
	{
		// Token: 0x0600299F RID: 10655 RVA: 0x0004319E File Offset: 0x0004139E
		protected override void OnStart()
		{
			AnalyticsEventData.TutorialStart(StepIDs.tut9_00_introduction);
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x000ED3C8 File Offset: 0x000EB5C8
		protected override void RegisterCustomCallbacks()
		{
			base.SetNextButton(2, GameController.Instance.matPlayer.matSection[3].downActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetShow(3, new Action(this.OnShow03));
			base.SetHide(3, new Action(this.OnHide03));
			base.SetHexSelection(4, 3, new GameHex[] { GameController.GameManager.gameBoard.hexMap[0, 4] });
			base.SetNextButton(5, this.chooseRecruitButton, HighlightController.HighlightStyle.Rectangle, 1);
			base.SetNextButton(6, this.chooseBonusButton, HighlightController.HighlightStyle.Rectangle, 1);
			base.SetShow(6, new Action(this.OnShow06));
			base.SetNextButton(7, this.confirmEnlistButton, HighlightController.HighlightStyle.Rectangle, 1);
			base.SetShow(7, new Action(this.OnShow06));
			base.SetShow(8, new Action(this.OnShow08));
			base.SetShow(9, new Action(this.OnShow09));
			base.SetHide(9, new Action(this.OnHide09));
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000431AA File Offset: 0x000413AA
		private void OnShow03()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			SingletonMono<TopMenuPanelsManager>.Instance.ShowPanel(2);
			this.HighlightOpponentCombatCards();
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x00042F26 File Offset: 0x00041126
		private void OnHide03()
		{
			SingletonMono<TopMenuPanelsManager>.Instance.Hide();
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x000ED4DC File Offset: 0x000EB6DC
		private void OnShow06()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			foreach (RectTransform rectTransform in this.peekHighlightAreas)
			{
				SingletonMono<HighlightController>.Instance.HighlightUI(rectTransform, HighlightController.HighlightStyle.Circle);
			}
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x000ED4DC File Offset: 0x000EB6DC
		private void OnShow07()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			foreach (RectTransform rectTransform in this.peekHighlightAreas)
			{
				SingletonMono<HighlightController>.Instance.HighlightUI(rectTransform, HighlightController.HighlightStyle.Circle);
			}
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x000431AA File Offset: 0x000413AA
		private void OnShow08()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			SingletonMono<TopMenuPanelsManager>.Instance.ShowPanel(2);
			this.HighlightOpponentCombatCards();
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x000431C4 File Offset: 0x000413C4
		private void OnShow09()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			this.HighlightOpponentCombatCards();
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x00042F26 File Offset: 0x00041126
		private void OnHide09()
		{
			SingletonMono<TopMenuPanelsManager>.Instance.Hide();
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x000ED518 File Offset: 0x000EB718
		private void HighlightOpponentCombatCards()
		{
			TopMenuStatsEntry topMenuStatsEntry = SingletonMono<TopMenuPanelsManager>.Instance.GetComponentsInChildren<TopMenuStatsEntry>()[1];
			SingletonMono<HighlightController>.Instance.HighlightUI(topMenuStatsEntry.TextCombatCards, HighlightController.HighlightStyle.Circle);
			TopMenuStatsEntry topMenuStatsEntry2 = SingletonMono<TopMenuPanelsManager>.Instance.GetComponentsInChildren<TopMenuStatsEntry>()[2];
			SingletonMono<HighlightController>.Instance.HighlightUI(topMenuStatsEntry2.TextCombatCards, HighlightController.HighlightStyle.Circle);
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x000431D3 File Offset: 0x000413D3
		protected override bool SkipCurrentState(StepIDs step)
		{
			if (base.IsDesktopBuild())
			{
				if (step == StepIDs.tut9_00_b_enlist_info)
				{
					return true;
				}
				if (step == StepIDs.tut9_05_b_enlist_accept)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001D9E RID: 7582
		[SerializeField]
		private Button chooseRecruitButton;

		// Token: 0x04001D9F RID: 7583
		[SerializeField]
		private Button chooseBonusButton;

		// Token: 0x04001DA0 RID: 7584
		[SerializeField]
		private RectTransform[] peekHighlightAreas;

		// Token: 0x04001DA1 RID: 7585
		[SerializeField]
		private Button confirmEnlistButton;
	}
}

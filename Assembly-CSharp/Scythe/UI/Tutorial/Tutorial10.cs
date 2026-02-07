using System;
using System.Linq;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x0200051A RID: 1306
	public class Tutorial10 : Tutorial
	{
		// Token: 0x060029B8 RID: 10680 RVA: 0x00043290 File Offset: 0x00041490
		protected override void OnStart()
		{
			AnalyticsEventData.TutorialStart(StepIDs.tut11_00_introduction);
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x000ED700 File Offset: 0x000EB900
		protected override void RegisterCustomCallbacks()
		{
			base.SetShow(0, new Action(this.OnShow00));
			base.SetNextButton(1, this.structureTabButton, HighlightController.HighlightStyle.Circle, 1);
			base.SetNextButton(3, this.scoreTabButton, HighlightController.HighlightStyle.Circle, 1);
			base.SetShow(4, new Action(this.OnShow04));
			base.SetShow(5, new Action(this.OnShow05));
			base.SetHide(5, new Action(this.OnHide05));
			base.SetShow(6, new Action(this.OnShow06));
			base.SetHide(6, new Action(this.OnHide06));
			base.SetNextButton(7, GameController.Instance.matPlayer.matSection[0].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			int num = 8;
			Unit[] array = new Unit[1];
			array[0] = GameController.GameManager.PlayerCurrent.GetAllUnits().Last((Unit u) => u is Worker);
			base.SetUnitSelection(num, array);
			base.SetHexSelection(9, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[3, 4] });
			int num2 = 10;
			Unit[] array2 = new Unit[1];
			array2[0] = GameController.GameManager.PlayerCurrent.GetAllUnits().First((Unit u) => u is Worker);
			base.SetUnitSelection(num2, array2);
			base.SetHexSelection(11, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[2, 3] });
			base.SetNextButton(12, GameController.Instance.matPlayer.matSection[2].downActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetHexSelection(13, 3, new GameHex[] { GameController.GameManager.gameBoard.hexMap[4, 1] });
			base.SetNextButton(14, this.monumentButton, HighlightController.HighlightStyle.Rectangle, 1);
			base.SetHexSelection(15, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[2, 3] });
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x0004329C File Offset: 0x0004149C
		private void OnShow00()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			SingletonMono<TopMenuPanelsManager>.Instance.ShowPanel(0);
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x000432B0 File Offset: 0x000414B0
		private void OnShow04()
		{
			this.showDetailsToggle.isOn = true;
			SingletonMono<HighlightController>.Instance.HighlightUI(this.peekHighlightAreas[0], HighlightController.HighlightStyle.Rectangle);
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x000432D1 File Offset: 0x000414D1
		private void OnShow05()
		{
			SingletonMono<HighlightController>.Instance.HighlightUI(this.peekHighlightAreas[1], HighlightController.HighlightStyle.Rectangle);
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x00042DF6 File Offset: 0x00040FF6
		private void OnHide05()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			SingletonMono<TopMenuPanelsManager>.Instance.Hide();
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x000432E6 File Offset: 0x000414E6
		private void OnShow06()
		{
			this.movementPlan.SetActive(true);
			SingletonMono<CameraAnimationController>.Instance.AnimateToAllHexes(new GameHex[] { GameController.GameManager.gameBoard.hexMap[3, 4] });
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x0004331D File Offset: 0x0004151D
		private void OnHide06()
		{
			this.movementPlan.SetActive(false);
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x0004332B File Offset: 0x0004152B
		protected override void OnScreenChanged(int currentScreenId)
		{
			if (currentScreenId == 7)
			{
				base.FreezeAnalyticsStep();
			}
			if (currentScreenId == 8)
			{
				base.UnfreezeAnalyticsStep();
			}
			if (currentScreenId == 9)
			{
				base.FreezeAnalyticsStep();
			}
			if (currentScreenId == 10)
			{
				base.UnfreezeAnalyticsStep();
			}
			if (currentScreenId == 15)
			{
				base.AnalyticsStepLogging(StepStatuses.completed, 0);
			}
		}

		// Token: 0x04001DA9 RID: 7593
		[SerializeField]
		private Button structureTabButton;

		// Token: 0x04001DAA RID: 7594
		[SerializeField]
		private RectTransform[] peekHighlightAreas;

		// Token: 0x04001DAB RID: 7595
		[SerializeField]
		private Toggle showDetailsToggle;

		// Token: 0x04001DAC RID: 7596
		[SerializeField]
		private GameObject movementPlan;

		// Token: 0x04001DAD RID: 7597
		[SerializeField]
		private Button scoreTabButton;

		// Token: 0x04001DAE RID: 7598
		[SerializeField]
		private Button monumentButton;
	}
}

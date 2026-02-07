using System;
using System.Linq;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x02000518 RID: 1304
	public class Tutorial09 : Tutorial
	{
		// Token: 0x060029AB RID: 10667 RVA: 0x000431F2 File Offset: 0x000413F2
		protected override void OnStart()
		{
			AnalyticsEventData.TutorialStart(StepIDs.tut10_00_introduction);
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x000ED564 File Offset: 0x000EB764
		protected override void RegisterCustomCallbacks()
		{
			base.SetShow(1, new Action(this.OnShow01));
			base.SetShow(2, new Action(this.OnShow02));
			base.SetNextButton(3, this.factionTabButton, HighlightController.HighlightStyle.Circle, 1);
			base.SetHide(4, new Action(this.OnHide04));
			base.SetNextButton(5, GameController.Instance.matPlayer.matSection[0].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetShow(5, new Action(this.OnShow05));
			base.SetHide(5, new Action(this.OnHide05));
			int num = 6;
			Unit[] array = new Unit[1];
			array[0] = GameController.GameManager.PlayerCurrent.GetAllUnits().First((Unit u) => u is Worker);
			base.SetUnitSelection(num, array);
			base.SetHexSelection(7, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[6, 1] });
			int num2 = 8;
			Unit[] array2 = new Unit[1];
			array2[0] = GameController.GameManager.PlayerCurrent.GetAllUnits().Last((Unit u) => u is Worker);
			base.SetUnitSelection(num2, array2);
			base.SetHexSelection(9, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[3, 1] });
			base.SetNextButton(10, this.objectivePreviewButton, HighlightController.HighlightStyle.Circle, 1);
			base.SetNextButton(11, this.completeObjectiveButton, HighlightController.HighlightStyle.Rectangle, 1);
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x000431FE File Offset: 0x000413FE
		private void OnShow01()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			SingletonMono<HighlightController>.Instance.HighlightUI(this.objectivePreviewButton, HighlightController.HighlightStyle.Circle);
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x00043218 File Offset: 0x00041418
		private void OnShow02()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			SingletonMono<TopMenuPanelsManager>.Instance.ShowPanel(5);
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x00042F26 File Offset: 0x00041126
		private void OnHide04()
		{
			SingletonMono<TopMenuPanelsManager>.Instance.Hide();
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x0004322C File Offset: 0x0004142C
		private void OnShow05()
		{
			this.movementPlan.SetActive(true);
			SingletonMono<CameraAnimationController>.Instance.AnimateToAllHexes(new GameHex[] { GameController.GameManager.gameBoard.hexMap[4, 1] });
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x00043263 File Offset: 0x00041463
		private void OnHide05()
		{
			this.movementPlan.SetActive(false);
			base.FreezeAnalyticsStep();
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x00043277 File Offset: 0x00041477
		protected override void OnScreenChanged(int currentScreenId)
		{
			if (currentScreenId == 9)
			{
				base.UnfreezeAnalyticsStep();
			}
		}

		// Token: 0x04001DA2 RID: 7586
		[SerializeField]
		private Button factionTabButton;

		// Token: 0x04001DA3 RID: 7587
		[SerializeField]
		private GameObject movementPlan;

		// Token: 0x04001DA4 RID: 7588
		[SerializeField]
		private Button objectivePreviewButton;

		// Token: 0x04001DA5 RID: 7589
		[SerializeField]
		private Button completeObjectiveButton;
	}
}

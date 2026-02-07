using System;
using System.Linq;
using Scythe.Analytics;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x02000514 RID: 1300
	public class Tutorial06 : Tutorial
	{
		// Token: 0x06002991 RID: 10641 RVA: 0x000430B3 File Offset: 0x000412B3
		protected override void OnStart()
		{
			AnalyticsEventData.TutorialStart(StepIDs.tut7_00_introduction);
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x000ED07C File Offset: 0x000EB27C
		protected override void RegisterCustomCallbacks()
		{
			base.SetNextButton(2, GameController.Instance.matPlayer.matSection[1].downActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetHexSelection(3, 2, new GameHex[] { GameController.GameManager.gameBoard.hexMap[1, 6] });
			base.SetHexSelection(4, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[0, 6] });
			base.SetNextButton(5, this.mechSelectionButton, HighlightController.HighlightStyle.Rectangle, 1);
			base.SetHexSelection(6, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[0, 6] });
			base.SetEndTurn(7);
			base.SetNextButton(8, GameController.Instance.matPlayer.matSection[0].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetUnitSelection(9, () => (from u in GameController.GameManager.PlayerCurrent.GetAllUnits()
				where u is Mech
				select u).ToArray<Unit>());
			base.SetHexSelection(10, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[0, 5] });
			base.SetUnitSelection(11, new Unit[] { GameController.GameManager.PlayerCurrent.character });
			base.SetNextButton(12, this.endMoveButton, HighlightController.HighlightStyle.Rectangle, 1);
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x000430BC File Offset: 0x000412BC
		protected override bool SkipCurrentState(StepIDs step)
		{
			if (base.IsDesktopBuild())
			{
				if (step == StepIDs.tut7_00_b_deploy_info)
				{
					return true;
				}
				if (step == StepIDs.tut7_02_b_deploy_pay_resources)
				{
					return true;
				}
				if (step == StepIDs.tut7_11_b_end_turn)
				{
					return true;
				}
			}
			if (!base.IsDesktopBuild())
			{
				if (step == StepIDs.tut7_09_ability_info_mechs)
				{
					return true;
				}
				if (step == StepIDs.tut7_11_wait_step)
				{
					return true;
				}
				if (step == StepIDs.tut7_12_ability_info_workers)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001D97 RID: 7575
		[SerializeField]
		private Button mechSelectionButton;

		// Token: 0x04001D98 RID: 7576
		[SerializeField]
		private Button endMoveButton;
	}
}

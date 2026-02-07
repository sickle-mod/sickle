using System;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x02000516 RID: 1302
	public class Tutorial07 : Tutorial
	{
		// Token: 0x06002999 RID: 10649 RVA: 0x0004313F File Offset: 0x0004133F
		protected override void OnStart()
		{
			AnalyticsEventData.TutorialStart(StepIDs.tut8_00_introduction);
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x000ED1E8 File Offset: 0x000EB3E8
		protected override void RegisterCustomCallbacks()
		{
			base.SetNextButton(3, GameController.Instance.matPlayer.matSection[0].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetUnitSelection(4, new Unit[] { GameController.GameManager.PlayerCurrent.character });
			base.SetHexSelection(5, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[3, 6] });
			base.SetNextButton(6, this.endMoveButton, HighlightController.HighlightStyle.Rectangle, 1);
			base.SetEndTurn(7);
			base.SetNextButton(8, GameController.Instance.matPlayer.matSection[2].topActionPresenter.gainActionButton[1], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetShow(9, new Action(this.OnShow09));
			base.SetNextButton(10, GameController.Instance.matPlayer.matSection[2].downActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetHexSelection(11, 4, new GameHex[] { GameController.GameManager.gameBoard.hexMap[3, 6] });
			base.SetNextButton(12, this.millButton, HighlightController.HighlightStyle.Rectangle, 1);
			base.SetHexSelection(13, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[1, 6] });
			base.SetEndTurn(14);
			base.SetNextButton(15, GameController.Instance.matPlayer.matSection[3].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetHexSelection(17, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[0, 6] });
			base.SetHexSelection(18, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[1, 6] });
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x00043148 File Offset: 0x00041348
		private void OnShow09()
		{
			SingletonMono<HighlightController>.Instance.HighlightUI(GameController.Instance.matPlayer.matSection[2].topActionPresenter.buildingBackground, HighlightController.HighlightStyle.Rectangle);
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x00043174 File Offset: 0x00041374
		protected override void OnScreenChanged(int currentScreenId)
		{
			if (currentScreenId == 17)
			{
				base.FreezeAnalyticsStep();
			}
			if (currentScreenId == 18)
			{
				base.UnfreezeAnalyticsStep();
			}
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x0004318C File Offset: 0x0004138C
		protected override bool SkipCurrentState(StepIDs step)
		{
			return base.IsDesktopBuild() && step == StepIDs.tut8_00_b_build_info;
		}

		// Token: 0x04001D9C RID: 7580
		[SerializeField]
		private Button endMoveButton;

		// Token: 0x04001D9D RID: 7581
		[SerializeField]
		private Button millButton;
	}
}

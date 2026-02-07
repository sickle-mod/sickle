using System;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Utilities;

namespace Scythe.UI.Tutorial
{
	// Token: 0x02000513 RID: 1299
	public class Tutorial05 : Tutorial
	{
		// Token: 0x0600298C RID: 10636 RVA: 0x00043098 File Offset: 0x00041298
		protected override void OnStart()
		{
			AnalyticsEventData.TutorialStart(StepIDs.tut6_00_introduction);
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x000ECF4C File Offset: 0x000EB14C
		protected override void RegisterCustomCallbacks()
		{
			base.SetNextButton(2, GameController.Instance.matPlayer.matSection[0].downActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetHexSelection(3, 3, new GameHex[] { GameController.GameManager.gameBoard.hexMap[1, 7] });
			base.SetNextButton(4, GameController.Instance.matPlayer.matSection[0].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetNextButton(5, GameController.Instance.matPlayer.matSection[1].downActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetShow(6, new Action(this.OnShow06));
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x000ED014 File Offset: 0x000EB214
		private void OnShow06()
		{
			SingletonMono<HighlightController>.Instance.HighlightUI(GameController.Instance.matPlayer.matSection[2].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle);
			SingletonMono<HighlightController>.Instance.HighlightUI(GameController.Instance.matPlayer.matSection[1].downActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle);
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x000430A1 File Offset: 0x000412A1
		protected override bool SkipCurrentState(StepIDs step)
		{
			return base.IsDesktopBuild() && step == StepIDs.tut6_00_b_upgrade_info;
		}
	}
}

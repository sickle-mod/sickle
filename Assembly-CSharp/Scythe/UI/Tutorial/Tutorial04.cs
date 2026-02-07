using System;
using Scythe.Analytics;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x02000512 RID: 1298
	public class Tutorial04 : Tutorial
	{
		// Token: 0x06002989 RID: 10633 RVA: 0x0004308F File Offset: 0x0004128F
		protected override void OnStart()
		{
			AnalyticsEventData.TutorialStart(StepIDs.tut5_00_introduction);
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x000ECE00 File Offset: 0x000EB000
		protected override void RegisterCustomCallbacks()
		{
			base.SetNextButton(3, GameController.Instance.matPlayer.matSection[3].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetHexSelection(4, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[4, 2] });
			base.SetHexSelection(6, 1, new GameHex[]
			{
				GameController.GameManager.gameBoard.hexMap[4, 1],
				GameController.GameManager.gameBoard.hexMap[5, 1]
			});
			base.SetEndTurn(7);
			base.SetNextButton(8, GameController.Instance.matPlayer.matSection[2].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetHexSelection(9, 1, new GameHex[]
			{
				GameController.GameManager.gameBoard.hexMap[4, 1],
				GameController.GameManager.gameBoard.hexMap[5, 1],
				GameController.GameManager.gameBoard.hexMap[4, 2]
			});
			base.SetNextButton(10, this.tradeFoodButton, HighlightController.HighlightStyle.Rectangle, 1);
			base.SetNextButton(11, this.tradeWoodButton, HighlightController.HighlightStyle.Rectangle, 1);
		}

		// Token: 0x04001D95 RID: 7573
		[SerializeField]
		private Button tradeFoodButton;

		// Token: 0x04001D96 RID: 7574
		[SerializeField]
		private Button tradeWoodButton;
	}
}

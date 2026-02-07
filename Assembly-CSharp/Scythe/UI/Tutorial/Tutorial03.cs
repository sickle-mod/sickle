using System;
using System.Linq;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x02000510 RID: 1296
	public class Tutorial03 : Tutorial
	{
		// Token: 0x0600297C RID: 10620 RVA: 0x00042FA7 File Offset: 0x000411A7
		protected override void OnStart()
		{
			AnalyticsEventData.TutorialStart(StepIDs.tut4_00_introduction);
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x000ECB18 File Offset: 0x000EAD18
		protected override void RegisterCustomCallbacks()
		{
			base.SetShow(0, new Action(this.OnShow00));
			base.SetNextButton(2, GameController.Instance.matPlayer.matSection[0].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetUnitSelection(3, new Unit[] { GameController.GameManager.PlayerCurrent.character });
			base.SetHexSelection(4, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[3, 4] });
			base.SetShow(5, new Action(this.OnShow05));
			base.SetHide(5, new Action(this.OnHide05));
			base.SetShow(6, new Action(this.OnShow06));
			base.SetHide(6, new Action(this.OnHide06));
			base.SetShow(7, new Action(this.OnShow07));
			base.SetEndTurn(8);
			base.SetNextButton(9, GameController.Instance.matPlayer.factoryCardSlot.GetComponentInChildren<PlayerTopActionPresenter>(true).gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetShow(10, new Action(this.OnShow10));
			base.SetNextButton(11, GameController.Instance.matPlayer.factoryCardSlot.GetComponentInChildren<PlayerDownActionPresenter>(true).gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetUnitSelection(12, new Unit[] { GameController.GameManager.PlayerCurrent.character });
			base.SetHexPresentation(13, new GameHex[] { GameController.GameManager.gameBoard.hexMap[2, 6] });
			base.SetHexSelection(15, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[2, 6] });
			base.SetNextButton(16, this.revealEncounterButton, HighlightController.HighlightStyle.Rectangle, 1);
			base.SetNextButton(18, GameController.Instance.encounterCardPresenter.optionButtons[0], HighlightController.HighlightStyle.Rectangle, 1);
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x00042FB0 File Offset: 0x000411B0
		private void OnShow00()
		{
			ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.factory).GetWorldPosition());
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x000ECD08 File Offset: 0x000EAF08
		private void OnShow05()
		{
			SingletonMono<HighlightController>.Instance.HighlightUI(this.endMoveButton, HighlightController.HighlightStyle.Rectangle);
			SingletonMono<InputBlockerController>.Instance.UnblockUI(this.endMoveButton);
			SingletonMono<InputBlockerController>.Instance.UnblockUnits((from u in GameController.GameManager.PlayerCurrent.GetAllUnits()
				where !(u is Character)
				select u).ToArray<Unit>());
			SingletonMono<InputBlockerController>.Instance.UnblockHexes(new GameHex[]
			{
				GameController.GameManager.gameBoard.hexMap[3, 1],
				GameController.GameManager.gameBoard.hexMap[3, 2],
				GameController.GameManager.gameBoard.hexMap[4, 2],
				GameController.GameManager.gameBoard.hexMap[5, 1]
			});
			MovePresenter.MoveEnded += base.Next;
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x00042FDF File Offset: 0x000411DF
		private void OnHide05()
		{
			MovePresenter.MoveEnded -= base.Next;
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x00042FF2 File Offset: 0x000411F2
		private void OnShow06()
		{
			SingletonMono<InputBlockerController>.Instance.UnblockUI(GameController.Instance.factoryCardPresenter);
			GameController.Instance.factoryCardPresenter.OnFactoryCardChoosen += base.Next;
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x00043023 File Offset: 0x00041223
		private void OnHide06()
		{
			GameController.Instance.factoryCardPresenter.OnFactoryCardChoosen -= base.Next;
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x00043040 File Offset: 0x00041240
		private void OnShow07()
		{
			SingletonMono<HighlightController>.Instance.HighlightUI(GameController.Instance.matPlayer.matSection[4], HighlightController.HighlightStyle.Rectangle);
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x00043062 File Offset: 0x00041262
		private void OnShow10()
		{
			SingletonMono<HighlightController>.Instance.HighlightUI(this.pointsPanel, HighlightController.HighlightStyle.Rectangle);
		}

		// Token: 0x04001D90 RID: 7568
		[SerializeField]
		private Button endMoveButton;

		// Token: 0x04001D91 RID: 7569
		[SerializeField]
		private RectTransform pointsPanel;

		// Token: 0x04001D92 RID: 7570
		[SerializeField]
		private Button revealEncounterButton;
	}
}

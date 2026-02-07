using System;
using System.Linq;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x0200050C RID: 1292
	public class Tutorial01 : Tutorial
	{
		// Token: 0x06002961 RID: 10593 RVA: 0x00042E8E File Offset: 0x0004108E
		protected override void OnStart()
		{
			AnalyticsEventData.TutorialStart(StepIDs.tut2_00_introduction);
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x000EC68C File Offset: 0x000EA88C
		protected override void RegisterCustomCallbacks()
		{
			base.SetHexPresentation(2, GameController.GameManager.gameBoard.tunnels.ToArray());
			base.SetHexPresentation(3, new GameHex[]
			{
				GameController.GameManager.gameBoard.hexMap[1, 4],
				GameController.GameManager.gameBoard.hexMap[2, 5]
			});
			base.SetHexPresentation(4, new GameHex[] { GameController.GameManager.gameBoard.bases[Faction.Polania] });
			base.SetHexPresentation(5, new GameHex[] { GameController.GameManager.gameBoard.hexMap[2, 5] });
			base.SetShow(6, new Action(this.OnShow06));
			base.SetHide(6, new Action(this.OnHide06));
			base.SetNextButton(7, GameController.Instance.matPlayer.matSection[0].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetShow(8, new Action(this.OnShow08));
			int num = 9;
			Unit[] array = new Unit[1];
			array[0] = GameController.GameManager.PlayerCurrent.GetAllUnits().First((Unit u) => u is Mech);
			base.SetUnitSelection(num, array);
			base.SetNextButton(10, this.takeAllButton, HighlightController.HighlightStyle.Rectangle, 1);
			base.SetHexSelection(11, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[0, 4] });
			base.SetUnitSelection(12, new Unit[] { GameController.GameManager.PlayerCurrent.character });
			base.SetNextButton(13, this.takeAllButton, HighlightController.HighlightStyle.Rectangle, 1);
			base.SetHexSelection(14, 1, new GameHex[] { GameController.GameManager.gameBoard.hexMap[2, 3] });
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x00042E97 File Offset: 0x00041097
		private void OnShow06()
		{
			this.movementPlan.SetActive(true);
			SingletonMono<CameraAnimationController>.Instance.AnimateToAllHexes(new GameHex[] { GameController.GameManager.gameBoard.hexMap[1, 4] });
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x00042ECE File Offset: 0x000410CE
		private void OnHide06()
		{
			this.movementPlan.SetActive(false);
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x00042EDC File Offset: 0x000410DC
		private void OnShow08()
		{
			SingletonMono<HighlightController>.Instance.HighlightUI(this.matPreviewButton, HighlightController.HighlightStyle.CircleBig);
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x00042EEF File Offset: 0x000410EF
		protected override bool SkipCurrentState(StepIDs step)
		{
			return base.IsDesktopBuild() && step == StepIDs.tut2_07_b_player_mat_hint;
		}

		// Token: 0x04001D83 RID: 7555
		[SerializeField]
		private GameObject movementPlan;

		// Token: 0x04001D84 RID: 7556
		[SerializeField]
		private RectTransform matPreviewButton;

		// Token: 0x04001D85 RID: 7557
		[SerializeField]
		private Button takeAllButton;
	}
}

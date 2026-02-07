using System;
using HoneyFramework;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;

namespace Scythe.BoardPresenter
{
	// Token: 0x020001DB RID: 475
	public abstract class GameBoardPresenter : MonoBehaviour
	{
		// Token: 0x06000DE4 RID: 3556
		public abstract void Initialize();

		// Token: 0x06000DE5 RID: 3557
		public abstract void InitializeFromSave();

		// Token: 0x06000DE6 RID: 3558 RVA: 0x000884E4 File Offset: 0x000866E4
		protected void LinkUnits()
		{
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				GameController.factionUnits[player.matFaction.faction].LinkUnits();
			}
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0003125E File Offset: 0x0002F45E
		public static float GetWorldHeightAt(Vector3 world3Dposition)
		{
			return 0f;
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00031265 File Offset: 0x0002F465
		public void EnableMapBlackout()
		{
			if (this.mapBlackout != null)
			{
				this.mapBlackout.Activate();
			}
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00031280 File Offset: 0x0002F480
		public void DisableMapBlackout()
		{
			if (this.mapBlackout != null)
			{
				this.mapBlackout.Deactivate();
			}
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0003129B File Offset: 0x0002F49B
		public void RevertResourcesLabels(GameHexPresenter hex)
		{
			if (this.mapBlackout != null)
			{
				this.mapBlackout.ShowResourcesLabels(hex);
			}
		}

		// Token: 0x06000DEB RID: 3563
		public abstract GameHexPresenter GetGameHexPresenter(int posX, int posY);

		// Token: 0x06000DEC RID: 3564
		public abstract GameHexPresenter GetGameHexPresenter(GameHex hex);

		// Token: 0x06000DED RID: 3565 RVA: 0x000312B7 File Offset: 0x0002F4B7
		public void UpdateBoard(bool updateUnits = true, bool updateStaticObjects = true)
		{
			if (updateStaticObjects)
			{
				this.UpdateStaticObjects();
			}
			this.UpdateHexOwnerships();
			this.UpdateUnits(updateUnits);
		}

		// Token: 0x06000DEE RID: 3566
		public abstract void UpdateStaticObjects();

		// Token: 0x06000DEF RID: 3567
		public abstract void UpdateHexOwnerships();

		// Token: 0x06000DF0 RID: 3568
		public abstract void UpdateUnits(bool updateUnits = true);

		// Token: 0x06000DF1 RID: 3569
		public abstract void UpdateTokens();

		// Token: 0x04000AF0 RID: 2800
		public GameBoardPresenter.Status status;

		// Token: 0x04000AF1 RID: 2801
		public GameHexPresenterObjectsPositions positionsTemplate;

		// Token: 0x04000AF2 RID: 2802
		[SerializeField]
		private MapBlackout mapBlackout;

		// Token: 0x020001DC RID: 476
		public enum Status
		{
			// Token: 0x04000AF4 RID: 2804
			NotReady,
			// Token: 0x04000AF5 RID: 2805
			Preparation,
			// Token: 0x04000AF6 RID: 2806
			TerrainGeneration,
			// Token: 0x04000AF7 RID: 2807
			Finishing,
			// Token: 0x04000AF8 RID: 2808
			RecalculatingNormals,
			// Token: 0x04000AF9 RID: 2809
			Foreground,
			// Token: 0x04000AFA RID: 2810
			PlacingScytheObjects,
			// Token: 0x04000AFB RID: 2811
			Ready
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;

namespace Scythe.UI.Tutorial
{
	// Token: 0x02000507 RID: 1287
	public class InputBlockerController : SingletonMono<InputBlockerController>
	{
		// Token: 0x06002914 RID: 10516 RVA: 0x00042B09 File Offset: 0x00040D09
		public void RegisterCanvas(InputBlockerCanvas canvas)
		{
			this.canvases.Add(canvas);
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x00042B17 File Offset: 0x00040D17
		public void Initialize()
		{
			this.BlockUI();
			this.BlockUnits();
			this.BlockHexes();
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x00042B2B File Offset: 0x00040D2B
		public void UnblockUI(GameObject unblockedGameObject)
		{
			this.UnblockUI(unblockedGameObject.transform as RectTransform);
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x00042B3E File Offset: 0x00040D3E
		public void UnblockUI(Component unblockedComponent)
		{
			this.UnblockUI(unblockedComponent.transform as RectTransform);
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x000EBDD0 File Offset: 0x000E9FD0
		public void UnblockAllUI()
		{
			foreach (InputBlockerCanvas inputBlockerCanvas in this.canvases)
			{
				this.UnblockCanvas(inputBlockerCanvas);
			}
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x000EBE24 File Offset: 0x000EA024
		public void UnblockUnits(params Unit[] unblockedUnits)
		{
			foreach (Unit unit in GameController.GameManager.PlayerCurrent.GetAllUnits())
			{
				UnitPresenter unitPresenter = GameController.GetUnitPresenter(unit);
				if (unitPresenter != null)
				{
					unitPresenter.SetColliderEnabled(unblockedUnits != null && unblockedUnits.Contains(unit));
				}
			}
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x000EBE9C File Offset: 0x000EA09C
		public void UnblockHexes(params GameHex[] unblockedHexes)
		{
			GameHex[,] hexMap = GameController.GameManager.gameBoard.hexMap;
			int upperBound = hexMap.GetUpperBound(0);
			int upperBound2 = hexMap.GetUpperBound(1);
			for (int i = hexMap.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = hexMap.GetLowerBound(1); j <= upperBound2; j++)
				{
					GameHex gameHex = hexMap[i, j];
					FlatGameHexPresenter flatGameHexPresenter = GameController.Instance.GetGameHexPresenter(gameHex) as FlatGameHexPresenter;
					if (flatGameHexPresenter != null)
					{
						flatGameHexPresenter.SetColliderEnabled(unblockedHexes != null && unblockedHexes.Contains(gameHex));
					}
				}
			}
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x00042B51 File Offset: 0x00040D51
		public void Reset()
		{
			this.ResetUIUnblocks();
			this.ResetUnitUnblocks();
			this.ResetHexUnblocks();
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x000EBF28 File Offset: 0x000EA128
		private void UnblockUI(RectTransform unblockedRectTransform)
		{
			CanvasGroup canvasGroup = unblockedRectTransform.gameObject.GetComponent<CanvasGroup>();
			if (canvasGroup == null)
			{
				canvasGroup = unblockedRectTransform.gameObject.AddComponent<CanvasGroup>();
			}
			canvasGroup.blocksRaycasts = true;
			canvasGroup.ignoreParentGroups = true;
			this.unblockedUI.Add(canvasGroup);
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x000EBF70 File Offset: 0x000EA170
		private void BlockUI()
		{
			foreach (InputBlockerCanvas inputBlockerCanvas in this.canvases)
			{
				this.BlockCanvas(inputBlockerCanvas);
			}
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x00042B65 File Offset: 0x00040D65
		private void BlockUnits()
		{
			this.UnblockUnits(null);
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x00042B6E File Offset: 0x00040D6E
		private void BlockHexes()
		{
			this.UnblockHexes(null);
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x00042B77 File Offset: 0x00040D77
		private void BlockCanvas(InputBlockerCanvas canvas)
		{
			canvas.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x00042B8A File Offset: 0x00040D8A
		private void UnblockCanvas(InputBlockerCanvas canvas)
		{
			canvas.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x000EBFC4 File Offset: 0x000EA1C4
		private void ResetUIUnblocks()
		{
			for (int i = 0; i < this.unblockedUI.Count; i++)
			{
				global::UnityEngine.Object.Destroy(this.unblockedUI[i]);
			}
			this.unblockedUI.Clear();
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x00042B9D File Offset: 0x00040D9D
		private void ResetUnitUnblocks()
		{
			this.BlockUnits();
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x00042BA5 File Offset: 0x00040DA5
		private void ResetHexUnblocks()
		{
			this.BlockHexes();
		}

		// Token: 0x04001D6F RID: 7535
		private List<InputBlockerCanvas> canvases = new List<InputBlockerCanvas>();

		// Token: 0x04001D70 RID: 7536
		private List<CanvasGroup> unblockedUI = new List<CanvasGroup>();
	}
}

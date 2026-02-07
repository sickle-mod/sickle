using System;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000417 RID: 1047
	public class MapBlackout : MonoBehaviour
	{
		// Token: 0x06001FF4 RID: 8180 RVA: 0x0003C7F8 File Offset: 0x0003A9F8
		public void Activate()
		{
			base.gameObject.SetActive(true);
			this.HideResourcesLabels();
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x0003C80C File Offset: 0x0003AA0C
		public void Deactivate()
		{
			this.RevertDefaultResourcesLayer();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x000C2E38 File Offset: 0x000C1038
		public void ShowResourcesLabels(GameHexPresenter hexPresenter)
		{
			foreach (GameHexPresenter.HexResource hexResource in hexPresenter.resources)
			{
				this.SetResourceLayer(hexResource, 8);
			}
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x000C2E68 File Offset: 0x000C1068
		private void RevertDefaultResourcesLayer()
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
						foreach (GameHexPresenter.HexResource hexResource in flatGameHexPresenter.resources)
						{
							this.SetResourceLayer(hexResource, 8);
						}
					}
				}
			}
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x000C2F0C File Offset: 0x000C110C
		private void HideResourcesLabels()
		{
			GameHex[,] hexMap = GameController.GameManager.gameBoard.hexMap;
			int upperBound = hexMap.GetUpperBound(0);
			int upperBound2 = hexMap.GetUpperBound(1);
			for (int i = hexMap.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = hexMap.GetLowerBound(1); j <= upperBound2; j++)
				{
					GameHex gameHex = hexMap[i, j];
					if (!PlatformManager.IsMobile || gameHex.Owner != GameController.GameManager.PlayerCurrent)
					{
						FlatGameHexPresenter flatGameHexPresenter = GameController.Instance.GetGameHexPresenter(gameHex) as FlatGameHexPresenter;
						if (flatGameHexPresenter != null)
						{
							foreach (GameHexPresenter.HexResource hexResource in flatGameHexPresenter.resources)
							{
								this.SetResourceLayer(hexResource, 1);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x0003C820 File Offset: 0x0003AA20
		private void SetResourceLayer(GameHexPresenter.HexResource hexResource, int layer)
		{
			hexResource.resourceObject.transform.GetChild(1).GetComponent<Canvas>().sortingOrder = layer;
		}

		// Token: 0x04001674 RID: 5748
		private const int DEFAULT_RESOURCE_LAYER = 8;

		// Token: 0x04001675 RID: 5749
		[SerializeField]
		private Image blackout;
	}
}

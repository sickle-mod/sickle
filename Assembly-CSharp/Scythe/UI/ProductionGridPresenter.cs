using System;
using System.Collections.Generic;
using HoneyFramework;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000403 RID: 1027
	public class ProductionGridPresenter : MonoBehaviour
	{
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06001F43 RID: 8003 RVA: 0x0003C1E3 File Offset: 0x0003A3E3
		// (set) Token: 0x06001F44 RID: 8004 RVA: 0x0003C1EB File Offset: 0x0003A3EB
		public Scythe.BoardPresenter.GameHexPresenter AttachedHex { get; private set; }

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001F45 RID: 8005 RVA: 0x0003C1F4 File Offset: 0x0003A3F4
		// (set) Token: 0x06001F46 RID: 8006 RVA: 0x0003C1FC File Offset: 0x0003A3FC
		public int WorkingUnitsOnHex { get; private set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06001F47 RID: 8007 RVA: 0x0003C205 File Offset: 0x0003A405
		// (set) Token: 0x06001F48 RID: 8008 RVA: 0x0003C20D File Offset: 0x0003A40D
		public int AmountToProduce { get; private set; }

		// Token: 0x06001F49 RID: 8009 RVA: 0x000BFECC File Offset: 0x000BE0CC
		private void Awake()
		{
			if (PlatformManager.IsMobile)
			{
				this.produceButton.enabled = false;
				GameController.HexGetFocused += this.OnHexFocused;
			}
			for (int i = 0; i < this.imageGrid.Count; i++)
			{
				for (int j = 0; j < this.imageGrid[i].Length; j++)
				{
					this.imageGrid[i][j].gameObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate
					{
						this.ToogleChangedSFX();
					});
				}
			}
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x0003C216 File Offset: 0x0003A416
		private void OnEnable()
		{
			base.transform.localScale = this.normalScale;
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x0003C229 File Offset: 0x0003A429
		private void OnDestroy()
		{
			if (PlatformManager.IsMobile)
			{
				GameController.HexGetFocused -= this.OnHexFocused;
			}
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x0003C243 File Offset: 0x0003A443
		public void AttachToHex(Scythe.BoardPresenter.GameHexPresenter hex)
		{
			this.AttachedHex = hex;
			this.UpdateProductionGrid();
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x0003C252 File Offset: 0x0003A452
		public void EnableMarker(bool enabled)
		{
			if (this.AttachedHex != null)
			{
				this.AttachedHex.SetFocus(enabled, HexMarkers.MarkerType.Move, 0f, false);
			}
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x0003C26F File Offset: 0x0003A46F
		private void ToogleChangedSFX()
		{
			WorldSFXManager.PlaySound(SoundEnum.ProduceSwitch, AudioSourceType.Buttons);
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x000BFF5C File Offset: 0x000BE15C
		private void CountWorkingUnits()
		{
			this.WorkingUnitsOnHex = this.AttachedHex.GetGameHexLogic().GetOwnerWorkers().Count;
			Building building = this.AttachedHex.GetGameHexLogic().Building;
			this.hasMill = building != null && building.buildingType == BuildingType.Mill && building.player == building.position.Owner;
			if (this.hasMill && !this.producePresenter.FactoryCardProduce())
			{
				int num = this.WorkingUnitsOnHex + 1;
				this.WorkingUnitsOnHex = num;
			}
			if (this.AttachedHex.hexType == HexType.village)
			{
				int count = this.AttachedHex.GetGameHexLogic().Owner.matPlayer.workers.Count;
				if (this.WorkingUnitsOnHex > 8 - count)
				{
					this.WorkingUnitsOnHex = 8 - count;
				}
			}
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x000C0028 File Offset: 0x000BE228
		public void FillImageGrid()
		{
			if (this.ProductionImagesGrid == null)
			{
				Debug.LogError("Grid of images for production is not assigned to ProducePresenter!");
				return;
			}
			for (int i = 0; i < this.gridRows.Length; i++)
			{
				List<Toggle> list = new List<Toggle>();
				foreach (object obj in this.gridRows[i])
				{
					Transform transform = (Transform)obj;
					list.Add(transform.GetComponent<Toggle>());
				}
				int count = list.Count;
				Toggle[] array = new Toggle[count];
				for (int j = 0; j < count; j++)
				{
					array[j] = list[j];
				}
				this.imageGrid.Add(array);
			}
			this.ProductionImagesGrid.gameObject.SetActive(true);
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x000C0110 File Offset: 0x000BE310
		private void ChangeSpritesBasingOnTheAttachedHex()
		{
			HexType hexType = this.AttachedHex.hexType;
			switch (hexType)
			{
			case HexType.mountain:
				this.ChangeProduceGridImagesTo(this.ToProduce[1]);
				this.ChangeImagesColorTo(this.SpriteColors[1]);
				if (!PlatformManager.IsStandalone)
				{
					this.produceButton.transform.GetChild(0).GetComponent<Image>().sprite = this.ToProduce[1];
					return;
				}
				break;
			case HexType.forest:
				this.ChangeProduceGridImagesTo(this.ToProduce[3]);
				this.ChangeImagesColorTo(this.SpriteColors[3]);
				if (!PlatformManager.IsStandalone)
				{
					this.produceButton.transform.GetChild(0).GetComponent<Image>().sprite = this.ToProduce[3];
					return;
				}
				break;
			case (HexType)3:
				break;
			case HexType.farm:
				this.ChangeProduceGridImagesTo(this.ToProduce[2]);
				this.ChangeImagesColorTo(this.SpriteColors[2]);
				if (!PlatformManager.IsStandalone)
				{
					this.produceButton.transform.GetChild(0).GetComponent<Image>().sprite = this.ToProduce[2];
					return;
				}
				break;
			default:
				if (hexType != HexType.tundra)
				{
					if (hexType != HexType.village)
					{
						return;
					}
					this.ChangeProduceGridImagesTo(this.ToProduce[4]);
					this.ChangeImagesColorTo(this.SpriteColors[4]);
					if (!PlatformManager.IsStandalone)
					{
						this.produceButton.transform.GetChild(0).GetComponent<Image>().sprite = this.ToProduce[4];
					}
				}
				else
				{
					this.ChangeProduceGridImagesTo(this.ToProduce[0]);
					this.ChangeImagesColorTo(this.SpriteColors[0]);
					if (!PlatformManager.IsStandalone)
					{
						this.produceButton.transform.GetChild(0).GetComponent<Image>().sprite = this.ToProduce[0];
						return;
					}
				}
				break;
			}
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x0003C279 File Offset: 0x0003A479
		public void UpdateProductionGrid()
		{
			this.CountWorkingUnits();
			this.AmountToProduce = this.WorkingUnitsOnHex;
			this.ChangeSpritesBasingOnTheAttachedHex();
			this.UpdatePosition();
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x000C02D0 File Offset: 0x000BE4D0
		private void ChangeProduceGridImagesTo(Sprite resourceImage)
		{
			if (this.imageGrid.Count == 0)
			{
				this.FillImageGrid();
			}
			for (int i = 0; i < this.imageGrid.Count; i++)
			{
				for (int j = 0; j < this.imageGrid[i].Length; j++)
				{
					this.imageGrid[i][j].isOn = true;
					this.imageGrid[i][j].transform.GetChild(0).GetComponent<Image>().sprite = resourceImage;
					this.imageGrid[i][j].transform.GetChild(0).GetChild(0).GetComponent<Image>()
						.sprite = resourceImage;
				}
			}
			this.TurnOffGrid();
			if (!PlatformManager.IsStandalone)
			{
				this.produceButton.gameObject.SetActive(true);
				this.amountMax.text = this.WorkingUnitsOnHex.ToString();
				this.amountMax.gameObject.SetActive(true);
				this.AttachedHex.SetFocus(true, HexMarkers.MarkerType.Move, 1f, false);
			}
			else
			{
				if (this.WorkingUnitsOnHex > 0)
				{
					if (this.producePresenter.FactoryCardProduce())
					{
						this.produceButton.gameObject.SetActive(true);
					}
					else
					{
						this.produceButton.gameObject.SetActive(!this.hasMill);
						this.produceMillButton.gameObject.SetActive(this.hasMill);
					}
					this.ProductionImagesGrid.transform.GetChild(0).gameObject.SetActive(true);
					this.ProductionImagesGrid.transform.GetChild(3).gameObject.SetActive(true);
					this.imageGrid[2][0].gameObject.SetActive(true);
					this.AttachedHex.SetFocus(true, HexMarkers.MarkerType.Move, 1f, false);
				}
				if (this.WorkingUnitsOnHex > 1)
				{
					this.ProductionImagesGrid.transform.GetChild(2).gameObject.SetActive(true);
					this.imageGrid[1][0].gameObject.SetActive(true);
				}
				if (this.WorkingUnitsOnHex > 2)
				{
					this.imageGrid[2][1].gameObject.SetActive(true);
				}
				if (this.WorkingUnitsOnHex > 3)
				{
					this.imageGrid[1][1].gameObject.SetActive(true);
				}
				if (this.WorkingUnitsOnHex > 4)
				{
					this.imageGrid[2][2].gameObject.SetActive(true);
				}
				if (this.WorkingUnitsOnHex > 5)
				{
					this.ProductionImagesGrid.transform.GetChild(1).gameObject.SetActive(true);
					this.imageGrid[0][0].gameObject.SetActive(true);
				}
				if (this.WorkingUnitsOnHex > 6)
				{
					this.imageGrid[1][2].gameObject.SetActive(true);
				}
				if (this.WorkingUnitsOnHex > 7)
				{
					this.imageGrid[0][1].gameObject.SetActive(true);
				}
				if (this.WorkingUnitsOnHex > 8)
				{
					this.imageGrid[0][2].gameObject.SetActive(true);
				}
			}
			this.ProductionImagesGrid.transform.position = Camera.main.WorldToScreenPoint(HexCoordinates.HexToWorld3D(this.AttachedHex.position));
			this.ProductionImagesGrid.SetActive(true);
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x000C0630 File Offset: 0x000BE830
		private void ChangeImagesColorTo(Color color)
		{
			for (int i = 0; i < this.imageGrid.Count; i++)
			{
				for (int j = 0; j < this.imageGrid[i].Length; j++)
				{
					Color black = Color.black;
					black.a = 0.75f;
					this.imageGrid[i][j].transform.GetChild(0).GetComponent<Image>().color = black;
					this.imageGrid[i][j].transform.GetChild(0).GetChild(0).GetComponent<Image>()
						.color = color;
				}
			}
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x000C06D0 File Offset: 0x000BE8D0
		public void TurnOffGrid()
		{
			foreach (object obj in this.ProductionImagesGrid.transform)
			{
				((Transform)obj).gameObject.SetActive(false);
			}
			foreach (Toggle[] array in this.imageGrid)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0003C299 File Offset: 0x0003A499
		private void Update()
		{
			this.UpdatePosition();
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x000C0790 File Offset: 0x000BE990
		private void UpdatePosition()
		{
			if (this.ProductionImagesGrid.activeInHierarchy)
			{
				this.ProductionImagesGrid.transform.position = HexCoordinates.HexToWorld3D(this.AttachedHex.position);
				this.ProductionImagesGrid.transform.position = new Vector3(this.ProductionImagesGrid.transform.position.x, this.ProductionImagesGrid.transform.position.y, this.ProductionImagesGrid.transform.position.z + 0.2f);
				base.transform.rotation = Quaternion.Euler(90f, 90f, CameraControler.Instance.encounterRotationAdjustment.z);
			}
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x000C0850 File Offset: 0x000BEA50
		private void UpdateScale()
		{
			float zoom = Camera.main.GetComponent<CameraControler>().zoom;
			if (zoom >= this.cameraZoomScalingBoundary)
			{
				float num = this.normalScale.x - (this.normalScale.x - this.maxZoomOutScale.x) * ((zoom - this.cameraZoomScalingBoundary) / (1f - this.cameraZoomScalingBoundary));
				base.transform.localScale = new Vector3(num, num, num);
			}
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0003C2A1 File Offset: 0x0003A4A1
		private void OnHexFocused(Scythe.BoardPresenter.GameHexPresenter gameHexPresenter)
		{
			if ((this.produceButton.interactable || this.produceMillButton.interactable) && this.AttachedHex != null && gameHexPresenter == this.AttachedHex)
			{
				this.ClickApprove();
			}
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x000C08C4 File Offset: 0x000BEAC4
		public void ClickApprove()
		{
			int num = 0;
			for (int i = 0; i < this.imageGrid.Count; i++)
			{
				for (int j = 0; j < this.imageGrid[i].Length; j++)
				{
					if (this.imageGrid[i][j].IsActive() && this.imageGrid[i][j].isOn)
					{
						num++;
					}
				}
			}
			if (this.AttachedHex.GetGameHexLogic().Building != null)
			{
				BuildingType buildingType = this.AttachedHex.GetGameHexLogic().Building.buildingType;
			}
			if (PlatformManager.IsStandalone)
			{
				this.producePresenter.OnHexApproved(this.AttachedHex, num);
				return;
			}
			this.producePresenter.ResetHexColors();
			this.hexBackground.color = Color.green;
			this.producePresenter.OnHexSelectedMobile(this.AttachedHex, this.WorkingUnitsOnHex);
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x0003C2D4 File Offset: 0x0003A4D4
		public void Clear()
		{
			this.AttachedHex = null;
			this.AmountToProduce = 0;
		}

		// Token: 0x04001604 RID: 5636
		[HideInInspector]
		public ProducePresenter producePresenter;

		// Token: 0x04001605 RID: 5637
		public GameObject ProductionImagesGrid;

		// Token: 0x04001606 RID: 5638
		public Sprite[] ToProduce;

		// Token: 0x04001607 RID: 5639
		public Color[] SpriteColors;

		// Token: 0x04001609 RID: 5641
		public Transform[] gridRows;

		// Token: 0x0400160A RID: 5642
		public Button produceButton;

		// Token: 0x0400160B RID: 5643
		public Button produceMillButton;

		// Token: 0x0400160C RID: 5644
		public Text amountMax;

		// Token: 0x0400160D RID: 5645
		public Image hexBackground;

		// Token: 0x04001610 RID: 5648
		public bool hasMill;

		// Token: 0x04001611 RID: 5649
		private List<Toggle[]> imageGrid = new List<Toggle[]>();

		// Token: 0x04001612 RID: 5650
		private float cameraZoomScalingBoundary = 0.2f;

		// Token: 0x04001613 RID: 5651
		private Vector3 normalScale = new Vector3(0.0405f, 0.0405f, 0.0405f);

		// Token: 0x04001614 RID: 5652
		private Vector3 maxZoomOutScale = new Vector3(0.6f, 0.6f, 0.6f);
	}
}

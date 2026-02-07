using System;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.BoardPresenter
{
	// Token: 0x020001EE RID: 494
	public class StrategicView : MonoBehaviour
	{
		// Token: 0x06000E4A RID: 3658 RVA: 0x00089678 File Offset: 0x00087878
		public void UpdateFromLogic(GameHex hex)
		{
			this.resOilIcon.enabled = (this.resOilCount.enabled = hex.resources[ResourceType.oil] > 0);
			this.resMetalIcon.enabled = (this.resMetalCount.enabled = hex.resources[ResourceType.metal] > 0);
			this.resWoodIcon.enabled = (this.resWoodCount.enabled = hex.resources[ResourceType.wood] > 0);
			this.resFoodIcon.enabled = (this.resFoodCount.enabled = hex.resources[ResourceType.food] > 0);
			this.resOilCount.text = hex.resources[ResourceType.oil].ToString();
			this.resMetalCount.text = hex.resources[ResourceType.metal].ToString();
			this.resWoodCount.text = hex.resources[ResourceType.wood].ToString();
			this.resFoodCount.text = hex.resources[ResourceType.food].ToString();
			if (hex.Owner == null)
			{
				this.background.color = Color.grey;
			}
			else
			{
				Color color = GameController.factionInfo[hex.Owner.matFaction.faction].color * 0.6f;
				color.a = 1f;
				this.background.color = color;
			}
			HexType hexType = hex.hexType;
			switch (hexType)
			{
			case HexType.mountain:
				this.hexTypeIcon.sprite = this.hexTypeList[0];
				break;
			case HexType.forest:
				this.hexTypeIcon.sprite = this.hexTypeList[1];
				break;
			case (HexType)3:
				break;
			case HexType.farm:
				this.hexTypeIcon.sprite = this.hexTypeList[2];
				break;
			default:
				if (hexType != HexType.tundra)
				{
					switch (hexType)
					{
					case HexType.village:
						this.hexTypeIcon.sprite = this.hexTypeList[4];
						break;
					case HexType.capital:
						this.hexTypeIcon.sprite = this.hexTypeList[5];
						break;
					case HexType.lake:
						this.hexTypeIcon.sprite = this.hexTypeList[6];
						break;
					case HexType.factory:
						this.hexTypeIcon.sprite = this.hexTypeList[7];
						break;
					}
				}
				else
				{
					this.hexTypeIcon.sprite = this.hexTypeList[3];
				}
				break;
			}
			int count = hex.GetOwnerWorkers().Count;
			this.workerCount.text = count.ToString();
			this.workerCount.enabled = count > 0;
			this.workerIcon.gameObject.SetActive(count > 0);
			int count2 = hex.GetOwnerMechs().Count;
			this.mechCount.text = count2.ToString();
			this.mechCount.enabled = count2 > 0;
			this.mechIcon.gameObject.SetActive(count2 > 0);
			this.characterIcon.SetActive(hex.HasOwnerCharacter());
			this.encounter.enabled = hex.hasEncounter && !hex.encounterUsed && !hex.encounterTaken;
			this.structureIcon.SetActive(hex.Building != null);
			this.structureType.enabled = hex.Building != null;
		}

		// Token: 0x04000B39 RID: 2873
		public Image background;

		// Token: 0x04000B3A RID: 2874
		public Image resOilIcon;

		// Token: 0x04000B3B RID: 2875
		public Image resMetalIcon;

		// Token: 0x04000B3C RID: 2876
		public Image resWoodIcon;

		// Token: 0x04000B3D RID: 2877
		public Image resFoodIcon;

		// Token: 0x04000B3E RID: 2878
		public Text resOilCount;

		// Token: 0x04000B3F RID: 2879
		public Text resMetalCount;

		// Token: 0x04000B40 RID: 2880
		public Text resWoodCount;

		// Token: 0x04000B41 RID: 2881
		public Text resFoodCount;

		// Token: 0x04000B42 RID: 2882
		public Image hexTypeIcon;

		// Token: 0x04000B43 RID: 2883
		public Sprite[] hexTypeList;

		// Token: 0x04000B44 RID: 2884
		public GameObject workerIcon;

		// Token: 0x04000B45 RID: 2885
		public GameObject mechIcon;

		// Token: 0x04000B46 RID: 2886
		public GameObject characterIcon;

		// Token: 0x04000B47 RID: 2887
		public GameObject structureIcon;

		// Token: 0x04000B48 RID: 2888
		public Text workerCount;

		// Token: 0x04000B49 RID: 2889
		public Text mechCount;

		// Token: 0x04000B4A RID: 2890
		public Image encounter;

		// Token: 0x04000B4B RID: 2891
		public Image structureType;

		// Token: 0x04000B4C RID: 2892
		public Sprite[] structureTypeList;
	}
}

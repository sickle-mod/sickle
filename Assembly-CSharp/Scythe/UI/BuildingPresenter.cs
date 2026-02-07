using System;
using System.Collections.Generic;
using System.Linq;
using cakeslice;
using DG.Tweening;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200040F RID: 1039
	public class BuildingPresenter : MonoBehaviour, ITooltipInfo, ISeismograph
	{
		// Token: 0x06001FD2 RID: 8146 RVA: 0x000C2364 File Offset: 0x000C0564
		private void OnEnable()
		{
			if (((GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerOwner != null && !GameController.GameManager.IsMyTurn()) || (!GameController.GameManager.IsMultiplayer && !GameController.GameManager.PlayerCurrent.IsHuman && GameController.GameManager.showEnemyActions)) && !this.firstSpawn)
			{
				this.spawnAnimation = true;
				base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 2f, base.transform.position.z);
			}
			if (PlatformManager.IsStandalone)
			{
				this.materialPropertyBlock = new MaterialPropertyBlock();
			}
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x000C2428 File Offset: 0x000C0628
		public void UpdateBuilding(Building building, Dictionary<Faction, GameController.FactionInfo> factionInfo, GameHexPresenter hex)
		{
			if (building != null)
			{
				base.gameObject.SetActive(true);
				this.hex = hex;
				this.buildingLogical = building;
				Transform child = base.transform.GetChild((int)building.buildingType);
				if (!child.gameObject.activeSelf && !this.buildingLogical.enemySpawnAnimation)
				{
					child.gameObject.SetActive(true);
					this.SetActive(true);
				}
				this.UpdateBuildingMaterial(child, this.buildingLogical.player.matFaction.faction);
				this.UpdateBuildingIcon(building, child);
				return;
			}
			this.SetActive(false);
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0003C6E2 File Offset: 0x0003A8E2
		private void UpdateBuildingMaterial(Transform buildingObject, Faction faction)
		{
			if (PlatformManager.IsMobile)
			{
				this.UpdateBuildingTexture(buildingObject, faction);
				return;
			}
			this.UpdateBuildingColor(buildingObject, faction);
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x000C24C0 File Offset: 0x000C06C0
		private void UpdateBuildingColor(Transform buildingObject, Faction faction)
		{
			switch (faction)
			{
			case Faction.Polania:
				this.SetColor(buildingObject, this.polaniaColor);
				return;
			case Faction.Albion:
				this.SetColor(buildingObject, this.albionColor);
				return;
			case Faction.Nordic:
				this.SetColor(buildingObject, this.nordicColor);
				return;
			case Faction.Rusviet:
				this.SetColor(buildingObject, this.rusvietColor);
				return;
			case Faction.Togawa:
				this.SetColor(buildingObject, this.togawaColor);
				return;
			case Faction.Crimea:
				this.SetColor(buildingObject, this.crimeaColor);
				return;
			case Faction.Saxony:
				this.SetColor(buildingObject, this.saxonyColor);
				return;
			default:
				throw new ArgumentOutOfRangeException("faction", faction, null);
			}
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x000C2564 File Offset: 0x000C0764
		private void UpdateBuildingTexture(Transform buildingObject, Faction faction)
		{
			switch (faction)
			{
			case Faction.Polania:
				this.SetTexture(buildingObject, this.polaniaTextureMobile);
				return;
			case Faction.Albion:
				this.SetTexture(buildingObject, this.albionTextureMobile);
				return;
			case Faction.Nordic:
				this.SetTexture(buildingObject, this.nordicTextureMobile);
				return;
			case Faction.Rusviet:
				this.SetTexture(buildingObject, this.rusvietTextureMobile);
				return;
			case Faction.Togawa:
				this.SetTexture(buildingObject, this.togawaTextureMobile);
				return;
			case Faction.Crimea:
				this.SetTexture(buildingObject, this.crimeaTextureMobile);
				return;
			case Faction.Saxony:
				this.SetTexture(buildingObject, this.saxonyTextureMobile);
				return;
			default:
				throw new ArgumentOutOfRangeException("faction", faction, null);
			}
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x0003C6FC File Offset: 0x0003A8FC
		private void SetColor(Transform buildingObject, Color32 color)
		{
			buildingObject.GetChild(0).GetChild(0).GetComponent<MeshRenderer>()
				.material.color = color;
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x0003C720 File Offset: 0x0003A920
		private void SetTexture(Transform buildingObject, Texture texture)
		{
			buildingObject.GetChild(0).GetChild(0).GetComponent<MeshRenderer>()
				.material.mainTexture = texture;
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x000C2608 File Offset: 0x000C0808
		private void UpdateBuildingIcon(Building building, Transform buildingObject)
		{
			buildingObject.GetChild(2).GetComponentInChildren<Image>().sprite = this.buildingFactionsIcons.First((BuildingPresenter.BuildingFactionsIconsData x) => x.faction == this.buildingLogical.player.matFaction.faction).iconsData.First((BuildingPresenter.BuildingIconsData x) => x.buildingType == building.buildingType).sprite;
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x000C266C File Offset: 0x000C086C
		private void UpdateBuildingIcon(Building building, Transform buildingObject, Faction faction)
		{
			buildingObject.GetChild(2).GetComponentInChildren<Image>().sprite = this.buildingFactionsIcons.First((BuildingPresenter.BuildingFactionsIconsData x) => x.faction == faction).iconsData.First((BuildingPresenter.BuildingIconsData x) => x.buildingType == building.buildingType).sprite;
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x000C26D0 File Offset: 0x000C08D0
		private void UpdateBuildingIcon(BuildingType buildingType, Transform buildingObject, Faction faction)
		{
			buildingObject.GetChild(2).GetComponentInChildren<Image>().sprite = this.buildingFactionsIcons.First((BuildingPresenter.BuildingFactionsIconsData x) => x.faction == faction).iconsData.First((BuildingPresenter.BuildingIconsData x) => x.buildingType == buildingType).sprite;
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x000C2734 File Offset: 0x000C0934
		public void LaunchSpawnAnimation(Building building, GameHexPresenter hex, Faction faction)
		{
			if (building == null || hex == null)
			{
				return;
			}
			base.gameObject.SetActive(true);
			this.buildingLogical = building;
			Transform child = base.transform.GetChild((int)building.buildingType);
			this.UpdateBuildingIcon(building, child, faction);
			if (this.firstSpawn)
			{
				base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 2f, base.transform.position.z);
				if (!this.spawnAnimation)
				{
					base.transform.DOMoveY(0f, 1f, false).SetEase(Ease.InExpo).OnComplete(delegate
					{
						this.SpawnAnimationEnd(building.buildingType, false);
					});
				}
			}
			this.SetActive(true);
			this.UpdateBuildingMaterial(child, faction);
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000C2834 File Offset: 0x000C0A34
		public void SpawnAnimation(BuildingType buildingType, Faction faction, bool enemyAction = false)
		{
			if (this.firstSpawn)
			{
				base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 2f, base.transform.position.z);
			}
			Transform child = base.transform.GetChild((int)buildingType);
			this.UpdateBuildingIcon(buildingType, child, faction);
			if (!child.gameObject.activeSelf)
			{
				child.gameObject.SetActive(true);
				this.SetActive(true);
			}
			this.UpdateBuildingMaterial(child, faction);
			base.transform.DOMoveY(0f, 1f, false).SetEase(Ease.InExpo).OnComplete(delegate
			{
				this.SpawnAnimationEnd(buildingType, enemyAction);
			});
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x000C2924 File Offset: 0x000C0B24
		private void SpawnAnimationEnd(BuildingType buildingType, bool enemyAction = false)
		{
			switch (buildingType)
			{
			case BuildingType.Mine:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardBuildMine, AudioSourceType.WorldSfx);
				break;
			case BuildingType.Monument:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardBuildMonument, AudioSourceType.WorldSfx);
				break;
			case BuildingType.Armory:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardBuildArmory, AudioSourceType.WorldSfx);
				break;
			case BuildingType.Mill:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardBuildMill, AudioSourceType.WorldSfx);
				break;
			}
			this.firstSpawn = false;
			if (enemyAction)
			{
				this.spawnAnimation = false;
				ShowEnemyMoves.Instance.SetAnimationInProgress(false);
				ShowEnemyMoves.Instance.GetNextAnimation();
			}
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x000C2998 File Offset: 0x000C0B98
		public void SetActive(bool active)
		{
			if (base.gameObject != null)
			{
				base.gameObject.SetActive(active);
				if (this.buildingLogical != null)
				{
					int buildingType = (int)this.buildingLogical.buildingType;
					foreach (object obj in base.transform)
					{
						Transform transform = (Transform)obj;
						if (transform == base.transform.GetChild(buildingType))
						{
							transform.gameObject.SetActive(active);
						}
						else if (transform.gameObject.activeSelf)
						{
							transform.gameObject.SetActive(false);
						}
					}
				}
			}
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x000C2A58 File Offset: 0x000C0C58
		public void ActivateOutline(bool active)
		{
			if (this.buildingLogical == null)
			{
				Debug.LogWarning("Building is empty, cannot get outline.");
				return;
			}
			if (base.transform.childCount < (int)this.buildingLogical.buildingType)
			{
				Debug.LogWarning("The number of children is lower than required.");
				return;
			}
			if (base.transform.GetChild((int)this.buildingLogical.buildingType).GetComponentInChildren<global::cakeslice.Outline>() == null)
			{
				Debug.LogWarning("Outline not found.");
				return;
			}
			if (PlatformManager.IsStandalone)
			{
				this.buildingRenderer = base.transform.GetChild((int)this.buildingLogical.buildingType).GetComponentInChildren<Renderer>();
				this.buildingRenderer.GetPropertyBlock(this.materialPropertyBlock);
				if (active)
				{
					this.materialPropertyBlock.SetFloat("_FresnelIntensivity", 11f);
					this.materialPropertyBlock.SetColor("_FresnelColor", this.fresnelColors[0]);
				}
				else
				{
					this.materialPropertyBlock.SetFloat("_FresnelIntensivity", 0f);
				}
				this.buildingRenderer.SetPropertyBlock(this.materialPropertyBlock);
				return;
			}
			base.transform.GetChild((int)this.buildingLogical.buildingType).GetComponentInChildren<global::cakeslice.Outline>().enabled = active;
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x000C2B84 File Offset: 0x000C0D84
		public void SetColliderEnabled(bool enabled)
		{
			Collider componentInChildren = base.GetComponentInChildren<Collider>();
			if (componentInChildren != null)
			{
				componentInChildren.enabled = enabled;
				return;
			}
			Debug.LogWarning("Collider not attached");
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x000C2BB4 File Offset: 0x000C0DB4
		public string InfoBasic()
		{
			return "Building" + this.buildingLogical.buildingType.ToString() + this.buildingLogical.player.matFaction.faction.ToString() + "Basic";
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x000C2C08 File Offset: 0x000C0E08
		public string InfoAdv()
		{
			Faction faction = GameController.GameManager.PlayerCurrent.matFaction.faction;
			if (GameController.GameManager.IsMultiplayer)
			{
				faction = GameController.GameManager.PlayerOwner.matFaction.faction;
			}
			return "Building" + this.buildingLogical.buildingType.ToString() + ((this.buildingLogical.player.matFaction.faction == faction) ? "Friend" : "Enemy") + "Adv";
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000C2C94 File Offset: 0x000C0E94
		public void OnQuakeDetected(Vector3 epicenter, float force, float radius)
		{
			Vector3 position = base.transform.position;
			position.y = 0f;
			float magnitude = (epicenter - position).magnitude;
			float num = (radius - magnitude) / radius;
			if (num <= 0f)
			{
				return;
			}
			float num2 = 0.5f * num;
			float num3 = force * num / (this.mass * this.forceAmortization);
			this.SetColliderEnabled(false);
			base.transform.DOJump(base.transform.position, num3, 1, num2, false).OnComplete(delegate
			{
				this.SetColliderEnabled(true);
			});
		}

		// Token: 0x0400164D RID: 5709
		public List<BuildingPresenter.BuildingFactionsIconsData> buildingFactionsIcons;

		// Token: 0x0400164E RID: 5710
		public GameHexPresenter hex;

		// Token: 0x0400164F RID: 5711
		public Building buildingLogical;

		// Token: 0x04001650 RID: 5712
		[SerializeField]
		private Color32 saxonyColor = new Color32(50, 50, 50, byte.MaxValue);

		// Token: 0x04001651 RID: 5713
		[SerializeField]
		private Color32 polaniaColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04001652 RID: 5714
		[SerializeField]
		private Color32 crimeaColor = new Color32(225, 130, 0, byte.MaxValue);

		// Token: 0x04001653 RID: 5715
		[SerializeField]
		private Color32 nordicColor = new Color32(0, 115, 172, byte.MaxValue);

		// Token: 0x04001654 RID: 5716
		[SerializeField]
		private Color32 rusvietColor = new Color32(180, 17, 18, byte.MaxValue);

		// Token: 0x04001655 RID: 5717
		[SerializeField]
		private Color32 togawaColor = new Color32(93, 13, 145, byte.MaxValue);

		// Token: 0x04001656 RID: 5718
		[SerializeField]
		private Color32 albionColor = new Color32(46, 68, 30, byte.MaxValue);

		// Token: 0x04001657 RID: 5719
		[SerializeField]
		private Texture saxonyTextureMobile;

		// Token: 0x04001658 RID: 5720
		[SerializeField]
		private Texture polaniaTextureMobile;

		// Token: 0x04001659 RID: 5721
		[SerializeField]
		private Texture crimeaTextureMobile;

		// Token: 0x0400165A RID: 5722
		[SerializeField]
		private Texture nordicTextureMobile;

		// Token: 0x0400165B RID: 5723
		[SerializeField]
		private Texture rusvietTextureMobile;

		// Token: 0x0400165C RID: 5724
		[SerializeField]
		private Texture togawaTextureMobile;

		// Token: 0x0400165D RID: 5725
		[SerializeField]
		private Texture albionTextureMobile;

		// Token: 0x0400165E RID: 5726
		private bool firstSpawn = true;

		// Token: 0x0400165F RID: 5727
		private bool spawnAnimation;

		// Token: 0x04001660 RID: 5728
		[SerializeField]
		private float mass = 0.7f;

		// Token: 0x04001661 RID: 5729
		private float forceAmortization = 50f;

		// Token: 0x04001662 RID: 5730
		[SerializeField]
		private Renderer buildingRenderer;

		// Token: 0x04001663 RID: 5731
		private MaterialPropertyBlock materialPropertyBlock;

		// Token: 0x04001664 RID: 5732
		private Color[] fresnelColors = new Color[]
		{
			Color.yellow,
			Color.green,
			Color.blue
		};

		// Token: 0x02000410 RID: 1040
		[Serializable]
		public struct BuildingIconsData
		{
			// Token: 0x04001665 RID: 5733
			public BuildingType buildingType;

			// Token: 0x04001666 RID: 5734
			public Sprite sprite;
		}

		// Token: 0x02000411 RID: 1041
		[Serializable]
		public struct BuildingFactionsIconsData
		{
			// Token: 0x04001667 RID: 5735
			public Faction faction;

			// Token: 0x04001668 RID: 5736
			public List<BuildingPresenter.BuildingIconsData> iconsData;
		}
	}
}

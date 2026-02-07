using System;
using System.Collections.Generic;
using System.Linq;
using cakeslice;
using DG.Tweening;
using HoneyFramework;
using Scythe.GameLogic;
using Scythe.UI;
using TMPro;
using UnityEngine;

namespace Scythe.BoardPresenter
{
	// Token: 0x020001DD RID: 477
	public abstract class GameHexPresenter : ITooltipInfo
	{
		// Token: 0x06000DF3 RID: 3571
		public abstract void InitScytheValues();

		// Token: 0x06000DF4 RID: 3572
		protected abstract void CreateBuilding(Building building);

		// Token: 0x06000DF5 RID: 3573
		protected abstract void CreateToken(FactionAbilityToken token);

		// Token: 0x06000DF6 RID: 3574
		public abstract Vector3 GetUnitPosition(Unit unit);

		// Token: 0x06000DF7 RID: 3575
		public abstract Vector3 GetEnemyUnitPosition(Unit unit);

		// Token: 0x06000DF8 RID: 3576
		public abstract Vector3 CharacterPosition();

		// Token: 0x06000DF9 RID: 3577
		public abstract Vector3 EnemyCharacterPosition();

		// Token: 0x06000DFA RID: 3578
		public abstract Vector3 MechPosition(int i);

		// Token: 0x06000DFB RID: 3579
		public abstract Vector3 EnemyMechPosition(int i);

		// Token: 0x06000DFC RID: 3580
		public abstract Vector3 WorkerPosition(int i);

		// Token: 0x06000DFD RID: 3581
		public abstract Vector3 EnemyWorkerPosition(int i);

		// Token: 0x06000DFE RID: 3582
		protected abstract Vector3 UpdateHeight(Vector3 pos);

		// Token: 0x06000DFF RID: 3583 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void SetResources()
		{
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void SetTunnel()
		{
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void SetEncounter()
		{
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void SetFactory()
		{
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void SetVillage()
		{
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void SetCapital()
		{
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void SetForest()
		{
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void SetTundra()
		{
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void SetMountain()
		{
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void SetFarm()
		{
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void SetLake()
		{
		}

		// Token: 0x06000E0A RID: 3594
		public abstract Vector3 GetTokenPosition();

		// Token: 0x06000E0B RID: 3595
		public abstract GameHex GetGameHexLogic();

		// Token: 0x06000E0C RID: 3596
		public abstract Vector2 GetWorldPosition();

		// Token: 0x06000E0D RID: 3597 RVA: 0x00088550 File Offset: 0x00086750
		public ResourcePresenter GetResourcePresenter(ResourceType type)
		{
			foreach (GameHexPresenter.HexResource hexResource in this.resources)
			{
				if (hexResource.resourceType == type)
				{
					return hexResource.resourceObject.GetComponent<ResourcePresenter>();
				}
			}
			return null;
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0008858C File Offset: 0x0008678C
		public void UpdateTempResource(ResourceType type, bool increase, bool isProduction, int amountToProduce)
		{
			foreach (GameHexPresenter.HexResource hexResource in this.resources)
			{
				if (hexResource.resourceType == type)
				{
					int num = int.Parse(hexResource.resourceCountInfo.text);
					if (increase)
					{
						num++;
					}
					else
					{
						num--;
					}
					hexResource.resourceObject.GetComponent<ResourcePresenter>().UpdateTempResource(num);
					if (increase)
					{
						if (num > 0)
						{
							if (num == 1)
							{
								if (PlatformManager.IsStandalone)
								{
									hexResource.resourceCountInfo.gameObject.SetActive(false);
								}
								hexResource.resourceObject.transform.position = new Vector3(hexResource.resourceObject.transform.position.x, hexResource.resourceObject.transform.position.y + 2f, hexResource.resourceObject.transform.position.z);
								hexResource.resourceObject.SetActive(true);
								hexResource.resourceObject.transform.DOMoveY(0f, 0.5f, false).SetEase(Ease.InExpo);
							}
							else if (type == ResourceType.metal)
							{
								GameObject resourceToAnimation2 = ResourcesObjectPoolScript.Instance.GetPooledObject(0);
								resourceToAnimation2.transform.position = new Vector3(hexResource.resourceObject.transform.position.x, hexResource.resourceObject.transform.position.y + 2f, hexResource.resourceObject.transform.position.z);
								resourceToAnimation2.SetActive(true);
								resourceToAnimation2.transform.DOMoveY(0f, 0.5f, false).SetEase(Ease.InExpo).OnComplete(delegate
								{
									this.SpawnResourceAnimationEnd(resourceToAnimation2, type, false, 0);
								});
							}
							else if (type == ResourceType.wood)
							{
								GameObject resourceToAnimation3 = ResourcesObjectPoolScript.Instance.GetPooledObject(1);
								resourceToAnimation3.transform.position = new Vector3(hexResource.resourceObject.transform.position.x, hexResource.resourceObject.transform.position.y + 2f, hexResource.resourceObject.transform.position.z);
								resourceToAnimation3.SetActive(true);
								resourceToAnimation3.transform.DOMoveY(0f, 0.5f, false).SetEase(Ease.InExpo).OnComplete(delegate
								{
									this.SpawnResourceAnimationEnd(resourceToAnimation3, type, false, 0);
								});
							}
							else if (type == ResourceType.oil)
							{
								GameObject resourceToAnimation4 = ResourcesObjectPoolScript.Instance.GetPooledObject(2);
								resourceToAnimation4.transform.position = new Vector3(hexResource.resourceObject.transform.position.x, hexResource.resourceObject.transform.position.y + 2f, hexResource.resourceObject.transform.position.z);
								resourceToAnimation4.SetActive(true);
								resourceToAnimation4.transform.DOMoveY(0f, 0.5f, false).SetEase(Ease.InExpo).OnComplete(delegate
								{
									this.SpawnResourceAnimationEnd(resourceToAnimation4, type, false, 0);
								});
							}
							else if (type == ResourceType.food)
							{
								GameObject resourceToAnimation5 = ResourcesObjectPoolScript.Instance.GetPooledObject(3);
								resourceToAnimation5.transform.position = new Vector3(hexResource.resourceObject.transform.position.x, hexResource.resourceObject.transform.position.y + 2f, hexResource.resourceObject.transform.position.z);
								resourceToAnimation5.SetActive(true);
								resourceToAnimation5.transform.DOMoveY(0f, 0.5f, false).SetEase(Ease.InExpo).OnComplete(delegate
								{
									this.SpawnResourceAnimationEnd(resourceToAnimation5, type, false, 0);
								});
							}
						}
					}
					else if (type == ResourceType.metal)
					{
						GameObject resourceToAnimation6 = ResourcesObjectPoolScript.Instance.GetPooledObject(0);
						resourceToAnimation6.transform.position = new Vector3(hexResource.resourceObject.transform.position.x, hexResource.resourceObject.transform.position.y, hexResource.resourceObject.transform.position.z);
						resourceToAnimation6.SetActive(true);
						resourceToAnimation6.transform.DOMoveY(2f, 0.5f, false).SetEase(Ease.InExpo).OnComplete(delegate
						{
							this.PayResourceAnimationEnd(resourceToAnimation6, type, false, 0);
						});
					}
					else if (type == ResourceType.wood)
					{
						GameObject resourceToAnimation7 = ResourcesObjectPoolScript.Instance.GetPooledObject(1);
						resourceToAnimation7.transform.position = new Vector3(hexResource.resourceObject.transform.position.x, hexResource.resourceObject.transform.position.y, hexResource.resourceObject.transform.position.z);
						resourceToAnimation7.SetActive(true);
						resourceToAnimation7.transform.DOMoveY(2f, 0.5f, false).SetEase(Ease.InExpo).OnComplete(delegate
						{
							this.PayResourceAnimationEnd(resourceToAnimation7, type, false, 0);
						});
					}
					else if (type == ResourceType.oil)
					{
						GameObject resourceToAnimation8 = ResourcesObjectPoolScript.Instance.GetPooledObject(2);
						resourceToAnimation8.transform.position = new Vector3(hexResource.resourceObject.transform.position.x, hexResource.resourceObject.transform.position.y, hexResource.resourceObject.transform.position.z);
						resourceToAnimation8.SetActive(true);
						resourceToAnimation8.transform.DOMoveY(2f, 0.5f, false).SetEase(Ease.InExpo).OnComplete(delegate
						{
							this.PayResourceAnimationEnd(resourceToAnimation8, type, false, 0);
						});
					}
					else if (type == ResourceType.food)
					{
						GameObject resourceToAnimation = ResourcesObjectPoolScript.Instance.GetPooledObject(3);
						resourceToAnimation.transform.position = new Vector3(hexResource.resourceObject.transform.position.x, hexResource.resourceObject.transform.position.y, hexResource.resourceObject.transform.position.z);
						resourceToAnimation.SetActive(true);
						resourceToAnimation.transform.DOMoveY(2f, 0.5f, false).SetEase(Ease.InExpo).OnComplete(delegate
						{
							this.PayResourceAnimationEnd(resourceToAnimation, type, false, 0);
						});
					}
				}
			}
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00088D30 File Offset: 0x00086F30
		public void ProduceAnimation(int amountToProduce, ResourceType type, bool enemyAction = false)
		{
			this.spawnResourceIndex = 0;
			for (int i = 0; i < amountToProduce - 1; i++)
			{
				this.SpawnProduceResources(type, enemyAction, amountToProduce);
				this.spawnResourceIndex++;
			}
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00088D6C File Offset: 0x00086F6C
		public void PayResourcesAnimation(int amountToPay, ResourceType type, bool enemyAction = false)
		{
			this.payResourceIndex = 0;
			ResourcePresenter resourcePresenter = this.GetResourcePresenter(type);
			int num = ((type != ResourceType.combatCard) ? resourcePresenter.GetTemporaryResourceValue() : 1);
			for (int i = 0; i < amountToPay; i++)
			{
				int num2 = i;
				this.PayResourceAnimation(num2, type, enemyAction, --num);
				this.payResourceIndex++;
			}
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00088DC0 File Offset: 0x00086FC0
		public void PayResourceAnimation(int id, ResourceType type, bool enemyAction = false, int amountToProduce = 0)
		{
			GameObject resourceToAnimation = null;
			if (type == ResourceType.metal)
			{
				resourceToAnimation = ResourcesObjectPoolScript.Instance.GetPooledObject(0);
				resourceToAnimation.transform.position = this.resources[1].resourceObject.transform.position;
			}
			else if (type == ResourceType.wood)
			{
				resourceToAnimation = ResourcesObjectPoolScript.Instance.GetPooledObject(1);
				resourceToAnimation.transform.position = this.resources[3].resourceObject.transform.position;
			}
			else if (type == ResourceType.oil)
			{
				resourceToAnimation = ResourcesObjectPoolScript.Instance.GetPooledObject(2);
				resourceToAnimation.transform.position = this.resources[0].resourceObject.transform.position;
			}
			else if (type == ResourceType.food)
			{
				resourceToAnimation = ResourcesObjectPoolScript.Instance.GetPooledObject(3);
				resourceToAnimation.transform.position = this.resources[2].resourceObject.transform.position;
			}
			else if (type == ResourceType.combatCard)
			{
				resourceToAnimation = ResourcesObjectPoolScript.Instance.GetPooledObject(7);
				resourceToAnimation.transform.position = this.resources[2].resourceObject.transform.position;
				resourceToAnimation.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
			}
			resourceToAnimation.SetActive(true);
			float num = (float)id * 0.5f;
			resourceToAnimation.transform.DOMoveY(0f, num, false).SetEase(Ease.InExpo).OnComplete(delegate
			{
				this.SubtractResourceAnimation(resourceToAnimation, type, enemyAction, amountToProduce);
			});
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00088FC0 File Offset: 0x000871C0
		private void SubtractResourceAnimation(GameObject resourceToAnimation, ResourceType type, bool enemyAction = false, int amountToProduce = 0)
		{
			if (enemyAction)
			{
				this.OneResourceUpdate(type, amountToProduce, false);
			}
			resourceToAnimation.transform.DOMoveY(2f, 0.5f, false).SetEase(Ease.InExpo).OnComplete(delegate
			{
				this.PayResourceAnimationEnd(resourceToAnimation, type, enemyAction, amountToProduce);
			});
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00089048 File Offset: 0x00087248
		public void SpawnProduceResources(ResourceType type, bool enemyAction = false, int amountToProduce = 0)
		{
			GameObject resourceToAnimation = null;
			if (type == ResourceType.metal)
			{
				resourceToAnimation = ResourcesObjectPoolScript.Instance.GetPooledObject(0);
				resourceToAnimation.transform.position = new Vector3(this.resources[1].resourceObject.transform.position.x, this.resources[1].resourceObject.transform.position.y + 2f, this.resources[1].resourceObject.transform.position.z);
			}
			else if (type == ResourceType.wood)
			{
				resourceToAnimation = ResourcesObjectPoolScript.Instance.GetPooledObject(1);
				resourceToAnimation.transform.position = new Vector3(this.resources[3].resourceObject.transform.position.x, this.resources[3].resourceObject.transform.position.y + 2f, this.resources[3].resourceObject.transform.position.z);
			}
			else if (type == ResourceType.oil)
			{
				resourceToAnimation = ResourcesObjectPoolScript.Instance.GetPooledObject(2);
				resourceToAnimation.transform.position = new Vector3(this.resources[0].resourceObject.transform.position.x, this.resources[0].resourceObject.transform.position.y + 2f, this.resources[0].resourceObject.transform.position.z);
			}
			else if (type == ResourceType.food)
			{
				resourceToAnimation = ResourcesObjectPoolScript.Instance.GetPooledObject(3);
				resourceToAnimation.transform.position = new Vector3(this.resources[2].resourceObject.transform.position.x, this.resources[2].resourceObject.transform.position.y + 2f, this.resources[2].resourceObject.transform.position.z);
			}
			resourceToAnimation.SetActive(true);
			resourceToAnimation.transform.DOMoveY(0f, 0.5f, false).SetEase(Ease.InExpo).OnComplete(delegate
			{
				this.SpawnResourceAnimationEnd(resourceToAnimation, type, enemyAction, amountToProduce);
			})
				.SetDelay(global::UnityEngine.Random.Range(0.05f, 0.25f));
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00089310 File Offset: 0x00087510
		private void OneResourceUpdate(ResourceType resourceToUpdate, int amountToProduce, bool gainUpdate)
		{
			int num = 0;
			switch (resourceToUpdate)
			{
			case ResourceType.oil:
				num = 0;
				break;
			case ResourceType.metal:
				num = 1;
				break;
			case ResourceType.food:
				num = 2;
				break;
			case ResourceType.wood:
				num = 3;
				break;
			}
			GameHex gameHexLogic = this.GetGameHexLogic();
			int num2 = amountToProduce;
			if (gainUpdate)
			{
				if (gameHexLogic.snapshotResourcesAfterTopAction.ContainsKey(resourceToUpdate))
				{
					num2 = gameHexLogic.snapshotResourcesAfterTopAction[resourceToUpdate];
				}
				else
				{
					num2 = 0;
				}
			}
			else if (PlatformManager.IsStandalone)
			{
				if (gameHexLogic.resources.ContainsKey(resourceToUpdate))
				{
					num2 = gameHexLogic.resources[resourceToUpdate];
				}
				else
				{
					num2 = 0;
				}
			}
			this.resources[num].resourceObject.GetComponent<ResourcePresenter>().UpdateTempResource(num2);
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x000893B0 File Offset: 0x000875B0
		private void SpawnResourceAnimationEnd(GameObject animationObject, ResourceType type, bool enemyAction, int amountToProduce = 0)
		{
			switch (type)
			{
			case ResourceType.oil:
				WorldSFXManager.PlaySound(SoundEnum.ProduceOil, AudioSourceType.WorldSfx);
				break;
			case ResourceType.metal:
				WorldSFXManager.PlaySound(SoundEnum.ProduceIron, AudioSourceType.WorldSfx);
				break;
			case ResourceType.food:
				WorldSFXManager.PlaySound(SoundEnum.ProduceGrain, AudioSourceType.WorldSfx);
				break;
			case ResourceType.wood:
				WorldSFXManager.PlaySound(SoundEnum.ProduceWood, AudioSourceType.WorldSfx);
				break;
			}
			animationObject.SetActive(false);
			this.spawnResourceIndex--;
			if (enemyAction && this.spawnResourceIndex == 0)
			{
				this.GetGameHexLogic().skipTopActionPresentationUpdate = false;
				this.OneResourceUpdate(type, amountToProduce, true);
				ShowEnemyMoves.Instance.SetAnimationInProgress(false);
				ShowEnemyMoves.Instance.GetNextAnimation();
			}
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00089448 File Offset: 0x00087648
		private void PayResourceAnimationEnd(GameObject animationObject, ResourceType type, bool enemyAction, int amountToProduce = 0)
		{
			animationObject.SetActive(false);
			this.payResourceIndex--;
			if (type == ResourceType.combatCard)
			{
				animationObject.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
			}
			if (enemyAction && this.payResourceIndex == 0)
			{
				this.GetGameHexLogic().skipDownActionPresentationUpdate = false;
				this.OneResourceUpdate(type, amountToProduce, false);
				ShowEnemyMoves.Instance.SetAnimationInProgress(false);
				ShowEnemyMoves.Instance.GetNextAnimation();
			}
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x000894D0 File Offset: 0x000876D0
		public string InfoBasic()
		{
			if (this.hexType == HexType.capital)
			{
				return "TerrainCapital" + this.GetGameHexLogic().factionBase.ToString() + "Basic";
			}
			return "Terrain" + this.hexType.ToString().Substring(0, 1).ToUpper() + this.hexType.ToString().Substring(1) + "Basic";
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00089550 File Offset: 0x00087750
		public string InfoAdv()
		{
			return "Terrain" + this.hexType.ToString().Substring(0, 1).ToUpper() + this.hexType.ToString().Substring(1) + "Adv";
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x000895A0 File Offset: 0x000877A0
		public void ShowResourceAnimAfterMove(ResourceType type, Vector3 unitPosition, int amount)
		{
			Vector3 zero = Vector3.zero;
			switch (type)
			{
			case ResourceType.oil:
			{
				Vector3 vector = this.resources[0].resourceObject.transform.position;
				break;
			}
			case ResourceType.metal:
			{
				Vector3 vector2 = this.resources[1].resourceObject.transform.position;
				break;
			}
			case ResourceType.food:
			{
				Vector3 vector3 = this.resources[2].resourceObject.transform.position;
				break;
			}
			case ResourceType.wood:
			{
				Vector3 vector4 = this.resources[3].resourceObject.transform.position;
				break;
			}
			}
			this.GetResourcePresenter(type).MoveResource(amount, true, unitPosition, false);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x000312CF File Offset: 0x0002F4CF
		public void ExchangeShowResourceMove(ResourceType type, Vector3 unitPosition, bool fromUnit, int amount, bool lastAnimation = false)
		{
			this.GetResourcePresenter(type).MoveResource(amount, fromUnit, unitPosition, lastAnimation);
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x000312E3 File Offset: 0x0002F4E3
		public void ForceResourceModelToFinishMove()
		{
			this.GetResourcePresenter(ResourceType.oil).ForceResourceModelToFinishMove();
			this.GetResourcePresenter(ResourceType.metal).ForceResourceModelToFinishMove();
			this.GetResourcePresenter(ResourceType.food).ForceResourceModelToFinishMove();
			this.GetResourcePresenter(ResourceType.wood).ForceResourceModelToFinishMove();
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00031315 File Offset: 0x0002F515
		private float Map(float value, float sourceMin, float sourceMax, float destinMin, float destinMax)
		{
			return (value - sourceMin) / (sourceMax - sourceMin) * (destinMax - destinMin) + destinMin;
		}

		// Token: 0x06000E1D RID: 3613
		public abstract void UpdateFromLogic(Dictionary<Faction, GameController.FactionInfo> factionInfo);

		// Token: 0x06000E1E RID: 3614
		public abstract void UpdateOwnership();

		// Token: 0x06000E1F RID: 3615
		public abstract void UpdateTokenLogic();

		// Token: 0x06000E20 RID: 3616
		public abstract void UpdateTokenState(Unit unit);

		// Token: 0x06000E21 RID: 3617
		public abstract void SetFocus(bool hasFocus, HexMarkers.MarkerType focusType = HexMarkers.MarkerType.FieldSelected, float animationTime = 0f, bool characterSelected = false);

		// Token: 0x06000E22 RID: 3618
		public abstract void ActivateEncounterWaitAnimation();

		// Token: 0x06000E23 RID: 3619
		public abstract void ActivateEncounterEndAnimation();

		// Token: 0x06000E24 RID: 3620
		public abstract void BreakWaitAnimation();

		// Token: 0x06000E25 RID: 3621 RVA: 0x00089644 File Offset: 0x00087844
		public GameHexPresenter.HexResource GetHexResourceData(ResourceType resource)
		{
			return this.resources.First((GameHexPresenter.HexResource x) => x.resourceType == resource);
		}

		// Token: 0x04000AFC RID: 2812
		public static float hexRadius = 2f;

		// Token: 0x04000AFD RID: 2813
		public Vector3i position;

		// Token: 0x04000AFE RID: 2814
		public Transform tunnelTransform;

		// Token: 0x04000AFF RID: 2815
		public GameObject fadedEncounterMarker;

		// Token: 0x04000B00 RID: 2816
		[NonSerialized]
		public HexType hexType;

		// Token: 0x04000B01 RID: 2817
		[NonSerialized]
		public Faction factionBase;

		// Token: 0x04000B02 RID: 2818
		[NonSerialized]
		public bool hasEncounter;

		// Token: 0x04000B03 RID: 2819
		[NonSerialized]
		public bool hasTunnel;

		// Token: 0x04000B04 RID: 2820
		[NonSerialized]
		public bool hasFocus;

		// Token: 0x04000B05 RID: 2821
		[NonSerialized]
		public GameHexPresenter.HexResource[] resources;

		// Token: 0x04000B06 RID: 2822
		[NonSerialized]
		public GameObject building;

		// Token: 0x04000B07 RID: 2823
		[NonSerialized]
		public GameObject token;

		// Token: 0x04000B08 RID: 2824
		[NonSerialized]
		public GameObject tunnel;

		// Token: 0x04000B09 RID: 2825
		[NonSerialized]
		public GameObject encounter;

		// Token: 0x04000B0A RID: 2826
		[NonSerialized]
		public GameObject hexTemplate;

		// Token: 0x04000B0B RID: 2827
		[NonSerialized]
		public StrategicView hexStrategicView;

		// Token: 0x04000B0C RID: 2828
		[NonSerialized]
		public bool[] ownerWorkersPositionsOccupied = new bool[8];

		// Token: 0x04000B0D RID: 2829
		[NonSerialized]
		public bool[] enemyWorkersPositionsOccupied = new bool[8];

		// Token: 0x04000B0E RID: 2830
		[NonSerialized]
		public bool[] ownerMechsPositionsOccupied = new bool[4];

		// Token: 0x04000B0F RID: 2831
		[NonSerialized]
		public bool[] enemyMechsPositionsOccupied = new bool[4];

		// Token: 0x04000B10 RID: 2832
		private int spawnResourceIndex;

		// Token: 0x04000B11 RID: 2833
		private int payResourceIndex;

		// Token: 0x020001DE RID: 478
		[Serializable]
		public class HexResource
		{
			// Token: 0x04000B12 RID: 2834
			public ResourceType resourceType;

			// Token: 0x04000B13 RID: 2835
			public GameObject resourceObject;

			// Token: 0x04000B14 RID: 2836
			public TextMeshProUGUI resourceCountInfo;

			// Token: 0x04000B15 RID: 2837
			public MeshRenderer resourceOutlineMobile;

			// Token: 0x04000B16 RID: 2838
			public Outline resourceOutlineStandalone;
		}
	}
}

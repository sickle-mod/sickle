using System;
using System.Collections.Generic;
using DG.Tweening;
using HoneyFramework;
using Scythe.GameLogic;
using Scythe.UI;
using TMPro;
using UnityEngine;

namespace Scythe.BoardPresenter
{
	// Token: 0x020001D6 RID: 470
	public class FlatGameHexPresenter : GameHexPresenter
	{
		// Token: 0x06000D87 RID: 3463 RVA: 0x00085C98 File Offset: 0x00083E98
		public override void InitScytheValues()
		{
			GameHex gameHexLogic = FlatGameHexPresenter.GetGameHexLogic(this.position);
			if (gameHexLogic == null)
			{
				return;
			}
			this.hexType = gameHexLogic.hexType;
			this.hasEncounter = gameHexLogic.hasEncounter;
			this.hasTunnel = gameHexLogic.hasTunnel;
			this.factionBase = gameHexLogic.factionBase;
			this.SetMarkerSprites();
			this.SetStrategicView();
			this.CreateBuilding(gameHexLogic.Building);
			this.CreateToken(gameHexLogic.Token);
			this.SetResources();
			this.SetTunnel();
			this.SetEncounter();
			this.SetCapital();
			this.SetFactory();
			this.SetVillage();
			this.SetForest();
			this.SetTundra();
			this.SetMountain();
			this.SetFarm();
			this.SetLake();
			if (this.hasEncounter)
			{
				this.BreakWaitAnimation();
			}
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00085D5C File Offset: 0x00083F5C
		protected override void CreateBuilding(Building building)
		{
			this.building = global::UnityEngine.Object.Instantiate<GameObject>(GameController.Instance.gameBoardPresenter.positionsTemplate.building, GameController.Instance.gameBoardPresenter.transform);
			this.building.gameObject.SetActive(false);
			this.building.GetComponent<BuildingPresenter>().buildingLogical = building;
			this.building.transform.position = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.building.transform.position;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00085DF4 File Offset: 0x00083FF4
		protected override void CreateToken(FactionAbilityToken token)
		{
			this.token = global::UnityEngine.Object.Instantiate<GameObject>(GameController.Instance.gameBoardPresenter.positionsTemplate.token, GameController.Instance.gameBoardPresenter.transform);
			this.token.gameObject.SetActive(false);
			this.token.GetComponent<TokenPresenter>().SetTokenLogic(token);
			this.token.transform.position = this.GetTokenPosition();
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00085E68 File Offset: 0x00084068
		protected void SetStrategicView()
		{
			this.hexStrategicView = global::UnityEngine.Object.Instantiate<StrategicView>(GameController.Instance.gameBoardPresenter.positionsTemplate.strategicView, GameController.Instance.gameBoardPresenter.transform);
			this.hexStrategicView.transform.position = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.strategicView.transform.position;
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00085EDC File Offset: 0x000840DC
		public override Vector3 GetUnitPosition(Unit unit)
		{
			Vector3 vector;
			switch (unit.UnitType)
			{
			case UnitType.Character:
				if (unit.Owner == this.GetGameHexLogic().Owner || this.GetGameHexLogic().Owner == null)
				{
					vector = this.CharacterPosition();
				}
				else
				{
					vector = this.EnemyCharacterPosition();
				}
				vector.y = -0.05f;
				break;
			case UnitType.Mech:
				if (unit.Owner == this.GetGameHexLogic().Owner || this.GetGameHexLogic().Owner == null)
				{
					vector = this.MechPosition(unit.IndexOnHex);
				}
				else
				{
					vector = this.EnemyMechPosition(unit.IndexOnHex);
				}
				vector.y = -0.05f;
				break;
			case UnitType.Worker:
				if (unit.Owner == this.GetGameHexLogic().Owner || this.GetGameHexLogic().Owner == null || this.GetGameHexLogic().GetOwnerUnitCount() == this.GetGameHexLogic().GetOwnerWorkers().Count)
				{
					vector = this.WorkerPosition(unit.IndexOnHex);
				}
				else
				{
					vector = this.EnemyWorkerPosition(unit.IndexOnHex);
				}
				vector.y = -0.06f;
				break;
			default:
				vector = HexCoordinates.HexToWorld3D(this.position);
				break;
			}
			return vector;
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x00086008 File Offset: 0x00084208
		public override Vector3 GetEnemyUnitPosition(Unit unit)
		{
			Vector3 vector;
			switch (unit.UnitType)
			{
			case UnitType.Character:
				vector = this.EnemyCharacterPosition();
				break;
			case UnitType.Mech:
				vector = this.EnemyMechPosition(unit.IndexOnHex);
				break;
			case UnitType.Worker:
				vector = this.EnemyWorkerPosition(unit.IndexOnHex);
				break;
			default:
				vector = HexCoordinates.HexToWorld3D(this.position);
				break;
			}
			vector.y = -0.05f;
			return vector;
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x00086070 File Offset: 0x00084270
		public override Vector3 CharacterPosition()
		{
			Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.characterObject.transform.position;
			return this.UpdateHeight(vector);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x000860B0 File Offset: 0x000842B0
		public override Vector3 EnemyCharacterPosition()
		{
			Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.characterObjectEnemy.transform.position;
			return this.UpdateHeight(vector);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x000860F0 File Offset: 0x000842F0
		public override Vector3 MechPosition(int i)
		{
			Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.mechObjects[i].transform.position;
			return this.UpdateHeight(vector);
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00086134 File Offset: 0x00084334
		public override Vector3 EnemyMechPosition(int i)
		{
			Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.mechObjectsEnemy[i].transform.position;
			return this.UpdateHeight(vector);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00086178 File Offset: 0x00084378
		public override Vector3 WorkerPosition(int i)
		{
			Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.workerObjects[i].transform.position;
			return this.UpdateHeight(vector);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x000861BC File Offset: 0x000843BC
		public override Vector3 EnemyWorkerPosition(int i)
		{
			Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.workerObjectsEnemy[i].transform.position;
			return this.UpdateHeight(vector);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00030F4C File Offset: 0x0002F14C
		protected override Vector3 UpdateHeight(Vector3 pos)
		{
			if (this.GetGameHexLogic() != null)
			{
				pos.y = 0f;
			}
			return pos;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00030F63 File Offset: 0x0002F163
		public override Vector3 GetTokenPosition()
		{
			return this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.token.transform.position;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00086200 File Offset: 0x00084400
		protected void SetMarkerSprites()
		{
			this.layerGroup = new GameObject("LayerGroup");
			this.layerGroup.transform.transform.parent = this.hexObject.transform;
			this.layerGroup.transform.localPosition = new Vector3(-0.05f, 0f, 0f);
			GameObject gameObject = new GameObject("Owner");
			GameObject gameObject2 = new GameObject("Hoover");
			GameObject gameObject3 = new GameObject("Conflict");
			GameObject gameObject4 = new GameObject("Action");
			gameObject.transform.parent = this.layerGroup.transform;
			gameObject2.transform.parent = this.layerGroup.transform;
			gameObject3.transform.parent = this.layerGroup.transform;
			gameObject4.transform.parent = this.layerGroup.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject3.transform.localPosition = Vector3.zero;
			gameObject4.transform.localPosition = Vector3.zero;
			gameObject.AddComponent<SpriteRenderer>();
			gameObject2.AddComponent<SpriteRenderer>();
			gameObject3.AddComponent<SpriteRenderer>();
			gameObject4.AddComponent<SpriteRenderer>();
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
			gameObject2.GetComponent<SpriteRenderer>().sortingOrder = 5;
			gameObject3.GetComponent<SpriteRenderer>().sortingOrder = 4;
			gameObject4.GetComponent<SpriteRenderer>().sortingOrder = 3;
			if (this.hexType == HexType.capital)
			{
				gameObject.SetActive(false);
			}
			this.SetFocus(false, HexMarkers.MarkerType.None, 0f, false);
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00030F8E File Offset: 0x0002F18E
		public void CorrectLayerGroupAngles()
		{
			this.layerGroup.transform.eulerAngles = new Vector3(90f, 90f, 90f);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00086390 File Offset: 0x00084590
		protected override void SetResources()
		{
			this.resources = new GameHexPresenter.HexResource[4];
			for (int i = 0; i < 4; i++)
			{
				this.resources[i] = new GameHexPresenter.HexResource();
				this.resources[i].resourceObject = global::UnityEngine.Object.Instantiate<GameObject>(GameController.Instance.gameBoardPresenter.positionsTemplate.resources[i], GameController.Instance.gameBoardPresenter.transform);
				this.resources[i].resourceObject.GetComponent<ResourcePresenter>().hex = this;
				if (PlatformManager.IsMobile)
				{
					this.resources[i].resourceOutlineMobile = this.resources[i].resourceObject.GetComponent<ResourcePresenter>().resourceOutlineMobile;
				}
				else
				{
					this.resources[i].resourceOutlineStandalone = this.resources[i].resourceObject.GetComponent<ResourcePresenter>().ResourceOutlineStandalone;
				}
				Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.resources[i].transform.position;
				this.resources[i].resourceObject.transform.position = vector;
				this.resources[i].resourceCountInfo = this.resources[i].resourceObject.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
				this.resources[i].resourceType = (ResourceType)i;
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x000864EC File Offset: 0x000846EC
		protected override void SetTunnel()
		{
			if (!this.hasTunnel)
			{
				return;
			}
			GameObject gameObject = new GameObject("Board Tunnel");
			gameObject.transform.parent = this.hexObject;
			gameObject.transform.localPosition = new Vector3(-0.055f, 0f, 0f);
			gameObject.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
			GameObject gameObject2 = new GameObject("Tunnel Outline");
			GameObject gameObject3 = new GameObject("Tunnel Icon");
			gameObject2.transform.parent = gameObject.transform;
			gameObject3.transform.parent = gameObject.transform;
			gameObject2.transform.localPosition = new Vector3(0.022f, -0.044f, 0f);
			gameObject3.transform.localPosition = new Vector3(0f, 1f, 0f);
			gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 30f);
			gameObject3.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject3.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
			gameObject2.AddComponent<SpriteRenderer>();
			gameObject3.AddComponent<SpriteRenderer>();
			gameObject2.GetComponent<SpriteRenderer>().sortingOrder = -1;
			gameObject3.GetComponent<SpriteRenderer>().sortingOrder = -1;
			gameObject3.GetComponent<SpriteRenderer>().sprite = ((FlatWorld)GameController.Instance.gameBoardPresenter).tunnelIcon;
			this.tunnelTransform = gameObject3.transform.parent.transform;
			gameObject2.GetComponent<SpriteRenderer>().color = new Color32(157, 27, 60, byte.MaxValue);
			Transform component = global::UnityEngine.Object.Instantiate<GameObject>(GameController.Instance.gameBoardPresenter.positionsTemplate.tunnel, GameController.Instance.gameBoardPresenter.transform).GetComponent<Transform>();
			Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.encounter.transform.position;
			component.transform.position = vector;
			component.gameObject.SetActive(true);
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00086738 File Offset: 0x00084938
		protected override void SetEncounter()
		{
			if (!this.hasEncounter)
			{
				return;
			}
			this.encounter = global::UnityEngine.Object.Instantiate<GameObject>(GameController.Instance.gameBoardPresenter.positionsTemplate.encounter, GameController.Instance.gameBoardPresenter.transform);
			Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.encounter.transform.position;
			this.encounter.transform.position = vector;
			if (!this.GetGameHexLogic().encounterTaken)
			{
				this.encounter.gameObject.SetActive(true);
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x000867D8 File Offset: 0x000849D8
		protected override void SetFactory()
		{
			if (this.hexType == HexType.factory)
			{
				this.hexTemplate = global::UnityEngine.Object.Instantiate<GameObject>(GameController.Instance.gameBoardPresenter.positionsTemplate.factory, GameController.Instance.gameBoardPresenter.transform);
				Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.factory.transform.position;
				this.hexTemplate.transform.position = vector;
				this.hexTemplate.SetActive(true);
			}
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00086864 File Offset: 0x00084A64
		protected override void SetVillage()
		{
			if (this.hexType == HexType.village)
			{
				this.hexTemplate = global::UnityEngine.Object.Instantiate<GameObject>(GameController.Instance.gameBoardPresenter.positionsTemplate.village, GameController.Instance.gameBoardPresenter.transform);
				Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.village.transform.position;
				this.hexTemplate.transform.position = vector;
				this.hexTemplate.SetActive(true);
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void SetCapital()
		{
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x000868F0 File Offset: 0x00084AF0
		protected override void SetForest()
		{
			if (this.hexType == HexType.forest)
			{
				this.hexTemplate = global::UnityEngine.Object.Instantiate<GameObject>(GameController.Instance.gameBoardPresenter.positionsTemplate.forest, GameController.Instance.gameBoardPresenter.transform);
				Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.forest.transform.position;
				this.hexTemplate.transform.position = vector;
				this.hexTemplate.SetActive(true);
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0008697C File Offset: 0x00084B7C
		protected override void SetTundra()
		{
			if (this.hexType == HexType.tundra)
			{
				this.hexTemplate = global::UnityEngine.Object.Instantiate<GameObject>(GameController.Instance.gameBoardPresenter.positionsTemplate.tundra, GameController.Instance.gameBoardPresenter.transform);
				Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.tundra.transform.position;
				this.hexTemplate.transform.position = vector;
				this.hexTemplate.SetActive(true);
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00086A08 File Offset: 0x00084C08
		protected override void SetMountain()
		{
			if (this.hexType == HexType.mountain)
			{
				this.hexTemplate = global::UnityEngine.Object.Instantiate<GameObject>(GameController.Instance.gameBoardPresenter.positionsTemplate.mountain, GameController.Instance.gameBoardPresenter.transform);
				Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.mountain.transform.position;
				this.hexTemplate.transform.position = vector;
				this.hexTemplate.SetActive(true);
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00086A94 File Offset: 0x00084C94
		protected override void SetFarm()
		{
			if (this.hexType == HexType.farm)
			{
				this.hexTemplate = global::UnityEngine.Object.Instantiate<GameObject>(GameController.Instance.gameBoardPresenter.positionsTemplate.farm, GameController.Instance.gameBoardPresenter.transform);
				Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.farm.transform.position;
				this.hexTemplate.transform.position = vector;
				this.hexTemplate.SetActive(true);
			}
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00086B20 File Offset: 0x00084D20
		protected override void SetLake()
		{
			if (this.hexType == HexType.lake)
			{
				this.hexTemplate = global::UnityEngine.Object.Instantiate<GameObject>(GameController.Instance.gameBoardPresenter.positionsTemplate.lake, GameController.Instance.gameBoardPresenter.transform);
				Vector3 vector = this.worldPosition + GameController.Instance.gameBoardPresenter.positionsTemplate.lake.transform.position;
				this.hexTemplate.transform.position = vector;
				this.hexTemplate.SetActive(true);
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00030FB4 File Offset: 0x0002F1B4
		public override GameHex GetGameHexLogic()
		{
			return FlatGameHexPresenter.GetGameHexLogic(this.position);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00084934 File Offset: 0x00082B34
		public static GameHex GetGameHexLogic(Vector3i position)
		{
			int num = 4 - position.x;
			int num2 = position.z - position.y;
			int num3 = 3 + ((num2 % 2 == 0) ? num2 : (num2 + 1)) / 2;
			if (num3 < 8 && num3 >= 0 && num < 9 && num >= 0)
			{
				return GameController.GameManager.gameBoard.hexMap[num3, num];
			}
			return null;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00030FC1 File Offset: 0x0002F1C1
		public override Vector2 GetWorldPosition()
		{
			return new Vector2(this.worldPosition.x, this.worldPosition.z);
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00086BAC File Offset: 0x00084DAC
		public override void UpdateFromLogic(Dictionary<Faction, GameController.FactionInfo> factionInfo)
		{
			GameHex gameHexLogic = this.GetGameHexLogic();
			if (gameHexLogic == null)
			{
				return;
			}
			this.hexStrategicView.UpdateFromLogic(gameHexLogic);
			if (this.building != null)
			{
				this.building.GetComponent<BuildingPresenter>().UpdateBuilding(gameHexLogic.Building, factionInfo, this);
			}
			if (this.resources != null && !gameHexLogic.skipTopActionPresentationUpdate && !gameHexLogic.skipDownActionPresentationUpdate)
			{
				base.GetResourcePresenter(ResourceType.oil).UpdateTempResource(gameHexLogic.resources[ResourceType.oil]);
				base.GetResourcePresenter(ResourceType.metal).UpdateTempResource(gameHexLogic.resources[ResourceType.metal]);
				base.GetResourcePresenter(ResourceType.food).UpdateTempResource(gameHexLogic.resources[ResourceType.food]);
				base.GetResourcePresenter(ResourceType.wood).UpdateTempResource(gameHexLogic.resources[ResourceType.wood]);
			}
			if (this.hasEncounter && GameController.Instance.GameIsLoaded)
			{
				if (!gameHexLogic.encounterTaken)
				{
					this.encounter.transform.GetChild(0).gameObject.SetActive(true);
					if (GameController.GameManager.GameLoading)
					{
						this.BreakWaitAnimation();
					}
				}
				else
				{
					this.encounter.transform.GetChild(0).gameObject.SetActive(false);
				}
			}
			this.UpdateOwnership();
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00086CDC File Offset: 0x00084EDC
		public override void UpdateOwnership()
		{
			GameHex gameHexLogic = this.GetGameHexLogic();
			if (gameHexLogic == null)
			{
				return;
			}
			if (gameHexLogic.Owner != null && gameHexLogic.hexType != HexType.capital)
			{
				switch (gameHexLogic.Owner.matFaction.faction)
				{
				case Faction.Polania:
					this.SetFocus(true, HexMarkers.MarkerType.OwnerPolania, 0f, false);
					break;
				case Faction.Albion:
					this.SetFocus(true, HexMarkers.MarkerType.OwnerAlbion, 0f, false);
					break;
				case Faction.Nordic:
					this.SetFocus(true, HexMarkers.MarkerType.OwnerNords, 0f, false);
					break;
				case Faction.Rusviet:
					this.SetFocus(true, HexMarkers.MarkerType.OwnerRusviet, 0f, false);
					break;
				case Faction.Togawa:
					this.SetFocus(true, HexMarkers.MarkerType.OwnerTogawa, 0f, false);
					break;
				case Faction.Crimea:
					this.SetFocus(true, HexMarkers.MarkerType.OwnerCrimea, 0f, false);
					break;
				case Faction.Saxony:
					this.SetFocus(true, HexMarkers.MarkerType.OwnerSaxony, 0f, false);
					break;
				}
			}
			else
			{
				this.SetFocus(true, HexMarkers.MarkerType.OwnerNone, 0f, false);
			}
			if (gameHexLogic.Owner != null && gameHexLogic.Enemy != null)
			{
				if (GameController.GameManager.combatManager.GetActualStage() != CombatStage.EndingTheBattle)
				{
					this.SetFocus(true, HexMarkers.MarkerType.Battle, 0f, false);
					return;
				}
			}
			else
			{
				this.SetFocus(false, HexMarkers.MarkerType.Battle, 0f, false);
			}
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0003087E File Offset: 0x0002EA7E
		public override void UpdateTokenLogic()
		{
			this.token.GetComponent<TokenPresenter>().SetTokenLogic(this.GetGameHexLogic().Token);
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0003089B File Offset: 0x0002EA9B
		public override void UpdateTokenState(Unit unit)
		{
			this.token.GetComponent<TokenPresenter>().UpdatePresenter(unit);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00030FDE File Offset: 0x0002F1DE
		public void SetColliderEnabled(bool enabled)
		{
			if (this.hexCollider == null)
			{
				this.hexCollider = this.hexObject.GetComponent<Collider>();
			}
			if (this.hexCollider != null)
			{
				this.hexCollider.enabled = enabled;
			}
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00031019 File Offset: 0x0002F219
		private void SaveEncounterAnimationState(GameHex hex)
		{
			hex.encounterAnimated = true;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00086E08 File Offset: 0x00085008
		private void OffEncounterAfterAnimation()
		{
			this.encounter.transform.GetChild(0).gameObject.SetActive(false);
			this.encounter.transform.position = new Vector3(this.encounter.transform.position.x, -0.01304357f, this.encounter.transform.position.z);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00031022 File Offset: 0x0002F222
		private void ResourceFirstAnimationEnd(GameHexPresenter.HexResource resource)
		{
			resource.resourceCountInfo.gameObject.SetActive(true);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00031035 File Offset: 0x0002F235
		private void ResourceAnimationEnd(GameObject animationObject, GameHexPresenter.HexResource resource, int resCount)
		{
			animationObject.SetActive(false);
			resource.resourceCountInfo.text = resCount.ToString();
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00086E78 File Offset: 0x00085078
		public override void SetFocus(bool hasFocus, HexMarkers.MarkerType focusType = HexMarkers.MarkerType.FieldSelected, float animationTime = 0f, bool characterSelected = false)
		{
			if (animationTime == 0f)
			{
				if (hasFocus)
				{
					FlatHexMarkers.SetMarkerType(this.layerGroup, focusType);
				}
				else
				{
					FlatHexMarkers.ClearMarkerType(this.layerGroup, focusType);
				}
			}
			else if (hasFocus)
			{
				if (focusType == HexMarkers.MarkerType.DeployTrade)
				{
					WorldSFXManager.PlaySound(SoundEnum.GlowLoop, AudioSourceType.Loops);
				}
				FlatHexMarkers.Instance.FadeInAnimation(this.layerGroup, focusType, animationTime);
			}
			else
			{
				FlatHexMarkers.Instance.FadeOutAnimation(this.layerGroup, focusType, animationTime);
				WorldSFXManager.StopLoopSFX();
			}
			this.PlayEncounterParticles(hasFocus, focusType, characterSelected);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00086EF0 File Offset: 0x000850F0
		private void PlayEncounterParticles(bool hasFocus, HexMarkers.MarkerType focusType = HexMarkers.MarkerType.FieldSelected, bool characterSelected = false)
		{
			if (this.encounter != null && focusType != HexMarkers.MarkerType.Hoover)
			{
				if (hasFocus && characterSelected && this.GetGameHexLogic().hasEncounter && !this.GetGameHexLogic().encounterUsed && (focusType == HexMarkers.MarkerType.MoveToEncounter || focusType == HexMarkers.MarkerType.MoveToEnemy))
				{
					this.encounter.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
					return;
				}
				this.encounter.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
			}
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00086F70 File Offset: 0x00085170
		public override void ActivateEncounterWaitAnimation()
		{
			if (this.encounter.transform.GetChild(0).gameObject.GetComponent<Animator>().isInitialized)
			{
				this.encounter.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("WaitForEncounter", true);
			}
			EncounterCardPresenter.encounterEnd += this.EncounterWaitAnimationEnd;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00086FD8 File Offset: 0x000851D8
		public override void BreakWaitAnimation()
		{
			if (this.encounter != null && this.encounter.transform.GetChild(0).gameObject != null && this.encounter.transform.GetChild(0).gameObject.GetComponent<Animator>() != null && this.encounter.transform.GetChild(0).gameObject.GetComponent<Animator>().isInitialized)
			{
				this.encounter.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("WaitForEncounter", false);
			}
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00087080 File Offset: 0x00085280
		private void EncounterWaitAnimationEnd()
		{
			WorldSFXManager.StopLoopSFX();
			EncounterCardPresenter.encounterEnd -= this.EncounterWaitAnimationEnd;
			if (this.encounter.transform.GetChild(0).gameObject.GetComponent<Animator>().isInitialized)
			{
				this.encounter.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("WaitForEncounter", false);
			}
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00031050 File Offset: 0x0002F250
		public override void ActivateEncounterEndAnimation()
		{
			this.encounter.transform.DOMoveY(20f, 1f, false).SetEase(Ease.InExpo).OnComplete(new TweenCallback(this.OffEncounterAfterAnimation));
		}

		// Token: 0x04000ACF RID: 2767
		public Transform hexObject;

		// Token: 0x04000AD0 RID: 2768
		public Vector3 worldPosition = Vector3.zero;

		// Token: 0x04000AD1 RID: 2769
		public GameObject layerGroup;

		// Token: 0x04000AD2 RID: 2770
		private Collider hexCollider;
	}
}

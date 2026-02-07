using System;
using System.Collections.Generic;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;

namespace HoneyFramework
{
	// Token: 0x020001BA RID: 442
	[Serializable]
	public class GameHexPresenter : GameHexPresenter
	{
		// Token: 0x06000CEB RID: 3307 RVA: 0x000844F8 File Offset: 0x000826F8
		public override void InitScytheValues()
		{
			GameHex gameHexLogic = GameHexPresenter.GetGameHexLogic(this.position);
			if (gameHexLogic == null)
			{
				return;
			}
			this.hexType = gameHexLogic.hexType;
			this.hasEncounter = gameHexLogic.hasEncounter;
			this.hasTunnel = gameHexLogic.hasTunnel;
			this.factionBase = gameHexLogic.factionBase;
			this.CreateBuilding(gameHexLogic.Building);
			this.CreateToken(gameHexLogic.Token);
			this.SetResources();
			this.SetTunnel();
			this.SetEncounter();
			this.SetVillage();
			this.SetCapital();
			this.SetForest();
			this.SetTundra();
			this.SetMountain();
			this.SetFarm();
			this.SetLake();
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void CreateBuilding(Building building)
		{
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void CreateToken(FactionAbilityToken token)
		{
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0008459C File Offset: 0x0008279C
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
				break;
			case UnitType.Worker:
				if (unit.Owner == this.GetGameHexLogic().Owner || this.GetGameHexLogic().Owner == null)
				{
					vector = this.WorkerPosition(unit.IndexOnHex);
				}
				else
				{
					vector = this.EnemyWorkerPosition(unit.IndexOnHex);
				}
				break;
			default:
				vector = HexCoordinates.HexToWorld3D(this.position);
				break;
			}
			vector.y -= 0.14f;
			return vector;
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00084698 File Offset: 0x00082898
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
			vector.y -= 0.14f;
			return vector;
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00084704 File Offset: 0x00082904
		public override Vector3 CharacterPosition()
		{
			Vector3 vector = HexCoordinates.HexToWorld3D(this.position) + GameController.Instance.gameBoardPresenter.positionsTemplate.characterObject.transform.position;
			return this.UpdateHeight(vector);
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0008474C File Offset: 0x0008294C
		public override Vector3 EnemyCharacterPosition()
		{
			Vector3 vector = HexCoordinates.HexToWorld3D(this.position) + GameController.Instance.gameBoardPresenter.positionsTemplate.characterObjectEnemy.transform.position;
			return this.UpdateHeight(vector);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00084794 File Offset: 0x00082994
		public override Vector3 MechPosition(int i)
		{
			Vector3 vector = HexCoordinates.HexToWorld3D(this.position) + GameController.Instance.gameBoardPresenter.positionsTemplate.mechObjects[i].transform.position;
			return this.UpdateHeight(vector);
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x000847DC File Offset: 0x000829DC
		public override Vector3 EnemyMechPosition(int i)
		{
			Vector3 vector = HexCoordinates.HexToWorld3D(this.position) + GameController.Instance.gameBoardPresenter.positionsTemplate.mechObjectsEnemy[i].transform.position;
			return this.UpdateHeight(vector);
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00084824 File Offset: 0x00082A24
		public override Vector3 WorkerPosition(int i)
		{
			Vector3 vector = HexCoordinates.HexToWorld3D(this.position) + GameController.Instance.gameBoardPresenter.positionsTemplate.workerObjects[i].transform.position;
			return this.UpdateHeight(vector);
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0008486C File Offset: 0x00082A6C
		public override Vector3 EnemyWorkerPosition(int i)
		{
			Vector3 vector = HexCoordinates.HexToWorld3D(this.position) + GameController.Instance.gameBoardPresenter.positionsTemplate.workerObjectsEnemy[i].transform.position;
			return this.UpdateHeight(vector);
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0002B03D File Offset: 0x0002923D
		protected override Vector3 UpdateHeight(Vector3 pos)
		{
			return pos;
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void SetResources()
		{
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void SetTunnel()
		{
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void SetEncounter()
		{
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void SetVillage()
		{
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void SetCapital()
		{
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void SetForest()
		{
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void SetTundra()
		{
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void SetMountain()
		{
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void SetFarm()
		{
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void SetLake()
		{
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0003082B File Offset: 0x0002EA2B
		public static Vector2 GetDirX()
		{
			if (!GameHexPresenter.dirInitialized)
			{
				GameHexPresenter.InitializeDirections();
			}
			return GameHexPresenter.Xdir;
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0003083E File Offset: 0x0002EA3E
		public static Vector2 GetDirY()
		{
			if (!GameHexPresenter.dirInitialized)
			{
				GameHexPresenter.InitializeDirections();
			}
			return GameHexPresenter.Ydir;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x00030851 File Offset: 0x0002EA51
		public static Vector2 GetDirZ()
		{
			if (!GameHexPresenter.dirInitialized)
			{
				GameHexPresenter.InitializeDirections();
			}
			return GameHexPresenter.Zdir;
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x000848B4 File Offset: 0x00082AB4
		private static void InitializeDirections()
		{
			Quaternion quaternion = Quaternion.Euler(0f, 0f, 120f);
			Quaternion quaternion2 = Quaternion.Euler(0f, 0f, 240f);
			Vector3 vector = new Vector3(GameHexPresenter.hexRadius, 0f, 0f);
			GameHexPresenter.Xdir = vector;
			GameHexPresenter.Ydir = quaternion * vector;
			GameHexPresenter.Zdir = quaternion2 * vector;
			GameHexPresenter.dirInitialized = true;
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00030864 File Offset: 0x0002EA64
		public override Vector2 GetWorldPosition()
		{
			return HexCoordinates.HexToWorld(this.position);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00030871 File Offset: 0x0002EA71
		public override GameHex GetGameHexLogic()
		{
			return GameHexPresenter.GetGameHexLogic(this.position);
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00084934 File Offset: 0x00082B34
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

		// Token: 0x06000D08 RID: 3336 RVA: 0x00084994 File Offset: 0x00082B94
		public override void UpdateFromLogic(Dictionary<Faction, GameController.FactionInfo> factionInfo)
		{
			GameHex gameHexLogic = this.GetGameHexLogic();
			if (gameHexLogic == null)
			{
				return;
			}
			if (this.resources == null || this.building == null)
			{
				return;
			}
			this.building.GetComponent<BuildingPresenter>().UpdateBuilding(gameHexLogic.Building, factionInfo, this);
			base.GetResourcePresenter(ResourceType.oil).UpdateTempResource(gameHexLogic.resources[ResourceType.oil]);
			base.GetResourcePresenter(ResourceType.metal).UpdateTempResource(gameHexLogic.resources[ResourceType.metal]);
			base.GetResourcePresenter(ResourceType.food).UpdateTempResource(gameHexLogic.resources[ResourceType.food]);
			base.GetResourcePresenter(ResourceType.wood).UpdateTempResource(gameHexLogic.resources[ResourceType.wood]);
			if (this.hasEncounter && gameHexLogic.encounterUsed)
			{
				for (int i = 0; i < this.encounter.transform.GetChild(0).childCount; i++)
				{
					this.encounter.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
				}
			}
			this.UpdateOwnership();
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00084A94 File Offset: 0x00082C94
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
				this.SetFocus(true, HexMarkers.MarkerType.Battle, 0f, false);
				return;
			}
			this.SetFocus(false, HexMarkers.MarkerType.Battle, 0f, false);
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0003087E File Offset: 0x0002EA7E
		public override void UpdateTokenLogic()
		{
			this.token.GetComponent<TokenPresenter>().SetTokenLogic(this.GetGameHexLogic().Token);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0003089B File Offset: 0x0002EA9B
		public override void UpdateTokenState(Unit unit)
		{
			this.token.GetComponent<TokenPresenter>().UpdatePresenter(unit);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00084BB0 File Offset: 0x00082DB0
		public override void SetFocus(bool hasFocus, HexMarkers.MarkerType focusType = HexMarkers.MarkerType.FieldSelected, float animationTime = 0f, bool characterSelected = false)
		{
			if (hasFocus)
			{
				HexMarkers.SetMarkerType(this.position, focusType);
			}
			else
			{
				HexMarkers.ClearMarkerType(this.position, focusType);
			}
			if (this.encounter != null)
			{
				if (focusType == HexMarkers.MarkerType.MoveToEncounter && hasFocus)
				{
					this.encounter.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
					return;
				}
				this.encounter.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
			}
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x000308AE File Offset: 0x0002EAAE
		public override Vector3 GetTokenPosition()
		{
			return GameController.Instance.gameBoardPresenter.positionsTemplate.token.transform.position;
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00027EF0 File Offset: 0x000260F0
		public override void ActivateEncounterWaitAnimation()
		{
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00027EF0 File Offset: 0x000260F0
		public override void ActivateEncounterEndAnimation()
		{
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00027EF0 File Offset: 0x000260F0
		public override void BreakWaitAnimation()
		{
		}

		// Token: 0x04000A5E RID: 2654
		protected static Vector2 Xdir;

		// Token: 0x04000A5F RID: 2655
		protected static Vector2 Ydir;

		// Token: 0x04000A60 RID: 2656
		protected static Vector2 Zdir;

		// Token: 0x04000A61 RID: 2657
		protected static bool dirInitialized;
	}
}

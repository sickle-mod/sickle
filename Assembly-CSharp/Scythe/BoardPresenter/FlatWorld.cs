using System;
using System.Collections.Generic;
using HoneyFramework;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;

namespace Scythe.BoardPresenter
{
	// Token: 0x020001DA RID: 474
	public class FlatWorld : GameBoardPresenter
	{
		// Token: 0x06000DD0 RID: 3536 RVA: 0x0003120E File Offset: 0x0002F40E
		public override void Initialize()
		{
			this.status = GameBoardPresenter.Status.PlacingScytheObjects;
			this.GenerateHexMap();
			this.status = GameBoardPresenter.Status.Ready;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00031224 File Offset: 0x0002F424
		public override void InitializeFromSave()
		{
			this.Initialize();
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x000877B0 File Offset: 0x000859B0
		private void GenerateHexMap()
		{
			GameHexPresenter.hexRadius = this.hexRadius;
			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (GameController.GameManager.gameBoard.hexMap[j, i].hexType != HexType.forbidden)
					{
						this.CreateGameHexPresenter(j, i);
					}
				}
			}
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00087808 File Offset: 0x00085A08
		private void CreateGameHexPresenter(int posX, int posY)
		{
			FlatGameHexPresenter flatGameHexPresenter = new FlatGameHexPresenter();
			GameObject gameObject = new GameObject(string.Format("Hex{0}{1}", posX, posY));
			gameObject.transform.position = this.CalculateHexPosition(posX, posY);
			gameObject.transform.parent = this.centerOfTheMap.transform;
			Vector3i vector3i = HexCoordinates.BoardToHexesPosition(posX, posY);
			flatGameHexPresenter.hexTemplate = GameController.Instance.gameBoardPresenter.positionsTemplate.gameObject;
			flatGameHexPresenter.hexObject = gameObject.transform;
			flatGameHexPresenter.position = vector3i;
			flatGameHexPresenter.worldPosition = gameObject.transform.position;
			flatGameHexPresenter.InitScytheValues();
			CylinderCollider cylinderCollider = gameObject.AddComponent<CylinderCollider>();
			cylinderCollider.verticiesForBase = 6;
			cylinderCollider.cylinderWidth = 0.01f;
			cylinderCollider.cylinderRadius = this.hexRadius;
			cylinderCollider.Recalculate();
			gameObject.transform.eulerAngles = new Vector3(0f, 0f, 90f);
			gameObject.layer = LayerMask.NameToLayer("Hex2d");
			flatGameHexPresenter.CorrectLayerGroupAngles();
			this.hexes.Add(vector3i, flatGameHexPresenter);
			this.resourceTypeLayer.AddFlatHex(flatGameHexPresenter);
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00087924 File Offset: 0x00085B24
		private Vector3 CalculateHexPosition(int posX, int posY)
		{
			float num = this.hexRadius * 0.8660254f;
			float num2 = num / Mathf.Sqrt(3f);
			float num3 = 0f;
			if (posY % 2 != 0)
			{
				num3 = num;
			}
			return new Vector3((float)(-(float)(posY - 4)) * (this.hexRadius + num2) + this.centerOfTheMap.transform.position.x, 0f, (float)(-(float)(posX - 3)) * (2f * num) + this.centerOfTheMap.transform.position.z + num3);
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0003122C File Offset: 0x0002F42C
		public override GameHexPresenter GetGameHexPresenter(GameHex hex)
		{
			return this.GetGameHexPresenter(hex.posX, hex.posY);
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x000879AC File Offset: 0x00085BAC
		public override GameHexPresenter GetGameHexPresenter(int posX, int posY)
		{
			Vector3i vector3i = HexCoordinates.BoardToHexesPosition(posX, posY);
			if (this.hexes.ContainsKey(vector3i))
			{
				return this.hexes[vector3i];
			}
			return null;
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x000879E0 File Offset: 0x00085BE0
		public void ClearMarkers()
		{
			foreach (Vector3i vector3i in this.hexes.Keys)
			{
				FlatGameHexPresenter flatGameHexPresenter = this.hexes[vector3i] as FlatGameHexPresenter;
				FlatHexMarkers.ClearMarkerLayer(flatGameHexPresenter.layerGroup, HexMarkers.Layer.Action);
				FlatHexMarkers.ClearMarkerLayer(flatGameHexPresenter.layerGroup, HexMarkers.Layer.Conflict);
				FlatHexMarkers.ClearMarkerLayer(flatGameHexPresenter.layerGroup, HexMarkers.Layer.Hoover);
			}
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00087A68 File Offset: 0x00085C68
		public override void UpdateStaticObjects()
		{
			foreach (KeyValuePair<Vector3i, GameHexPresenter> keyValuePair in this.hexes)
			{
				keyValuePair.Value.UpdateFromLogic(GameController.factionInfo);
			}
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00087AC8 File Offset: 0x00085CC8
		public override void UpdateHexOwnerships()
		{
			foreach (KeyValuePair<Vector3i, GameHexPresenter> keyValuePair in this.hexes)
			{
				keyValuePair.Value.UpdateOwnership();
			}
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00087B20 File Offset: 0x00085D20
		public override void UpdateUnits(bool updateUnits = true)
		{
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				GameController.factionUnits[player.matFaction.faction].LinkUnits();
				UnitPresenter unitPresenter = GameController.GetUnitPresenter(player.character);
				if (unitPresenter != null)
				{
					if (updateUnits)
					{
						unitPresenter.gameObject.SetActive(true);
					}
					if (!player.character.IsMoving())
					{
						unitPresenter.SetHex(this.GetGameHexPresenter(player.character.position));
						if (updateUnits)
						{
							unitPresenter.transform.position = unitPresenter.hex.GetUnitPosition(unitPresenter.UnitLogic);
						}
					}
					else if (updateUnits)
					{
						unitPresenter.ForceFinishMoveAnimation();
					}
				}
				for (int i = 0; i < player.matFaction.mechs.Count; i++)
				{
					unitPresenter = GameController.GetUnitPresenter(player.matFaction.mechs[i]);
					if (unitPresenter != null)
					{
						if (!player.matFaction.mechs[i].IsMoving())
						{
							unitPresenter.SetHex(this.GetGameHexPresenter(player.matFaction.mechs[i].position));
							if (!unitPresenter.UnitLogic.spawnAnimation && updateUnits)
							{
								unitPresenter.transform.position = unitPresenter.hex.GetUnitPosition(unitPresenter.UnitLogic);
							}
							else if (unitPresenter.UnitLogic.spawnAnimation)
							{
								unitPresenter.transform.position = new Vector3(unitPresenter.hex.GetUnitPosition(unitPresenter.UnitLogic).x, unitPresenter.hex.GetUnitPosition(unitPresenter.UnitLogic).y + 2f, unitPresenter.hex.GetUnitPosition(unitPresenter.UnitLogic).z);
							}
						}
						else if (updateUnits)
						{
							unitPresenter.ForceFinishMoveAnimation();
						}
						if (!unitPresenter.gameObject.activeSelf && !unitPresenter.UnitLogic.enemySpawnAnimation && updateUnits)
						{
							unitPresenter.gameObject.SetActive(true);
						}
					}
				}
				for (int j = 0; j < player.matPlayer.workers.Count; j++)
				{
					unitPresenter = GameController.GetUnitPresenter(player.matPlayer.workers[j]);
					if (unitPresenter != null)
					{
						if (!player.matPlayer.workers[j].IsMoving())
						{
							unitPresenter.SetHex(this.GetGameHexPresenter(player.matPlayer.workers[j].position));
							if (!unitPresenter.UnitLogic.spawnAnimation && updateUnits)
							{
								unitPresenter.transform.position = unitPresenter.hex.GetUnitPosition(unitPresenter.UnitLogic);
							}
							else if (unitPresenter.UnitLogic.spawnAnimation)
							{
								unitPresenter.transform.position = new Vector3(unitPresenter.hex.GetUnitPosition(unitPresenter.UnitLogic).x, unitPresenter.hex.GetUnitPosition(unitPresenter.UnitLogic).y + 2f, unitPresenter.hex.GetUnitPosition(unitPresenter.UnitLogic).z);
							}
							if (!unitPresenter.gameObject.activeSelf && !unitPresenter.UnitLogic.enemySpawnAnimation && updateUnits)
							{
								unitPresenter.gameObject.SetActive(true);
							}
						}
						else if (updateUnits)
						{
							unitPresenter.ForceFinishMoveAnimation();
						}
					}
				}
			}
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00087EB4 File Offset: 0x000860B4
		public void ReadRotationFromLogicUnit()
		{
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				GameController.factionUnits[player.matFaction.faction].LinkUnits();
				UnitPresenter unitPresenter = GameController.GetUnitPresenter(player.character);
				if (unitPresenter != null)
				{
					unitPresenter.ReadRotationFromLogicUnit();
				}
				for (int i = 0; i < player.matFaction.mechs.Count; i++)
				{
					unitPresenter = GameController.GetUnitPresenter(player.matFaction.mechs[i]);
					if (unitPresenter != null)
					{
						unitPresenter.ReadRotationFromLogicUnit();
					}
				}
				for (int j = 0; j < player.matPlayer.workers.Count; j++)
				{
					unitPresenter = GameController.GetUnitPresenter(player.matPlayer.workers[j]);
					if (unitPresenter != null)
					{
						unitPresenter.ReadRotationFromLogicUnit();
					}
				}
			}
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00087FC8 File Offset: 0x000861C8
		public override void UpdateTokens()
		{
			foreach (KeyValuePair<Vector3i, GameHexPresenter> keyValuePair in this.hexes)
			{
				keyValuePair.Value.UpdateTokenLogic();
			}
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00088020 File Offset: 0x00086220
		public void RandomizePresentersRotationAfterLoad()
		{
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				GameController.factionUnits[player.matFaction.faction].LinkUnits();
				UnitPresenter unitPresenter = GameController.GetUnitPresenter(player.character);
				if (unitPresenter != null && unitPresenter.gameObject.activeInHierarchy)
				{
					unitPresenter.transform.Rotate(new Vector3(0f, global::UnityEngine.Random.Range(0f, 360f)));
				}
				for (int i = 0; i < player.matFaction.mechs.Count; i++)
				{
					unitPresenter = GameController.GetUnitPresenter(player.matFaction.mechs[i]);
					if (unitPresenter != null && unitPresenter.gameObject.activeInHierarchy)
					{
						unitPresenter.transform.Rotate(new Vector3(0f, global::UnityEngine.Random.Range(0f, 360f)));
					}
				}
			}
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00088144 File Offset: 0x00086344
		public void SetUnitStartingRotations()
		{
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				UnitPresenter unitPresenter = GameController.GetUnitPresenter(player.character);
				if (unitPresenter != null)
				{
					this.SetCharacterStartingRotation(unitPresenter);
				}
			}
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x000881B0 File Offset: 0x000863B0
		private void SetCharacterStartingRotation(UnitPresenter up)
		{
			if (up != null && up.UnitLogic.UnitType != UnitType.Character)
			{
				return;
			}
			Vector3 eulerAngles = up.transform.rotation.eulerAngles;
			switch (up.UnitLogic.Owner.matFaction.faction)
			{
			case Faction.Polania:
				eulerAngles.y = 340f;
				break;
			case Faction.Albion:
			case Faction.Nordic:
				eulerAngles.y = 0f;
				break;
			case Faction.Rusviet:
				eulerAngles.y = 60f;
				break;
			case Faction.Togawa:
				eulerAngles.y = 120f;
				break;
			case Faction.Crimea:
			case Faction.Saxony:
				eulerAngles.y = 240f;
				break;
			}
			up.transform.Rotate(eulerAngles);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00088274 File Offset: 0x00086474
		private void SetWorkerStartingRotation(UnitPresenter up)
		{
			if (up != null && up.UnitLogic.UnitType != UnitType.Worker)
			{
				return;
			}
			Vector3 vector = (GameController.GetUnitPresenter(up.UnitLogic.Owner.character).hex as FlatGameHexPresenter).worldPosition - up.transform.position;
			vector.y = 0f;
			vector = vector.normalized;
			float num = Mathf.Acos(Vector3.Dot(vector, up.transform.right)) * 57.29578f * ((Vector3.Cross(vector, up.transform.right).y >= 0f) ? (-1f) : 1f);
			up.transform.Rotate(new Vector3(0f, num, 0f));
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00088344 File Offset: 0x00086544
		public void ClearUnitPresenters()
		{
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				UnitPresenter unitPresenter = GameController.GetUnitPresenter(player.character);
				if (unitPresenter != null)
				{
					unitPresenter.Clear();
				}
				for (int i = 0; i < player.matFaction.mechs.Count; i++)
				{
					unitPresenter = GameController.GetUnitPresenter(player.matFaction.mechs[i]);
					if (unitPresenter != null)
					{
						unitPresenter.Clear();
					}
				}
				for (int j = 0; j < player.matPlayer.workers.Count; j++)
				{
					unitPresenter = GameController.GetUnitPresenter(player.matPlayer.workers[j]);
					if (unitPresenter != null)
					{
						unitPresenter.Clear();
					}
				}
			}
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0008843C File Offset: 0x0008663C
		public void SetCollidersEnabledOnUnits(bool enabled, GameHex ommitHex = null, Unit ommitUnit = null)
		{
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				foreach (Unit unit in player.GetAllUnits())
				{
					if (unit != ommitUnit && (ommitHex == null || unit.position != ommitHex))
					{
						GameController.GetUnitPresenter(unit).SetColliderEnabled(enabled);
					}
				}
			}
		}

		// Token: 0x04000AE9 RID: 2793
		public Sprite tunnelOutline;

		// Token: 0x04000AEA RID: 2794
		public Sprite tunnelIcon;

		// Token: 0x04000AEB RID: 2795
		public ResourceTypeLayer resourceTypeLayer;

		// Token: 0x04000AEC RID: 2796
		public Transform hexCollection;

		// Token: 0x04000AED RID: 2797
		public Dictionary<Vector3i, GameHexPresenter> hexes = new Dictionary<Vector3i, GameHexPresenter>();

		// Token: 0x04000AEE RID: 2798
		public Transform centerOfTheMap;

		// Token: 0x04000AEF RID: 2799
		public float hexRadius = 2.027118f;
	}
}

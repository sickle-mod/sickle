using System;
using System.Collections.Generic;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x0200056B RID: 1387
	public class AiAction
	{
		// Token: 0x06002C95 RID: 11413 RVA: 0x000444AA File Offset: 0x000426AA
		public AiAction(int matSectionId, TopAction topAction, int gainActionId, DownAction downAction, GameManager gameManager)
		{
			this.matSectionId = matSectionId;
			this.topAction = topAction;
			this.gainActionId = gainActionId;
			this.downAction = downAction;
			this.gameManager = gameManager;
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x000F804C File Offset: 0x000F624C
		public void ActionTopExecute(AiRecipe recipe, AiPlayer player)
		{
			switch (this.topAction.GetGainAction(this.gainActionId).GetGainType())
			{
			case GainType.Coin:
				this.GainCoin(recipe, player);
				return;
			case GainType.Popularity:
				this.GainPopularity(recipe, player);
				return;
			case GainType.Power:
				this.GainPower(recipe, player);
				return;
			case GainType.CombatCard:
				this.GainCombatCard(recipe, player);
				return;
			case GainType.Produce:
				this.GainProduce(recipe, player);
				return;
			case GainType.AnyResource:
				this.GainTrade(recipe, player);
				return;
			case GainType.Resource:
				return;
			case GainType.Move:
				this.GainMove(recipe, player);
				return;
			case GainType.Upgrade:
				this.PerformGainUpgrade(player, (GainUpgrade)this.topAction.GetGainAction(this.gainActionId), "Gain Upgrade");
				return;
			case GainType.Mech:
				if (player.player.aiDifficulty == AIDifficulty.Hard)
				{
					this.PerformGainMechAdvanced(player, (GainMech)this.topAction.GetGainAction(this.gainActionId), "Gain Mech");
				}
				else
				{
					this.PerformGainMechBasic(player, (GainMech)this.topAction.GetGainAction(this.gainActionId), "Gain Mech");
				}
				return;
			case GainType.Building:
				this.PerformGainBuilding(player, (GainBuilding)this.topAction.GetGainAction(this.gainActionId), "Gain Building");
				return;
			case GainType.Recruit:
				this.PerformGainRecruit(player, (GainRecruit)this.topAction.GetGainAction(this.gainActionId), "Gain Recruit");
				return;
			default:
				return;
			}
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x000F80D8 File Offset: 0x000F62D8
		public void ActionDownExecute(AiPlayer player)
		{
			switch (this.downAction.GetGainAction(0).GetGainType())
			{
			case GainType.Upgrade:
				this.GainUpgrade(player);
				return;
			case GainType.Mech:
				this.GainMech(player);
				return;
			case GainType.Worker:
				return;
			case GainType.Building:
				this.GainBuilding(player);
				return;
			case GainType.Recruit:
				this.GainRecruit(player);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x000444D7 File Offset: 0x000426D7
		public GainAction GetTopGainAction()
		{
			return this.topAction.GetGainAction(this.gainActionId);
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void GainPower(AiRecipe recipe, AiPlayer player)
		{
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void GainPopularity(AiRecipe recipe, AiPlayer player)
		{
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void GainCoin(AiRecipe recipe, AiPlayer player)
		{
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x000F8134 File Offset: 0x000F6334
		public static int FindResourceGain(SectionAction action)
		{
			for (int i = 0; i < action.gainActionsCount; i++)
			{
				GainAction gainAction = action.GetGainAction(i);
				if (gainAction is GainResource || gainAction is GainAnyResource)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x000F8170 File Offset: 0x000F6370
		public bool ShouldTakeResources(Unit unit, List<GameHex> target)
		{
			foreach (GameHex gameHex in target)
			{
				if (gameHex.Owner != null && gameHex.Owner != unit.Owner)
				{
					return unit.position.GetResourceCount() > 0 && unit.position.GetOwnerUnitCount() == 1;
				}
			}
			if (unit.Owner.matFaction.faction == Faction.Nordic || unit.Owner.matFaction.faction == Faction.Saxony)
			{
				if ((unit.Owner.matFaction.faction == Faction.Saxony && unit.Owner.matPlayer.matType == PlayerMatType.Agricultural) || (unit.Owner.matFaction.faction == Faction.Saxony && unit.Owner.matPlayer.matType == PlayerMatType.Industrial))
				{
					foreach (GameHex gameHex2 in target)
					{
						if (gameHex2.Owner != null && gameHex2.Owner != unit.Owner && gameHex2.GetOwnerUnitCount() - gameHex2.GetOwnerWorkers().Count > 0 && unit.position.GetOwnerUnitCount() > 1)
						{
							return false;
						}
					}
				}
				return unit.position.GetOwnerUnitCount() - unit.position.GetOwnerWorkers().Count <= 1;
			}
			return true;
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x000F82E4 File Offset: 0x000F64E4
		public void MoveByAnalysisPriority(AiRecipe recipe, AiPlayer player)
		{
			GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
			this.gameManager.moveManager.SetMoveAction(gainMove);
			for (int i = 0; i < (int)gainMove.Amount; i++)
			{
				if (i < player.strategicAnalysis.movePrioritySorted.Count)
				{
					Unit unit = player.strategicAnalysis.movePrioritySorted.Values[i];
					// Fix 2: Passenger workers are added to movePriority at priority -1 so they
					// ride on their mech, but they never receive a moveTarget entry of their own.
					// Accessing moveTarget[unit] for such a worker throws KeyNotFoundException,
					// which escapes uncaught and deadlocks the game. Skip any unit without a target.
					if (!player.strategicAnalysis.moveTarget.ContainsKey(unit)) continue;
					this.gameManager.moveManager.SelectUnit(unit);
					Dictionary<ResourceType, int> dictionary = null;
					if (this.ShouldTakeResources(unit, player.strategicAnalysis.moveTarget[unit]) && unit.position.GetResourceCount() > 0)
					{
						dictionary = new Dictionary<ResourceType, int>();
						dictionary.Add(ResourceType.oil, unit.position.resources[ResourceType.oil]);
						dictionary.Add(ResourceType.metal, unit.position.resources[ResourceType.metal]);
						dictionary.Add(ResourceType.food, unit.position.resources[ResourceType.food]);
						dictionary.Add(ResourceType.wood, unit.position.resources[ResourceType.wood]);
					}
					List<Unit> list = new List<Unit>();
					if (unit.UnitType == UnitType.Mech)
					{
						Mech mech = (Mech)unit;
						// Passenger Spreading: If some workers should stay behind, unload them now.
						// This ensures we only carry those in the intended passenger list.
						if (player.strategicAnalysis.moveMechPassengers.ContainsKey(mech))
						{
							foreach (Worker workerOnMech in new List<Worker>(mech.LoadedWorkers))
							{
								if (!player.strategicAnalysis.moveMechPassengers[mech].Contains(workerOnMech))
								{
									this.gameManager.moveManager.UnloadWorkerFromSelectedMech(workerOnMech);
								}
							}
							foreach (Worker worker in player.strategicAnalysis.moveMechPassengers[mech])
							{
								list.Add(worker);
							}
						}
						else
						{
							// If no passengers assigned, ensure ALL workers are unloaded for max spreading
							this.gameManager.moveManager.UnloadAllWorkersFromMech(mech);
						}
					}
					this.gameManager.moveManager.MoveSelectedUnit(player.strategicAnalysis.moveTarget[unit][0], dictionary, list);
					// Fix C: Crimea Wayfare+Speed capital-hop execution.
					// When moveTarget[0] is an unoccupied enemy capital (Wayfare move), the workers
					// remain on the mech and must be carried to moveTarget[1] (the Speed landing hex)
					// where they are dropped for territory scoring.
					// This branch must come BEFORE the production two-target branch because
					// ResourceProduced(HexType.capital) returns combatCard which gives a nonsensical
					// `num` value and would either skip the second hop or drop workers at the capital.
					if (unit.UnitType == UnitType.Mech
						&& player.strategicAnalysis.moveTarget[unit].Count > 1
						&& player.strategicAnalysis.moveTarget[unit][0].hexType == HexType.capital
						&& player.strategicAnalysis.moveTarget[unit][0].Owner != null
						&& player.strategicAnalysis.moveTarget[unit][0].Owner != player.player)
					{
						// Workers are still on the mech after the Wayfare hop (they are not
						// automatically dropped; the engine waits for an explicit unload call).
						// Carry them to the landing hex using the same passenger list.
						List<Unit> list2 = new List<Unit>(list);
						this.gameManager.moveManager.MoveSelectedUnit(
							player.strategicAnalysis.moveTarget[unit][1], dictionary, list2);
						// Drop all workers at the landing hex so they score that territory.
						this.gameManager.moveManager.UnloadAllWorkersFromMech((Mech)unit);
						dictionary = null; // Don't try to take resources again
					}
					else if (unit.UnitType == UnitType.Mech && player.strategicAnalysis.moveTarget[unit].Count > 1)
					{
						// Spreading logic: if we have multiple targets, drop one worker at the first target
						// and carry the rest to the next targets.
						if (list != null && list.Count > 0)
						{
							// If we have 1 unit to move 2 hexes, drop at hex 1.
							// If we have 2 units to move 2 hexes, drop 1 at hex 1 and carry 1 to hex 2.
							if (list.Count == 1)
							{
								this.gameManager.moveManager.UnloadAllWorkersFromMech((Mech)unit);
							}
							else if (list.Count >= 2 && (unit.MovesLeft > 0 || gainMove.MovesLeft > 0))
							{
								// Drop exactly one worker at the first stop
								this.gameManager.moveManager.UnloadWorkerFromSelectedMech(list[0]);
								
								// Prepare to carry the rest
								List<Unit> carryForward = new List<Unit>();
								for (int j = 1; j < list.Count; j++)
								{
									carryForward.Add(list[j]);
								}
								this.gameManager.moveManager.MoveSelectedUnit(player.strategicAnalysis.moveTarget[unit][1], null, carryForward);
							}
							
							// Final unload at destination
							this.gameManager.moveManager.UnloadAllWorkersFromMech((Mech)unit);
							dictionary = null; // Don't try to take resources again
						}
					}
				}
			}
			this.gameManager.moveManager.Clear();
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x000F85F0 File Offset: 0x000F67F0
		public void GainMove(AiRecipe recipe, AiPlayer aiPlayer)
		{
			if (recipe.moveAction != null)
			{
				try
				{
					recipe.moveAction(recipe, aiPlayer);
					goto IL_002A;
				}
				catch (Exception)
				{
					this.MoveByAnalysisPriority(recipe, aiPlayer);
					goto IL_002A;
				}
			}
			// Fix 3: Wrap MoveByAnalysisPriority in try-catch. Any unhandled exception here
			// (KeyNotFoundException from a missing moveRange or moveTarget entry, or any other
			// analysis edge case) would escape all the way up through Bot() without calling
			// InformAboutEndedTurn(), permanently deadlocking the game. Better to take no move
			// and end the turn cleanly than to hang forever.
			try
			{
				this.MoveByAnalysisPriority(recipe, aiPlayer);
			}
			catch (Exception)
			{
				// Analysis produced bad data — skip the move, clear state, and continue.
			}
			IL_002A:
			aiPlayer.HandleEncounterAndFactory();
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x000F8650 File Offset: 0x000F6850
		private int MechDeployPriorityV1(GameHex hex)
		{
			int num = hex.GetOwnerWorkers().Count;
			if (hex.GetOwnerMechs().Count == 0)
			{
				num += 10;
			}
			if (hex.hexType == HexType.capital)
			{
				num = -1;
			}
			return num;
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x000F8688 File Offset: 0x000F6888
		private int MechDeployPriorityV2(GameHex hex)
		{
			int num = hex.GetOwnerWorkers().Count * 10;
			if (hex.GetOwnerMechs().Count == 0)
			{
				num -= 100;
			}
			if (hex.hexType == HexType.capital)
			{
				num = -10000;
			}
			return num;
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x000F86C8 File Offset: 0x000F68C8
		private int MechDeployPriorityV3(GameHex hex)
		{
			int num = hex.GetOwnerWorkers().Count * 10;
			num -= hex.GetOwnerMechs().Count * 100;
			if (hex.HasOwnerCharacter())
			{
				num -= 50;
			}
			if (hex.hexType == HexType.capital)
			{
				num = -10000;
			}
			return num;
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x000F8714 File Offset: 0x000F6914
		private int MechDeployPriorityV4(GameHex hex)
		{
			int num = hex.GetOwnerWorkers().Count * 10;
			num -= hex.GetOwnerMechs().Count * 100;
			if (hex.HasOwnerCharacter())
			{
				num -= 5;
			}
			if (hex.hexType == HexType.capital)
			{
				num = -10000;
			}
			return num;
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x000F8714 File Offset: 0x000F6914
		private int MechDeployPriorityV5(GameHex hex)
		{
			int num = hex.GetOwnerWorkers().Count * 10;
			num -= hex.GetOwnerMechs().Count * 100;
			if (hex.HasOwnerCharacter())
			{
				num -= 5;
			}
			if (hex.hexType == HexType.capital)
			{
				num = -10000;
			}
			return num;
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x000444EA File Offset: 0x000426EA
		public void GainMech(AiPlayer aiPlayer)
		{
			if (aiPlayer.player.aiDifficulty == AIDifficulty.Hard)
			{
				this.GainMechAdvanced(aiPlayer);
				return;
			}
			this.GainMechBasic(aiPlayer);
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x000F8760 File Offset: 0x000F6960
		private void GainMechBasic(AiPlayer aiPlayer)
		{
			string text = "Gain Mech";
			GainMech gainMech = (GainMech)aiPlayer.AiActions[aiPlayer.gainMechActionPosition[0]].downAction.GetGainAction(0);
			this.PerformGainMechBasic(aiPlayer, gainMech, text);
			if (!gainMech.ActionSelected)
			{
				return;
			}
			if (!aiPlayer.Pay4Action((PayResource)aiPlayer.AiActions[aiPlayer.gainMechActionPosition[0]].downAction.GetPayAction(0)))
			{
				text += " ...too poor";
				gainMech.Clear();
				return;
			}
			gainMech.Execute();
			this.downAction.ReportLog(gainMech.GetLogInfo());
		}

		private void PerformGainMechBasic(AiPlayer aiPlayer, GainMech gainMech, string text)
		{
			if (!gainMech.GainAvaliable())
			{
				text += " ...Mech gain unavailable";
				return;
			}
			Mech mech = new Mech(this.gameManager, this.gameManager.PlayerCurrent, 1);
			GameHex gameHex = (this.IsValidDeployHex(aiPlayer.player.matPlayer.workers[0].position, aiPlayer.player) ? aiPlayer.player.matPlayer.workers[0].position : null);
			if (aiPlayer.strategicAnalysis.preferredDeployPosition != null && this.IsValidDeployHex(aiPlayer.strategicAnalysis.preferredDeployPosition, aiPlayer.player))
			{
				gameHex = aiPlayer.strategicAnalysis.preferredDeployPosition;
			}
			else
			{
				for (int i = 0; i < aiPlayer.player.matPlayer.workers.Count; i++)
				{
					GameHex position = aiPlayer.player.matPlayer.workers[i].position;
					if ((gameHex == null || gameHex.hexType == HexType.capital) && this.IsValidDeployHex(position, aiPlayer.player))
					{
						gameHex = position;
					}
				}
				if (aiPlayer.player.matFaction.faction == Faction.Nordic)
				{
					foreach (Worker worker in aiPlayer.player.matPlayer.workers)
					{
						if (this.IsValidDeployHex(worker.position, aiPlayer.player) && (gameHex == null || this.MechDeployPriorityV1(worker.position) > this.MechDeployPriorityV1(gameHex)))
						{
							gameHex = worker.position;
						}
					}
				}
				if ((aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical))
				{
					foreach (Worker worker2 in aiPlayer.player.matPlayer.workers)
					{
						if (this.IsValidDeployHex(worker2.position, aiPlayer.player) && (gameHex == null || this.MechDeployPriorityV2(worker2.position) > this.MechDeployPriorityV2(gameHex)))
						{
							gameHex = worker2.position;
						}
					}
				}
				if ((aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural) || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Rusviet)
				{
					foreach (Worker worker3 in aiPlayer.player.matPlayer.workers)
					{
						if (this.IsValidDeployHex(worker3.position, aiPlayer.player) && (gameHex == null || this.MechDeployPriorityV3(worker3.position) > this.MechDeployPriorityV3(gameHex)))
						{
							gameHex = worker3.position;
						}
					}
				}
				if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					foreach (Worker worker4 in aiPlayer.player.matPlayer.workers)
					{
						if (this.IsValidDeployHex(worker4.position, aiPlayer.player) && (gameHex == null || this.MechDeployPriorityV4(worker4.position) > this.MechDeployPriorityV4(gameHex)))
						{
							gameHex = worker4.position;
						}
					}
				}
			}
			if (gameHex != null && gameHex.hexType != HexType.capital)
			{
				gainMech.SetMechAndLocation(mech, gameHex, aiPlayer.strategicAnalysis.mechNext);
					text = text + " ...Mech " + mech.Owner.matFaction.abilities[mech.Id].ToString() + " deployed";
				return;
			}
			text += "...No suitable place to Deploy";
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x000F8CDC File Offset: 0x000F6EDC
		public void GainBuilding(AiPlayer aiPlayer)
		{
			string text = "Gain Building";
			GainBuilding gainBuilding = (GainBuilding)aiPlayer.AiActions[aiPlayer.gainBuildingActionPosition[0]].downAction.GetGainAction(0);
			this.PerformGainBuilding(aiPlayer, gainBuilding, text);
			if (!gainBuilding.ActionSelected)
			{
				return;
			}
			if (!aiPlayer.Pay4Action((PayResource)aiPlayer.AiActions[aiPlayer.gainBuildingActionPosition[0]].downAction.GetPayAction(0)))
			{
				text += " ...too poor";
				gainBuilding.Clear();
				return;
			}
			gainBuilding.Execute();
			this.downAction.ReportLog(gainBuilding.GetLogInfo());
		}

		private void PerformGainBuilding(AiPlayer aiPlayer, GainBuilding gainBuilding, string text)
		{
			if (!gainBuilding.GainAvaliable())
			{
				text += " ...Construction gain unavailable";
				return;
			}
			Building building = null;
			GameHex gameHex = null;
			for (int i = 0; i < 4; i++)
			{
				if (aiPlayer.player.matPlayer.GetPlayerMatSection(i).ActionTop.Structure.position == null && aiPlayer.player.matPlayer.GetPlayerMatSection(i).ActionTop.Structure.buildingType == aiPlayer.strategicAnalysis.buildingNext)
				{
					building = aiPlayer.player.matPlayer.GetPlayerMatSection(i).ActionTop.Structure;
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Polania && building != null && building.buildingType == BuildingType.Mine)
			{
				if (this.gameManager.gameBoard.hexMap[1, 4].Building == null && this.gameManager.gameBoard.hexMap[1, 4].GetOwnerWorkers().Count > 0)
				{
					gameHex = this.gameManager.gameBoard.hexMap[1, 4];
				}
				else if (aiPlayer.player.character.position.Building == null && aiPlayer.player.character.position.GetOwnerWorkers().Count > 0 && (aiPlayer.player.character.position.hexType == HexType.farm || aiPlayer.player.character.position.hexType == HexType.forest || aiPlayer.player.character.position.hexType == HexType.mountain || aiPlayer.player.character.position.hexType == HexType.tundra || aiPlayer.player.character.position.hexType == HexType.village))
				{
					gameHex = aiPlayer.player.character.position;
				}
			}
			if ((aiPlayer.player.matFaction.faction == Faction.Saxony || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType != PlayerMatType.Mechanical)) && building != null && building.buildingType == BuildingType.Mill && gameHex == null)
			{
				foreach (Worker worker in aiPlayer.player.matPlayer.workers)
				{
					if (worker.position.Building == null && (worker.position.hexType == HexType.farm || worker.position.hexType == HexType.forest || worker.position.hexType == HexType.mountain || worker.position.hexType == HexType.tundra))
					{
						gameHex = worker.position;
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural && building != null && building.buildingType == BuildingType.Mill && gameHex == null)
			{
				foreach (Worker worker2 in aiPlayer.player.matPlayer.workers)
				{
					if (worker2.position.Building == null && worker2.position.hexType == HexType.forest)
					{
						gameHex = worker2.position;
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && building != null && building.buildingType == BuildingType.Mill && gameHex == null)
			{
				foreach (Worker worker3 in aiPlayer.player.matPlayer.workers)
				{
					if (worker3.position.Building == null && worker3.position.hexType == HexType.farm)
					{
						gameHex = worker3.position;
					}
				}
			}
			if (gameHex == null)
			{
				int num = -99999;
				foreach (Worker worker4 in aiPlayer.player.matPlayer.workers)
				{
					if (worker4.position.Building == null && (worker4.position.hexType == HexType.farm || worker4.position.hexType == HexType.forest || worker4.position.hexType == HexType.mountain || worker4.position.hexType == HexType.tundra || worker4.position.hexType == HexType.village))
					{
						int num2 = 0;
						if (building != null && building.buildingType == BuildingType.Mill && worker4.position.hexType == HexType.village)
						{
							num2 -= 500;
							if (aiPlayer.player.matPlayer.workers.Count >= 4)
							{
								num2 -= 1000;
							}
						}
						if (num2 > num)
						{
							num = num2;
							gameHex = worker4.position;
						}
					}
				}
			}
			if (building != null && gameHex != null)
			{
				gainBuilding.SetStructureAndLocation(building, gameHex);
				text = text + " ..." + building.buildingType.ToString() + " built";
				return;
			}
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x000F92C8 File Offset: 0x000F74C8
		public void GainRecruit(AiPlayer aiPlayer)
		{
			string text = "Gain Recruit";
			GainRecruit gainRecruit = (GainRecruit)aiPlayer.AiActions[aiPlayer.gainRecruitActionPosition[0]].downAction.GetGainAction(0);
			this.PerformGainRecruit(aiPlayer, gainRecruit, text);
			if (!gainRecruit.ActionSelected)
			{
				return;
			}
			if (!aiPlayer.Pay4Action((PayResource)aiPlayer.AiActions[aiPlayer.gainRecruitActionPosition[0]].downAction.GetPayAction(0)))
			{
				text += " ...too poor";
				gainRecruit.Clear();
				return;
			}
			gainRecruit.Execute();
			this.downAction.ReportLog(gainRecruit.GetLogInfo());
		}

		private void PerformGainRecruit(AiPlayer aiPlayer, GainRecruit gainRecruit, string text)
		{
			if (gainRecruit.GainAvaliable())
			{
				DownActionType downActionType = DownActionType.Factory;
				for (int i = 0; i < 4; i++)
				{
					if (!aiPlayer.player.matPlayer.GetPlayerMatSection(i).ActionDown.IsRecruitEnlisted && (downActionType == DownActionType.Factory || aiPlayer.strategicAnalysis.recruitPriority[aiPlayer.player.matPlayer.GetPlayerMatSection(i).ActionDown.Type] > aiPlayer.strategicAnalysis.recruitPriority[downActionType]))
					{
						downActionType = aiPlayer.player.matPlayer.GetPlayerMatSection(i).ActionDown.Type;
					}
				}
				GainType gainType = GainType.CombatCard;
				foreach (GainType gainType2 in aiPlayer.player.matFaction.OneTimeBonuses.Keys)
				{
					if (!aiPlayer.player.matFaction.OneTimeBonusUsed(gainType2))
					{
						if (aiPlayer.player.matFaction.OneTimeBonusUsed(gainType))
						{
							gainType = gainType2;
						}
						else if (aiPlayer.strategicAnalysis.recruitOneTimePriority[gainType2] > aiPlayer.strategicAnalysis.recruitOneTimePriority[gainType])
						{
							gainType = gainType2;
						}
					}
				}
				GainAction oneTimeBonus = aiPlayer.player.matFaction.GetOneTimeBonus(gainType);
				gainRecruit.SetSectionAndBonus(downActionType, oneTimeBonus);
				text += " ...Recruit enlisted";
				return;
			}
			text += " ...Recruit gain unavailable";
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x00044509 File Offset: 0x00042709
		private int UpgradePriority(GainType gainType, AiPlayer player)
		{
			if (player.player.aiDifficulty == AIDifficulty.Hard)
			{
				return this.UpgradePriorityAdvanced(gainType, player);
			}
			return this.UpgradePriorityBasic(gainType, player);
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x000F94CC File Offset: 0x000F76CC
		private int UpgradePriorityBasic(GainType gainType, AiPlayer player)
		{
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 200;
				case GainType.Building:
					return 80;
				case GainType.Recruit:
					return 70;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Saxony && player.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 200;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 70;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Saxony && player.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 160;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 170;
				case GainType.Produce:
					return 150;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 60;
				case GainType.Building:
					return 80;
				case GainType.Recruit:
					return 70;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Saxony && player.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 80;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Saxony && player.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 90;
				case GainType.Mech:
					if (player.player.matFaction.mechs.Count < 2)
					{
						return 80;
					}
					return 50;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 70;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					if (player.player.stars[StarType.Combat] < 2)
					{
						return 210;
					}
					return 190;
				case GainType.CombatCard:
					if (player.player.stars[StarType.Combat] < 2)
					{
						return 180;
					}
					return 160;
				case GainType.Produce:
					if (player.player.matPlayer.workers.Count >= 6)
					{
						return 0;
					}
					if (player.player.matPlayer.workers.Count == 5)
					{
						return 20;
					}
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 120;
				case GainType.Building:
					return 70;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					if (player.player.matFaction.mechs.Count == 0)
					{
						return 80;
					}
					return 50;
				case GainType.Building:
					return 70;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					if (player.player.matPlayer.workers.Count >= 6)
					{
						return 0;
					}
					if (player.player.matPlayer.workers.Count == 5)
					{
						return 20;
					}
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 80;
				case GainType.Mech:
					return 90;
				case GainType.Building:
					return 70;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Polania && player.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 90;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 80;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Polania && player.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 70;
				case GainType.Mech:
					return 60;
				case GainType.Building:
					return 80;
				case GainType.Recruit:
					return 90;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Polania && player.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 90;
				case GainType.Mech:
					return 140;
				case GainType.Building:
					return 40;
				case GainType.Recruit:
					return 70;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Polania && player.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 70;
				case GainType.Mech:
					return 90;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 80;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Rusviet && player.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 80;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Rusviet && player.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 90;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 80;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Crimea && player.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 160;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 190;
				case GainType.Produce:
					return 150;
				case GainType.Move:
					return 170;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 80;
				}
				return 1;
			}
			switch (gainType)
			{
			case GainType.Coin:
				return 140;
			case GainType.Popularity:
				return 150;
			case GainType.Power:
				return 180;
			case GainType.CombatCard:
				return 160;
			case GainType.Produce:
				return 170;
			case GainType.Move:
				return 190;
			case GainType.Upgrade:
				return 90;
			case GainType.Mech:
				return 80;
			case GainType.Building:
				return 70;
			case GainType.Recruit:
				return 60;
			}
			return 1;
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x000F9F94 File Offset: 0x000F8194
		public void GainUpgrade(AiPlayer player)
		{
			string text = "Gain Upgrade";
			GainUpgrade gainUpgrade = (GainUpgrade)player.AiActions[player.gainUpgradeActionPosition[0]].downAction.GetGainAction(0);
			if (!gainUpgrade.GainAvaliable())
			{
				text += " ...Upgrade gain unavailable";
				return;
			}
			this.SetUpgradeActions(player, gainUpgrade);
			if (gainUpgrade.GainToUpgrade == null || gainUpgrade.PayToUpgrade == null)
			{
				text += " ...Upgrade logic failed";
				return;
			}
			if (!player.Pay4Action((PayResource)player.AiActions[player.gainUpgradeActionPosition[0]].downAction.GetPayAction(0)))
			{
				text += " ...too poor";
				gainUpgrade.Clear();
				return;
			}
			text += " ...Upgrade performed";
			gainUpgrade.Execute();
			this.downAction.ReportLog(gainUpgrade.GetLogInfo());
		}

		private void PerformGainUpgrade(AiPlayer player, GainUpgrade gainUpgrade, string text)
		{
			if (!gainUpgrade.GainAvaliable())
			{
				text += " ...Upgrade gain unavailable";
				return;
			}
			this.SetUpgradeActions(player, gainUpgrade);
			if (gainUpgrade.GainToUpgrade == null || gainUpgrade.PayToUpgrade == null)
			{
				text += " ...Upgrade logic failed";
				gainUpgrade.Clear();
				return;
			}
			text += " ...Upgrade performed";
			gainUpgrade.Execute();
			this.downAction.ReportLog(gainUpgrade.GetLogInfo());
		}

		public void SetUpgradeActions(AiPlayer player, GainUpgrade gainUpgrade)
		{
			GainAction gainAction = null;
			PayAction payAction = null;
			GainAction gainAction2 = null;
			for (int i = 0; i < player.AiActions.Count; i++)
			{
				if (player.AiActions[i].topAction.GetGainAction(player.AiActions[i].gainActionId).CanUpgrade() && (gainAction == null || this.UpgradePriority(player.AiActions[i].topAction.GetGainAction(player.AiActions[i].gainActionId).GetGainType(), player) > this.UpgradePriority(gainAction.GetGainType(), player)))
				{
					gainAction = player.AiActions[i].topAction.GetGainAction(player.AiActions[i].gainActionId);
				}
				if (player.AiActions[i].downAction.GetNumberOfPayActions() > 0 && player.AiActions[i].downAction.GetPayAction(0).CanUpgrade() && (payAction == null || this.UpgradePriority(player.AiActions[i].downAction.GetGainAction(0).GetGainType(), player) > this.UpgradePriority(gainAction2.GetGainType(), player)))
				{
					payAction = player.AiActions[i].downAction.GetPayAction(0);
					gainAction2 = player.AiActions[i].downAction.GetGainAction(0);
				}
			}
			gainUpgrade.SetPayAndGainActions(gainAction, payAction);
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x000FA19C File Offset: 0x000F839C
		public void GainTrade(AiRecipe receipe, AiPlayer aiPlayer)
		{
			if (!this.topAction.ActionPayed())
			{
				return;
			}
			string text = "Gain Trade: ";
			GainAnyResource gainAnyResource = (GainAnyResource)aiPlayer.AiTopActions[GainType.AnyResource].GetTopGainAction();
			gainAnyResource.Clear();
			ResourceType resourceType = aiPlayer.TradeResourceType();
			if (resourceType == ResourceType.combatCard)
			{
				text += " ...no resource needed...";
				resourceType = ResourceType.oil;
			}
			List<GameHex> list = new List<GameHex>();
			GameHex gameHex = aiPlayer.player.matPlayer.workers[0].position;
			for (int i = 0; i < aiPlayer.player.matPlayer.workers.Count; i++)
			{
				if (gameHex.hexType == HexType.capital && aiPlayer.player.matPlayer.workers[i].position != null && aiPlayer.player.matPlayer.workers[i].position.hexType != HexType.capital)
				{
					list.Add(aiPlayer.player.matPlayer.workers[i].position);
				}
			}
			if (list.Count > 0)
			{
				gameHex = list[0];
				foreach (GameHex gameHex2 in list)
				{
					if (gameHex2.GetOwnerMechs().Count > gameHex.GetOwnerMechs().Count)
					{
						gameHex = gameHex2;
					}
				}
			}
			if (gameHex.hexType != HexType.capital)
			{
				if (receipe.tradeResource == null)
				{
					gainAnyResource.AddResourceToField(resourceType, gameHex, 2);
				}
				else
				{
					gainAnyResource.AddResourceToField(receipe.tradeResource[0], gameHex, 1);
					gainAnyResource.AddResourceToField(receipe.tradeResource[1], gameHex, 1);
				}
			}
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x000FA368 File Offset: 0x000F8568
		public void GainProduce(AiRecipe recipe, AiPlayer aiPlayer)
		{
			if (!this.topAction.ActionPayed())
			{
				return;
			}
			if (recipe.moveAction != null)
			{
				recipe.moveAction(recipe, aiPlayer);
				return;
			}
			GainProduce gainProduce = (GainProduce)aiPlayer.AiTopActions[GainType.Produce].GetTopGainAction();
			gainProduce.Clear();
			foreach (GameHex gameHex in aiPlayer.player.OwnedFields(false))
			{
				int count = gameHex.GetOwnerWorkers().Count;
				if (count > 0 && (gameHex.Building == null || gameHex.Building.buildingType != BuildingType.Mill || gameHex.Building.player != aiPlayer.player) && (gameHex.hexType == HexType.farm || gameHex.hexType == HexType.forest || gameHex.hexType == HexType.mountain || gameHex.hexType == HexType.tundra || (gameHex.hexType == HexType.village && aiPlayer.player.matPlayer.workers.Count < aiPlayer.strategicAnalysis.workerCountTarget)))
				{
					gainProduce.ExecuteOnce(gameHex, count);
				}
			}
			foreach (GameHex gameHex2 in aiPlayer.player.FieldsWithPlayerBuildings())
			{
				if (gameHex2.Building.buildingType == BuildingType.Mill && gameHex2.Owner == aiPlayer.player)
				{
					gainProduce.ExecuteOnce(gameHex2, gameHex2.GetOwnerWorkers().Count + 1);
				}
			}
			gainProduce.SelectAction();
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x0004452A File Offset: 0x0004272A
		public override string ToString()
		{
			return "top: " + this.topAction.GetGainAction(this.gainActionId).ToString() + " bottom: " + this.downAction.GetGainAction(0).ToString();
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x000FA50C File Offset: 0x000F870C
		private int UpgradePriorityAdvanced(GainType gainType, AiPlayer player)
		{
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Militant)
			{
				switch (gainType)
				{
				case GainType.Move:
					return 200;
				case GainType.Power:
				case GainType.CombatCard:
					return 190;
				case GainType.Upgrade:
					return 180;
				case GainType.Mech:
					return 170;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 200;
				case GainType.Building:
					return 80;
				case GainType.Recruit:
					return 70;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Polania && player.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 180;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					return 70;
				case GainType.Mech:
					return 110;
				case GainType.Building:
					return 80;
				case GainType.Recruit:
					return 90;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Polania && player.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 180;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					return 10;
				case GainType.Mech:
					return 130;
				case GainType.Building:
					return 40;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Polania && player.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 80;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Polania && player.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 40;
				case GainType.Recruit:
					if (player.strategicAnalysis.objectiveBalancedWorkforce)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Polania && player.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 140;
				case GainType.Building:
					if (player.player.matPlayer.buildings.Count >= 1)
					{
						return 30;
					}
					return 50;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Polania && player.player.matPlayer.matType == PlayerMatType.Militant)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 20;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Rusviet && player.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 170;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 190;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 50;
				case GainType.Recruit:
					return 10;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					if (player.player.matFaction.mechs.Count == 0)
					{
						return 80;
					}
					return 50;
				case GainType.Building:
					return 70;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 195;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 185;
				case GainType.AnyResource:
					return 175;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 110;
				case GainType.Building:
					return 80;
				case GainType.Recruit:
					return 120;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 190;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 80;
				case GainType.Mech:
					return 120;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 10;
				case GainType.Mech:
					if (player.player.matFaction.mechs.Count >= 1)
					{
						return 20;
					}
					return 70;
				case GainType.Building:
					return 30;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Militant)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 20;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					return 65;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 50;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Rusviet && player.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Rusviet && player.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					return 70;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					return 90;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Rusviet && player.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 170;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 190;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 10;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 70;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Rusviet && player.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 80;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Rusviet && player.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					return 90;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Rusviet && player.player.matPlayer.matType == PlayerMatType.Militant)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 20;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Rusviet && player.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 170;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 190;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Crimea && player.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 160;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 190;
				case GainType.Produce:
					return 150;
				case GainType.Move:
					return 170;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 80;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Crimea && player.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Crimea && player.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 150;
				case GainType.Popularity:
					return 170;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 200;
				case GainType.Produce:
					return 160;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 80;
				case GainType.Mech:
					return 140;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					return 130;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Crimea && player.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 80;
				case GainType.Mech:
					return 180;
				case GainType.Building:
					return 1;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Crimea && player.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 1;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Crimea && player.player.matPlayer.matType == PlayerMatType.Militant)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 20;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 1;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Crimea && player.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 170;
				case GainType.Produce:
					return 160;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 70;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 90;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Saxony && player.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 180;
				case GainType.Move:
					return 170;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 40;
				case GainType.Recruit:
					return 70;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Saxony && player.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 170;
				case GainType.Produce:
					return 160;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 20;
				case GainType.Mech:
					return (player.player.matPlayer.UpgradesDone == 0) ? 50 : 70;
				case GainType.Building:
					return (player.player.matPlayer.UpgradesDone == 0) ? 60 : 40;
				case GainType.Recruit:
					return 10;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Saxony && player.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Saxony && player.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 90;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 40;
				case GainType.Recruit:
					if (player.strategicAnalysis.objectiveBalancedWorkforce)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Saxony && player.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 10;
				case GainType.Mech:
					if (player.player.matFaction.mechs.Count < 2)
					{
						return 80;
					}
					return 50;
				case GainType.Building:
					return 40;
				case GainType.Recruit:
					return 70;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Saxony && player.player.matPlayer.matType == PlayerMatType.Militant)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 20;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Saxony && player.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Albion && player.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 70;
				case GainType.Mech:
					return 90;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Albion && player.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Albion && player.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Upgrade:
					return 145;
				case GainType.Mech:
					return 185;
				case GainType.Building:
					return 130;
				case GainType.Recruit:
					return 155;
				case GainType.Power:
					return 205;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Albion && player.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Albion && player.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Albion && player.player.matPlayer.matType == PlayerMatType.Militant)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 20;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Albion && player.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 170;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 180;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 70;
				case GainType.Mech:
					return 90;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 80;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Togawa && player.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Togawa && player.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 90;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 70;
				case GainType.Recruit:
					return 100;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Togawa && player.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Togawa && player.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 80;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					return 10;
				case GainType.Recruit:
					if (player.player.matFaction.mechs.Count >= 2)
					{
						return 85;
					}
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Togawa && player.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 90;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 70;
				case GainType.Recruit:
					return 100;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Togawa && player.player.matPlayer.matType == PlayerMatType.Militant)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 160;
				case GainType.Produce:
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					if (player.player.matPlayer.UpgradesDone >= 2)
					{
						return 20;
					}
					return 80;
				case GainType.Mech:
					return 90;
				case GainType.Building:
					return 70;
				case GainType.Recruit:
					return 60;
				}
				return 1;
			}
			if (player.player.matFaction.faction == Faction.Togawa && player.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 140;
				case GainType.Popularity:
					return 160;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 150;
				case GainType.Produce:
					return 180;
				case GainType.Move:
					return 200;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 60;
				case GainType.Recruit:
					return 70;
				}
				return 1;
			}
			switch (gainType)
			{
			case GainType.Coin:
				return 140;
			case GainType.Popularity:
				return 150;
			case GainType.Power:
				return 190;
			case GainType.CombatCard:
				return 160;
			case GainType.Produce:
				return 170;
			case GainType.Move:
				return 180;
			case GainType.Upgrade:
				return 80;
			case GainType.Mech:
				return 70;
			case GainType.Building:
				return 10;
			case GainType.Recruit:
				if (player.player.matFaction.mechs.Count >= 2)
				{
					return 85;
				}
				return 60;
			}
			return 1;
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000FC6D0 File Offset: 0x000FA8D0
		private void GainMechAdvanced(AiPlayer aiPlayer)
		{
			string text = "Gain Mech";
			GainMech gainMech = (GainMech)aiPlayer.AiActions[aiPlayer.gainMechActionPosition[0]].downAction.GetGainAction(0);
			this.PerformGainMechAdvanced(aiPlayer, gainMech, text);
			if (!gainMech.ActionSelected)
			{
				return;
			}
			if (!aiPlayer.Pay4Action((PayResource)aiPlayer.AiActions[aiPlayer.gainMechActionPosition[0]].downAction.GetPayAction(0)))
			{
				text += " ...too poor";
				gainMech.Clear();
				return;
			}
			gainMech.Execute();
			this.downAction.ReportLog(gainMech.GetLogInfo());
		}

		private void PerformGainMechAdvanced(AiPlayer aiPlayer, GainMech gainMech, string text)
		{
			if (!gainMech.GainAvaliable())
			{
				text += " ...Mech gain unavailable";
				return;
			}
			Mech mech = new Mech(this.gameManager, this.gameManager.PlayerCurrent, 1);
			GameHex gameHex = (this.IsValidDeployHex(aiPlayer.player.matPlayer.workers[0].position, aiPlayer.player) ? aiPlayer.player.matPlayer.workers[0].position : null);
			if (aiPlayer.strategicAnalysis.preferredDeployPosition != null && this.IsValidDeployHex(aiPlayer.strategicAnalysis.preferredDeployPosition, aiPlayer.player))
			{
				gameHex = aiPlayer.strategicAnalysis.preferredDeployPosition;
			}
			else
			{
				for (int i = 0; i < aiPlayer.player.matPlayer.workers.Count; i++)
				{
					GameHex position = aiPlayer.player.matPlayer.workers[i].position;
					if ((gameHex == null || gameHex.hexType == HexType.capital) && this.IsValidDeployHex(position, aiPlayer.player))
					{
						gameHex = position;
					}
				}
				if (aiPlayer.player.matFaction.faction == Faction.Nordic)
				{
					foreach (Worker worker in aiPlayer.player.matPlayer.workers)
					{
						if (this.IsValidDeployHex(worker.position, aiPlayer.player) && (gameHex == null || this.MechDeployPriorityV1(worker.position) > this.MechDeployPriorityV1(gameHex)))
						{
							gameHex = worker.position;
						}
					}
				}
				if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					foreach (Worker worker2 in aiPlayer.player.matPlayer.workers)
					{
						if (this.IsValidDeployHex(worker2.position, aiPlayer.player) && (gameHex == null || this.MechDeployPriorityV2(worker2.position) > this.MechDeployPriorityV2(gameHex)))
						{
							gameHex = worker2.position;
						}
					}
				}
				if ((aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural) || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Rusviet)
				{
					foreach (Worker worker3 in aiPlayer.player.matPlayer.workers)
					{
						if (this.IsValidDeployHex(worker3.position, aiPlayer.player) && (gameHex == null || this.MechDeployPriorityV3(worker3.position) > this.MechDeployPriorityV3(gameHex)))
						{
							gameHex = worker3.position;
						}
					}
				}
				if ((aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant) || (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative))
				{
					foreach (Worker worker4 in aiPlayer.player.matPlayer.workers)
					{
						if (this.IsValidDeployHex(worker4.position, aiPlayer.player) && (gameHex == null || this.MechDeployPriorityV4(worker4.position) > this.MechDeployPriorityV4(gameHex)))
						{
							gameHex = worker4.position;
						}
					}
				}
			}
			if (gameHex != null && gameHex.hexType != HexType.capital)
			{
				gainMech.SetMechAndLocation(mech, gameHex, aiPlayer.strategicAnalysis.mechNext);
					text = text + " ...Mech " + mech.Owner.matFaction.abilities[mech.Id].ToString() + " deployed";
				return;
			}
			text += "...No suitable place to Deploy";
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000FCDB4 File Offset: 0x000FAFB4
		public void GainCombatCard(AiRecipe recipe, AiPlayer player)
		{
			GainCombatCard gainCombatCard = (GainCombatCard)this.GetTopGainAction();
			if (!gainCombatCard.GainAvaliable())
			{
				return;
			}
			gainCombatCard.SetCards(gainCombatCard.Amount);
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x00044562 File Offset: 0x00042762
		private bool IsValidDeployHex(GameHex hex, Player owner)
		{
			return hex != null && hex.hexType != HexType.capital && hex.GetOwnerWorkers().Count != 0;
		}

		// Token: 0x04001E81 RID: 7809
		private GameManager gameManager;

		// Token: 0x04001E82 RID: 7810
		public readonly int matSectionId;

		// Token: 0x04001E83 RID: 7811
		public readonly TopAction topAction;

		// Token: 0x04001E84 RID: 7812
		public readonly int gainActionId;

		// Token: 0x04001E85 RID: 7813
		public readonly DownAction downAction;

		// Token: 0x0200056C RID: 1388
		// (Invoke) Token: 0x06002CB5 RID: 11445
		public delegate void ActionExecute(AiRecipe recipe, AiPlayer player);
	}
}

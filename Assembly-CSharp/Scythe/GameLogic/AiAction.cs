using System;
using System.Collections.Generic;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x02000566 RID: 1382
	public class AiAction
	{
		// Token: 0x06002C77 RID: 11383 RVA: 0x00044493 File Offset: 0x00042693
		public AiAction(int matSectionId, TopAction topAction, int gainActionId, DownAction downAction, GameManager gameManager)
		{
			this.matSectionId = matSectionId;
			this.topAction = topAction;
			this.gainActionId = gainActionId;
			this.downAction = downAction;
			this.gameManager = gameManager;
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x000F708C File Offset: 0x000F528C
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
			case GainType.Resource:
				return;
			case GainType.Produce:
				this.GainProduce(player);
				return;
			case GainType.AnyResource:
				this.GainTrade(recipe, player);
				return;
			case GainType.Move:
				this.GainMove(recipe, player);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x000F710C File Offset: 0x000F530C
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

		// Token: 0x06002C7A RID: 11386 RVA: 0x000444C0 File Offset: 0x000426C0
		public GainAction GetTopGainAction()
		{
			return this.topAction.GetGainAction(this.gainActionId);
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void GainPower(AiRecipe recipe, AiPlayer player)
		{
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void GainPopularity(AiRecipe recipe, AiPlayer player)
		{
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void GainCoin(AiRecipe recipe, AiPlayer player)
		{
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x000F7168 File Offset: 0x000F5368
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

		// Token: 0x06002C7F RID: 11391 RVA: 0x000F71A4 File Offset: 0x000F53A4
		public bool ShouldTakeResources(Unit unit, List<GameHex> target)
		{
			if (unit.Owner.matFaction.faction == Faction.Nordic || unit.Owner.matFaction.faction == Faction.Saxony)
			{
				if ((unit.Owner.matFaction.faction == Faction.Saxony && unit.Owner.matPlayer.matType == PlayerMatType.Agricultural) || (unit.Owner.matFaction.faction == Faction.Saxony && unit.Owner.matPlayer.matType == PlayerMatType.Industrial))
				{
					foreach (GameHex gameHex in target)
					{
						if (gameHex.Owner != null && gameHex.Owner != unit.Owner && gameHex.GetOwnerUnitCount() - gameHex.GetOwnerWorkers().Count > 0 && unit.position.GetOwnerUnitCount() > 1)
						{
							return false;
						}
					}
				}
				return unit.position.GetOwnerUnitCount() - unit.position.GetOwnerWorkers().Count <= 1;
			}
			return true;
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x000F72C4 File Offset: 0x000F54C4
		public void MoveByAnalysisPriority(AiRecipe recipe, AiPlayer player)
		{
			GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
			this.gameManager.moveManager.SetMoveAction(gainMove);
			for (int i = 0; i < (int)gainMove.Amount; i++)
			{
				if (i < player.strategicAnalysis.movePrioritySorted.Count)
				{
					Unit unit = player.strategicAnalysis.movePrioritySorted.Values[i];
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
					if (unit.UnitType == UnitType.Mech && player.strategicAnalysis.moveMechPassengers.ContainsKey((Mech)unit))
					{
						foreach (Worker worker in player.strategicAnalysis.moveMechPassengers[(Mech)unit])
						{
							list.Add(worker);
						}
					}
					this.gameManager.moveManager.MoveSelectedUnit(player.strategicAnalysis.moveTarget[unit][0], dictionary, list);
					if (unit.UnitType == UnitType.Mech && player.strategicAnalysis.moveTarget[unit].Count > 1)
					{
						ResourceType resourceType = AiStrategicAnalysis.ResourceProduced(player.strategicAnalysis.moveTarget[unit][0].hexType);
						int num = player.strategicAnalysis.resourceCostSingleAction[resourceType];
						if (list.Count > num)
						{
							this.gameManager.moveManager.UnloadAllWorkersFromMech((Mech)unit);
							List<Unit> list2 = new List<Unit>();
							if (player.strategicAnalysis.moveMechPassengers.ContainsKey((Mech)unit))
							{
								foreach (Worker worker2 in player.strategicAnalysis.moveTarget[unit][0].GetOwnerWorkers())
								{
									list2.Add(worker2);
								}
							}
							list2.RemoveRange(0, num);
							this.gameManager.moveManager.MoveSelectedUnit(player.strategicAnalysis.moveTarget[unit][1], dictionary, list2);
						}
					}
				}
			}
			this.gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x000F75D0 File Offset: 0x000F57D0
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
			this.MoveByAnalysisPriority(recipe, aiPlayer);
			IL_002A:
			this.gameManager.moveManager.Clear();
			aiPlayer.HandleEncounterAndFactory();
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x000F7630 File Offset: 0x000F5830
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

		// Token: 0x06002C83 RID: 11395 RVA: 0x000F7668 File Offset: 0x000F5868
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

		// Token: 0x06002C84 RID: 11396 RVA: 0x000F76A8 File Offset: 0x000F58A8
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

		// Token: 0x06002C85 RID: 11397 RVA: 0x000F76F4 File Offset: 0x000F58F4
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

		// Token: 0x06002C86 RID: 11398 RVA: 0x000F76F4 File Offset: 0x000F58F4
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

		// Token: 0x06002C87 RID: 11399 RVA: 0x000444D3 File Offset: 0x000426D3
		public void GainMech(AiPlayer aiPlayer)
		{
			if (aiPlayer.player.aiDifficulty == AIDifficulty.Hard)
			{
				this.GainMechAdvanced(aiPlayer);
				return;
			}
			this.GainMechBasic(aiPlayer);
		}

		// Token: 0x06002C88 RID: 11400 RVA: 0x000F7740 File Offset: 0x000F5940
		private void GainMechBasic(AiPlayer aiPlayer)
		{
			string text = "Gain Mech";
			if (!aiPlayer.Pay4Action((PayResource)aiPlayer.AiActions[aiPlayer.gainMechActionPosition[0]].downAction.GetPayAction(0)))
			{
				text += " ...too poor";
				return;
			}
			GainMech gainMech = (GainMech)aiPlayer.AiActions[aiPlayer.gainMechActionPosition[0]].downAction.GetGainAction(0);
			if (!gainMech.GainAvaliable())
			{
				text += " ...Mech gain unavailable";
				return;
			}
			Mech mech = new Mech(this.gameManager, this.gameManager.PlayerCurrent, 1);
			GameHex gameHex = aiPlayer.player.matPlayer.workers[0].position;
			if (aiPlayer.strategicAnalysis.preferredDeployPosition != null && aiPlayer.strategicAnalysis.preferredDeployPosition.Owner == aiPlayer.player && aiPlayer.strategicAnalysis.preferredDeployPosition.GetOwnerWorkers().Count > 0)
			{
				gameHex = aiPlayer.strategicAnalysis.preferredDeployPosition;
			}
			else
			{
				for (int i = 0; i < aiPlayer.player.matPlayer.workers.Count; i++)
				{
					if (gameHex.hexType == HexType.capital && aiPlayer.player.matPlayer.workers[i].position != null && aiPlayer.player.matPlayer.workers[i].position.hexType != HexType.capital)
					{
						gameHex = aiPlayer.player.matPlayer.workers[i].position;
					}
				}
				if (aiPlayer.player.matFaction.faction == Faction.Nordic)
				{
					foreach (Worker worker in aiPlayer.player.matPlayer.workers)
					{
						if (worker.position.hexType != HexType.capital && this.MechDeployPriorityV1(worker.position) > this.MechDeployPriorityV1(gameHex))
						{
							gameHex = worker.position;
						}
					}
				}
				if ((aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical))
				{
					foreach (Worker worker2 in aiPlayer.player.matPlayer.workers)
					{
						if (worker2.position.hexType != HexType.capital && this.MechDeployPriorityV2(worker2.position) > this.MechDeployPriorityV2(gameHex))
						{
							gameHex = worker2.position;
						}
					}
				}
				if ((aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural) || aiPlayer.player.matFaction.faction == Faction.Crimea)
				{
					foreach (Worker worker3 in aiPlayer.player.matPlayer.workers)
					{
						if (worker3.position.hexType != HexType.capital && this.MechDeployPriorityV3(worker3.position) > this.MechDeployPriorityV3(gameHex))
						{
							gameHex = worker3.position;
						}
					}
				}
				if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					foreach (Worker worker4 in aiPlayer.player.matPlayer.workers)
					{
						if (worker4.position.hexType != HexType.capital && this.MechDeployPriorityV4(worker4.position) > this.MechDeployPriorityV4(gameHex))
						{
							gameHex = worker4.position;
						}
					}
				}
			}
			if (gameHex.hexType != HexType.capital)
			{
				gainMech.SetMechAndLocation(mech, gameHex, aiPlayer.strategicAnalysis.mechNext);
				this.gameManager.actionManager.PrepareNextAction();
				text = text + " ...Mech " + mech.Owner.matFaction.abilities[mech.Id].ToString() + " deployed";
				return;
			}
			text += "...No suitable place to Deploy";
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x000F7CAC File Offset: 0x000F5EAC
		public void GainBuilding(AiPlayer aiPlayer)
		{
			string text = "Gain Building";
			if (aiPlayer.Pay4Action((PayResource)aiPlayer.AiActions[aiPlayer.gainBuildingActionPosition[0]].downAction.GetPayAction(0)))
			{
				GainBuilding gainBuilding = (GainBuilding)aiPlayer.AiActions[aiPlayer.gainBuildingActionPosition[0]].downAction.GetGainAction(0);
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
					foreach (Worker worker4 in aiPlayer.player.matPlayer.workers)
					{
						if (worker4.position.Building == null && (worker4.position.hexType == HexType.farm || worker4.position.hexType == HexType.forest || worker4.position.hexType == HexType.mountain || worker4.position.hexType == HexType.tundra || worker4.position.hexType == HexType.village))
						{
							gameHex = worker4.position;
						}
					}
				}
				if (building != null && gameHex != null)
				{
					gainBuilding.SetStructureAndLocation(building, gameHex);
					this.gameManager.actionManager.PrepareNextAction();
					text = text + " ..." + building.buildingType.ToString() + " built";
					return;
				}
			}
			else
			{
				text += " ...too poor";
			}
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x000F8230 File Offset: 0x000F6430
		public void GainRecruit(AiPlayer aiPlayer)
		{
			string text = "Gain Recruit";
			if (!aiPlayer.Pay4Action((PayResource)aiPlayer.AiActions[aiPlayer.gainRecruitActionPosition[0]].downAction.GetPayAction(0)))
			{
				text += " ...too poor";
				return;
			}
			GainRecruit gainRecruit = (GainRecruit)aiPlayer.AiActions[aiPlayer.gainRecruitActionPosition[0]].downAction.GetGainAction(0);
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
				this.gameManager.actionManager.PrepareNextAction();
				text += " ...Recruit enlisted";
				return;
			}
			text += " ...Recruit gain unavailable";
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x000444F2 File Offset: 0x000426F2
		private int UpgradePriority(GainType gainType, AiPlayer player)
		{
			if (player.player.aiDifficulty == AIDifficulty.Hard)
			{
				return this.UpgradePriorityAdvanced(gainType, player);
			}
			return this.UpgradePriorityBasic(gainType, player);
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x000F8434 File Offset: 0x000F6634
		private int UpgradePriorityBasic(GainType gainType, AiPlayer player)
		{
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
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					if (player.player.matFaction.mechs.Count == 0)
					{
						return 80;
					}
					return 64;
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
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 200;
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

		// Token: 0x06002C8D RID: 11405 RVA: 0x000F8D60 File Offset: 0x000F6F60
		public void GainUpgrade(AiPlayer player)
		{
			string text = "Gain Upgrade";
			if (!player.Pay4Action((PayResource)player.AiActions[player.gainUpgradeActionPosition[0]].downAction.GetPayAction(0)))
			{
				text += " ...too poor";
				return;
			}
			GainUpgrade gainUpgrade = (GainUpgrade)player.AiActions[player.gainUpgradeActionPosition[0]].downAction.GetGainAction(0);
			if (gainUpgrade.GainAvaliable())
			{
				this.SetUpgradeActions(player, gainUpgrade);
				this.gameManager.actionManager.PrepareNextAction();
				text += " ...Upgrade performed";
				return;
			}
			text += " ...Upgrade gain unavailable";
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x000F8E10 File Offset: 0x000F7010
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
				if (player.AiActions[i].downAction.GetPayAction(0).CanUpgrade() && (payAction == null || this.UpgradePriority(player.AiActions[i].downAction.GetGainAction(0).GetGainType(), player) > this.UpgradePriority(gainAction2.GetGainType(), player)))
				{
					payAction = player.AiActions[i].downAction.GetPayAction(0);
					gainAction2 = player.AiActions[i].downAction.GetGainAction(0);
				}
			}
			gainUpgrade.SetPayAndGainActions(gainAction, payAction);
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x000F8F68 File Offset: 0x000F7168
		public void GainTrade(AiRecipe receipe, AiPlayer aiPlayer)
		{
			if (!this.topAction.ActionPayed())
			{
				return;
			}
			string text = "Gain Trade: ";
			GainAnyResource gainAnyResource = (GainAnyResource)aiPlayer.AiTopActions[GainType.AnyResource].GetTopGainAction();
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
			this.gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x000F9134 File Offset: 0x000F7334
		public void GainProduce(AiPlayer aiPlayer)
		{
			if (!this.topAction.ActionPayed())
			{
				return;
			}
			GainProduce gainProduce = (GainProduce)aiPlayer.AiTopActions[GainType.Produce].GetTopGainAction();
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
			this.gameManager.actionManager.PrepareNextAction();
			gainProduce.Clear();
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x00044513 File Offset: 0x00042713
		public override string ToString()
		{
			return "top: " + this.topAction.GetGainAction(this.gainActionId).ToString() + " bottom: " + this.downAction.GetGainAction(0).ToString();
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x000F92D8 File Offset: 0x000F74D8
		private int UpgradePriorityAdvanced(GainType gainType, AiPlayer player)
		{
			if (player.player.matFaction.faction == Faction.Saxony && player.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				switch (gainType)
				{
				case GainType.Coin:
					return 150;
				case GainType.Popularity:
					return 180;
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
					return 80;
				case GainType.Building:
					return 40;
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
					return 150;
				case GainType.Power:
					return 190;
				case GainType.CombatCard:
					return 170;
				case GainType.Produce:
					return 160;
				case GainType.Move:
					return 180;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					return 80;
				case GainType.Building:
					return 50;
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
					return 85;
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
					return 90;
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
			if (player.player.matFaction.faction == Faction.Saxony && player.player.matPlayer.matType == PlayerMatType.Mechanical)
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
			if (player.player.matFaction.faction == Faction.Nordic && player.player.matPlayer.matType == PlayerMatType.Engineering)
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
					return 170;
				case GainType.Move:
					return 190;
				case GainType.Upgrade:
					return 90;
				case GainType.Mech:
					if (player.player.matFaction.mechs.Count == 0)
					{
						return 80;
					}
					return 64;
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
					return 90;
				case GainType.Mech:
					return 70;
				case GainType.Building:
					if (player.player.matPlayer.buildings.Count >= 1)
					{
						return 50;
					}
					return 80;
				case GainType.Recruit:
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
					return 150;
				case GainType.Power:
					return 180;
				case GainType.CombatCard:
					return 200;
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
			if (player.player.matFaction.faction == Faction.Togawa && player.player.matPlayer.matType == PlayerMatType.Innovative)
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
					return 150;
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
			if (player.player.matFaction.faction == Faction.Albion && player.player.matPlayer.matType == PlayerMatType.Militant)
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
					return 150;
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
			if (player.player.matFaction.faction == Faction.Albion && player.player.matPlayer.matType == PlayerMatType.Innovative)
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
			if (player.player.matFaction.faction == Faction.Albion && player.player.matPlayer.matType == PlayerMatType.Patriotic)
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

		// Token: 0x06002C93 RID: 11411 RVA: 0x000FA0E8 File Offset: 0x000F82E8
		private void GainMechAdvanced(AiPlayer aiPlayer)
		{
			string text = "Gain Mech";
			if (!aiPlayer.Pay4Action((PayResource)aiPlayer.AiActions[aiPlayer.gainMechActionPosition[0]].downAction.GetPayAction(0)))
			{
				text += " ...too poor";
				return;
			}
			GainMech gainMech = (GainMech)aiPlayer.AiActions[aiPlayer.gainMechActionPosition[0]].downAction.GetGainAction(0);
			if (!gainMech.GainAvaliable())
			{
				text += " ...Mech gain unavailable";
				return;
			}
			Mech mech = new Mech(this.gameManager, this.gameManager.PlayerCurrent, 1);
			GameHex gameHex = aiPlayer.player.matPlayer.workers[0].position;
			if (aiPlayer.strategicAnalysis.preferredDeployPosition != null && aiPlayer.strategicAnalysis.preferredDeployPosition.Owner == aiPlayer.player && aiPlayer.strategicAnalysis.preferredDeployPosition.GetOwnerWorkers().Count > 0)
			{
				gameHex = aiPlayer.strategicAnalysis.preferredDeployPosition;
			}
			else
			{
				for (int i = 0; i < aiPlayer.player.matPlayer.workers.Count; i++)
				{
					if (gameHex.hexType == HexType.capital && aiPlayer.player.matPlayer.workers[i].position != null && aiPlayer.player.matPlayer.workers[i].position.hexType != HexType.capital)
					{
						gameHex = aiPlayer.player.matPlayer.workers[i].position;
					}
				}
				if (aiPlayer.player.matFaction.faction == Faction.Nordic)
				{
					foreach (Worker worker in aiPlayer.player.matPlayer.workers)
					{
						if (worker.position.hexType != HexType.capital && this.MechDeployPriorityV1(worker.position) > this.MechDeployPriorityV1(gameHex))
						{
							gameHex = worker.position;
						}
					}
				}
				if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					foreach (Worker worker2 in aiPlayer.player.matPlayer.workers)
					{
						if (worker2.position.hexType != HexType.capital && this.MechDeployPriorityV2(worker2.position) > this.MechDeployPriorityV2(gameHex))
						{
							gameHex = worker2.position;
						}
					}
				}
				if ((aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural) || aiPlayer.player.matFaction.faction == Faction.Crimea)
				{
					foreach (Worker worker3 in aiPlayer.player.matPlayer.workers)
					{
						if (worker3.position.hexType != HexType.capital && this.MechDeployPriorityV3(worker3.position) > this.MechDeployPriorityV3(gameHex))
						{
							gameHex = worker3.position;
						}
					}
				}
				if ((aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant) || (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative))
				{
					foreach (Worker worker4 in aiPlayer.player.matPlayer.workers)
					{
						if (worker4.position.hexType != HexType.capital && this.MechDeployPriorityV4(worker4.position) > this.MechDeployPriorityV4(gameHex))
						{
							gameHex = worker4.position;
						}
					}
				}
			}
			if (gameHex.hexType != HexType.capital)
			{
				gainMech.SetMechAndLocation(mech, gameHex, aiPlayer.strategicAnalysis.mechNext);
				this.gameManager.actionManager.PrepareNextAction();
				text = text + " ...Mech " + mech.Owner.matFaction.abilities[mech.Id].ToString() + " deployed";
				return;
			}
			text += "...No suitable place to Deploy";
		}

		// Token: 0x04001E72 RID: 7794
		private GameManager gameManager;

		// Token: 0x04001E73 RID: 7795
		public readonly int matSectionId;

		// Token: 0x04001E74 RID: 7796
		public readonly TopAction topAction;

		// Token: 0x04001E75 RID: 7797
		public readonly int gainActionId;

		// Token: 0x04001E76 RID: 7798
		public readonly DownAction downAction;

		// Token: 0x02000567 RID: 1383
		// (Invoke) Token: 0x06002C95 RID: 11413
		public delegate void ActionExecute(AiRecipe recipe, AiPlayer player);
	}
}

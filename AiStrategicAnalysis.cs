using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005AE RID: 1454
	public class AiStrategicAnalysis
	{
		// Token: 0x06002DFC RID: 11772 RVA: 0x00112574 File Offset: 0x00110774
		public AiStrategicAnalysis(GameManager gameManager)
		{
			this.gameManager = gameManager;
			this.spreadingTriggered = false;
		}

		public bool spreadingTriggered;
		public bool winDesperation;

		// Token: 0x06002DFD RID: 11773 RVA: 0x00112770 File Offset: 0x00110970
		public virtual void Run(AiPlayer aiPlayer, int priMoveToProduction1, int priMoveToProduction2, int priMoveToEncounter, int priMoveToFight, int priMoveToBuild)
		{
			this.workersOutOfBase = 0;
			this.workersInaVillage = 0;
			this.priorityFight = priMoveToFight;
			this.preferredDeployPosition = null;
			this.UpdateEncounterAndFactory(aiPlayer);
			this.UpdateResourceAccess(aiPlayer);
			this.UpdateResourceDemand(aiPlayer);
			this.UpdateTradeLoop(aiPlayer);
			this.UpdateResourceDemandPriority(aiPlayer, this.resourceDemandTotal);
			this.SortResourcePriority();
			this.UpdateProduceLoop(aiPlayer);
			this.canUpgrade = aiPlayer.player.GetNumberOfStars(StarType.Upgrades) == 0 && aiPlayer.CanPlayerPayPredictive(aiPlayer.AiActions[aiPlayer.gainUpgradeActionPosition[0]]);
			this.canDeploy = aiPlayer.player.GetNumberOfStars(StarType.Mechs) == 0 && aiPlayer.CanPlayerPayPredictive(aiPlayer.AiActions[aiPlayer.gainMechActionPosition[0]]);
			this.canBuild = aiPlayer.player.GetNumberOfStars(StarType.Structures) == 0 && aiPlayer.CanPlayerPayPredictive(aiPlayer.AiActions[aiPlayer.gainBuildingActionPosition[0]]) && ((GainBuilding)aiPlayer.AiActions[aiPlayer.gainBuildingActionPosition[0]].downAction.GetGainAction(0)).GainAvaliable();
			this.canEnlist = aiPlayer.player.GetNumberOfStars(StarType.Recruits) == 0 && aiPlayer.CanPlayerPayPredictive(aiPlayer.AiActions[aiPlayer.gainRecruitActionPosition[0]]);
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.gottaMoveToBuild = !this.canBuild && aiPlayer.player.GetNumberOfStars(StarType.Structures) == 0 && aiPlayer.CanPlayerPayPredictive(aiPlayer.AiActions[aiPlayer.gainBuildingActionPosition[0]]) && aiPlayer.CanPlayTopAction(GainType.Move);
			}
			else
			{
				this.gottaMoveToBuild = false;
			}
			this.UpdateWorkerCountTarget(aiPlayer);
			this.UpdateUselessWorkers4Production(aiPlayer);
			this.UpdateMoveRangeAll(aiPlayer);
			this.UpdateMoveTargets(aiPlayer, priMoveToProduction1, priMoveToProduction2, priMoveToEncounter, priMoveToFight, priMoveToBuild);
			this.UpdateBuildingOrder(aiPlayer);
			this.UpdateMechOrder(aiPlayer);
			this.UpdateRecruitOrder(aiPlayer);
			this.UpdateRecruitOneTimeOrder(aiPlayer);
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x0011297C File Offset: 0x00110B7C
		protected void UpdateProduceLoop(AiPlayer aiPlayer)
		{
			int amount = (int)aiPlayer.AiTopActions[GainType.Produce].downAction.GetPayAction(0).Amount;
			DownActionType type = aiPlayer.AiTopActions[GainType.Produce].downAction.Type;
			ResourceType resourceType = ResourceType.combatCard;
			switch (type)
			{
			case DownActionType.Upgrade:
				resourceType = ResourceType.oil;
				break;
			case DownActionType.Deploy:
				resourceType = ResourceType.metal;
				break;
			case DownActionType.Build:
				resourceType = ResourceType.wood;
				break;
			case DownActionType.Enlist:
				resourceType = ResourceType.food;
				break;
			}
			this.produceLoopPresent = amount <= this.resourceAccess[resourceType] && aiPlayer.AiTopActions[GainType.Produce].downAction.GetGainAction(0).GainAvaliable();
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x00112A1C File Offset: 0x00110C1C
		protected void UpdateTradeLoop(AiPlayer aiPlayer)
		{
			int amount = (int)aiPlayer.AiTopActions[GainType.AnyResource].downAction.GetPayAction(0).Amount;
			switch (aiPlayer.AiTopActions[GainType.AnyResource].downAction.Type)
			{
			case DownActionType.Upgrade:
				this.tradeLoopResource = ResourceType.oil;
				break;
			case DownActionType.Deploy:
				this.tradeLoopResource = ResourceType.metal;
				break;
			case DownActionType.Build:
				this.tradeLoopResource = ResourceType.wood;
				break;
			case DownActionType.Enlist:
				this.tradeLoopResource = ResourceType.food;
				break;
			}
			if ((aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant) || (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant))
			{
				this.tradeLoopPresent = amount <= 2 + aiPlayer.player.Resources(false)[this.tradeLoopResource] && aiPlayer.AiTopActions[GainType.AnyResource].downAction.GetGainAction(0).GainAvaliable() && this.resourceDemandTotal[this.tradeLoopResource] > 0;
				return;
			}
			this.tradeLoopPresent = amount <= 3 && aiPlayer.AiTopActions[GainType.AnyResource].downAction.GetGainAction(0).GainAvaliable() && this.resourceDemandTotal[this.tradeLoopResource] > 0;
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x00112B9C File Offset: 0x00110D9C
		protected void UpdateResourceAccess(AiPlayer aiPlayer)
		{
			this.resourceAccess[ResourceType.oil] = 0;
			this.resourceAccess[ResourceType.wood] = 0;
			this.resourceAccess[ResourceType.metal] = 0;
			this.resourceAccess[ResourceType.food] = 0;
			foreach (Worker worker in aiPlayer.player.matPlayer.workers)
			{
				if (worker.position != null)
				{
					ResourceType resourceType = AiStrategicAnalysis.ResourceProduced(worker.position.hexType);
					if (this.resourceAccess.ContainsKey(resourceType))
					{
						Dictionary<ResourceType, int> dictionary = this.resourceAccess;
						ResourceType resourceType2 = resourceType;
						int num = dictionary[resourceType2];
						dictionary[resourceType2] = num + 1;
					}
				}
			}
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x00112C68 File Offset: 0x00110E68
		protected void UpdateResourceDemand(AiPlayer aiPlayer)
		{
			this.resourceCostSingleAction[ResourceType.oil] = (int)aiPlayer.AiActions[aiPlayer.gainUpgradeActionPosition[0]].downAction.GetPayAction(0).Amount;
			this.resourceCostSingleAction[ResourceType.metal] = (int)aiPlayer.AiActions[aiPlayer.gainMechActionPosition[0]].downAction.GetPayAction(0).Amount;
			this.resourceCostSingleAction[ResourceType.wood] = (int)aiPlayer.AiActions[aiPlayer.gainBuildingActionPosition[0]].downAction.GetPayAction(0).Amount;
			this.resourceCostSingleAction[ResourceType.food] = (int)aiPlayer.AiActions[aiPlayer.gainRecruitActionPosition[0]].downAction.GetPayAction(0).Amount;
			int num = this.resourceCostSingleAction[ResourceType.oil] * (6 - aiPlayer.player.matPlayer.UpgradesDone);
			int num2 = this.resourceCostSingleAction[ResourceType.metal] * (4 - aiPlayer.player.matFaction.mechs.Count);
			int num3 = this.resourceCostSingleAction[ResourceType.wood] * (4 - aiPlayer.player.matPlayer.buildings.Count);
			if (aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Albion) num3 = 0; // Crimea, Rusviet, and Albion ignore wood and buildings
			int num4 = this.resourceCostSingleAction[ResourceType.food] * (4 - aiPlayer.player.matPlayer.RecruitsEnlisted);
			Dictionary<ResourceType, int> dictionary = aiPlayer.player.Resources(false);
			this.resourceDemandTotal[ResourceType.oil] = num - dictionary[ResourceType.oil];
			this.resourceDemandTotal[ResourceType.metal] = num2 - dictionary[ResourceType.metal];
			this.resourceDemandTotal[ResourceType.wood] = num3 - dictionary[ResourceType.wood];
			this.resourceDemandTotal[ResourceType.food] = num4 - dictionary[ResourceType.food];
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x00112E34 File Offset: 0x00111034
		protected virtual void UpdateResourceDemandPriority(AiPlayer aiPlayer, Dictionary<ResourceType, int> resourceDemand)
		{
			this.resourceDemandPriority[ResourceType.oil] = 5;
			this.resourceDemandPriority[ResourceType.food] = 4;
			this.resourceDemandPriority[ResourceType.metal] = 3;
			this.resourceDemandPriority[ResourceType.wood] = 2;
			Faction faction = aiPlayer.player.matFaction.faction;
			if (faction != Faction.Polania)
			{
				if (faction != Faction.Nordic)
				{
					if (faction == Faction.Saxony)
					{
						if (aiPlayer.player.matFaction.mechs.Count <= 1)
						{
							this.resourceDemandPriority[ResourceType.metal] = 10;
						}
						if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
						{
							this.resourceDemandPriority[ResourceType.food] = 5;
							this.resourceDemandPriority[ResourceType.metal] = 4;
							this.resourceDemandPriority[ResourceType.oil] = 3;
							this.resourceDemandPriority[ResourceType.wood] = 2;
						}
						if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
						{
							this.resourceDemandPriority[ResourceType.metal] = 5;
							this.resourceDemandPriority[ResourceType.wood] = 4;
							this.resourceDemandPriority[ResourceType.oil] = 3;
							this.resourceDemandPriority[ResourceType.food] = 2;
						}
					}
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic || aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					this.resourceDemandPriority[ResourceType.wood] = 8;
					this.resourceDemandPriority[ResourceType.food] = 9;
					if (this.resourceAccess[ResourceType.food] > 0)
					{
						this.resourceDemandPriority[ResourceType.metal] = 10;
					}
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					if (this.tradeLoopPresent)
					{
						this.resourceDemandPriority[this.tradeLoopResource] = 1;
					}
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					if (aiPlayer.player.matPlayer.UpgradesDone >= 3)
					{
						this.resourceDemandPriority[ResourceType.food] = 10;
					}
				}
				
				// Apply Metal priority boost if low on mechs
				if (aiPlayer.player.matFaction.mechs.Count <= 1)
				{
					this.resourceDemandPriority[ResourceType.metal] = 8;
				}

				// Nordic Patriotic/Agricultural specific overrides
				if (faction == Faction.Nordic && (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic || aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural))
				{
					this.resourceDemandPriority[ResourceType.wood] = 8;
					this.resourceDemandPriority[ResourceType.food] = 9;
					if (this.resourceAccess[ResourceType.food] > 0)
					{
						this.resourceDemandPriority[ResourceType.metal] = 10;
					}
				}
			}
			else
			{
				if (aiPlayer.player.matPlayer.buildings.Count == 0)
				{
					this.resourceDemandPriority[ResourceType.wood] = 10;
				}
				if (aiPlayer.player.matFaction.mechs.Count == 0)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
				}
			}
			foreach (ResourceType resourceType in resourceDemand.Keys)
			{
				if (resourceDemand[resourceType] <= 0)
				{
					this.resourceDemandPriority[resourceType] = 0;
				}
			}
			this.resourceHighestPriority = ResourceType.oil;
			this.resourceHighestPriorityNoProduce = ResourceType.oil;
			if (this.resourceDemandPriority.Values.Max() > 0)
			{
				foreach (ResourceType resourceType2 in resourceDemand.Keys)
				{
					if (this.resourceDemandPriority[resourceType2] > this.resourceDemandPriority[this.resourceHighestPriority])
					{
						this.resourceHighestPriority = resourceType2;
					}
					if ((this.resourceDemandPriority[resourceType2] > this.resourceDemandPriority[this.resourceHighestPriorityNoProduce] || this.resourceAccess[this.resourceHighestPriorityNoProduce] > 0) && (this.resourceAccess[resourceType2] == 0 || (this.resourceDemandPriority[resourceType2] >= 10 && this.resourceDemandPriority[this.resourceHighestPriorityNoProduce] < 10)))
					{
						this.resourceHighestPriorityNoProduce = resourceType2;
					}
				}
			}
			if (this.resourceDemandPriority[this.resourceHighestPriority] == 0)
			{
				this.resourceHighestPriority = ResourceType.combatCard;
			}
			if (this.resourceDemandPriority[this.resourceHighestPriorityNoProduce] == 0 || this.resourceAccess[this.resourceHighestPriorityNoProduce] > 0)
			{
				this.resourceHighestPriorityNoProduce = ResourceType.combatCard;
			}
		}

		public void SortResourcePriority()
		{
			this.resourcePrioritySorted = new SortedList<int, ResourceType>(new InvertedComparer());
			foreach (ResourceType resourceType in this.resourceDemandPriority.Keys)
			{
				int num = this.resourceDemandPriority[resourceType];
				while (this.resourcePrioritySorted.ContainsKey(num))
				{
					num--;
				}
				this.resourcePrioritySorted.Add(num, resourceType);
			}
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x000448CA File Offset: 0x00042ACA
		public static ResourceType ResourceProduced(HexType hex)
		{
			switch (hex)
			{
			case HexType.mountain:
				return ResourceType.metal;
			case HexType.forest:
				return ResourceType.wood;
			case (HexType)3:
				break;
			case HexType.farm:
				return ResourceType.food;
			default:
				if (hex == HexType.tundra)
				{
					return ResourceType.oil;
				}
				break;
			}
			return ResourceType.combatCard;
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x0011321C File Offset: 0x0011141C
		protected bool HasNeighbourTunel(AiPlayer aiPlayer, GameHex hex)
		{
			Dictionary<GameHex, int> dictionary;
			this.gameManager.gameBoard.MoveRange(aiPlayer.player.character, hex, 1, out dictionary);
			foreach (GameHex gameHex in dictionary.Keys)
			{
				if (dictionary[gameHex] == 1 && gameHex.hasTunnel)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x001132A4 File Offset: 0x001114A4
		protected virtual float ResourcePriority(AiPlayer aiPlayer, GameHex hex)
		{
			if (this.resourceDemandPriority.ContainsKey(AiStrategicAnalysis.ResourceProduced(hex.hexType)))
			{
				float num = (float)this.resourceDemandPriority[AiStrategicAnalysis.ResourceProduced(hex.hexType)];
				if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && this.HasNeighbourTunel(aiPlayer, hex))
				{
					num += 0.4f;
				}
				if (hex.hasTunnel && ((aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)))
				{
					num += 0.5f;
				}
				if (this.gottaMoveToBuild && hex.Building == null)
				{
					float num2 = 100f;
					if (aiPlayer.strategicAnalysis.buildingNext == BuildingType.Mill && hex.hexType == HexType.village)
					{
						num2 -= 500f;
						if (aiPlayer.player.matPlayer.workers.Count >= 4)
						{
							num2 -= 1000f;
						}
					}
					num += num2;
				}
				return num;
			}
			if (hex.hexType == HexType.village && aiPlayer.player.matPlayer.workers.Count < this.workerCountTarget)
			{
				return 10f;
			}
			return 0f;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x00113454 File Offset: 0x00111654
		protected virtual void UpdateUselessWorkers4Production(AiPlayer aiPlayer)
		{
			this.uselessWorkers4Production.Clear();
			this.uselessWorkersTargets.Clear();
			this.moveMechPassengers.Clear();
			int bottomStarsEarned = aiPlayer.player.stars[StarType.Upgrades] + aiPlayer.player.stars[StarType.Mechs] + aiPlayer.player.stars[StarType.Structures] + aiPlayer.player.stars[StarType.Recruits] + aiPlayer.player.stars[StarType.Workers];
			bool spreadingTriggered = bottomStarsEarned >= 3;
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.GetNumberOfStars(StarType.Mechs) > 0 && aiPlayer.player.GetNumberOfStars(StarType.Workers) > 0)
			{
				spreadingTriggered = true;
			}
			foreach (Worker worker in aiPlayer.player.matPlayer.workers)
			{
				if (worker.position != null)
				{
					bool isUseless = this.ResourcePriority(aiPlayer, worker.position) <= 0f;
					if (spreadingTriggered)
					{
						int workersOnHex = worker.position.GetOwnerWorkers().Count;
						int uselessOnHex = this.uselessWorkers4Production.Count((Worker w) => w.position == worker.position);
						if (uselessOnHex + 1 < workersOnHex)
						{
							isUseless = true;
						}
					}
					else if (isUseless)
					{
						// Even if not globally triggered, if a hex is "useless" (resource not needed), 
						// only mark workers as useless if we can leave one behind.
						int workersOnHex = worker.position.GetOwnerWorkers().Count;
						int uselessOnHex = this.uselessWorkers4Production.Count((Worker w) => w.position == worker.position);
						if (uselessOnHex + 1 >= workersOnHex)
						{
							isUseless = false;
						}
					}
					if (isUseless)
					{
						if (worker.position.GetResourceCount() > 0)
						{
							int workersOnHex2 = worker.position.GetOwnerWorkers().Count;
							int mechsOnHex = worker.position.GetOwnerMechs().Count;
							int uselessOnHex2 = this.uselessWorkers4Production.Count((Worker w) => w.position == worker.position);
							if (uselessOnHex2 + 1 >= workersOnHex2 && mechsOnHex == 0)
							{
								continue;
							}
						}
						this.uselessWorkers4Production.Add(worker);
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				List<Worker> list = new List<Worker>();
				foreach (Worker worker2 in aiPlayer.player.matPlayer.workers)
				{
					if (worker2.position.hexType == HexType.forest && worker2.position.Building != null && worker2.position.Building.buildingType == BuildingType.Mill)
					{
						list.Add(worker2);
					}
				}
				if (list.Count > 1)
				{
					list.RemoveRange(0, 1);
				}
				foreach (Worker worker3 in list)
				{
					if (!this.uselessWorkers4Production.Contains(worker3))
					{
						this.uselessWorkers4Production.Add(worker3);
					}
				}
			}
			foreach (Worker worker4 in this.uselessWorkers4Production)
			{
				if (worker4.position.Owner == aiPlayer.player && worker4.position.GetOwnerMechs().Count > 0)
				{
					Mech mech = worker4.position.GetOwnerMechs()[0];
					if (!this.moveMechPassengers.ContainsKey(mech))
					{
						this.moveMechPassengers.Add(mech, new List<Worker>());
					}
					this.moveMechPassengers[mech].Add(worker4);
				}
				else
				{
					GameHex gameHex = null;
					foreach (GameHex gameHex2 in worker4.position.GetFieldsAccessible(worker4, false))
					{
						if (this.ResourcePriority(aiPlayer, gameHex2) > 0f && (gameHex2.Owner == null || gameHex2.Owner == aiPlayer.player) && (gameHex == null || this.ResourcePriority(aiPlayer, gameHex2) > this.ResourcePriority(aiPlayer, gameHex)))
						{
							gameHex = gameHex2;
						}
					}
					if (gameHex != null)
					{
						this.uselessWorkersTargets.Add(worker4, gameHex);
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering && aiPlayer.player.matFaction.mechs.Count > 0 && this.resourceAccess[ResourceType.oil] == 1 && this.resourceCostSingleAction[ResourceType.oil] > 1)
			{
				Mech mech2 = aiPlayer.player.matFaction.mechs[0];
				if (this.moveMechPassengers.ContainsKey(mech2) && this.moveMechPassengers[mech2].Count > 0 && mech2.position.hexType == HexType.village)
				{
					GameHex gameHex3 = null;
					foreach (GameHex gameHex4 in this.moveMechPassengers[mech2][0].position.GetFieldsAccessible(this.moveMechPassengers[mech2][0], false))
					{
						if (gameHex4.hexType == HexType.tundra && (gameHex4.Owner == null || gameHex4.Owner == aiPlayer.player) && gameHex3 == null)
						{
							gameHex3 = gameHex4;
						}
					}
					if (gameHex3 != null)
					{
						Worker worker5 = this.moveMechPassengers[mech2][0];
						this.moveMechPassengers[mech2].RemoveAt(0);
						this.uselessWorkersTargets.Add(worker5, gameHex3);
					}
				}
			}
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x00113934 File Offset: 0x00111B34
		protected void UpdateEncounterAndFactory(AiPlayer aiPlayer)
		{
			this.gameManager.gameBoard.MoveRange(aiPlayer.player.character, 3, out this.characterDistance);
			this.encounterNearestHex = null;
			this.factoryDistance = int.MaxValue;
			foreach (KeyValuePair<GameHex, int> keyValuePair in this.characterDistance)
			{
				if (keyValuePair.Key.hasEncounter && !keyValuePair.Key.encounterUsed && (keyValuePair.Key.Owner == null || keyValuePair.Key.Owner == aiPlayer.player) && (this.encounterNearestHex == null || keyValuePair.Value < this.characterDistance[this.encounterNearestHex]))
				{
					this.encounterNearestHex = keyValuePair.Key;
				}
				if (keyValuePair.Key.hexType == HexType.factory && (keyValuePair.Key.Owner == null || keyValuePair.Key.Owner == aiPlayer.player))
				{
					this.factoryDistance = this.characterDistance[keyValuePair.Key];
				}
			}
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x00113A74 File Offset: 0x00111C74
		protected virtual float isWorthAttacking(GameHex hex, AiPlayer aiPlayer)
		{
			if (aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Crimea)
			{
				return (float)(2 - hex.GetOwnerWorkers().Count);
			}
			if ((aiPlayer.player.matFaction.faction != Faction.Saxony || aiPlayer.player.matPlayer.matType != PlayerMatType.Industrial) && (aiPlayer.player.matFaction.faction != Faction.Polania || aiPlayer.player.matPlayer.matType != PlayerMatType.Industrial))
			{
				return 1f;
			}
			int num = hex.GetOwnerMechs().Count<Mech>();
			if (hex.HasOwnerCharacter())
			{
				num++;
			}
			if (hex.Owner.matFaction.faction == Faction.Rusviet && hex.Owner.matFaction.SkillUnlocked[2] && hex.GetOwnerWorkers().Count<Worker>() > 0)
			{
				num++;
			}
			int num2 = num;
			if (num2 > hex.Owner.combatCards.Count<CombatCard>())
			{
				num2 = hex.Owner.combatCards.Count<CombatCard>();
			}
			int num3 = Math.Min(7, hex.Owner.Power);
			num3 += num2 * 5;
			if (hex.Owner.matFaction.faction == Faction.Albion && hex.Owner.matFaction.SkillUnlocked[1])
			{
				num3 += 2;
			}
			if (hex.Owner.matFaction.faction == Faction.Togawa && hex.Owner.matFaction.SkillUnlocked[2] && hex.GetOwnerWorkers().Count == 0 && (hex.GetOwnerMechs().Count > 0 || hex.HasOwnerCharacter()))
			{
				num3 += 2;
			}
			int num4 = 0;
			for (int i = 0; i < this.enemyCanBeAttackedBy[hex].Count; i++)
			{
				if (!this.movePriority.ContainsKey(this.enemyCanBeAttackedBy[hex][i]))
				{
					num4++;
				}
			}
			if (num4 > 2)
			{
				num4 = 2;
			}
			int num5 = num4;
			if (num5 > aiPlayer.player.combatCards.Count)
			{
				num5 = aiPlayer.player.combatCards.Count;
			}
			int num6 = Math.Min(7, aiPlayer.player.Power);
			num6 += num5 * 5;
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matFaction.SkillUnlocked[2])
			{
				num3 -= 2;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matFaction.SkillUnlocked[0])
			{
				num3 -= 2;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matFaction.SkillUnlocked[2] && (hex.hasTunnel || (hex.Building != null && hex.Building.buildingType == BuildingType.Mine)))
			{
				num3 -= 2;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matFaction.SkillUnlocked[2])
			{
				num5++;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matFaction.SkillUnlocked[2])
			{
				num5++;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Togawa)
			{
				bool flag = aiPlayer.player.matFaction.SkillUnlocked[0];
			}
			num6 = Math.Min(7, aiPlayer.player.Power);
			num6 += Math.Min(num5, aiPlayer.player.combatCards.Count) * 5;
			if (num3 > num6 + 5)
			{
				return 0f;
			}
			if (num2 > num5)
			{
				return 0f;
			}
			if (num2 == num5 && hex.Owner.Power >= aiPlayer.player.Power && hex.Owner.combatCards.Count >= aiPlayer.player.combatCards.Count)
			{
				return 0f;
			}
			float num7 = (float)(hex.GetResourceCount() / 2);
			if (aiPlayer.player.matFaction.faction == Faction.Saxony)
			{
				num7 += 1.51f - 1.5f * (float)hex.GetOwnerWorkers().Count;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matFaction.SkillUnlocked[2])
			{
				num7 += (float)(hex.GetOwnerWorkers().Count / 8);
			}
			else
			{
				num7 += (float)(2.01f - hex.GetOwnerWorkers().Count);
			}
			if (num7 > 0f && this.enemyCanBeAttackedBy[hex].Count<Unit>() > 1)
			{
				num7 += 10f;
			}
			return num7;
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x00113F00 File Offset: 0x00112100
		protected void UpdateMoveRangeAll(AiPlayer aiPlayer)
		{
			this.moveRangeAll.Clear();
			Dictionary<GameHex, int> dictionary;
			this.gameManager.gameBoard.MoveRange(aiPlayer.player.character, (int)aiPlayer.player.character.MaxMoveCount, out dictionary);
			this.moveRangeAll.Add(aiPlayer.player.character, dictionary);
			foreach (Mech mech in aiPlayer.player.matFaction.mechs)
			{
				Dictionary<GameHex, int> dictionary2;
				this.gameManager.gameBoard.MoveRange(mech, (int)mech.MaxMoveCount, out dictionary2);
				this.moveRangeAll.Add(mech, dictionary2);
			}
			foreach (Worker worker in aiPlayer.player.matPlayer.workers)
			{
				Dictionary<GameHex, int> dictionary3;
				this.gameManager.gameBoard.MoveRange(worker, 1, out dictionary3);
				this.moveRangeAll.Add(worker, dictionary3);
			}
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x0011403C File Offset: 0x0011223C
		protected virtual void UpdateMoveTargets(AiPlayer aiPlayer, int priProduce1, int priProduce2, int priEncounter, int priFight, int priBuildSpot)
		{
			this.movePriority.Clear();
			this.moveTarget.Clear();
			this.movePrioritySorted.Clear();
			this.moveDistance.Clear();
			this.enemyCanBeAttackedBy.Clear();
			this.moveRange.Clear();
			foreach (KeyValuePair<Worker, GameHex> keyValuePair in this.uselessWorkersTargets)
			{
				int num = ((this.uselessWorkers4Production.Count < 2) ? priProduce1 : priProduce2);
				if (this.gottaMoveToBuild && priBuildSpot > num)
				{
					num = priBuildSpot;
				}
				this.movePriority.Add(keyValuePair.Key, num);
				this.moveTarget.Add(keyValuePair.Key, new List<GameHex> { keyValuePair.Value });
				this.moveDistance.Add(keyValuePair.Key, 1);
			}
			foreach (Mech mech in aiPlayer.player.matFaction.mechs)
			{
				Dictionary<GameHex, int> dictionary;
				this.gameManager.gameBoard.MoveRange(mech, (int)mech.MaxMoveCount, out dictionary);
				this.moveRange.Add(mech, dictionary);
				foreach (GameHex gameHex in this.moveRange[mech].Keys)
				{
					if (gameHex.Owner != null && gameHex.Owner != aiPlayer.player && gameHex.GetOwnerUnitCount() > 0)
					{
						if (!this.enemyCanBeAttackedBy.ContainsKey(gameHex))
						{
							this.enemyCanBeAttackedBy.Add(gameHex, new List<Unit>());
						}
						this.enemyCanBeAttackedBy[gameHex].Add(mech);
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				Dictionary<GameHex, int> dictionary2;
				this.gameManager.gameBoard.MoveRange(aiPlayer.player.character, (int)aiPlayer.player.character.MaxMoveCount, out dictionary2);
				this.moveRange.Add(aiPlayer.player.character, dictionary2);
				foreach (GameHex gameHex2 in this.moveRange[aiPlayer.player.character].Keys)
				{
					if (gameHex2.Owner != null && gameHex2.Owner != aiPlayer.player && gameHex2.GetOwnerUnitCount() > 0)
					{
						if (!this.enemyCanBeAttackedBy.ContainsKey(gameHex2))
						{
							this.enemyCanBeAttackedBy.Add(gameHex2, new List<Unit>());
						}
						this.enemyCanBeAttackedBy[gameHex2].Add(aiPlayer.player.character);
					}
				}
			}
			foreach (Mech mech2 in this.moveMechPassengers.Keys)
			{
				GameHex gameHex3 = null;
				foreach (GameHex gameHex4 in this.moveRange[mech2].Keys)
				{
					if (this.ResourcePriority(aiPlayer, gameHex4) > 0f && (gameHex4.Owner == null || gameHex4.Owner == aiPlayer.player) && (gameHex3 == null || this.ResourcePriority(aiPlayer, gameHex4) > this.ResourcePriority(aiPlayer, gameHex3)))
					{
						gameHex3 = gameHex4;
					}
				}
				if (gameHex3 != null)
				{
					bool flag = false;
					GameHex gameHex5 = null;
					GameHex gameHex6 = null;
					int bottomStarsEarned2 = aiPlayer.player.stars[StarType.Upgrades] + aiPlayer.player.stars[StarType.Mechs] + aiPlayer.player.stars[StarType.Structures] + aiPlayer.player.stars[StarType.Recruits] + aiPlayer.player.stars[StarType.Workers];
					bool spreadingTriggered2 = bottomStarsEarned2 >= 3;
					if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.GetNumberOfStars(StarType.Mechs) > 0 && aiPlayer.player.GetNumberOfStars(StarType.Workers) > 0)
					{
						spreadingTriggered2 = true;
					}
					if (spreadingTriggered2 && aiPlayer.player.matFaction.SkillUnlocked[3])
					{
						foreach (GameHex gameHex7 in this.moveRange[mech2].Keys)
						{
							if (this.ResourcePriority(aiPlayer, gameHex7) > 0f && (gameHex7.Owner == null || gameHex7.Owner == aiPlayer.player) && this.moveRange[mech2][gameHex7] == 1 && AiStrategicAnalysis.ResourceProduced(gameHex7.hexType) != ResourceType.combatCard && (gameHex5 == null || this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex7.hexType)] <= this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex5.hexType)]) && (gameHex5 == null || this.ResourcePriority(aiPlayer, gameHex7) > this.ResourcePriority(aiPlayer, gameHex5)))
							{
								gameHex5 = gameHex7;
							}
						}
						
						if (gameHex5 != null)
						{
							ResourceType resourceType = AiStrategicAnalysis.ResourceProduced(gameHex5.hexType);
							int num2 = this.resourceCostSingleAction[resourceType];
							if (this.moveMechPassengers[mech2].Count > num2)
							{
								Dictionary<GameHex, int> dictionary3;
								this.gameManager.gameBoard.MoveRange(mech2, gameHex5, 1, out dictionary3);
								foreach (GameHex gameHex8 in dictionary3.Keys)
								{
									if (this.ResourcePriority(aiPlayer, gameHex8) > 0f && (gameHex8.Owner == null || gameHex8.Owner == aiPlayer.player) && gameHex8.hexType != gameHex5.hexType && AiStrategicAnalysis.ResourceProduced(gameHex8.hexType) != ResourceType.combatCard && this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex8.hexType)] == 0 && (gameHex6 == null || this.ResourcePriority(aiPlayer, gameHex8) > this.ResourcePriority(aiPlayer, gameHex6)))
									{
										gameHex6 = gameHex8;
									}
								}
								if (gameHex6 != null)
								{
									flag = true;
								}
							}
						}
					}
					IL_072F:
					if (flag)
					{
						this.movePriority.Add(mech2, (this.moveMechPassengers[mech2].Count < 2) ? priProduce1 : (priProduce2 + 2));
						this.moveTarget.Add(mech2, new List<GameHex> { gameHex5, gameHex6 });
						this.moveDistance.Add(mech2, 2);
					}
					else
					{
						this.movePriority.Add(mech2, (this.moveMechPassengers[mech2].Count < 2) ? priProduce1 : (priProduce2 + 2));
						this.moveTarget.Add(mech2, new List<GameHex> { gameHex3 });
						this.moveDistance.Add(mech2, this.moveRange[mech2][gameHex3]);
					}
					if (mech2.position.GetOwnerMechs().Count > 1 && aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
					{
						Mech mech3 = null;
						foreach (Mech mech4 in mech2.position.GetOwnerMechs())
						{
							if (mech4 != mech2)
							{
								mech3 = mech4;
							}
						}
						if (mech3 != null && !this.moveTarget.ContainsKey(mech3))
						{
							this.movePriority.Add(mech3, 1);
							this.moveTarget.Add(mech3, new List<GameHex> { gameHex3 });
							this.moveDistance.Add(mech3, this.moveRange[mech3][gameHex3]);
						}
					}
				}
			}
			bool isHoardingPower = aiPlayer.player.Power >= 10 && aiPlayer.player.GetNumberOfStars(StarType.Combat) < 2 && aiPlayer.player.matFaction.faction != Faction.Saxony;
			if (priFight > 0 && aiPlayer.strategicAnalysis.enemyCanBeAttackedBy.Count > 0 && (aiPlayer.player.Power >= 7 || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)) && (aiPlayer.player.GetNumberOfStars(StarType.Combat) < 2 || aiPlayer.player.matFaction.factionPerk == AbilityPerk.Dominate))
			{
				GameHex gameHex10 = null;
				float num3 = 0f;
				if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					using (Dictionary<GameHex, List<Unit>>.KeyCollection.Enumerator enumerator6 = this.enemyCanBeAttackedBy.Keys.GetEnumerator())
					{
						while (enumerator6.MoveNext())
						{
							GameHex gameHex11 = enumerator6.Current;
							if (this.isWorthAttacking(gameHex11, aiPlayer) > num3 && aiPlayer.CombatPreparation(true, gameHex11, gameHex11.Owner))
							{
								gameHex10 = gameHex11;
								num3 = this.isWorthAttacking(gameHex11, aiPlayer);
							}
						}
						goto IL_0A47;
					}
				}
				foreach (GameHex gameHex12 in this.enemyCanBeAttackedBy.Keys)
				{
					// Step 2c / 2d: Weak Defender Exception
					if (isHoardingPower)
					{
						int enemyUnits = (gameHex12.HasOwnerCharacter() ? 1 : 0) + gameHex12.GetOwnerMechs().Count;
						if (enemyUnits > 1 || gameHex12.Owner.Power >= 3)
						{
							continue; // Skip attacking if hoarding and the target is not weak
						}
					}
					
					if (this.isWorthAttacking(gameHex12, aiPlayer) > 0f)
					{
						gameHex10 = gameHex12;
					}
				}
				IL_0A47:
				if (gameHex10 != null)
				{
					int amount = (int)((GainMove)aiPlayer.AiTopActions[GainType.Move].GetTopGainAction()).Amount;
					int num4 = 0;
					List<Unit> list = new List<Unit>();
					foreach (Unit unit in this.enemyCanBeAttackedBy[gameHex10])
					{
						if (!this.movePriority.ContainsKey(unit) && (unit.UnitType == UnitType.Mech || unit.UnitType == UnitType.Character))
						{
							list.Add(unit);
						}
					}
					list.Sort(delegate(Unit a, Unit b)
					{
						if (a.UnitType == b.UnitType)
						{
							return 0;
						}
						if (a.UnitType == UnitType.Character)
						{
							return -1;
						}
						if (b.UnitType == UnitType.Character)
						{
							return 1;
						}
						return 0;
					});
					Unit unit2 = null;
					
					// Step 2d: Crimea Weak Defender Max Combat Cards
					if (isHoardingPower && aiPlayer.player.matFaction.faction == Faction.Crimea)
					{
						// Crimea wants to send exactly 2-3 units (usually 3 if available) to combat when hoarding (weak defender) to maximize CCs
						amount = Math.Min(amount, 3);
					}
					
					foreach (Unit unit3 in list)
					{
						if (num4 >= amount)
						{
							break;
						}
						this.movePriority.Add(unit3, priFight);
						this.moveTarget.Add(unit3, new List<GameHex> { gameHex10 });
						this.moveDistance.Add(unit3, this.moveRange[unit3][gameHex10]);
						if (unit2 == null)
						{
							unit2 = unit3;
						}
						num4++;
					}
					
					if (unit2 != null && unit2.UnitType == UnitType.Mech && aiPlayer.player.matFaction.abilities.Contains(AbilityPerk.PeoplesArmy) && aiPlayer.player.matFaction.SkillUnlocked[2])
					{
						Worker worker = null;
						foreach (Worker worker2 in unit2.position.GetOwnerWorkers())
						{
							bool flag2 = false;
							foreach (Mech mech5 in this.moveMechPassengers.Keys)
							{
								if (this.moveMechPassengers[mech5].Contains(worker2))
								{
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								worker = worker2;
								break;
							}
						}
						if (worker != null)
						{
							if (!this.moveMechPassengers.ContainsKey((Mech)unit2))
							{
								this.moveMechPassengers.Add((Mech)unit2, new List<Worker>());
							}
							this.moveMechPassengers[(Mech)unit2].Add(worker);
						}
					}
				}
			}
			this.workersInaVillage = 0;
			foreach (Worker worker3 in aiPlayer.player.matPlayer.workers)
			{
				if (worker3.position.hexType == HexType.village)
				{
					this.workersInaVillage++;
				}
				if (worker3.position.hexType != HexType.capital)
				{
					this.workersOutOfBase++;
				}
			}
			if (aiPlayer.player.matPlayer.workers.Count < this.workerCountTarget)
			{
				if (this.pursuingWorkerStar && this.workersInaVillage < 3)
				{
					int num = 0;
					int num2 = 3 - this.workersInaVillage;
					foreach (Worker worker4 in aiPlayer.player.matPlayer.workers)
					{
						if (num >= num2)
						{
							break;
						}
						if (worker4.position.hexType != HexType.village && !this.movePriority.ContainsKey(worker4))
						{
							GameHex gameHex = null;
							foreach (GameHex gameHex2 in worker4.position.GetFieldsAccessible(worker4, false))
							{
								if (gameHex2.hexType == HexType.village && (gameHex == null || this.ResourcePriority(aiPlayer, gameHex2) > this.ResourcePriority(aiPlayer, gameHex)))
								{
									gameHex = gameHex2;
								}
							}
							if (gameHex != null)
							{
								this.movePriority.Add(worker4, 9790);
								this.moveTarget.Add(worker4, new List<GameHex> { gameHex });
								this.moveDistance.Add(worker4, 1);
								num++;
							}
						}
					}
				}
				if (this.workersInaVillage == 0)
				{
					Worker worker4 = null;
					GameHex gameHex13 = null;
					foreach (Worker worker5 in aiPlayer.player.matPlayer.workers)
					{
						if (worker5.position != null && (worker4 == null || this.ResourcePriority(aiPlayer, worker5.position) < this.ResourcePriority(aiPlayer, worker4.position)))
						{
							foreach (GameHex gameHex14 in worker5.position.GetFieldsAccessible(worker5, false))
							{
								if (gameHex14.hexType == HexType.village)
								{
									worker4 = worker5;
									gameHex13 = gameHex14;
								}
							}
						}
					}
					if (worker4 != null)
					{
						if (!this.movePriority.ContainsKey(worker4))
						{
							this.movePriority.Add(worker4, priProduce2 + 1);
							this.moveTarget.Add(worker4, new List<GameHex> { gameHex13 });
							this.moveDistance.Add(worker4, 1);
						}
						else
						{
							this.movePriority[worker4] = priProduce2 + 1;
							this.moveTarget[worker4] = new List<GameHex> { gameHex13 };
							this.moveDistance[worker4] = 1;
						}
					}
				}
			}
			if (!this.movePriority.ContainsKey(aiPlayer.player.character) && this.encounterNearestHex != null && this.characterDistance[this.encounterNearestHex] <= (int)aiPlayer.player.character.MaxMoveCount)
			{
				this.movePriority.Add(aiPlayer.player.character, priEncounter);
				this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { this.encounterNearestHex });
				this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[this.encounterNearestHex]);
			}
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matPlayerSectionsCount <= 4 && this.factoryDistance <= (int)aiPlayer.player.character.MaxMoveCount && !this.movePriority.ContainsKey(aiPlayer.player.character) && (this.gameManager.gameBoard.factory.Owner == aiPlayer.player || this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0))
			{
				this.movePriority.Add(aiPlayer.player.character, priEncounter + 31);
				this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { this.gameManager.gameBoard.factory });
				this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[this.gameManager.gameBoard.factory]);
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony)
			{
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					Mech mech6 = null;
					if (this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0)
					{
						foreach (Mech mech7 in aiPlayer.player.matFaction.mechs)
						{
							if (!this.movePriority.ContainsKey(mech7) && this.moveRange[mech7].ContainsKey(this.gameManager.gameBoard.factory))
							{
								mech6 = mech7;
							}
						}
					}
					if (mech6 != null)
					{
						this.movePriority.Add(mech6, priEncounter + 1);
						this.moveTarget.Add(mech6, new List<GameHex> { this.gameManager.gameBoard.factory });
						this.moveDistance.Add(mech6, this.moveRange[mech6][this.gameManager.gameBoard.factory]);
					}
				}
				if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && aiPlayer.player.character.position.hexType != HexType.mountain && !aiPlayer.player.character.position.hasTunnel)
				{
					Dictionary<GameHex, int> dictionary4;
					foreach (GameHex gameHex15 in this.gameManager.gameBoard.MoveRange(aiPlayer.player.character, (int)aiPlayer.player.character.MaxMoveCount, out dictionary4).Keys)
					{
						if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && (gameHex15.Owner == null || gameHex15.Owner == aiPlayer.player) && (gameHex15.hexType == HexType.mountain || gameHex15.hasTunnel))
						{
							this.movePriority.Add(aiPlayer.player.character, priEncounter);
							this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex15 });
							this.moveDistance.Add(aiPlayer.player.character, dictionary4[gameHex15]);
						}
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Polania)
			{
				if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && aiPlayer.player.character.position.hexType != HexType.lake)
				{
					Dictionary<GameHex, int> dictionary5;
					foreach (GameHex gameHex16 in this.gameManager.gameBoard.MoveRange(aiPlayer.player.character, (int)aiPlayer.player.character.MaxMoveCount, out dictionary5).Keys)
					{
						if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && (gameHex16.Owner == null || gameHex16.Owner == aiPlayer.player) && gameHex16.hexType == HexType.lake)
						{
							this.movePriority.Add(aiPlayer.player.character, priEncounter);
							this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex16 });
							this.moveDistance.Add(aiPlayer.player.character, dictionary5[gameHex16]);
						}
					}
				}
				if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && aiPlayer.player.character.position.hexType != HexType.lake && !aiPlayer.player.character.position.hasTunnel)
				{
					Dictionary<GameHex, int> dictionary6;
					foreach (GameHex gameHex17 in this.gameManager.gameBoard.MoveRange(aiPlayer.player.character, (int)aiPlayer.player.character.MaxMoveCount, out dictionary6).Keys)
					{
						if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && (gameHex17.Owner == null || gameHex17.Owner == aiPlayer.player) && (gameHex17.hexType == HexType.lake || gameHex17.hasTunnel))
						{
							this.movePriority.Add(aiPlayer.player.character, priEncounter);
							this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex17 });
							this.moveDistance.Add(aiPlayer.player.character, dictionary6[gameHex17]);
						}
					}
				}
				if (aiPlayer.player.matFaction.abilities.Contains(AbilityPerk.Camaraderie) && aiPlayer.player.matFaction.SkillUnlocked[2])
				{
					foreach (Mech mech8 in aiPlayer.player.matFaction.mechs)
					{
						if (!this.moveTarget.ContainsKey(mech8))
						{
							Dictionary<GameHex, int> dictionary8;
							Dictionary<GameHex, GameHex> dictionary7 = this.gameManager.gameBoard.MoveRange(mech8, (int)mech8.MaxMoveCount, out dictionary8);
							GameHex gameHex18 = null;
							int num5 = 0;
							foreach (GameHex gameHex19 in dictionary7.Keys)
							{
								if (gameHex19.Owner != aiPlayer.player && !gameHex19.HasOwnerCharacter() && gameHex19.GetOwnerMechs().Count == 0)
								{
									int num6 = gameHex19.GetEnemyWorkers().Count + gameHex19.GetResourceCount() * 8;
									if (num6 > num5)
									{
										gameHex18 = gameHex19;
										num5 = num6;
									}
								}
							}
							if (gameHex18 != null)
							{
								this.movePriority.Add(mech8, num5);
								this.moveTarget.Add(mech8, new List<GameHex> { gameHex18 });
								this.moveDistance.Add(mech8, dictionary8[gameHex18]);
							}
						}
					}
				}
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural && this.resourceCostSingleAction[ResourceType.food] <= 2 && this.resourceAccess[ResourceType.food] >= 2)
				{
					Worker worker6 = null;
					foreach (Worker worker7 in aiPlayer.player.matPlayer.workers)
					{
						if (worker7.position.hexType == HexType.farm && !this.moveTarget.ContainsKey(worker7))
						{
							worker6 = worker7;
						}
					}
					if (worker6 != null)
					{
						Dictionary<GameHex, int> dictionary9;
						this.gameManager.gameBoard.MoveRange(worker6, 1, out dictionary9);
						GameHex gameHex20 = null;
						foreach (GameHex gameHex21 in dictionary9.Keys)
						{
							if (gameHex21.Owner == null || gameHex21.Owner == aiPlayer.player)
							{
								gameHex20 = gameHex21;
							}
						}
						if (gameHex20 != null)
						{
							this.movePriority.Add(worker6, 1);
							this.moveTarget.Add(worker6, new List<GameHex> { gameHex20 });
							this.moveDistance.Add(worker6, 1);
						}
					}
				}
			}
			foreach (Unit unit4 in this.moveRangeAll.Keys)
			{
				if (unit4.position.hexType == HexType.capital && !this.movePriority.ContainsKey(unit4))
				{
					GameHex gameHex22 = null;
					foreach (GameHex gameHex23 in this.moveRangeAll[unit4].Keys)
					{
						if (gameHex23.hexType != HexType.capital && (gameHex23.Owner == null || gameHex23.Owner == aiPlayer.player) && (gameHex22 == null || gameHex22.Owner != null))
						{
							gameHex22 = gameHex23;
						}
					}
					if (gameHex22 != null)
					{
						this.movePriority.Add(unit4, priEncounter);
						this.moveTarget.Add(unit4, new List<GameHex> { gameHex22 });
						this.moveDistance.Add(unit4, this.moveRangeAll[unit4][gameHex22]);
					}
				}
			}
			if (aiPlayer.player.matPlayer.matPlayerSectionsCount <= 4 && !this.movePriority.ContainsKey(aiPlayer.player.character))
			{
				GameHex factory = this.gameManager.gameBoard.factory;
				if (this.characterDistance.ContainsKey(factory))
				{
					int dist = this.characterDistance[factory];
					int p = priEncounter + (dist <= (int)aiPlayer.player.character.MaxMoveCount ? 35 : 10);
					this.movePriority.Add(aiPlayer.player.character, p);
					this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { factory });
					this.moveDistance.Add(aiPlayer.player.character, dist);
				}
			}
			else if (!this.movePriority.ContainsKey(aiPlayer.player.character) && this.encounterNearestHex != null && this.characterDistance.ContainsKey(this.encounterNearestHex) && this.characterDistance[this.encounterNearestHex] <= (int)aiPlayer.player.character.MaxMoveCount)
			{
				this.movePriority.Add(aiPlayer.player.character, priEncounter);
				this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { this.encounterNearestHex });
				this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[this.encounterNearestHex]);
			}
			int moveTieBreaker = 0;
			foreach (KeyValuePair<Unit, int> keyValuePair2 in this.movePriority)
			{
				int key = keyValuePair2.Value * 100 + moveTieBreaker++;
				this.movePrioritySorted.Add(key, keyValuePair2.Key);
			}
			if (this.movePriority.Values.Count > 0)
			{
				this.movePriorityHighest = this.movePriority.Values.Max();
				return;
			}
			this.movePriorityHighest = -1;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x00115E84 File Offset: 0x00114084
		public virtual void UpdateMechOrder(AiPlayer aiPlayer)
		{
			int num = -1;
			switch (aiPlayer.player.matFaction.faction)
			{
			case Faction.Polania:
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
				break;
			case Faction.Nordic:
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
				break;
			case Faction.Rusviet:
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					this.mechPriority[0] = 10;
					this.mechPriority[1] = 8;
					this.mechPriority[2] = 6;
					this.mechPriority[3] = 4;
				}
				break;
			case Faction.Crimea:
				this.mechPriority[0] = 10;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 8;
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					this.mechPriority[3] = 11;
				}
				break;
			case Faction.Saxony:
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					this.mechPriority[0] = 9;
				}
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					this.mechPriority[1] = 11;
				}
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					this.mechPriority[0] = 11;
				}
				break;
			}
			for (int i = 0; i < this.mechPriority.Count; i++)
			{
				if (!aiPlayer.player.matFaction.SkillUnlocked[i] && (num == -1 || this.mechPriority[i] > this.mechPriority[num]))
				{
					num = i;
				}
			}
			this.mechNext = num;
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x0011610C File Offset: 0x0011430C
		public void UpdateBuildingOrder(AiPlayer aiPlayer)
		{
			BuildingType buildingType = BuildingType.Mine;
			if ((aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural) || (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial))
			{
				this.buildingPriority[BuildingType.Mill] = 12;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Rusviet && (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial || aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative))
			{
				this.buildingPriority[BuildingType.Mill] = 10;
				this.buildingPriority[BuildingType.Armory] = 8;
				this.buildingPriority[BuildingType.Monument] = 6;
				this.buildingPriority[BuildingType.Mine] = 4;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.buildingPriority[BuildingType.Mine] = 10;
				this.buildingPriority[BuildingType.Mill] = 8;
				this.buildingPriority[BuildingType.Armory] = 6;
				this.buildingPriority[BuildingType.Monument] = 4;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.buildingPriority[BuildingType.Mine] = 10;
				this.buildingPriority[BuildingType.Mill] = 8;
				this.buildingPriority[BuildingType.Armory] = 6;
				this.buildingPriority[BuildingType.Monument] = 4;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony)
			{
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic || aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					this.buildingPriority[BuildingType.Mill] = 8;
					this.buildingPriority[BuildingType.Armory] = 6;
					this.buildingPriority[BuildingType.Monument] = 4;
					this.buildingPriority[BuildingType.Mine] = 10;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					this.buildingPriority[BuildingType.Mill] = 8;
					this.buildingPriority[BuildingType.Armory] = 6;
					this.buildingPriority[BuildingType.Monument] = 4;
					this.buildingPriority[BuildingType.Mine] = 10;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					this.buildingPriority[BuildingType.Mill] = 4;
					this.buildingPriority[BuildingType.Armory] = 8;
					this.buildingPriority[BuildingType.Monument] = 6;
					this.buildingPriority[BuildingType.Mine] = 10;
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType != PlayerMatType.Agricultural && aiPlayer.player.matPlayer.matType != PlayerMatType.Industrial)
			{
				this.buildingPriority[BuildingType.Monument] = 10;
				this.buildingPriority[BuildingType.Armory] = 8;
				this.buildingPriority[BuildingType.Mill] = 6;
				this.buildingPriority[BuildingType.Mine] = 4;
			}
			
			bool ignoreBuilding = false;
			if (aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Albion) {
				ignoreBuilding = true;
			}
			if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial || aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative || (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && aiPlayer.player.matFaction.faction != Faction.Polania) || aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) {
				ignoreBuilding = true;
			}
			if (ignoreBuilding) {
				bool objNeedsBuilding = false;
				foreach (var obj in aiPlayer.player.objectiveCards) {
					if (obj.CardId == 20 || obj.CardId == 27) objNeedsBuilding = true;
				}
				if (!objNeedsBuilding) {
					this.buildingPriority[BuildingType.Mine] = 0;
					this.buildingPriority[BuildingType.Mill] = 0;
					this.buildingPriority[BuildingType.Armory] = 0;
					this.buildingPriority[BuildingType.Monument] = 0;
				}
			}

			foreach (BuildingType buildingType2 in this.buildingPriority.Keys)
			{
				if (aiPlayer.player.matPlayer.GetBuilding(buildingType2) == null && (aiPlayer.player.matPlayer.GetBuilding(buildingType) != null || this.buildingPriority[buildingType2] > this.buildingPriority[buildingType]))
				{
					buildingType = buildingType2;
				}
			}
			this.buildingNext = buildingType;
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x001164EC File Offset: 0x001146EC
		public void UpdateRecruitOrder(AiPlayer aiPlayer)
		{
			if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 8;
				this.recruitPriority[DownActionType.Deploy] = 6;
				this.recruitPriority[DownActionType.Upgrade] = 4;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.recruitPriority[DownActionType.Enlist] = 12;
				this.recruitPriority[DownActionType.Deploy] = 10;
				this.recruitPriority[DownActionType.Upgrade] = 8;
				this.recruitPriority[DownActionType.Build] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.recruitPriority[DownActionType.Deploy] = 10;
				this.recruitPriority[DownActionType.Enlist] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				this.recruitPriority[DownActionType.Build] = 4;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 6;
				this.recruitPriority[DownActionType.Upgrade] = 8;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 6;
				this.recruitPriority[DownActionType.Upgrade] = 8;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.recruitPriority[DownActionType.Enlist] = 8;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 10;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
			}
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x000448F3 File Offset: 0x00042AF3
		public virtual void UpdateWorkerCountTarget(AiPlayer aiPlayer)
		{
			this.workerCountTarget = 5;
			this.pursuingWorkerStar = false;
			// Saxony and Crimea always want 8 workers from the start
			if (aiPlayer.player.matFaction.faction == Faction.Saxony || aiPlayer.player.matFaction.faction == Faction.Crimea)
			{
				this.workerCountTarget = 8;
				this.pursuingWorkerStar = true;
				return;
			}
			// Once 2+ of the 4 bottom-row stars (Mechs, Recruits, Structures, Upgrades) are earned,
			// raise the worker count target to 8 and pursue the Workers star
			int bottomRowStars = 0;
			if (aiPlayer.player.GetNumberOfStars(StarType.Mechs) > 0) bottomRowStars++;
			if (aiPlayer.player.GetNumberOfStars(StarType.Recruits) > 0) bottomRowStars++;
			if (aiPlayer.player.GetNumberOfStars(StarType.Structures) > 0) bottomRowStars++;
			if (aiPlayer.player.GetNumberOfStars(StarType.Upgrades) > 0) bottomRowStars++;
			
			int totalStars = aiPlayer.player.GetNumberOfStars();

			if ((bottomRowStars >= 2 || (totalStars >= 5 && aiPlayer.player.matPlayer.workers.Count >= 5)) && aiPlayer.player.GetNumberOfStars(StarType.Workers) == 0)
			{
				this.workerCountTarget = 8;
				this.pursuingWorkerStar = true;
			}
			if ((aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic) && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.workerCountTarget = 8;
				this.pursuingWorkerStar = true;
			}
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x00117688 File Offset: 0x00115888
		public virtual void UpdateRecruitOneTimeOrder(AiPlayer aiPlayer)
		{
			if (aiPlayer.player.Power < 3)
			{
				this.recruitOneTimePriority[GainType.Power] = 30;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony)
			{
				this.recruitOneTimePriority[GainType.Power] = 20;
			}
			if (aiPlayer.player.GetNumberOfStars(StarType.Combat) < 2)
			{
				this.recruitOneTimePriority[GainType.Power] = 20;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.recruitOneTimePriority[GainType.Power] = 25;
				this.recruitOneTimePriority[GainType.CombatCard] = 22;
				this.recruitOneTimePriority[GainType.Popularity] = 5;
				this.recruitOneTimePriority[GainType.Coin] = 10;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.recruitOneTimePriority[GainType.CombatCard] = 25;
				this.recruitOneTimePriority[GainType.Power] = 22;
				this.recruitOneTimePriority[GainType.Coin] = 15;
				this.recruitOneTimePriority[GainType.Popularity] = 10;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
						{
				this.recruitOneTimePriority[GainType.CombatCard] = 23;
				this.recruitOneTimePriority[GainType.Power] = 22;
				this.recruitOneTimePriority[GainType.Coin] = 15;
				this.recruitOneTimePriority[GainType.Popularity] = 25;
			}
			
		}

		// Token: 0x04001F17 RID: 7959
		public Dictionary<ResourceType, int> resourceAccess = new Dictionary<ResourceType, int>
		{
			{
				ResourceType.food,
				0
			},
			{
				ResourceType.metal,
				0
			},
			{
				ResourceType.oil,
				0
			},
			{
				ResourceType.wood,
				0
			}
		};

		// Token: 0x04001F18 RID: 7960
		public Dictionary<ResourceType, int> resourceCostSingleAction = new Dictionary<ResourceType, int>
		{
			{
				ResourceType.food,
				0
			},
			{
				ResourceType.metal,
				0
			},
			{
				ResourceType.oil,
				0
			},
			{
				ResourceType.wood,
				0
			}
		};

		// Token: 0x04001F19 RID: 7961
		public Dictionary<ResourceType, int> resourceDemandTotal = new Dictionary<ResourceType, int>
		{
			{
				ResourceType.food,
				0
			},
			{
				ResourceType.metal,
				0
			},
			{
				ResourceType.oil,
				0
			},
			{
				ResourceType.wood,
				0
			}
		};

		// Token: 0x04001F1A RID: 7962
		public Dictionary<ResourceType, int> resourceDemandPriority = new Dictionary<ResourceType, int>
		{
			{
				ResourceType.food,
				0
			},
			{
				ResourceType.metal,
				0
			},
			{
				ResourceType.oil,
				0
			},
			{
				ResourceType.wood,
				0
			}
		};

		// Token: 0x04001F1B RID: 7963
		public ResourceType resourceHighestPriority;

		// Token: 0x04001F1C RID: 7964
		public ResourceType resourceHighestPriorityNoProduce;

		// Token: 0x04001F1D RID: 7965
		public bool canUpgrade;

		// Token: 0x04001F1E RID: 7966
		public bool canDeploy;

		// Token: 0x04001F1F RID: 7967
		public bool canBuild;

		// Token: 0x04001F20 RID: 7968
		public bool canEnlist;

		// Token: 0x04001F21 RID: 7969
		public List<Worker> uselessWorkers4Production = new List<Worker>();

		// Token: 0x04001F22 RID: 7970
		public Dictionary<Worker, GameHex> uselessWorkersTargets = new Dictionary<Worker, GameHex>();

		// Token: 0x04001F23 RID: 7971
		public Dictionary<GameHex, int> characterDistance;

		// Token: 0x04001F24 RID: 7972
		public GameHex encounterNearestHex;

		// Token: 0x04001F25 RID: 7973
		public int factoryDistance;

		// Token: 0x04001F26 RID: 7974
		public Dictionary<Unit, Dictionary<GameHex, int>> moveRange = new Dictionary<Unit, Dictionary<GameHex, int>>();

		// Token: 0x04001F27 RID: 7975
		public Dictionary<Unit, Dictionary<GameHex, int>> moveRangeAll = new Dictionary<Unit, Dictionary<GameHex, int>>();

		// Token: 0x04001F28 RID: 7976
		public Dictionary<GameHex, List<Unit>> enemyCanBeAttackedBy = new Dictionary<GameHex, List<Unit>>();

		// Token: 0x04001F29 RID: 7977
		public Dictionary<Unit, int> movePriority = new Dictionary<Unit, int>();

		// Token: 0x04001F2A RID: 7978
		public SortedList<int, Unit> movePrioritySorted = new SortedList<int, Unit>(new InvertedComparer());

		public SortedList<int, ResourceType> resourcePrioritySorted = new SortedList<int, ResourceType>(new InvertedComparer());

		// Token: 0x04001F2B RID: 7979
		public Dictionary<Unit, List<GameHex>> moveTarget = new Dictionary<Unit, List<GameHex>>();

		// Token: 0x04001F2C RID: 7980
		public Dictionary<Unit, int> moveDistance = new Dictionary<Unit, int>();

		// Token: 0x04001F2D RID: 7981
		public Dictionary<Mech, List<Worker>> moveMechPassengers = new Dictionary<Mech, List<Worker>>();

		// Token: 0x04001F2E RID: 7982
		public int movePriorityHighest;

		// Token: 0x04001F2F RID: 7983
		public int workerCountTarget = 3;

		// Flag: AI is actively pursuing the Workers star (workerCountTarget raised to 8)
		public bool pursuingWorkerStar;

		// Token: 0x04001F30 RID: 7984
		public int workersInaVillage;

		// Token: 0x04001F31 RID: 7985
		public int workersOutOfBase;

		// Token: 0x04001F32 RID: 7986
		public Dictionary<int, int> mechPriority = new Dictionary<int, int>
		{
			{ 0, 10 },
			{ 1, 9 },
			{ 2, 8 },
			{ 3, 7 }
		};

		// Token: 0x04001F33 RID: 7987
		public Dictionary<BuildingType, int> buildingPriority = new Dictionary<BuildingType, int>
		{
			{
				BuildingType.Mine,
				10
			},
			{
				BuildingType.Monument,
				9
			},
			{
				BuildingType.Mill,
				8
			},
			{
				BuildingType.Armory,
				7
			}
		};

		// Token: 0x04001F34 RID: 7988
		public Dictionary<DownActionType, int> recruitPriority = new Dictionary<DownActionType, int>
		{
			{
				DownActionType.Enlist,
				10
			},
			{
				DownActionType.Upgrade,
				9
			},
			{
				DownActionType.Build,
				8
			},
			{
				DownActionType.Deploy,
				7
			}
		};

		// Token: 0x04001F35 RID: 7989
		public Dictionary<GainType, int> recruitOneTimePriority = new Dictionary<GainType, int>
		{
			{
				GainType.CombatCard,
				10
			},
			{
				GainType.Popularity,
				8
			},
			{
				GainType.Coin,
				5
			},
			{
				GainType.Power,
				9
			}
		};

		// Token: 0x04001F36 RID: 7990
		public int mechNext;

		// Token: 0x04001F37 RID: 7991
		public BuildingType buildingNext;

		// Token: 0x04001F38 RID: 7992
		public bool produceLoopPresent;

		// Token: 0x04001F39 RID: 7993
		public bool tradeLoopPresent;

		// Token: 0x04001F3A RID: 7994
		public ResourceType tradeLoopResource = ResourceType.combatCard;

		// Token: 0x04001F3B RID: 7995
		public GameHex preferredDeployPosition;

		public GameHex preferredBuildPosition;

		// Token: 0x04001F3C RID: 7996
		public bool gottaMoveToBuild;

		// Token: 0x04001F3D RID: 7997
		public bool objectiveBalancedWorkforce;

		// Token: 0x04001F3E RID: 7998
		public bool objectiveMachineOverMuscle;

		public int priorityFight;

		// 3-turn cycle system: repeating pattern of top-action types after kickstart
		// Step 0: e.g. Produce, Step 1: e.g. Trade(AnyResource), Step 2: e.g. Move
		public bool turnCyclePresent;
		public GainType[] turnCycleSteps; // length 3
		public ResourceType turnCycleTradeResource; // what resource to trade for in the trade step
		public int turnCycleStartTurn; // the turn number at which the cycle begins (after kickstart)

		// Token: 0x04001F3F RID: 7999
		protected GameManager gameManager;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005A5 RID: 1445
	public class AiStrategicAnalysis
	{
		// Token: 0x06002DC7 RID: 11719 RVA: 0x00110418 File Offset: 0x0010E618
		public AiStrategicAnalysis(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x00110614 File Offset: 0x0010E814
		public virtual void Run(AiPlayer aiPlayer, int priMoveToProduction1, int priMoveToProduction2, int priMoveToEncounter, int priMoveToFight, int priMoveToBuild)
		{
			this.preferredDeployPosition = null;
			this.UpdateEncounterAndFactory(aiPlayer);
			this.UpdateResourceAccess(aiPlayer);
			this.UpdateTradeLoop(aiPlayer);
			this.UpdateResourceDemand(aiPlayer);
			this.UpdateResourceDemandPriority(aiPlayer, this.resourceDemandTotal);
			this.UpdateProduceLoop(aiPlayer);
			this.canUpgrade = aiPlayer.player.GetNumberOfStars(StarType.Upgrades) == 0 && aiPlayer.AiActions[aiPlayer.gainUpgradeActionPosition[0]].downAction.CanPlayerPayActions();
			this.canDeploy = aiPlayer.player.GetNumberOfStars(StarType.Mechs) == 0 && aiPlayer.AiActions[aiPlayer.gainMechActionPosition[0]].downAction.CanPlayerPayActions();
			this.canBuild = aiPlayer.player.GetNumberOfStars(StarType.Structures) == 0 && aiPlayer.AiActions[aiPlayer.gainBuildingActionPosition[0]].downAction.CanPlayerPayActions() && ((GainBuilding)aiPlayer.AiActions[aiPlayer.gainBuildingActionPosition[0]].downAction.GetGainAction(0)).GainAvaliable();
			this.canEnlist = aiPlayer.player.GetNumberOfStars(StarType.Recruits) == 0 && aiPlayer.AiActions[aiPlayer.gainRecruitActionPosition[0]].downAction.CanPlayerPayActions();
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.gottaMoveToBuild = !this.canBuild && aiPlayer.player.GetNumberOfStars(StarType.Structures) == 0 && aiPlayer.AiActions[aiPlayer.gainBuildingActionPosition[0]].downAction.CanPlayerPayActions() && aiPlayer.CanPlayTopAction(GainType.Move);
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

		// Token: 0x06002DC9 RID: 11721 RVA: 0x00110818 File Offset: 0x0010EA18
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

		// Token: 0x06002DCA RID: 11722 RVA: 0x001108B8 File Offset: 0x0010EAB8
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
				this.tradeLoopPresent = amount <= 2 + aiPlayer.player.Resources(false)[this.tradeLoopResource] && aiPlayer.AiTopActions[GainType.AnyResource].downAction.GetGainAction(0).GainAvaliable();
				return;
			}
			this.tradeLoopPresent = amount <= 2 && aiPlayer.AiTopActions[GainType.AnyResource].downAction.GetGainAction(0).GainAvaliable();
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x00110A38 File Offset: 0x0010EC38
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

		// Token: 0x06002DCC RID: 11724 RVA: 0x00110B04 File Offset: 0x0010ED04
		protected void UpdateResourceDemand(AiPlayer aiPlayer)
		{
			this.resourceCostSingleAction[ResourceType.oil] = (int)aiPlayer.AiActions[aiPlayer.gainUpgradeActionPosition[0]].downAction.GetPayAction(0).Amount;
			this.resourceCostSingleAction[ResourceType.metal] = (int)aiPlayer.AiActions[aiPlayer.gainMechActionPosition[0]].downAction.GetPayAction(0).Amount;
			this.resourceCostSingleAction[ResourceType.wood] = (int)aiPlayer.AiActions[aiPlayer.gainBuildingActionPosition[0]].downAction.GetPayAction(0).Amount;
			this.resourceCostSingleAction[ResourceType.food] = (int)aiPlayer.AiActions[aiPlayer.gainRecruitActionPosition[0]].downAction.GetPayAction(0).Amount;
			int num = this.resourceCostSingleAction[ResourceType.oil] * (6 - aiPlayer.player.matPlayer.UpgradesDone);
			int num2 = this.resourceCostSingleAction[ResourceType.metal] * (4 - aiPlayer.player.matFaction.mechs.Count);
			int num3 = this.resourceCostSingleAction[ResourceType.wood] * (4 - aiPlayer.player.matPlayer.buildings.Count);
			int num4 = this.resourceCostSingleAction[ResourceType.food] * (4 - aiPlayer.player.matPlayer.RecruitsEnlisted);
			Dictionary<ResourceType, int> dictionary = aiPlayer.player.Resources(false);
			this.resourceDemandTotal[ResourceType.oil] = num - dictionary[ResourceType.oil];
			this.resourceDemandTotal[ResourceType.metal] = num2 - dictionary[ResourceType.metal];
			this.resourceDemandTotal[ResourceType.wood] = num3 - dictionary[ResourceType.wood];
			this.resourceDemandTotal[ResourceType.food] = num4 - dictionary[ResourceType.food];
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x00110CD0 File Offset: 0x0010EED0
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
				else if (aiPlayer.player.matFaction.mechs.Count <= 1)
				{
					this.resourceDemandPriority[ResourceType.metal] = 8;
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
			if (this.resourceDemandPriority[this.resourceHighestPriority] == 0)
			{
				this.resourceHighestPriority = ResourceType.combatCard;
			}
			if (this.resourceDemandPriority[this.resourceHighestPriorityNoProduce] == 0 || this.resourceAccess[this.resourceHighestPriorityNoProduce] > 0)
			{
				this.resourceHighestPriorityNoProduce = ResourceType.combatCard;
			}
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x0004485D File Offset: 0x00042A5D
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

		// Token: 0x06002DCF RID: 11727 RVA: 0x001110B8 File Offset: 0x0010F2B8
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

		// Token: 0x06002DD0 RID: 11728 RVA: 0x00111140 File Offset: 0x0010F340
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

		// Token: 0x06002DD1 RID: 11729 RVA: 0x001112B0 File Offset: 0x0010F4B0
		protected virtual void UpdateUselessWorkers4Production(AiPlayer aiPlayer)
		{
			this.uselessWorkers4Production.Clear();
			this.uselessWorkersTargets.Clear();
			this.moveMechPassengers.Clear();
			foreach (Worker worker in aiPlayer.player.matPlayer.workers)
			{
				if (worker.position != null && this.ResourcePriority(aiPlayer, worker.position) <= 0f)
				{
					this.uselessWorkers4Production.Add(worker);
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

		// Token: 0x06002DD2 RID: 11730 RVA: 0x00111790 File Offset: 0x0010F990
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

		// Token: 0x06002DD3 RID: 11731 RVA: 0x001118D0 File Offset: 0x0010FAD0
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
				num7 += 1.5f - 1.5f * (float)hex.GetOwnerWorkers().Count;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matFaction.SkillUnlocked[2])
			{
				num7 += (float)(hex.GetOwnerWorkers().Count / 8);
			}
			else
			{
				num7 += (float)(2 - hex.GetOwnerWorkers().Count);
			}
			if (num7 > 0f && this.enemyCanBeAttackedBy[hex].Count<Unit>() > 1)
			{
				num7 += 10f;
			}
			return num7;
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x00111D5C File Offset: 0x0010FF5C
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

		// Token: 0x06002DD5 RID: 11733 RVA: 0x00111E98 File Offset: 0x00110098
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
					if (gameHex.Owner != null && gameHex.Owner != aiPlayer.player && (gameHex.HasOwnerCharacter() || gameHex.GetOwnerMechs().Count > 0))
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
					if (gameHex2.Owner != null && gameHex2.Owner != aiPlayer.player && (gameHex2.HasOwnerCharacter() || gameHex2.GetOwnerMechs().Count > 0))
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
					if (aiPlayer.player.matFaction.SkillUnlocked[3] && aiPlayer.player.matFaction.faction == Faction.Saxony && (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical || aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial || aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural))
					{
						if (aiPlayer.player.matFaction.faction != Faction.Saxony || aiPlayer.player.matPlayer.matType != PlayerMatType.Industrial)
						{
							using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator3 = this.moveRange[mech2].Keys.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									GameHex gameHex7 = enumerator3.Current;
									if (this.ResourcePriority(aiPlayer, gameHex7) > 0f && (gameHex7.Owner == null || gameHex7.Owner == aiPlayer.player) && this.moveRange[mech2][gameHex7] == 1 && AiStrategicAnalysis.ResourceProduced(gameHex7.hexType) != ResourceType.combatCard && (gameHex5 == null || this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex7.hexType)] <= this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex5.hexType)]) && (gameHex5 == null || this.ResourcePriority(aiPlayer, gameHex7) > this.ResourcePriority(aiPlayer, gameHex5)))
									{
										gameHex5 = gameHex7;
									}
								}
								goto IL_053B;
							}
							goto IL_073A;
						}
						foreach (GameHex gameHex8 in this.moveRange[mech2].Keys)
						{
							if (this.ResourcePriority(aiPlayer, gameHex8) > 0f && (gameHex8.Owner == null || gameHex8.Owner == aiPlayer.player) && this.moveRange[mech2][gameHex8] == 1 && AiStrategicAnalysis.ResourceProduced(gameHex8.hexType) != ResourceType.combatCard && (gameHex5 == null || this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex8.hexType)] <= this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex5.hexType)]) && (gameHex5 == null || this.ResourcePriority(aiPlayer, gameHex8) > this.ResourcePriority(aiPlayer, gameHex5)))
							{
								gameHex5 = gameHex8;
							}
						}
						IL_053B:
						if (gameHex5 != null)
						{
							ResourceType resourceType = AiStrategicAnalysis.ResourceProduced(gameHex5.hexType);
							int num2 = this.resourceCostSingleAction[resourceType];
							if (this.moveMechPassengers[mech2].Count > num2)
							{
								Dictionary<GameHex, int> dictionary3;
								this.gameManager.gameBoard.MoveRange(mech2, gameHex5, 1, out dictionary3);
								foreach (GameHex gameHex9 in dictionary3.Keys)
								{
									if (this.ResourcePriority(aiPlayer, gameHex9) > 0f && (gameHex9.Owner == null || gameHex9.Owner == aiPlayer.player) && gameHex9.hexType != gameHex5.hexType && AiStrategicAnalysis.ResourceProduced(gameHex9.hexType) != ResourceType.combatCard && this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex9.hexType)] == 0 && (gameHex6 == null || this.ResourcePriority(aiPlayer, gameHex9) > this.ResourcePriority(aiPlayer, gameHex6)))
									{
										gameHex6 = gameHex9;
									}
								}
								if (gameHex6 != null)
								{
									flag = true;
								}
							}
						}
					}
					IL_073A:
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
						goto IL_0A52;
					}
				}
				foreach (GameHex gameHex12 in this.enemyCanBeAttackedBy.Keys)
				{
					if (this.isWorthAttacking(gameHex12, aiPlayer) > 0f)
					{
						gameHex10 = gameHex12;
					}
				}
				IL_0A52:
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
			if (aiPlayer.player.matPlayer.workers.Count < this.workerCountTarget)
			{
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
							this.movePriority.Add(aiPlayer.player.character, 1);
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
							this.movePriority.Add(aiPlayer.player.character, 1);
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
							this.movePriority.Add(aiPlayer.player.character, 1);
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
						this.movePriority.Add(unit4, 1);
						this.moveTarget.Add(unit4, new List<GameHex> { gameHex22 });
						this.moveDistance.Add(unit4, this.moveRangeAll[unit4][gameHex22]);
					}
				}
			}
			if (aiPlayer.player.matPlayer.matPlayerSectionsCount <= 4 && this.factoryDistance <= (int)aiPlayer.player.character.MaxMoveCount && !this.movePriority.ContainsKey(aiPlayer.player.character) && (this.gameManager.gameBoard.factory.Owner == aiPlayer.player || this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0))
			{
				this.movePriority.Add(aiPlayer.player.character, priEncounter + 35);
				this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { this.gameManager.gameBoard.factory });
				this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[this.gameManager.gameBoard.factory]);
			}
			else if (!this.movePriority.ContainsKey(aiPlayer.player.character) && this.encounterNearestHex != null && this.characterDistance[this.encounterNearestHex] <= (int)aiPlayer.player.character.MaxMoveCount)
			{
				this.movePriority.Add(aiPlayer.player.character, priEncounter);
				this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { this.encounterNearestHex });
				this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[this.encounterNearestHex]);
			}
			foreach (KeyValuePair<Unit, int> keyValuePair2 in this.movePriority)
			{
				this.movePrioritySorted.Add(keyValuePair2.Value, keyValuePair2.Key);
			}
			if (this.movePriority.Values.Count > 0)
			{
				this.movePriorityHighest = this.movePriority.Values.Max();
				return;
			}
			this.movePriorityHighest = -1;
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x00113B38 File Offset: 0x00111D38
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

		// Token: 0x06002DD7 RID: 11735 RVA: 0x00113DC0 File Offset: 0x00111FC0
		public void UpdateBuildingOrder(AiPlayer aiPlayer)
		{
			BuildingType buildingType = BuildingType.Mine;
			if ((aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural) || (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial))
			{
				this.buildingPriority[BuildingType.Mill] = 12;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
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
			if (aiPlayer.player.matFaction.faction == Faction.Saxony)
			{
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic || aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					this.buildingPriority[BuildingType.Mill] = 10;
					this.buildingPriority[BuildingType.Armory] = 8;
					this.buildingPriority[BuildingType.Monument] = 6;
					this.buildingPriority[BuildingType.Mine] = 4;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					this.buildingPriority[BuildingType.Mill] = 10;
					this.buildingPriority[BuildingType.Monument] = 8;
					this.buildingPriority[BuildingType.Armory] = 6;
					this.buildingPriority[BuildingType.Mine] = 4;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					this.buildingPriority[BuildingType.Armory] = 10;
					this.buildingPriority[BuildingType.Mill] = 8;
					this.buildingPriority[BuildingType.Monument] = 6;
					this.buildingPriority[BuildingType.Mine] = 4;
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType != PlayerMatType.Agricultural && aiPlayer.player.matPlayer.matType != PlayerMatType.Industrial)
			{
				this.buildingPriority[BuildingType.Monument] = 10;
				this.buildingPriority[BuildingType.Armory] = 8;
				this.buildingPriority[BuildingType.Mill] = 6;
				this.buildingPriority[BuildingType.Mine] = 4;
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

		// Token: 0x06002DD8 RID: 11736 RVA: 0x001141A0 File Offset: 0x001123A0
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
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 8;
				this.recruitPriority[DownActionType.Deploy] = 6;
				this.recruitPriority[DownActionType.Upgrade] = 4;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
				return;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
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
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
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
				this.recruitPriority[DownActionType.Deploy] = 8;
				this.recruitPriority[DownActionType.Upgrade] = 6;
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
				this.recruitPriority[DownActionType.Enlist] = 10;
				this.recruitPriority[DownActionType.Build] = 4;
				this.recruitPriority[DownActionType.Deploy] = 8;
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

		public virtual void UpdateRecruitOneTimeOrder(AiPlayer aiPlayer)
		{
			if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType != PlayerMatType.Patriotic)
			{
				this.recruitOneTimePriority[GainType.Power] = 20;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony)
			{
				this.recruitOneTimePriority[GainType.Power] = 20;
				this.recruitOneTimePriority[GainType.CombatCard] = 20;
			}
			else if (aiPlayer.player.stars.Values.Sum() >= 5 && aiPlayer.player.GetNumberOfStars(StarType.Combat) < 2)
			{
				this.recruitOneTimePriority[GainType.Power] = 50;
				this.recruitOneTimePriority[GainType.CombatCard] = 50;
			}
			else if (aiPlayer.player.GetNumberOfStars(StarType.Combat) < 2)
			{
				this.recruitOneTimePriority[GainType.Power] = 20;
			}
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x00044886 File Offset: 0x00042A86
		public virtual void UpdateWorkerCountTarget(AiPlayer aiPlayer)
		{
			this.workerCountTarget = 5;
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && aiPlayer.TradeResourceType() == ResourceType.combatCard)
			{
				this.workerCountTarget = 8;
			}
		}

		// Token: 0x04001EFE RID: 7934
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

		// Token: 0x04001EFF RID: 7935
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

		// Token: 0x04001F00 RID: 7936
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

		// Token: 0x04001F01 RID: 7937
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

		// Token: 0x04001F02 RID: 7938
		public ResourceType resourceHighestPriority;

		// Token: 0x04001F03 RID: 7939
		public ResourceType resourceHighestPriorityNoProduce;

		// Token: 0x04001F04 RID: 7940
		public bool canUpgrade;

		// Token: 0x04001F05 RID: 7941
		public bool canDeploy;

		// Token: 0x04001F06 RID: 7942
		public bool canBuild;

		// Token: 0x04001F07 RID: 7943
		public bool canEnlist;

		// Token: 0x04001F08 RID: 7944
		public List<Worker> uselessWorkers4Production = new List<Worker>();

		// Token: 0x04001F09 RID: 7945
		public Dictionary<Worker, GameHex> uselessWorkersTargets = new Dictionary<Worker, GameHex>();

		// Token: 0x04001F0A RID: 7946
		public Dictionary<GameHex, int> characterDistance;

		// Token: 0x04001F0B RID: 7947
		public GameHex encounterNearestHex;

		// Token: 0x04001F0C RID: 7948
		public int factoryDistance;

		// Token: 0x04001F0D RID: 7949
		public Dictionary<Unit, Dictionary<GameHex, int>> moveRange = new Dictionary<Unit, Dictionary<GameHex, int>>();

		// Token: 0x04001F0E RID: 7950
		public Dictionary<Unit, Dictionary<GameHex, int>> moveRangeAll = new Dictionary<Unit, Dictionary<GameHex, int>>();

		// Token: 0x04001F0F RID: 7951
		public Dictionary<GameHex, List<Unit>> enemyCanBeAttackedBy = new Dictionary<GameHex, List<Unit>>();

		// Token: 0x04001F10 RID: 7952
		public Dictionary<Unit, int> movePriority = new Dictionary<Unit, int>();

		// Token: 0x04001F11 RID: 7953
		public SortedList<int, Unit> movePrioritySorted = new SortedList<int, Unit>(new InvertedComparer());

		// Token: 0x04001F12 RID: 7954
		public Dictionary<Unit, List<GameHex>> moveTarget = new Dictionary<Unit, List<GameHex>>();

		// Token: 0x04001F13 RID: 7955
		public Dictionary<Unit, int> moveDistance = new Dictionary<Unit, int>();

		// Token: 0x04001F14 RID: 7956
		public Dictionary<Mech, List<Worker>> moveMechPassengers = new Dictionary<Mech, List<Worker>>();

		// Token: 0x04001F15 RID: 7957
		public int movePriorityHighest;

		// Token: 0x04001F16 RID: 7958
		public int workerCountTarget = 3;

		// Token: 0x04001F17 RID: 7959
		public int workersInaVillage;

		// Token: 0x04001F18 RID: 7960
		public int workersOutOfBase;

		// Token: 0x04001F19 RID: 7961
		public Dictionary<int, int> mechPriority = new Dictionary<int, int>
		{
			{ 0, 10 },
			{ 1, 9 },
			{ 2, 8 },
			{ 3, 7 }
		};

		// Token: 0x04001F1A RID: 7962
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

		// Token: 0x04001F1B RID: 7963
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

		// Token: 0x04001F1C RID: 7964
		public Dictionary<GainType, int> recruitOneTimePriority = new Dictionary<GainType, int>
		{
			{
				GainType.CombatCard,
				10
			},
			{
				GainType.Popularity,
				9
			},
			{
				GainType.Coin,
				8
			},
			{
				GainType.Power,
				7
			}
		};

		// Token: 0x04001F1D RID: 7965
		public int mechNext;

		// Token: 0x04001F1E RID: 7966
		public BuildingType buildingNext;

		// Token: 0x04001F1F RID: 7967
		public bool produceLoopPresent;

		// Token: 0x04001F20 RID: 7968
		public bool tradeLoopPresent;

		// Token: 0x04001F21 RID: 7969
		public ResourceType tradeLoopResource = ResourceType.combatCard;

		// Token: 0x04001F22 RID: 7970
		public GameHex preferredDeployPosition;

		// Token: 0x04001F23 RID: 7971
		public bool gottaMoveToBuild;

		// Token: 0x04001F24 RID: 7972
		public bool objectiveBalancedWorkforce;

		// Token: 0x04001F25 RID: 7973
		public bool objectiveMachineOverMuscle;

		// Token: 0x04001F26 RID: 7974
		protected GameManager gameManager;
	}
}

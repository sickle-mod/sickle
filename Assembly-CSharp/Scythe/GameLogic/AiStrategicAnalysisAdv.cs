using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005A3 RID: 1443
	public class AiStrategicAnalysisAdv : AiStrategicAnalysis
	{
		// Token: 0x06002DC5 RID: 11717 RVA: 0x000448D4 File Offset: 0x00042AD4
		public AiStrategicAnalysisAdv(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x00110A88 File Offset: 0x0010EC88
		public override void Run(AiPlayer aiPlayer, int priMoveToProduction1, int priMoveToProduction2, int priMoveToEncounter, int priMoveToFight, int priMoveToBuild)
		{
			this.preferredDeployPosition = null;
			this.moveToObjective = false;
			this.objectiveBalancedWorkforce = false;
			foreach (ObjectiveCard objectiveCard in aiPlayer.player.objectiveCards)
			{
				if (objectiveCard.status == ObjectiveCard.ObjectiveStatus.Open && objectiveCard.CardId == 15)
				{
					this.objectiveBalancedWorkforce = true;
				}
				if (objectiveCard.status == ObjectiveCard.ObjectiveStatus.Open && objectiveCard.CardId == 7)
				{
					this.objectiveMachineOverMuscle = true;
				}
			}
			base.UpdateEncounterAndFactory(aiPlayer);
			base.UpdateResourceAccess(aiPlayer);
			base.UpdateTradeLoop(aiPlayer);
			base.UpdateResourceDemand(aiPlayer);
			this.UpdateResourceDemandPriority(aiPlayer, this.resourceDemandTotal);
			base.UpdateProduceLoop(aiPlayer);
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
			this.UpdateObjectiveArea(aiPlayer);
			this.UpdateWorkerCountTarget(aiPlayer);
			this.UpdateUselessWorkers4Production(aiPlayer);
			base.UpdateMoveRangeAll(aiPlayer);
			this.movePriority.Clear();
			this.moveTarget.Clear();
			this.movePrioritySorted.Clear();
			this.moveDistance.Clear();
			this.enemyCanBeAttackedBy.Clear();
			this.moveRange.Clear();
			this.UpdateMoveTargetsProduction(aiPlayer, priMoveToProduction1, priMoveToProduction2, priMoveToEncounter, priMoveToFight, priMoveToBuild);
			this.UpdateMoveTargetsObjective(aiPlayer, priMoveToProduction1, priMoveToProduction2, priMoveToEncounter, priMoveToFight, priMoveToBuild);
			this.UpdateMoveTargetsObjectiveSendOneBackAsAWarning(aiPlayer, priMoveToProduction1, priMoveToProduction2, priMoveToEncounter, priMoveToFight, priMoveToBuild);
			this.UpdateMoveTargetsCombat(aiPlayer, priMoveToProduction1, priMoveToProduction2, priMoveToEncounter, priMoveToFight, priMoveToBuild);
			this.UpdateMoveTargetsSecondary(aiPlayer, priMoveToProduction1, priMoveToProduction2, priMoveToEncounter, priMoveToFight, priMoveToBuild);
			base.UpdateBuildingOrder(aiPlayer);
			this.UpdateMechOrder(aiPlayer);
			base.UpdateRecruitOrder(aiPlayer);
			this.UpdateRecruitOneTimeOrder(aiPlayer);
			this.ScatterCheck(aiPlayer, 2000);
			foreach (KeyValuePair<Unit, int> keyValuePair in this.movePriority)
			{
				this.movePrioritySorted.Add(keyValuePair.Value, keyValuePair.Key);
			}
			if (this.movePriority.Values.Count > 0)
			{
				this.movePriorityHighest = this.movePriority.Values.Max();
				return;
			}
			this.movePriorityHighest = -1;
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x00110E2C File Offset: 0x0010F02C
		private void ScatterCheck(AiPlayer aiPlayer, int pri)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (Unit unit in this.moveTarget.Keys)
			{
				foreach (GameHex gameHex in this.moveTarget[unit])
				{
					if (gameHex.Owner != null && gameHex.Owner != aiPlayer.player && gameHex.GetOwnerUnits().Count > 0)
					{
						flag2 = true;
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && aiPlayer.player.matPlayer.buildings.Count == 3 && aiPlayer.player.Resources(false)[ResourceType.wood] >= this.resourceCostSingleAction[ResourceType.wood] && !flag2)
			{
				flag = true;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				int num = 0;
				foreach (GameHex gameHex2 in aiPlayer.player.OwnedFields(false))
				{
					if (gameHex2.GetOwnerWorkers().Count > 0 && gameHex2.hexType != HexType.capital && gameHex2.hexType != HexType.lake && gameHex2.Building == null)
					{
						num++;
					}
				}
				if (num <= 1 && aiPlayer.player.Resources(false)[ResourceType.wood] >= this.resourceCostSingleAction[ResourceType.wood])
				{
					flag = true;
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				int num2 = 0;
				foreach (Player player in this.gameManager.GetAIPlayers())
				{
					if (player.GetNumberOfStars() > num2)
					{
						num2 = player.GetNumberOfStars();
					}
				}
				if (num2 >= 5)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.Scatter(aiPlayer, pri);
			}
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000448E8 File Offset: 0x00042AE8
		public void UpdateRecruitOneTimeOrder(AiPlayer aiPlayer)
		{
			if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType != PlayerMatType.Patriotic)
			{
				this.recruitOneTimePriority[GainType.Power] = 20;
			}
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x001110AC File Offset: 0x0010F2AC
		public override void UpdateMechOrder(AiPlayer aiPlayer)
		{
			int num = -1;
			switch (aiPlayer.player.matFaction.faction)
			{
			case Faction.Polania:
				this.mechPriority[0] = 4;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 10;
				break;
			case Faction.Albion:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant || aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					this.mechPriority[0] = 10;
					this.mechPriority[1] = 6;
					this.mechPriority[2] = 4;
					this.mechPriority[3] = 8;
				}
				break;
			case Faction.Nordic:
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
				break;
			case Faction.Rusviet:
				this.mechPriority[0] = 10;
				this.mechPriority[1] = 4;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 8;
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					this.mechPriority[0] = 10;
					this.mechPriority[1] = 8;
					this.mechPriority[2] = 6;
					this.mechPriority[3] = 4;
				}
				break;
			case Faction.Togawa:
				if (aiPlayer.player.matPlayer.matType != PlayerMatType.Industrial && aiPlayer.player.matPlayer.matType != PlayerMatType.Patriotic)
				{
					this.mechPriority[0] = 8;
					this.mechPriority[1] = 6;
					this.mechPriority[2] = 4;
					this.mechPriority[3] = 10;
				}
				break;
			case Faction.Crimea:
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 4;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 10;
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					this.mechPriority[3] = 11;
				}
				break;
			case Faction.Saxony:
				this.mechPriority[0] = 4;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 6;
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
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
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

		// Token: 0x06002DCA RID: 11722 RVA: 0x00111420 File Offset: 0x0010F620
		protected override float ResourcePriority(AiPlayer aiPlayer, GameHex hex)
		{
			ResourceType resourceType = AiStrategicAnalysis.ResourceProduced(hex.hexType);
			if (this.resourceDemandPriority.ContainsKey(resourceType))
			{
				float num = (float)this.resourceDemandPriority[resourceType];
				if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && base.HasNeighbourTunel(aiPlayer, hex) && num != 0f)
				{
					num += 0.4f;
				}
				if (hex.hasTunnel && ((aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)))
				{
					num += 0.5f;
				}
				if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && hex.Owner == null)
				{
					num += 0.6f;
				}
				if (this.gottaMoveToBuild && hex.Building == null)
				{
					num += 100f;
				}
				return num;
			}
			if (hex.hexType == HexType.village && aiPlayer.player.matPlayer.workers.Count < this.workerCountTarget)
			{
				return 10f;
			}
			return 0f;
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x001115C4 File Offset: 0x0010F7C4
		protected override void UpdateResourceDemandPriority(AiPlayer aiPlayer, Dictionary<ResourceType, int> resourceDemand)
		{
			this.resourceDemandPriority[ResourceType.oil] = 5;
			this.resourceDemandPriority[ResourceType.food] = 4;
			this.resourceDemandPriority[ResourceType.metal] = 3;
			this.resourceDemandPriority[ResourceType.wood] = 2;
			switch (aiPlayer.player.matFaction.faction)
			{
			case Faction.Polania:
				if (aiPlayer.player.matPlayer.buildings.Count == 0)
				{
					this.resourceDemandPriority[ResourceType.wood] = 10;
				}
				if (aiPlayer.player.matFaction.mechs.Count == 0)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
				}
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					this.resourceDemandPriority[ResourceType.oil] = 1;
				}
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && aiPlayer.player.matPlayer.GetBuilding(BuildingType.Mill) != null && aiPlayer.player.matPlayer.GetBuilding(BuildingType.Mill).position.Owner == aiPlayer.player && aiPlayer.player.matPlayer.GetBuilding(BuildingType.Mill).position.hexType == HexType.tundra && aiPlayer.player.matPlayer.GetBuilding(BuildingType.Mill).position.GetOwnerWorkers().Count >= 2)
				{
					this.resourceDemandPriority[ResourceType.oil] = 1;
				}
				break;
			case Faction.Albion:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					this.resourceDemandPriority[ResourceType.food] = 4;
					this.resourceDemandPriority[ResourceType.metal] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
					this.resourceDemandPriority[ResourceType.oil] = 1;
				}
				break;
			case Faction.Nordic:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic || aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
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
				break;
			case Faction.Togawa:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative && this.resourceAccess[ResourceType.metal] >= 2)
				{
					this.resourceDemandPriority[ResourceType.metal] = 1;
				}
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && this.resourceAccess[ResourceType.food] >= 0 && this.resourceAccess[ResourceType.wood] == 0)
				{
					this.resourceDemandPriority[ResourceType.wood] = 10;
				}
				break;
			case Faction.Saxony:
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
				break;
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

		// Token: 0x06002DCC RID: 11724 RVA: 0x00111B48 File Offset: 0x0010FD48
		protected override void UpdateUselessWorkers4Production(AiPlayer aiPlayer)
		{
			this.uselessWorkers4Production.Clear();
			this.uselessWorkersTargets.Clear();
			this.moveMechPassengers.Clear();
			int num = 0;
			foreach (GameHex gameHex in aiPlayer.player.OwnedFields(false))
			{
				if (gameHex.GetOwnerWorkers().Count > 0 && gameHex.hexType != HexType.capital && gameHex.hexType != HexType.lake && gameHex.Building == null)
				{
					num++;
				}
			}
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
					GameHex gameHex2 = null;
					foreach (GameHex gameHex3 in worker4.position.GetFieldsAccessible(worker4, false))
					{
						if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
						{
							ResourceType resourceType = AiStrategicAnalysis.ResourceProduced(gameHex3.hexType);
							if (this.ResourcePriority(aiPlayer, gameHex3) > 0f && (gameHex3.Owner == null || gameHex3.Owner == aiPlayer.player) && resourceType != ResourceType.combatCard && (gameHex2 == null || (this.ResourcePriority(aiPlayer, gameHex3) > this.ResourcePriority(aiPlayer, gameHex2) && (this.resourceAccess[resourceType] <= 0 || this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex2.hexType)] != 0)) || (this.resourceAccess[resourceType] == 0 && this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex2.hexType)] > 0)))
							{
								gameHex2 = gameHex3;
							}
						}
						else if (this.ResourcePriority(aiPlayer, gameHex3) > 0f && (gameHex3.Owner == null || gameHex3.Owner == aiPlayer.player) && (gameHex2 == null || this.ResourcePriority(aiPlayer, gameHex3) > this.ResourcePriority(aiPlayer, gameHex2)))
						{
							gameHex2 = gameHex3;
						}
					}
					if (gameHex2 != null)
					{
						this.uselessWorkersTargets.Add(worker4, gameHex2);
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering && aiPlayer.player.matFaction.mechs.Count > 0 && this.resourceAccess[ResourceType.oil] == 1 && this.resourceCostSingleAction[ResourceType.oil] > 1)
			{
				Mech mech2 = aiPlayer.player.matFaction.mechs[0];
				if (this.moveMechPassengers.ContainsKey(mech2) && this.moveMechPassengers[mech2].Count > 0 && mech2.position.hexType == HexType.village)
				{
					GameHex gameHex4 = null;
					foreach (GameHex gameHex5 in this.moveMechPassengers[mech2][0].position.GetFieldsAccessible(this.moveMechPassengers[mech2][0], false))
					{
						if (gameHex5.hexType == HexType.tundra && (gameHex5.Owner == null || gameHex5.Owner == aiPlayer.player) && gameHex4 == null)
						{
							gameHex4 = gameHex5;
						}
					}
					if (gameHex4 != null)
					{
						Worker worker5 = this.moveMechPassengers[mech2][0];
						this.moveMechPassengers[mech2].RemoveAt(0);
						this.uselessWorkersTargets.Add(worker5, gameHex4);
					}
				}
			}
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x00112198 File Offset: 0x00110398
		private void UpdateObjectiveArea(AiPlayer aiPlayer)
		{
			int num = 0;
			Dictionary<int, int> dictionary = new Dictionary<int, int>
			{
				{ 1, 9 },
				{ 2, 10 },
				{ 3, 9 },
				{ 4, 9 },
				{ 10, 9 },
				{ 11, 9 },
				{ 17, 8 }
			};
			foreach (ObjectiveCard objectiveCard in aiPlayer.player.objectiveCards)
			{
				if (dictionary.ContainsKey(objectiveCard.CardId) && objectiveCard.status == ObjectiveCard.ObjectiveStatus.Open && (num == 0 || dictionary[objectiveCard.CardId] > dictionary[num]))
				{
					num = objectiveCard.CardId;
				}
			}
			HashSet<GameHex> hashSet = new HashSet<GameHex>();
			switch (num)
			{
			case 1:
				hashSet = new HashSet<GameHex>(this.gameManager.gameBoard.mountains);
				this.objectiveTarget = 3;
				break;
			case 2:
				hashSet = new HashSet<GameHex>(this.gameManager.gameBoard.tunnels);
				this.objectiveTarget = 3;
				break;
			case 3:
				hashSet = new HashSet<GameHex>(this.gameManager.gameBoard.farms);
				this.objectiveTarget = 3;
				break;
			case 4:
				hashSet = new HashSet<GameHex>(this.gameManager.gameBoard.tundras);
				this.objectiveTarget = 3;
				break;
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				break;
			case 10:
				hashSet = new HashSet<GameHex>(this.gameManager.gameBoard.forest);
				this.objectiveTarget = 3;
				break;
			case 11:
				hashSet = new HashSet<GameHex>(this.gameManager.gameBoard.villages);
				this.objectiveTarget = 3;
				break;
			default:
				if (num == 17)
				{
					if (aiPlayer.player.matPlayer.matPlayerSectionsCount <= 4)
					{
						hashSet = new HashSet<GameHex>(this.gameManager.gameBoard.factory.GetNeighboursAll());
						this.objectiveTarget = 2;
					}
				}
				break;
			}
			this.objectiveArea.Clear();
			foreach (GameHex gameHex in hashSet)
			{
				if (gameHex.Owner == null || gameHex.Owner == aiPlayer.player || gameHex.GetOwnerUnitCount() == 0)
				{
					this.objectiveArea.Add(gameHex);
				}
			}
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x00112420 File Offset: 0x00110620
		protected void Scatter(AiPlayer aiPlayer, int pri)
		{
			this.movePriority.Clear();
			this.moveTarget.Clear();
			this.movePrioritySorted.Clear();
			this.moveDistance.Clear();
			this.enemyCanBeAttackedBy.Clear();
			this.moveRange.Clear();
			HashSet<GameHex> hashSet = new HashSet<GameHex>();
			foreach (Unit unit in this.moveRangeAll.Keys)
			{
				if (!this.movePriority.ContainsKey(unit) && unit.position.hexType == HexType.factory)
				{
					this.movePriority.Add(unit, 0);
					this.moveTarget.Add(unit, new List<GameHex> { unit.position });
					this.moveDistance.Add(unit, this.moveRangeAll[unit][unit.position]);
				}
			}
			Unit unit2 = null;
			if (this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0)
			{
				foreach (Unit unit3 in this.moveRangeAll.Keys)
				{
					if (!this.movePriority.ContainsKey(unit3) && this.moveRangeAll[unit3].ContainsKey(this.gameManager.gameBoard.factory))
					{
						unit2 = unit3;
					}
				}
			}
			if (unit2 != null)
			{
				this.movePriority.Add(unit2, pri + 10);
				this.moveTarget.Add(unit2, new List<GameHex> { this.gameManager.gameBoard.factory });
				this.moveDistance.Add(unit2, this.moveRangeAll[unit2][this.gameManager.gameBoard.factory]);
				hashSet.Add(this.gameManager.gameBoard.factory);
			}
			foreach (Unit unit4 in this.moveRangeAll.Keys)
			{
				if (!this.movePriority.ContainsKey(unit4))
				{
					foreach (GameHex gameHex in this.moveRangeAll[unit4].Keys)
					{
						if (!this.movePriority.ContainsKey(unit4) && gameHex.Owner != null && gameHex.Owner != aiPlayer.player && gameHex.Owner.GetAllUnits().Count == 0 && !hashSet.Contains(gameHex))
						{
							this.movePriority.Add(unit4, pri + 8);
							this.moveTarget.Add(unit4, new List<GameHex> { gameHex });
							this.moveDistance.Add(unit4, this.moveRangeAll[unit4][gameHex]);
							hashSet.Add(gameHex);
						}
					}
				}
			}
			foreach (Unit unit5 in this.moveRangeAll.Keys)
			{
				if (!this.movePriority.ContainsKey(unit5) && unit5.position.GetOwnerUnits().Count > 1)
				{
					foreach (GameHex gameHex2 in this.moveRangeAll[unit5].Keys)
					{
						if (!this.movePriority.ContainsKey(unit5) && gameHex2.Owner == null && !hashSet.Contains(gameHex2))
						{
							this.movePriority.Add(unit5, pri + 6);
							this.moveTarget.Add(unit5, new List<GameHex> { gameHex2 });
							this.moveDistance.Add(unit5, this.moveRangeAll[unit5][gameHex2]);
							hashSet.Add(gameHex2);
						}
					}
				}
			}
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x001128F4 File Offset: 0x00110AF4
		protected void UpdateMoveTargetsProduction(AiPlayer aiPlayer, int priProduce1, int priProduce2, int priEncounter, int priFight, int priBuildSpot)
		{
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
			foreach (Mech mech in this.moveMechPassengers.Keys)
			{
				GameHex gameHex = null;
				if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator3 = this.moveRangeAll[mech].Keys.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							GameHex gameHex2 = enumerator3.Current;
							if (this.ResourcePriority(aiPlayer, gameHex2) > 0f && (gameHex2.Owner == null || gameHex2.Owner == aiPlayer.player) && (gameHex == null || (this.ResourcePriority(aiPlayer, gameHex2) > this.ResourcePriority(aiPlayer, gameHex) && this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex2.hexType)] <= this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex.hexType)])))
							{
								gameHex = gameHex2;
							}
						}
						goto IL_0221;
					}
					goto IL_01A0;
				}
				goto IL_01A0;
				IL_0221:
				if (gameHex == null)
				{
					continue;
				}
				bool flag = false;
				GameHex gameHex3 = null;
				GameHex gameHex4 = null;
				if (aiPlayer.player.matFaction.SkillUnlocked[3] && ((aiPlayer.player.matFaction.faction == Faction.Saxony && (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical || aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial || aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)))
				{
					foreach (GameHex gameHex5 in this.moveRangeAll[mech].Keys)
					{
						if (this.ResourcePriority(aiPlayer, gameHex5) > 0f && (gameHex5.Owner == null || gameHex5.Owner == aiPlayer.player) && this.moveRangeAll[mech][gameHex5] == 1 && AiStrategicAnalysis.ResourceProduced(gameHex5.hexType) != ResourceType.combatCard && (gameHex3 == null || this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex5.hexType)] <= this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex3.hexType)]) && (gameHex3 == null || this.ResourcePriority(aiPlayer, gameHex5) > this.ResourcePriority(aiPlayer, gameHex3)))
						{
							gameHex3 = gameHex5;
						}
					}
					if (gameHex3 != null)
					{
						ResourceType resourceType = AiStrategicAnalysis.ResourceProduced(gameHex3.hexType);
						int num2 = this.resourceCostSingleAction[resourceType];
						if (this.moveMechPassengers[mech].Count > num2)
						{
							Dictionary<GameHex, int> dictionary;
							this.gameManager.gameBoard.MoveRange(mech, gameHex3, 1, out dictionary);
							foreach (GameHex gameHex6 in dictionary.Keys)
							{
								if (this.ResourcePriority(aiPlayer, gameHex6) > 0f && (gameHex6.Owner == null || gameHex6.Owner == aiPlayer.player) && gameHex6.hexType != gameHex3.hexType && AiStrategicAnalysis.ResourceProduced(gameHex6.hexType) != ResourceType.combatCard && this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex6.hexType)] == 0 && (gameHex4 == null || this.ResourcePriority(aiPlayer, gameHex6) > this.ResourcePriority(aiPlayer, gameHex4)))
								{
									gameHex4 = gameHex6;
								}
							}
							if (gameHex4 != null)
							{
								flag = true;
							}
						}
					}
				}
				if (flag)
				{
					this.movePriority.Add(mech, (this.moveMechPassengers[mech].Count < 2) ? priProduce1 : (priProduce2 + 2));
					this.moveTarget.Add(mech, new List<GameHex> { gameHex3, gameHex4 });
					this.moveDistance.Add(mech, 2);
				}
				else
				{
					this.movePriority.Add(mech, (this.moveMechPassengers[mech].Count < 2) ? priProduce1 : (priProduce2 + 2));
					this.moveTarget.Add(mech, new List<GameHex> { gameHex });
					this.moveDistance.Add(mech, this.moveRangeAll[mech][gameHex]);
				}
				if (mech.position.GetOwnerMechs().Count <= 1 || aiPlayer.player.matFaction.faction != Faction.Saxony || aiPlayer.player.matPlayer.matType != PlayerMatType.Industrial)
				{
					continue;
				}
				Mech mech2 = null;
				foreach (Mech mech3 in mech.position.GetOwnerMechs())
				{
					if (mech3 != mech)
					{
						mech2 = mech3;
					}
				}
				if (mech2 != null && !this.moveTarget.ContainsKey(mech2))
				{
					this.movePriority.Add(mech2, 1);
					this.moveTarget.Add(mech2, new List<GameHex> { gameHex });
					this.moveDistance.Add(mech2, this.moveRangeAll[mech2][gameHex]);
					continue;
				}
				continue;
				IL_01A0:
				foreach (GameHex gameHex7 in this.moveRangeAll[mech].Keys)
				{
					if (this.ResourcePriority(aiPlayer, gameHex7) > 0f && (gameHex7.Owner == null || gameHex7.Owner == aiPlayer.player) && (gameHex == null || this.ResourcePriority(aiPlayer, gameHex7) > this.ResourcePriority(aiPlayer, gameHex)))
					{
						gameHex = gameHex7;
					}
				}
				goto IL_0221;
			}
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x00113020 File Offset: 0x00111220
		protected void UpdateMoveTargetsObjective(AiPlayer aiPlayer, int priProduce1, int priProduce2, int priEncounter, int priFight, int priBuildSpot)
		{
			if (this.objectiveArea.Count > 0 && ((aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)))
			{
				int num = 0;
				Dictionary<Unit, int> dictionary = new Dictionary<Unit, int>();
				Dictionary<Unit, List<GameHex>> dictionary2 = new Dictionary<Unit, List<GameHex>>();
				Dictionary<Unit, int> dictionary3 = new Dictionary<Unit, int>();
				foreach (Unit unit in this.moveRangeAll.Keys)
				{
					if (!this.movePriority.ContainsKey(unit) && !dictionary.ContainsKey(unit) && this.objectiveArea.Contains(unit.position))
					{
						num++;
						this.objectiveArea.Remove(unit.position);
						dictionary.Add(unit, 0);
						dictionary2.Add(unit, new List<GameHex> { unit.position });
						dictionary3.Add(unit, 0);
					}
				}
				if (this.objectiveTarget - num <= (int)aiPlayer.AiTopActions[GainType.Move].GetTopGainAction().Amount)
				{
					foreach (Unit unit2 in this.moveRangeAll.Keys)
					{
						if (num < this.objectiveTarget && !this.movePriority.ContainsKey(unit2) && !dictionary.ContainsKey(unit2))
						{
							foreach (GameHex gameHex in this.moveRangeAll[unit2].Keys)
							{
								if (this.objectiveArea.Contains(gameHex) && !this.movePriority.ContainsKey(unit2) && !dictionary.ContainsKey(unit2))
								{
									num++;
									this.objectiveArea.Remove(gameHex);
									dictionary.Add(unit2, priFight + 100);
									dictionary2.Add(unit2, new List<GameHex> { gameHex });
									dictionary3.Add(unit2, this.moveRangeAll[unit2][gameHex]);
								}
							}
						}
					}
					if (num >= this.objectiveTarget)
					{
						this.moveToObjective = true;
						foreach (Unit unit3 in dictionary.Keys)
						{
							this.movePriority.Add(unit3, dictionary[unit3]);
						}
						foreach (Unit unit4 in dictionary2.Keys)
						{
							this.moveTarget.Add(unit4, dictionary2[unit4]);
						}
						foreach (Unit unit5 in dictionary3.Keys)
						{
							this.moveDistance.Add(unit5, dictionary3[unit5]);
						}
					}
				}
			}
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x001134A8 File Offset: 0x001116A8
		protected void UpdateMoveTargetsObjectiveSendOneBackAsAWarning(AiPlayer aiPlayer, int priProduce1, int priProduce2, int priEncounter, int priFight, int priBuildSpot)
		{
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && aiPlayer.player.Power >= 7)
			{
				bool flag = false;
				foreach (ObjectiveCard objectiveCard in aiPlayer.player.objectiveCards)
				{
					if (objectiveCard.CardId == 6 && objectiveCard.status == ObjectiveCard.ObjectiveStatus.Open)
					{
						flag = true;
					}
				}
				if (flag)
				{
					GameHex gameHex = null;
					Unit unit = null;
					foreach (Unit unit2 in this.moveRangeAll.Keys)
					{
						if ((unit2.UnitType == UnitType.Mech || unit2.UnitType == UnitType.Character) && !this.movePriority.ContainsKey(unit2))
						{
							foreach (GameHex gameHex2 in this.moveRangeAll[unit2].Keys)
							{
								if (gameHex2.Owner != aiPlayer.player && gameHex2.GetOwnerWorkers().Count >= 1 && gameHex2.GetOwnerWorkers().Count <= 2 && gameHex2.GetOwnerWorkers().Count == gameHex2.GetOwnerUnits().Count && (gameHex == null || this.ObjectiveSendOneBackPriority(unit2, gameHex2) > this.ObjectiveSendOneBackPriority(unit, gameHex)))
								{
									gameHex = gameHex2;
									unit = unit2;
								}
							}
						}
					}
					if (gameHex != null)
					{
						this.movePriority.Add(unit, priFight + 100);
						this.moveTarget.Add(unit, new List<GameHex> { gameHex });
						this.moveDistance.Add(unit, this.moveRangeAll[unit][gameHex]);
					}
				}
			}
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x0004491E File Offset: 0x00042B1E
		private int ObjectiveSendOneBackPriority(Unit unit, GameHex hex)
		{
			if (unit.UnitType == UnitType.Mech)
			{
				return 10;
			}
			return 1;
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x001136C4 File Offset: 0x001118C4
		protected override float isWorthAttacking(GameHex hex, AiPlayer aiPlayer)
		{
			if (aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Crimea)
			{
				return (float)(2 - hex.GetOwnerWorkers().Count);
			}
			if ((aiPlayer.player.matFaction.faction != Faction.Saxony || aiPlayer.player.matPlayer.matType != PlayerMatType.Industrial) && (aiPlayer.player.matFaction.faction != Faction.Saxony || aiPlayer.player.matPlayer.matType != PlayerMatType.Mechanical) && (aiPlayer.player.matFaction.faction != Faction.Saxony || aiPlayer.player.matPlayer.matType != PlayerMatType.Engineering) && (aiPlayer.player.matFaction.faction != Faction.Polania || aiPlayer.player.matPlayer.matType != PlayerMatType.Industrial) && (aiPlayer.player.matFaction.faction != Faction.Albion || aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial))
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
			if (num > hex.Owner.combatCards.Count<CombatCard>())
			{
				num = hex.Owner.combatCards.Count<CombatCard>();
			}
			int num2 = 0;
			for (int i = 0; i < this.enemyCanBeAttackedBy[hex].Count; i++)
			{
				if (!this.movePriority.ContainsKey(this.enemyCanBeAttackedBy[hex][i]))
				{
					num2++;
				}
			}
			if (num2 > 2)
			{
				num2 = 2;
			}
			if (num2 > aiPlayer.player.combatCards.Count)
			{
				num2 = aiPlayer.player.combatCards.Count;
			}
			if (num > num2)
			{
				return 0f;
			}
			if (num == num2 && hex.Owner.Power >= aiPlayer.player.Power && hex.Owner.combatCards.Count >= aiPlayer.player.combatCards.Count)
			{
				return 0f;
			}
			float num3 = (float)(hex.GetResourceCount() / 2);
			if (aiPlayer.player.Popularity < 7 && aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				return num3;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony)
			{
				num3 += 1.5f - 1.5f * (float)hex.GetOwnerWorkers().Count;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matFaction.SkillUnlocked[2])
			{
				num3 += (float)(hex.GetOwnerWorkers().Count / 8);
			}
			else
			{
				num3 += (float)(2 - hex.GetOwnerWorkers().Count);
			}
			if (num3 > 0f && this.enemyCanBeAttackedBy[hex].Count<Unit>() > 1)
			{
				num3 += 10f;
			}
			return num3;
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x001139DC File Offset: 0x00111BDC
		protected void UpdateMoveTargetsCombat(AiPlayer aiPlayer, int priProduce1, int priProduce2, int priEncounter, int priFight, int priBuildSpot)
		{
			foreach (Mech mech in aiPlayer.player.matFaction.mechs)
			{
				if (!this.movePriority.ContainsKey(mech))
				{
					this.moveRange.Add(mech, this.moveRangeAll[mech]);
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
			}
			if (!this.movePriority.ContainsKey(aiPlayer.player.character) && ((aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)))
			{
				this.moveRange.Add(aiPlayer.player.character, this.moveRangeAll[aiPlayer.player.character]);
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
			if (priFight > 0 && aiPlayer.strategicAnalysis.enemyCanBeAttackedBy.Count > 0 && (aiPlayer.player.Power >= 7 || (aiPlayer.player.matFaction.faction == Faction.Saxony && (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial || aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering))) && (aiPlayer.player.GetNumberOfStars(StarType.Combat) < 2 || aiPlayer.player.matFaction.factionPerk == AbilityPerk.Dominate))
			{
				GameHex gameHex3 = null;
				float num = 0f;
				if (aiPlayer.player.matFaction.faction == Faction.Saxony && (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial || aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering || aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical))
				{
					using (Dictionary<GameHex, List<Unit>>.KeyCollection.Enumerator enumerator3 = this.enemyCanBeAttackedBy.Keys.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							GameHex gameHex4 = enumerator3.Current;
							if (this.isWorthAttacking(gameHex4, aiPlayer) > num && aiPlayer.CombatPreparation(true, gameHex4, gameHex4.Owner))
							{
								gameHex3 = gameHex4;
								num = this.isWorthAttacking(gameHex4, aiPlayer);
							}
						}
						goto IL_0405;
					}
				}
				foreach (GameHex gameHex5 in this.enemyCanBeAttackedBy.Keys)
				{
					if (this.isWorthAttacking(gameHex5, aiPlayer) > 0f)
					{
						gameHex3 = gameHex5;
					}
				}
				IL_0405:
				if (gameHex3 != null)
				{
					Unit unit = null;
					if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
					{
						for (int i = 0; i < this.enemyCanBeAttackedBy[gameHex3].Count<Unit>(); i++)
						{
							if (unit == null && !this.movePriority.ContainsKey(this.enemyCanBeAttackedBy[gameHex3][i]))
							{
								unit = this.enemyCanBeAttackedBy[gameHex3][i];
							}
						}
					}
					else
					{
						unit = this.enemyCanBeAttackedBy[gameHex3][0];
					}
					if (unit != null && !this.movePriority.ContainsKey(unit))
					{
						this.movePriority.Add(unit, priFight);
						this.moveTarget.Add(unit, new List<GameHex> { gameHex3 });
						this.moveDistance.Add(unit, this.moveRange[unit][gameHex3]);
						if (aiPlayer.player.matFaction.abilities.Contains(AbilityPerk.PeoplesArmy) && aiPlayer.player.matFaction.SkillUnlocked[2] && (AiStrategicAnalysis.ResourceProduced(gameHex3.hexType) == ResourceType.combatCard || unit.position.GetOwnerWorkers().Count > this.resourceCostSingleAction[AiStrategicAnalysis.ResourceProduced(gameHex3.hexType)]))
						{
							Worker worker = null;
							foreach (Worker worker2 in unit.position.GetOwnerWorkers())
							{
								if (worker == null)
								{
									bool flag = true;
									foreach (Mech mech2 in this.moveMechPassengers.Keys)
									{
										foreach (Worker worker3 in this.moveMechPassengers[mech2])
										{
											if (worker == worker3)
											{
												flag = false;
											}
										}
									}
									if (flag)
									{
										worker = worker2;
									}
								}
							}
							if (worker != null && unit.UnitType == UnitType.Mech)
							{
								if (!this.moveMechPassengers.ContainsKey((Mech)unit))
								{
									this.moveMechPassengers.Add((Mech)unit, new List<Worker>());
								}
								this.moveMechPassengers[(Mech)unit].Add(worker);
							}
						}
					}
					if ((aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial))
					{
						int num2 = 0;
						for (int j = 0; j < this.enemyCanBeAttackedBy[gameHex3].Count<Unit>(); j++)
						{
							if (num2 == 0 && !this.movePriority.ContainsKey(this.enemyCanBeAttackedBy[gameHex3][j]))
							{
								Unit unit2 = this.enemyCanBeAttackedBy[gameHex3][j];
								this.movePriority.Add(unit2, priFight);
								this.moveTarget.Add(unit2, new List<GameHex> { gameHex3 });
								this.moveDistance.Add(unit2, this.moveRange[unit2][gameHex3]);
								num2++;
							}
						}
					}
				}
			}
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x00114210 File Offset: 0x00112410
		private void UpdateMoveTargetsSecondary(AiPlayer aiPlayer, int priProduce1, int priProduce2, int priEncounter, int priFight, int priBuildSpot)
		{
			if (aiPlayer.player.matPlayer.workers.Count < this.workerCountTarget)
			{
				this.workersInaVillage = 0;
				foreach (Worker worker in aiPlayer.player.matPlayer.workers)
				{
					if (worker.position.hexType == HexType.village)
					{
						this.workersInaVillage++;
					}
					if (worker.position.hexType != HexType.capital)
					{
						this.workersOutOfBase++;
					}
				}
				if (this.workersInaVillage == 0)
				{
					Worker worker2 = null;
					GameHex gameHex = null;
					foreach (Worker worker3 in aiPlayer.player.matPlayer.workers)
					{
						if (worker3.position != null && (worker2 == null || this.ResourcePriority(aiPlayer, worker3.position) < this.ResourcePriority(aiPlayer, worker2.position)))
						{
							foreach (GameHex gameHex2 in worker3.position.GetFieldsAccessible(worker3, false))
							{
								if (gameHex2.hexType == HexType.village)
								{
									worker2 = worker3;
									gameHex = gameHex2;
								}
							}
						}
					}
					if (worker2 != null)
					{
						if (!this.movePriority.ContainsKey(worker2))
						{
							this.movePriority.Add(worker2, priProduce2 + 1);
							this.moveTarget.Add(worker2, new List<GameHex> { gameHex });
							this.moveDistance.Add(worker2, 1);
						}
						else
						{
							this.movePriority[worker2] = priProduce2 + 1;
							this.moveTarget[worker2] = new List<GameHex> { gameHex };
							this.moveDistance[worker2] = 1;
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
			if (aiPlayer.player.matFaction.faction == Faction.Nordic)
			{
				if (aiPlayer.player.matPlayer.matPlayerSectionsCount <= 4 && this.factoryDistance <= (int)aiPlayer.player.character.MaxMoveCount && !this.movePriority.ContainsKey(aiPlayer.player.character) && (this.gameManager.gameBoard.factory.Owner == aiPlayer.player || this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0))
				{
					this.movePriority.Add(aiPlayer.player.character, priEncounter + 1);
					this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { this.gameManager.gameBoard.factory });
					this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[this.gameManager.gameBoard.factory]);
				}
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					Mech mech = null;
					if (this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0 || (this.gameManager.gameBoard.factory.Owner == aiPlayer.player && this.gameManager.gameBoard.factory.GetOwnerUnitCount() - this.gameManager.gameBoard.factory.GetOwnerWorkers().Count <= 1))
					{
						foreach (Mech mech2 in aiPlayer.player.matFaction.mechs)
						{
							if (!this.movePriority.ContainsKey(mech2) && mech2.position.hexType != HexType.factory && this.moveRange[mech2].ContainsKey(this.gameManager.gameBoard.factory) && mech2.position.GetOwnerWorkers().Count == 0)
							{
								mech = mech2;
							}
						}
					}
					if (mech != null)
					{
						this.movePriority.Add(mech, priEncounter + 1);
						this.moveTarget.Add(mech, new List<GameHex> { this.gameManager.gameBoard.factory });
						this.moveDistance.Add(mech, this.moveRange[mech][this.gameManager.gameBoard.factory]);
					}
					foreach (Unit unit in this.moveRangeAll.Keys)
					{
						if (unit.position.hexType == HexType.capital && !this.movePriority.ContainsKey(unit))
						{
							GameHex gameHex3 = null;
							foreach (GameHex gameHex4 in this.moveRangeAll[unit].Keys)
							{
								if (gameHex4.hexType != HexType.capital && (gameHex4.Owner == null || gameHex4.Owner == aiPlayer.player) && (gameHex3 == null || gameHex3.Owner != null))
								{
									gameHex3 = gameHex4;
								}
							}
							if (gameHex3 != null)
							{
								this.movePriority.Add(unit, 1);
								this.moveTarget.Add(unit, new List<GameHex> { gameHex3 });
								this.moveDistance.Add(unit, this.moveRangeAll[unit][gameHex3]);
							}
						}
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony)
			{
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					Mech mech3 = null;
					if (this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0)
					{
						foreach (Mech mech4 in aiPlayer.player.matFaction.mechs)
						{
							if (!this.movePriority.ContainsKey(mech4) && this.moveRange[mech4].ContainsKey(this.gameManager.gameBoard.factory))
							{
								mech3 = mech4;
							}
						}
					}
					if (mech3 != null)
					{
						this.movePriority.Add(mech3, priEncounter + 1);
						this.moveTarget.Add(mech3, new List<GameHex> { this.gameManager.gameBoard.factory });
						this.moveDistance.Add(mech3, this.moveRange[mech3][this.gameManager.gameBoard.factory]);
					}
					mech3 = null;
					if (this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0 || (this.gameManager.gameBoard.factory.Owner == aiPlayer.player && this.gameManager.gameBoard.factory.GetOwnerUnitCount() - this.gameManager.gameBoard.factory.GetOwnerWorkers().Count <= 1))
					{
						foreach (Mech mech5 in aiPlayer.player.matFaction.mechs)
						{
							if (!this.movePriority.ContainsKey(mech5) && mech5.position.hexType != HexType.factory && this.moveRange[mech5].ContainsKey(this.gameManager.gameBoard.factory) && mech5.position.GetOwnerWorkers().Count == 0)
							{
								mech3 = mech5;
							}
						}
					}
					if (mech3 != null)
					{
						this.movePriority.Add(mech3, priEncounter + 1);
						this.moveTarget.Add(mech3, new List<GameHex> { this.gameManager.gameBoard.factory });
						this.moveDistance.Add(mech3, this.moveRange[mech3][this.gameManager.gameBoard.factory]);
					}
					foreach (Unit unit2 in this.moveRangeAll.Keys)
					{
						if (unit2.position.hexType == HexType.capital && !this.movePriority.ContainsKey(unit2))
						{
							GameHex gameHex5 = null;
							foreach (GameHex gameHex6 in this.moveRangeAll[unit2].Keys)
							{
								if (gameHex6.hexType != HexType.capital && (gameHex6.Owner == null || gameHex6.Owner == aiPlayer.player) && (gameHex5 == null || gameHex5.Owner != null))
								{
									gameHex5 = gameHex6;
								}
							}
							if (gameHex5 != null)
							{
								this.movePriority.Add(unit2, 1);
								this.moveTarget.Add(unit2, new List<GameHex> { gameHex5 });
								this.moveDistance.Add(unit2, this.moveRangeAll[unit2][gameHex5]);
							}
						}
					}
				}
				if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && aiPlayer.player.character.position.hexType != HexType.mountain && !aiPlayer.player.character.position.hasTunnel)
				{
					if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
					{
						using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator5 = this.moveRangeAll[aiPlayer.player.character].Keys.GetEnumerator())
						{
							while (enumerator5.MoveNext())
							{
								GameHex gameHex7 = enumerator5.Current;
								if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && (gameHex7.Owner == null || gameHex7.Owner == aiPlayer.player) && (gameHex7.hexType == HexType.mountain || gameHex7.hasTunnel))
								{
									this.movePriority.Add(aiPlayer.player.character, 1);
									this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex7 });
									this.moveDistance.Add(aiPlayer.player.character, this.moveRangeAll[aiPlayer.player.character][gameHex7]);
								}
							}
							goto IL_0C91;
						}
					}
					GameHex gameHex8 = null;
					foreach (GameHex gameHex9 in this.moveRangeAll[aiPlayer.player.character].Keys)
					{
						if ((gameHex9.Owner == null || gameHex9.Owner == aiPlayer.player) && (gameHex9.hexType == HexType.mountain || gameHex9.hasTunnel))
						{
							int num = 0;
							int num2 = 0;
							if (gameHex8 != null)
							{
								if (gameHex8.hasTunnel)
								{
									num += 2;
								}
								if (gameHex8.hexType == HexType.mountain)
								{
									num++;
								}
							}
							if (gameHex9.hasTunnel)
							{
								num2 += 2;
							}
							if (gameHex9.hexType == HexType.mountain)
							{
								num2++;
							}
							if (num2 > num)
							{
								gameHex8 = gameHex9;
							}
						}
					}
					if (gameHex8 != null)
					{
						this.movePriority.Add(aiPlayer.player.character, 1);
						this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex8 });
						this.moveDistance.Add(aiPlayer.player.character, this.moveRangeAll[aiPlayer.player.character][gameHex8]);
					}
				}
			}
			IL_0C91:
			if (aiPlayer.player.matFaction.faction == Faction.Polania)
			{
				if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && aiPlayer.player.character.position.hexType != HexType.lake)
				{
					foreach (GameHex gameHex10 in this.moveRangeAll[aiPlayer.player.character].Keys)
					{
						if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && (gameHex10.Owner == null || gameHex10.Owner == aiPlayer.player) && gameHex10.hexType == HexType.lake)
						{
							this.movePriority.Add(aiPlayer.player.character, 1);
							this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex10 });
							this.moveDistance.Add(aiPlayer.player.character, this.moveRangeAll[aiPlayer.player.character][gameHex10]);
						}
					}
				}
				if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && aiPlayer.player.character.position.hexType != HexType.lake && !aiPlayer.player.character.position.hasTunnel)
				{
					foreach (GameHex gameHex11 in this.moveRangeAll[aiPlayer.player.character].Keys)
					{
						if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && (gameHex11.Owner == null || gameHex11.Owner == aiPlayer.player) && (gameHex11.hexType == HexType.lake || gameHex11.hasTunnel))
						{
							this.movePriority.Add(aiPlayer.player.character, 1);
							this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex11 });
							this.moveDistance.Add(aiPlayer.player.character, this.moveRangeAll[aiPlayer.player.character][gameHex11]);
						}
					}
				}
				if (aiPlayer.player.matFaction.abilities.Contains(AbilityPerk.Camaraderie) && aiPlayer.player.matFaction.SkillUnlocked[2])
				{
					foreach (Mech mech6 in aiPlayer.player.matFaction.mechs)
					{
						if (!this.moveTarget.ContainsKey(mech6))
						{
							GameHex gameHex12 = null;
							int num3 = 0;
							foreach (GameHex gameHex13 in this.moveRangeAll[aiPlayer.player.character].Keys)
							{
								if (gameHex13.Owner != aiPlayer.player && !gameHex13.HasOwnerCharacter() && gameHex13.GetOwnerMechs().Count == 0)
								{
									int num4 = gameHex13.GetEnemyWorkers().Count + gameHex13.GetResourceCount() * 8;
									if (num4 > num3)
									{
										gameHex12 = gameHex13;
										num3 = num4;
									}
								}
							}
							if (gameHex12 != null)
							{
								this.movePriority.Add(mech6, num3);
								this.moveTarget.Add(mech6, new List<GameHex> { gameHex12 });
								this.moveDistance.Add(mech6, this.moveRangeAll[aiPlayer.player.character][gameHex12]);
							}
						}
					}
				}
				Mech mech7 = null;
				if (this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0)
				{
					foreach (Mech mech8 in aiPlayer.player.matFaction.mechs)
					{
						if (!this.movePriority.ContainsKey(mech8) && this.moveRange[mech8].ContainsKey(this.gameManager.gameBoard.factory))
						{
							mech7 = mech8;
						}
					}
				}
				if (mech7 != null)
				{
					this.movePriority.Add(mech7, priEncounter + 1);
					this.moveTarget.Add(mech7, new List<GameHex> { this.gameManager.gameBoard.factory });
					this.moveDistance.Add(mech7, this.moveRange[mech7][this.gameManager.gameBoard.factory]);
				}
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural && this.resourceCostSingleAction[ResourceType.food] <= 2 && this.resourceAccess[ResourceType.food] >= 2)
				{
					Worker worker4 = null;
					foreach (Worker worker5 in aiPlayer.player.matPlayer.workers)
					{
						if (worker5.position.hexType == HexType.farm && !this.moveTarget.ContainsKey(worker5))
						{
							worker4 = worker5;
						}
					}
					if (worker4 != null)
					{
						GameHex gameHex14 = null;
						foreach (GameHex gameHex15 in this.moveRangeAll[worker4].Keys)
						{
							if (gameHex15.Owner == null || gameHex15.Owner == aiPlayer.player)
							{
								gameHex14 = gameHex15;
							}
						}
						if (gameHex14 != null)
						{
							this.movePriority.Add(worker4, 1);
							this.moveTarget.Add(worker4, new List<GameHex> { gameHex14 });
							this.moveDistance.Add(worker4, 1);
						}
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Albion)
			{
				if ((aiPlayer.player.matPlayer.matType == PlayerMatType.Militant || aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical || aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural || aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) && !this.movePriority.ContainsKey(aiPlayer.player.character))
				{
					GameHex gameHex16 = null;
					int num5 = 0;
					foreach (GameHex gameHex17 in this.moveRangeAll[aiPlayer.player.character].Keys)
					{
						int num6 = 0;
						if (gameHex17.Token == null)
						{
							num6 += 100;
						}
						if (gameHex17.hexType != HexType.capital)
						{
							num6 += 10;
						}
						if (gameHex17.hasTunnel)
						{
							num6 += 20;
						}
						if (gameHex17.hexType == HexType.factory)
						{
							num6 += 50;
						}
						if (gameHex17.Owner != null && gameHex17.Owner.matFaction.faction != Faction.Albion && gameHex17.GetOwnerUnitCount() > 0)
						{
							num6 -= 1000;
						}
						if (num6 > num5)
						{
							gameHex16 = gameHex17;
							num5 = num6;
						}
					}
					if (gameHex16 != null)
					{
						this.movePriority.Add(aiPlayer.player.character, 4);
						this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex16 });
						this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[gameHex16]);
					}
				}
				foreach (Worker worker6 in aiPlayer.player.matPlayer.workers)
				{
					if (!this.movePriority.Keys.Contains(worker6))
					{
						bool flag = this.uselessWorkers4Production.Contains(worker6);
						if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && !flag)
						{
							ResourceType resourceType = AiStrategicAnalysis.ResourceProduced(worker6.position.hexType);
							if (resourceType != ResourceType.combatCard)
							{
								if (worker6.position.GetOwnerWorkers().Count > this.resourceDemandTotal[resourceType])
								{
									flag = true;
								}
								if (resourceType == this.tradeLoopResource && this.resourceDemandTotal[resourceType] - aiPlayer.player.Resources(false)[resourceType] <= 2)
								{
									flag = true;
								}
							}
						}
						if (flag)
						{
							bool flag2 = false;
							foreach (Mech mech9 in worker6.position.GetOwnerMechs())
							{
								if (this.moveMechPassengers.Keys.Contains(mech9) && this.moveMechPassengers[mech9].Contains(worker6))
								{
									flag2 = true;
								}
							}
							if (!flag2)
							{
								GameHex gameHex18 = null;
								switch (aiPlayer.player.matPlayer.matType)
								{
								case PlayerMatType.Patriotic:
									goto IL_1646;
								case PlayerMatType.Mechanical:
								{
									using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator5 = this.moveRangeAll[worker6].Keys.GetEnumerator())
									{
										while (enumerator5.MoveNext())
										{
											GameHex gameHex19 = enumerator5.Current;
											if (gameHex19.Owner == null)
											{
												gameHex18 = gameHex19;
											}
										}
										break;
									}
									goto IL_1646;
								}
								case PlayerMatType.Agricultural:
									goto IL_173D;
								case PlayerMatType.Militant:
									goto IL_16AB;
								default:
									goto IL_173D;
								}
								IL_17CE:
								if (gameHex18 != null)
								{
									this.movePriority.Add(worker6, 1);
									this.moveTarget.Add(worker6, new List<GameHex> { gameHex18 });
									this.moveDistance.Add(worker6, 1);
									continue;
								}
								continue;
								IL_173D:
								foreach (GameHex gameHex20 in this.moveRangeAll[worker6].Keys)
								{
									ResourceType resourceType2 = AiStrategicAnalysis.ResourceProduced(gameHex20.hexType);
									if (gameHex20.Owner == null && resourceType2 != ResourceType.combatCard && (gameHex18 == null || (this.ResourcePriority(aiPlayer, gameHex20) > this.ResourcePriority(aiPlayer, gameHex18) && this.resourceDemandTotal[resourceType2] > this.resourceAccess[resourceType2])))
									{
										gameHex18 = gameHex20;
									}
								}
								goto IL_17CE;
								IL_16AB:
								using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator5 = this.moveRangeAll[worker6].Keys.GetEnumerator())
								{
									while (enumerator5.MoveNext())
									{
										GameHex gameHex21 = enumerator5.Current;
										ResourceType resourceType3 = AiStrategicAnalysis.ResourceProduced(gameHex21.hexType);
										if ((gameHex21.Owner == null || gameHex21.Owner == aiPlayer.player) && resourceType3 != ResourceType.combatCard && (gameHex18 == null || (this.resourceDemandTotal[resourceType3] > 0 && this.resourceAccess[resourceType3] == 0)))
										{
											gameHex18 = gameHex21;
										}
									}
									goto IL_17CE;
								}
								goto IL_173D;
								IL_1646:
								using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator5 = this.moveRangeAll[worker6].Keys.GetEnumerator())
								{
									while (enumerator5.MoveNext())
									{
										GameHex gameHex22 = enumerator5.Current;
										if (gameHex22.Owner == null && (gameHex18 == null || this.ResourcePriority(aiPlayer, gameHex22) > this.ResourcePriority(aiPlayer, gameHex18)))
										{
											gameHex18 = gameHex22;
										}
									}
									goto IL_17CE;
								}
								goto IL_16AB;
							}
						}
					}
				}
			}
			foreach (Unit unit3 in this.moveRangeAll.Keys)
			{
				if (unit3.position.hexType == HexType.capital && !this.movePriority.ContainsKey(unit3))
				{
					GameHex gameHex23 = null;
					foreach (GameHex gameHex24 in this.moveRangeAll[unit3].Keys)
					{
						if (gameHex24.hexType != HexType.capital && (gameHex24.Owner == null || gameHex24.Owner == aiPlayer.player) && (gameHex23 == null || gameHex23.Owner != null))
						{
							gameHex23 = gameHex24;
						}
					}
					if (gameHex23 != null)
					{
						this.movePriority.Add(unit3, 1);
						this.moveTarget.Add(unit3, new List<GameHex> { gameHex23 });
						this.moveDistance.Add(unit3, this.moveRangeAll[unit3][gameHex23]);
					}
				}
			}
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x00044896 File Offset: 0x00042A96
		public override void UpdateWorkerCountTarget(AiPlayer aiPlayer)
		{
			this.workerCountTarget = 5;
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && aiPlayer.TradeResourceType() == ResourceType.combatCard)
			{
				this.workerCountTarget = 8;
			}
		}

		// Token: 0x04001F21 RID: 7969
		public HashSet<GameHex> objectiveArea = new HashSet<GameHex>();

		// Token: 0x04001F22 RID: 7970
		public int objectiveTarget;

		// Token: 0x04001F23 RID: 7971
		public bool moveToObjective;
	}
}

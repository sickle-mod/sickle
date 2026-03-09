using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005B2 RID: 1458
	public class AiStrategicAnalysisAdv : AiStrategicAnalysis
	{
		// Token: 0x06002E1B RID: 11803
		public AiStrategicAnalysisAdv(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002E1C RID: 11804
		public override void Run(AiPlayer aiPlayer, int priMoveToProduction1, int priMoveToProduction2, int priMoveToEncounter, int priMoveToFight, int priMoveToBuild)
		{
			this.priorityFight = priMoveToFight;
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
			this.PlanNextTwoTurns(aiPlayer);
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
			if (aiPlayer.player.matFaction.faction == Faction.Saxony)
			{
				bool hasObj14 = false;
				foreach (ObjectiveCard card in aiPlayer.player.objectiveCards)
				{
					if (card.CardId == 14 && card.status == ObjectiveCard.ObjectiveStatus.Open)
					{
						hasObj14 = true;
						break;
					}
				}
				if (!hasObj14 || aiPlayer.player.matPlayer.buildings.Count >= 1)
				{
					this.canBuild = false;
					this.gottaMoveToBuild = false;
					this.resourceDemandPriority[ResourceType.wood] = 1;
				}
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
			if (aiPlayer.player.matFaction.faction == Faction.Saxony)
			{
				bool hasObj15 = false;
				foreach (ObjectiveCard card2 in aiPlayer.player.objectiveCards)
				{
					if (card2.CardId == 6 && card2.status == ObjectiveCard.ObjectiveStatus.Open)
					{
						hasObj15 = true;
						break;
					}
				}
				if (!hasObj15)
				{
					foreach (Unit unit in this.moveTarget.Keys.ToList<Unit>())
					{
						for (int i = this.moveTarget[unit].Count - 1; i >= 0; i--)
						{
							GameHex hex = this.moveTarget[unit][i];
							if (hex.Owner != null && hex.Owner != aiPlayer.player && !hex.HasOwnerCharacter() && hex.GetOwnerMechs().Count == 0 && hex.GetOwnerWorkers().Count > 0)
							{
								this.moveTarget[unit].RemoveAt(i);
							}
						}
						if (this.moveTarget[unit].Count == 0)
						{
							this.moveTarget.Remove(unit);
							this.movePriority.Remove(unit);
							this.moveDistance.Remove(unit);
						}
					}
				}
			}
			int moveTieBreaker = 0;
			foreach (KeyValuePair<Unit, int> keyValuePair in this.movePriority)
			{
				int key = keyValuePair.Value * 100 + moveTieBreaker++;
				this.movePrioritySorted.Add(key, keyValuePair.Key);
			}
			if (this.movePriority.Values.Count > 0)
			{
				this.movePriorityHighest = this.movePriority.Values.Max();
			}
			else
			{
				this.movePriorityHighest = -1;
			}
			this.UpdateTurnCycle(aiPlayer);
		}

		// Sets up a 3-turn repeating cycle for specific faction/mat combos.
		// Each cycle step maps to a GainType (top action). The bot will follow this pattern
		// at high priority after the kickstart phase ends.
		private void UpdateTurnCycle(AiPlayer aiPlayer)
		{
			this.turnCyclePresent = false;
			Faction faction = aiPlayer.player.matFaction.faction;
			PlayerMatType matType = aiPlayer.player.matPlayer.matType;

			// Engineering mat: Produce/Upgrade → Trade Metal/Deploy → Move/Enlist
			if (matType == PlayerMatType.Engineering && (faction == Faction.Crimea || faction == Faction.Albion))
			{
				// Stop the cycle once both mech and recruit stars are earned
				if (aiPlayer.player.GetNumberOfStars(StarType.Mechs) > 0 && aiPlayer.player.GetNumberOfStars(StarType.Recruits) > 0)
				{
					return;
				}
				// Don't start cycle until after kickstart (turn 11 for Rusviet Engineering)
				this.turnCycleStartTurn = 11;
				if (this.gameManager.TurnCount < this.turnCycleStartTurn)
				{
					return;
				}
				this.turnCyclePresent = true;
				this.turnCycleSteps = new GainType[] { GainType.Produce, GainType.AnyResource, GainType.Move };
				this.turnCycleTradeResource = ResourceType.metal;
			}
		}

		// Token: 0x06002E1D RID: 11805
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
			int starsCount = 0;
			if (aiPlayer.player.GetNumberOfStars(StarType.Upgrades) > 0)
			{
				starsCount++;
			}
			if (aiPlayer.player.GetNumberOfStars(StarType.Mechs) > 0)
			{
				starsCount++;
			}
			if (aiPlayer.player.GetNumberOfStars(StarType.Structures) > 0)
			{
				starsCount++;
			}
			if (aiPlayer.player.GetNumberOfStars(StarType.Recruits) > 0)
			{
				starsCount++;
			}
			if (starsCount >= 2 && aiPlayer.player.GetNumberOfStars(StarType.Workers) > 0 && !flag2)
			{
				flag = true;
			}
			if (flag)
			{
				this.Scatter(aiPlayer, pri);
			}
		}

		// Token: 0x06002E1E RID: 11806
		public new void UpdateRecruitOneTimeOrder(AiPlayer aiPlayer)
		{
			if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType != PlayerMatType.Patriotic)
			{
				this.recruitOneTimePriority[GainType.Power] = 20;
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony)
			{
				this.recruitOneTimePriority[GainType.Power] = 20;
				this.recruitOneTimePriority[GainType.CombatCard] = 20;
				return;
			}
			if ((aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && (aiPlayer.player.matFaction.faction == Faction.Polania || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering && (aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative && (aiPlayer.player.matFaction.faction == Faction.Polania || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)))
			{
				this.recruitOneTimePriority[GainType.Power] = 20;
				this.recruitOneTimePriority[GainType.CombatCard] = 20;
				return;
			}
			if (aiPlayer.player.stars.Values.Sum() >= 5 && aiPlayer.player.GetNumberOfStars(StarType.Combat) < 2)
			{
				this.recruitOneTimePriority[GainType.Power] = 50;
				this.recruitOneTimePriority[GainType.CombatCard] = 50;
				return;
			}
			if (aiPlayer.player.GetNumberOfStars(StarType.Combat) < 2)
			{
				this.recruitOneTimePriority[GainType.Power] = 20;
			}
			this.recruitOneTimePriority[GainType.Popularity] = 9;
			Dictionary<GainType, int> dictionary;
			if (aiPlayer.player.Popularity >= 13)
			{
				dictionary = this.recruitOneTimePriority;
				dictionary[GainType.Popularity] = dictionary[GainType.Popularity] - 7;
				return;
			}
			if (aiPlayer.player.Popularity >= 7)
			{
				dictionary = this.recruitOneTimePriority;
				dictionary[GainType.Popularity] = dictionary[GainType.Popularity] - 4;
				return;
			}
			dictionary = this.recruitOneTimePriority;
			dictionary[GainType.Popularity] = dictionary[GainType.Popularity] - 2;
		}

		// Token: 0x06002E1F RID: 11807
		public override void UpdateMechOrder(AiPlayer aiPlayer)
		{
			int num = -1;
			if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 10;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.mechPriority[0] = 10;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 6;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				this.mechPriority[0] = 10;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 10;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 10;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.mechPriority[0] = 10;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 4;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.mechPriority[0] = 10;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.mechPriority[0] = 10;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				this.mechPriority[0] = 10;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.mechPriority[0] = 10;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.mechPriority[0] = 10;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Rusviet && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.mechPriority[0] = 4;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 4;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 4;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 4;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 4;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 4;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 4;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 4;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.mechPriority[0] = 4;
				this.mechPriority[1] = 10;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.mechPriority[0] = 4;
				this.mechPriority[1] = 10;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.mechPriority[0] = 4;
				this.mechPriority[1] = 10;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				this.mechPriority[0] = 4;
				this.mechPriority[1] = 10;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.mechPriority[0] = 4;
				this.mechPriority[1] = 10;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.mechPriority[0] = 4;
				this.mechPriority[1] = 10;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.mechPriority[0] = 4;
				this.mechPriority[1] = 10;
				this.mechPriority[2] = 6;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.mechPriority[0] = 10;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.mechPriority[0] = 10;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 8;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
			{
				this.mechPriority[0] = 6;
				this.mechPriority[1] = 8;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
			{
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
			{
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
			}
			else if (aiPlayer.player.matFaction.faction == Faction.Togawa && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
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

		// Token: 0x06002E20 RID: 11808
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

		// Token: 0x06002E21 RID: 11809
		protected override void UpdateResourceDemandPriority(AiPlayer aiPlayer, Dictionary<ResourceType, int> resourceDemand)
		{
			this.resourceDemandPriority[ResourceType.oil] = 5;
			this.resourceDemandPriority[ResourceType.food] = 4;
			this.resourceDemandPriority[ResourceType.metal] = 3;
			this.resourceDemandPriority[ResourceType.wood] = 2;
			switch (aiPlayer.player.matFaction.faction)
			{
			case Faction.Polania:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
					if (aiPlayer.player.matPlayer.GetBuilding(BuildingType.Mill) != null && aiPlayer.player.matPlayer.GetBuilding(BuildingType.Mill).position.Owner == aiPlayer.player && aiPlayer.player.matPlayer.GetBuilding(BuildingType.Mill).position.hexType == HexType.tundra && aiPlayer.player.matPlayer.GetBuilding(BuildingType.Mill).position.GetOwnerWorkers().Count >= 2)
					{
						this.resourceDemandPriority[ResourceType.oil] = 1;
					}
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
				{
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
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
					if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
					{
						this.resourceDemandPriority[ResourceType.oil] = 1;
					}
					if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
					{
						this.resourceDemandPriority[ResourceType.metal] = 11;
						this.resourceDemandPriority[ResourceType.food] = 10;
						this.resourceDemandPriority[ResourceType.oil] = 3;
						this.resourceDemandPriority[ResourceType.wood] = 2;
					}
				}
				break;
			case Faction.Albion:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
				{
					this.resourceDemandPriority[ResourceType.oil] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.oil] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
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
						this.resourceDemandPriority[this.tradeLoopResource] = 13; // Fixed Nordic priority bug
					}
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					if (aiPlayer.player.matPlayer.UpgradesDone >= 3)
					{
						this.resourceDemandPriority[ResourceType.food] = 10;
					}
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
				{
					this.resourceDemandPriority[ResourceType.oil] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matFaction.mechs.Count <= 1)
				{
					this.resourceDemandPriority[ResourceType.metal] = 8;
				}
				break;
			case Faction.Rusviet:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
				{
					this.resourceDemandPriority[ResourceType.oil] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.oil] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				break;
			case Faction.Togawa:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
				{
					if (this.resourceAccess[ResourceType.metal] >= 2)
					{
						this.resourceDemandPriority[ResourceType.metal] = 1;
					}
					this.resourceDemandPriority[ResourceType.oil] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.oil] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative && this.resourceAccess[ResourceType.metal] >= 2)
				{
					this.resourceDemandPriority[ResourceType.metal] = 1;
				}
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && this.resourceAccess[ResourceType.food] >= 0 && this.resourceAccess[ResourceType.wood] == 0)
				{
					this.resourceDemandPriority[ResourceType.wood] = 10;
				}
				break;
			case Faction.Crimea:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
				{
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				break;
			case Faction.Saxony:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial || aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.oil] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
				{
					if (aiPlayer.player.matPlayer.UpgradesDone < 2) {
						this.resourceDemandPriority[ResourceType.oil] = 11;
						this.resourceDemandPriority[ResourceType.metal] = 10;
						this.resourceDemandPriority[ResourceType.food] = 3;
					} else if (aiPlayer.player.matFaction.mechs.Count < 4) {
						this.resourceDemandPriority[ResourceType.metal] = 11;
						this.resourceDemandPriority[ResourceType.oil] = 10;
						this.resourceDemandPriority[ResourceType.food] = 3;
					} else {
						this.resourceDemandPriority[ResourceType.oil] = 11;
						this.resourceDemandPriority[ResourceType.metal] = 10;
						this.resourceDemandPriority[ResourceType.food] = 3;
					}
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else
				{
					if (aiPlayer.player.matPlayer.UpgradesDone < 2) {
						this.resourceDemandPriority[ResourceType.oil] = 11;
						this.resourceDemandPriority[ResourceType.metal] = 10;
						this.resourceDemandPriority[ResourceType.food] = 9;
					} else {
						this.resourceDemandPriority[ResourceType.metal] = 11;
						this.resourceDemandPriority[ResourceType.food] = 10;
						this.resourceDemandPriority[ResourceType.oil] = 3;
					}
					this.resourceDemandPriority[ResourceType.wood] = 2;
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
			if (aiPlayer.player.matPlayer.RecruitsEnlisted == 3 && aiPlayer.player.GetNumberOfStars(StarType.Recruits) == 0)
			{
				this.resourceDemandPriority[ResourceType.food] = 13;
			}
			if (aiPlayer.player.matPlayer.workers.Count == 7 && aiPlayer.player.GetNumberOfStars(StarType.Workers) == 0 && this.pursuingWorkerStar)
			{
				this.resourceDemandPriority[ResourceType.food] = Math.Max(this.resourceDemandPriority[ResourceType.food], 3);
			}
			this.resourceHighestPriority = ResourceType.oil;
			this.resourceHighestPriorityNoProduce = ResourceType.oil;

			bool ignoreBuilding = false;
			if (aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Albion) {
				ignoreBuilding = true;
			}
			if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial || aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative || aiPlayer.player.matPlayer.matType == PlayerMatType.Militant || aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) {
				ignoreBuilding = true;
			}
			if (ignoreBuilding) {
				bool objNeedsBuilding = false;
				foreach (var obj in aiPlayer.player.objectiveCards) {
					if (obj.Id == 20 || obj.Id == 27) objNeedsBuilding = true;
				}
				if (!objNeedsBuilding) {
					this.resourceDemandPriority[ResourceType.wood] = 0;
				}
			}

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

		// Token: 0x06002E22 RID: 11810
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

		// Token: 0x06002E23 RID: 11811
		private void UpdateObjectiveArea(AiPlayer aiPlayer)
		{
			int num = 0;
			int highestPriority = -1;
			foreach (ObjectiveCard objectiveCard in aiPlayer.player.objectiveCards)
			{
				if (objectiveCard.status == ObjectiveCard.ObjectiveStatus.Open)
				{
					int priority = this.GetObjectivePriority(objectiveCard.CardId, aiPlayer);
					if (num == 0 || priority > highestPriority)
					{
						num = objectiveCard.CardId;
						highestPriority = priority;
					}
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
			case 19:
				hashSet = new HashSet<GameHex>(this.gameManager.gameBoard.lakes);
				this.objectiveTarget = 1;
				break;
			case 24:
				hashSet = new HashSet<GameHex>(this.gameManager.gameBoard.factory.GetNeighboursAll());
				this.objectiveTarget = 5;
				break;
			default:
				if (num == 17 && aiPlayer.player.matPlayer.matPlayerSectionsCount <= 4)
				{
					hashSet = new HashSet<GameHex>(this.gameManager.gameBoard.factory.GetNeighboursAll());
					this.objectiveTarget = 2;
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

		// Token: 0x06002E24 RID: 11812
		protected void Scatter(AiPlayer aiPlayer, int pri)
		{
			this.movePriority.Clear();
			this.moveTarget.Clear();
			this.movePrioritySorted.Clear();
			this.moveDistance.Clear();
			this.enemyCanBeAttackedBy.Clear();
			this.moveRange.Clear();
			this.moveMechPassengers.Clear();
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
				if (unit2.UnitType == UnitType.Mech)
				{
					Mech mech = (Mech)unit2;
					List<Worker> workers = mech.position.GetOwnerWorkers();
					if (workers.Count > 0)
					{
						if (!this.moveMechPassengers.ContainsKey(mech))
						{
							this.moveMechPassengers.Add(mech, new List<Worker>());
						}
						this.moveMechPassengers[mech].Add(workers[0]);
						if (!this.movePriority.ContainsKey(workers[0]))
						{
							this.movePriority.Add(workers[0], -1);
						}
					}
				}
			}
			foreach (Mech mech2 in aiPlayer.player.matFaction.mechs)
			{
				if (!this.movePriority.ContainsKey(mech2) && mech2.position.GetOwnerUnits().Count > 1)
				{
					foreach (GameHex gameHex in this.moveRangeAll[mech2].Keys)
					{
						if (!this.movePriority.ContainsKey(mech2) && gameHex.Owner == null && !hashSet.Contains(gameHex))
						{
							this.movePriority.Add(mech2, pri + 8);
							this.moveTarget.Add(mech2, new List<GameHex> { gameHex });
							this.moveDistance.Add(mech2, this.moveRangeAll[mech2][gameHex]);
							hashSet.Add(gameHex);
							using (List<Worker>.Enumerator enumerator4 = mech2.position.GetOwnerWorkers().GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									Worker worker = enumerator4.Current;
									if (!this.movePriority.ContainsKey(worker))
									{
										if (!this.moveMechPassengers.ContainsKey(mech2))
										{
											this.moveMechPassengers.Add(mech2, new List<Worker>());
										}
										this.moveMechPassengers[mech2].Add(worker);
										this.movePriority.Add(worker, -1);
										break;
									}
								}
								break;
							}
						}
					}
				}
			}
			foreach (Unit unit4 in this.moveRangeAll.Keys)
			{
				if (!this.movePriority.ContainsKey(unit4))
				{
					foreach (GameHex gameHex2 in this.moveRangeAll[unit4].Keys)
					{
						if (!this.movePriority.ContainsKey(unit4) && gameHex2.Owner != null && gameHex2.Owner != aiPlayer.player && gameHex2.Owner.GetAllUnits().Count == 0 && !hashSet.Contains(gameHex2))
						{
							this.movePriority.Add(unit4, pri + 6);
							this.moveTarget.Add(unit4, new List<GameHex> { gameHex2 });
							this.moveDistance.Add(unit4, this.moveRangeAll[unit4][gameHex2]);
							hashSet.Add(gameHex2);
						}
					}
				}
			}
			foreach (Unit unit5 in this.moveRangeAll.Keys)
			{
				if (!this.movePriority.ContainsKey(unit5) && unit5.position.GetOwnerUnits().Count > 1)
				{
					foreach (GameHex gameHex3 in this.moveRangeAll[unit5].Keys)
					{
						if (!this.movePriority.ContainsKey(unit5) && gameHex3.Owner == null && !hashSet.Contains(gameHex3))
						{
							this.movePriority.Add(unit5, pri + 4);
							this.moveTarget.Add(unit5, new List<GameHex> { gameHex3 });
							this.moveDistance.Add(unit5, this.moveRangeAll[unit5][gameHex3]);
							hashSet.Add(gameHex3);
						}
					}
				}
			}
		}

		// Token: 0x06002E25 RID: 11813
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
				if (aiPlayer.player.matFaction.faction != Faction.Polania || aiPlayer.player.matPlayer.matType != PlayerMatType.Engineering)
				{
					using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator9 = this.moveRangeAll[mech].Keys.GetEnumerator())
					{
						while (enumerator9.MoveNext())
						{
							GameHex gameHex2 = enumerator9.Current;
							if (this.ResourcePriority(aiPlayer, gameHex2) > 0f && (gameHex2.Owner == null || gameHex2.Owner == aiPlayer.player) && (gameHex == null || this.ResourcePriority(aiPlayer, gameHex2) > this.ResourcePriority(aiPlayer, gameHex)))
							{
								gameHex = gameHex2;
							}
						}
						goto IL_019D;
					}
				}
				foreach (GameHex gameHex3 in this.moveRangeAll[mech].Keys)
				{
					if (this.ResourcePriority(aiPlayer, gameHex3) > 0f && (gameHex3.Owner == null || gameHex3.Owner == aiPlayer.player) && (gameHex == null || (this.ResourcePriority(aiPlayer, gameHex3) > this.ResourcePriority(aiPlayer, gameHex) && this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex3.hexType)] <= this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex.hexType)])))
					{
						gameHex = gameHex3;
					}
				}
				IL_019D:
				if (gameHex != null)
				{
					bool flag = false;
					GameHex gameHex4 = null;
					GameHex gameHex5 = null;
					if (aiPlayer.player.matFaction.SkillUnlocked[3] && ((aiPlayer.player.matFaction.faction == Faction.Saxony && (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical || aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial || aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)))
					{
						foreach (GameHex gameHex6 in this.moveRangeAll[mech].Keys)
						{
							if (this.ResourcePriority(aiPlayer, gameHex6) > 0f && (gameHex6.Owner == null || gameHex6.Owner == aiPlayer.player) && this.moveRangeAll[mech][gameHex6] == 1 && AiStrategicAnalysis.ResourceProduced(gameHex6.hexType) != ResourceType.combatCard && (gameHex4 == null || this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex6.hexType)] <= this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex4.hexType)]) && (gameHex4 == null || this.ResourcePriority(aiPlayer, gameHex6) > this.ResourcePriority(aiPlayer, gameHex4)))
							{
								gameHex4 = gameHex6;
							}
						}
						if (gameHex4 != null)
						{
							ResourceType resourceType = AiStrategicAnalysis.ResourceProduced(gameHex4.hexType);
							int num2 = this.resourceCostSingleAction[resourceType];
							if (this.moveMechPassengers[mech].Count > num2)
							{
								Dictionary<GameHex, int> dictionary;
								this.gameManager.gameBoard.MoveRange(mech, gameHex4, 1, out dictionary);
								foreach (GameHex gameHex7 in dictionary.Keys)
								{
									if (this.ResourcePriority(aiPlayer, gameHex7) > 0f && (gameHex7.Owner == null || gameHex7.Owner == aiPlayer.player) && gameHex7.hexType != gameHex4.hexType && AiStrategicAnalysis.ResourceProduced(gameHex7.hexType) != ResourceType.combatCard && this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex7.hexType)] == 0 && (gameHex5 == null || this.ResourcePriority(aiPlayer, gameHex7) > this.ResourcePriority(aiPlayer, gameHex5)))
									{
										gameHex5 = gameHex7;
									}
								}
								if (gameHex5 != null)
								{
									flag = true;
								}
							}
						}
					}
					if (flag)
					{
						this.movePriority.Add(mech, (this.moveMechPassengers[mech].Count < 2) ? priProduce1 : (priProduce2 + 2));
						this.moveTarget.Add(mech, new List<GameHex> { gameHex4, gameHex5 });
						this.moveDistance.Add(mech, 2);
					}
					else
					{
						this.movePriority.Add(mech, (this.moveMechPassengers[mech].Count < 2) ? priProduce1 : (priProduce2 + 2));
						this.moveTarget.Add(mech, new List<GameHex> { gameHex });
						this.moveDistance.Add(mech, this.moveRangeAll[mech][gameHex]);
					}
					if (mech.position.GetOwnerMechs().Count > 1 && aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
					{
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
						}
					}
				}
			}
		}

		// Token: 0x06002E26 RID: 11814
		protected void UpdateMoveTargetsObjective(AiPlayer aiPlayer, int priProduce1, int priProduce2, int priEncounter, int priFight, int priBuildSpot)
		{
			if (this.objectiveArea.Count > 0 && (aiPlayer.player.matFaction.faction == Faction.Saxony || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)))
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
									int baseObjectivePri = priFight + 100;
									int highestObjectiveProgress = 0;
									foreach (ObjectiveCard objectiveCard in aiPlayer.player.objectiveCards)
									{
										if (objectiveCard.status == ObjectiveCard.ObjectiveStatus.Open)
										{
											int progress = this.CalculateObjectiveProgress(objectiveCard);
											if (progress > highestObjectiveProgress)
											{
												highestObjectiveProgress = progress;
											}
										}
									}
									if (highestObjectiveProgress >= 80)
									{
										baseObjectivePri = 5000;
									}
									else if (highestObjectiveProgress >= 50)
									{
										baseObjectivePri = 3000;
									}
									else
									{
										baseObjectivePri = 2000;
									}
									foreach (ObjectiveCard objCard in aiPlayer.player.objectiveCards)
									{
										if (objCard.CardId == 5 && objCard.status == ObjectiveCard.ObjectiveStatus.Open)
										{
											baseObjectivePri = 6000;
											break;
										}
									}
									dictionary.Add(unit2, baseObjectivePri);
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

		// Token: 0x06002E27 RID: 11815
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

		// Token: 0x06002E28 RID: 11816
		private int ObjectiveSendOneBackPriority(Unit unit, GameHex hex)
		{
			if (unit.UnitType == UnitType.Mech)
			{
				return 10;
			}
			return 1;
		}

		// Token: 0x06002E29 RID: 11817
		protected override float isWorthAttacking(GameHex hex, AiPlayer aiPlayer)
		{
			if (aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Crimea)
			{
				return (float)(2 - hex.GetOwnerWorkers().Count);
			}
			if (aiPlayer.player.matFaction.faction != Faction.Saxony && (aiPlayer.player.matFaction.faction != Faction.Saxony || aiPlayer.player.matPlayer.matType != PlayerMatType.Industrial) && (aiPlayer.player.matFaction.faction != Faction.Saxony || aiPlayer.player.matPlayer.matType != PlayerMatType.Mechanical) && (aiPlayer.player.matFaction.faction != Faction.Saxony || aiPlayer.player.matPlayer.matType != PlayerMatType.Engineering) && (aiPlayer.player.matFaction.faction != Faction.Polania || aiPlayer.player.matPlayer.matType != PlayerMatType.Industrial) && (aiPlayer.player.matFaction.faction != Faction.Polania || aiPlayer.player.matPlayer.matType != PlayerMatType.Innovative) && (aiPlayer.player.matFaction.faction != Faction.Crimea || aiPlayer.player.matPlayer.matType != PlayerMatType.Industrial) && (aiPlayer.player.matFaction.faction != Faction.Crimea || aiPlayer.player.matPlayer.matType != PlayerMatType.Engineering) && (aiPlayer.player.matFaction.faction != Faction.Crimea || aiPlayer.player.matPlayer.matType != PlayerMatType.Innovative) && (aiPlayer.player.matPlayer.matType != PlayerMatType.Innovative || (aiPlayer.player.matFaction.faction != Faction.Albion && aiPlayer.player.matFaction.faction != Faction.Nordic && aiPlayer.player.matFaction.faction != Faction.Rusviet && aiPlayer.player.matFaction.faction != Faction.Togawa)) && (aiPlayer.player.matPlayer.matType != PlayerMatType.Industrial || (aiPlayer.player.matFaction.faction != Faction.Albion && aiPlayer.player.matFaction.faction != Faction.Nordic && aiPlayer.player.matFaction.faction != Faction.Rusviet && aiPlayer.player.matFaction.faction != Faction.Togawa)) && (aiPlayer.player.matPlayer.matType != PlayerMatType.Engineering || (aiPlayer.player.matFaction.faction != Faction.Albion && aiPlayer.player.matFaction.faction != Faction.Rusviet && aiPlayer.player.matFaction.faction != Faction.Togawa)) && (aiPlayer.player.matPlayer.matType != PlayerMatType.Patriotic || (aiPlayer.player.matFaction.faction != Faction.Albion && aiPlayer.player.matFaction.faction != Faction.Rusviet && aiPlayer.player.matFaction.faction != Faction.Togawa && aiPlayer.player.matFaction.faction != Faction.Crimea && aiPlayer.player.matFaction.faction != Faction.Nordic && aiPlayer.player.matFaction.faction != Faction.Polania)) && (aiPlayer.player.matPlayer.matType != PlayerMatType.Mechanical || (aiPlayer.player.matFaction.faction != Faction.Albion && aiPlayer.player.matFaction.faction != Faction.Rusviet && aiPlayer.player.matFaction.faction != Faction.Togawa && aiPlayer.player.matFaction.faction != Faction.Crimea && aiPlayer.player.matFaction.faction != Faction.Nordic && aiPlayer.player.matFaction.faction != Faction.Polania)) && (aiPlayer.player.matPlayer.matType != PlayerMatType.Agricultural || (aiPlayer.player.matFaction.faction != Faction.Albion && aiPlayer.player.matFaction.faction != Faction.Rusviet && aiPlayer.player.matFaction.faction != Faction.Togawa && aiPlayer.player.matFaction.faction != Faction.Crimea)) && (aiPlayer.player.matPlayer.matType != PlayerMatType.Militant || (aiPlayer.player.matFaction.faction != Faction.Albion && aiPlayer.player.matFaction.faction != Faction.Rusviet && aiPlayer.player.matFaction.faction != Faction.Togawa && aiPlayer.player.matFaction.faction != Faction.Crimea && aiPlayer.player.matFaction.faction != Faction.Nordic && aiPlayer.player.matFaction.faction != Faction.Polania)) && (aiPlayer.player.matFaction.faction != Faction.Albion || aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial))
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
			foreach (ObjectiveCard objCard in aiPlayer.player.objectiveCards)
			{
				if (objCard.CardId == 6 && objCard.status == ObjectiveCard.ObjectiveStatus.Open)
				{
					if (num6 < num3 || hex.GetOwnerWorkers().Count == 0 || aiPlayer.player.Power < 14)
					{
						return 0f;
					}
					break;
				}
			}
			float num7 = (float)(hex.GetResourceCount() / 2);
			if (aiPlayer.player.Popularity < 7 && aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				return num7;
			}
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
				float num8 = (float)(2 - hex.GetOwnerWorkers().Count);
				if ((aiPlayer.player.Power >= 12 || aiPlayer.player.GetNumberOfStars(StarType.Power) > 0) && aiPlayer.player.GetNumberOfStars(StarType.Combat) < 2)
				{
					num8 += (float)hex.GetOwnerWorkers().Count;
					num8 += 2f;
				}
				num7 += num8;
			}
			if (num7 > 0f && this.enemyCanBeAttackedBy[hex].Count<Unit>() > 1)
			{
				num7 += 10f;
			}
			return num7;
		}

		// Token: 0x06002E2A RID: 11818
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
			if (!this.movePriority.ContainsKey(aiPlayer.player.character) && (aiPlayer.player.matFaction.faction == Faction.Saxony || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)))
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
			if (priFight > 0 && aiPlayer.strategicAnalysis.enemyCanBeAttackedBy.Count > 0 && (aiPlayer.player.Power >= 7 || aiPlayer.player.GetNumberOfStars(StarType.Power) > 0 || aiPlayer.player.matFaction.faction == Faction.Saxony || (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && (aiPlayer.player.matFaction.faction == Faction.Polania || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering && (aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative && (aiPlayer.player.matFaction.faction == Faction.Polania || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa))) && (aiPlayer.player.GetNumberOfStars(StarType.Combat) < ((aiPlayer.player.matFaction.faction == Faction.Saxony) ? 6 : 2) || aiPlayer.player.matFaction.factionPerk == AbilityPerk.Dominate))
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
						goto IL_0BD9;
					}
				}
				foreach (GameHex gameHex5 in this.enemyCanBeAttackedBy.Keys)
				{
					if (this.isWorthAttacking(gameHex5, aiPlayer) > 0f)
					{
						gameHex3 = gameHex5;
					}
				}
				IL_0BD9:
				if (gameHex3 != null)
				{
					Unit unit = null;
					if ((aiPlayer.player.matFaction.faction == Faction.Saxony || aiPlayer.player.matFaction.faction == Faction.Polania || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa) && (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial || aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering || aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic || aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical || aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural || aiPlayer.player.matPlayer.matType == PlayerMatType.Militant || (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative && (aiPlayer.player.matFaction.faction == Faction.Polania || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa))))
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
						int appliedPriFight = priFight;
						if (gameHex3 != null && gameHex3.Owner != null && gameHex3.Owner.GetNumberOfStars() >= 5)
						{
							appliedPriFight += 1000;
						}
						if (aiPlayer.player.Popularity > 12)
						{
							appliedPriFight += 500;
						}
						this.movePriority.Add(unit, appliedPriFight);
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
					if (aiPlayer.player.matFaction.faction == Faction.Saxony || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Nordic)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Nordic)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Nordic)) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial))
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

		// Token: 0x06002E2B RID: 11819
		private void UpdateMoveTargetsSecondary(AiPlayer aiPlayer, int priProduce1, int priProduce2, int priEncounter, int priFight, int priBuildSpot)
		{
			if (aiPlayer.player.matPlayer.workers.Count < this.workerCountTarget)
			{
				this.workersInaVillage = 0;
				foreach (Worker worker10 in aiPlayer.player.matPlayer.workers)
				{
					if (worker10.position.hexType == HexType.village)
					{
						this.workersInaVillage++;
					}
					if (worker10.position.hexType != HexType.capital)
					{
						this.workersOutOfBase++;
					}
				}
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
							foreach (GameHex gameHex2 in this.moveRangeAll[worker4].Keys)
							{
								if (gameHex2.hexType == HexType.village && (gameHex == null || this.moveRangeAll[worker4][gameHex2] < this.moveRangeAll[worker4][gameHex]))
								{
									gameHex = gameHex2;
								}
							}
							if (gameHex != null)
							{
								this.movePriority.Add(worker4, 9790);
								this.moveTarget.Add(worker4, new List<GameHex> { gameHex });
								this.moveDistance.Add(worker4, this.moveRangeAll[worker4][gameHex]);
								num++;
							}
						}
					}
				}
				if (this.workersInaVillage == 0)
				{
					Worker worker5 = null;
					GameHex gameHex3 = null;
					foreach (Worker worker6 in aiPlayer.player.matPlayer.workers)
					{
						if (worker6.position != null && (worker5 == null || this.ResourcePriority(aiPlayer, worker6.position) < this.ResourcePriority(aiPlayer, worker5.position)))
						{
							foreach (GameHex gameHex4 in worker6.position.GetFieldsAccessible(worker6, false))
							{
								if (gameHex4.hexType == HexType.village)
								{
									worker5 = worker6;
									gameHex3 = gameHex4;
								}
							}
						}
					}
					if (worker5 != null)
					{
						int movePri = (this.pursuingWorkerStar ? 9790 : (priProduce2 + 1));
						if (!this.movePriority.ContainsKey(worker5))
						{
							this.movePriority.Add(worker5, movePri);
							this.moveTarget.Add(worker5, new List<GameHex> { gameHex3 });
							this.moveDistance.Add(worker5, 1);
						}
						else
						{
							this.movePriority[worker5] = movePri;
							this.moveTarget[worker5] = new List<GameHex> { gameHex3 };
							this.moveDistance[worker5] = 1;
						}
					}
				}
			}
			if (aiPlayer.player.matPlayer.matPlayerSectionsCount <= 4 && !this.movePriority.ContainsKey(aiPlayer.player.character))
			{
				GameHex factory = this.gameManager.gameBoard.factory;
				if (this.characterDistance.ContainsKey(factory))
				{
					int dist = this.characterDistance[factory];
					int p = ((dist <= (int)aiPlayer.player.character.MaxMoveCount) ? (priEncounter + 35) : (priEncounter + 10));
					if (aiPlayer.player.matFaction.faction == Faction.Albion)
					{
						using (List<GameHex>.Enumerator enumerator13 = new List<GameHex>
						{
							this.gameManager.gameBoard.hexMap[1, 2],
							this.gameManager.gameBoard.hexMap[2, 3],
							this.gameManager.gameBoard.hexMap[3, 3],
							factory
						}.GetEnumerator())
						{
							while (enumerator13.MoveNext())
							{
								GameHex target = enumerator13.Current;
								if (target.Token == null && (target.Owner == null || target.Owner == aiPlayer.player))
								{
									this.movePriority.Add(aiPlayer.player.character, p + 5);
									this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { target });
									this.moveDistance.Add(aiPlayer.player.character, this.characterDistance.ContainsKey(target) ? this.characterDistance[target] : dist);
									goto IL_0680;
								}
							}
							goto IL_062D;
						}
					}
					if (aiPlayer.player.matFaction.faction == Faction.Togawa)
					{
						foreach (GameHex target2 in new List<GameHex>
						{
							this.gameManager.gameBoard.hexMap[5, 6],
							factory
						})
						{
							if (target2.Token == null && (target2.Owner == null || target2.Owner == aiPlayer.player))
							{
								this.movePriority.Add(aiPlayer.player.character, p + 5);
								this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { target2 });
								this.moveDistance.Add(aiPlayer.player.character, this.characterDistance.ContainsKey(target2) ? this.characterDistance[target2] : dist);
								goto IL_0680;
							}
						}
					}
					IL_062D:
					this.movePriority.Add(aiPlayer.player.character, p);
					this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { factory });
					this.moveDistance.Add(aiPlayer.player.character, dist);
				}
			}
			IL_0680:
			if (!this.movePriority.ContainsKey(aiPlayer.player.character) && this.encounterNearestHex != null && this.characterDistance.ContainsKey(this.encounterNearestHex) && this.characterDistance[this.encounterNearestHex] <= (int)aiPlayer.player.character.MaxMoveCount)
			{
				this.movePriority.Add(aiPlayer.player.character, priEncounter);
				this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { this.encounterNearestHex });
				this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[this.encounterNearestHex]);
			}
			foreach (KeyValuePair<Unit, int> keyValuePair in this.movePriority.ToList<KeyValuePair<Unit, int>>())
			{
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
						this.movePriority.Add(mech, priEncounter + 31);
						this.moveTarget.Add(mech, new List<GameHex> { this.gameManager.gameBoard.factory });
						this.moveDistance.Add(mech, this.moveRange[mech][this.gameManager.gameBoard.factory]);
					}
					foreach (Unit unit in this.moveRangeAll.Keys)
					{
						if (unit.position.hexType == HexType.capital && !this.movePriority.ContainsKey(unit))
						{
							GameHex gameHex5 = null;
							foreach (GameHex gameHex6 in this.moveRangeAll[unit].Keys)
							{
								if (gameHex6.hexType != HexType.capital && (gameHex6.Owner == null || gameHex6.Owner == aiPlayer.player) && (gameHex5 == null || gameHex5.Owner != null))
								{
									gameHex5 = gameHex6;
								}
							}
							if (gameHex5 != null)
							{
								this.movePriority.Add(unit, 1);
								this.moveTarget.Add(unit, new List<GameHex> { gameHex5 });
								this.moveDistance.Add(unit, this.moveRangeAll[unit][gameHex5]);
							}
						}
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Nordic && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				Mech mech3 = null;
				if (this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0 || (this.gameManager.gameBoard.factory.Owner == aiPlayer.player && this.gameManager.gameBoard.factory.GetOwnerUnitCount() - this.gameManager.gameBoard.factory.GetOwnerWorkers().Count <= 1))
				{
					foreach (Mech mech4 in aiPlayer.player.matFaction.mechs)
					{
						if (!this.movePriority.ContainsKey(mech4) && mech4.position.hexType != HexType.factory && this.moveRange[mech4].ContainsKey(this.gameManager.gameBoard.factory) && mech4.position.GetOwnerWorkers().Count == 0)
						{
							mech3 = mech4;
						}
					}
				}
				if (mech3 != null)
				{
					this.movePriority.Add(mech3, priEncounter + 31);
					this.moveTarget.Add(mech3, new List<GameHex> { this.gameManager.gameBoard.factory });
					this.moveDistance.Add(mech3, this.moveRange[mech3][this.gameManager.gameBoard.factory]);
				}
				foreach (Unit unit2 in this.moveRangeAll.Keys)
				{
					if (unit2.position.hexType == HexType.capital && !this.movePriority.ContainsKey(unit2))
					{
						GameHex gameHex7 = null;
						foreach (GameHex gameHex8 in this.moveRangeAll[unit2].Keys)
						{
							if (gameHex8.hexType != HexType.capital && (gameHex8.Owner == null || gameHex8.Owner == aiPlayer.player) && (gameHex7 == null || gameHex7.Owner != null))
							{
								gameHex7 = gameHex8;
							}
						}
						if (gameHex7 != null)
						{
							this.movePriority.Add(unit2, 1);
							this.moveTarget.Add(unit2, new List<GameHex> { gameHex7 });
							this.moveDistance.Add(unit2, this.moveRangeAll[unit2][gameHex7]);
						}
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Saxony)
			{
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					Mech mech5 = null;
					if (this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0)
					{
						foreach (Mech mech6 in aiPlayer.player.matFaction.mechs)
						{
							if (!this.movePriority.ContainsKey(mech6) && this.moveRange[mech6].ContainsKey(this.gameManager.gameBoard.factory))
							{
								mech5 = mech6;
							}
						}
					}
					if (mech5 != null)
					{
						this.movePriority.Add(mech5, priEncounter + 1);
						this.moveTarget.Add(mech5, new List<GameHex> { this.gameManager.gameBoard.factory });
						this.moveDistance.Add(mech5, this.moveRange[mech5][this.gameManager.gameBoard.factory]);
					}
					mech5 = null;
					if (this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0 || (this.gameManager.gameBoard.factory.Owner == aiPlayer.player && this.gameManager.gameBoard.factory.GetOwnerUnitCount() - this.gameManager.gameBoard.factory.GetOwnerWorkers().Count <= 1))
					{
						foreach (Mech mech7 in aiPlayer.player.matFaction.mechs)
						{
							if (!this.movePriority.ContainsKey(mech7) && mech7.position.hexType != HexType.factory && this.moveRange[mech7].ContainsKey(this.gameManager.gameBoard.factory) && mech7.position.GetOwnerWorkers().Count == 0)
							{
								mech5 = mech7;
							}
						}
					}
					if (mech5 != null)
					{
						this.movePriority.Add(mech5, priEncounter + 1);
						this.moveTarget.Add(mech5, new List<GameHex> { this.gameManager.gameBoard.factory });
						this.moveDistance.Add(mech5, this.moveRange[mech5][this.gameManager.gameBoard.factory]);
					}
					foreach (Unit unit3 in this.moveRangeAll.Keys)
					{
						if (unit3.position.hexType == HexType.capital && !this.movePriority.ContainsKey(unit3))
						{
							GameHex gameHex9 = null;
							foreach (GameHex gameHex10 in this.moveRangeAll[unit3].Keys)
							{
								if (gameHex10.hexType != HexType.capital && (gameHex10.Owner == null || gameHex10.Owner == aiPlayer.player) && (gameHex9 == null || gameHex9.Owner != null))
								{
									gameHex9 = gameHex10;
								}
							}
							if (gameHex9 != null)
							{
								this.movePriority.Add(unit3, 1);
								this.moveTarget.Add(unit3, new List<GameHex> { gameHex9 });
								this.moveDistance.Add(unit3, this.moveRangeAll[unit3][gameHex9]);
							}
						}
					}
				}
				if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && aiPlayer.player.character.position.hexType != HexType.mountain && !aiPlayer.player.character.position.hasTunnel)
				{
					if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
					{
						using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator6 = this.moveRangeAll[aiPlayer.player.character].Keys.GetEnumerator())
						{
							while (enumerator6.MoveNext())
							{
								GameHex gameHex11 = enumerator6.Current;
								if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && (gameHex11.Owner == null || gameHex11.Owner == aiPlayer.player) && (gameHex11.hexType == HexType.mountain || gameHex11.hasTunnel))
								{
									this.movePriority.Add(aiPlayer.player.character, 1);
									this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex11 });
									this.moveDistance.Add(aiPlayer.player.character, this.moveRangeAll[aiPlayer.player.character][gameHex11]);
								}
							}
							goto IL_1346;
						}
					}
					GameHex gameHex12 = null;
					foreach (GameHex gameHex13 in this.moveRangeAll[aiPlayer.player.character].Keys)
					{
						if ((gameHex13.Owner == null || gameHex13.Owner == aiPlayer.player) && (gameHex13.hexType == HexType.mountain || gameHex13.hasTunnel))
						{
							int num3 = 0;
							int num4 = 0;
							if (gameHex12 != null)
							{
								if (gameHex12.hasTunnel)
								{
									num3 += 2;
								}
								if (gameHex12.hexType == HexType.mountain)
								{
									num3++;
								}
							}
							if (gameHex13.hasTunnel)
							{
								num4 += 2;
							}
							if (gameHex13.hexType == HexType.mountain)
							{
								num4++;
							}
							if (num4 > num3)
							{
								gameHex12 = gameHex13;
							}
						}
					}
					if (gameHex12 != null)
					{
						this.movePriority.Add(aiPlayer.player.character, 1);
						this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex12 });
						this.moveDistance.Add(aiPlayer.player.character, this.moveRangeAll[aiPlayer.player.character][gameHex12]);
					}
				}
			}
			IL_1346:
			if (aiPlayer.player.matFaction.faction == Faction.Polania)
			{
				if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && aiPlayer.player.character.position.hexType != HexType.lake)
				{
					foreach (GameHex gameHex14 in this.moveRangeAll[aiPlayer.player.character].Keys)
					{
						if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && (gameHex14.Owner == null || gameHex14.Owner == aiPlayer.player) && gameHex14.hexType == HexType.lake)
						{
							this.movePriority.Add(aiPlayer.player.character, 1);
							this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex14 });
							this.moveDistance.Add(aiPlayer.player.character, this.moveRangeAll[aiPlayer.player.character][gameHex14]);
						}
					}
				}
				if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && aiPlayer.player.character.position.hexType != HexType.lake && !aiPlayer.player.character.position.hasTunnel)
				{
					foreach (GameHex gameHex15 in this.moveRangeAll[aiPlayer.player.character].Keys)
					{
						if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && (gameHex15.Owner == null || gameHex15.Owner == aiPlayer.player) && (gameHex15.hexType == HexType.lake || gameHex15.hasTunnel))
						{
							this.movePriority.Add(aiPlayer.player.character, 1);
							this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex15 });
							this.moveDistance.Add(aiPlayer.player.character, this.moveRangeAll[aiPlayer.player.character][gameHex15]);
						}
					}
				}
				if (aiPlayer.player.matFaction.abilities.Contains(AbilityPerk.Camaraderie) && aiPlayer.player.matFaction.SkillUnlocked[2])
				{
					foreach (Mech mech8 in aiPlayer.player.matFaction.mechs)
					{
						if (!this.moveTarget.ContainsKey(mech8))
						{
							GameHex gameHex16 = null;
							int num5 = 0;
							foreach (GameHex gameHex17 in this.moveRangeAll[aiPlayer.player.character].Keys)
							{
								if (gameHex17.Owner != aiPlayer.player && !gameHex17.HasOwnerCharacter() && gameHex17.GetOwnerMechs().Count == 0)
								{
									int num6 = gameHex17.GetEnemyWorkers().Count + gameHex17.GetResourceCount() * 8;
									if (num6 > num5)
									{
										gameHex16 = gameHex17;
										num5 = num6;
									}
								}
							}
							if (gameHex16 != null)
							{
								this.movePriority.Add(mech8, num5);
								this.moveTarget.Add(mech8, new List<GameHex> { gameHex16 });
								this.moveDistance.Add(mech8, this.moveRangeAll[aiPlayer.player.character][gameHex16]);
							}
						}
					}
				}
				Mech mech9 = null;
				if (this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0)
				{
					foreach (Mech mech10 in aiPlayer.player.matFaction.mechs)
					{
						if (!this.movePriority.ContainsKey(mech10) && this.moveRange[mech10].ContainsKey(this.gameManager.gameBoard.factory))
						{
							mech9 = mech10;
						}
					}
				}
				if (mech9 != null && this.moveRange[mech9].ContainsKey(this.gameManager.gameBoard.factory))
				{
					this.movePriority.Add(mech9, priEncounter + 1);
					this.moveTarget.Add(mech9, new List<GameHex> { this.gameManager.gameBoard.factory });
					this.moveDistance.Add(mech9, this.moveRange[mech9][this.gameManager.gameBoard.factory]);
				}
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural && this.resourceCostSingleAction[ResourceType.food] <= 2 && this.resourceAccess[ResourceType.food] >= 2)
				{
					Worker worker7 = null;
					foreach (Worker worker8 in aiPlayer.player.matPlayer.workers)
					{
						if (worker8.position.hexType == HexType.farm && !this.moveTarget.ContainsKey(worker8))
						{
							worker7 = worker8;
						}
					}
					if (worker7 != null)
					{
						GameHex gameHex18 = null;
						foreach (GameHex gameHex19 in this.moveRangeAll[worker7].Keys)
						{
							if (gameHex19.Owner == null || gameHex19.Owner == aiPlayer.player)
							{
								gameHex18 = gameHex19;
							}
						}
						if (gameHex18 != null)
						{
							this.movePriority.Add(worker7, 1);
							this.moveTarget.Add(worker7, new List<GameHex> { gameHex18 });
							this.moveDistance.Add(worker7, 1);
						}
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Albion)
			{
				int albBottomRowStars = 0;
				if (aiPlayer.player.GetNumberOfStars(StarType.Mechs) > 0)
				{
					albBottomRowStars++;
				}
				if (aiPlayer.player.GetNumberOfStars(StarType.Recruits) > 0)
				{
					albBottomRowStars++;
				}
				if (aiPlayer.player.GetNumberOfStars(StarType.Structures) > 0)
				{
					albBottomRowStars++;
				}
				if (aiPlayer.player.GetNumberOfStars(StarType.Upgrades) > 0)
				{
					albBottomRowStars++;
				}
				if (!this.movePriority.ContainsKey(aiPlayer.player.character))
				{
					GameHex gameHexAlbChar = null;
					int maxVal = 0;
					foreach (GameHex gh in this.moveRangeAll[aiPlayer.player.character].Keys)
					{
						if (gh.Token == null && gh.hexType != HexType.capital && (gh.Owner == null || gh.Owner == aiPlayer.player))
						{
							int val = 0;
							if (gh.hexType == HexType.factory)
							{
								val += 200;
							}
							else if (gh.hasTunnel)
							{
								val += 50;
							}
							else
							{
								val += 10;
							}
							if (val > maxVal)
							{
								maxVal = val;
								gameHexAlbChar = gh;
							}
						}
					}
					if (gameHexAlbChar != null)
					{
						int alb_pri = ((albBottomRowStars >= 2) ? (priEncounter + 60) : (priEncounter + 20));
						this.movePriority.Add(aiPlayer.player.character, alb_pri);
						this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHexAlbChar });
						this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[gameHexAlbChar]);
					}
				}
				foreach (Mech mechAlbFollow in aiPlayer.player.matFaction.mechs)
				{
					if (!this.movePriority.ContainsKey(mechAlbFollow))
					{
						GameHex gameHexAlbMech = null;
						foreach (GameHex gh2 in this.moveRangeAll[mechAlbFollow].Keys)
						{
							if (gh2.Token != null && gh2.Token.Owner == aiPlayer.player && gh2.GetOwnerWorkers().Count == 0)
							{
								gameHexAlbMech = gh2;
								break;
							}
						}
						if (gameHexAlbMech != null)
						{
							this.movePriority.Add(mechAlbFollow, priEncounter + 55);
							this.moveTarget.Add(mechAlbFollow, new List<GameHex> { gameHexAlbMech });
							this.moveDistance.Add(mechAlbFollow, this.moveRange[mechAlbFollow][gameHexAlbMech]);
							List<Worker> listPassengers = mechAlbFollow.position.GetOwnerWorkers();
							if (listPassengers.Count > 0)
							{
								if (!this.moveMechPassengers.ContainsKey(mechAlbFollow))
								{
									this.moveMechPassengers.Add(mechAlbFollow, new List<Worker>());
								}
								this.moveMechPassengers[mechAlbFollow].Add(listPassengers[0]);
								if (!this.movePriority.ContainsKey(listPassengers[0]))
								{
									this.movePriority.Add(listPassengers[0], -1);
								}
							}
						}
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Togawa)
			{
				int togBottomRowStars = 0;
				if (aiPlayer.player.GetNumberOfStars(StarType.Mechs) > 0)
				{
					togBottomRowStars++;
				}
				if (aiPlayer.player.GetNumberOfStars(StarType.Recruits) > 0)
				{
					togBottomRowStars++;
				}
				if (aiPlayer.player.GetNumberOfStars(StarType.Structures) > 0)
				{
					togBottomRowStars++;
				}
				if (aiPlayer.player.GetNumberOfStars(StarType.Upgrades) > 0)
				{
					togBottomRowStars++;
				}
				if (!this.movePriority.ContainsKey(aiPlayer.player.character))
				{
					GameHex gameHexTogChar = null;
					int maxValTog = 0;
					foreach (GameHex gh3 in this.moveRangeAll[aiPlayer.player.character].Keys)
					{
						if (gh3.Token == null && gh3.hexType != HexType.capital && (gh3.Owner == null || gh3.Owner == aiPlayer.player))
						{
							int val2 = 0;
							if (gh3.hexType == HexType.factory)
							{
								val2 += 200;
							}
							else
							{
								val2 += 10;
							}
							if (val2 > maxValTog)
							{
								maxValTog = val2;
								gameHexTogChar = gh3;
							}
						}
					}
					if (gameHexTogChar != null)
					{
						int tog_pri = ((togBottomRowStars >= 2) ? (priEncounter + 50) : (priEncounter + 15));
						this.movePriority.Add(aiPlayer.player.character, tog_pri);
						this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHexTogChar });
						this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[gameHexTogChar]);
					}
				}
			}
			foreach (Worker worker9 in aiPlayer.player.matPlayer.workers)
			{
				if (!this.movePriority.Keys.Contains(worker9))
				{
					bool flag = this.uselessWorkers4Production.Contains(worker9);
					if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && !flag)
					{
						ResourceType resourceType = AiStrategicAnalysis.ResourceProduced(worker9.position.hexType);
						if (resourceType != ResourceType.combatCard)
						{
							if (worker9.position.GetOwnerWorkers().Count > this.resourceDemandTotal[resourceType])
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
						foreach (Mech mech11 in worker9.position.GetOwnerMechs())
						{
							if (this.moveMechPassengers.Keys.Contains(mech11) && this.moveMechPassengers[mech11].Contains(worker9))
							{
								flag2 = true;
							}
						}
						if (!flag2)
						{
							GameHex gameHex20 = null;
							switch (aiPlayer.player.matPlayer.matType)
							{
							case PlayerMatType.Patriotic:
								goto IL_21BC;
							case PlayerMatType.Mechanical:
								foreach (GameHex gameHex21 in this.moveRangeAll[worker9].Keys)
								{
									if (gameHex21.Owner == null)
									{
										gameHex20 = gameHex21;
									}
								}
								break;
							case PlayerMatType.Agricultural:
								goto IL_2128;
							case PlayerMatType.Militant:
							{
								using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator7 = this.moveRangeAll[worker9].Keys.GetEnumerator())
								{
									while (enumerator7.MoveNext())
									{
										GameHex gameHex22 = enumerator7.Current;
										ResourceType resourceType2 = AiStrategicAnalysis.ResourceProduced(gameHex22.hexType);
										if ((gameHex22.Owner == null || gameHex22.Owner == aiPlayer.player) && resourceType2 != ResourceType.combatCard && (gameHex20 == null || (this.resourceDemandTotal[resourceType2] > 0 && this.resourceAccess[resourceType2] == 0)))
										{
											gameHex20 = gameHex22;
										}
									}
									break;
								}
							}
							default:
								goto IL_2128;
							}
							IL_2054:
							if (gameHex20 != null)
							{
								this.movePriority.Add(worker9, 1);
								this.moveTarget.Add(worker9, new List<GameHex> { gameHex20 });
								this.moveDistance.Add(worker9, 1);
								continue;
							}
							continue;
							IL_2128:
							using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator8 = this.moveRangeAll[worker9].Keys.GetEnumerator())
							{
								while (enumerator8.MoveNext())
								{
									GameHex gameHex23 = enumerator8.Current;
									ResourceType resourceType3 = AiStrategicAnalysis.ResourceProduced(gameHex23.hexType);
									if (gameHex23.Owner == null && resourceType3 != ResourceType.combatCard && (gameHex20 == null || (this.ResourcePriority(aiPlayer, gameHex23) > this.ResourcePriority(aiPlayer, gameHex20) && this.resourceDemandTotal[resourceType3] > this.resourceAccess[resourceType3])))
									{
										gameHex20 = gameHex23;
									}
								}
								goto IL_2054;
							}
							IL_21BC:
							using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator9 = this.moveRangeAll[worker9].Keys.GetEnumerator())
							{
								while (enumerator9.MoveNext())
								{
									GameHex gameHex24 = enumerator9.Current;
									if (gameHex24.Owner == null && (gameHex20 == null || this.ResourcePriority(aiPlayer, gameHex24) > this.ResourcePriority(aiPlayer, gameHex20)))
									{
										gameHex20 = gameHex24;
									}
								}
								goto IL_2054;
							}
						}
					}
				}
			}
			foreach (Unit unit4 in this.moveRangeAll.Keys)
			{
				if (unit4.position.hexType == HexType.capital && !this.movePriority.ContainsKey(unit4))
				{
					GameHex gameHex25 = null;
					foreach (GameHex gameHex26 in this.moveRangeAll[unit4].Keys)
					{
						if (gameHex26.hexType != HexType.capital && (gameHex26.Owner == null || gameHex26.Owner == aiPlayer.player) && (gameHex25 == null || gameHex25.Owner != null))
						{
							gameHex25 = gameHex26;
						}
					}
					if (gameHex25 != null)
					{
						this.movePriority.Add(unit4, 1);
						this.moveTarget.Add(unit4, new List<GameHex> { gameHex25 });
						this.moveDistance.Add(unit4, this.moveRangeAll[unit4][gameHex25]);
					}
				}
			}
			if (aiPlayer.player.GetNumberOfStars(StarType.Power) > 0 && aiPlayer.player.GetNumberOfStars(StarType.Combat) < ((aiPlayer.player.matFaction.faction == Faction.Saxony) ? 6 : 2) && this.enemyCanBeAttackedBy.Count == 0 && !aiPlayer.player.matFaction.factionPerk.HasFlag(AbilityPerk.Dominate))
			{
				GameHex factory2 = this.gameManager.gameBoard.factory;
				if (!this.movePriority.ContainsKey(aiPlayer.player.character) && this.characterDistance.ContainsKey(factory2))
				{
					this.movePriority.Add(aiPlayer.player.character, priFight + 10);
					this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { factory2 });
					this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[factory2]);
				}
				foreach (Mech mech12 in aiPlayer.player.matFaction.mechs)
				{
					if (!this.movePriority.ContainsKey(mech12) && this.moveRange[mech12].ContainsKey(factory2))
					{
						this.movePriority.Add(mech12, priFight + 9);
						this.moveTarget.Add(mech12, new List<GameHex> { factory2 });
						this.moveDistance.Add(mech12, this.moveRange[mech12][factory2]);
					}
				}
			}
		}

		// Token: 0x06002E2C RID: 11820
		public override void UpdateWorkerCountTarget(AiPlayer aiPlayer)
		{
			this.workerCountTarget = 5;
			this.pursuingWorkerStar = false;
			int bottomRowStars = 0;
			if (aiPlayer.player.GetNumberOfStars(StarType.Mechs) > 0)
			{
				bottomRowStars++;
			}
			if (aiPlayer.player.GetNumberOfStars(StarType.Recruits) > 0)
			{
				bottomRowStars++;
			}
			if (aiPlayer.player.GetNumberOfStars(StarType.Structures) > 0)
			{
				bottomRowStars++;
			}
			if (aiPlayer.player.GetNumberOfStars(StarType.Upgrades) > 0)
			{
				bottomRowStars++;
			}
			if (bottomRowStars >= 2 && aiPlayer.player.GetNumberOfStars(StarType.Workers) == 0)
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

		// Token: 0x06002E2D RID: 11821
		private void PlanNextTwoTurns(AiPlayer aiPlayer)
		{
			if (this.PredictResourcesNextTurn(aiPlayer)[ResourceType.oil] >= this.resourceCostSingleAction[ResourceType.oil] && aiPlayer.player.GetNumberOfStars(StarType.Upgrades) == 0)
			{
				Dictionary<ResourceType, int> resourceDemandPriority = this.resourceDemandPriority;
				resourceDemandPriority[ResourceType.oil] = resourceDemandPriority[ResourceType.oil] + 5;
			}
		}

		// Token: 0x06002E2E RID: 11822
		private Dictionary<ResourceType, int> PredictResourcesNextTurn(AiPlayer aiPlayer)
		{
			Dictionary<ResourceType, int> prediction = new Dictionary<ResourceType, int>();
			foreach (ResourceType type in this.resourceAccess.Keys)
			{
				int current = aiPlayer.player.Resources(false)[type];
				int willProduce = this.resourceAccess[type];
				prediction[type] = current + willProduce;
			}
			return prediction;
		}

		// Token: 0x06002E2F RID: 11823
		private int CalculateObjectiveProgress(ObjectiveCard objective)
		{
			return 0;
		}

		// Objective Priority Matrix Implementation
		public int GetObjectivePriority(int objId, AiPlayer aiPlayer)
		{
			Faction faction = aiPlayer.player.matFaction.faction;
			PlayerMatType matType = aiPlayer.player.matPlayer.matType;
			int priority = 0;

			// Base Faction Matrix
			switch (objId)
			{
				case 1: priority = (faction == Faction.Nordic || faction == Faction.Saxony || faction == Faction.Albion) ? 9 : ((faction == Faction.Crimea || faction == Faction.Togawa) ? 7 : 8); break;
				case 2: priority = (faction == Faction.Saxony) ? 10 : ((faction == Faction.Rusviet || faction == Faction.Togawa) ? 8 : 9); break;
				case 3: priority = (faction == Faction.Polania || faction == Faction.Togawa) ? 9 : ((faction == Faction.Saxony) ? 7 : 8); break;
				case 4: priority = (faction == Faction.Nordic) ? 9 : ((faction == Faction.Crimea || faction == Faction.Togawa) ? 7 : 8); break;
				case 5: priority = (faction == Faction.Nordic || faction == Faction.Crimea || faction == Faction.Saxony) ? 7 : ((faction == Faction.Polania) ? 4 : ((faction == Faction.Togawa) ? 3 : 5)); break;
				case 6: priority = (faction == Faction.Crimea) ? 10 : ((faction == Faction.Nordic || faction == Faction.Saxony) ? 9 : ((faction == Faction.Rusviet) ? 8 : ((faction == Faction.Polania || faction == Faction.Albion) ? 6 : 5))); break;
				case 7: priority = (faction == Faction.Rusviet) ? 3 : ((faction == Faction.Saxony) ? 1 : 2); break;
				case 8: priority = 1; break;
				case 9: priority = 3; break;
				case 10: priority = (faction == Faction.Polania) ? 9 : ((faction == Faction.Saxony) ? 10 : ((faction == Faction.Crimea || faction == Faction.Togawa) ? 7 : 8)); break;
				case 11: priority = (faction == Faction.Saxony) ? 7 : 8; break;
				case 12: priority = (faction == Faction.Rusviet) ? 10 : ((faction == Faction.Crimea) ? 8 : 9); break;
				case 13: priority = (faction == Faction.Nordic || faction == Faction.Saxony) ? 9 : 10; break;
				case 14: priority = (faction == Faction.Polania || faction == Faction.Rusviet || faction == Faction.Saxony || faction == Faction.Albion) ? 7 : 6; break;
				case 15: priority = (faction == Faction.Crimea) ? 4 : ((faction == Faction.Polania || faction == Faction.Togawa) ? 3 : ((faction == Faction.Nordic || faction == Faction.Saxony) ? 1 : 2)); break;
				case 16: priority = (faction == Faction.Togawa) ? 4 : 5; break;
				case 17: priority = 2; break;
				case 18: priority = (faction == Faction.Polania) ? 2 : 1; break;
				case 19: priority = (faction == Faction.Albion) ? 8 : 7; break;
				case 20: priority = (faction == Faction.Polania) ? 3 : ((faction == Faction.Albion) ? 4 : ((faction == Faction.Togawa) ? 2 : 1)); break;
				case 21: priority = 3; break;
				case 22: priority = (faction == Faction.Nordic || faction == Faction.Rusviet || faction == Faction.Crimea || faction == Faction.Albion || faction == Faction.Togawa) ? 5 : 4; break;
				case 23: priority = (faction == Faction.Nordic || faction == Faction.Crimea) ? 5 : ((faction == Faction.Saxony) ? 7 : 3); break;
				case 24: priority = 4; break;
				case 25: priority = (faction == Faction.Nordic || faction == Faction.Crimea) ? 6 : ((faction == Faction.Saxony) ? 8 : ((faction == Faction.Albion) ? 3 : ((faction == Faction.Polania || faction == Faction.Togawa) ? 2 : 4))); break;
				case 26: priority = (faction == Faction.Polania || faction == Faction.Togawa) ? 10 : ((faction == Faction.Saxony) ? 8 : 9); break;
				case 27: priority = (faction == Faction.Albion) ? 3 : 1; break;
				default: priority = 0; break;
			}

			// Mat Type Modifiers
			if (matType == PlayerMatType.Patriotic || matType == PlayerMatType.Militant)
			{
				if (objId == 6) priority += 1;
				if (objId == 12) priority += 1;
			}
			if (matType == PlayerMatType.Industrial)
			{
				if (objId == 12) priority += 1;
			}
			if (matType == PlayerMatType.Mechanical)
			{
				if (objId == 7) priority += 2;
				if (objId == 25) priority += 1;
			}
			if (matType == PlayerMatType.Agricultural)
			{
				if (objId == 15) priority += 3;
				if (objId == 20) priority += 1;
			}
			if (matType == PlayerMatType.Engineering)
			{
				if (objId == 20) priority += 2;
				if (objId == 22) priority += 2;
			}
			if (matType == PlayerMatType.Militant)
			{
				if (objId == 23) priority += 2;
				if (objId == 25) priority += 2;
			}
			if (matType == PlayerMatType.Innovative)
			{
				if (objId == 22) priority += 1;
			}

			// Boundary Check
			if (priority > 10) priority = 10;
			return priority;
		}

		// Token: 0x04001F45 RID: 8005
		public HashSet<GameHex> objectiveArea = new HashSet<GameHex>();

		// Token: 0x04001F46 RID: 8006
		public int objectiveTarget;

		// Token: 0x04001F47 RID: 8007
		public bool moveToObjective;
	}
}

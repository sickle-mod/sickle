using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005B1 RID: 1457
	public class AiStrategicAnalysisAdv : AiStrategicAnalysis
	{
		// Token: 0x06002E15 RID: 11797
		public AiStrategicAnalysisAdv(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002E16 RID: 11798
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
				bool hasObj6 = false;
				foreach (ObjectiveCard card2 in aiPlayer.player.objectiveCards)
				{
					if (card2.CardId == 6 && card2.status == ObjectiveCard.ObjectiveStatus.Open)
					{
						hasObj6 = true;
						break;
					}
				}
				if (!hasObj6)
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

		// Token: 0x06002E17 RID: 11799
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

		// Token: 0x06002E18 RID: 11800
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
			// Industrial + (Polania or Crimea or Albion or Nordic or Rusviet or Togawa) or Engineering + (Crimea or Albion or Rusviet or Togawa) or Innovative + (Polania or Crimea or Albion or Nordic or Rusviet or Togawa) or Patriotic + (Albion or Rusviet or Togawa or Crimea or Nordic or Polania) or Mechanical + (Albion or Rusviet or Togawa or Crimea or Nordic or Polania) or Agricultural + (Albion or Rusviet or Togawa or Crimea) or Militant + (Albion or Rusviet or Togawa or Crimea or Nordic or Polania): boost Power and CombatCard for 2 combat stars
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
		}

		// Token: 0x06002E19 RID: 11801
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
				this.mechPriority[0] = 8;
				this.mechPriority[1] = 6;
				this.mechPriority[2] = 4;
				this.mechPriority[3] = 10;
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

		// Token: 0x06002E1A RID: 11802
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

		// Token: 0x06002E1B RID: 11803
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
					// Industrial Polania: Workers, Mechs, Recruits, Power, 2 combats, Objective
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
					// Innovative Polania: Recruits, Mechs, Workers, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					// Patriotic Polania: Workers, Recruits, Mechs, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					// Mechanical Polania: Workers, Mechs, Recruits, Power, 2 combats, Objective
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
						// Militant Polania: Workers, Mechs, Enlists, 2 combats, Objective
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
					// Mechanical Albion: Workers, Mechs, Recruits, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
				{
					// Innovative Albion: 2 Upgrades, Mechs, 4 Upgrades, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.oil] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					// Industrial Albion: Workers, Mechs, Recruits, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					// Engineering Albion: Workers, Mechs, Upgrades, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.oil] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					// Patriotic Albion: Workers, Recruits, Mechs, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					// Agricultural Albion: Workers, Recruits, Mechs, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
				{
					// Militant Albion: Workers, Mechs, Enlists, 2 combats, Objective
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
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
				{
					// Innovative Nordic: 2 Upgrades, Mechs, 4 Upgrades, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.oil] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					// Industrial Nordic: Workers, Mechs, Recruits, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					// Patriotic Nordic: Workers, Recruits, Mechs, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					// Mechanical Nordic: Workers, Mechs, Recruits, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
				{
					// Militant Nordic: Workers, Mechs, Enlists, 2 combats, Objective
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
			case Faction.Crimea:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					// Industrial Crimea: Workers, Mechs, Recruits, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					// Engineering Crimea: Recruits, Mechs, Workers, Objective, 2 combats
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
				{
					// Innovative Crimea: Recruits, Mechs, Workers, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					// Patriotic Crimea: Workers, Recruits, Mechs, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					// Mechanical Crimea: Workers, Mechs, Recruits, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					// Agricultural Crimea: Workers, Recruits, Mechs, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
				{
					// Militant Crimea: Workers, Mechs, Enlists, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				break;
			case Faction.Togawa:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
				{
					// Innovative Togawa: 2 Upgrades, Mechs, 4 Upgrades, Power, 2 combats, Objective
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
					// Industrial Togawa: Workers, Mechs, Recruits, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					// Engineering Togawa: Workers, Mechs, Upgrades, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.oil] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					// Patriotic Togawa: Workers, Recruits, Mechs, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					// Mechanical Togawa: Workers, Mechs, Recruits, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					// Agricultural Togawa: Workers, Recruits, Mechs, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
				{
					// Militant Togawa: Workers, Mechs, Enlists, 2 combats, Objective
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
			case Faction.Rusviet:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
				{
					// Innovative Rusviet: 2 Upgrades, Mechs, 4 Upgrades, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.oil] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					// Industrial Rusviet: Workers, Mechs, Recruits, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					// Engineering Rusviet: Workers, Mechs, Upgrades, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.oil] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					// Patriotic Rusviet: Workers, Recruits, Mechs, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					// Agricultural Rusviet: Workers, Recruits, Mechs, Power, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.food] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
				{
					// Militant Rusviet: Workers, Mechs, Enlists, 2 combats, Objective
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.food] = 10;
					this.resourceDemandPriority[ResourceType.oil] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				break;
			case Faction.Saxony:
				if (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					// Industrial Saxony: 6 combat stars, 2 objective stars, workers, 2 upgrades, mechs, power, upgrades
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.oil] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					// Engineering Saxony: 6 combat stars, 2 objective stars, workers, 2 upgrades, mechs, upgrades, recruits
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.oil] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic)
				{
					// Patriotic Saxony: 6 combat stars, 2 objective stars, workers, 2 upgrades, mechs, recruits, upgrades
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.oil] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical)
				{
					// Mechanical Saxony: 6 combat stars, 2 objective stars, workers, 2 upgrades, mechs, power, recruits
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.oil] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					// Agricultural Saxony: 6 combat stars, 2 objective stars, workers, 2 upgrades, mechs, recruits
					this.resourceDemandPriority[ResourceType.metal] = 11;
					this.resourceDemandPriority[ResourceType.oil] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant)
				{
					// Militant Saxony: 6 combat stars, 2 objective stars, 2 upgrades, workers, mechs, recruits, upgrades
					this.resourceDemandPriority[ResourceType.oil] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
					this.resourceDemandPriority[ResourceType.wood] = 2;
				}
				else if (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)
				{
					// Innovative Saxony: 6 combat stars, 2 objective stars, 2 upgrades, workers, mechs, upgrades, recruits
					this.resourceDemandPriority[ResourceType.oil] = 11;
					this.resourceDemandPriority[ResourceType.metal] = 10;
					this.resourceDemandPriority[ResourceType.food] = 3;
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

		// Token: 0x06002E1C RID: 11804
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

		// Token: 0x06002E1D RID: 11805
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

		// Token: 0x06002E1E RID: 11806
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

		// Token: 0x06002E1F RID: 11807
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
					using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator6 = this.moveRangeAll[mech].Keys.GetEnumerator())
					{
						while (enumerator6.MoveNext())
						{
							GameHex gameHex2 = enumerator6.Current;
							if (this.ResourcePriority(aiPlayer, gameHex2) > 0f && (gameHex2.Owner == null || gameHex2.Owner == aiPlayer.player) && (gameHex == null || (this.ResourcePriority(aiPlayer, gameHex2) > this.ResourcePriority(aiPlayer, gameHex) && this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex2.hexType)] <= this.resourceAccess[AiStrategicAnalysis.ResourceProduced(gameHex.hexType)])))
							{
								gameHex = gameHex2;
							}
						}
						goto IL_0167;
					}
					continue;
				}
				foreach (GameHex gameHex3 in this.moveRangeAll[mech].Keys)
				{
					if (this.ResourcePriority(aiPlayer, gameHex3) > 0f && (gameHex3.Owner == null || gameHex3.Owner == aiPlayer.player) && (gameHex == null || this.ResourcePriority(aiPlayer, gameHex3) > this.ResourcePriority(aiPlayer, gameHex)))
					{
						gameHex = gameHex3;
					}
				}
				IL_0167:
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

		// Token: 0x06002E20 RID: 11808
		protected void UpdateMoveTargetsObjective(AiPlayer aiPlayer, int priProduce1, int priProduce2, int priEncounter, int priFight, int priBuildSpot)
		{
			if (this.objectiveArea.Count > 0 && ((aiPlayer.player.matFaction.faction == Faction.Saxony) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative)))
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

		// Token: 0x06002E21 RID: 11809
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

		// Token: 0x06002E22 RID: 11810
		private int ObjectiveSendOneBackPriority(Unit unit, GameHex hex)
		{
			if (unit.UnitType == UnitType.Mech)
			{
				return 10;
			}
			return 1;
		}

		// Token: 0x06002E23 RID: 11811
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
				num7 += (float)(2 - hex.GetOwnerWorkers().Count);
			}
			if (num7 > 0f && this.enemyCanBeAttackedBy[hex].Count<Unit>() > 1)
			{
				num7 += 10f;
			}
			return num7;
		}

		// Token: 0x06002E24 RID: 11812
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
			if (!this.movePriority.ContainsKey(aiPlayer.player.character) && ((aiPlayer.player.matFaction.faction == Faction.Saxony) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural)))
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
			if (priFight > 0 && aiPlayer.strategicAnalysis.enemyCanBeAttackedBy.Count > 0 && (aiPlayer.player.Power >= 7 || aiPlayer.player.GetNumberOfStars(StarType.Power) > 0 || (aiPlayer.player.matFaction.faction == Faction.Saxony) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && (aiPlayer.player.matFaction.faction == Faction.Polania || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering && (aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Polania)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative && (aiPlayer.player.matFaction.faction == Faction.Polania || aiPlayer.player.matFaction.faction == Faction.Crimea || aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa))) && (aiPlayer.player.GetNumberOfStars(StarType.Combat) < (aiPlayer.player.matFaction.faction == Faction.Saxony ? 6 : 2) || aiPlayer.player.matFaction.factionPerk == AbilityPerk.Dominate))
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
					if ((aiPlayer.player.matFaction.faction == Faction.Saxony) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Polania && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural) || (aiPlayer.player.matFaction.faction == Faction.Crimea && aiPlayer.player.matPlayer.matType == PlayerMatType.Militant) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Nordic || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Engineering && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Patriotic && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Nordic)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Nordic)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa)) || (aiPlayer.player.matPlayer.matType == PlayerMatType.Militant && (aiPlayer.player.matFaction.faction == Faction.Albion || aiPlayer.player.matFaction.faction == Faction.Rusviet || aiPlayer.player.matFaction.faction == Faction.Togawa || aiPlayer.player.matFaction.faction == Faction.Nordic)) || (aiPlayer.player.matFaction.faction == Faction.Albion && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial))
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

		// Token: 0x06002E25 RID: 11813
		private void UpdateMoveTargetsSecondary(AiPlayer aiPlayer, int priProduce1, int priProduce2, int priEncounter, int priFight, int priBuildSpot)
		{
			if (aiPlayer.player.matPlayer.workers.Count < this.workerCountTarget)
			{
				this.workersInaVillage = 0;
				foreach (Worker worker7 in aiPlayer.player.matPlayer.workers)
				{
					if (worker7.position.hexType == HexType.village)
					{
						this.workersInaVillage++;
					}
					if (worker7.position.hexType != HexType.capital)
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
			if (aiPlayer.player.matPlayer.matPlayerSectionsCount <= 4 && this.factoryDistance <= (int)aiPlayer.player.character.MaxMoveCount && !this.movePriority.ContainsKey(aiPlayer.player.character) && (this.gameManager.gameBoard.factory.Owner == aiPlayer.player || this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0))
			{
				if (this.characterDistance.ContainsKey(this.gameManager.gameBoard.factory))
				{
					this.movePriority.Add(aiPlayer.player.character, priEncounter + 35);
					this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { this.gameManager.gameBoard.factory });
					this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[this.gameManager.gameBoard.factory]);
				}
			}
			else if (!this.movePriority.ContainsKey(aiPlayer.player.character) && this.encounterNearestHex != null && this.characterDistance.ContainsKey(this.encounterNearestHex) && this.characterDistance[this.encounterNearestHex] <= (int)aiPlayer.player.character.MaxMoveCount)
			{
				this.movePriority.Add(aiPlayer.player.character, priEncounter);
				this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { this.encounterNearestHex });
				this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[this.encounterNearestHex]);
			}
			foreach (KeyValuePair<Unit, int> keyValuePair in this.movePriority.ToList())
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
							GameHex gameHex7 = null;
							foreach (GameHex gameHex8 in this.moveRangeAll[unit3].Keys)
							{
								if (gameHex8.hexType != HexType.capital && (gameHex8.Owner == null || gameHex8.Owner == aiPlayer.player) && (gameHex7 == null || gameHex7.Owner != null))
								{
									gameHex7 = gameHex8;
								}
							}
							if (gameHex7 != null)
							{
								this.movePriority.Add(unit3, 1);
								this.moveTarget.Add(unit3, new List<GameHex> { gameHex7 });
								this.moveDistance.Add(unit3, this.moveRangeAll[unit3][gameHex7]);
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
								GameHex gameHex9 = enumerator6.Current;
								if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && (gameHex9.Owner == null || gameHex9.Owner == aiPlayer.player) && (gameHex9.hexType == HexType.mountain || gameHex9.hasTunnel))
								{
									this.movePriority.Add(aiPlayer.player.character, 1);
									this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex9 });
									this.moveDistance.Add(aiPlayer.player.character, this.moveRangeAll[aiPlayer.player.character][gameHex9]);
								}
							}
							goto IL_0FB0;
						}
					}
					GameHex gameHex10 = null;
					foreach (GameHex gameHex11 in this.moveRangeAll[aiPlayer.player.character].Keys)
					{
						if ((gameHex11.Owner == null || gameHex11.Owner == aiPlayer.player) && (gameHex11.hexType == HexType.mountain || gameHex11.hasTunnel))
						{
							int num = 0;
							int num2 = 0;
							if (gameHex10 != null)
							{
								if (gameHex10.hasTunnel)
								{
									num += 2;
								}
								if (gameHex10.hexType == HexType.mountain)
								{
									num++;
								}
							}
							if (gameHex11.hasTunnel)
							{
								num2 += 2;
							}
							if (gameHex11.hexType == HexType.mountain)
							{
								num2++;
							}
							if (num2 > num)
							{
								gameHex10 = gameHex11;
							}
						}
					}
					if (gameHex10 != null)
					{
						this.movePriority.Add(aiPlayer.player.character, 1);
						this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex10 });
						this.moveDistance.Add(aiPlayer.player.character, this.moveRangeAll[aiPlayer.player.character][gameHex10]);
					}
				}
			}
			IL_0FB0:
			if (aiPlayer.player.matFaction.faction == Faction.Polania)
			{
				if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && aiPlayer.player.character.position.hexType != HexType.lake)
				{
					foreach (GameHex gameHex12 in this.moveRangeAll[aiPlayer.player.character].Keys)
					{
						if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && (gameHex12.Owner == null || gameHex12.Owner == aiPlayer.player) && gameHex12.hexType == HexType.lake)
						{
							this.movePriority.Add(aiPlayer.player.character, 1);
							this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex12 });
							this.moveDistance.Add(aiPlayer.player.character, this.moveRangeAll[aiPlayer.player.character][gameHex12]);
						}
					}
				}
				if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && aiPlayer.player.character.position.hexType != HexType.lake && !aiPlayer.player.character.position.hasTunnel)
				{
					foreach (GameHex gameHex13 in this.moveRangeAll[aiPlayer.player.character].Keys)
					{
						if (!this.moveTarget.ContainsKey(aiPlayer.player.character) && (gameHex13.Owner == null || gameHex13.Owner == aiPlayer.player) && (gameHex13.hexType == HexType.lake || gameHex13.hasTunnel))
						{
							this.movePriority.Add(aiPlayer.player.character, 1);
							this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex13 });
							this.moveDistance.Add(aiPlayer.player.character, this.moveRangeAll[aiPlayer.player.character][gameHex13]);
						}
					}
				}
				if (aiPlayer.player.matFaction.abilities.Contains(AbilityPerk.Camaraderie) && aiPlayer.player.matFaction.SkillUnlocked[2])
				{
					foreach (Mech mech8 in aiPlayer.player.matFaction.mechs)
					{
						if (!this.moveTarget.ContainsKey(mech8))
						{
							GameHex gameHex14 = null;
							int num3 = 0;
							foreach (GameHex gameHex15 in this.moveRangeAll[aiPlayer.player.character].Keys)
							{
								if (gameHex15.Owner != aiPlayer.player && !gameHex15.HasOwnerCharacter() && gameHex15.GetOwnerMechs().Count == 0)
								{
									int num4 = gameHex15.GetEnemyWorkers().Count + gameHex15.GetResourceCount() * 8;
									if (num4 > num3)
									{
										gameHex14 = gameHex15;
										num3 = num4;
									}
								}
							}
							if (gameHex14 != null)
							{
								this.movePriority.Add(mech8, num3);
								this.moveTarget.Add(mech8, new List<GameHex> { gameHex14 });
								this.moveDistance.Add(mech8, this.moveRangeAll[aiPlayer.player.character][gameHex14]);
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
						GameHex gameHex16 = null;
						foreach (GameHex gameHex17 in this.moveRangeAll[worker4].Keys)
						{
							if (gameHex17.Owner == null || gameHex17.Owner == aiPlayer.player)
							{
								gameHex16 = gameHex17;
							}
						}
						if (gameHex16 != null)
						{
							this.movePriority.Add(worker4, 1);
							this.moveTarget.Add(worker4, new List<GameHex> { gameHex16 });
							this.moveDistance.Add(worker4, 1);
						}
					}
				}
			}
			if (aiPlayer.player.matFaction.faction == Faction.Albion)
			{
				if ((aiPlayer.player.matPlayer.matType == PlayerMatType.Militant || aiPlayer.player.matPlayer.matType == PlayerMatType.Mechanical || aiPlayer.player.matPlayer.matType == PlayerMatType.Agricultural || aiPlayer.player.matPlayer.matType == PlayerMatType.Innovative) && !this.movePriority.ContainsKey(aiPlayer.player.character))
				{
					GameHex gameHex18 = null;
					int num5 = 0;
					foreach (GameHex gameHex19 in this.moveRangeAll[aiPlayer.player.character].Keys)
					{
						int num6 = 0;
						if (gameHex19.Token == null)
						{
							num6 += 100;
						}
						if (gameHex19.hexType != HexType.capital)
						{
							num6 += 10;
						}
						if (gameHex19.hasTunnel)
						{
							num6 += 20;
						}
						if (gameHex19.hexType == HexType.factory)
						{
							num6 += 80;
						}
						if (gameHex19.Owner != null && gameHex19.Owner.matFaction.faction != Faction.Albion && gameHex19.GetOwnerUnitCount() > 0)
						{
							num6 -= 1000;
						}
						if (num6 > num5)
						{
							gameHex18 = gameHex19;
							num5 = num6;
						}
					}
					if (gameHex18 != null)
					{
						this.movePriority.Add(aiPlayer.player.character, 4);
						this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { gameHex18 });
						this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[gameHex18]);
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
							foreach (Mech mech11 in worker6.position.GetOwnerMechs())
							{
								if (this.moveMechPassengers.Keys.Contains(mech11) && this.moveMechPassengers[mech11].Contains(worker6))
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
									goto IL_1AF1;
								case PlayerMatType.Mechanical:
								{
									using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator15 = this.moveRangeAll[worker6].Keys.GetEnumerator())
									{
										while (enumerator15.MoveNext())
										{
											GameHex gameHex21 = enumerator15.Current;
											if (gameHex21.Owner == null)
											{
												gameHex20 = gameHex21;
											}
										}
										break;
									}
									goto IL_198C;
								}
								case PlayerMatType.Agricultural:
									goto IL_1A5D;
								case PlayerMatType.Militant:
									goto IL_198C;
								default:
									goto IL_1A5D;
								}
								IL_1A1B:
								if (gameHex20 != null)
								{
									this.movePriority.Add(worker6, 1);
									this.moveTarget.Add(worker6, new List<GameHex> { gameHex20 });
									this.moveDistance.Add(worker6, 1);
									continue;
								}
								continue;
								IL_1A5D:
								using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator7 = this.moveRangeAll[worker6].Keys.GetEnumerator())
								{
									while (enumerator7.MoveNext())
									{
										GameHex gameHex22 = enumerator7.Current;
										ResourceType resourceType2 = AiStrategicAnalysis.ResourceProduced(gameHex22.hexType);
										if (gameHex22.Owner == null && resourceType2 != ResourceType.combatCard && (gameHex20 == null || (this.ResourcePriority(aiPlayer, gameHex22) > this.ResourcePriority(aiPlayer, gameHex20) && this.resourceDemandTotal[resourceType2] > this.resourceAccess[resourceType2])))
										{
											gameHex20 = gameHex22;
										}
									}
									goto IL_1A1B;
								}
								goto IL_1AF1;
								IL_198C:
								foreach (GameHex gameHex23 in this.moveRangeAll[worker6].Keys)
								{
									ResourceType resourceType3 = AiStrategicAnalysis.ResourceProduced(gameHex23.hexType);
									if ((gameHex23.Owner == null || gameHex23.Owner == aiPlayer.player) && resourceType3 != ResourceType.combatCard && (gameHex20 == null || (this.resourceDemandTotal[resourceType3] > 0 && this.resourceAccess[resourceType3] == 0)))
									{
										gameHex20 = gameHex23;
									}
								}
								goto IL_1A1B;
								IL_1AF1:
								using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator9 = this.moveRangeAll[worker6].Keys.GetEnumerator())
								{
									while (enumerator9.MoveNext())
									{
										GameHex gameHex24 = enumerator9.Current;
										if (gameHex24.Owner == null && (gameHex20 == null || this.ResourcePriority(aiPlayer, gameHex24) > this.ResourcePriority(aiPlayer, gameHex20)))
										{
											gameHex20 = gameHex24;
										}
									}
									goto IL_1A1B;
								}
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
			if (aiPlayer.player.GetNumberOfStars(StarType.Power) > 0 && aiPlayer.player.GetNumberOfStars(StarType.Combat) < (aiPlayer.player.matFaction.faction == Faction.Saxony ? 6 : 2) && this.enemyCanBeAttackedBy.Count == 0 && !aiPlayer.player.matFaction.factionPerk.HasFlag(AbilityPerk.Dominate))
			{
				GameHex factory = this.gameManager.gameBoard.factory;
				if (!this.movePriority.ContainsKey(aiPlayer.player.character) && this.characterDistance.ContainsKey(factory))
				{
					this.movePriority.Add(aiPlayer.player.character, priFight + 10);
					this.moveTarget.Add(aiPlayer.player.character, new List<GameHex> { factory });
					this.moveDistance.Add(aiPlayer.player.character, this.characterDistance[factory]);
				}
				foreach (Mech mech in aiPlayer.player.matFaction.mechs)
				{
					if (!this.movePriority.ContainsKey(mech) && this.moveRange[mech].ContainsKey(factory))
					{
						this.movePriority.Add(mech, priFight + 9);
						this.moveTarget.Add(mech, new List<GameHex> { factory });
						this.moveDistance.Add(mech, this.moveRange[mech][factory]);
					}
				}
			}
		}

		// Token: 0x06002E26 RID: 11814
		public override void UpdateWorkerCountTarget(AiPlayer aiPlayer)
		{
			this.workerCountTarget = 5;
			this.pursuingWorkerStar = false;
			if (aiPlayer.player.matFaction.faction == Faction.Saxony && aiPlayer.player.matPlayer.matType == PlayerMatType.Industrial && aiPlayer.TradeResourceType() == ResourceType.combatCard)
			{
				this.workerCountTarget = 8;
			}
			// Once 2+ of the 4 bottom-row stars (Mechs, Recruits, Structures, Upgrades) are earned,
			// raise the worker count target to 8 and pursue the Workers star
			int bottomRowStars = 0;
			if (aiPlayer.player.GetNumberOfStars(StarType.Mechs) > 0) bottomRowStars++;
			if (aiPlayer.player.GetNumberOfStars(StarType.Recruits) > 0) bottomRowStars++;
			if (aiPlayer.player.GetNumberOfStars(StarType.Structures) > 0) bottomRowStars++;
			if (aiPlayer.player.GetNumberOfStars(StarType.Upgrades) > 0) bottomRowStars++;
			if (bottomRowStars >= 2 && aiPlayer.player.GetNumberOfStars(StarType.Workers) == 0)
			{
				this.workerCountTarget = 8;
				this.pursuingWorkerStar = true;
			}
		}

		// Token: 0x06002E27 RID: 11815
		private void PlanNextTwoTurns(AiPlayer aiPlayer)
		{
			if (this.PredictResourcesNextTurn(aiPlayer)[ResourceType.oil] >= this.resourceCostSingleAction[ResourceType.oil] && aiPlayer.player.GetNumberOfStars(StarType.Upgrades) == 0)
			{
				Dictionary<ResourceType, int> resourceDemandPriority = this.resourceDemandPriority;
				resourceDemandPriority[ResourceType.oil] = resourceDemandPriority[ResourceType.oil] + 5;
			}
		}

		// Token: 0x06002E28 RID: 11816
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

		// Token: 0x06002E29 RID: 11817
		private int CalculateObjectiveProgress(ObjectiveCard objective)
		{
			return 0;
		}

		// Token: 0x04001F42 RID: 8002
		public HashSet<GameHex> objectiveArea = new HashSet<GameHex>();

		// Token: 0x04001F43 RID: 8003
		public int objectiveTarget;

		// Token: 0x04001F44 RID: 8004
		public bool moveToObjective;
	}
}

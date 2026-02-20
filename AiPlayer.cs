using System;
using System.Collections.Generic;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic
{
	// Token: 0x0200059B RID: 1435
	public class AiPlayer
	{
		// Token: 0x14000135 RID: 309
		// (add) Token: 0x06002D5B RID: 11611 RVA: 0x0010AA10 File Offset: 0x00108C10
		// (remove) Token: 0x06002D5C RID: 11612 RVA: 0x0010AA44 File Offset: 0x00108C44
		public static event AiPlayer.LogDelegate LogMessage;

		// Token: 0x14000136 RID: 310
		// (add) Token: 0x06002D5D RID: 11613 RVA: 0x0010AA78 File Offset: 0x00108C78
		// (remove) Token: 0x06002D5E RID: 11614 RVA: 0x0010AAAC File Offset: 0x00108CAC
		public static event AiPlayer.CombatAbilityUsedDelegate CombatAbilityUsed;

		// Token: 0x06002D5F RID: 11615 RVA: 0x0010AAE0 File Offset: 0x00108CE0
		public AiPlayer(Player player, GameManager gameManager)
		{
			this.player = player;
			this.gameManager = gameManager;
			this.strategicAnalysis = new AiStrategicAnalysis(gameManager);
			this.kickstarter = new AiKickStart(gameManager);
			this.kickstarterAdv = new AiKickStartAdv(gameManager);
			this.Init();
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x0010AB78 File Offset: 0x00108D78
		public void Init()
		{
			if (this.player.aiDifficulty == AIDifficulty.Hard)
			{
				this.strategicAnalysis = new AiStrategicAnalysisAdv(this.gameManager);
			}
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				MatPlayerSection playerMatSection = this.player.matPlayer.GetPlayerMatSection(i);
				TopAction actionTop = playerMatSection.ActionTop;
				for (int j = 0; j < actionTop.gainActionsCount; j++)
				{
					AiAction aiAction = new AiAction(i, actionTop, j, playerMatSection.ActionDown, this.gameManager);
					switch (actionTop.GetGainAction(j).GetGainType())
					{
					case GainType.Coin:
						this.AiTopActions.Add(GainType.Coin, aiAction);
						break;
					case GainType.Popularity:
						this.AiTopActions.Add(GainType.Popularity, aiAction);
						break;
					case GainType.Power:
						this.AiTopActions.Add(GainType.Power, aiAction);
						break;
					case GainType.CombatCard:
						this.AiTopActions.Add(GainType.CombatCard, aiAction);
						break;
					case GainType.Produce:
						this.AiTopActions.Add(GainType.Produce, aiAction);
						break;
					case GainType.AnyResource:
						this.AiTopActions.Add(GainType.AnyResource, aiAction);
						break;
					case GainType.Move:
						this.AiTopActions.Add(GainType.Move, aiAction);
						break;
					}
					switch (playerMatSection.ActionDown.GetGainAction(0).GetGainType())
					{
					case GainType.Upgrade:
						this.gainUpgradeActionPosition.Add(num);
						break;
					case GainType.Mech:
						this.gainMechActionPosition.Add(num);
						break;
					case GainType.Building:
						this.gainBuildingActionPosition.Add(num);
						break;
					case GainType.Recruit:
						this.gainRecruitActionPosition.Add(num);
						break;
					}
					this.AiActions.Add(aiAction);
					num++;
				}
			}
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x0010AD24 File Offset: 0x00108F24
		public void MoveWorkerToBuild(GainMove gainMove, Worker w)
		{
			GameHex gameHex = null;
			if (w.position.Building != null)
			{
				if (w.position.Building.buildingType == BuildingType.Mine)
				{
					using (HashSet<GameHex>.Enumerator enumerator = w.position.GetFieldsAccessible(w, false).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GameHex gameHex2 = enumerator.Current;
							if (gameHex2.hasTunnel && gameHex2.Building == null && (gameHex2.Owner == null || gameHex2.Owner == this.player))
							{
								gameHex = gameHex2;
							}
						}
						goto IL_00DD;
					}
				}
				foreach (GameHex gameHex3 in w.position.GetFieldsAccessible(w, false))
				{
					if (gameHex3.Building == null && (gameHex3.Owner == null || gameHex3.Owner == this.player))
					{
						gameHex = gameHex3;
					}
				}
			}
			IL_00DD:
			if (gameHex != null)
			{
				this.gameManager.moveManager.SelectUnit(w);
				this.gameManager.moveManager.MoveSelectedUnit(gameHex, null, null);
			}
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x0004451F File Offset: 0x0004271F
		public List<ResourceBundle> FindResources(ResourceType resType, int resAmount, out bool fulfilled)
		{
			if (this.player.aiDifficulty == AIDifficulty.Hard)
			{
				return this.FindResourcesAdvanced(resType, resAmount, out fulfilled);
			}
			return this.FindResourcesBasic(resType, resAmount, out fulfilled);
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x0010AE54 File Offset: 0x00109054
		public List<ResourceBundle> FindResourcesBasic(ResourceType resType, int resAmount, out bool fulfilled)
		{
			List<ResourceBundle> list = new List<ResourceBundle>();
			List<GameHex> list2 = new List<GameHex>();
			foreach (GameHex gameHex in this.player.OwnedFields(false))
			{
				if (gameHex.hexType != HexType.capital && gameHex.resources[resType] > 0)
				{
					list2.Add(gameHex);
				}
			}
			if (this.player.matFaction.faction == Faction.Saxony && this.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				list2.Sort(delegate(GameHex A, GameHex B)
				{
					int num2 = 10 * (A.GetOwnerUnitCount() - A.GetOwnerWorkers().Count) + A.GetOwnerWorkers().Count;
					int num3 = 10 * (B.GetOwnerUnitCount() - B.GetOwnerWorkers().Count) + B.GetOwnerWorkers().Count;
					return num2.CompareTo(num3);
				});
			}
			foreach (GameHex gameHex2 in list2)
			{
				if (resAmount > 0)
				{
					int num = gameHex2.resources[resType];
					if (num > resAmount)
					{
						num = resAmount;
					}
					list.Add(new ResourceBundle
					{
						amount = (int)((short)num),
						gameHex = gameHex2,
						resourceType = resType
					});
					resAmount -= num;
				}
			}
			fulfilled = resAmount == 0;
			return list;
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x00044542 File Offset: 0x00042742
		public bool Pay4Action(PayResource payResource)
		{
			if (this.player.aiDifficulty == AIDifficulty.Hard)
			{
				return this.Pay4ActionAdvanced(payResource);
			}
			return this.Pay4ActionBasic(payResource);
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x0010AFA8 File Offset: 0x001091A8
		public bool Pay4ActionBasic(PayResource payResource)
		{
			bool flag = false;
			if (this.player.matFaction.factionPerk == AbilityPerk.Coercion && this.player.combatCards.Count > 0 && (this.player.matPlayer.matType == PlayerMatType.Industrial || this.player.matPlayer.matType == PlayerMatType.Patriotic || this.player.matPlayer.matType == PlayerMatType.Agricultural))
			{
				List<ResourceBundle> list = this.FindResources(payResource.ResourceToPay, (int)(payResource.Amount - 1), out flag);
				if (flag)
				{
					list.Add(new ResourceBundle
					{
						amount = 1,
						resourceType = ResourceType.combatCard,
						gameHex = this.player.character.position
					});
					this.player.combatCards.Sort((CombatCard A, CombatCard B) => A.CombatBonus.CompareTo(B.CombatBonus));
					payResource.SetResources(list, this.player.combatCards[0]);
					bool flag2 = payResource.CanExecute();
					if (flag2)
					{
						this.gameManager.actionManager.SetPayAction(payResource);
						this.gameManager.actionManager.PrepareNextAction();
					}
					return flag2;
				}
			}
			List<ResourceBundle> list2 = this.FindResources(payResource.ResourceToPay, (int)payResource.Amount, out flag);
			if (flag)
			{
				payResource.SetResources(list2, null);
			}
			else if (this.player.matFaction.factionPerk == AbilityPerk.Coercion && this.player.combatCards.Count > 0)
			{
				list2.Add(new ResourceBundle
				{
					amount = 1,
					resourceType = ResourceType.combatCard,
					gameHex = this.player.character.position
				});
				this.player.combatCards.Sort((CombatCard A, CombatCard B) => A.CombatBonus.CompareTo(B.CombatBonus));
				payResource.SetResources(list2, this.player.combatCards[0]);
			}
			bool flag3 = payResource.CanExecute();
			if (flag3)
			{
				this.gameManager.actionManager.SetPayAction(payResource);
				this.gameManager.actionManager.PrepareNextAction();
			}
			return flag3;
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x00044561 File Offset: 0x00042761
		public void PlayTurn()
		{
			this.PlayAction(this.Bot());
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x0010B1E0 File Offset: 0x001093E0
		public void ContinueLeaverAction()
		{
			if (this.player.currentMatSection == -1)
			{
				this.PlayTurn();
				return;
			}
			AiAction aiAction = this.CreateCurrentAction();
			if (this.player.topActionInProgress)
			{
				if (this.gameManager.combatManager.GetActualStage() != CombatStage.CombatResovled)
				{
					this.ContinueCombat();
					return;
				}
				this.ContinueTopAction(aiAction);
				if (!this.gameManager.combatManager.IsPlayerInCombat())
				{
					this.HandleEncounterAndFactory();
					this.PlayBottomAction(aiAction);
					return;
				}
			}
			else if (this.player.topActionFinished)
			{
				if (this.gameManager.CheckEncounter(false) || this.gameManager.LastEncounterCard != null || this.gameManager.CheckFactory(false))
				{
					this.HandleEncounterAndFactory();
					this.PlayBottomAction(aiAction);
					return;
				}
				if (this.player.bottomActionInProgress)
				{
					if (this.gameManager.combatManager.GetActualStage() != CombatStage.CombatResovled)
					{
						this.ContinueCombat();
						return;
					}
					this.ContinueDownAction(aiAction);
					return;
				}
				else
				{
					if (this.player.downActionFinished)
					{
						if (this.gameManager.CheckEncounter(false) || this.gameManager.LastEncounterCard != null || this.gameManager.CheckFactory(false))
						{
							this.HandleEncounterAndFactory();
						}
						this.TryToCompleteObjective();
						this.InformAboutEndedTurn();
						return;
					}
					this.PlayBottomAction(aiAction);
					return;
				}
			}
			else
			{
				this.PlayTurn();
			}
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x0010B328 File Offset: 0x00109528
		private AiAction CreateCurrentAction()
		{
			int currentMatSection = this.player.currentMatSection;
			MatPlayerSection playerMatSection = this.player.matPlayer.GetPlayerMatSection(currentMatSection);
			int gainActionId = this.gameManager.actionManager.GetGainActionId();
			return new AiAction(currentMatSection, playerMatSection.ActionTop, gainActionId, playerMatSection.ActionDown, this.gameManager);
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x0004456F File Offset: 0x0004276F
		private void ContinueCombat()
		{
			if (this.gameManager.combatManager.CanPerformStep(this.player))
			{
				this.PerformCombatStage(this.gameManager.combatManager.GetActualStage());
			}
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x0004459F File Offset: 0x0004279F
		private void ContinueTopAction(AiAction aiAction)
		{
			this.gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x000445B1 File Offset: 0x000427B1
		private void ContinueDownAction(AiAction aiAction)
		{
			this.gameManager.actionManager.PrepareNextAction();
			this.TryToCompleteObjective();
			this.InformAboutEndedTurn();
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x0010B380 File Offset: 0x00109580
		private void PlayAction(AiRecipe recipe)
		{
			AiAction action = recipe.action;
			this.bottomActionExecuted = false;
			this.gameManager.ChooseSection(action.matSectionId);
			this.PlayTopAction(recipe, action);
			if (!this.gameManager.combatManager.IsPlayerInCombat())
			{
				this.PlayBottomAction(action);
			}
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x0010B3D0 File Offset: 0x001095D0
		private void PlayTopAction(AiRecipe recipe, AiAction action)
		{
			if (!this.forbiddenActions.Contains(action.topAction.GetGainAction(action.gainActionId).GetGainType()))
			{
				this.gameManager.actionManager.SetSectionAction(action.topAction, null, action.gainActionId);
				action.ActionTopExecute(recipe, this);
			}
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x0010B428 File Offset: 0x00109628
		private void PlayBottomAction(AiAction action)
		{
			if (this.player.currentMatSection == -1 || this.gameManager.actionManager.GetLastBonusAction() != null || this.WaitingForCard || this.bottomActionExecuted)
			{
				return;
			}
			if (this.gameManager.GameFinished)
			{
				return;
			}
			if (this.gameManager.combatManager.GetBattlefields().Count == 0 && !this.forbiddenActions.Contains(action.downAction.GetGainAction(0).GetGainType()))
			{
				this.bottomActionExecuted = true;
				this.gameManager.actionManager.SetSectionAction(action.downAction, null, -1);
				action.ActionDownExecute(this);
				this.TryToCompleteObjective();
				this.InformAboutEndedTurn();
			}
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x0010B4DC File Offset: 0x001096DC
		private void TryToCompleteObjective()
		{
			if (this.gameManager.PlayerCurrent.objectiveCards != null)
			{
				if (this.gameManager.PlayerCurrent.objectiveCards.Count > 0 && this.gameManager.PlayerCurrent.objectiveCards[0].status == ObjectiveCard.ObjectiveStatus.Open && this.gameManager.PlayerCurrent.objectiveCards[0].CheckCondition())
				{
					this.gameManager.OnObjectiveCompleted(0);
					return;
				}
				if (this.gameManager.PlayerCurrent.objectiveCards.Count > 1 && this.gameManager.PlayerCurrent.objectiveCards[1].status == ObjectiveCard.ObjectiveStatus.Open && this.gameManager.PlayerCurrent.objectiveCards[1].CheckCondition())
				{
					this.gameManager.OnObjectiveCompleted(1);
				}
			}
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x0010B5BC File Offset: 0x001097BC
		private void InformAboutEndedTurn()
		{
			if (this.gameManager.PlayerCurrent == this.player && this.gameManager.actionManager.GetLastBonusAction() == null && !this.gameManager.GameFinished)
			{
				if (this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner == null)
				{
					this.gameManager.actionManager.BreakSectionAction(false);
				}
				this.gameManager.EndBotTurn();
			}
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x0010B634 File Offset: 0x00109834
		public bool CanPlayTopAction(GainType gain)
		{
			return !this.forbiddenActions.Contains(gain) && !this.player.matFaction.DidPlayerUsedMatLastTurn(this.AiTopActions[gain].matSectionId, this.player.lastMatSection);
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x0010B680 File Offset: 0x00109880
		private bool CanPlayDownAction(AiAction action)
		{
			return !this.forbiddenActions.Contains(action.downAction.GetGainAction(0).GetGainType()) && !this.player.matFaction.DidPlayerUsedMatLastTurn(action.matSectionId, this.player.lastMatSection);
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x0010B6D4 File Offset: 0x001098D4
		public void PerformCombatStage(CombatStage stage)
		{
			switch (stage)
			{
			case CombatStage.SelectingBattlefield:
			{
				List<GameHex> battlefields = this.gameManager.combatManager.GetBattlefields();
				if (battlefields.Count > 0)
				{
					this.gameManager.combatManager.SelectBattlefield(battlefields[0]);
					this.gameManager.combatManager.SwitchToNextStage();
					return;
				}
				break;
			}
			case CombatStage.Diversion:
				if (this.gameManager.combatManager.GetActualPlayer().aiPlayer != this)
				{
					return;
				}
				if (this.gameManager.combatManager.CanUseAbility(this.player, this.gameManager.combatManager.GetSelectedBattlefield()))
				{
					this.UseAbility();
				}
				this.gameManager.combatManager.SwitchToNextStage();
				return;
			case CombatStage.Preparation:
				if (!this.gameManager.combatManager.GetUsedPowers().ContainsKey(this.player))
				{
					this.CombatPreparation(false, null, null);
					this.gameManager.combatManager.SwitchToNextStage();
					return;
				}
				break;
			case CombatStage.DeterminatingTheWinner:
				if (this.gameManager.IsMultiplayer)
				{
					this.gameManager.combatManager.SwitchToNextStage();
					return;
				}
				if (!this.gameManager.IsMultiplayer && !this.gameManager.combatManager.GetAttacker().IsHuman && !this.gameManager.combatManager.GetDefender().IsHuman)
				{
					this.gameManager.combatManager.SwitchToNextStage();
					return;
				}
				break;
			case CombatStage.EndingTheBattle:
			{
				Player player = this.player;
				if (this.gameManager.combatManager.CanPlayerGetCombatCard(player))
				{
					this.gameManager.combatManager.AddCombatCard(true);
				}
				if (this.gameManager.combatManager.GetDefeated() == player)
				{
					this.WithdrawUnits();
				}
				if (this.gameManager.combatManager.CanUseAfterBattleAbility(player))
				{
					this.UsePolaniaAbility();
					this.UseTogawaAbility();
				}
				this.gameManager.combatManager.SwitchToNextStage();
				return;
			}
			case CombatStage.CombatResovled:
				if (this.gameManager.PlayerCurrent.aiPlayer == this)
				{
					if (this.gameManager.combatManager.GetBattlefields().Count > 0)
					{
						this.gameManager.combatManager.SwitchToNextStage();
						return;
					}
					if (this.player.currentMatSection != -1 && this.gameManager.actionManager.GetLastBonusAction() == null && !this.player.bottomActionInProgress)
					{
						this.gameManager.actionManager.PrepareNextAction();
						this.gameManager.moveManager.Clear();
						this.HandleEncounterAndFactory();
						if (!this.WaitingForCard)
						{
							int currentMatSection = this.player.currentMatSection;
							MatPlayerSection playerMatSection = this.player.matPlayer.GetPlayerMatSection(currentMatSection);
							AiAction aiAction = new AiAction(currentMatSection, playerMatSection.ActionTop, 0, playerMatSection.ActionDown, this.gameManager);
							this.PlayBottomAction(aiAction);
							return;
						}
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x0010B9A0 File Offset: 0x00109BA0
		public void UseAbility()
		{
			if (this.player.matFaction.faction == Faction.Albion && this.player.matFaction.SkillUnlocked[1] && this.player == this.gameManager.combatManager.GetAttacker())
			{
				this.gameManager.combatManager.BolsterBeforeCombat();
				if (this.gameManager.combatManager.GetEnemyOf(this.player).IsHuman)
				{
					AiPlayer.CombatAbilityUsedDelegate combatAbilityUsed = AiPlayer.CombatAbilityUsed;
					if (combatAbilityUsed != null)
					{
						combatAbilityUsed(this.player.matFaction.abilities[1]);
					}
				}
			}
			AbilityPerk abilityPerk = this.player.matFaction.abilities[2];
			if (abilityPerk <= AbilityPerk.Scout)
			{
				if (abilityPerk != AbilityPerk.Artillery)
				{
					if (abilityPerk != AbilityPerk.Scout)
					{
						return;
					}
					Player enemyOf = this.gameManager.combatManager.GetEnemyOf(this.player);
					if (enemyOf.combatCards.Count > 0)
					{
						int num = this.gameManager.random.Next(0, enemyOf.combatCards.Count);
						CombatCard combatCard = enemyOf.combatCards[num];
						this.gameManager.combatManager.StealCombatCardFromEnemy(combatCard);
						if (this.gameManager.combatManager.GetEnemyOf(this.player).IsHuman)
						{
							AiPlayer.CombatAbilityUsedDelegate combatAbilityUsed2 = AiPlayer.CombatAbilityUsed;
							if (combatAbilityUsed2 == null)
							{
								return;
							}
							combatAbilityUsed2(this.player.matFaction.abilities[2]);
							return;
						}
					}
				}
				else if (this.player.Power >= 1)
				{
					this.gameManager.combatManager.RemovePowerFromEnemy(1);
					if (this.gameManager.combatManager.GetEnemyOf(this.player).IsHuman)
					{
						AiPlayer.CombatAbilityUsedDelegate combatAbilityUsed3 = AiPlayer.CombatAbilityUsed;
						if (combatAbilityUsed3 == null)
						{
							return;
						}
						combatAbilityUsed3(this.player.matFaction.abilities[2]);
						return;
					}
				}
			}
			else if (abilityPerk != AbilityPerk.Disarm)
			{
				if (abilityPerk != AbilityPerk.Shield)
				{
					if (abilityPerk != AbilityPerk.Ronin)
					{
						return;
					}
					if (this.gameManager.combatManager.GetSelectedBattlefield().GetPlayerUnitCount(this.player) == 1)
					{
						this.gameManager.combatManager.BolsterBeforeCombat();
						if (this.gameManager.combatManager.GetEnemyOf(this.player).IsHuman)
						{
							AiPlayer.CombatAbilityUsedDelegate combatAbilityUsed4 = AiPlayer.CombatAbilityUsed;
							if (combatAbilityUsed4 == null)
							{
								return;
							}
							combatAbilityUsed4(this.player.matFaction.abilities[2]);
							return;
						}
					}
				}
				else if (this.player == this.gameManager.combatManager.GetDefender())
				{
					this.gameManager.combatManager.RemovePowerFromEnemy(0);
					if (this.gameManager.combatManager.GetEnemyOf(this.player).IsHuman)
					{
						AiPlayer.CombatAbilityUsedDelegate combatAbilityUsed5 = AiPlayer.CombatAbilityUsed;
						if (combatAbilityUsed5 == null)
						{
							return;
						}
						combatAbilityUsed5(this.player.matFaction.abilities[2]);
						return;
					}
				}
			}
			else
			{
				this.gameManager.combatManager.RemovePowerFromEnemy(0);
				if (this.gameManager.combatManager.GetEnemyOf(this.player).IsHuman)
				{
					AiPlayer.CombatAbilityUsedDelegate combatAbilityUsed6 = AiPlayer.CombatAbilityUsed;
					if (combatAbilityUsed6 == null)
					{
						return;
					}
					combatAbilityUsed6(this.player.matFaction.abilities[2]);
				}
			}
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x0010BCE4 File Offset: 0x00109EE4
		public bool CombatPreparation(bool simulate = false, GameHex battlefield = null, Player opponent = null)
		{
			if (this.player.matFaction.faction == Faction.Saxony || this.player.matFaction.faction == Faction.Polania || this.player.matFaction.faction == Faction.Nordic || this.player.matFaction.faction == Faction.Rusviet || this.player.matFaction.faction == Faction.Albion || this.player.matFaction.faction == Faction.Togawa)
			{
				if (opponent == null)
				{
					opponent = this.gameManager.combatManager.GetEnemyOf(this.player);
				}
				bool flag = opponent.combatCards.Count > 0;
				int power = opponent.Power;
				if (battlefield == null)
				{
					battlefield = this.gameManager.combatManager.GetSelectedBattlefield();
				}
				int num;
				int num2;
				if (battlefield.Owner == this.player)
				{
					num = battlefield.GetOwnerMechs().Count + (battlefield.HasOwnerCharacter() ? 1 : 0);
					num2 = battlefield.GetEnemyMechs().Count + (battlefield.HasEnemyCharacter() ? 1 : 0);
					if (this.player.matFaction.abilities.Contains(AbilityPerk.PeoplesArmy) && this.player.matFaction.SkillUnlocked[2] && battlefield.GetOwnerWorkers().Count > 0)
					{
						num++;
					}
					if (opponent.matFaction.abilities.Contains(AbilityPerk.PeoplesArmy) && opponent.matFaction.SkillUnlocked[2] && battlefield.GetEnemyWorkers().Count > 0)
					{
						num2++;
					}
				}
				else
				{
					num = battlefield.GetEnemyMechs().Count + (battlefield.HasEnemyCharacter() ? 1 : 0);
					num2 = battlefield.GetOwnerMechs().Count + (battlefield.HasOwnerCharacter() ? 1 : 0);
					if (this.player.matFaction.abilities.Contains(AbilityPerk.PeoplesArmy) && this.player.matFaction.SkillUnlocked[2] && battlefield.GetEnemyWorkers().Count > 0)
					{
						num++;
					}
					if (opponent.matFaction.abilities.Contains(AbilityPerk.PeoplesArmy) && opponent.matFaction.SkillUnlocked[2] && battlefield.GetOwnerWorkers().Count > 0)
					{
						num2++;
					}
				}
				int num3 = num;
				if (num3 > this.player.combatCards.Count)
				{
					num3 = this.player.combatCards.Count;
				}
				int num4 = num2;
				if (num4 > opponent.combatCards.Count)
				{
					num4 = opponent.combatCards.Count;
				}
				this.player.combatCards.Sort((CombatCard A, CombatCard B) => B.CombatBonus.CompareTo(A.CombatBonus));
				int num5 = this.player.Power;
				int num6 = opponent.Power;
				for (int i = 0; i < num3; i++)
				{
					num5 += this.player.combatCards[i].CombatBonus;
				}
				for (int j = 0; j < num4; j++)
				{
					num6 += 5;
				}
				bool flag2 = true;
				if (Math.Min(this.player.Power, 7) >= Math.Min(power, 7) && (!flag || this.player.combatCards.Count != 0))
				{
					flag2 = false;
				}
				else if (num5 >= num6)
				{
					flag2 = false;
				}
				if (flag2)
				{
					int num7 = 0;
					if (this.player.Power > 0)
					{
						num7 = 1;
					}
					List<CombatCard> list = new List<CombatCard>();
					if (num7 == 0 && this.player.combatCards.Count > 0 && this.player.combatCards[this.player.combatCards.Count - 1].CombatBonus == 2)
					{
						list.Add(this.player.combatCards[this.player.combatCards.Count - 1]);
					}
					int num8 = 0;
					foreach (CombatCard combatCard in list)
					{
						num8 += combatCard.CombatBonus;
					}
					if (!simulate)
					{
						this.gameManager.combatManager.AddPlayerPowerInBattle(this.player, new PowerSelected
						{
							selectedPower = num7,
							cardsPower = num8,
							selectedCards = list
						});
					}
				}
				else
				{
					int num9 = 0;
					List<CombatCard> list2 = new List<CombatCard>();
					if (this.player.combatCards.Count > 0)
					{
						this.player.combatCards.Sort((CombatCard A, CombatCard B) => B.CombatBonus.CompareTo(A.CombatBonus));
						for (int k = 0; k < num3; k++)
						{
							if (num9 < num6)
							{
								num9 += this.player.combatCards[k].CombatBonus;
								list2.Add(this.player.combatCards[k]);
							}
						}
					}
					int num10 = Math.Max(num6 - num9, 0);
					if (num10 > 7)
					{
						num10 = 7;
					}
					if (num10 > this.player.Power)
					{
						num10 = this.player.Power;
					}
					if (!simulate)
					{
						this.gameManager.combatManager.AddPlayerPowerInBattle(this.player, new PowerSelected
						{
							selectedPower = num10,
							cardsPower = num9,
							selectedCards = list2
						});
					}
				}
				return !flag2;
			}
			int num11 = this.player.Power;
			if (num11 > 7)
			{
				num11 = 7;
			}
			List<CombatCard> list3 = new List<CombatCard>();
			if (this.player.combatCards.Count > 0)
			{
				list3.Add(this.player.combatCards[0]);
			}
			int num12 = 0;
			foreach (CombatCard combatCard2 in list3)
			{
				num12 += combatCard2.CombatBonus;
			}
			if (!simulate)
			{
				this.gameManager.combatManager.AddPlayerPowerInBattle(this.player, new PowerSelected
				{
					selectedPower = num11,
					cardsPower = num12,
					selectedCards = list3
				});
			}
			return true;
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x0010C318 File Offset: 0x0010A518
		public void WithdrawUnits()
		{
			GameHex gameHex = this.gameManager.combatManager.GetWithdrawPositions()[0];
			List<Unit> unitsToWithdraw = this.gameManager.combatManager.GetUnitsToWithdraw();
			bool flag = false;
			using (List<Unit>.Enumerator enumerator = unitsToWithdraw.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is Mech)
					{
						flag = true;
					}
				}
			}
			if (this.player.aiDifficulty == AIDifficulty.Hard)
			{
				foreach (GameHex gameHex2 in this.gameManager.combatManager.GetWithdrawPositions())
				{
					if (gameHex2.hexType != HexType.capital)
					{
						gameHex = gameHex2;
					}
				}
			}
			if (gameHex.hexType == HexType.lake && !flag)
			{
				List<Unit> list = new List<Unit>();
				List<Unit> list2 = new List<Unit>();
				foreach (Unit unit in unitsToWithdraw)
				{
					if (unit is Worker)
					{
						list.Add(unit);
					}
					else
					{
						list2.Add(unit);
					}
				}
				if (list2.Count > 0)
				{
					this.gameManager.combatManager.WithdrawToPosition(list2, gameHex);
				}
				if (list.Count > 0)
				{
					this.gameManager.combatManager.WithdrawToPosition(list, list[0].Owner.GetCapital());
				}
			}
			else
			{
				this.gameManager.combatManager.WithdrawToPosition(unitsToWithdraw, gameHex);
			}
			if (this.gameManager.combatManager.CanUseAfterBattleAbility(this.player) && this.gameManager.IsMultiplayer)
			{
				this.gameManager.OnActionSent(new EndNordicSkillMessage((int)this.player.matFaction.faction));
			}
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x0010C514 File Offset: 0x0010A714
		public void UseTogawaAbility()
		{
			if (this.player.matFaction.abilities.Contains(AbilityPerk.Shinobi) && this.player.matFaction.SkillUnlocked[3] && this.gameManager.tokenManager.CanRearmTrap(this.gameManager.combatManager.GetSelectedBattlefield(), this.player.matFaction.faction))
			{
				this.gameManager.tokenManager.ArmTrap(this.gameManager.combatManager.GetSelectedBattlefield());
			}
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000445CF File Offset: 0x000427CF
		public void UsePolaniaAbility()
		{
			if (this.player.matFaction.abilities.Contains(AbilityPerk.Camaraderie) && this.player.matFaction.SkillUnlocked[2])
			{
				this.gameManager.combatManager.PolaniaCamaraderieChoice(true);
			}
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x0010C5A4 File Offset: 0x0010A7A4
		public void HandleEncounterAndFactory()
		{
			if (this.player.aiDifficulty == AIDifficulty.Hard)
			{
				this.HandleEncounterAndFactoryAdvanced();
			}
			else
			{
				this.HandleEncounterAndFactoryBasic();
			}
			if (this.gameManager.tokenManager.CanPlaceFlag(this.player.character.position, this.player.character) && this.gameManager.tokenManager.HexCondition(this.player.character))
			{
				bool flag = false;
				foreach (GameHex gameHex in this.player.character.position.GetNeighboursAll())
				{
					if (gameHex.hexType == HexType.capital && gameHex.factionBase == Faction.Albion)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					this.gameManager.tokenManager.PlaceToken(this.player.character.position);
				}
			}
			if (this.gameManager.tokenManager.CanPlaceTrap(this.player.character.position, this.player.character) && this.gameManager.tokenManager.HexCondition(this.player.character))
			{
				this.gameManager.tokenManager.PlaceToken(this.player.character.position);
			}
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x0010C710 File Offset: 0x0010A910
		private void HandleEncounterAndFactoryBasic()
		{
			if (this.gameManager.CheckEncounter(false) || this.gameManager.LastEncounterCard != null)
			{
				EncounterCard encounterCard;
				if (this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null)
				{
					if (!this.WaitingForCard)
					{
						this.gameManager.OnEncounter();
						this.WaitingForCard = true;
						return;
					}
					this.WaitingForCard = false;
					encounterCard = this.gameManager.LastEncounterCard;
				}
				else
				{
					encounterCard = this.gameManager.GetEncounterCard();
				}
				SectionAction sectionAction = encounterCard.GetAction(0);
				for (int i = 0; i < sectionAction.gainActionsCount; i++)
				{
					GainAction gainAction = sectionAction.GetGainAction(i);
					if (gainAction is GainResource)
					{
						((GainResource)gainAction).SetDestinationAmount(this.player.character.position, gainAction.Amount);
					}
				}
				if (this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner == null)
				{
					this.gameManager.OnActionSent(new EncounterCardMessage(encounterCard.CardId.ToString(), (int)this.player.matFaction.faction));
				}
				this.gameManager.ChooseEncounterOption(0);
				this.gameManager.actionManager.SetSectionAction(sectionAction, null, 0);
				if (this.player.matFaction.factionPerk == AbilityPerk.Meander && (!this.gameManager.IsMultiplayer || this.gameManager.PlayerOwner == null))
				{
					int num = -1;
					int[] array = new int[]
					{
						AiAction.FindResourceGain(encounterCard.GetAction(1)),
						AiAction.FindResourceGain(encounterCard.GetAction(2))
					};
					if (array[0] >= 0)
					{
						num = 0;
					}
					if (array[1] >= 0)
					{
						num = 1;
					}
					if (this.player.matPlayer.matType == PlayerMatType.Patriotic)
					{
						num = -1;
					}
					if (num >= 0 && this.player.matPlayer.matType == PlayerMatType.Industrial && encounterCard.GetAction(num + 1).GetPayAction(0).GetPayType() == PayType.Popularity)
					{
						num = -1;
					}
					if (num >= 0)
					{
						sectionAction = encounterCard.GetAction(num + 1);
						if (this.CanGetResourcesFromEncounter(sectionAction, array[num]))
						{
							this.gameManager.ChooseEncounterOption(num + 1);
							this.gameManager.actionManager.SetSectionAction(encounterCard.GetAction(num + 1), null, 0);
							for (int j = 0; j < sectionAction.gainActionsCount; j++)
							{
								GainAction gainAction2 = sectionAction.GetGainAction(j);
								if (gainAction2.GainAvaliable() && !this.player.automaticGain.ContainsKey(gainAction2.GetGainType()))
								{
									this.PrepareEncounterAction(gainAction2);
									this.gameManager.actionManager.PrepareNextAction();
								}
							}
						}
					}
				}
				this.player.character.position.encounterUsed = true;
				this.player.character.position.encounterTaken = true;
				this.gameManager.ClearLastEncounterCard();
			}
			if (this.player.character.position.hexType == HexType.factory && this.player.matPlayer.matPlayerSectionsCount == 4 && this.gameManager.combatManager.GetBattlefields().Count == 0)
			{
				if (this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null)
				{
					if (!this.WaitingForCard)
					{
						this.gameManager.OnGainFactoryCards();
						this.WaitingForCard = true;
						return;
					}
					this.WaitingForCard = false;
				}
				if (this.gameManager.IsMultiplayer)
				{
					List<FactoryCard> factoryCards = this.gameManager.GetFactoryCards();
					if (this.gameManager.PlayerOwner == null)
					{
						this.gameManager.OnActionSent(new EmptyCardsMessage(factoryCards.Count));
						this.gameManager.OnActionSent(new FactoryMessage(factoryCards[0].CardId.ToString(), (int)this.player.matFaction.faction, 0));
					}
					else
					{
						this.gameManager.OnActionSent(new GetFactoryMessage(0));
					}
				}
				this.gameManager.AddFactoryCard(0);
			}
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x0010CAF8 File Offset: 0x0010ACF8
		private bool CanGetResourcesFromEncounter(SectionAction section, int gainId)
		{
			GainAction gainAction = section.GetGainAction(gainId);
			if (gainAction is GainResource)
			{
				return this.strategicAnalysis.resourceDemandPriority[((GainResource)gainAction).ResourceToGain] > 0 && section.CanPlayerPayActions();
			}
			return gainAction is GainAnyResource && this.strategicAnalysis.resourceHighestPriority != ResourceType.combatCard && section.CanPlayerPayActions();
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x0010CB5C File Offset: 0x0010AD5C
		private void PrepareEncounterAction(GainAction action)
		{
			GainType gainType = action.GetGainType();
			if (gainType == GainType.Coin)
			{
				((GainCoin)action).SetCoins(action.Amount);
				return;
			}
			switch (gainType)
			{
			case GainType.AnyResource:
				((GainAnyResource)action).AddResourceToField(this.strategicAnalysis.resourceHighestPriority, this.player.character.position, (int)action.Amount);
				return;
			case GainType.Resource:
				((GainResource)action).SetDestinationAmount(this.player.character.position, action.Amount);
				return;
			case GainType.Move:
			case GainType.Mech:
				return;
			case GainType.Upgrade:
				this.AiActions[0].SetUpgradeActions(this, (GainUpgrade)action);
				return;
			case GainType.Worker:
				((GainWorker)action).SetLocationAndWorkersAmount(this.player.character.position, 0);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x0010CC34 File Offset: 0x0010AE34
		private void HandleEncounterAndFactoryAdvanced()
		{
			if (this.gameManager.CheckEncounter(false) || this.gameManager.LastEncounterCard != null)
			{
				EncounterCard encounterCard;
				if (this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null)
				{
					if (!this.WaitingForCard)
					{
						this.gameManager.OnEncounter();
						this.WaitingForCard = true;
						return;
					}
					this.WaitingForCard = false;
					encounterCard = this.gameManager.LastEncounterCard;
				}
				else
				{
					encounterCard = this.gameManager.GetEncounterCard();
				}
				this.gameManager.OnActionSent(new EncounterCardMessage(encounterCard.CardId.ToString(), (int)this.player.matFaction.faction));
				int[] array = new int[]
				{
					AiAction.FindResourceGain(encounterCard.GetAction(1)),
					AiAction.FindResourceGain(encounterCard.GetAction(2))
				};
				SectionAction sectionAction = encounterCard.GetAction(1);
				if (this.player.matFaction.faction == Faction.Saxony && this.player.matPlayer.matType == PlayerMatType.Industrial && array[0] >= 0 && this.CanGetResourcesFromEncounter(sectionAction, array[0]))
				{
					this.gameManager.ChooseEncounterOption(1);
					this.gameManager.actionManager.SetSectionAction(encounterCard.GetAction(1), null, array[0]);
					for (int i = 0; i < sectionAction.gainActionsCount; i++)
					{
						GainAction gainAction = sectionAction.GetGainAction(i);
						if (gainAction.GainAvaliable() && !this.player.automaticGain.ContainsKey(gainAction.GetGainType()))
						{
							this.PrepareEncounterAction(gainAction);
							this.gameManager.actionManager.PrepareNextAction();
						}
					}
				}
				else if (encounterCard.GetAction(1).CanPlayerPayActions() && encounterCard.GetAction(1).gainActionsCount == 1 && encounterCard.GetAction(1).GetGainAction(0).GetGainType() == GainType.Popularity && this.player.matFaction.faction == Faction.Saxony && this.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					this.gameManager.ChooseEncounterOption(1);
					this.gameManager.actionManager.SetSectionAction(sectionAction, null, 0);
				}
				else
				{
					sectionAction = encounterCard.GetAction(0);
					for (int j = 0; j < sectionAction.gainActionsCount; j++)
					{
						GainAction gainAction2 = sectionAction.GetGainAction(j);
						if (gainAction2 is GainResource)
						{
							((GainResource)gainAction2).SetDestinationAmount(this.player.character.position, gainAction2.Amount);
						}
					}
					this.gameManager.ChooseEncounterOption(0);
					this.gameManager.actionManager.SetSectionAction(sectionAction, null, 0);
				}
				if (this.player.matFaction.factionPerk == AbilityPerk.Meander && (!this.gameManager.IsMultiplayer || this.gameManager.PlayerOwner == null))
				{
					int num = -1;
					if (array[0] >= 0)
					{
						num = 0;
					}
					if (array[1] >= 0 && (num != 0 || this.player.matPlayer.matType == PlayerMatType.Agricultural))
					{
						num = 1;
					}
					if (this.player.matPlayer.matType == PlayerMatType.Patriotic)
					{
						num = -1;
					}
					if (num >= 0 && this.player.matPlayer.matType == PlayerMatType.Industrial && encounterCard.GetAction(num + 1).GetPayAction(0).GetPayType() == PayType.Popularity)
					{
						num = -1;
					}
					if (num >= 0)
					{
						sectionAction = encounterCard.GetAction(num + 1);
						if (this.CanGetResourcesFromEncounter(sectionAction, array[num]))
						{
							this.gameManager.ChooseEncounterOption(num + 1);
							this.gameManager.actionManager.SetSectionAction(encounterCard.GetAction(num + 1), null, 0);
							for (int k = 0; k < sectionAction.gainActionsCount; k++)
							{
								GainAction gainAction3 = sectionAction.GetGainAction(k);
								if (gainAction3.GainAvaliable() && !this.player.automaticGain.ContainsKey(gainAction3.GetGainType()))
								{
									this.PrepareEncounterAction(gainAction3);
									this.gameManager.actionManager.PrepareNextAction();
								}
							}
						}
					}
				}
				this.player.character.position.encounterUsed = true;
				this.player.character.position.encounterTaken = true;
				this.gameManager.ClearLastEncounterCard();
			}
			if (this.player.character.position.hexType == HexType.factory && this.player.matPlayer.matPlayerSectionsCount == 4 && this.gameManager.combatManager.GetBattlefields().Count == 0)
			{
				if (this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null)
				{
					if (!this.WaitingForCard)
					{
						this.gameManager.OnGainFactoryCards();
						this.WaitingForCard = true;
						return;
					}
					this.WaitingForCard = false;
				}
				if (this.gameManager.IsMultiplayer)
				{
					List<FactoryCard> factoryCards = this.gameManager.GetFactoryCards();
					if (this.gameManager.PlayerOwner == null)
					{
						this.gameManager.OnActionSent(new EmptyCardsMessage(factoryCards.Count));
						this.gameManager.OnActionSent(new FactoryMessage(factoryCards[0].CardId.ToString(), (int)this.player.matFaction.faction, 0));
					}
					else
					{
						this.gameManager.OnActionSent(new GetFactoryMessage(0));
					}
				}
				this.gameManager.AddFactoryCard(0);
			}
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x0004460F File Offset: 0x0004280F
		private void StarPopularity(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.player.GetNumberOfStars(StarType.Popularity) == 0 && this.CanPlayTopAction(GainType.Popularity))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Popularity], "Star Popularity"));
			}
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x00044645 File Offset: 0x00042845
		private void StarPower(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.player.GetNumberOfStars(StarType.Power) == 0 && this.CanPlayTopAction(GainType.Power))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Power], "Star Power"));
			}
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x0004467B File Offset: 0x0004287B
		private void PowerIfWeak(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.player.Power == 0 && this.CanPlayTopAction(GainType.Power))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Power], "No Power -> Bolster"));
			}
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x0010D164 File Offset: 0x0010B364
		private void TradeForResource(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.CanPlayTopAction(GainType.AnyResource) && this.strategicAnalysis.resourceHighestPriority != ResourceType.combatCard && this.strategicAnalysis.workersOutOfBase > 0)
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.AnyResource], "Trade for resource"));
			}
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x0010D1B4 File Offset: 0x0010B3B4
		private void TradeForBottomAction(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.CanPlayTopAction(GainType.AnyResource) && this.strategicAnalysis.resourceHighestPriority != ResourceType.combatCard && this.player.Coins > 0 && ((this.strategicAnalysis.resourceHighestPriority == ResourceType.food && this.AiTopActions[GainType.AnyResource].downAction.GetGainAction(0) is GainRecruit && this.AiTopActions[GainType.AnyResource].downAction.GetGainAction(0).GainAvaliable() && (int)this.AiTopActions[GainType.AnyResource].downAction.GetPayAction(0).Amount - this.player.Resources(false)[ResourceType.food] <= 2) || (this.strategicAnalysis.resourceHighestPriority == ResourceType.metal && this.AiTopActions[GainType.AnyResource].downAction.GetGainAction(0) is GainMech && this.AiTopActions[GainType.AnyResource].downAction.GetGainAction(0).GainAvaliable() && (int)this.AiTopActions[GainType.AnyResource].downAction.GetPayAction(0).Amount - this.player.Resources(false)[ResourceType.metal] <= 2) || (this.strategicAnalysis.resourceHighestPriority == ResourceType.oil && this.AiTopActions[GainType.AnyResource].downAction.GetGainAction(0) is GainUpgrade && this.AiTopActions[GainType.AnyResource].downAction.GetGainAction(0).GainAvaliable() && (int)this.AiTopActions[GainType.AnyResource].downAction.GetPayAction(0).Amount - this.player.Resources(false)[ResourceType.oil] <= 2) || (this.strategicAnalysis.resourceHighestPriority == ResourceType.wood && this.AiTopActions[GainType.AnyResource].downAction.GetGainAction(0) is GainBuilding && this.AiTopActions[GainType.AnyResource].downAction.GetGainAction(0).GainAvaliable() && (int)this.AiTopActions[GainType.AnyResource].downAction.GetPayAction(0).Amount - this.player.Resources(false)[ResourceType.wood] <= 2)))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.AnyResource], "Trade for bottom stuff"));
			}
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x000446B0 File Offset: 0x000428B0
		private void CoinIfPoor(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.player.Coins == 0 && this.CanPlayTopAction(GainType.Coin))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Coin], "No cash -> get Coin"));
			}
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x000446E5 File Offset: 0x000428E5
		private void MoveToBuild(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.player.aiDifficulty == AIDifficulty.Hard)
			{
				this.MoveToBuildAdvanced(actionOptions, priority);
				return;
			}
			this.MoveToBuildBasic(actionOptions, priority);
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x0010D404 File Offset: 0x0010B604
		private void MoveToBuildBasic(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (!this.strategicAnalysis.canBuild && this.player.GetNumberOfStars(StarType.Structures) == 0 && this.AiActions[this.gainBuildingActionPosition[0]].downAction.CanPlayerPayActions() && this.CanPlayTopAction(GainType.Move))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Move], "Move workers to build")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)this.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.MoveWorkerToBuild(gainMove, this.player.matPlayer.workers[0]);
						this.MoveWorkerToBuild(gainMove, this.player.matPlayer.workers[1]);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
			}
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x00044706 File Offset: 0x00042906
		private void MoveByAnalysisPriority(SortedList<int, AiRecipe> actionOptions)
		{
			if (this.CanPlayTopAction(GainType.Move))
			{
				actionOptions.Add(this.strategicAnalysis.movePriorityHighest, new AiRecipe(this.AiTopActions[GainType.Move], "Move by priority"));
			}
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x0010D488 File Offset: 0x0010B688
		public ResourceType TradeResourceType()
		{
			ResourceType resourceType = this.strategicAnalysis.resourceHighestPriority;
			if (this.strategicAnalysis.resourceHighestPriorityNoProduce != ResourceType.combatCard)
			{
				resourceType = this.strategicAnalysis.resourceHighestPriorityNoProduce;
			}
			return resourceType;
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x0010D4BC File Offset: 0x0010B6BC
		private int SelectTopActionFlavor(List<int> options)
		{
			if (options.Count == 1)
			{
				return options[0];
			}
			if (this.AiActions[options[0]].topAction.Type == TopActionType.Bolster && this.player.matFaction.faction == Faction.Saxony && this.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				int num = options[0];
				int num2 = options[1];
				if (this.player.Power < 1)
				{
					return num;
				}
				if (this.player.combatCards.Count < 2)
				{
					return num2;
				}
			}
			if (this.AiActions[options[0]].topAction.Type == TopActionType.Trade && this.player.matFaction.faction == Faction.Saxony && this.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				int num3 = options[0];
				int num4 = options[1];
				if (this.player.Popularity == 18)
				{
					return num3;
				}
				if (this.TradeResourceType() == ResourceType.combatCard)
				{
					return num4;
				}
			}
			return options[0];
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x0010D5CC File Offset: 0x0010B7CC
		private void StarConstructions(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.strategicAnalysis.canBuild && this.CanPlayDownAction(this.AiActions[this.gainBuildingActionPosition[0]]))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainBuildingActionPosition)], "Star Constructions"));
			}
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x0010D630 File Offset: 0x0010B830
		private void StarMechs(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.strategicAnalysis.canDeploy && this.CanPlayDownAction(this.AiActions[this.gainMechActionPosition[0]]))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainMechActionPosition)], "Star Mechs"));
			}
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x0010D694 File Offset: 0x0010B894
		private void StarRecruits(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.strategicAnalysis.canEnlist && this.CanPlayDownAction(this.AiActions[this.gainRecruitActionPosition[0]]))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainRecruitActionPosition)], "Star Recruits"));
			}
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x0010D6F8 File Offset: 0x0010B8F8
		private void StarUpgrades(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.strategicAnalysis.canUpgrade && this.CanPlayDownAction(this.AiActions[this.gainUpgradeActionPosition[0]]))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainUpgradeActionPosition)], "Star Upgrades"));
			}
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x0010D75C File Offset: 0x0010B95C
		private void TradeLoop(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.strategicAnalysis.tradeLoopPresent && this.CanPlayTopAction(GainType.AnyResource) && this.AiTopActions[GainType.AnyResource].topAction.CanPlayerPayActions())
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.AnyResource], "Trade Loop")
				{
					tradeResource = new ResourceType[]
					{
						this.strategicAnalysis.tradeLoopResource,
						this.strategicAnalysis.tradeLoopResource
					}
				});
			}
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x0010D7E0 File Offset: 0x0010B9E0
		private void ProduceLoop(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.strategicAnalysis.produceLoopPresent && this.CanPlayTopAction(GainType.Produce) && this.AiTopActions[GainType.Produce].topAction.CanPlayerPayActions())
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Produce], "Produce Loop"));
			}
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x0010D838 File Offset: 0x0010BA38
		private void RandomAllowed(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			int num = this.gameManager.random.Next(0, this.AiActions.Count - 1);
			while (this.player.matFaction.DidPlayerUsedMatLastTurn(this.AiActions[num].matSectionId, this.player.lastMatSection))
			{
				num = this.gameManager.random.Next(0, this.AiActions.Count - 1);
			}
			actionOptions.Add(priority, new AiRecipe(this.AiActions[num], "Random allowed"));
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x0010D8D0 File Offset: 0x0010BAD0
		private void ProduceForDemand(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.CanPlayTopAction(GainType.Produce))
			{
				int num = 0;
				bool flag = false;
				for (int i = 0; i < this.player.matPlayer.workers.Count; i++)
				{
					if (this.player.matPlayer.workers[i].position != null && ((this.player.matPlayer.workers[i].position.hexType == HexType.forest && this.strategicAnalysis.resourceDemandTotal[ResourceType.wood] > 0) || (this.player.matPlayer.workers[i].position.hexType == HexType.farm && this.strategicAnalysis.resourceDemandTotal[ResourceType.food] > 0) || (this.player.matPlayer.workers[i].position.hexType == HexType.mountain && this.strategicAnalysis.resourceDemandTotal[ResourceType.metal] > 0) || (this.player.matPlayer.workers[i].position.hexType == HexType.tundra && this.strategicAnalysis.resourceDemandTotal[ResourceType.oil] > 0)))
					{
						num++;
					}
				}
				if (this.player.matPlayer.workers.Count < this.strategicAnalysis.workerCountTarget && this.strategicAnalysis.workersInaVillage > 0)
				{
					num++;
					flag = true;
				}
				if (num >= 2)
				{
					int num2 = priority;
					if (flag && this.player.matPlayer.workers.Count < 5)
					{
						num2 = priority + 75;
					}
					actionOptions.Add(num2, new AiRecipe(this.AiTopActions[GainType.Produce], "Produce for demand"));
				}
			}
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x0010DA90 File Offset: 0x0010BC90
		private void MovePreparation(SortedList<int, AiRecipe> actionOptions)
		{
			AiAction action = actionOptions.Values[0].action;
			if (action.topAction.Type == TopActionType.MoveGain && this.player.stars[StarType.Structures] == 0 && !action.downAction.CanPlayerPayActions() && this.CanPlayTopAction(GainType.AnyResource))
			{
				int amount = (int)action.downAction.GetPayAction(0).Amount;
				ResourceType resourceType = ResourceType.combatCard;
				switch (action.downAction.Type)
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
				if (amount - this.player.Resources(false)[resourceType] <= 2)
				{
					actionOptions.Add(this.MovePreparationPriority(), new AiRecipe(this.AiTopActions[GainType.AnyResource], "Move preparation")
					{
						tradeResource = new ResourceType[] { resourceType, resourceType }
					});
				}
			}
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x0010DB8C File Offset: 0x0010BD8C
		private void PurgeActions(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < actionOptions.Count; i++)
			{
				AiAction action = actionOptions.Values[i].action;
				if ((action == player.AiTopActions[GainType.Produce] || action == player.AiTopActions[GainType.AnyResource]) && this.strategicAnalysis.workersOutOfBase == 0)
				{
					list.Add(i);
				}
			}
			for (int j = list.Count - 1; j >= 0; j--)
			{
				actionOptions.RemoveAt(list[j]);
			}
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x0010DC14 File Offset: 0x0010BE14
		protected void StrategicAnalysisRun()
		{
			int num = 500;
			if (this.player.matFaction.faction == Faction.Saxony)
			{
				num = 650;
			}
			else if (this.player.stars[StarType.Combat] >= 2)
			{
				num = 250;
			}
			if (this.player.stars[StarType.Power] > 0)
			{
				if (this.player.matFaction.faction == Faction.Saxony)
				{
					num += 100;
				}
				else if (this.player.stars[StarType.Combat] < 2)
				{
					num += 100;
				}
			}
			switch (this.player.matFaction.faction)
			{
			case Faction.Polania:
				this.strategicAnalysis.Run(this, 130, 134, 158, num, 145);
				return;
			case Faction.Nordic:
				this.strategicAnalysis.Run(this, 102, 134, 104, num, 145);
				return;
			case Faction.Rusviet:
				this.strategicAnalysis.Run(this, 130, 134, 104, num, 145);
				return;
			case Faction.Crimea:
				this.strategicAnalysis.Run(this, 102, 134, 104, num, 145);
				return;
			case Faction.Saxony:
				this.strategicAnalysis.Run(this, 130, 134, 104, num, 145);
				return;
			}
			this.strategicAnalysis.Run(this, 130, 130, 104, num, 145);
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x0010DD94 File Offset: 0x0010BF94
		public virtual AiRecipe Bot()
		{
			SortedList<int, AiRecipe> sortedList = new SortedList<int, AiRecipe>(new InvertedComparer());
			int num = 0;
			foreach (StarType starType in this.player.stars.Keys)
			{
				num += this.player.stars[starType];
			}
			if (num == 5)
			{
				if (this.player.stars[StarType.Popularity] == 0 && this.player.GetNumberOfStars(StarType.Popularity) == 0 && this.CanPlayTopAction(GainType.Popularity))
				{
					sortedList.Add(10000, new AiRecipe(this.AiTopActions[GainType.Popularity], "WIN: 6th Star - Popularity"));
					return sortedList.Values[0];
				}
				if (this.player.stars[StarType.Power] == 0 && this.player.GetNumberOfStars(StarType.Power) == 0 && this.CanPlayTopAction(GainType.Power))
				{
					sortedList.Add(10000, new AiRecipe(this.AiTopActions[GainType.Power], "WIN: 6th Star - Power"));
					return sortedList.Values[0];
				}
				if (this.player.stars[StarType.Combat] == 0)
				{
					int count = this.strategicAnalysis.movePriority.Count;
				}
				int num2 = this.player.stars[StarType.Objective];
				if (this.player.stars[StarType.Upgrades] == 0 && this.player.GetNumberOfStars(StarType.Upgrades) == 0 && this.AiActions[this.gainUpgradeActionPosition[0]].downAction.CanPlayerPayActions())
				{
					sortedList.Add(10000, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainUpgradeActionPosition)], "WIN: 6th Star - Upgrades"));
					return sortedList.Values[0];
				}
				if (this.player.stars[StarType.Mechs] == 0 && this.player.GetNumberOfStars(StarType.Mechs) == 0 && this.AiActions[this.gainMechActionPosition[0]].downAction.CanPlayerPayActions())
				{
					sortedList.Add(10000, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainMechActionPosition)], "WIN: 6th Star - Mechs"));
					return sortedList.Values[0];
				}
				if (this.player.stars[StarType.Structures] == 0 && this.player.GetNumberOfStars(StarType.Structures) == 0 && this.AiActions[this.gainBuildingActionPosition[0]].downAction.CanPlayerPayActions())
				{
					sortedList.Add(10000, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainBuildingActionPosition)], "WIN: 6th Star - Structures"));
					return sortedList.Values[0];
				}
				if (this.player.stars[StarType.Recruits] == 0 && this.player.GetNumberOfStars(StarType.Recruits) == 0 && this.AiActions[this.gainRecruitActionPosition[0]].downAction.CanPlayerPayActions())
				{
					sortedList.Add(10000, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainRecruitActionPosition)], "WIN: 6th Star - Recruits"));
					return sortedList.Values[0];
				}
			}
			sortedList.Clear();
			Dictionary<DownActionType, int> dictionary = new Dictionary<DownActionType, int>
			{
				{
					DownActionType.Upgrade,
					206
				},
				{
					DownActionType.Deploy,
					202
				},
				{
					DownActionType.Build,
					200
				},
				{
					DownActionType.Enlist,
					204
				}
			};
			if (this.player.matPlayer.UpgradesDone >= 3)
			{
				if (this.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					dictionary[DownActionType.Upgrade] = 210;
				}
				else
				{
					dictionary[DownActionType.Upgrade] = 150;
				}
			}
			if ((this.player.matFaction.faction == Faction.Polania && this.player.matPlayer.matType == PlayerMatType.Agricultural) || (this.player.matFaction.faction == Faction.Polania && this.player.matPlayer.matType == PlayerMatType.Engineering) || (this.player.matFaction.faction == Faction.Nordic && this.player.matPlayer.matType == PlayerMatType.Agricultural) || (this.player.matFaction.faction == Faction.Nordic && this.player.matPlayer.matType == PlayerMatType.Engineering))
			{
				dictionary[DownActionType.Build] = 215;
			}
			else
			{
				dictionary[DownActionType.Build] = 170;
			}
			bool flag = (this.player.matFaction.faction == Faction.Polania && this.player.matPlayer.matType == PlayerMatType.Agricultural) || (this.player.matFaction.faction == Faction.Polania && this.player.matPlayer.matType == PlayerMatType.Engineering) || (this.player.matFaction.faction == Faction.Nordic && this.player.matPlayer.matType == PlayerMatType.Agricultural) || (this.player.matFaction.faction == Faction.Nordic && this.player.matPlayer.matType == PlayerMatType.Engineering);
			if (flag)
			{
				dictionary[DownActionType.Deploy] = 195;
			}
			else
			{
				dictionary[DownActionType.Deploy] = 270;
			}
			if (this.player.matFaction.faction == Faction.Saxony)
			{
				PlayerMatType matType = this.player.matPlayer.matType;
				if (matType != PlayerMatType.Industrial)
				{
					if (matType - PlayerMatType.Patriotic <= 2)
					{
						dictionary[DownActionType.Deploy] = (flag ? 195 : 273);
						dictionary[DownActionType.Enlist] = 204;
					}
				}
				else
				{
					dictionary[DownActionType.Enlist] = 206;
					dictionary[DownActionType.Deploy] = (flag ? 195 : 271);
				}
			}
			if (this.player.matFaction.faction == Faction.Crimea)
			{
				PlayerMatType crimeaMat = this.player.matPlayer.matType;
				bool crimeaEnlistAboveDeployMats = crimeaMat == PlayerMatType.Mechanical || crimeaMat == PlayerMatType.Agricultural || crimeaMat == PlayerMatType.Patriotic || crimeaMat == PlayerMatType.Militant;
				if (crimeaEnlistAboveDeployMats)
				{
					int mechCount = this.player.matFaction.mechs.Count;
					int recruitCount = this.player.matPlayer.RecruitsEnlisted;
					if (mechCount >= 2 && recruitCount < 4)
					{
						dictionary[DownActionType.Deploy] = 200;
						dictionary[DownActionType.Enlist] = 280;
					}
					else if (mechCount >= 2 && recruitCount >= 4)
					{
						dictionary[DownActionType.Deploy] = 280;
						dictionary[DownActionType.Enlist] = 200;
					}
					else
					{
						dictionary[DownActionType.Enlist] = 275;
						dictionary[DownActionType.Deploy] = 270;
					}
				}
				else
				{
					dictionary[DownActionType.Enlist] = 234;
				}
			}
			this.StrategicAnalysisRun();
			if (this.player.aiDifficulty == AIDifficulty.Medium)
			{
				this.kickstarter.KickStart(sortedList, this);
			}
			else if (this.player.aiDifficulty == AIDifficulty.Hard)
			{
				this.kickstarterAdv.KickStart(sortedList, this);
			}
			this.PurgeActions(sortedList, this);
			if (this.player.matFaction.faction == Faction.Nordic)
			{
				int count2 = this.player.matFaction.mechs.Count;
				if (count2 < 2 && this.AiActions[this.gainMechActionPosition[0]].downAction.CanPlayerPayActions())
				{
					sortedList.Add(9000, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainMechActionPosition)], "Nordic: Deploy Mech " + (count2 + 1).ToString() + "/2"));
				}
				else if (count2 >= 2 && this.player.matPlayer.matPlayerSectionsCount <= 4 && this.strategicAnalysis.factoryDistance <= (int)this.player.character.MaxMoveCount && (this.gameManager.gameBoard.factory.Owner == this.player || this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0) && this.CanPlayTopAction(GainType.Move))
				{
					sortedList.Add(8500, new AiRecipe(this.AiTopActions[GainType.Move], "Nordic: Move to Factory for card"));
				}
			}
			if (this.player.matFaction.faction == Faction.Saxony)
			{
				int count3 = this.player.matPlayer.workers.Count;
				int upgradesDone = this.player.matPlayer.UpgradesDone;
				int count4 = this.player.matFaction.mechs.Count;
				if (count3 < 5)
				{
					if (this.CanPlayTopAction(GainType.Produce) && this.strategicAnalysis.workersInaVillage > 0)
					{
						sortedList.Add(9500, new AiRecipe(this.AiTopActions[GainType.Produce], "Saxony: Produce workers (" + count3.ToString() + "/5)"));
					}
				}
				else if (upgradesDone < 1 && this.AiActions[this.gainUpgradeActionPosition[0]].downAction.CanPlayerPayActions())
				{
					sortedList.Add(9400, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainUpgradeActionPosition)], "Saxony: First upgrade"));
				}
				else if (upgradesDone >= 1 && count4 < 2 && this.AiActions[this.gainMechActionPosition[0]].downAction.CanPlayerPayActions())
				{
					sortedList.Add(9300, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainMechActionPosition)], "Saxony: Deploy Mech " + (count4 + 1).ToString() + "/2"));
				}
				else if (upgradesDone >= 1 && count4 >= 2)
				{
					if (this.player.Power < 3 && this.CanPlayTopAction(GainType.Power))
					{
						sortedList.Add(9200, new AiRecipe(this.AiTopActions[GainType.Power], "Saxony: Gain power for combat"));
					}
					else if (count4 < 3 && this.strategicAnalysis.enemyCanBeAttackedBy.Count > 0 && this.AiActions[this.gainMechActionPosition[0]].downAction.CanPlayerPayActions())
					{
						bool flag2 = false;
						foreach (KeyValuePair<GameHex, List<Unit>> keyValuePair in this.strategicAnalysis.enemyCanBeAttackedBy)
						{
							if (keyValuePair.Key.GetOwnerMechs().Count >= 2)
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							sortedList.Add(9100, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainMechActionPosition)], "Saxony: Additional mech for combat"));
						}
					}
				}
			}
			if (sortedList.Count == 0)
			{
				this.StarUpgrades(sortedList, dictionary[DownActionType.Upgrade]);
				this.StarRecruits(sortedList, dictionary[DownActionType.Enlist]);
				this.StarMechs(sortedList, dictionary[DownActionType.Deploy]);
				this.StarConstructions(sortedList, dictionary[DownActionType.Build]);
				if (this.player.matFaction.faction == Faction.Polania && this.player.matPlayer.matType == PlayerMatType.Industrial)
				{
					this.ProduceLoop(sortedList, 180);
				}
				if ((this.player.matFaction.faction == Faction.Saxony && this.player.matPlayer.matType == PlayerMatType.Mechanical) || (this.player.matFaction.faction == Faction.Saxony && this.player.matPlayer.matType == PlayerMatType.Industrial) || (this.player.matFaction.faction == Faction.Nordic && this.player.matPlayer.matType == PlayerMatType.Mechanical) || (this.player.matFaction.faction == Faction.Polania && this.player.matPlayer.matType == PlayerMatType.Agricultural) || (this.player.matFaction.faction == Faction.Polania && this.player.matPlayer.matType == PlayerMatType.Industrial) || (this.player.matFaction.faction == Faction.Albion && this.player.matPlayer.matType == PlayerMatType.Innovative) || (this.player.matFaction.faction == Faction.Albion && this.player.matPlayer.matType == PlayerMatType.Industrial) || (this.player.matFaction.faction == Faction.Albion && this.player.matPlayer.matType == PlayerMatType.Mechanical) || (this.player.matFaction.faction == Faction.Albion && this.player.matPlayer.matType == PlayerMatType.Militant) || (this.player.matFaction.faction == Faction.Togawa && this.player.matPlayer.matType == PlayerMatType.Militant))
				{
					this.TradeLoop(sortedList, 170);
				}
				this.CoinIfPoor(sortedList, 142);
				if (this.player.matFaction.faction == Faction.Polania && this.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					this.PowerIfWeak(sortedList, 114);
				}
				else
				{
					this.PowerIfWeak(sortedList, 140);
				}
				if (this.player.matFaction.faction != Faction.Saxony || this.player.matPlayer.matType != PlayerMatType.Engineering)
				{
					this.TradeForBottomAction(sortedList, 136);
				}
				this.ProduceForDemand(sortedList, 132);
				this.MoveByAnalysisPriority(sortedList);
				this.TradeForResource(sortedList, 120);
				this.StarPower(sortedList, 112);
				this.StarPopularity(sortedList, 110);
				this.RandomAllowed(sortedList, 100);
				if ((this.player.matFaction.faction == Faction.Nordic && this.player.matPlayer.matType == PlayerMatType.Industrial) || (this.player.matFaction.faction == Faction.Polania && this.player.matPlayer.matType == PlayerMatType.Patriotic) || (this.player.matFaction.faction == Faction.Polania && this.player.matPlayer.matType == PlayerMatType.Agricultural))
				{
					if (sortedList.Values[0].action.topAction.Type != TopActionType.MoveGain)
					{
						this.MoveToBuild(sortedList, 144);
					}
				}
				else
				{
					this.MoveToBuild(sortedList, 144);
				}
				this.MovePreparation(sortedList);
				this.ProducePreparation(sortedList);
			}
			this.Log(sortedList);
			return sortedList.Values[0];
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x0010EB0C File Offset: 0x0010CD0C
		public void Log(SortedList<int, AiRecipe> actionOptions)
		{
			string text = "---" + this.player.matFaction.faction.ToString() + "---" + Environment.NewLine;
			if (this.player.objectiveCards.Count > 1)
			{
				text = string.Concat(new string[]
				{
					text,
					"Objectives: ",
					this.player.objectiveCards[0].CardId.ToString(),
					this.player.objectiveCards[0].status.ToString(),
					"; ",
					this.player.objectiveCards[1].CardId.ToString(),
					this.player.objectiveCards[1].status.ToString()
				});
			}
			this.Log(text);
			foreach (KeyValuePair<int, AiRecipe> keyValuePair in actionOptions)
			{
				this.Log(string.Concat(new string[]
				{
					"★ ",
					keyValuePair.Key.ToString(),
					": ",
					keyValuePair.Value.description,
					" (section: ",
					keyValuePair.Value.action.matSectionId.ToString(),
					")",
					Environment.NewLine
				}));
			}
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x00044738 File Offset: 0x00042938
		public void Log(string msg)
		{
			if (AiPlayer.LogMessage != null)
			{
				AiPlayer.LogMessage(msg);
			}
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x0010ECBC File Offset: 0x0010CEBC
		public void LogAiPlayerStatus()
		{
			this.Log(this.ToString());
			string text = "Stars: ";
			foreach (StarType starType in this.player.stars.Keys)
			{
				if (this.player.stars[starType] > 0)
				{
					text = string.Concat(new string[]
					{
						text,
						starType.ToString(),
						": ",
						this.player.stars[starType].ToString(),
						"; "
					});
				}
			}
			this.Log(text + Environment.NewLine);
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x0004474C File Offset: 0x0004294C
		public static void AttachLogDelegate(AiPlayer.LogDelegate func)
		{
			AiPlayer.LogMessage = null;
			AiPlayer.LogMessage += func;
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x0010ED98 File Offset: 0x0010CF98
		private int MovePreparationPriority()
		{
			if (this.player.aiDifficulty == AIDifficulty.Hard)
			{
				if ((this.player.matFaction.faction == Faction.Nordic && this.player.matPlayer.matType == PlayerMatType.Patriotic) || (this.player.matFaction.faction == Faction.Nordic && this.player.matPlayer.matType == PlayerMatType.Mechanical) || (this.player.matFaction.faction == Faction.Nordic && this.player.matPlayer.matType == PlayerMatType.Industrial) || (this.player.matFaction.faction == Faction.Rusviet && this.player.matPlayer.matType == PlayerMatType.Industrial) || (this.player.matFaction.faction == Faction.Saxony && this.player.matPlayer.matType == PlayerMatType.Engineering) || (this.player.matFaction.faction == Faction.Saxony && this.player.matPlayer.matType == PlayerMatType.Industrial) || (this.player.matFaction.faction == Faction.Polania && this.player.matPlayer.matType == PlayerMatType.Agricultural) || (this.player.matFaction.faction == Faction.Albion && this.player.matPlayer.matType == PlayerMatType.Militant))
				{
					return 150;
				}
				return 0;
			}
			else
			{
				if ((this.player.matFaction.faction == Faction.Nordic && this.player.matPlayer.matType == PlayerMatType.Patriotic) || (this.player.matFaction.faction == Faction.Nordic && this.player.matPlayer.matType == PlayerMatType.Mechanical) || (this.player.matFaction.faction == Faction.Nordic && this.player.matPlayer.matType == PlayerMatType.Industrial) || (this.player.matFaction.faction == Faction.Rusviet && this.player.matPlayer.matType == PlayerMatType.Industrial) || (this.player.matFaction.faction == Faction.Saxony && this.player.matPlayer.matType == PlayerMatType.Industrial) || (this.player.matFaction.faction == Faction.Saxony && this.player.matPlayer.matType == PlayerMatType.Engineering) || (this.player.matFaction.faction == Faction.Polania && this.player.matPlayer.matType == PlayerMatType.Agricultural))
				{
					return 150;
				}
				return 0;
			}
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x0010F00C File Offset: 0x0010D20C
		public List<ResourceBundle> FindResourcesAdvanced(ResourceType resType, int resAmount, out bool fulfilled)
		{
			List<ResourceBundle> list = new List<ResourceBundle>();
			List<GameHex> list2 = new List<GameHex>();
			foreach (GameHex gameHex in this.player.OwnedFields(false))
			{
				if (gameHex.hexType != HexType.capital && gameHex.resources[resType] > 0)
				{
					list2.Add(gameHex);
				}
			}
			if (this.player.matFaction.faction == Faction.Saxony && this.player.matPlayer.matType == PlayerMatType.Industrial)
			{
				list2.Sort(delegate(GameHex A, GameHex B)
				{
					int num2 = 10 * (A.GetOwnerUnitCount() - A.GetOwnerWorkers().Count) + A.GetOwnerWorkers().Count;
					int num3 = 10 * (B.GetOwnerUnitCount() - B.GetOwnerWorkers().Count) + B.GetOwnerWorkers().Count;
					return num2.CompareTo(num3);
				});
			}
			foreach (GameHex gameHex2 in list2)
			{
				if (resAmount > 0)
				{
					int num = gameHex2.resources[resType];
					if (num > resAmount)
					{
						num = resAmount;
					}
					list.Add(new ResourceBundle
					{
						amount = (int)((short)num),
						gameHex = gameHex2,
						resourceType = resType
					});
					resAmount -= num;
				}
			}
			fulfilled = resAmount == 0;
			return list;
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x0010F160 File Offset: 0x0010D360
		public bool Pay4ActionAdvanced(PayResource payResource)
		{
			bool flag = false;
			if (this.player.matFaction.factionPerk == AbilityPerk.Coercion && this.player.combatCards.Count > 0 && (this.player.matPlayer.matType == PlayerMatType.Industrial || this.player.matPlayer.matType == PlayerMatType.Patriotic || this.player.matPlayer.matType == PlayerMatType.Agricultural))
			{
				List<ResourceBundle> list = this.FindResources(payResource.ResourceToPay, (int)(payResource.Amount - 1), out flag);
				if (flag)
				{
					list.Add(new ResourceBundle
					{
						amount = 1,
						resourceType = ResourceType.combatCard,
						gameHex = this.player.character.position
					});
					this.player.combatCards.Sort((CombatCard A, CombatCard B) => A.CombatBonus.CompareTo(B.CombatBonus));
					payResource.SetResources(list, this.player.combatCards[0]);
					bool flag2 = payResource.CanExecute();
					if (flag2)
					{
						this.gameManager.actionManager.SetPayAction(payResource);
						this.gameManager.actionManager.PrepareNextAction();
					}
					return flag2;
				}
			}
			List<ResourceBundle> list2 = this.FindResources(payResource.ResourceToPay, (int)payResource.Amount, out flag);
			if (flag)
			{
				payResource.SetResources(list2, null);
			}
			else if (this.player.matFaction.factionPerk == AbilityPerk.Coercion && this.player.combatCards.Count > 0)
			{
				list2.Add(new ResourceBundle
				{
					amount = 1,
					resourceType = ResourceType.combatCard,
					gameHex = this.player.character.position
				});
				this.player.combatCards.Sort((CombatCard A, CombatCard B) => A.CombatBonus.CompareTo(B.CombatBonus));
				payResource.SetResources(list2, this.player.combatCards[0]);
			}
			bool flag3 = payResource.CanExecute();
			if (flag3)
			{
				this.gameManager.actionManager.SetPayAction(payResource);
				this.gameManager.actionManager.PrepareNextAction();
			}
			return flag3;
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x0004475A File Offset: 0x0004295A
		private int ProducePreparationPriority()
		{
			if (this.player.aiDifficulty != AIDifficulty.Hard)
			{
				return 0;
			}
			if (this.player.matFaction.faction == Faction.Saxony && this.player.matPlayer.matType == PlayerMatType.Mechanical)
			{
				return 150;
			}
			return 0;
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x0010F398 File Offset: 0x0010D598
		private void ProducePreparation(SortedList<int, AiRecipe> actionOptions)
		{
			AiAction action = actionOptions.Values[0].action;
			if (action.topAction.Type == TopActionType.Produce && this.player.stars[StarType.Recruits] == 0 && !action.downAction.CanPlayerPayActions() && this.CanPlayTopAction(GainType.AnyResource))
			{
				int amount = (int)action.downAction.GetPayAction(0).Amount;
				ResourceType resourceType = ResourceType.combatCard;
				switch (action.downAction.Type)
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
				if (amount - this.player.Resources(false)[resourceType] <= 2)
				{
					actionOptions.Add(this.ProducePreparationPriority(), new AiRecipe(this.AiTopActions[GainType.AnyResource], "Produce preparation")
					{
						tradeResource = new ResourceType[] { resourceType, resourceType }
					});
				}
			}
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x0010F494 File Offset: 0x0010D694
		public void MoveCharacterToEncounter(GainMove gainMove)
		{
			GameHex gameHex = null;
			foreach (GameHex gameHex2 in ((AiStrategicAnalysisAdv)this.strategicAnalysis).moveRangeAll[this.player.character].Keys)
			{
				if (gameHex2.hasEncounter && !gameHex2.encounterUsed && (gameHex2.Owner == null || gameHex2.Owner == this.player))
				{
					gameHex = gameHex2;
				}
			}
			if (gameHex != null)
			{
				this.gameManager.moveManager.SelectUnit(this.player.character);
				this.gameManager.moveManager.MoveSelectedUnit(gameHex, this.kickstarterAdv.HexResources(this.player.character.position), null);
			}
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x0010F578 File Offset: 0x0010D778
		private void MoveToBuildAdvanced(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (!this.strategicAnalysis.canBuild && this.player.GetNumberOfStars(StarType.Structures) == 0 && this.AiActions[this.gainBuildingActionPosition[0]].downAction.CanPlayerPayActions() && this.CanPlayTopAction(GainType.Move))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Move], "Move workers to build")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)this.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.MoveWorkerToBuild(gainMove, aiPlayer.player.matPlayer.workers[0]);
						this.MoveWorkerToBuild(gainMove, aiPlayer.player.matPlayer.workers[1]);
						if (gainMove.Amount == 3 && this.player.matFaction.faction == Faction.Polania)
						{
							this.MoveCharacterToEncounter(gainMove);
						}
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
			}
		}

		// Token: 0x04001ED9 RID: 7897
		public readonly Player player;

		// Token: 0x04001EDA RID: 7898
		public List<AiAction> AiActions = new List<AiAction>();

		// Token: 0x04001EDB RID: 7899
		public Dictionary<GainType, AiAction> AiTopActions = new Dictionary<GainType, AiAction>();

		// Token: 0x04001EDC RID: 7900
		public AiStrategicAnalysis strategicAnalysis;

		// Token: 0x04001EDD RID: 7901
		public AiKickStart kickstarter;

		// Token: 0x04001EDE RID: 7902
		public AiKickStartAdv kickstarterAdv;

		// Token: 0x04001EDF RID: 7903
		public List<int> gainUpgradeActionPosition = new List<int>();

		// Token: 0x04001EE0 RID: 7904
		public List<int> gainMechActionPosition = new List<int>();

		// Token: 0x04001EE1 RID: 7905
		public List<int> gainBuildingActionPosition = new List<int>();

		// Token: 0x04001EE2 RID: 7906
		public List<int> gainRecruitActionPosition = new List<int>();

		// Token: 0x04001EE3 RID: 7907
		public HashSet<GainType> forbiddenActions = new HashSet<GainType>();

		// Token: 0x04001EE4 RID: 7908
		public bool WaitingForCard;

		// Token: 0x04001EE5 RID: 7909
		private bool bottomActionExecuted;

		// Token: 0x04001EE6 RID: 7910
		private GameManager gameManager;

		// Token: 0x0200059C RID: 1436
		// (Invoke) Token: 0x06002DA3 RID: 11683
		public delegate void LogDelegate(string s);

		// Token: 0x0200059D RID: 1437
		// (Invoke) Token: 0x06002DA7 RID: 11687
		public delegate void CombatAbilityUsedDelegate(AbilityPerk abilityPerk);
	}
}

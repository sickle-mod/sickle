using System;
using System.Collections.Generic;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic
{
	// Token: 0x020005A8 RID: 1448
	public class AiPlayer
	{
		public delegate void LogDelegate(string s);
		public delegate void CombatAbilityUsedDelegate(AbilityPerk abilityPerk);

		public static event LogDelegate LogMessage;
		public static event CombatAbilityUsedDelegate CombatAbilityUsed;

		// Token: 0x06002DA0 RID: 11680 RVA: 0x0010CB24 File Offset: 0x0010AD24
		public AiPlayer(Player player, GameManager gameManager)
		{
			this.player = player;
			this.gameManager = gameManager;
			this.strategicAnalysis = new AiStrategicAnalysis(gameManager);
			this.kickstarter = new AiKickStart(gameManager);
			this.kickstarterAdv = new AiKickStartAdv(gameManager);
			this.Init();
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x0010CBBC File Offset: 0x0010ADBC
		public void Init()
		{
			if (this.player.aiDifficulty == AIDifficulty.Hard)
			{
				this.strategicAnalysis = new AiStrategicAnalysisAdv(this.gameManager);
			}
			this.InitActions();
		}

		public void InitActions()
		{
			this.AiTopActions.Clear();
			this.AiActions.Clear();
			this.gainUpgradeActionPosition.Clear();
			this.gainMechActionPosition.Clear();
			this.gainBuildingActionPosition.Clear();
			this.gainRecruitActionPosition.Clear();

			int num = 0;
			int sectionsCount = this.player.matPlayer.matPlayerSectionsCount;
			for (int i = 0; i < sectionsCount; i++)
			{
				MatPlayerSection playerMatSection = this.player.matPlayer.GetPlayerMatSection(i);
				if (playerMatSection == null) continue;
				TopAction actionTop = playerMatSection.ActionTop;
				for (int j = 0; j < actionTop.gainActionsCount; j++)
				{
					AiAction aiAction = new AiAction(i, actionTop, j, playerMatSection.ActionDown, this.gameManager);
					GainType topType = actionTop.GetGainAction(j).GetGainType();
					if (!this.AiTopActions.ContainsKey(topType))
					{
						this.AiTopActions.Add(topType, aiAction);
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

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000445FC File Offset: 0x000427FC
		public List<ResourceBundle> FindResources(ResourceType resType, int resAmount, out bool fulfilled)
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

		// Token: 0x06002DA5 RID: 11685 RVA: 0x0004461F File Offset: 0x0004281F
		public bool Pay4Action(PayResource payResource)
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

		// Token: 0x06002DA7 RID: 11687 RVA: 0x0004463E File Offset: 0x0004283E
		public void PlayTurn()
		{
			this.PlayAction(this.Bot());
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x0010D224 File Offset: 0x0010B424
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

		// Token: 0x06002DA9 RID: 11689 RVA: 0x0010D36C File Offset: 0x0010B56C
		private AiAction CreateCurrentAction()
		{
			int currentMatSection = this.player.currentMatSection;
			MatPlayerSection playerMatSection = this.player.matPlayer.GetPlayerMatSection(currentMatSection);
			int gainActionId = this.gameManager.actionManager.GetGainActionId();
			return new AiAction(currentMatSection, playerMatSection.ActionTop, gainActionId, playerMatSection.ActionDown, this.gameManager);
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x0004464C File Offset: 0x0004284C
		private void ContinueCombat()
		{
			if (this.gameManager.combatManager.CanPerformStep(this.player))
			{
				this.PerformCombatStage(this.gameManager.combatManager.GetActualStage());
			}
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x0004467C File Offset: 0x0004287C
		private void ContinueTopAction(AiAction aiAction)
		{
			this.gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x0004468E File Offset: 0x0004288E
		private void ContinueDownAction(AiAction aiAction)
		{
			this.gameManager.actionManager.PrepareNextAction();
			this.TryToCompleteObjective();
			this.InformAboutEndedTurn();
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x0010D3C4 File Offset: 0x0010B5C4
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

		// Token: 0x06002DAE RID: 11694 RVA: 0x0010D414 File Offset: 0x0010B614
		private void PlayTopAction(AiRecipe recipe, AiAction action)
		{
			if (!this.forbiddenActions.Contains(action.topAction.GetGainAction(action.gainActionId).GetGainType()))
			{
				this.gameManager.actionManager.SetSectionAction(action.topAction, null, action.gainActionId);
				action.ActionTopExecute(recipe, this);
			}
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x0010D46C File Offset: 0x0010B66C
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

		// Token: 0x06002DB0 RID: 11696 RVA: 0x0010D520 File Offset: 0x0010B720
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

		// Token: 0x06002DB1 RID: 11697 RVA: 0x0010D600 File Offset: 0x0010B800
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

		// Token: 0x06002DB2 RID: 11698 RVA: 0x0010D678 File Offset: 0x0010B878
		public bool CanPlayTopAction(GainType gain)
		{
			return !this.forbiddenActions.Contains(gain) && !this.player.matFaction.DidPlayerUsedMatLastTurn(this.AiTopActions[gain].matSectionId, this.player.lastMatSection);
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x0010D6C4 File Offset: 0x0010B8C4
		private bool CanPlayDownAction(AiAction action)
		{
			return !this.forbiddenActions.Contains(action.downAction.GetGainAction(0).GetGainType()) && !this.player.matFaction.DidPlayerUsedMatLastTurn(action.matSectionId, this.player.lastMatSection);
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x0010D718 File Offset: 0x0010B918
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

		// Token: 0x06002DB5 RID: 11701 RVA: 0x0010D9E4 File Offset: 0x0010BBE4
		public void UseAbility()
		{
			// Albion: Sword (Attacker gets +2 power)
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

			// Abilities at index 2 (Shield, Disarm, Artillery, Scout, Ronin, etc.)
			if (!this.player.matFaction.SkillUnlocked[2])
			{
				return;
			}

			AbilityPerk abilityPerk = this.player.matFaction.abilities[2];
			bool used = false;

			switch (abilityPerk)
			{
				case AbilityPerk.Artillery:
					if (this.player.Power >= 1)
					{
						this.gameManager.combatManager.RemovePowerFromEnemy(1);
						used = true;
					}
					break;
				case AbilityPerk.Scout:
					Player enemyOf = this.gameManager.combatManager.GetEnemyOf(this.player);
					if (enemyOf.combatCards.Count > 0)
					{
						int num = this.gameManager.random.Next(0, enemyOf.combatCards.Count);
						CombatCard combatCard = enemyOf.combatCards[num];
						this.gameManager.combatManager.StealCombatCardFromEnemy(combatCard);
						used = true;
					}
					break;
				case AbilityPerk.Shield:
					// Albion: Shield (Defender gets +2 power)
					if (this.player == this.gameManager.combatManager.GetDefender())
					{
						this.gameManager.combatManager.BolsterBeforeCombat();
						used = true;
					}
					break;
				case AbilityPerk.Disarm:
					// Saxony: Disarm (Attacker forces defender to lose 2 power)
					this.gameManager.combatManager.RemovePowerFromEnemy(0);
					used = true;
					break;
				case AbilityPerk.Ronin:
					// Togawa: Ronin (Single unit gets +2 power)
					if (this.gameManager.combatManager.GetSelectedBattlefield().GetPlayerUnitCount(this.player) == 1)
					{
						this.gameManager.combatManager.BolsterBeforeCombat();
						used = true;
					}
					break;
			}

			if (used)
			{
				if (this.gameManager.combatManager.GetEnemyOf(this.player).IsHuman)
				{
					AiPlayer.CombatAbilityUsedDelegate combatAbilityUsedDelegate = AiPlayer.CombatAbilityUsed;
					if (combatAbilityUsedDelegate != null)
					{
						combatAbilityUsedDelegate(abilityPerk);
					}
				}
			}
			else
			{
				// FIX COMBAT FREEZE: Must set used flag in CombatManager even if skipping
				this.gameManager.combatManager.PassDiversionAbility();
			}
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x0010DD28 File Offset: 0x0010BF28
		public bool CombatPreparation(bool simulate = false, GameHex battlefield = null, Player opponent = null)
		{
			if (this.player.matFaction.faction == Faction.Crimea && this.player.matPlayer.matType == PlayerMatType.Innovative && this.player.stars[StarType.Objective] == 0 && this.player.objectiveCards != null)
			{
				bool hasObj18 = false;
				foreach (var obj in this.player.objectiveCards)
				{
					if (obj.CardId == 18 && obj.status == ObjectiveCard.ObjectiveStatus.Open)
					{
						hasObj18 = true;
						break;
					}
				}
				if (hasObj18)
				{
					if (opponent == null) opponent = this.gameManager.combatManager.GetEnemyOf(this.player);
					if (battlefield == null) battlefield = this.gameManager.combatManager.GetSelectedBattlefield();

					// 1. Calculate opponent's max potential output
					int oppPowerLimit = Math.Min(opponent.Power, 7);
					int oppUnits = 0;
					int myUnits = 0;
					if (battlefield != null)
					{
						if (battlefield.Owner == this.player)
						{
							myUnits = battlefield.GetOwnerMechs().Count + (battlefield.HasOwnerCharacter() ? 1 : 0);
							oppUnits = battlefield.GetEnemyMechs().Count + (battlefield.HasEnemyCharacter() ? 1 : 0);
						}
						else
						{
							oppUnits = battlefield.GetOwnerMechs().Count + (battlefield.HasOwnerCharacter() ? 1 : 0);
							myUnits = battlefield.GetEnemyMechs().Count + (battlefield.HasEnemyCharacter() ? 1 : 0);
						}
					}
					else
					{
						oppUnits = 2; // Fallback
						myUnits = 2;
					}

					int oppCardsPossible = Math.Min(opponent.combatCards.Count, oppUnits);
					int oppPotentialTotal = oppPowerLimit + (oppCardsPossible * 5); // Assume all cards are 5s

					// 2. We WANT to spend as much physical power as possible (max 7) to reach 0 for Objective 18.
					int myPowerToSpend = Math.Min(this.player.Power, 7);

					// 3. Winning Target (Attacker wins ties in Scythe)
					bool isAttacker = (this.gameManager.combatManager.GetAttacker() == this.player);
					int winTarget = oppPotentialTotal + (isAttacker ? 0 : 1);

					List<CombatCard> cardsToSpend = new List<CombatCard>();
					int myCardsPower = 0;

					// 4. If power alone isn't enough to beat their MAX potential, add minimum cards needed
					if (myPowerToSpend < winTarget)
					{
						int cardsValueNeeded = winTarget - myPowerToSpend;
						this.player.combatCards.Sort((CombatCard A, CombatCard B) => B.CombatBonus.CompareTo(A.CombatBonus));
						
						for (int i = 0; i < Math.Min(myUnits, this.player.combatCards.Count); i++)
						{
							if (myCardsPower < cardsValueNeeded)
							{
								cardsToSpend.Add(this.player.combatCards[i]);
								myCardsPower += this.player.combatCards[i].CombatBonus;
							}
							else break;
						}
					}

					if (!simulate)
					{
						this.gameManager.combatManager.AddPlayerPowerInBattle(this.player, new PowerSelected
						{
							selectedPower = myPowerToSpend,
							cardsPower = myCardsPower,
							selectedCards = cardsToSpend
						});
					}
					return true;
				}
			}
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
					// If they have no power but have cards, spend the lowest card to qualify for the consolation card.
					if (num7 == 0 && this.player.combatCards.Count > 0)
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

		// Token: 0x06002DB7 RID: 11703 RVA: 0x0010E35C File Offset: 0x0010C55C
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

		// Token: 0x06002DB8 RID: 11704 RVA: 0x0010E558 File Offset: 0x0010C758
		public void UseTogawaAbility()
		{
			if (this.player.matFaction.abilities.Contains(AbilityPerk.Shinobi) && this.player.matFaction.SkillUnlocked[3] && this.gameManager.tokenManager.CanRearmTrap(this.gameManager.combatManager.GetSelectedBattlefield(), this.player.matFaction.faction))
			{
				this.gameManager.tokenManager.ArmTrap(this.gameManager.combatManager.GetSelectedBattlefield());
			}
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x000446AC File Offset: 0x000428AC
		public void UsePolaniaAbility()
		{
			if (this.player.matFaction.abilities.Contains(AbilityPerk.Camaraderie) && this.player.matFaction.SkillUnlocked[2])
			{
				this.gameManager.combatManager.PolaniaCamaraderieChoice(true);
			}
		}

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
					int num2 = this.SelectBestFactoryCard(factoryCards);
					if (this.gameManager.PlayerOwner == null)
					{
						this.gameManager.OnActionSent(new EmptyCardsMessage(factoryCards.Count));
						this.gameManager.OnActionSent(new FactoryMessage(factoryCards[num2].CardId.ToString(), (int)this.player.matFaction.faction, num2));
					}
					else
					{
						this.gameManager.OnActionSent(new GetFactoryMessage(num2));
					}
					this.gameManager.AddFactoryCard(num2);
					return;
				}
				List<FactoryCard> factoryCards2 = this.gameManager.GetFactoryCards();
				int num3 = this.SelectBestFactoryCard(factoryCards2);
				this.gameManager.AddFactoryCard(num3);
			}
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x0010E754 File Offset: 0x0010C954
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
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x0010EB74 File Offset: 0x0010CD74
		private bool CanGetResourcesFromEncounter(SectionAction section, int gainId)
		{
			if (gainId < 0 || gainId >= section.gainActionsCount)
			{
				return false;
			}
			GainAction gainAction = section.GetGainAction(gainId);
			if (gainAction is GainResource)
			{
				return this.strategicAnalysis.resourceDemandPriority[((GainResource)gainAction).ResourceToGain] > 0 && section.CanPlayerPayActions();
			}
			return gainAction is GainAnyResource && this.strategicAnalysis.resourceHighestPriority != ResourceType.combatCard && section.CanPlayerPayActions();
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x0010EBD8 File Offset: 0x0010CDD8
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

		// Token: 0x06002DBE RID: 11710 RVA: 0x0010ECAC File Offset: 0x0010CEAC
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
				bool flag = this.IsEncounterOptionAcceptable(encounterCard, 1);
				bool flag2 = this.IsEncounterOptionAcceptable(encounterCard, 2);
				bool flag3 = false;
				for (int i = 0; i < encounterCard.GetAction(2).gainActionsCount; i++)
				{
					GainAction gainAction = encounterCard.GetAction(2).GetGainAction(i);
					if (gainAction is GainResource)
					{
						GainResource gainResource = (GainResource)gainAction;
						if (gainResource.ResourceToGain == ResourceType.oil && gainResource.Amount == 4)
						{
							flag3 = true;
						}
					}
				}
				int num = 0;
				if (this.player.matFaction.factionPerk != AbilityPerk.Meander && flag2 && this.player.Popularity >= 3 && this.player.Popularity <= 5)
				{
					if (encounterCard.CardId == 14 && this.player.matPlayer.workers.Count < 5 && this.player.GetNumberOfStars(StarType.Mechs) == 0)
					{
						num = 2;
					}
					else if (encounterCard.CardId == 23 && this.player.matPlayer.workers.Count < 5 && this.player.GetNumberOfStars(StarType.Recruits) == 0)
					{
						num = 2;
					}
					else if ((encounterCard.CardId == 6 || encounterCard.CardId == 8 || encounterCard.CardId == 16) && this.player.GetNumberOfStars(StarType.Recruits) == 0)
					{
						num = 2;
					}
					else if ((encounterCard.CardId == 4 || encounterCard.CardId == 3 || encounterCard.CardId == 27) && this.player.GetNumberOfStars(StarType.Mechs) == 0)
					{
						num = 2;
					}
					else if (flag3 && this.player.GetNumberOfStars(StarType.Upgrades) == 0 && (this.player.matPlayer.matType == PlayerMatType.Industrial || this.player.matPlayer.matType == PlayerMatType.Patriotic || this.player.matPlayer.matType == PlayerMatType.Agricultural || this.player.matPlayer.matType == PlayerMatType.Engineering))
					{
						num = 2;
					}
					else if ((encounterCard.CardId == 19 || encounterCard.CardId == 21) && this.player.stars[StarType.Combat] < 2)
					{
						num = 2;
					}
					else if (encounterCard.CardId == 5 && this.player.GetNumberOfStars(StarType.Recruits) == 0 && this.player.GetNumberOfStars(StarType.Mechs) == 0)
					{
						num = 2;
					}
					else if (encounterCard.CardId == 15 && (this.player.GetNumberOfStars(StarType.Recruits) == 0 || this.player.GetNumberOfStars(StarType.Mechs) == 0))
					{
						num = 2;
					}
				}
				if (num == 0 && this.player.matFaction.faction == Faction.Rusviet && this.player.matPlayer.matType == PlayerMatType.Engineering)
				{
					int weight1 = 0;
					int weight2 = 0;
					if (flag)
					{
						for (int j = 0; j < encounterCard.GetAction(1).gainActionsCount; j++)
						{
							GainAction ga = encounterCard.GetAction(1).GetGainAction(j);
							GainType gt = ga.GetGainType();
							if (gt == GainType.Recruit) weight1 += 200;
							else if (gt == GainType.Resource && ga is GainResource && ((GainResource)ga).ResourceToGain == ResourceType.food) weight1 += 180;
							else if (gt == GainType.Power) weight1 += 160;
							else if (gt == GainType.CombatCard) weight1 += 140;
							else if (gt == GainType.Mech) weight1 += 120;
							else weight1 += 50;
						}
					}
					if (flag2)
					{
						for (int k = 0; k < encounterCard.GetAction(2).gainActionsCount; k++)
						{
							GainAction ga = encounterCard.GetAction(2).GetGainAction(k);
							GainType gt = ga.GetGainType();
							if (gt == GainType.Recruit) weight2 += 200;
							else if (gt == GainType.Resource && ga is GainResource && ((GainResource)ga).ResourceToGain == ResourceType.food) weight2 += 180;
							else if (gt == GainType.Power) weight2 += 160;
							else if (gt == GainType.CombatCard) weight2 += 140;
							else if (gt == GainType.Mech) weight2 += 120;
							else weight2 += 50;
						}
					}
					if (weight1 > 0 || weight2 > 0)
					{
						num = (weight1 >= weight2) ? 1 : 2;
					}
				}
				if (num == 0 && this.player.matFaction.faction == Faction.Saxony)
				{
					bool flag4 = false;
					if (flag)
					{
						for (int j = 0; j < encounterCard.GetAction(1).gainActionsCount; j++)
						{
							GainType gainType = encounterCard.GetAction(1).GetGainAction(j).GetGainType();
							if (gainType == GainType.Power || gainType == GainType.CombatCard)
							{
								flag4 = true;
							}
						}
						if (flag4)
						{
							num = 1;
						}
					}
					if (!flag4 && flag2)
					{
						for (int k = 0; k < encounterCard.GetAction(2).gainActionsCount; k++)
						{
							GainType gainType2 = encounterCard.GetAction(2).GetGainAction(k).GetGainType();
							if (gainType2 == GainType.Power || gainType2 == GainType.CombatCard)
							{
								flag4 = true;
							}
						}
						if (flag4)
						{
							num = 2;
						}
					}
					if (!flag4 && flag)
					{
						num = 1;
					}
					else if (!flag4 && flag2)
					{
						num = 2;
					}
				}
				else if (this.player.matFaction.factionPerk != AbilityPerk.Meander)
				{
					int weight1 = flag ? this.CalculateEncounterWeight(encounterCard.GetAction(1)) : -1;
					int weight2 = flag2 ? this.CalculateEncounterWeight(encounterCard.GetAction(2)) : -1;
					if (weight1 >= 0 || weight2 >= 0)
					{
						num = (weight1 >= weight2) ? 1 : 2;
					}
				}
				SectionAction sectionAction = encounterCard.GetAction(num);
				if (num > 0)
				{
					int[] array = new int[]
					{
						AiAction.FindResourceGain(encounterCard.GetAction(1)),
						AiAction.FindResourceGain(encounterCard.GetAction(2))
					};
					this.gameManager.ChooseEncounterOption(num);
					this.gameManager.actionManager.SetSectionAction(encounterCard.GetAction(num), null, array[num - 1]);
					for (int l = 0; l < sectionAction.gainActionsCount; l++)
					{
						GainAction gainAction2 = sectionAction.GetGainAction(l);
						if (gainAction2.GainAvaliable() && !this.player.automaticGain.ContainsKey(gainAction2.GetGainType()))
						{
							this.PrepareEncounterAction(gainAction2);
							this.gameManager.actionManager.PrepareNextAction();
						}
					}
				}
				else
				{
					for (int m = 0; m < sectionAction.gainActionsCount; m++)
					{
						GainAction gainAction3 = sectionAction.GetGainAction(m);
						if (gainAction3 is GainResource)
						{
							((GainResource)gainAction3).SetDestinationAmount(this.player.character.position, gainAction3.Amount);
						}
					}
					this.gameManager.ChooseEncounterOption(0);
					this.gameManager.actionManager.SetSectionAction(sectionAction, null, 0);
				}
				if (this.player.matFaction.factionPerk == AbilityPerk.Meander && (!this.gameManager.IsMultiplayer || this.gameManager.PlayerOwner == null))
				{
					int num2 = -1;
					if (flag && flag2)
					{
						bool flag5 = false;
						if (this.player.Popularity >= 3 && this.player.Popularity <= 5)
						{
							if (encounterCard.CardId == 14 && this.player.matPlayer.workers.Count < 5 && this.player.GetNumberOfStars(StarType.Mechs) == 0)
							{
								flag5 = true;
							}
							else if (encounterCard.CardId == 23 && this.player.matPlayer.workers.Count < 5 && this.player.GetNumberOfStars(StarType.Recruits) == 0)
							{
								flag5 = true;
							}
							else if ((encounterCard.CardId == 6 || encounterCard.CardId == 8 || encounterCard.CardId == 16) && this.player.GetNumberOfStars(StarType.Recruits) == 0)
							{
								flag5 = true;
							}
							else if ((encounterCard.CardId == 4 || encounterCard.CardId == 3 || encounterCard.CardId == 27) && this.player.GetNumberOfStars(StarType.Mechs) == 0)
							{
								flag5 = true;
							}
							else if (flag3 && this.player.GetNumberOfStars(StarType.Upgrades) == 0)
							{
								flag5 = true;
							}
							else if ((encounterCard.CardId == 19 || encounterCard.CardId == 21) && this.player.stars[StarType.Combat] < 2)
							{
								flag5 = true;
							}
							else if (encounterCard.CardId == 5 && this.player.GetNumberOfStars(StarType.Recruits) == 0 && this.player.GetNumberOfStars(StarType.Mechs) == 0)
							{
								flag5 = true;
							}
							else if (encounterCard.CardId == 15 && (this.player.GetNumberOfStars(StarType.Recruits) == 0 || this.player.GetNumberOfStars(StarType.Mechs) == 0))
							{
								flag5 = true;
							}
						}
						if (flag5)
						{
							num2 = 2;
						}
						else
						{
							int weight1 = this.CalculateEncounterWeight(encounterCard.GetAction(1));
							int weight2 = this.CalculateEncounterWeight(encounterCard.GetAction(2));
							if (num == 1)
							{
								num2 = 2; // If already picked 1, pick 2
							}
							else if (num == 2)
							{
								num2 = 1; // If already picked 2, pick 1
							}
							else
							{
								num2 = (weight1 >= weight2) ? 1 : 2;
							}
						}
					}
					else if (flag)
					{
						num2 = 1;
					}
					else if (flag2)
					{
						num2 = 2;
					}
					if (num2 >= 0 && num2 != num)
					{
						int[] array2 = new int[]
						{
							AiAction.FindResourceGain(encounterCard.GetAction(1)),
							AiAction.FindResourceGain(encounterCard.GetAction(2))
						};
						sectionAction = encounterCard.GetAction(num2);
						if (this.CanGetResourcesFromEncounter(sectionAction, array2[num2 - 1]))
						{
							this.gameManager.ChooseEncounterOption(num2);
							this.gameManager.actionManager.SetSectionAction(encounterCard.GetAction(num2), null, 0);
							for (int num6 = 0; num6 < sectionAction.gainActionsCount; num6++)
							{
								GainAction gainAction4 = sectionAction.GetGainAction(num6);
								if (gainAction4.GainAvaliable() && !this.player.automaticGain.ContainsKey(gainAction4.GetGainType()))
								{
									this.PrepareEncounterAction(gainAction4);
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
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x000446EC File Offset: 0x000428EC
		private void StarPopularity(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.player.GetNumberOfStars(StarType.Popularity) == 0 && this.CanPlayTopAction(GainType.Popularity))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Popularity], "Star Popularity"));
			}
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x0010F714 File Offset: 0x0010D914
		private void StarPower(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.player.GetNumberOfStars(StarType.Power) == 0 && this.player.Power >= 10 && this.CanPlayTopAction(GainType.Power))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Power], "Star Power"));
			}
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x00044722 File Offset: 0x00042922
		private void PowerIfWeak(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.player.Power <= 2 && this.CanPlayTopAction(GainType.Power))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Power], "Low Power -> Bolster"));
			}
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x0010F784 File Offset: 0x0010D984
		private void TradeForResource(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.CanPlayTopAction(GainType.AnyResource) && this.strategicAnalysis.resourceHighestPriority != ResourceType.combatCard && this.strategicAnalysis.workersOutOfBase > 0)
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.AnyResource], "Trade for resource"));
			}
		}



		// Token: 0x06002DC4 RID: 11716 RVA: 0x00044757 File Offset: 0x00042957
		private void CoinIfPoor(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.player.Coins == 0 && this.CanPlayTopAction(GainType.Coin))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Coin], "No cash -> get Coin"));
			}
		}

		private void MoveToBuild(SortedList<int, AiRecipe> actionOptions, int priority)
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
						if (aiPlayer.player.aiDifficulty == AIDifficulty.Hard && gainMove.Amount == 3 && aiPlayer.player.matFaction.faction == Faction.Polania)
						{
							this.MoveCharacterToEncounter(gainMove);
						}
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
			}
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x000447AD File Offset: 0x000429AD
		private void MoveByAnalysisPriority(SortedList<int, AiRecipe> actionOptions)
		{
			if (this.CanPlayTopAction(GainType.Move))
			{
				actionOptions.Add(this.strategicAnalysis.movePriorityHighest, new AiRecipe(this.AiTopActions[GainType.Move], "Move by priority"));
			}
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x0010FAA8 File Offset: 0x0010DCA8
		public ResourceType TradeResourceType()
		{
			ResourceType resourceType = this.strategicAnalysis.resourceHighestPriority;
			if (this.strategicAnalysis.resourceHighestPriorityNoProduce != ResourceType.combatCard)
			{
				resourceType = this.strategicAnalysis.resourceHighestPriorityNoProduce;
			}
			return resourceType;
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x0010FADC File Offset: 0x0010DCDC
		public int SelectTopActionFlavor(List<int> options)
		{
			if (options.Count <= 1)
			{
				return options[0];
			}
			foreach (int index in options)
			{
				if (this.AiActions[index].matSectionId == 4)
				{
					return index;
				}
			}
			if (this.AiActions[options[0]].topAction.Type == TopActionType.Bolster && this.player.matFaction.faction == Faction.Saxony)
			{
				int num = options[0];
				int num2 = options[1];
				if (this.player.Power <= this.player.combatCards.Count * 2)
				{
					return num;
				}
				return num2;
			}
			else
			{
				if (this.AiActions[options[0]].topAction.Type != TopActionType.Trade || this.player.matFaction.faction != Faction.Saxony)
				{
					return options[0];
				}
				int num3 = options[0];
				int num4 = options[1];
				if (this.TradeResourceType() == ResourceType.combatCard)
				{
					return num4;
				}
				return num3;
			}
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x0010FBB4 File Offset: 0x0010DDB4
		private void StarConstructions(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.strategicAnalysis.canBuild && this.CanPlayDownAction(this.AiActions[this.gainBuildingActionPosition[0]]))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainBuildingActionPosition)], "Star Constructions"));
			}
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x0010FC18 File Offset: 0x0010DE18
		private void StarMechs(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.strategicAnalysis.canDeploy && this.CanPlayDownAction(this.AiActions[this.gainMechActionPosition[0]]))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainMechActionPosition)], "Star Mechs"));
			}
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x0010FC7C File Offset: 0x0010DE7C
		private void StarRecruits(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.strategicAnalysis.canEnlist && this.CanPlayDownAction(this.AiActions[this.gainRecruitActionPosition[0]]))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainRecruitActionPosition)], "Star Recruits"));
			}
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x0010FCE0 File Offset: 0x0010DEE0
		private void StarUpgrades(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.strategicAnalysis.canUpgrade && this.CanPlayDownAction(this.AiActions[this.gainUpgradeActionPosition[0]]))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainUpgradeActionPosition)], "Star Upgrades"));
			}
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x0010FD44 File Offset: 0x0010DF44
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

		// Token: 0x06002DCF RID: 11727 RVA: 0x0010FDC8 File Offset: 0x0010DFC8
		private void ProduceLoop(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.strategicAnalysis.produceLoopPresent && this.CanPlayTopAction(GainType.Produce) && this.AiTopActions[GainType.Produce].topAction.CanPlayerPayActions())
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Produce], "Produce Loop"));
			}
		}

		// 3-turn cycle: determines which step we're on based on turn count after kickstart,
		// and injects the appropriate action at high priority.
		private void TurnCycle(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (!this.strategicAnalysis.turnCyclePresent || this.strategicAnalysis.turnCycleSteps == null)
			{
				return;
			}
			int cycleLength = this.strategicAnalysis.turnCycleSteps.Length;
			int step = (this.gameManager.TurnCount - this.strategicAnalysis.turnCycleStartTurn) % cycleLength;
			if (step < 0) step = 0;
			GainType cycleAction = this.strategicAnalysis.turnCycleSteps[step];

			switch (cycleAction)
			{
			case GainType.Produce:
				// Step 1: Produce food and metal (Produce top action, Upgrade bottom action on Engineering)
				if (this.CanPlayTopAction(GainType.Produce) && this.AiTopActions[GainType.Produce].topAction.CanPlayerPayActions())
				{
					actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Produce], "Turn Cycle: Produce"));
				}
				break;
			case GainType.AnyResource:
				// Step 2: Trade for metal (Trade top action)
				// Skip if mech star already earned
				if (this.player.GetNumberOfStars(StarType.Mechs) > 0)
				{
					break;
				}
				if (this.CanPlayTopAction(GainType.AnyResource) && this.player.Coins > 0)
				{
					actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.AnyResource], "Turn Cycle: Trade Metal")
					{
						tradeResource = new ResourceType[] { this.strategicAnalysis.turnCycleTradeResource, this.strategicAnalysis.turnCycleTradeResource }
					});
				}
				break;
			case GainType.Move:
				// Step 3: Move and Enlist (Move top action, Enlist bottom action on Engineering)
				// Skip if recruit star already earned
				if (this.player.GetNumberOfStars(StarType.Recruits) > 0)
				{
					break;
				}
				if (this.CanPlayTopAction(GainType.Move))
				{
					actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Move], "Turn Cycle: Move/Enlist"));
				}
				break;
			default:
				// Generic fallback
				if (this.AiTopActions.ContainsKey(cycleAction) && this.CanPlayTopAction(cycleAction))
				{
					actionOptions.Add(priority, new AiRecipe(this.AiTopActions[cycleAction], "Turn Cycle: " + cycleAction.ToString()));
				}
				break;
			}
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x0010FE20 File Offset: 0x0010E020
		private void RandomAllowed(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			int num = this.gameManager.random.Next(0, this.AiActions.Count - 1);
			while (this.player.matFaction.DidPlayerUsedMatLastTurn(this.AiActions[num].matSectionId, this.player.lastMatSection))
			{
				num = this.gameManager.random.Next(0, this.AiActions.Count - 1);
			}
			actionOptions.Add(priority, new AiRecipe(this.AiActions[num], "Random allowed"));
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x0010FEB8 File Offset: 0x0010E0B8
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

		private void ProduceForWorkerStar(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.strategicAnalysis.pursuingWorkerStar && this.strategicAnalysis.workersInaVillage > 0 && this.player.matPlayer.workers.Count < 8 && this.CanPlayTopAction(GainType.Produce))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Produce], "Produce for worker star"));
			}
		}

		private void TradeForBottomAction(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.CanPlayTopAction(GainType.AnyResource))
			{
				AiAction aiAction = this.AiTopActions[GainType.AnyResource];
				if (aiAction.downAction.GetGainAction(0).GainAvaliable())
				{
					// Default to paired resource
					ResourceType resourceType = ResourceType.combatCard;
					switch (aiAction.downAction.Type)
					{
					case DownActionType.Upgrade: resourceType = ResourceType.oil; break;
					case DownActionType.Deploy: resourceType = ResourceType.metal; break;
					case DownActionType.Build: resourceType = ResourceType.wood; break;
					case DownActionType.Enlist: resourceType = ResourceType.food; break;
					}

					// Improvement: If wood is low priority (as usual), and we need something else, trade for that!
					if (this.strategicAnalysis.resourcePrioritySorted.Count > 0)
					{
						ResourceType bestResource = this.strategicAnalysis.resourcePrioritySorted.Values[0];
						// Only switch away from Wood if it's actually low priority
						if (resourceType == ResourceType.wood && this.strategicAnalysis.resourceDemandPriority[ResourceType.wood] < 10)
						{
							resourceType = bestResource;
						}
					}

					actionOptions.Add(priority, new AiRecipe(aiAction, "Trade for bottom action resource")
					{
						tradeResource = new ResourceType[] { resourceType, resourceType }
					});
				}
			}
		}

		private ResourceType GetRequiredResourceTypeForDownAction(DownActionType type)
		{
			switch (type)
			{
			case DownActionType.Upgrade:
				return ResourceType.oil;
			case DownActionType.Deploy:
				return ResourceType.metal;
			case DownActionType.Build:
				return ResourceType.wood;
			case DownActionType.Enlist:
				return ResourceType.food;
			default:
				return ResourceType.combatCard;
			}
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x00110078 File Offset: 0x0010E278
		private void MovePreparation(SortedList<int, AiRecipe> actionOptions)
		{
			AiAction action = actionOptions.Values[0].action;
			if (action.topAction.Type == TopActionType.MoveGain && this.player.stars[StarType.Structures] == 0 && !action.downAction.CanPlayerPayActions() && this.CanPlayTopAction(GainType.AnyResource))
			{
				int amount = (int)action.downAction.GetPayAction(0).Amount;
				ResourceType resourceType = this.GetRequiredResourceTypeForDownAction(action.downAction.Type);
				
				if (amount - this.player.Resources(false)[resourceType] <= 2)
				{
					actionOptions.Add(this.MovePreparationPriority(), new AiRecipe(this.AiTopActions[GainType.AnyResource], "Move preparation")
					{
						tradeResource = new ResourceType[] { resourceType, resourceType }
					});
				}
			}
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x00110174 File Offset: 0x0010E374
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

		// Token: 0x06002DD4 RID: 11732 RVA: 0x001101FC File Offset: 0x0010E3FC
		protected void StrategicAnalysisRun()
		{
			int num = 500;
			if (this.player.matFaction.faction == Faction.Saxony)
			{
				num = 650;
			}
			else if (this.player.matFaction.faction == Faction.Crimea && this.player.matPlayer.matType == PlayerMatType.Innovative && this.gameManager.TurnCount >= 11)
			{
				num = 800;
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
				this.strategicAnalysis.Run(this, 130, 134, 130, num, 145);
				return;
			case Faction.Nordic:
				this.strategicAnalysis.Run(this, 102, 134, 125, num, 145);
				return;
			case Faction.Rusviet:
				this.strategicAnalysis.Run(this, 130, 134, 125, num, 145);
				return;
			case Faction.Crimea:
				this.strategicAnalysis.Run(this, 102, 134, 125, num, 145);
				return;
			case Faction.Saxony:
				this.strategicAnalysis.Run(this, 130, 134, 125, num, 145);
				return;
			}
			this.strategicAnalysis.Run(this, 130, 130, 125, num, 145);
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x0011037C File Offset: 0x0010E57C
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
					202
				},
				{
					DownActionType.Deploy,
					208
				},
				{
					DownActionType.Build,
					160
				},
				{
					DownActionType.Enlist,
					210
				}
			};
			int num3 = 112;
			switch (this.player.matPlayer.matType)
			{
			case PlayerMatType.Industrial:
				dictionary[DownActionType.Deploy] = 210;
				num3 = 208;
				dictionary[DownActionType.Upgrade] = 206;
				dictionary[DownActionType.Enlist] = 200;
				break;
			case PlayerMatType.Engineering:
				dictionary[DownActionType.Upgrade] = 202;
				dictionary[DownActionType.Deploy] = 208;
				dictionary[DownActionType.Enlist] = 210;
				num3 = 200;
				// When the 3-turn cycle is active, further suppress Upgrade and Build
				if (this.strategicAnalysis.turnCyclePresent)
				{
					dictionary[DownActionType.Upgrade] = 150;
					dictionary[DownActionType.Build] = 140;
				}
				break;
			case PlayerMatType.Patriotic:
			case PlayerMatType.Mechanical:
				num3 = 210;
				dictionary[DownActionType.Deploy] = 208;
				dictionary[DownActionType.Enlist] = 206;
				dictionary[DownActionType.Upgrade] = 200;
				break;
			case PlayerMatType.Agricultural:
				num3 = 210;
				dictionary[DownActionType.Enlist] = 208;
				dictionary[DownActionType.Deploy] = 206;
				dictionary[DownActionType.Upgrade] = 200;
				break;
			case PlayerMatType.Militant:
				dictionary[DownActionType.Deploy] = 210;
				dictionary[DownActionType.Enlist] = 208;
				dictionary[DownActionType.Upgrade] = 206;
				num3 = 200;
				break;
			case PlayerMatType.Innovative:
				dictionary[DownActionType.Upgrade] = 210;
				dictionary[DownActionType.Enlist] = 208;
				dictionary[DownActionType.Deploy] = 206;
				num3 = 200;
				break;
			}
			if (this.player.matFaction.faction == Faction.Crimea && this.player.matPlayer.matType == PlayerMatType.Innovative && this.gameManager.TurnCount >= 11)
			{
				dictionary[DownActionType.Enlist] = 285;
				num3 = 250;
			}
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
				PlayerMatType matType2 = this.player.matPlayer.matType;
				if (matType2 == PlayerMatType.Mechanical || matType2 == PlayerMatType.Agricultural || matType2 == PlayerMatType.Patriotic || matType2 == PlayerMatType.Militant)
				{
					int count2 = this.player.matFaction.mechs.Count;
					int recruitsEnlisted = this.player.matPlayer.RecruitsEnlisted;
					if (count2 >= 2 && recruitsEnlisted < 4)
					{
						dictionary[DownActionType.Deploy] = 200;
						dictionary[DownActionType.Enlist] = 280;
					}
					else if (count2 >= 2 && recruitsEnlisted >= 4)
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
			if (this.AiActions.Count < this.player.matPlayer.matPlayerSectionsCount * 2)
			{
				this.InitActions();
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
			if (this.player.matFaction.faction == Faction.Rusviet && this.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				// 1. Mech star completion (one more trade/deploy should do it)
				if (this.player.GetNumberOfStars(StarType.Mechs) == 0)
				{
					if (this.AiActions[this.gainMechActionPosition[0]].downAction.CanPlayerPayActions())
					{
						sortedList.Add(9300, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainMechActionPosition)], "Rusviet Engineering: Mech Star Priority"));
					}
					else if (this.CanPlayTopAction(GainType.AnyResource) && this.player.Coins > 0)
					{
						sortedList.Add(9250, new AiRecipe(this.AiTopActions[GainType.AnyResource], "Rusviet Engineering: Trade Metal for Mech")
						{
							tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
						});
					}
				}

				// 2. Worker star (via village production)
				if (this.player.GetNumberOfStars(StarType.Workers) == 0 && this.player.matPlayer.workers.Count < 8)
				{
					if (this.CanPlayTopAction(GainType.Produce) && this.strategicAnalysis.workersInaVillage >= 3)
					{
						sortedList.Add(9200, new AiRecipe(this.AiTopActions[GainType.Produce], "Rusviet Engineering: Produce for Worker Star"));
					}
				}

				// 3. Enlist Priority (Move/Enlist)
				if (this.player.GetNumberOfStars(StarType.Recruits) == 0)
				{
					if (this.AiActions[this.gainRecruitActionPosition[0]].downAction.CanPlayerPayActions())
					{
						sortedList.Add(9150, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainRecruitActionPosition)], "Rusviet Engineering: Enlist Priority"));
					}
					else if (this.CanPlayTopAction(GainType.AnyResource) && this.player.Coins > 0 && this.player.Resources(false)[ResourceType.food] < 3)
					{
						// 4. Fallback trade for food (if less than 3 food)
						sortedList.Add(9100, new AiRecipe(this.AiTopActions[GainType.AnyResource], "Rusviet Engineering: Trade Food for Enlist")
						{
							tradeResource = new ResourceType[] { ResourceType.food, ResourceType.food }
						});
					}
				}
			}
			if (this.player.matFaction.faction == Faction.Nordic)
			{
				int count3 = this.player.matFaction.mechs.Count;
				if (count3 < 2 && this.AiActions[this.gainMechActionPosition[0]].downAction.CanPlayerPayActions())
				{
					sortedList.Add(9000, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainMechActionPosition)], "Nordic: Deploy Mech " + (count3 + 1).ToString() + "/2"));
				}
				else if (count3 >= 2 && this.player.matPlayer.matPlayerSectionsCount <= 4 && this.strategicAnalysis.factoryDistance <= (int)this.player.character.MaxMoveCount && (this.gameManager.gameBoard.factory.Owner == this.player || this.gameManager.gameBoard.factory.GetOwnerUnitCount() == 0) && this.CanPlayTopAction(GainType.Move))
				{
					sortedList.Add(8500, new AiRecipe(this.AiTopActions[GainType.Move], "Nordic: Move to Factory for card"));
				}
			}
			if (this.player.matFaction.faction == Faction.Saxony)
			{
				int count4 = this.player.matPlayer.workers.Count;
				int upgradesDone = this.player.matPlayer.UpgradesDone;
				int count5 = this.player.matFaction.mechs.Count;
				if (count4 < 5)
				{
					if (this.CanPlayTopAction(GainType.Produce) && this.strategicAnalysis.workersInaVillage > 0)
					{
						sortedList.Add(9500, new AiRecipe(this.AiTopActions[GainType.Produce], "Saxony: Produce workers (" + count4.ToString() + "/5)"));
					}
				}
				else if (upgradesDone < 1 && this.AiActions[this.gainUpgradeActionPosition[0]].downAction.CanPlayerPayActions())
				{
					sortedList.Add(9400, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainUpgradeActionPosition)], "Saxony: First upgrade"));
				}
				else if (upgradesDone >= 1 && count5 < 2 && this.AiActions[this.gainMechActionPosition[0]].downAction.CanPlayerPayActions())
				{
					sortedList.Add(9300, new AiRecipe(this.AiActions[this.SelectTopActionFlavor(this.gainMechActionPosition)], "Saxony: Deploy Mech " + (count5 + 1).ToString() + "/2"));
				}
				else if (upgradesDone >= 1 && count5 >= 2)
				{
					if (this.player.Power < 3 && this.CanPlayTopAction(GainType.Power))
					{
						sortedList.Add(9200, new AiRecipe(this.AiTopActions[GainType.Power], "Saxony: Gain power for combat"));
					}
					else if (count5 < 3 && this.strategicAnalysis.enemyCanBeAttackedBy.Count > 0 && this.AiActions[this.gainMechActionPosition[0]].downAction.CanPlayerPayActions())
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
			if (this.player.matFaction.faction == Faction.Crimea && this.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				bool isObjImpossible = this.AreAllObjectivesImpossible();
				
				// 1. Mechs Star Priority (Deploy mechs - StarType is Mechs)
				if (this.player.stars[StarType.Mechs] == 0 && this.player.matFaction.mechs.Count < 4)
				{
					AiAction mechAction = this.AiActions[this.SelectTopActionFlavor(this.gainMechActionPosition)];
					if (mechAction.downAction.CanPlayerPayActions())
					{
						sortedList.Add(9990, new AiRecipe(mechAction, "Crimea Innovative: Mechs Star Priority"));
					}
					else if (this.CanPlayTopAction(GainType.AnyResource) && this.player.Coins > 0)
					{
						sortedList.Add(9985, new AiRecipe(this.AiTopActions[GainType.AnyResource], "Crimea Innovative: Trade Metal for Mechs") { tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal } });
					}
				}

				// 2. Recruits Star Priority (Enlist recruits - StarType is Recruits)
				if (this.player.stars[StarType.Recruits] == 0 && this.player.matPlayer.RecruitsEnlisted < 4)
				{
					AiAction enlistAction = this.AiActions[this.SelectTopActionFlavor(this.gainRecruitActionPosition)];
					bool canPayEnlist = enlistAction.downAction.CanPlayerPayActions();
					bool canPayWithCoercion = (this.player.combatCards.Count > 0 && this.player.Resources(false)[ResourceType.food] >= Math.Max(0, (int)enlistAction.downAction.GetPayAction(0).Amount - 1));

					if (canPayEnlist || canPayWithCoercion)
					{
						sortedList.Add(9980, new AiRecipe(enlistAction, "Crimea Innovative: Recruits Star Priority (Coercion-aware)"));
					}
					else if (this.CanPlayTopAction(GainType.AnyResource) && this.player.Coins > 0)
					{
						sortedList.Add(9975, new AiRecipe(this.AiTopActions[GainType.AnyResource], "Crimea Innovative: Trade Food for Enlist") { tradeResource = new ResourceType[] { ResourceType.food, ResourceType.food } });
					}
					else if (this.CanPlayTopAction(GainType.Produce) && this.strategicAnalysis.resourceAccess[ResourceType.food] > 0)
					{
						sortedList.Add(9972, new AiRecipe(this.AiTopActions[GainType.Produce], "Crimea Innovative: Produce Food for Enlist"));
					}
				}

				// 3. Workers Star Priority (8 workers - StarType is Workers)
				if (this.player.stars[StarType.Workers] == 0 && this.player.matPlayer.workers.Count < 8)
				{
					if (this.CanPlayTopAction(GainType.Produce) && this.strategicAnalysis.workersInaVillage > 0)
					{
						sortedList.Add(9970, new AiRecipe(this.AiTopActions[GainType.Produce], "Crimea Innovative: Workers Star Priority"));
					}
				}

				// 4. (Removed Crimea-specific Obj 13 logic - generalized below)


				// Pivot for impossible objectives
				if (isObjImpossible)
				{
					AiAction upgradeAction = this.AiActions[this.SelectTopActionFlavor(this.gainUpgradeActionPosition)];
					if (this.player.stars[StarType.Upgrades] == 0 && upgradeAction.downAction.CanPlayerPayActions())
					{
						sortedList.Add(9700, new AiRecipe(upgradeAction, "Crimea Innovative: Impossible Objectives -> Upgrade Focus"));
					}
					if (this.player.stars[StarType.Power] == 0 && this.player.Power < 16 && this.CanPlayTopAction(GainType.Power))
					{
						sortedList.Add(9650, new AiRecipe(this.AiTopActions[GainType.Power], "Crimea Innovative: Impossible Objectives -> Power Focus"));
					}
				}

				if (this.gameManager.TurnCount >= 11 && (this.player.stars[StarType.Combat] < 2 || (this.player.objectiveCards != null && this.player.objectiveCards.Count > 0 && this.player.stars[StarType.Objective] == 0 && !isObjImpossible)))
				{
					if (this.CanPlayTopAction(GainType.Move))
					{
						sortedList.Add(9800, new AiRecipe(this.AiTopActions[GainType.Move], "Crimea Innovative: Combat/Objective Focus"));
					}
				}

				// 5. Force Move to Farm if we need food and workers are lost/stolen
				if (this.player.stars[StarType.Recruits] == 0 && this.player.Resources(false)[ResourceType.food] < 2)
				{
					bool hasWorkersOnFarm = false;
					foreach (var worker in this.player.matPlayer.workers)
					{
						if (worker.position != null && worker.position.hexType == HexType.farm)
						{
							hasWorkersOnFarm = true;
							break;
						}
					}
					if (!hasWorkersOnFarm && this.CanPlayTopAction(GainType.Move))
					{
						sortedList.Add(9110, new AiRecipe(this.AiTopActions[GainType.Move], "Crimea Innovative: Move Workers to Farm for Food"));
					}
				}
			}

			// General Objective 13 Logic for ALL AI bots
			if (this.player.stars[StarType.Objective] == 0 && this.player.objectiveCards != null)
			{
				foreach (var obj in this.player.objectiveCards)
				{
					if (obj.CardId == 13 && obj.status == ObjectiveCard.ObjectiveStatus.Open)
					{
						if (this.CanPlayTopAction(GainType.Move))
						{
							// Find hub hex with most units
							GameHex hub = null;
							int maxUnits = -1;
							Dictionary<GameHex, int> hexUnitCount = new Dictionary<GameHex, int>();
							
							if (this.player.character.position != null)
							{
								if (!hexUnitCount.ContainsKey(this.player.character.position)) hexUnitCount[this.player.character.position] = 0;
								hexUnitCount[this.player.character.position]++;
							}
							foreach (var m in this.player.matFaction.mechs)
							{
								if (m.position != null)
								{
									if (!hexUnitCount.ContainsKey(m.position)) hexUnitCount[m.position] = 0;
									hexUnitCount[m.position]++;
								}
							}
							foreach (var w in this.player.matPlayer.workers)
							{
								if (w.position != null)
								{
									if (!hexUnitCount.ContainsKey(w.position)) hexUnitCount[w.position] = 0;
									hexUnitCount[w.position]++;
								}
							}

							foreach (var kvp in hexUnitCount)
							{
								if (kvp.Value > maxUnits)
								{
									maxUnits = kvp.Value;
									hub = kvp.Key;
								}
							}

							if (hub != null)
							{
								Unit mover = null;
								if (this.player.character.position != hub) mover = this.player.character;
								else
								{
									foreach (var m in this.player.matFaction.mechs)
									{
										if (m.position != null && m.position != hub) { mover = m; break; }
									}
									if (mover == null)
									{
										foreach (var w in this.player.matPlayer.workers)
										{
											if (w.position != null && w.position != hub) { mover = w; break; }
										}
									}
								}

								if (mover != null)
								{
									if (!this.strategicAnalysis.movePriority.ContainsKey(mover)) this.strategicAnalysis.movePriority.Add(mover, 9960);
									else this.strategicAnalysis.movePriority[mover] = 9960;
									
									if (!this.strategicAnalysis.moveTarget.ContainsKey(mover)) this.strategicAnalysis.moveTarget.Add(mover, new List<GameHex> { hub });
									else this.strategicAnalysis.moveTarget[mover] = new List<GameHex> { hub };
									
									// Re-sort move list
									this.strategicAnalysis.movePrioritySorted.Clear();
									int moveTieBreaker = 0;
									foreach (var kvp in this.strategicAnalysis.movePriority)
									{
										this.strategicAnalysis.movePrioritySorted.Add(kvp.Value * 100 + moveTieBreaker++, kvp.Key);
									}
								}
							}
							sortedList.Add(9960, new AiRecipe(this.AiTopActions[GainType.Move], "Objective 13 Priority (Grouping units)"));
						}
						break;
					}
				}
			}

			// Removed if (sortedList.Count == 0) to allow star actions and factory card usage to be considered alongside faction priorities.
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
				// 3-turn cycle: runs at priority 220 to override star actions while active
				this.TurnCycle(sortedList, 220);
				this.CoinIfPoor(sortedList, 142);
				if (this.player.matFaction.faction == Faction.Polania && this.player.matPlayer.matType == PlayerMatType.Agricultural)
				{
					this.PowerIfWeak(sortedList, 114);
				}
				else
				{
					this.PowerIfWeak(sortedList, 140);
				}
				if ((this.player.matFaction.faction != Faction.Saxony || this.player.matPlayer.matType != PlayerMatType.Engineering) && !this.strategicAnalysis.turnCyclePresent)
				{
					this.TradeForBottomAction(sortedList, 136);
				}
				this.ProduceForDemand(sortedList, 132);
				this.ProduceForWorkerStar(sortedList, 205);
				this.MoveByAnalysisPriority(sortedList);
				this.TradeForResource(sortedList, 120);
				this.StarPower(sortedList, num3);
				this.RandomAllowed(sortedList, 100);
				this.StarPopularity(sortedList, 90);
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
				int factoryPriority = 250;
				if (this.player.matFaction.faction == Faction.Crimea && this.player.matPlayer.matType == PlayerMatType.Innovative && this.AreAllObjectivesImpossible())
				{
					factoryPriority = 9000; // Boost factory card usage if objectives are impossible
				}
				this.FactoryCardUsage(sortedList, factoryPriority);
			}
			SortedList<int, AiRecipe> sortedList2 = new SortedList<int, AiRecipe>(new InvertedComparer());
			foreach (KeyValuePair<int, AiRecipe> keyValuePair2 in sortedList)
			{
				int num4 = keyValuePair2.Key;
				AiAction action = keyValuePair2.Value.action;

				// Boost any action that finishes a star, especially the 6th star.
				int starsEarned = this.player.GetNumberOfStars();
				bool finishesStar = false;
				if (action.downAction != null) // Normal and Factory cards with bottom actions
				{
					if (action.downAction.Type == DownActionType.Upgrade && this.player.matPlayer.UpgradesDone == 5 && this.player.stars[StarType.Upgrades] == 0) finishesStar = true;
					else if (action.downAction.Type == DownActionType.Deploy && this.player.matFaction.mechs.Count == 3 && this.player.stars[StarType.Mechs] == 0) finishesStar = true;
					else if (action.downAction.Type == DownActionType.Enlist && this.player.matPlayer.RecruitsEnlisted == 3 && this.player.stars[StarType.Recruits] == 0) finishesStar = true;
					else if (action.downAction.Type == DownActionType.Build && this.player.matPlayer.buildings.Count == 3 && this.player.stars[StarType.Structures] == 0) finishesStar = true;
				}
				if (action.topAction != null)
				{
					if (action.topAction.Type == TopActionType.Produce && this.player.matPlayer.workers.Count == 7 && this.player.stars[StarType.Workers] == 0) finishesStar = true;
					else if (this.player.stars[StarType.Power] == 0 && this.player.Power >= 14 && action.topAction.GetGainAction(action.gainActionId).GetGainType() == GainType.Power) finishesStar = true;
					else if (this.player.stars[StarType.Popularity] == 0 && this.player.Popularity >= 16 && action.topAction.GetGainAction(action.gainActionId).GetGainType() == GainType.Popularity) finishesStar = true;
				}

				if (finishesStar)
				{
					num4 += (starsEarned == 5) ? 10000 : 1000;
				}

				if (action.matSectionId == 4)
				{
					if (this.player.matFaction.faction == Faction.Rusviet && this.player.matPlayer.matType == PlayerMatType.Engineering)
					{
						bool productive = false;
						GainType gt = action.topAction.GetGainAction(action.gainActionId).GetGainType();
						if (gt == GainType.Recruit && this.player.GetNumberOfStars(StarType.Recruits) == 0) productive = true;
						else if (gt == GainType.Upgrade && this.player.GetNumberOfStars(StarType.Upgrades) == 0) productive = true;
						else if (gt == GainType.Mech && this.player.GetNumberOfStars(StarType.Mechs) == 0) productive = true;
						else if (gt == GainType.Building && this.player.GetNumberOfStars(StarType.Structures) == 0) productive = true;
						else if (gt == GainType.Power && this.player.Power < 16) productive = true;
						else if (gt == GainType.Popularity && this.player.Popularity < 18) productive = true;
						else if (gt == GainType.Coin || gt == GainType.CombatCard || gt == GainType.Resource || gt == GainType.AnyResource) productive = true;
						else if (gt == GainType.Worker && this.player.matPlayer.workers.Count < 5) productive = true;

						if (productive)
						{
							num4 += 1000;
							FactoryCard fc = this.player.matPlayer.GetPlayerMatSection(4) as FactoryCard;
							if (fc != null && fc.CardId == 2)
							{
								if (gt == GainType.Recruit && this.player.GetNumberOfStars(StarType.Recruits) == 0)
								{
									num4 += 100; // Enlist first
								}
								else if (gt == GainType.Upgrade && this.player.GetNumberOfStars(StarType.Recruits) > 0 && this.player.GetNumberOfStars(StarType.Upgrades) == 0)
								{
									num4 += 100; // Then Upgrade
								}
								else
								{
									num4 -= 500; // Deprioritize if neither star needed or out of order
								}
							}
						}
					}
					else if (this.player.Power >= 1 && this.player.Popularity >= 1 && this.player.Coins >= 1)
					{
						num4 += 200;
					}
				}
				if (action.topAction != null)
				{
					for (int i = 0; i < action.topAction.GetNumberOfPayActions(); i++)
					{
						PayType payType = action.topAction.GetPayAction(i).GetPayType();
						int amount = (int)action.topAction.GetPayAction(i).GetAmount();
						if (payType == PayType.Power && this.player.Power - amount < 1)
						{
							num4 -= 1000;
						}
						if (payType == PayType.Popularity && this.player.Popularity - amount < 1)
						{
							num4 -= 1000;
						}
						if (payType == PayType.Coin && this.player.Coins - amount < 1)
						{
							num4 -= 1000;
						}
					}
				}
				while (sortedList2.ContainsKey(num4))
				{
					num4--;
				}
				sortedList2.Add(num4, keyValuePair2.Value);
			}
			sortedList = sortedList2;
			this.Log(sortedList);
			return sortedList.Values[0];
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x00111484 File Offset: 0x0010F684
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

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000447DF File Offset: 0x000429DF
		public void Log(string msg)
		{
			if (AiPlayer.LogMessage != null)
			{
				AiPlayer.LogMessage(msg);
			}
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x00111634 File Offset: 0x0010F834
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

		// Token: 0x06002DD9 RID: 11737 RVA: 0x000447F3 File Offset: 0x000429F3
		public static void AttachLogDelegate(AiPlayer.LogDelegate func)
		{
			AiPlayer.LogMessage = null;
			AiPlayer.LogMessage += func;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x00111710 File Offset: 0x0010F910
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





		// Token: 0x06002DDD RID: 11741 RVA: 0x00044801 File Offset: 0x00042A01
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

		// Token: 0x06002DDE RID: 11742 RVA: 0x00111D10 File Offset: 0x0010FF10
		private void ProducePreparation(SortedList<int, AiRecipe> actionOptions)
		{
			AiAction action = actionOptions.Values[0].action;
			if (action.topAction.Type == TopActionType.Produce && this.player.stars[StarType.Recruits] == 0 && !action.downAction.CanPlayerPayActions() && this.CanPlayTopAction(GainType.AnyResource))
			{
				int amount = (int)action.downAction.GetPayAction(0).Amount;
				ResourceType resourceType = this.GetRequiredResourceTypeForDownAction(action.downAction.Type);
				
				if (amount - this.player.Resources(false)[resourceType] <= 2)
				{
					actionOptions.Add(this.ProducePreparationPriority(), new AiRecipe(this.AiTopActions[GainType.AnyResource], "Produce preparation")
					{
						tradeResource = new ResourceType[] { resourceType, resourceType }
					});
				}
			}
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x00111E0C File Offset: 0x0011000C
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

		private void FactoryCardUsage(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (this.player.matPlayer.matPlayerSectionsCount < 5)
			{
				return;
			}
			foreach (AiAction action in this.AiActions)
			{
				if (action.matSectionId == 4 && this.CanPlayDownAction(action))
				{
					actionOptions.Add(priority, new AiRecipe(action, "Factory Card Usage"));
				}
			}
		}

		private bool IsEncounterOptionAcceptable(EncounterCard encounterCard, int optionIndex)
		{
			if (this.player.Coins <= 4)
			{
				return false;
			}
			SectionAction action = encounterCard.GetAction(optionIndex);
			if (!action.CanPlayerPayActions())
			{
				return false;
			}
			for (int i = 0; i < action.gainActionsCount; i++)
			{
				switch (action.GetGainAction(i).GetGainType())
				{
				case GainType.Power:
					if (this.player.GetNumberOfStars(StarType.Power) > 0 || this.player.Power >= 16)
					{
						return false;
					}
					break;
				case GainType.Popularity:
					if (this.player.GetNumberOfStars(StarType.Popularity) > 0 || this.player.Popularity >= 18)
					{
						return false;
					}
					break;
				case GainType.Upgrade:
					if (this.player.GetNumberOfStars(StarType.Upgrades) > 0)
					{
						return false;
					}
					break;
				case GainType.Mech:
					if (this.player.GetNumberOfStars(StarType.Mechs) > 0 || this.player.matFaction.mechs.Count >= 4)
					{
						return false;
					}
					break;
				case GainType.Worker:
					if (this.player.GetNumberOfStars(StarType.Workers) > 0 || this.player.matPlayer.workers.Count >= 8)
					{
						return false;
					}
					break;

				case GainType.Recruit:
					if (this.player.GetNumberOfStars(StarType.Recruits) > 0 || this.player.matPlayer.RecruitsEnlisted >= 4)
					{
						return false;
					}
					break;
				case GainType.Resource:
					ResourceType rt = (ResourceType)action.GetGainAction(i).Amount;
					if (rt == ResourceType.metal && this.player.GetNumberOfStars(StarType.Mechs) > 0) return false;
					if (rt == ResourceType.food && this.player.GetNumberOfStars(StarType.Recruits) > 0) return false;
					if (rt == ResourceType.oil && this.player.GetNumberOfStars(StarType.Upgrades) > 0) return false;
					if (rt == ResourceType.wood && this.player.GetNumberOfStars(StarType.Structures) > 0) return false;
					break;
				case GainType.Building:
				{
					if (this.player.matPlayer.buildings.Count >= 4)
					{
						return false;
					}
					PlayerMatType matType = this.player.matPlayer.matType;
					if (matType != PlayerMatType.Agricultural && matType != PlayerMatType.Engineering && matType != PlayerMatType.Mechanical)
					{
						return false;
					}
					Faction faction = this.player.matFaction.faction;
					if (faction != Faction.Polania && faction != Faction.Nordic)
					{
						return false;
					}
					break;
				}

				}
			}
			return true;
		}

		private int CalculateEncounterWeight(SectionAction action)
		{
			if (this.player.matFaction.faction == Faction.Crimea && this.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				return this.CalculateEncounterWeightCrimeaInnovative(action);
			}
			int weight = 0;
			for (int i = 0; i < action.gainActionsCount; i++)
			{
				GainAction ga = action.GetGainAction(i);
				if (ga == null) continue;
				GainType gt = ga.GetGainType();
				switch (gt)
				{
				case GainType.Recruit:
					if (this.player.GetNumberOfStars(StarType.Recruits) == 0)
					{
						weight += 200;
					}
					break;
				case GainType.Mech:
					if (this.player.GetNumberOfStars(StarType.Mechs) == 0)
					{
						weight += 200;
					}
					break;
				case GainType.Upgrade:
					if (this.player.GetNumberOfStars(StarType.Upgrades) == 0)
					{
						weight += 150;
					}
					break;
				case GainType.Building:
					if (this.player.GetNumberOfStars(StarType.Structures) == 0)
					{
						weight += 100;
					}
					break;
				case GainType.Power:
					// Power star is 16 power.
					if (this.player.Power < 16)
					{
						weight += 120;
					}
					break;
				case GainType.Popularity:
					// Popularity star is 18 popularity.
					if (this.player.Popularity < 18)
					{
						weight += 100;
					}
					break;
				case GainType.CombatCard:
					weight += 80;
					break;
				case GainType.Coin:
					weight += 60;
					break;
				case GainType.Resource:
				{
					if (!(ga is GainResource))
					{
						weight += 40;
						break;
					}
					GainResource gainResource = (GainResource)ga;
					if (gainResource.ResourceToGain == ResourceType.metal && this.player.GetNumberOfStars(StarType.Mechs) == 0)
					{
						weight += 110;
					}
					else if (gainResource.ResourceToGain == ResourceType.food && this.player.GetNumberOfStars(StarType.Recruits) == 0)
					{
						weight += 110;
					}
					else if (gainResource.ResourceToGain == ResourceType.oil && this.player.GetNumberOfStars(StarType.Upgrades) == 0)
					{
						weight += 90;
					}
					else if (gainResource.ResourceToGain == ResourceType.wood && this.player.GetNumberOfStars(StarType.Structures) == 0)
					{
						weight += 70;
					}
					else
					{
						weight += 20;
					}
					break;
				}
				case GainType.AnyResource:
					weight += 100;
					break;
				case GainType.Worker:
					if (this.player.matPlayer.workers.Count < 5)
					{
						weight += 100;
					}
					break;
				default:
					weight += 30;
					break;
				}
			}
			return weight;
		}

		private int CalculateEncounterWeightCrimeaInnovative(SectionAction action)
		{
			int weight = 0;
			for (int i = 0; i < action.gainActionsCount; i++)
			{
				GainAction ga = action.GetGainAction(i);
				if (ga == null) continue;
				GainType gt = ga.GetGainType();
				switch (gt)
				{
				case GainType.Power:
					if (this.player.Power < 16) weight += 200;
					break;
				case GainType.CombatCard:
					weight += 190;
					break;
				case GainType.Resource:
					if (ga is GainResource)
					{
						GainResource gr = (GainResource)ga;
						if (gr.ResourceToGain == ResourceType.oil && this.player.GetNumberOfStars(StarType.Upgrades) == 0) weight += 180;
						else weight += 20;
					}
					else weight += 20;
					break;
				case GainType.Upgrade:
					if (this.player.GetNumberOfStars(StarType.Upgrades) == 0) weight += 150;
					break;
				case GainType.Popularity:
					if (this.player.Popularity < 18) weight += 100;
					break;
				case GainType.Coin:
					weight += 60;
					break;
				default:
					weight += 20;
					break;
				}
			}
			return weight;
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x001120C8 File Offset: 0x001102C8
		private int SelectBestFactoryCard(List<FactoryCard> factoryCards)
		{
			List<int> list = new List<int>();
			int numberOfStars = this.player.GetNumberOfStars(StarType.Combat);
			int numberOfStars2 = this.player.GetNumberOfStars(StarType.Mechs);
			int numberOfStars3 = this.player.GetNumberOfStars(StarType.Recruits);
			int num = numberOfStars2 + numberOfStars3;
			int count = this.player.matFaction.mechs.Count;
			int count2 = this.player.matPlayer.buildings.Count;
			int numberOfStars4 = this.player.GetNumberOfStars(StarType.Upgrades);
			int coins = this.player.Coins;
			int power = this.player.Power;
			int count3 = this.player.combatCards.Count;
			bool flag = this.player.matFaction.faction == Faction.Saxony;
			bool flag2 = this.player.matFaction.faction == Faction.Polania || this.player.matFaction.faction == Faction.Nordic;
			bool flag3 = this.player.matPlayer.matType == PlayerMatType.Agricultural || this.player.matPlayer.matType == PlayerMatType.Engineering || this.player.matPlayer.matType == PlayerMatType.Mechanical;
			
			if (this.player.matFaction.faction == Faction.Rusviet && this.player.matPlayer.matType == PlayerMatType.Engineering)
			{
				if (this.player.matPlayer.RecruitsEnlisted < 3)
				{
					if (this.player.Popularity > 2) list.Add(2);
					list.Add(12);
					list.Add(8);
					list.Add(18);
				}
				else
				{
					list.AddRange(new int[] { 15, 16, 17 });
					if (this.player.GetNumberOfStars(StarType.Combat) >= 2)
					{
						list.AddRange(new int[] { 3, 4, 5, 6 });
					}
					list.AddRange(new int[] { 9, 2 });
				}
			}
			else if (this.player.matFaction.faction == Faction.Crimea && this.player.matPlayer.matType == PlayerMatType.Innovative)
			{
				int[] avoid = { 9, 21, 16 };
				int[] impossible = { 7, 15, 20, 8, 14, 17, 23 };
				int obj1 = (this.player.objectiveCards != null && this.player.objectiveCards.Count > 0) ? this.player.objectiveCards[0].CardId : -1;
				int obj2 = (this.player.objectiveCards != null && this.player.objectiveCards.Count > 1) ? this.player.objectiveCards[1].CardId : -1;

				bool obj1Achieved = (this.player.objectiveCards != null && this.player.objectiveCards.Count > 0 && (this.player.objectiveCards[0].status == ObjectiveCard.ObjectiveStatus.Completed || this.player.objectiveCards[0].CheckCondition() || (obj1 == 22 && this.player.matPlayer.UpgradesDone == 0)));
				bool obj2Achieved = (this.player.objectiveCards != null && this.player.objectiveCards.Count > 1 && (this.player.objectiveCards[1].status == ObjectiveCard.ObjectiveStatus.Completed || this.player.objectiveCards[1].CheckCondition() || (obj2 == 22 && this.player.matPlayer.UpgradesDone == 0)));

				bool isObj1Impossible = System.Array.IndexOf(impossible, obj1) >= 0;
				bool isObj2Impossible = System.Array.IndexOf(impossible, obj2) >= 0 || (this.player.objectiveCards != null && this.player.objectiveCards.Count < 2);
				bool isObj1Avoid = System.Array.IndexOf(avoid, obj1) >= 0;
				bool isObj2Avoid = System.Array.IndexOf(avoid, obj2) >= 0;

				bool isObj1Easy = obj1Achieved || obj1 == 13 || obj1 == 22;
				bool isObj2Easy = obj2Achieved || obj2 == 13 || obj2 == 22;

				if (obj1Achieved || obj2Achieved || isObj1Easy || isObj2Easy)
				{
					list.AddRange(new int[] { 15, 16, 6, 5, 4, 3 });
				}
				else if (isObj1Impossible && isObj2Impossible)
				{
					list.AddRange(new int[] { 8, 9, 17, 18, 2 });
				}
				else if ((isObj1Impossible && isObj1Easy == false && isObj2Avoid) || (isObj2Impossible && isObj2Easy == false && isObj1Avoid))
				{
					list.AddRange(new int[] { 8, 1 });
				}
				else if ((obj1 == 18 && !isObj1Impossible && !isObj1Avoid) || (obj2 == 18 && !isObj2Impossible && !isObj2Avoid))
				{
					list.AddRange(new int[] { 3, 5, 9 });
				}
				else if ((obj1 == 12 && !isObj1Impossible && !isObj1Avoid) || (obj2 == 12 && !isObj2Impossible && !isObj2Avoid))
				{
					list.AddRange(new int[] { 15, 6, 4, 16 });
				}
				else
				{
					list.AddRange(new int[] { 15, 16, 6, 5, 4, 3 });
				}
			}
			else
			{
				if (flag || numberOfStars == 0)
				{
					list.Add(15);
				}
				if (numberOfStars3 == 0)
				{
					list.Add(18);
				}
				if (numberOfStars3 == 0 && coins >= 4)
				{
					list.Add(12);
				}
				if (count < 3 && coins >= 4)
				{
					list.Add(10);
					list.Add(13);
				}
				if (count2 < 3 && flag3 && flag2)
				{
					list.Add(11);
					list.Add(13);
				}
				if (numberOfStars4 == 0 && coins >= 4)
				{
					list.Add(9);
				}
				if (numberOfStars4 == 0 && count3 >= 2)
				{
					list.Add(17);
				}
				if (!flag && numberOfStars == 2 && num < 2 && count3 >= 2 && coins >= 3)
				{
					list.Add(8);
					list.Add(1);
				}
				if (!flag && numberOfStars == 2)
				{
					if (power >= 4)
					{
						list.Add(5);
					}
					if (count3 >= 2)
					{
						list.Add(3);
					}
					if (power >= 4)
					{
						list.Add(6);
					}
					if (count3 >= 2)
					{
						list.Add(4);
					}
				}
				list.Add(7);
				list.Add(2);
				list.Add(16);
				list.Add(14);
			}
			foreach (int num2 in list)
			{
				for (int i = 0; i < factoryCards.Count; i++)
				{
					if (factoryCards[i].CardId == num2)
					{
						return i;
					}
				}
			}
			return 0;
		}

		private bool AreAllObjectivesImpossible()
		{
			int[] impossible = { 7, 15, 20, 8, 14, 17, 23 };
			if (this.player.objectiveCards == null || this.player.objectiveCards.Count == 0)
			{
				return false;
			}
			foreach (var obj in this.player.objectiveCards)
			{
				if (obj.status == ObjectiveCard.ObjectiveStatus.Open)
				{
					if (System.Array.IndexOf(impossible, obj.CardId) < 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04001EFA RID: 7930
		public readonly Player player;

		// Token: 0x04001EFB RID: 7931
		public List<AiAction> AiActions = new List<AiAction>();

		// Token: 0x04001EFC RID: 7932
		public Dictionary<GainType, AiAction> AiTopActions = new Dictionary<GainType, AiAction>();

		// Token: 0x04001EFD RID: 7933
		public AiStrategicAnalysis strategicAnalysis;

		// Token: 0x04001EFE RID: 7934
		public AiKickStart kickstarter;

		// Token: 0x04001EFF RID: 7935
		public AiKickStartAdv kickstarterAdv;

		// Token: 0x04001F00 RID: 7936
		public List<int> gainUpgradeActionPosition = new List<int>();

		// Token: 0x04001F01 RID: 7937
		public List<int> gainMechActionPosition = new List<int>();

		// Token: 0x04001F02 RID: 7938
		public List<int> gainBuildingActionPosition = new List<int>();

		// Token: 0x04001F03 RID: 7939
		public List<int> gainRecruitActionPosition = new List<int>();

		// Token: 0x04001F04 RID: 7940
		public HashSet<GainType> forbiddenActions = new HashSet<GainType>();

		// Token: 0x04001F05 RID: 7941
		public bool WaitingForCard;

		// Token: 0x04001F06 RID: 7942
		private bool bottomActionExecuted;

		// Token: 0x04001F07 RID: 7943
		private GameManager gameManager;

	}
}

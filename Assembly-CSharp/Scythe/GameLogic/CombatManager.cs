using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic
{
	// Token: 0x020005AB RID: 1451
	public class CombatManager : IXmlSerializable
	{
		// Token: 0x14000137 RID: 311
		// (add) Token: 0x06002DF5 RID: 11765 RVA: 0x00116168 File Offset: 0x00114368
		// (remove) Token: 0x06002DF6 RID: 11766 RVA: 0x001161A0 File Offset: 0x001143A0
		public event CombatManager.CombatStageChanged OnCombatStageChanged;

		// Token: 0x14000138 RID: 312
		// (add) Token: 0x06002DF7 RID: 11767 RVA: 0x001161D8 File Offset: 0x001143D8
		// (remove) Token: 0x06002DF8 RID: 11768 RVA: 0x00116210 File Offset: 0x00114410
		public event CombatManager.WithdrawUnits OnWithdrawUnits;

		// Token: 0x14000139 RID: 313
		// (add) Token: 0x06002DF9 RID: 11769 RVA: 0x00116248 File Offset: 0x00114448
		// (remove) Token: 0x06002DFA RID: 11770 RVA: 0x00116280 File Offset: 0x00114480
		public event CombatManager.OvertakeTheOwnerPositions TakeOwnersPositions;

		// Token: 0x06002DFB RID: 11771 RVA: 0x001162B8 File Offset: 0x001144B8
		public CombatManager(GameManager gameManager)
		{
			this.retreatMove = new GainMove(gameManager, 1, 0);
			this.gameManager = gameManager;
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x00044A47 File Offset: 0x00042C47
		public void SetAttacker(Player attacker)
		{
			this.attacker = attacker;
			this.actualPlayer = attacker;
			this.stage = CombatStage.SelectingBattlefield;
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x00044A5E File Offset: 0x00042C5E
		public bool WorkersRetreat()
		{
			return this.workersWithoutCombatSupport;
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x00116308 File Offset: 0x00114508
		public void SwitchToNextStage()
		{
			if (this.stage == CombatStage.SelectingBattlefield && this.selectedBattlefield != null)
			{
				this.combatAlreadyStarted = true;
				if (this.gameManager.IsMultiplayer && ((this.gameManager.PlayerOwner != null && this.gameManager.PlayerOwner == this.attacker) || (this.gameManager.PlayerOwner == null && !this.gameManager.PlayerCurrent.IsHuman)))
				{
					this.gameManager.OnActionSent(new StartCombatMessage(this.gameManager.PlayerCurrent.matFaction.faction, this.selectedBattlefield.posX, this.selectedBattlefield.posY));
				}
				if (this.selectedBattlefield.GetOwnerMechs().Count > 0 || this.selectedBattlefield.Owner.character.position == this.selectedBattlefield)
				{
					this.stage = CombatStage.Diversion;
				}
				else if (this.selectedBattlefield.GetOwnerWorkers().Count > 0)
				{
					this.AloneWorkersCase();
					this.stage = CombatStage.EndingTheBattle;
				}
				this.CountDefenderWorkers();
			}
			if (this.stage == CombatStage.Diversion && !this.DiversionNextStep())
			{
				this.actionCounter = 0;
				this.stage = CombatStage.Preparation;
			}
			if (this.stage == CombatStage.Preparation && !this.PreparationNextStep())
			{
				this.actionCounter = 0;
				this.stage = CombatStage.DeterminatingTheWinner;
			}
			if (this.stage == CombatStage.DeterminatingTheWinner && !this.DeterminatingTheWinnerNextStep())
			{
				this.actionCounter = 0;
				this.actualPlayer = this.attacker;
				this.stage = CombatStage.EndingTheBattle;
			}
			if (this.stage == CombatStage.EndingTheBattle && !this.EndingTheBattleNextStep())
			{
				if (!this.starChecked && !this.workersWithoutCombatSupport)
				{
					Player winner = this.GetWinner();
					this.starChecked = true;
					this.actionCounter = 0;
					winner.CheckCombatStar();
					winner.BattleVictory();
					this.CleanUpBattle();
				}
				else if (this.workersWithoutCombatSupport)
				{
					this.actionCounter = 0;
					this.CleanUpBattle();
				}
			}
			if (this.stage == CombatStage.CombatResovled)
			{
				if (!this.workersWithoutCombatSupport)
				{
					if (this.IsPlayerInCombat())
					{
						this.SetAttacker(this.gameManager.PlayerCurrent);
						this.SwitchToNextStage();
						return;
					}
					this.Clear();
				}
				else
				{
					this.workersWithoutCombatSupport = false;
				}
			}
			this.PerformAIStep();
			if (this.OnCombatStageChanged != null)
			{
				this.OnCombatStageChanged(this.stage);
			}
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x00116540 File Offset: 0x00114740
		public Delegate[] GetInvocationListForOnCombatStageChanged()
		{
			if (this.OnCombatStageChanged != null)
			{
				Delegate[] invocationList = this.OnCombatStageChanged.GetInvocationList();
				string text = "";
				foreach (Delegate @delegate in invocationList)
				{
					text += "\n";
					if (@delegate == null)
					{
						text += "null/none";
					}
					else
					{
						if (@delegate != null && @delegate.Target != null)
						{
							text = text + @delegate.Target.GetType().ToString() + ".";
						}
						else
						{
							text += "null.";
						}
						if (@delegate != null && @delegate.Method != null)
						{
							text += @delegate.Method.Name;
						}
						else
						{
							text += "null";
						}
					}
				}
				return invocationList;
			}
			return new Delegate[0];
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x00116618 File Offset: 0x00114818
		private void PerformAIStep()
		{
			if (this.gameManager.TestingMode && this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null)
			{
				if (this.GetActualPlayer() != null)
				{
					if (this.GetActualPlayer() == this.gameManager.PlayerOwner || this.stage == CombatStage.DeterminatingTheWinner)
					{
						this.GetActualPlayer().aiPlayer.PerformCombatStage(this.stage);
						return;
					}
				}
				else if (this.gameManager.PlayerOwner == this.gameManager.PlayerCurrent)
				{
					this.gameManager.PlayerOwner.aiPlayer.PerformCombatStage(this.stage);
				}
				return;
			}
			if (this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null)
			{
				return;
			}
			if (this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null)
			{
				if (this.AttackerIsBot() && this.DefenderIsBot() && this.stage == CombatStage.DeterminatingTheWinner)
				{
					this.SwitchToNextStage();
				}
				return;
			}
			if (this.AttackerIsBot() || this.DefenderIsBot() || !this.gameManager.PlayerCurrent.IsHuman)
			{
				AiPlayer aiPlayer = null;
				if (this.GetActualPlayer() != null)
				{
					aiPlayer = this.GetActualPlayer().aiPlayer;
				}
				if (aiPlayer == null)
				{
					aiPlayer = this.gameManager.PlayerCurrent.aiPlayer;
				}
				if (aiPlayer != null && !aiPlayer.player.IsHuman)
				{
					aiPlayer.PerformCombatStage(this.stage);
					return;
				}
				if (this.stage == CombatStage.DeterminatingTheWinner && this.gameManager.IsMultiplayer)
				{
					this.SwitchToNextStage();
				}
			}
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x00044A66 File Offset: 0x00042C66
		private bool AttackerIsBot()
		{
			return this.attacker != null && !this.attacker.IsHuman;
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x00044A80 File Offset: 0x00042C80
		private bool DefenderIsBot()
		{
			return this.defender != null && !this.defender.IsHuman;
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x00116798 File Offset: 0x00114998
		private LogInfo CreateInfoLog()
		{
			CombatLogInfo combatLogInfo = new CombatLogInfo(this.gameManager);
			combatLogInfo.Type = (this.workersWithoutCombatSupport ? LogInfoType.ForceWorkersToRetreat : LogInfoType.Combat);
			combatLogInfo.PlayerAssigned = this.gameManager.PlayerCurrent.matFaction.faction;
			combatLogInfo.Battlefield = this.selectedBattlefield;
			combatLogInfo.Units.AddRange(this.units);
			combatLogInfo.Winner = this.GetWinner();
			combatLogInfo.Defeated = this.GetDefeated();
			combatLogInfo.WinnerAbilityUsed = ((combatLogInfo.Winner == this.attacker) ? this.attackerAbilityUsed : this.defenderAbilityUsed);
			combatLogInfo.DefeatedAbilityUsed = ((combatLogInfo.Defeated == this.attacker) ? this.attackerAbilityUsed : this.defenderAbilityUsed);
			if ((this.attackerAbilityUsed && this.attacker.matFaction.faction == Faction.Polania) || !this.attackerIsWinner)
			{
				combatLogInfo.LostPopularity = 0;
			}
			else
			{
				combatLogInfo.LostPopularity = this.numberOfWorkersOnBattlefield;
			}
			if (this.usedPower.Count > 0)
			{
				combatLogInfo.WinnerPower = this.usedPower[this.GetWinner()];
				combatLogInfo.DefeatedPower = this.usedPower[this.GetDefeated()];
			}
			return combatLogInfo;
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x00044A9A File Offset: 0x00042C9A
		public bool IsPlayerInCombat()
		{
			return this.battlefields.Count > 0 && !this.gameManager.GameFinished;
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x00044ABA File Offset: 0x00042CBA
		public CombatStage GetActualStage()
		{
			return this.stage;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x00044AC2 File Offset: 0x00042CC2
		public bool BonusCombatCardActionExecuted()
		{
			return this.combatCardChoosingExecuted;
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x00044ACA File Offset: 0x00042CCA
		private void ChangeActualPlayer()
		{
			this.actualPlayer = ((this.actualPlayer != this.attacker) ? this.attacker : this.defender);
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x00044AEE File Offset: 0x00042CEE
		public Player GetAttacker()
		{
			return this.attacker;
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x00044AF6 File Offset: 0x00042CF6
		public Player GetDefender()
		{
			return this.defender;
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x00044AFE File Offset: 0x00042CFE
		public Player GetEnemyOf(Player player)
		{
			if (player != this.attacker)
			{
				return this.attacker;
			}
			return this.defender;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x00044B16 File Offset: 0x00042D16
		public Player GetActualPlayer()
		{
			return this.actualPlayer;
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x00044B1E File Offset: 0x00042D1E
		public Player GetWinner()
		{
			if (!this.attackerIsWinner)
			{
				return this.defender;
			}
			return this.attacker;
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x00044B35 File Offset: 0x00042D35
		public Player GetDefeated()
		{
			if (!this.attackerIsWinner)
			{
				return this.attacker;
			}
			return this.defender;
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x00044B4C File Offset: 0x00042D4C
		public bool AttackerIsWinner()
		{
			return this.attackerIsWinner;
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x00044B54 File Offset: 0x00042D54
		public void SetAttackerIsWinner(bool isWinner)
		{
			this.attackerIsWinner = isWinner;
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x00044B5D File Offset: 0x00042D5D
		public bool IsPlayerInCombat(Player player)
		{
			return player == this.attacker || player == this.defender;
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x00044B73 File Offset: 0x00042D73
		public bool HasPlayerPassedPower(Player player)
		{
			return this.usedPower.ContainsKey(player);
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x001168D0 File Offset: 0x00114AD0
		public bool CanPerformStep(Player player)
		{
			if (player != this.attacker && player != this.defender)
			{
				return false;
			}
			switch (this.stage)
			{
			case CombatStage.SelectingBattlefield:
				return this.attacker == player;
			case CombatStage.Diversion:
				return this.actualPlayer == player;
			case CombatStage.Preparation:
				return !this.usedPower.ContainsKey(player);
			case CombatStage.DeterminatingTheWinner:
				return true;
			case CombatStage.EndingTheBattle:
				return this.actualPlayer == player;
			default:
				return false;
			}
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x00044B81 File Offset: 0x00042D81
		public void AddBattlefield(GameHex battle)
		{
			if (!this.battlefields.Contains(battle))
			{
				this.battlefields.Add(battle);
			}
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x00116948 File Offset: 0x00114B48
		public bool SelectBattlefield(GameHex selectedField)
		{
			if (this.battlefields.Contains(selectedField))
			{
				this.selectedBattlefield = selectedField;
				this.defender = this.selectedBattlefield.Owner;
				this.attacker = this.selectedBattlefield.Enemy;
				this.attacker.StartedCombatsAsAnAttacker++;
				this.units.AddRange(this.selectedBattlefield.GetOwnerUnits());
				this.units.AddRange(this.selectedBattlefield.GetEnemyUnits());
				this.CountDefenderWorkers();
				return true;
			}
			return false;
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x00044B9D File Offset: 0x00042D9D
		public GameHex GetSelectedBattlefield()
		{
			return this.selectedBattlefield;
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x00044BA5 File Offset: 0x00042DA5
		public List<GameHex> GetBattlefields()
		{
			return this.battlefields;
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x00044BAD File Offset: 0x00042DAD
		private void AloneWorkersCase()
		{
			this.workersWithoutCombatSupport = true;
			this.attackerIsWinner = true;
			this.defender = this.selectedBattlefield.Owner;
			this.attacker = this.selectedBattlefield.Enemy;
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x00044BDF File Offset: 0x00042DDF
		public bool CombatAlreadyStarted()
		{
			return this.combatAlreadyStarted;
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x001169D4 File Offset: 0x00114BD4
		private bool DiversionNextStep()
		{
			if (this.actionCounter == 1)
			{
				this.ChangeActualPlayer();
			}
			while (this.actionCounter != 2)
			{
				this.actionCounter++;
				if (this.CanUseAbility(this.actualPlayer, this.selectedBattlefield))
				{
					if (this.gameManager.IsMultiplayer)
					{
						if (this.gameManager.PlayerOwner == null)
						{
							this.gameManager.ChangeActivePlayer((int)this.GetEnemyOf(this.actualPlayer).matFaction.faction, (int)this.actualPlayer.matFaction.faction);
						}
						else
						{
							this.gameManager.ChangeActivePlayer((int)this.actualPlayer.matFaction.faction);
						}
					}
					return true;
				}
				this.ChangeActualPlayer();
			}
			return false;
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x00116A9C File Offset: 0x00114C9C
		public bool CanUseAbility(Player player, GameHex battlefield)
		{
			if (!player.matFaction.DiversionAbilityUnlocked())
			{
				return false;
			}
			if (player == this.attacker && this.attackerAbilityUsed)
			{
				return false;
			}
			if (player == this.defender && this.defenderAbilityUsed)
			{
				return false;
			}
			Player player2 = ((player == battlefield.Enemy) ? battlefield.Owner : battlefield.Enemy);
			if (player.matFaction.faction == Faction.Albion && player.matFaction.SkillUnlocked[1] && player == this.attacker)
			{
				return player == this.attacker;
			}
			if (player.matFaction.SkillUnlocked[2])
			{
				AbilityPerk abilityPerk = player.matFaction.abilities[2];
				if (abilityPerk <= AbilityPerk.Scout)
				{
					if (abilityPerk == AbilityPerk.Artillery)
					{
						return player.Power >= 1;
					}
					if (abilityPerk == AbilityPerk.Scout)
					{
						return player2.combatCards.Count > 0;
					}
				}
				else
				{
					if (abilityPerk == AbilityPerk.Disarm)
					{
						return battlefield.hasTunnel || (battlefield.Building != null && battlefield.Building.buildingType == BuildingType.Mine && battlefield.Building.player == player);
					}
					if (abilityPerk == AbilityPerk.Shield)
					{
						return player == this.defender;
					}
					if (abilityPerk == AbilityPerk.Ronin)
					{
						return battlefield.GetPlayerUnitCount(player) == 1;
					}
				}
			}
			return false;
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x00116BD0 File Offset: 0x00114DD0
		public void StealCombatCardFromEnemy(CombatCard stolenCard)
		{
			if (this.gameManager.IsMultiplayer)
			{
				if (this.gameManager.PlayerOwner != null || (this.gameManager.PlayerOwner == null && !this.actualPlayer.IsHuman))
				{
					this.gameManager.OnActionSent(new StealCardMessage(stolenCard.CombatBonus, (int)this.actualPlayer.matFaction.faction, (int)this.GetEnemyOf(this.actualPlayer).matFaction.faction));
				}
				if (this.actualPlayer != this.gameManager.PlayerCurrent)
				{
					if (this.gameManager.PlayerOwner != null)
					{
						this.gameManager.ChangeActivePlayer((int)this.gameManager.PlayerCurrent.matFaction.faction);
					}
					else
					{
						this.gameManager.ChangeActivePlayer((int)this.GetActualPlayer().matFaction.faction, (int)this.gameManager.PlayerCurrent.matFaction.faction);
					}
				}
			}
			if (this.actualPlayer == this.attacker)
			{
				this.attackerAbilityUsed = true;
			}
			else
			{
				this.defenderAbilityUsed = true;
			}
			this.GetEnemyOf(this.actualPlayer).RemoveCombatCard(stolenCard);
			this.actualPlayer.AddCombatCard(stolenCard);
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x00116D00 File Offset: 0x00114F00
		public void RemovePowerFromEnemy(int cost = 0)
		{
			if (this.gameManager.IsMultiplayer)
			{
				if (this.gameManager.PlayerOwner != null || (this.gameManager.PlayerOwner == null && !this.actualPlayer.IsHuman))
				{
					this.gameManager.OnActionSent(new RemovePowerMessage(cost, (int)this.actualPlayer.matFaction.faction, (int)this.GetEnemyOf(this.actualPlayer).matFaction.faction));
				}
				if (this.actualPlayer != this.gameManager.PlayerCurrent)
				{
					if (this.gameManager.PlayerOwner != null)
					{
						this.gameManager.ChangeActivePlayer((int)this.gameManager.PlayerCurrent.matFaction.faction);
					}
					else
					{
						this.gameManager.ChangeActivePlayer((int)this.GetActualPlayer().matFaction.faction, (int)this.gameManager.PlayerCurrent.matFaction.faction);
					}
				}
			}
			if (this.actualPlayer == this.attacker)
			{
				this.attackerAbilityUsed = true;
			}
			else
			{
				this.defenderAbilityUsed = true;
			}
			this.GetEnemyOf(this.actualPlayer).Power -= 2;
			this.actualPlayer.Power -= cost;
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x00116E38 File Offset: 0x00115038
		public void BolsterBeforeCombat()
		{
			if (this.gameManager.IsMultiplayer)
			{
				if (this.gameManager.PlayerOwner != null || (this.gameManager.PlayerOwner == null && !this.actualPlayer.IsHuman))
				{
					this.gameManager.OnActionSent(new BolsterBeforeCombatMessage((int)this.actualPlayer.matFaction.faction));
				}
				if (this.actualPlayer != this.gameManager.PlayerCurrent)
				{
					if (this.gameManager.PlayerOwner != null)
					{
						this.gameManager.ChangeActivePlayer((int)this.gameManager.PlayerCurrent.matFaction.faction);
					}
					else
					{
						this.gameManager.ChangeActivePlayer((int)this.GetActualPlayer().matFaction.faction, (int)this.gameManager.PlayerCurrent.matFaction.faction);
					}
				}
			}
			if (this.actualPlayer == this.attacker)
			{
				this.attackerAbilityUsed = true;
			}
			else
			{
				this.defenderAbilityUsed = true;
			}
			this.actualPlayer.Power += 2;
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x00044BE7 File Offset: 0x00042DE7
		public void ReshowDiversionPanelIfAble()
		{
			if (!this.CanUseAbility(this.gameManager.PlayerOwner, this.selectedBattlefield))
			{
				return;
			}
			this.OnCombatStageChanged(this.stage);
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x00116F40 File Offset: 0x00115140
		private bool PreparationNextStep()
		{
			if (this.gameManager.IsMultiplayer && this.actionCounter == 0)
			{
				if (this.gameManager.PlayerOwner == null)
				{
					this.actualPlayer = this.attacker;
				}
				else if (this.IsPlayerInCombat(this.gameManager.PlayerOwner))
				{
					this.actualPlayer = this.gameManager.PlayerOwner;
				}
				else
				{
					this.actualPlayer = this.attacker;
				}
				this.gameManager.ChangeSecondActivePlayer((int)this.defender.matFaction.faction);
			}
			if (this.actionCounter == 1)
			{
				this.ChangeActualPlayer();
			}
			if (this.actionCounter != 2)
			{
				this.actionCounter++;
				return true;
			}
			return false;
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x00116FF4 File Offset: 0x001151F4
		public int GetPossibleAmountOfCombatCardsToUse(Player actualPlayer)
		{
			int num = this.GetPlayerCombatUnitsCount(actualPlayer);
			if (num > actualPlayer.combatCards.Count)
			{
				num = actualPlayer.combatCards.Count;
			}
			return num;
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x00117024 File Offset: 0x00115224
		public int GetPlayerCombatUnitsCount(Player actualPlayer)
		{
			int num = 0;
			num += ((actualPlayer.character.position == this.selectedBattlefield) ? 1 : 0);
			using (List<Mech>.Enumerator enumerator = actualPlayer.matFaction.mechs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.position == this.selectedBattlefield)
					{
						num++;
					}
				}
			}
			num += this.GetAdditionalCardsForCombatAbility(actualPlayer);
			return num;
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x001170AC File Offset: 0x001152AC
		private int GetAdditionalCardsForCombatAbility(Player actualPlayer)
		{
			if (actualPlayer.matFaction.abilities.Contains(AbilityPerk.PeoplesArmy) && actualPlayer.matFaction.SkillUnlocked[2])
			{
				using (List<Worker>.Enumerator enumerator = actualPlayer.matPlayer.workers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.position == this.selectedBattlefield)
						{
							return 1;
						}
					}
					return 0;
				}
			}
			if (actualPlayer.matFaction.abilities.Contains(AbilityPerk.Suiton) && actualPlayer.matFaction.SkillUnlocked[1] && this.GetSelectedBattlefield().hexType == HexType.lake)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x00117168 File Offset: 0x00115368
		public void AddPlayerPowerInBattle(Player player, PowerSelected power)
		{
			if (this.gameManager.IsMultiplayer && ((this.gameManager.PlayerOwner != null && player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !player.IsHuman)))
			{
				this.gameManager.OnActionSent(new AddPowerMessage((int)player.matFaction.faction, power.selectedPower, power.cardsPower, power.selectedCards));
				this.gameManager.DisableActivePlayer((int)player.matFaction.faction);
			}
			if (!this.gameManager.IsMultiplayer || this.gameManager.PlayerOwner == null)
			{
				int num = power.selectedPower + power.cardsPower;
				player.CombatPowerSpent += num;
				if (num > player.CombatMaxPowerUsed)
				{
					player.CombatMaxPowerUsed = num;
				}
			}
			if (!this.usedPower.ContainsKey(player))
			{
				this.usedPower.Add(player, power);
			}
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x00044B73 File Offset: 0x00042D73
		public bool IsPowerSet(Player player)
		{
			return this.usedPower.ContainsKey(player);
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x00117258 File Offset: 0x00115458
		public void ReshowPreparationPanelIfAble()
		{
			if (this.usedPower.ContainsKey(this.gameManager.PlayerOwner))
			{
				return;
			}
			this.actionCounter--;
			if (this.actualPlayer == this.gameManager.PlayerOwner)
			{
				this.ChangeActualPlayer();
			}
			this.gameManager.combatManager.SwitchToNextStage();
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x00044C14 File Offset: 0x00042E14
		private bool DeterminatingTheWinnerNextStep()
		{
			if (this.actionCounter == 0)
			{
				this.actionCounter++;
				this.CalculateWinner();
				return true;
			}
			return false;
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x001172B8 File Offset: 0x001154B8
		private void CalculateWinner()
		{
			int num = this.CalculatePlayerPowerPoints(this.attacker);
			int num2 = this.CalculatePlayerPowerPoints(this.defender);
			this.attackerIsWinner = num >= num2;
			this.UpdatePlayersPowerAndCards();
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x00044C35 File Offset: 0x00042E35
		private void UpdatePlayersPowerAndCards()
		{
			this.UpdatePlayerPowerAndCards(this.attacker);
			this.UpdatePlayerPowerAndCards(this.defender);
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x00044C4F File Offset: 0x00042E4F
		private void UpdatePlayerPowerAndCards(Player player)
		{
			player.Power -= this.usedPower[player].selectedPower;
			this.RemovePowerCards(player);
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x00044C76 File Offset: 0x00042E76
		private int CalculatePlayerPowerPoints(Player player)
		{
			if (this.usedPower.ContainsKey(player))
			{
				return this.usedPower[player].cardsPower + this.usedPower[player].selectedPower;
			}
			return 0;
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x00044CAB File Offset: 0x00042EAB
		public Dictionary<Player, PowerSelected> GetUsedPowers()
		{
			return this.usedPower;
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x001172F4 File Offset: 0x001154F4
		private void RemovePowerCards(Player player)
		{
			foreach (CombatCard combatCard in this.usedPower[player].selectedCards)
			{
				player.RemoveCombatCard(combatCard);
				this.gameManager.AddUsedCombatCard(combatCard);
			}
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x00044CB3 File Offset: 0x00042EB3
		public void SetPower(Player attacker, PowerSelected attackerPS, Player defender, PowerSelected defenderPS)
		{
			this.usedPower.Add(attacker, attackerPS);
			this.usedPower.Add(defender, defenderPS);
			this.CountDefenderWorkers();
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x00117360 File Offset: 0x00115560
		private bool EndingTheBattleNextStep()
		{
			while (this.actionCounter != 2)
			{
				if (this.GetActualPlayer() == this.GetWinner())
				{
					if (this.GetWinner() == this.GetAttacker())
					{
						if (this.gameManager.PlayerCurrent == this.GetAttacker() && this.numberOfWorkersOnBattlefield > 0)
						{
							this.gameManager.PlayerCurrent.wonBattle = true;
						}
						if (this.CanUseAfterBattleAbility(this.GetActualPlayer()))
						{
							if (this.gameManager.IsMultiplayer)
							{
								if (this.gameManager.PlayerOwner != null)
								{
									this.gameManager.ChangeActivePlayer((int)this.GetActualPlayer().matFaction.faction);
								}
								else if (this.gameManager.PlayerOwner == null)
								{
									this.gameManager.ChangeActivePlayer((int)this.gameManager.PlayerCurrent.matFaction.faction, (int)this.GetActualPlayer().matFaction.faction);
								}
							}
							return true;
						}
						this.AddWorkersPenalty(true);
					}
					else
					{
						this.actionCounter++;
						this.ChangeActualPlayer();
					}
				}
				else if (this.GetActualPlayer() == this.GetDefeated())
				{
					if (this.CanActualPlayerGetCombatCard())
					{
						if (this.gameManager.IsMultiplayer)
						{
							if (this.gameManager.PlayerOwner != null)
							{
								this.gameManager.ChangeActivePlayer((int)this.GetActualPlayer().matFaction.faction);
							}
							else if (this.gameManager.PlayerOwner == null)
							{
								this.gameManager.ChangeActivePlayer((int)this.gameManager.PlayerCurrent.matFaction.faction, (int)this.GetActualPlayer().matFaction.faction);
							}
						}
						return true;
					}
					if (this.CanUseAfterBattleAbility(this.GetActualPlayer()) && this.GetUnitsToWithdraw().Count > 0)
					{
						if (this.gameManager.IsMultiplayer)
						{
							if (this.gameManager.PlayerOwner != null)
							{
								this.gameManager.ChangeActivePlayer((int)this.GetActualPlayer().matFaction.faction);
							}
							else if (this.gameManager.PlayerOwner == null)
							{
								this.gameManager.ChangeActivePlayer((int)this.gameManager.PlayerCurrent.matFaction.faction, (int)this.GetActualPlayer().matFaction.faction);
							}
						}
						return true;
					}
					if (this.actionCounter != 2)
					{
						this.WithdrawToPosition(this.GetUnitsToWithdraw(), this.GetWithdrawPositions(this.selectedBattlefield)[0]);
					}
				}
			}
			return false;
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x001175C4 File Offset: 0x001157C4
		private void CountDefenderWorkers()
		{
			this.numberOfWorkersOnBattlefield = 0;
			using (List<Worker>.Enumerator enumerator = this.defender.matPlayer.workers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.position == this.selectedBattlefield)
					{
						this.numberOfWorkersOnBattlefield++;
					}
				}
			}
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x0011763C File Offset: 0x0011583C
		public bool CanUseAfterBattleAbility(Player player)
		{
			return (player.matFaction.abilities.Contains(AbilityPerk.Camaraderie) && player.matFaction.SkillUnlocked[2] && player == this.GetWinner() && this.numberOfWorkersOnBattlefield > 0 && !this.workersWithoutCombatSupport && !this.attackerAbilityUsed) || (player.matFaction.abilities.Contains(AbilityPerk.Seaworthy) && player.matFaction.SkillUnlocked[1] && !this.workersWithoutCombatSupport && player == this.GetDefeated() && this.GetWithdrawPositions(this.selectedBattlefield).Count > 1);
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x001176DC File Offset: 0x001158DC
		public bool CanRearmTrap(Player player)
		{
			return !this.tokenRearmChoosingExecuted && player.matFaction.faction == Faction.Togawa && player.matFaction.abilities.Contains(AbilityPerk.Shinobi) && player.matFaction.SkillUnlocked[3] && !this.workersWithoutCombatSupport && player == this.GetWinner() && player == this.GetAttacker() && this.gameManager.tokenManager.CanRearmTrap(this.GetSelectedBattlefield(), player.matFaction.faction);
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x00044CD6 File Offset: 0x00042ED6
		private void AddWorkersPenalty(bool add = true)
		{
			if (this.attackerIsWinner && add)
			{
				this.attacker.Popularity -= this.numberOfWorkersOnBattlefield;
			}
			this.actionCounter++;
			this.ChangeActualPlayer();
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x00044D0E File Offset: 0x00042F0E
		public bool CanActualPlayerGetCombatCard()
		{
			return this.CanPlayerGetCombatCard(this.GetActualPlayer());
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x00117760 File Offset: 0x00115960
		public bool CanPlayerGetCombatCard(Player player)
		{
			if (!this.BonusCombatCardActionExecuted() && !this.workersWithoutCombatSupport && player != null && player == this.GetDefeated())
			{
				PowerSelected powerSelected = this.usedPower[player];
				if (powerSelected.cardsPower + powerSelected.selectedPower > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x001177AC File Offset: 0x001159AC
		public void AddCombatCard(bool choice)
		{
			if (choice)
			{
				GainNonboardResourceLogInfo gainNonboardResourceLogInfo = new GainNonboardResourceLogInfo(this.gameManager);
				gainNonboardResourceLogInfo.PlayerAssigned = this.GetActualPlayer().matFaction.faction;
				gainNonboardResourceLogInfo.Type = LogInfoType.GainCombatCard;
				gainNonboardResourceLogInfo.Gained = GainType.CombatCard;
				gainNonboardResourceLogInfo.ActionPlacement = ActionPositionType.Combat;
				gainNonboardResourceLogInfo.Amount = 1;
				this.gameManager.actionLog.LogInfoReported(gainNonboardResourceLogInfo);
				if (!this.gameManager.IsMultiplayer || (this.gameManager.PlayerOwner == null && !this.GetDefeated().IsHuman))
				{
					List<CombatCard> combatCards = this.gameManager.GetCombatCards(1);
					if (this.gameManager.IsMultiplayer)
					{
						this.gameManager.SendCards(1, (int)this.GetDefeated().matFaction.faction, GainCombatCard.CombatCardGainType.Combat);
					}
					else if (combatCards.Count > 0)
					{
						this.GetDefeated().AddCombatCard(combatCards.First<CombatCard>());
					}
				}
				else
				{
					this.gameManager.OnGainCombatCards(1, GainCombatCard.CombatCardGainType.Combat);
				}
			}
			this.combatCardChoosingExecuted = true;
			if (this.gameManager.IsMultiplayer)
			{
				if (this.gameManager.PlayerOwner == null && !this.GetDefeated().IsHuman)
				{
					this.gameManager.OnActionSent(new CardChoosenMessage((int)this.GetDefeated().matFaction.faction));
				}
				else if (this.gameManager.PlayerOwner == this.GetDefeated())
				{
					this.gameManager.OnActionSent(new CardChoosenMessage((int)this.GetDefeated().matFaction.faction));
				}
				if (this.gameManager.PlayerOwner != null)
				{
					this.gameManager.ChangeActivePlayer((int)this.gameManager.PlayerCurrent.matFaction.faction);
					return;
				}
				if (this.gameManager.PlayerOwner == null)
				{
					this.gameManager.ChangeActivePlayer((int)this.actualPlayer.matFaction.faction, (int)this.gameManager.PlayerCurrent.matFaction.faction);
				}
			}
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x00117988 File Offset: 0x00115B88
		public bool WithdrawToPosition(List<Unit> unitsToWithdraw, GameHex withdrawPosition)
		{
			this.retreatMove.SetPlayer(this.GetDefeated());
			bool flag = this.retreatMove.WithdrawDefeatedPlayer(this.GetDefeated(), unitsToWithdraw, withdrawPosition);
			this.retreatMove.Execute();
			this.retreatMove.Clear();
			if (this.OnWithdrawUnits != null && ((this.gameManager.IsMultiplayer && this.GetDefeated() == this.gameManager.PlayerOwner) || (!this.gameManager.IsMultiplayer && this.GetDefeated().IsHuman)))
			{
				this.OnWithdrawUnits(unitsToWithdraw, this.selectedBattlefield, withdrawPosition);
			}
			if (this.GetUnitsToWithdraw().Count == 0)
			{
				this.actionCounter++;
				this.ChangeActualPlayer();
				if (this.gameManager.IsMultiplayer)
				{
					if (this.gameManager.PlayerOwner != null)
					{
						this.gameManager.ChangeActivePlayer((int)this.gameManager.PlayerCurrent.matFaction.faction);
						return flag;
					}
					if (this.gameManager.PlayerOwner == null)
					{
						this.gameManager.ChangeActivePlayer((int)this.GetEnemyOf(this.actualPlayer).matFaction.faction, (int)this.gameManager.PlayerCurrent.matFaction.faction);
					}
				}
			}
			return flag;
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x00044D1C File Offset: 0x00042F1C
		private void CallOvertakeOwnerPositions()
		{
			if (this.attackerIsWinner && this.TakeOwnersPositions != null)
			{
				this.TakeOwnersPositions();
			}
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x00044D39 File Offset: 0x00042F39
		public void CallReportActionLog()
		{
			this.gameManager.actionLog.LogInfoReported(this.CreateInfoLog());
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x00117AC8 File Offset: 0x00115CC8
		public List<GameHex> GetWithdrawPositions(GameHex battlefield)
		{
			List<GameHex> list = new List<GameHex>();
			list.Add(this.gameManager.gameBoard.bases[this.GetDefeated().matFaction.faction]);
			if (this.GetDefeated().matFaction.abilities.Contains(AbilityPerk.Seaworthy) && this.GetDefeated().matFaction.SkillUnlocked[1])
			{
				foreach (GameHex gameHex in battlefield.GetNeighboursAll())
				{
					if (gameHex.hexType == HexType.lake && gameHex.Enemy == null && (gameHex.Owner == null || gameHex.Owner == this.GetDefeated()))
					{
						list.Add(gameHex);
					}
				}
			}
			return list;
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x00117BA4 File Offset: 0x00115DA4
		public List<GameHex> GetWithdrawPositions()
		{
			List<GameHex> list = new List<GameHex>();
			list.Add(this.gameManager.gameBoard.bases[this.GetDefeated().matFaction.faction]);
			if (this.GetDefeated().matFaction.abilities.Contains(AbilityPerk.Seaworthy) && this.GetDefeated().matFaction.SkillUnlocked[1])
			{
				foreach (GameHex gameHex in this.selectedBattlefield.GetNeighboursAll())
				{
					if (gameHex.hexType == HexType.lake && gameHex.Enemy == null && (gameHex.Owner == null || gameHex.Owner == this.GetDefeated()))
					{
						list.Add(gameHex);
					}
				}
			}
			return list;
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x00117C84 File Offset: 0x00115E84
		public List<Unit> GetUnitsToWithdraw()
		{
			List<Unit> list = new List<Unit>();
			Player defeated = this.GetDefeated();
			if (defeated.character.position == this.selectedBattlefield)
			{
				list.Add(defeated.character);
			}
			foreach (Mech mech in defeated.matFaction.mechs)
			{
				if (mech.position == this.selectedBattlefield)
				{
					list.Add(mech);
				}
			}
			foreach (Worker worker in defeated.matPlayer.workers)
			{
				if (worker.position == this.selectedBattlefield && !worker.OnMech)
				{
					list.Add(worker);
				}
			}
			return list;
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x00117D7C File Offset: 0x00115F7C
		public void LoadWorkers(List<Unit> workers, Mech mech)
		{
			if (mech != null)
			{
				this.retreatMove.SetPlayer(mech.Owner);
			}
			foreach (Unit unit in workers)
			{
				this.retreatMove.LoadWorkerToMech(unit, mech);
			}
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x00117DE8 File Offset: 0x00115FE8
		public bool AreDefeatedMechsOnBattlefield()
		{
			using (List<Mech>.Enumerator enumerator = this.GetDefeated().matFaction.mechs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.position == this.selectedBattlefield)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x00117E54 File Offset: 0x00116054
		public bool AreDefeatedWorkersOnBattlefield()
		{
			using (List<Worker>.Enumerator enumerator = this.GetDefeated().matPlayer.workers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.position == this.selectedBattlefield)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x00044D51 File Offset: 0x00042F51
		public bool IsDefeatedCharacterOnBattlefield()
		{
			return this.GetDefeated().character.position == this.selectedBattlefield;
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x00117EC0 File Offset: 0x001160C0
		public void PolaniaCamaraderieChoice(bool choice)
		{
			this.attackerAbilityUsed = choice;
			if (this.gameManager.IsMultiplayer)
			{
				if ((this.gameManager.PlayerOwner != null && this.gameManager.PlayerOwner == this.attacker) || (this.gameManager.PlayerOwner == null && !this.attacker.IsHuman))
				{
					this.gameManager.OnActionSent(new PolaniaAbilityMessage((int)this.gameManager.PlayerCurrent.matFaction.faction, choice));
				}
				if (this.gameManager.IsMultiplayer)
				{
					if (this.gameManager.PlayerOwner != null)
					{
						this.gameManager.ChangeActivePlayer((int)this.gameManager.PlayerCurrent.matFaction.faction);
					}
					else if (this.gameManager.PlayerOwner == null)
					{
						this.gameManager.ChangeActivePlayer((int)this.GetActualPlayer().matFaction.faction, (int)this.gameManager.PlayerCurrent.matFaction.faction);
					}
				}
			}
			this.AddWorkersPenalty(!choice);
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x00044D6B File Offset: 0x00042F6B
		public void TogawaRearmChoice(bool choice)
		{
			this.attackerAbilityUsed = choice;
			this.tokenRearmChoosingExecuted = true;
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x00044D7B File Offset: 0x00042F7B
		public void ReshowDefenderEndingPanel()
		{
			this.SwitchToNextStage();
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x00117FC8 File Offset: 0x001161C8
		public void CleanUpBattle()
		{
			this.CallReportActionLog();
			if (!this.workersWithoutCombatSupport)
			{
				this.GetWinner().CombatWon++;
				this.GetDefeated().CombatLost++;
			}
			if (this.gameManager.PlayerOwner == null && this.GetWinner() == this.GetAttacker())
			{
				this.GetWinner().CombatWorkersChased += this.numberOfWorkersOnBattlefield;
			}
			this.selectedBattlefield.Owner = this.GetWinner();
			this.selectedBattlefield.Enemy = null;
			this.numberOfWorkersOnBattlefield = 0;
			this.starChecked = false;
			this.CallOvertakeOwnerPositions();
			this.attackerAbilityUsed = false;
			this.defenderAbilityUsed = false;
			this.battlefields.Remove(this.selectedBattlefield);
			this.selectedBattlefield = null;
			this.combatCardChoosingExecuted = false;
			this.tokenRearmChoosingExecuted = false;
			this.units.Clear();
			this.gameManager.CheckStars();
			if (this.battlefields.Count == 0 || this.workersWithoutCombatSupport || this.gameManager.GameFinished)
			{
				if (this.gameManager.GameFinished)
				{
					this.EndGameCleaning();
				}
				this.stage = CombatStage.CombatResovled;
				this.attacker = (this.actualPlayer = (this.defender = null));
			}
			else
			{
				this.stage = CombatStage.SelectingBattlefield;
				this.actualPlayer = this.attacker;
			}
			this.usedPower.Clear();
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x00118130 File Offset: 0x00116330
		public void EndGameCleaning()
		{
			if (this.battlefields.Count == 0)
			{
				return;
			}
			GainMove gainMove = new GainMove(this.gameManager);
			gainMove.SetPlayer(this.attacker);
			foreach (GameHex gameHex in new List<GameHex>(this.battlefields))
			{
				if (gameHex.HasEnemyCharacter())
				{
					this.PerformEndGameCleaningMove(this.attacker.character, gainMove);
				}
				foreach (Mech mech in gameHex.GetEnemyMechs())
				{
					this.PerformEndGameCleaningMove(mech, gainMove);
				}
				foreach (Worker worker in gameHex.GetEnemyWorkers())
				{
					this.PerformEndGameCleaningMove(worker, gainMove);
				}
				gameHex.UpdateOwnership();
			}
			this.Clear();
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x00118260 File Offset: 0x00116460
		private void PerformEndGameCleaningMove(Unit unit, GainMove cleaningReatreatMove)
		{
			cleaningReatreatMove.ExchangeResources(unit, unit.savedResources[ResourceType.oil], unit.savedResources[ResourceType.metal], unit.savedResources[ResourceType.food], unit.savedResources[ResourceType.wood]);
			if (unit.lastX > 0 && unit.lastY > 0)
			{
				GameHex gameHex = this.gameManager.gameBoard.hexMap[unit.lastX, unit.lastY];
				List<Unit> list = new List<Unit>();
				list.Add(unit);
				cleaningReatreatMove.WithdrawDefeatedPlayer(this.attacker, list, gameHex);
				cleaningReatreatMove.UnloadAllResources(unit);
			}
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x00118300 File Offset: 0x00116500
		public void Clear()
		{
			this.attacker = (this.defender = (this.actualPlayer = null));
			if (!this.workersWithoutCombatSupport)
			{
				this.battlefields.Clear();
			}
			this.selectedBattlefield = null;
			this.actionCounter = 0;
			this.stage = CombatStage.CombatResovled;
			this.attackerIsWinner = false;
			this.starChecked = false;
			this.attackerAbilityUsed = false;
			this.defenderAbilityUsed = false;
			this.combatAlreadyStarted = false;
			this.combatCardChoosingExecuted = false;
			this.workersWithoutCombatSupport = false;
			this.usedPower.Clear();
			this.attackerIsWinner = false;
			this.numberOfWorkersOnBattlefield = 0;
			this.retreatMove.Clear();
			this.units.Clear();
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x00044D83 File Offset: 0x00042F83
		public void ClearDelegates()
		{
			this.OnCombatStageChanged = null;
			this.OnWithdrawUnits = null;
			this.TakeOwnersPositions = null;
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x00044D9A File Offset: 0x00042F9A
		public bool CanGetStar(Player player)
		{
			return this.stage == CombatStage.EndingTheBattle && player == this.GetWinner() && !this.starChecked && !this.workersWithoutCombatSupport;
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x001183B0 File Offset: 0x001165B0
		public bool CanLosePop(Player player, int amount)
		{
			return this.stage == CombatStage.EndingTheBattle && this.attackerIsWinner && player.matFaction.faction == this.GetAttacker().matFaction.faction && amount == this.numberOfWorkersOnBattlefield;
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x00118400 File Offset: 0x00116600
		public void UpdateBattlefieldsOwners()
		{
			foreach (GameHex gameHex in this.battlefields)
			{
				foreach (Player player in this.gameManager.GetPlayers())
				{
					foreach (Unit unit in player.GetAllUnits())
					{
						if (unit.position == gameHex && this.gameManager.PlayerCurrent == player)
						{
							gameHex.Enemy = player;
						}
						else if (unit.position == gameHex)
						{
							gameHex.Owner = player;
						}
					}
				}
			}
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x00118504 File Offset: 0x00116704
		public bool IsEventHandlerRegistered(Delegate prospectiveHandler)
		{
			if (this.OnCombatStageChanged != null)
			{
				Delegate[] invocationList = this.OnCombatStageChanged.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					if (invocationList[i] == prospectiveHandler)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x00118544 File Offset: 0x00116744
		public void ReadXml(XmlReader reader)
		{
			this.Clear();
			this.battlefields.Clear();
			this.stage = (CombatStage)int.Parse(reader.GetAttribute("Stage"));
			int num = int.Parse(reader.GetAttribute("Battles"));
			int num2;
			int num3;
			for (int i = 0; i < num; i++)
			{
				num2 = int.Parse(reader.GetAttribute("X" + i.ToString()));
				num3 = int.Parse(reader.GetAttribute("Y" + i.ToString()));
				this.battlefields.Add(this.gameManager.gameBoard.hexMap[num2, num3]);
			}
			if (reader.GetAttribute("WorCom") != null)
			{
				this.workersWithoutCombatSupport = true;
			}
			if (reader.GetAttribute("Att") == null)
			{
				return;
			}
			this.attacker = this.gameManager.GetPlayers()[int.Parse(reader.GetAttribute("Att"))];
			this.actualPlayer = this.gameManager.GetPlayers()[int.Parse(reader.GetAttribute("Act"))];
			if (reader.GetAttribute("SelX") == null)
			{
				return;
			}
			this.defender = this.gameManager.GetPlayers()[int.Parse(reader.GetAttribute("Def"))];
			this.actionCounter = int.Parse(reader.GetAttribute("Counter"));
			this.numberOfWorkersOnBattlefield = int.Parse(reader.GetAttribute("Workers"));
			num2 = int.Parse(reader.GetAttribute("SelX"));
			num3 = int.Parse(reader.GetAttribute("SelY"));
			this.selectedBattlefield = this.gameManager.gameBoard.hexMap[num2, num3];
			this.selectedBattlefield.Enemy = this.attacker;
			this.selectedBattlefield.Owner = this.defender;
			if (reader.GetAttribute("Started") != null)
			{
				this.combatAlreadyStarted = true;
			}
			if (reader.GetAttribute("AttWin") != null)
			{
				this.attackerIsWinner = true;
			}
			if (reader.GetAttribute("CombatCard") != null)
			{
				this.combatCardChoosingExecuted = true;
			}
			if (reader.GetAttribute("Star") != null)
			{
				this.starChecked = true;
			}
			if (reader.GetAttribute("AttAbilityUsed") != null)
			{
				this.attackerAbilityUsed = true;
			}
			if (reader.GetAttribute("DefAbilityUsed") != null)
			{
				this.defenderAbilityUsed = true;
			}
			reader.ReadStartElement();
			while (reader.Name == "PS")
			{
				int num4 = int.Parse(reader.GetAttribute("Player"));
				Player player = this.gameManager.GetPlayers()[num4];
				PowerSelected powerSelected;
				powerSelected.cardsPower = int.Parse(reader.GetAttribute("CPower"));
				powerSelected.selectedPower = int.Parse(reader.GetAttribute("SPower"));
				powerSelected.selectedCards = new List<CombatCard>();
				List<CombatCard> list = new List<CombatCard>(player.combatCards);
				reader.ReadStartElement();
				if (reader.Name == "CC")
				{
					string[] array = reader.ReadElementContentAsString().Split(' ', StringSplitOptions.None);
					for (int j = 0; j < array.Length; j++)
					{
						string id = array[j];
						if (id == "")
						{
							break;
						}
						if (this.stage == CombatStage.DeterminatingTheWinner || this.stage == CombatStage.EndingTheBattle)
						{
							powerSelected.selectedCards.Add(new CombatCard(int.Parse(id)));
						}
						else
						{
							CombatCard combatCard = list.Find((CombatCard c) => c.CardId == int.Parse(id));
							powerSelected.selectedCards.Add(combatCard);
							list.Remove(combatCard);
						}
					}
				}
				if (!this.usedPower.ContainsKey(player))
				{
					this.usedPower.Add(player, powerSelected);
				}
				reader.ReadEndElement();
				if (reader.Name != "PS")
				{
					reader.ReadEndElement();
				}
			}
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x0011892C File Offset: 0x00116B2C
		public void WriteXml(XmlWriter writer)
		{
			string text = "Stage";
			int num = (int)this.stage;
			writer.WriteAttributeString(text, num.ToString());
			writer.WriteAttributeString("Battles", this.battlefields.Count.ToString());
			for (int i = 0; i < this.battlefields.Count; i++)
			{
				writer.WriteAttributeString("X" + i.ToString(), this.battlefields[i].posX.ToString());
				writer.WriteAttributeString("Y" + i.ToString(), this.battlefields[i].posY.ToString());
			}
			if (this.workersWithoutCombatSupport)
			{
				writer.WriteAttributeString("WorCom", "");
			}
			if (this.attacker == null)
			{
				return;
			}
			int num2 = 0;
			int num3 = 0;
			for (int j = 0; j < this.gameManager.GetPlayers().Count; j++)
			{
				if (this.gameManager.GetPlayers()[j] == this.attacker)
				{
					num2 = j;
				}
				else if (this.gameManager.GetPlayers()[j] == this.defender)
				{
					num3 = j;
				}
			}
			writer.WriteAttributeString("Att", num2.ToString());
			if (this.attacker == this.actualPlayer)
			{
				writer.WriteAttributeString("Act", num2.ToString());
			}
			else
			{
				writer.WriteAttributeString("Act", num3.ToString());
			}
			if (this.selectedBattlefield == null)
			{
				return;
			}
			writer.WriteAttributeString("Def", num3.ToString());
			writer.WriteAttributeString("Counter", this.actionCounter.ToString());
			writer.WriteAttributeString("Workers", this.numberOfWorkersOnBattlefield.ToString());
			writer.WriteAttributeString("SelX", this.selectedBattlefield.posX.ToString());
			writer.WriteAttributeString("SelY", this.selectedBattlefield.posY.ToString());
			if (this.combatAlreadyStarted)
			{
				writer.WriteAttributeString("Started", "");
			}
			if (this.attackerIsWinner)
			{
				writer.WriteAttributeString("AttWin", "");
			}
			if (this.combatCardChoosingExecuted)
			{
				writer.WriteAttributeString("CombatCard", "");
			}
			if (this.starChecked)
			{
				writer.WriteAttributeString("Star", "");
			}
			if (this.attackerAbilityUsed)
			{
				writer.WriteAttributeString("AttAbilityUsed", "");
			}
			if (this.defenderAbilityUsed)
			{
				writer.WriteAttributeString("DefAbilityUsed", "");
			}
			foreach (Player player in this.usedPower.Keys)
			{
				writer.WriteStartElement("PS");
				if (player == this.attacker)
				{
					writer.WriteAttributeString("Player", num2.ToString());
				}
				else
				{
					writer.WriteAttributeString("Player", num3.ToString());
				}
				string text2 = "SPower";
				PowerSelected powerSelected = this.usedPower[player];
				writer.WriteAttributeString(text2, powerSelected.selectedPower.ToString());
				string text3 = "CPower";
				powerSelected = this.usedPower[player];
				writer.WriteAttributeString(text3, powerSelected.cardsPower.ToString());
				writer.WriteStartElement("CC");
				for (int k = 0; k < this.usedPower[player].selectedCards.Count; k++)
				{
					((IXmlSerializable)this.usedPower[player].selectedCards[k]).WriteXml(writer);
				}
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}

		// Token: 0x04001F3E RID: 7998
		private CombatStage stage = CombatStage.CombatResovled;

		// Token: 0x04001F3F RID: 7999
		private Player attacker;

		// Token: 0x04001F40 RID: 8000
		private Player defender;

		// Token: 0x04001F41 RID: 8001
		private Player actualPlayer;

		// Token: 0x04001F42 RID: 8002
		private List<Unit> units = new List<Unit>();

		// Token: 0x04001F43 RID: 8003
		private List<GameHex> battlefields = new List<GameHex>();

		// Token: 0x04001F44 RID: 8004
		private GameHex selectedBattlefield;

		// Token: 0x04001F45 RID: 8005
		private int actionCounter;

		// Token: 0x04001F46 RID: 8006
		private Dictionary<Player, PowerSelected> usedPower = new Dictionary<Player, PowerSelected>();

		// Token: 0x04001F47 RID: 8007
		private bool attackerIsWinner;

		// Token: 0x04001F48 RID: 8008
		private bool combatAlreadyStarted;

		// Token: 0x04001F49 RID: 8009
		private bool combatCardChoosingExecuted;

		// Token: 0x04001F4A RID: 8010
		private bool workersWithoutCombatSupport;

		// Token: 0x04001F4B RID: 8011
		private bool starChecked;

		// Token: 0x04001F4C RID: 8012
		private int numberOfWorkersOnBattlefield;

		// Token: 0x04001F4D RID: 8013
		private bool attackerAbilityUsed;

		// Token: 0x04001F4E RID: 8014
		private bool defenderAbilityUsed;

		// Token: 0x04001F4F RID: 8015
		private bool tokenRearmChoosingExecuted;

		// Token: 0x04001F50 RID: 8016
		private GainMove retreatMove;

		// Token: 0x04001F51 RID: 8017
		private GameManager gameManager;

		// Token: 0x020005AC RID: 1452
		// (Invoke) Token: 0x06002E50 RID: 11856
		public delegate void CombatStageChanged(CombatStage combatStage);

		// Token: 0x020005AD RID: 1453
		// (Invoke) Token: 0x06002E54 RID: 11860
		public delegate void WithdrawUnits(List<Unit> units, GameHex battlefield, GameHex withdrawPosition);

		// Token: 0x020005AE RID: 1454
		// (Invoke) Token: 0x06002E58 RID: 11864
		public delegate void OvertakeTheOwnerPositions();
	}
}

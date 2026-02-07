using System;
using System.Collections.Generic;
using cakeslice;
using DG.Tweening;
using HoneyFramework;
using I2.Loc;
using Scythe.Analytics;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.Multiplayer.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003D7 RID: 983
	public class CombatPresenter : MonoBehaviour, IHooksUnitControllerUser
	{
		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06001CED RID: 7405 RVA: 0x0003AD4A File Offset: 0x00038F4A
		private HookController HookController
		{
			get
			{
				return GameController.Instance.hookController;
			}
		}

		// Token: 0x140000BA RID: 186
		// (add) Token: 0x06001CEE RID: 7406 RVA: 0x000B3924 File Offset: 0x000B1B24
		// (remove) Token: 0x06001CEF RID: 7407 RVA: 0x000B3958 File Offset: 0x000B1B58
		public static event CombatPresenter.BattleEnd OnBattleEnd;

		// Token: 0x140000BB RID: 187
		// (add) Token: 0x06001CF0 RID: 7408 RVA: 0x000B398C File Offset: 0x000B1B8C
		// (remove) Token: 0x06001CF1 RID: 7409 RVA: 0x000B39C0 File Offset: 0x000B1BC0
		public static event CombatPresenter.BattlefieldSelect OnBattleFieldSelected;

		// Token: 0x06001CF2 RID: 7410 RVA: 0x0003AD56 File Offset: 0x00038F56
		private void OnEnable()
		{
			this.AttachListeners();
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x0003AD5E File Offset: 0x00038F5E
		private void OnDisable()
		{
			this.RemoveListeners();
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x0003AD5E File Offset: 0x00038F5E
		private void OnDestroy()
		{
			this.RemoveListeners();
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x000B39F4 File Offset: 0x000B1BF4
		private void AttachListeners()
		{
			GameController.GameManager.combatManager.OnCombatStageChanged += this.ChangeLayout;
			GameController.GameManager.combatManager.OnWithdrawUnits += this.OnUnitsWithdraw;
			GameController.GameManager.combatManager.TakeOwnersPositions += this.MoveAttackerUnitsToOwnerPositions;
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x000B3A54 File Offset: 0x000B1C54
		private void RemoveListeners()
		{
			if (GameController.GameManager != null)
			{
				GameController.GameManager.combatManager.OnCombatStageChanged -= this.ChangeLayout;
				GameController.GameManager.combatManager.OnWithdrawUnits -= this.OnUnitsWithdraw;
				GameController.GameManager.combatManager.TakeOwnersPositions -= this.MoveAttackerUnitsToOwnerPositions;
			}
			GameController.HexGetFocused -= this.OnBattlefieldSelected;
			GameController.HexGetFocused -= this.OnWithdrawPositionSelected;
			GameController.UnitGetFocused -= this.OnUnitSelected;
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x0003AD66 File Offset: 0x00038F66
		public void OnUndo()
		{
			this.ClearData();
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			GameController.GameManager.combatManager.Clear();
			this.RemoveListeners();
			this.AttachListeners();
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x000B3AEC File Offset: 0x000B1CEC
		public void Clear()
		{
			this.ClearData();
			if (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.PlayerOwner.currentMatSection != -1))
			{
				this.SetEndingMoveButtonsInteractable(!GameController.GameManager.GameFinished && GameController.GameManager.PlayerCurrent.IsHuman);
			}
			PlayerUnits.ChangeUnitsColliderState(true);
			GameController.ClearFocus();
			if (this.preperationPanel.activeInHierarchy)
			{
				if (this.previousCombatStage == CombatStage.Preparation)
				{
					this.preperationPanel.GetComponent<CombatPreperationPresenter>().ClosePanel();
				}
				else if (!GameController.GameManager.IsMultiplayer)
				{
					this.resultPanel.GetComponent<BattleResultPanel>().ClosePanel(true);
				}
			}
			GameController.Instance.gameBoardPresenter.UpdateStaticObjects();
			this.ChangeUIState(true, false);
			if (!GameController.Instance.GameIsLoaded)
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
			}
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x000B3BD0 File Offset: 0x000B1DD0
		private void ClearData()
		{
			foreach (GameHex gameHex in this.battlefields)
			{
				GameController.Instance.GetGameHexPresenter(gameHex).SetFocus(false, HexMarkers.MarkerType.Battle, 0f, false);
			}
			if (this.selectedHex != null)
			{
				this.selectedHex.SetFocus(false, HexMarkers.MarkerType.Battle, 0f, false);
			}
			this.exchangePanel.GetComponent<ExchangePanelPresenter>();
			this.exchangePanel.SetActive(false);
			if (this.diversionPanel.activeInHierarchy)
			{
				this.diversionPanel.SetActive(false);
				this.diversionPanel.GetComponent<DiversionPresenter>().DiversionEnd();
			}
			this.selectionPanel.SetActive(false);
			GameController.Instance.selectedBattlefieldEffects.SetActive(false);
			this.mapDarken.enabled = false;
			this.previousPlayer = null;
			this.selectedHex = null;
			this.skipPreparation = false;
			this.battlefieldLayer.Clear();
			this.battlefieldLayer.gameObject.SetActive(false);
			this.previousCombatStage = CombatStage.CombatResovled;
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x0003ADA2 File Offset: 0x00038FA2
		public void OnDragAndDropStateChanged(bool enabled)
		{
			if (!enabled)
			{
				if (this.nordicRetreatIsOn)
				{
					this.HookController.FinishWork();
					return;
				}
			}
			else if (this.nordicRetreatIsOn)
			{
				this.HookController.StartWorkWithUser(this);
				this.HookController.StartWorkWithUnits(this);
			}
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x0003ADDB File Offset: 0x00038FDB
		private void SetEndingMoveButtonsInteractable(bool interactable)
		{
			if (interactable)
			{
				GameController.Instance.EndTurnButtonEnable();
				return;
			}
			GameController.Instance.endTurnHintType = GameController.EndTurnHintType.Combat;
			GameController.Instance.EndTurnButtonDisable();
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x000B3CF0 File Offset: 0x000B1EF0
		public void ChangeLayout(CombatStage stage)
		{
			if (stage == CombatStage.SelectingBattlefield)
			{
				GameController.Instance.undoController.TriggerUndoInteractivityChange(false);
			}
			if (GameController.GameManager.IsMultiplayer && stage == CombatStage.Preparation)
			{
				this.battlefields = new List<GameHex>(GameController.GameManager.combatManager.GetBattlefields());
			}
			if (stage != GameController.GameManager.combatManager.GetActualStage())
			{
				return;
			}
			Player actualPlayer = GameController.GameManager.combatManager.GetActualPlayer();
			if (actualPlayer == null && stage != CombatStage.CombatResovled)
			{
				return;
			}
			if (!this.CanShowPresenterInMulti(actualPlayer))
			{
				return;
			}
			this.ClosePreviousBattleResult(stage);
			if (this.previousCombatStage == CombatStage.CombatResovled)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			}
			this.previousCombatStage = stage;
			if (GameController.GameManager.TestingMode)
			{
				return;
			}
			this.ChangeLayoutNextStage(stage);
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x000B3DA4 File Offset: 0x000B1FA4
		private bool CanShowPresenterInMulti(Player actualPlayer)
		{
			if (!GameController.GameManager.IsMultiplayer)
			{
				return true;
			}
			CombatStage actualStage = GameController.GameManager.combatManager.GetActualStage();
			return actualStage == CombatStage.DeterminatingTheWinner || actualStage == CombatStage.CombatResovled || (!GameController.GameManager.SpectatorMode && ((actualStage == CombatStage.SelectingBattlefield && GameController.GameManager.IsMyTurn()) || (GameController.GameManager.combatManager.IsPlayerInCombat(GameController.GameManager.PlayerOwner) && (actualPlayer == GameController.GameManager.PlayerOwner || (actualStage == CombatStage.Preparation && !GameController.GameManager.combatManager.IsPowerSet(GameController.GameManager.PlayerOwner) && !this.preperationPanel.activeInHierarchy)))));
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x0003AE00 File Offset: 0x00039000
		private void ClosePreviousBattleResult(CombatStage stage)
		{
			if (GameController.GameManager.IsMultiplayer && (stage == CombatStage.DeterminatingTheWinner || stage == CombatStage.Preparation) && this.resultPanel.GetComponent<BattleResultPanel>().IsOldBattleResultVisible())
			{
				this.resultPanel.GetComponent<BattleResultPanel>().QuickCloseBeforeOpening();
			}
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x000B3E54 File Offset: 0x000B2054
		public void ChangeLayoutNextStage(CombatStage stage)
		{
			Player actualPlayer = GameController.GameManager.combatManager.GetActualPlayer();
			this.ChangeUIState(stage);
			if (actualPlayer != null && !actualPlayer.IsHuman && stage == CombatStage.DeterminatingTheWinner)
			{
				if (GameController.GameManager.combatManager.GetEnemyOf(actualPlayer).IsHuman)
				{
					this.ShowBattleResult();
				}
				else
				{
					this.EnqueueBattleResult();
				}
			}
			if (actualPlayer == null || actualPlayer.IsHuman)
			{
				if (!GameController.GameManager.IsMultiplayer)
				{
					GameController.Instance.turnInfoPanel.DeactivateTurnInfoPanel();
					if (actualPlayer != null && actualPlayer.IsHuman)
					{
						this.statsRegular.UpdateAllStats(actualPlayer, GameController.factionInfo[actualPlayer.matFaction.faction].logo);
					}
				}
				else
				{
					this.statsRegular.UpdateAllStats(GameController.GameManager.PlayerOwner, GameController.factionInfo[GameController.GameManager.PlayerOwner.matFaction.faction].logo);
				}
				switch (stage)
				{
				case CombatStage.SelectingBattlefield:
					GameController.Instance.gameBoardPresenter.UpdateBoard(false, true);
					if (GameController.GameManager.combatManager.GetAttacker() != null && GameController.GameManager.combatManager.GetAttacker().IsHuman)
					{
						this.SwitchToBattleSelectionStage();
						return;
					}
					break;
				case CombatStage.Diversion:
					if (actualPlayer != null)
					{
						this.ChangeLayoutToDiversion();
						return;
					}
					break;
				case CombatStage.Preparation:
					if (actualPlayer != null)
					{
						this.ChangeLayoutToPreperation();
						return;
					}
					break;
				case CombatStage.DeterminatingTheWinner:
					this.ShowBattleResult();
					return;
				case CombatStage.EndingTheBattle:
					this.ChangeLayoutToResolvingCombatStage();
					return;
				case CombatStage.CombatResovled:
					this.Clear();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x0003AE38 File Offset: 0x00039038
		private void ChangeUIState(CombatStage stage)
		{
			if (stage == CombatStage.SelectingBattlefield)
			{
				this.ChangeUIState(true, false);
				return;
			}
			if (stage - CombatStage.Diversion > 1)
			{
				return;
			}
			this.ChangeUIState(false, false);
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000B3FC8 File Offset: 0x000B21C8
		public void ChangeUIState(bool enabled, bool statsOnly)
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				return;
			}
			if (!statsOnly)
			{
				for (int i = 0; i < this.hotseatOpponentHideUI.Length; i++)
				{
					this.hotseatOpponentHideUI[i].SetActive(enabled);
				}
			}
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x0003AE55 File Offset: 0x00039055
		public void UpdateStats(Player player, Sprite logo)
		{
			this.statsRegular.UpdateAllStats(player, logo);
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x000B4008 File Offset: 0x000B2208
		private void UpdateStatsForCombat(Player player)
		{
			GameController.FactionInfo factionInfo = GameController.factionInfo[player.matFaction.faction];
			GameController.Instance.matFaction.UpdateMat(player, factionInfo, GameController.GameManager.actionManager.GetLastSelectedGainAction() == null);
			GameController.Instance.matPlayer.UpdateMat(player, false);
			GameController.Instance.playerStats.UpdateAllStats(player, factionInfo.logo);
			GameController.Instance.panelInfo.UpdatePlayerInfo(factionInfo, GameController.GameManager.factionBasicInfo[player.matFaction.faction], player.objectiveCards, player.combatCards, false);
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x0003AE64 File Offset: 0x00039064
		public void RemoveDelegater()
		{
			GameController.GameManager.combatManager.OnCombatStageChanged -= this.ChangeLayout;
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x0003AE81 File Offset: 0x00039081
		public void AddDelegate()
		{
			this.RemoveDelegater();
			GameController.GameManager.combatManager.OnCombatStageChanged += this.ChangeLayout;
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x000B40AC File Offset: 0x000B22AC
		private void SwitchToBattleSelectionStage()
		{
			if (!GameController.GameManager.IsMultiplayer && this.previousPlayer != null && GameController.GameManager.combatManager.GetAttacker() != GameController.Instance.combatPresenter.GetPreviousPlayer() && GameController.GameManager.combatManager.GetAttacker().IsHuman)
			{
				this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(GameController.GameManager.combatManager.GetAttacker());
			}
			PlayerUnits.ChangeUnitsColliderState(false);
			this.SetEndingMoveButtonsInteractable(false);
			this.mapDarken.enabled = true;
			this.SetPreviousPlayer(GameController.GameManager.combatManager.GetAttacker());
			GameController.Instance.selectedBattlefieldEffects.SetActive(false);
			GameController.HexGetFocused += this.OnBattlefieldSelected;
			GameController.HexSelectionMode = GameController.SelectionMode.Combat;
			if (this.selectedHex != null)
			{
				this.selectedHex.SetFocus(false, HexMarkers.MarkerType.Move, 0.5f, false);
			}
			this.battlefieldLayer.gameObject.SetActive(true);
			this.battlefieldLayer.Init();
			foreach (GameHex gameHex in GameController.GameManager.combatManager.GetBattlefields())
			{
				Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = GameController.Instance.gameBoardPresenter.GetGameHexPresenter(gameHex);
				gameHexPresenter.SetFocus(true, HexMarkers.MarkerType.Battle, 0f, false);
				this.battlefieldLayer.AddHex(gameHexPresenter);
			}
			PlayerUnits.ChangeUnitsColliderState(false);
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x000B4228 File Offset: 0x000B2428
		private void OnBattlefieldSelected(Scythe.BoardPresenter.GameHexPresenter gameHexPres)
		{
			if (GameController.GameManager.combatManager.GetActualStage() == CombatStage.SelectingBattlefield && GameController.GameManager.combatManager.GetBattlefields().Contains(gameHexPres.GetGameHexLogic()))
			{
				WorldSFXManager.PlaySound(SoundEnum.AttackBegnActionCrossClick, AudioSourceType.WorldSfx);
				PlayerUnits.ChangeUnitsColliderState(true);
				this.OnBattlefieldClicked(gameHexPres);
			}
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x000B4278 File Offset: 0x000B2478
		private void OnBattlefieldClicked(Scythe.BoardPresenter.GameHexPresenter gameHexPres)
		{
			if (this == null)
			{
				return;
			}
			if (this.mapDarken == null)
			{
				return;
			}
			if (CombatPresenter.OnBattleFieldSelected != null)
			{
				CombatPresenter.OnBattleFieldSelected();
			}
			GameController.HexGetFocused -= this.OnBattlefieldSelected;
			GameController.HexSelectionMode = GameController.SelectionMode.Normal;
			this.mapDarken.enabled = false;
			this.battlefieldLayer.gameObject.SetActive(false);
			this.PlaceSelectedBattlefieldEffects(gameHexPres.GetWorldPosition());
			if (GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerOwner != GameController.GameManager.PlayerCurrent)
			{
				return;
			}
			if (this.selectedHex != null)
			{
				this.selectedHex.SetFocus(false, HexMarkers.MarkerType.Battle, 0f, false);
			}
			this.selectedHex = gameHexPres;
			GameController.GameManager.combatManager.SelectBattlefield(gameHexPres.GetGameHexLogic());
			GameController.GameManager.combatManager.SwitchToNextStage();
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x000B435C File Offset: 0x000B255C
		private void PlaceSelectedBattlefieldEffects(Vector2 hexPosition)
		{
			Vector3 vector = new Vector3(hexPosition.x, 0f, hexPosition.y);
			GameController.Instance.selectedBattlefieldEffects.transform.position = vector;
			GameController.Instance.selectedBattlefieldEffects.SetActive(true);
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x0003AEA4 File Offset: 0x000390A4
		public void PlaceSelectedBattlefieldEffect(GameHex hex)
		{
			this.PlaceSelectedBattlefieldEffects(GameController.Instance.GetGameHexPresenter(hex).GetWorldPosition());
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x000B43A8 File Offset: 0x000B25A8
		public void ShowInfoAboutUsedCombatAbility()
		{
			Player playerMaster = GameController.GameManager.PlayerMaster;
			if (this.abilityUsed && (playerMaster == GameController.GameManager.combatManager.GetAttacker() || playerMaster == GameController.GameManager.combatManager.GetDefender()))
			{
				this.abilityUsed = false;
				this.abilityInformation.ShowInfoAboutUsedCombatAbility(this.combatAbility);
			}
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x0003AEBC File Offset: 0x000390BC
		public void TurnOffBattlefieldEffect()
		{
			GameController.Instance.selectedBattlefieldEffects.SetActive(false);
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x000B4404 File Offset: 0x000B2604
		private void OnUnitSelected(UnitPresenter unit)
		{
			if (unit.UnitLogic.Owner != GameController.GameManager.combatManager.GetDefeated())
			{
				return;
			}
			if (this.unitsToWithdraw.Contains(unit.UnitLogic))
			{
				GameController.HexGetFocused -= this.OnWithdrawPositionSelected;
				this.ShowUnit(unit);
			}
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000B445C File Offset: 0x000B265C
		public void ChangeLayoutToDiversion()
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.combat_diversion_panel, Contexts.ingame);
			this.diversionPanel.SetActive(true);
			this.diversionPanel.GetComponent<DiversionPresenter>().ChangeLayoutForPlayer(GameController.GameManager.combatManager.GetActualPlayer(), GameController.GameManager.combatManager.GetSelectedBattlefield());
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x0003AECE File Offset: 0x000390CE
		public void EndDiversion()
		{
			this.diversionPanel.GetComponent<DiversionPresenter>().DiversionEnd();
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000B44B0 File Offset: 0x000B26B0
		public void ChangeLayoutToPreperation()
		{
			if (!this.skipPreparation)
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.combat_preparation_panel, Contexts.ingame);
				Player player = GameController.GameManager.combatManager.GetActualPlayer();
				if (GameController.GameManager.IsMultiplayer)
				{
					player = GameController.GameManager.PlayerOwner;
				}
				this.preperationPanel.GetComponent<CombatPreperationPresenter>().ChangeLayoutForPreparation(player, 0);
			}
			this.skipPreparation = false;
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void ChangePreperationWindowVisibility(bool visible)
		{
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x000B4514 File Offset: 0x000B2714
		private void EnqueueBattleResult()
		{
			CombatPresenter.battleResultQueue.Enqueue(new BattleResult
			{
				battlefield = GameController.GameManager.combatManager.GetSelectedBattlefield(),
				attackerIsWinner = GameController.GameManager.combatManager.AttackerIsWinner(),
				attacker = GameController.GameManager.combatManager.GetAttacker(),
				defender = GameController.GameManager.combatManager.GetDefender(),
				usedPower = new Dictionary<Player, PowerSelected>(GameController.GameManager.combatManager.GetUsedPowers())
			});
			if (GameController.GameManager.IsCampaign && GameController.GameManager.missionId == 2)
			{
				CombatPresenter.OnBattleEnd(GameController.GameManager.combatManager.AttackerIsWinner(), GameController.GameManager.combatManager.GetAttacker(), GameController.GameManager.combatManager.GetDefender());
			}
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x000B45F0 File Offset: 0x000B27F0
		private void ShowBattleResult()
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.combat_result_panel, Contexts.ingame);
			this.resultPanel.GetComponent<BattleResultPanel>().ChangeLayout(GameController.GameManager.combatManager.GetSelectedBattlefield(), GameController.GameManager.combatManager.AttackerIsWinner(), GameController.GameManager.combatManager.GetAttacker(), GameController.GameManager.combatManager.GetDefender(), GameController.GameManager.combatManager.GetUsedPowers());
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x000B4668 File Offset: 0x000B2868
		private void ChangeLayoutToResolvingCombatStage()
		{
			if (GameController.GameManager.combatManager.CanActualPlayerGetCombatCard())
			{
				if (!GameController.GameManager.IsMultiplayer && GameController.GameManager.combatManager.GetAttacker().IsHuman)
				{
					this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(GameController.GameManager.combatManager.GetDefeated());
					this.SetPreviousPlayer(GameController.GameManager.combatManager.GetDefeated());
				}
				if (GameController.GameManager.combatManager.GetDefeated().IsHuman)
				{
					this.EnableAdditionalCombatCard();
				}
				return;
			}
			if (!this.selectingBonusCard)
			{
				Player actualPlayer = GameController.GameManager.combatManager.GetActualPlayer();
				if (actualPlayer.matFaction.faction == Faction.Polania)
				{
					if (!GameController.GameManager.IsMultiplayer && actualPlayer != GameController.Instance.combatPresenter.GetPreviousPlayer() && GameController.GameManager.combatManager.GetAttacker().IsHuman && !GameController.GameManager.combatManager.WorkersRetreat())
					{
						this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(GameController.GameManager.combatManager.GetDefeated());
						this.SetPreviousPlayer(actualPlayer);
					}
					this.EnablePolaniaAbility();
				}
				else if (actualPlayer.matFaction.faction == Faction.Nordic)
				{
					if (!GameController.GameManager.IsMultiplayer && actualPlayer != GameController.Instance.combatPresenter.GetPreviousPlayer() && GameController.GameManager.combatManager.GetAttacker().IsHuman)
					{
						this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(GameController.GameManager.combatManager.GetDefeated());
						this.SetPreviousPlayer(actualPlayer);
					}
					this.EnableNordicAbility();
				}
			}
			if (this.selectedHex != null)
			{
				this.selectedHex.SetFocus(false, HexMarkers.MarkerType.Battle, 0f, false);
			}
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x000B4820 File Offset: 0x000B2A20
		private void EnableAdditionalCombatCard()
		{
			this.selectionPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = ScriptLocalization.Get("GameScene/DefenderDefeatBonus");
			this.selectionPanel.transform.Find("Info").GetComponent<Text>().text = ScriptLocalization.Get("GameScene/CombatCardForFight");
			this.selectingBonusCard = true;
			Faction faction = GameController.GameManager.combatManager.GetActualPlayer().matFaction.faction;
			this.selectionPanel.transform.Find("FactionLogo").GetComponent<Image>().sprite = GameController.factionInfo[faction].logo;
			this.selectionPanel.SetActive(true);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.combat_card_bonus_panel, Contexts.ingame);
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x000B48F0 File Offset: 0x000B2AF0
		private void EnablePolaniaAbility()
		{
			this.selectionPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = ScriptLocalization.Get("GameScene/DefenderDefeatBonus");
			this.selectionPanel.transform.Find("Info").GetComponent<Text>().text = ScriptLocalization.Get("GameScene/CombatPolaniaAbility");
			this.selectingBonusCard = false;
			Faction faction = GameController.GameManager.combatManager.GetActualPlayer().matFaction.faction;
			this.selectionPanel.transform.Find("FactionLogo").GetComponent<Image>().sprite = GameController.factionInfo[faction].logo;
			this.selectionPanel.SetActive(true);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.combat_ability_panel, Contexts.ingame);
			if (!GameController.GameManager.IsMultiplayer)
			{
				this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(GameController.GameManager.PlayerCurrent);
			}
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x000B49E0 File Offset: 0x000B2BE0
		private void EnableTogawaAbility()
		{
			this.selectionPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = ScriptLocalization.Get("GameScene/ArmTrap");
			this.selectionPanel.transform.Find("Info").GetComponent<Text>().text = ScriptLocalization.Get("GameScene/ArmTrapInfo");
			this.selectingBonusCard = false;
			Faction faction = GameController.GameManager.combatManager.GetActualPlayer().matFaction.faction;
			this.selectionPanel.transform.Find("FactionLogo").GetComponent<Image>().sprite = GameController.factionInfo[faction].logo;
			this.selectionPanel.SetActive(true);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.combat_ability_panel, Contexts.ingame);
			if (!GameController.GameManager.IsMultiplayer)
			{
				this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(GameController.GameManager.PlayerCurrent);
			}
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x000B4AD0 File Offset: 0x000B2CD0
		public void OnSelectionPanelRespondClicked(bool choice)
		{
			if (choice)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_accept_button);
			}
			else
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_reject_button);
			}
			if ((this.selectingBonusCard && (GameController.GameManager.combatManager.GetAttacker().matFaction.faction != Faction.Polania || !GameController.GameManager.combatManager.CanUseAfterBattleAbility(GameController.GameManager.combatManager.GetAttacker()))) || (GameController.GameManager.combatManager.GetAttacker().matFaction.faction == Faction.Polania && GameController.GameManager.combatManager.CanUseAfterBattleAbility(GameController.GameManager.combatManager.GetAttacker())))
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
			}
			this.SelectionPanelRespond(choice);
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x000B4B80 File Offset: 0x000B2D80
		public void SelectionPanelRespond(bool choice)
		{
			if (this.selectingBonusCard)
			{
				GameController.GameManager.combatManager.AddCombatCard(choice);
				this.selectingBonusCard = false;
			}
			else if (GameController.GameManager.combatManager.GetActualPlayer().matFaction.faction == Faction.Polania)
			{
				GameController.GameManager.combatManager.PolaniaCamaraderieChoice(choice);
				if (choice && !GameController.GameManager.IsMultiplayer)
				{
					this.SetLastUsedAbility(AbilityPerk.Camaraderie);
				}
			}
			this.selectionPanel.SetActive(false);
			GameController.GameManager.combatManager.SwitchToNextStage();
			Player actualPlayer = GameController.GameManager.combatManager.GetActualPlayer();
			if (!GameController.GameManager.IsMultiplayer && actualPlayer != null && actualPlayer.IsHuman)
			{
				GameController.Instance.UpdateStats(actualPlayer, true, true);
			}
			else
			{
				GameController.Instance.UpdateStats(true, true);
			}
			GameController.Instance.matFaction.combatCardsPresenter.Refresh();
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x0003AEE0 File Offset: 0x000390E0
		private Unit GetRandomCombatUnitOnCurrentCombatField(Player player)
		{
			if (GameController.GameManager.combatManager.GetSelectedBattlefield() == null || player == null)
			{
				return null;
			}
			return GameController.GameManager.combatManager.GetSelectedBattlefield().GetCombatUnit(player);
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x000B4C64 File Offset: 0x000B2E64
		private void EnableNordicAbility()
		{
			this.selectedUnit = null;
			this.nordicAbilityEnabled = true;
			if (!GameController.GameManager.IsMultiplayer)
			{
				Player defeated = GameController.GameManager.combatManager.GetDefeated();
				this.ChangeUIState(true, false);
				GameController.Instance.matPlayer.isPreview = true;
				GameController.Instance.matPlayer.UpdateMat(defeated, false);
				this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(defeated);
				this.UpdateStatsForCombat(defeated);
			}
			if (GameController.GameManager.IsHotSeat && GameController.GameManager.GetPlayersWithoutAICount() > 1)
			{
				GameController.Instance.HideHumanPlayerInfo();
			}
			PlayerUnits.ChangeUnitsColliderState(true);
			Camera.main.GetComponent<OutlineEffect>().ActivateFlashing(true);
			this.withdrawPositions = GameController.GameManager.combatManager.GetWithdrawPositions();
			this.unitsToWithdraw = GameController.GameManager.combatManager.GetUnitsToWithdraw();
			this.workersOnBattlefield = GameController.GameManager.combatManager.AreDefeatedWorkersOnBattlefield();
			this.exchangePanel.GetComponent<ExchangePanelPresenter>().DragAndDropBar.AttachExchangePanel(this.exchangePanel.GetComponent<ExchangePanelPresenter>());
			GameController.HexGetFocused += this.OnWithdrawPositionSelected;
			GameController.UnitGetFocused += this.OnUnitSelected;
			GameController.HexSelectionMode = GameController.SelectionMode.Combat;
			this.ShowWithdrawUnits();
			this.ShowWithdrawPositions();
			GameController.factionUnits[Faction.Nordic].SetColliders(true);
			if (!GameController.GameManager.IsMultiplayer)
			{
				GameController.Instance.undoController.TriggerUndoInteractivityChange(false);
			}
			this.HookController.StartWorkWithUser(this);
			this.HookController.StartWorkWithUnits(this);
			this.nordicRetreatIsOn = true;
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x000B4DF4 File Offset: 0x000B2FF4
		private void ShowWithdrawUnits()
		{
			foreach (Unit unit in this.unitsToWithdraw)
			{
				this.SetHighlightOnUnit(unit, 0);
			}
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x0003AF0D File Offset: 0x0003910D
		private void SetHighlightOnUnit(Unit unit, int color = 0)
		{
			UnitPresenter unitPresenter = GameController.GetUnitPresenter(unit);
			if (!this.unitsToWithdraw.Contains(unit))
			{
				this.unitsToWithdraw.Add(unit);
			}
			unitPresenter.SetFocus(true, color);
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x000B4E48 File Offset: 0x000B3048
		private void ShowWithdrawPositions()
		{
			bool flag = GameController.GameManager.combatManager.AreDefeatedMechsOnBattlefield();
			foreach (GameHex gameHex in this.withdrawPositions)
			{
				Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = GameController.Instance.GetGameHexPresenter(gameHex);
				gameHexPresenter.SetFocus(false, HexMarkers.MarkerType.FieldSelected, 0f, false);
				GameController.Instance.gameBoardPresenter.GetGameHexPresenter(gameHex).SetFocus(false, HexMarkers.MarkerType.Move, 0.5f, false);
				if (this.selectedUnit == null)
				{
					if (gameHex.hexType == HexType.capital || flag || !this.workersOnBattlefield)
					{
						gameHexPresenter.SetFocus(true, HexMarkers.MarkerType.Move, 1f, false);
					}
					else
					{
						gameHexPresenter.SetFocus(true, HexMarkers.MarkerType.RetreatWithoutWorkers, 0.3f, false);
					}
				}
				else if (this.selectedUnit.UnitLogic is Worker)
				{
					if (gameHex.hexType == HexType.capital)
					{
						gameHexPresenter.SetFocus(true, HexMarkers.MarkerType.Move, 1f, false);
					}
				}
				else
				{
					gameHexPresenter.SetFocus(true, HexMarkers.MarkerType.Move, 1f, false);
				}
			}
			if (this.selectedUnit == null)
			{
				if (!flag && this.workersOnBattlefield)
				{
					this.nordicRetreatInfoText.text = ScriptLocalization.Get("GameScene/SeaworthyWorkerInfo");
				}
				else
				{
					this.nordicRetreatInfoText.text = ScriptLocalization.Get("GameScene/SeaworthyInfo");
				}
			}
			else if (this.selectedUnit.UnitLogic is Worker)
			{
				this.nordicRetreatInfoText.text = ScriptLocalization.Get("GameScene/SeaworthyWorkerInfo");
			}
			else
			{
				this.nordicRetreatInfoText.text = ScriptLocalization.Get("GameScene/SeaworthyInfo");
			}
			this.nordicRetreatInfo.SetActive(true);
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x0003AF36 File Offset: 0x00039136
		public Unit GetSelectedUnit()
		{
			return this.selectedUnit.UnitLogic;
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public bool CursorRaycastBlocked()
		{
			return false;
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x000B4FF4 File Offset: 0x000B31F4
		public void CursorHitUnit(UnitPresenter unitUnderTheCursor)
		{
			Unit unitLogic = this.selectedUnit.UnitLogic;
			UnitPresenter unitPresenter = this.selectedUnit;
			if (unitLogic.UnitType == UnitType.Worker)
			{
				GameController.HexGetFocused -= this.OnWithdrawPositionSelected;
				unitPresenter.transform.DOComplete(true);
				this.ShowUnit(unitUnderTheCursor, SelectMethod.LoadingWorker);
				this.HookController.Detach(unitUnderTheCursor.transform.position, true);
				this.exchangePanel.GetComponent<ExchangePanelPresenter>().LoadWorker(unitLogic);
				GameController.UnitGetFocused += this.OnUnitSelected;
			}
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x000B507C File Offset: 0x000B327C
		public void CursorHitHex()
		{
			if (!this.exchangePanel.GetComponent<ExchangePanelPresenter>().DragAndDropBar.PreviousUnitEqualsUnit(this.selectedUnit.UnitLogic))
			{
				this.exchangePanel.GetComponent<ExchangePanelPresenter>().ClearPreviousUnit(true);
				this.exchangePanel.GetComponent<ExchangePanelPresenter>().DragAndDropBar.OnUnitChanged(this.selectedUnit.UnitLogic, SelectMethod.Default);
			}
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x0003AF43 File Offset: 0x00039143
		public void CursorNoHit()
		{
			this.HookController.Detach(this.selectedUnit.hex.GetUnitPosition(this.selectedUnit.UnitLogic), false);
			GameController.UnitGetFocused += this.OnUnitSelected;
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x0003AF7D File Offset: 0x0003917D
		public ExchangePanelPresenter GetExchangePanel()
		{
			return this.exchangePanel.GetComponent<ExchangePanelPresenter>();
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x000B50E0 File Offset: 0x000B32E0
		public bool UnitUnderTheCursorIsCorrect(UnitPresenter unitUnderTheCursor)
		{
			Unit unit = this.GetSelectedUnit();
			return unitUnderTheCursor.UnitLogic.UnitType == UnitType.Mech && unitUnderTheCursor.UnitLogic.position == unit.position && unitUnderTheCursor.UnitLogic.Owner == unit.Owner;
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x000B512C File Offset: 0x000B332C
		private void ShowUnit(UnitPresenter unit, SelectMethod selectMethod = SelectMethod.Default)
		{
			if (this.selectedUnit != null)
			{
				if (this.unitsToWithdraw.Contains(this.selectedUnit.UnitLogic))
				{
					this.SetHighlightOnUnit(this.selectedUnit.UnitLogic, 0);
				}
				else
				{
					this.selectedUnit.SetFocus(false, 0);
				}
			}
			this.selectedUnit = unit;
			GameController.HexGetFocused += this.OnWithdrawPositionSelected;
			this.SetHighlightOnUnit(unit.UnitLogic, 1);
			if (unit.UnitLogic is Mech)
			{
				if (unit.UnitLogic.Owner.matFaction.faction == GameController.GameManager.combatManager.GetDefender().matFaction.faction && unit.UnitLogic.position.GetOwnerWorkers().Count > 0)
				{
					this.exchangePanel.GetComponent<ExchangePanelPresenter>().SetPanelParameters(unit.UnitLogic, true, selectMethod);
					this.exchangePanel.SetActive(true);
				}
				if (unit.UnitLogic.Owner.matFaction.faction == GameController.GameManager.combatManager.GetAttacker().matFaction.faction && unit.UnitLogic.position.GetEnemyWorkers().Count > 0)
				{
					this.exchangePanel.GetComponent<ExchangePanelPresenter>().SetPanelParameters(unit.UnitLogic, true, selectMethod);
					this.exchangePanel.SetActive(true);
				}
			}
			else
			{
				this.exchangePanel.SetActive(false);
			}
			this.ShowWithdrawPositions();
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x0003AF8A File Offset: 0x0003918A
		private void OnUnitStateChanged(UnitState unitState, UnitPresenter unitPresenter)
		{
			if (this.selectedUnit == unitPresenter && unitState == UnitState.Standing)
			{
				UnitPresenter.UnitStatusChanged -= this.OnUnitStateChanged;
				this.exchangePanel.GetComponent<ExchangePanelPresenter>().ClearPreviousUnit(true);
				this.selectedUnit = null;
			}
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x0003AFC6 File Offset: 0x000391C6
		public void OnConnectionBroken()
		{
			GameController.UnitGetFocused += this.OnUnitSelected;
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x000B52A4 File Offset: 0x000B34A4
		private void ShowUnit(UnitPresenter unit)
		{
			if (this.selectedUnit != null)
			{
				if (this.unitsToWithdraw.Contains(this.selectedUnit.UnitLogic))
				{
					this.SetHighlightOnUnit(this.selectedUnit.UnitLogic, 0);
				}
				else
				{
					this.selectedUnit.SetFocus(false, 0);
				}
			}
			this.selectedUnit = unit;
			GameController.HexGetFocused += this.OnWithdrawPositionSelected;
			if (GameController.Instance.DragAndDrop)
			{
				this.HookController.Attach(unit.gameObject, unit);
			}
			this.SetHighlightOnUnit(unit.UnitLogic, 1);
			if (unit.UnitLogic is Mech)
			{
				if (unit.UnitLogic.Owner.matFaction.faction == GameController.GameManager.combatManager.GetDefender().matFaction.faction && unit.UnitLogic.position.GetOwnerWorkers().Count > 0)
				{
					this.exchangePanel.GetComponent<ExchangePanelPresenter>().SetPanelParameters(unit.UnitLogic, true, SelectMethod.Default);
					this.exchangePanel.SetActive(true);
				}
				if (unit.UnitLogic.Owner.matFaction.faction == GameController.GameManager.combatManager.GetAttacker().matFaction.faction && unit.UnitLogic.position.GetEnemyWorkers().Count > 0)
				{
					this.exchangePanel.GetComponent<ExchangePanelPresenter>().SetPanelParameters(unit.UnitLogic, true, SelectMethod.Default);
					this.exchangePanel.SetActive(true);
				}
			}
			else
			{
				this.exchangePanel.SetActive(false);
			}
			this.ShowWithdrawPositions();
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x000B5438 File Offset: 0x000B3638
		private void OnWithdrawPositionSelected(Scythe.BoardPresenter.GameHexPresenter retreatPosition)
		{
			if (!this.withdrawPositions.Contains(retreatPosition.GetGameHexLogic()))
			{
				if (this.selectedUnit != null && GameController.Instance.DragAndDrop)
				{
					this.HookController.Detach(this.selectedUnit.hex.GetUnitPosition(this.selectedUnit.UnitLogic), false);
				}
				return;
			}
			GameHex gameHexLogic = retreatPosition.GetGameHexLogic();
			CombatManager combatManager = GameController.GameManager.combatManager;
			bool flag = GameController.GameManager.combatManager.AreDefeatedMechsOnBattlefield();
			bool flag2 = GameController.GameManager.combatManager.IsDefeatedCharacterOnBattlefield();
			if (this.selectedUnit == null)
			{
				this.hookUsed = false;
				if (gameHexLogic.hexType == HexType.capital || (flag && this.workersOnBattlefield) || ((flag || flag2) && !this.workersOnBattlefield))
				{
					combatManager.WithdrawToPosition(this.unitsToWithdraw, gameHexLogic);
				}
			}
			else
			{
				if (!combatManager.GetUnitsToWithdraw().Contains(this.selectedUnit.UnitLogic))
				{
					return;
				}
				UnitPresenter.UnitStatusChanged += this.OnUnitStateChanged;
				if (this.selectedUnit.UnitLogic is Worker && gameHexLogic.hexType != HexType.capital)
				{
					if (GameController.Instance.DragAndDrop)
					{
						this.HookController.Detach(this.selectedUnit.hex.GetUnitPosition(this.selectedUnit.UnitLogic), false);
					}
					return;
				}
				if (GameController.Instance.DragAndDrop && this.HookController.IsUnitDragged())
				{
					this.HookController.Detach(retreatPosition.GetUnitPosition(this.selectedUnit.UnitLogic), false);
					this.hookUsed = true;
				}
				List<Unit> list = new List<Unit>();
				list.Add(this.selectedUnit.UnitLogic);
				if (this.selectedUnit.UnitLogic is Mech && this.exchangePanel.GetComponent<ExchangePanelPresenter>().GetContext().LoadedWorkers.Count != 0)
				{
					GameController.GameManager.combatManager.LoadWorkers(this.exchangePanel.GetComponent<ExchangePanelPresenter>().GetContext().LoadedWorkers, this.selectedUnit.UnitLogic as Mech);
					list.AddRange(this.exchangePanel.GetComponent<ExchangePanelPresenter>().GetContext().LoadedWorkers);
				}
				combatManager.WithdrawToPosition(list, gameHexLogic);
				this.selectedUnit.SetFocus(false, 0);
			}
			this.unitsToWithdraw = combatManager.GetUnitsToWithdraw();
			if (this.unitsToWithdraw.Count == 0)
			{
				this.DisableNordicAbility();
			}
			else
			{
				this.ShowWithdrawPositions();
			}
			this.exchangePanel.SetActive(false);
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x000B56B4 File Offset: 0x000B38B4
		public void MoveAttackerUnitsToOwnerPositions()
		{
			CombatManager combatManager = GameController.GameManager.combatManager;
			List<Unit> ownerUnits = combatManager.GetSelectedBattlefield().GetOwnerUnits();
			new Dictionary<GameHex, GameHex>().Add(combatManager.GetSelectedBattlefield(), combatManager.GetSelectedBattlefield());
			foreach (Unit unit in ownerUnits)
			{
				if ((!GameController.GameManager.IsMultiplayer && unit.Owner.IsHuman) || (GameController.GameManager.IsMultiplayer && unit.Owner.IsHuman && unit.Owner.IsHuman && unit.Owner == GameController.GameManager.PlayerOwner))
				{
					GameController.GetUnitPresenter(unit).RunMoveToOwnerPosition(combatManager.GetSelectedBattlefield(), false);
				}
				else
				{
					MoveEnemyActionInfo moveEnemyActionInfo = new MoveEnemyActionInfo();
					moveEnemyActionInfo.actionOwner = unit.Owner.matFaction.faction;
					moveEnemyActionInfo.actionType = LogInfoType.Move;
					moveEnemyActionInfo.unit = unit;
					moveEnemyActionInfo.positionAfterFight = combatManager.GetSelectedBattlefield();
					moveEnemyActionInfo.destinationIsBattlefield = false;
					moveEnemyActionInfo.takingOwnerPosition = true;
					moveEnemyActionInfo.fromHex = combatManager.GetSelectedBattlefield();
					moveEnemyActionInfo.toHex = combatManager.GetSelectedBattlefield();
					GameController.GameManager.EnemyMoveUnit(moveEnemyActionInfo);
				}
			}
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x000B57F8 File Offset: 0x000B39F8
		private void OnUnitsWithdraw(List<Unit> units, GameHex battlefield, GameHex withdrawPosition)
		{
			Dictionary<GameHex, GameHex> dictionary = new Dictionary<GameHex, GameHex>();
			dictionary.Add(battlefield, null);
			dictionary.Add(withdrawPosition, battlefield);
			bool flag = false;
			if (units[0].Owner == GameController.GameManager.PlayerMaster && units[0].UnitType == UnitType.Mech && (units[0] as Mech).LoadedWorkers.Count != 0)
			{
				flag = true;
			}
			foreach (Unit unit in units)
			{
				GameController.GetUnitPresenter(unit).SetFocus(false, 0);
				if (!this.hookUsed && ((unit != units[0] && flag) || !flag))
				{
					GameController.GetUnitPresenter(unit).RunTheMoveAnimation(dictionary, false, true, false, null, null);
				}
			}
			this.hookUsed = false;
			this.GetExchangePanel().ClearOnUnitsWithdraw();
			this.GetExchangePanel().UpdateHexWorkers(GameController.GameManager.combatManager.GetSelectedBattlefield());
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x000B5900 File Offset: 0x000B3B00
		private void DisableNordicAbility()
		{
			this.nordicRetreatIsOn = false;
			this.HookController.FinishWork();
			this.nordicAbilityEnabled = false;
			Player attacker = GameController.GameManager.combatManager.GetAttacker();
			if (!GameController.GameManager.IsMultiplayer)
			{
				this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(attacker);
				GameController.Instance.undoController.TriggerUndoInteractivityChange(true);
			}
			Camera.main.GetComponent<OutlineEffect>().ActivateFlashing(false);
			this.nordicRetreatInfo.SetActive(false);
			GameController.factionUnits[Faction.Nordic].SetColliders(false);
			GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.combatManager.GetSelectedBattlefield()).SetFocus(false, HexMarkers.MarkerType.Battle, 0f, false);
			foreach (GameHex gameHex in this.withdrawPositions)
			{
				GameController.Instance.gameBoardPresenter.GetGameHexPresenter(gameHex).SetFocus(false, HexMarkers.MarkerType.Move, 0.5f, false);
				GameController.Instance.gameBoardPresenter.GetGameHexPresenter(gameHex).SetFocus(false, HexMarkers.MarkerType.RetreatWithoutWorkers, 0.3f, false);
			}
			this.withdrawPositions = new List<GameHex>();
			this.exchangePanel.SetActive(false);
			this.selectedHex = null;
			GameController.HexGetFocused -= this.OnWithdrawPositionSelected;
			GameController.UnitGetFocused -= this.OnUnitSelected;
			GameController.HexSelectionMode = GameController.SelectionMode.Normal;
			if (GameController.GameManager.IsMultiplayer)
			{
				GameController.GameManager.OnActionSent(new EndNordicSkillMessage((int)GameController.GameManager.PlayerOwner.matFaction.faction));
			}
			if (!GameController.GameManager.IsMultiplayer)
			{
				this.UpdateStatsForCombat(GameController.GameManager.PlayerCurrent);
				GameController.Instance.matPlayer.UpdateMat(GameController.GameManager.PlayerCurrent, false);
				GameController.Instance.matPlayer.isPreview = false;
			}
			GameController.GameManager.combatManager.SwitchToNextStage();
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x0003AFD9 File Offset: 0x000391D9
		public void SetPreviousPlayer(Player previousPlayer)
		{
			this.previousPlayer = previousPlayer;
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x0003AFE2 File Offset: 0x000391E2
		public Player GetPreviousPlayer()
		{
			return this.previousPlayer;
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x0003AFEA File Offset: 0x000391EA
		public Scythe.BoardPresenter.GameHexPresenter GetSelectedHex()
		{
			return this.selectedHex;
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x0003AFF2 File Offset: 0x000391F2
		public void SetLastUsedAbility(AbilityPerk ability)
		{
			this.abilityUsed = true;
			this.combatAbility = ability;
		}

		// Token: 0x040014DC RID: 5340
		public GameObject exchangePanel;

		// Token: 0x040014DD RID: 5341
		public GameObject diversionPanel;

		// Token: 0x040014DE RID: 5342
		public GameObject preperationPanel;

		// Token: 0x040014DF RID: 5343
		public GameObject resultPanel;

		// Token: 0x040014E0 RID: 5344
		public GameObject selectionPanel;

		// Token: 0x040014E1 RID: 5345
		public CombatAbilityInfo abilityInformation;

		// Token: 0x040014E2 RID: 5346
		public GameObject nordicRetreatInfo;

		// Token: 0x040014E3 RID: 5347
		public GameObject turnInfoPanelCombat;

		// Token: 0x040014E4 RID: 5348
		public TextMeshProUGUI nordicRetreatInfoText;

		// Token: 0x040014E5 RID: 5349
		public GameObject[] hotseatOpponentHideUI;

		// Token: 0x040014E6 RID: 5350
		public PlayerStatsPresenter statsRegular;

		// Token: 0x040014E7 RID: 5351
		public Image mapDarken;

		// Token: 0x040014E8 RID: 5352
		public BattlefieldsLayer battlefieldLayer;

		// Token: 0x040014E9 RID: 5353
		public bool skipPreparation;

		// Token: 0x040014EA RID: 5354
		public bool nordicAbilityEnabled;

		// Token: 0x040014EB RID: 5355
		private bool selectingBonusCard;

		// Token: 0x040014EC RID: 5356
		private Scythe.BoardPresenter.GameHexPresenter selectedHex;

		// Token: 0x040014ED RID: 5357
		private UnitPresenter selectedUnit;

		// Token: 0x040014EE RID: 5358
		private List<GameHex> withdrawPositions;

		// Token: 0x040014EF RID: 5359
		private List<Unit> unitsToWithdraw;

		// Token: 0x040014F0 RID: 5360
		private bool workersOnBattlefield;

		// Token: 0x040014F1 RID: 5361
		private Player previousPlayer;

		// Token: 0x040014F2 RID: 5362
		private GameHex selectedBattlefield;

		// Token: 0x040014F3 RID: 5363
		public static Queue<BattleResult> battleResultQueue = new Queue<BattleResult>();

		// Token: 0x040014F6 RID: 5366
		private bool abilityUsed;

		// Token: 0x040014F7 RID: 5367
		private AbilityPerk combatAbility;

		// Token: 0x040014F8 RID: 5368
		private CombatStage previousCombatStage = CombatStage.CombatResovled;

		// Token: 0x040014F9 RID: 5369
		private List<GameHex> battlefields = new List<GameHex>();

		// Token: 0x040014FA RID: 5370
		private bool hookUsed;

		// Token: 0x040014FB RID: 5371
		private bool nordicRetreatIsOn;

		// Token: 0x020003D8 RID: 984
		// (Invoke) Token: 0x06001D35 RID: 7477
		public delegate void BattleEnd(bool attackerIsWinner, Player Attacker, Player Defender);

		// Token: 0x020003D9 RID: 985
		// (Invoke) Token: 0x06001D39 RID: 7481
		public delegate void BattlefieldSelect();
	}
}

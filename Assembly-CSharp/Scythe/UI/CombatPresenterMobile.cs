using System;
using System.Collections.Generic;
using System.Linq;
using cakeslice;
using DG.Tweening;
using HoneyFramework;
using I2.Loc;
using Scythe.Analytics;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.Multiplayer.Messages;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000443 RID: 1091
	public class CombatPresenterMobile : MonoBehaviour, IHooksUnitControllerUser
	{
		// Token: 0x140000DF RID: 223
		// (add) Token: 0x060021C1 RID: 8641 RVA: 0x000C9F20 File Offset: 0x000C8120
		// (remove) Token: 0x060021C2 RID: 8642 RVA: 0x000C9F54 File Offset: 0x000C8154
		public static event CombatPresenterMobile.BattleEnd OnBattleEnd;

		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x060021C3 RID: 8643 RVA: 0x000C9F88 File Offset: 0x000C8188
		// (remove) Token: 0x060021C4 RID: 8644 RVA: 0x000C9FBC File Offset: 0x000C81BC
		public static event CombatPresenterMobile.BattlefieldSelect OnBattleFieldSelected;

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x060021C5 RID: 8645 RVA: 0x0003AD4A File Offset: 0x00038F4A
		private HookController HookController
		{
			get
			{
				return GameController.Instance.hookController;
			}
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x000C9FF0 File Offset: 0x000C81F0
		private void Awake()
		{
			for (int i = 0; i < 3; i++)
			{
				CombatButtonsPanelMobile combatButtonsPanelMobile = global::UnityEngine.Object.Instantiate<CombatButtonsPanelMobile>(this.combatButtonsPanel, this.combatButtonsParent.transform);
				this.combatHexButtons.Add(combatButtonsPanelMobile);
				combatButtonsPanelMobile.gameObject.SetActive(false);
			}
			GameController.GameManager.BattlefieldChoosen += this.OnBattlefieldChosen;
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x0003D96E File Offset: 0x0003BB6E
		private void OnEnable()
		{
			this.AttachListeners();
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x0003D976 File Offset: 0x0003BB76
		private void OnDisable()
		{
			this.RemoveListeners();
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x0003D976 File Offset: 0x0003BB76
		private void OnDestroy()
		{
			this.RemoveListeners();
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x000CA050 File Offset: 0x000C8250
		private void AttachListeners()
		{
			GameController.GameManager.combatManager.OnCombatStageChanged += this.ChangeLayout;
			GameController.GameManager.combatManager.OnWithdrawUnits += this.OnUnitsWithdraw;
			GameController.GameManager.combatManager.TakeOwnersPositions += this.MoveAttackerUnitsToOwnerPositions;
			this.resultPanel.GetComponent<CombatPanelMobile>().CombatPanelClosed += this.OnCombatResolved;
			this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().OnAnyKeyClicked += this.TurnInfoPanelClosed;
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x000CA0E8 File Offset: 0x000C82E8
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
			GameController.HexGetFocused -= this.OnBattleResumed;
			this.resultPanel.GetComponent<CombatPanelMobile>().CombatPanelClosed -= this.OnCombatResolved;
			this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().OnAnyKeyClicked -= this.TurnInfoPanelClosed;
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x0003D97E File Offset: 0x0003BB7E
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

		// Token: 0x060021CD RID: 8653 RVA: 0x000CA1CC File Offset: 0x000C83CC
		public void Clear()
		{
			GameController.HexGetFocused -= this.OnBattleResumed;
			this.ClearData();
			if (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.PlayerOwner.currentMatSection != -1))
			{
				this.SetEndingMoveButtonsInteractable(!GameController.GameManager.GameFinished && GameController.GameManager.PlayerCurrent.IsHuman);
			}
			PlayerUnits.ChangeUnitsColliderState(true);
			GameController.ClearFocus();
			GameController.Instance.gameBoardPresenter.UpdateStaticObjects();
			this.ChangeUIState(true, false);
			if (!GameController.Instance.GameIsLoaded)
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
			}
			for (int i = 0; i < this.combatHexButtons.Count; i++)
			{
				this.combatHexButtons[i].Clear();
			}
			this.combatHexButtonsBattlefieldDictionary.Clear();
			if (!PlatformManager.IsStandalone && GameController.GameManager.moveManager.GetMovesLeft() == 0)
			{
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Visible, 0.25f);
			}
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x000CA2D4 File Offset: 0x000C84D4
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
			this.HideSelectionPanel();
			GameController.Instance.selectedBattlefieldEffects.SetActive(false);
			if (this.mapDarken != null)
			{
				this.mapDarken.enabled = false;
			}
			this.previousPlayer = null;
			this.selectedHex = null;
			this.skipPreparation = false;
			this.battlefieldLayer.Clear();
			this.battlefieldLayer.gameObject.SetActive(false);
			this.previousCombatStage = CombatStage.CombatResovled;
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x0003D9BA File Offset: 0x0003BBBA
		public void OnDragAndDropStateChanged(bool enabled)
		{
			if (this.nordicRetreatIsOn)
			{
				if (!enabled)
				{
					this.HookController.FinishWork();
					return;
				}
				this.HookController.StartWorkWithUser(this);
				this.HookController.StartWorkWithUnits(this);
			}
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x0003ADDB File Offset: 0x00038FDB
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

		// Token: 0x060021D1 RID: 8657 RVA: 0x000CA3FC File Offset: 0x000C85FC
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
			if (this.combatEnemyActionInfo != null)
			{
				this.combatEnemyActionInfo.Hide();
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
				if (this.combatEnemyActionInfo != null && this.IsWaitingForEnemy())
				{
					this.combatEnemyActionInfo.Show(actualPlayer, stage);
				}
				return;
			}
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

		// Token: 0x060021D2 RID: 8658 RVA: 0x000CA4E4 File Offset: 0x000C86E4
		private bool CanShowPresenterInMulti(Player actualPlayer)
		{
			if (!GameController.GameManager.IsMultiplayer)
			{
				return true;
			}
			CombatStage actualStage = GameController.GameManager.combatManager.GetActualStage();
			if (actualStage == CombatStage.DeterminatingTheWinner || actualStage == CombatStage.CombatResovled)
			{
				return true;
			}
			if (GameController.GameManager.SpectatorMode)
			{
				return false;
			}
			if (actualStage == CombatStage.SelectingBattlefield && GameController.GameManager.IsMyTurn())
			{
				return true;
			}
			if (GameController.GameManager.combatManager.IsPlayerInCombat(GameController.GameManager.PlayerOwner))
			{
				if (actualPlayer == GameController.GameManager.PlayerOwner)
				{
					return true;
				}
				if (actualStage == CombatStage.Preparation && !GameController.GameManager.combatManager.IsPowerSet(GameController.GameManager.PlayerOwner) && !this.preperationPanel.activeInHierarchy)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000CA594 File Offset: 0x000C8794
		private bool IsWaitingForEnemy()
		{
			if (GameController.GameManager.SpectatorMode)
			{
				return false;
			}
			CombatStage actualStage = GameController.GameManager.combatManager.GetActualStage();
			if (GameController.GameManager.combatManager.IsPlayerInCombat(GameController.GameManager.PlayerOwner))
			{
				if (actualStage == CombatStage.Diversion)
				{
					return true;
				}
				if (actualStage == CombatStage.EndingTheBattle)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x0003D9EB File Offset: 0x0003BBEB
		private void ClosePreviousBattleResult(CombatStage stage)
		{
			this.resultPanel.GetComponent<CombatPanelMobile>().ClosePanel();
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x000CA5E8 File Offset: 0x000C87E8
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
			if (actualPlayer != null && GameController.GameManager.combatManager.GetAttacker() != null && !GameController.GameManager.combatManager.GetAttacker().IsHuman)
			{
				if (GameController.GameManager.combatManager.GetBattlefields().Count > 0)
				{
					this.CreateCombatButtons(true, null);
				}
				if (!PlatformManager.IsStandalone)
				{
					SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
				}
			}
			if (actualPlayer == null || actualPlayer.IsHuman)
			{
				if (!GameController.GameManager.IsMultiplayer)
				{
					GameController.Instance.turnInfoPanel.DeactivateTurnInfoPanel();
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

		// Token: 0x060021D6 RID: 8662 RVA: 0x0003D9FD File Offset: 0x0003BBFD
		private void OnCombatResolved()
		{
			GameController.HexSelectionMode = GameController.SelectionMode.Combat;
			if (!GameController.GameManager.IsMultiplayer)
			{
				GameController.GameManager.combatManager.SwitchToNextStage();
			}
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x0003DA20 File Offset: 0x0003BC20
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

		// Token: 0x060021D8 RID: 8664 RVA: 0x000CA754 File Offset: 0x000C8954
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

		// Token: 0x060021D9 RID: 8665 RVA: 0x0003DA3D File Offset: 0x0003BC3D
		public void UpdateStats(Player player, Sprite logo)
		{
			this.statsRegular.UpdateAllStats(player, logo);
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x000CA794 File Offset: 0x000C8994
		private void UpdateStatsForCombat(Player player)
		{
			GameController.FactionInfo factionInfo = GameController.factionInfo[player.matFaction.faction];
			GameController.Instance.matFaction.UpdateMat(player, factionInfo, GameController.GameManager.actionManager.GetLastSelectedGainAction() == null);
			GameController.Instance.matPlayer.UpdateMat(player, false);
			GameController.Instance.playerStats.UpdateAllStats(player, factionInfo.logo);
			if (GameController.Instance.panelInfo != null)
			{
				GameController.Instance.panelInfo.UpdatePlayerInfo(factionInfo, GameController.GameManager.factionBasicInfo[player.matFaction.faction], player.objectiveCards, player.combatCards, false);
			}
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x0003DA4C File Offset: 0x0003BC4C
		public void RemoveDelegater()
		{
			GameController.GameManager.combatManager.OnCombatStageChanged -= this.ChangeLayout;
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x0003DA69 File Offset: 0x0003BC69
		public void AddDelegate()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.RemoveDelegater();
			GameController.GameManager.combatManager.OnCombatStageChanged += this.ChangeLayout;
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x000CA84C File Offset: 0x000C8A4C
		private void SwitchToBattleSelectionStage()
		{
			if (!GameController.GameManager.IsMultiplayer && this.previousPlayer != null && GameController.GameManager.combatManager.GetAttacker() != GameController.Instance.combatPresenterMobile.GetPreviousPlayer() && GameController.GameManager.combatManager.GetAttacker().IsHuman)
			{
				this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(GameController.GameManager.combatManager.GetAttacker());
			}
			PlayerUnits.ChangeUnitsColliderState(false);
			this.SetEndingMoveButtonsInteractable(false);
			if (this.mapDarken != null)
			{
				this.mapDarken.enabled = true;
			}
			this.SetPreviousPlayer(GameController.GameManager.combatManager.GetAttacker());
			GameController.Instance.selectedBattlefieldEffects.SetActive(false);
			GameController.HexSelectionMode = GameController.SelectionMode.Combat;
			if (this.selectedHex != null)
			{
				this.selectedHex.SetFocus(false, HexMarkers.MarkerType.Move, 0.5f, false);
			}
			this.battlefieldLayer.gameObject.SetActive(true);
			this.battlefieldLayer.Init();
			this.CreateCombatButtons(true, null);
			SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
			PlayerUnits.ChangeUnitsColliderState(false);
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x000CA96C File Offset: 0x000C8B6C
		private void OnBattlefieldChosen()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				GameHex gameHex = GameController.GameManager.combatManager.GetSelectedBattlefield();
				if (gameHex == null)
				{
					return;
				}
				if (!this.combatHexButtonsBattlefieldDictionary.ContainsKey(gameHex))
				{
					this.CreateCombatButtons(false, gameHex);
				}
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
			}
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x000CA9C0 File Offset: 0x000C8BC0
		private void OnBattlefieldSelected(Scythe.BoardPresenter.GameHexPresenter gameHexPres)
		{
			if (GameController.GameManager.combatManager.GetActualStage() == CombatStage.SelectingBattlefield && GameController.GameManager.combatManager.GetBattlefields().Contains(gameHexPres.GetGameHexLogic()))
			{
				WorldSFXManager.PlaySound(SoundEnum.AttackBegnActionCrossClick, AudioSourceType.WorldSfx);
				PlayerUnits.ChangeUnitsColliderState(true);
				this.OnBattlefieldClicked(gameHexPres);
			}
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x000CAA10 File Offset: 0x000C8C10
		private void OnBattlefieldClicked(Scythe.BoardPresenter.GameHexPresenter gameHexPres)
		{
			if (this == null)
			{
				return;
			}
			if (CombatPresenterMobile.OnBattleFieldSelected != null)
			{
				CombatPresenterMobile.OnBattleFieldSelected();
			}
			GameController.HexGetFocused -= this.OnBattlefieldSelected;
			if (this.mapDarken != null)
			{
				this.mapDarken.enabled = false;
			}
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

		// Token: 0x060021E1 RID: 8673 RVA: 0x000CAAEC File Offset: 0x000C8CEC
		private void OnBattleResumed(Scythe.BoardPresenter.GameHexPresenter gameHexPres)
		{
			Debug.Log("[CombatPresenterMobile] OnBattleResumed");
			GameHex gameHex = GameController.GameManager.combatManager.GetSelectedBattlefield();
			if (gameHex != null && gameHex == gameHexPres.GetGameHexLogic() && GameController.GameManager.combatManager.GetActualStage() != CombatStage.Diversion && ((GameController.GameManager.IsMultiplayer && (GameController.GameManager.combatManager.GetAttacker() == GameController.GameManager.PlayerOwner || GameController.GameManager.combatManager.GetDefender() == GameController.GameManager.PlayerOwner)) || !GameController.GameManager.IsMultiplayer))
			{
				this.preperationPanel.SetActive(true);
			}
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x000B435C File Offset: 0x000B255C
		private void PlaceSelectedBattlefieldEffects(Vector2 hexPosition)
		{
			Vector3 vector = new Vector3(hexPosition.x, 0f, hexPosition.y);
			GameController.Instance.selectedBattlefieldEffects.transform.position = vector;
			GameController.Instance.selectedBattlefieldEffects.SetActive(true);
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x0003DA9A File Offset: 0x0003BC9A
		public void PlaceSelectedBattlefieldEffect(GameHex hex)
		{
			this.PlaceSelectedBattlefieldEffects(GameController.Instance.GetGameHexPresenter(hex).GetWorldPosition());
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x000CAB8C File Offset: 0x000C8D8C
		public void ShowInfoAboutUsedCombatAbility()
		{
			Player playerMaster = GameController.GameManager.PlayerMaster;
			if (this.abilityUsed && (playerMaster == GameController.GameManager.combatManager.GetAttacker() || playerMaster == GameController.GameManager.combatManager.GetDefender()))
			{
				this.abilityUsed = false;
				this.abilityInformation.ShowInfoAboutUsedCombatAbility(this.combatAbility);
			}
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x000CABE8 File Offset: 0x000C8DE8
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

		// Token: 0x060021E6 RID: 8678 RVA: 0x000CAC40 File Offset: 0x000C8E40
		public void ChangeLayoutToDiversion()
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.combat_diversion_panel, Contexts.ingame);
			this.diversionPanel.SetActive(true);
			this.diversionPanel.GetComponent<DiversionPresenter>().ChangeLayoutForPlayer(GameController.GameManager.combatManager.GetActualPlayer(), GameController.GameManager.combatManager.GetSelectedBattlefield());
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x0003DAB2 File Offset: 0x0003BCB2
		public void EndDiversion()
		{
			this.diversionPanel.GetComponent<DiversionPresenter>().DiversionEnd();
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x000CAC94 File Offset: 0x000C8E94
		public void ChangeLayoutToPreperation()
		{
			GameHex gameHex = GameController.GameManager.combatManager.GetSelectedBattlefield();
			if (!this.combatHexButtonsBattlefieldDictionary.ContainsKey(gameHex))
			{
				this.CreateCombatButtons(true, null);
			}
			CombatButtonsPanelMobile combatButtonsPanelMobile = this.combatHexButtonsBattlefieldDictionary[gameHex];
			combatButtonsPanelMobile.CombatButtonClicked -= this.OnBattlefieldSelected;
			combatButtonsPanelMobile.CombatButtonClicked += this.OnBattleResumed;
			combatButtonsPanelMobile.ResumeMode = true;
			combatButtonsPanelMobile.BackToBattleText.gameObject.SetActive(true);
			GameController.GameManager.combatManager.GetBattlefields();
			GameController.HexSelectionMode = GameController.SelectionMode.Combat;
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.combat_preparation_panel, Contexts.ingame);
			Player player = GameController.GameManager.combatManager.GetActualPlayer();
			if (GameController.GameManager.IsMultiplayer)
			{
				player = GameController.GameManager.PlayerOwner;
			}
			this.preperationPanel.SetActive(true);
			if (this.previousPlayer != player && GameController.GameManager.IsHotSeat)
			{
				this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(player);
			}
			this.preperationPanel.GetComponent<CombatPanelMobile>().ChangeLayoutForPreparation(player, GameController.GameManager.combatManager.GetEnemyOf(player));
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void ChangePreperationWindowVisibility(bool visible)
		{
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x000CADAC File Offset: 0x000C8FAC
		private void EnqueueBattleResult()
		{
			this.battleResultQueue.Enqueue(new BattleResult
			{
				battlefield = GameController.GameManager.combatManager.GetSelectedBattlefield(),
				attackerIsWinner = GameController.GameManager.combatManager.AttackerIsWinner(),
				attacker = GameController.GameManager.combatManager.GetAttacker(),
				defender = GameController.GameManager.combatManager.GetDefender(),
				usedPower = new Dictionary<Player, PowerSelected>(GameController.GameManager.combatManager.GetUsedPowers())
			});
			if (GameController.GameManager.IsCampaign && GameController.GameManager.missionId == 2)
			{
				CombatPresenterMobile.OnBattleEnd(GameController.GameManager.combatManager.AttackerIsWinner(), GameController.GameManager.combatManager.GetAttacker(), GameController.GameManager.combatManager.GetDefender());
			}
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x000CAE88 File Offset: 0x000C9088
		private void ShowBattleResult()
		{
			for (int i = 0; i < this.combatHexButtons.Count; i++)
			{
				if (this.combatHexButtons[i].ResumeMode)
				{
					this.combatHexButtons[i].Clear();
				}
			}
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.combat_result_panel, Contexts.ingame);
			this.resultPanel.SetActive(true);
			if (PlatformManager.IsMobile)
			{
				GameController.Instance.tokensController.HidePanels();
			}
			Player player;
			if (GameController.GameManager.IsMultiplayer && GameController.GameManager.combatManager.IsPlayerInCombat(GameController.GameManager.PlayerMaster))
			{
				player = GameController.GameManager.PlayerMaster;
			}
			else
			{
				player = GameController.GameManager.combatManager.GetActualPlayer();
			}
			if (player.IsHuman)
			{
				this.resultPanel.GetComponent<CombatPanelMobile>().ChangeLayoutForResult(player, GameController.GameManager.combatManager.GetEnemyOf(player));
			}
			else
			{
				this.resultPanel.GetComponent<CombatPanelMobile>().ChangeLayoutForResult(GameController.GameManager.combatManager.GetEnemyOf(player), player);
			}
			if (GameController.GameManager.IsMultiplayer)
			{
				GameController.GameManager.combatManager.SwitchToNextStage();
			}
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x000CAFAC File Offset: 0x000C91AC
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
					if (!GameController.GameManager.IsMultiplayer && actualPlayer != GameController.Instance.combatPresenterMobile.GetPreviousPlayer() && GameController.GameManager.combatManager.GetAttacker().IsHuman && !GameController.GameManager.combatManager.WorkersRetreat())
					{
						this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(GameController.GameManager.combatManager.GetDefeated());
						this.turnInfoPanelVisible = true;
						this.SetPreviousPlayer(actualPlayer);
					}
					this.EnablePolaniaAbility();
				}
				else if (actualPlayer.matFaction.faction == Faction.Nordic)
				{
					if (!GameController.GameManager.IsMultiplayer && actualPlayer != GameController.Instance.combatPresenterMobile.GetPreviousPlayer() && GameController.GameManager.combatManager.GetAttacker().IsHuman)
					{
						this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(GameController.GameManager.combatManager.GetDefeated());
						this.turnInfoPanelVisible = true;
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

		// Token: 0x060021ED RID: 8685 RVA: 0x000CB170 File Offset: 0x000C9370
		private void EnableAdditionalCombatCard()
		{
			this.selectingBonusCard = true;
			Faction faction = GameController.GameManager.combatManager.GetActualPlayer().matFaction.faction;
			this.ShowSelectionPanel(ScriptLocalization.Get("GameScene/DefenderDefeatBonus"), ScriptLocalization.Get("GameScene/CombatCardForFight"), GameController.factionInfo[faction].logo, new Action<bool>(this.SelectionPanelRespond));
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.combat_card_bonus_panel, Contexts.ingame);
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x000CB1E8 File Offset: 0x000C93E8
		private void EnablePolaniaAbility()
		{
			this.selectingBonusCard = false;
			Faction faction = GameController.GameManager.combatManager.GetActualPlayer().matFaction.faction;
			this.ShowSelectionPanel(ScriptLocalization.Get("GameScene/CombatAbility"), ScriptLocalization.Get("GameScene/CombatPolaniaAbility"), GameController.factionInfo[faction].logo, new Action<bool>(this.SelectionPanelRespond));
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.combat_ability_panel, Contexts.ingame);
			if (!GameController.GameManager.IsMultiplayer)
			{
				this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(GameController.GameManager.PlayerCurrent);
				this.turnInfoPanelVisible = true;
			}
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x000CB288 File Offset: 0x000C9488
		private void EnableTogawaAbility()
		{
			this.selectingBonusCard = false;
			Faction faction = GameController.GameManager.combatManager.GetActualPlayer().matFaction.faction;
			this.ShowSelectionPanel(ScriptLocalization.Get("GameScene/ArmTrap"), ScriptLocalization.Get("GameScene/ArmTrapInfo"), GameController.factionInfo[faction].logo, new Action<bool>(this.SelectionPanelRespond));
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.combat_ability_panel, Contexts.ingame);
			if (!GameController.GameManager.IsMultiplayer)
			{
				this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(GameController.GameManager.PlayerCurrent);
				this.turnInfoPanelVisible = true;
			}
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x0003DAC4 File Offset: 0x0003BCC4
		public void OnSelectionPanelRespondClicked(bool choice)
		{
			WorldSFXManager.PlaySound(SoundEnum.CommonBgGreenButton, AudioSourceType.Buttons);
			this.selectionPanelCallback(choice);
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x000CB328 File Offset: 0x000C9528
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
			this.HideSelectionPanel();
			GameController.Instance.UpdateStats(true, true);
			GameController.GameManager.combatManager.SwitchToNextStage();
			GameController.Instance.matFaction.combatCardsPresenter.Refresh();
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x0003AEE0 File Offset: 0x000390E0
		private Unit GetRandomCombatUnitOnCurrentCombatField(Player player)
		{
			if (GameController.GameManager.combatManager.GetSelectedBattlefield() == null || player == null)
			{
				return null;
			}
			return GameController.GameManager.combatManager.GetSelectedBattlefield().GetCombatUnit(player);
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x000CB3D0 File Offset: 0x000C95D0
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
				if (!PlatformManager.IsStandalone)
				{
					SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
				}
				this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(defeated);
				this.turnInfoPanelVisible = true;
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

		// Token: 0x060021F4 RID: 8692 RVA: 0x000CB580 File Offset: 0x000C9780
		private void ShowWithdrawUnits()
		{
			foreach (Unit unit in this.unitsToWithdraw)
			{
				this.SetHighlightOnUnit(unit, 0);
			}
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x0003DADA File Offset: 0x0003BCDA
		private void SetHighlightOnUnit(Unit unit, int color = 0)
		{
			UnitPresenter unitPresenter = GameController.GetUnitPresenter(unit);
			if (!this.unitsToWithdraw.Contains(unit))
			{
				this.unitsToWithdraw.Add(unit);
			}
			unitPresenter.SetFocus(true, color);
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x000CB5D4 File Offset: 0x000C97D4
		private void ShowWithdrawPositions()
		{
			bool flag = GameController.GameManager.combatManager.AreDefeatedMechsOnBattlefield();
			bool flag2 = true;
			foreach (GameHex gameHex in this.withdrawPositions)
			{
				Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = GameController.Instance.GetGameHexPresenter(gameHex);
				gameHexPresenter.SetFocus(false, HexMarkers.MarkerType.FieldSelected, 0f, false);
				GameController.Instance.gameBoardPresenter.GetGameHexPresenter(gameHex).SetFocus(false, HexMarkers.MarkerType.Move, 0.5f, false);
				if (gameHex.hexType != HexType.capital)
				{
					flag2 = false;
				}
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
				if (flag2)
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

		// Token: 0x060021F7 RID: 8695 RVA: 0x0003DB03 File Offset: 0x0003BD03
		public Unit GetSelectedUnit()
		{
			return this.selectedUnit.UnitLogic;
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public bool CursorRaycastBlocked()
		{
			return false;
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x000CB78C File Offset: 0x000C998C
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

		// Token: 0x060021FA RID: 8698 RVA: 0x000CB814 File Offset: 0x000C9A14
		public void CursorHitHex()
		{
			if (!this.exchangePanel.GetComponent<ExchangePanelPresenter>().DragAndDropBar.PreviousUnitEqualsUnit(this.selectedUnit.UnitLogic))
			{
				this.exchangePanel.GetComponent<ExchangePanelPresenter>().ClearPreviousUnit(true);
				this.exchangePanel.GetComponent<ExchangePanelPresenter>().DragAndDropBar.OnUnitChanged(this.selectedUnit.UnitLogic, SelectMethod.Default);
			}
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x0003DB10 File Offset: 0x0003BD10
		public void CursorNoHit()
		{
			this.HookController.Detach(this.selectedUnit.hex.GetUnitPosition(this.selectedUnit.UnitLogic), false);
			GameController.UnitGetFocused += this.OnUnitSelected;
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x0003DB4A File Offset: 0x0003BD4A
		public ExchangePanelPresenter GetExchangePanel()
		{
			return this.exchangePanel.GetComponent<ExchangePanelPresenter>();
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x000CB878 File Offset: 0x000C9A78
		public bool UnitUnderTheCursorIsCorrect(UnitPresenter unitUnderTheCursor)
		{
			Unit unit = this.GetSelectedUnit();
			return unitUnderTheCursor.UnitLogic.UnitType == UnitType.Mech && unitUnderTheCursor.UnitLogic.position == unit.position && unitUnderTheCursor.UnitLogic.Owner == unit.Owner;
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x000CB8C4 File Offset: 0x000C9AC4
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

		// Token: 0x060021FF RID: 8703 RVA: 0x0003DB57 File Offset: 0x0003BD57
		private void OnUnitStateChanged(UnitState unitState, UnitPresenter unitPresenter)
		{
			if (this.selectedUnit == unitPresenter && unitState == UnitState.Standing)
			{
				UnitPresenter.UnitStatusChanged -= this.OnUnitStateChanged;
				this.exchangePanel.GetComponent<ExchangePanelPresenter>().ClearPreviousUnit(true);
				this.selectedUnit = null;
			}
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x0003DB93 File Offset: 0x0003BD93
		public void OnConnectionBroken()
		{
			GameController.UnitGetFocused += this.OnUnitSelected;
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x000CBA3C File Offset: 0x000C9C3C
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
					this.exchangePanel.SetActive(true);
					this.exchangePanel.GetComponent<ExchangePanelPresenter>().SetPanelParameters(unit.UnitLogic, true, SelectMethod.Default);
				}
				if (unit.UnitLogic.Owner.matFaction.faction == GameController.GameManager.combatManager.GetAttacker().matFaction.faction && unit.UnitLogic.position.GetEnemyWorkers().Count > 0)
				{
					this.exchangePanel.SetActive(true);
					this.exchangePanel.GetComponent<ExchangePanelPresenter>().SetPanelParameters(unit.UnitLogic, true, SelectMethod.Default);
				}
			}
			else
			{
				this.exchangePanel.SetActive(false);
			}
			this.ShowWithdrawPositions();
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x0003DBA6 File Offset: 0x0003BDA6
		public void OnEndSeaworthyClicked()
		{
			this.ShowSelectionPanel(ScriptLocalization.Get("FactionMat/NordicMechAbilityTitle2"), ScriptLocalization.Get("GameScene/SeaworthyEndConfirmMessage"), GameController.factionInfo[Faction.Nordic].logo, delegate(bool choice)
			{
				if (choice)
				{
					this.RetreatRemainingUnitsToBase();
					this.HideSelectionPanel();
					return;
				}
				this.HideSelectionPanel();
			});
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x000CBBD0 File Offset: 0x000C9DD0
		private void RetreatRemainingUnitsToBase()
		{
			if (this.selectedUnit != null)
			{
				if (this.selectedUnit.UnitLogic is Mech)
				{
					this.exchangePanel.GetComponent<ExchangePanelPresenter>().UnloadAllWorkersFromMech();
					this.exchangePanel.SetActive(false);
				}
				this.selectedUnit.SetFocus(true, 0);
			}
			this.selectedUnit = null;
			Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = GameController.Instance.GetGameHexPresenter(this.withdrawPositions[0]);
			this.OnWithdrawPositionSelected(gameHexPresenter);
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000CBC4C File Offset: 0x000C9E4C
		private void OnWithdrawPositionSelected(Scythe.BoardPresenter.GameHexPresenter retreatPosition)
		{
			if (this.turnInfoPanelVisible && !PlatformManager.IsMobile)
			{
				return;
			}
			if (!this.withdrawPositions.Contains(retreatPosition.GetGameHexLogic()))
			{
				if (this.selectedUnit != null && GameController.Instance.DragAndDrop)
				{
					this.HookController.Detach(this.selectedUnit.hex.GetUnitPosition(this.selectedUnit.UnitLogic), false);
				}
				return;
			}
			GameHex retreatHex = retreatPosition.GetGameHexLogic();
			CombatManager combatManager = GameController.GameManager.combatManager;
			bool flag = combatManager.AreDefeatedMechsOnBattlefield();
			bool flag2 = combatManager.IsDefeatedCharacterOnBattlefield();
			if (this.selectedUnit == null)
			{
				this.hookUsed = false;
				if (retreatHex.hexType == HexType.capital || (flag && this.workersOnBattlefield) || ((flag || flag2) && !this.workersOnBattlefield))
				{
					combatManager.WithdrawToPosition(this.unitsToWithdraw, retreatHex);
					this.RefreshRetreatUnits();
					return;
				}
			}
			else
			{
				if (!combatManager.GetUnitsToWithdraw().Contains(this.selectedUnit.UnitLogic))
				{
					return;
				}
				UnitPresenter.UnitStatusChanged += this.OnUnitStateChanged;
				if (this.selectedUnit.UnitLogic is Worker && retreatHex.hexType != HexType.capital)
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
				List<Unit> units = new List<Unit>();
				units.Add(this.selectedUnit.UnitLogic);
				if (this.selectedUnit.UnitLogic is Mech && this.exchangePanel.GetComponent<ExchangePanelPresenter>().GetContext().LoadedWorkers.Count != 0)
				{
					combatManager.LoadWorkers(this.exchangePanel.GetComponent<ExchangePanelPresenter>().GetContext().LoadedWorkers, this.selectedUnit.UnitLogic as Mech);
					units.AddRange(this.exchangePanel.GetComponent<ExchangePanelPresenter>().GetContext().LoadedWorkers);
				}
				if (this.selectedUnit.UnitLogic is Mech)
				{
					if (combatManager.GetUnitsToWithdraw().Count((Unit unit) => unit is Mech) == 1)
					{
						if (combatManager.GetUnitsToWithdraw().Count((Unit unit) => unit is Worker) > units.Count - 1)
						{
							int num = combatManager.GetUnitsToWithdraw().Count((Unit unit) => unit is Worker) - (units.Count - 1);
							this.ShowSelectionPanel(ScriptLocalization.Get("FactionMat/NordicMechAbilityTitle2"), string.Format(ScriptLocalization.Get("GameScene/SeaworthyWorkersLeftMessage"), num), GameController.factionInfo[Faction.Nordic].logo, delegate(bool choice)
							{
								if (choice)
								{
									combatManager.WithdrawToPosition(units, retreatHex);
									this.selectedUnit.SetFocus(false, 0);
									this.RefreshRetreatUnits();
								}
								else
								{
									this.OnUnitSelected(this.selectedUnit);
								}
								this.HideSelectionPanel();
							});
							return;
						}
					}
				}
				combatManager.WithdrawToPosition(units, retreatHex);
				this.selectedUnit.SetFocus(false, 0);
				this.RefreshRetreatUnits();
			}
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x000CBFF8 File Offset: 0x000CA1F8
		private void RefreshRetreatUnits()
		{
			this.exchangePanel.SetActive(false);
			this.unitsToWithdraw = GameController.GameManager.combatManager.GetUnitsToWithdraw();
			if (this.unitsToWithdraw.Count > 0)
			{
				if (!this.unitsToWithdraw.Any((Unit unit) => !(unit is Worker)))
				{
					this.RetreatRemainingUnitsToBase();
					return;
				}
			}
			if (this.unitsToWithdraw.Count == 0)
			{
				this.DisableNordicAbility();
				return;
			}
			this.ShowWithdrawPositions();
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x000B56B4 File Offset: 0x000B38B4
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

		// Token: 0x06002207 RID: 8711 RVA: 0x000CC084 File Offset: 0x000CA284
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

		// Token: 0x06002208 RID: 8712 RVA: 0x000CC18C File Offset: 0x000CA38C
		private void DisableNordicAbility()
		{
			this.nordicRetreatIsOn = false;
			this.HookController.FinishWork();
			this.nordicAbilityEnabled = false;
			Player attacker = GameController.GameManager.combatManager.GetAttacker();
			if (!GameController.GameManager.IsMultiplayer && attacker.IsHuman)
			{
				this.turnInfoPanelCombat.GetComponent<TurnInfoPanel>().ActivateTurnInfoPanelCombat(attacker);
				GameController.Instance.undoController.TriggerUndoInteractivityChange(true);
				if (!PlatformManager.IsStandalone)
				{
					SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Visible, 0.25f);
				}
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

		// Token: 0x06002209 RID: 8713 RVA: 0x0003DBDE File Offset: 0x0003BDDE
		public void SetPreviousPlayer(Player previousPlayer)
		{
			this.previousPlayer = previousPlayer;
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x0003DBE7 File Offset: 0x0003BDE7
		public Player GetPreviousPlayer()
		{
			return this.previousPlayer;
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x0003DBEF File Offset: 0x0003BDEF
		public Scythe.BoardPresenter.GameHexPresenter GetSelectedHex()
		{
			return this.selectedHex;
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x0003DBF7 File Offset: 0x0003BDF7
		public void SetLastUsedAbility(AbilityPerk ability)
		{
			this.abilityUsed = true;
			this.combatAbility = ability;
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x0003DC07 File Offset: 0x0003BE07
		private void ShowSelectionPanel(string title, string description, Sprite factionLogo, Action<bool> callback)
		{
			this.selectionPanelTitleText.text = title;
			this.selectionPanelDescriptionText.text = description;
			this.selectionPanelImage.sprite = factionLogo;
			this.selectionPanelCallback = callback;
			this.selectionPanel.SetActive(true);
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x0003DC41 File Offset: 0x0003BE41
		private void HideSelectionPanel()
		{
			this.selectionPanelCallback = null;
			this.selectionPanel.SetActive(false);
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x0003DC56 File Offset: 0x0003BE56
		private void TurnInfoPanelClosed()
		{
			this.turnInfoPanelVisible = false;
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x000CC3AC File Offset: 0x000CA5AC
		private void CreateCombatButtons(bool allBattlefields = true, GameHex selectedBattlefield = null)
		{
			if (allBattlefields)
			{
				List<GameHex> list = GameController.GameManager.combatManager.GetBattlefields();
				for (int i = 0; i < list.Count; i++)
				{
					if (!this.combatHexButtonsBattlefieldDictionary.ContainsKey(list[i]))
					{
						Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = GameController.Instance.gameBoardPresenter.GetGameHexPresenter(list[i]);
						gameHexPresenter.SetFocus(true, HexMarkers.MarkerType.Battle, 0f, false);
						this.battlefieldLayer.AddHex(gameHexPresenter);
						this.combatHexButtonsBattlefieldDictionary.Add(list[i], this.combatHexButtons[i]);
						int num = 0;
						if (list[i].HasEnemyCharacter())
						{
							num++;
						}
						if (list[i].HasOwnerCharacter())
						{
							num++;
						}
						this.combatHexButtons[i].AttachCombatPanel(this, gameHexPresenter, num);
						this.combatHexButtons[i].CombatButtonClicked += this.OnBattlefieldSelected;
						this.combatHexButtons[i].gameObject.SetActive(true);
					}
				}
				return;
			}
			if (!allBattlefields && selectedBattlefield != null)
			{
				Scythe.BoardPresenter.GameHexPresenter gameHexPresenter2 = GameController.Instance.gameBoardPresenter.GetGameHexPresenter(selectedBattlefield);
				gameHexPresenter2.SetFocus(true, HexMarkers.MarkerType.Battle, 0f, false);
				this.battlefieldLayer.AddHex(gameHexPresenter2);
				this.combatHexButtonsBattlefieldDictionary.Add(selectedBattlefield, this.combatHexButtons[0]);
				int num2 = 0;
				if (selectedBattlefield.HasEnemyCharacter())
				{
					num2++;
				}
				if (selectedBattlefield.HasOwnerCharacter())
				{
					num2++;
				}
				this.combatHexButtons[0].AttachCombatPanel(this, gameHexPresenter2, num2);
				this.combatHexButtons[0].CombatButtonClicked += this.OnBattleResumed;
				this.combatHexButtons[0].ResumeMode = true;
				this.combatHexButtons[0].gameObject.SetActive(true);
			}
		}

		// Token: 0x0400178E RID: 6030
		public GameObject exchangePanel;

		// Token: 0x0400178F RID: 6031
		public GameObject diversionPanel;

		// Token: 0x04001790 RID: 6032
		public GameObject preperationPanel;

		// Token: 0x04001791 RID: 6033
		public GameObject resultPanel;

		// Token: 0x04001792 RID: 6034
		public GameObject selectionPanel;

		// Token: 0x04001793 RID: 6035
		public TextMeshProUGUI selectionPanelTitleText;

		// Token: 0x04001794 RID: 6036
		public TextMeshProUGUI selectionPanelDescriptionText;

		// Token: 0x04001795 RID: 6037
		public Image selectionPanelImage;

		// Token: 0x04001796 RID: 6038
		public CombatAbilityInfo abilityInformation;

		// Token: 0x04001797 RID: 6039
		public GameObject nordicRetreatInfo;

		// Token: 0x04001798 RID: 6040
		public GameObject turnInfoPanelCombat;

		// Token: 0x04001799 RID: 6041
		public CombatButtonsPanelMobile combatButtonsPanel;

		// Token: 0x0400179A RID: 6042
		public GameObject combatButtonsParent;

		// Token: 0x0400179B RID: 6043
		public TextMeshProUGUI nordicRetreatInfoText;

		// Token: 0x0400179C RID: 6044
		public GameObject[] hotseatOpponentHideUI;

		// Token: 0x0400179D RID: 6045
		public PlayerStatsPresenter statsRegular;

		// Token: 0x0400179E RID: 6046
		public Image mapDarken;

		// Token: 0x0400179F RID: 6047
		public BattlefieldsLayer battlefieldLayer;

		// Token: 0x040017A0 RID: 6048
		public bool skipPreparation;

		// Token: 0x040017A1 RID: 6049
		public bool nordicAbilityEnabled;

		// Token: 0x040017A2 RID: 6050
		public CombatEnemyActionInfo combatEnemyActionInfo;

		// Token: 0x040017A3 RID: 6051
		private bool selectingBonusCard;

		// Token: 0x040017A4 RID: 6052
		private Scythe.BoardPresenter.GameHexPresenter selectedHex;

		// Token: 0x040017A5 RID: 6053
		private UnitPresenter selectedUnit;

		// Token: 0x040017A6 RID: 6054
		private List<GameHex> withdrawPositions;

		// Token: 0x040017A7 RID: 6055
		private List<Unit> unitsToWithdraw;

		// Token: 0x040017A8 RID: 6056
		private bool workersOnBattlefield;

		// Token: 0x040017A9 RID: 6057
		private Player previousPlayer;

		// Token: 0x040017AA RID: 6058
		private GameHex selectedBattlefield;

		// Token: 0x040017AB RID: 6059
		private Queue<BattleResult> battleResultQueue = new Queue<BattleResult>();

		// Token: 0x040017AC RID: 6060
		private bool abilityUsed;

		// Token: 0x040017AD RID: 6061
		private AbilityPerk combatAbility;

		// Token: 0x040017AE RID: 6062
		private CombatStage previousCombatStage = CombatStage.CombatResovled;

		// Token: 0x040017AF RID: 6063
		private List<GameHex> battlefields = new List<GameHex>();

		// Token: 0x040017B0 RID: 6064
		private List<CombatButtonsPanelMobile> combatHexButtons = new List<CombatButtonsPanelMobile>();

		// Token: 0x040017B1 RID: 6065
		private Dictionary<GameHex, CombatButtonsPanelMobile> combatHexButtonsBattlefieldDictionary = new Dictionary<GameHex, CombatButtonsPanelMobile>();

		// Token: 0x040017B2 RID: 6066
		private bool hookUsed;

		// Token: 0x040017B3 RID: 6067
		private bool nordicRetreatIsOn;

		// Token: 0x040017B4 RID: 6068
		private Action<bool> selectionPanelCallback = delegate
		{
		};

		// Token: 0x040017B5 RID: 6069
		private bool turnInfoPanelVisible;

		// Token: 0x02000444 RID: 1092
		// (Invoke) Token: 0x06002214 RID: 8724
		public delegate void BattleEnd(bool attackerIsWinner, Player Attacker, Player Defender);

		// Token: 0x02000445 RID: 1093
		// (Invoke) Token: 0x06002218 RID: 8728
		public delegate void BattlefieldSelect();
	}
}

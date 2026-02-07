using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HoneyFramework;
using I2.Loc;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003F0 RID: 1008
	public class MovePresenter : ActionPresenter, IHooksUnitControllerUser, IHooksResourceControllerUser
	{
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06001E3D RID: 7741 RVA: 0x0003AD4A File Offset: 0x00038F4A
		private HookController HookController
		{
			get
			{
				return GameController.Instance.hookController;
			}
		}

		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x06001E3E RID: 7742 RVA: 0x000B9F08 File Offset: 0x000B8108
		// (remove) Token: 0x06001E3F RID: 7743 RVA: 0x000B9F3C File Offset: 0x000B813C
		public static event MovePresenter.MoveEnd MoveEnded;

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06001E40 RID: 7744 RVA: 0x000283FB File Offset: 0x000265FB
		private GameManager gameManager
		{
			get
			{
				return GameController.GameManager;
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0003B9AC File Offset: 0x00039BAC
		private void OnDisable()
		{
			this.DisconnectDelegates();
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x000B9F70 File Offset: 0x000B8170
		public override void ChangeLayoutForAction(BaseAction action)
		{
			this.SetAction(action as GainMove);
			UnitPresenter.UnitStatusChanged -= this.ChangePresenterInputState;
			GameController.ClearFocus();
			this.unitsHighlight = new HashSet<UnitPresenter>();
			this.possibleMoves = new Dictionary<GameHex, GameHex>();
			this.hexesWithUnits = new HashSet<Scythe.BoardPresenter.GameHexPresenter>();
			this.exchangePanel.DragAndDropBar.AttachExchangePanel(this.exchangePanel);
			GameController.factionUnits[GameController.GameManager.PlayerCurrent.matFaction.faction].SetColliders(true);
			this.gainStuffAnimation.SetActive(false);
			this.lastClickedUnit = null;
			if (!PlatformManager.IsStandalone)
			{
				for (int i = 0; i < this.mobileInfoTiles.Length; i++)
				{
					this.mobileInfoTiles[i].SetActive(i < (int)this.action.Amount);
					this.mobileInfoCheckmarks[i].SetActive(false);
				}
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
				GameController.Instance.matFaction.ClearHintStories();
				this.unitSelectInfo.SetActive(true);
			}
			this.EnableInput();
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000BA084 File Offset: 0x000B8284
		public void ClearLastClickedUnit()
		{
			if (this.lastClickedUnit != null)
			{
				UnitPresenter unitPresenter = GameController.GetUnitPresenter(this.lastClickedUnit);
				this.exchangePanel.ResetLoadedWorkersInPanel();
				this.exchangePanel.UpdateWorkersButtons();
				if (unitPresenter != null)
				{
					unitPresenter.ForceToHideMovesHighlight();
				}
			}
			this.lastClickedUnit = null;
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000BA0D4 File Offset: 0x000B82D4
		private void EnableInput()
		{
			GameController.HexSelectionMode = GameController.SelectionMode.MoveAction;
			this.ShowUnitsWithMove();
			if (PlatformManager.IsStandalone)
			{
				this.endMoveButton.gameObject.SetActive(true);
			}
			GameController.GameManager.combatManager.OnCombatStageChanged += this.OnCombatStateChanged;
			GameController.UnitGetFocused += this.OnUnitClicked;
			if (!GameController.Instance.DragAndDrop)
			{
				GameController.HexGetFocused += this.OnGameHexSelected;
				this.HookController.SetResourceFocusDetectorEnabled(false);
			}
			else
			{
				this.HookController.StartWorkWithUser(this);
				this.HookController.StartWorkWithUnits(this);
				this.HookController.StartWorkWithResources(this);
				this.HookController.SetResourceFocusDetectorEnabled(true);
			}
			ExchangePanelPresenter.OnWorkerButtonClicked += this.UnloadWorker;
			UnitPresenter.UnitGetLoaded += this.OnUnitLoaded;
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000BA1B0 File Offset: 0x000B83B0
		private void Update()
		{
			if (this.teleportLine.widthMultiplier > 0f)
			{
				this.teleportLine.widthMultiplier -= Time.deltaTime / 4f;
				if (this.teleportLine.widthMultiplier < 0f)
				{
					this.teleportLine.widthMultiplier = 0f;
				}
			}
			if (Input.anyKeyDown || (Input.touchCount > 0 && !this.unattendedResources.gameObject.activeSelf))
			{
				this.FinishMoveOnClick();
			}
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x000BA238 File Offset: 0x000B8438
		private void ShowUnitsWithMove()
		{
			this.hexesWithUnits.Clear();
			foreach (Unit unit in GameController.GameManager.moveManager.GetUnitsWithMove())
			{
				GameController.GetUnitPresenter(unit).ForceToHideMovesHighlight();
				this.SetHighlightOnUnit(unit, 0);
			}
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x000BA2AC File Offset: 0x000B84AC
		private void HideUnitsWithMove()
		{
			if (this.unitsHighlight == null)
			{
				return;
			}
			foreach (UnitPresenter unitPresenter in this.unitsHighlight)
			{
				unitPresenter.SetFocus(false, 0);
			}
			this.unitsHighlight.Clear();
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x000BA314 File Offset: 0x000B8514
		public void ShowUnitsOutline()
		{
			if (this.unitsHighlight == null)
			{
				return;
			}
			foreach (UnitPresenter unitPresenter in this.unitsHighlight)
			{
				if (unitPresenter.UnitLogic == GameController.GameManager.moveManager.GetLastMovedUnit() && unitPresenter.UnitLogic != this.lastClickedUnit)
				{
					this.SetHighlightOnUnit(unitPresenter.UnitLogic, 2);
				}
				else if (this.action.MovesLeft >= 1)
				{
					if (unitPresenter.UnitLogic == this.lastClickedUnit)
					{
						this.SetHighlightOnUnit(unitPresenter.UnitLogic, 1);
					}
					else
					{
						this.SetHighlightOnUnit(unitPresenter.UnitLogic, 0);
					}
				}
			}
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000BA3D8 File Offset: 0x000B85D8
		public void HideUnitsOutline()
		{
			if (this.unitsHighlight == null)
			{
				return;
			}
			foreach (UnitPresenter unitPresenter in this.unitsHighlight)
			{
				unitPresenter.SetFocus(false, 0);
			}
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000BA434 File Offset: 0x000B8634
		private void SetHighlightOnUnit(Unit unit, int color = 0)
		{
			UnitPresenter unitPresenter = GameController.GetUnitPresenter(unit);
			if (!this.unitsHighlight.Contains(unitPresenter) && GameController.GameManager.moveManager.CanUnitMove(unit))
			{
				this.unitsHighlight.Add(unitPresenter);
			}
			if (GameController.GameManager.moveManager.CanUnitMove(unit))
			{
				unitPresenter.SetFocus(true, color);
			}
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x000BA490 File Offset: 0x000B8690
		private void TurnOffHighlightExpectUnit(Unit unit)
		{
			foreach (UnitPresenter unitPresenter in this.unitsHighlight)
			{
				if (unitPresenter.UnitLogic != unit)
				{
					unitPresenter.SetFocus(false, 0);
				}
			}
			this.unitsHighlight.Clear();
			this.unitsHighlight.Add(GameController.GetUnitPresenter(unit));
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x0003B9B4 File Offset: 0x00039BB4
		private void TurnOffHighlightOnUnit(Unit unit)
		{
			if (unit == null)
			{
				return;
			}
			GameController.GetUnitPresenter(unit).SetFocus(false, 0);
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x0002BA43 File Offset: 0x00029C43
		private Scythe.BoardPresenter.GameHexPresenter GetGameHexPresenter(int posX, int posY)
		{
			return GameController.Instance.gameBoardPresenter.GetGameHexPresenter(posX, posY);
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x000BA50C File Offset: 0x000B870C
		public void ShowPossibleMoves(bool forced = false)
		{
			switch (GameController.GameManager.moveManager.GetSelectedUnit().UnitType)
			{
			case UnitType.Character:
				WorldSFXManager.PlaySound(SoundEnum.ClickMainCharracter, AudioSourceType.WorldSfx);
				break;
			case UnitType.Mech:
				WorldSFXManager.PlaySound(SoundEnum.ClickMech, AudioSourceType.WorldSfx);
				break;
			case UnitType.Worker:
				WorldSFXManager.PlaySound(SoundEnum.ClickWorker, AudioSourceType.WorldSfx);
				break;
			}
			this.ShowPossibleMoves(GameController.GameManager.moveManager.GetSelectedUnit(), true, forced);
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x000BA578 File Offset: 0x000B8778
		public void ShowPossibleMoves(Unit selectedUnit, bool isMoveAction, bool forced = false)
		{
			if (selectedUnit == null)
			{
				return;
			}
			if (this.lastClickedUnit != null)
			{
				if (isMoveAction)
				{
					this.HidePossibleMoves(isMoveAction, selectedUnit, true);
				}
				else
				{
					UnitPresenter unitPresenter = GameController.GetUnitPresenter(this.lastClickedUnit);
					if (unitPresenter != null)
					{
						unitPresenter.ForceToHideMovesHighlight();
					}
				}
			}
			this.lastClickedUnit = selectedUnit;
			this.possibleMoves = GameController.GameManager.moveManager.GetPossibleMovesForSelectedUnit(selectedUnit);
			GameController.GameManager.gameBoard.MoveRange(selectedUnit, (int)selectedUnit.MovesLeft, out this.movesForDistanceCheck);
			this.firstAnimationHexAmount = 0;
			this.secondAnimationHexAmount = 0;
			this.thirdAnimationHexAmount = 0;
			this.spawnOrder = 1;
			this.iterations = 0;
			this.CalculatEachHexTypeAmount(selectedUnit);
			this.ShowMarkers(selectedUnit, this.spawnOrder, isMoveAction, forced);
			if (this.secondAnimationHexAmount != 0)
			{
				this.spawnOrder++;
				if (forced)
				{
					this.ShowMarkers(selectedUnit, this.spawnOrder, isMoveAction, forced);
				}
				else
				{
					this.secondMoveRangeHighlight = base.StartCoroutine(this.ActivateShowMarkers(selectedUnit, this.spawnOrder, isMoveAction));
				}
			}
			if (this.thirdAnimationHexAmount != 0)
			{
				this.spawnOrder++;
				if (forced)
				{
					this.ShowMarkers(selectedUnit, this.spawnOrder, isMoveAction, forced);
					return;
				}
				this.thirdMoveRangeHighlight = base.StartCoroutine(this.ActivateShowMarkers(selectedUnit, this.spawnOrder, isMoveAction));
			}
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x0003B9C7 File Offset: 0x00039BC7
		private IEnumerator ActivateShowMarkers(Unit selectedUnit, int markersRange, bool isMoveAction)
		{
			yield return new WaitForSeconds(this.rangeMarkersAnimationOffset * (float)(markersRange - 1));
			this.ShowMarkers(selectedUnit, markersRange, isMoveAction, false);
			yield break;
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x000BA6B4 File Offset: 0x000B88B4
		private void ShowMarkers(Unit selectedUnit, int markersRange, bool isMoveAction, bool forced = false)
		{
			float num = (forced ? 0f : this.fadeInHexTime);
			if (Time.timeScale > 1f)
			{
				num *= ShowEnemyMoves.Instance.FastForwardSpeed;
			}
			bool flag = selectedUnit != null && selectedUnit.UnitType == UnitType.Character;
			this.possibleMoves = GameController.GameManager.moveManager.GetPossibleMovesForSelectedUnit(selectedUnit);
			foreach (GameHex gameHex in this.possibleMoves.Keys)
			{
				if (this.possibleMoves[gameHex] != null && (selectedUnit.position.posX != gameHex.posX || selectedUnit.position.posY != gameHex.posY))
				{
					if (gameHex.Conflict(selectedUnit) || (gameHex.hasEncounter && !gameHex.encounterUsed && selectedUnit.UnitType == UnitType.Character) || (gameHex.Token is TrapToken && selectedUnit.Owner != gameHex.Token.Owner && (gameHex.Token as TrapToken).Armed))
					{
						if (this.movesForDistanceCheck[gameHex] == markersRange)
						{
							this.GetGameHexPresenter(gameHex.posX, gameHex.posY).SetFocus(true, HexMarkers.MarkerType.MoveToEnemy, num, flag);
							this.iterations++;
						}
					}
					else if (isMoveAction)
					{
						if (this.movesForDistanceCheck[gameHex] == markersRange)
						{
							this.GetGameHexPresenter(gameHex.posX, gameHex.posY).SetFocus(true, HexMarkers.MarkerType.Move, num, flag);
							this.iterations++;
						}
					}
					else if (this.movesForDistanceCheck[gameHex] == markersRange)
					{
						this.GetGameHexPresenter(gameHex.posX, gameHex.posY).SetFocus(true, HexMarkers.MarkerType.DistanceRange, num, flag);
						this.iterations++;
					}
				}
			}
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x000BA8A8 File Offset: 0x000B8AA8
		private void CalculatEachHexTypeAmount(Unit selectedUnit)
		{
			this.possibleMoves = GameController.GameManager.moveManager.GetPossibleMovesForSelectedUnit(selectedUnit);
			foreach (GameHex gameHex in this.possibleMoves.Keys)
			{
				if (this.possibleMoves[gameHex] != null && (selectedUnit.position.posX != gameHex.posX || selectedUnit.position.posY != gameHex.posY))
				{
					if (this.movesForDistanceCheck[gameHex] == 1)
					{
						this.firstAnimationHexAmount++;
					}
					if (this.movesForDistanceCheck[gameHex] == 2)
					{
						this.secondAnimationHexAmount++;
					}
					if (this.movesForDistanceCheck[gameHex] == 3)
					{
						this.thirdAnimationHexAmount++;
					}
				}
			}
			this.allMarkers = this.firstAnimationHexAmount + this.secondAnimationHexAmount + this.thirdAnimationHexAmount;
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x0003B9EB File Offset: 0x00039BEB
		public void HidePossibleMoves()
		{
			this.HidePossibleMoves(true, null, true);
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x000BA9BC File Offset: 0x000B8BBC
		private void HidePossibleMoves(bool isMoveAction, Unit unit = null, bool force = false)
		{
			if (this.secondMoveRangeHighlight != null)
			{
				base.StopCoroutine(this.secondMoveRangeHighlight);
			}
			if (this.thirdMoveRangeHighlight != null)
			{
				base.StopCoroutine(this.thirdMoveRangeHighlight);
			}
			if (unit != null)
			{
				this.possibleMoves = GameController.GameManager.moveManager.GetPossibleMovesForSelectedUnit(unit);
			}
			float num = (force ? 0f : 0.3f);
			bool flag = unit != null && unit.UnitType == UnitType.Character;
			if (this.possibleMoves != null || this.hexesWithUnits != null)
			{
				foreach (GameHex gameHex in this.possibleMoves.Keys)
				{
					if (this.possibleMoves[gameHex] != null)
					{
						if (isMoveAction && (GameController.GameManager.PlayerCurrent != gameHex.Owner || (gameHex.Token != null && gameHex.Token is TrapToken && (gameHex.Token as TrapToken).Armed)))
						{
							this.GetGameHexPresenter(gameHex.posX, gameHex.posY).SetFocus(false, HexMarkers.MarkerType.MoveToEnemy, num, flag);
						}
						else if (!isMoveAction && ((gameHex.Enemy == null && gameHex.Owner != null && unit.Owner != gameHex.Owner) || (gameHex.hasEncounter && !gameHex.encounterUsed && unit.UnitType == UnitType.Character) || (gameHex.Token != null && gameHex.Token is TrapToken && unit.Owner != gameHex.Token.Owner && (gameHex.Token as TrapToken).Armed)))
						{
							this.GetGameHexPresenter(gameHex.posX, gameHex.posY).SetFocus(false, HexMarkers.MarkerType.MoveToEnemy, num, flag);
						}
						if (isMoveAction)
						{
							this.GetGameHexPresenter(gameHex.posX, gameHex.posY).SetFocus(false, HexMarkers.MarkerType.Move, num, flag);
						}
						else
						{
							this.GetGameHexPresenter(gameHex.posX, gameHex.posY).SetFocus(false, HexMarkers.MarkerType.DistanceRange, num, flag);
						}
					}
				}
			}
			this.possibleMoves = new Dictionary<GameHex, GameHex>();
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x0003B9F6 File Offset: 0x00039BF6
		public void HideMoveHexesHighlight(Unit unit, bool force = false)
		{
			this.HidePossibleMoves(false, unit, force);
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x0003BA01 File Offset: 0x00039C01
		public bool IsMovePossible(GameHex hex)
		{
			return this.possibleMoves != null && hex != null && this.possibleMoves.ContainsKey(hex) && this.possibleMoves[hex] != null;
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x000BABD8 File Offset: 0x000B8DD8
		public void ShowPath(GameHex hex)
		{
			if (GameController.GameManager.moveManager.DoesUnitCannotMoveBecauseOfTheLakeCaseClick() && GameController.GameManager.moveManager.GetSelectedUnit() is Worker)
			{
				return;
			}
			if (hex == null || (GameController.Instance.DragAndDrop && this.HookController.IsResourceDragged()))
			{
				this.pathLine.enabled = false;
				return;
			}
			List<Vector3> list = new List<Vector3>();
			Unit selectedUnit = GameController.GameManager.moveManager.GetSelectedUnit();
			while (hex != null)
			{
				if (selectedUnit != null && hex == selectedUnit.position)
				{
					if (GameController.Instance.DragAndDrop)
					{
						Vector3 unitPosition = GameController.Instance.GetGameHexPresenter(hex).GetUnitPosition(selectedUnit);
						unitPosition.y = 0f;
						list.Add(unitPosition);
					}
					else
					{
						list.Add(GameController.GetUnitPresenter(selectedUnit).transform.position + 0.6f * Vector3.up);
					}
				}
				else
				{
					list.Add(HexCoordinates.HexToWorld3D(this.GetGameHexPresenter(hex.posX, hex.posY).position) + 0.6f * Vector3.up);
				}
				hex = this.possibleMoves[hex];
			}
			if (!GameController.Instance.DragAndDrop || this.HookController.IsUnitDragged())
			{
				this.pathLine.enabled = true;
				this.pathLine.positionCount = list.Count;
				this.pathLine.SetPositions(list.ToArray());
			}
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x0003BA2D File Offset: 0x00039C2D
		private void ChangeCameraMovementState(bool enabled)
		{
			CameraControler.CameraMovementBlocked = !enabled;
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x0003BA38 File Offset: 0x00039C38
		public Unit GetSelectedUnit()
		{
			return GameController.GameManager.moveManager.GetSelectedUnit();
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x0003BA49 File Offset: 0x00039C49
		public bool CursorRaycastBlocked()
		{
			return !GameController.GameManager.moveManager.CanSelectedUnitMove() && this.GetSelectedUnit().UnitType != UnitType.Worker;
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x000BAD50 File Offset: 0x000B8F50
		public void CursorHitUnit(UnitPresenter unitUnderTheCursor)
		{
			Unit selectedUnit = GameController.GameManager.moveManager.GetSelectedUnit();
			UnitPresenter unitPresenter = GameController.GetUnitPresenter(selectedUnit);
			if (selectedUnit.UnitType == UnitType.Worker)
			{
				GameController.HexGetFocused -= this.OnGameHexSelected;
				unitPresenter.ForceFinishMoveAnimation();
				this.SelectUnit(unitUnderTheCursor, SelectMethod.LoadingWorker);
				this.HookController.Detach(unitUnderTheCursor.transform.position, true);
				this.exchangePanel.LoadWorker(selectedUnit);
				GameController.UnitGetFocused += this.OnUnitClicked;
			}
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x0003BA6F File Offset: 0x00039C6F
		public void CursorHitHex()
		{
			if (!this.exchangePanel.DragAndDropBar.PreviousUnitEqualsUnit(this.GetSelectedUnit()))
			{
				this.exchangePanel.ClearPreviousUnit(true);
				this.exchangePanel.DragAndDropBar.OnUnitChanged(this.GetSelectedUnit(), SelectMethod.Default);
			}
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x0003BAAD File Offset: 0x00039CAD
		public void CursorNoHit()
		{
			this.DetachOnFailAndSetUpDelegates();
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x000BADD0 File Offset: 0x000B8FD0
		private void DetachOnFailAndSetUpDelegates()
		{
			if (this.HookController.IsUnitDragged())
			{
				this.HidePossibleMoves(true, null, false);
				this.HookController.Detach(GameController.Instance.GetGameHexPresenter(this.GetSelectedUnit().position).GetUnitPosition(this.GetSelectedUnit()), false);
				GameController.HexGetFocused -= this.OnGameHexSelected;
				GameController.UnitGetFocused += this.OnUnitClicked;
			}
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x0003BAB5 File Offset: 0x00039CB5
		public ExchangePanelPresenter GetExchangePanel()
		{
			return this.exchangePanel;
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000BAE44 File Offset: 0x000B9044
		public bool UnitUnderTheCursorIsCorrect(UnitPresenter unitUnderTheCursor)
		{
			Unit selectedUnit = this.GetSelectedUnit();
			return unitUnderTheCursor.UnitLogic.UnitType == UnitType.Mech && unitUnderTheCursor.UnitLogic.MovesLeft != 0 && unitUnderTheCursor.UnitLogic.position == selectedUnit.position && unitUnderTheCursor.UnitLogic.Owner == selectedUnit.Owner;
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000BAE9C File Offset: 0x000B909C
		public void OnDragAndDropStateChanged(bool enabled)
		{
			if (this.action == null)
			{
				return;
			}
			if (!enabled)
			{
				GameController.HexGetFocused += this.OnGameHexSelected;
				this.HookController.SetResourceFocusDetectorEnabled(false);
				this.HookController.FinishWork();
				if (this.GetSelectedUnit() != null)
				{
					this.ShowPossibleMoves(this.GetSelectedUnit(), true, false);
					return;
				}
			}
			else
			{
				GameController.HexGetFocused -= this.OnGameHexSelected;
				this.HookController.SetResourceFocusDetectorEnabled(true);
				this.HookController.SetMovingActive(true);
				this.HookController.StartWorkWithUser(this);
				this.HookController.StartWorkWithUnits(this);
				this.HookController.StartWorkWithResources(this);
				this.HidePossibleMoves();
			}
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x000BAF48 File Offset: 0x000B9148
		public void OnResourceDropUnitSelected(UnitPresenter unitUnderTheCursor)
		{
			if (unitUnderTheCursor != null && unitUnderTheCursor.UnitLogic.Owner != this.action.GetPlayer())
			{
				return;
			}
			this.unitClickedTemp = true;
			if (GameController.GameManager.moveManager.CanUnitMove(unitUnderTheCursor.UnitLogic) && this.unitsHighlight.Contains(unitUnderTheCursor) && unitUnderTheCursor.UnitLogic != this.lastClickedUnit)
			{
				this.SelectUnit(unitUnderTheCursor, SelectMethod.LoadingResource);
			}
			base.StartCoroutine(this.UnitClickedTempReset());
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x0003BABD File Offset: 0x00039CBD
		public void OnConnectionBroken()
		{
			GameController.Instance.cameraControler.HooverReset();
			this.ShowPath(null);
			GameController.HexGetFocused -= this.OnGameHexSelected;
			GameController.UnitGetFocused += this.OnUnitClicked;
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x0003BAF7 File Offset: 0x00039CF7
		void IHooksResourceControllerUser.CursorHitUnit(ResourcePresenter resourcePresenter, UnitPresenter unitPresenter)
		{
			this.OnResourceDropUnitSelected(unitPresenter);
			this.exchangePanel.PassOneResourceToUnitOnDrop((int)resourcePresenter.resourceType);
			this.HookController.Detach(unitPresenter.gameObject.transform.position, true);
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x0003BB2D File Offset: 0x00039D2D
		bool IHooksResourceControllerUser.UnitUnderTheCursorIsCorrect(UnitPresenter unitUnderTheCursor)
		{
			return unitUnderTheCursor.UnitLogic.MovesLeft != 0 && GameController.GameManager.moveManager.CanUnitMove(unitUnderTheCursor.UnitLogic);
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x000BAFC8 File Offset: 0x000B91C8
		private void FinishMoveOnClick()
		{
			Unit selectedUnit = GameController.GameManager.moveManager.GetSelectedUnit();
			if (selectedUnit != null)
			{
				UnitPresenter unitPresenter = GameController.GetUnitPresenter(selectedUnit);
				if (unitPresenter != null && unitPresenter.actualState != UnitState.Standing)
				{
					unitPresenter.ForceFinishMoveAnimation();
				}
			}
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x0003BB53 File Offset: 0x00039D53
		private void OnUnitClicked(UnitPresenter presenter)
		{
			if (presenter != null && presenter.UnitLogic.Owner != this.action.GetPlayer())
			{
				return;
			}
			this.unitClickedTemp = true;
			this.SelectUnit(presenter, SelectMethod.Default);
			base.StartCoroutine(this.UnitClickedTempReset());
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x0003BB93 File Offset: 0x00039D93
		private IEnumerator UnitClickedTempReset()
		{
			yield return new WaitForEndOfFrame();
			this.unitClickedTemp = false;
			yield break;
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x000BB008 File Offset: 0x000B9208
		private void SelectUnit(UnitPresenter presenter, SelectMethod selectMethod = SelectMethod.Default)
		{
			Unit selectedUnit = GameController.GameManager.moveManager.GetSelectedUnit();
			if (selectedUnit != null && selectedUnit != presenter.UnitLogic)
			{
				this.lakeMoveInfo.transform.parent.gameObject.SetActive(false);
				this.HideExchangePanel();
				this.HidePossibleMoves(true, null, true);
				if (selectedUnit.MovesLeft > 0)
				{
					if (selectedUnit == GameController.GameManager.moveManager.GetLastMovedUnit())
					{
						this.SetHighlightOnUnit(selectedUnit, 2);
					}
					else if (this.action.MovesLeft >= 1)
					{
						this.SetHighlightOnUnit(selectedUnit, 0);
					}
				}
				if (selectMethod != SelectMethod.Default)
				{
					GameController.GameManager.moveManager.UnloadResources();
				}
				if (selectedUnit.UnitType == UnitType.Mech && !GameController.Instance.DragAndDrop)
				{
					GameController.GetUnitPresenter(selectedUnit).UnloadAllWorkers(false);
					this.UpdateWorkerButtons(selectedUnit);
				}
			}
			this.possibleMoves = GameController.GameManager.moveManager.SelectUnit(presenter.UnitLogic);
			bool dragAndDrop = GameController.Instance.DragAndDrop;
			if (GameController.GameManager.moveManager.CanSelectedUnitMove() && presenter.UnitLogic.MovesLeft > 0)
			{
				this.SetHighlightOnUnit(presenter.UnitLogic, 1);
				if (!GameController.GameManager.moveManager.DoesUnitCannotMoveBecauseOfTheLakeCaseClick() || !(GameController.GameManager.moveManager.GetSelectedUnit() is Worker))
				{
					this.ShowExchangePanel(presenter.UnitLogic, selectMethod);
					this.ShowPossibleMoves(false);
				}
				else if (GameController.GameManager.moveManager.GetSelectedUnit() is Worker && GameController.Instance.DragAndDrop)
				{
					this.ShowExchangePanel(presenter.UnitLogic, selectMethod);
				}
				if (selectMethod == SelectMethod.Default && GameController.Instance.DragAndDrop)
				{
					this.PickUpUnit(presenter);
				}
			}
			else if (GameController.Instance.DragAndDrop)
			{
				this.lastClickedUnit = presenter.UnitLogic;
				this.PickUpUnit(presenter);
				this.possibleMoves = GameController.GameManager.moveManager.SelectUnit(presenter.UnitLogic);
			}
			else
			{
				this.possibleMoves = GameController.GameManager.moveManager.SelectUnit(presenter.UnitLogic);
			}
			if (GameController.GameManager.moveManager.DoesUnitCannotMoveBecauseOfTheLakeCaseClick() && GameController.GameManager.moveManager.GetSelectedUnit() is Worker)
			{
				this.ShowInformationAboutLakeCaseWorker();
			}
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x000BB238 File Offset: 0x000B9438
		private void PickUpUnit(UnitPresenter presenter)
		{
			this.HookController.Attach(presenter.gameObject, presenter);
			if (this.exchangePanel.DragAndDropBar.PreviousUnitEqualsUnit(presenter.UnitLogic))
			{
				this.exchangePanel.DragAndDropBar.SetActive(false, true);
			}
			GameController.HexGetFocused += this.OnGameHexSelected;
			GameController.UnitGetFocused -= this.OnUnitClicked;
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x000BB2A4 File Offset: 0x000B94A4
		private void MoveUnit(Unit selectedUnitLogic, Scythe.BoardPresenter.GameHexPresenter selectedHex)
		{
			if (PlatformManager.IsStandalone)
			{
				this.ChangeCameraMovementState(true);
			}
			if (GameController.Instance.DragAndDrop)
			{
				this.HookController.SetMovingActive(true);
				this.HookController.PauseWork(false);
			}
			Unit lastMovedUnit = GameController.GameManager.moveManager.GetLastMovedUnit();
			Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = GameController.Instance.GetGameHexPresenter(selectedUnitLogic.position);
			if (!GameController.GameManager.moveManager.MoveSelectedUnit(selectedHex.GetGameHexLogic(), this.exchangePanel.GetResources(), this.exchangePanel.GetContext().LoadedWorkers))
			{
				if (GameController.GameManager.moveManager.DoesUnitCannotMoveBecauseOfTheLakeCase())
				{
					this.ShowInformationAboutLakeCase();
				}
				if (GameController.Instance.DragAndDrop)
				{
					if (this.HookController.IsUnitDragged())
					{
						GameController.HexGetFocused -= this.OnGameHexSelected;
						GameController.UnitGetFocused += this.OnUnitClicked;
					}
					this.HookController.Detach(GameController.GetUnitPresenter(selectedUnitLogic).hex.GetUnitPosition(selectedUnitLogic), false);
				}
				if (this.unitClickedTemp)
				{
					this.unitClickedTemp = false;
					return;
				}
			}
			else
			{
				if (lastMovedUnit != null && lastMovedUnit != selectedUnitLogic)
				{
					GameController.GetUnitPresenter(lastMovedUnit).OutlineActivation(false, 0, false);
				}
				UnitPresenter.UnitStatusChanged += this.ChangePresenterInputState;
				if (GameController.Instance.DragAndDrop)
				{
					if (!this.exchangePanel.DragAndDropBar.PreviousUnitEqualsUnit(selectedUnitLogic))
					{
						this.exchangePanel.ClearPreviousUnit(true);
					}
					this.HookController.Detach(selectedHex.GetUnitPosition(selectedUnitLogic), false);
				}
				else
				{
					this.FinishLoadedMechAnimations();
					if (this.possibleMoves.Count == 0)
					{
						Debug.LogError("Launching animation with no moves avaliable!");
						return;
					}
					GameController.GetUnitPresenter(selectedUnitLogic).RunTheMoveAnimation(this.possibleMoves, false, false, false, null, null);
				}
				gameHexPresenter.UpdateOwnership();
				selectedHex.UpdateOwnership();
				this.CheckMovesLeftCases(selectedUnitLogic, selectedHex);
				this.UpdateViewsAndStateAfterMove(selectedUnitLogic, selectedHex);
			}
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x000BB474 File Offset: 0x000B9674
		public void FinishLoadedMechAnimations()
		{
			if (this.exchangePanel != null && this.exchangePanel.GetContext().LoadedWorkers != null && this.exchangePanel.GetContext().LoadedWorkers.Count > 0)
			{
				for (int i = 0; i < this.exchangePanel.GetContext().LoadedWorkers.Count; i++)
				{
					try
					{
						GameController.GetUnitPresenter(this.exchangePanel.GetContext().LoadedWorkers[i]).ForceFinishMoveAnimation();
					}
					catch (Exception ex)
					{
						Debug.LogError("Exception caught: " + ex.ToString());
						DOTween.CompleteAll(true);
					}
				}
				return;
			}
			DOTween.CompleteAll(true);
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x000BB538 File Offset: 0x000B9738
		private void OnGameHexSelected(Scythe.BoardPresenter.GameHexPresenter selectedHex)
		{
			Unit selectedUnitLogic = GameController.GameManager.moveManager.GetSelectedUnit();
			if (selectedUnitLogic == null)
			{
				return;
			}
			if (GameController.Instance.DragAndDrop && selectedUnitLogic.MovesLeft == 0)
			{
				this.DetachOnFailAndSetUpDelegates();
				return;
			}
			if (GameController.GameManager.moveManager.DoesUnitCannotMoveBecauseOfTheLakeCaseClick() && GameController.GameManager.moveManager.GetSelectedUnit() is Worker)
			{
				this.DetachOnFailAndSetUpDelegates();
				return;
			}
			if (GameController.Instance.DragAndDrop)
			{
				GameController.HexGetFocused -= this.OnGameHexSelected;
			}
			if (!GameController.GameManager.moveManager.CanSelectedUnitMove() || this.HookController.IsResourceDragged())
			{
				this.DetachOnFailAndSetUpDelegates();
				return;
			}
			if (this.LeavingUnattendedResources(selectedUnitLogic, selectedHex.GetGameHexLogic()) && OptionsManager.IsWarningsActive())
			{
				this.HookController.SetMovingActive(false);
				this.HookController.PauseWork(true);
				if (PlatformManager.IsStandalone)
				{
					this.ChangeCameraMovementState(false);
				}
				WorldSFXManager.PlaySound(SoundEnum.Alert, AudioSourceType.Buttons);
				this.unattendedResources.Show(GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo, delegate
				{
					this.MoveUnit(selectedUnitLogic, selectedHex);
				}, delegate
				{
					this.DoNotLeaveResourcesAlone();
				});
			}
			else
			{
				this.MoveUnit(selectedUnitLogic, selectedHex);
			}
			if (GameController.Instance.DragAndDrop)
			{
				this.ShowPath(null);
				GameController.Instance.cameraControler.HooverReset();
			}
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x000BB6D0 File Offset: 0x000B98D0
		private void DoNotLeaveResourcesAlone()
		{
			if (GameController.Instance.DragAndDrop)
			{
				if (PlatformManager.IsStandalone)
				{
					this.ChangeCameraMovementState(true);
				}
				GameController.HexGetFocused -= this.OnGameHexSelected;
				GameController.UnitGetFocused += this.OnUnitClicked;
				this.HookController.SetMovingActive(true);
				this.HookController.PauseWork(false);
				Unit selectedUnit = GameController.GameManager.moveManager.GetSelectedUnit();
				this.HookController.Detach(GameController.GetUnitPresenter(selectedUnit).hex.GetUnitPosition(selectedUnit), false);
			}
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x000BB760 File Offset: 0x000B9960
		private bool LeavingUnattendedResources(Unit selectedUnit, GameHex selectedHex)
		{
			return !this.exchangePanel.AllResourcesLoaded() && this.possibleMoves.ContainsKey(selectedHex) && selectedHex != selectedUnit.position && (selectedUnit.position.Building == null || selectedUnit.position.Building.player != selectedUnit.Owner) && selectedUnit.position.GetOwnerUnitCount() - this.exchangePanel.GetContext().LoadedWorkers.Count == 1;
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x000BB7DC File Offset: 0x000B99DC
		private void ShowInformationAboutLakeCase()
		{
			this.lakeMoveInfo.transform.parent.gameObject.SetActive(true);
			if (GameController.GameManager.moveManager.GetSelectedUnit() is Mech)
			{
				this.lakeMoveInfo.text = ScriptLocalization.Get("PlayerMat/MechLakeInfo");
				return;
			}
			this.lakeMoveInfo.text = ScriptLocalization.Get("PlayerMat/UnitLakeInfo");
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x0003BBA2 File Offset: 0x00039DA2
		private void ShowInformationAboutLakeCaseWorker()
		{
			this.lakeMoveInfo.transform.parent.gameObject.SetActive(true);
			this.lakeMoveInfo.text = ScriptLocalization.Get("PlayerMat/UnitLakeInfo");
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x000BB848 File Offset: 0x000B9A48
		public void UpdateMoveTile(int moveId)
		{
			if (this.action != null && this.action.GetPlayer() == GameController.GameManager.PlayerCurrent)
			{
				if (GameController.GameManager.PlayerCurrent.topActionInProgress)
				{
					GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.AcquireGain(moveId);
				}
				else
				{
					GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].downActionPresenter.AcquireGain(moveId);
				}
				if (!PlatformManager.IsStandalone)
				{
					int num = (int)this.action.Amount - (moveId + 1);
					if (num >= 0)
					{
						this.mobileInfoCheckmarks[num].SetActive(true);
						return;
					}
				}
			}
			else if (this.action == null)
			{
				Debug.LogError("[Move presenter] Action is null!!");
			}
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x000BB928 File Offset: 0x000B9B28
		private void CheckMovesLeftCases(Unit selectedUnitLogic, Scythe.BoardPresenter.GameHexPresenter selectedHex)
		{
			this.HidePossibleMoves(true, null, false);
			if (selectedUnitLogic.MovesLeft == 0)
			{
				this.HideExchangePanel();
				this.TurnOffHighlightOnUnit(selectedUnitLogic);
				if (this.unitsHighlight != null && this.unitsHighlight.Contains(GameController.GetUnitPresenter(selectedUnitLogic)))
				{
					this.unitsHighlight.Remove(GameController.GetUnitPresenter(selectedUnitLogic));
				}
				if (GameController.GameManager.moveManager.CombatOnTheHex(selectedHex.GetGameHexLogic()))
				{
					selectedHex.SetFocus(true, HexMarkers.MarkerType.Battle, 0f, false);
				}
				this.UpdateMoveTile((int)this.action.MovesLeft);
			}
			else
			{
				this.UpdateMoveTile((int)(this.action.MovesLeft - 1));
			}
			if (this.action.MovesLeft == 1 && selectedUnitLogic.MovesLeft != 0)
			{
				this.TurnOffHighlightExpectUnit(selectedUnitLogic);
			}
			if (this.action.MovesLeft == 0)
			{
				GameController.GetUnitPresenter(selectedUnitLogic).SetFocus(false, 0);
				this.HideUnitsWithMove();
				this.HideExchangePanel();
				return;
			}
			if (selectedUnitLogic.MovesLeft != 0)
			{
				this.HideWorkersPresenters();
				this.exchangePanel.UpdateResourceInfo();
				this.exchangePanel.UpdateHexWorkers(selectedUnitLogic.position);
				this.exchangePanel.UpdateWorkersButtons();
				if (!GameController.Instance.DragAndDrop)
				{
					this.ShowPossibleMoves(false);
				}
			}
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x000BBA58 File Offset: 0x000B9C58
		private void UpdateViewsAndStateAfterMove(Unit selectedUnitLogic, Scythe.BoardPresenter.GameHexPresenter selectedHex)
		{
			this.lakeMoveInfo.transform.parent.gameObject.SetActive(false);
			List<Worker> list;
			if (selectedUnitLogic.position.Enemy == selectedUnitLogic.Owner)
			{
				list = selectedHex.GetGameHexLogic().GetEnemyWorkers();
			}
			else
			{
				list = selectedHex.GetGameHexLogic().GetOwnerWorkers();
			}
			if (GameController.GameManager.combatManager.IsPlayerInCombat())
			{
				if (PlatformManager.IsStandalone)
				{
					GameController.Instance.panelInfo.DisableEndTurnObjectives();
				}
				else
				{
					SingletonMono<TopMenuPanelsManager>.Instance.GetTopMenuObjectivesPresenter().DisableEndTurnObjectives();
				}
				for (int i = 0; i < list.Count; i++)
				{
					UnitPresenter unitPresenter = GameController.GetUnitPresenter(list[i]);
					if (unitPresenter.UnitLogic.position.Conflict(unitPresenter.UnitLogic) && unitPresenter.UnitLogic.position.GetOwnerUnitCount() != unitPresenter.UnitLogic.position.GetOwnerWorkers().Count)
					{
						unitPresenter.transform.position = selectedHex.GetUnitPosition(list[i]);
						this.TurnOffHighlightOnUnit(list[i]);
						if (this.unitsHighlight != null && this.unitsHighlight.Contains(GameController.GetUnitPresenter(list[i])))
						{
							this.unitsHighlight.Remove(GameController.GetUnitPresenter(list[i]));
						}
					}
				}
			}
			if (this.MovingToFieldWithEnemyWorkers(selectedHex.GetGameHexLogic(), selectedUnitLogic) && PlatformManager.IsStandalone)
			{
				this.endMoveButton.interactable = false;
			}
			GameController.Instance.UpdateStatsPresenter();
			if (GameController.GameManager.combatManager.GetBattlefields().Count != 0)
			{
				this.SetEndingMoveButtonsInteractable(false);
			}
			if (selectedUnitLogic.UnitType == UnitType.Character)
			{
				if (selectedUnitLogic.position.hexType == HexType.factory && selectedUnitLogic.Owner.matPlayer.GetPlayerMatSection(4) == null)
				{
					this.DisableEndTurnObjectives();
				}
				if (selectedUnitLogic.position.hasEncounter && !selectedUnitLogic.position.encounterTaken)
				{
					this.DisableEndTurnObjectives();
				}
			}
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x0003BBD4 File Offset: 0x00039DD4
		private void DisableEndTurnObjectives()
		{
			if (PlatformManager.IsStandalone)
			{
				GameController.Instance.panelInfo.DisableEndTurnObjectives();
				return;
			}
			SingletonMono<TopMenuPanelsManager>.Instance.GetTopMenuObjectivesPresenter().DisableEndTurnObjectives();
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x0003BBFC File Offset: 0x00039DFC
		private bool MovingToFieldWithEnemyWorkers(GameHex hex, Unit movedUnit)
		{
			return hex.Conflict(movedUnit) && !hex.HasOwnerCharacter() && hex.GetOwnerMechs().Count == 0;
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x000BBC48 File Offset: 0x000B9E48
		private void HideWorkersPresenters()
		{
			foreach (Unit unit in this.exchangePanel.GetContext().LoadedWorkers)
			{
				GameController.GetUnitPresenter(unit).gameObject.SetActive(false);
			}
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000BBCB0 File Offset: 0x000B9EB0
		private void ChangePresenterInputState(UnitState unitStatus, UnitPresenter unitPresenter)
		{
			if (unitStatus != UnitState.Standing)
			{
				if (unitStatus != UnitState.PreparingToMove)
				{
					return;
				}
				GameController.UnitGetFocused -= this.OnUnitClicked;
				GameController.UnitGetFocused += this.OnInterruptUnitMove;
				GameController.HexGetFocused -= this.OnGameHexSelected;
				this.exchangePanel.SetInteractable(false);
			}
			else
			{
				GameController.UnitGetFocused -= this.OnInterruptUnitMove;
				GameController.UnitGetFocused += this.OnUnitClicked;
				UnitPresenter.UnitStatusChanged -= this.ChangePresenterInputState;
				if (!GameController.Instance.DragAndDrop)
				{
					GameController.HexGetFocused += this.OnGameHexSelected;
				}
				GameController.GameManager.moveManager.DoesWorkersNeedToRetreat(GameController.GameManager.moveManager.GetLastSelectedGameHex());
				if (unitPresenter.UnitLogic == this.lastClickedUnit)
				{
					if (this.lastClickedUnit.MovesLeft != 0)
					{
						if (GameController.Instance.DragAndDrop && this.exchangePanel.DragAndDropBar.PreviousUnitEqualsUnit(this.lastClickedUnit) && this.exchangePanel.DragAndDropBar.AnythingLoaded())
						{
							this.exchangePanel.DragAndDropBar.SetActive(true, true);
						}
					}
					else
					{
						this.exchangePanel.UpdateUnitInfo(this.lastClickedUnit, false);
						this.exchangePanel.UnloadAllWorkersFromMech();
						this.exchangePanel.ClearPreviousUnit(false);
						this.exchangePanel.OnUnitMoveFinished();
					}
				}
				this.exchangePanel.SetInteractable(true);
				if (unitPresenter.UnitLogic == this.lastClickedUnit && this.action.MovesLeft == 0 && !GameController.GameManager.combatManager.WorkersRetreat())
				{
					this.OnActionEnded();
					return;
				}
			}
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000BBE54 File Offset: 0x000BA054
		private void OnInterruptUnitMove(UnitPresenter presenter)
		{
			if (presenter.UnitLogic.Owner == GameController.GameManager.PlayerCurrent && presenter.UnitLogic.MovesLeft > 0)
			{
				GameController.GetUnitPresenter(this.lastClickedUnit).ForceFinishMoveAnimation();
				GameController.UnitGetFocused -= this.OnInterruptUnitMove;
				GameController.Instance.gameBoardPresenter.UpdateUnits(true);
			}
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000BBEB8 File Offset: 0x000BA0B8
		public void OnTokenPlaced(Unit unit)
		{
			if (unit == this.gameManager.moveManager.GetSelectedUnit())
			{
				this.CheckMovesLeftCases(unit, GameController.Instance.GetGameHexPresenter(unit.position));
				this.UpdateViewsAndStateAfterMove(unit, GameController.Instance.GetGameHexPresenter(unit.position));
			}
			if (this.action.MovesLeft == 0 && !this.gameManager.combatManager.WorkersRetreat())
			{
				this.OnActionEnded();
			}
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x0003BC1F File Offset: 0x00039E1F
		private void OnUnitLoaded(UnitPresenter presenter)
		{
			if (this.lastClickedUnit != null && this.lastClickedUnit.UnitType == UnitType.Mech)
			{
				Debug.Log("Loading");
				return;
			}
			Debug.Log("NOPE");
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000BBF2C File Offset: 0x000BA12C
		private void ShowExchangePanel(Unit unit, SelectMethod selectMethod = SelectMethod.Default)
		{
			this.exchangePanel.gameObject.SetActive(true);
			this.exchangePanel.SetPanelParameters(unit, false, selectMethod);
			if (MobileChat.IsSupported)
			{
				SingletonMono<MobileChat>.Instance.ResetScroll();
			}
			if (this.unitSelectInfo != null)
			{
				this.unitSelectInfo.SetActive(false);
			}
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0003BC4C File Offset: 0x00039E4C
		private void HideExchangePanel()
		{
			this.exchangePanel.gameObject.SetActive(false);
			if (MobileChat.IsSupported)
			{
				SingletonMono<MobileChat>.Instance.ResetScroll();
			}
			if (this.unitSelectInfo != null)
			{
				this.unitSelectInfo.SetActive(true);
			}
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x0003BC8A File Offset: 0x00039E8A
		public void UnloadWorker(Unit worker)
		{
			GameController.GameManager.moveManager.UnloadWorkerFromSelectedMech(worker);
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x0003BC9D File Offset: 0x00039E9D
		public void UpdateWorkerButtons(Unit mech)
		{
			if (mech != this.lastClickedUnit)
			{
				return;
			}
			this.exchangePanel.UpdateHexWorkers(mech.position);
			this.exchangePanel.UpdateWorkersButtons();
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x0003BCC5 File Offset: 0x00039EC5
		public void OnCombatStateChanged(CombatStage stage)
		{
			if (stage != CombatStage.CombatResovled && !GameController.GameManager.combatManager.CombatAlreadyStarted())
			{
				this.OnCombatBegin();
				return;
			}
			if (stage == CombatStage.CombatResovled && GameController.GameManager.combatManager.CombatAlreadyStarted())
			{
				this.OnCombatEnded();
			}
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x000BBF84 File Offset: 0x000BA184
		public void OnCombatBegin()
		{
			GameController.UnitGetFocused -= this.OnUnitClicked;
			GameController.HexGetFocused -= this.OnGameHexSelected;
			ExchangePanelPresenter.OnWorkerButtonClicked -= this.UnloadWorker;
			this.SetEndingMoveButtonsInteractable(false);
			GameController.Instance.UpdateStats(true, true);
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x000BBFD8 File Offset: 0x000BA1D8
		public void OnCombatEnded()
		{
			if (this.action == null)
			{
				Debug.LogError("Action is empty!!!!");
				return;
			}
			if (GameController.GameManager.combatManager.GetBattlefields().Count == 0)
			{
				this.SetEndingMoveButtonsInteractable(true);
			}
			ExchangePanelPresenter.OnWorkerButtonClicked += this.UnloadWorker;
			if (GameController.GameManager.PlayerCurrent.matPlayer.matPlayerSectionsCount != 5 && GameController.GameManager.PlayerCurrent.character.position.hexType != HexType.factory && this.action.MovesLeft != 0)
			{
				GameController.Instance.UpdateStats(true, false);
			}
			if (PlatformManager.IsStandalone)
			{
				this.endMoveButton.interactable = true;
			}
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x0003BCFE File Offset: 0x00039EFE
		private void SetEndingMoveButtonsInteractable(bool interactable)
		{
			if (interactable)
			{
				GameController.Instance.EndTurnButtonEnable();
				return;
			}
			GameController.Instance.EndTurnButtonDisable();
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x0003BD18 File Offset: 0x00039F18
		public bool HaveAction()
		{
			return this.action != null;
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0003BD23 File Offset: 0x00039F23
		private void SetAction(GainMove gainMove)
		{
			this.action = gainMove;
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x0003BD2C File Offset: 0x00039F2C
		public void SetActionOnLoad(GainMove gainMove)
		{
			this.SetAction(gainMove);
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x000BC088 File Offset: 0x000BA288
		public void WarningBeforeActionEnd()
		{
			if (GameController.GameManager.IsCampaign)
			{
				this.OnActionEnded();
				return;
			}
			if (this.action.MovesLeft > 0 && OptionsManager.IsWarningsActive())
			{
				if (PlatformManager.IsStandalone)
				{
					this.endAction.GetComponentInChildren<Text>().text = ScriptLocalization.Get("PlayerMat/EndMoveText");
				}
				else
				{
					this.endAction.GetFirstDescriptionLine().GetComponent<TextMeshProUGUI>().text = ScriptLocalization.Get("PlayerMat/EndMoveText");
				}
				WorldSFXManager.PlaySound(SoundEnum.Alert, AudioSourceType.Buttons);
				this.endAction.Show(GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo, delegate
				{
					this.OnActionEnded();
				}, null);
				return;
			}
			this.OnActionEnded();
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x000BC154 File Offset: 0x000BA354
		public override void Clear()
		{
			this.lastClickedUnit = null;
			this.exchangePanel.Clear();
			this.exchangePanel.gameObject.SetActive(false);
			this.lakeMoveInfo.transform.parent.gameObject.SetActive(false);
			this.exchangePanel.Clear();
			if (GameController.Instance.DragAndDrop)
			{
				this.CursorNoHit();
				this.exchangePanel.DragAndDropBar.Clear();
				this.exchangePanel.DragAndDropBar.SetActive(false, false);
			}
			this.gainStuffAnimation.SetActive(true);
			this.teleportLine.widthMultiplier = 0f;
			this.ShowPath(null);
			GameController.Instance.cameraControler.HooverReset();
			if (PlatformManager.IsStandalone)
			{
				this.endMoveButton.gameObject.SetActive(false);
			}
			this.HidePossibleMoves(true, null, false);
			this.HideUnitsWithMove();
			this.HookController.SetResourceFocusDetectorEnabled(false);
			GameController.HexSelectionMode = GameController.SelectionMode.Normal;
			GameController.ClearFocus();
			this.SetAction(null);
			if (GameController.GameManager != null)
			{
				GameController.GameManager.moveManager.Clear();
			}
			this.DisconnectDelegates();
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x000BC274 File Offset: 0x000BA474
		private void DisconnectDelegates()
		{
			MovePresenter.MoveEnded = null;
			UnitPresenter.UnitStatusChanged -= this.ChangePresenterInputState;
			if (GameController.GameManager != null)
			{
				GameController.GameManager.combatManager.OnCombatStageChanged -= this.OnCombatStateChanged;
			}
			GameController.UnitGetFocused -= this.OnUnitClicked;
			GameController.UnitGetFocused -= this.OnInterruptUnitMove;
			GameController.HexGetFocused -= this.OnGameHexSelected;
			ExchangePanelPresenter.OnWorkerButtonClicked -= this.UnloadWorker;
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x000BC300 File Offset: 0x000BA500
		public override void OnActionEnded()
		{
			if (MovePresenter.MoveEnded != null)
			{
				MovePresenter.MoveEnded();
			}
			if (!PlatformManager.IsStandalone)
			{
				this.unitSelectInfo.SetActive(false);
				GameController.Instance.tokensController.HidePanels();
			}
			this.HookController.FinishWork();
			this.Clear();
			HumanInputHandler.Instance.OnInputEnded();
			if (GameController.GameManager.combatManager.GetBattlefields().Count > 0)
			{
				if (GameController.GameManager.combatManager.GetBattlefields().Count == 1)
				{
					if (OptionsManager.IsCameraAnimationsActive())
					{
						ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.combatManager.GetBattlefields()[0]).GetWorldPosition());
					}
				}
				else
				{
					List<Vector3> list = new List<Vector3>();
					foreach (GameHex gameHex in GameController.GameManager.combatManager.GetBattlefields())
					{
						list.Add(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(gameHex).GetWorldPosition());
					}
					Vector3 vector = AnimateCamera.Instance.CalculateCenterOfHexes(list);
					float num = AnimateCamera.Instance.CalculateZoomToShowGivenHexes(new HashSet<GameHex>(GameController.GameManager.combatManager.GetBattlefields()));
					if (OptionsManager.IsCameraAnimationsActive())
					{
						ShowEnemyMoves.Instance.AnimateCamToShowAllHexes(vector, num);
					}
				}
			}
			GameController.Instance.undoController.TriggerUndoInteractivityChange(true);
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x0003BD35 File Offset: 0x00039F35
		public void SelectUnitToShowRange(UnitPresenter unit)
		{
			this.SelectUnit(unit, SelectMethod.Default);
		}

		// Token: 0x04001587 RID: 5511
		public ExchangePanelPresenter exchangePanel;

		// Token: 0x04001588 RID: 5512
		public GameObject unitSelectInfo;

		// Token: 0x04001589 RID: 5513
		public Text lakeMoveInfo;

		// Token: 0x0400158A RID: 5514
		public YesNoDialog unattendedResources;

		// Token: 0x0400158B RID: 5515
		public YesNoDialog endAction;

		// Token: 0x0400158C RID: 5516
		public Button endMoveButton;

		// Token: 0x0400158D RID: 5517
		public GameObject gainStuffAnimation;

		// Token: 0x0400158E RID: 5518
		public LineRenderer teleportLine;

		// Token: 0x0400158F RID: 5519
		public LineRenderer pathLine;

		// Token: 0x04001590 RID: 5520
		public GameObject[] mobileInfoTiles;

		// Token: 0x04001591 RID: 5521
		public GameObject[] mobileInfoCheckmarks;

		// Token: 0x04001592 RID: 5522
		private GainMove action;

		// Token: 0x04001593 RID: 5523
		private HashSet<Scythe.BoardPresenter.GameHexPresenter> hexesWithUnits;

		// Token: 0x04001594 RID: 5524
		private Dictionary<GameHex, GameHex> possibleMoves;

		// Token: 0x04001595 RID: 5525
		private HashSet<UnitPresenter> unitsHighlight;

		// Token: 0x04001596 RID: 5526
		private int spawnOrder = 1;

		// Token: 0x04001597 RID: 5527
		[SerializeField]
		private int firstAnimationHexAmount;

		// Token: 0x04001598 RID: 5528
		[SerializeField]
		private int secondAnimationHexAmount;

		// Token: 0x04001599 RID: 5529
		[SerializeField]
		private int thirdAnimationHexAmount;

		// Token: 0x0400159A RID: 5530
		[SerializeField]
		private int allMarkers;

		// Token: 0x0400159B RID: 5531
		[SerializeField]
		private float rangeMarkersAnimationOffset = 0.2f;

		// Token: 0x0400159C RID: 5532
		[SerializeField]
		private float fadeInHexTime = 0.3f;

		// Token: 0x0400159D RID: 5533
		private Dictionary<GameHex, int> movesForDistanceCheck;

		// Token: 0x0400159E RID: 5534
		private int iterations;

		// Token: 0x0400159F RID: 5535
		private Unit lastClickedUnit;

		// Token: 0x040015A0 RID: 5536
		private Coroutine secondMoveRangeHighlight;

		// Token: 0x040015A1 RID: 5537
		private Coroutine thirdMoveRangeHighlight;

		// Token: 0x040015A3 RID: 5539
		private bool unitClickedTemp;

		// Token: 0x020003F1 RID: 1009
		// (Invoke) Token: 0x06001E8F RID: 7823
		public delegate void MoveEnd();
	}
}

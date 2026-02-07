using System;
using System.Collections;
using System.Collections.Generic;
using HoneyFramework;
using I2.Loc;
using Scythe.Analytics;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200038A RID: 906
	public class CampaignPresenter : MonoBehaviour
	{
		// Token: 0x06001A2B RID: 6699 RVA: 0x000A3754 File Offset: 0x000A1954
		private void RemoveAllListenersOfTutorial02()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial02AsmPopup00;
			this.tutorial02AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup01));
			this.tutorial02AsmPanels[1].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup02));
			this.tutorial02AsmPanels[2].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup03));
			this.tutorial02AsmPanels[3].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup04));
			this.tutorial02AsmPanels[4].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup05));
			this.tutorial02AsmPanels[5].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup06));
			this.tutorial02AsmPanels[6].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup07));
			this.playerMatSections[0].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup10));
			GameController.UnitGetFocused -= this.Tutorial02AsmPopup11SelectionResolver;
			ExchangePanelPresenter.OnWorkerLoaded -= this.Tutorial02AsmPopup12;
			GameController.HexGetFocused -= this.Tutorial02AsmPopup13HexFocused;
			ExchangePanelPresenter.OnResourceLoaded -= this.Tutorial02AsmPopup15;
			GameController.HexGetFocused -= this.Tutorial02AsmPopup16HexFocused;
			UnitPresenter.UnitStatusChanged -= this.Tutorial02AsmPopup17;
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial02AsmEnd));
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x000A3920 File Offset: 0x000A1B20
		private void Tutorial02AsmStart()
		{
			this.MissionProgress.enabled = true;
			this.MissionProgress.text = string.Empty;
			GameObject[] array = this.tutorial02AsmPanels;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial02AsmPopup05;
			CameraControler.Instance.mouseLastMoved = Time.realtimeSinceStartup;
			AnalyticsEventData.TutorialStart(StepIDs.tut2_00_introduction);
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x000A399C File Offset: 0x000A1B9C
		private void Tutorial02AsmPopup05()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial02AsmPopup05;
			CameraControler.Instance.mouseLastMoved = Time.realtimeSinceStartup;
			this.tutorial02AsmPanels[5].SetActive(true);
			this.tutorial02AsmPanels[5].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial02AsmPopup00));
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x000A3A08 File Offset: 0x000A1C08
		private void Tutorial02AsmPopup00()
		{
			this.tutorial02AsmPanels[5].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup00));
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[i, j], false);
				}
			}
			this.tutorial02AsmPanels[5].SetActive(false);
			this.tutorial02AsmPanels[0].SetActive(true);
			this.tutorial02AsmPanels[0].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial02AsmPopup01));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x000A3AB4 File Offset: 0x000A1CB4
		private void Tutorial02AsmPopup01()
		{
			this.tutorial02AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup01));
			this.tutorial02AsmPanels[0].SetActive(false);
			this.tutorial02AsmPanels[1].SetActive(true);
			this.tutorial02AsmPanels[1].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial02AsmPopup02));
			for (int i = 0; i < GameController.GameManager.gameBoard.tunnels.Count; i++)
			{
				this.arrowBoard[i].Show();
				this.arrowBoard[i].SetPosition(GameController.GameManager.gameBoard.tunnels[i]);
			}
			ShowEnemyMoves.Instance.AnimateCamToShowAllHexes(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.factory).GetWorldPosition(), 0.6f);
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x000A3BAC File Offset: 0x000A1DAC
		private void Tutorial02AsmPopup02()
		{
			this.tutorial02AsmPanels[1].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup02));
			for (int i = 0; i < this.arrowBoard.Length; i++)
			{
				this.arrowBoard[i].Hide();
			}
			this.tutorial02AsmPanels[1].SetActive(false);
			this.tutorial02AsmPanels[2].SetActive(true);
			this.tutorial02AsmPanels[2].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial02AsmPopup03));
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[1, 4]);
			this.arrowBoard[1].Show();
			this.arrowBoard[1].SetPosition(GameController.GameManager.gameBoard.hexMap[2, 5]);
			ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.hexMap[2, 4]).GetWorldPosition());
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x000A3CD8 File Offset: 0x000A1ED8
		private void Tutorial02AsmPopup03()
		{
			this.tutorial02AsmPanels[2].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup03));
			this.arrowBoard[0].Hide();
			this.arrowBoard[1].Hide();
			this.tutorial02AsmPanels[2].SetActive(false);
			this.tutorial02AsmPanels[3].SetActive(true);
			this.tutorial02AsmPanels[3].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial02AsmPopup04));
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.bases[Faction.Polania]);
			ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.bases[Faction.Polania]).GetWorldPosition());
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x000A3DCC File Offset: 0x000A1FCC
		private void Tutorial02AsmPopup04()
		{
			this.tutorial02AsmPanels[3].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup04));
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[2, 3]);
			this.arrowBoard[1].Show();
			this.arrowBoard[1].SetPosition(GameController.GameManager.gameBoard.hexMap[2, 5]);
			ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.hexMap[1, 4]).GetWorldPosition());
			this.tutorial02AsmPanels[3].SetActive(false);
			this.tutorial02AsmPanels[4].SetActive(true);
			this.tutorial02AsmPanels[4].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial02AsmPopup06));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x000A3ED8 File Offset: 0x000A20D8
		private void Tutorial02AsmPopup06()
		{
			this.tutorial02AsmPanels[4].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup06));
			this.arrowBoard[0].Hide();
			this.arrowBoard[1].Hide();
			this.movementPlan.SetActive(true);
			this.tutorial02AsmPanels[4].SetActive(false);
			this.tutorial02AsmPanels[6].SetActive(true);
			this.tutorial02AsmPanels[6].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial02AsmPopup07));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x000A3F78 File Offset: 0x000A2178
		private void Tutorial02AsmPopup07()
		{
			this.tutorial02AsmPanels[6].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup07));
			this.tutorial02AsmPanels[6].SetActive(false);
			this.tutorial02AsmPanels[7].SetActive(true);
			this.playerMatSections[0].topActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial02AsmPopup10));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x000A3FF8 File Offset: 0x000A21F8
		private void Tutorial02AsmPopup10()
		{
			this.playerMatSections[0].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup10));
			this.movementPlan.SetActive(false);
			this.tutorial02AsmPanels[7].SetActive(false);
			this.tutorial02AsmPanels[10].SetActive(true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[1, 4]);
			Collider[] array = GameController.GetUnitPresenter(GameController.GameManager.PlayerCurrent.character).GetComponentsInChildren<Collider>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
			foreach (Worker worker in GameController.GameManager.PlayerCurrent.matPlayer.workers)
			{
				array = GameController.GetUnitPresenter(worker).GetComponentsInChildren<Collider>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].enabled = false;
				}
			}
			GameController.UnitGetFocused += this.Tutorial02AsmPopup11SelectionResolver;
			if (!this.revertStep)
			{
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
			this.backSteps = 0;
			this.backSteps++;
			this.revertStep = false;
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x000A4164 File Offset: 0x000A2364
		private void Tutorial02AsmPopup11SelectionResolver(UnitPresenter presenter)
		{
			if (presenter.UnitLogic.UnitType == UnitType.Mech)
			{
				this.Tutorial02AsmPopup11();
				return;
			}
			this.AllowClickOnHex(0, 4, false);
			this.tutorial02AsmPanels[11].SetActive(false);
			this.tutorial02AsmPanels[12].SetActive(false);
			GameController.UnitGetFocused -= this.Tutorial02AsmPopup11SelectionResolver;
			this.revertStep = true;
			this.DefaultTutorialStepLog(StepStatuses.failed, this.backSteps);
			this.Tutorial02AsmPopup10();
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x000A41D8 File Offset: 0x000A23D8
		private void Tutorial02AsmPopup11()
		{
			GameController.GetUnitPresenter(GameController.GameManager.PlayerCurrent.character).GetComponent<Collider>().enabled = false;
			this.gameController.matPlayer.confirmActionSelection.onClick.RemoveListener(new UnityAction(this.Tutorial02AsmPopup10));
			if (this.tutorial02AsmPanels[11].activeInHierarchy)
			{
				this.DefaultTutorialStepLog(StepStatuses.failed, 1);
				return;
			}
			this.tutorial02AsmPanels[10].SetActive(false);
			this.tutorial02AsmPanels[11].SetActive(true);
			this.arrowBoard[0].Hide();
			ExchangePanelPresenter.OnWorkerLoaded += this.Tutorial02AsmPopup12;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			this.backSteps++;
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x000A4298 File Offset: 0x000A2498
		private void Tutorial02AsmPopup12(Unit worker)
		{
			GameController.UnitGetFocused -= this.Tutorial02AsmPopup11SelectionResolver;
			if (GameController.GetUnitPresenter(GameController.GameManager.gameBoard.hexMap[1, 4].GetOwnerMechs()[0]).GetWorkersList().Count == 4 && this.tutorial02AsmPanels[11].activeInHierarchy)
			{
				this.tutorial02AsmPanels[11].SetActive(false);
				this.tutorial02AsmPanels[12].SetActive(true);
				this.arrowBoard[0].Show();
				this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[0, 4]);
				this.AllowClickOnHex(0, 4, true);
				GameController.HexGetFocused += this.Tutorial02AsmPopup13HexFocused;
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
				this.backSteps++;
			}
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x000A437C File Offset: 0x000A257C
		private void Tutorial02AsmPopup13HexFocused(Scythe.BoardPresenter.GameHexPresenter presenter)
		{
			GameController.GetUnitPresenter(GameController.GameManager.PlayerCurrent.character).GetComponent<Collider>().enabled = true;
			GameController.HexGetFocused -= this.Tutorial02AsmPopup13HexFocused;
			ExchangePanelPresenter.OnWorkerLoaded -= this.Tutorial02AsmPopup12;
			GameController.UnitGetFocused -= this.Tutorial02AsmPopup11SelectionResolver;
			this.backSteps = 0;
			this.arrowBoard[0].Hide();
			UnitPresenter.UnitStatusChanged += this.Tutorial02AsmPopup13;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x000A4408 File Offset: 0x000A2608
		private void Tutorial02AsmPopup13(UnitState unitState, UnitPresenter unitPresenter)
		{
			UnitPresenter.UnitStatusChanged -= this.Tutorial02AsmPopup13;
			this.tutorial02AsmPanels[12].SetActive(false);
			this.tutorial02AsmPanels[13].SetActive(true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[2, 5]);
			Collider[] array = GameController.GetUnitPresenter(GameController.GameManager.PlayerCurrent.character).GetComponentsInChildren<Collider>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = true;
			}
			foreach (Worker worker in GameController.GameManager.PlayerCurrent.matPlayer.workers)
			{
				array = GameController.GetUnitPresenter(worker).GetComponentsInChildren<Collider>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].enabled = true;
				}
			}
			GameController.UnitGetFocused += this.Tutorial02AsmPopup14SelectionResolver;
			this.backSteps = 0;
			this.backSteps++;
			this.revertStep = false;
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x000A4540 File Offset: 0x000A2740
		private void Tutorial02AsmPopup14SelectionResolver(UnitPresenter presenter)
		{
			this.tutorial02AsmPanels[14].SetActive(false);
			this.tutorial02AsmPanels[15].SetActive(false);
			this.tutorial02AsmPanels[16].SetActive(false);
			if (presenter.UnitLogic.UnitType == UnitType.Character)
			{
				this.Tutorial02AsmPopup14();
				return;
			}
			GameController.UnitGetFocused -= this.Tutorial02AsmPopup14SelectionResolver;
			this.AllowClickOnHex(2, 3, false);
			this.revertStep = true;
			this.DefaultTutorialStepLog(StepStatuses.failed, this.backSteps);
			this.Tutorial02AsmPopup13(UnitState.Standing, presenter);
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x000A45C4 File Offset: 0x000A27C4
		private void Tutorial02AsmPopup14()
		{
			if (this.tutorial02AsmPanels[14].activeInHierarchy)
			{
				this.DefaultTutorialStepLog(StepStatuses.failed, 1);
				return;
			}
			this.tutorial02AsmPanels[13].SetActive(false);
			this.tutorial02AsmPanels[14].SetActive(true);
			this.arrowBoard[0].Hide();
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			this.backSteps++;
			ExchangePanelPresenter.OnResourceLoaded += this.Tutorial02AsmPopup15;
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x000A463C File Offset: 0x000A283C
		private void Tutorial02AsmPopup15()
		{
			if (this.exchangePanelPresenter.GetResources()[ResourceType.food] == 2 && !this.tutorial02AsmPanels[15].activeInHierarchy)
			{
				ExchangePanelPresenter.OnResourceLoaded -= this.Tutorial02AsmPopup15;
				this.tutorial02AsmPanels[14].SetActive(false);
				this.tutorial02AsmPanels[15].SetActive(true);
				this.arrowBoard[0].Show();
				this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[2, 3]);
				this.AllowClickOnHex(2, 3, true);
				GameController.HexGetFocused += this.Tutorial02AsmPopup16HexFocused;
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
				this.backSteps++;
			}
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x000396B1 File Offset: 0x000378B1
		private void Tutorial02AsmPopup16HexFocused(Scythe.BoardPresenter.GameHexPresenter presenter)
		{
			this.backSteps = 0;
			GameController.HexGetFocused -= this.Tutorial02AsmPopup16HexFocused;
			this.arrowBoard[0].Hide();
			UnitPresenter.UnitStatusChanged += this.Tutorial02AsmPopup17;
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x000A4704 File Offset: 0x000A2904
		private void Tutorial02AsmPopup17(UnitState unitState, UnitPresenter unitPresenter)
		{
			if (unitState != UnitState.Standing || this.tutorial02AsmPanels[16].activeInHierarchy)
			{
				return;
			}
			UnitPresenter.UnitStatusChanged -= this.Tutorial02AsmPopup17;
			this.tutorial02AsmPanels[15].SetActive(false);
			this.tutorial02AsmPanels[16].SetActive(true);
			this.endMoveText.text = ScriptLocalization.Get("GameScene/EndMission");
			this.endTurnButton.onClick.AddListener(new UnityAction(this.Tutorial02AsmEnd));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x000A4790 File Offset: 0x000A2990
		private void Tutorial02AsmCleanup()
		{
			AnalyticsEventData.TutorialFinished();
			AnalyticsEventData.TutorialStepStoped();
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			AnalyticsEventData.ResetTutorialStepTimer();
			this.backSteps = 0;
			this.revertStep = false;
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial02AsmEnd));
			this.tutorial02AsmPanels[16].SetActive(false);
			TutorialMissionSelection.MissionCompleted(1);
			GameController.GameManager.EndTutorial();
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x000396E9 File Offset: 0x000378E9
		public void Tutorial02AsmEnd()
		{
			this.Tutorial02AsmCleanup();
			this.gameController.ExitGame();
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x000396FC File Offset: 0x000378FC
		public void Tutorial02AsmNext()
		{
			this.Tutorial02AsmCleanup();
			this.LoadTutorial(2, 0);
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x0003970C File Offset: 0x0003790C
		private void OnDestroy()
		{
			if (!PlatformManager.IsMobile)
			{
				BattleResultPanel.OnShowWinnerAnimationComplete -= this.ShowResultOKArrow;
			}
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x000A4800 File Offset: 0x000A2A00
		private void RemoveAllListenersOfTutorial03()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial03AsmPopup00;
			PlayerStatsPresenter.BottomArrowExpanded -= this.Tutorial03AsmPopup01;
			this.tutorial03AsmPanels[1].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup02));
			this.tutorial03AsmPanels[2].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup03));
			this.playerMatSections[1].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup05));
			this.endTurnButton.onClick.RemoveListener(new UnityAction(this.CloseTutorial03AsmPopup05));
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial03AsmPopup06;
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup08));
			GameController.UnitGetFocused -= this.Tutorial03AsmPopup09SelectionResolver;
			UnitPresenter.UnitStatusChanged -= this.Tutorial03AsmPopup10SelectionResolver;
			MovePresenter.MoveEnded -= this.Tutorial03AsmPopup11;
			GameController.GameManager.combatManager.OnCombatStageChanged -= this.Tutorial03AsmPopup12;
			CombatPresenter.OnBattleFieldSelected -= this.Tutorial03AsmPopup13;
			CombatPreperationPresenter.OnPowerChanged -= this.Tutorial03AsmPopup14;
			CombatPreperationPresenter.OnCardAdded -= this.Tutorial03AsmPopup15;
			CombatPreperationPresenter.OnFightClicked -= this.Tutorial03AsmPopup16;
			BattleResultPanel.OnResultAccept -= this.Tutorial03AsmPopup17;
			this.tutorial03AsmPanels[17].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup18));
			this.tutorial03AsmPanels[18].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup19));
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial03AsmEnd));
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x000A4A14 File Offset: 0x000A2C14
		private void Tutorial03AsmStart()
		{
			this.MissionProgress.enabled = true;
			this.MissionProgress.text = string.Empty;
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial03AsmPopup00a;
			AnalyticsEventData.TutorialStart(StepIDs.tut3_00_introduction);
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x000A4A60 File Offset: 0x000A2C60
		private void Tutorial03AsmPopup00a()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial03AsmPopup00a;
			this.tutorial03AsmPanels[20].SetActive(true);
			this.tutorial03AsmPanels[20].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial03AsmPopup00b));
			GameController.GameManager.PlayerCurrent.combatCards.Clear();
			GameController.GameManager.PlayerCurrent.combatCards.Add(new CombatCard(2));
			GameController.GameManager.PlayerCurrent.combatCards.Add(new CombatCard(2));
			GameController.GameManager.PlayerCurrent.combatCards.Add(new CombatCard(2));
			GameController.GameManager.PlayerCurrent.combatCards.Add(new CombatCard(2));
			this.gameController.UpdateStats(false, false);
			if (this.combatCardPanelPresenter != null)
			{
				this.combatCardPanelPresenter.SetCards(GameController.GameManager.PlayerCurrent.combatCards, null);
			}
			CameraControler.Instance.mouseLastMoved = Time.realtimeSinceStartup;
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x000A4B80 File Offset: 0x000A2D80
		private void Tutorial03AsmPopup00b()
		{
			this.tutorial03AsmPanels[20].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup00b));
			this.tutorial03AsmPanels[20].SetActive(false);
			this.tutorial03AsmPanels[21].SetActive(true);
			this.tutorial03AsmPanels[21].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial03AsmPopup00c));
			CameraControler.Instance.mouseLastMoved = Time.realtimeSinceStartup;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x000A4C0C File Offset: 0x000A2E0C
		private void Tutorial03AsmPopup00c()
		{
			this.tutorial03AsmPanels[21].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup00c));
			this.tutorial03AsmPanels[21].SetActive(false);
			this.tutorial03AsmPanels[22].SetActive(true);
			this.tutorial03AsmPanels[22].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial03AsmPopup00));
			CameraControler.Instance.mouseLastMoved = Time.realtimeSinceStartup;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x000A4C98 File Offset: 0x000A2E98
		private void Tutorial03AsmPopup00()
		{
			this.tutorial03AsmPanels[22].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup00));
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[i, j], false);
				}
			}
			this.AllowClickOnHex(2, 3, true);
			this.tutorial03AsmPanels[22].SetActive(false);
			this.tutorial03AsmPanels[0].SetActive(true);
			ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.hexMap[2, 3]).GetWorldPosition());
			PlayerStatsPresenter.BottomArrowExpanded += this.Tutorial03AsmPopup01;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x000A4D74 File Offset: 0x000A2F74
		private void Tutorial03AsmPopup01()
		{
			PlayerStatsPresenter.BottomArrowExpanded -= this.Tutorial03AsmPopup01;
			this.tutorial03AsmPanels[0].SetActive(false);
			this.tutorial03AsmPanels[1].SetActive(true);
			this.tutorial03AsmPanels[1].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial03AsmPopup02));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x000A4DDC File Offset: 0x000A2FDC
		private void Tutorial03AsmPopup02()
		{
			this.tutorial03AsmPanels[1].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup02));
			this.tutorial03AsmPanels[1].SetActive(false);
			this.tutorial03AsmPanels[2].SetActive(true);
			this.tutorial03AsmPanels[2].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial03AsmPopup03));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x000A4E54 File Offset: 0x000A3054
		private void Tutorial03AsmPopup03()
		{
			this.tutorial03AsmPanels[2].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup03));
			this.tutorial03AsmPanels[2].SetActive(false);
			this.tutorial03AsmPanels[3].SetActive(true);
			this.playerMatSections[1].topActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial03AsmPopup05));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x000A4ED4 File Offset: 0x000A30D4
		private void Tutorial03AsmPopup05()
		{
			this.playerMatSections[1].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup05));
			this.tutorial03AsmPanels[3].SetActive(false);
			this.tutorial03AsmPanels[5].SetActive(true);
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial03AsmPopup06;
			this.endTurnButton.onClick.AddListener(new UnityAction(this.CloseTutorial03AsmPopup05));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x00039726 File Offset: 0x00037926
		private void CloseTutorial03AsmPopup05()
		{
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			this.endTurnButton.onClick.RemoveListener(new UnityAction(this.CloseTutorial03AsmPopup05));
			this.tutorial03AsmPanels[5].SetActive(false);
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x000A4F68 File Offset: 0x000A3168
		private void Tutorial03AsmPopup06()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial03AsmPopup06;
			this.tutorial03AsmPanels[5].SetActive(false);
			this.tutorial03AsmPanels[6].SetActive(true);
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial03AsmPopup08));
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x000A4FD8 File Offset: 0x000A31D8
		private void Tutorial03AsmPopup08()
		{
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup08));
			this.tutorial03AsmPanels[6].SetActive(false);
			this.tutorial03AsmPanels[8].SetActive(true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[3, 3]);
			GameController.UnitGetFocused += this.Tutorial03AsmPopup09SelectionResolver;
			if (!this.revertStep)
			{
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
			this.backSteps = 0;
			this.backSteps++;
			this.revertStep = false;
			GameController.GetUnitPresenter(GameController.GameManager.PlayerCurrent.character).GetComponent<Collider>().enabled = false;
			if (!PlatformManager.IsMobile)
			{
				foreach (Worker worker in GameController.GameManager.PlayerCurrent.matPlayer.workers)
				{
					Collider[] componentsInChildren = GameController.GetUnitPresenter(worker).GetComponentsInChildren<Collider>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].enabled = false;
					}
				}
			}
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x000A512C File Offset: 0x000A332C
		private void Tutorial03AsmPopup09SelectionResolver(UnitPresenter presenter)
		{
			if (presenter.UnitLogic.UnitType == UnitType.Mech)
			{
				if (this.tutorial03AsmPanels[9].activeInHierarchy)
				{
					this.DefaultTutorialStepLog(StepStatuses.failed, 1);
				}
				this.AllowClickOnHex(2, 3, true);
				this.Tutorial03AsmPopup09();
				return;
			}
			this.tutorial03AsmPanels[9].SetActive(false);
			this.tutorial03AsmPanels[10].SetActive(false);
			GameController.UnitGetFocused -= this.Tutorial03AsmPopup09SelectionResolver;
			this.DefaultTutorialStepLog(StepStatuses.failed, this.backSteps);
			this.revertStep = true;
			this.AllowClickOnHex(2, 3, false);
			this.Tutorial03AsmPopup08();
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x000A51C4 File Offset: 0x000A33C4
		private void Tutorial03AsmPopup09()
		{
			this.tutorial03AsmPanels[8].SetActive(false);
			this.tutorial03AsmPanels[9].SetActive(true);
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[2, 3]);
			GameController.UnitGetFocused -= this.Tutorial03AsmPopup09SelectionResolver;
			UnitPresenter.UnitStatusChanged += this.Tutorial03AsmPopup10SelectionResolver;
			this.backSteps++;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x0003975A File Offset: 0x0003795A
		private void Tutorial03AsmPopup10SelectionResolver(UnitState status, UnitPresenter unitPresenter)
		{
			if (status == UnitState.Standing)
			{
				GameController.UnitGetFocused -= this.Tutorial03AsmPopup09SelectionResolver;
				UnitPresenter.UnitStatusChanged -= this.Tutorial03AsmPopup10SelectionResolver;
				this.Tutorial03AsmPopup10();
			}
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x000A524C File Offset: 0x000A344C
		private void Tutorial03AsmPopup10()
		{
			GameController.GetUnitPresenter(GameController.GameManager.PlayerCurrent.character).GetComponent<Collider>().enabled = true;
			this.tutorial03AsmPanels[9].SetActive(false);
			this.tutorial03AsmPanels[10].SetActive(true);
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[3, 3]);
			GameController.UnitGetFocused += this.Tutorial03AsmPopup11SelectionResolver;
			if (!this.revertStep)
			{
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
			this.backSteps = 0;
			this.backSteps++;
			this.revertStep = false;
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x000A52F8 File Offset: 0x000A34F8
		private void Tutorial03AsmPopup11SelectionResolver(UnitPresenter presenter)
		{
			GameController.UnitGetFocused -= this.Tutorial03AsmPopup11SelectionResolver;
			if (presenter.UnitLogic.UnitType != UnitType.Character)
			{
				this.tutorial03AsmPanels[11].SetActive(false);
				this.tutorial03AsmPanels[12].SetActive(false);
				this.DefaultTutorialStepLog(StepStatuses.failed, this.backSteps);
				this.revertStep = true;
				this.Tutorial03AsmPopup10();
				return;
			}
			if (this.tutorial03AsmPanels[11].activeInHierarchy)
			{
				this.DefaultTutorialStepLog(StepStatuses.failed, 1);
				return;
			}
			this.Tutorial03AsmPopup11();
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x000A537C File Offset: 0x000A357C
		private void Tutorial03AsmPopup11()
		{
			this.tutorial03AsmPanels[10].SetActive(false);
			this.tutorial03AsmPanels[11].SetActive(true);
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[2, 3]);
			GameController.GameManager.combatManager.OnCombatStageChanged += this.Tutorial03AsmPopup12;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x000A53F0 File Offset: 0x000A35F0
		private void Tutorial03AsmPopup12(CombatStage combatStage)
		{
			GameController.UnitGetFocused -= this.Tutorial03AsmPopup11SelectionResolver;
			GameController.GameManager.combatManager.OnCombatStageChanged -= this.Tutorial03AsmPopup12;
			this.tutorial03AsmPanels[11].SetActive(false);
			this.tutorial03AsmPanels[12].SetActive(true);
			CombatPresenter.OnBattleFieldSelected += this.Tutorial03AsmPopup13;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x000A5460 File Offset: 0x000A3660
		private void Tutorial03AsmPopup13()
		{
			CombatPresenter.OnBattleFieldSelected -= this.Tutorial03AsmPopup13;
			this.tutorial03AsmPanels[12].SetActive(false);
			this.tutorial03AsmPanels[13].SetActive(true);
			this.arrowBoard[0].Hide();
			CombatPreperationPresenter.OnPowerChanged += this.Tutorial03AsmPopup14;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x000A54C4 File Offset: 0x000A36C4
		private void Tutorial03AsmPopup14(float powerValue)
		{
			if (powerValue == 3f)
			{
				CombatPreperationPresenter.OnPowerChanged -= this.Tutorial03AsmPopup14;
				this.combatPreperationPresenter.combatPanel.SetSliderInteractable(PanelSide.Left, false);
				this.tutorial03AsmPanels[13].SetActive(false);
				this.tutorial03AsmPanels[14].SetActive(true);
				CombatPreperationPresenter.OnCardAdded += this.Tutorial03AsmPopup15;
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x000A5534 File Offset: 0x000A3734
		private void Tutorial03AsmPopup15(CombatCard card, List<CombatCard> selectedCards)
		{
			int num = 0;
			foreach (CombatCard combatCard in selectedCards)
			{
				num += combatCard.CombatBonus;
			}
			if (num == 4)
			{
				CombatPreperationPresenter.OnCardAdded -= this.Tutorial03AsmPopup15;
				this.tutorial03AsmPanels[14].SetActive(false);
				this.tutorial03AsmPanels[15].SetActive(true);
				CombatPreperationPresenter.OnFightClicked += this.Tutorial03AsmPopup16;
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x000A55D4 File Offset: 0x000A37D4
		private void Tutorial03AsmPopup16()
		{
			CombatPreperationPresenter.OnFightClicked -= this.Tutorial03AsmPopup16;
			this.tutorial03AsmPanels[15].SetActive(false);
			this.tutorial03AsmPanels[16].SetActive(true);
			BattleResultPanel.OnResultAccept += this.Tutorial03AsmPopup17;
			if (PlatformManager.IsMobile)
			{
				base.StartCoroutine(this.WaitAndShowResultOKArrow());
			}
			else
			{
				BattleResultPanel.OnShowWinnerAnimationComplete += this.ShowResultOKArrow;
			}
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x00039787 File Offset: 0x00037987
		private IEnumerator WaitAndShowResultOKArrow()
		{
			yield return new WaitForSeconds(5f);
			this.ShowResultOKArrow();
			yield break;
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x00039796 File Offset: 0x00037996
		private void ShowResultOKArrow()
		{
			if (!PlatformManager.IsMobile)
			{
				BattleResultPanel.OnShowWinnerAnimationComplete -= this.ShowResultOKArrow;
			}
			this.combatResultOKArrow.SetActive(true);
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x000A5650 File Offset: 0x000A3850
		private void Tutorial03AsmPopup17()
		{
			BattleResultPanel.OnResultAccept -= this.Tutorial03AsmPopup17;
			this.tutorial03AsmPanels[16].SetActive(false);
			this.tutorial03AsmPanels[17].SetActive(true);
			this.tutorial03AsmPanels[17].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial03AsmPopup18));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x000A56B8 File Offset: 0x000A38B8
		private void Tutorial03AsmPopup18()
		{
			this.tutorial03AsmPanels[17].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup18));
			this.tutorial03AsmPanels[17].SetActive(false);
			this.tutorial03AsmPanels[18].SetActive(true);
			this.tutorial03AsmPanels[18].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial03AsmPopup19));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x000A5734 File Offset: 0x000A3934
		private void Tutorial03AsmPopup19()
		{
			this.tutorial03AsmPanels[18].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial03AsmPopup19));
			this.tutorial03AsmPanels[18].SetActive(false);
			this.tutorial03AsmPanels[19].SetActive(true);
			this.endMoveText.text = ScriptLocalization.Get("GameScene/EndMission");
			this.endTurnButton.onClick.AddListener(new UnityAction(this.Tutorial03AsmEnd));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x000A57BC File Offset: 0x000A39BC
		private void Tutorial03AsmCleanup()
		{
			AnalyticsEventData.TutorialFinished();
			AnalyticsEventData.TutorialStepStoped();
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			AnalyticsEventData.ResetTutorialStepTimer();
			this.backSteps = 0;
			this.revertStep = false;
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial03AsmEnd));
			this.tutorial03AsmPanels[19].SetActive(false);
			TutorialMissionSelection.MissionCompleted(2);
			GameController.GameManager.EndTutorial();
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x000397BC File Offset: 0x000379BC
		public void Tutorial03AsmEnd()
		{
			this.Tutorial03AsmCleanup();
			this.gameController.ExitGame();
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x000397CF File Offset: 0x000379CF
		public void Tutorial03AsmNext()
		{
			this.Tutorial03AsmCleanup();
			this.LoadTutorial(3, 0);
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x000A582C File Offset: 0x000A3A2C
		private void RemoveAllListenersOfTutorial04()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial04AsmPopup00;
			this.tutorial04AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup01));
			this.tutorial04AsmPanels[1].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup02));
			this.playerMatSections[0].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup04));
			GameController.UnitGetFocused -= this.Tutorial04AsmPopup05;
			GameController.HexGetFocused -= this.Tutorial04AsmPopup06;
			MovePresenter.MoveEnded -= this.Tutorial04AsmPopup07;
			GameController.Instance.factoryCardPresenter.OnFactoryCardChoosen -= this.Tutorial04AsmPopup08;
			this.tutorial04AsmPanels[8].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup09));
			this.endTurnButton.onClick.RemoveListener(new UnityAction(this.CloseTutorial04AsmPopup09));
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial04AsmPopup10;
			this.factoryMatSection.topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup12));
			this.tutorial04AsmPanels[12].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup13));
			this.factoryMatSection.downActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup15));
			GameController.UnitGetFocused -= this.Tutorial04AsmPopup16;
			this.tutorial04AsmPanels[16].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup17));
			this.tutorial04AsmPanels[17].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup18));
			UnitPresenter.UnitStatusChanged -= this.Tutorial04AsmPopup19;
			EncounterCardPresenter.OnRevealedCard -= this.Tutorial04AsmPopup20;
			EncounterCardPresenter.encounterEnd -= this.Tutorial04AsmPopup21;
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial04AsmEnd));
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x000A5A8C File Offset: 0x000A3C8C
		private void Tutorial04AsmStart()
		{
			this.MissionProgress.enabled = true;
			this.MissionProgress.text = string.Empty;
			this.UIBlocker.SetActive(false);
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial04AsmPopup00;
			AnalyticsEventData.TutorialStart(StepIDs.tut4_00_introduction);
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x000A5AE4 File Offset: 0x000A3CE4
		private void Tutorial04AsmPopup00()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial04AsmPopup00;
			ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.factory).GetWorldPosition());
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[i, j], false);
				}
			}
			this.AllowClickOnHex(3, 4, true);
			this.tutorial04AsmPanels[0].SetActive(true);
			this.tutorial04AsmPanels[0].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial04AsmPopup01));
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x000A5BAC File Offset: 0x000A3DAC
		private void Tutorial04AsmPopup01()
		{
			this.tutorial04AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup01));
			this.tutorial04AsmPanels[0].SetActive(false);
			this.tutorial04AsmPanels[1].SetActive(true);
			this.tutorial04AsmPanels[1].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial04AsmPopup02));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x000A5C24 File Offset: 0x000A3E24
		private void Tutorial04AsmPopup02()
		{
			this.tutorial04AsmPanels[1].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup02));
			this.tutorial04AsmPanels[1].SetActive(false);
			this.tutorial04AsmPanels[2].SetActive(true);
			this.playerMatSections[0].topActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial04AsmPopup04));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x000A5CA4 File Offset: 0x000A3EA4
		private void Tutorial04AsmPopup04()
		{
			this.playerMatSections[0].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup04));
			this.tutorial04AsmPanels[2].SetActive(false);
			this.tutorial04AsmPanels[4].SetActive(true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[4, 4]);
			GameController.UnitGetFocused += this.Tutorial04AsmPopup05;
			if (!this.revertStep)
			{
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
			this.backSteps = 0;
			this.backSteps++;
			this.revertStep = false;
			if (!PlatformManager.IsMobile)
			{
				foreach (Worker worker in GameController.GameManager.PlayerCurrent.matPlayer.workers)
				{
					Collider[] array = GameController.GetUnitPresenter(worker).GetComponentsInChildren<Collider>();
					for (int i = 0; i < array.Length; i++)
					{
						array[i].enabled = false;
					}
				}
				foreach (Mech mech in GameController.GameManager.PlayerCurrent.matFaction.mechs)
				{
					Collider[] array = GameController.GetUnitPresenter(mech).GetComponentsInChildren<Collider>();
					for (int i = 0; i < array.Length; i++)
					{
						array[i].enabled = false;
					}
				}
			}
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x000A5E48 File Offset: 0x000A4048
		private void Tutorial04AsmPopup05(UnitPresenter presenter)
		{
			if (presenter.UnitLogic.UnitType == UnitType.Character)
			{
				if (!this.tutorial04AsmPanels[4].activeInHierarchy)
				{
					this.DefaultTutorialStepLog(StepStatuses.failed, 1);
					return;
				}
				this.tutorial04AsmPanels[4].SetActive(false);
				this.tutorial04AsmPanels[5].SetActive(true);
				this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[3, 4]);
				GameController.HexGetFocused += this.Tutorial04AsmPopup06;
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
				this.backSteps++;
				return;
			}
			else
			{
				if (this.tutorial04AsmPanels[6].activeInHierarchy)
				{
					this.DefaultTutorialStepLog(StepStatuses.failed, 1);
					return;
				}
				GameController.UnitGetFocused -= this.Tutorial04AsmPopup05;
				this.DefaultTutorialStepLog(StepStatuses.failed, this.backSteps);
				this.tutorial04AsmPanels[5].SetActive(false);
				this.tutorial04AsmPanels[6].SetActive(false);
				this.revertStep = true;
				this.Tutorial04AsmPopup04();
				return;
			}
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x000A5F40 File Offset: 0x000A4140
		private void Tutorial04AsmPopup06(Scythe.BoardPresenter.GameHexPresenter presenter)
		{
			if (presenter.GetGameHexLogic().posX == 3 && presenter.GetGameHexLogic().posY == 4 && !this.tutorial04AsmPanels[6].activeInHierarchy)
			{
				GameController.HexGetFocused -= this.Tutorial04AsmPopup06;
				this.tutorial04AsmPanels[5].SetActive(false);
				this.tutorial04AsmPanels[6].SetActive(true);
				this.arrowBoard[0].Hide();
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[i, j], true);
					}
				}
				MovePresenter.MoveEnded += this.Tutorial04AsmPopup07;
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
				this.backSteps++;
			}
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x000A6018 File Offset: 0x000A4218
		private void Tutorial04AsmPopup07()
		{
			GameController.UnitGetFocused -= this.Tutorial04AsmPopup05;
			MovePresenter.MoveEnded -= this.Tutorial04AsmPopup07;
			this.tutorial04AsmPanels[6].SetActive(false);
			this.tutorial04AsmPanels[7].SetActive(true);
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[i, j], false);
				}
			}
			GameController.Instance.factoryCardPresenter.OnFactoryCardChoosen += this.Tutorial04AsmPopup08;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x000A60BC File Offset: 0x000A42BC
		private void Tutorial04AsmPopup08()
		{
			GameController.Instance.factoryCardPresenter.OnFactoryCardChoosen -= this.Tutorial04AsmPopup08;
			this.tutorial04AsmPanels[7].SetActive(false);
			this.tutorial04AsmPanels[8].SetActive(true);
			this.tutorial04AsmPanels[8].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial04AsmPopup09));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x000A612C File Offset: 0x000A432C
		private void Tutorial04AsmPopup09()
		{
			this.tutorial04AsmPanels[8].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup09));
			this.tutorial04AsmPanels[8].SetActive(false);
			this.tutorial04AsmPanels[9].SetActive(true);
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial04AsmPopup10;
			this.endTurnButton.onClick.AddListener(new UnityAction(this.CloseTutorial04AsmPopup09));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x000397DF File Offset: 0x000379DF
		private void CloseTutorial04AsmPopup09()
		{
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			this.endTurnButton.onClick.RemoveListener(new UnityAction(this.CloseTutorial04AsmPopup09));
			this.tutorial04AsmPanels[9].SetActive(false);
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x000A61BC File Offset: 0x000A43BC
		private void Tutorial04AsmPopup10()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial04AsmPopup10;
			this.tutorial04AsmPanels[9].SetActive(false);
			this.tutorial04AsmPanels[10].SetActive(true);
			base.StartCoroutine(this.AddFactoryListener());
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x00039814 File Offset: 0x00037A14
		private IEnumerator AddFactoryListener()
		{
			yield return new WaitForEndOfFrame();
			this.factoryMatSection.topActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial04AsmPopup12));
			yield break;
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x000A6210 File Offset: 0x000A4410
		private void Tutorial04AsmPopup12()
		{
			this.factoryMatSection.topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup12));
			this.tutorial04AsmPanels[10].SetActive(false);
			this.tutorial04AsmPanels[12].SetActive(true);
			this.tutorial04AsmPanels[12].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial04AsmPopup13));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x000A6290 File Offset: 0x000A4490
		private void Tutorial04AsmPopup13()
		{
			this.tutorial04AsmPanels[12].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup13));
			this.tutorial04AsmPanels[12].SetActive(false);
			this.tutorial04AsmPanels[13].SetActive(true);
			this.factoryMatSection.downActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial04AsmPopup15));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x000A6310 File Offset: 0x000A4510
		private void Tutorial04AsmPopup15()
		{
			this.factoryMatSection.downActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup15));
			this.tutorial04AsmPanels[13].SetActive(false);
			this.tutorial04AsmPanels[15].SetActive(true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[3, 4]);
			GameController.UnitGetFocused += this.Tutorial04AsmPopup16;
			if (!this.revertStep)
			{
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
			this.backSteps = 0;
			this.backSteps++;
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x000A63CC File Offset: 0x000A45CC
		private void Tutorial04AsmPopup19(UnitState state, UnitPresenter unitPresenter)
		{
			if (state == UnitState.Standing)
			{
				GameController.UnitGetFocused -= this.Tutorial04AsmPopup16;
				UnitPresenter.UnitStatusChanged -= this.Tutorial04AsmPopup19;
				this.tutorial04AsmPanels[18].SetActive(false);
				this.tutorial04AsmPanels[19].SetActive(true);
				this.arrowBoard[0].Hide();
				EncounterCardPresenter.OnRevealedCard += this.Tutorial04AsmPopup20;
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x000A6444 File Offset: 0x000A4644
		private void Tutorial04AsmPopup16(UnitPresenter presenter)
		{
			if (presenter.UnitLogic.UnitType != UnitType.Character)
			{
				this.tutorial04AsmPanels[15].SetActive(false);
				this.tutorial04AsmPanels[16].SetActive(false);
				this.tutorial04AsmPanels[17].SetActive(false);
				this.tutorial04AsmPanels[18].SetActive(false);
				this.tutorial04AsmPanels[19].SetActive(false);
				GameController.UnitGetFocused -= this.Tutorial04AsmPopup16;
				this.tutorial04AsmPanels[16].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup17));
				this.tutorial04AsmPanels[17].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup18));
				UnitPresenter.UnitStatusChanged -= this.Tutorial04AsmPopup19;
				this.DefaultTutorialStepLog(StepStatuses.failed, this.backSteps);
				this.revertStep = true;
				this.AllowClickOnHex(2, 6, false);
				this.Tutorial04AsmPopup15();
				return;
			}
			if (!this.tutorial04AsmPanels[15].activeInHierarchy)
			{
				Debug.Log("Step failed");
				this.DefaultTutorialStepLog(StepStatuses.failed, 1);
				return;
			}
			this.tutorial04AsmPanels[15].SetActive(false);
			this.tutorial04AsmPanels[16].SetActive(true);
			this.arrowBoard[0].Hide();
			this.tutorial04AsmPanels[16].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial04AsmPopup17));
			ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.hexMap[2, 6]).GetWorldPosition());
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			this.backSteps++;
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x000A65F8 File Offset: 0x000A47F8
		private void Tutorial04AsmPopup17()
		{
			this.tutorial04AsmPanels[16].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup17));
			this.tutorial04AsmPanels[16].SetActive(false);
			this.tutorial04AsmPanels[17].SetActive(true);
			this.tutorial04AsmPanels[17].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial04AsmPopup18));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			this.backSteps++;
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x000A6684 File Offset: 0x000A4884
		private void Tutorial04AsmPopup18()
		{
			this.tutorial04AsmPanels[17].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup18));
			ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.hexMap[2, 6]).GetWorldPosition());
			this.AllowClickOnHex(2, 6, true);
			this.tutorial04AsmPanels[17].SetActive(false);
			this.tutorial04AsmPanels[18].SetActive(true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[2, 6]);
			UnitPresenter.UnitStatusChanged += this.Tutorial04AsmPopup19;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			this.backSteps++;
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x000A63CC File Offset: 0x000A45CC
		private void Tutorial04AsmPopup19(UnitState state)
		{
			if (state == UnitState.Standing)
			{
				GameController.UnitGetFocused -= this.Tutorial04AsmPopup16;
				UnitPresenter.UnitStatusChanged -= this.Tutorial04AsmPopup19;
				this.tutorial04AsmPanels[18].SetActive(false);
				this.tutorial04AsmPanels[19].SetActive(true);
				this.arrowBoard[0].Hide();
				EncounterCardPresenter.OnRevealedCard += this.Tutorial04AsmPopup20;
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x000A6768 File Offset: 0x000A4968
		private void Tutorial04AsmPopup20()
		{
			EncounterCardPresenter.OnRevealedCard -= this.Tutorial04AsmPopup20;
			this.tutorial04AsmPanels[19].SetActive(false);
			this.tutorial04AsmPanels[20].SetActive(true);
			this.tutorial04AsmPanels[20].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial04AsmPopup21));
			EncounterCardPresenter.encounterEnd += this.Tutorial04AsmPopup22;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x00039823 File Offset: 0x00037A23
		private void Tutorial04AsmPopup21()
		{
			this.tutorial04AsmPanels[20].SetActive(false);
			this.tutorial04AsmPanels[21].SetActive(true);
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x000A67E4 File Offset: 0x000A49E4
		private void Tutorial04AsmPopup22()
		{
			EncounterCardPresenter.encounterEnd -= this.Tutorial04AsmPopup21;
			this.tutorial04AsmPanels[20].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial04AsmPopup21));
			this.tutorial04AsmPanels[20].SetActive(false);
			this.tutorial04AsmPanels[21].SetActive(false);
			this.tutorial04AsmPanels[22].SetActive(true);
			this.endMoveText.text = ScriptLocalization.Get("GameScene/EndMission");
			this.endTurnButton.onClick.AddListener(new UnityAction(this.Tutorial04AsmEnd));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x000A688C File Offset: 0x000A4A8C
		private void Tutorial04AsmCleanup()
		{
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial04AsmEnd));
			this.tutorial04AsmPanels[21].SetActive(false);
			AnalyticsEventData.TutorialFinished();
			AnalyticsEventData.TutorialStepStoped();
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			AnalyticsEventData.ResetTutorialStepTimer();
			this.backSteps = 0;
			this.revertStep = false;
			TutorialMissionSelection.MissionCompleted(3);
			GameController.GameManager.EndTutorial();
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x0003984B File Offset: 0x00037A4B
		public void Tutorial04AsmEnd()
		{
			this.Tutorial04AsmCleanup();
			this.gameController.ExitGame();
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x0003985E File Offset: 0x00037A5E
		public void Tutorial04AsmNext()
		{
			this.Tutorial04AsmCleanup();
			this.LoadTutorial(4, 0);
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x000A68FC File Offset: 0x000A4AFC
		private void RemoveAllListenersOfTutorial05()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial05AsmPopup01;
			this.tutorial05AsmPanels[1].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup02));
			this.tutorial05AsmPanels[3].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup04));
			this.tutorial05AsmPanels[4].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup05));
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup06));
			ProducePresenter.OnHexProductionClicked -= this.Tutorial05AsmPopup07;
			this.tutorial05AsmPanels[7].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup08));
			ProducePresenter.OnHexProductionClicked -= this.Tutorial05AsmPopup09;
			this.endTurnButton.onClick.RemoveListener(new UnityAction(this.CloseTutorial05AsmPopup09));
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial05AsmPopup10;
			this.playerMatSections[1].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup12));
			TradePresenter.OnResourceSelected -= this.Tutorial05AsmPopup13;
			TradePresenter.OnResourceSelected -= this.Tutorial05AsmPopup14;
			this.tutorial05AsmPanels[14].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup15));
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial05AsmEnd));
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x000A6AC4 File Offset: 0x000A4CC4
		private void Tutorial05AsmStart()
		{
			this.MissionProgress.enabled = true;
			this.MissionProgress.text = string.Empty;
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial05AsmPopup01;
			AnalyticsEventData.TutorialStart(StepIDs.tut5_00_introduction);
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x000A6B10 File Offset: 0x000A4D10
		private void Tutorial05AsmPopup01()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial05AsmPopup01;
			this.tutorial05AsmPanels[1].SetActive(true);
			this.tutorial05AsmPanels[1].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial05AsmPopup02));
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x000A6B6C File Offset: 0x000A4D6C
		private void Tutorial05AsmPopup02()
		{
			this.tutorial05AsmPanels[1].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup02));
			this.tutorial05AsmPanels[1].SetActive(false);
			this.tutorial05AsmPanels[2].SetActive(true);
			this.tutorial05AsmPanels[2].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial05AsmPopup04));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x000A6BE4 File Offset: 0x000A4DE4
		private void Tutorial05AsmPopup04()
		{
			this.tutorial05AsmPanels[2].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup04));
			this.tutorial05AsmPanels[2].SetActive(false);
			this.tutorial05AsmPanels[4].SetActive(true);
			this.tutorial05AsmPanels[4].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial05AsmPopup05));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x000A6C5C File Offset: 0x000A4E5C
		private void Tutorial05AsmPopup05()
		{
			this.tutorial05AsmPanels[4].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup05));
			this.tutorial05AsmPanels[4].SetActive(false);
			this.tutorial05AsmPanels[5].SetActive(true);
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial05AsmPopup06));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x000A6CDC File Offset: 0x000A4EDC
		private void Tutorial05AsmPopup06()
		{
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup06));
			this.tutorial05AsmPanels[5].SetActive(false);
			this.tutorial05AsmPanels[6].SetActive(true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[4, 2]);
			ProducePresenter producePresenter = (ProducePresenter)HumanInputHandler.Instance.producePresenter;
			ProductionGridPresenter[] componentsInChildren = producePresenter.GridClipboard.GetComponentsInChildren<ProductionGridPresenter>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].AttachedHex.hexType != HexType.mountain)
				{
					componentsInChildren[i].produceButton.interactable = false;
				}
			}
			Toggle[] componentsInChildren2 = producePresenter.GridClipboard.GetComponentsInChildren<Toggle>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].interactable = false;
			}
			ProducePresenter.OnHexProductionClicked += this.Tutorial05AsmPopup07;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x000A6DE8 File Offset: 0x000A4FE8
		private void Tutorial05AsmPopup07(Scythe.BoardPresenter.GameHexPresenter hex, int amount)
		{
			if (hex.GetGameHexLogic().posX == 4 && hex.GetGameHexLogic().posY == 2)
			{
				ProducePresenter.OnHexProductionClicked -= this.Tutorial05AsmPopup07;
				this.tutorial05AsmPanels[6].SetActive(false);
				this.tutorial05AsmPanels[7].SetActive(true);
				this.arrowBoard[0].Hide();
				this.tutorial05AsmPanels[7].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial05AsmPopup08));
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x000A6E78 File Offset: 0x000A5078
		private void Tutorial05AsmPopup08()
		{
			this.tutorial05AsmPanels[7].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup08));
			this.tutorial05AsmPanels[7].SetActive(false);
			this.tutorial05AsmPanels[8].SetActive(true);
			ProducePresenter.OnHexProductionClicked += this.Tutorial05AsmPopup09;
			ProductionGridPresenter[] componentsInChildren = ((ProducePresenter)HumanInputHandler.Instance.producePresenter).GridClipboard.GetComponentsInChildren<ProductionGridPresenter>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].produceButton.interactable = true;
			}
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x000A6F14 File Offset: 0x000A5114
		private void Tutorial05AsmPopup09(Scythe.BoardPresenter.GameHexPresenter hex, int amount)
		{
			ProducePresenter.OnHexProductionClicked -= this.Tutorial05AsmPopup09;
			this.tutorial05AsmPanels[8].SetActive(false);
			this.tutorial05AsmPanels[9].SetActive(true);
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial05AsmPopup10;
			this.endTurnButton.onClick.AddListener(new UnityAction(this.CloseTutorial05AsmPopup09));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x0003986E File Offset: 0x00037A6E
		private void CloseTutorial05AsmPopup09()
		{
			this.endTurnButton.onClick.RemoveListener(new UnityAction(this.CloseTutorial05AsmPopup09));
			this.tutorial05AsmPanels[9].SetActive(false);
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x000A6F90 File Offset: 0x000A5190
		private void Tutorial05AsmPopup10()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial05AsmPopup10;
			this.tutorial05AsmPanels[9].SetActive(false);
			this.tutorial05AsmPanels[10].SetActive(true);
			this.playerMatSections[1].topActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial05AsmPopup12));
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x000A7004 File Offset: 0x000A5204
		private void Tutorial05AsmPopup12()
		{
			this.playerMatSections[1].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup12));
			this.tutorial05AsmPanels[10].SetActive(false);
			this.tutorial05AsmPanels[12].SetActive(true);
			TradePresenter.OnResourceSelected += this.Tutorial05AsmPopup13;
			TradeButtonsPanel[] componentsInChildren = ((TradePresenter)HumanInputHandler.Instance.tradePresenter).tradeButtonsMain.GetComponentsInChildren<TradeButtonsPanel>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ResourceButtons.transform.GetChild(0).GetComponent<Button>().interactable = false;
				componentsInChildren[i].ResourceButtons.transform.GetChild(1).GetComponent<Button>().interactable = false;
				componentsInChildren[i].ResourceButtons.transform.GetChild(2).GetComponent<Button>().interactable = false;
				componentsInChildren[i].ButtonHooverSFXDisable(componentsInChildren[i].ResourceButtons.transform.GetChild(0));
				componentsInChildren[i].ButtonHooverSFXDisable(componentsInChildren[i].ResourceButtons.transform.GetChild(1));
				componentsInChildren[i].ButtonHooverSFXDisable(componentsInChildren[i].ResourceButtons.transform.GetChild(2));
			}
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x000A714C File Offset: 0x000A534C
		private void Tutorial05AsmPopup13(ResourceType resource, Scythe.BoardPresenter.GameHexPresenter hex)
		{
			TradePresenter.OnResourceSelected -= this.Tutorial05AsmPopup13;
			this.tutorial05AsmPanels[12].SetActive(false);
			this.tutorial05AsmPanels[13].SetActive(true);
			TradePresenter.OnResourceSelected += this.Tutorial05AsmPopup14;
			TradeButtonsPanel[] componentsInChildren = ((TradePresenter)HumanInputHandler.Instance.tradePresenter).tradeButtonsMain.GetComponentsInChildren<TradeButtonsPanel>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ResourceButtons.transform.GetChild(0).GetComponent<Button>().interactable = true;
				componentsInChildren[i].ResourceButtons.transform.GetChild(1).GetComponent<Button>().interactable = true;
				componentsInChildren[i].ResourceButtons.transform.GetChild(2).GetComponent<Button>().interactable = true;
			}
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x000A7224 File Offset: 0x000A5424
		private void Tutorial05AsmPopup14(ResourceType resource, Scythe.BoardPresenter.GameHexPresenter hex)
		{
			TradePresenter.OnResourceSelected -= this.Tutorial05AsmPopup14;
			this.tutorial05AsmPanels[13].SetActive(false);
			this.tutorial05AsmPanels[14].SetActive(true);
			this.tutorial05AsmPanels[14].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial05AsmPopup15));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x000A728C File Offset: 0x000A548C
		private void Tutorial05AsmPopup15()
		{
			this.tutorial05AsmPanels[14].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial05AsmPopup15));
			this.tutorial05AsmPanels[14].SetActive(false);
			this.tutorial05AsmPanels[15].SetActive(true);
			this.endMoveText.text = ScriptLocalization.Get("GameScene/EndMission");
			this.endTurnButton.onClick.AddListener(new UnityAction(this.Tutorial05AsmEnd));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x000A7314 File Offset: 0x000A5514
		private void Tutorial05AsmCleanup()
		{
			AnalyticsEventData.TutorialFinished();
			AnalyticsEventData.TutorialStepStoped();
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			AnalyticsEventData.ResetTutorialStepTimer();
			this.backSteps = 0;
			this.revertStep = false;
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial05AsmEnd));
			this.tutorial05AsmPanels[15].SetActive(false);
			TutorialMissionSelection.MissionCompleted(4);
			GameController.GameManager.EndTutorial();
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x000398A3 File Offset: 0x00037AA3
		public void Tutorial05AsmEnd()
		{
			this.Tutorial05AsmCleanup();
			this.gameController.ExitGame();
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x000398B6 File Offset: 0x00037AB6
		public void Tutorial05AsmNext()
		{
			this.Tutorial05AsmCleanup();
			this.LoadTutorial(5, 0);
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x000A7384 File Offset: 0x000A5584
		private void RemoveAllListenersOfTutorial06()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial06AsmPopup00;
			this.tutorial06AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial06AsmPopup01));
			this.playerMatSections[0].downActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial06AsmPopup04));
			PayResourcePresenter.PayResourceEnded -= this.Tutorial06AsmPopup05;
			this.playerMatSections[2].topActionPresenter.upgradeListeners[0].RemoveListener(new UnityAction(this.Tutorial06AsmPopup06));
			this.playerMatSections[1].downActionPresenter.upgradeListeners[0].RemoveListener(new UnityAction(this.Tutorial06AsmPopup07));
			this.tutorial06AsmPanels[7].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial06AsmPopup10));
			this.endTurnButtonFake.onClick.RemoveAllListeners();
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x000A7488 File Offset: 0x000A5688
		private void Tutorial06AsmStart()
		{
			this.MissionProgress.enabled = true;
			this.MissionProgress.text = string.Empty;
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial06AsmPopup00;
			AnalyticsEventData.TutorialStart(StepIDs.tut6_00_introduction);
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x000A74D4 File Offset: 0x000A56D4
		private void Tutorial06AsmPopup00()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial06AsmPopup00;
			this.tutorial06AsmPanels[0].SetActive(true);
			this.tutorial06AsmPanels[0].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial06AsmPopup01));
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x000A7530 File Offset: 0x000A5730
		private void Tutorial06AsmPopup01()
		{
			this.tutorial06AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial06AsmPopup01));
			this.tutorial06AsmPanels[0].SetActive(false);
			this.tutorial06AsmPanels[1].SetActive(true);
			this.playerMatSections[0].downActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial06AsmPopup04));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x000A75B0 File Offset: 0x000A57B0
		private void Tutorial06AsmPopup04()
		{
			this.playerMatSections[0].downActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial06AsmPopup04));
			this.gameController.matPlayer.topActionAvailable.OnYesClick -= this.Tutorial06AsmPopup04;
			this.tutorial06AsmPanels[1].SetActive(false);
			this.tutorial06AsmPanels[4].SetActive(true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[1, 7]);
			PayResourcePresenter.PayResourceEnded += this.Tutorial06AsmPopup05;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x000A7670 File Offset: 0x000A5870
		private void Tutorial06AsmPopup05()
		{
			PayResourcePresenter.PayResourceEnded -= this.Tutorial06AsmPopup05;
			this.tutorial06AsmPanels[4].SetActive(false);
			this.tutorial06AsmPanels[5].SetActive(true);
			this.arrowBoard[0].Hide();
			this.playerMatSections[2].topActionPresenter.upgradeListeners[0].AddListener(new UnityAction(this.Tutorial06AsmPopup06));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x000A76E4 File Offset: 0x000A58E4
		private void Tutorial06AsmPopup06()
		{
			this.playerMatSections[2].topActionPresenter.upgradeListeners[0].RemoveListener(new UnityAction(this.Tutorial06AsmPopup06));
			this.tutorial06AsmPanels[5].SetActive(false);
			this.tutorial06AsmPanels[6].SetActive(true);
			this.playerMatSections[1].downActionPresenter.upgradeListeners[0].AddListener(new UnityAction(this.Tutorial06AsmPopup07));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x000A7760 File Offset: 0x000A5960
		private void Tutorial06AsmPopup07()
		{
			this.playerMatSections[1].downActionPresenter.upgradeListeners[0].RemoveListener(new UnityAction(this.Tutorial06AsmPopup07));
			this.tutorial06AsmPanels[6].SetActive(false);
			this.tutorial06AsmPanels[7].SetActive(true);
			this.tutorial06AsmPanels[7].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial06AsmPopup10));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x000A77DC File Offset: 0x000A59DC
		private void Tutorial06AsmPopup10()
		{
			this.tutorial06AsmPanels[7].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial06AsmPopup10));
			this.tutorial06AsmPanels[7].SetActive(false);
			this.tutorial06AsmPanels[9].SetActive(true);
			this.endTurnButton.gameObject.SetActive(false);
			this.endTurnButtonFake.gameObject.SetActive(true);
			this.endTurnButtonFake.onClick.RemoveAllListeners();
			this.gameController.endTurnHintButton.SetActive(false);
			this.endTurnFakeText.text = ScriptLocalization.Get("GameScene/EndMission");
			this.endTurnButtonFake.onClick.AddListener(new UnityAction(this.Tutorial06AsmEnd));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x000A78A8 File Offset: 0x000A5AA8
		private void Tutorial06AsmCleanup()
		{
			AnalyticsEventData.TutorialFinished();
			AnalyticsEventData.TutorialStepStoped();
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			AnalyticsEventData.ResetTutorialStepTimer();
			this.backSteps = 0;
			this.revertStep = false;
			this.endTurnButton.onClick.RemoveListener(new UnityAction(this.Tutorial06AsmEnd));
			this.tutorial06AsmPanels[9].SetActive(false);
			TutorialMissionSelection.MissionCompleted(5);
			GameController.GameManager.EndTutorial();
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x000398C6 File Offset: 0x00037AC6
		public void Tutorial06AsmEnd()
		{
			this.Tutorial06AsmCleanup();
			this.gameController.ExitGame();
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x000398D9 File Offset: 0x00037AD9
		public void Tutorial06AsmNext()
		{
			this.Tutorial06AsmCleanup();
			this.LoadTutorial(6, 0);
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x000A7918 File Offset: 0x000A5B18
		private void RemoveAllListenersOfTutorial07()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial07AsmPopup00;
			this.tutorial07AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial07AsmPopup01));
			this.playerMatSections[1].downActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial07AsmPopup04));
			GameController.HexGetFocused -= this.Tutorial07AsmPopup05;
			if (this.mechsAbilities[0] != null)
			{
				this.mechsAbilities[0].onClick.RemoveListener(new UnityAction(this.Tutorial07AsmPopup06));
			}
			DeployPresenter.DeployEnded -= this.Tutorial07AsmPopup07;
			this.endTurnButton.onClick.RemoveListener(new UnityAction(this.CloseTutorial07AsmPopup07));
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial07AsmPopup08;
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial07AsmPopup10));
			GameController.UnitGetFocused -= this.Tutorial07AsmPopup11;
			GameController.HexGetFocused -= this.Tutorial07AsmPopup12;
			GameController.UnitGetFocused -= this.Tutorial07AsmPopup13;
			GameController.UnitGetFocused -= this.Tutorial07AsmPopup14;
			GameController.UnitGetFocused -= this.Tutorial07AsmPopup15;
			MovePresenter.MoveEnded -= this.Tutorial07AsmPopup16;
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial07AsmEnd));
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x000398E9 File Offset: 0x00037AE9
		private void Tutorial07AsmStart()
		{
			this.MissionProgress.enabled = true;
			this.MissionProgress.text = string.Empty;
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial07AsmPopup00;
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x000A7AC4 File Offset: 0x000A5CC4
		private void Tutorial07AsmPopup00()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial07AsmPopup00;
			this.tutorial07AsmPanels[0].SetActive(true);
			this.tutorial07AsmPanels[0].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial07AsmPopup01));
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x000A7B20 File Offset: 0x000A5D20
		private void Tutorial07AsmPopup01()
		{
			this.tutorial07AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial07AsmPopup01));
			this.tutorial07AsmPanels[0].SetActive(false);
			this.tutorial07AsmPanels[1].SetActive(true);
			this.playerMatSections[1].downActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial07AsmPopup04));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x000A7BA0 File Offset: 0x000A5DA0
		private void Tutorial07AsmPopup04()
		{
			this.playerMatSections[1].downActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial07AsmPopup04));
			if (this.tutorial07AsmPanels[2].activeInHierarchy)
			{
				this.DefaultTutorialStepLog(StepStatuses.completed, -1);
			}
			else
			{
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
			this.tutorial07AsmPanels[1].SetActive(false);
			this.tutorial07AsmPanels[4].SetActive(true);
			GameController.HexGetFocused += this.Tutorial07AsmPopup05;
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x000A7C28 File Offset: 0x000A5E28
		private void Tutorial07AsmPopup05(Scythe.BoardPresenter.GameHexPresenter presenter)
		{
			if (GameController.GameManager.PlayerCurrent.Resources(false)[ResourceType.metal] == 0)
			{
				GameController.HexGetFocused -= this.Tutorial07AsmPopup05;
				this.tutorial07AsmPanels[4].SetActive(false);
				this.tutorial07AsmPanels[5].SetActive(true);
				this.mechRiverwalkOutline.SetActive(true);
				Button[] array = this.mechButtonDisable;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].interactable = false;
				}
				if (this.mechsAbilities[0] != null)
				{
					this.mechsAbilities[0].onClick.AddListener(new UnityAction(this.Tutorial07AsmPopup06));
				}
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x000A7CE0 File Offset: 0x000A5EE0
		private void Tutorial07AsmPopup06()
		{
			if (this.mechsAbilities[0] != null)
			{
				this.mechsAbilities[0].onClick.RemoveListener(new UnityAction(this.Tutorial07AsmPopup06));
			}
			this.tutorial07AsmPanels[5].SetActive(false);
			this.tutorial07AsmPanels[6].SetActive(true);
			this.mechRiverwalkOutline.SetActive(false);
			Button[] array = this.mechButtonDisable;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].interactable = false;
			}
			DeployPresenter.DeployEnded += this.Tutorial07AsmPopup07;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x000A7D7C File Offset: 0x000A5F7C
		private void Tutorial07AsmPopup07()
		{
			DeployPresenter.DeployEnded -= this.Tutorial07AsmPopup07;
			this.tutorial07AsmPanels[6].SetActive(false);
			this.tutorial07AsmPanels[7].SetActive(true);
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial07AsmPopup08;
			this.endTurnButton.onClick.AddListener(new UnityAction(this.CloseTutorial07AsmPopup07));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x00039923 File Offset: 0x00037B23
		private void CloseTutorial07AsmPopup07()
		{
			this.endTurnButton.onClick.RemoveListener(new UnityAction(this.CloseTutorial07AsmPopup07));
			this.tutorial07AsmPanels[7].SetActive(false);
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x000A7DF8 File Offset: 0x000A5FF8
		private void Tutorial07AsmPopup08()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial07AsmPopup08;
			this.tutorial07AsmPanels[8].SetActive(true);
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial07AsmPopup10));
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x000A7E5C File Offset: 0x000A605C
		private void Tutorial07AsmPopup10()
		{
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial07AsmPopup10));
			this.tutorial07AsmPanels[8].SetActive(false);
			this.tutorial07AsmPanels[10].SetActive(true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[0, 6]);
			foreach (GameHex gameHex in new GameHex[]
			{
				GameController.GameManager.gameBoard.hexMap[0, 5],
				GameController.GameManager.gameBoard.hexMap[1, 5],
				GameController.GameManager.gameBoard.hexMap[2, 5],
				GameController.GameManager.gameBoard.hexMap[2, 6],
				GameController.GameManager.gameBoard.hexMap[2, 7],
				GameController.GameManager.gameBoard.hexMap[2, 5],
				GameController.GameManager.gameBoard.hexMap[1, 7],
				GameController.GameManager.gameBoard.hexMap[0, 6],
				GameController.GameManager.gameBoard.hexMap[1, 6]
			})
			{
				this.AllowClickOnHex(gameHex, false);
			}
			GameController.UnitGetFocused += this.Tutorial07AsmPopup11;
			if (!this.revertStep)
			{
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
			this.backSteps = 0;
			this.backSteps++;
			this.revertStep = false;
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x000A8020 File Offset: 0x000A6220
		private void Tutorial07AsmPopup11(UnitPresenter presenter)
		{
			if (presenter.UnitLogic.UnitType != UnitType.Mech)
			{
				GameController.UnitGetFocused -= this.Tutorial07AsmPopup11;
				GameController.HexGetFocused -= this.Tutorial07AsmPopup12;
				this.tutorial07AsmPanels[11].SetActive(false);
				this.tutorial07AsmPanels[12].SetActive(false);
				this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[0, 5], false);
				this.DefaultTutorialStepLog(StepStatuses.failed, this.backSteps);
				this.revertStep = true;
				this.Tutorial07AsmPopup10();
				return;
			}
			if (!this.tutorial07AsmPanels[10].activeInHierarchy)
			{
				this.DefaultTutorialStepLog(StepStatuses.failed, 1);
				return;
			}
			this.tutorial07AsmPanels[10].SetActive(false);
			this.tutorial07AsmPanels[11].SetActive(true);
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[0, 5]);
			this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[0, 5], true);
			GameController.HexGetFocused += this.Tutorial07AsmPopup12;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			this.backSteps++;
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x000A8154 File Offset: 0x000A6354
		private void Tutorial07AsmPopup12(Scythe.BoardPresenter.GameHexPresenter presenter)
		{
			if (presenter.GetGameHexLogic().posX == 0 && presenter.GetGameHexLogic().posY == 5)
			{
				GameController.UnitGetFocused -= this.Tutorial07AsmPopup11;
				GameController.HexGetFocused -= this.Tutorial07AsmPopup12;
				this.tutorial07AsmPanels[11].SetActive(false);
				this.tutorial07AsmPanels[13].SetActive(true);
				this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[1, 6]);
				this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[0, 5], false);
				GameController.UnitGetFocused += this.Tutorial07AsmPopup15;
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x000A821C File Offset: 0x000A641C
		private void Tutorial07AsmPopup13(UnitPresenter presenter)
		{
			if (presenter.UnitLogic.UnitType == UnitType.Mech)
			{
				GameController.UnitGetFocused -= this.Tutorial07AsmPopup13;
				this.tutorial07AsmPanels[12].SetActive(false);
				this.tutorial07AsmPanels[13].SetActive(true);
				this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[1, 6]);
				GameController.UnitGetFocused += this.Tutorial07AsmPopup14;
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
				return;
			}
			GameController.UnitGetFocused -= this.Tutorial07AsmPopup14;
			this.tutorial07AsmPanels[12].SetActive(true);
			this.tutorial07AsmPanels[13].SetActive(false);
			this.DefaultTutorialStepLog(StepStatuses.failed, 1);
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x000A82DC File Offset: 0x000A64DC
		private void Tutorial07AsmPopup14(UnitPresenter presenter)
		{
			if (presenter.UnitLogic.UnitType == UnitType.Character)
			{
				GameController.UnitGetFocused -= this.Tutorial07AsmPopup14;
				this.tutorial07AsmPanels[13].SetActive(false);
				this.tutorial07AsmPanels[14].SetActive(true);
				this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[0, 6]);
				GameController.UnitGetFocused += this.Tutorial07AsmPopup15;
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
				return;
			}
			this.DefaultTutorialStepLog(StepStatuses.failed, 1);
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x000A836C File Offset: 0x000A656C
		private void Tutorial07AsmPopup15(UnitPresenter presenter)
		{
			if (this.tutorial07AsmPanels[15].activeInHierarchy)
			{
				this.DefaultTutorialStepLog(StepStatuses.failed, 1);
				return;
			}
			if (presenter.UnitLogic.UnitType == UnitType.Character)
			{
				this.tutorial07AsmPanels[13].SetActive(false);
				this.tutorial07AsmPanels[14].SetActive(false);
				this.tutorial07AsmPanels[15].SetActive(true);
				this.arrowBoard[0].Hide();
				MovePresenter.MoveEnded += this.Tutorial07AsmPopup16;
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 9; j++)
					{
						this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[i, j], true);
					}
				}
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
				return;
			}
			this.DefaultTutorialStepLog(StepStatuses.failed, 1);
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x000A8434 File Offset: 0x000A6634
		private void Tutorial07AsmPopup16()
		{
			GameController.UnitGetFocused -= this.Tutorial07AsmPopup15;
			MovePresenter.MoveEnded -= this.Tutorial07AsmPopup16;
			this.tutorial07AsmPanels[15].SetActive(false);
			this.tutorial07AsmPanels[16].SetActive(true);
			this.endMoveText.text = ScriptLocalization.Get("GameScene/EndMission");
			this.endTurnButton.onClick.AddListener(new UnityAction(this.Tutorial07AsmEnd));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x000A84BC File Offset: 0x000A66BC
		private void Tutorial07AsmCleanup()
		{
			AnalyticsEventData.TutorialFinished();
			AnalyticsEventData.TutorialStepStoped();
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			AnalyticsEventData.ResetTutorialStepTimer();
			this.backSteps = 0;
			this.revertStep = false;
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial07AsmEnd));
			this.tutorial07AsmPanels[16].SetActive(false);
			TutorialMissionSelection.MissionCompleted(6);
			GameController.GameManager.EndTutorial();
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x00039957 File Offset: 0x00037B57
		public void Tutorial07AsmEnd()
		{
			this.Tutorial07AsmCleanup();
			this.gameController.ExitGame();
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x0003996A File Offset: 0x00037B6A
		public void Tutorial07AsmNext()
		{
			this.Tutorial07AsmCleanup();
			this.LoadTutorial(7, 0);
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void RemoveAllListenersOfTutorial08()
		{
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x000A852C File Offset: 0x000A672C
		private void Tutorial08AsmStart()
		{
			this.MissionProgress.enabled = true;
			this.MissionProgress.text = string.Empty;
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial08AsmPopup00;
			AnalyticsEventData.TutorialStart(StepIDs.tut8_00_introduction);
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x000A8578 File Offset: 0x000A6778
		private void Tutorial08AsmPopup00()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial08AsmPopup00;
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[i, j], false);
				}
			}
			this.tutorial08AsmPanels[0].SetActive(true);
			this.tutorial08AsmPanels[0].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial08AsmPopup01));
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x000A8608 File Offset: 0x000A6808
		private void Tutorial08AsmPopup01()
		{
			this.tutorial08AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial08AsmPopup01));
			this.tutorial08AsmPanels[0].SetActive(false);
			this.tutorial08AsmPanels[1].SetActive(true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[1, 7]);
			this.arrowBoard[1].Show();
			this.arrowBoard[1].SetPosition(GameController.GameManager.gameBoard.hexMap[0, 6]);
			ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.hexMap[1, 7]).GetWorldPosition());
			this.tutorial08AsmPanels[1].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial08AsmPopup02));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x000A8714 File Offset: 0x000A6914
		private void Tutorial08AsmPopup02()
		{
			this.tutorial08AsmPanels[1].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial08AsmPopup02));
			this.tutorial08AsmPanels[1].SetActive(false);
			this.tutorial08AsmPanels[2].SetActive(true);
			this.arrowBoard[0].Hide();
			this.arrowBoard[1].Hide();
			this.playerMatSections[0].topActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial08AsmPopup04));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x000A87AC File Offset: 0x000A69AC
		private void Tutorial08AsmPopup04()
		{
			this.playerMatSections[0].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial08AsmPopup04));
			this.tutorial08AsmPanels[2].SetActive(false);
			this.tutorial08AsmPanels[4].SetActive(true);
			GameController.UnitGetFocused += this.Tutorial08AsmPopup05;
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[1, 7]);
			if (!this.revertStep)
			{
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
			this.backSteps = 0;
			this.backSteps++;
			this.revertStep = false;
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x000A886C File Offset: 0x000A6A6C
		private void Tutorial08AsmPopup05(UnitPresenter presenter)
		{
			if (presenter.UnitLogic.UnitType != UnitType.Character)
			{
				GameController.UnitGetFocused -= this.Tutorial08AsmPopup05;
				UnitPresenter.UnitStatusChanged -= this.Tutorial08AsmPopup06;
				this.tutorial08AsmPanels[5].SetActive(false);
				this.DefaultTutorialStepLog(StepStatuses.failed, this.backSteps);
				this.revertStep = true;
				this.Tutorial08AsmPopup04();
				return;
			}
			if (!this.tutorial08AsmPanels[4].activeInHierarchy)
			{
				this.DefaultTutorialStepLog(StepStatuses.failed, 1);
				return;
			}
			this.tutorial08AsmPanels[4].SetActive(false);
			this.tutorial08AsmPanels[5].SetActive(true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[3, 6]);
			this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[3, 6], true);
			UnitPresenter.UnitStatusChanged += this.Tutorial08AsmPopup06;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			this.backSteps++;
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x000A897C File Offset: 0x000A6B7C
		private void Tutorial08AsmPopup06(UnitState state, UnitPresenter unitPresenter)
		{
			if (state == UnitState.Standing)
			{
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
				GameController.UnitGetFocused -= this.Tutorial08AsmPopup05;
				UnitPresenter.UnitStatusChanged -= this.Tutorial08AsmPopup06;
				this.arrowBoard[0].Hide();
				this.tutorial08AsmPanels[5].SetActive(false);
				this.tutorial08AsmPanels[6].SetActive(true);
				this.endMoveButton.onClick.AddListener(new UnityAction(this.Tutorial08AsmPopup07));
			}
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x000A89FC File Offset: 0x000A6BFC
		private void Tutorial08AsmPopup07()
		{
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial08AsmPopup07));
			this.tutorial08AsmPanels[6].SetActive(false);
			this.tutorial08AsmPanels[7].SetActive(true);
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial08AsmPopup08;
			this.endTurnButton.onClick.AddListener(new UnityAction(this.CloseTutorial08AsmPopup07));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x0003997A File Offset: 0x00037B7A
		private void CloseTutorial08AsmPopup07()
		{
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			this.endTurnButton.onClick.RemoveListener(new UnityAction(this.CloseTutorial08AsmPopup07));
			this.tutorial08AsmPanels[7].SetActive(false);
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x000A8A84 File Offset: 0x000A6C84
		private void Tutorial08AsmPopup08()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial08AsmPopup08;
			this.tutorial08AsmPanels[8].SetActive(true);
			this.playerMatSections[2].topActionPresenter.gainActionButton[1].onClick.AddListener(new UnityAction(this.Tutorial08AsmPopup10));
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x000A8AE8 File Offset: 0x000A6CE8
		private void Tutorial08AsmPopup10()
		{
			this.playerMatSections[2].topActionPresenter.gainActionButton[1].onClick.RemoveListener(new UnityAction(this.Tutorial08AsmPopup10));
			this.tutorial08AsmPanels[8].SetActive(false);
			this.tutorial08AsmPanels[10].SetActive(true);
			this.tutorial08AsmPanels[10].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial08AsmPopup11));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x000A8B68 File Offset: 0x000A6D68
		private void Tutorial08AsmPopup11()
		{
			this.tutorial08AsmPanels[10].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial08AsmPopup11));
			this.tutorial08AsmPanels[10].SetActive(false);
			this.tutorial08AsmPanels[11].SetActive(true);
			this.playerMatSections[2].downActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial08AsmPopup11b));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x000A8BEC File Offset: 0x000A6DEC
		private void Tutorial08AsmPopup11b()
		{
			this.playerMatSections[2].downActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial08AsmPopup11b));
			this.tutorial08AsmPanels[11].SetActive(false);
			this.tutorial08AsmPanels[3].SetActive(true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[3, 6]);
			GameController.HexGetFocused += this.Tutorial08AsmPopup12;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x000A8C8C File Offset: 0x000A6E8C
		private void Tutorial08AsmPopup12(Scythe.BoardPresenter.GameHexPresenter presenter)
		{
			if (GameController.GameManager.PlayerCurrent.Resources(false)[ResourceType.wood] == 0)
			{
				GameController.HexGetFocused -= this.Tutorial08AsmPopup12;
				this.arrowBoard[0].Hide();
				this.tutorial08AsmPanels[3].SetActive(false);
				this.tutorial08AsmPanels[12].SetActive(true);
				BuildPresenter.BuildingSelected += this.Tutorial08AsmPopup13;
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x000A8D08 File Offset: 0x000A6F08
		private void Tutorial08AsmPopup13()
		{
			BuildPresenter.BuildingSelected -= this.Tutorial08AsmPopup13;
			this.tutorial08AsmPanels[12].SetActive(false);
			this.tutorial08AsmPanels[13].SetActive(true);
			this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[1, 6], true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[1, 6]);
			BuildPresenter.BuildEnded += this.Tutorial08AsmPopup14;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x000A8DAC File Offset: 0x000A6FAC
		private void Tutorial08AsmPopup14()
		{
			BuildPresenter.BuildEnded -= this.Tutorial08AsmPopup14;
			this.arrowBoard[0].Hide();
			this.tutorial08AsmPanels[13].SetActive(false);
			this.tutorial08AsmPanels[14].SetActive(true);
			this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[1, 6], false);
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial08AsmPopup15;
			this.endTurnButton.onClick.AddListener(new UnityAction(this.CloseTutorial08AsmPopup14));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x000399AE File Offset: 0x00037BAE
		private void CloseTutorial08AsmPopup14()
		{
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			this.endTurnButton.onClick.RemoveListener(new UnityAction(this.CloseTutorial08AsmPopup14));
			this.tutorial08AsmPanels[14].SetActive(false);
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x000A8E54 File Offset: 0x000A7054
		private void Tutorial08AsmPopup15()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial08AsmPopup15;
			this.tutorial08AsmPanels[15].SetActive(true);
			this.playerMatSections[3].topActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial08AsmPopup17));
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x000A8EB8 File Offset: 0x000A70B8
		private void Tutorial08AsmPopup17()
		{
			this.playerMatSections[3].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial08AsmPopup17));
			this.tutorial08AsmPanels[15].SetActive(false);
			this.tutorial08AsmPanels[17].SetActive(true);
			this.tutorial08AsmPanels[17].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial08AsmPopup18));
			ProducePresenter.ProductionEnded += this.Tutorial08AsmPopup19;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x000A8F4C File Offset: 0x000A714C
		private void Tutorial08AsmPopup18()
		{
			this.tutorial08AsmPanels[17].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial08AsmPopup18));
			this.tutorial08AsmPanels[17].SetActive(false);
			this.tutorial08AsmPanels[18].SetActive(true);
			ProducePresenter.ProductionEnded -= this.Tutorial08AsmPopup19;
			ProducePresenter.ProductionEnded += this.Tutorial08AsmPopup19;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x000A8FC8 File Offset: 0x000A71C8
		private void Tutorial08AsmPopup19()
		{
			ProducePresenter.ProductionEnded -= this.Tutorial08AsmPopup19;
			this.tutorial08AsmPanels[17].SetActive(false);
			this.tutorial08AsmPanels[18].SetActive(false);
			this.tutorial08AsmPanels[19].SetActive(true);
			this.endMoveText.text = ScriptLocalization.Get("GameScene/EndMission");
			this.endTurnButton.onClick.AddListener(new UnityAction(this.Tutorial08AsmEnd));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x000A904C File Offset: 0x000A724C
		private void Tutorial08AsmCleanup()
		{
			AnalyticsEventData.TutorialFinished();
			AnalyticsEventData.TutorialStepStoped();
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			AnalyticsEventData.ResetTutorialStepTimer();
			this.backSteps = 0;
			this.revertStep = false;
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial08AsmEnd));
			this.tutorial08AsmPanels[19].SetActive(false);
			TutorialMissionSelection.MissionCompleted(7);
			GameController.GameManager.EndTutorial();
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x000399E3 File Offset: 0x00037BE3
		public void Tutorial08AsmEnd()
		{
			this.Tutorial08AsmCleanup();
			this.gameController.ExitGame();
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x000399F6 File Offset: 0x00037BF6
		public void Tutorial08AsmNext()
		{
			this.Tutorial08AsmCleanup();
			this.LoadTutorial(8, 0);
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x000A90BC File Offset: 0x000A72BC
		private void RemoveAllListenersOfTutorial09()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial09AsmPopup00;
			this.tutorial09AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial09AsmPopup01));
			this.playerMatSections[3].downActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial09AsmPopup04));
			this.tutorial09AsmPanels[4].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial09AsmPopup05));
			PayResourcePresenter.PayResourceEnded -= this.Tutorial09AsmPopup06;
			EnlistPresenter.RecruitSelected -= this.Tutorial09AsmPopup07;
			EnlistPresenter.BonusSelected -= this.Tutorial09AsmPopup08;
			this.tutorial09AsmPanels[9].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial09AsmPopup10));
			this.tutorial09AsmPanels[10].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial09AsmPopup11));
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial09AsmEnd));
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x000A91EC File Offset: 0x000A73EC
		private void Tutorial09AsmStart()
		{
			this.MissionProgress.enabled = true;
			this.MissionProgress.text = string.Empty;
			this.ClosePopups(this.tutorial09AsmPanels);
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial09AsmPopup00;
			AnalyticsEventData.TutorialStart(StepIDs.tut9_00_introduction);
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x000A9248 File Offset: 0x000A7448
		private void Tutorial09AsmPopup00()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial09AsmPopup00;
			this.tutorial09AsmPanels[0].SetActive(true);
			this.tutorial09AsmPanels[0].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial09AsmPopup01));
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x000A92A4 File Offset: 0x000A74A4
		private void Tutorial09AsmPopup01()
		{
			this.tutorial09AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial09AsmPopup01));
			this.tutorial09AsmPanels[0].SetActive(false);
			this.tutorial09AsmPanels[1].SetActive(true);
			this.playerMatSections[3].downActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial09AsmPopup04));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x000A9324 File Offset: 0x000A7524
		private void Tutorial09AsmPopup04()
		{
			this.playerMatSections[3].downActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial09AsmPopup04));
			if (this.tutorial09AsmPanels[2].activeInHierarchy)
			{
				this.DefaultTutorialStepLog(StepStatuses.completed, -1);
			}
			else
			{
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
			this.playerStatsPresenter.ExpandDown();
			this.tutorial09AsmPanels[1].SetActive(false);
			this.tutorial09AsmPanels[4].SetActive(true);
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[i, j], false);
				}
			}
			this.tutorial09AsmPanels[4].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial09AsmPopup05));
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x000A93FC File Offset: 0x000A75FC
		private void Tutorial09AsmPopup05()
		{
			this.tutorial09AsmPanels[4].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial09AsmPopup05));
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[0, 4]);
			this.tutorial09AsmPanels[4].SetActive(false);
			this.tutorial09AsmPanels[5].SetActive(true);
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[i, j], true);
				}
			}
			PayResourcePresenter.PayResourceEnded += this.Tutorial09AsmPopup06;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x000A94C8 File Offset: 0x000A76C8
		private void Tutorial09AsmPopup06()
		{
			PayResourcePresenter.PayResourceEnded -= this.Tutorial09AsmPopup06;
			this.arrowBoard[0].Hide();
			this.tutorial09AsmPanels[5].SetActive(false);
			this.tutorial09AsmPanels[6].SetActive(true);
			EnlistPresenter.RecruitSelected += this.Tutorial09AsmPopup07;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x000A9528 File Offset: 0x000A7728
		private void Tutorial09AsmPopup07()
		{
			EnlistPresenter.RecruitSelected -= this.Tutorial09AsmPopup07;
			this.tutorial09AsmPanels[6].SetActive(false);
			this.tutorial09AsmPanels[7].SetActive(true);
			EnlistPresenter.BonusSelected += this.Tutorial09AsmPopup08;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x000A957C File Offset: 0x000A777C
		private void Tutorial09AsmPopup08(GainType type)
		{
			EnlistPresenter.BonusSelected -= this.Tutorial09AsmPopup08;
			this.tutorial09AsmPanels[7].SetActive(false);
			this.tutorial09AsmPanels[8].SetActive(true);
			this.tutorial09AsmPanels[8].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial09AsmPopup10));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x000A95E4 File Offset: 0x000A77E4
		private void Tutorial09AsmPopup10()
		{
			this.tutorial09AsmPanels[8].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial09AsmPopup10));
			this.tutorial09AsmPanels[8].SetActive(false);
			this.tutorial09AsmPanels[10].SetActive(true);
			this.tutorial09AsmPanels[10].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial09AsmPopup11));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x000A9660 File Offset: 0x000A7860
		private void Tutorial09AsmPopup11()
		{
			this.tutorial09AsmPanels[10].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial09AsmPopup11));
			this.tutorial09AsmPanels[10].SetActive(false);
			this.tutorial09AsmPanels[11].SetActive(true);
			this.endMoveText.text = ScriptLocalization.Get("GameScene/EndMission");
			this.endTurnButton.onClick.AddListener(new UnityAction(this.Tutorial09AsmEnd));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x000A96E8 File Offset: 0x000A78E8
		private void Tutorial09AsmCleanup()
		{
			AnalyticsEventData.TutorialFinished();
			AnalyticsEventData.TutorialStepStoped();
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			AnalyticsEventData.ResetTutorialStepTimer();
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial09AsmEnd));
			this.tutorial09AsmPanels[11].SetActive(false);
			TutorialMissionSelection.MissionCompleted(8);
			GameController.GameManager.EndTutorial();
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x00039A06 File Offset: 0x00037C06
		public void Tutorial09AsmEnd()
		{
			this.Tutorial09AsmCleanup();
			this.gameController.ExitGame();
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x00039A19 File Offset: 0x00037C19
		public void Tutorial09AsmNext()
		{
			this.Tutorial09AsmCleanup();
			this.LoadTutorial(9, 0);
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x000A9748 File Offset: 0x000A7948
		private void RemoveAllListenersOfTutorial10()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial10AsmPopup00;
			this.tutorial10AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial10AsmPopup01));
			GameController.OnTopTabClicked -= this.Tutorial10AsmPopup02;
			this.tutorial10AsmPanels[2].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial10AsmPopup03));
			GameController.OnTopTabClicked -= this.Tutorial10AsmPopup04;
			this.tutorial10AsmPanels[4].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial10AsmPopup05));
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial10AsmPopup07));
			MovePresenter.MoveEnded -= this.Tutorial10AsmPopup08;
			GameController.OnTopTabClicked -= this.Tutorial10AsmPopup09;
			GameController.AfterEndTurnAIAndPlayer -= this.Tutorial10AsmPopup10;
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial10AsmEnd));
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x000A9878 File Offset: 0x000A7A78
		private void Tutorial10AsmStart()
		{
			this.objectivePreview.SetActive(true);
			this.objectiveNamesButton.enabled = false;
			this.MissionProgress.enabled = true;
			this.MissionProgress.text = string.Empty;
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial10AsmPopup00;
			AnalyticsEventData.TutorialStart(StepIDs.tut10_00_introduction);
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x000A98E0 File Offset: 0x000A7AE0
		private void Tutorial10AsmPopup00()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial10AsmPopup00;
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[i, j], false);
				}
			}
			this.AllowClickOnHex(3, 1, true);
			this.AllowClickOnHex(6, 1, true);
			this.tutorial10AsmPanels[0].SetActive(true);
			this.tutorial10AsmPanels[0].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial10AsmPopup01));
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x000A9984 File Offset: 0x000A7B84
		private void Tutorial10AsmPopup01()
		{
			this.tutorial10AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial10AsmPopup01));
			this.tutorial10AsmPanels[0].SetActive(false);
			this.tutorial10AsmPanels[1].SetActive(true);
			GameController.OnTopTabClicked += this.Tutorial10AsmPopup02;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x000A99EC File Offset: 0x000A7BEC
		private void Tutorial10AsmPopup02(int id)
		{
			GameController.OnTopTabClicked -= this.Tutorial10AsmPopup02;
			this.tutorial10AsmPanels[1].SetActive(false);
			this.tutorial10AsmPanels[2].SetActive(true);
			this.tutorial10AsmPanels[2].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial10AsmPopup03));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x000A9A54 File Offset: 0x000A7C54
		private void Tutorial10AsmPopup03()
		{
			this.tutorial10AsmPanels[2].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial10AsmPopup03));
			this.topLeftTabs.SetAllTogglesOff();
			this.tutorial10AsmPanels[2].SetActive(false);
			this.tutorial10AsmPanels[3].SetActive(true);
			GameController.OnTopTabClicked += this.Tutorial10AsmPopup04;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x000A9AC4 File Offset: 0x000A7CC4
		private void Tutorial10AsmPopup04(int id)
		{
			GameController.OnTopTabClicked -= this.Tutorial10AsmPopup04;
			this.tutorial10AsmPanels[3].SetActive(false);
			this.tutorial10AsmPanels[4].SetActive(true);
			this.tutorial10AsmPanels[4].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial10AsmPopup05));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x000A9B2C File Offset: 0x000A7D2C
		private void Tutorial10AsmPopup05()
		{
			this.tutorial10AsmPanels[4].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial10AsmPopup05));
			this.topLeftTabs.SetAllTogglesOff();
			this.tutorial10AsmPanels[4].SetActive(false);
			this.tutorial10AsmPanels[5].SetActive(true);
			this.plan.SetActive(true);
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial10AsmPopup07));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x000A9BC4 File Offset: 0x000A7DC4
		private void Tutorial10AsmPopup07()
		{
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial10AsmPopup07));
			this.plan.SetActive(false);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[3, 1]);
			this.arrowBoard[1].Show();
			this.arrowBoard[1].SetPosition(GameController.GameManager.gameBoard.hexMap[6, 1]);
			this.tutorial10AsmPanels[5].SetActive(false);
			this.tutorial10AsmPanels[7].SetActive(true);
			MovePresenter.MoveEnded += this.Tutorial10AsmPopup08;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x000A9C9C File Offset: 0x000A7E9C
		private void Tutorial10AsmPopup08()
		{
			MovePresenter.MoveEnded -= this.Tutorial10AsmPopup08;
			this.arrowBoard[0].Hide();
			this.arrowBoard[1].Hide();
			this.tutorial10AsmPanels[7].SetActive(false);
			this.tutorial10AsmPanels[8].SetActive(true);
			GameController.OnTopTabClicked += this.Tutorial10AsmPopup09;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x000A9D0C File Offset: 0x000A7F0C
		private void Tutorial10AsmPopup09(int id)
		{
			GameController.OnTopTabClicked -= this.Tutorial10AsmPopup09;
			this.tutorial10AsmPanels[8].SetActive(false);
			this.tutorial10AsmPanels[9].SetActive(true);
			this.outlineObjective.SetActive(true);
			GameController.AfterEndTurnAIAndPlayer += this.Tutorial10AsmPopup10;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x000A9D6C File Offset: 0x000A7F6C
		private void Tutorial10AsmPopup10()
		{
			GameController.AfterEndTurnAIAndPlayer -= this.Tutorial10AsmPopup10;
			this.tutorial10AsmPanels[9].SetActive(false);
			this.tutorial10AsmPanels[10].SetActive(true);
			this.outlineObjective.SetActive(false);
			this.tabObjective.SetActive(true);
			this.actionLog.SetActive(false);
			this.gameController.EndTurnButtonEnable();
			this.gameController.endTurnButton.image.sprite = this.gameController.endTurnButtonImageActiveGlow;
			this.gameController.endTurnButton.GetComponent<Animator>().Play("EndTurnPulse");
			this.endMoveText.text = ScriptLocalization.Get("GameScene/EndMission");
			this.endTurnButton.onClick.AddListener(new UnityAction(this.Tutorial10AsmEnd));
			this.endTurnButton.interactable = true;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x000A9E58 File Offset: 0x000A8058
		private void Tutorial10AsmCleanup()
		{
			AnalyticsEventData.TutorialFinished();
			AnalyticsEventData.TutorialStepStoped();
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			AnalyticsEventData.ResetTutorialStepTimer();
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial10AsmEnd));
			this.tutorial10AsmPanels[10].SetActive(false);
			TutorialMissionSelection.MissionCompleted(9);
			GameController.GameManager.EndTutorial();
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x00039A2A File Offset: 0x00037C2A
		public void Tutorial10AsmEnd()
		{
			this.Tutorial10AsmCleanup();
			this.gameController.ExitGame();
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x00039A3D File Offset: 0x00037C3D
		public void Tutorial10AsmNext()
		{
			this.Tutorial10AsmCleanup();
			this.LoadTutorial(10, 0);
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x000A9EB8 File Offset: 0x000A80B8
		private void RemoveAllListenersOfTutorial11()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial11AsmPopup00;
			this.tutorial11AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial11AsmPopup01));
			GameController.OnTopTabClicked -= this.Tutorial11AsmPopup02;
			this.tutorial11AsmPanels[2].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial11AsmPopup03));
			GameController.OnTopTabClicked -= this.Tutorial11AsmPopup04;
			this.tutorial11AsmPanels[4].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial11AsmPopup05));
			this.tutorial11AsmPanels[6].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial11AsmPopup07));
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial11AsmPopup09));
			GameController.HexGetFocused -= this.Tutorial11AsmPopup10;
			GameController.UnitGetFocused -= this.FactoryWorkerSelected;
			MovePresenter.MoveEnded -= this.Tutorial11AsmPopup11;
			GameController.UnitGetFocused -= this.TunnelWorkerSelected;
			this.playerMatSections[2].downActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial11AsmPopup13));
			PayResourcePresenter.PayResourceEnded -= this.Tutorial11AsmPopup14;
			BuildPresenter.BuildingSelected -= this.Tutorial11AsmPopup15;
			GameController.GameManager.GameHasEnded -= this.Tutorial11AsmPopup16;
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x000AA060 File Offset: 0x000A8260
		private void Tutorial11AsmStart()
		{
			this.pointerAreaPresenter.SetActive(false);
			this.MissionProgress.enabled = true;
			this.MissionProgress.text = string.Empty;
			this.gameController.turnInfoPanel.OnEndTurnClick += this.Tutorial11AsmPopup00;
			AnalyticsEventData.TutorialStart(StepIDs.tut11_00_introduction);
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x000AA0BC File Offset: 0x000A82BC
		private void Tutorial11AsmPopup00()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.Tutorial11AsmPopup00;
			this.tutorial11AsmPanels[0].SetActive(true);
			this.tutorial11AsmPanels[0].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial11AsmPopup01));
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x000AA118 File Offset: 0x000A8318
		private void Tutorial11AsmPopup01()
		{
			this.tutorial11AsmPanels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial11AsmPopup01));
			this.tutorial11AsmPanels[0].SetActive(false);
			this.tutorial11AsmPanels[1].SetActive(true);
			GameController.OnTopTabClicked += this.Tutorial11AsmPopup02;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x000AA180 File Offset: 0x000A8380
		private void Tutorial11AsmPopup02(int id)
		{
			GameController.OnTopTabClicked -= this.Tutorial11AsmPopup02;
			this.tutorial11AsmPanels[1].SetActive(false);
			this.tutorial11AsmPanels[2].SetActive(true);
			this.tutorial11AsmPanels[2].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial11AsmPopup03));
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[5, 3]);
			this.arrowBoard[1].Show();
			this.arrowBoard[1].SetPosition(GameController.GameManager.gameBoard.hexMap[3, 2]);
			ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.hexMap[5, 3]).GetWorldPosition());
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x000AA27C File Offset: 0x000A847C
		private void Tutorial11AsmPopup03()
		{
			this.tutorial11AsmPanels[2].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial11AsmPopup03));
			this.tutorial11AsmPanels[2].SetActive(false);
			this.tutorial11AsmPanels[3].SetActive(true);
			this.arrowBoard[0].Hide();
			this.arrowBoard[1].Hide();
			GameController.OnTopTabClicked += this.Tutorial11AsmPopup04;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x000AA2FC File Offset: 0x000A84FC
		public void Tutorial11AsmPopup04(int id)
		{
			GameController.OnTopTabClicked -= this.Tutorial11AsmPopup04;
			if (this.tutorial11AsmPanels[3].activeInHierarchy)
			{
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
			else
			{
				this.DefaultTutorialStepLog(StepStatuses.failed, 2);
			}
			this.tutorial11AsmPanels[3].SetActive(false);
			this.tutorial11AsmPanels[4].SetActive(true);
			this.tutorial11AsmPanels[4].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial11AsmPopup05));
			this.tutorial11AsmPanels[5].SetActive(false);
			if (!this.playerStatsPresenter.expandedDown)
			{
				this.playerStatsPresenter.ExpandDown();
			}
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x000AA3A0 File Offset: 0x000A85A0
		private void Tutorial11AsmPopup05()
		{
			this.tutorial11AsmPanels[4].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial11AsmPopup05));
			this.tutorial11AsmPanels[4].SetActive(false);
			this.tutorial11AsmPanels[5].SetActive(true);
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x000AA3F4 File Offset: 0x000A85F4
		public void Tutorial11AsmPopup06()
		{
			this.tutorial11AsmPanels[5].SetActive(false);
			this.tutorial11AsmPanels[6].SetActive(true);
			this.tutorial11AsmPanels[6].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.Tutorial11AsmPopup07));
			this.toggleGroup.SetAllTogglesOff();
			ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.hexMap[4, 4]).GetWorldPosition());
			this.plan11.SetActive(true);
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x000AA494 File Offset: 0x000A8694
		private void Tutorial11AsmPopup07()
		{
			this.tutorial11AsmPanels[6].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.Tutorial11AsmPopup07));
			this.plan11.SetActive(false);
			this.tutorial11AsmPanels[6].SetActive(false);
			this.tutorial11AsmPanels[7].SetActive(true);
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial11AsmPopup09));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x000AA520 File Offset: 0x000A8720
		private void Tutorial11AsmPopup09()
		{
			this.playerMatSections[2].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial11AsmPopup09));
			this.tutorial11AsmPanels[7].SetActive(false);
			this.tutorial11AsmPanels[9].SetActive(true);
			UnitPresenter unitPresenter = GameController.GetUnitPresenter(GameController.GameManager.gameBoard.hexMap[4, 4].GetOwnerWorkers()[0]);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(unitPresenter.transform.position);
			foreach (GameHex gameHex in new GameHex[]
			{
				GameController.GameManager.gameBoard.hexMap[5, 3],
				GameController.GameManager.gameBoard.hexMap[5, 4],
				GameController.GameManager.gameBoard.hexMap[5, 5],
				GameController.GameManager.gameBoard.hexMap[2, 3],
				GameController.GameManager.gameBoard.hexMap[3, 1],
				GameController.GameManager.gameBoard.hexMap[3, 2],
				GameController.GameManager.gameBoard.hexMap[5, 1],
				GameController.GameManager.gameBoard.hexMap[4, 1],
				GameController.GameManager.gameBoard.hexMap[4, 2],
				GameController.GameManager.gameBoard.hexMap[5, 3],
				GameController.GameManager.gameBoard.hexMap[5, 5],
				GameController.GameManager.gameBoard.hexMap[2, 5],
				GameController.GameManager.gameBoard.hexMap[3, 6]
			})
			{
				this.AllowClickOnHex(gameHex, false);
			}
			GameController.HexGetFocused += this.Tutorial11AsmPopup10;
			GameController.UnitGetFocused += this.FactoryWorkerSelected;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x000AA758 File Offset: 0x000A8958
		private void FactoryWorkerSelected(UnitPresenter unitPresenter)
		{
			if (unitPresenter.UnitLogic.position == GameController.GameManager.gameBoard.hexMap[4, 4])
			{
				this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.factory);
				return;
			}
			UnitPresenter unitPresenter2 = GameController.GetUnitPresenter(GameController.GameManager.gameBoard.hexMap[4, 4].GetOwnerWorkers()[0]);
			this.arrowBoard[0].SetPosition(unitPresenter2.transform.position);
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x000AA7E4 File Offset: 0x000A89E4
		private void Tutorial11AsmPopup10(Scythe.BoardPresenter.GameHexPresenter presenter)
		{
			if (presenter.GetGameHexLogic() == GameController.GameManager.gameBoard.factory && GameController.GameManager.moveManager.GetSelectedUnit().position == GameController.GameManager.gameBoard.factory)
			{
				GameController.HexGetFocused -= this.Tutorial11AsmPopup10;
				GameController.UnitGetFocused -= this.FactoryWorkerSelected;
				this.AllowClickOnHex(GameController.GameManager.gameBoard.factory, false);
				this.tutorial11AsmPanels[9].SetActive(false);
				this.tutorial11AsmPanels[10].SetActive(true);
				MovePresenter.MoveEnded += this.Tutorial11AsmPopup11;
				GameController.UnitGetFocused += this.TunnelWorkerSelected;
				UnitPresenter unitPresenter = GameController.GetUnitPresenter(GameController.GameManager.gameBoard.hexMap[4, 1].GetOwnerWorkers()[0]);
				this.arrowBoard[0].SetPosition(unitPresenter.transform.position);
				if (!PlatformManager.IsMobile)
				{
					ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.hexMap[4, 1]).GetWorldPosition());
				}
				this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[2, 3], true);
				this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			}
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x000AA94C File Offset: 0x000A8B4C
		private void TunnelWorkerSelected(UnitPresenter unitPresenter)
		{
			if (unitPresenter.UnitLogic.position == GameController.GameManager.gameBoard.hexMap[4, 1])
			{
				this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[2, 3]);
				return;
			}
			UnitPresenter unitPresenter2 = GameController.GetUnitPresenter(GameController.GameManager.gameBoard.hexMap[4, 1].GetOwnerWorkers()[0]);
			this.arrowBoard[0].SetPosition(unitPresenter2.transform.position);
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x000AA9E0 File Offset: 0x000A8BE0
		private void Tutorial11AsmPopup11()
		{
			this.arrowBoard[0].Hide();
			MovePresenter.MoveEnded -= this.Tutorial11AsmPopup11;
			GameController.UnitGetFocused -= this.TunnelWorkerSelected;
			this.AllowClickOnHex(GameController.GameManager.gameBoard.hexMap[4, 1], true);
			this.AllowClickOnHex(GameController.GameManager.gameBoard.factory, false);
			this.tutorial11AsmPanels[10].SetActive(false);
			this.tutorial11AsmPanels[11].SetActive(true);
			this.playerMatSections[2].downActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.Tutorial11AsmPopup13));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x000AAAA0 File Offset: 0x000A8CA0
		private void Tutorial11AsmPopup13()
		{
			this.playerMatSections[2].downActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.Tutorial11AsmPopup13));
			this.tutorial11AsmPanels[11].SetActive(false);
			this.tutorial11AsmPanels[13].SetActive(true);
			PayResourcePresenter.PayResourceEnded += this.Tutorial11AsmPopup14;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x000AAB10 File Offset: 0x000A8D10
		private void Tutorial11AsmPopup14()
		{
			PayResourcePresenter.PayResourceEnded -= this.Tutorial11AsmPopup14;
			this.tutorial11AsmPanels[13].SetActive(false);
			this.tutorial11AsmPanels[14].SetActive(true);
			BuildPresenter.BuildingSelected += this.Tutorial11AsmPopup15;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x000AAB68 File Offset: 0x000A8D68
		private void Tutorial11AsmPopup15()
		{
			BuildPresenter.BuildingSelected -= this.Tutorial11AsmPopup15;
			this.tutorial11AsmPanels[14].SetActive(false);
			this.tutorial11AsmPanels[15].SetActive(true);
			this.arrowBoard[0].Show();
			this.arrowBoard[0].SetPosition(GameController.GameManager.gameBoard.hexMap[2, 3]);
			GameController.GameManager.GameHasEnded += this.Tutorial11AsmPopup16;
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x000AABF4 File Offset: 0x000A8DF4
		private void Tutorial11AsmPopup16()
		{
			GameController.GameManager.GameHasEnded -= this.Tutorial11AsmPopup16;
			this.arrowBoard[0].Hide();
			this.tutorial11AsmPanels[15].SetActive(false);
			this.tutorial11AsmPanels[16].SetActive(true);
			this.endTurnButton.interactable = true;
			this.endTurnButton.gameObject.SetActive(true);
			this.endMoveText.text = ScriptLocalization.Get("GameScene/EndMission");
			this.endTurnButton.onClick.RemoveAllListeners();
			this.endTurnButton.onClick.AddListener(new UnityAction(this.Tutorial11AsmEnd));
			this.winnerPanelExitButton.onClick.RemoveAllListeners();
			this.winnerPanelExitButton.onClick.AddListener(new UnityAction(this.EndGameFromWinnerPanel));
			this.gameController.endTurnHintButton.SetActive(false);
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x00039A4E File Offset: 0x00037C4E
		private void EndGameFromWinnerPanel()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_exit_button);
			this.Tutorial11AsmEnd();
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x000AACE8 File Offset: 0x000A8EE8
		private void Tutorial11AsmEnd()
		{
			TutorialMissionSelection.MissionCompleted(10);
			AnalyticsEventData.TutorialFinished();
			AnalyticsEventData.TutorialStepStoped();
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			AnalyticsEventData.ResetTutorialStepTimer();
			this.winnerPanelExitButton.onClick.RemoveListener(new UnityAction(this.Tutorial11AsmEnd));
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.Tutorial11AsmEnd));
			this.tutorial11AsmPanels[16].SetActive(false);
			GameController.GameManager.EndTutorial();
			this.gameController.ExitGame();
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x000AAD70 File Offset: 0x000A8F70
		private void RemoveAllListenersOfTutorial01()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.TutorialAsmB01Popup00a;
			this.tutorialAsmB1Panels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup01));
			if (this.podiumToggle != null)
			{
				this.podiumToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.TutorialAsmB01Popup02));
			}
			this.tutorialAsmB1Panels[2].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup03));
			this.tutorialAsmB1Panels[3].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup04));
			this.tutorialAsmB1Panels[4].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup05));
			this.endTurnButton.onClick.RemoveListener(new UnityAction(this.CloseTutorialAsmB01Popup06));
			this.playerMatSections[0].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup06));
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.TutorialAsmB01Popup07;
			this.tutorialAsmB1Panels[7].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup08));
			if (this.podiumToggle != null)
			{
				this.podiumToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.TutorialAsmB01Popup09));
			}
			this.tutorialAsmB1Panels[9].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup10));
			this.playerMatSections[0].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup11));
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.TutorialAsmB01End));
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x000AAF68 File Offset: 0x000A9168
		private void TutorialAsmB01Start()
		{
			this.MissionProgress.enabled = true;
			this.MissionProgress.text = string.Empty;
			GameObject[] array = this.tutorialAsmB1Panels;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			this.gameController.turnInfoPanel.OnEndTurnClick += this.TutorialAsmB01Popup00a;
			AnalyticsEventData.TutorialStart(StepIDs.tut1_00_introduction);
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x000AAFD4 File Offset: 0x000A91D4
		private void TutorialAsmB01Popup00a()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.TutorialAsmB01Popup00a;
			this.tutorialAsmB1Panels[12].SetActive(true);
			this.tutorialAsmB1Panels[12].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.TutorialAsmB01Popup00b));
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x000AB030 File Offset: 0x000A9230
		private void TutorialAsmB01Popup00b()
		{
			this.tutorialAsmB1Panels[12].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup00b));
			this.tutorialAsmB1Panels[12].SetActive(false);
			this.tutorialAsmB1Panels[13].SetActive(true);
			this.tutorialAsmB1Panels[13].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.TutorialAsmB01Popup00c));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x000AB0AC File Offset: 0x000A92AC
		private void TutorialAsmB01Popup00c()
		{
			this.tutorialAsmB1Panels[13].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup00c));
			this.tutorialAsmB1Panels[13].SetActive(false);
			this.tutorialAsmB1Panels[14].SetActive(true);
			this.structureBonusToggle.isOn = true;
			this.tutorialAsmB1Panels[14].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.TutorialAsmB01Popup00d));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x000AB134 File Offset: 0x000A9334
		private void TutorialAsmB01Popup00d()
		{
			this.tutorialAsmB1Panels[14].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup00d));
			this.tutorialAsmB1Panels[14].SetActive(false);
			this.tutorialAsmB1Panels[0].SetActive(true);
			this.structureBonusToggle.isOn = false;
			this.tutorialAsmB1Panels[0].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.TutorialAsmB01Popup01));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x000AB1BC File Offset: 0x000A93BC
		private void TutorialAsmB01Popup01()
		{
			this.tutorialAsmB1Panels[0].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup01));
			this.tutorialAsmB1Panels[0].SetActive(false);
			this.tutorialAsmB1Panels[1].SetActive(true);
			if (this.podiumToggle != null)
			{
				this.podiumToggle.onValueChanged.AddListener(new UnityAction<bool>(this.TutorialAsmB01Popup02));
			}
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x000AB23C File Offset: 0x000A943C
		private void TutorialAsmB01Popup02(bool state)
		{
			if (this.podiumToggle != null)
			{
				this.podiumToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.TutorialAsmB01Popup02));
			}
			this.tutorialAsmB1Panels[1].SetActive(false);
			this.tutorialAsmB1Panels[2].SetActive(true);
			this.tutorialAsmB1Panels[2].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.TutorialAsmB01Popup03));
			this.podiumClose.onClick.AddListener(new UnityAction(this.TutorialAsmB01Popup03));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x000AB2D8 File Offset: 0x000A94D8
		private void TutorialAsmB01Popup03()
		{
			this.tutorialAsmB1Panels[2].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup03));
			this.podiumClose.onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup03));
			if (this.podiumClose.IsActive())
			{
				this.podiumClose.onClick.Invoke();
			}
			this.tutorialAsmB1Panels[2].SetActive(false);
			this.tutorialAsmB1Panels[3].SetActive(true);
			this.tutorialAsmB1Panels[3].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.TutorialAsmB01Popup04));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x000AB388 File Offset: 0x000A9588
		private void TutorialAsmB01Popup04()
		{
			this.tutorialAsmB1Panels[3].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup04));
			this.tutorialAsmB1Panels[3].SetActive(false);
			this.tutorialAsmB1Panels[4].SetActive(true);
			this.tutorialAsmB1Panels[4].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.TutorialAsmB01Popup05));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x000AB400 File Offset: 0x000A9600
		private void TutorialAsmB01Popup05()
		{
			this.tutorialAsmB1Panels[4].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup05));
			this.tutorialAsmB1Panels[4].SetActive(false);
			this.tutorialAsmB1Panels[5].SetActive(true);
			this.playerMatSections[2].topActionPresenter.gainActionButton[1].onClick.AddListener(new UnityAction(this.TutorialAsmB01Popup06));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x000AB480 File Offset: 0x000A9680
		private void TutorialAsmB01Popup06()
		{
			this.playerMatSections[0].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup06));
			this.tutorialAsmB1Panels[5].SetActive(false);
			this.tutorialAsmB1Panels[6].SetActive(true);
			this.gameController.turnInfoPanel.OnEndTurnClick += this.TutorialAsmB01Popup07;
			this.endTurnButton.onClick.AddListener(new UnityAction(this.CloseTutorialAsmB01Popup06));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x00039A5C File Offset: 0x00037C5C
		private void CloseTutorialAsmB01Popup06()
		{
			this.endTurnButton.onClick.RemoveListener(new UnityAction(this.CloseTutorialAsmB01Popup06));
			this.tutorialAsmB1Panels[6].SetActive(false);
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x000AB514 File Offset: 0x000A9714
		private void TutorialAsmB01Popup07()
		{
			this.gameController.turnInfoPanel.OnEndTurnClick -= this.TutorialAsmB01Popup07;
			this.tutorialAsmB1Panels[6].SetActive(false);
			this.tutorialAsmB1Panels[7].SetActive(true);
			this.tutorialAsmB1Panels[7].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.TutorialAsmB01Popup08));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x000AB584 File Offset: 0x000A9784
		private void TutorialAsmB01Popup08()
		{
			this.tutorialAsmB1Panels[7].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup08));
			this.tutorialAsmB1Panels[7].SetActive(false);
			this.tutorialAsmB1Panels[8].SetActive(true);
			if (this.podiumToggle != null)
			{
				this.podiumToggle.onValueChanged.AddListener(new UnityAction<bool>(this.TutorialAsmB01Popup09));
			}
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x000AB604 File Offset: 0x000A9804
		private void TutorialAsmB01Popup09(bool status)
		{
			if (this.podiumToggle != null)
			{
				this.podiumToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.TutorialAsmB01Popup09));
			}
			this.tutorialAsmB1Panels[8].SetActive(false);
			this.tutorialAsmB1Panels[9].SetActive(true);
			this.tutorialAsmB1Panels[9].GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(this.TutorialAsmB01Popup10));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x000AB684 File Offset: 0x000A9884
		private void TutorialAsmB01Popup10()
		{
			this.tutorialAsmB1Panels[9].GetComponentInChildren<Button>().onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup10));
			this.tutorialAsmB1Panels[9].SetActive(false);
			this.tutorialAsmB1Panels[10].SetActive(true);
			this.playerMatSections[0].topActionPresenter.gainActionButton[0].onClick.AddListener(new UnityAction(this.TutorialAsmB01Popup11));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x000AB708 File Offset: 0x000A9908
		private void TutorialAsmB01Popup11()
		{
			this.playerMatSections[0].topActionPresenter.gainActionButton[0].onClick.RemoveListener(new UnityAction(this.TutorialAsmB01Popup11));
			this.tutorialAsmB1Panels[10].SetActive(false);
			this.tutorialAsmB1Panels[11].SetActive(true);
			if (this.podiumClose.IsActive())
			{
				this.podiumClose.onClick.Invoke();
			}
			this.endMoveText.text = ScriptLocalization.Get("GameScene/EndMission");
			this.endTurnButton.onClick.AddListener(new UnityAction(this.TutorialAsmB01End));
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x000AB7B4 File Offset: 0x000A99B4
		private void TutorialAsmB01Cleanup()
		{
			AnalyticsEventData.TutorialFinished();
			AnalyticsEventData.TutorialStepStoped();
			this.DefaultTutorialStepLog(StepStatuses.completed, 0);
			AnalyticsEventData.ResetTutorialStepTimer();
			this.endMoveButton.onClick.RemoveListener(new UnityAction(this.TutorialAsmB01End));
			this.tutorialAsmB1Panels[11].SetActive(false);
			TutorialMissionSelection.MissionCompleted(0);
			GameController.GameManager.EndTutorial();
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x00039A88 File Offset: 0x00037C88
		public void TutorialAsmB01End()
		{
			this.TutorialAsmB01Cleanup();
			this.gameController.ExitGame();
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x00039A9B File Offset: 0x00037C9B
		public void TutorialAsmB01Next()
		{
			this.TutorialAsmB01Cleanup();
			this.LoadTutorial(1, 0);
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x00039AAB File Offset: 0x00037CAB
		// (set) Token: 0x06001B14 RID: 6932 RVA: 0x00039AB3 File Offset: 0x00037CB3
		public bool TutorialCompleted { get; private set; }

		// Token: 0x06001B15 RID: 6933 RVA: 0x000AB814 File Offset: 0x000A9A14
		private void Start()
		{
			if (!GameController.GameManager.IsCampaign)
			{
				return;
			}
			if (GameController.GameManager.missionId >= 0)
			{
				MainMenu.loadState = MainMenu.State.Campaign;
			}
			this.UIBlocker.SetActive(false);
			this.objectivePreview.SetActive(false);
			this.optionsBonusInspection.SetActive(false);
			switch (GameController.GameManager.missionId)
			{
			case 0:
				this.TutorialAsmB01Start();
				return;
			case 1:
				this.Tutorial02AsmStart();
				return;
			case 2:
				this.Tutorial03AsmStart();
				return;
			case 3:
				this.Tutorial04AsmStart();
				return;
			case 4:
				this.Tutorial05AsmStart();
				return;
			case 5:
				this.Tutorial06AsmStart();
				return;
			case 6:
				this.Tutorial07AsmStart();
				return;
			case 7:
				this.Tutorial08AsmStart();
				return;
			case 8:
				this.Tutorial09AsmStart();
				return;
			case 9:
				this.Tutorial10AsmStart();
				return;
			case 10:
				this.Tutorial11AsmStart();
				return;
			default:
				Debug.LogWarning("Mission with this ID: " + GameController.GameManager.missionId.ToString() + " doesn't exist");
				return;
			}
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x000AB914 File Offset: 0x000A9B14
		private void Update()
		{
			if (this.camAnim)
			{
				float num = (Time.time - this.camAnimStartTime) / this.camAnimLength;
				float num2 = this.camAnimationCurve.Evaluate(num);
				if (num > 1f)
				{
					num2 = 1f;
					this.camAnim = false;
					CameraControler.CameraMovementBlocked = false;
				}
				this.gameController.cameraControler.swivel.transform.position = new Vector3(Mathf.Lerp(this.camXfrom, this.camXto, num2), 0f, Mathf.Lerp(this.camYfrom, this.camYto, num2));
				this.gameController.cameraControler.zoom = Mathf.Lerp(this.camZfrom, this.camZto, num2);
				this.gameController.cameraControler.stickMinZoom = Mathf.Lerp(this.camParamsFrom.stickMinZoom, this.camParamsTo.stickMinZoom, num2);
				this.gameController.cameraControler.stickMaxZoom = Mathf.Lerp(this.camParamsFrom.stickMaxZoom, this.camParamsTo.stickMaxZoom, num2);
				this.gameController.cameraControler.swivelMinZoom = Mathf.Lerp(this.camParamsFrom.swivelMinZoom, this.camParamsTo.swivelMinZoom, num2);
				this.gameController.cameraControler.swivelMaxZoom = Mathf.Lerp(this.camParamsFrom.swivelMaxZoom, this.camParamsTo.swivelMaxZoom, num2);
				this.gameController.cameraControler.posXMin = Mathf.Lerp(this.camParamsFrom.posXMin, this.camParamsTo.posXMin, num2);
				this.gameController.cameraControler.posXMax = Mathf.Lerp(this.camParamsFrom.posXMax, this.camParamsTo.posXMax, num2);
				this.gameController.cameraControler.posYMin = Mathf.Lerp(this.camParamsFrom.posYMin, this.camParamsTo.posYMin, num2);
				this.gameController.cameraControler.posYMax = Mathf.Lerp(this.camParamsFrom.posYMax, this.camParamsTo.posYMax, num2);
			}
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x000ABB34 File Offset: 0x000A9D34
		public void AnimateCam(float camXto, float camYto, float camZto, CameraParams camParamsTo, float camAnimLength = 1f)
		{
			CameraControler.CameraMovementBlocked = true;
			this.camXfrom = this.gameController.cameraControler.swivel.transform.position.x;
			this.camYfrom = this.gameController.cameraControler.swivel.transform.position.z;
			this.camZfrom = this.gameController.cameraControler.zoom;
			this.camParamsFrom = new CameraParams
			{
				stickMinZoom = this.gameController.cameraControler.stickMinZoom,
				stickMaxZoom = this.gameController.cameraControler.stickMaxZoom,
				swivelMinZoom = this.gameController.cameraControler.swivelMinZoom,
				swivelMaxZoom = this.gameController.cameraControler.swivelMaxZoom,
				posXMin = this.gameController.cameraControler.posXMin,
				posXMax = this.gameController.cameraControler.posXMax,
				posYMin = this.gameController.cameraControler.posYMin,
				posYMax = this.gameController.cameraControler.posYMax
			};
			this.camXto = camXto;
			this.camYto = camYto;
			this.camZto = camZto;
			this.camParamsTo = camParamsTo;
			if (camAnimLength == 0f)
			{
				camAnimLength = 0.01f;
			}
			this.camAnimLength = camAnimLength;
			this.camAnimStartTime = Time.time;
			this.camAnim = true;
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x0002D4C7 File Offset: 0x0002B6C7
		public void TutorialAcceptButton()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x000ABCAC File Offset: 0x000A9EAC
		private void SetCamera(Vector3 position, float posXMin, float posXMax, float posYMin, float posYMax, float zoom, float stickMinZoom, float stickMaxZoom, float swivelMinZoom, float swivelMaxZoom, float focalSizeFar)
		{
			this.gameController.cameraControler.swivel.transform.position = new Vector3(position.x, 0f, position.z);
			this.gameController.cameraControler.posXMin = posXMin;
			this.gameController.cameraControler.posXMax = posXMax;
			this.gameController.cameraControler.posYMin = posYMin;
			this.gameController.cameraControler.posYMax = posYMax;
			this.gameController.cameraControler.zoom = zoom;
			this.gameController.cameraControler.stickMinZoom = stickMinZoom;
			this.gameController.cameraControler.stickMaxZoom = stickMaxZoom;
			this.gameController.cameraControler.swivelMinZoom = swivelMinZoom;
			this.gameController.cameraControler.swivelMaxZoom = swivelMaxZoom;
			this.gameController.cameraControler.focalSizeFar = focalSizeFar;
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x00039ABC File Offset: 0x00037CBC
		private void PopUpWindow(GameObject window)
		{
			window.SetActive(true);
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x00039AC5 File Offset: 0x00037CC5
		private void Button_SetEndTurn()
		{
			if (this.endMoveText != null)
			{
				this.endMoveText.text = "End Turn";
			}
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x00039AE5 File Offset: 0x00037CE5
		private void Button_SetEndMission()
		{
			if (this.endMoveText != null)
			{
				this.endMoveText.text = "End Mission";
			}
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x00039B05 File Offset: 0x00037D05
		public void HideDarkenUI()
		{
			this.DarkenUI.enabled = false;
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x00039B13 File Offset: 0x00037D13
		public void ShowDarkenUI()
		{
			this.DarkenUI.enabled = true;
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x000ABD9C File Offset: 0x000A9F9C
		private void AllowClickOnHex(GameHex gameHex, bool isAllowed = true)
		{
			GameObject gameObject = GameObject.Find("Hex" + gameHex.posX.ToString() + gameHex.posY.ToString());
			if (gameObject != null)
			{
				gameObject.GetComponent<CylinderCollider>().cylinderCollider.enabled = isAllowed;
			}
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x000ABDEC File Offset: 0x000A9FEC
		private void AllowClickOnHex(int posX, int posY, bool isAllowed = true)
		{
			GameObject gameObject = GameObject.Find("Hex" + posX.ToString() + posY.ToString());
			if (gameObject != null)
			{
				gameObject.GetComponent<CylinderCollider>().cylinderCollider.enabled = isAllowed;
			}
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x000ABE34 File Offset: 0x000AA034
		private void AllowClickOnHex(string gameObjectName, bool isAllowed = true)
		{
			GameObject gameObject = GameObject.Find(gameObjectName);
			if (gameObject != null)
			{
				gameObject.GetComponent<CylinderCollider>().cylinderCollider.enabled = isAllowed;
			}
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x000ABE64 File Offset: 0x000AA064
		private void ClosePopups(GameObject[] popups)
		{
			for (int i = 0; i < popups.Length; i++)
			{
				popups[i].SetActive(false);
			}
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x000ABE8C File Offset: 0x000AA08C
		private void OnDisable()
		{
			this.RemoveAllListenersOfTutorial01();
			this.RemoveAllListenersOfTutorial02();
			this.RemoveAllListenersOfTutorial03();
			this.RemoveAllListenersOfTutorial04();
			this.RemoveAllListenersOfTutorial05();
			this.RemoveAllListenersOfTutorial06();
			this.RemoveAllListenersOfTutorial07();
			this.RemoveAllListenersOfTutorial08();
			this.RemoveAllListenersOfTutorial09();
			this.RemoveAllListenersOfTutorial10();
			this.RemoveAllListenersOfTutorial11();
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x00039B21 File Offset: 0x00037D21
		private void DefaultTutorialStepLog(StepStatuses stepStatus, int revertSteps = 0)
		{
			AnalyticsEventData.TutorialStepStoped();
			AnalyticsEventLogger.Instance.LogTutorialStep(stepStatus);
			AnalyticsEventData.ResetTutorialStepTimer();
			AnalyticsEventData.TutorialStepStarted();
			AnalyticsEventData.RevertTutorialStepBy(revertSteps);
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x00039B43 File Offset: 0x00037D43
		private void LoadTutorial(int tutorialId, int campaingId = 0)
		{
			this.OnLoadingNextTutorial();
			GameController.GameManager.InitCampaign(tutorialId, campaingId);
			GameController.gameFromSave = false;
			base.StartCoroutine(this.LoadMainScene());
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x00039B6A File Offset: 0x00037D6A
		private IEnumerator LoadMainScene()
		{
			SceneController.Instance.LoadScene(SceneController.SCENE_MAIN_NAME);
			yield return null;
			yield break;
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x00039B72 File Offset: 0x00037D72
		private void OnLoadingNextTutorial()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_new_game_button);
			AnalyticsEventLogger.Instance.LogMatchStop(EndReasons.game_completed);
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.previousMap);
		}

		// Token: 0x040012C8 RID: 4808
		[Space(10f)]
		[Header("Tutorial02 (Asmodee Redesign) variables")]
		public GameObject[] tutorial02AsmPanels;

		// Token: 0x040012C9 RID: 4809
		public ExchangePanelPresenter exchangePanelPresenter;

		// Token: 0x040012CA RID: 4810
		public GameObject movementPlan;

		// Token: 0x040012CB RID: 4811
		[Space(10f)]
		[Header("Tutorial03 (Asmodee Redesign) variables")]
		public GameObject[] tutorial03AsmPanels;

		// Token: 0x040012CC RID: 4812
		public CombatCardsPanelPresenter combatCardPanelPresenter;

		// Token: 0x040012CD RID: 4813
		public CombatPreperationPresenter combatPreperationPresenter;

		// Token: 0x040012CE RID: 4814
		public GameObject combatResultOKArrow;

		// Token: 0x040012CF RID: 4815
		[Space(10f)]
		[Header("Tutorial04 (Asmodee Redesign) variables")]
		public GameObject[] tutorial04AsmPanels;

		// Token: 0x040012D0 RID: 4816
		[Space(10f)]
		[Header("Tutorial05 (Asmodee Redesign) variables")]
		public GameObject[] tutorial05AsmPanels;

		// Token: 0x040012D1 RID: 4817
		[Space(10f)]
		[Header("Tutorial06 (Asmodee Redesign) variables")]
		public GameObject[] tutorial06AsmPanels;

		// Token: 0x040012D2 RID: 4818
		[Space(10f)]
		[Header("Tutorial07 (Asmodee Redesign) variables")]
		public GameObject[] tutorial07AsmPanels;

		// Token: 0x040012D3 RID: 4819
		public Button[] mechButtonDisable;

		// Token: 0x040012D4 RID: 4820
		[Space(10f)]
		[Header("Tutorial08 (Asmodee Redesign) variables")]
		public GameObject[] tutorial08AsmPanels;

		// Token: 0x040012D5 RID: 4821
		[Space(10f)]
		[Header("Tutorial09 (Asmodee Redesign) variables")]
		public GameObject[] tutorial09AsmPanels;

		// Token: 0x040012D6 RID: 4822
		public PlayerStatsPresenter playerStatsPresenter;

		// Token: 0x040012D7 RID: 4823
		[Space(10f)]
		[Header("Tutorial10 (Asmodee Redesign) variables")]
		public GameObject[] tutorial10AsmPanels;

		// Token: 0x040012D8 RID: 4824
		public Toggle2Group topLeftTabs;

		// Token: 0x040012D9 RID: 4825
		public GameObject plan;

		// Token: 0x040012DA RID: 4826
		public GameObject outlineObjective;

		// Token: 0x040012DB RID: 4827
		public GameObject tabObjective;

		// Token: 0x040012DC RID: 4828
		public GameObject actionLog;

		// Token: 0x040012DD RID: 4829
		public Button objectiveNamesButton;

		// Token: 0x040012DE RID: 4830
		[Space(10f)]
		[Header("Tutorial11 (Asmodee Redesign) variables")]
		public GameObject[] tutorial11AsmPanels;

		// Token: 0x040012DF RID: 4831
		[SerializeField]
		private EndGameStatsPresenter winnerPanel;

		// Token: 0x040012E0 RID: 4832
		[SerializeField]
		private Button winnerPanelExitButton;

		// Token: 0x040012E1 RID: 4833
		[SerializeField]
		private Toggle2Group toggleGroup;

		// Token: 0x040012E2 RID: 4834
		[SerializeField]
		private GameObject plan11;

		// Token: 0x040012E3 RID: 4835
		[SerializeField]
		private GameObject pointerAreaPresenter;

		// Token: 0x040012E4 RID: 4836
		[Space(10f)]
		[Header("Tutorial01 B (Asmodee Redesign) variables")]
		public Button[] mechsAbilities;

		// Token: 0x040012E5 RID: 4837
		public GameObject[] tutorialAsmB1Panels;

		// Token: 0x040012E6 RID: 4838
		public GameObject produceGridContainer;

		// Token: 0x040012E7 RID: 4839
		public Toggle2 structureBonusToggle;

		// Token: 0x040012E8 RID: 4840
		public Toggle2 podiumToggle;

		// Token: 0x040012E9 RID: 4841
		public Button podiumClose;

		// Token: 0x040012EA RID: 4842
		private Toggle[] toggles;

		// Token: 0x040012EB RID: 4843
		public GameObject TopPanelLeft;

		// Token: 0x040012EC RID: 4844
		public GameObject TopPanelRight;

		// Token: 0x040012ED RID: 4845
		public GameObject BottomBackground;

		// Token: 0x040012EE RID: 4846
		public GameObject MiddlePanel;

		// Token: 0x040012EF RID: 4847
		public GameObject endTurn;

		// Token: 0x040012F0 RID: 4848
		public GameObject objectivePreview;

		// Token: 0x040012F1 RID: 4849
		public GameObject[] action1;

		// Token: 0x040012F2 RID: 4850
		public GameObject[] action2;

		// Token: 0x040012F3 RID: 4851
		public GameObject[] action3;

		// Token: 0x040012F4 RID: 4852
		public GameObject[] action4;

		// Token: 0x040012F5 RID: 4853
		public GameObject[] allActions;

		// Token: 0x040012F6 RID: 4854
		public Button[] tradeButtons;

		// Token: 0x040012F7 RID: 4855
		public Button endTrade;

		// Token: 0x040012F8 RID: 4856
		public Button endTurnButton;

		// Token: 0x040012F9 RID: 4857
		public Button endTurnButtonFake;

		// Token: 0x040012FA RID: 4858
		public Button moveButtons;

		// Token: 0x040012FB RID: 4859
		public GameController gameController;

		// Token: 0x040012FC RID: 4860
		public GameObject contextPanel;

		// Token: 0x040012FD RID: 4861
		public Slider oilSlider;

		// Token: 0x040012FE RID: 4862
		public Slider metalSlider;

		// Token: 0x040012FF RID: 4863
		public Slider woodSlider;

		// Token: 0x04001300 RID: 4864
		public Slider foodSlider;

		// Token: 0x04001301 RID: 4865
		public GameObject leftPanel;

		// Token: 0x04001302 RID: 4866
		public GameObject rightPanel;

		// Token: 0x04001303 RID: 4867
		public GameObject turnInfo;

		// Token: 0x04001304 RID: 4868
		public GameObject mechRiverwalkOutline;

		// Token: 0x04001305 RID: 4869
		public GameObject UIBlocker;

		// Token: 0x04001306 RID: 4870
		public MatPlayerSectionPresenter[] playerMatSections;

		// Token: 0x04001307 RID: 4871
		public MatPlayerSectionPresenter factoryMatSection;

		// Token: 0x04001308 RID: 4872
		public GameObject factoryCardContainer;

		// Token: 0x04001309 RID: 4873
		public Animator[] statIcons1;

		// Token: 0x0400130A RID: 4874
		public Animator[] statIcons2;

		// Token: 0x0400130B RID: 4875
		public GameObject workerDeclineButton;

		// Token: 0x0400130C RID: 4876
		public Text MissionProgress;

		// Token: 0x0400130D RID: 4877
		public AnimationCurve camAnimationCurve;

		// Token: 0x0400130E RID: 4878
		public Button endMoveButton;

		// Token: 0x0400130F RID: 4879
		public TextMeshProUGUI endMoveText;

		// Token: 0x04001310 RID: 4880
		public TextMeshProUGUI endTurnFakeText;

		// Token: 0x04001311 RID: 4881
		public GameObject menuButton;

		// Token: 0x04001312 RID: 4882
		public TutorialArrowBoard[] arrowBoard;

		// Token: 0x04001313 RID: 4883
		public YesNoDialog notEnoughResourcesYesNo;

		// Token: 0x04001314 RID: 4884
		public GameObject optionsBonusInspection;

		// Token: 0x04001315 RID: 4885
		private float camXfrom;

		// Token: 0x04001316 RID: 4886
		private float camXto;

		// Token: 0x04001317 RID: 4887
		private float camYfrom;

		// Token: 0x04001318 RID: 4888
		private float camYto;

		// Token: 0x04001319 RID: 4889
		private float camZfrom;

		// Token: 0x0400131A RID: 4890
		private float camZto;

		// Token: 0x0400131B RID: 4891
		private float camAnimStartTime;

		// Token: 0x0400131C RID: 4892
		private CameraParams camParamsFrom;

		// Token: 0x0400131D RID: 4893
		private CameraParams camParamsTo;

		// Token: 0x0400131E RID: 4894
		private bool camAnim;

		// Token: 0x0400131F RID: 4895
		private float camAnimLength = 0.01f;

		// Token: 0x04001321 RID: 4897
		public Image DarkenUI;

		// Token: 0x04001322 RID: 4898
		private bool revertStep;

		// Token: 0x04001323 RID: 4899
		private int backSteps;
	}
}

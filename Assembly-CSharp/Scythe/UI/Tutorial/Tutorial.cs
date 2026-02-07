using System;
using System.Collections;
using Scythe.Analytics;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x02000508 RID: 1288
	public abstract class Tutorial : MonoBehaviour
	{
		// Token: 0x06002926 RID: 10534 RVA: 0x00042BCB File Offset: 0x00040DCB
		private void OnDestroy()
		{
			this.ClearEventListeners();
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x00042BD3 File Offset: 0x00040DD3
		public void Begin()
		{
			this.OnStart();
			this.Initialize();
			this.RegisterCustomCallbacks();
			this.Next();
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void OnStart()
		{
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void RegisterCustomCallbacks()
		{
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x00042BED File Offset: 0x00040DED
		protected virtual void ClearEventListeners()
		{
			GameController.HexGetFocused -= this.OnHexGetFocused;
			GameController.UnitGetFocused -= this.OnUnitGetFocused;
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x00042C11 File Offset: 0x00040E11
		protected void Next()
		{
			base.StartCoroutine(this.NextCoroutine());
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x00042C20 File Offset: 0x00040E20
		protected void Minimize()
		{
			this.screens[this.currentScreenId].Hide();
			SingletonMono<InputBlockerController>.Instance.Reset();
			SingletonMono<HighlightController>.Instance.Reset();
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x00042C48 File Offset: 0x00040E48
		protected void SetShow(int screenId, Action behaviour)
		{
			this.customShowCallbacks[screenId] = behaviour;
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x00042C53 File Offset: 0x00040E53
		protected void SetHide(int screenId, Action behaviour)
		{
			this.customHideCallbacks[screenId] = behaviour;
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x00042C5E File Offset: 0x00040E5E
		protected void SetNextButton(int screenId, Button button, int clickCount)
		{
			this.SetNextButton(screenId, button, HighlightController.HighlightStyle.Rectangle, clickCount);
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x00042C6A File Offset: 0x00040E6A
		protected void SetNextButton(int screenId, Button button, HighlightController.HighlightStyle highlightStyle = HighlightController.HighlightStyle.Rectangle, int clickCount = 1)
		{
			this.customNextButtons[screenId] = new Tuple<Button, int, HighlightController.HighlightStyle>(button, clickCount, highlightStyle);
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x00042C7D File Offset: 0x00040E7D
		protected void SetHexPresentation(int screenId, params GameHex[] hexes)
		{
			this.customHexPresentations[screenId] = hexes;
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x000EC004 File Offset: 0x000EA204
		protected void SetUnitSelection(int screenId, params Unit[] units)
		{
			this.SetUnitSelection(screenId, () => units);
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x00042C88 File Offset: 0x00040E88
		protected void SetUnitSelection(int screenId, Func<Unit[]> unitSelection)
		{
			this.customUnitSelections[screenId] = unitSelection;
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x00042C93 File Offset: 0x00040E93
		protected void SetHexSelection(int screenId, int clickCount = 1, params GameHex[] hexes)
		{
			this.customHexSelections[screenId] = new Tuple<GameHex[], int>(hexes, clickCount);
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x00042CA4 File Offset: 0x00040EA4
		protected void SetEndTurn(int screenId)
		{
			this.customEndTurn[screenId] = true;
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x000EC034 File Offset: 0x000EA234
		private void Initialize()
		{
			this.customShowCallbacks = new Action[this.screens.Length];
			this.customHideCallbacks = new Action[this.screens.Length];
			this.customNextButtons = new Tuple<Button, int, HighlightController.HighlightStyle>[this.screens.Length];
			this.customHexPresentations = new GameHex[this.screens.Length][];
			this.customUnitSelections = new Func<Unit[]>[this.screens.Length];
			this.customHexSelections = new Tuple<GameHex[], int>[this.screens.Length];
			this.customEndTurn = new bool[this.screens.Length];
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x00042CAF File Offset: 0x00040EAF
		private IEnumerator NextCoroutine()
		{
			yield return null;
			this.HideCurrentScreen();
			this.currentScreenId++;
			this.ShowCurrentScreen();
			yield break;
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x000EC0C8 File Offset: 0x000EA2C8
		private void ShowCurrentScreen()
		{
			if (this.currentScreenId < this.screens.Length)
			{
				this.screens[this.currentScreenId].Show(new Action(this.Next));
				this.TryActivateCustomCallbacks();
				if (this.customShowCallbacks[this.currentScreenId] != null)
				{
					this.customShowCallbacks[this.currentScreenId]();
					return;
				}
			}
			else
			{
				SingletonMono<TutorialEndScreen>.Instance.Show();
			}
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x000EC138 File Offset: 0x000EA338
		private void HideCurrentScreen()
		{
			if (this.currentScreenId > -1)
			{
				this.OnScreenChanged(this.currentScreenId);
				this.AnalyticsStepLogging(StepStatuses.completed, 0);
				this.screens[this.currentScreenId].Hide();
				if (this.customHideCallbacks[this.currentScreenId] != null)
				{
					this.customHideCallbacks[this.currentScreenId]();
				}
				SingletonMono<InputBlockerController>.Instance.Reset();
				SingletonMono<HighlightController>.Instance.Reset();
			}
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x00042CBE File Offset: 0x00040EBE
		public bool IsTutorialFinished(int currentScreenId)
		{
			return currentScreenId >= this.screens.Length;
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void OnScreenChanged(int currentScreenId)
		{
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x00042CCE File Offset: 0x00040ECE
		private void TryActivateCustomCallbacks()
		{
			this.TryActivateCustomNextButton();
			this.TryActivateCustomHexPresentation();
			this.TryActivateCustomUnitSelection();
			this.TryActivateCustomHexSelection();
			this.TryActivateCustomEndTurn();
		}

		// Token: 0x0600293D RID: 10557 RVA: 0x000EC1AC File Offset: 0x000EA3AC
		private void TryActivateCustomNextButton()
		{
			if (this.currentScreenId > -1 && this.customNextButtons[this.currentScreenId] != null)
			{
				SingletonMono<HighlightController>.Instance.HighlightUI(this.customNextButtons[this.currentScreenId].Item1, this.customNextButtons[this.currentScreenId].Item3);
				SingletonMono<InputBlockerController>.Instance.UnblockUI(this.customNextButtons[this.currentScreenId].Item1);
				this.customNextButtons[this.currentScreenId].Item1.onClick.AddListener(new UnityAction(this.OnCustomNextButtonClicked));
				this.clickCounter = this.customNextButtons[this.currentScreenId].Item2;
			}
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x000EC264 File Offset: 0x000EA464
		private void TryActivateCustomHexPresentation()
		{
			if (this.currentScreenId > 0 && this.customHexPresentations[this.currentScreenId] != null)
			{
				SingletonMono<HighlightController>.Instance.HighlightHexesArrow(this.customHexPresentations[this.currentScreenId]);
				SingletonMono<CameraAnimationController>.Instance.AnimateToAllHexes(this.customHexPresentations[this.currentScreenId]);
			}
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x000EC2B8 File Offset: 0x000EA4B8
		private void TryActivateCustomUnitSelection()
		{
			if (this.currentScreenId > 0 && this.customUnitSelections[this.currentScreenId] != null)
			{
				Unit[] array = this.customUnitSelections[this.currentScreenId]();
				SingletonMono<CameraAnimationController>.Instance.AnimateToAllUnits(array);
				SingletonMono<HighlightController>.Instance.HighlightUnitsArrow(array);
				SingletonMono<InputBlockerController>.Instance.UnblockUnits(array);
				GameController.UnitGetFocused += this.OnUnitGetFocused;
			}
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x000EC324 File Offset: 0x000EA524
		private void TryActivateCustomHexSelection()
		{
			if (this.currentScreenId > 0 && this.customHexSelections[this.currentScreenId] != null)
			{
				SingletonMono<CameraAnimationController>.Instance.AnimateToAllHexes(this.customHexSelections[this.currentScreenId].Item1);
				SingletonMono<HighlightController>.Instance.HighlightHexesArrow(this.customHexSelections[this.currentScreenId].Item1);
				SingletonMono<InputBlockerController>.Instance.UnblockHexes(this.customHexSelections[this.currentScreenId].Item1);
				GameController.HexGetFocused += this.OnHexGetFocused;
				this.clickCounter = this.customHexSelections[this.currentScreenId].Item2;
			}
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x000EC3CC File Offset: 0x000EA5CC
		private void TryActivateCustomEndTurn()
		{
			if (this.currentScreenId > 0 && this.customEndTurn[this.currentScreenId])
			{
				SingletonMono<HighlightController>.Instance.HighlightUI(GameController.Instance.endTurnButton, HighlightController.HighlightStyle.Rectangle);
				SingletonMono<InputBlockerController>.Instance.UnblockUI(GameController.Instance.endTurnButton);
				GameController.AfterEndTurnAIAndPlayer += this.OnTurnEnded;
			}
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x000EC42C File Offset: 0x000EA62C
		private void OnCustomNextButtonClicked()
		{
			this.clickCounter--;
			if (this.clickCounter < 1)
			{
				this.customNextButtons[this.currentScreenId].Item1.onClick.RemoveListener(new UnityAction(this.OnCustomNextButtonClicked));
				this.Next();
			}
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x00042CEE File Offset: 0x00040EEE
		private void OnHexGetFocused(GameHexPresenter hexPresenter)
		{
			this.clickCounter--;
			if (this.clickCounter < 1)
			{
				this.Next();
				GameController.HexGetFocused -= this.OnHexGetFocused;
			}
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x00042D1E File Offset: 0x00040F1E
		private void OnUnitGetFocused(UnitPresenter unitPresenter)
		{
			this.Next();
			GameController.UnitGetFocused -= this.OnUnitGetFocused;
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x00042D37 File Offset: 0x00040F37
		private void OnTurnEnded()
		{
			if (GameController.GameManager.PlayerCurrent.IsHuman)
			{
				GameController.AfterEndTurnAIAndPlayer -= this.OnTurnEnded;
				this.Next();
				return;
			}
			this.Minimize();
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x000EC480 File Offset: 0x000EA680
		protected void AnalyticsStepLogging(StepStatuses stepStatus, int revertSteps = 0)
		{
			if (this.freezeAnalyticsSteps)
			{
				return;
			}
			AnalyticsEventData.TutorialStepStoped();
			if (this.SkipCurrentState(AnalyticsEventData.CurrentTutorialStepId()))
			{
				AnalyticsEventData.SetCurrentStep(AnalyticsEventData.CurrentTutorialStepId() + 1);
			}
			if (this.IsTutorialFinished(this.currentScreenId + 1))
			{
				AnalyticsEventData.TutorialFinished();
			}
			AnalyticsEventLogger.Instance.LogTutorialStep(stepStatus);
			AnalyticsEventData.ResetTutorialStepTimer();
			AnalyticsEventData.TutorialStepStarted();
			AnalyticsEventData.RevertTutorialStepBy(revertSteps);
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x0002A1D9 File Offset: 0x000283D9
		protected virtual bool SkipCurrentState(StepIDs step)
		{
			return false;
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x00042D68 File Offset: 0x00040F68
		protected bool IsDesktopBuild()
		{
			return PlatformManager.IsSteam;
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x00042D6F File Offset: 0x00040F6F
		protected void FreezeAnalyticsStep()
		{
			this.freezeAnalyticsSteps = true;
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x00042D78 File Offset: 0x00040F78
		protected void UnfreezeAnalyticsStep()
		{
			this.freezeAnalyticsSteps = false;
		}

		// Token: 0x04001D71 RID: 7537
		[SerializeField]
		private TutorialScreen[] screens;

		// Token: 0x04001D72 RID: 7538
		private int currentScreenId = -1;

		// Token: 0x04001D73 RID: 7539
		private int clickCounter;

		// Token: 0x04001D74 RID: 7540
		private Action[] customShowCallbacks;

		// Token: 0x04001D75 RID: 7541
		private Action[] customHideCallbacks;

		// Token: 0x04001D76 RID: 7542
		private Tuple<Button, int, HighlightController.HighlightStyle>[] customNextButtons;

		// Token: 0x04001D77 RID: 7543
		private GameHex[][] customHexPresentations;

		// Token: 0x04001D78 RID: 7544
		private Func<Unit[]>[] customUnitSelections;

		// Token: 0x04001D79 RID: 7545
		private Tuple<GameHex[], int>[] customHexSelections;

		// Token: 0x04001D7A RID: 7546
		private bool[] customEndTurn;

		// Token: 0x04001D7B RID: 7547
		private bool freezeAnalyticsSteps;
	}
}

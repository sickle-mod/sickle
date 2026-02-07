using System;
using System.Collections.Generic;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000498 RID: 1176
	public class ObjectivesPreview : MonoBehaviour
	{
		// Token: 0x06002552 RID: 9554 RVA: 0x0003FB79 File Offset: 0x0003DD79
		private void Awake()
		{
			this.openObjectivesButton.onClick.AddListener(new UnityAction(this.OnOpenObjectivesButtonClicked));
			this.dismissButton.onClick.AddListener(new UnityAction(this.OnDismissButtonClicked));
			this.Clear();
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000DE4DC File Offset: 0x000DC6DC
		private void Start()
		{
			GameController.GameManager.ObjectiveCardGetHighlighted += this.UpdateObjectivePreview;
			GameController.AfterEndTurnAIAndPlayer += this.ClearDismissedObjectiveCards;
			if (GameController.GameManager.IsHotSeat)
			{
				GameController.AfterEndTurnAIAndPlayer += this.UpdatePreviewVisibility;
			}
			this.UpdatePreviewVisibility();
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000DE534 File Offset: 0x000DC734
		private void OnDestroy()
		{
			if (GameController.GameManager != null)
			{
				GameController.GameManager.ObjectiveCardGetHighlighted -= this.UpdateObjectivePreview;
				if (GameController.GameManager.IsHotSeat)
				{
					GameController.AfterEndTurnAIAndPlayer -= this.UpdatePreviewVisibility;
				}
			}
			GameController.AfterEndTurnAIAndPlayer -= this.ClearDismissedObjectiveCards;
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x000DE58C File Offset: 0x000DC78C
		private void UpdatePreviewVisibility()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				if (GameController.GameManager.SpectatorMode)
				{
					this.Hide();
					return;
				}
				this.Show();
				return;
			}
			else
			{
				if (GameController.GameManager.PlayerCurrent.IsHuman)
				{
					this.Show();
					return;
				}
				this.Hide();
				return;
			}
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x0003FBB9 File Offset: 0x0003DDB9
		private void Show()
		{
			this.openObjectivesButton.gameObject.SetActive(true);
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x0003FBCC File Offset: 0x0003DDCC
		private void Hide()
		{
			this.openObjectivesButton.gameObject.SetActive(false);
			this.objectiveTitleText.gameObject.SetActive(false);
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x0003FBF0 File Offset: 0x0003DDF0
		private void ClearDismissedObjectiveCards()
		{
			this.dismissedObjectiveCards.Clear();
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x0003FBFD File Offset: 0x0003DDFD
		private void UpdateObjectivePreview(int index, bool focus)
		{
			this.Clear();
			this.CheckObjectives();
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000DE5E0 File Offset: 0x000DC7E0
		public void CheckObjectives()
		{
			if (GameController.GameManager.IsMultiplayer && GameController.GameManager.SpectatorMode)
			{
				return;
			}
			Player player = this.FindCurrentUIPlayer();
			if (player == null)
			{
				return;
			}
			foreach (ObjectiveCard objectiveCard in player.objectiveCards)
			{
				if (this.highlightedObjectiveCard == null && !this.dismissedObjectiveCards.Contains(objectiveCard) && objectiveCard.status == ObjectiveCard.ObjectiveStatus.Open && objectiveCard.CheckCondition() && player.CanStillDoneTheObjective() && (objectiveCard.CanDoActionAfterCompletingObjective() || player.PlayerMatSectionSelected()))
				{
					this.Highlight(objectiveCard);
				}
			}
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x000DE698 File Offset: 0x000DC898
		private Player FindCurrentUIPlayer()
		{
			if (GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerOwner != null)
			{
				return GameController.GameManager.PlayerOwner;
			}
			if (GameController.GameManager.PlayerCurrent == null)
			{
				return null;
			}
			if (!GameController.GameManager.PlayerCurrent.IsHuman)
			{
				return GameController.GameManager.GetPreviousHumanPlayer();
			}
			return GameController.GameManager.PlayerCurrent;
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x000DE6FC File Offset: 0x000DC8FC
		private void Highlight(ObjectiveCard objectiveCard)
		{
			this.openObjectivesButtonHighlight.gameObject.SetActive(true);
			this.objectiveTitleText.text = GameController.GetObjectiveTitle(objectiveCard.CardId);
			this.objectiveTitleText.gameObject.SetActive(true);
			this.dismissButton.gameObject.SetActive(true);
			this.highlightedObjectiveCard = objectiveCard;
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x0003FC0B File Offset: 0x0003DE0B
		private void Dismiss()
		{
			if (this.highlightedObjectiveCard != null)
			{
				this.dismissedObjectiveCards.Add(this.highlightedObjectiveCard);
				this.highlightedObjectiveCard = null;
			}
			this.Clear();
			this.CheckObjectives();
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x0003FC39 File Offset: 0x0003DE39
		public void Clear()
		{
			this.openObjectivesButtonHighlight.gameObject.SetActive(false);
			this.objectiveTitleText.gameObject.SetActive(false);
			this.dismissButton.gameObject.SetActive(false);
			this.highlightedObjectiveCard = null;
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x0003FC75 File Offset: 0x0003DE75
		private void OnOpenObjectivesButtonClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_objective_preview_button);
			WorldSFXManager.PlaySound(SoundEnum.GuiUpperBeltClick, AudioSourceType.Buttons);
			SingletonMono<TopMenuPanelsManager>.Instance.ShowPanel(5);
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x0003FC91 File Offset: 0x0003DE91
		private void OnDismissButtonClicked()
		{
			this.Dismiss();
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x0003FC99 File Offset: 0x0003DE99
		public void OnUndo()
		{
			this.Clear();
			this.OnDestroy();
			this.Start();
		}

		// Token: 0x04001A2E RID: 6702
		[SerializeField]
		private Button openObjectivesButton;

		// Token: 0x04001A2F RID: 6703
		[SerializeField]
		private Image openObjectivesButtonHighlight;

		// Token: 0x04001A30 RID: 6704
		[SerializeField]
		private TextMeshProUGUI objectiveTitleText;

		// Token: 0x04001A31 RID: 6705
		[SerializeField]
		private Button dismissButton;

		// Token: 0x04001A32 RID: 6706
		private ObjectiveCard highlightedObjectiveCard;

		// Token: 0x04001A33 RID: 6707
		private List<ObjectiveCard> dismissedObjectiveCards = new List<ObjectiveCard>();
	}
}

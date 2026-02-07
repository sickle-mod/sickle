using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004A0 RID: 1184
	public class TopMenuObjectivesPresenter : MonoBehaviour
	{
		// Token: 0x06002591 RID: 9617 RVA: 0x0003FEA0 File Offset: 0x0003E0A0
		private void Awake()
		{
			OptionsManager.OnLanguageChanged += this.UpdateTexts;
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x0003FEB3 File Offset: 0x0003E0B3
		private void OnDestroy()
		{
			OptionsManager.OnLanguageChanged -= this.UpdateTexts;
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x0003FEC6 File Offset: 0x0003E0C6
		public void OnToggleClicked()
		{
			if (!GameController.GameManager.IsCampaign)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.tab_objectives_toggle);
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x0003FEE7 File Offset: 0x0003E0E7
		private void OnEnable()
		{
			this.UpdateTexts();
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.tab_objectives, Contexts.ingame);
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x0003FEFC File Offset: 0x0003E0FC
		public void CompleteObjective(int index)
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_complete_objective_button);
			GameController.Instance.CompleteObjectiveWithWarning(index);
			SingletonMono<TopMenuPanelsManager>.Instance.DisableObjectiveCards();
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x0003FF1A File Offset: 0x0003E11A
		public void UpdatePlayerInfo(List<ObjectiveCard> objectiveCards)
		{
			this.SetObjectiveCards(objectiveCards);
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000DF9AC File Offset: 0x000DDBAC
		public void SetObjectiveCards(List<ObjectiveCard> objectiveCards)
		{
			this.objectiveCards = objectiveCards;
			this.UpdateObjectivesTexts(objectiveCards);
			for (int i = 0; i < objectiveCards.Count; i++)
			{
				TextMeshProUGUI component = this.objectiveCardButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
				this.objectiveCardButton[i].interactable = false;
				component.color = this.disabledOpacity;
				this.objectiveCardButtonImage[i].color = this.disabledOpacity;
				if (objectiveCards[i].status == ObjectiveCard.ObjectiveStatus.Completed)
				{
					this.objectiveStatePlateText[i].text = ScriptLocalization.Get("GameScene/Completed");
					this.objectiveStatePlateText[i].color = this.completeLabelColor;
					this.objectiveCardButtonImage[i].color = this.disabledOpacity;
					component.color = this.disabledOpacity;
					this.objectiveStatePlate[i].SetActive(true);
				}
				else if (objectiveCards[i].status == ObjectiveCard.ObjectiveStatus.Disabled)
				{
					this.objectiveStatePlateText[i].text = ScriptLocalization.Get("GameScene/Obsolete");
					this.objectiveStatePlateText[i].color = this.obsoleteLabelColor;
					this.objectiveCardButtonImage[i].color = this.disabledOpacity;
					component.color = this.disabledOpacity;
					this.objectiveStatePlate[i].SetActive(true);
				}
				else if (objectiveCards[i].status == ObjectiveCard.ObjectiveStatus.Open)
				{
					this.objectiveStatePlate[i].SetActive(false);
				}
				Color color = this.objectiveCardTitle[i].color;
				if (objectiveCards[i].status == ObjectiveCard.ObjectiveStatus.Open)
				{
					color.a = 1f;
				}
				else
				{
					color.a = 0.4f;
				}
				this.objectiveCardTitle[i].color = color;
			}
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x000DFB58 File Offset: 0x000DDD58
		public void UpdateObjectiveButtonLabels(List<ObjectiveCard> objectiveCards, int objectiveIndex)
		{
			TextMeshProUGUI component = this.objectiveCardButton[objectiveIndex].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
			if (objectiveIndex >= objectiveCards.Count || objectiveCards[objectiveIndex].CanDoActionAfterCompletingObjective())
			{
				component.text = ScriptLocalization.Get("GameScene/Complete");
				return;
			}
			if (GameController.GameManager.PlayerCurrent.currentMatSection == -1 && objectiveCards[objectiveIndex].status == ObjectiveCard.ObjectiveStatus.Open && objectiveCards[objectiveIndex].CheckCondition())
			{
				component.text = ScriptLocalization.Get("GameScene/BlockedObjectiveInfo");
				return;
			}
			component.text = ScriptLocalization.Get("GameScene/Complete") + " & " + ScriptLocalization.Get("GameScene/EndTurn");
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x000DFC0C File Offset: 0x000DDE0C
		public bool IsAnyObjectiveReady()
		{
			return GameController.GameManager.PlayerCurrent.objectiveCards.Count != 0 && ((GameController.GameManager.PlayerCurrent.objectiveCards[0].CheckCondition() && GameController.GameManager.PlayerCurrent.objectiveCards[0].status == ObjectiveCard.ObjectiveStatus.Open) || (GameController.GameManager.PlayerCurrent.objectiveCards[1].CheckCondition() && GameController.GameManager.PlayerCurrent.objectiveCards[0].status == ObjectiveCard.ObjectiveStatus.Open));
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x000DFCA8 File Offset: 0x000DDEA8
		public void FocusObjectiveCard(int index, bool focus)
		{
			if (GameController.GameManager.GameFinished)
			{
				return;
			}
			Player player;
			if (GameController.GameManager.IsMultiplayer)
			{
				player = GameController.GameManager.PlayerOwner;
			}
			else if (!GameController.GameManager.PlayerCurrent.IsHuman)
			{
				player = GameController.GameManager.GetPreviousHumanPlayer();
			}
			else
			{
				player = GameController.GameManager.PlayerCurrent;
			}
			this.UpdateObjectiveButtonLabels(player.objectiveCards, index);
			this.objectiveCardButton[index].interactable = focus;
			if (focus)
			{
				this.objectiveCardButtonImage[index].color = this.enabledOpacity;
				this.objectiveCardButton[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = this.enabledOpacity;
			}
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x000DFD5C File Offset: 0x000DDF5C
		public void FocusAllObjectiveCards(bool focus)
		{
			for (int i = 0; i < this.objectiveCardPanel.Length; i++)
			{
				this.FocusObjectiveCard(i, focus);
			}
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x000DFD84 File Offset: 0x000DDF84
		public void DisableEndTurnObjectives()
		{
			for (int i = 0; i < this.objectiveCardPanel.Length; i++)
			{
				if (this.objectiveCards != null && this.objectiveCards.Count > 0 && !this.objectiveCards[i].CanDoActionAfterCompletingObjective())
				{
					this.FocusObjectiveCard(i, false);
				}
			}
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x000DFDD8 File Offset: 0x000DDFD8
		private void UpdateObjectivesTexts(List<ObjectiveCard> objectiveCards)
		{
			for (int i = 0; i < objectiveCards.Count; i++)
			{
				this.objectiveCardTitle[i].text = GameController.GetObjectiveTitle(objectiveCards[i].CardId);
				this.objectiveCardText[i].text = GameController.GetObjectiveDescription(objectiveCards[i].CardId);
				string text = objectiveCards[i].CardId.ToString();
				if (this.objectiveCardImage[i].sprite == null || !this.objectiveCardImage[i].sprite.name.EndsWith(text))
				{
					AssetBundle assetBundle = AssetBundleManager.LoadAssetBundle("graphic_objectivesmobileingame");
					this.objectiveCardImage[i].sprite = assetBundle.LoadAllAssets<Sprite>()[objectiveCards[i].CardId - 1];
				}
				this.UpdateObjectiveButtonLabels(objectiveCards, i);
			}
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x000DFEB4 File Offset: 0x000DE0B4
		private void UpdateTexts()
		{
			Player player;
			if (GameController.GameManager.IsMultiplayer)
			{
				player = GameController.GameManager.PlayerOwner;
			}
			else if (!GameController.GameManager.PlayerCurrent.IsHuman)
			{
				player = GameController.GameManager.GetPreviousHumanPlayer();
			}
			else
			{
				player = GameController.GameManager.PlayerCurrent;
			}
			this.UpdateObjectivesTexts(player.objectiveCards);
			for (int i = 0; i < this.objectiveCards.Count; i++)
			{
				if (this.objectiveCards[i].status == ObjectiveCard.ObjectiveStatus.Completed)
				{
					this.objectiveStatePlateText[i].text = ScriptLocalization.Get("GameScene/Completed");
				}
				else if (this.objectiveCards[i].status == ObjectiveCard.ObjectiveStatus.Disabled)
				{
					this.objectiveStatePlateText[i].text = ScriptLocalization.Get("GameScene/Obsolete");
				}
			}
		}

		// Token: 0x04001A6A RID: 6762
		public ObjectivesPreview objectivesPreview;

		// Token: 0x04001A6B RID: 6763
		[SerializeField]
		private Toggle2 objectiveToggle;

		// Token: 0x04001A6C RID: 6764
		[SerializeField]
		private TextMeshProUGUI[] objectiveCardTitle = new TextMeshProUGUI[2];

		// Token: 0x04001A6D RID: 6765
		[SerializeField]
		private Text[] objectiveCardText = new Text[2];

		// Token: 0x04001A6E RID: 6766
		[SerializeField]
		private Button[] objectiveCardButton = new Button[2];

		// Token: 0x04001A6F RID: 6767
		[SerializeField]
		private GameObject[] objectiveStatePlate = new GameObject[2];

		// Token: 0x04001A70 RID: 6768
		[SerializeField]
		private TextMeshProUGUI[] objectiveStatePlateText = new TextMeshProUGUI[2];

		// Token: 0x04001A71 RID: 6769
		[SerializeField]
		private GameObject[] objectiveCardPanel = new GameObject[2];

		// Token: 0x04001A72 RID: 6770
		[SerializeField]
		private Image[] objectiveCardImage = new Image[2];

		// Token: 0x04001A73 RID: 6771
		[SerializeField]
		private Color completeLabelColor;

		// Token: 0x04001A74 RID: 6772
		[SerializeField]
		private Color obsoleteLabelColor;

		// Token: 0x04001A75 RID: 6773
		[SerializeField]
		private Image[] objectiveCardButtonImage = new Image[2];

		// Token: 0x04001A76 RID: 6774
		private List<ObjectiveCard> objectiveCards;

		// Token: 0x04001A77 RID: 6775
		private Color disabledOpacity = new Color(1f, 1f, 1f, 0.27f);

		// Token: 0x04001A78 RID: 6776
		private Color enabledOpacity = new Color(1f, 1f, 1f, 1f);
	}
}

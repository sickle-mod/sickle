using System;
using System.Collections.Generic;
using DG.Tweening;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003E1 RID: 993
	public class EnlistPresenter : ActionPresenter
	{
		// Token: 0x140000BE RID: 190
		// (add) Token: 0x06001D7F RID: 7551 RVA: 0x000B6BF4 File Offset: 0x000B4DF4
		// (remove) Token: 0x06001D80 RID: 7552 RVA: 0x000B6C28 File Offset: 0x000B4E28
		public static event EnlistPresenter.BonusSelect BonusSelected;

		// Token: 0x140000BF RID: 191
		// (add) Token: 0x06001D81 RID: 7553 RVA: 0x000B6C5C File Offset: 0x000B4E5C
		// (remove) Token: 0x06001D82 RID: 7554 RVA: 0x000B6C90 File Offset: 0x000B4E90
		public static event EnlistPresenter.RecruitSelect RecruitSelected;

		// Token: 0x06001D83 RID: 7555 RVA: 0x000B6CC4 File Offset: 0x000B4EC4
		public void Awake()
		{
			if (!PlatformManager.IsStandalone)
			{
				this.recruitSelectionPanelLayoutElement = this.recruitSelectionPanelMobile.GetComponent<LayoutElement>();
				this.originalRecruitSelectionAnchoredPosition = this.recruitSelectionPanelRect.anchoredPosition;
				this.originalRecruitSelectionAnchoredPosition.y = -30.5f;
				this.originalRecruitSelectionSizeDelta = this.recruitSelectionPanelRect.sizeDelta;
			}
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x0003B294 File Offset: 0x00039494
		private void Update()
		{
			if ((Input.anyKeyDown || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)) && this.panelAnimationTween != null)
			{
				this.panelAnimationTween.Complete(true);
			}
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x000B6D1C File Offset: 0x000B4F1C
		public override void ChangeLayoutForAction(BaseAction action)
		{
			this.action = action as GainRecruit;
			this.sectionID = -1;
			this.bonusID = -1;
			if (this.guidelineMain != null && GameController.GameManager.PlayerCurrent.currentMatSection != 4)
			{
				this.guidelineMain.enabled = true;
			}
			GameController.Instance.matFaction.ClearHintStories();
			this.EnableInput();
			this.openedEarlier = PlatformManager.IsStandalone && this.toggleRecruit.isOn;
			this.AssistHighlight(1);
			if (PlatformManager.IsStandalone)
			{
				this.hintAction.SetActive(true);
				if (OptionsManager.IsActionAssist())
				{
					GameController.Instance.MaximizePlayerMat();
					return;
				}
			}
			else
			{
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
				this.UpdateEnemyEnlistStatusMobile();
				this.recruitSelectionPanelMobile.SetActive(true);
				if (OptionsManager.IsConfirmActions())
				{
					this.AnimateRecruitSelectionPanel();
				}
			}
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x000B6DFC File Offset: 0x000B4FFC
		private void AnimateRecruitSelectionPanel()
		{
			Vector2 vector = new Vector2(266f, -140f);
			this.recruitSelectionPanelLayoutElement.ignoreLayout = true;
			this.SetButtonsInteractable(false);
			this.recruitSelectionPanelRect.anchoredPosition = vector;
			this.recruitSelectionPanelRect.localScale = Vector3.zero;
			Canvas.ForceUpdateCanvases();
			this.panelAnimationTween = DOTween.Sequence();
			this.panelAnimationTween.Append(this.recruitSelectionPanelRect.DOScale(1f, this.popupAnimationSpeed).SetEase(Ease.InOutCubic));
			this.panelAnimationTween.Join(this.recruitSelectionPanelRect.DOAnchorPos(this.originalRecruitSelectionAnchoredPosition, this.popupAnimationSpeed, false).SetEase(Ease.InOutCubic));
			this.panelAnimationTween.OnComplete(delegate
			{
				this.OnAnimationComplete();
			});
			this.panelAnimationTween.Play<Sequence>();
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x000B6ED4 File Offset: 0x000B50D4
		private void OnAnimationComplete()
		{
			this.SetButtonsInteractable(true);
			this.recruitSelectionPanelRect.sizeDelta = this.originalRecruitSelectionSizeDelta;
			this.recruitSelectionPanelLayoutElement.ignoreLayout = false;
			this.bonusSelectionPanelMobile.GetComponent<CanvasGroup>().alpha = 1f;
			this.RefreshButtonsMobile();
			this.panelAnimationTween = null;
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x000B6F28 File Offset: 0x000B5128
		private void SetButtonsInteractable(bool interactable)
		{
			for (int i = 0; i < this.recruitSelectionButtonsMobile.Length; i++)
			{
				this.recruitSelectionButtonsMobile[i].interactable = interactable;
			}
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x000B6F58 File Offset: 0x000B5158
		private void UpdateEnemyEnlistStatusMobile()
		{
			List<Player> playerNeighbours = GameController.GameManager.GetPlayerNeighbours(this.action.GetPlayer());
			for (int i = 0; i < 4; i++)
			{
				DownActionType downActionType = (DownActionType)i;
				Image image = this.recruitSelectionButtonRecruitNeighbourRightMobile[i];
				if (playerNeighbours.Count > 0)
				{
					image.enabled = true;
					image.sprite = GameController.factionInfo[playerNeighbours[0].matFaction.faction].logo;
					if (playerNeighbours[0].matPlayer.GetDownAction(downActionType).IsRecruitEnlisted)
					{
						image.material = null;
					}
					else
					{
						image.material = this.recruitNeighbourInactive;
					}
				}
				else
				{
					image.enabled = false;
				}
				Image image2 = this.recruitSelectionButtonRecruitNeighbourLeftMobile[i];
				if (playerNeighbours.Count > 1)
				{
					image2.enabled = true;
					image2.sprite = GameController.factionInfo[playerNeighbours[1].matFaction.faction].logo;
					if (playerNeighbours[1].matPlayer.GetDownAction(downActionType).IsRecruitEnlisted)
					{
						image2.material = null;
					}
					else
					{
						image2.material = this.recruitNeighbourInactive;
					}
				}
				else
				{
					image2.enabled = false;
				}
			}
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x000B7084 File Offset: 0x000B5284
		public void AssistHighlight(int step)
		{
			for (int i = 0; i < this.assistIcon.Length; i++)
			{
				this.assistHighlight[i].SetActive(i == step);
				this.assistIcon[i].color = ((i == step) ? Color.white : Color.black);
			}
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x0003B2CB File Offset: 0x000394CB
		public void OnBonusHighlighted(int bonusID)
		{
			this.bonusID = bonusID;
			this.RefreshButtonsMobile();
			if (!PlatformManager.IsStandalone)
			{
				this.confirmButtonMobile.GetComponent<CanvasGroup>().alpha = 1f;
			}
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x000B70D4 File Offset: 0x000B52D4
		public void OnBonusSelected(int bonusID)
		{
			if (this.enlistStep == EnlistPresenter.EnlistStep.selectBonus)
			{
				this.enlistStep = EnlistPresenter.EnlistStep.none;
				GainType gainType;
				if (bonusID == 0)
				{
					gainType = GainType.Coin;
				}
				else if (bonusID == 1)
				{
					gainType = GainType.Popularity;
				}
				else if (bonusID == 2)
				{
					gainType = GainType.Power;
				}
				else
				{
					gainType = GainType.CombatCard;
				}
				this.oneTimeBonus = this.action.GetPlayer().matFaction.GetOneTimeBonus(gainType);
				if (this.action.GetPlayer().automaticGain.ContainsKey(gainType) && this.action.GetPlayer().automaticGain[gainType])
				{
					GameController.GameManager.actionManager.HandleAutomaticGain(this.oneTimeBonus);
					this.OnNestedActionEnded();
				}
				else
				{
					this.DisableInput();
					this.amountPresenter.SetActive(true);
					this.amountPresenter.GetComponent<ActionPresenter>().ChangeLayoutForAction(this.oneTimeBonus, this);
				}
				if (EnlistPresenter.BonusSelected != null)
				{
					EnlistPresenter.BonusSelected(gainType);
				}
			}
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x000B71B4 File Offset: 0x000B53B4
		public override void OnNestedActionEnded()
		{
			if (this.action.SetSectionAndBonus(GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(this.sectionID).ActionDown.Type, this.oneTimeBonus))
			{
				this.OnActionEnded();
				return;
			}
			Debug.LogError("Couldn't execute action");
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x000B720C File Offset: 0x000B540C
		public void OnRecruitSelected(int sectionID)
		{
			this.sectionID = sectionID;
			this.enlistStep = EnlistPresenter.EnlistStep.selectBonus;
			switch (sectionID)
			{
			case 0:
				WorldSFXManager.PlaySound(SoundEnum.EnlistGainPowerClick, AudioSourceType.Buttons);
				break;
			case 1:
				WorldSFXManager.PlaySound(SoundEnum.EnlistEarnCashClick, AudioSourceType.Buttons);
				break;
			case 2:
				WorldSFXManager.PlaySound(SoundEnum.EnlistPopularityClick, AudioSourceType.Buttons);
				break;
			case 3:
				WorldSFXManager.PlaySound(SoundEnum.EnlistBolsterAmmoClick, AudioSourceType.Buttons);
				break;
			}
			GameController.Instance.matFaction.OneTimeBonusShow(0, !GameController.GameManager.PlayerCurrent.matFaction.OneTimeBonusUsed(GainType.Coin));
			GameController.Instance.matFaction.OneTimeBonusShow(1, !GameController.GameManager.PlayerCurrent.matFaction.OneTimeBonusUsed(GainType.Popularity));
			GameController.Instance.matFaction.OneTimeBonusShow(2, !GameController.GameManager.PlayerCurrent.matFaction.OneTimeBonusUsed(GainType.Power));
			GameController.Instance.matFaction.OneTimeBonusShow(3, !GameController.GameManager.PlayerCurrent.matFaction.OneTimeBonusUsed(GainType.CombatCard));
			if (!this.openedEarlier && PlatformManager.IsStandalone)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.tab_enlist, Screens.in_game, Contexts.ingame);
			}
			for (int i = 0; i < 4; i++)
			{
				MatPlayerSectionPresenter matPlayerSectionPresenter = GameController.Instance.matPlayer.matSection[i];
				if (i != sectionID)
				{
					matPlayerSectionPresenter.downActionPresenter.recruitButton.image.color = this.glowDefault1;
				}
				else
				{
					matPlayerSectionPresenter.downActionPresenter.recruitButton.image.color = this.glowSelected1;
				}
			}
			if (EnlistPresenter.RecruitSelected != null)
			{
				EnlistPresenter.RecruitSelected();
			}
			this.AssistHighlight(2);
			if (PlatformManager.IsStandalone)
			{
				this.toggleRecruit.isOn = true;
				this.guideline2nd.enabled = true;
				if (OptionsManager.IsActionAssist())
				{
					GameController.Instance.MinimizePlayerMat();
					return;
				}
			}
			else
			{
				this.recruitSelectionPanelRect.sizeDelta = this.originalRecruitSelectionSizeDelta;
				this.recruitSelectionPanelLayoutElement.ignoreLayout = false;
				this.bonusSelectionPanelMobile.GetComponent<CanvasGroup>().alpha = 1f;
				this.RefreshButtonsMobile();
			}
		}

		// Token: 0x06001D8F RID: 7567 RVA: 0x000B740C File Offset: 0x000B560C
		private void DisableInput()
		{
			this.enlistStep = EnlistPresenter.EnlistStep.none;
			for (int i = 0; i < 4; i++)
			{
				GameController.Instance.matFaction.OneTimeBonusShow(i, false);
				if (PlatformManager.IsStandalone)
				{
					GameController.Instance.matFaction.recruitButton[i].interactable = false;
				}
				GameController.Instance.matPlayer.matSection[i].downActionPresenter.recruitButton.gameObject.SetActive(false);
				if (PlatformManager.IsStandalone && GameController.Instance.matPlayer.matSection[i].downActionPresenter.guideline != null)
				{
					GameController.Instance.matPlayer.matSection[i].downActionPresenter.guideline.enabled = false;
				}
			}
			if (GameController.Instance.matFaction.oneTimeBonusAccept != null)
			{
				GameController.Instance.matFaction.oneTimeBonusAccept.interactable = false;
			}
			if (this.sectionID != -1)
			{
				GameController.Instance.matPlayer.matSection[this.sectionID].downActionPresenter.recruitButton.interactable = true;
			}
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x000B7540 File Offset: 0x000B5740
		private void EnableInput()
		{
			this.enlistStep = EnlistPresenter.EnlistStep.selectRecruit;
			for (int i = 0; i < 4; i++)
			{
				if (!GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(i).ActionDown.RecruitEnlisted())
				{
					MatPlayerSectionPresenter matPlayerSectionPresenter = GameController.Instance.matPlayer.matSection[i];
					matPlayerSectionPresenter.sectionGlass.enabled = false;
					if (PlatformManager.IsStandalone)
					{
						matPlayerSectionPresenter.SetSectionCooldown(false, false);
					}
					matPlayerSectionPresenter.downActionPresenter.recruitButton.gameObject.SetActive(true);
					matPlayerSectionPresenter.downActionPresenter.recruitButton.image.color = this.glowDefault1;
					matPlayerSectionPresenter.downActionPresenter.recruitButton.GetComponent<Animator>().StopPlayback();
					matPlayerSectionPresenter.downActionPresenter.recruitBonus.material = null;
					if (matPlayerSectionPresenter.downActionPresenter.guideline != null)
					{
						matPlayerSectionPresenter.downActionPresenter.guideline.enabled = true;
					}
				}
			}
			base.EnableMapBlackout();
			this.RefreshButtonsMobile();
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x000B7644 File Offset: 0x000B5844
		private void RefreshButtonsMobile()
		{
			for (int i = 0; i < this.recruitSelectionButtonsMobile.Length; i++)
			{
				bool flag = !GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(i).ActionDown.IsRecruitEnlisted;
				bool flag2 = this.sectionID != -1 && this.sectionID == i;
				this.recruitSelectionButtonsMobile[i].interactable = flag;
				this.recruitSelectionButtonHighlightsMobile[i].enabled = flag2;
				this.recruitSelectionButtonGlowsMobile[i].enabled = flag && !flag2;
			}
			for (int j = 0; j < this.bonusSelectionButtonsMobile.Length; j++)
			{
				bool flag3 = !GameController.GameManager.PlayerCurrent.matFaction.OneTimeBonusUsed((GainType)j);
				bool flag4 = this.bonusID == j;
				this.bonusSelectionButtonsMobile[j].interactable = flag3;
				this.bonusSelectionButtonHighlightsMobile[j].enabled = flag4;
				this.bonusSelectionButtonGlowsMobile[j].enabled = flag3 && !flag4;
			}
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x0003AD11 File Offset: 0x00038F11
		public void OnEndEnlistButtonClicked()
		{
			GameController.Instance.PopupWindowsBeforeNextTurn();
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x000B7740 File Offset: 0x000B5940
		public override void OnActionEnded()
		{
			if (this.guidelineMain != null)
			{
				this.guidelineMain.enabled = false;
			}
			if (this.guideline2nd != null)
			{
				this.guideline2nd.enabled = false;
			}
			GameController.Instance.matFaction.ClearHintStories();
			base.DisableMapBlackout();
			GameObject[] array = this.assistHighlight;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			if (!this.openedEarlier && PlatformManager.IsStandalone)
			{
				this.toggleRecruit.isOn = false;
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Screens.tab_enlist, Contexts.ingame);
			}
			this.openedEarlier = false;
			if (!PlatformManager.IsStandalone)
			{
				this.recruitSelectionPanelMobile.SetActive(false);
				this.bonusSelectionPanelMobile.GetComponent<CanvasGroup>().alpha = 0f;
				this.confirmButtonMobile.GetComponent<CanvasGroup>().alpha = 0f;
			}
			this.DisableInput();
			this.amountPresenter.SetActive(false);
			HumanInputHandler.Instance.OnInputEnded();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x000B7854 File Offset: 0x000B5A54
		public override void Clear()
		{
			if (this.guidelineMain != null)
			{
				this.guidelineMain.enabled = false;
			}
			if (this.guideline2nd != null)
			{
				this.guideline2nd.enabled = false;
			}
			GameController.Instance.matFaction.ClearHintStories();
			GameObject[] array = this.assistHighlight;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
		}

		// Token: 0x04001520 RID: 5408
		public GameObject amountPresenter;

		// Token: 0x04001521 RID: 5409
		public Image guidelineMain;

		// Token: 0x04001522 RID: 5410
		public Image guideline2nd;

		// Token: 0x04001523 RID: 5411
		public Toggle2 toggleRecruit;

		// Token: 0x04001524 RID: 5412
		public GameObject hintAction;

		// Token: 0x04001525 RID: 5413
		public GameObject[] assistHighlight;

		// Token: 0x04001526 RID: 5414
		public Image[] assistIcon;

		// Token: 0x04001527 RID: 5415
		public Color glowSelected1;

		// Token: 0x04001528 RID: 5416
		public Color glowDefault1;

		// Token: 0x04001529 RID: 5417
		public GameObject recruitSelectionPanelMobile;

		// Token: 0x0400152A RID: 5418
		public GameObject bonusSelectionPanelMobile;

		// Token: 0x0400152B RID: 5419
		public GameObject confirmButtonMobile;

		// Token: 0x0400152C RID: 5420
		public Button[] recruitSelectionButtonsMobile;

		// Token: 0x0400152D RID: 5421
		public Image[] recruitSelectionButtonHighlightsMobile;

		// Token: 0x0400152E RID: 5422
		public Image[] recruitSelectionButtonGlowsMobile;

		// Token: 0x0400152F RID: 5423
		public Image[] recruitSelectionButtonRecruitNeighbourLeftMobile;

		// Token: 0x04001530 RID: 5424
		public Image[] recruitSelectionButtonRecruitNeighbourRightMobile;

		// Token: 0x04001531 RID: 5425
		public Button[] bonusSelectionButtonsMobile;

		// Token: 0x04001532 RID: 5426
		public Image[] bonusSelectionButtonHighlightsMobile;

		// Token: 0x04001533 RID: 5427
		public Image[] bonusSelectionButtonGlowsMobile;

		// Token: 0x04001534 RID: 5428
		public Material recruitNeighbourInactive;

		// Token: 0x04001535 RID: 5429
		private GainRecruit action;

		// Token: 0x04001536 RID: 5430
		private GainAction oneTimeBonus;

		// Token: 0x04001537 RID: 5431
		private int sectionID = -1;

		// Token: 0x04001538 RID: 5432
		private int bonusID = -1;

		// Token: 0x04001539 RID: 5433
		private bool openedEarlier;

		// Token: 0x0400153A RID: 5434
		[HideInInspector]
		public EnlistPresenter.EnlistStep enlistStep;

		// Token: 0x0400153D RID: 5437
		[SerializeField]
		private RectTransform recruitSelectionPanelRect;

		// Token: 0x0400153E RID: 5438
		[SerializeField]
		private float popupAnimationSpeed = 0.5f;

		// Token: 0x0400153F RID: 5439
		private Vector2 originalRecruitSelectionAnchoredPosition;

		// Token: 0x04001540 RID: 5440
		private Vector2 originalRecruitSelectionSizeDelta;

		// Token: 0x04001541 RID: 5441
		private LayoutElement recruitSelectionPanelLayoutElement;

		// Token: 0x04001542 RID: 5442
		private Sequence panelAnimationTween;

		// Token: 0x020003E2 RID: 994
		public enum EnlistStep
		{
			// Token: 0x04001544 RID: 5444
			none,
			// Token: 0x04001545 RID: 5445
			selectRecruit,
			// Token: 0x04001546 RID: 5446
			selectBonus
		}

		// Token: 0x020003E3 RID: 995
		// (Invoke) Token: 0x06001D98 RID: 7576
		public delegate void BonusSelect(GainType type);

		// Token: 0x020003E4 RID: 996
		// (Invoke) Token: 0x06001D9C RID: 7580
		public delegate void RecruitSelect();
	}
}

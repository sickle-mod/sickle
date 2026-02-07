using System;
using System.Collections;
using DG.Tweening;
using I2.Loc;
using Scythe.Analytics;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200046E RID: 1134
	public class EncounterCardPresenter : MonoBehaviour, IActionProxy
	{
		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x060023C4 RID: 9156 RVA: 0x000D3D74 File Offset: 0x000D1F74
		// (remove) Token: 0x060023C5 RID: 9157 RVA: 0x000D3DA8 File Offset: 0x000D1FA8
		public static event EncounterCardPresenter.EncounterEnd encounterEnd;

		// Token: 0x140000EA RID: 234
		// (add) Token: 0x060023C6 RID: 9158 RVA: 0x000D3DDC File Offset: 0x000D1FDC
		// (remove) Token: 0x060023C7 RID: 9159 RVA: 0x000D3E10 File Offset: 0x000D2010
		public static event EncounterCardPresenter.RevealCard OnRevealedCard;

		// Token: 0x060023C8 RID: 9160 RVA: 0x000D3E44 File Offset: 0x000D2044
		private void Awake()
		{
			this.rect = base.gameObject.GetComponent<RectTransform>();
			if (this.closeButton != null)
			{
				this.closeButton.onClick.AddListener(new UnityAction(this.OnCloseEncounterClicked));
			}
			this.rectMinSizeDelta = new Vector2(516f, 320f);
			this.rectMinAnchorMin = new Vector2(1f, 1f);
			this.rectMinAnchorMax = new Vector2(1f, 1f);
			this.rectMinPivot = new Vector2(1f, 1f);
			this.rectMinAnchoredPosition = new Vector2(-2.5f, -28f);
			this.rectMinLocalScale = new Vector3(0.2f, 0.2f, 0.2f);
			if (PlatformManager.IsStandalone)
			{
				this.rectMaxSizeDelta = new Vector2(516f, 320f);
				this.rectMaxAnchorMin = new Vector2(0.5f, 0.5f);
				this.rectMaxAnchorMax = new Vector2(0.5f, 0.5f);
				this.rectMaxPivot = new Vector2(0.5f, 0.5f);
				this.rectMaxAnchoredPosition = new Vector2(0f, 34f);
				this.rectMaxLocalScale = new Vector3(1f, 1f, 1f);
				return;
			}
			this.rectMaxSizeDelta = new Vector2(0f, 0f);
			this.rectMaxAnchorMin = new Vector2(0f, 0f);
			this.rectMaxAnchorMax = new Vector2(1f, 1f);
			this.rectMaxPivot = new Vector2(0.5f, 0.5f);
			this.rectMaxAnchoredPosition = new Vector2(0f, 0f);
			this.rectMaxLocalScale = new Vector3(1f, 1f, 1f);
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x000D4020 File Offset: 0x000D2220
		private void OnEnable()
		{
			EncounterCardPresenter.encounterEnd = null;
			this.hexWithToken = GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.PlayerCurrent.character.position);
			this.hexWithToken.ActivateEncounterWaitAnimation();
			this.mouseHeldDownOnEnable = Input.GetMouseButton(0);
			this.ClearNotEnoughResources();
			this.minimized = false;
			if (this.encounterCardSmall == null && this.glass != null)
			{
				this.glass.enabled = true;
			}
			if (this.dismissDialog != null && (!GameController.GameManager.IsMultiplayer || GameController.GameManager.IsMyTurn()))
			{
				this.dismissDialog.Show(GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo, delegate
				{
					this.DismissEncounter();
				});
			}
			OptionsManager.OnLanguageChanged += this.UpdateTextAndColors;
			WorldSFXManager.PlaySound(SoundEnum.EncounterStart, AudioSourceType.WorldSfx);
			if (PlatformManager.IsStandalone)
			{
				Button[] array = this.optionButtons;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].gameObject.GetComponent<PointerEventsController>().buttonHoover += this.EncounterOptionHoover;
				}
			}
			GameController.Instance.matPlayer.ShowSectionMatActionReloading(false);
			if (!PlatformManager.IsStandalone)
			{
				this.MaximizeButton_OnClick();
				if (GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsMyTurn())
				{
					this.DarkenBackgroundActive(false);
				}
				else
				{
					this.DarkenBackgroundActive(true);
				}
				this.minimizingScreen.OnMaximizeEnd += this.MaximizeEnd;
				if (GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsMyTurn())
				{
					this.minimizeButton.SetActive(false);
					this.exitButton.SetActive(false);
				}
			}
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x000D41E8 File Offset: 0x000D23E8
		private void OnDisable()
		{
			if (PlatformManager.IsStandalone)
			{
				OptionsManager.OnLanguageChanged -= this.UpdateTextAndColors;
				if (this.dismissDialog != null && this.dismissDialog.gameObject.activeInHierarchy)
				{
					this.dismissDialog.gameObject.SetActive(false);
				}
				if (this.hexWithToken != null)
				{
					this.hexWithToken.BreakWaitAnimation();
					this.hexWithToken = null;
				}
				Button[] array = this.optionButtons;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].gameObject.GetComponent<PointerEventsController>().buttonHoover -= this.EncounterOptionHoover;
				}
				if (!GameController.GameManager.IsMultiplayer || GameController.GameManager.IsMyTurn())
				{
					GameController.Instance.EndTurnButtonEnable();
					return;
				}
			}
			else
			{
				this.minimizingScreen.OnMaximizeEnd -= this.MaximizeEnd;
			}
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x0003ED58 File Offset: 0x0003CF58
		private void EncounterOptionHoover()
		{
			WorldSFXManager.PlaySound(SoundEnum.EncounterHover, AudioSourceType.Buttons);
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x000D42C8 File Offset: 0x000D24C8
		private void ClearNotEnoughResources()
		{
			for (int i = 0; i < 3; i++)
			{
				this.notEnough[i].SetActive(false);
			}
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x0003ED62 File Offset: 0x0003CF62
		private void Update()
		{
			if (Input.GetMouseButtonUp(0))
			{
				this.mouseHeldDownOnEnable = false;
			}
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x0003ED73 File Offset: 0x0003CF73
		public bool IsAnimatorEnabled()
		{
			return base.GetComponent<Animator>().enabled;
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x000D42F0 File Offset: 0x000D24F0
		public void SetOptions(EncounterCard encounterCard)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.encounter_popup, Contexts.ingame);
				AchievementManager.UpdateAchievementEncounter();
			}
			base.gameObject.SetActive(true);
			GameController.Instance.waitInfoEncounter.SetActive(false);
			if (base.GetComponent<Animator>().isInitialized)
			{
				base.GetComponent<Animator>().Play("EncounterPopUp", 0, 0f);
				if (this.clickToUncover != null)
				{
					this.clickToUncover.Play("ClickToUncover");
				}
				if (!PlatformManager.IsStandalone)
				{
					SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
				}
			}
			this.encounterCard = encounterCard;
			GameController.GameManager.PlayerCurrent.character.position.encounterUsed = true;
			Image[] array = this.factionLogo;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].sprite = GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo;
			}
			Button[] array2 = this.optionButtons;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].GetComponent<Image>().color = Color.white;
			}
			if (PlatformManager.IsStandalone)
			{
				AssetBundle assetBundle = AssetBundleManager.LoadAssetBundle("graphic_encounters");
				this.art.sprite = assetBundle.LoadAsset<Sprite>("encounter_" + encounterCard.CardId.ToString().PadLeft(2, '0'));
			}
			else
			{
				AssetBundle assetBundle2 = AssetBundleManager.LoadAssetBundle("graphic_encounters_mobile");
				this.art.sprite = assetBundle2.LoadAsset<Sprite>("encounter_" + encounterCard.CardId.ToString() + "_mobile");
			}
			if (GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsMyTurn())
			{
				if (this.glass != null)
				{
					this.glass.enabled = true;
				}
				this.chosingInfo.enabled = true;
				this.chosingInfo.text = ScriptLocalization.Get("FactionMat/" + GameController.GameManager.PlayerCurrent.matFaction.faction.ToString()) + " " + ScriptLocalization.Get("GameScene/IsChoosing");
				if (base.gameObject.activeInHierarchy)
				{
					base.StartCoroutine(this.Reveal());
				}
			}
			else
			{
				if (this.glass != null)
				{
					this.glass.enabled = false;
				}
				this.chosingInfo.enabled = false;
			}
			if (GameController.Instance.AdjustingPresenters && base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.Reveal());
			}
			this.UpdateTextAndColors();
			this.UpdateCloseButton();
			if (this.encounterCardSmall != null)
			{
				this.encounterCardSmall.SetOptions(encounterCard);
				this.encounterCardSmall.transform.parent.gameObject.SetActive(false);
			}
			if (this.encounterCard.GetPreviousActionId() != -1)
			{
				this.GrayOutOption(encounterCard.GetPreviousActionId());
				this.optionButtons[encounterCard.GetPreviousActionId()].interactable = false;
			}
			if (this.encounterCard.GetCurrentActionId() != -1 && PlatformManager.IsMobile)
			{
				this.Minimize();
			}
			if (GameController.GameManager.PlayerCurrent.IsHuman)
			{
				GameController.Instance.undoController.TriggerUndoInteractivityChange(false);
				GameController.Instance.playersFactions.blockPassCoins = true;
			}
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x0003ED80 File Offset: 0x0003CF80
		private IEnumerator Reveal()
		{
			yield return new WaitForSeconds(1f);
			this.RevealAnimation();
			yield break;
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x0003ED8F File Offset: 0x0003CF8F
		public void RevealAnimation()
		{
			base.GetComponent<Animator>().Play("EncounterReveal");
			WorldSFXManager.PlaySound(SoundEnum.EncounterShow, AudioSourceType.WorldSfx);
			if (EncounterCardPresenter.OnRevealedCard != null)
			{
				EncounterCardPresenter.OnRevealedCard();
			}
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x0003EDBA File Offset: 0x0003CFBA
		public void OnRevealFinished()
		{
			if (this.minimizeArea != null)
			{
				this.minimizeArea.SetActive(true);
			}
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x000D4654 File Offset: 0x000D2854
		public void UpdateTextAndColors()
		{
			for (int i = 0; i < this.optionButtons.Length; i++)
			{
				if (PlatformManager.IsStandalone)
				{
					this.optionTexts1[i].text = string.Format(this.textFormat, GameController.GetEncounterDescription(this.encounterCard.CardId, i + 1), GameController.GetEncounterActionDescription(this.encounterCard.CardId, i + 1));
				}
				else
				{
					this.optionTexts1[i].text = GameController.GetEncounterDescription(this.encounterCard.CardId, i + 1);
					this.optionDescriptions[i].text = GameController.GetEncounterActionDescription(this.encounterCard.CardId, i + 1);
					this.encounterIndexText.text = this.encounterCard.CardId.ToString();
				}
				if (GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsMyTurn())
				{
					this.optionTexts1[i].color = Color.black;
					if (this.optionIcons.Length != 0)
					{
						this.optionIcons[i].color = Color.black;
					}
					this.optionButtons[i].interactable = false;
				}
				else if (!GameController.GameManager.CanPlayerDoEncounter(i))
				{
					this.optionTexts1[i].color = Color.red;
					if (this.optionIcons.Length != 0)
					{
						this.optionIcons[i].color = Color.red;
					}
					this.optionButtons[i].interactable = true;
				}
				else
				{
					this.optionTexts1[i].color = Color.black;
					if (this.optionIcons.Length != 0)
					{
						this.optionIcons[i].color = Color.black;
					}
					this.optionButtons[i].interactable = true;
				}
			}
			this.chosingInfo.text = ScriptLocalization.Get("FactionMat/" + GameController.GameManager.PlayerCurrent.matFaction.faction.ToString()) + " " + ScriptLocalization.Get("GameScene/IsChoosing");
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x000D484C File Offset: 0x000D2A4C
		private void UpdateCloseButton()
		{
			if (this.closeButton != null)
			{
				GameManager gameManager = GameController.GameManager;
				if ((gameManager.IsMultiplayer && gameManager.PlayerCurrent != gameManager.PlayerOwner) || gameManager.SpectatorMode)
				{
					this.closeButton.gameObject.SetActive(false);
					return;
				}
				this.closeButton.gameObject.SetActive(true);
			}
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x000D48B0 File Offset: 0x000D2AB0
		public void GrayOutOption(int number)
		{
			if (PlatformManager.IsStandalone)
			{
				this.optionTexts1[number].color = Color.gray;
				if (this.optionIcons.Length != 0)
				{
					this.optionIcons[number].color = Color.gray;
					return;
				}
			}
			else
			{
				this.optionButtons[number].GetComponent<Image>().color = Color.gray;
			}
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x0003EDD6 File Offset: 0x0003CFD6
		public void DisableOption(int number)
		{
			this.optionButtons[number].interactable = false;
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x000D490C File Offset: 0x000D2B0C
		public void DisableAllOptions()
		{
			for (int i = 0; i < this.optionButtons.Length; i++)
			{
				this.optionButtons[i].interactable = false;
			}
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x000D493C File Offset: 0x000D2B3C
		public void ChooseOption(int number)
		{
			if (this.mouseHeldDownOnEnable)
			{
				return;
			}
			if (GameController.GameManager.CanPlayerDoEncounter(number))
			{
				if (GameController.GameManager.IsMultiplayer && GameController.GameManager.IsMyTurn())
				{
					GameController.GameManager.ChooseEncounterOption(number);
				}
				this.GrayOutOption(number);
				this.DisableAllOptions();
				if (!GameController.GameManager.IsMultiplayer || GameController.GameManager.IsMyTurn())
				{
					this.SectionActionSelected(this.encounterCard.GetAction(number), number);
				}
				WorldSFXManager.PlaySound(SoundEnum.EncounterChoose, AudioSourceType.Buttons);
				return;
			}
			this.notEnough[number].SetActive(true);
			if (PlatformManager.IsStandalone && this.optionTexts1[number].GetComponent<Animator>() != null)
			{
				this.optionTexts1[number].GetComponent<Animator>().Play("EncounterNotEnough", 0, 0f);
			}
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x0003EDE6 File Offset: 0x0003CFE6
		public void SectionActionSelected(SectionAction action, int actionId = -1)
		{
			base.GetComponent<Animator>().enabled = true;
			this.Minimize();
			GameController.GameManager.actionManager.SetSectionAction(action, this, actionId);
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x000D4A0C File Offset: 0x000D2C0C
		public void SectionActionFinished()
		{
			GameController.Instance.gameBoardPresenter.UpdateBoard(true, true);
			GameController.Instance.UpdateStats(true, true);
			if (GameController.GameManager.PlayerCurrent.matFaction.factionPerk != AbilityPerk.Meander || this.encounterCard.AmountChoosen >= 2)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
				this.Close();
				return;
			}
			this.Maximize();
			for (int i = 0; i < this.optionButtons.Length; i++)
			{
				if (i != this.encounterCard.GetPreviousActionId())
				{
					if (!GameController.GameManager.CanPlayerDoEncounter(i))
					{
						this.optionTexts1[i].color = Color.red;
						if (this.optionIcons.Length != 0)
						{
							this.optionIcons[i].color = Color.red;
						}
						this.optionButtons[i].interactable = true;
					}
					else
					{
						this.optionTexts1[i].color = Color.black;
						if (this.optionIcons.Length != 0)
						{
							this.optionIcons[i].color = Color.black;
						}
						this.optionButtons[i].interactable = true;
					}
				}
			}
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x0003EE0C File Offset: 0x0003D00C
		public void MaximizeButton_OnClicked()
		{
			this.Maximize();
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x0003EE14 File Offset: 0x0003D014
		public void MinimizeButton_OnClicked()
		{
			this.Minimize();
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x0003EE1C File Offset: 0x0003D01C
		public void KeyboardShortcut_ChangeSize()
		{
			if (this.minimized)
			{
				this.Maximize();
				return;
			}
			this.Minimize();
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x000D4B20 File Offset: 0x000D2D20
		private void Maximize()
		{
			if (PlatformManager.IsMobile)
			{
				if (this.encounterCardSmall != null)
				{
					this.encounterCardSmall.transform.parent.gameObject.SetActive(false);
				}
				base.gameObject.SetActive(true);
				this.minimized = false;
				if (MobileChat.IsSupported)
				{
					SingletonMono<MobileChat>.Instance.ResetScroll();
					return;
				}
			}
			else
			{
				if (!this.maximizeArea.activeInHierarchy)
				{
					return;
				}
				base.GetComponent<Animator>().enabled = false;
				this.rect.DOSizeDelta(this.rectMaxSizeDelta, this.duration, false).SetEase(this.easeMaximize);
				this.rect.DOAnchorMin(this.rectMaxAnchorMin, this.duration, false).SetEase(this.easeMaximize);
				this.rect.DOAnchorMax(this.rectMaxAnchorMax, this.duration, false).SetEase(this.easeMaximize);
				this.rect.DOAnchorPos(this.rectMaxAnchoredPosition, this.duration, false).SetEase(this.easeMaximize);
				this.rect.DOPivot(this.rectMaxPivot, this.duration).SetEase(this.easeMaximize);
				this.rect.DOScale(this.rectMaxLocalScale, this.duration).SetEase(this.easeMaximize);
				this.glass.enabled = false;
				this.minimized = false;
				this.minimizeArea.SetActive(true);
				this.maximizeArea.SetActive(false);
			}
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x000D4CA0 File Offset: 0x000D2EA0
		private void Minimize()
		{
			if (!PlatformManager.IsStandalone)
			{
				if (this.encounterCardSmall != null)
				{
					this.encounterCardSmall.transform.parent.gameObject.SetActive(true);
				}
				this.minimized = true;
				base.gameObject.SetActive(false);
				return;
			}
			if (this.encounterCardSmall == null)
			{
				return;
			}
			if (!this.minimizeArea.activeInHierarchy)
			{
				return;
			}
			base.GetComponent<Animator>().enabled = false;
			this.minimized = true;
			this.glass.enabled = true;
			this.minimizeArea.SetActive(false);
			this.maximizeArea.SetActive(true);
			this.rect.DOSizeDelta(this.rectMinSizeDelta, this.duration, true).SetEase(this.easeMinimize);
			this.rect.DOSizeDelta(this.rectMinSizeDelta, this.duration, false).SetEase(this.easeMinimize);
			this.rect.DOAnchorMin(this.rectMinAnchorMin, this.duration, false).SetEase(this.easeMinimize);
			this.rect.DOAnchorMax(this.rectMinAnchorMax, this.duration, false).SetEase(this.easeMinimize);
			this.rect.DOAnchorPos(this.rectMinAnchoredPosition, this.duration, false).SetEase(this.easeMinimize);
			this.rect.DOPivot(this.rectMinPivot, this.duration).SetEase(this.easeMinimize);
			this.rect.DOScale(this.rectMinLocalScale, this.duration).SetEase(this.easeMinimize);
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x0003EE33 File Offset: 0x0003D033
		public void OnXButtonClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
			this.Close();
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x000D4E44 File Offset: 0x000D3044
		public void DismissEncounter()
		{
			if (GameController.GameManager.actionManager.GetLastSelectedGainAction() != null)
			{
				GameController.GameManager.actionManager.BreakSectionAction(false);
				this.Close();
			}
			else
			{
				this.SectionActionFinished();
				this.Close();
			}
			GameController.Instance.undoController.PushToStack();
			GameController.GameManager.actionLog.FlushAwaitingPayActions();
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x000D4EA4 File Offset: 0x000D30A4
		public void Close()
		{
			if (!PlatformManager.IsStandalone)
			{
				this.OnClose();
			}
			if (GameController.GameManager.PlayerCurrent.IsHuman)
			{
				if (!GameController.GameManager.GameFinished)
				{
					GameController.Instance.undoController.TriggerUndoInteractivityChange(true);
				}
				GameController.Instance.playersFactions.blockPassCoins = false;
			}
			int currentMatSection = GameController.GameManager.PlayerCurrent.currentMatSection;
			bool downActionFinished = GameController.GameManager.PlayerCurrent.downActionFinished;
			MatPlayerPresenter matPlayer = GameController.Instance.matPlayer;
			if ((!GameController.GameManager.IsMultiplayer || GameController.GameManager.IsMyTurn()) && !downActionFinished && currentMatSection < matPlayer.matSection.Count && currentMatSection >= 0)
			{
				matPlayer.matSection[currentMatSection].CheckDownAction();
			}
			if (this.encounterCardSmall != null)
			{
				this.encounterCardSmall.transform.parent.gameObject.SetActive(false);
			}
			if (EncounterCardPresenter.encounterEnd != null)
			{
				EncounterCardPresenter.encounterEnd();
			}
			GameController.GameManager.ClearLastEncounterCard();
			if (!GameController.Instance.GameIsLoaded && this.hexWithToken != null && (this.encounterCardSmall != null || !PlatformManager.IsStandalone))
			{
				this.hexWithToken.ActivateEncounterEndAnimation();
			}
			if (this.minimized && this.encounterCardSmall != null)
			{
				this.Maximize();
				DOTween.Complete(base.gameObject, false);
			}
			this.minimized = false;
			base.GetComponent<Animator>().enabled = true;
			if (this.minimizeArea != null)
			{
				this.minimizeArea.SetActive(false);
			}
			if (this.maximizeArea != null)
			{
				this.maximizeArea.SetActive(false);
			}
			if (!PlatformManager.IsStandalone)
			{
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Visible, 0.25f);
			}
			AssetBundleManager.UnloadAssetBundle("graphic_encounters", true);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
			base.gameObject.SetActive(false);
			if (this.hexWithToken != null)
			{
				this.hexWithToken.BreakWaitAnimation();
				this.hexWithToken = null;
			}
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x000D50A4 File Offset: 0x000D32A4
		private void OnClose()
		{
			OptionsManager.OnLanguageChanged -= this.UpdateTextAndColors;
			if (this.dismissDialog != null && this.dismissDialog.gameObject.activeInHierarchy)
			{
				this.dismissDialog.gameObject.SetActive(false);
			}
			if (!GameController.GameManager.IsMultiplayer || GameController.GameManager.IsMyTurn())
			{
				GameController.Instance.EndTurnButtonEnable();
			}
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x000D5118 File Offset: 0x000D3318
		public void CloseAfterLoadingTheGame()
		{
			if (PlatformManager.IsMobile)
			{
				this.hexWithToken.BreakWaitAnimation();
			}
			if (this.minimized && this.encounterCardSmall != null)
			{
				this.Maximize();
				DOTween.Complete(base.gameObject, false);
			}
			this.minimized = false;
			base.GetComponent<Animator>().enabled = true;
			if (this.minimizeArea != null)
			{
				this.minimizeArea.SetActive(false);
			}
			if (this.maximizeArea != null)
			{
				this.maximizeArea.SetActive(false);
			}
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
			base.gameObject.SetActive(false);
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x000D51C0 File Offset: 0x000D33C0
		private void OnCloseEncounterClicked()
		{
			if (!OptionsManager.IsWarningsActive())
			{
				this.Close();
				return;
			}
			this.endEncounterDialog.gameObject.SetActive(true);
			this.endEncounterDialog.Show(null, new YesNoDialog.OnClick(this.Close), delegate
			{
				this.endEncounterDialog.gameObject.SetActive(false);
			});
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x0003EE41 File Offset: 0x0003D041
		private void DarkenBackgroundActive(bool active)
		{
			this.darkenPanel.SetActive(active);
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x000D5210 File Offset: 0x000D3410
		public void MinimizeButton_OnClick()
		{
			this.animator.enabled = false;
			this.darkenPanel.SetActive(false);
			this.minimizeButton.SetActive(false);
			this.exitButton.SetActive(false);
			WorldSFXManager.PlaySound(SoundEnum.GuiChatButton, AudioSourceType.Buttons);
			this.minimizingScreen.Minimize();
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x0003EE4F File Offset: 0x0003D04F
		public void MaximizeButton_OnClick()
		{
			this.animator.enabled = true;
			this.darkenPanel.SetActive(true);
			WorldSFXManager.PlaySound(SoundEnum.GuiChatButton, AudioSourceType.Buttons);
			this.minimizingScreen.Maximize();
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x0003EE7C File Offset: 0x0003D07C
		private void MaximizeEnd()
		{
			if (GameController.GameManager.IsMyTurn() && !GameController.GameManager.IsCampaign)
			{
				this.minimizeButton.SetActive(true);
				this.exitButton.SetActive(true);
			}
		}

		// Token: 0x040018E9 RID: 6377
		public Button[] optionButtons;

		// Token: 0x040018EA RID: 6378
		public TextMeshProUGUI[] optionTexts1;

		// Token: 0x040018EB RID: 6379
		public TextMeshProUGUI[] optionDescriptions;

		// Token: 0x040018EC RID: 6380
		public Image[] optionIcons;

		// Token: 0x040018ED RID: 6381
		public TMP_Text encounterIndexText;

		// Token: 0x040018EE RID: 6382
		public GameObject[] notEnough;

		// Token: 0x040018EF RID: 6383
		public Image[] factionLogo;

		// Token: 0x040018F0 RID: 6384
		public Image glass;

		// Token: 0x040018F1 RID: 6385
		public Image art;

		// Token: 0x040018F2 RID: 6386
		public Text chosingInfo;

		// Token: 0x040018F3 RID: 6387
		public EncounterCardPresenter encounterCardSmall;

		// Token: 0x040018F4 RID: 6388
		public DismissDialog dismissDialog;

		// Token: 0x040018F5 RID: 6389
		public YesNoDialog endEncounterDialog;

		// Token: 0x040018F6 RID: 6390
		public Button closeButton;

		// Token: 0x040018F7 RID: 6391
		public Animator clickToUncover;

		// Token: 0x040018F8 RID: 6392
		public GameObject minimizeArea;

		// Token: 0x040018F9 RID: 6393
		public GameObject maximizeArea;

		// Token: 0x040018FA RID: 6394
		public GameHexPresenter hexWithToken;

		// Token: 0x040018FD RID: 6397
		private EncounterCard encounterCard;

		// Token: 0x040018FE RID: 6398
		private bool mouseHeldDownOnEnable;

		// Token: 0x040018FF RID: 6399
		public bool minimized;

		// Token: 0x04001900 RID: 6400
		[SerializeField]
		private GameObject darkenPanel;

		// Token: 0x04001901 RID: 6401
		[SerializeField]
		private MinimizingScreen minimizingScreen;

		// Token: 0x04001902 RID: 6402
		[SerializeField]
		private GameObject minimizeButton;

		// Token: 0x04001903 RID: 6403
		[SerializeField]
		private GameObject exitButton;

		// Token: 0x04001904 RID: 6404
		[SerializeField]
		private Animator animator;

		// Token: 0x04001905 RID: 6405
		[Space]
		[Header("Anim settings")]
		public float duration = 1f;

		// Token: 0x04001906 RID: 6406
		public Ease easeMaximize;

		// Token: 0x04001907 RID: 6407
		public Ease easeMinimize;

		// Token: 0x04001908 RID: 6408
		private RectTransform rect;

		// Token: 0x04001909 RID: 6409
		private Vector2 rectMinSizeDelta;

		// Token: 0x0400190A RID: 6410
		private Vector2 rectMinAnchorMin;

		// Token: 0x0400190B RID: 6411
		private Vector2 rectMinAnchorMax;

		// Token: 0x0400190C RID: 6412
		private Vector2 rectMinPivot;

		// Token: 0x0400190D RID: 6413
		private Vector3 rectMinLocalScale;

		// Token: 0x0400190E RID: 6414
		private Vector2 rectMinAnchoredPosition;

		// Token: 0x0400190F RID: 6415
		private Vector2 rectMaxSizeDelta;

		// Token: 0x04001910 RID: 6416
		private Vector2 rectMaxAnchorMin;

		// Token: 0x04001911 RID: 6417
		private Vector2 rectMaxPivot;

		// Token: 0x04001912 RID: 6418
		private Vector2 rectMaxAnchoredPosition;

		// Token: 0x04001913 RID: 6419
		private Vector3 rectMaxLocalScale;

		// Token: 0x04001914 RID: 6420
		private Vector2 rectMaxAnchorMax;

		// Token: 0x04001915 RID: 6421
		private string textFormat = (PlatformManager.IsStandalone ? "<cspace=0.5>{0}</cspace> <font=\"MinionPro-ext\"><size=11>{1}</size></font>" : "{0}<font=\"TCM_____ Menu\">\n\n<size=11>{1}</size></font>");

		// Token: 0x0200046F RID: 1135
		// (Invoke) Token: 0x060023EE RID: 9198
		public delegate void EncounterEnd();

		// Token: 0x02000470 RID: 1136
		// (Invoke) Token: 0x060023F2 RID: 9202
		public delegate void RevealCard();
	}
}

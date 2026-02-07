using System;
using System.Collections.Generic;
using DG.Tweening;
using I2.Loc;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000439 RID: 1081
	public class CombatPanel : MonoBehaviour
	{
		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x0600211C RID: 8476 RVA: 0x0003D267 File Offset: 0x0003B467
		public CombatPanelAction State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x0003D26F File Offset: 0x0003B46F
		private void OnEnable()
		{
			OptionsManager.OnLanguageChanged += this.UpdateDoneButtonText;
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x0003D282 File Offset: 0x0003B482
		private void OnDisable()
		{
			OptionsManager.OnLanguageChanged -= this.UpdateDoneButtonText;
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x0003D295 File Offset: 0x0003B495
		public void SetBattlefield(GameHexPresenter battlefield)
		{
			this.battlefield = battlefield;
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x000C7998 File Offset: 0x000C5B98
		public void UpdateLogos(Faction player, Faction enemy)
		{
			this.combatControls[0].factionLogo.sprite = GameController.factionInfo[player].logo;
			this.combatControls[1].factionLogo.sprite = GameController.factionInfo[enemy].logo;
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000C79EC File Offset: 0x000C5BEC
		public void UpdateLabels(Player player)
		{
			if (GameController.GameManager.combatManager.GetAttacker() == player)
			{
				this.UpdateLabel(0, ScriptLocalization.Get("GameScene/Attacker"));
				this.UpdateLabel(1, ScriptLocalization.Get("GameScene/Defender"));
				return;
			}
			this.UpdateLabel(0, ScriptLocalization.Get("GameScene/Defender"));
			this.UpdateLabel(1, ScriptLocalization.Get("GameScene/Attacker"));
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x0003D29E File Offset: 0x0003B49E
		private void UpdateLabel(int panel, string text)
		{
			this.combatControls[panel].label.text = text;
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x0003D2B3 File Offset: 0x0003B4B3
		public void HideWinnerText()
		{
			this.winnerText.gameObject.SetActive(false);
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
		public bool WinnerTextIsActive()
		{
			return this.winnerText.gameObject.activeInHierarchy;
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x0003D2D8 File Offset: 0x0003B4D8
		public void UpdateWinnerText(string text)
		{
			this.winnerText.text = text;
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x000C7A50 File Offset: 0x000C5C50
		public void ResetTextsScale()
		{
			this.combatControls[0].overallPower.transform.DOComplete(false);
			this.combatControls[0].overallPower.transform.localScale = Vector3.one;
			this.combatControls[1].overallPower.transform.DOComplete(false);
			this.combatControls[1].overallPower.transform.localScale = Vector3.one;
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x000C7AC8 File Offset: 0x000C5CC8
		public void ResetLogosScales()
		{
			this.combatControls[0].factionLogo.transform.DOComplete(false);
			this.combatControls[0].factionLogo.transform.localScale = Vector3.one;
			this.combatControls[1].factionLogo.transform.DOComplete(false);
			this.combatControls[1].factionLogo.transform.localScale = Vector3.one;
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x0003D2E6 File Offset: 0x0003B4E6
		public void UpdateDoneButton(UnityAction call, CombatPanelAction state)
		{
			this.doneButton.onClick.RemoveAllListeners();
			this.doneButton.onClick.AddListener(call);
			this.state = state;
			this.UpdateDoneButtonText();
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000C7B40 File Offset: 0x000C5D40
		private void UpdateDoneButtonText()
		{
			string text = "";
			switch (this.state)
			{
			case CombatPanelAction.fight:
				if (PlatformManager.IsStandalone)
				{
					KeyboardShortcuts.Instance.isCombatFinished = false;
				}
				text = ScriptLocalization.Get("GameScene/Fight");
				break;
			case CombatPanelAction.wait_for_opponent:
				if (PlatformManager.IsStandalone)
				{
					KeyboardShortcuts.Instance.isCombatFinished = true;
				}
				text = ScriptLocalization.Get("GameScene/WaitForOpponent");
				break;
			case CombatPanelAction.result:
				if (PlatformManager.IsStandalone)
				{
					KeyboardShortcuts.Instance.isCombatFinished = true;
				}
				text = ScriptLocalization.Get("GameScene/Result");
				break;
			case CombatPanelAction.wait:
				if (PlatformManager.IsStandalone)
				{
					KeyboardShortcuts.Instance.isCombatFinished = true;
				}
				text = "";
				break;
			case CombatPanelAction.ok:
				if (PlatformManager.IsStandalone)
				{
					KeyboardShortcuts.Instance.isCombatFinished = true;
				}
				text = ScriptLocalization.Get("Common/Ok");
				break;
			}
			this.doneButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x0003D316 File Offset: 0x0003B516
		public void SetDoneButtonInteractable(bool interactable)
		{
			this.doneButton.interactable = interactable;
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x0003D324 File Offset: 0x0003B524
		public void DoneButtonTextBestFit(bool bestFit)
		{
			this.doneButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enableAutoSizing = bestFit;
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x0003D342 File Offset: 0x0003B542
		public void SetDoneButtonColor(Color color)
		{
			this.doneButton.GetComponent<Image>().color = color;
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x000C7C2C File Offset: 0x000C5E2C
		public void UpdateUnitsImages(Player player)
		{
			GameHex selectedBattlefield = GameController.GameManager.combatManager.GetSelectedBattlefield();
			Player enemyOf = GameController.GameManager.combatManager.GetEnemyOf(player);
			this.SetUnits(PanelSide.Left, player, selectedBattlefield);
			this.SetUnits(PanelSide.Right, enemyOf, selectedBattlefield);
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x000C7C6C File Offset: 0x000C5E6C
		private void SetUnits(PanelSide panel, Player player, GameHex hex)
		{
			this.DisableUnits((int)panel);
			this.SetCharacterImage((int)panel, player, hex);
			this.SetWorkerImage((int)panel, player, hex);
			this.SetMechsImages((int)panel, player, hex);
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x000C7CA0 File Offset: 0x000C5EA0
		private void DisableUnits(int panel)
		{
			foreach (object obj in Enum.GetValues(typeof(Faction)))
			{
				Faction faction = (Faction)obj;
				this.DisableImages(this.combatControls[panel].FactionUnits(faction));
			}
			this.DisableImages(this.combatControls[panel].characters);
			this.combatControls[panel].worker.enabled = false;
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x000C7D38 File Offset: 0x000C5F38
		private void DisableImages(Transform imagesParent)
		{
			foreach (object obj in imagesParent)
			{
				((Transform)obj).GetComponent<Image>().enabled = false;
			}
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x0003D355 File Offset: 0x0003B555
		private void SetCharacterImage(int panel, Player player, GameHex hex)
		{
			if (player.character.position == hex)
			{
				this.combatControls[panel].FactionCharacter(player.matFaction.faction).enabled = true;
			}
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x000C7D90 File Offset: 0x000C5F90
		private void SetWorkerImage(int panel, Player player, GameHex hex)
		{
			if (player.matFaction.abilities.Contains(AbilityPerk.PeoplesArmy) && player.matFaction.SkillUnlocked[2])
			{
				using (List<Worker>.Enumerator enumerator = player.matPlayer.workers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.position == hex)
						{
							this.combatControls[panel].worker.enabled = true;
						}
					}
				}
			}
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x000C7E20 File Offset: 0x000C6020
		private void SetMechsImages(int panel, Player player, GameHex hex)
		{
			int num = 0;
			using (List<Mech>.Enumerator enumerator = player.matFaction.mechs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.position == hex)
					{
						num++;
					}
				}
			}
			Transform transform = this.combatControls[panel].FactionUnits(player.matFaction.faction);
			for (int i = 0; i < num; i++)
			{
				transform.GetChild(3 - i).GetComponent<Image>().enabled = true;
			}
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x000C7EB8 File Offset: 0x000C60B8
		public void SaveCombatCardPresenterPosition()
		{
			this.leftCombatCardPresenterPosition = this.leftCombatCardPresenter.rect.position;
			this.rightCombatCardPresenterPosition = this.rightCombatCardPresenter.rect.position;
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000C7F04 File Offset: 0x000C6104
		public void SetCards(PanelSide panel, List<CombatCard> selectedCards)
		{
			Transform bombs = this.combatControls[(int)panel].bombs;
			for (int i = 0; i < bombs.childCount; i++)
			{
				if (i < selectedCards.Count)
				{
					bombs.GetChild(i).GetChild(1).gameObject.SetActive(true);
					bombs.GetChild(i).GetChild(1).GetChild(3)
						.GetComponent<Image>()
						.sprite = this.bombDigits[selectedCards[i].CombatBonus - 2];
				}
				else
				{
					bombs.GetChild(i).GetChild(1).gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000C7FA0 File Offset: 0x000C61A0
		public void UpdateCardSlots(PanelSide panel, int numberOfCardsToUse)
		{
			Transform bombs = this.combatControls[(int)panel].bombs;
			for (int i = 0; i < bombs.childCount; i++)
			{
				if (i < numberOfCardsToUse)
				{
					bombs.GetChild(i).gameObject.SetActive(true);
				}
				else
				{
					bombs.GetChild(i).gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x0003D383 File Offset: 0x0003B583
		public int NumberOfCardSlots(Player player, List<CombatCard> selectedCards = null)
		{
			return GameController.GameManager.combatManager.GetPlayerCombatUnitsCount(player);
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000C7FF8 File Offset: 0x000C61F8
		public void SetCombatCardDoorsActive(bool active, Player leftPlayer, Player rightPlayer, Dictionary<Player, PowerSelected> usedPower)
		{
			int num = (active ? this.NumberOfCardSlots(leftPlayer, usedPower[leftPlayer].selectedCards) : this.combatControls[0].bombs.childCount);
			int num2 = (active ? this.NumberOfCardSlots(rightPlayer, usedPower[rightPlayer].selectedCards) : this.combatControls[1].bombs.childCount);
			this.SetCombatCardsDoorActive(PanelSide.Left, active, leftPlayer, num, true);
			this.SetCombatCardsDoorActive(PanelSide.Right, active, rightPlayer, num2, true);
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000C8074 File Offset: 0x000C6274
		public void SetCombatCardsDoorActive(PanelSide panel, bool active, Player player, int numberOfSlots, bool closed = true)
		{
			for (int i = 0; i < numberOfSlots; i++)
			{
				this.SetCombatCardDoorActive(this.combatControls[(int)panel].bombs.GetChild(i), active, closed);
			}
			if (active)
			{
				this.enemyCombatCardsCount.text = player.combatCards.Count.ToString().PadLeft(2, '0');
				this.enemyCombatCardImage.enabled = player.combatCards.Count > 0;
			}
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x0003D395 File Offset: 0x0003B595
		private void SetCombatCardDoorActive(Transform slot, bool active, bool closed = true)
		{
			slot.GetChild(2).gameObject.SetActive(active);
			if (closed)
			{
				this.SetCombatCardDoorClosed(slot);
				return;
			}
			this.SetCombatCardDoorOpened(slot);
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x000C80F0 File Offset: 0x000C62F0
		public void SetCombatCardDoorOpened(Transform slot)
		{
			RectTransform component = slot.GetChild(2).GetChild(0).GetComponent<RectTransform>();
			RectTransform component2 = slot.GetChild(2).GetChild(1).GetComponent<RectTransform>();
			float x = component.rect.size.x;
			this.SetCoverPosition(component, -x);
			this.SetCoverPosition(component2, x);
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x000C8148 File Offset: 0x000C6348
		public void SetCombatCardDoorClosed(Transform slot)
		{
			RectTransform component = slot.GetChild(2).GetChild(0).GetComponent<RectTransform>();
			RectTransform component2 = slot.GetChild(2).GetChild(1).GetComponent<RectTransform>();
			this.SetCoverPosition(component, 0f);
			this.SetCoverPosition(component2, 0f);
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x000C8194 File Offset: 0x000C6394
		private void SetCoverPosition(RectTransform cover, float xPosition)
		{
			Vector3 localPosition = cover.localPosition;
			localPosition.x = xPosition;
			cover.localPosition = localPosition;
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x0003D3BB File Offset: 0x0003B5BB
		public void SetPowerCardsCounters(Player attacker, Player defender, Dictionary<Player, PowerSelected> usedPower)
		{
			this.SetCardCounters(PanelSide.Left, usedPower[attacker]);
			this.SetCardCounters(PanelSide.Right, usedPower[defender]);
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x000C81B8 File Offset: 0x000C63B8
		private void SetCardCounters(PanelSide panel, PowerSelected usedPower)
		{
			for (int i = 0; i < 6; i++)
			{
				if (i < usedPower.selectedCards.Count)
				{
					this.SetBomb((int)panel, i, true, usedPower.selectedCards[i].CombatBonus);
				}
				else
				{
					this.SetBomb((int)panel, i, false, 2);
				}
			}
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x000C8208 File Offset: 0x000C6408
		private void SetBomb(int panel, int id, bool active, int combatBonus)
		{
			Transform transform = this.combatControls[panel].Bomb(id);
			transform.GetChild(1).gameObject.SetActive(active);
			transform.GetChild(1).GetChild(3).GetComponent<Image>()
				.sprite = this.bombDigits[combatBonus - 2];
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000C8258 File Offset: 0x000C6458
		public void SetBombsInteractable(PanelSide panel, bool interactable, int numberOfCards = 6)
		{
			for (int i = 0; i < numberOfCards; i++)
			{
				this.combatControls[(int)panel].Bomb(i).GetChild(1).GetComponent<Button>()
					.interactable = interactable;
			}
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x0003D3D9 File Offset: 0x0003B5D9
		private void SetLeftOverallPower(float totalPower)
		{
			this.SetOverallPower(PanelSide.Left, (int)totalPower);
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x0003D3E4 File Offset: 0x0003B5E4
		private void SetRightOverallPower(float totalPower)
		{
			this.SetOverallPower(PanelSide.Right, (int)totalPower);
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x0003D3EF File Offset: 0x0003B5EF
		public void SetOverallPower(PanelSide panel, int power)
		{
			if (power != -1)
			{
				this.combatControls[(int)panel].overallPower.text = power.ToString();
				return;
			}
			this.combatControls[(int)panel].overallPower.text = "?";
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x0003D426 File Offset: 0x0003B626
		public void SetPossiblePower(int power, PanelSide panelSide)
		{
			this.combatControls[(int)panelSide].possiblePowerRange.text = "0 - " + power.ToString();
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x0003D44B File Offset: 0x0003B64B
		public void SetSliderListener(PanelSide panel, UnityAction<float> call)
		{
			this.RemoveSliderListeners(panel);
			this.combatControls[(int)panel].powerSlider.onValueChanged.AddListener(call);
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x0003D46C File Offset: 0x0003B66C
		public void RemoveSliderListeners(PanelSide panel)
		{
			this.combatControls[(int)panel].powerSlider.onValueChanged.RemoveAllListeners();
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x0003D485 File Offset: 0x0003B685
		public int GetSliderValue(PanelSide panel)
		{
			return (int)this.combatControls[(int)panel].powerSlider.value;
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x0003D49A File Offset: 0x0003B69A
		public void SetPowerUnavaliable(PanelSide panel, int power)
		{
			this.combatControls[(int)panel].powerUnavaliableSlider.value = (float)power;
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x0003D4B0 File Offset: 0x0003B6B0
		public void SetSliderValue(PanelSide panel, int power)
		{
			this.combatControls[(int)panel].powerSlider.value = (float)power;
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x0003D4C6 File Offset: 0x0003B6C6
		public void SetPossiblePowerActive(bool active, PanelSide panelSide)
		{
			this.combatControls[(int)panelSide].possiblePowerRange.gameObject.SetActive(active);
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x000C8294 File Offset: 0x000C6494
		public void SetAlfaOnSliderTics(int power)
		{
			for (int i = 0; i < this.sliderScaleTicks.childCount; i++)
			{
				if (i <= power)
				{
					this.sliderScaleTicks.GetChild(i).GetComponent<Image>().color = this.enoughPowerColor;
				}
				else
				{
					this.sliderScaleTicks.GetChild(i).GetComponent<Image>().color = this.notEnoughPowerColor;
				}
			}
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x0003D4E0 File Offset: 0x0003B6E0
		public void SetSliderInteractable(PanelSide panel, bool interactable)
		{
			this.combatControls[(int)panel].powerSlider.interactable = interactable;
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x0003D4F5 File Offset: 0x0003B6F5
		public void SetSliderHandleActive(PanelSide panel, bool active)
		{
			this.combatControls[(int)panel].powerSlider.transform.GetChild(3).GetChild(0).gameObject.SetActive(active);
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x000C82F8 File Offset: 0x000C64F8
		public void ResetSliderHandleSize(PanelSide panel)
		{
			this.combatControls[(int)panel].powerSlider.transform.GetChild(3).GetChild(0).DOComplete(false);
			this.combatControls[(int)panel].powerSlider.transform.GetChild(3).GetChild(0).localScale = Vector3.one;
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x0003D520 File Offset: 0x0003B720
		public void PopupCombatPanel()
		{
			base.GetComponent<Animator>().Play("CombatWindowPopUp");
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x000C8354 File Offset: 0x000C6554
		public void ShowCombatCardsPresenter()
		{
			this.leftCombatCardPresenter.DOLocalMoveX(-250f, 1f, false).SetEase(Ease.OutQuad).Play<Tweener>();
			this.rightCombatCardPresenter.DOLocalMoveX(250f, 1f, false).SetEase(Ease.OutQuad).Play<Tweener>();
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x000C83A8 File Offset: 0x000C65A8
		public void HideCombatCardsPresenter()
		{
			this.leftCombatCardPresenter.DOLocalMoveX(this.leftCombatCardPresenterPosition.x, 1f, false).SetEase(Ease.InQuad).Play<Tweener>();
			this.rightCombatCardPresenter.DOLocalMoveX(this.rightCombatCardPresenterPosition.x, 1f, false).SetEase(Ease.InQuad).Play<Tweener>();
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x000C8408 File Offset: 0x000C6608
		public Sequence OpenCombatCardSlot(PanelSide panel, int i)
		{
			Transform child = this.combatControls[(int)panel].bombs.GetChild(i);
			RectTransform component = child.GetChild(2).GetChild(0).GetComponent<RectTransform>();
			RectTransform component2 = child.GetChild(2).GetChild(1).GetComponent<RectTransform>();
			Sequence sequence = DOTween.Sequence();
			float x = component.rect.size.x;
			sequence.Append(component.DOLocalMoveX(component.localPosition.x - x, 1.5f, false).SetEase(Ease.OutExpo));
			sequence.Join(component2.DOLocalMoveX(component2.localPosition.x + x, 1.5f, false).SetEase(Ease.OutExpo));
			return sequence;
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x000C84B4 File Offset: 0x000C66B4
		public Sequence CloseCombatCardSlots(PanelSide panel, int i)
		{
			Transform child = this.combatControls[(int)panel].bombs.GetChild(i);
			RectTransform component = child.GetChild(2).GetChild(0).GetComponent<RectTransform>();
			RectTransform component2 = child.GetChild(2).GetChild(1).GetComponent<RectTransform>();
			Sequence sequence = DOTween.Sequence();
			float x = component.rect.size.x;
			sequence.Append(component.DOLocalMoveX(component.localPosition.x + x, 1.5f, false).SetEase(Ease.OutExpo));
			sequence.Join(component2.DOLocalMoveX(component2.localPosition.x - x, 1.5f, false).SetEase(Ease.OutExpo));
			return sequence;
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x0003D532 File Offset: 0x0003B732
		public Tween MinimizeSliderHandle(PanelSide panel, float duration)
		{
			return this.combatControls[(int)panel].powerSlider.transform.GetChild(3).GetChild(0).DOScale(0f, duration);
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x0003D55D File Offset: 0x0003B75D
		public Sequence FillSliders(float leftValue, float rightValue, float duration)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.Append(this.FillSlider(PanelSide.Left, leftValue, duration));
			sequence.Join(this.FillSlider(PanelSide.Right, rightValue, duration));
			return sequence;
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x000C8560 File Offset: 0x000C6760
		public Tween FillSlider(PanelSide panel, float endValue, float duration)
		{
			int panelID = (int)panel;
			this.combatControls[panelID].powerSlider.wholeNumbers = false;
			return this.combatControls[panelID].powerSlider.DOValue(endValue, duration, false).OnComplete(delegate
			{
				this.combatControls[panelID].powerSlider.wholeNumbers = true;
			});
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x0003D584 File Offset: 0x0003B784
		public Tween CountPointsLeft(int from, int to, float duration)
		{
			return DOVirtual.Float((float)from, (float)to, duration, new TweenCallback<float>(this.SetLeftOverallPower));
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x0003D59C File Offset: 0x0003B79C
		public Tween CountPointsRight(int from, int to, float duration)
		{
			return DOVirtual.Float((float)from, (float)to, duration, new TweenCallback<float>(this.SetRightOverallPower));
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x0003D5B4 File Offset: 0x0003B7B4
		public Tween AnimateTotalPointsSize(PanelSide panel, float size, float duration)
		{
			return this.combatControls[(int)panel].overallPower.transform.DOScale(size, duration);
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x0003D5CF File Offset: 0x0003B7CF
		public Tween AnimateLogoSize(PanelSide panel, float size, float duration)
		{
			return this.combatControls[(int)panel].factionLogo.transform.DOScale(size, duration);
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x000C85C4 File Offset: 0x000C67C4
		public Sequence ShowWinnerText(string text, float endScale, float duration)
		{
			this.winnerText.text = text;
			Color color = this.winnerText.color;
			color.a = 0f;
			this.winnerText.color = color;
			this.winnerText.transform.localScale = Vector3.zero;
			this.winnerText.gameObject.SetActive(true);
			Sequence sequence = DOTween.Sequence();
			sequence.Append(this.winnerText.transform.DOScale(endScale, duration).SetEase(Ease.OutQuad));
			sequence.Join(this.winnerText.DOFade(1f, duration).SetEase(Ease.OutQuad));
			return sequence;
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x000C866C File Offset: 0x000C686C
		public void ZoomToBattlefield()
		{
			if (GameController.GameManager.combatManager.GetActualStage() == CombatStage.Preparation)
			{
				GameController.Instance.combatPresenter.skipPreparation = true;
			}
			if (GameController.GameManager.combatManager.GetActualStage() == CombatStage.DeterminatingTheWinner)
			{
				base.GetComponent<BattleResultPanel>().blockWinnerAnimation = true;
			}
			ShowEnemyMoves.Instance.AnimateCamToHex(this.battlefield.GetWorldPosition());
		}

		// Token: 0x0400173E RID: 5950
		public CombatControls[] combatControls = new CombatControls[2];

		// Token: 0x0400173F RID: 5951
		public Button doneButton;

		// Token: 0x04001740 RID: 5952
		public Text winnerText;

		// Token: 0x04001741 RID: 5953
		public Sprite bombSlotEnabled;

		// Token: 0x04001742 RID: 5954
		public Sprite bombSlotDisabled;

		// Token: 0x04001743 RID: 5955
		public Sprite[] bombDigits = new Sprite[4];

		// Token: 0x04001744 RID: 5956
		public RectTransform leftCombatCardPresenter;

		// Token: 0x04001745 RID: 5957
		public RectTransform rightCombatCardPresenter;

		// Token: 0x04001746 RID: 5958
		public Transform sliderScaleTicks;

		// Token: 0x04001747 RID: 5959
		public GameObject combatWindow;

		// Token: 0x04001748 RID: 5960
		public Text enemyCombatCardsCount;

		// Token: 0x04001749 RID: 5961
		public Image enemyCombatCardImage;

		// Token: 0x0400174A RID: 5962
		private const int LEFT_PANEL = 0;

		// Token: 0x0400174B RID: 5963
		private const int RIGHT_PANEL = 1;

		// Token: 0x0400174C RID: 5964
		private Player leftPlayer;

		// Token: 0x0400174D RID: 5965
		private Player rightPlayer;

		// Token: 0x0400174E RID: 5966
		private Color lightMaxColor = Color.blue;

		// Token: 0x0400174F RID: 5967
		private Color enoughPowerColor = Color.white;

		// Token: 0x04001750 RID: 5968
		private Color notEnoughPowerColor = new Color(1f, 1f, 1f, 0.25f);

		// Token: 0x04001751 RID: 5969
		private const float COMBAT_CARD_OPEN_TIME = 1.5f;

		// Token: 0x04001752 RID: 5970
		private const float COMBAT_CARD_OPEN_OFFSET = 1f;

		// Token: 0x04001753 RID: 5971
		private Vector3 leftCombatCardPresenterPosition;

		// Token: 0x04001754 RID: 5972
		private Vector3 rightCombatCardPresenterPosition;

		// Token: 0x04001755 RID: 5973
		private GameHexPresenter battlefield;

		// Token: 0x04001756 RID: 5974
		private CombatPanelAction state;
	}
}

using System;
using System.Collections.Generic;
using DG.Tweening;
using HoneyFramework;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200042E RID: 1070
	public class BattleResultPanel : MonoBehaviour
	{
		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x060020CD RID: 8397 RVA: 0x000C63E4 File Offset: 0x000C45E4
		// (remove) Token: 0x060020CE RID: 8398 RVA: 0x000C6418 File Offset: 0x000C4618
		public static event BattleResultPanel.AIBattleResultAccept OnResultAccept;

		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x060020CF RID: 8399 RVA: 0x000C644C File Offset: 0x000C464C
		// (remove) Token: 0x060020D0 RID: 8400 RVA: 0x000C6480 File Offset: 0x000C4680
		public static event BattleResultPanel.ShowWinnerAnimationComplete OnShowWinnerAnimationComplete;

		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x060020D1 RID: 8401 RVA: 0x000C64B4 File Offset: 0x000C46B4
		// (remove) Token: 0x060020D2 RID: 8402 RVA: 0x000C64EC File Offset: 0x000C46EC
		public event Action OnPanelClose;

		// Token: 0x060020D3 RID: 8403 RVA: 0x0003CEDD File Offset: 0x0003B0DD
		private void OnDisable()
		{
			this.active = false;
			OptionsManager.OnLanguageChanged -= this.UpdateTexts;
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x0003CEF7 File Offset: 0x0003B0F7
		public bool IsOldBattleResultVisible()
		{
			return this.active;
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x0003CEFF File Offset: 0x0003B0FF
		public void SetLastPlayer(Player player)
		{
			this.leftPlayer = player;
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x000C6524 File Offset: 0x000C4724
		public void ChangeLayout(GameHex battlefield, bool attackerIsWinner, Player attacker, Player defender, Dictionary<Player, PowerSelected> usedPower)
		{
			if (this.leftPlayer == null)
			{
				this.leftPlayer = attacker;
			}
			this.rightPlayer = ((this.leftPlayer.matFaction.faction == attacker.matFaction.faction) ? defender : attacker);
			this.attacker = attacker;
			this.defender = defender;
			this.attackerIsWinner = attackerIsWinner;
			this.battlefield = battlefield;
			this.usedPower = new Dictionary<Player, PowerSelected>(usedPower);
			if (!this.blockWinnerAnimation)
			{
				if (this.ShouldAnimateHidingLeftPanel())
				{
					this.HideLeftPlayerPowerAnimation();
				}
				else
				{
					this.FillControls();
				}
			}
			this.blockWinnerAnimation = false;
			this.combatPanel.SetBattlefield(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(battlefield));
			if (GameController.GameManager.IsMultiplayer)
			{
				GameController.Instance.combatPresenter.TurnOffBattlefieldEffect();
			}
			OptionsManager.OnLanguageChanged += this.UpdateTexts;
			this.uiDarken.enabled = true;
			base.enabled = true;
			this.active = true;
			if (!base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(true);
				base.GetComponent<CombatPanel>().PopupCombatPanel();
			}
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x0003CF08 File Offset: 0x0003B108
		private bool ShouldAnimateHidingLeftPanel()
		{
			return GameController.GameManager.IsHotSeat && this.leftPlayer.IsHuman && this.rightPlayer.IsHuman;
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x0003CF32 File Offset: 0x0003B132
		private bool AnimatePanelForObserver()
		{
			return GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerOwner != this.leftPlayer && GameController.GameManager.PlayerOwner != this.rightPlayer;
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x000C663C File Offset: 0x000C483C
		private void FillControls()
		{
			this.combatPanel.DoneButtonTextBestFit(false);
			this.combatPanel.UpdateDoneButton(null, CombatPanelAction.wait);
			this.combatPanel.SetDoneButtonInteractable(false);
			this.combatPanel.UpdateLogos(this.leftPlayer.matFaction.faction, this.rightPlayer.matFaction.faction);
			this.combatPanel.UpdateLabels(this.leftPlayer);
			this.combatPanel.UpdateUnitsImages(this.leftPlayer);
			this.combatPanel.SetSliderInteractable(PanelSide.Left, false);
			this.combatPanel.SetAlfaOnSliderTics(7);
			if (GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerOwner != this.attacker && GameController.GameManager.PlayerOwner != this.defender)
			{
				int num = 7 - (this.usedPower[this.leftPlayer].selectedPower + this.leftPlayer.Power);
				this.combatPanel.SetPowerUnavaliable(PanelSide.Left, num);
				num = 7 - (this.usedPower[this.rightPlayer].selectedPower + this.rightPlayer.Power);
				this.combatPanel.SetPowerUnavaliable(PanelSide.Right, num);
			}
			this.combatPanel.UpdateCardSlots(PanelSide.Left, this.combatPanel.NumberOfCardSlots(this.leftPlayer, this.usedPower[this.leftPlayer].selectedCards));
			this.combatPanel.UpdateCardSlots(PanelSide.Right, this.combatPanel.NumberOfCardSlots(this.rightPlayer, this.usedPower[this.rightPlayer].selectedCards));
			this.combatPanel.SetPowerCardsCounters(this.leftPlayer, this.rightPlayer, this.usedPower);
			this.combatPanel.SetCombatCardsDoorActive(PanelSide.Right, true, this.rightPlayer, this.combatPanel.NumberOfCardSlots(this.rightPlayer, this.usedPower[this.rightPlayer].selectedCards), true);
			this.SetPowerMeters();
			this.ShowWinnerAnimation();
			if (GameController.GameManager.IsMultiplayer)
			{
				GameController.GameManager.combatManager.SwitchToNextStage();
			}
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x000C6850 File Offset: 0x000C4A50
		private void UpdateUnitAnimations(Player winner, Player loser)
		{
			foreach (Unit unit in winner.GetAllUnits())
			{
				if (unit.position.posX == this.battlefield.posX && unit.position.posY == this.battlefield.posY)
				{
					GameController.GetUnitPresenter(unit).transform.rotation = Quaternion.Euler(Vector3.zero);
				}
			}
			foreach (Unit unit2 in loser.GetAllUnits())
			{
				if (unit2.position.posX == this.battlefield.posX && unit2.position.posY == this.battlefield.posY)
				{
					GameController.GetUnitPresenter(unit2).transform.rotation = Quaternion.Euler(Vector3.zero);
				}
			}
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000C696C File Offset: 0x000C4B6C
		private void UpdateTexts()
		{
			this.combatPanel.UpdateLabels(this.leftPlayer);
			if (this.combatPanel.WinnerTextIsActive())
			{
				Faction faction = (this.attackerIsWinner ? this.attacker.matFaction.faction : this.defender.matFaction.faction);
				this.combatPanel.UpdateWinnerText(ScriptLocalization.Get("GameScene/Victory" + faction.ToString()));
			}
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x0003CF6B File Offset: 0x0003B16B
		private int CalculatePlayerPowerPoints(Player player)
		{
			return this.usedPower[player].cardsPower + this.usedPower[player].selectedPower;
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000C69EC File Offset: 0x000C4BEC
		private void SetPowerMeters()
		{
			this.combatPanel.SetPossiblePowerActive(false, PanelSide.Right);
			int num = this.CalculatePlayerPowerPoints(GameController.GameManager.combatManager.GetAttacker());
			int num2 = this.CalculatePlayerPowerPoints(GameController.GameManager.combatManager.GetDefender());
			if (this.ShouldAnimateHidingLeftPanel() || this.AnimatePanelForObserver())
			{
				this.combatPanel.SetSliderValue(PanelSide.Left, 0);
				this.combatPanel.SetOverallPower(PanelSide.Left, 0);
			}
			else
			{
				this.SetLeftPlayerValues();
			}
			this.combatPanel.SetSliderValue(PanelSide.Right, 0);
			this.combatPanel.SetOverallPower(PanelSide.Right, 0);
			if (this.attackerIsWinner)
			{
				AchievementManager.UpdateAchievementCombat(this.battlefield, this.attacker, num, num2);
				return;
			}
			AchievementManager.UpdateAchievementCombat(this.battlefield, this.defender, num2, num);
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000C6AB0 File Offset: 0x000C4CB0
		private void SetLeftPlayerValues()
		{
			this.combatPanel.SetSliderValue(PanelSide.Left, this.usedPower[this.leftPlayer].selectedPower);
			this.combatPanel.SetOverallPower(PanelSide.Left, this.usedPower[this.leftPlayer].cardsPower + this.usedPower[this.leftPlayer].selectedPower);
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x000C6B18 File Offset: 0x000C4D18
		private int[] CountCards(Player player)
		{
			int[] array = new int[4];
			foreach (CombatCard combatCard in this.usedPower[player].selectedCards)
			{
				int num = 5 - combatCard.CombatBonus;
				array[num]++;
			}
			return array;
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x0003CF90 File Offset: 0x0003B190
		public void OnShowClicked()
		{
			this.FillControls();
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x0003CF98 File Offset: 0x0003B198
		public void OnClosePanelClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_ok_button);
			this.ClosePanel(GameController.GameManager.IsMultiplayer);
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000C6B8C File Offset: 0x000C4D8C
		public void ClosePanel(bool donNotCallNextStage = false)
		{
			OptionsManager.OnLanguageChanged -= this.UpdateTexts;
			this.combatPanel.SetCombatCardDoorsActive(false, this.leftPlayer, this.rightPlayer, this.usedPower);
			this.combatPanel.SetSliderInteractable(PanelSide.Left, true);
			this.combatPanel.RemoveSliderListeners(PanelSide.Left);
			this.combatPanel.RemoveSliderListeners(PanelSide.Right);
			this.leftPlayer = (this.rightPlayer = null);
			this.uiDarken.enabled = false;
			this.usedPower = new Dictionary<Player, PowerSelected>();
			if (this.battlefield != null)
			{
				GameController.Instance.gameBoardPresenter.GetGameHexPresenter(this.battlefield).SetFocus(false, HexMarkers.MarkerType.Battle, 0f, false);
			}
			if (!donNotCallNextStage && !GameController.GameManager.IsMultiplayer)
			{
				GameController.GameManager.combatManager.SwitchToNextStage();
			}
			if (GameController.GameManager.IsCampaign && GameController.GameManager.missionId == 2 && BattleResultPanel.OnResultAccept != null)
			{
				BattleResultPanel.OnResultAccept();
			}
			base.enabled = false;
			this.active = false;
			base.gameObject.SetActive(false);
			Action onPanelClose = this.OnPanelClose;
			if (onPanelClose == null)
			{
				return;
			}
			onPanelClose();
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x0003CFB0 File Offset: 0x0003B1B0
		public void QuickCloseBeforeOpening()
		{
			this.ClosePanel(true);
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x000C6CB0 File Offset: 0x000C4EB0
		private void HideLeftPlayerPowerAnimation()
		{
			this.combatPanel.RemoveSliderListeners(PanelSide.Left);
			this.combatPanel.RemoveSliderListeners(PanelSide.Right);
			this.combatPanel.SetSliderInteractable(PanelSide.Left, false);
			this.combatPanel.SetAlfaOnSliderTics(7);
			this.combatPanel.SetCombatCardsDoorActive(PanelSide.Left, true, this.leftPlayer, this.combatPanel.NumberOfCardSlots(this.leftPlayer, this.usedPower[this.leftPlayer].selectedCards), false);
			Sequence sequence = DOTween.Sequence();
			sequence.Append(this.combatPanel.MinimizeSliderHandle(PanelSide.Left, 0.2f));
			if (this.combatPanel.NumberOfCardSlots(this.leftPlayer, this.usedPower[this.leftPlayer].selectedCards) > 0)
			{
				sequence.Append(this.CloseLeftCardSlots());
			}
			sequence.Join(this.EmptyLeftPowerSlider());
			sequence.Join(this.CountDownLeftOverallPower());
			sequence.OnComplete(delegate
			{
				this.OnHidingComplete();
			});
			sequence.PlayForward();
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x000C6DB4 File Offset: 0x000C4FB4
		private Sequence CloseLeftCardSlots()
		{
			Sequence sequence = DOTween.Sequence();
			int num = this.combatPanel.NumberOfCardSlots(this.leftPlayer, this.usedPower[this.leftPlayer].selectedCards);
			for (int i = 0; i < num; i++)
			{
				sequence.Join(this.combatPanel.CloseCombatCardSlots(PanelSide.Left, i));
			}
			return sequence;
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x0003CFB9 File Offset: 0x0003B1B9
		private Tween EmptyLeftPowerSlider()
		{
			return this.combatPanel.FillSlider(PanelSide.Left, 0f, 1.5f);
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x0003CFD1 File Offset: 0x0003B1D1
		private Tween CountDownLeftOverallPower()
		{
			return this.combatPanel.CountPointsLeft(this.usedPower[this.leftPlayer].cardsPower + this.usedPower[this.leftPlayer].selectedPower, 0, 1.5f);
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x0003D011 File Offset: 0x0003B211
		private void OnHidingComplete()
		{
			this.combatPanel.DoneButtonTextBestFit(false);
			this.combatPanel.UpdateDoneButton(new UnityAction(this.OnShowClicked), CombatPanelAction.result);
			this.combatPanel.SetDoneButtonInteractable(true);
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x000C6E10 File Offset: 0x000C5010
		private void ShowWinnerAnimation()
		{
			this.combatPanel.SetSliderListener(PanelSide.Left, new UnityAction<float>(this.OnLeftSliderValueChanged));
			this.combatPanel.SetSliderListener(PanelSide.Right, new UnityAction<float>(this.OnRightSliderValueChanged));
			Sequence sequence = DOTween.Sequence();
			sequence.Append(this.OpenCardSlots());
			sequence.Append(this.FillUpPowerSliders());
			sequence.Append(this.UpdateLogoAndTexts());
			sequence.Join(this.ShowWinnerText());
			sequence.OnComplete(delegate
			{
				this.OnAnimationsComplete();
			});
			sequence.PlayForward();
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x000C6EA0 File Offset: 0x000C50A0
		private void OnAnimationsComplete()
		{
			if (GameController.GameManager.combatManager.GetActualStage() == CombatStage.Preparation || GameController.GameManager.combatManager.GetActualStage() == CombatStage.Diversion)
			{
				return;
			}
			this.combatPanel.DoneButtonTextBestFit(false);
			this.combatPanel.UpdateDoneButton(new UnityAction(this.OnClosePanelClicked), CombatPanelAction.ok);
			this.combatPanel.SetDoneButtonInteractable(true);
			WorldSFXManager.PlaySound(SoundEnum.AttackResultInfo, AudioSourceType.WorldSfx);
			if (BattleResultPanel.OnShowWinnerAnimationComplete != null)
			{
				BattleResultPanel.OnShowWinnerAnimationComplete();
			}
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000C6F1C File Offset: 0x000C511C
		private Sequence OpenCardSlots()
		{
			Sequence sequence = DOTween.Sequence();
			if (this.ShouldAnimateHidingLeftPanel() || this.AnimatePanelForObserver())
			{
				int num = this.combatPanel.NumberOfCardSlots(this.leftPlayer, this.usedPower[this.leftPlayer].selectedCards);
				if (num != 0)
				{
					this.cardsPowerLeft = this.usedPower[this.leftPlayer].cardsPower;
					sequence.Join(this.CreateLeftCardsSlotsOpeningSequence(num, true));
				}
			}
			int num2 = this.combatPanel.NumberOfCardSlots(this.rightPlayer, this.usedPower[this.rightPlayer].selectedCards);
			if (num2 != 0)
			{
				this.cardsPowerRight = this.usedPower[this.rightPlayer].cardsPower;
				sequence.Join(this.CreateRightCardsSlotsOpeningSequence(num2, true));
			}
			if (sequence.Duration(true) != 0f)
			{
				return sequence;
			}
			return null;
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000C7000 File Offset: 0x000C5200
		private Sequence CreateLeftCardsSlotsOpeningSequence(int depthLevel, bool start)
		{
			if (depthLevel == 0)
			{
				return null;
			}
			depthLevel--;
			int combatCardPowerForDepth = this.GetCombatCardPowerForDepth(this.leftPlayer, depthLevel);
			Sequence sequence = DOTween.Sequence();
			Sequence sequence2 = this.CreateLeftCardsSlotsOpeningSequence(depthLevel, false);
			Sequence sequence3 = DOTween.Sequence();
			if (depthLevel >= 0)
			{
				sequence3.Append(this.combatPanel.OpenCombatCardSlot(PanelSide.Left, depthLevel));
				if (combatCardPowerForDepth != 0)
				{
					sequence3.Append(this.combatPanel.CountPointsLeft(this.cardsPowerLeft - combatCardPowerForDepth, this.cardsPowerLeft, 0.5f));
					this.cardsPowerLeft -= combatCardPowerForDepth;
				}
			}
			if (!start)
			{
				sequence.AppendInterval(1.1f * Time.timeScale);
			}
			sequence.Append(sequence3);
			if (sequence2 != null)
			{
				sequence.Join(sequence2);
			}
			return sequence;
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x000C70B4 File Offset: 0x000C52B4
		private Sequence CreateRightCardsSlotsOpeningSequence(int depthLevel, bool start)
		{
			if (depthLevel == 0)
			{
				return null;
			}
			depthLevel--;
			int combatCardPowerForDepth = this.GetCombatCardPowerForDepth(this.rightPlayer, depthLevel);
			Sequence sequence = DOTween.Sequence();
			Sequence sequence2 = this.CreateRightCardsSlotsOpeningSequence(depthLevel, false);
			Sequence sequence3 = DOTween.Sequence();
			if (depthLevel >= 0)
			{
				sequence3.Append(this.combatPanel.OpenCombatCardSlot(PanelSide.Right, depthLevel));
				if (combatCardPowerForDepth != 0)
				{
					sequence3.Append(this.combatPanel.CountPointsRight(this.cardsPowerRight - combatCardPowerForDepth, this.cardsPowerRight, 0.5f));
					this.cardsPowerRight -= combatCardPowerForDepth;
				}
			}
			if (!start)
			{
				sequence.AppendInterval(1.1f * Time.timeScale);
			}
			sequence.Append(sequence3);
			if (sequence2 != null)
			{
				sequence.Join(sequence2);
			}
			return sequence;
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x0003D043 File Offset: 0x0003B243
		private int GetCombatCardPowerForDepth(Player player, int depth)
		{
			if (depth < this.usedPower[player].selectedCards.Count && depth >= 0)
			{
				return this.usedPower[player].selectedCards[depth].CombatBonus;
			}
			return 0;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x000C7168 File Offset: 0x000C5368
		private Sequence FillUpPowerSliders()
		{
			Sequence sequence = DOTween.Sequence();
			if (this.ShouldAnimateHidingLeftPanel() || this.AnimatePanelForObserver())
			{
				sequence.Join(this.combatPanel.FillSlider(PanelSide.Left, (float)this.usedPower[this.leftPlayer].selectedPower, 2f * Time.timeScale));
			}
			sequence.Join(this.combatPanel.FillSlider(PanelSide.Right, (float)this.usedPower[this.rightPlayer].selectedPower, 2f * Time.timeScale));
			return sequence;
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x000C71F8 File Offset: 0x000C53F8
		private void OnLeftSliderValueChanged(float value)
		{
			int cardsPower = this.usedPower[this.leftPlayer].cardsPower;
			this.combatPanel.SetOverallPower(PanelSide.Left, cardsPower + Mathf.FloorToInt(value));
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x000C7230 File Offset: 0x000C5430
		private void OnRightSliderValueChanged(float value)
		{
			int cardsPower = this.usedPower[this.rightPlayer].cardsPower;
			this.combatPanel.SetOverallPower(PanelSide.Right, cardsPower + Mathf.FloorToInt(value));
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000C7268 File Offset: 0x000C5468
		private Sequence UpdateLogoAndTexts()
		{
			Sequence sequence = DOTween.Sequence();
			int num = this.usedPower[this.leftPlayer].cardsPower + this.usedPower[this.leftPlayer].selectedPower;
			int num2 = this.usedPower[this.rightPlayer].cardsPower + this.usedPower[this.rightPlayer].selectedPower;
			if (num == num2)
			{
				if (this.leftPlayer == this.attacker)
				{
					this.LeftPlayerIsWinnerAnimations(sequence);
				}
				else
				{
					this.RightPlayerIsWinnerAnimations(sequence);
				}
			}
			else if (num < num2)
			{
				this.RightPlayerIsWinnerAnimations(sequence);
			}
			else
			{
				this.LeftPlayerIsWinnerAnimations(sequence);
			}
			return sequence;
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000C7310 File Offset: 0x000C5510
		private void LeftPlayerIsWinnerAnimations(Sequence pointsAndLogos)
		{
			pointsAndLogos.Append(this.combatPanel.AnimateTotalPointsSize(PanelSide.Left, 1.2f, 3f * Time.timeScale));
			pointsAndLogos.Join(this.combatPanel.AnimateLogoSize(PanelSide.Left, 1.3f, 3f * Time.timeScale));
			pointsAndLogos.Join(this.combatPanel.AnimateLogoSize(PanelSide.Right, 0.7f, 3f * Time.timeScale));
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x000C7388 File Offset: 0x000C5588
		private void RightPlayerIsWinnerAnimations(Sequence pointsAndLogos)
		{
			pointsAndLogos.Append(this.combatPanel.AnimateTotalPointsSize(PanelSide.Right, 1.2f, 3f * Time.timeScale));
			pointsAndLogos.Join(this.combatPanel.AnimateLogoSize(PanelSide.Left, 0.7f, 3f * Time.timeScale));
			pointsAndLogos.Join(this.combatPanel.AnimateLogoSize(PanelSide.Right, 1.3f, 3f * Time.timeScale));
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x000C7400 File Offset: 0x000C5600
		private Sequence ShowWinnerText()
		{
			Faction faction = (this.attackerIsWinner ? this.attacker.matFaction.faction : this.defender.matFaction.faction);
			return this.combatPanel.ShowWinnerText(ScriptLocalization.Get("GameScene/Victory" + faction.ToString()), 1f, 3f * Time.timeScale);
		}

		// Token: 0x040016F1 RID: 5873
		public CombatPanel combatPanel;

		// Token: 0x040016F2 RID: 5874
		public Image uiDarken;

		// Token: 0x040016F3 RID: 5875
		public bool blockWinnerAnimation;

		// Token: 0x040016F4 RID: 5876
		private GameHex battlefield;

		// Token: 0x040016F5 RID: 5877
		private Dictionary<Player, PowerSelected> usedPower;

		// Token: 0x040016F6 RID: 5878
		private Player attacker;

		// Token: 0x040016F7 RID: 5879
		private Player defender;

		// Token: 0x040016F8 RID: 5880
		private bool attackerIsWinner;

		// Token: 0x040016FB RID: 5883
		private const float COMBAT_CARD_OPEN_OFFSET = 1.1f;

		// Token: 0x040016FC RID: 5884
		private const float FILL_UP_SLIDER = 2f;

		// Token: 0x040016FD RID: 5885
		private const float LOGO_MAX_SIZE = 1.3f;

		// Token: 0x040016FE RID: 5886
		private const float LOGO_MIN_SIZE = 0.7f;

		// Token: 0x040016FF RID: 5887
		private const float OVERALL_POWER_TEST_SIZE = 1.2f;

		// Token: 0x04001700 RID: 5888
		private const float SHOW_WINNER_DURATION = 3f;

		// Token: 0x04001701 RID: 5889
		private int cardsPowerLeft;

		// Token: 0x04001702 RID: 5890
		private int cardsPowerRight;

		// Token: 0x04001703 RID: 5891
		private Player leftPlayer;

		// Token: 0x04001704 RID: 5892
		private Player rightPlayer;

		// Token: 0x04001705 RID: 5893
		private bool active;

		// Token: 0x0200042F RID: 1071
		private enum BattleResultState
		{
			// Token: 0x04001708 RID: 5896
			waiting_for_enemy,
			// Token: 0x04001709 RID: 5897
			waiting_for_player_input,
			// Token: 0x0400170A RID: 5898
			presenting_the_result,
			// Token: 0x0400170B RID: 5899
			waiting_for_closing
		}

		// Token: 0x02000430 RID: 1072
		// (Invoke) Token: 0x060020FA RID: 8442
		public delegate void AIBattleResultAccept();

		// Token: 0x02000431 RID: 1073
		// (Invoke) Token: 0x060020FE RID: 8446
		public delegate void ShowWinnerAnimationComplete();
	}
}

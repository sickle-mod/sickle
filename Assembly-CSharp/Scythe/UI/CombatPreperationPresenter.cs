using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.Events;

namespace Scythe.UI
{
	// Token: 0x0200043B RID: 1083
	public class CombatPreperationPresenter : MonoBehaviour
	{
		// Token: 0x140000DA RID: 218
		// (add) Token: 0x06002161 RID: 8545 RVA: 0x000C8730 File Offset: 0x000C6930
		// (remove) Token: 0x06002162 RID: 8546 RVA: 0x000C8764 File Offset: 0x000C6964
		public static event CombatPreperationPresenter.PowerChange OnPowerChanged;

		// Token: 0x140000DB RID: 219
		// (add) Token: 0x06002163 RID: 8547 RVA: 0x000C8798 File Offset: 0x000C6998
		// (remove) Token: 0x06002164 RID: 8548 RVA: 0x000C87CC File Offset: 0x000C69CC
		public static event CombatPreperationPresenter.CardAdd OnCardAdded;

		// Token: 0x140000DC RID: 220
		// (add) Token: 0x06002165 RID: 8549 RVA: 0x000C8800 File Offset: 0x000C6A00
		// (remove) Token: 0x06002166 RID: 8550 RVA: 0x000C8834 File Offset: 0x000C6A34
		public static event CombatPreperationPresenter.Fight OnFightClicked;

		// Token: 0x06002167 RID: 8551 RVA: 0x0003D609 File Offset: 0x0003B809
		private void Start()
		{
			this.battleResultPanel = base.GetComponent<BattleResultPanel>();
			this.battleResultPanel.OnPanelClose += this.BattleResultPanel_OnPanelClose;
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000C8868 File Offset: 0x000C6A68
		public void ChangeLayoutForPreparation(Player player, int combatCards)
		{
			base.enabled = true;
			this.player = player;
			Player enemyOf = GameController.GameManager.combatManager.GetEnemyOf(player);
			GameController.Instance.combatPresenter.statsRegular.transform.parent.SetParent(base.transform);
			this.combatPanel.UpdateDoneButton(new UnityAction(this.OnFightButtonClicked), CombatPanelAction.fight);
			this.combatPanel.SetDoneButtonInteractable(true);
			if (!GameController.GameManager.IsMultiplayer)
			{
				GameController.Instance.combatPresenter.statsRegular.UpdateAllStats(player, GameController.factionInfo[player.matFaction.faction].logo);
			}
			else
			{
				GameController.Instance.combatPresenter.statsRegular.UpdateAllStats(GameController.GameManager.PlayerOwner, GameController.factionInfo[GameController.GameManager.PlayerOwner.matFaction.faction].logo);
			}
			GameController.Instance.endTurnHintType = GameController.EndTurnHintType.Combat;
			this.combatPanel.UpdateLogos(player.matFaction.faction, enemyOf.matFaction.faction);
			this.combatPanel.ResetLogosScales();
			this.combatPanel.ResetTextsScale();
			this.combatPanel.SetSliderValue(PanelSide.Left, 0);
			this.combatPanel.SetSliderValue(PanelSide.Right, 0);
			this.combatPanel.SetAlfaOnSliderTics(player.Power);
			this.combatPanel.SetPowerUnavaliable(PanelSide.Left, 7 - player.Power);
			this.combatPanel.SetPowerUnavaliable(PanelSide.Right, 7 - enemyOf.Power);
			this.combatPanel.HideWinnerText();
			this.combatCardsPanel = this.hotseatOpponentAmmo;
			if (!GameController.GameManager.IsMultiplayer)
			{
				if (player != GameController.Instance.combatPresenter.GetPreviousPlayer() && player.IsHuman && player.IsHuman && enemyOf.IsHuman)
				{
					this.turnInfoPanel.ActivateTurnInfoPanelCombat(player);
				}
				this.hotseatOpponentAmmo.transform.parent.gameObject.SetActive(true);
			}
			this.combatPanel.SaveCombatCardPresenterPosition();
			this.combatPanel.ShowCombatCardsPresenter();
			this.InitCardSlots();
			this.combatPanel.UpdateLabels(player);
			this.combatPanel.UpdateUnitsImages(player);
			this.SetPossiblePower(enemyOf, PanelSide.Right);
			this.combatPanel.SetSliderListener(PanelSide.Left, new UnityAction<float>(this.PowerSliderChanged));
			this.combatPanel.SetSliderInteractable(PanelSide.Left, true);
			this.combatPanel.ResetSliderHandleSize(PanelSide.Left);
			this.combatPanel.SetBattlefield(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.combatManager.GetSelectedBattlefield()));
			this.combatPanel.SetDoneButtonColor(Color.red);
			OptionsManager.OnLanguageChanged += this.UpdateTexts;
			base.GetComponent<CombatPanel>().PopupCombatPanel();
			base.gameObject.SetActive(true);
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x000C8B34 File Offset: 0x000C6D34
		private void SetPossiblePower(Player enemy, PanelSide panelSide)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>
			{
				{ 2, 0 },
				{ 3, 0 },
				{ 4, 0 },
				{ 5, 0 }
			};
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>
			{
				{ 2, 16 },
				{ 3, 12 },
				{ 4, 8 },
				{ 5, 6 }
			};
			for (int i = 0; i < this.player.combatCards.Count; i++)
			{
				Dictionary<int, int> dictionary3 = dictionary2;
				int combatBonus = this.player.combatCards[i].CombatBonus;
				int num = dictionary3[combatBonus] + 1;
				dictionary3[combatBonus] = num;
			}
			this.combatPanel.SetOverallPower(panelSide, -1);
			int num2 = ((enemy.Power < 7) ? enemy.Power : 7);
			int j = GameController.GameManager.combatManager.GetPossibleAmountOfCombatCardsToUse(enemy);
			int num3 = 5;
			while (j > 0)
			{
				int num4 = dictionary2[num3] - dictionary[num3];
				if (j > num4)
				{
					num2 += num3 * num4;
					j -= num4;
				}
				else
				{
					num2 += num3 * j;
					j = 0;
				}
				num3--;
			}
			this.combatPanel.SetPossiblePower(num2, panelSide);
			this.combatPanel.SetPossiblePowerActive(true, panelSide);
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x0003D62E File Offset: 0x0003B82E
		private void UpdateTexts()
		{
			this.combatPanel.UpdateLabels(this.player);
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x0003D641 File Offset: 0x0003B841
		public void ChangeVisibility(bool visible)
		{
			if (this.combatWindow.activeSelf != visible)
			{
				this.combatWindow.SetActive(visible);
			}
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x000C8C68 File Offset: 0x000C6E68
		private void InitCardSlots()
		{
			this.selectedCards = new List<CombatCard>();
			this.combatPanel.SetCards(PanelSide.Left, this.selectedCards);
			this.UpdatePowerMeters();
			this.EnableCardSlots(this.player, PanelSide.Left);
			this.DisableCardSlots(GameController.GameManager.combatManager.GetEnemyOf(this.player), PanelSide.Right);
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x000C8CC4 File Offset: 0x000C6EC4
		private void EnableCardSlots(Player p, PanelSide panelSide)
		{
			this.numberOfCardsToUse = this.combatPanel.NumberOfCardSlots(p, null);
			this.combatPanel.UpdateCardSlots(panelSide, this.numberOfCardsToUse);
			this.combatCardsPanel.SetCards(p.combatCards, null);
			this.combatCardsPanel.BattleMode();
			this.combatCardsPanel.FocusCards(this.selectedCards.Count < this.numberOfCardsToUse);
			this.combatPanel.SetBombsInteractable(panelSide, true, 6);
			this.combatPanel.SetCombatCardsDoorActive(panelSide, false, p, 6, true);
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x0003D65D File Offset: 0x0003B85D
		private void DisableCardSlots(Player p, PanelSide panelSide)
		{
			this.combatPanel.UpdateCardSlots(panelSide, this.combatPanel.NumberOfCardSlots(p, null));
			this.combatPanel.SetCombatCardsDoorActive(panelSide, true, p, this.combatPanel.NumberOfCardSlots(p, null), true);
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x000C8D50 File Offset: 0x000C6F50
		private int GetCardsPower()
		{
			int num = 0;
			for (int i = 0; i < this.selectedCards.Count; i++)
			{
				num += this.selectedCards[i].CombatBonus;
			}
			return num;
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x0003D694 File Offset: 0x0003B894
		private void UpdatePowerMeters()
		{
			this.combatPanel.SetOverallPower(PanelSide.Left, this.combatPanel.GetSliderValue(PanelSide.Left) + this.GetCardsPower());
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x000C8D8C File Offset: 0x000C6F8C
		public void PowerSliderChanged(float value)
		{
			if (this.combatPanel.GetSliderValue(PanelSide.Left) > this.player.Power)
			{
				this.combatPanel.SetSliderValue(PanelSide.Left, this.player.Power);
				return;
			}
			WorldSFXManager.PlaySound(SoundEnum.AttacSetPower, AudioSourceType.Buttons);
			if (CombatPreperationPresenter.OnPowerChanged != null)
			{
				CombatPreperationPresenter.OnPowerChanged((float)this.combatPanel.GetSliderValue(PanelSide.Left));
			}
			this.UpdatePowerMeters();
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x000C8DF8 File Offset: 0x000C6FF8
		public void OnCardAdd(CombatCard cc)
		{
			WorldSFXManager.PlaySound(SoundEnum.AttacBolsterAmmoLoad, AudioSourceType.Buttons);
			GameController.Instance.matFaction.combatCardsPresenter.LockCard(cc);
			this.selectedCards.Add(cc);
			this.combatPanel.SetCards(PanelSide.Left, this.selectedCards);
			this.UpdatePowerMeters();
			this.combatCardsPanel.SetCards(this.player.combatCards, this.selectedCards);
			this.combatCardsPanel.BattleMode();
			this.combatCardsPanel.FocusCards(this.selectedCards.Count < this.numberOfCardsToUse);
			if (CombatPreperationPresenter.OnCardAdded != null)
			{
				CombatPreperationPresenter.OnCardAdded(cc, this.selectedCards);
			}
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x000C8EA4 File Offset: 0x000C70A4
		public void OnCardRemove(int id)
		{
			WorldSFXManager.PlaySound(SoundEnum.AttacBolsterAmmoUnload, AudioSourceType.Buttons);
			GameController.Instance.matFaction.combatCardsPresenter.LockCard(this.selectedCards[id]);
			this.selectedCards.RemoveAt(id);
			this.combatPanel.SetCards(PanelSide.Left, this.selectedCards);
			this.UpdatePowerMeters();
			this.combatCardsPanel.SetCards(this.player.combatCards, this.selectedCards);
			this.combatCardsPanel.BattleMode();
			this.combatCardsPanel.FocusCards(this.selectedCards.Count < this.numberOfCardsToUse);
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x000C8F44 File Offset: 0x000C7144
		public void AddPlayerPreparation()
		{
			PowerSelected powerSelected = default(PowerSelected);
			powerSelected.cardsPower = this.GetCardsPower();
			powerSelected.selectedPower = this.combatPanel.GetSliderValue(PanelSide.Left);
			powerSelected.selectedCards = this.selectedCards;
			GameController.GameManager.combatManager.AddPlayerPowerInBattle(this.player, powerSelected);
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x0003D6B5 File Offset: 0x0003B8B5
		public void OnFightButtonClicked()
		{
			this.FightButtonClicked();
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x0003D6BD File Offset: 0x0003B8BD
		private void FightButtonClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_accept_button);
			this.EndPreparation();
			WorldSFXManager.PlaySound(SoundEnum.AttackFightButtonClick, AudioSourceType.Buttons);
			if (CombatPreperationPresenter.OnFightClicked != null)
			{
				CombatPreperationPresenter.OnFightClicked();
			}
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x000C8F9C File Offset: 0x000C719C
		public void EndPreparation()
		{
			base.gameObject.GetComponent<BattleResultPanel>().SetLastPlayer(this.player);
			GameController.Instance.combatPresenter.statsRegular.transform.parent.SetParent(this.statsContainer);
			this.AddPlayerPreparation();
			OptionsManager.OnLanguageChanged -= this.UpdateTexts;
			GameController.Instance.combatPresenter.SetPreviousPlayer(this.player);
			this.combatCardsPanel.FocusCards(false);
			this.combatPanel.HideCombatCardsPresenter();
			this.combatPanel.SetBombsInteractable(PanelSide.Left, false, 6);
			this.combatPanel.RemoveSliderListeners(PanelSide.Left);
			this.combatPanel.SetSliderInteractable(PanelSide.Left, false);
			this.combatPanel.SetOverallPower(PanelSide.Right, 0);
			this.combatPanel.SetPossiblePowerActive(false, PanelSide.Right);
			this.combatPanel.SetDoneButtonColor(Color.white);
			this.combatPanel.SetDoneButtonInteractable(false);
			if (GameController.GameManager.IsMultiplayer)
			{
				string text = ScriptLocalization.Get("GameScene/WaitForOpponent");
				text.Remove(text.Length - 1);
				this.combatPanel.DoneButtonTextBestFit(true);
				this.combatPanel.UpdateDoneButton(null, CombatPanelAction.wait_for_opponent);
			}
			else
			{
				this.combatPanel.UpdateDoneButton(null, CombatPanelAction.wait);
			}
			GameController.GameManager.combatManager.SwitchToNextStage();
			base.GetComponent<BattleResultPanel>().blockWinnerAnimation = false;
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x000C90EC File Offset: 0x000C72EC
		public void ClosePanel()
		{
			this.combatPanel.RemoveSliderListeners(PanelSide.Left);
			this.player = null;
			GameController.Instance.combatPresenter.statsRegular.transform.parent.SetParent(this.statsContainer);
			this.combatCardsPanel.FocusCards(false);
			base.gameObject.SetActive(false);
			base.enabled = false;
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x000C9150 File Offset: 0x000C7350
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
			this.combatPanel.ZoomToBattlefield();
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x0003D6E3 File Offset: 0x0003B8E3
		private void BattleResultPanel_OnPanelClose()
		{
			GameController.Instance.combatPresenter.statsRegular.transform.parent.SetParent(this.statsContainer);
		}

		// Token: 0x04001759 RID: 5977
		public CombatPanel combatPanel;

		// Token: 0x0400175A RID: 5978
		public GameObject combatWindow;

		// Token: 0x0400175B RID: 5979
		public CombatCardsPanelPresenter hotseatOpponentAmmo;

		// Token: 0x0400175C RID: 5980
		public TurnInfoPanel turnInfoPanel;

		// Token: 0x0400175D RID: 5981
		public YesNoDialog zeroCombatWarning;

		// Token: 0x04001761 RID: 5985
		private BattleResultPanel battleResultPanel;

		// Token: 0x04001762 RID: 5986
		public int numberOfCardsToUse;

		// Token: 0x04001763 RID: 5987
		private Player player;

		// Token: 0x04001764 RID: 5988
		public List<CombatCard> selectedCards = new List<CombatCard>();

		// Token: 0x04001765 RID: 5989
		private CombatCardsPanelPresenter combatCardsPanel;

		// Token: 0x04001766 RID: 5990
		[SerializeField]
		private Transform statsContainer;

		// Token: 0x0200043C RID: 1084
		// (Invoke) Token: 0x0600217D RID: 8573
		public delegate void PowerChange(float powerValue);

		// Token: 0x0200043D RID: 1085
		// (Invoke) Token: 0x06002181 RID: 8577
		public delegate void CardAdd(CombatCard selectedCard, List<CombatCard> allSelectedCards);

		// Token: 0x0200043E RID: 1086
		// (Invoke) Token: 0x06002185 RID: 8581
		public delegate void Fight();
	}
}

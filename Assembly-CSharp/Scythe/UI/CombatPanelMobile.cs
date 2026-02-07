using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x02000442 RID: 1090
	public class CombatPanelMobile : MonoBehaviour
	{
		// Token: 0x140000DE RID: 222
		// (add) Token: 0x060021B5 RID: 8629 RVA: 0x000C9BC4 File Offset: 0x000C7DC4
		// (remove) Token: 0x060021B6 RID: 8630 RVA: 0x000C9BFC File Offset: 0x000C7DFC
		public event Action CombatPanelClosed;

		// Token: 0x060021B7 RID: 8631 RVA: 0x0003D8D4 File Offset: 0x0003BAD4
		private void Awake()
		{
			this._combatPanelSummary.FightButtonClicked += this.OnFightButtonClickd;
			this._combatPanelSummary.OkButtonClicked += this.OnOkButtonClicked;
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x0003D904 File Offset: 0x0003BB04
		public void ClosePanel()
		{
			this._playerPart.Reset();
			this._opponentPart.Reset();
			if (this.CombatPanelClosed != null)
			{
				this.CombatPanelClosed();
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x000C9C34 File Offset: 0x000C7E34
		public void ChangeLayoutForPreparation(Player player, Player opponent)
		{
			this._isPreparation = true;
			this._playerPart.Reset();
			this._opponentPart.Reset();
			this._playerPart.gameObject.SetActive(true);
			this._opponentPart.gameObject.SetActive(true);
			this._combatPanelResult.gameObject.SetActive(false);
			this._actualPlayer = player;
			this._playerPart.SetUpPart(player, true);
			this._playerPart.TotalPowerChanged += this.OnPlayerTotalPowerChanged;
			this._opponentPart.SetUpPart(opponent, true);
			this._combatPanelSummary.SetUpForPreparation();
			this._combatPanelSummary.UpdatePlayerPower(0);
			this._combatPanelSummary.SetOpponentMaxPossiblePower(Mathf.Min(7, opponent.Power) + Mathf.Min(opponent.combatCards.Count, GameController.GameManager.combatManager.GetPossibleAmountOfCombatCardsToUse(opponent)) * 5);
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x000C9D1C File Offset: 0x000C7F1C
		public void ChangeLayoutForResult(Player player, Player opponent)
		{
			this._isPreparation = false;
			this._playerPart.Reset();
			this._opponentPart.Reset();
			this._playerPart.TotalPowerChanged -= this.OnPlayerTotalPowerChanged;
			Dictionary<Player, PowerSelected> usedPowers = GameController.GameManager.combatManager.GetUsedPowers();
			this._playerPart.SetUpPart(player, false);
			this._opponentPart.SetUpPart(opponent, false);
			this._combatPanelSummary.SetUpForResult();
			this._combatPanelSummary.UpdatePlayerPower(usedPowers[player].cardsPower + usedPowers[player].selectedPower);
			this._combatPanelSummary.UpdateOpponentPower(usedPowers[opponent].cardsPower + usedPowers[opponent].selectedPower);
			this._combatPanelResult.gameObject.SetActive(true);
			this._combatPanelResult.SetResult(player, opponent, usedPowers);
			this.UpdateAchievements(usedPowers);
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x0003D93B File Offset: 0x0003BB3B
		public void OnShowMapButtonClicked()
		{
			if (this._isPreparation)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.OnOkButtonClicked();
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x000C9E00 File Offset: 0x000C8000
		private void UpdateAchievements(Dictionary<Player, PowerSelected> selectedPowers)
		{
			Player attacker = GameController.GameManager.combatManager.GetAttacker();
			Player defender = GameController.GameManager.combatManager.GetDefender();
			int num = selectedPowers[attacker].cardsPower + selectedPowers[attacker].selectedPower;
			int num2 = selectedPowers[defender].cardsPower + selectedPowers[defender].selectedPower;
			GameHex selectedBattlefield = GameController.GameManager.combatManager.GetSelectedBattlefield();
			if (GameController.GameManager.combatManager.AttackerIsWinner())
			{
				AchievementManager.UpdateAchievementCombat(selectedBattlefield, attacker, num, num2);
				return;
			}
			AchievementManager.UpdateAchievementCombat(selectedBattlefield, defender, num2, num);
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x0003D958 File Offset: 0x0003BB58
		private void OnPlayerTotalPowerChanged(int totalPower)
		{
			this._combatPanelSummary.UpdatePlayerPower(totalPower);
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x000C9E98 File Offset: 0x000C8098
		private void OnFightButtonClickd()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				this._playerPart.LockState();
			}
			PowerSelected powerSelected = default(PowerSelected);
			powerSelected.cardsPower = this._playerPart.BonusCombatCardPower;
			powerSelected.selectedPower = this._playerPart.DistributedPower;
			powerSelected.selectedCards = this._playerPart.SelectedCards;
			GameController.GameManager.combatManager.AddPlayerPowerInBattle(this._actualPlayer, powerSelected);
			GameController.GameManager.combatManager.SwitchToNextStage();
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x0003D966 File Offset: 0x0003BB66
		private void OnOkButtonClicked()
		{
			this.ClosePanel();
		}

		// Token: 0x04001785 RID: 6021
		[SerializeField]
		private CombatPanelPlayerPart _playerPart;

		// Token: 0x04001786 RID: 6022
		[SerializeField]
		private CombatPanelOpponentPart _opponentPart;

		// Token: 0x04001787 RID: 6023
		[SerializeField]
		private CombatPanelSummary _combatPanelSummary;

		// Token: 0x04001788 RID: 6024
		[SerializeField]
		private CombatPanelResult _combatPanelResult;

		// Token: 0x04001789 RID: 6025
		private Player _actualPlayer;

		// Token: 0x0400178A RID: 6026
		private Player _mockedOpponent;

		// Token: 0x0400178B RID: 6027
		private bool _isPreparation;
	}
}

using System;
using System.Text.RegularExpressions;
using I2.Loc;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000441 RID: 1089
	public class CombatEnemyActionInfo : MonoBehaviour
	{
		// Token: 0x060021A9 RID: 8617 RVA: 0x0003D896 File Offset: 0x0003BA96
		private void Awake()
		{
			OptionsManager.OnLanguageChanged += this.UpdateTexts;
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x0003D8A9 File Offset: 0x0003BAA9
		private void OnDestroy()
		{
			OptionsManager.OnLanguageChanged -= this.UpdateTexts;
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000C9A64 File Offset: 0x000C7C64
		public void Show(Player actualPlayer, CombatStage combatStage)
		{
			this.SetTitle();
			this.factionEmblem.sprite = GameController.factionInfo[actualPlayer.matFaction.faction].logo;
			base.gameObject.SetActive(true);
			this.SetInfoDescription(actualPlayer, combatStage);
			GameController.Instance.turnInfoPanel.DisableEnemyActionInfo();
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x00029172 File Offset: 0x00027372
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x00032537 File Offset: 0x00030737
		public bool IsActive()
		{
			return base.gameObject.activeInHierarchy;
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x000C9AC0 File Offset: 0x000C7CC0
		private void UpdateTexts()
		{
			if (base.gameObject.activeInHierarchy)
			{
				Player actualPlayer = GameController.GameManager.combatManager.GetActualPlayer();
				if (actualPlayer != null)
				{
					this.SetTitle();
					this.SetInfoDescription(actualPlayer, GameController.GameManager.combatManager.GetActualStage());
				}
			}
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x000C9B0C File Offset: 0x000C7D0C
		private void SetTitle()
		{
			string text = ScriptLocalization.Get("GameScene/WaitForOpponent");
			text = Regex.Replace(text, "\\t|\\n|\\r", "");
			this.title.text = text;
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000C9B44 File Offset: 0x000C7D44
		private void SetInfoDescription(Player actualPlayer, CombatStage combatStage)
		{
			if (combatStage == CombatStage.Diversion)
			{
				this.description.text = this.DiversionLocalizedTitle();
				return;
			}
			if (combatStage != CombatStage.EndingTheBattle)
			{
				this.Hide();
				return;
			}
			if (GameController.GameManager.combatManager.CanActualPlayerGetCombatCard())
			{
				this.description.text = this.BonusCombatCardTitle();
				return;
			}
			if (GameController.GameManager.combatManager.CanUseAfterBattleAbility(actualPlayer))
			{
				this.description.text = this.EndingTheBattleAbilityTitle();
				return;
			}
			this.Hide();
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x0003D8BC File Offset: 0x0003BABC
		private string DiversionLocalizedTitle()
		{
			return ScriptLocalization.Get("GameScene/CombatAbility");
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x0003D8C8 File Offset: 0x0003BAC8
		private string BonusCombatCardTitle()
		{
			return ScriptLocalization.Get("GameScene/DefenderDefeatBonus");
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x0003D8BC File Offset: 0x0003BABC
		private string EndingTheBattleAbilityTitle()
		{
			return ScriptLocalization.Get("GameScene/CombatAbility");
		}

		// Token: 0x0400177E RID: 6014
		private const string LOCALIZATION_INFO_TITLE_TERM = "GameScene/WaitForOpponent";

		// Token: 0x0400177F RID: 6015
		private const string LOCALIZATION_COMBAT_ABILITY_TERM = "GameScene/CombatAbility";

		// Token: 0x04001780 RID: 6016
		private const string LOCALIZATION_BONUS_COMBAT_CARD_TERM = "GameScene/DefenderDefeatBonus";

		// Token: 0x04001781 RID: 6017
		[SerializeField]
		private Image factionEmblem;

		// Token: 0x04001782 RID: 6018
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x04001783 RID: 6019
		[SerializeField]
		private TextMeshProUGUI description;
	}
}

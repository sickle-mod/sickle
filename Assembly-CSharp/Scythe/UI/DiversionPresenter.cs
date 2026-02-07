using System;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Multiplayer.Messages;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200043F RID: 1087
	public class DiversionPresenter : MonoBehaviour
	{
		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x000283FB File Offset: 0x000265FB
		private GameManager gameManager
		{
			get
			{
				return GameController.GameManager;
			}
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x0003D71C File Offset: 0x0003B91C
		private void OnEnable()
		{
			OptionsManager.OnLanguageChanged += this.UpdateLocalization;
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x0003D72F File Offset: 0x0003B92F
		private void OnDisable()
		{
			OptionsManager.OnLanguageChanged -= this.UpdateLocalization;
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000C91A8 File Offset: 0x000C73A8
		private string AbilityText(Player player)
		{
			int num = ((player.matFaction.faction == Faction.Albion && this.gameManager.combatManager.GetAttacker() == player) ? 1 : 2);
			return string.Concat(new string[]
			{
				this.gameManager.factionBasicInfo[player.matFaction.faction].mechAbilityTitles[num].ToUpper(),
				": ",
				this.gameManager.factionBasicInfo[player.matFaction.faction].mechAbilityDescriptions[num].Replace("|", " "),
				Environment.NewLine,
				ScriptLocalization.Get("GameScene/AbilityQuestion")
			});
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x000C9264 File Offset: 0x000C7464
		public void ChangeLayoutForPlayer(Player current, Player opponent)
		{
			this.player = current;
			this.opponent = opponent;
			this.abilityUsed = false;
			this.ability = this.GetUsedAbilityPerk();
			if (!GameController.GameManager.IsMultiplayer && this.player != GameController.Instance.combatPresenter.GetPreviousPlayer())
			{
				this.turnInfoPanel.ActivateTurnInfoPanelCombat(this.player);
			}
			this.UpdateAbilityDescription();
			this.factionEmblem.sprite = GameController.factionInfo[this.player.matFaction.faction].logo;
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x000C92F8 File Offset: 0x000C74F8
		public void ChangeLayoutForPlayer(Player player, GameHex battlefield)
		{
			this.player = player;
			this.abilityUsed = false;
			this.opponent = this.gameManager.combatManager.GetEnemyOf(player);
			this.ability = this.GetUsedAbilityPerk();
			if (!PlatformManager.IsMobile)
			{
				if (!GameController.GameManager.IsMultiplayer && player != GameController.Instance.combatPresenter.GetPreviousPlayer())
				{
					this.turnInfoPanel.ActivateTurnInfoPanelCombat(player);
				}
			}
			else if (!GameController.GameManager.IsMultiplayer && player != GameController.Instance.combatPresenterMobile.GetPreviousPlayer())
			{
				this.turnInfoPanel.ActivateTurnInfoPanelCombat(player);
			}
			this.UpdateAbilityDescription();
			this.factionEmblem.sprite = GameController.factionInfo[player.matFaction.faction].logo;
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x000C93C0 File Offset: 0x000C75C0
		public void UseAbility()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_yes_button);
			AbilityPerk abilityPerk = this.ability;
			if (abilityPerk <= AbilityPerk.Disarm)
			{
				if (abilityPerk != AbilityPerk.Artillery)
				{
					if (abilityPerk != AbilityPerk.Scout)
					{
						if (abilityPerk == AbilityPerk.Disarm)
						{
							this.RemovePowerFromEnemy(0);
						}
					}
					else
					{
						this.StealOpponentsCard();
					}
				}
				else
				{
					this.RemovePowerFromEnemy(1);
				}
			}
			else if (abilityPerk != AbilityPerk.Sword)
			{
				if (abilityPerk != AbilityPerk.Shield)
				{
					if (abilityPerk == AbilityPerk.Ronin)
					{
						this.BolsterBeforeCombat();
					}
				}
				else
				{
					this.BolsterBeforeCombat();
				}
			}
			else
			{
				this.RemovePowerFromEnemy(0);
			}
			if (PlatformManager.IsStandalone)
			{
				if (!GameController.GameManager.IsMultiplayer)
				{
					GameController.Instance.combatPresenter.SetLastUsedAbility(this.ability);
				}
			}
			else if (!GameController.GameManager.IsMultiplayer)
			{
				GameController.Instance.combatPresenterMobile.SetLastUsedAbility(this.ability);
			}
			this.abilityUsed = true;
			this.DiversionEnd();
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000C948C File Offset: 0x000C768C
		private AbilityPerk GetUsedAbilityPerk()
		{
			AbilityPerk abilityPerk;
			if (this.player.matFaction.faction == Faction.Albion && this.player.matFaction.SkillUnlocked[1] && this.player == this.gameManager.combatManager.GetAttacker())
			{
				abilityPerk = AbilityPerk.Sword;
			}
			else
			{
				abilityPerk = this.player.matFaction.abilities[2];
			}
			return abilityPerk;
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x000C94F8 File Offset: 0x000C76F8
		private void RemovePowerFromEnemy(int cost = 0)
		{
			GameController.GameManager.combatManager.RemovePowerFromEnemy(cost);
			Player playerMaster = GameController.GameManager.PlayerMaster;
			if (playerMaster.IsHuman)
			{
				GameController.FactionInfo factionInfo = GameController.factionInfo[playerMaster.matFaction.faction];
				GameController.Instance.playerStats.UpdateAllStats(playerMaster, factionInfo.logo);
				if (PlatformManager.IsStandalone)
				{
					GameController.Instance.panelInfo.UpdatePlayerInfo(factionInfo, GameController.GameManager.factionBasicInfo[playerMaster.matFaction.faction], playerMaster.objectiveCards, playerMaster.combatCards, false);
					return;
				}
				SingletonMono<TopMenuPanelsManager>.Instance.UpdatePlayerInfo(factionInfo, GameController.GameManager.factionBasicInfo[playerMaster.matFaction.faction], playerMaster.objectiveCards, playerMaster.combatCards, false);
			}
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x000C95C8 File Offset: 0x000C77C8
		private void BolsterBeforeCombat()
		{
			this.gameManager.combatManager.BolsterBeforeCombat();
			Player playerMaster = this.gameManager.PlayerMaster;
			if (playerMaster.IsHuman)
			{
				GameController.FactionInfo factionInfo = GameController.factionInfo[playerMaster.matFaction.faction];
				GameController.Instance.playerStats.UpdateAllStats(playerMaster, factionInfo.logo);
				if (PlatformManager.IsStandalone)
				{
					GameController.Instance.panelInfo.UpdatePlayerInfo(factionInfo, this.gameManager.factionBasicInfo[playerMaster.matFaction.faction], playerMaster.objectiveCards, playerMaster.combatCards, false);
					return;
				}
				SingletonMono<TopMenuPanelsManager>.Instance.UpdatePlayerInfo(factionInfo, this.gameManager.factionBasicInfo[playerMaster.matFaction.faction], playerMaster.objectiveCards, playerMaster.combatCards, false);
			}
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x000C969C File Offset: 0x000C789C
		private void StealOpponentsCard()
		{
			int num = new global::System.Random().Next(0, this.opponent.combatCards.Count);
			CombatCard combatCard = this.opponent.combatCards[num];
			GameController.GameManager.combatManager.StealCombatCardFromEnemy(combatCard);
			Player playerMaster = GameController.GameManager.PlayerMaster;
			if (playerMaster.IsHuman)
			{
				GameController.FactionInfo factionInfo = GameController.factionInfo[playerMaster.matFaction.faction];
				GameController.Instance.playerStats.UpdateAllStats(playerMaster, factionInfo.logo);
				if (PlatformManager.IsStandalone)
				{
					GameController.Instance.panelInfo.UpdatePlayerInfo(factionInfo, GameController.GameManager.factionBasicInfo[playerMaster.matFaction.faction], playerMaster.objectiveCards, playerMaster.combatCards, false);
					return;
				}
				SingletonMono<TopMenuPanelsManager>.Instance.UpdatePlayerInfo(factionInfo, GameController.GameManager.factionBasicInfo[playerMaster.matFaction.faction], playerMaster.objectiveCards, playerMaster.combatCards, false);
			}
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x0003D742 File Offset: 0x0003B942
		private void UpdateLocalization()
		{
			this.UpdateAbilityDescription();
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x0003D74A File Offset: 0x0003B94A
		private void UpdateAbilityDescription()
		{
			this.abilityDescription.text = this.AbilityText(this.player);
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x0003D763 File Offset: 0x0003B963
		public void OnNoButtonClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_no_button);
			this.DiversionEnd();
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x000C979C File Offset: 0x000C799C
		public void DiversionEnd()
		{
			base.gameObject.SetActive(false);
			if (PlatformManager.IsStandalone)
			{
				GameController.Instance.combatPresenter.SetPreviousPlayer(this.player);
			}
			else
			{
				GameController.Instance.combatPresenterMobile.SetPreviousPlayer(this.player);
			}
			if (!GameController.GameManager.IsMultiplayer)
			{
				this.player = (this.opponent = null);
				GameController.GameManager.combatManager.SwitchToNextStage();
				return;
			}
			this.player = (this.opponent = null);
			GameController.GameManager.combatManager.SwitchToNextStage();
			if (!this.abilityUsed)
			{
				GameController.GameManager.OnActionSent(new AbilityNotUsedMessage());
			}
		}

		// Token: 0x04001767 RID: 5991
		public Text abilityDescription;

		// Token: 0x04001768 RID: 5992
		public Image factionEmblem;

		// Token: 0x04001769 RID: 5993
		public TurnInfoPanel turnInfoPanel;

		// Token: 0x0400176A RID: 5994
		private Player player;

		// Token: 0x0400176B RID: 5995
		private Player opponent;

		// Token: 0x0400176C RID: 5996
		private bool abilityUsed;

		// Token: 0x0400176D RID: 5997
		private AbilityPerk ability = AbilityPerk.Artillery;
	}
}

using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200049F RID: 1183
	public class TopMenuFactionPresenter : MonoBehaviour
	{
		// Token: 0x0600257F RID: 9599 RVA: 0x0003FDE4 File Offset: 0x0003DFE4
		private void Awake()
		{
			OptionsManager.OnLanguageChanged += this.UpdateTexts;
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x000DED78 File Offset: 0x000DCF78
		private void Start()
		{
			Faction faction = Faction.Nordic;
			if (GameController.GameManager.IsMultiplayer)
			{
				faction = GameController.GameManager.PlayerOwner.matFaction.faction;
			}
			else if (GameController.GameManager.IsHotSeat)
			{
				List<Player> players = GameController.GameManager.GetPlayers();
				Player player = GameController.GameManager.PlayerCurrent;
				while (!player.IsHuman)
				{
					player = players[(GameController.GameManager.GetPlayerLocalId(player) + 1) % players.Count];
				}
				faction = player.matFaction.faction;
			}
			this.SetupFactionBackground(faction);
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x0003FDF7 File Offset: 0x0003DFF7
		public void OnToggleClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.tab_faction_toggle);
			base.gameObject.SetActive(true);
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x000DEE04 File Offset: 0x000DD004
		private void OnEnable()
		{
			this.UpdateTexts();
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.tab_faction, Contexts.ingame);
			if (MatAndFactionSelection.IsDLCFaction(GameController.GameManager.PlayerMaster.matFaction.faction))
			{
				this.ShowDLCTokens();
				return;
			}
			this.flagsIcons.gameObject.SetActive(false);
			this.trapsIcons.gameObject.SetActive(false);
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000DEE68 File Offset: 0x000DD068
		private void ShowDLCTokens()
		{
			if (GameController.GameManager.PlayerCurrent.matFaction.faction.Equals(Faction.Togawa))
			{
				this.flagsIcons.gameObject.SetActive(false);
				this.trapsIcons.gameObject.SetActive(true);
				using (List<FactionAbilityToken>.Enumerator enumerator = GameController.GameManager.PlayerCurrent.matFaction.FactionTokens.GetPlacedTokens().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FactionAbilityToken factionAbilityToken = enumerator.Current;
						TrapToken trapToken = (TrapToken)factionAbilityToken;
						this.DisableTrapButton(trapToken.Penalty);
					}
					return;
				}
			}
			if (GameController.GameManager.PlayerCurrent.matFaction.faction.Equals(Faction.Albion))
			{
				this.trapsIcons.gameObject.SetActive(false);
				this.flagsIcons.gameObject.SetActive(true);
				int placedTokensCount = GameController.GameManager.PlayerCurrent.matFaction.FactionTokens.GetPlacedTokensCount();
				int num = 3;
				for (int i = placedTokensCount; i > 0; i--)
				{
					this.flagImage[num].color = new Color(this.coinTrapImage.color.r, this.coinTrapImage.color.g, this.coinTrapImage.color.b, 0.3f);
					num--;
				}
			}
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x0003FE0C File Offset: 0x0003E00C
		private void UpdateFactionName(Faction factionToSetup)
		{
			if (base.gameObject.activeInHierarchy)
			{
				this.panelTitle.text = ScriptLocalization.Get("FactionMat/" + factionToSetup.ToString());
			}
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x0003FE42 File Offset: 0x0003E042
		private void OnDestroy()
		{
			OptionsManager.OnLanguageChanged -= this.UpdateTexts;
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x000DEFE4 File Offset: 0x000DD1E4
		private void SetupFactionBackground(Faction factionToSetup)
		{
			switch (factionToSetup)
			{
			case Faction.Polania:
				this.backgroundImage.sprite = this.factionBackground[0];
				return;
			case Faction.Albion:
				this.backgroundImage.sprite = this.factionBackground[5];
				return;
			case Faction.Nordic:
				this.backgroundImage.sprite = this.factionBackground[1];
				return;
			case Faction.Rusviet:
				this.backgroundImage.sprite = this.factionBackground[2];
				return;
			case Faction.Togawa:
				this.backgroundImage.sprite = this.factionBackground[6];
				return;
			case Faction.Crimea:
				this.backgroundImage.sprite = this.factionBackground[3];
				return;
			case Faction.Saxony:
				this.backgroundImage.sprite = this.factionBackground[4];
				return;
			default:
				return;
			}
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000DF0A0 File Offset: 0x000DD2A0
		public void SetupFactionAbilityInfo(GameController.FactionInfo factionInfo, FactionBasicInfo factionBasicInfo, Player player)
		{
			this.player = player;
			this.factionAbilityTitle.text = ScriptLocalization.Get("FactionMat/" + factionInfo.faction.ToString() + "FactionAbilityName");
			if (factionInfo.faction == Faction.Polania && GameController.GameManager.players.Count > 5)
			{
				this.factionAbilityDescription.text = ScriptLocalization.Get("FactionMat/PolaniaFactionAbilityDescriptionA").Replace("|", Environment.NewLine);
			}
			else
			{
				this.factionAbilityDescription.text = ScriptLocalization.Get("FactionMat/" + factionInfo.faction.ToString() + "FactionAbilityDescription").Replace("|", Environment.NewLine);
			}
			this.SetupFactionBackground(player.matFaction.faction);
			this.UpdateFactionName(player.matFaction.faction);
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000DF188 File Offset: 0x000DD388
		public void SetupMechAbilities(GameController.FactionInfo factionInfo, Player player)
		{
			for (int i = 0; i < 4; i++)
			{
				FactionBasicInfo factionBasicInfo = GameController.GameManager.factionBasicInfo[player.matFaction.faction];
				bool flag = factionInfo.faction == Faction.Crimea && GameController.GameManager.players.Count > 5 && i == 1;
				this.mechAbilityIcons[i].sprite = (flag ? this.CrimeaWayfareA : factionInfo.mechAbilityIcons[i]);
				this.mechAbilityTitles[i].text = factionBasicInfo.mechAbilityTitles[i];
				this.mechAbilityDescriptions[i].text = (flag ? ScriptLocalization.Get("FactionMat/CrimeaMechAbilityDescription2A").Replace("|", Environment.NewLine) : factionBasicInfo.mechAbilityDescriptions[i].Replace("|", Environment.NewLine));
			}
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000DF25C File Offset: 0x000DD45C
		public void UpdateRecruitBonusButtons()
		{
			this.recruitBackground[0].color = (this.player.matFaction.OneTimeBonusUsed(GainType.Coin) ? (this.recruitBackground[0].color = this.backgroundActive) : (this.recruitBackground[0].color = this.backgroundInActive));
			this.oneTimeBonusIcon[0].color = (this.oneTimeBonusText[0].color = (this.player.matFaction.OneTimeBonusUsed(GainType.Coin) ? this.colActive : this.colInactive));
			this.oneTimeBonusIcon[0].material = (this.player.matFaction.OneTimeBonusUsed(GainType.Coin) ? null : this.sepiaUI);
			this.recruitBackground[1].color = (this.player.matFaction.OneTimeBonusUsed(GainType.Popularity) ? (this.recruitBackground[1].color = this.backgroundActive) : (this.recruitBackground[1].color = this.backgroundInActive));
			this.oneTimeBonusIcon[1].color = (this.oneTimeBonusText[1].color = (this.player.matFaction.OneTimeBonusUsed(GainType.Popularity) ? this.colActive : this.colInactive));
			this.oneTimeBonusIcon[1].material = (this.player.matFaction.OneTimeBonusUsed(GainType.Popularity) ? null : this.sepiaUI);
			this.recruitBackground[2].color = (this.player.matFaction.OneTimeBonusUsed(GainType.Power) ? (this.recruitBackground[2].color = this.backgroundActive) : (this.recruitBackground[2].color = this.backgroundInActive));
			this.oneTimeBonusIcon[2].color = (this.oneTimeBonusText[2].color = (this.player.matFaction.OneTimeBonusUsed(GainType.Power) ? this.colActive : this.colInactive));
			this.oneTimeBonusIcon[2].material = (this.player.matFaction.OneTimeBonusUsed(GainType.Power) ? null : this.sepiaUI);
			this.recruitBackground[3].color = (this.player.matFaction.OneTimeBonusUsed(GainType.CombatCard) ? (this.recruitBackground[3].color = this.backgroundActive) : (this.recruitBackground[3].color = this.backgroundInActive));
			this.oneTimeBonusIcon[3].color = (this.oneTimeBonusText[3].color = (this.player.matFaction.OneTimeBonusUsed(GainType.CombatCard) ? this.colActive : this.colInactive));
			this.oneTimeBonusIcon[3].material = (this.player.matFaction.OneTimeBonusUsed(GainType.CombatCard) ? null : this.sepiaUI);
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x000DF540 File Offset: 0x000DD740
		public void UpdateMechButtons()
		{
			for (int i = 0; i < 4; i++)
			{
				if (!this.player.matFaction.SkillUnlocked[i])
				{
					this.mechIcon[i].material = this.sepiaUI;
					this.mechTextTitle[i].color = (this.mechTextDescription[i].color = this.colInactive);
					this.mechBackgroundShrink[i].color = this.backgroundInActive;
				}
				else
				{
					this.mechIcon[i].material = null;
					this.mechTextTitle[i].color = (this.mechTextDescription[i].color = this.colActive);
					this.mechBackgroundShrink[i].color = this.backgroundActive;
				}
			}
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x0003FE55 File Offset: 0x0003E055
		private void UpdateTexts()
		{
			this.UpdateFactionNameText();
			this.UpdateFactionAbilityTexts();
			this.UpdateMechTexts();
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x0003FE69 File Offset: 0x0003E069
		private void UpdateFactionNameText()
		{
			this.panelTitle.text = ScriptLocalization.Get("FactionMat/" + this.player.matFaction.faction.ToString());
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x000DF604 File Offset: 0x000DD804
		private void UpdateFactionAbilityTexts()
		{
			this.factionAbilityTitle.text = ScriptLocalization.Get("FactionMat/" + this.player.matFaction.faction.ToString() + "FactionAbilityName");
			if (this.player.matFaction.faction == Faction.Polania && GameController.GameManager.players.Count > 5)
			{
				this.factionAbilityDescription.text = ScriptLocalization.Get("FactionMat/PolaniaFactionAbilityDescriptionA").Replace("|", Environment.NewLine);
			}
			else
			{
				this.factionAbilityDescription.text = ScriptLocalization.Get("FactionMat/" + this.player.matFaction.faction.ToString() + "FactionAbilityDescription").Replace("|", Environment.NewLine);
			}
			this.SetupFactionBackground(this.player.matFaction.faction);
			if (base.gameObject.activeInHierarchy)
			{
				this.UpdateFactionName(this.player.matFaction.faction);
			}
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000DF718 File Offset: 0x000DD918
		private void UpdateMechTexts()
		{
			for (int i = 0; i < 4; i++)
			{
				FactionBasicInfo factionBasicInfo = GameController.GameManager.factionBasicInfo[this.player.matFaction.faction];
				bool flag = factionBasicInfo.faction == Faction.Crimea && GameController.GameManager.players.Count > 5 && i == 1;
				this.mechAbilityTitles[i].text = factionBasicInfo.mechAbilityTitles[i];
				this.mechAbilityDescriptions[i].text = (flag ? ScriptLocalization.Get("FactionMat/CrimeaMechAbilityDescription2A").Replace("|", Environment.NewLine) : factionBasicInfo.mechAbilityDescriptions[i].Replace("|", Environment.NewLine));
			}
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000DF7D4 File Offset: 0x000DD9D4
		private void DisableTrapButton(PayType penalty)
		{
			switch (penalty)
			{
			case PayType.Coin:
				this.coinTrapImage.color = new Color(this.coinTrapImage.color.r, this.coinTrapImage.color.g, this.coinTrapImage.color.b, 0.3f);
				return;
			case PayType.Popularity:
				this.popularityTrapImage.color = new Color(this.popularityTrapImage.color.r, this.popularityTrapImage.color.g, this.popularityTrapImage.color.b, 0.3f);
				return;
			case PayType.Power:
				this.powerTrapImage.color = new Color(this.powerTrapImage.color.r, this.powerTrapImage.color.g, this.powerTrapImage.color.b, 0.3f);
				return;
			case PayType.CombatCard:
				this.combatCardTrapImage.color = new Color(this.combatCardTrapImage.color.r, this.combatCardTrapImage.color.g, this.combatCardTrapImage.color.b, 0.3f);
				return;
			default:
				return;
			}
		}

		// Token: 0x04001A4A RID: 6730
		[SerializeField]
		private Image backgroundImage;

		// Token: 0x04001A4B RID: 6731
		[SerializeField]
		private TextMeshProUGUI factionAbilityDescription;

		// Token: 0x04001A4C RID: 6732
		[SerializeField]
		private TextMeshProUGUI factionAbilityTitle;

		// Token: 0x04001A4D RID: 6733
		[SerializeField]
		private TextMeshProUGUI[] mechAbilityTitles;

		// Token: 0x04001A4E RID: 6734
		[SerializeField]
		private TextMeshProUGUI[] mechAbilityDescriptions;

		// Token: 0x04001A4F RID: 6735
		[SerializeField]
		private Image[] mechAbilityIcons;

		// Token: 0x04001A50 RID: 6736
		[SerializeField]
		private Sprite CrimeaWayfareA;

		// Token: 0x04001A51 RID: 6737
		[SerializeField]
		private Sprite[] factionBackground = new Sprite[7];

		// Token: 0x04001A52 RID: 6738
		[SerializeField]
		private Image[] oneTimeBonusIcon;

		// Token: 0x04001A53 RID: 6739
		[SerializeField]
		private Text[] oneTimeBonusText;

		// Token: 0x04001A54 RID: 6740
		[SerializeField]
		private Image[] recruitBackground;

		// Token: 0x04001A55 RID: 6741
		[SerializeField]
		private Material sepiaUI;

		// Token: 0x04001A56 RID: 6742
		[SerializeField]
		private Sprite recruitButtonSpriteDefault;

		// Token: 0x04001A57 RID: 6743
		[SerializeField]
		private Sprite recruitButtonSpriteTaken;

		// Token: 0x04001A58 RID: 6744
		[SerializeField]
		private Sprite recruitButtonSpriteActive;

		// Token: 0x04001A59 RID: 6745
		[SerializeField]
		private Image[] mechIcon;

		// Token: 0x04001A5A RID: 6746
		[SerializeField]
		private TextMeshProUGUI[] mechTextTitle;

		// Token: 0x04001A5B RID: 6747
		[SerializeField]
		private TextMeshProUGUI[] mechTextDescription;

		// Token: 0x04001A5C RID: 6748
		[SerializeField]
		private Image[] mechBackgroundShrink;

		// Token: 0x04001A5D RID: 6749
		[SerializeField]
		private TextMeshProUGUI panelTitle;

		// Token: 0x04001A5E RID: 6750
		[SerializeField]
		private Image popularityTrapImage;

		// Token: 0x04001A5F RID: 6751
		[SerializeField]
		private Image combatCardTrapImage;

		// Token: 0x04001A60 RID: 6752
		[SerializeField]
		private Image powerTrapImage;

		// Token: 0x04001A61 RID: 6753
		[SerializeField]
		private Image coinTrapImage;

		// Token: 0x04001A62 RID: 6754
		[SerializeField]
		private Image[] flagImage;

		// Token: 0x04001A63 RID: 6755
		[SerializeField]
		private Transform trapsIcons;

		// Token: 0x04001A64 RID: 6756
		[SerializeField]
		private Transform flagsIcons;

		// Token: 0x04001A65 RID: 6757
		private Color colInactive = new Color(1f, 1f, 1f, 0.2f);

		// Token: 0x04001A66 RID: 6758
		private Color colActive = new Color(1f, 1f, 1f, 0.75f);

		// Token: 0x04001A67 RID: 6759
		private Color backgroundActive = new Color(0f, 0f, 0f, 1f);

		// Token: 0x04001A68 RID: 6760
		private Color backgroundInActive = new Color(0f, 0f, 0f, 0.6f);

		// Token: 0x04001A69 RID: 6761
		private Player player;
	}
}

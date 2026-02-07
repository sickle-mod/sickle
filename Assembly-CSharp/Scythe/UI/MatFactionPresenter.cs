using System;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200047D RID: 1149
	public class MatFactionPresenter : MonoBehaviour
	{
		// Token: 0x06002467 RID: 9319 RVA: 0x0003F374 File Offset: 0x0003D574
		private void Awake()
		{
			OptionsManager.OnLanguageChanged += this.UpdateMechTexts;
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x0003F387 File Offset: 0x0003D587
		private void OnDestroy()
		{
			OptionsManager.OnLanguageChanged -= this.UpdateMechTexts;
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000D75F4 File Offset: 0x000D57F4
		public void UpdateMat(Player player, GameController.FactionInfo factionInfo, bool updateInteractability = true)
		{
			this.player = player;
			for (int i = 0; i < 4; i++)
			{
				FactionBasicInfo factionBasicInfo = GameController.GameManager.factionBasicInfo[player.matFaction.faction];
				bool flag = factionInfo.faction == Faction.Crimea && GameController.GameManager.players.Count > 5 && i == 1;
				this.mechAbilityIconsMobileShort[i].sprite = (this.mechAbilityIcons[i].sprite = (flag ? this.CrimeaWayfareA : factionInfo.mechAbilityIcons[i]));
				this.mechAbilityTitlesMobileShort[i].text = (this.mechAbilityTitles[i].text = factionBasicInfo.mechAbilityTitles[i]);
				this.mechAbilityDescriptions[i].text = (flag ? ScriptLocalization.Get("FactionMat/CrimeaMechAbilityDescription2A").Replace("|", Environment.NewLine) : factionBasicInfo.mechAbilityDescriptions[i].Replace("|", Environment.NewLine));
				if (!player.matFaction.SkillUnlocked[i])
				{
					this.mechAbilityIconsMobileShort[i].material = (this.mechAbilityIcons[i].material = this.grayUI);
					this.mechAbilityTitlesMobileShort[i].color = (this.mechAbilityTitles[i].color = (this.mechAbilityDescriptions[i].color = this.MechTitleColorInactive));
					this.mechButtonMobileShort[i].image.sprite = (this.mechButton[i].image.sprite = this.MechTileInactive);
				}
				else
				{
					this.mechAbilityIconsMobileShort[i].material = (this.mechAbilityIcons[i].material = null);
					this.mechAbilityTitlesMobileShort[i].color = (this.mechAbilityTitles[i].color = this.MechTitleColorActive);
					this.mechAbilityDescriptions[i].color = this.MechDescriptionColorActive;
					this.mechButtonMobileShort[i].image.sprite = (this.mechButton[i].image.sprite = this.MechTileActive);
				}
				if (updateInteractability)
				{
					if (PlatformManager.IsStandalone)
					{
						this.recruitButton[i].interactable = false;
					}
					this.mechButtonMobileShort[i].enabled = (this.mechButton[i].enabled = true);
					this.mechButtonMobileShort[i].interactable = (this.mechButton[i].interactable = false);
				}
			}
			Color color = new Color(1f, 1f, 1f, 0.2f);
			Color color2 = new Color(1f, 1f, 1f, 0.75f);
			if (PlatformManager.IsStandalone)
			{
				this.recruitButton[0].image.sprite = (player.matFaction.OneTimeBonusUsed(GainType.Coin) ? this.recruitButtonSpriteTaken : this.recruitButtonSpriteDefault);
				this.oneTimeBonusIcon[0].color = (this.oneTimeBonus2[0].color = (player.matFaction.OneTimeBonusUsed(GainType.Coin) ? color : color2));
				this.oneTimeBonusIcon[0].material = (player.matFaction.OneTimeBonusUsed(GainType.Coin) ? this.sepiaUI : null);
				this.recruitButton[1].image.sprite = (player.matFaction.OneTimeBonusUsed(GainType.Popularity) ? this.recruitButtonSpriteTaken : this.recruitButtonSpriteDefault);
				this.oneTimeBonusIcon[1].color = (this.oneTimeBonus2[1].color = (player.matFaction.OneTimeBonusUsed(GainType.Popularity) ? color : color2));
				this.oneTimeBonusIcon[1].material = (player.matFaction.OneTimeBonusUsed(GainType.Popularity) ? this.sepiaUI : null);
				this.recruitButton[2].image.sprite = (player.matFaction.OneTimeBonusUsed(GainType.Power) ? this.recruitButtonSpriteTaken : this.recruitButtonSpriteDefault);
				this.oneTimeBonusIcon[2].color = (this.oneTimeBonus2[2].color = (player.matFaction.OneTimeBonusUsed(GainType.Power) ? color : color2));
				this.oneTimeBonusIcon[2].material = (player.matFaction.OneTimeBonusUsed(GainType.Power) ? this.sepiaUI : null);
				this.recruitButton[3].image.sprite = (player.matFaction.OneTimeBonusUsed(GainType.CombatCard) ? this.recruitButtonSpriteTaken : this.recruitButtonSpriteDefault);
				this.oneTimeBonusIcon[3].color = (this.oneTimeBonus2[3].color = (player.matFaction.OneTimeBonusUsed(GainType.CombatCard) ? color : color2));
				this.oneTimeBonusIcon[3].material = (player.matFaction.OneTimeBonusUsed(GainType.CombatCard) ? this.sepiaUI : null);
			}
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x000D7AC4 File Offset: 0x000D5CC4
		private void UpdateMechTexts()
		{
			for (int i = 0; i < 4; i++)
			{
				FactionBasicInfo factionBasicInfo = GameController.GameManager.factionBasicInfo[this.player.matFaction.faction];
				bool flag = factionBasicInfo.faction == Faction.Crimea && GameController.GameManager.players.Count > 5 && i == 1;
				this.mechAbilityTitlesMobileShort[i].text = (this.mechAbilityTitles[i].text = factionBasicInfo.mechAbilityTitles[i]);
				this.mechAbilityDescriptions[i].text = (flag ? ScriptLocalization.Get("FactionMat/CrimeaMechAbilityDescription2A").Replace("|", Environment.NewLine) : factionBasicInfo.mechAbilityDescriptions[i].Replace("|", Environment.NewLine));
			}
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x000D7B90 File Offset: 0x000D5D90
		public void OnMechSelected(int index)
		{
			this.deployPresenter.OnMechSelected(index);
			Color color;
			for (int i = 0; i < 4; i++)
			{
				if (!GameController.GameManager.PlayerCurrent.matFaction.SkillUnlocked[i])
				{
					Graphic graphic = this.mechAbilityIconsMobileShort[i];
					Graphic graphic2 = this.mechAbilityIcons[i];
					color = new Color(0.4375f, 0.4375f, 0.4375f, 1f);
					graphic2.color = color;
					graphic.color = color;
					this.mechAbilityIconsMobileShort[i].material = (this.mechAbilityIcons[i].material = this.sepiaUI);
				}
				if (!PlatformManager.IsStandalone)
				{
					this.mechButton[i].gameObject.SetActive(false);
					this.mechButtonMobileShort[i].gameObject.SetActive(true);
				}
			}
			this.mechAbilityIconsMobileShort[index].material = (this.mechAbilityIcons[index].material = null);
			Graphic graphic3 = this.mechAbilityIconsMobileShort[index];
			Graphic graphic4 = this.mechAbilityIcons[index];
			color = new Color(1f, 1f, 1f, 0.8f);
			graphic4.color = color;
			graphic3.color = color;
			if (!PlatformManager.IsStandalone)
			{
				this.mechButton[index].gameObject.SetActive(true);
				this.mechButtonMobileShort[index].gameObject.SetActive(false);
			}
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x000D7CE0 File Offset: 0x000D5EE0
		public void OnRecruitSelected(int index)
		{
			if (this.enlistPresenter.enlistStep == EnlistPresenter.EnlistStep.selectBonus)
			{
				if (PlatformManager.IsStandalone)
				{
					if (this.selectedBonus != -1)
					{
						this.recruitButton[this.selectedBonus].image.color = Color.white;
					}
					this.recruitButton[index].image.color = new Color(0.25f, 1f, 0f);
					this.oneTimeBonusAccept.interactable = true;
					this.oneTimeBonusAccept.image.sprite = this.recruitButtonSpriteActive;
					this.oneTimeBonusAccept.GetComponent<Animator>().enabled = true;
				}
				this.enlistPresenter.OnBonusHighlighted(index);
				this.selectedBonus = index;
				switch (index)
				{
				case 0:
					WorldSFXManager.PlaySound(SoundEnum.RecruitBonusEarnCashClick, AudioSourceType.Buttons);
					return;
				case 1:
					WorldSFXManager.PlaySound(SoundEnum.RecruitBonusPopularityClick, AudioSourceType.Buttons);
					return;
				case 2:
					WorldSFXManager.PlaySound(SoundEnum.RecruitBonusGainPowerClick, AudioSourceType.Buttons);
					return;
				case 3:
					WorldSFXManager.PlaySound(SoundEnum.RecruitBonusBolsterAmmoClick, AudioSourceType.Buttons);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x000D7DD0 File Offset: 0x000D5FD0
		public void OnAcceptBonusClicked()
		{
			if (this.enlistPresenter.enlistStep == EnlistPresenter.EnlistStep.selectBonus)
			{
				if (PlatformManager.IsStandalone)
				{
					this.oneTimeBonusAccept.interactable = false;
					this.oneTimeBonusAccept.image.sprite = this.recruitButtonSpriteDefault;
					this.oneTimeBonusAccept.GetComponent<Animator>().enabled = false;
					this.recruitButton[this.selectedBonus].image.color = Color.white;
				}
				this.enlistPresenter.OnBonusSelected(this.selectedBonus);
				this.selectedBonus = -1;
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_accept_button);
			}
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x000D7E60 File Offset: 0x000D6060
		public void OneTimeBonusShow(int id, bool active)
		{
			if (PlatformManager.IsStandalone)
			{
				this.recruitButton[id].interactable = active;
				this.oneTimeBonusIcon[id].material = (active ? null : this.sepiaUI);
				this.oneTimeBonus2[id].color = (active ? Color.white : new Color(1f, 1f, 1f, 0.25f));
				this.recruitButton[id].image.sprite = (active ? this.recruitButtonSpriteActive : this.recruitButtonSpriteTaken);
				this.recruitButton[id].image.color = ((id == this.selectedBonus) ? new Color(0.25f, 1f, 0f) : Color.white);
				this.recruitButton[id].GetComponent<Animator>().enabled = active;
			}
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x0003F39A File Offset: 0x0003D59A
		public void ShowBuildingHint(BuildingType buildingType, int sectionId, bool buildAction = false)
		{
			this.ClearHintStories();
			if (buildAction)
			{
				this.CheckBuildingTypeShowHint(sectionId);
				return;
			}
			if (!this.player.matPlayer.GetPlayerMatSection(sectionId).ActionTop.Structure.IsOnMap())
			{
				this.CheckBuildingTypeShowHint(sectionId);
			}
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x000D7F3C File Offset: 0x000D613C
		public void BuildingTypeHideHints()
		{
			for (int i = 0; i < 4; i++)
			{
				this.hintBuildStructure[i].SetActive(false);
			}
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x000D7F64 File Offset: 0x000D6164
		private void CheckBuildingTypeShowHint(int sectionId)
		{
			this.BuildingTypeHideHints();
			if (this.player.matPlayer.GetPlayerMatSection(sectionId).ActionTop.Structure.buildingType == BuildingType.Armory)
			{
				this.hintBuildStructure[0].SetActive(true);
			}
			if (this.player.matPlayer.GetPlayerMatSection(sectionId).ActionTop.Structure.buildingType == BuildingType.Mill)
			{
				this.hintBuildStructure[1].SetActive(true);
			}
			if (this.player.matPlayer.GetPlayerMatSection(sectionId).ActionTop.Structure.buildingType == BuildingType.Mine)
			{
				this.hintBuildStructure[2].SetActive(true);
			}
			if (this.player.matPlayer.GetPlayerMatSection(sectionId).ActionTop.Structure.buildingType == BuildingType.Monument)
			{
				this.hintBuildStructure[3].SetActive(true);
			}
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x0003F3D6 File Offset: 0x0003D5D6
		public void ClearHintStories()
		{
			GameController.Instance.matPlayer.DisableHints();
		}

		// Token: 0x04001969 RID: 6505
		public Image[] mechAbilityIcons;

		// Token: 0x0400196A RID: 6506
		public TextMeshProUGUI[] mechAbilityTitles;

		// Token: 0x0400196B RID: 6507
		public TextMeshProUGUI[] mechAbilityDescriptions;

		// Token: 0x0400196C RID: 6508
		public Image[] mechGlow;

		// Token: 0x0400196D RID: 6509
		public Button[] mechButton;

		// Token: 0x0400196E RID: 6510
		public Sprite CrimeaWayfareA;

		// Token: 0x0400196F RID: 6511
		public Image[] mechAbilityIconsMobileShort;

		// Token: 0x04001970 RID: 6512
		public TextMeshProUGUI[] mechAbilityTitlesMobileShort;

		// Token: 0x04001971 RID: 6513
		public Image[] mechGlowMobileShort;

		// Token: 0x04001972 RID: 6514
		public Button[] mechButtonMobileShort;

		// Token: 0x04001973 RID: 6515
		public Image[] oneTimeBonusIcon;

		// Token: 0x04001974 RID: 6516
		public Text[] oneTimeBonus2;

		// Token: 0x04001975 RID: 6517
		public Button[] recruitButton;

		// Token: 0x04001976 RID: 6518
		public Button oneTimeBonusAccept;

		// Token: 0x04001977 RID: 6519
		public Sprite recruitButtonSpriteDefault;

		// Token: 0x04001978 RID: 6520
		public Sprite recruitButtonSpriteTaken;

		// Token: 0x04001979 RID: 6521
		public Sprite recruitButtonSpriteActive;

		// Token: 0x0400197A RID: 6522
		public GameObject hintWindow;

		// Token: 0x0400197B RID: 6523
		public GameObject[] hintBuildStructure;

		// Token: 0x0400197C RID: 6524
		public Material sepiaUI;

		// Token: 0x0400197D RID: 6525
		public Material grayUI;

		// Token: 0x0400197E RID: 6526
		public CombatCardsPanelPresenter combatCardsPresenter;

		// Token: 0x0400197F RID: 6527
		public DeployPresenter deployPresenter;

		// Token: 0x04001980 RID: 6528
		public EnlistPresenter enlistPresenter;

		// Token: 0x04001981 RID: 6529
		public Sprite MechTileActive;

		// Token: 0x04001982 RID: 6530
		public Sprite MechTileInactive;

		// Token: 0x04001983 RID: 6531
		public Color MechTitleColorActive = new Color(1f, 0.875f, 0f, 0.875f);

		// Token: 0x04001984 RID: 6532
		public Color MechTitleColorInactive = new Color(0.5f, 0.5f, 0.5f, 0.75f);

		// Token: 0x04001985 RID: 6533
		public Color MechDescriptionColorActive = new Color(1f, 1f, 1f, 0.875f);

		// Token: 0x04001986 RID: 6534
		private Player player;

		// Token: 0x04001987 RID: 6535
		private int selectedBonus = -1;
	}
}

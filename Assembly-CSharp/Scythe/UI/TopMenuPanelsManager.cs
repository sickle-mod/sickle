using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004A1 RID: 1185
	public class TopMenuPanelsManager : SingletonMono<TopMenuPanelsManager>
	{
		// Token: 0x060025A0 RID: 9632 RVA: 0x0003FF23 File Offset: 0x0003E123
		private void OnEnable()
		{
			this.TrunOffAllPanels();
			this.panelsObjects[this.lastScreenIndex].SetActive(true);
			this.UpdatePanelTitle(this.lastScreenIndex);
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x0003FF4A File Offset: 0x0003E14A
		public void Show()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_menu_with_tabs_button);
			this.TopMenuUI.SetActive(true);
			this.UpdatePanelTitle(this.lastScreenIndex);
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x0003FF6B File Offset: 0x0003E16B
		public TopMenuObjectivesPresenter GetTopMenuObjectivesPresenter()
		{
			return this.topMenuObjectivesPresenter;
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x0003FF73 File Offset: 0x0003E173
		public Sprite GetEmptyStar()
		{
			return this.emptyStar;
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x000E0034 File Offset: 0x000DE234
		public void OnToggleChanged(int id)
		{
			if (!this.topMenuTabToggles[id].interactable)
			{
				return;
			}
			WorldSFXManager.PlaySound(SoundEnum.GuiUpperBeltClick, AudioSourceType.Buttons);
			this.lastScreenIndex = id;
			for (int i = 0; i < 7; i++)
			{
				if (id != i)
				{
					this.panelsObjects[i].SetActive(false);
				}
				else
				{
					this.panelsObjects[i].SetActive(true);
				}
			}
			this.UpdatePanelTitle(id);
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x0003FF7B File Offset: 0x0003E17B
		public void ShowPanel(int id)
		{
			this.TopMenuUI.SetActive(true);
			this.HighlightToggle(id);
			this.OnToggleChanged(id);
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x0003FF97 File Offset: 0x0003E197
		public void SwitchPanel(int id)
		{
			this.topMenuTabToggles[id].isOn = true;
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x0003FFAB File Offset: 0x0003E1AB
		public void Hide()
		{
			this.TopMenuUI.SetActive(false);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x000E009C File Offset: 0x000DE29C
		private void UpdatePanelTitle(int id)
		{
			switch (id)
			{
			case 0:
				this.panelTitle.text = ScriptLocalization.Get("Common/Stars");
				return;
			case 1:
				this.panelTitle.text = ScriptLocalization.Get("PlayerMat/Popularity") + " & " + ScriptLocalization.Get("Statistics/Power");
				return;
			case 2:
				this.panelTitle.text = string.Concat(new string[]
				{
					ScriptLocalization.Get("Common/Stats"),
					" - ",
					ScriptLocalization.Get("GameScene/Round"),
					" ",
					(GameController.GameManager.TurnCount + 1).ToString()
				});
				return;
			case 3:
				this.panelTitle.text = string.Concat(new string[]
				{
					ScriptLocalization.Get("Common/Score"),
					" - ",
					ScriptLocalization.Get("GameScene/Round"),
					" ",
					(GameController.GameManager.TurnCount + 1).ToString()
				});
				return;
			case 4:
				this.panelTitle.text = ScriptLocalization.Get("Common/Structures");
				return;
			case 5:
				this.panelTitle.text = ScriptLocalization.Get("Common/Objectives");
				return;
			case 6:
				this.panelTitle.text = ScriptLocalization.Get("FactionMat/" + this.player.matFaction.faction.ToString());
				return;
			default:
				return;
			}
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x000E0220 File Offset: 0x000DE420
		private void TrunOffAllPanels()
		{
			for (int i = 0; i < 7; i++)
			{
				this.panelsObjects[i].SetActive(false);
			}
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x0003FFC6 File Offset: 0x0003E1C6
		private void HighlightToggle(int id)
		{
			if (this.topMenuTabToggles.Count > id)
			{
				this.topMenuTabToggles[id].isOn = true;
				this.topMenuTabToggles[id].OnSelect(new BaseEventData(EventSystem.current));
			}
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x00040003 File Offset: 0x0003E203
		public void StructuresBonusInit(StructureBonusCard structCard)
		{
			this.topMenuStructureBonus.Init(structCard);
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x000E0248 File Offset: 0x000DE448
		public void UpdateStats(Player player)
		{
			this.topMenuStarsPresenter.UpdatePlayersEntries();
			this.topMenuTracksPresenter.UpdateTracks();
			this.topMenuStatsPresenter.UpdateSingleEntry(player);
			if (player.IsHuman && player == GameController.GameManager.PlayerMaster)
			{
				this.player = player;
				this.factionLogoCheckmark.sprite = (this.factionLogo.sprite = GameController.factionInfo[player.matFaction.faction].logo);
			}
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x000E02C8 File Offset: 0x000DE4C8
		public void UpdatePlayerInfo(GameController.FactionInfo factionInfo, FactionBasicInfo factionBasicInfo, Player player, bool preview = false)
		{
			this.topMenuFactionPresenter.SetupFactionAbilityInfo(factionInfo, factionBasicInfo, player);
			this.topMenuFactionPresenter.SetupMechAbilities(factionInfo, player);
			this.topMenuFactionPresenter.UpdateRecruitBonusButtons();
			this.topMenuFactionPresenter.UpdateMechButtons();
			this.topMenuObjectivesPresenter.UpdatePlayerInfo(player.objectiveCards);
			this.topMenuStructureBonus.UpdateUnlockedBuildings(player);
			if (this.combatCardsPresenter != null)
			{
				GameController.Instance.matFaction.combatCardsPresenter.SetCards(player.combatCards, null);
			}
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x000E034C File Offset: 0x000DE54C
		public void UpdatePlayerInfo(GameController.FactionInfo factionInfo, FactionBasicInfo factionBasicInfo, List<ObjectiveCard> objectiveCards, List<CombatCard> combatCards, bool preview = false)
		{
			this.topMenuFactionPresenter.SetupFactionAbilityInfo(factionInfo, factionBasicInfo, this.player);
			this.topMenuFactionPresenter.SetupMechAbilities(factionInfo, this.player);
			this.topMenuFactionPresenter.UpdateRecruitBonusButtons();
			this.topMenuFactionPresenter.UpdateMechButtons();
			this.topMenuObjectivesPresenter.UpdatePlayerInfo(objectiveCards);
			this.topMenuStructureBonus.UpdateUnlockedBuildings(this.player);
			if (this.combatCardsPresenter != null)
			{
				GameController.Instance.matFaction.combatCardsPresenter.SetCards(combatCards, null);
			}
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x00040011 File Offset: 0x0003E211
		public void DisableObjectiveCardsTabInteraction()
		{
			this.topMenuTabToggles[5].interactable = false;
			this.topMenuTabToggles[5].image.sprite = this.objectiveCardsCover;
			AssetBundleManager.UnloadAssetBundle("graphic_objectivesmobileingame", false);
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x0004004D File Offset: 0x0003E24D
		public void DisableObjectiveCards()
		{
			this.Hide();
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x00040055 File Offset: 0x0003E255
		public void OnCloseButtonClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
			this.Hide();
			this.topMenuObjectivesPresenter.objectivesPreview.CheckObjectives();
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void UpdateInfoFromMat(Player player, GameController.FactionInfo factionInfo, bool updateInteractability = true)
		{
		}

		// Token: 0x04001A79 RID: 6777
		[SerializeField]
		private TopMenuStarsPresenter topMenuStarsPresenter;

		// Token: 0x04001A7A RID: 6778
		[SerializeField]
		private TopMenuTracksPresenter topMenuTracksPresenter;

		// Token: 0x04001A7B RID: 6779
		[SerializeField]
		private TopMenuStatsPresenter topMenuStatsPresenter;

		// Token: 0x04001A7C RID: 6780
		[SerializeField]
		private TopMenuScorePresenter topMenuScorePresenter;

		// Token: 0x04001A7D RID: 6781
		[SerializeField]
		private TopMenuStructureBonus topMenuStructureBonus;

		// Token: 0x04001A7E RID: 6782
		[SerializeField]
		private TopMenuObjectivesPresenter topMenuObjectivesPresenter;

		// Token: 0x04001A7F RID: 6783
		[SerializeField]
		private TopMenuFactionPresenter topMenuFactionPresenter;

		// Token: 0x04001A80 RID: 6784
		[SerializeField]
		private Image factionLogo;

		// Token: 0x04001A81 RID: 6785
		[SerializeField]
		private Image factionLogoCheckmark;

		// Token: 0x04001A82 RID: 6786
		[SerializeField]
		private GameObject[] panelsObjects = new GameObject[7];

		// Token: 0x04001A83 RID: 6787
		[SerializeField]
		private TextMeshProUGUI panelTitle;

		// Token: 0x04001A84 RID: 6788
		[SerializeField]
		private List<Toggle2> topMenuTabToggles;

		// Token: 0x04001A85 RID: 6789
		[SerializeField]
		private Sprite objectiveCardsCover;

		// Token: 0x04001A86 RID: 6790
		[SerializeField]
		private GameObject TopMenuUI;

		// Token: 0x04001A87 RID: 6791
		[SerializeField]
		private CombatCardsPanelPresenter combatCardsPresenter;

		// Token: 0x04001A88 RID: 6792
		[SerializeField]
		private Sprite emptyStar;

		// Token: 0x04001A89 RID: 6793
		private Player player;

		// Token: 0x04001A8A RID: 6794
		private int lastScreenIndex;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using HoneyFramework;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200047E RID: 1150
	public class EndGameStatsPresenter : MonoBehaviour
	{
		// Token: 0x0600244F RID: 9295 RVA: 0x0003F066 File Offset: 0x0003D266
		private void Awake()
		{
			this.allowForMinimize = false;
			this.allowForMaximize = false;
			this.animator = base.GetComponent<Animator>();
			LocalizationManager.OnLocalizeEvent += this.LocalizationManager_OnLocalizeEvent;
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x0003F093 File Offset: 0x0003D293
		private void OnDestroy()
		{
			LocalizationManager.OnLocalizeEvent -= this.LocalizationManager_OnLocalizeEvent;
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x0003F0A6 File Offset: 0x0003D2A6
		public List<PlayerEndGameStats> GetPlayerEndGameStats()
		{
			return this.stats;
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x0003F0AE File Offset: 0x0003D2AE
		private void LocalizationManager_OnLocalizeEvent()
		{
			this.UpdateStructurePointsDescription();
			this.UpdateLocalizedStats();
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000D7794 File Offset: 0x000D5994
		private void OnEnable()
		{
			this.roundCount.text = (GameController.GameManager.TurnCount + 1).ToString();
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000D77C0 File Offset: 0x000D59C0
		public void ShowStats(List<PlayerEndGameStats> playerStats)
		{
			this.ResetToDefault();
			this.Bounce();
			CameraControler.CameraMovementBlocked = true;
			this.stats = playerStats;
			this.DisableUnnecessaryEntries(this.stats.Count);
			this.allowForMinimize = true;
			this.DisableOverlapingObjects();
			this.UpdateLabels();
			this.UpdateEntries(true, GameController.GameManager.IsMultiplayer);
			if (!GameController.GameManager.IsCampaign)
			{
				this.okButton.onClick.RemoveAllListeners();
				if (!GameController.GameManager.IsMultiplayer)
				{
					this.okButton.onClick.AddListener(new UnityAction(this.ReturnToMenu));
				}
				else
				{
					this.okButton.onClick.AddListener(new UnityAction(this.ReturnToLobby));
				}
			}
			else if (this.exitButtonOutline != null)
			{
				this.exitButtonOutline.SetActive(true);
			}
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.score, Contexts.ingame);
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000D78A8 File Offset: 0x000D5AA8
		public void ShowPreviewStats(List<PlayerEndGameStats> playerStats)
		{
			this.ResetToDefault();
			CameraControler.CameraMovementBlocked = true;
			this.stats = playerStats;
			this.DisableUnnecessaryEntries(this.stats.Count);
			this.DisableOverlapingObjects();
			this.UpdateLabels();
			this.UpdateEntries(true, GameController.GameManager.IsMultiplayer);
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x000D78F8 File Offset: 0x000D5AF8
		public void ShowMultiplayerStats(List<PlayerEndGameStats> serverStats)
		{
			this.ResetToDefault();
			this.Bounce();
			CameraControler.CameraMovementBlocked = true;
			this.stats = serverStats;
			this.allowForMinimize = true;
			this.DisableOverlapingObjects();
			this.DisableUnnecessaryEntries(this.stats.Count);
			this.okButton.onClick.RemoveAllListeners();
			this.okButton.onClick.AddListener(new UnityAction(this.ReturnToLobby));
			this.UpdateLabels();
			this.UpdateEntries(true, true);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.score, Contexts.ingame);
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000D7984 File Offset: 0x000D5B84
		private void DisableOverlapingObjects()
		{
			GameController.Instance.turnInfoPanel.gameObject.SetActive(false);
			if (GameController.GameManager.GameFinished && GameController.Instance.combatPresenter != null)
			{
				GameController.Instance.combatPresenter.resultPanel.SetActive(false);
			}
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000D79DC File Offset: 0x000D5BDC
		private void DisableUnnecessaryEntries(int numberOfPlayers)
		{
			for (int i = numberOfPlayers; i < this.Entries.childCount; i++)
			{
				this.Entries.GetChild(i).gameObject.SetActive(false);
			}
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000D7A18 File Offset: 0x000D5C18
		public static string GetMultiplayerName(int faction)
		{
			return MultiplayerController.Instance.GetPlayersInGame().ToList<PlayerData>().Find((PlayerData player) => player.Faction == faction)
				.Name;
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000D7A58 File Offset: 0x000D5C58
		private void UpdateEntry(int entryId, PlayerEndGameStats playerStats, bool normal = true, bool multiplayer = false)
		{
			Transform child = this.Entries.GetChild(entryId);
			string popularityRichTextColor = EndGameStatsPresenter.GetPopularityRichTextColor(playerStats.player.Popularity);
			if (EndGameStatsPresenter.ShouldHighlightPlayer(playerStats.player))
			{
				child.GetChild(0).gameObject.SetActive(true);
			}
			child.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>()
				.text = (entryId + 1).ToString() + ".";
			this.UpdatePlayerNameAndFaction(child, playerStats);
			child.GetChild(2).GetComponent<TextMeshProUGUI>().text = (normal ? playerStats.player.Popularity.ToString() : this.GetTierStringPopularity(PopularityTrack.PopularityTier(playerStats.player.Popularity), popularityRichTextColor));
			int num = PopularityTrack.StarsMultiplier(playerStats.player.Popularity);
			child.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>()
				.text = (normal ? this.GetColorizedString(playerStats.starPoints.ToString(), popularityRichTextColor) : this.GetEquation(playerStats.starPoints, num, popularityRichTextColor));
			num = PopularityTrack.TerritoryMultiplier(playerStats.player.Popularity);
			child.GetChild(4).GetChild(3).GetComponent<TextMeshProUGUI>()
				.text = (normal ? this.GetColorizedString(playerStats.territoryPoints.ToString(), popularityRichTextColor) : this.GetEquation(playerStats.territoryPoints, num, popularityRichTextColor));
			num = PopularityTrack.ResourceMultiplier(playerStats.player.Popularity);
			child.GetChild(4).GetChild(5).GetComponent<TextMeshProUGUI>()
				.text = (normal ? this.GetColorizedString(playerStats.resourcePoints.ToString(), popularityRichTextColor) : this.GetEquation(playerStats.resourcePoints, num, popularityRichTextColor));
			this.UpdatePlayerStructurePoints(child, playerStats, normal);
			child.GetChild(4).GetChild(9).GetComponent<TextMeshProUGUI>()
				.text = playerStats.coinPoints.ToString();
			child.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>()
				.text = playerStats.totalPoints.ToString();
			if (multiplayer && GameController.GameManager.IsRanked && GameController.GameManager.GameFinished)
			{
				child.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>()
					.text = this.FormELOString(playerStats.oldRating, playerStats.rating);
				child.GetChild(7).gameObject.SetActive(true);
			}
			else
			{
				child.GetChild(7).gameObject.SetActive(false);
			}
			if (playerStats.player.matFaction.faction == Faction.Polania && GameController.GameManager.players.Count<Player>() >= 6)
			{
				child.GetChild(8).gameObject.SetActive(true);
				child.GetChild(8).GetChild(0).GetChild(0)
					.GetComponent<TextMeshProUGUI>()
					.text = "+" + new FindWinner(GameController.GameManager).CalculatePolaniaBonusPoints(playerStats.player, GameController.GameManager.players.Count<Player>()).ToString();
				return;
			}
			child.GetChild(8).gameObject.SetActive(false);
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x000D7D54 File Offset: 0x000D5F54
		private void UpdatePlayerNameAndFaction(Transform entry, PlayerEndGameStats playerStats)
		{
			string name = playerStats.player.Name;
			entry.GetChild(1).GetChild(1).GetChild(0)
				.GetComponent<Image>()
				.sprite = GameController.factionInfo[playerStats.player.matFaction.faction].logo;
			entry.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>()
				.text = string.Concat(new string[]
			{
				" ",
				ScriptLocalization.Get("FactionMat/" + playerStats.player.matFaction.faction.ToString()),
				Environment.NewLine,
				" <color=#999999>",
				name,
				"</color>"
			});
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x000D7E1C File Offset: 0x000D601C
		private void UpdatePlayerStructurePoints(Transform entry, PlayerEndGameStats playerStats, bool basic)
		{
			string structureRichTextColor = this.GetStructureRichTextColor(playerStats.structurePoints);
			int num = GameController.GameManager.StructureBonus.structureBonus.TierForValue(playerStats.structurePoints);
			entry.GetChild(4).GetChild(7).GetComponent<TextMeshProUGUI>()
				.text = (basic ? this.GetColorizedString(playerStats.structurePoints.ToString(), structureRichTextColor) : this.GetTierStringConstruction(num, structureRichTextColor));
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x000D7E88 File Offset: 0x000D6088
		private static bool ShouldHighlightPlayer(Player player)
		{
			List<Player> list = GameController.GameManager.GetPlayersWithoutAI().ToList<Player>();
			if (GameController.GameManager.IsMultiplayer)
			{
				return player == GameController.GameManager.PlayerOwner;
			}
			return list.Count == 1 && player == list[0];
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x000D7ED4 File Offset: 0x000D60D4
		private string GetStructureRichTextColor(int structureBonus)
		{
			StructureBonus structureBonus2 = GameController.GameManager.StructureBonus.structureBonus;
			int num = structureBonus2.NumberOfTiers();
			int num2 = structureBonus2.TierForValue(structureBonus);
			if (num2 == 1)
			{
				return "#F82424FF";
			}
			if (num2 == 2 && num == 4)
			{
				return "#F66110FF";
			}
			if ((num2 == 2 && num == 3) || (num2 == 3 && num == 4))
			{
				return "#F6C010FF";
			}
			if (num2 == num)
			{
				return "#0BFF2BFF";
			}
			return "\"white\"";
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x0003F0BC File Offset: 0x0003D2BC
		private static string GetPopularityRichTextColor(int popularity)
		{
			if (PopularityTrack.LowTier(popularity))
			{
				return "#F82424FF";
			}
			if (PopularityTrack.MediumTier(popularity))
			{
				return "#F6C010FF";
			}
			if (PopularityTrack.HighTier(popularity))
			{
				return "#0BFF2BFF";
			}
			return "\"white\"";
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x0003F0ED File Offset: 0x0003D2ED
		private string GetColorizedString(string data, string color)
		{
			return string.Format("<color={0}>{1}</color>", color, data);
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000D7F3C File Offset: 0x000D613C
		private string GetTierStringConstruction(int level, string color)
		{
			string text = ScriptLocalization.Get("Statistics/Tier");
			return string.Format("<color={0}>{1} {2}</color>", color, text, level);
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x000D7F68 File Offset: 0x000D6168
		private string GetTierStringPopularity(int level, string color)
		{
			string text = string.Empty;
			for (int i = 0; i < level; i++)
			{
				text += "I";
			}
			return string.Format("<color={0}>{1}</color>", color, text);
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x0003F0FB File Offset: 0x0003D2FB
		private string GetEquation(int score, int multiplier, string color)
		{
			return string.Format("({0} * {1})", score / multiplier, this.GetColorizedString(multiplier.ToString(), color));
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x0003F11D File Offset: 0x0003D31D
		private string FormELOString(int oldRating, int rating)
		{
			return string.Format("{0} ({1})", rating, rating - oldRating);
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x000D7FA0 File Offset: 0x000D61A0
		private void UpdateLabels()
		{
			if (GameController.GameManager.IsRanked && GameController.GameManager.GameFinished)
			{
				this.Lables.GetChild(5).gameObject.SetActive(true);
				this.Lables.GetChild(6).gameObject.SetActive(true);
				return;
			}
			this.Lables.GetChild(5).gameObject.SetActive(false);
			this.Lables.GetChild(6).gameObject.SetActive(false);
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x000D8024 File Offset: 0x000D6224
		public void UpdateEntries(bool normal = true, bool multiplayer = false)
		{
			for (int i = 0; i < this.stats.Count; i++)
			{
				this.UpdateEntry(i, this.stats[i], normal, multiplayer);
			}
			List<Player> list = GameController.GameManager.GetPlayersWithoutAI().ToList<Player>();
			Player player = null;
			if (multiplayer)
			{
				player = GameController.GameManager.PlayerOwner;
			}
			else if (list.Count > 0)
			{
				player = list[0];
			}
			if (GameController.GameManager.GameFinished && player != null)
			{
				AchievementManager.UpdateAchievementsEndgame(this.stats, player);
			}
			this.UpdateStructurePointsDescription();
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x0003F137 File Offset: 0x0003D337
		public void ShowDetailsToggled()
		{
			this.UpdateLabels();
			this.UpdateEntries(!this.detailsToggle.isOn, GameController.GameManager.IsMultiplayer);
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x000D80A4 File Offset: 0x000D62A4
		private void UpdateLocalizedStats()
		{
			for (int i = 0; i < this.stats.Count; i++)
			{
				Transform child = this.Entries.GetChild(i);
				this.UpdatePlayerNameAndFaction(child, this.stats[i]);
				this.UpdatePlayerStructurePoints(child, this.stats[i], !this.detailsToggle.isOn);
			}
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000D8108 File Offset: 0x000D6308
		private void UpdateStructurePointsDescription()
		{
			this.StructureBonusDescription.text = GameController.GetStructureBonusName(GameController.GameManager.StructureBonus.CardId) + Environment.NewLine + Environment.NewLine + GameController.GetStructureBonusDescription(GameController.GameManager.StructureBonus.CardId);
			this.StructureBonusValues.text = GameController.GameManager.StructureBonus.structureBonus.ToString();
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x0003F15D File Offset: 0x0003D35D
		public void Minimize()
		{
			if (!this.allowForMinimize)
			{
				return;
			}
			CameraControler.CameraMovementBlocked = false;
			if (this.animator != null)
			{
				this.animator.Play("WinnerPanelMinimize");
			}
			this.allowForMaximize = true;
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x0003F193 File Offset: 0x0003D393
		public void Maximize()
		{
			if (!this.allowForMaximize)
			{
				return;
			}
			CameraControler.CameraMovementBlocked = true;
			if (this.animator != null)
			{
				this.animator.Play("WinnerPanelMaximize");
			}
			this.allowForMaximize = false;
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x0003F1C9 File Offset: 0x0003D3C9
		public void PrepareInitial()
		{
			if (this.animator != null)
			{
				this.animator.Play("WinnerPanelInitial");
			}
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x0003F1E9 File Offset: 0x0003D3E9
		public void PrepareHoldForWinnerShowcase()
		{
			if (this.animator != null)
			{
				this.animator.Play("WinnerPanelHold");
			}
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x0003F209 File Offset: 0x0003D409
		private void ResetToDefault()
		{
			CameraControler.CameraMovementBlocked = true;
			if (this.animator != null)
			{
				this.animator.Play("WinnerPanelDefault");
			}
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x0003F22F File Offset: 0x0003D42F
		private void Bounce()
		{
			if (this.animator != null)
			{
				this.animator.Play("WinnerPanelBounce");
			}
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x000D8178 File Offset: 0x000D6378
		public void TurnOff()
		{
			base.gameObject.SetActive(false);
			if (PlatformManager.IsStandalone)
			{
				this.detailsToggle.isOn = false;
			}
			this.allowForMinimize = false;
			this.allowForMaximize = false;
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
			CameraControler.CameraMovementBlocked = false;
			if (!this.isPreview)
			{
				Transform transform = base.transform.Find("WinnerPanel");
				Transform transform2 = base.transform.Find("Blur");
				transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				transform.localScale = Vector3.zero;
				if (transform2 != null)
				{
					transform2.GetComponent<Image>().enabled = true;
				}
			}
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x0003F24F File Offset: 0x0003D44F
		public void ReturnToLobby()
		{
			WorldSFXManager.PlaySound(SoundEnum.ExitButton, AudioSourceType.Buttons);
			CameraControler.CameraMovementBlocked = false;
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_exit_button);
			GameController.Instance.ExitGame();
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x0003F24F File Offset: 0x0003D44F
		public void ReturnToMenu()
		{
			WorldSFXManager.PlaySound(SoundEnum.ExitButton, AudioSourceType.Buttons);
			CameraControler.CameraMovementBlocked = false;
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_exit_button);
			GameController.Instance.ExitGame();
		}

		// Token: 0x0400195D RID: 6493
		public bool isPreview;

		// Token: 0x0400195E RID: 6494
		public Transform Lables;

		// Token: 0x0400195F RID: 6495
		public Transform Entries;

		// Token: 0x04001960 RID: 6496
		public TextMeshProUGUI StructureBonusDescription;

		// Token: 0x04001961 RID: 6497
		public Text StructureBonusValues;

		// Token: 0x04001962 RID: 6498
		public Button okButton;

		// Token: 0x04001963 RID: 6499
		public GameObject exitButtonOutline;

		// Token: 0x04001964 RID: 6500
		public Toggle detailsToggle;

		// Token: 0x04001965 RID: 6501
		public TextMeshProUGUI roundCount;

		// Token: 0x04001966 RID: 6502
		public const int TEXT_HEIGHT = 20;

		// Token: 0x04001967 RID: 6503
		public const int ICON_HEIGHT = 30;

		// Token: 0x04001968 RID: 6504
		private List<PlayerEndGameStats> stats;

		// Token: 0x04001969 RID: 6505
		private Animator animator;

		// Token: 0x0400196A RID: 6506
		private bool allowForMinimize;

		// Token: 0x0400196B RID: 6507
		private bool allowForMaximize;
	}
}

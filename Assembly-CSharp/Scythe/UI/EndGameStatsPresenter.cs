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
	// Token: 0x02000479 RID: 1145
	public class EndGameStatsPresenter : MonoBehaviour
	{
		// Token: 0x06002431 RID: 9265 RVA: 0x0003F026 File Offset: 0x0003D226
		private void Awake()
		{
			this.allowForMinimize = false;
			this.allowForMaximize = false;
			this.animator = base.GetComponent<Animator>();
			LocalizationManager.OnLocalizeEvent += this.LocalizationManager_OnLocalizeEvent;
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x0003F053 File Offset: 0x0003D253
		private void OnDestroy()
		{
			LocalizationManager.OnLocalizeEvent -= this.LocalizationManager_OnLocalizeEvent;
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x0003F066 File Offset: 0x0003D266
		public List<PlayerEndGameStats> GetPlayerEndGameStats()
		{
			return this.stats;
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x0003F06E File Offset: 0x0003D26E
		private void LocalizationManager_OnLocalizeEvent()
		{
			this.UpdateStructurePointsDescription();
			this.UpdateLocalizedStats();
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000D6934 File Offset: 0x000D4B34
		private void OnEnable()
		{
			this.roundCount.text = (GameController.GameManager.TurnCount + 1).ToString();
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x000D6960 File Offset: 0x000D4B60
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

		// Token: 0x06002437 RID: 9271 RVA: 0x000D6A48 File Offset: 0x000D4C48
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

		// Token: 0x06002438 RID: 9272 RVA: 0x000D6A98 File Offset: 0x000D4C98
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

		// Token: 0x06002439 RID: 9273 RVA: 0x000D6B24 File Offset: 0x000D4D24
		private void DisableOverlapingObjects()
		{
			GameController.Instance.turnInfoPanel.gameObject.SetActive(false);
			if (GameController.GameManager.GameFinished && GameController.Instance.combatPresenter != null)
			{
				GameController.Instance.combatPresenter.resultPanel.SetActive(false);
			}
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000D6B7C File Offset: 0x000D4D7C
		private void DisableUnnecessaryEntries(int numberOfPlayers)
		{
			for (int i = numberOfPlayers; i < this.Entries.childCount; i++)
			{
				this.Entries.GetChild(i).gameObject.SetActive(false);
			}
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x000D6BB8 File Offset: 0x000D4DB8
		public static string GetMultiplayerName(int faction)
		{
			return MultiplayerController.Instance.GetPlayersInGame().ToList<PlayerData>().Find((PlayerData player) => player.Faction == faction)
				.Name;
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x000D6BF8 File Offset: 0x000D4DF8
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

		// Token: 0x0600243D RID: 9277 RVA: 0x000D6EF4 File Offset: 0x000D50F4
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

		// Token: 0x0600243E RID: 9278 RVA: 0x000D6FBC File Offset: 0x000D51BC
		private void UpdatePlayerStructurePoints(Transform entry, PlayerEndGameStats playerStats, bool basic)
		{
			string structureRichTextColor = this.GetStructureRichTextColor(playerStats.structurePoints);
			int num = GameController.GameManager.StructureBonus.structureBonus.TierForValue(playerStats.structurePoints);
			entry.GetChild(4).GetChild(7).GetComponent<TextMeshProUGUI>()
				.text = (basic ? this.GetColorizedString(playerStats.structurePoints.ToString(), structureRichTextColor) : this.GetTierStringConstruction(num, structureRichTextColor));
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x000D7028 File Offset: 0x000D5228
		private static bool ShouldHighlightPlayer(Player player)
		{
			List<Player> list = GameController.GameManager.GetPlayersWithoutAI().ToList<Player>();
			if (GameController.GameManager.IsMultiplayer)
			{
				return player == GameController.GameManager.PlayerOwner;
			}
			return list.Count == 1 && player == list[0];
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000D7074 File Offset: 0x000D5274
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

		// Token: 0x06002441 RID: 9281 RVA: 0x0003F07C File Offset: 0x0003D27C
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

		// Token: 0x06002442 RID: 9282 RVA: 0x0003F0AD File Offset: 0x0003D2AD
		private string GetColorizedString(string data, string color)
		{
			return string.Format("<color={0}>{1}</color>", color, data);
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x000D70DC File Offset: 0x000D52DC
		private string GetTierStringConstruction(int level, string color)
		{
			string text = ScriptLocalization.Get("Statistics/Tier");
			return string.Format("<color={0}>{1} {2}</color>", color, text, level);
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x000D7108 File Offset: 0x000D5308
		private string GetTierStringPopularity(int level, string color)
		{
			string text = string.Empty;
			for (int i = 0; i < level; i++)
			{
				text += "I";
			}
			return string.Format("<color={0}>{1}</color>", color, text);
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x0003F0BB File Offset: 0x0003D2BB
		private string GetEquation(int score, int multiplier, string color)
		{
			return string.Format("({0} * {1})", score / multiplier, this.GetColorizedString(multiplier.ToString(), color));
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x0003F0DD File Offset: 0x0003D2DD
		private string FormELOString(int oldRating, int rating)
		{
			return string.Format("{0} ({1})", rating, rating - oldRating);
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x000D7140 File Offset: 0x000D5340
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

		// Token: 0x06002448 RID: 9288 RVA: 0x000D71C4 File Offset: 0x000D53C4
		public void UpdateEntries(bool normal = true, bool multiplayer = false)
		{
			for (int i = 0; i < this.stats.Count; i++)
			{
				this.UpdateEntry(i, this.stats[i], normal, multiplayer);
			}
			List<Player> list = GameController.GameManager.GetPlayersWithoutAI().ToList<Player>();
			Player player = (multiplayer ? GameController.GameManager.PlayerOwner : list[0]);
			if (GameController.GameManager.GameFinished)
			{
				AchievementManager.UpdateAchievementsEndgame(this.stats, player);
			}
			this.UpdateStructurePointsDescription();
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x0003F0F7 File Offset: 0x0003D2F7
		public void ShowDetailsToggled()
		{
			this.UpdateLabels();
			this.UpdateEntries(!this.detailsToggle.isOn, GameController.GameManager.IsMultiplayer);
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x000D7244 File Offset: 0x000D5444
		private void UpdateLocalizedStats()
		{
			for (int i = 0; i < this.stats.Count; i++)
			{
				Transform child = this.Entries.GetChild(i);
				this.UpdatePlayerNameAndFaction(child, this.stats[i]);
				this.UpdatePlayerStructurePoints(child, this.stats[i], !this.detailsToggle.isOn);
			}
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x000D72A8 File Offset: 0x000D54A8
		private void UpdateStructurePointsDescription()
		{
			this.StructureBonusDescription.text = GameController.GetStructureBonusName(GameController.GameManager.StructureBonus.CardId) + Environment.NewLine + Environment.NewLine + GameController.GetStructureBonusDescription(GameController.GameManager.StructureBonus.CardId);
			this.StructureBonusValues.text = GameController.GameManager.StructureBonus.structureBonus.ToString();
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x0003F11D File Offset: 0x0003D31D
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

		// Token: 0x0600244D RID: 9293 RVA: 0x0003F153 File Offset: 0x0003D353
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

		// Token: 0x0600244E RID: 9294 RVA: 0x0003F189 File Offset: 0x0003D389
		public void PrepareInitial()
		{
			if (this.animator != null)
			{
				this.animator.Play("WinnerPanelInitial");
			}
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x0003F1A9 File Offset: 0x0003D3A9
		public void PrepareHoldForWinnerShowcase()
		{
			if (this.animator != null)
			{
				this.animator.Play("WinnerPanelHold");
			}
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x0003F1C9 File Offset: 0x0003D3C9
		private void ResetToDefault()
		{
			CameraControler.CameraMovementBlocked = true;
			if (this.animator != null)
			{
				this.animator.Play("WinnerPanelDefault");
			}
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x0003F1EF File Offset: 0x0003D3EF
		private void Bounce()
		{
			if (this.animator != null)
			{
				this.animator.Play("WinnerPanelBounce");
			}
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x000D7318 File Offset: 0x000D5518
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

		// Token: 0x06002453 RID: 9299 RVA: 0x0003F20F File Offset: 0x0003D40F
		public void ReturnToLobby()
		{
			WorldSFXManager.PlaySound(SoundEnum.ExitButton, AudioSourceType.Buttons);
			CameraControler.CameraMovementBlocked = false;
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_exit_button);
			GameController.Instance.ExitGame();
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x0003F20F File Offset: 0x0003D40F
		public void ReturnToMenu()
		{
			WorldSFXManager.PlaySound(SoundEnum.ExitButton, AudioSourceType.Buttons);
			CameraControler.CameraMovementBlocked = false;
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_exit_button);
			GameController.Instance.ExitGame();
		}

		// Token: 0x0400194E RID: 6478
		public bool isPreview;

		// Token: 0x0400194F RID: 6479
		public Transform Lables;

		// Token: 0x04001950 RID: 6480
		public Transform Entries;

		// Token: 0x04001951 RID: 6481
		public TextMeshProUGUI StructureBonusDescription;

		// Token: 0x04001952 RID: 6482
		public Text StructureBonusValues;

		// Token: 0x04001953 RID: 6483
		public Button okButton;

		// Token: 0x04001954 RID: 6484
		public GameObject exitButtonOutline;

		// Token: 0x04001955 RID: 6485
		public Toggle detailsToggle;

		// Token: 0x04001956 RID: 6486
		public TextMeshProUGUI roundCount;

		// Token: 0x04001957 RID: 6487
		public const int TEXT_HEIGHT = 20;

		// Token: 0x04001958 RID: 6488
		public const int ICON_HEIGHT = 30;

		// Token: 0x04001959 RID: 6489
		private List<PlayerEndGameStats> stats;

		// Token: 0x0400195A RID: 6490
		private Animator animator;

		// Token: 0x0400195B RID: 6491
		private bool allowForMinimize;

		// Token: 0x0400195C RID: 6492
		private bool allowForMaximize;
	}
}

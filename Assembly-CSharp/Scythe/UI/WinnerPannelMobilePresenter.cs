using System;
using System.Collections.Generic;
using System.Linq;
using HoneyFramework;
using I2.Loc;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004B1 RID: 1201
	public class WinnerPannelMobilePresenter : MonoBehaviour
	{
		// Token: 0x06002615 RID: 9749 RVA: 0x000E2298 File Offset: 0x000E0498
		public void ShowStats(List<PlayerEndGameStats> playerStats, bool multiplayer)
		{
			this.isMultiplayer = multiplayer;
			this.roundsCounter.text = ScriptLocalization.Get("GameScene/Round") + ": " + (GameController.GameManager.TurnCount + 1).ToString();
			if (multiplayer)
			{
				this.bigWinnerName.text = EndGameStatsPresenter.GetMultiplayerName((int)playerStats[0].player.matFaction.faction) + " " + ScriptLocalization.Get("Common/Won");
			}
			else
			{
				this.bigWinnerName.text = playerStats[0].player.Name + " " + ScriptLocalization.Get("Common/Won");
			}
			this.winnerPanelUI.SetActive(true);
			this.SetupFirstPanelSummaries(playerStats, multiplayer);
			CameraControler.CameraMovementBlocked = true;
			this.stats = playerStats;
			this.DisableOverlapingObjects();
			this.EndGameAchievment(GameController.GameManager.IsMultiplayer);
			this.backToMenuButton.onClick.RemoveAllListeners();
			if (!GameController.GameManager.IsMultiplayer)
			{
				this.backToMenuButton.onClick.AddListener(new UnityAction(this.ReturnToMenu));
			}
			else
			{
				this.backToMenuButton.onClick.AddListener(new UnityAction(this.ReturnToLobby));
			}
			this.SetupTopScorePresenter(playerStats);
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x0004057B File Offset: 0x0003E77B
		public TopMenuScorePresenter GetTopScoreMenuPresenter()
		{
			return this.topMenuScorePresenter;
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x000E23E4 File Offset: 0x000E05E4
		private void SetupFirstPanelSummaries(List<PlayerEndGameStats> playerStats, bool multiplayer)
		{
			if (playerStats.Count < 3)
			{
				this.factionSummaryEndGames[0].gameObject.SetActive(true);
				this.factionSummaryEndGames[1].gameObject.SetActive(true);
				this.factionSummaryEndGames[2].gameObject.SetActive(false);
				if (multiplayer)
				{
					this.factionSummaryEndGames[0].SetupPlayerSummaryInfo(0, playerStats[0].player.matFaction.faction, playerStats[0].totalPoints, EndGameStatsPresenter.GetMultiplayerName((int)playerStats[0].player.matFaction.faction), this.GetLaurelImage(0), this.GetFactionBackgroundImage(playerStats[0].player.matFaction.faction));
				}
				else
				{
					this.factionSummaryEndGames[0].SetupPlayerSummaryInfo(0, playerStats[0].player.matFaction.faction, playerStats[0].totalPoints, playerStats[0].player.Name, this.GetLaurelImage(0), this.GetFactionBackgroundImage(playerStats[0].player.matFaction.faction));
				}
				if (multiplayer)
				{
					this.factionSummaryEndGames[1].SetupPlayerSummaryInfo(1, playerStats[1].player.matFaction.faction, playerStats[1].totalPoints, EndGameStatsPresenter.GetMultiplayerName((int)playerStats[1].player.matFaction.faction), this.GetLaurelImage(1), this.GetFactionBackgroundImage(playerStats[1].player.matFaction.faction));
					return;
				}
				this.factionSummaryEndGames[1].SetupPlayerSummaryInfo(1, playerStats[1].player.matFaction.faction, playerStats[1].totalPoints, playerStats[1].player.Name, this.GetLaurelImage(1), this.GetFactionBackgroundImage(playerStats[1].player.matFaction.faction));
				return;
			}
			else
			{
				this.factionSummaryEndGames[0].gameObject.SetActive(true);
				this.factionSummaryEndGames[1].gameObject.SetActive(true);
				this.factionSummaryEndGames[2].gameObject.SetActive(true);
				if (multiplayer)
				{
					this.factionSummaryEndGames[0].SetupPlayerSummaryInfo(0, playerStats[0].player.matFaction.faction, playerStats[0].totalPoints, EndGameStatsPresenter.GetMultiplayerName((int)playerStats[0].player.matFaction.faction), this.GetLaurelImage(0), this.GetFactionBackgroundImage(playerStats[0].player.matFaction.faction));
				}
				else
				{
					this.factionSummaryEndGames[0].SetupPlayerSummaryInfo(0, playerStats[0].player.matFaction.faction, playerStats[0].totalPoints, playerStats[0].player.Name, this.GetLaurelImage(0), this.GetFactionBackgroundImage(playerStats[0].player.matFaction.faction));
				}
				if (multiplayer)
				{
					this.factionSummaryEndGames[1].SetupPlayerSummaryInfo(1, playerStats[1].player.matFaction.faction, playerStats[1].totalPoints, EndGameStatsPresenter.GetMultiplayerName((int)playerStats[1].player.matFaction.faction), this.GetLaurelImage(1), this.GetFactionBackgroundImage(playerStats[1].player.matFaction.faction));
				}
				else
				{
					this.factionSummaryEndGames[1].SetupPlayerSummaryInfo(1, playerStats[1].player.matFaction.faction, playerStats[1].totalPoints, playerStats[1].player.Name, this.GetLaurelImage(1), this.GetFactionBackgroundImage(playerStats[1].player.matFaction.faction));
				}
				if (multiplayer)
				{
					this.factionSummaryEndGames[2].SetupPlayerSummaryInfo(2, playerStats[2].player.matFaction.faction, playerStats[2].totalPoints, EndGameStatsPresenter.GetMultiplayerName((int)playerStats[2].player.matFaction.faction), this.GetLaurelImage(2), this.GetFactionBackgroundImage(playerStats[2].player.matFaction.faction));
					return;
				}
				this.factionSummaryEndGames[2].SetupPlayerSummaryInfo(2, playerStats[2].player.matFaction.faction, playerStats[2].totalPoints, playerStats[2].player.Name, this.GetLaurelImage(2), this.GetFactionBackgroundImage(playerStats[2].player.matFaction.faction));
				return;
			}
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x000E28A4 File Offset: 0x000E0AA4
		private void SetupTopScorePresenter(List<PlayerEndGameStats> playerStats)
		{
			this.topMenuScorePresenter.InitScoreEntries(playerStats);
			for (int i = 0; i < this.stats.Count; i++)
			{
				this.topMenuScorePresenter.SetPlaceOnPodium(i, i);
			}
			this.topMenuScorePresenter.ShowStats(playerStats);
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x000E28EC File Offset: 0x000E0AEC
		public void EndGameAchievment(bool multiplayer = false)
		{
			List<Player> list = GameController.GameManager.GetPlayersWithoutAI().ToList<Player>();
			Player player = (multiplayer ? GameController.GameManager.PlayerOwner : list[0]);
			if (GameController.GameManager.GameFinished)
			{
				AchievementManager.UpdateAchievementsEndgame(this.stats, player);
			}
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x00040583 File Offset: 0x0003E783
		private Sprite GetLaurelImage(int placeOnPodium)
		{
			switch (placeOnPodium)
			{
			case 0:
				return this.laurelImages[0];
			case 1:
				return this.laurelImages[1];
			case 2:
				return this.laurelImages[2];
			default:
				return this.laurelImages[0];
			}
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x000E2938 File Offset: 0x000E0B38
		private Sprite GetFactionBackgroundImage(Faction faction)
		{
			switch (faction)
			{
			case Faction.Polania:
				return this.factionBackgroundsImages[0];
			case Faction.Albion:
				return this.factionBackgroundsImages[1];
			case Faction.Nordic:
				return this.factionBackgroundsImages[2];
			case Faction.Rusviet:
				return this.factionBackgroundsImages[3];
			case Faction.Togawa:
				return this.factionBackgroundsImages[4];
			case Faction.Crimea:
				return this.factionBackgroundsImages[5];
			case Faction.Saxony:
				return this.factionBackgroundsImages[6];
			default:
				return this.factionBackgroundsImages[7];
			}
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x000405BC File Offset: 0x0003E7BC
		public void ReturnToLobby()
		{
			WorldSFXManager.PlaySound(SoundEnum.ExitButton, AudioSourceType.Buttons);
			CameraControler.CameraMovementBlocked = false;
			GameController.Instance.ExitGame();
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x000405BC File Offset: 0x0003E7BC
		public void ReturnToMenu()
		{
			WorldSFXManager.PlaySound(SoundEnum.ExitButton, AudioSourceType.Buttons);
			CameraControler.CameraMovementBlocked = false;
			GameController.Instance.ExitGame();
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x000405D6 File Offset: 0x0003E7D6
		public void NextPanelButton()
		{
			this.winnerPanels[0].SetActive(false);
			this.winnerPanels[1].SetActive(true);
			this.winnerPanelsButtons[0].SetActive(false);
			this.winnerPanelsButtons[1].SetActive(true);
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x000E29B0 File Offset: 0x000E0BB0
		public void Reset()
		{
			this.minimizingScreen.Reset();
			this.topMenuScorePresenter.Reset();
			this.maximizeButton.gameObject.SetActive(false);
			this.winnerPanels[0].SetActive(true);
			this.winnerPanels[1].SetActive(false);
			this.winnerPanelsButtons[0].SetActive(true);
			this.winnerPanelsButtons[1].SetActive(false);
			this.winnerPanelUI.SetActive(false);
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x000E2A34 File Offset: 0x000E0C34
		private void DisableOverlapingObjects()
		{
			GameController.Instance.turnInfoPanel.gameObject.SetActive(false);
			GameController.Instance.combatPresenterMobile.turnInfoPanelCombat.gameObject.SetActive(false);
			if (GameController.GameManager.GameFinished && GameController.Instance.combatPresenter != null)
			{
				GameController.Instance.combatPresenter.resultPanel.SetActive(false);
			}
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x00040610 File Offset: 0x0003E810
		public void MinimizeButton_OnClick()
		{
			WorldSFXManager.PlaySound(SoundEnum.GuiChatButton, AudioSourceType.Buttons);
			this.minimizingScreen.Minimize();
			CameraControler.CameraMovementBlocked = false;
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x0004062B File Offset: 0x0003E82B
		public void MaximizeButton_OnClick()
		{
			WorldSFXManager.PlaySound(SoundEnum.GuiChatButton, AudioSourceType.Buttons);
			this.minimizingScreen.Maximize();
			CameraControler.CameraMovementBlocked = true;
			this.UpdateLanguage();
			this.topMenuScorePresenter.UpdateEntriesLanguage();
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x000E2AA4 File Offset: 0x000E0CA4
		private void UpdateLanguage()
		{
			this.roundsCounter.text = ScriptLocalization.Get("GameScene/Round") + ": " + (GameController.GameManager.TurnCount + 1).ToString();
			if (this.isMultiplayer)
			{
				this.bigWinnerName.text = EndGameStatsPresenter.GetMultiplayerName((int)this.stats[0].player.matFaction.faction) + " " + ScriptLocalization.Get("Common/Won");
				return;
			}
			this.bigWinnerName.text = this.stats[0].player.Name + " " + ScriptLocalization.Get("Common/Won");
		}

		// Token: 0x04001B06 RID: 6918
		[SerializeField]
		private FactionSummaryEndGame[] factionSummaryEndGames;

		// Token: 0x04001B07 RID: 6919
		[SerializeField]
		private GameObject[] winnerPanels = new GameObject[2];

		// Token: 0x04001B08 RID: 6920
		[SerializeField]
		private Sprite[] laurelImages = new Sprite[3];

		// Token: 0x04001B09 RID: 6921
		[SerializeField]
		private Sprite[] factionBackgroundsImages = new Sprite[7];

		// Token: 0x04001B0A RID: 6922
		[SerializeField]
		private TextMeshProUGUI roundsCounter;

		// Token: 0x04001B0B RID: 6923
		[SerializeField]
		private GameObject[] winnerPanelsButtons = new GameObject[2];

		// Token: 0x04001B0C RID: 6924
		[SerializeField]
		private Button backToMenuButton;

		// Token: 0x04001B0D RID: 6925
		[SerializeField]
		private GameObject winnerPanelUI;

		// Token: 0x04001B0E RID: 6926
		[SerializeField]
		private TextMeshProUGUI bigWinnerName;

		// Token: 0x04001B0F RID: 6927
		[SerializeField]
		private TopMenuScorePresenter topMenuScorePresenter;

		// Token: 0x04001B10 RID: 6928
		[SerializeField]
		private MinimizingScreen minimizingScreen;

		// Token: 0x04001B11 RID: 6929
		[SerializeField]
		private Button maximizeButton;

		// Token: 0x04001B12 RID: 6930
		private List<PlayerEndGameStats> stats;

		// Token: 0x04001B13 RID: 6931
		private bool isMultiplayer;
	}
}

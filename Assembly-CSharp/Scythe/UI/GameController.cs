using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.GameSaves;
using DG.Tweening;
using HoneyFramework;
using I2.Loc;
using Scythe.Analytics;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using Scythe.Multiplayer.Messages;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000454 RID: 1108
	public class GameController : MonoBehaviour
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x0600229B RID: 8859 RVA: 0x0003E271 File Offset: 0x0003C471
		// (set) Token: 0x0600229C RID: 8860 RVA: 0x0003E278 File Offset: 0x0003C478
		public static HashSet<UnitPresenter> FocusUnit { get; private set; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x0600229D RID: 8861 RVA: 0x0003E280 File Offset: 0x0003C480
		// (set) Token: 0x0600229E RID: 8862 RVA: 0x0003E287 File Offset: 0x0003C487
		public static GameController.SelectionMode HexSelectionMode { get; set; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x0600229F RID: 8863 RVA: 0x0003E28F File Offset: 0x0003C48F
		// (set) Token: 0x060022A0 RID: 8864 RVA: 0x0003E297 File Offset: 0x0003C497
		public bool AdjustingPresenters { get; private set; }

		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x060022A1 RID: 8865 RVA: 0x000CD8A4 File Offset: 0x000CBAA4
		// (remove) Token: 0x060022A2 RID: 8866 RVA: 0x000CD8D8 File Offset: 0x000CBAD8
		public static event GameController.HexFocused HexGetFocused;

		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x060022A3 RID: 8867 RVA: 0x000CD90C File Offset: 0x000CBB0C
		// (remove) Token: 0x060022A4 RID: 8868 RVA: 0x000CD940 File Offset: 0x000CBB40
		public static event GameController.UnitFocused UnitGetFocused;

		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x060022A5 RID: 8869 RVA: 0x000CD974 File Offset: 0x000CBB74
		// (remove) Token: 0x060022A6 RID: 8870 RVA: 0x000CD9A8 File Offset: 0x000CBBA8
		public static event GameController.OnEndTurn OnEndTurnClick;

		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x060022A7 RID: 8871 RVA: 0x000CD9DC File Offset: 0x000CBBDC
		// (remove) Token: 0x060022A8 RID: 8872 RVA: 0x000CDA10 File Offset: 0x000CBC10
		public static event GameController.OnEndTurn AfterEndTurnAIAndPlayer;

		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x060022A9 RID: 8873 RVA: 0x000CDA44 File Offset: 0x000CBC44
		// (remove) Token: 0x060022AA RID: 8874 RVA: 0x000CDA78 File Offset: 0x000CBC78
		public static event GameController.OnObjectiveComplete OnObjectiveCompleted;

		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x060022AB RID: 8875 RVA: 0x000CDAAC File Offset: 0x000CBCAC
		// (remove) Token: 0x060022AC RID: 8876 RVA: 0x000CDAE0 File Offset: 0x000CBCE0
		public static event GameController.OnTopTabClick OnTopTabClicked;

		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x060022AD RID: 8877 RVA: 0x000CDB14 File Offset: 0x000CBD14
		// (remove) Token: 0x060022AE RID: 8878 RVA: 0x000CDB48 File Offset: 0x000CBD48
		public static event GameController.OnGameLoad OnGameLoaded;

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x060022AF RID: 8879 RVA: 0x0003E2A0 File Offset: 0x0003C4A0
		// (set) Token: 0x060022B0 RID: 8880 RVA: 0x0003E2A7 File Offset: 0x0003C4A7
		public static Dictionary<Faction, GameController.FactionInfo> factionInfo { get; private set; }

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x060022B1 RID: 8881 RVA: 0x0003E2AF File Offset: 0x0003C4AF
		// (set) Token: 0x060022B2 RID: 8882 RVA: 0x0003E2B6 File Offset: 0x0003C4B6
		public static Dictionary<Faction, PlayerUnits> factionUnits { get; private set; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x060022B3 RID: 8883 RVA: 0x0003E2BE File Offset: 0x0003C4BE
		public static Scythe.GameLogic.Game Game
		{
			get
			{
				if (PersistentSingleton<GameLogicHandler>.Instance == null)
				{
					return null;
				}
				return PersistentSingleton<GameLogicHandler>.Instance.Game;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x060022B4 RID: 8884 RVA: 0x0003E2D9 File Offset: 0x0003C4D9
		public static GameManager GameManager
		{
			get
			{
				if (PersistentSingleton<GameLogicHandler>.Instance == null)
				{
					return null;
				}
				return PersistentSingleton<GameLogicHandler>.Instance.GameManager;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x060022B5 RID: 8885 RVA: 0x0003E2F4 File Offset: 0x0003C4F4
		// (set) Token: 0x060022B6 RID: 8886 RVA: 0x0003E2FC File Offset: 0x0003C4FC
		public bool GameFinishedTimeOut
		{
			get
			{
				return this.gameFinishedTimeOut;
			}
			set
			{
				this.gameFinishedTimeOut = value;
			}
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x000CDB7C File Offset: 0x000CBD7C
		private void Awake()
		{
			GameController.Instance = this;
			OptionsManager.OnLanguageChanged += this.UpdateFactionInfo;
			OptionsManager.OnLanguageChanged += this.UpdateTurnCounterText;
			this.UpdateFactionInfo();
			if (GameController.GameManager.PlayerCount == 0)
			{
				switch (this.quickstartMode)
				{
				case GameController.QuickstartMode.AI:
				{
					bool flag = GameServiceController.Instance.InvadersFromAfarUnlocked();
					List<MatAndFactionSelection.PlayerEntry> list = new List<MatAndFactionSelection.PlayerEntry>();
					list.Add(new MatAndFactionSelection.PlayerEntry("Kosynier", 0, flag, 5, 0, default(Guid)));
					list.Add(new MatAndFactionSelection.PlayerEntry("Bot 1", 1, flag, 2, 1, default(Guid)));
					list.Add(new MatAndFactionSelection.PlayerEntry("Bot 2", 1, flag, 0, 2, default(Guid)));
					GameController.GameManager.Init(list, true, flag, true);
					break;
				}
				case GameController.QuickstartMode.Tutorial01:
					GameController.GameManager.missionId = 0;
					GameController.GameManager.InitTutorialAsmB01(GameController.GameManager);
					GameController.GameManager.IsCampaign = true;
					break;
				case GameController.QuickstartMode.Tutorial02:
					GameController.GameManager.missionId = 1;
					GameController.GameManager.InitTutorial02Asm(GameController.GameManager);
					GameController.GameManager.IsCampaign = true;
					break;
				case GameController.QuickstartMode.Tutorial03:
					GameController.GameManager.missionId = 2;
					GameController.GameManager.InitTutorial03Asm(GameController.GameManager);
					GameController.GameManager.IsCampaign = true;
					break;
				case GameController.QuickstartMode.Tutorial04:
					GameController.GameManager.missionId = 3;
					GameController.GameManager.InitTutorial04Asm(GameController.GameManager);
					GameController.GameManager.IsCampaign = true;
					break;
				case GameController.QuickstartMode.Tutorial05:
					GameController.GameManager.missionId = 4;
					GameController.GameManager.InitTutorial05Asm(GameController.GameManager);
					GameController.GameManager.IsCampaign = true;
					break;
				case GameController.QuickstartMode.Tutorial06:
					GameController.GameManager.missionId = 5;
					GameController.GameManager.InitTutorial06Asm(GameController.GameManager);
					GameController.GameManager.IsCampaign = true;
					break;
				case GameController.QuickstartMode.Tutorial07:
					GameController.GameManager.missionId = 6;
					GameController.GameManager.InitTutorial07Asm(GameController.GameManager);
					GameController.GameManager.IsCampaign = true;
					break;
				case GameController.QuickstartMode.Tutorial08:
					GameController.GameManager.missionId = 7;
					GameController.GameManager.InitTutorial08Asm(GameController.GameManager);
					GameController.GameManager.IsCampaign = true;
					break;
				case GameController.QuickstartMode.Tutorial09:
					GameController.GameManager.missionId = 8;
					GameController.GameManager.InitTutorial09Asm(GameController.GameManager);
					GameController.GameManager.IsCampaign = true;
					break;
				case GameController.QuickstartMode.Tutorial10:
					GameController.GameManager.missionId = 9;
					GameController.GameManager.InitTutorial10Asm(GameController.GameManager);
					GameController.GameManager.IsCampaign = true;
					break;
				case GameController.QuickstartMode.Tutorial11:
					GameController.GameManager.missionId = 10;
					GameController.GameManager.InitTutorial11Asm(GameController.GameManager);
					GameController.GameManager.IsCampaign = true;
					break;
				case GameController.QuickstartMode.Challenge1:
					GameController.GameManager.challengesLogicStarter.InitChallenge(GameController.GameManager, 0, 0);
					GameController.GameManager.IsChallenge = true;
					break;
				case GameController.QuickstartMode.Challenge2:
					GameController.GameManager.challengesLogicStarter.InitChallenge(GameController.GameManager, 1, 0);
					GameController.GameManager.IsChallenge = true;
					break;
				case GameController.QuickstartMode.Challenge3:
					GameController.GameManager.challengesLogicStarter.InitChallenge(GameController.GameManager, 2, 0);
					GameController.GameManager.IsChallenge = true;
					break;
				case GameController.QuickstartMode.Challenge4:
					GameController.GameManager.challengesLogicStarter.InitChallenge(GameController.GameManager, 3, 0);
					GameController.GameManager.IsChallenge = true;
					break;
				default:
					Debug.Log("Quickstart mode: " + this.quickstartMode.ToString());
					GameController.GameManager.InitCampaign((int)this.quickstartMode, 0);
					break;
				}
			}
			this.InitializeFactionProperties();
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x000CDF2C File Offset: 0x000CC12C
		private void InitializeFactionProperties()
		{
			if (GameController.factionInfo == null)
			{
				GameController.factionInfo = new Dictionary<Faction, GameController.FactionInfo>();
				this.factionCamera = new Dictionary<Faction, Vector3>();
			}
			else
			{
				GameController.factionInfo.Clear();
				this.factionCamera.Clear();
			}
			for (int i = 0; i < this.factionLogos.Length; i++)
			{
				GameController.factionInfo.Add(this.factionLogos[i].faction, this.factionLogos[i]);
				this.factionCamera.Add(this.factionLogos[i].faction, new Vector3(0f, 0.5f, 0f));
			}
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x000CDFCC File Offset: 0x000CC1CC
		private void Start()
		{
			SceneController.AfterLoadingTheGame();
			MainMenu.AfterLoadingTheGame();
			this.gameBoard2d.SetActive(true);
			this.gameBoardPresenter.Initialize();
			this.reflectionProbes.SetActive(true);
			DynamicGI.UpdateEnvironment();
			this.Init(false);
			this.optionsMenu.GetComponent<OptionsManager>().LoadOptionsPrefs();
			if (!GameController.GameManager.IsMultiplayer)
			{
				if (GameController.GameManager.GetPlayersWithoutAICount() > 1)
				{
					this.HideHumanPlayerInfo();
				}
				if (this.changeFactionPanel != null)
				{
					this.changeFactionPanel.Disable();
				}
			}
			else if (GameController.GameManager.SpectatorMode)
			{
				this.SetDragAndDrop(false);
				this.HideHumanPlayerInfo();
				if (this.changeFactionPanel != null)
				{
					this.changeFactionPanel.Init();
				}
			}
			if (!GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsCampaign)
			{
				PlayerPrefs.SetInt(OptionsManager.PREFS_GAME_CLOSED_CORRECT, OptionsManager.BoolToInt(false));
				PlayerPrefs.SetString(OptionsManager.PREFS_OFFLINE_GAME_ID, GameController.Game.GetGameId());
			}
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x0003E305 File Offset: 0x0003C505
		public void CheckDLCSanity()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				return;
			}
			if (GameController.GameManager.HasDLCContent() && !GameServiceController.Instance.InvadersFromAfarUnlocked())
			{
				Debug.LogWarning("Loading DLC save but DLC not detected.");
				this.ExitGame();
			}
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x0003E33C File Offset: 0x0003C53C
		private void OnDestroy()
		{
			DOTween.Clear(false);
			this.ClearDelegates();
			if (GameController.GameManager != null && !GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsCampaign)
			{
				PlayerPrefs.SetInt(OptionsManager.PREFS_GAME_CLOSED_CORRECT, OptionsManager.BoolToInt(true));
			}
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x000CE0CC File Offset: 0x000CC2CC
		private void Update()
		{
			if (ShowEnemyMoves.Instance != null)
			{
				if (ShowEnemyMoves.Instance.AllAnimationsFinished())
				{
					if (this.multiplayerActions.Count > 0)
					{
						this.multiplayerActions.Dequeue().Execute(GameController.GameManager);
					}
				}
				else
				{
					ShowEnemyMoves.Instance.GetNextAnimation();
				}
			}
			else if (this.multiplayerActions.Count > 0)
			{
				this.multiplayerActions.Dequeue().Execute(GameController.GameManager);
			}
			if (this.waitForGameEnd && GameController.GameManager.GameFinished)
			{
				this.waitForGameEnd = false;
				this.OnMultiplayerGameEnded();
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				bool presenationFinished = this.cameraMovementEffects.presenationFinished;
			}
			if (Input.GetKeyDown(KeyCode.F11) && this.ui != null)
			{
				this.ui.enabled = !this.ui.enabled;
			}
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x000CE1B0 File Offset: 0x000CC3B0
		private void Init(bool undoLoad = false)
		{
			this.isUndoLoad = undoLoad;
			GameController.HexSelectionMode = GameController.SelectionMode.Normal;
			ShowEnemyMoves.Instance.Init();
			this.StatsSaved = false;
			this.InitializeFactionProperties();
			GameController.factionUnits = new Dictionary<Faction, PlayerUnits>();
			GameController.FocusUnit = new HashSet<UnitPresenter>();
			this.DragAndDrop = OptionsManager.IsDragAndDropActive();
			this.waitForGameEnd = false;
			for (int i = 0; i < this.playerUnits.Length; i++)
			{
				if (this.playerUnits[i] != null)
				{
					GameController.factionUnits.Add(this.playerUnits[i].faction, this.playerUnits[i]);
					this.playerUnits[i].Init();
				}
			}
			this.playersFactions.UpdateStatus();
			if (PlatformManager.IsStandalone)
			{
				this.panelInfo.Init(GameController.GameManager.StructureBonus);
			}
			else
			{
				SingletonMono<TopMenuPanelsManager>.Instance.StructuresBonusInit(GameController.GameManager.StructureBonus);
			}
			GameController.GameManager.ObjectiveCardGetHighlighted += this.OnObjectiveCardHighlight;
			GameController.GameManager.EncounterGetEnabled += this.OnEncounterEnabled;
			GameController.GameManager.FactoryGetEnabled += this.OnFactoryEnabled;
			GameController.GameManager.GameHasEnded += this.OnGameEnded;
			GameController.GameManager.UpdatePlayerStats += this.OnUpdatePlayerStats;
			GameController.GameManager.CombatCardsAmountStatus += this.OnNoMoreCombatCards;
			GameController.GameManager.BotTurnEnded += this.ContinueBotAction;
			GameController.GameManager.OnEnemyUnitMoved += this.OnEnemyUnitMove;
			if (GameController.GameManager.showEnemyActions)
			{
				GameController.GameManager.OnEnemyProduced += this.OnEnemyProduce;
				GameController.GameManager.OnEnemyGainedWorker += this.OnEnemyGainWorker;
				GameController.GameManager.OnEnemyGainedWorkerEnds += this.OnEnemyGainWorkerEnds;
				GameController.GameManager.OnEnemyTraded += this.OnEnemyTrade;
				GameController.GameManager.OnEnemyRecruitBonusObtain += this.OnEnemyRecruitBonus;
				GameController.GameManager.OnEnemysBonusEnd += this.OnEnemysBonusEnd;
				GameController.GameManager.OnEnemyRecruited += this.OnEnemyRecruit;
				GameController.GameManager.OnEnemyUpgraded += this.OnEnemyUpgrade;
				GameController.GameManager.OnEnemyDeployed += this.OnEnemyDeploy;
				GameController.GameManager.OnEnemyBuilded += this.OnEnemyBuild;
				GameController.GameManager.OnEnemyPaidResources += this.OnEnemyPayResource;
				GameController.GameManager.OnEnemyMoved += this.OnEnemyMove;
				GameController.GameManager.OnEnemyLoadedResources += this.OnEnemyLoadResources;
				GameController.GameManager.OnEnemyLoadedWorker += this.OnEnemyLoadWorker;
				GameController.GameManager.OnEnemyUnloadedWorker += this.OnEnemyUnloadWorker;
				GameController.GameManager.OnEnemyRetreatMoved += this.OnEnemyRetreatMove;
				GameController.GameManager.OnEnemyGainStats += this.OnEnemyGainTopStats;
			}
			GameController.UnitGetFocused = null;
			if (GameController.GameManager.IsMultiplayer)
			{
				GameController.GameManager.ActionHasFinished += this.OnActionFinished;
				GameController.GameManager.InputWasEnabled += this.OnInputEnabled;
				GameController.GameManager.ShowedFactoryCards += this.OnShowFactoryCards;
				GameController.GameManager.CardAdded += this.AfterCardAdded;
				GameController.GameManager.ShowEncounter += this.ShowEncounter;
				GameController.GameManager.ChooseOption += this.ChooseOption;
				GameController.GameManager.ShowedFactory += this.OnShowChoosenFactory;
				GameController.GameManager.ShowedEmptyCards += this.OnShowEmptyCards;
				GameController.GameManager.ShowObjective += this.ShowObjectiveCard;
				GameController.GameManager.CombatAbilityUsed += this.CombatAbilityUsed;
				GameController.GameManager.OnShowStats += this.OnShowPoints;
				GameController.GameManager.MultiplayerGameEnded += this.GameEnded;
				GameController.GameManager.GameSynchronized += this.SynchronizeGame;
				GameController.GameManager.GameLoaded += this.GameLoaded;
				GameController.GameManager.BattlefieldChoosen += this.ChooseFirstBattlefield;
				GameController.GameManager.BotTurnEnded += this.OnTurnEnded;
				GameController.GameManager.TurnEnded += this.OnTurnEnded;
				GameController.GameManager.OnEncounterEnded += this.EndEncounter;
				this.returnToLobby = false;
			}
			this.EndTurnButtonReset();
			if (this.showAILogs)
			{
				AiPlayer.AttachLogDelegate(delegate(string s)
				{
					Debug.Log(s);
				});
			}
			this.autoGainPanel.LoadPreferences();
			if (GameController.GameManager.IsMultiplayer)
			{
				Player playerOwner = GameController.GameManager.PlayerOwner;
				this.matFaction.UpdateMat(playerOwner, GameController.factionInfo[playerOwner.matFaction.faction], true);
				this.matPlayer.isPreview = true;
				this.matPlayer.UpdateMat(playerOwner, false);
				this.matPlayer.isPreview = false;
				if (!PlatformManager.IsStandalone)
				{
					SingletonMono<TopMenuPanelsManager>.Instance.UpdateInfoFromMat(playerOwner, GameController.factionInfo[playerOwner.matFaction.faction], true);
				}
				this.playerStats.UpdateAllStats(playerOwner, GameController.factionInfo[playerOwner.matFaction.faction].logo);
				if (PlatformManager.IsStandalone)
				{
					this.panelInfo.UpdatePlayerInfo(GameController.factionInfo[playerOwner.matFaction.faction], GameController.GameManager.factionBasicInfo[playerOwner.matFaction.faction], playerOwner.objectiveCards, playerOwner.combatCards, false);
					this.chat.gameObject.SetActive(true);
					this.chat.chatElements.SetActive(false);
				}
				else
				{
					SingletonMono<TopMenuPanelsManager>.Instance.UpdateStats(playerOwner);
					SingletonMono<TopMenuPanelsManager>.Instance.UpdatePlayerInfo(GameController.factionInfo[playerOwner.matFaction.faction], GameController.GameManager.factionBasicInfo[playerOwner.matFaction.faction], playerOwner.objectiveCards, playerOwner.combatCards, false);
					if (MobileChat.IsSupported)
					{
						SingletonMono<MobileChat>.Instance.gameObject.SetActive(true);
						SingletonMono<MobileChat>.Instance.chatElements.SetActive(false);
					}
				}
			}
			else
			{
				if (GameController.GameManager.IsHotSeat)
				{
					AiPlayer.CombatAbilityUsed += this.CombatAbilityUsed;
					if (PlatformManager.IsStandalone)
					{
						this.chat.gameObject.SetActive(true);
						this.chat.chatElements.SetActive(false);
					}
					else if (MobileChat.IsSupported)
					{
						SingletonMono<MobileChat>.Instance.gameObject.SetActive(true);
						SingletonMono<MobileChat>.Instance.chatElements.SetActive(false);
					}
					if (!GameController.GameManager.PlayerCurrent.IsHuman)
					{
						List<Player> players = GameController.GameManager.GetPlayers();
						Player player = GameController.GameManager.PlayerCurrent;
						while (!player.IsHuman)
						{
							player = players[(GameController.GameManager.GetPlayerLocalId(player) + 1) % players.Count];
						}
						this.matFaction.UpdateMat(player, GameController.factionInfo[player.matFaction.faction], true);
						this.matPlayer.isPreview = true;
						this.matPlayer.UpdateMat(player, false);
						this.matPlayer.isPreview = false;
						if (!PlatformManager.IsStandalone)
						{
							SingletonMono<TopMenuPanelsManager>.Instance.UpdateInfoFromMat(player, GameController.factionInfo[player.matFaction.faction], true);
						}
						foreach (MatPlayerSectionPresenter matPlayerSectionPresenter in this.matPlayer.matSection)
						{
							matPlayerSectionPresenter.DisableActionsAndSection(0, true);
						}
						this.playerStats.UpdateAllStats(player, GameController.factionInfo[player.matFaction.faction].logo);
						if (PlatformManager.IsStandalone)
						{
							this.panelInfo.UpdatePlayerInfo(GameController.factionInfo[player.matFaction.faction], GameController.GameManager.factionBasicInfo[player.matFaction.faction], player.objectiveCards, player.combatCards, false);
						}
						if (!PlatformManager.IsStandalone)
						{
							SingletonMono<TopMenuPanelsManager>.Instance.UpdateStats(player);
							if (player.IsHuman)
							{
								SingletonMono<TopMenuPanelsManager>.Instance.UpdatePlayerInfo(GameController.factionInfo[player.matFaction.faction], GameController.GameManager.factionBasicInfo[player.matFaction.faction], player, false);
							}
							else
							{
								player = GameController.GameManager.GetNextHumanPlayer();
								SingletonMono<TopMenuPanelsManager>.Instance.UpdatePlayerInfo(GameController.factionInfo[player.matFaction.faction], GameController.GameManager.factionBasicInfo[player.matFaction.faction], player, false);
							}
						}
					}
				}
				if (this.chatButton != null)
				{
					this.chatButton.SetActive(false);
				}
			}
			this.AttachTopLeftTogglesHoover();
			this.TurnCounter.text = ScriptLocalization.Get("GameScene/Round") + " " + (GameController.GameManager.TurnCount + 1).ToString();
			this.gameBoardPresenter.UpdateBoard(true, true);
			this.gameBoardPresenter.UpdateTokens();
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x000CEB4C File Offset: 0x000CCD4C
		private void UpdateTurnCounterText()
		{
			this.TurnCounter.text = ScriptLocalization.Get("GameScene/Round") + " " + (GameController.GameManager.TurnCount + 1).ToString();
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x000CEB8C File Offset: 0x000CCD8C
		private void AttachTopLeftTogglesHoover()
		{
			foreach (Toggle2 toggle in this.tabToggles)
			{
				toggle.gameObject.GetComponent<PointerEventsController>().buttonHoover += this.TopLeftToggleSFX;
			}
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x0003C419 File Offset: 0x0003A619
		private void TopLeftToggleSFX()
		{
			WorldSFXManager.PlaySound(SoundEnum.PlayersBoardShowEnemysboardRelease, AudioSourceType.Buttons);
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x000CEBF4 File Offset: 0x000CCDF4
		private void EndTurnButtonReset()
		{
			this.endTurnButton.interactable = false;
			this.endTurnButton.image.sprite = this.endTurnButtonImageActive;
			this.endTurnHint.text = ScriptLocalization.Get("GameScene/ChooseAnyAction") + Environment.NewLine + ScriptLocalization.Get("GameScene/FromTheMatFirst");
			this.endTurnHintType = GameController.EndTurnHintType.ChoseActionFirst;
			this.endTurnHintButton.SetActive(true);
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x000CEC60 File Offset: 0x000CCE60
		public void EndTurnButtonEnable()
		{
			this.endTurnButton.interactable = true;
			this.endTurnButton.image.sprite = this.endTurnButtonImageActive;
			this.endTurnButton.GetComponent<Animator>().Play("EndTurnStatic");
			this.endTurnHintButton.SetActive(false);
			this.endTurnHint.enabled = false;
			this.endTurnShade.enabled = false;
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x0003E379 File Offset: 0x0003C579
		public void EndTurnButtonDisable()
		{
			this.endTurnButton.interactable = false;
			this.endTurnHintButton.SetActive(true);
			this.endTurnHint.enabled = false;
			this.endTurnShade.enabled = false;
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x000CECC8 File Offset: 0x000CCEC8
		public void EndTurnHint()
		{
			if ((!GameController.GameManager.IsMultiplayer || GameController.GameManager.PlayerOwner == GameController.GameManager.PlayerCurrent) && GameController.GameManager.PlayerCurrent.IsHuman)
			{
				switch (this.endTurnHintType)
				{
				case GameController.EndTurnHintType.ChoseActionFirst:
					this.endTurnHint.text = ScriptLocalization.Get("GameScene/ChooseAnyAction") + Environment.NewLine + ScriptLocalization.Get("GameScene/FromTheMatFirst");
					break;
				case GameController.EndTurnHintType.Combat:
					this.endTurnHint.text = ScriptLocalization.Get("GameScene/CombatEndTurnHint");
					break;
				case GameController.EndTurnHintType.WaitForOpponent:
					this.endTurnHint.text = ScriptLocalization.Get("GameScene/WaitForOpponent");
					break;
				case GameController.EndTurnHintType.ChoseFactoryCard:
					this.endTurnHint.text = ScriptLocalization.Get("GameScene/ChooseFactoryCard");
					break;
				case GameController.EndTurnHintType.GameEnded:
					this.endTurnHint.text = ScriptLocalization.Get("GameScene/GameEnded");
					break;
				}
				this.endTurnHint.enabled = !this.endTurnHint.enabled;
				this.endTurnShade.enabled = this.endTurnHint.enabled;
			}
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x0003E3AB File Offset: 0x0003C5AB
		public void OnLoadingScreenStartButtonClicked(bool loadingSave)
		{
			base.StartCoroutine(GameController.Instance.AfterMapLoad(loadingSave, false));
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x0003E3C0 File Offset: 0x0003C5C0
		public IEnumerator AfterMapLoad(bool saveFileLoaded = false, bool undo = false)
		{
			DOTween.KillAll(true);
			this.cameraControler.transform.localPosition = Vector3.zero;
			this.cameraControler.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
			this.undoController.PrepareAfterGameLoad(undo);
			if (ActionLogPresenter.Instance != null)
			{
				ActionLogPresenter.Instance.AfterGameLoaded();
				if (PlatformManager.IsMobile)
				{
					SingletonMono<ActionLogVisibilityMobile>.Instance.OpenActionLogButtonActive(true);
				}
			}
			CombatPresenter.battleResultQueue.Clear();
			if (undo)
			{
				this.tokensController.OnUndo();
				this.combatPresenter.OnUndo();
				this.playersFactions.OnUndo();
			}
			foreach (Faction faction in GameController.GameManager.GetPlayersFactions())
			{
				Vector2 vector = 0.8f * this.gameBoardPresenter.GetGameHexPresenter(GameController.GameManager.gameBoard.bases[faction]).GetWorldPosition();
				this.factionCamera[faction] = new Vector3(vector.x - 1.5f, 0.2f, vector.y);
			}
			if (GameController.GameManager.IsMultiplayer && !GameController.GameManager.showEnemyActions)
			{
				this.cameraControler.Load(this.factionCamera[GameController.GameManager.PlayerOwner.matFaction.faction]);
			}
			if (this.encounterCardPresenter != null && this.encounterCardPresenter.gameObject.activeInHierarchy)
			{
				this.encounterCardPresenter.CloseAfterLoadingTheGame();
				if (this.encounterCardPresenter.encounterCardSmall != null)
				{
					this.encounterCardPresenter.encounterCardSmall.transform.parent.gameObject.SetActive(false);
				}
			}
			if (!GameController.gameFromSave && !GameController.GameManager.IsCampaign && !MultiplayerController.Instance.ReturningToStartedGame && this.cameraPresentationEnabled)
			{
				yield return base.StartCoroutine(CameraMovementEffects.Instance.ShowPresentation());
			}
			else
			{
				if (ActionLogPresenter.Instance != null)
				{
					ActionLogPresenter.Instance.Show();
				}
				this.cameraMovementEffects.presenationFinished = true;
				if (!PlatformManager.IsStandalone)
				{
					SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Visible, 0.25f);
				}
				else
				{
					this.playersFactions.actionLog.gameObject.SetActive(true);
				}
			}
			this.gameBoardPresenter.UpdateBoard(false, false);
			this.gameBoardPresenter.UpdateTokens();
			if (GameController.GameManager.IsMultiplayer)
			{
				MultiplayerController.Instance.MyMapLoaded();
				this.turnInfoPanel.SetActualPlayerInfo();
			}
			this.matPlayer.isPreview = false;
			if (!GameController.GameManager.IsMultiplayer)
			{
				if (!GameController.GameManager.PlayerCurrent.IsHuman)
				{
					yield return base.StartCoroutine(this.AITurn());
				}
				else
				{
					this.UpdateTurnInfo(GameController.GameManager.PlayerCurrent);
				}
			}
			if (saveFileLoaded)
			{
				this.CherryPickUpdateAfterLoadGame();
				if (GameController.GameManager.FactoryCardsShown)
				{
					this.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].DisableActionsAndSection(0, true);
					this.EndTurnButtonDisable();
					this.OnShowFactoryCards();
				}
				if (this.gameBoardPresenter is FlatWorld)
				{
					(this.gameBoardPresenter as FlatWorld).ReadRotationFromLogicUnit();
				}
			}
			if (PlatformManager.IsMobile)
			{
				SingletonMono<TopMenuPanelsManager>.Instance.GetTopMenuObjectivesPresenter().objectivesPreview.OnUndo();
				GameController.GameManager.CheckObjectiveCards();
				RateGamePanel.SaveLastGameWon(false);
			}
			if (GameController.OnGameLoaded != null)
			{
				GameController.OnGameLoaded();
			}
			this.GameIsLoaded = false;
			if (!GameController.Game.GameManager.IsMultiplayer)
			{
				this.tokensController.AfterGameLoad();
			}
			if (PlatformManager.IsMobile)
			{
				this.uiCamera.gameObject.SetActive(false);
			}
			yield break;
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x000CEDE8 File Offset: 0x000CCFE8
		public void ActionsAfterCameraPresentation()
		{
			base.StartCoroutine(this.AITurn());
			if (!GameController.GameManager.IsCampaign)
			{
				this.ShowTurnInfo();
			}
			else
			{
				this.turnInfoPanel.DeactivateTurnInfoPanel();
			}
			if (GameController.GameManager.IsMultiplayer)
			{
				if (PlatformManager.IsMobile)
				{
					if (MobileChat.IsSupported)
					{
						SingletonMono<MobileChat>.Instance.chatElements.SetActive(true);
						return;
					}
				}
				else
				{
					this.chat.chatElements.SetActive(true);
				}
			}
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x0003E3DD File Offset: 0x0003C5DD
		private void AdjustPresentersAfterLoad()
		{
			if (!GameController.GameManager.IsMultiplayer || GameController.GameManager.SpectatorMode)
			{
				return;
			}
			if (GameController.GameManager.PlayerCurrent == GameController.GameManager.PlayerOwner)
			{
				this.AdjustCurrentPlayerPresenters();
				return;
			}
			this.AdjustOwnerPresenters();
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x000CEE60 File Offset: 0x000CD060
		private void AdjustCurrentPlayerPresenters()
		{
			if (GameController.GameManager.PlayerOwner.currentMatSection == -1)
			{
				return;
			}
			this.AdjustingPresenters = true;
			this.CherryPickUpdateAfterLoadGame();
			MatPlayerSectionPresenter matPlayerSectionPresenter = this.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection];
			if (GameController.GameManager.PlayerOwner.topActionInProgress)
			{
				this.matPlayer.StartTopAction(GameController.GameManager.actionManager.GetGainActionId());
				this.AdjustActionManager();
				matPlayerSectionPresenter.DisableActionsAndSection(0, true);
				if (GameController.GameManager.combatManager.GetBattlefields().Count > 0)
				{
					if (PlatformManager.IsMobile)
					{
						SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
					}
					this.EndTurnButtonDisable();
					((MovePresenter)HumanInputHandler.Instance.movePresenter).OnCombatBegin();
					GameController.GameManager.combatManager.OnCombatStageChanged += ((MovePresenter)HumanInputHandler.Instance.movePresenter).OnCombatStateChanged;
					this.AdjustAttackerCombatPresenters();
				}
			}
			else if (GameController.GameManager.PlayerOwner.topActionFinished)
			{
				this.matPlayer.SectionActionFinished();
				matPlayerSectionPresenter.topActionPresenter.DisableAllButtons(false);
				if (this.waitInfoFactory.activeInHierarchy || this.waitInfoEncounter.activeInHierarchy)
				{
					matPlayerSectionPresenter.DisableActionsAndSection(0, true);
					this.EndTurnButtonDisable();
				}
				if (GameController.GameManager.LastEncounterCard != null)
				{
					matPlayerSectionPresenter.DisableActionsAndSection(0, true);
					this.encounterCardPresenter.SetOptions(GameController.GameManager.LastEncounterCard);
					this.AdjustActionManager();
				}
				else if (GameController.GameManager.FactoryCardsShown)
				{
					matPlayerSectionPresenter.DisableActionsAndSection(0, true);
					this.EndTurnButtonDisable();
				}
				else if (GameController.GameManager.PlayerOwner.bottomActionInProgress)
				{
					this.matPlayer.StartBottomAction();
					this.AdjustActionManager();
					matPlayerSectionPresenter.DisableActionsAndSection(0, true);
					if (GameController.GameManager.actionManager.GetLastBonusAction() != null)
					{
						this.EndTurnButtonDisable();
						if (GameController.GameManager.actionManager.GetLastBonusAction().GetPlayer() != GameController.GameManager.PlayerOwner)
						{
							this.waitInfoRecruit.gameObject.SetActive(true);
						}
					}
					if (GameController.GameManager.combatManager.GetBattlefields().Count > 0)
					{
						this.EndTurnButtonDisable();
						((MovePresenter)HumanInputHandler.Instance.movePresenter).OnCombatBegin();
						GameController.GameManager.combatManager.OnCombatStageChanged += ((MovePresenter)HumanInputHandler.Instance.movePresenter).OnCombatStateChanged;
						this.AdjustAttackerCombatPresenters();
					}
				}
				else if (GameController.GameManager.PlayerOwner.downActionFinished)
				{
					this.matPlayer.SectionActionFinished();
					matPlayerSectionPresenter.DisableActionsAndSection(0, true);
					if (GameController.GameManager.LastEncounterCard != null)
					{
						this.encounterCardPresenter.SetOptions(GameController.GameManager.LastEncounterCard);
						this.AdjustActionManager();
					}
				}
			}
			this.AdjustActionPresenters();
			this.tokensController.AfterGameLoad();
			this.AdjustingPresenters = false;
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x000CF14C File Offset: 0x000CD34C
		private void AdjustOwnerPresenters()
		{
			if (GameController.GameManager.combatManager.GetBattlefields().Count > 0)
			{
				if (GameController.GameManager.combatManager.GetDefender() == GameController.GameManager.PlayerOwner)
				{
					this.AdjustDefenderCombatPresenters();
				}
			}
			else if (GameController.GameManager.LastEncounterCard != null)
			{
				this.encounterCardPresenter.SetOptions(GameController.GameManager.LastEncounterCard);
			}
			else if (GameController.GameManager.FactoryCardsShown)
			{
				int num = 1;
				using (List<Player>.Enumerator enumerator = GameController.GameManager.GetPlayers().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.matPlayer.GetPlayerMatSection(4) == null)
						{
							num++;
						}
					}
				}
				GameController.GameManager.OnShowEmptyCards(num);
			}
			else if (GameController.GameManager.actionManager.GetLastBonusAction() != null && GameController.GameManager.actionManager.GetLastBonusAction().GetPlayer() == GameController.GameManager.PlayerOwner)
			{
				GameController.GameManager.OnGainOngoingRecruitBonus(GameController.GameManager.actionManager.GetLastBonusAction());
			}
			this.matPlayer.AfterLoadUpdate();
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x000CF284 File Offset: 0x000CD484
		private void AdjustActionManager()
		{
			ActionManager actionManager = GameController.GameManager.actionManager;
			if (GameController.GameManager.LastEncounterCard == null)
			{
				actionManager.SetActionProxy(this.matPlayer);
			}
			else
			{
				actionManager.SetActionProxy(this.encounterCardPresenter);
			}
			actionManager.PreparePreviousAction();
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000CF2C8 File Offset: 0x000CD4C8
		private void AdjustActionPresenters()
		{
			ActionManager actionManager = GameController.GameManager.actionManager;
			if (actionManager.GetLastSelectedGainAction() is GainMove)
			{
				GainMove gainMove = actionManager.GetLastSelectedGainAction() as GainMove;
				if (!((MovePresenter)HumanInputHandler.Instance.movePresenter).HaveAction())
				{
					((MovePresenter)HumanInputHandler.Instance.movePresenter).SetActionOnLoad(gainMove);
				}
				for (int i = (int)gainMove.MovesLeft; i < (int)gainMove.Amount; i++)
				{
					((MovePresenter)HumanInputHandler.Instance.movePresenter).UpdateMoveTile(i);
				}
			}
			if (GameController.GameManager.LastEncounterCard != null)
			{
				int previousActionId = GameController.GameManager.LastEncounterCard.GetPreviousActionId();
				if (previousActionId != -1)
				{
					this.encounterCardPresenter.GrayOutOption(previousActionId);
					this.encounterCardPresenter.DisableOption(previousActionId);
				}
				int currentActionId = GameController.GameManager.LastEncounterCard.GetCurrentActionId();
				if (currentActionId != -1)
				{
					this.encounterCardPresenter.DisableAllOptions();
					this.encounterCardPresenter.GrayOutOption(currentActionId);
				}
			}
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x000CF3B8 File Offset: 0x000CD5B8
		private void AdjustAttackerCombatPresenters()
		{
			switch (GameController.GameManager.combatManager.GetActualStage())
			{
			case CombatStage.SelectingBattlefield:
				GameController.HexGetFocused = null;
				GameController.GameManager.combatManager.SwitchToNextStage();
				return;
			case CombatStage.Diversion:
				GameController.GameManager.combatManager.ReshowDiversionPanelIfAble();
				return;
			case CombatStage.Preparation:
				GameController.GameManager.combatManager.ReshowPreparationPanelIfAble();
				return;
			case CombatStage.DeterminatingTheWinner:
			case CombatStage.CombatResovled:
				break;
			case CombatStage.EndingTheBattle:
				GameController.GameManager.combatManager.SwitchToNextStage();
				break;
			default:
				return;
			}
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x000CF43C File Offset: 0x000CD63C
		private void AdjustDefenderCombatPresenters()
		{
			switch (GameController.GameManager.combatManager.GetActualStage())
			{
			case CombatStage.SelectingBattlefield:
			case CombatStage.DeterminatingTheWinner:
			case CombatStage.CombatResovled:
				break;
			case CombatStage.Diversion:
				GameController.GameManager.combatManager.ReshowDiversionPanelIfAble();
				return;
			case CombatStage.Preparation:
				GameController.GameManager.combatManager.ReshowPreparationPanelIfAble();
				return;
			case CombatStage.EndingTheBattle:
				GameController.GameManager.combatManager.ReshowDefenderEndingPanel();
				break;
			default:
				return;
			}
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x000CF4A8 File Offset: 0x000CD6A8
		public void LoadGame(string filename)
		{
			AnalyticsEventLogger.Instance.LogMatchStop(EndReasons.game_load);
			this.GameIsLoaded = true;
			GameController.gameFromSave = true;
			this.ClearOnLoad();
			GameController.GameManager.actionManager.BreakSectionAction(false);
			GameSavesManager.LoadGame(filename);
			this.ResetButtonsAndContextAtLoad();
			this.Init(false);
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.load);
			if (ActionLogPresenter.Instance != null)
			{
				ActionLogPresenter.Instance.Clear();
			}
			base.StartCoroutine(this.AfterMapLoad(true, false));
			this.menu.GetComponent<GameMenu>().ShowMenu(false);
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x0003E41B File Offset: 0x0003C61B
		public void PrepareBeforeUndo()
		{
			this.GameIsLoaded = true;
			GameController.gameFromSave = true;
			this.ClearOnLoad();
			GameController.GameManager.actionManager.BreakSectionAction(true);
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x000CF534 File Offset: 0x000CD734
		public void AfterUndo()
		{
			GameController.GameManager.actionLog.ClearAwaitingPayActions();
			HumanInputHandler.Instance.Clear();
			this.UpdateFactionInfo();
			this.ResetButtonsAndContextAtLoad();
			this.matFaction.ClearHintStories();
			foreach (MatPlayerSectionPresenter matPlayerSectionPresenter in this.matPlayer.matSection)
			{
				matPlayerSectionPresenter.HideAssist(false);
			}
			this.Init(true);
			WorldSFXManager.OnUndo();
			MusicManager.Instance.OnUndo();
			this.endGameSequencePresenter.Reset();
			this.combatPresenter.AddDelegate();
			this.matFaction.combatCardsPresenter.SetCards(GameController.GameManager.PlayerCurrent.combatCards, null);
			ShowEnemyMoves.Instance.OnUndo();
			base.StartCoroutine(this.AfterMapLoad(true, true));
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x000CF620 File Offset: 0x000CD820
		private void ClearOnLoad()
		{
			DOTween.KillAll(true);
			if (SceneManager.GetActiveScene().buildIndex == 1)
			{
				ShowEnemyMoves.Instance.ClearOnLoad();
			}
			if (this.matFaction != null)
			{
				this.matFaction.ClearHintStories();
			}
			((MovePresenter)HumanInputHandler.Instance.movePresenter).ClearLastClickedUnit();
			(this.gameBoardPresenter as FlatWorld).SetCollidersEnabledOnUnits(true, null, null);
			this.ClearMarkers();
			if (ActionLogInterpreter.IsSupported)
			{
				SingletonMono<ActionLogInterpreter>.Instance.Clear();
			}
			this.tokensController.Clear();
			this.ClearUnits();
			this.factoryCardPresenter.Clear();
			GameController.GameManager.combatManager.Clear();
			if (ActionLogPresenter.Instance != null)
			{
				ActionLogPresenter.Instance.Clear();
			}
			this.combatPresenter.Clear();
			((MovePresenter)HumanInputHandler.Instance.movePresenter).Clear();
			((GainWorkerPresenter)HumanInputHandler.Instance.gainWorkerPresenter).Clear();
			this.dragAndDropPanel.Clear();
			this.MinimizePlayerMat();
			this.endGameSequencePresenter.Reset();
			GameController.GameManager.actionManager.Clear();
			if (PlatformManager.IsMobile)
			{
				if (MobileChat.IsSupported)
				{
					SingletonMono<MobileChat>.Instance.ClearChat();
				}
			}
			else
			{
				this.chat.ClearChat();
			}
			this.AIActionInterrupt = false;
			this.OnDestroy();
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x0003E440 File Offset: 0x0003C640
		private void ClearMarkers()
		{
			(this.gameBoardPresenter as FlatWorld).ClearMarkers();
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x0003E452 File Offset: 0x0003C652
		private void ClearUnits()
		{
			(this.gameBoardPresenter as FlatWorld).ClearUnitPresenters();
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x000CF778 File Offset: 0x000CD978
		private void CherryPickUpdateAfterLoadGame()
		{
			this.AIActionInterrupt = false;
			this.matPlayer.AfterLoadUpdate();
			if (GameController.GameManager.PlayerCurrent.currentMatSection != -1)
			{
				this.EndTurnButtonEnable();
			}
			else
			{
				this.EndTurnButtonDisable();
			}
			if (!this.isUndoLoad)
			{
				MusicManager.Instance.InitGameMusic();
			}
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x000CF7CC File Offset: 0x000CD9CC
		public void AfterGameSynchronize()
		{
			if (this.factoryCardPresenter.gameObject.activeInHierarchy)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
				this.factoryCardPresenter.gameObject.SetActive(false);
				this.factoryCardPresenter.Clear();
			}
			this.objectiveCardPresenter.Close();
			this.encounterCardPresenter.encounterCardSmall.transform.parent.gameObject.SetActive(false);
			this.encounterCardPresenter.gameObject.SetActive(false);
			GameController.GameManager.PlayerCurrent.wonBattle = false;
			this.playersFactions.UpdateStatus();
			GameController.FactionInfo factionInfo = GameController.factionInfo[GameController.GameManager.PlayerOwner.matFaction.faction];
			this.playerStats.UpdateAllStats(GameController.GameManager.PlayerOwner, factionInfo.logo);
			this.EndTurnButtonReset();
			this.UpdateTurnInfo(GameController.GameManager.PlayerCurrent);
			this.UpdateTurnCounterText();
			this.LoadAutomaticGainPreferences();
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x000CF8D0 File Offset: 0x000CDAD0
		public void LoadAutomaticGainPreferences()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				Player playerOwner = GameController.GameManager.PlayerOwner;
				playerOwner.automaticGain[GainType.Power] = PlayerPrefs.GetInt("AutoGainPower", 1) == 1;
				playerOwner.automaticGain[GainType.Coin] = PlayerPrefs.GetInt("AutoGainCoin", 1) == 1;
				playerOwner.automaticGain[GainType.Popularity] = PlayerPrefs.GetInt("AutoGainPopularity", 1) == 1;
				playerOwner.automaticGain[GainType.CombatCard] = PlayerPrefs.GetInt("AutoGainAmmo", 1) == 1;
			}
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x0003E464 File Offset: 0x0003C664
		public void SaveGame()
		{
			if (GameController.GameManager.GameFinished)
			{
				return;
			}
			GameSavesManager.SaveGame(GameSavesManager.GetAutomaticSaveSlotId(), ScriptLocalization.Get("GameScene/AutomaticSave"), GameController.Game.GetGameId());
			this.undoController.OnAutosave();
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x000CF95C File Offset: 0x000CDB5C
		public static void CreateSaveFolderIfNotPresent()
		{
			string text = Path.Combine(Application.persistentDataPath, "Saves");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x000CF988 File Offset: 0x000CDB88
		public void MenuOpening()
		{
			if (this.cameraMovementEffects.presenationFinished)
			{
				if (!this.ui.enabled)
				{
					this.ui.enabled = true;
					return;
				}
				if (!this.menu.GetComponent<GameMenu>().LoadSaveWindow.activeInHierarchy && !this.menu.GetComponent<GameMenu>().exitGameDialog.activeInHierarchy)
				{
					AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.esc);
					this.menu.GetComponent<GameMenu>().ShowMenu(!this.menu.gameObject.activeSelf);
					return;
				}
				if (this.menu.GetComponent<GameMenu>().exitGameDialog.activeInHierarchy)
				{
					AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.esc);
					this.menu.GetComponent<GameMenu>().CloseExitGameDialog();
				}
			}
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x0003E49C File Offset: 0x0003C69C
		private IEnumerator PopupWindowsBeforeNextTurnDelayed()
		{
			yield return new WaitForEndOfFrame();
			this.PopupWindowsBeforeNextTurn();
			yield break;
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x000CFA44 File Offset: 0x000CDC44
		public void PopupWindowsBeforeNextTurn()
		{
			if (GameController.GameManager.PlayerCurrent.currentMatSection == -1)
			{
				this.EndTurnButtonReset();
			}
			else if (this.endTurnCacheMode)
			{
				this.endTurnCached = true;
				this.endTurnButton.interactable = false;
				this.endTurnHintButton.SetActive(true);
			}
			else if (GameController.GameManager.PlayerCurrent.CanCompleteAnyObjective() && OptionsManager.IsWarningsActive())
			{
				if (this.endTurnMessage != null)
				{
					this.endTurnMessage.text = ScriptLocalization.Get("GameScene/WarningObjective");
				}
				else
				{
					this.endTurnWarning.GetComponentInChildren<Text>().text = ScriptLocalization.Get("GameScene/WarningObjective");
				}
				this.endTurnWarning.Show(GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo, delegate
				{
					this.NextTurn();
				}, null);
			}
			else if (GameController.GameManager.PlayerCurrent.bottomActionInProgress || (GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(GameController.GameManager.PlayerCurrent.currentMatSection).ActionDown.CanPlayerPayActions() && !GameController.GameManager.PlayerCurrent.downActionFinished))
			{
				if (GameController.GameManager.PlayerCurrent.bottomActionInProgress && OptionsManager.IsWarningsActive())
				{
					if (this.endTurnMessage != null)
					{
						this.endTurnMessage.text = ScriptLocalization.Get("GameScene/WarningBottomInProgress");
					}
					else
					{
						this.endTurnWarning.GetComponentInChildren<Text>().text = ScriptLocalization.Get("GameScene/WarningBottomInProgress");
					}
					this.endTurnWarning.Show(GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo, delegate
					{
						this.NextTurn();
					}, null);
				}
				else if (GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(GameController.GameManager.PlayerCurrent.currentMatSection).ActionDown.CanPlayerGainFromActions() && GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(GameController.GameManager.PlayerCurrent.currentMatSection).ActionDown.CanPlayerPayActions() && OptionsManager.IsWarningsActive())
				{
					if (this.endTurnMessage != null)
					{
						this.endTurnMessage.text = ScriptLocalization.Get("GameScene/WarningBottomActive");
					}
					else
					{
						this.endTurnWarning.GetComponentInChildren<Text>().text = ScriptLocalization.Get("GameScene/WarningBottomActive");
					}
					this.endTurnWarning.Show(GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo, delegate
					{
						this.NextTurn();
					}, null);
				}
				else
				{
					this.NextTurn();
				}
			}
			else
			{
				this.NextTurn();
			}
			WorldSFXManager.PlaySound(SoundEnum.EndOfTurnButtonClick, AudioSourceType.Buttons);
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x000CFD24 File Offset: 0x000CDF24
		public void NextTurn()
		{
			if (PlatformManager.IsMobile)
			{
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Visible, 0.25f);
			}
			GameController.Instance.matPlayer.ShowSectionMatActionSelection(false);
			if (GameController.GameManager.IsHotSeat && GameController.GameManager.GetPlayersWithoutAICount() > 1)
			{
				this.HideHumanPlayerInfo();
			}
			if (PlatformManager.IsStandalone)
			{
				this.panelInfo.FocusAllObjectiveCards(false);
			}
			else
			{
				SingletonMono<TopMenuPanelsManager>.Instance.GetTopMenuObjectivesPresenter().FocusAllObjectiveCards(false);
			}
			this.FinishCurrentAction();
			base.StartCoroutine(this.NextTurnCoroutine());
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x0003E4AB File Offset: 0x0003C6AB
		public void HideHumanPlayerInfo()
		{
			this.matFaction.combatCardsPresenter.HideCombatCards();
			if (this.panelInfo != null)
			{
				this.panelInfo.HideObjectiveCardPreviewTitles();
			}
			if (GameController.GameManager.SpectatorMode)
			{
				this.DisableSecretInfoInSpectatorMode();
			}
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000CFDB0 File Offset: 0x000CDFB0
		private void DisableSecretInfoInSpectatorMode()
		{
			if (PlatformManager.IsStandalone)
			{
				this.panelInfo.DisableObjectiveCardsTab();
				this.chatButton.SetActive(false);
			}
			else
			{
				this.matFaction.combatCardsPresenter.showCombatCardsButtonImage.GetComponent<Button>().interactable = false;
				SingletonMono<TopMenuPanelsManager>.Instance.DisableObjectiveCardsTabInteraction();
			}
			this.EndTurnButtonDisable();
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x0003E4E8 File Offset: 0x0003C6E8
		public void ShowHumanPlayerInfo()
		{
			this.matFaction.combatCardsPresenter.ShowCombatCards();
			this.panelInfo.ShowObjectiveCardPreviewTitles();
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x0003E505 File Offset: 0x0003C705
		public void ShowCombatCards()
		{
			this.matFaction.combatCardsPresenter.ShowCombatCards();
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x0003E517 File Offset: 0x0003C717
		public void HideCombatCards()
		{
			this.matFaction.combatCardsPresenter.HideCombatCards();
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x0003E529 File Offset: 0x0003C729
		public bool AmmoCardsInvisible()
		{
			return this.matFaction.combatCardsPresenter.AmmoCardsInvisible();
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x000CFE08 File Offset: 0x000CE008
		public void NextTurnMulti()
		{
			if ((GameController.GameManager.IsMyTurn() || !GameController.GameManager.IsMultiplayer) && (this.encounterCardPresenter.gameObject.activeInHierarchy || (this.encounterCardPresenter.encounterCardSmall != null && this.encounterCardPresenter.encounterCardSmall.gameObject.activeInHierarchy)))
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
				this.encounterCardPresenter.Close();
			}
			if (GameController.GameManager.IsMultiplayer && GameController.GameManager.IsMyTurn())
			{
				this.matPlayer.isPreview = true;
				this.matPlayer.UpdateMat(GameController.GameManager.PlayerOwner, false);
				this.matPlayer.isPreview = false;
				this.matFaction.ClearHintStories();
				foreach (MatPlayerSectionPresenter matPlayerSectionPresenter in this.matPlayer.matSection)
				{
					matPlayerSectionPresenter.DisableActionsAndSection(GameController.GameManager.PlayerCurrent.matFaction.DidPlayerUsedMatLastTurn(GameController.GameManager.PlayerMaster.currentMatSection, matPlayerSectionPresenter.sectionID) ? (-1) : 0, true);
					matPlayerSectionPresenter.SetSectionCooldown(false, false);
					if (GameController.GameManager.PlayerCurrent.matFaction.DidPlayerUsedMatLastTurn(GameController.GameManager.PlayerCurrent.currentMatSection, matPlayerSectionPresenter.sectionID))
					{
						matPlayerSectionPresenter.SetSectionCooldown(true, false);
					}
				}
			}
			if (this.factoryCardPresenter.gameObject.activeInHierarchy)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
				this.factoryCardPresenter.gameObject.SetActive(false);
				this.factoryCardPresenter.Clear();
			}
			this.objectiveCardPresenter.Close();
			if (this.encounterCardPresenter.encounterCardSmall != null)
			{
				this.encounterCardPresenter.encounterCardSmall.transform.parent.gameObject.SetActive(false);
			}
			this.encounterCardPresenter.gameObject.SetActive(false);
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				if (player.character.position.hasEncounter)
				{
					player.character.position.encounterAnimated = false;
					player.character.position.encounterTaken = true;
					player.character.position.encounterUsed = true;
				}
			}
			GameController.GameManager.NextTurn();
			this.TurnCounter.text = ScriptLocalization.Get("GameScene/Round") + " " + (GameController.GameManager.TurnCount + 1).ToString();
			this.waitInfoEncounter.SetActive(false);
			this.waitInfoFactory.SetActive(false);
			this.EndTurnButtonReset();
			this.playersFactions.UpdateStatus();
			this.UpdateTurnInfo(GameController.GameManager.PlayerCurrent);
			this.HandleCapitalIcon();
			this.PlayNewTurnSound();
			if (GameController.AfterEndTurnAIAndPlayer != null)
			{
				GameController.AfterEndTurnAIAndPlayer();
			}
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x0003E53B File Offset: 0x0003C73B
		private IEnumerator NextTurnCoroutine()
		{
			this.playersFactions.UpdateStatus();
			this.gameBoardPresenter.UpdateBoard(false, true);
			if ((GameController.GameManager.IsMyTurn() || !GameController.GameManager.IsMultiplayer) && (this.encounterCardPresenter.gameObject.activeInHierarchy || (this.encounterCardPresenter.encounterCardSmall != null && this.encounterCardPresenter.encounterCardSmall.gameObject.activeInHierarchy)))
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
				this.encounterCardPresenter.Close();
			}
			if (GameController.GameManager.IsMultiplayer && GameController.GameManager.IsMyTurn())
			{
				GameController.GameManager.actionManager.BreakSectionAction(false);
				this.matPlayer.isPreview = true;
				this.matPlayer.UpdateMat(GameController.GameManager.PlayerOwner, true);
				this.matPlayer.isPreview = false;
				this.matFaction.ClearHintStories();
				foreach (MatPlayerSectionPresenter matPlayerSectionPresenter in this.matPlayer.matSection)
				{
					matPlayerSectionPresenter.DisableActionsAndSection(GameController.GameManager.PlayerCurrent.matFaction.DidPlayerUsedMatLastTurn(GameController.GameManager.PlayerMaster.currentMatSection, matPlayerSectionPresenter.sectionID) ? (-1) : 0, true);
					matPlayerSectionPresenter.SetSectionCooldown(false, false);
					if (GameController.GameManager.PlayerCurrent.matFaction.DidPlayerUsedMatLastTurn(GameController.GameManager.PlayerCurrent.currentMatSection, matPlayerSectionPresenter.sectionID))
					{
						matPlayerSectionPresenter.SetSectionCooldown(true, false);
					}
				}
				PlayerPrefs.SetInt("PlayerClock", MultiplayerController.Instance.GetOwnerPlayer.PlayerClock);
			}
			if (!GameController.GameManager.IsMultiplayer)
			{
				GameController.GameManager.actionManager.BreakSectionAction(false);
				this.matFaction.ClearHintStories();
				this.factionCamera[GameController.GameManager.PlayerCurrent.matFaction.faction] = this.cameraControler.Serialize();
				if (!GameController.GameManager.IsAIHotSeat)
				{
					foreach (Toggle2 toggle in this.tabToggles)
					{
						if (toggle.isOn)
						{
							toggle.isOn = false;
						}
					}
				}
				if (!GameController.GameManager.PlayerCurrent.IsHuman)
				{
					goto IL_0389;
				}
				using (List<MatPlayerSectionPresenter>.Enumerator enumerator = this.matPlayer.matSection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MatPlayerSectionPresenter matPlayerSectionPresenter2 = enumerator.Current;
						matPlayerSectionPresenter2.DisableActionsAndSection(GameController.GameManager.PlayerCurrent.matFaction.DidPlayerUsedMatLastTurn(GameController.GameManager.PlayerCurrent.currentMatSection, matPlayerSectionPresenter2.sectionID) ? (-1) : 0, true);
						matPlayerSectionPresenter2.SetSectionCooldown(false, false);
						if (GameController.GameManager.PlayerCurrent.matFaction.DidPlayerUsedMatLastTurn(GameController.GameManager.PlayerCurrent.currentMatSection, matPlayerSectionPresenter2.sectionID))
						{
							matPlayerSectionPresenter2.SetSectionCooldown(true, false);
						}
					}
					goto IL_0389;
				}
			}
			if (this.factoryCardPresenter.gameObject.activeInHierarchy)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
				this.factoryCardPresenter.gameObject.SetActive(false);
				this.factoryCardPresenter.Clear();
			}
			this.objectiveCardPresenter.Close();
			if (PlatformManager.IsMobile)
			{
				ShowEnemyMoves.Instance.ResumeAnimationsSpeed();
			}
			IL_0389:
			if (this.encounterCardPresenter.encounterCardSmall != null)
			{
				this.encounterCardPresenter.encounterCardSmall.transform.parent.gameObject.SetActive(false);
			}
			this.encounterCardPresenter.gameObject.SetActive(false);
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				if (player.character.position.hasEncounter)
				{
					player.character.position.encounterAnimated = false;
					player.character.position.encounterTaken = true;
					player.character.position.encounterUsed = true;
				}
			}
			if (!GameController.GameManager.GameFinished)
			{
				this.EndTurnButtonReset();
				this.undoController.OnEndTurn();
				GameController.GameManager.NextTurn();
				this.TurnCounter.text = ScriptLocalization.Get("GameScene/Round") + " " + (GameController.GameManager.TurnCount + 1).ToString();
				if (GameController.GameManager.IsMultiplayer)
				{
					this.UpdateTurnInfo(GameController.GameManager.PlayerCurrent);
				}
				if (!GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerCurrent.IsHuman)
				{
					this.autoGainPanel.UpdateToggles();
				}
				this.waitInfoEncounter.SetActive(false);
				this.waitInfoFactory.SetActive(false);
				if (!GameController.GameManager.IsMultiplayer && !GameController.GameManager.PlayerCurrent.IsHuman)
				{
					yield return base.StartCoroutine(this.AITurn());
				}
				if (GameController.GameManager.PlayerCurrent.IsHuman)
				{
					this.EndTurnButtonReset();
					this.playersFactions.UpdateStatus();
					this.UpdateTurnInfo(GameController.GameManager.PlayerCurrent);
					if (!GameController.GameManager.IsMultiplayer)
					{
						this.undoController.TriggerUnlimitedUndoInteractivityChange(true);
					}
				}
				if (GameController.AfterEndTurnAIAndPlayer != null)
				{
					GameController.AfterEndTurnAIAndPlayer();
				}
				this.HandleCapitalIcon();
			}
			if (GameController.GameManager.IsHotSeat && GameController.GameManager.GetPlayersWithoutAICount() > 1 && GameController.GameManager.PlayerCurrent.IsHuman && this.panelInfo != null)
			{
				this.panelInfo.MechPanelUpgrade();
			}
			if (GameController.OnEndTurnClick != null)
			{
				GameController.OnEndTurnClick();
			}
			this.PlayNewTurnSound();
			yield break;
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x0003E54A File Offset: 0x0003C74A
		private void FinishCurrentAction()
		{
			if (HumanInputHandler.Instance.GetSelectedPresenter() != null)
			{
				HumanInputHandler.Instance.GetSelectedPresenter().OnEndActionConfirmClicked();
			}
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x000D0134 File Offset: 0x000CE334
		private void PlayNewTurnSound()
		{
			if (!GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerCurrent.IsHuman)
			{
				WorldSFXManager.PlaySound(SoundEnum.RoundStart1, AudioSourceType.StartTurnTheme);
			}
			if (GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerOwner == GameController.GameManager.PlayerCurrent)
			{
				WorldSFXManager.PlaySound(SoundEnum.RoundStart1, AudioSourceType.StartTurnTheme);
			}
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x000D0190 File Offset: 0x000CE390
		private void HandleCapitalIcon()
		{
			if (GameController.GameManager.IsCampaign)
			{
				return;
			}
			foreach (Player player in GameController.GameManager.GetPlayers())
			{
				FlatGameHexPresenter flatGameHexPresenter = this.GetGameHexPresenter(player.GetCapital()) as FlatGameHexPresenter;
				if (flatGameHexPresenter.hexObject.Find("Hex Type Icon") == null)
				{
					return;
				}
				flatGameHexPresenter.hexObject.Find("Hex Type Icon").gameObject.GetComponent<SpriteRenderer>().sprite = this.capitalsIcons[2];
			}
			if (!GameController.GameManager.IsMultiplayer)
			{
				FlatGameHexPresenter flatGameHexPresenter2 = this.GetGameHexPresenter(GameController.GameManager.PlayerCurrent.GetCapital()) as FlatGameHexPresenter;
				if (flatGameHexPresenter2.hexObject.Find("Hex Type Icon") == null)
				{
					Debug.LogWarning("Missing GameObject in hex hierarchy. Probably it has changed and this section need to be reimplement.");
					return;
				}
				flatGameHexPresenter2.hexObject.Find("Hex Type Icon").gameObject.GetComponent<SpriteRenderer>().sprite = this.capitalsIcons[1];
				return;
			}
			else
			{
				Player playerOwner = GameController.GameManager.PlayerOwner;
				FlatGameHexPresenter flatGameHexPresenter3 = this.GetGameHexPresenter(playerOwner.GetCapital()) as FlatGameHexPresenter;
				if (flatGameHexPresenter3.hexObject.Find("Hex Type Icon") == null)
				{
					Debug.LogWarning("Missing GameObject in hex hierarchy. Probably it has changed and this section need to be reimplement.");
					return;
				}
				SpriteRenderer component = flatGameHexPresenter3.hexObject.Find("Hex Type Icon").gameObject.GetComponent<SpriteRenderer>();
				if (playerOwner == GameController.GameManager.PlayerCurrent)
				{
					component.sprite = this.capitalsIcons[1];
					return;
				}
				component.sprite = this.capitalsIcons[0];
			}
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x0003E56D File Offset: 0x0003C76D
		public IEnumerator AITurn()
		{
			if (!GameController.GameManager.PlayerCurrent.IsHuman)
			{
				this.combatPresenter.RemoveDelegater();
				this.TurnOffGameEndedListerner();
				Player playerCurrent = GameController.GameManager.PlayerCurrent;
				if (!GameController.GameManager.showEnemyActions)
				{
					this.ShowTurnInfoNoInput(GameController.GameManager.PlayerCurrent);
				}
				GameController.GameManager.PlayerCurrent.aiPlayer.PlayTurn();
				GameController.GameManager.CheckStars();
				EndTurnActionInfo endTurnActionInfo = new EndTurnActionInfo();
				endTurnActionInfo.actionOwner = GameController.GameManager.PlayerCurrent.matFaction.faction;
				endTurnActionInfo.owner = GameController.GameManager.PlayerCurrent.matFaction.faction;
				ShowEnemyMoves.Instance.OnBotEndTurn(endTurnActionInfo);
				if (PlatformManager.IsMobile)
				{
					ShowEnemyMoves.Instance.ResumeAnimationsSpeed();
				}
				this.playersFactions.UpdateStatus();
				this.gameBoardPresenter.UpdateBoard(false, false);
				if (GameController.GameManager.combatManager.IsPlayerInCombat())
				{
					this.AIActionInterrupt = true;
				}
				else if (!ShowEnemyMoves.Instance.MoreAnimations() && !GameController.GameManager.PlayerCurrent.IsHuman)
				{
					yield return base.StartCoroutine(this.NextTurnCoroutine());
				}
			}
			yield break;
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x0003E57C File Offset: 0x0003C77C
		public void TurnOffGameEndedListerner()
		{
			GameController.GameManager.GameHasEnded -= this.OnGameEnded;
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x0003E594 File Offset: 0x0003C794
		public void AddGameEndedListerner()
		{
			this.TurnOffGameEndedListerner();
			GameController.GameManager.GameHasEnded += this.OnGameEnded;
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x0003E5B2 File Offset: 0x0003C7B2
		private void ContinueBotAction()
		{
			if (this.AIActionInterrupt && !ShowEnemyMoves.Instance.MoreAnimations())
			{
				this.AIActionInterrupt = false;
				this.playersFactions.UpdateStatus();
				this.NextTurn();
			}
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x000D0344 File Offset: 0x000CE544
		public void UpdateTurnInfo(Player player)
		{
			if (this.cameraMovementEffects.presenationFinished && !GameController.GameManager.IsCampaign)
			{
				this.ShowTurnInfo();
			}
			if (GameController.GameManager.IsCampaign)
			{
				this.turnInfoPanel.DeactivateTurnInfoPanel();
			}
			GameController.Instance.matPlayer.ShowSectionMatActionSelection(false);
			this.UpdateStats(player, false, true);
			this.gameBoardPresenter.UpdateBoard(true, true);
			if (!GameController.GameManager.IsMultiplayer)
			{
				if (!GameController.GameManager.GameFinished)
				{
					if (!GameController.GameManager.IsCampaign && (!player.character.position.hasEncounter || (player.character.position.hasEncounter && player.character.position.encounterTaken)))
					{
						this.SaveGame();
					}
					if (GameController.GameManager.PlayerCurrent.IsHuman && GameController.GameManager.showEnemyActions)
					{
						if (AnimateCamera.Instance != null)
						{
							AnimateCamera.Instance.StopAnimating();
						}
						if (!GameController.GameManager.IsCampaign || GameController.GameManager.missionId != 3)
						{
							ShowEnemyMoves.Instance.CamToHexOnTurnStart(new Vector2(this.factionCamera[GameController.GameManager.PlayerMaster.matFaction.faction].x, this.factionCamera[GameController.GameManager.PlayerMaster.matFaction.faction].z), GameController.GameManager.PlayerMaster.matFaction.faction);
						}
					}
					if (!GameController.GameManager.showEnemyActions)
					{
						if (AnimateCamera.Instance != null)
						{
							AnimateCamera.Instance.StopAnimating();
						}
						this.cameraControler.Load(this.factionCamera[GameController.GameManager.PlayerMaster.matFaction.faction]);
					}
				}
			}
			else
			{
				if (GameController.GameManager.PlayerCurrent != GameController.GameManager.PlayerOwner)
				{
					this.endTurnButtonText.text = ScriptLocalization.Get("GameScene/EnemyTurn").ToUpper();
					if (PlatformManager.IsMobile)
					{
						this.endTurnHintButton.SetActive(false);
					}
				}
				else
				{
					this.endTurnButtonText.text = ScriptLocalization.Get("GameScene/EndTurn").ToUpper();
				}
				if (GameController.GameManager.PlayerCurrent == GameController.GameManager.PlayerOwner && GameController.GameManager.showEnemyActions && OptionsManager.IsCameraAnimationsActive())
				{
					if (AnimateCamera.Instance != null)
					{
						AnimateCamera.Instance.StopAnimating();
					}
					ShowEnemyMoves.Instance.CamToHexOnTurnStart(new Vector2(this.factionCamera[GameController.GameManager.PlayerMaster.matFaction.faction].x, this.factionCamera[GameController.GameManager.PlayerMaster.matFaction.faction].z), GameController.GameManager.PlayerCurrent.matFaction.faction);
				}
			}
			if (GameController.GameManager.IsMultiplayer && GameController.GameManager.IsMyTurn() && GameController.GameManager.TestingMode)
			{
				GameController.GameManager.PlayerCurrent.aiPlayer.PlayTurn();
				this.EndTurnButtonDisable();
			}
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x000D0678 File Offset: 0x000CE878
		public void OnLeftTabToggleClicked(int id)
		{
			WorldSFXManager.PlaySound(SoundEnum.GuiUpperBeltClick, AudioSourceType.Buttons);
			if (GameController.OnTopTabClicked != null)
			{
				GameController.OnTopTabClicked(id);
			}
			if (this.tabToggles[id].isOn)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.tab_enlist_toggle + id);
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.tab_enlist + id, Screens.in_game, Contexts.ingame);
				return;
			}
			if (this.tabToggles.FindIndex((Toggle2 toggle) => toggle.isOn) == -1)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.tab_enlist_toggle + id);
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Screens.tab_enlist + id, Contexts.ingame);
			}
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x0003E5E0 File Offset: 0x0003C7E0
		public void OnLeftTabClosed(int id)
		{
			WorldSFXManager.PlaySound(SoundEnum.GuiUpperBeltClick, AudioSourceType.Buttons);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Screens.tab_enlist + id, Contexts.ingame);
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x000D0714 File Offset: 0x000CE914
		private static FactionBasicInfo GetFactionBasicInfo(Faction faction)
		{
			return new FactionBasicInfo
			{
				faction = faction,
				factionName = ScriptLocalization.Get(string.Format("FactionMat/{0}", faction.ToString())),
				abilityName = ScriptLocalization.Get(string.Format("FactionMat/{0}FactionAbilityName", faction.ToString())),
				abilityDescription = ScriptLocalization.Get(string.Format("FactionMat/{0}FactionAbilityDescription", faction.ToString())),
				mechAbilityTitles = new string[]
				{
					ScriptLocalization.Get(string.Format("FactionMat/{0}MechAbilityTitle1", faction.ToString())),
					ScriptLocalization.Get(string.Format("FactionMat/{0}MechAbilityTitle2", faction.ToString())),
					ScriptLocalization.Get(string.Format("FactionMat/{0}MechAbilityTitle3", faction.ToString())),
					ScriptLocalization.Get(string.Format("FactionMat/{0}MechAbilityTitle4", faction.ToString()))
				},
				mechAbilityDescriptions = new string[]
				{
					ScriptLocalization.Get(string.Format("FactionMat/{0}MechAbilityDescription1", faction.ToString())),
					ScriptLocalization.Get(string.Format("FactionMat/{0}MechAbilityDescription2", faction.ToString())),
					ScriptLocalization.Get(string.Format("FactionMat/{0}MechAbilityDescription3", faction.ToString())),
					ScriptLocalization.Get(string.Format("FactionMat/{0}MechAbilityDescription4", faction.ToString()))
				}
			};
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x0003E601 File Offset: 0x0003C801
		private void UpdateFactionInfo()
		{
			GameController.GameManager.factionBasicInfo = GameController.CreateArrayOfInfo();
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x000D08A8 File Offset: 0x000CEAA8
		public static Dictionary<Faction, FactionBasicInfo> CreateArrayOfInfo()
		{
			return new Dictionary<Faction, FactionBasicInfo>
			{
				{
					Faction.Polania,
					GameController.GetFactionBasicInfo(Faction.Polania)
				},
				{
					Faction.Albion,
					GameController.GetFactionBasicInfo(Faction.Albion)
				},
				{
					Faction.Nordic,
					GameController.GetFactionBasicInfo(Faction.Nordic)
				},
				{
					Faction.Rusviet,
					GameController.GetFactionBasicInfo(Faction.Rusviet)
				},
				{
					Faction.Togawa,
					GameController.GetFactionBasicInfo(Faction.Togawa)
				},
				{
					Faction.Crimea,
					GameController.GetFactionBasicInfo(Faction.Crimea)
				},
				{
					Faction.Saxony,
					GameController.GetFactionBasicInfo(Faction.Saxony)
				}
			};
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x000D0918 File Offset: 0x000CEB18
		private void ResetButtonsAndContextAtLoad()
		{
			this.matPlayer.ShowSectionMatActionReloading(false);
			this.matPlayer.ShowSectionMatActionSelection(false);
			this.matPlayer.DisableHints();
			this.matPlayer.ResetActionSectionButtonsBackgrounds();
			foreach (Toggle2 toggle in this.tabToggles)
			{
				if (toggle.isOn)
				{
					toggle.isOn = false;
				}
			}
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x0003E612 File Offset: 0x0003C812
		public void SetDragAndDrop(bool enabled)
		{
			if (this.DragAndDrop != enabled)
			{
				this.DragAndDrop = enabled;
				this.dragAndDropPanel.OnDragAndDropStateChanged(enabled);
				(HumanInputHandler.Instance.movePresenter as MovePresenter).OnDragAndDropStateChanged(enabled);
				this.combatPresenter.OnDragAndDropStateChanged(enabled);
			}
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x000D09A4 File Offset: 0x000CEBA4
		public void UpdateStats(Player player, bool refreshMatSections = false, bool updateStaticObjects = true)
		{
			this.gameBoardPresenter.UpdateBoard(false, updateStaticObjects);
			if (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && !GameController.GameManager.TestingMode))
			{
				GameController.FactionInfo factionInfo = GameController.factionInfo[player.matFaction.faction];
				this.matFaction.UpdateMat(player, factionInfo, GameController.GameManager.actionManager.GetLastSelectedGainAction() == null);
				this.matPlayer.UpdateMat(player, refreshMatSections);
				if (!PlatformManager.IsStandalone)
				{
					SingletonMono<TopMenuPanelsManager>.Instance.UpdateInfoFromMat(player, factionInfo, GameController.GameManager.actionManager.GetLastSelectedGainAction() == null);
				}
				this.playerStats.UpdateAllStats(player, factionInfo.logo);
				if (PlatformManager.IsStandalone)
				{
					this.panelInfo.UpdatePlayerInfo(factionInfo, GameController.GameManager.factionBasicInfo[player.matFaction.faction], player.objectiveCards, player.combatCards, false);
				}
				else
				{
					SingletonMono<TopMenuPanelsManager>.Instance.UpdatePlayerInfo(factionInfo, GameController.GameManager.factionBasicInfo[player.matFaction.faction], player.objectiveCards, player.combatCards, false);
				}
				GameController.GameManager.CheckObjectiveCards();
			}
			else
			{
				GameController.FactionInfo factionInfo2 = GameController.factionInfo[GameController.GameManager.PlayerOwner.matFaction.faction];
				this.matPlayer.UpdateRecruits(GameController.GameManager.PlayerOwner);
				this.playerStats.UpdateAllStats(GameController.GameManager.PlayerOwner, factionInfo2.logo);
				if (PlatformManager.IsStandalone)
				{
					this.panelInfo.UpdatePlayerInfo(factionInfo2, GameController.GameManager.factionBasicInfo[GameController.GameManager.PlayerOwner.matFaction.faction], GameController.GameManager.PlayerOwner.objectiveCards, GameController.GameManager.PlayerOwner.combatCards, false);
				}
				else
				{
					SingletonMono<TopMenuPanelsManager>.Instance.UpdatePlayerInfo(factionInfo2, GameController.GameManager.factionBasicInfo[GameController.GameManager.PlayerOwner.matFaction.faction], GameController.GameManager.PlayerOwner.objectiveCards, GameController.GameManager.PlayerOwner.combatCards, false);
				}
			}
			this.playersFactions.UpdateStatus();
			if (!GameController.GameManager.IsMultiplayer || !GameController.GameManager.SpectatorMode)
			{
				GameController.GameManager.CheckStars();
				GameController.GameManager.CheckFactory(true);
				GameController.GameManager.CheckEncounter(true);
			}
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x000D0C18 File Offset: 0x000CEE18
		public void UpdateStats(bool refreshMatSections = false, bool updateStaticObjects = true)
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				this.UpdateStats(GameController.GameManager.PlayerOwner, refreshMatSections, updateStaticObjects);
				return;
			}
			if (GameController.GameManager.PlayerCurrent.IsHuman)
			{
				this.UpdateStats(GameController.GameManager.PlayerCurrent, refreshMatSections, updateStaticObjects);
				return;
			}
			Player previousHumanPlayer = GameController.GameManager.GetPreviousHumanPlayer();
			GameController.FactionInfo factionInfo = GameController.factionInfo[previousHumanPlayer.matFaction.faction];
			this.matPlayer.UpdateRecruits(previousHumanPlayer);
			this.playerStats.UpdateAllStats(previousHumanPlayer, factionInfo.logo);
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x000D0CA8 File Offset: 0x000CEEA8
		public void UpdateStatsPresenter()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				this.playerStats.UpdateAllStats(GameController.GameManager.PlayerOwner, GameController.factionInfo[GameController.GameManager.PlayerOwner.matFaction.faction].logo);
				return;
			}
			if (GameController.GameManager.PlayerCurrent.IsHuman)
			{
				this.playerStats.UpdateAllStats(GameController.GameManager.PlayerCurrent, GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo);
				return;
			}
			Player previousHumanPlayer = GameController.GameManager.GetPreviousHumanPlayer();
			GameController.FactionInfo factionInfo = GameController.factionInfo[previousHumanPlayer.matFaction.faction];
			this.playerStats.UpdateAllStats(previousHumanPlayer, factionInfo.logo);
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x0003E651 File Offset: 0x0003C851
		public void ShowTurnInfo()
		{
			this.turnInfoPanel.ActivateTurnInfoPanel();
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x0003E65E File Offset: 0x0003C85E
		public void ShowTurnInfoNoInput()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				this.turnInfoPanel.ActivateTurnInfoPanelNoInput();
				return;
			}
			this.turnInfoPanel.ActivateTurnInfoPanelNoInput();
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x0003E683 File Offset: 0x0003C883
		public void ShowTurnInfoNoInput(Player player)
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				this.turnInfoPanel.ActivateTurnInfoPanelNoInput();
				return;
			}
			this.turnInfoPanel.ActivateTurnInfoPanelNoInput(player);
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x000D0D78 File Offset: 0x000CEF78
		public void OnUpdatePlayerStats()
		{
			if (GameController.GameManager.PlayerCurrent.IsHuman)
			{
				this.playerStats.UpdateAllStats(GameController.GameManager.PlayerCurrent, GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo);
			}
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x0003E6A9 File Offset: 0x0003C8A9
		public void AddMessageToExecute(IExecutableMessage message)
		{
			this.multiplayerActions.Enqueue(message);
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x000D0DD0 File Offset: 0x000CEFD0
		public void OnActionFinished()
		{
			this.gameBoardPresenter.UpdateBoard(false, false);
			Player playerOwner = GameController.GameManager.PlayerOwner;
			if (!GameController.GameManager.IsMyTurn() || !playerOwner.topActionFinished || playerOwner.downActionFinished)
			{
				this.UpdateStats(true, false);
				return;
			}
			TopAction actionTop = playerOwner.matPlayer.GetPlayerMatSection(playerOwner.currentMatSection).ActionTop;
			if (actionTop.GetNumberOfGainActions() < 2 || actionTop.GetGainAction(1).GetGainType() != GainType.CombatCard)
			{
				this.UpdateStats(true, true);
				return;
			}
			this.matPlayer.matSection[playerOwner.currentMatSection].UpdateDownActionColors();
			GameController.FactionInfo factionInfo = GameController.factionInfo[playerOwner.matFaction.faction];
			this.playerStats.UpdateAllStats(playerOwner, factionInfo.logo);
			if (PlatformManager.IsStandalone)
			{
				this.panelInfo.UpdatePlayerInfo(factionInfo, GameController.GameManager.factionBasicInfo[playerOwner.matFaction.faction], playerOwner.objectiveCards, playerOwner.combatCards, false);
				return;
			}
			SingletonMono<TopMenuPanelsManager>.Instance.UpdatePlayerInfo(factionInfo, GameController.GameManager.factionBasicInfo[playerOwner.matFaction.faction], playerOwner.objectiveCards, playerOwner.combatCards, false);
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x0003E6B7 File Offset: 0x0003C8B7
		public void OnTurnEnded()
		{
			this.NextTurnMulti();
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x000D0F14 File Offset: 0x000CF114
		public void OnInputEnabled(bool enabled)
		{
			if (GameController.GameManager.SpectatorMode)
			{
				return;
			}
			if (!GameController.GameManager.GameFinished)
			{
				this.waitInfoRecruit.SetActive(!enabled);
			}
			else
			{
				this.waitInfoRecruit.SetActive(false);
			}
			if (enabled)
			{
				this.endTurnCacheMode = false;
				GameController.GameManager.CheckObjectiveCards();
				this.endTurnHint.enabled = false;
				this.endTurnShade.enabled = false;
				this.endTurnButton.interactable = true;
				this.endTurnHintButton.SetActive(false);
				if (this.endTurnCached && !GameController.GameManager.actionManager.MoreHumanPlayersToGetBonus())
				{
					this.endTurnCached = false;
					base.StartCoroutine(this.PopupWindowsBeforeNextTurnDelayed());
					return;
				}
			}
			else
			{
				if (PlatformManager.IsStandalone)
				{
					this.panelInfo.FocusAllObjectiveCards(false);
				}
				else
				{
					SingletonMono<TopMenuPanelsManager>.Instance.GetTopMenuObjectivesPresenter().FocusAllObjectiveCards(false);
				}
				this.endTurnHintType = GameController.EndTurnHintType.WaitForOpponent;
				this.endTurnCacheMode = true;
			}
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x000D0FFC File Offset: 0x000CF1FC
		public void OnShowFactoryCards()
		{
			if (GameController.GameManager.TestingMode && GameController.GameManager.IsMyTurn())
			{
				GameController.GameManager.PlayerCurrent.aiPlayer.HandleEncounterAndFactory();
				GameController.GameManager.PlayerCurrent.aiPlayer.ContinueLeaverAction();
				return;
			}
			this.factoryCardPresenter.gameObject.SetActive(true);
			this.factoryCardPresenter.SetCards();
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x000D1068 File Offset: 0x000CF268
		public void OnShowEmptyCards(int amount)
		{
			if (GameController.GameManager.TestingMode && GameController.GameManager.IsMyTurn())
			{
				GameController.GameManager.PlayerCurrent.aiPlayer.HandleEncounterAndFactory();
				GameController.GameManager.PlayerCurrent.aiPlayer.ContinueLeaverAction();
				return;
			}
			this.factoryCardPresenter.gameObject.SetActive(true);
			this.factoryCardPresenter.ShowEmptyCards(amount);
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x0003E6BF File Offset: 0x0003C8BF
		public void AfterCardAdded()
		{
			if (GameController.GameManager.IsMyTurn())
			{
				this.AddFactoryCard();
				this.factoryCardPresenter.Clear();
			}
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x0003E6DE File Offset: 0x0003C8DE
		public void OnShowChoosenFactory(int cardIndex, int positionIndex)
		{
			this.factoryCardPresenter.ShowCard(cardIndex, positionIndex);
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x000D10D4 File Offset: 0x000CF2D4
		public void ShowEncounter()
		{
			if (GameController.GameManager.TestingMode && GameController.GameManager.IsMyTurn())
			{
				GameController.GameManager.PlayerCurrent.aiPlayer.HandleEncounterAndFactory();
				GameController.GameManager.PlayerCurrent.aiPlayer.ContinueLeaverAction();
				return;
			}
			this.encounterCardPresenter.SetOptions(GameController.GameManager.LastEncounterCard);
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x0003E6ED File Offset: 0x0003C8ED
		public void ChooseOption(int index)
		{
			this.encounterCardPresenter.GrayOutOption(index);
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x000D1138 File Offset: 0x000CF338
		public void EndEncounter(int x, int y)
		{
			this.gameBoardPresenter.GetGameHexPresenter(x, y).ActivateEncounterEndAnimation();
			GameController.GameManager.gameBoard.hexMap[x, y].encounterTaken = true;
			GameController.GameManager.gameBoard.hexMap[x, y].encounterUsed = true;
			this.encounterCardPresenter.Close();
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0003E6FB File Offset: 0x0003C8FB
		public void ShowObjectiveCard(ObjectiveCard objectiveCard, Player player)
		{
			if (GameController.GameManager.TestingMode)
			{
				return;
			}
			this.UpdateStats(false, true);
			this.objectiveCardPresenter.SetCard(objectiveCard, GameController.factionInfo[player.matFaction.faction].logo);
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x0003E738 File Offset: 0x0003C938
		public void CombatAbilityUsed(AbilityPerk ability)
		{
			this.combatPresenter.SetLastUsedAbility(ability);
			this.combatPresenter.ShowInfoAboutUsedCombatAbility();
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x000D11B0 File Offset: 0x000CF3B0
		public void OnEnemyUnitMove(Unit unit, Dictionary<GameHex, GameHex> possibleMoves)
		{
			UnitPresenter unitPresenter = GameController.GetUnitPresenter(unit);
			if (unitPresenter != null)
			{
				unitPresenter.RunTheMoveAnimation(possibleMoves, false, false, false, null, null);
			}
			this.gameBoardPresenter.UpdateStaticObjects();
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x0003E751 File Offset: 0x0003C951
		public void GameEnded()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				return;
			}
			base.StartCoroutine(this.WaitWithEndGamePresentationUntilAnimationsAreFinished());
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x000D11E4 File Offset: 0x000CF3E4
		public void RunOutOfTime()
		{
			if (GameController.GameManager.GetPlayersWithoutAICount() == 1 && GameController.GameManager.IsMultiplayer && MultiplayerController.Instance.RunOutOfTime)
			{
				this.GameFinishedTimeOut = true;
				GameController.GameManager.GameFinishedState(true);
				GameController.GameManager.GetPlayersWithoutAI().ToList<Player>();
				Player playerOwner = GameController.GameManager.PlayerOwner;
				if (PlatformManager.IsMobile)
				{
					this.endGameSequencePresenter.GetMobileWinnerPanel().GetTopScoreMenuPresenter().UpdateEntries(true);
					this.endGameSequencePresenter.GetMobileWinnerPanel().GetTopScoreMenuPresenter().GetPlayerEndGameStats();
				}
				Debug.Log("Only one human player");
			}
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x0003E76D File Offset: 0x0003C96D
		public IEnumerator WaitWithEndGamePresentationUntilAnimationsAreFinished()
		{
			yield return new WaitForEndOfFrame();
			while (!ShowEnemyMoves.Instance.AllAnimationsFinished())
			{
				yield return new WaitForEndOfFrame();
			}
			this.OnGameEnded();
			yield break;
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x0003E77C File Offset: 0x0003C97C
		public void SynchronizeGame()
		{
			this.AfterGameSynchronize();
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x0003E784 File Offset: 0x0003C984
		public void GameLoaded()
		{
			this.UpdateTurnInfo(GameController.GameManager.PlayerCurrent);
			this.AdjustPresentersAfterLoad();
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x000D1284 File Offset: 0x000CF484
		public void ChooseFirstBattlefield()
		{
			if (GameController.GameManager.combatManager.GetBattlefields().Count > 0)
			{
				int posX = GameController.GameManager.combatManager.GetBattlefields()[0].posX;
				int posY = GameController.GameManager.combatManager.GetBattlefields()[0].posY;
				this.combatPresenter.PlaceSelectedBattlefieldEffect(GameController.GameManager.gameBoard.hexMap[posX, posY]);
			}
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x0003E79C File Offset: 0x0003C99C
		public void OnShowPoints(List<PlayerEndGameStats> stats)
		{
			this.endGameSequencePresenter.EndGameStats = stats;
			if (!GameController.GameManager.GameFinished && !GameController.GameManager.SpectatorMode)
			{
				this.waitForGameEnd = true;
				return;
			}
			this.OnMultiplayerGameEnded();
			this.CheckIfPlayerMasterWonOnline();
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x0003E7D6 File Offset: 0x0003C9D6
		public void OnObjectiveCardHighlight(int index, bool highlight)
		{
			if (PlatformManager.IsStandalone)
			{
				this.panelInfo.FocusObjectiveCard(index, highlight);
				return;
			}
			SingletonMono<TopMenuPanelsManager>.Instance.GetTopMenuObjectivesPresenter().FocusObjectiveCard(index, highlight);
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x0003E7FE File Offset: 0x0003C9FE
		public void OnEncounterEnabled()
		{
			this.ShowEncounterCard();
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x0003E806 File Offset: 0x0003CA06
		public static string GetEncounterDescription(int cardId, int option)
		{
			return ScriptLocalization.Get(string.Concat(new object[] { "Encounters/EncounterDescription", cardId, "-", option }));
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x0003E83A File Offset: 0x0003CA3A
		public static string GetEncounterActionDescription(int cardId, int option)
		{
			return ScriptLocalization.Get(string.Concat(new object[] { "Encounters/EncounterAction", cardId, "-", option }));
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x0003E86E File Offset: 0x0003CA6E
		public void OnFactoryEnabled()
		{
			this.endTurnButton.interactable = false;
			this.endTurnHintButton.SetActive(true);
			this.endTurnHintType = GameController.EndTurnHintType.ChoseFactoryCard;
			this.ShowFactoryCards();
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x0003E895 File Offset: 0x0003CA95
		private void OnEnemyProduce(ProduceEnemyActionInfo enemyProduceInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyProduceActionAnim(enemyProduceInfo);
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x0003E8A2 File Offset: 0x0003CAA2
		private void OnEnemyGainWorker(GainWorkerEnemyActionInfo enemyGainWorkerInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyProduceWorkerSave(enemyGainWorkerInfo);
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x0003E8AF File Offset: 0x0003CAAF
		private void OnEnemyGainWorkerEnds(GainWorkersEndEnemyActionInfo enemyGainWorkersInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyProduceWorkerActionAnim(enemyGainWorkersInfo);
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x0003E8BC File Offset: 0x0003CABC
		private void OnEnemyTrade(TradeEnemyActionInfo enemyTradeInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyTradeActionAnim(enemyTradeInfo);
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x0003E8C9 File Offset: 0x0003CAC9
		private void OnEnemyRecruitBonus(EnlistBonusEnemyActionInfo enlistBonusEnemyInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyGenerateRecruitBonusActionAnim(enlistBonusEnemyInfo);
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x0003E8D6 File Offset: 0x0003CAD6
		private void OnEnemysBonusEnd()
		{
			ShowEnemyMoves.Instance.OnEnemysBonusesEnd();
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0003E8E2 File Offset: 0x0003CAE2
		private void OnEnemyRecruit(EnlistEnemyActionInfo enlistEnemyInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyRecruitActionAnim(enlistEnemyInfo);
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x0003E8EF File Offset: 0x0003CAEF
		private void OnEnemyUpgrade(UpgradeEnemyActionInfo upgradeEnemyInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyUpgradeActionAnim(upgradeEnemyInfo);
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x0003E8FC File Offset: 0x0003CAFC
		private void OnEnemyDeploy(DeployEnemyActionInfo deployEnemyInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyDeployActionAnim(deployEnemyInfo);
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x0003E909 File Offset: 0x0003CB09
		private void OnEnemyBuild(BuildEnemyActionInfo buildEnemyInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyBuildActionAnim(buildEnemyInfo);
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x0003E916 File Offset: 0x0003CB16
		private void OnEnemyPayResource(EnemyPayResourceFromHexInfo payResourcesInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyPayResourcesFromHex(payResourcesInfo);
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x0003E923 File Offset: 0x0003CB23
		private void OnEnemyMove(MoveEnemyActionInfo enemyActionInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyUnitMoveAnim(enemyActionInfo);
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x0003E930 File Offset: 0x0003CB30
		private void OnEnemyLoadResources(LoadResourcesEnemyActionInfo enemyActionInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyLoadResourcesAnim(enemyActionInfo);
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x0003E93D File Offset: 0x0003CB3D
		private void OnEnemyLoadWorker(LoadWorkerActionInfo enemyActionInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyLoadWorkerAnim(enemyActionInfo);
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x0003E94A File Offset: 0x0003CB4A
		private void OnEnemyUnloadWorker(UnloadWorkerActionInfo enemyActionInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyUnloadWorkerAnim(enemyActionInfo);
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x0003E957 File Offset: 0x0003CB57
		private void OnEnemyRetreatMove(MoveRetreatEnemyActionInfo enemyActionInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyRetreatUnitsMoveAnim(enemyActionInfo);
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x0003E964 File Offset: 0x0003CB64
		private void OnEnemyGainTopStats(GainTopStatsEnemyActionInfo enemyActionInfo)
		{
			ShowEnemyMoves.Instance.OnEnemyGainTopStatsAnim(enemyActionInfo);
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x000D1300 File Offset: 0x000CF500
		public void OnGameEnded()
		{
			this.SetControlsAtGameEnd();
			if (!GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsCampaign)
			{
				PlayerPrefs.SetInt(OptionsManager.PREFS_GAME_CLOSED_CORRECT, OptionsManager.BoolToInt(true));
				if (GameController.GameManager.GameFinished)
				{
					GameSavesManager.DeleteGame("ScytheSaveTmp.xml");
				}
			}
			if (!GameController.GameManager.IsCampaign || GameController.GameManager.missionId == 10)
			{
				this.endGameSequencePresenter.EndGameStats = GameController.GameManager.CalculateStats();
				this.endGameSequencePresenter.StartEndGameSequence();
				MusicManager.Instance.StopMusic();
				MusicManager.Instance.PrepareWinnerMusic();
				if (!GameController.GameManager.IsCampaign && !this.StatsSaved)
				{
					int num = 0;
					this.StatsSaved = true;
					foreach (PlayerEndGameStats playerEndGameStats in this.endGameSequencePresenter.EndGameStats)
					{
						num++;
						if (playerEndGameStats.player.IsHuman)
						{
							LocalStats.UpdateLocalPlayerStats(playerEndGameStats.player, playerEndGameStats.totalPoints, num);
						}
					}
				}
				if (!GameController.GameManager.IsCampaign)
				{
					this.CheckIfPlayerMasterWonOffline();
				}
			}
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0003E971 File Offset: 0x0003CB71
		private void CheckIfPlayerMasterWonOffline()
		{
			if (this.endGameSequencePresenter.EndGameStats[0].player.IsHuman)
			{
				RateGamePanel.SaveLastGameWon(true);
			}
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x0003E996 File Offset: 0x0003CB96
		private void CheckIfPlayerMasterWonOnline()
		{
			if (this.endGameSequencePresenter.EndGameStats[0].faction == PlayerInfo.me.Faction)
			{
				RateGamePanel.SaveLastGameWon(true);
			}
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x0003E9C0 File Offset: 0x0003CBC0
		public void OnMultiplayerGameEnded()
		{
			this.SetControlsAtGameEnd();
			this.endGameSequencePresenter.StartEndGameSequence();
			MusicManager.Instance.StopMusic();
			MusicManager.Instance.PrepareWinnerMusic();
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x000D1438 File Offset: 0x000CF638
		public void SetControlsAtGameEnd()
		{
			this.gameBoardPresenter.UpdateStaticObjects();
			this.combatPresenter.battlefieldLayer.Clear();
			this.combatPresenter.battlefieldLayer.gameObject.SetActive(false);
			if (PlatformManager.IsStandalone)
			{
				this.panelInfo.DisableEndTurnObjectives();
			}
			else
			{
				SingletonMono<TopMenuPanelsManager>.Instance.GetTopMenuObjectivesPresenter().DisableEndTurnObjectives();
			}
			this.endTurnButton.interactable = false;
			this.endTurnHintButton.SetActive(true);
			this.endTurnHintType = GameController.EndTurnHintType.GameEnded;
			GameController.GameManager.actionManager.BreakSectionAction(false);
			foreach (MatPlayerSectionPresenter matPlayerSectionPresenter in this.matPlayer.matSection)
			{
				matPlayerSectionPresenter.DisableActionsAndSection(0, true);
			}
			if (GameController.GameManager.IsMultiplayer)
			{
				PlayerClock.StopTimer();
			}
			this.combatPresenter.gameObject.SetActive(false);
			this.UpdateStatsPresenter();
			this.objectiveCardPresenter.gameObject.SetActive(false);
			this.factoryCardPresenter.gameObject.SetActive(false);
			this.darkenUI.gameObject.SetActive(false);
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x000D1570 File Offset: 0x000CF770
		public void AddFactoryCard()
		{
			this.matPlayer.matSection.Add(this.factoryCardPresenter.GenerateSectionPresenter(this.matPlayer.factoryCardSlot, GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(4) as FactoryCard));
			this.matPlayer.factoryCardImage.SetActive(false);
			this.matPlayer.factoryCardSlot.SetActive(true);
			if (PlatformManager.IsMobile)
			{
				this.matPlayer.matSection[4].Init(GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(4).ActionTop, GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(4).ActionDown, GameController.GameManager.PlayerCurrent, true);
				this.matPlayer.matSection[4].SetSectionCooldown(true, false);
				this.matPlayer.matSection[4].DisableActionsAndSection(0, false);
				this.matPlayer.matSection[4].sectionGlass.enabled = false;
			}
			this.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].CheckDownAction();
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x000D16B0 File Offset: 0x000CF8B0
		public void ShowEncounterCard()
		{
			int currentMatSection = GameController.GameManager.PlayerCurrent.currentMatSection;
			if (currentMatSection < this.matPlayer.matSection.Count && currentMatSection >= 0)
			{
				this.matPlayer.matSection[currentMatSection].DisableActionsAndSection(0, true);
			}
			this.endTurnButton.interactable = false;
			if (!GameController.GameManager.IsMultiplayer)
			{
				this.encounterCardPresenter.SetOptions(GameController.GameManager.GetEncounterCard());
				return;
			}
			if (GameController.GameManager.IsMyTurn())
			{
				this.waitInfoEncounter.SetActive(true);
				GameController.GameManager.OnEncounter();
				if (!PlatformManager.IsStandalone)
				{
					SingletonMono<TopMenuPanelsManager>.Instance.GetTopMenuObjectivesPresenter().DisableEndTurnObjectives();
				}
			}
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x000D1764 File Offset: 0x000CF964
		public void ShowFactoryCards()
		{
			AchievementManager.UpdateAchievementFirstInFactory();
			WorldSFXManager.PlaySound(SoundEnum.FactoryCardsAppear, AudioSourceType.WorldSfx);
			this.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].DisableActionsAndSection(0, true);
			if (GameController.GameManager.IsMultiplayer && GameController.GameManager.IsMyTurn())
			{
				this.waitInfoFactory.SetActive(true);
				GameController.GameManager.OnGainFactoryCards();
				return;
			}
			if (!GameController.GameManager.IsMultiplayer)
			{
				this.factoryCardPresenter.gameObject.SetActive(true);
				this.factoryCardPresenter.SetCards();
				if (!PlatformManager.IsStandalone)
				{
					this.undoController.PopFromStack();
				}
			}
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x0003E9E7 File Offset: 0x0003CBE7
		public bool IsAmmoPoolEmpty()
		{
			return this.AmmoPoolEmpty;
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x0003E9EF File Offset: 0x0003CBEF
		public void OnNoMoreCombatCards(int cards)
		{
			if (cards == 0 && !this.AmmoPoolEmpty)
			{
				this.AmmoPoolEmpty = true;
				this.NoMoreBattleAmmoInfo.SetActive(true);
				return;
			}
			if (cards != 0)
			{
				this.AmmoPoolEmpty = false;
			}
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x000D1810 File Offset: 0x000CFA10
		public void ShowObjectiveCurrentPlayer(int index)
		{
			Player player;
			if (GameController.GameManager.IsMultiplayer)
			{
				player = GameController.GameManager.PlayerOwner;
			}
			else if (!GameController.GameManager.PlayerCurrent.IsHuman)
			{
				player = GameController.GameManager.GetPreviousHumanPlayer();
			}
			else
			{
				player = GameController.GameManager.PlayerCurrent;
			}
			this.objectiveCardPresenter.SetCard(player.objectiveCards[index], GameController.factionInfo[player.matFaction.faction].logo);
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x0003EA1A File Offset: 0x0003CC1A
		public static string GetObjectiveTitle(int objectiveId)
		{
			return ScriptLocalization.Get(string.Format("Objectives/ObjectiveTitle{0}", objectiveId));
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0003EA31 File Offset: 0x0003CC31
		public static string GetObjectiveDescription(int objectiveId)
		{
			return ScriptLocalization.Get(string.Format("Objectives/ObjectiveDescription{0}", objectiveId));
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x000D1890 File Offset: 0x000CFA90
		public void CompleteObjectiveWithWarning(int index)
		{
			if (OptionsManager.IsWarningsActive() && !GameController.GameManager.CanPlayerDoActionAfterObjective(index) && GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(GameController.GameManager.PlayerCurrent.currentMatSection).ActionDown.CanPlayerGainFromActions() && GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(GameController.GameManager.PlayerCurrent.currentMatSection).ActionDown.CanPlayerPayActions())
			{
				this.ShowObjectivEndTurnWarning(index);
				return;
			}
			this.CompleteObjective(index);
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x000D1920 File Offset: 0x000CFB20
		private void ShowObjectivEndTurnWarning(int index)
		{
			if (this.endTurnMessage != null)
			{
				this.endTurnMessage.text = ScriptLocalization.Get("GameScene/WarningBottomActive");
			}
			else
			{
				this.endTurnWarning.GetComponentInChildren<Text>().text = ScriptLocalization.Get("GameScene/WarningBottomActive");
			}
			this.endTurnWarning.Show(GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo, delegate
			{
				this.CompleteObjective(index);
			}, null);
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x000D19BC File Offset: 0x000CFBBC
		public void CompleteObjective(int index)
		{
			WorldSFXManager.PlaySound(SoundEnum.GuiOjectivesComplitedClick, AudioSourceType.Buttons);
			if (GameController.OnObjectiveCompleted != null)
			{
				GameController.OnObjectiveCompleted();
			}
			if (!GameController.GameManager.CanPlayerDoActionAfterObjective(index))
			{
				this.EndTurnButtonEnable();
				if (GameController.GameManager.PlayerCurrent.character.position.hasEncounter && !GameController.GameManager.PlayerCurrent.character.position.encounterUsed)
				{
					GameController.GameManager.PlayerCurrent.character.position.encounterUsed = true;
					GameController.GameManager.PlayerCurrent.character.position.encounterTaken = true;
					this.GetGameHexPresenter(GameController.GameManager.PlayerCurrent.character.position).ActivateEncounterEndAnimation();
				}
				GameController.GameManager.actionManager.BreakSectionAction(false);
				foreach (MatPlayerSectionPresenter matPlayerSectionPresenter in this.matPlayer.matSection)
				{
					matPlayerSectionPresenter.DisableActionsAndSection(0, true);
				}
				GameController.GameManager.OnObjectiveCompleted(index);
				this.UpdateStats(true, true);
				if ((GameController.GameManager.PlayerCurrent.matFaction.faction != Faction.Saxony || GameController.GameManager.PlayerCurrent.ObjectivesDone != 1 || !GameController.GameManager.PlayerCurrent.CanCompleteObjective(0)) && !GameController.GameManager.PlayerCurrent.CanCompleteObjective(1))
				{
					this.NextTurn();
					return;
				}
			}
			else
			{
				GameController.GameManager.OnObjectiveCompleted(index);
				this.UpdateStats(true, true);
			}
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0003EA48 File Offset: 0x0003CC48
		public static string GetStructureBonusName(int bonusId)
		{
			return ScriptLocalization.Get("StructureBonus/BonusName" + bonusId.ToString());
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x0003EA60 File Offset: 0x0003CC60
		public static string GetStructureBonusDescription(int bonusId)
		{
			return ScriptLocalization.Get("StructureBonus/BonusDescription" + bonusId.ToString());
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x000D1B58 File Offset: 0x000CFD58
		public void ForfeitGame()
		{
			if (!GameController.GameManager.GameFinished)
			{
				AnalyticsEventLogger.Instance.LogMatchStop(EndReasons.surrender);
			}
			AnalyticsEventData.UpdateMatchSessionID("");
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_yes_button);
			((MovePresenter)HumanInputHandler.Instance.movePresenter).Clear();
			if (GameController.GameManager.IsMultiplayer && !MultiplayerController.Instance.Disconnected && !GameController.GameManager.TestingMode)
			{
				this.returnToLobby = true;
				MultiplayerController.Instance.Forfeit();
				SceneController.Instance.LoadScene(SceneController.SCENE_LOBBY_NAME);
				return;
			}
			SceneController.Instance.LoadGame(SceneController.SCENE_MENU_NAME);
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x000D1BF8 File Offset: 0x000CFDF8
		public void ShowScorePreview()
		{
			this.endGamePreviewStats.gameObject.SetActive(true);
			List<PlayerEndGameStats> list = GameController.GameManager.CalculateStats();
			this.endGamePreviewStats.ShowPreviewStats(list);
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x000D1C30 File Offset: 0x000CFE30
		public void ShowRiverwalkPreview()
		{
			this.riverwalkPreview.SetActive(!this.riverwalkPreview.activeSelf);
			if (GameServiceController.Instance.InvadersFromAfarUnlocked() && !this.riverwalkPreview.transform.GetChild(6).gameObject.activeSelf)
			{
				this.riverwalkPreview.transform.GetChild(6).gameObject.SetActive(true);
			}
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x000D1C9C File Offset: 0x000CFE9C
		public void ExitGame()
		{
			if (this.GameIsLoaded)
			{
				AnalyticsEventLogger.Instance.LogMatchStop(EndReasons.game_load);
			}
			else if (!GameController.Game.GameManager.GameFinished)
			{
				if (GameController.Game.GameManager.IsCampaign)
				{
					if (AnalyticsEventData.IsCurrentTutorialSessionFinished())
					{
						AnalyticsEventLogger.Instance.LogMatchStop(EndReasons.game_completed);
					}
					else
					{
						AnalyticsEventLogger.Instance.LogTutorialStep(StepStatuses.quit);
						AnalyticsEventLogger.Instance.LogMatchStop(EndReasons.quit);
					}
				}
				else if (GameController.Game.GameManager.IsMultiplayer && MultiplayerController.Instance.RunOutOfTime)
				{
					AnalyticsEventLogger.Instance.LogMatchStop(EndReasons.time_up);
				}
				else
				{
					AnalyticsEventLogger.Instance.LogMatchStop(EndReasons.quit);
				}
			}
			else if (GameController.Game.GameManager.GameFinished)
			{
				AnalyticsEventLogger.Instance.LogMatchStop(EndReasons.game_completed);
			}
			else if (!GameController.Game.GameManager.IsMultiplayer && !GameController.Game.GameManager.IsCampaign)
			{
				GameSavesManager.DeleteGame("ScytheSaveTmp.xml");
			}
			if (!this.GameIsLoaded)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_exit_button);
			}
			this.menu.GetComponent<GameMenu>().exitGameDialog.SetActive(false);
			((MovePresenter)HumanInputHandler.Instance.movePresenter).Clear();
			if (GameController.GameManager.IsMultiplayer && !MultiplayerController.Instance.Disconnected && !GameController.GameManager.TestingMode)
			{
				this.returnToLobby = true;
				MultiplayerController.Instance.ReturnToLobby();
				SceneController.Instance.LoadScene(SceneController.SCENE_LOBBY_NAME);
				return;
			}
			if (GameController.GameManager.TestingMode)
			{
				MultiplayerController.Instance.CloseMultiplayer();
				SceneController.Instance.LoadScene(SceneController.SCENE_MENU_NAME);
				return;
			}
			if (!this.GameIsLoaded)
			{
				SceneController.Instance.LoadScene(SceneController.SCENE_MENU_NAME);
			}
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x000D1E50 File Offset: 0x000D0050
		public static void SetFocusHex(Scythe.BoardPresenter.GameHexPresenter hex)
		{
			GameHex gameHexLogic = hex.GetGameHexLogic();
			if (gameHexLogic == null || gameHexLogic.hexType == HexType.forbidden)
			{
				return;
			}
			GameController.SelectionMode hexSelectionMode = GameController.HexSelectionMode;
			if (hexSelectionMode - GameController.SelectionMode.MoveAction <= 7 && GameController.HexGetFocused != null)
			{
				GameController.HexGetFocused(hex);
			}
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x000D1E90 File Offset: 0x000D0090
		public static void SetFocusUnit(UnitPresenter unit)
		{
			GameController.SelectionMode hexSelectionMode = GameController.HexSelectionMode;
			if (hexSelectionMode == GameController.SelectionMode.MoveAction || hexSelectionMode == GameController.SelectionMode.Combat)
			{
				GameController.Instance.cameraControler.HooverReset();
				if (GameController.UnitGetFocused != null)
				{
					GameController.UnitGetFocused(unit);
				}
			}
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x000D1ECC File Offset: 0x000D00CC
		public static void ClearFocus()
		{
			foreach (UnitPresenter unitPresenter in GameController.FocusUnit)
			{
				unitPresenter.SetFocus(false, 0);
			}
			GameController.FocusUnit.Clear();
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x0003EA78 File Offset: 0x0003CC78
		public Scythe.BoardPresenter.GameHexPresenter GetGameHexPresenter(GameHex position)
		{
			return this.gameBoardPresenter.GetGameHexPresenter(position);
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x0003EA86 File Offset: 0x0003CC86
		public Scythe.BoardPresenter.GameHexPresenter GetGameHexPresenter(int x, int y)
		{
			return this.gameBoardPresenter.GetGameHexPresenter(x, y);
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x0003EA95 File Offset: 0x0003CC95
		public Vector3 GetGameBoardPosition()
		{
			return this.gameBoardPresenter.transform.position;
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x0003EAA7 File Offset: 0x0003CCA7
		public void GetNextPlayerAfterEnemyAnimation()
		{
			base.StartCoroutine(this.NextTurnCoroutine());
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x0003EAB6 File Offset: 0x0003CCB6
		public void MinimizePlayerMat()
		{
			if (PlatformManager.IsStandalone)
			{
				this.bottomBar.Play("PlayerMatShrink");
			}
			this.matPlayer.ShowActionLabels(false);
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x0003EADB File Offset: 0x0003CCDB
		public void MaximizePlayerMat()
		{
			if (PlatformManager.IsStandalone)
			{
				this.bottomBar.Play("PlayerMatExpand");
			}
			this.matPlayer.ShowActionLabels(true);
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x0003EB00 File Offset: 0x0003CD00
		public static UnitPresenter GetUnitPresenter(Unit unit)
		{
			return GameController.factionUnits[unit.Owner.matFaction.faction].GetUnitPresenter(unit);
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x000D1F28 File Offset: 0x000D0128
		private void ClearDelegates()
		{
			GameController.AfterEndTurnAIAndPlayer = null;
			OptionsManager.OnLanguageChanged -= this.UpdateFactionInfo;
			OptionsManager.OnLanguageChanged -= this.UpdateTurnCounterText;
			if (GameController.GameManager != null)
			{
				GameController.GameManager.ObjectiveCardGetHighlighted -= this.OnObjectiveCardHighlight;
				GameController.GameManager.UpdatePlayerStats -= this.OnUpdatePlayerStats;
				GameController.GameManager.FactoryGetEnabled -= this.OnFactoryEnabled;
				GameController.GameManager.EncounterGetEnabled -= this.OnEncounterEnabled;
				GameController.GameManager.GameHasEnded -= this.OnGameEnded;
				GameController.GameManager.UpdatePlayerStats -= this.OnUpdatePlayerStats;
				GameController.GameManager.CombatCardsAmountStatus -= this.OnNoMoreCombatCards;
				GameController.GameManager.BotTurnEnded -= this.ContinueBotAction;
				GameController.GameManager.OnEnemyUnitMoved -= this.OnEnemyUnitMove;
				GameController.GameManager.OnEnemyProduced -= this.OnEnemyProduce;
				GameController.GameManager.OnEnemyGainedWorker -= this.OnEnemyGainWorker;
				GameController.GameManager.OnEnemyGainedWorkerEnds -= this.OnEnemyGainWorkerEnds;
				GameController.GameManager.OnEnemyTraded -= this.OnEnemyTrade;
				GameController.GameManager.OnEnemyRecruitBonusObtain -= this.OnEnemyRecruitBonus;
				GameController.GameManager.OnEnemysBonusEnd -= this.OnEnemysBonusEnd;
				GameController.GameManager.OnEnemyRecruited -= this.OnEnemyRecruit;
				GameController.GameManager.OnEnemyUpgraded -= this.OnEnemyUpgrade;
				GameController.GameManager.OnEnemyDeployed -= this.OnEnemyDeploy;
				GameController.GameManager.OnEnemyBuilded -= this.OnEnemyBuild;
				GameController.GameManager.OnEnemyPaidResources -= this.OnEnemyPayResource;
				GameController.GameManager.OnEnemyMoved -= this.OnEnemyMove;
				GameController.GameManager.OnEnemyLoadedResources -= this.OnEnemyLoadResources;
				GameController.GameManager.OnEnemyLoadedWorker -= this.OnEnemyLoadWorker;
				GameController.GameManager.OnEnemyUnloadedWorker -= this.OnEnemyUnloadWorker;
				GameController.GameManager.OnEnemyRetreatMoved -= this.OnEnemyRetreatMove;
				GameController.GameManager.OnEnemyGainStats -= this.OnEnemyGainTopStats;
				if (GameController.GameManager.IsMultiplayer)
				{
					GameController.GameManager.ActionHasFinished -= this.OnActionFinished;
					GameController.GameManager.InputWasEnabled -= this.OnInputEnabled;
					GameController.GameManager.ShowedFactoryCards -= this.OnShowFactoryCards;
					GameController.GameManager.CardAdded -= this.AfterCardAdded;
					GameController.GameManager.ShowedEmptyCards -= this.OnShowEmptyCards;
					GameController.GameManager.ShowEncounter -= this.ShowEncounter;
					GameController.GameManager.ChooseOption -= this.ChooseOption;
					GameController.GameManager.ShowedFactory -= this.OnShowChoosenFactory;
					GameController.GameManager.ShowObjective -= this.ShowObjectiveCard;
					GameController.GameManager.CombatAbilityUsed -= this.CombatAbilityUsed;
					GameController.GameManager.OnShowStats -= this.OnShowPoints;
					GameController.GameManager.MultiplayerGameEnded -= this.GameEnded;
					GameController.GameManager.GameSynchronized -= this.SynchronizeGame;
					GameController.GameManager.GameLoaded -= this.GameLoaded;
					GameController.GameManager.BattlefieldChoosen -= this.ChooseFirstBattlefield;
					GameController.GameManager.BotTurnEnded -= this.OnTurnEnded;
					GameController.GameManager.TurnEnded -= this.OnTurnEnded;
					GameController.GameManager.OnEncounterEnded -= this.EndEncounter;
				}
				AiPlayer.CombatAbilityUsed -= this.CombatAbilityUsed;
			}
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x0003EB22 File Offset: 0x0003CD22
		private void OnApplicationQuit()
		{
			if (GameController.GameManager.IsMultiplayer && !this.returnToLobby)
			{
				MultiplayerController.Instance.CloseMultiplayer();
			}
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x0003EB42 File Offset: 0x0003CD42
		private void OnApplicationPause(bool pauseStatus)
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				RequestController.OnAppPausedStateChanged(pauseStatus);
			}
		}

		// Token: 0x04001812 RID: 6162
		public static GameController Instance;

		// Token: 0x04001816 RID: 6166
		public static int focusHexMax = int.MaxValue;

		// Token: 0x04001817 RID: 6167
		public static int resourcePayed = int.MaxValue;

		// Token: 0x04001818 RID: 6168
		public GameController.QuickstartMode quickstartMode;

		// Token: 0x04001819 RID: 6169
		public bool showAILogs;

		// Token: 0x0400181A RID: 6170
		public Camera uiCamera;

		// Token: 0x0400181B RID: 6171
		public Canvas ui;

		// Token: 0x0400181C RID: 6172
		public GameObject NoMoreBattleAmmoInfo;

		// Token: 0x0400181D RID: 6173
		public TurnInfoPanel turnInfoPanel;

		// Token: 0x0400181E RID: 6174
		public Text endTurnHint;

		// Token: 0x0400181F RID: 6175
		public Image endTurnShade;

		// Token: 0x04001820 RID: 6176
		public GameObject endTurnHintButton;

		// Token: 0x04001821 RID: 6177
		public GameController.EndTurnHintType endTurnHintType;

		// Token: 0x04001822 RID: 6178
		public Button endTurnButton;

		// Token: 0x04001823 RID: 6179
		public Button forfeitButton;

		// Token: 0x04001824 RID: 6180
		public TextMeshProUGUI endTurnButtonText;

		// Token: 0x04001825 RID: 6181
		public Sprite endTurnButtonImageActive;

		// Token: 0x04001826 RID: 6182
		public Sprite endTurnButtonImageActiveGlow;

		// Token: 0x04001827 RID: 6183
		public GameObject gameBoard2d;

		// Token: 0x04001828 RID: 6184
		public GameBoardPresenter gameBoardPresenter;

		// Token: 0x04001829 RID: 6185
		public Animator bottomBar;

		// Token: 0x0400182A RID: 6186
		public MatFactionPresenter matFaction;

		// Token: 0x0400182B RID: 6187
		public MatPlayerPresenter matPlayer;

		// Token: 0x0400182C RID: 6188
		public HexPointerController hexPointerController;

		// Token: 0x0400182D RID: 6189
		public EncounterCardPresenter encounterCardPresenter;

		// Token: 0x0400182E RID: 6190
		public FactoryCardsPresenter factoryCardPresenter;

		// Token: 0x0400182F RID: 6191
		public TabPanelWindow panelInfo;

		// Token: 0x04001830 RID: 6192
		public LastActionInfoPresenter lastActionInfoPresenter;

		// Token: 0x04001831 RID: 6193
		public PlayerStatsPresenter playerStats;

		// Token: 0x04001832 RID: 6194
		public EndGameSequencePresenter endGameSequencePresenter;

		// Token: 0x04001833 RID: 6195
		public EndGameStatsPresenter endGamePreviewStats;

		// Token: 0x04001834 RID: 6196
		public PlayerOrder playersFactions;

		// Token: 0x04001835 RID: 6197
		public ObjectiveCardPresenter objectiveCardPresenter;

		// Token: 0x04001836 RID: 6198
		public GameObject objectiveContextHint;

		// Token: 0x04001837 RID: 6199
		public ResourceTypeLayer ResourceTypeLayer;

		// Token: 0x04001838 RID: 6200
		public CombatPresenter combatPresenter;

		// Token: 0x04001839 RID: 6201
		public CombatPresenterMobile combatPresenterMobile;

		// Token: 0x0400183A RID: 6202
		public GameObject riverwalkPreview;

		// Token: 0x0400183B RID: 6203
		public DragAndDropPanel dragAndDropPanel;

		// Token: 0x0400183C RID: 6204
		public DragAndDropHook hook;

		// Token: 0x0400183D RID: 6205
		public HookController hookController;

		// Token: 0x0400183E RID: 6206
		public TokensController tokensController;

		// Token: 0x0400183F RID: 6207
		public ActionLogInterpreter logInterpreter;

		// Token: 0x04001840 RID: 6208
		public LoadingScreenPresenter loadingScreen;

		// Token: 0x04001841 RID: 6209
		public Image darkenUI;

		// Token: 0x04001842 RID: 6210
		public GameObject menu;

		// Token: 0x04001843 RID: 6211
		public GameObject[] menuHotseatOnly;

		// Token: 0x04001844 RID: 6212
		public Button menuLoadButton;

		// Token: 0x04001845 RID: 6213
		public UndoController undoController;

		// Token: 0x04001846 RID: 6214
		public GameObject exitingScreen;

		// Token: 0x04001847 RID: 6215
		public CameraControler cameraControler;

		// Token: 0x04001848 RID: 6216
		public AutomaticGainPresenter autoGainPanel;

		// Token: 0x04001849 RID: 6217
		public Chat chat;

		// Token: 0x0400184A RID: 6218
		public GameObject chatButton;

		// Token: 0x0400184B RID: 6219
		public GameObject waitInfoEncounter;

		// Token: 0x0400184C RID: 6220
		public GameObject waitInfoFactory;

		// Token: 0x0400184D RID: 6221
		public GameObject waitInfoRecruit;

		// Token: 0x0400184E RID: 6222
		public Dictionary<string, GameObject> capitalsGodrays = new Dictionary<string, GameObject>();

		// Token: 0x0400184F RID: 6223
		public GameObject selectedBattlefieldEffects;

		// Token: 0x04001850 RID: 6224
		public List<Toggle2> tabToggles;

		// Token: 0x04001851 RID: 6225
		public GameController.FactionInfo[] factionLogos;

		// Token: 0x04001852 RID: 6226
		public PlayerUnits[] playerUnits;

		// Token: 0x04001853 RID: 6227
		public YesNoDialog endTurnWarning;

		// Token: 0x04001854 RID: 6228
		public TextMeshProUGUI endTurnMessage;

		// Token: 0x04001855 RID: 6229
		public bool returnToLobby;

		// Token: 0x04001856 RID: 6230
		public bool GameIsLoaded;

		// Token: 0x04001857 RID: 6231
		public bool DragAndDrop = PlatformManager.IsStandalone;

		// Token: 0x04001861 RID: 6241
		public bool endTurnCacheMode;

		// Token: 0x04001862 RID: 6242
		public bool endTurnCached;

		// Token: 0x04001863 RID: 6243
		private bool waitForGameEnd;

		// Token: 0x04001864 RID: 6244
		private Queue<IExecutableMessage> multiplayerActions = new Queue<IExecutableMessage>();

		// Token: 0x04001865 RID: 6245
		public Dictionary<Faction, Vector3> factionCamera = new Dictionary<Faction, Vector3>();

		// Token: 0x04001866 RID: 6246
		private bool AmmoPoolEmpty;

		// Token: 0x04001867 RID: 6247
		private bool AIActionInterrupt;

		// Token: 0x04001868 RID: 6248
		private bool StatsSaved;

		// Token: 0x04001869 RID: 6249
		public bool gameFinishedTimeOut;

		// Token: 0x0400186A RID: 6250
		[Header("Units move animation")]
		public AnimationCurve unitsHorizontalEase;

		// Token: 0x0400186B RID: 6251
		public AnimationCurve unitsVerticalEase;

		// Token: 0x0400186C RID: 6252
		public AnimationCurve unitsRotationEase;

		// Token: 0x0400186D RID: 6253
		public AnimationCurve unitsSpawn;

		// Token: 0x0400186E RID: 6254
		[Range(0.1f, 10f)]
		public float unitsMoveSpeed;

		// Token: 0x0400186F RID: 6255
		[Range(0f, 10f)]
		public float jumpHeight = 2.25f;

		// Token: 0x04001870 RID: 6256
		[Range(0.1f, 10f)]
		public float resourcesMoveSpeed = 1f;

		// Token: 0x04001871 RID: 6257
		[Range(0f, 10f)]
		public float resourcesJumpHeight = 1f;

		// Token: 0x04001872 RID: 6258
		[Range(0f, 90f)]
		[Tooltip("Max rotation which pawn can achieve.\nE.g. for value 20 pawn can make rotation in X-axis and Z-axis from -20 to 20")]
		public float maxRotation = 20f;

		// Token: 0x04001873 RID: 6259
		public float rangeVisibilityTime = 1f;

		// Token: 0x04001874 RID: 6260
		public GameObject optionsMenu;

		// Token: 0x04001875 RID: 6261
		[HideInInspector]
		public LogInfo actualLogInfo;

		// Token: 0x04001876 RID: 6262
		public static bool gameFromSave = false;

		// Token: 0x04001877 RID: 6263
		public CameraMovementEffects cameraMovementEffects;

		// Token: 0x04001878 RID: 6264
		public bool cameraPresentationEnabled = true;

		// Token: 0x04001879 RID: 6265
		public TextMeshProUGUI TurnCounter;

		// Token: 0x0400187A RID: 6266
		public GameObject reflectionProbes;

		// Token: 0x0400187B RID: 6267
		public ChangeFactionPanel changeFactionPanel;

		// Token: 0x0400187C RID: 6268
		public bool isUndoLoad;

		// Token: 0x0400187D RID: 6269
		public Sprite[] capitalsIcons;

		// Token: 0x02000455 RID: 1109
		public enum QuickstartMode
		{
			// Token: 0x0400187F RID: 6271
			AI = -1,
			// Token: 0x04001880 RID: 6272
			Tutorial01,
			// Token: 0x04001881 RID: 6273
			Tutorial02,
			// Token: 0x04001882 RID: 6274
			Tutorial03,
			// Token: 0x04001883 RID: 6275
			Tutorial04,
			// Token: 0x04001884 RID: 6276
			Tutorial05,
			// Token: 0x04001885 RID: 6277
			Tutorial06,
			// Token: 0x04001886 RID: 6278
			Tutorial07,
			// Token: 0x04001887 RID: 6279
			Tutorial08,
			// Token: 0x04001888 RID: 6280
			Tutorial09,
			// Token: 0x04001889 RID: 6281
			Tutorial10,
			// Token: 0x0400188A RID: 6282
			Tutorial11,
			// Token: 0x0400188B RID: 6283
			Challenge1,
			// Token: 0x0400188C RID: 6284
			Challenge2,
			// Token: 0x0400188D RID: 6285
			Challenge3,
			// Token: 0x0400188E RID: 6286
			Challenge4
		}

		// Token: 0x02000456 RID: 1110
		public enum SelectionMode
		{
			// Token: 0x04001890 RID: 6288
			Normal,
			// Token: 0x04001891 RID: 6289
			MoveAction,
			// Token: 0x04001892 RID: 6290
			DownAction,
			// Token: 0x04001893 RID: 6291
			TradeAction,
			// Token: 0x04001894 RID: 6292
			GainWorker,
			// Token: 0x04001895 RID: 6293
			Combat,
			// Token: 0x04001896 RID: 6294
			Deploy,
			// Token: 0x04001897 RID: 6295
			Build,
			// Token: 0x04001898 RID: 6296
			PayResource
		}

		// Token: 0x02000457 RID: 1111
		public enum EndTurnHintType
		{
			// Token: 0x0400189A RID: 6298
			ChoseActionFirst,
			// Token: 0x0400189B RID: 6299
			Combat,
			// Token: 0x0400189C RID: 6300
			WaitForOpponent,
			// Token: 0x0400189D RID: 6301
			ChoseFactoryCard,
			// Token: 0x0400189E RID: 6302
			GameEnded
		}

		// Token: 0x02000458 RID: 1112
		// (Invoke) Token: 0x06002350 RID: 9040
		public delegate void HexFocused(Scythe.BoardPresenter.GameHexPresenter presenter);

		// Token: 0x02000459 RID: 1113
		// (Invoke) Token: 0x06002354 RID: 9044
		public delegate void UnitFocused(UnitPresenter presenter);

		// Token: 0x0200045A RID: 1114
		// (Invoke) Token: 0x06002358 RID: 9048
		public delegate void OnEndTurn();

		// Token: 0x0200045B RID: 1115
		// (Invoke) Token: 0x0600235C RID: 9052
		public delegate void OnEndTurnAIAndPlayer();

		// Token: 0x0200045C RID: 1116
		// (Invoke) Token: 0x06002360 RID: 9056
		public delegate void OnObjectiveComplete();

		// Token: 0x0200045D RID: 1117
		// (Invoke) Token: 0x06002364 RID: 9060
		public delegate void OnTopTabClick(int id);

		// Token: 0x0200045E RID: 1118
		// (Invoke) Token: 0x06002368 RID: 9064
		public delegate void OnGameLoad();

		// Token: 0x0200045F RID: 1119
		[Serializable]
		public class FactionInfo
		{
			// Token: 0x0400189F RID: 6303
			public Faction faction;

			// Token: 0x040018A0 RID: 6304
			public Sprite logo;

			// Token: 0x040018A1 RID: 6305
			public Texture logoTex;

			// Token: 0x040018A2 RID: 6306
			public Color color;

			// Token: 0x040018A3 RID: 6307
			public Color colorAccent;

			// Token: 0x040018A4 RID: 6308
			public Sprite[] mechAbilityIcons;
		}
	}
}

using System;
using System.Collections;
using I2.Loc;
using Newtonsoft.Json;
using Scythe.Analytics;
using Scythe.Multiplayer.Data;
using Scythe.Multiplayer.Messages;
using Scythe.UI;
using Scythe.Utilities;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x02000254 RID: 596
	public class Lobby : MonoBehaviour
	{
		// Token: 0x06001219 RID: 4633 RVA: 0x00033BB4 File Offset: 0x00031DB4
		private void Awake()
		{
			this.AddDelegates();
			this.ShowWaitingScreen(ScriptLocalization.Get("Lobby/Loading"), true);
			MultiplayerController.Instance.InitMultiplayer();
			RequestController.OnAppPausedStateChanged(false);
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x00033BDD File Offset: 0x00031DDD
		private void Start()
		{
			this.FirstJoinToLobby();
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x000963CC File Offset: 0x000945CC
		private void Update()
		{
			if (LobbyLogic.Instance.GameStarted)
			{
				return;
			}
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.esc);
				this.BreakLoadingAndReturnToMenu();
			}
			if (RequestController.serverUpdates.Count <= 0)
			{
				if (RequestController.getUpdateReady && this.appUnpaused)
				{
					this.appUnpaused = false;
					if (this.gameRoomPanelMobile && this.gameRoomPanelMobile.matChoiceTimer && this.gameRoomPanelMobile.matChoiceTimer.IsActive && this.gameRoomPanelMobile && this.gameRoomPanelMobile.matAndFactionChoose != null)
					{
						if (this.pauseSelectionSlot != this.gameRoomPanelMobile.matAndFactionChoose.GetCurrentSlot())
						{
							bool flag = false;
							int currentSlot = this.gameRoomPanelMobile.matAndFactionChoose.GetCurrentSlot();
							if (this.gameRoomPanelMobile.gameRoomSlots[currentSlot].PlayerData != null && this.gameRoomPanelMobile.gameRoomSlots[currentSlot].PlayerData.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id)
							{
								flag = true;
							}
							if (flag)
							{
								int num = 60 - (int)this.diffTimeSpan.TotalSeconds;
								if (num < 0)
								{
									num = 0;
								}
								this.gameRoomPanelMobile.matChoiceTimer.SetTime(num);
							}
							else
							{
								int num2 = 60;
								this.gameRoomPanelMobile.matChoiceTimer.SetTime(num2);
							}
						}
						else
						{
							int num3 = this.gameRoomPanelMobile.matChoiceTimer.TimeLeft - (int)this.diffTimeSpan.TotalSeconds;
							if (num3 < 0)
							{
								num3 = 0;
							}
							this.gameRoomPanelMobile.matChoiceTimer.SetTime(num3);
						}
					}
					this.pauseSelectionSlot = -1;
					this.waitingPanel.Deactivate();
				}
				return;
			}
			Scythe.Multiplayer.Messages.Message message = RequestController.serverUpdates.Dequeue();
			if (message is ChatMessage)
			{
				MultiplayerController.Instance.ReceivedChatMessage(message as ChatMessage);
				return;
			}
			if (!(message is IExecutableLobbyMessage))
			{
				Debug.LogWarning("Received unknown message. Message: " + GameSerializer.JsonMessageSerializer<Scythe.Multiplayer.Messages.Message>(message));
				return;
			}
			if (this.gameRoomPanel != null)
			{
				(message as IExecutableLobbyMessage).Execute(this, this.playerListPanel, this.gameListPanel, this.gameRoomPanel);
				return;
			}
			(message as IExecutableLobbyMessage).Execute(this, this.playerListPanel, this.gameListPanel, this.gameRoomPanelMobile);
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x0009661C File Offset: 0x0009481C
		private void OnApplicationPause(bool pause)
		{
			if (pause)
			{
				if (this.gameRoomPanelMobile && this.gameRoomPanelMobile.matAndFactionChoose != null)
				{
					this.pauseSelectionSlot = this.gameRoomPanelMobile.matAndFactionChoose.GetCurrentSlot();
				}
				else
				{
					this.pauseSelectionSlot = -1;
				}
				this.pauseDateTime = DateTime.Now;
				RequestController.getUpdateReady = false;
				this.appUnpaused = false;
			}
			else
			{
				this.waitingPanel.Activate(ScriptLocalization.Get("Lobby/Loading"), true);
				this.appUnpaused = true;
				this.diffTimeSpan = DateTime.Now - this.pauseDateTime;
			}
			RequestController.OnAppPausedStateChanged(pause);
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00033BE5 File Offset: 0x00031DE5
		private void OnApplicationFocus(bool focus)
		{
			if (focus)
			{
				DateTime.Now - this.lostFocusDateTime;
				return;
			}
			this.lostFocusDateTime = DateTime.Now;
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00033C07 File Offset: 0x00031E07
		private void OnDestroy()
		{
			this.RemoveDelegates();
			if (!LobbyLogic.Instance.GameStarted)
			{
				MultiplayerController.Instance.ClearInLobby();
				this.ExitGame();
			}
			LobbyLogic.Instance.Clear();
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x000966B8 File Offset: 0x000948B8
		public void LeaveRoom()
		{
			this.ShowWaitingScreen(ScriptLocalization.Get("Lobby/Loading"), true);
			this.mainLobby.Activate();
			this.mainLobby.ChangeCreateGameActivity(false);
			this.mainLobby.ActivateResumeJoinPanel();
			LobbyLogic.Instance.LeaveRoomAndJoinLobby(delegate(string leaveRoomResponse)
			{
				this.gameListPanel.RefreshRoomsLists();
				this.HideWaitingScreenIfAble();
			}, delegate(Exception exception)
			{
				this.GetUpdateException(exception);
			});
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0009671C File Offset: 0x0009491C
		public void QuickPlay(Preferences preferences)
		{
			this.ShowWaitingScreen(ScriptLocalization.Get("Lobby/Loading"), true);
			LobbyLogic.Instance.QuickPlay(preferences, delegate(string roomId)
			{
				this.JoinRoom(roomId);
			}, delegate(QuickPlayErrorStatus errorStatus)
			{
				if (errorStatus == QuickPlayErrorStatus.NoGameFound)
				{
					this.CreateRoom(CreateRoomHelper.CreateRoomWithPreferences(preferences));
					return;
				}
				this.HideWaitingScreenIfAble();
			});
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00033C35 File Offset: 0x00031E35
		public void CreateRoom(LobbyRoom roomData)
		{
			this.ShowWaitingScreen(ScriptLocalization.Get("Lobby/Loading"), true);
			LobbyLogic.Instance.CreateAndJoinRoom(roomData, delegate(string response)
			{
				LobbyGame currentLobbyRoom = PlayerInfo.me.CurrentLobbyRoom;
				PlayerPrefs.SetInt("PlayerClock", currentLobbyRoom.PlayerClockTime * 60);
				this.mainLobby.Deactivate();
				if (this.gameRoomPanel)
				{
					this.gameRoomPanel.Init(currentLobbyRoom);
				}
				else
				{
					this.gameRoomPanelMobile.Init(currentLobbyRoom);
					this.gameRoomPanelMobile.Show();
				}
				this.HideWaitingScreenIfAble();
			}, delegate(JoinRoomErrorStatus error)
			{
				this.JoinToLobby();
			});
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00096778 File Offset: 0x00094978
		public void SpectateGame(int faction, string roomId)
		{
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.join);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_spectate_button);
			this.ShowWaitingScreen(ScriptLocalization.Get("Lobby/Loading"), true);
			LobbyLogic.Instance.SpectateGame(faction, roomId, delegate(string response)
			{
				this.LoadGameScene();
			}, delegate(Exception exception)
			{
				Debug.LogError(exception);
				this.HideWaitingScreenIfAble();
				this.gameListPanel.RemoveRoom(roomId);
			});
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x00033C6B File Offset: 0x00031E6B
		public void JoinRoom(string roomId)
		{
			this.ShowWaitingScreen(ScriptLocalization.Get("Lobby/Loading"), true);
			LobbyLogic.Instance.JoinRoom(roomId, delegate(string joinRoomResponse)
			{
				LobbyGame currentLobbyRoom = PlayerInfo.me.CurrentLobbyRoom;
				PlayerPrefs.SetInt("PlayerClock", currentLobbyRoom.PlayerClockTime * 60);
				this.mainLobby.Deactivate();
				if (this.gameRoomPanel)
				{
					this.playerStatsScreen.Deactivate();
					this.playerStatsScreen.DeactivatePlayerStatsPresenter();
					this.gameRoomPanel.Init(currentLobbyRoom);
				}
				else
				{
					this.gameRoomPanelMobile.Init(currentLobbyRoom);
					this.gameRoomPanelMobile.Show();
				}
				this.HideWaitingScreenIfAble();
			}, delegate(JoinRoomErrorStatus joinRoomErrorStatus)
			{
				if (!this.connectionProblemPanel.IsActive())
				{
					this.joinRoomErrorPanel.Open(joinRoomErrorStatus);
					this.JoinToLobby();
				}
			});
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x000967E0 File Offset: 0x000949E0
		public void Reconnect(string gameId, int timeLeft, bool ranked, GameType gameType)
		{
			if (LobbyLogic.Instance.ReconnectingInProgress)
			{
				throw new Exception("Reconnecting in progress");
			}
			this.ShowWaitingScreen(ScriptLocalization.Get("Lobby/Loading"), true);
			if (gameType == GameType.Asynchronous)
			{
				AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.playAndGoReturn);
			}
			string text = GameServiceController.Instance.PlayerId();
			LobbyLogic.Instance.Reconnect(gameId, timeLeft, ranked, gameType, text, delegate(string response)
			{
				this.LoadGameScene();
			}, delegate(Exception error)
			{
				this.HideWaitingScreenIfAble();
				throw error;
			});
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x00033CA1 File Offset: 0x00031EA1
		public void OnGameStarted()
		{
			LobbyLogic.Instance.OnGameStarted();
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x00033CAD File Offset: 0x00031EAD
		public void ShowGameInterruptedPanel(GameType gameType, bool isRanked, int timeInMinutes)
		{
			this.gameInterruptedPopup.Activate(gameType, isRanked, timeInMinutes);
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00033CBD File Offset: 0x00031EBD
		public void ShowPlayAndStayReconnectPanel(string roomId, int timeLeft, GameType gameType, bool rankedGame)
		{
			this.playAndStayReconnectPanel.Activate(roomId, timeLeft, gameType, rankedGame);
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x00033CCF File Offset: 0x00031ECF
		public void ShowPlayAndGoReconnectPanel(string roomId, int timeLeft, GameType gameType, bool rankedGame)
		{
			this.playAndGoReconnectPanel.Activate(roomId, timeLeft, gameType, rankedGame);
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00033CE1 File Offset: 0x00031EE1
		public void ShowInvite(InvitationReceived inviteData)
		{
			this.invitationPanel.Activate(inviteData);
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x00033CEF File Offset: 0x00031EEF
		public void ShowForfeitGamePanel(string gameId, string gameName, bool isRanked)
		{
			this.forfeitGamePanel.Activate(gameId, gameName, isRanked, this);
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x00033D00 File Offset: 0x00031F00
		public void RefreshGameList()
		{
			this.gameListPanel.RefreshRoomsLists();
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00033D0D File Offset: 0x00031F0D
		public void ShowRemoveFriendPanel(PlayerListEntry playerListEntry)
		{
			this.removeFriendPanel.Activate(playerListEntry);
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00033D1B File Offset: 0x00031F1B
		public void GetUpdateException(Exception exception)
		{
			MultiplayerController.Instance.ClearInLobby();
			this.StopCoroutines();
			this.ShowConnectionProblemPanel(exception);
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00096854 File Offset: 0x00094A54
		private void AddDelegates()
		{
			LobbyLogic.Instance.PlayersListLogic.PlayersListDownloaded += this.OnPlayerlistDownloaded;
			this.gameListPanel.RoomListLoaded += this.OnGameListDownloaded;
			LobbyLogic.Instance.CurrentPlayerStatsLoaded += this.OnCurrentPlayerStatsLoaded;
			if (this.gameRoomPanel)
			{
				this.gameRoomPanel.PlayerKicked += this.OnPlayerKicked;
				return;
			}
			this.gameRoomPanelMobile.PlayerKicked += this.OnPlayerKicked;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00033D34 File Offset: 0x00031F34
		private void OnPlayerlistDownloaded()
		{
			this.HideWaitingScreenIfAble();
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00033D3C File Offset: 0x00031F3C
		private void OnGameListDownloaded()
		{
			LobbyLogic.Instance.OnGameListDownloaded();
			this.HideWaitingScreenIfAble();
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00033D4E File Offset: 0x00031F4E
		private void OnPlayerKicked()
		{
			this.ShowWaitingScreen(ScriptLocalization.Get("Lobby/Loading"), true);
			this.mainLobby.Activate();
			this.mainLobby.ChangeCreateGameActivity(false);
			this.mainLobby.ActivateResumeJoinPanel();
			this.JoinToLobby();
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x000968E8 File Offset: 0x00094AE8
		private void FirstJoinToLobby()
		{
			this.mainLobby.Activate();
			this.mainLobby.ChangeCreateGameActivity(false);
			this.mainLobby.ActivateResumeJoinPanel();
			LobbyLogic.Instance.FirstJoinToLobby(delegate(string firstJoinToLobbyResponse)
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.lobby, Contexts.outgame);
				this.playerListPanel.StartCoroutines();
				this.gameListPanel.RefreshRoomsLists();
			}, delegate(Exception firstJoinToLobbyError)
			{
				this.GetUpdateException(firstJoinToLobbyError);
			});
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x00033D89 File Offset: 0x00031F89
		private void JoinToLobby()
		{
			LobbyLogic.Instance.JoinToLobby(delegate(string joinToLobbyResponse)
			{
				this.gameListPanel.RefreshRoomsLists();
				this.HideWaitingScreenIfAble();
			}, delegate(Exception joinToLobbyError)
			{
				this.HideWaitingScreenIfAble();
				this.GetUpdateException(joinToLobbyError);
			});
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00033DAD File Offset: 0x00031FAD
		public void ShowWaitingScreen(string text, bool hideLobby)
		{
			this.waitingPanel.Activate(text, hideLobby);
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00033DBC File Offset: 0x00031FBC
		public void HideWaitingScreenIfAble()
		{
			if (LobbyLogic.Instance.LobbyLoaded())
			{
				this.waitingPanel.Deactivate();
			}
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00033DD5 File Offset: 0x00031FD5
		public void ForceHideWaitingScreen()
		{
			this.waitingPanel.Deactivate();
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x00033DE2 File Offset: 0x00031FE2
		private void ShowConnectionProblemPanel(Exception error)
		{
			this.waitingPanel.Deactivate();
			this.connectionProblemPanel.Activate(error);
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0009693C File Offset: 0x00094B3C
		public void ShowStats(PlayerStats stats)
		{
			if (PlayerInfo.me.CurrentLobbyRoom == null)
			{
				this.mainLobby.Deactivate();
			}
			else if (this.gameRoomPanel)
			{
				this.gameRoomPanel.Hide();
			}
			else
			{
				this.gameRoomPanelMobile.Hide();
			}
			this.playerStatsScreen.Activate(stats, this);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00096994 File Offset: 0x00094B94
		public void ShowCurrentPlayerStats()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_player_name);
			this.mainLobby.Deactivate();
			if (this.gameRoomPanel)
			{
				this.gameRoomPanel.Hide();
			}
			else
			{
				this.gameRoomPanelMobile.Hide();
			}
			this.playerStatsScreen.Activate(PlayerInfo.me.PlayerStats, this);
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00033DFB File Offset: 0x00031FFB
		public void MinimizeGameRoom()
		{
			this.gameRoomPanel.Hide();
			this.mainLobby.Activate();
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00033E13 File Offset: 0x00032013
		public void ReturnToGameRoom()
		{
			this.gameRoomPanel.Restore();
			this.mainLobby.Deactivate();
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00033E2B File Offset: 0x0003202B
		public void ReturnedToMainLobbyFromPlayerStats()
		{
			if (PlayerInfo.me.CurrentLobbyRoom == null)
			{
				this.mainLobby.Activate();
				return;
			}
			if (this.gameRoomPanel)
			{
				this.gameRoomPanel.Restore();
				return;
			}
			this.gameRoomPanelMobile.Restore();
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00033D34 File Offset: 0x00031F34
		private void OnCurrentPlayerStatsLoaded()
		{
			this.HideWaitingScreenIfAble();
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00033E69 File Offset: 0x00032069
		private void StopCoroutines()
		{
			this.playerListPanel.StopCoroutines();
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x000969FC File Offset: 0x00094BFC
		private void ExitGame()
		{
			if (!MultiplayerController.Instance.Disconnected)
			{
				RequestController.RequestPutCall("Lobby/ExitGame", JsonConvert.SerializeObject(new Data()), true, delegate(string s)
				{
				}, delegate(Exception exception)
				{
				});
			}
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00096A6C File Offset: 0x00094C6C
		private void RemoveDelegates()
		{
			LobbyLogic.Instance.PlayersListLogic.PlayersListDownloaded -= this.OnPlayerlistDownloaded;
			this.gameListPanel.RoomListLoaded -= this.OnGameListDownloaded;
			LobbyLogic.Instance.CurrentPlayerStatsLoaded -= this.OnCurrentPlayerStatsLoaded;
			if (this.gameRoomPanel)
			{
				this.gameRoomPanel.PlayerKicked -= this.OnPlayerKicked;
			}
			else
			{
				this.gameRoomPanelMobile.PlayerKicked -= this.OnPlayerKicked;
			}
			this.playerListPanel.RemoveDelegates();
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00033E76 File Offset: 0x00032076
		public void LoadGameScene()
		{
			GameController.gameFromSave = false;
			base.StartCoroutine(this.LoadMainScene());
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00033E8B File Offset: 0x0003208B
		private IEnumerator LoadMainScene()
		{
			MultiplayerController.Instance.IsLoading = true;
			base.GetComponent<Canvas>().enabled = false;
			SceneController.Instance.LoadScene(SceneController.SCENE_MAIN_NAME);
			yield return null;
			yield break;
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x00033E9A File Offset: 0x0003209A
		public void BreakLoadingAndReturnToMenu()
		{
			if (!LobbyLogic.Instance.PlayersListDownloaded() || !LobbyLogic.Instance.GameListDownloaded || !LobbyLogic.Instance.PlayerStatsDownloaded)
			{
				this.OnBackToMainScreen();
			}
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x00033EC6 File Offset: 0x000320C6
		public void OnBackToMainScreen()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			SceneController.Instance.LoadScene(SceneController.SCENE_MENU_NAME);
		}

		// Token: 0x04000DF0 RID: 3568
		[SerializeField]
		private PlayerListPanel playerListPanel;

		// Token: 0x04000DF1 RID: 3569
		[SerializeField]
		private GameListPanel gameListPanel;

		// Token: 0x04000DF2 RID: 3570
		[SerializeField]
		private GameRoom gameRoomPanel;

		// Token: 0x04000DF3 RID: 3571
		[SerializeField]
		private GameRoomMobile gameRoomPanelMobile;

		// Token: 0x04000DF4 RID: 3572
		[SerializeField]
		private MainLobbyScreen mainLobby;

		// Token: 0x04000DF5 RID: 3573
		[SerializeField]
		private PlayerStatsScreen playerStatsScreen;

		// Token: 0x04000DF6 RID: 3574
		[SerializeField]
		private WaitingPanel waitingPanel;

		// Token: 0x04000DF7 RID: 3575
		[SerializeField]
		private ConnectionProblemPanel connectionProblemPanel;

		// Token: 0x04000DF8 RID: 3576
		[SerializeField]
		private JoinRoomErrorPanel joinRoomErrorPanel;

		// Token: 0x04000DF9 RID: 3577
		[SerializeField]
		private GameInterruptedPopup gameInterruptedPopup;

		// Token: 0x04000DFA RID: 3578
		[SerializeField]
		private ReconnectPanel playAndStayReconnectPanel;

		// Token: 0x04000DFB RID: 3579
		[SerializeField]
		private ReconnectPanel playAndGoReconnectPanel;

		// Token: 0x04000DFC RID: 3580
		[SerializeField]
		private InvitationPanel invitationPanel;

		// Token: 0x04000DFD RID: 3581
		[SerializeField]
		private ForfeitGamePanel forfeitGamePanel;

		// Token: 0x04000DFE RID: 3582
		[SerializeField]
		private RemoveFriendPanel removeFriendPanel;

		// Token: 0x04000DFF RID: 3583
		private bool appUnpaused;

		// Token: 0x04000E00 RID: 3584
		private DateTime pauseDateTime;

		// Token: 0x04000E01 RID: 3585
		private int pauseSelectionSlot = -1;

		// Token: 0x04000E02 RID: 3586
		private TimeSpan diffTimeSpan;

		// Token: 0x04000E03 RID: 3587
		private DateTime lostFocusDateTime;
	}
}

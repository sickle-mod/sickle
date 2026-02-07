using System;
using System.Collections.Generic;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;
using Scythe.Multiplayer.Messages;
using Scythe.UI;
using Scythe.Utilities;

namespace Scythe.Multiplayer
{
	// Token: 0x02000259 RID: 601
	public sealed class LobbyLogic
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06001262 RID: 4706 RVA: 0x0003400B File Offset: 0x0003220B
		// (set) Token: 0x06001263 RID: 4707 RVA: 0x00034013 File Offset: 0x00032213
		public FriendsLogic FriendsLogic { get; private set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x0003401C File Offset: 0x0003221C
		// (set) Token: 0x06001265 RID: 4709 RVA: 0x00034024 File Offset: 0x00032224
		public PlayerListLogic PlayersListLogic { get; private set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06001266 RID: 4710 RVA: 0x0003402D File Offset: 0x0003222D
		// (set) Token: 0x06001267 RID: 4711 RVA: 0x00034035 File Offset: 0x00032235
		public bool PlayerStatsDownloaded { get; private set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x0003403E File Offset: 0x0003223E
		// (set) Token: 0x06001269 RID: 4713 RVA: 0x00034046 File Offset: 0x00032246
		public bool GameListDownloaded { get; private set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600126A RID: 4714 RVA: 0x0003404F File Offset: 0x0003224F
		// (set) Token: 0x0600126B RID: 4715 RVA: 0x00034057 File Offset: 0x00032257
		public bool GameStarted { get; private set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x00034060 File Offset: 0x00032260
		// (set) Token: 0x0600126D RID: 4717 RVA: 0x00034068 File Offset: 0x00032268
		public bool ReconnectingInProgress { get; private set; }

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x0600126E RID: 4718 RVA: 0x00096C6C File Offset: 0x00094E6C
		// (remove) Token: 0x0600126F RID: 4719 RVA: 0x00096CA4 File Offset: 0x00094EA4
		public event global::System.Action CurrentPlayerStatsLoaded;

		// Token: 0x06001271 RID: 4721 RVA: 0x0003407D File Offset: 0x0003227D
		private LobbyLogic()
		{
			this.FriendsLogic = new FriendsLogic();
			this.PlayersListLogic = new PlayerListLogic();
			this.PlayerStatsDownloaded = false;
			this.GameListDownloaded = false;
			this.GameStarted = false;
			this.ReconnectingInProgress = false;
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x000340B7 File Offset: 0x000322B7
		public static LobbyLogic Instance
		{
			get
			{
				return LobbyLogic.instance;
			}
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x000340BE File Offset: 0x000322BE
		public void Clear()
		{
			this.PlayerStatsDownloaded = false;
			this.GameListDownloaded = false;
			this.GameStarted = false;
			this.ReconnectingInProgress = false;
			this.FriendsLogic.Clear();
			this.PlayersListLogic.Clear();
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x000340F2 File Offset: 0x000322F2
		public void RefreshPlayerStats()
		{
			this.PlayerStatsDownloaded = false;
			this.GetCurrentPlayerStats();
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00034101 File Offset: 0x00032301
		public bool PlayersListDownloaded()
		{
			return this.PlayersListLogic.IsPlayersListDownloaded();
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x0003410E File Offset: 0x0003230E
		public bool LobbyLoaded()
		{
			return this.PlayerStatsDownloaded && this.GameListDownloaded && this.PlayerStatsDownloaded;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00096CDC File Offset: 0x00094EDC
		public void FirstJoinToLobby(Action<string> onSuccess, Action<Exception> onError)
		{
			LobbyRestAPI.JoinLobby(delegate(string response)
			{
				PlayerInfo.me.CurrentLobbyRoom = null;
				PlayerInfo.me.RoomId = GameSerializer.GetInnerTextFromXmlDocument(response);
				PlayerInfo.me.IsReady = false;
				PlayerInfo.me.IsAdmin = false;
				this.GetCurrentPlayerStats();
				onSuccess(response);
			}, delegate(Exception e)
			{
				onError(e);
			});
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00096D24 File Offset: 0x00094F24
		public void JoinToLobby(Action<string> onSuccess, Action<Exception> onError)
		{
			LobbyRestAPI.JoinLobby(delegate(string response)
			{
				PlayerInfo.me.CurrentLobbyRoom = null;
				PlayerInfo.me.RoomId = GameSerializer.GetInnerTextFromXmlDocument(response);
				PlayerInfo.me.IsReady = false;
				PlayerInfo.me.IsAdmin = false;
				onSuccess(response);
			}, delegate(Exception e)
			{
				onError(e);
			});
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00096D64 File Offset: 0x00094F64
		public void JoinRoom(string roomId, Action<string> onSuccess, Action<JoinRoomErrorStatus> onError)
		{
			LobbyRestAPI.JoinRoom(roomId, delegate(string response)
			{
				LobbyGame lobbyGame = GameSerializer.DeserializeObject<LobbyGame>(response);
				PlayerInfo.me.CurrentLobbyRoom = lobbyGame;
				PlayerInfo.me.RoomId = roomId;
				PlayerInfo.me.IsAdmin = lobbyGame.AdminId == PlayerInfo.me.PlayerStats.Id;
				PlayerInfo.me.IsReady = lobbyGame.AdminId == PlayerInfo.me.PlayerStats.Id;
				onSuccess(response);
			}, delegate(Exception e)
			{
				onError(this.GetErrorStatusFromResponse<JoinRoomErrorStatus>(e.Message, JoinRoomErrorStatus.UnknownError));
			});
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00096DB8 File Offset: 0x00094FB8
		public void LeaveRoom(Action<string> onSuccess, Action<Exception> onError)
		{
			LobbyRestAPI.LeaveRoom(delegate(string response)
			{
				PlayerInfo.me.IsAdmin = false;
				PlayerInfo.me.IsReady = false;
				PlayerInfo.me.CurrentLobbyRoom = null;
				PlayerInfo.me.RoomId = string.Empty;
				onSuccess(response);
			}, delegate(Exception e)
			{
				onError(e);
			});
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00096DF8 File Offset: 0x00094FF8
		public void LeaveRoomAndJoinLobby(Action<string> onSuccess, Action<Exception> onError)
		{
			LobbyRestAPI.LeaveRoomAndJoinLobby(delegate(string response)
			{
				PlayerInfo.me.CurrentLobbyRoom = null;
				PlayerInfo.me.RoomId = GameSerializer.GetInnerTextFromXmlDocument(response);
				PlayerInfo.me.IsReady = false;
				PlayerInfo.me.IsAdmin = false;
				onSuccess(response);
			}, delegate(Exception e)
			{
				onError(e);
			});
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00096E38 File Offset: 0x00095038
		public void CreateAndJoinRoom(LobbyRoom roomData, Action<string> onSuccess, Action<JoinRoomErrorStatus> onError)
		{
			LobbyRestAPI.CreateAndJoinRoom(roomData, delegate(string response)
			{
				LobbyGame lobbyGame = GameSerializer.DeserializeObject<LobbyGame>(response);
				PlayerInfo.me.CurrentLobbyRoom = lobbyGame;
				PlayerInfo.me.RoomId = lobbyGame.RoomId;
				onSuccess(response);
			}, delegate(Exception error)
			{
				onError(this.GetErrorStatusFromResponse<JoinRoomErrorStatus>(error.Message, JoinRoomErrorStatus.UnknownError));
			});
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00096E80 File Offset: 0x00095080
		public void QuickPlay(Preferences preferences, Action<string> onSuccess, Action<QuickPlayErrorStatus> onError)
		{
			LobbyRestAPI.QuickPlay(preferences, delegate(string response)
			{
				string innerTextFromXmlDocument = GameSerializer.GetInnerTextFromXmlDocument(response);
				onSuccess(innerTextFromXmlDocument);
			}, delegate(Exception error)
			{
				onError(this.GetErrorStatusFromResponse<QuickPlayErrorStatus>(error.Message, QuickPlayErrorStatus.UnknownError));
			});
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00096EC8 File Offset: 0x000950C8
		public void Reconnect(string gameId, int timeLeft, bool ranked, GameType gameType, string thirdPartyPlayerId, Action<string> onSuccess, Action<Exception> onError)
		{
			if (this.ReconnectingInProgress)
			{
				onError(new Exception("Reconnecting in progress."));
				return;
			}
			ReconnectData data = new ReconnectData(gameId, timeLeft, ranked, gameType, thirdPartyPlayerId);
			this.ReconnectingInProgress = true;
			LobbyRestAPI.Reconnect(data, delegate(string response)
			{
				this.ReconnectingInProgress = false;
				GameData gameData = GameSerializer.DeserializeObject<GameData>(response);
				this.InitGame(gameData, data.RoomId);
				onSuccess(response);
			}, delegate(Exception error)
			{
				this.ReconnectingInProgress = false;
				onError(error);
			});
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00096F4C File Offset: 0x0009514C
		public void SpectateGame(int faction, string roomId, Action<string> onSuccess, Action<Exception> onError)
		{
			SpectateGame data = new SpectateGame(faction, roomId);
			LobbyRestAPI.SpectateGame(data, delegate(string response)
			{
				GameData gameData = GameSerializer.DeserializeObject<GameData>(response);
				this.InitObserverGame(gameData, data.RoomId);
				onSuccess(response);
			}, delegate(Exception exception)
			{
				onError(exception);
			});
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00034128 File Offset: 0x00032328
		public void GetCurrentPlayerStats()
		{
			LobbyRestAPI.GetPlayerStats(PlayerInfo.me.PlayerStats.Id, delegate(string response)
			{
				string name = PlayerInfo.me.PlayerStats.Name;
				PlayerInfo.me.PlayerStats = PlayerStats.FromJson(response);
				PlayerInfo.me.PlayerStats.Name = name;
				PlayerInfo.me.IsLoaded = true;
				this.PlayerStatsDownloaded = true;
				if (GameServiceController.Instance.InvadersFromAfarUnlocked())
				{
					AchievementManager.CheckMultiplayerAchievements();
				}
				if (this.CurrentPlayerStatsLoaded != null)
				{
					this.CurrentPlayerStatsLoaded();
				}
			});
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00096FA8 File Offset: 0x000951A8
		public void GetPlayersDataForRanking(int[] rankingIds, Action<UserSearchResult[]> onSuccess, Action<WebError> onError)
		{
			new SearchByIdEndpoint(rankingIds, Extras.None, -1, -1, null).Execute(delegate(PaginatedResult<UserSearchResult> response, WebError error)
			{
				if (error == null)
				{
					onSuccess((response.TotalElement == 0) ? new UserSearchResult[0] : response.Elements);
					return;
				}
				onError(error);
			});
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0003414A File Offset: 0x0003234A
		public void OnGameStarted()
		{
			this.GameStarted = true;
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00034153 File Offset: 0x00032353
		public void OnGameListDownloaded()
		{
			this.GameListDownloaded = true;
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00096FE4 File Offset: 0x000951E4
		private void InitObserverGame(GameData gameData, string gameId)
		{
			Scythe.GameLogic.Game game = GameController.Game;
			game.LoadFromString(gameData.GameState);
			game.GameManager.StartSpectatorMode();
			List<int> list = new List<int>();
			if (!string.IsNullOrEmpty(gameData.LeaversFactions))
			{
				foreach (string text in gameData.LeaversFactions.Split(' ', StringSplitOptions.None))
				{
					list.Add(int.Parse(text));
				}
			}
			using (List<Player>.Enumerator enumerator = game.GameManager.GetPlayers().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Player player = enumerator.Current;
					if (!player.IsHuman)
					{
						PlayerData playerData = new PlayerData();
						playerData.PlayerClock = 0;
						playerData.Faction = (int)player.matFaction.faction;
						if (list.Contains(playerData.Faction))
						{
							playerData.Id = PlayerData.LeaverId;
						}
						else
						{
							playerData.Id = PlayerData.BotId;
						}
						playerData.HasTurn = false;
						playerData.MapLoaded = true;
						playerData.Name = "Bot";
						MultiplayerController.Instance.AddPlayer(playerData);
					}
					else
					{
						PlayerData playerData2 = gameData.Players.Find((PlayerData p) => p.Faction == (int)player.matFaction.faction);
						MultiplayerController.Instance.AddPlayer(playerData2);
					}
				}
			}
			PlayerInfo.me.RoomId = gameId;
			MultiplayerController.Instance.InitObserverGame(game.GameManager, gameData.MessageCounter);
			this.GameStarted = true;
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00097180 File Offset: 0x00095380
		private void InitGame(GameData gameData, string gameId)
		{
			Scythe.GameLogic.Game game = GameController.Game;
			game.LoadFromString(gameData.GameState);
			game.GameManager.SetAmountOfCombatCardsLeft();
			List<int> list = new List<int>();
			if (!string.IsNullOrEmpty(gameData.LeaversFactions))
			{
				foreach (string text in gameData.LeaversFactions.Split(' ', StringSplitOptions.None))
				{
					list.Add(int.Parse(text));
				}
			}
			using (List<Player>.Enumerator enumerator = game.GameManager.GetPlayers().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Player player = enumerator.Current;
					if (!player.IsHuman)
					{
						PlayerData playerData = new PlayerData();
						playerData.PlayerClock = 0;
						playerData.Faction = (int)player.matFaction.faction;
						if (list.Contains(playerData.Faction))
						{
							playerData.Id = PlayerData.LeaverId;
						}
						else
						{
							playerData.Id = PlayerData.BotId;
						}
						playerData.HasTurn = false;
						playerData.MapLoaded = true;
						playerData.Name = "Bot";
						MultiplayerController.Instance.AddPlayer(playerData);
					}
					else
					{
						PlayerData playerData2 = gameData.Players.Find((PlayerData p) => p.Faction == (int)player.matFaction.faction);
						if (playerData2.Id == PlayerInfo.me.PlayerStats.Id)
						{
							PlayerInfo.me.Faction = playerData2.Faction;
							PlayerInfo.me.RoomId = gameId;
							game.GameManager.SetOwnerIdFromFaction((Faction)playerData2.Faction);
						}
						MultiplayerController.Instance.AddPlayer(playerData2);
					}
				}
			}
			MultiplayerController.Instance.InitAfterReturnToGame(game.GameManager, gameData.PlayerClock, gameData.MessageCounter);
			List<ChatMessage> list2 = JsonConvert.DeserializeObject<List<ChatMessage>>(gameData.ChatInJsonFormat);
			MultiplayerController.Instance.RestoredChatMessages(list2);
			this.GameStarted = true;
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00097388 File Offset: 0x00095588
		private T GetErrorStatusFromResponse<T>(string response, T defaultStatus)
		{
			T t;
			try
			{
				t = JsonConvert.DeserializeObject<LobbyErrorData<T>>(response).ErrorStatus;
			}
			catch
			{
				t = defaultStatus;
			}
			return t;
		}

		// Token: 0x04000E15 RID: 3605
		private static readonly LobbyLogic instance = new LobbyLogic();
	}
}

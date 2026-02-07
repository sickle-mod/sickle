using System;
using System.Collections.Generic;
using System.Net;
using System.Timers;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer.Data;
using Scythe.Multiplayer.Messages;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x02000282 RID: 642
	public class MultiplayerController
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06001441 RID: 5185 RVA: 0x00035A50 File Offset: 0x00033C50
		// (set) Token: 0x06001442 RID: 5186 RVA: 0x00035A58 File Offset: 0x00033C58
		public bool Disconnected { get; set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x00035A61 File Offset: 0x00033C61
		// (set) Token: 0x06001444 RID: 5188 RVA: 0x00035A69 File Offset: 0x00033C69
		public bool RunOutOfTime { get; set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x00035A72 File Offset: 0x00033C72
		// (set) Token: 0x06001446 RID: 5190 RVA: 0x00035A7A File Offset: 0x00033C7A
		public bool ReturningToStartedGame { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06001447 RID: 5191 RVA: 0x00035A83 File Offset: 0x00033C83
		// (set) Token: 0x06001448 RID: 5192 RVA: 0x00035A8B File Offset: 0x00033C8B
		public bool IsLoading { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x00035A94 File Offset: 0x00033C94
		// (set) Token: 0x0600144A RID: 5194 RVA: 0x00035A9C File Offset: 0x00033C9C
		public bool IsFactionPresentationInProgress { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x00035AA5 File Offset: 0x00033CA5
		// (set) Token: 0x0600144C RID: 5196 RVA: 0x00035AAD File Offset: 0x00033CAD
		public bool WaitingForCombatCards { get; private set; }

		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x0600144D RID: 5197 RVA: 0x0009AF04 File Offset: 0x00099104
		// (remove) Token: 0x0600144E RID: 5198 RVA: 0x0009AF38 File Offset: 0x00099138
		public static event MultiplayerController.ChatMessageReceived OnChatMessageReceived;

		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x0600144F RID: 5199 RVA: 0x0009AF6C File Offset: 0x0009916C
		// (remove) Token: 0x06001450 RID: 5200 RVA: 0x0009AFA0 File Offset: 0x000991A0
		public static event MultiplayerController.ChatMessagesRestored OnChatMessageRestored;

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x06001451 RID: 5201 RVA: 0x0009AFD4 File Offset: 0x000991D4
		// (remove) Token: 0x06001452 RID: 5202 RVA: 0x0009B008 File Offset: 0x00099208
		public static event global::System.Action OnPlayerReconnected;

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06001453 RID: 5203 RVA: 0x00035AB6 File Offset: 0x00033CB6
		public PlayerData GetActivePlayer
		{
			get
			{
				return this.playersInGame[this.activePlayerId];
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x00035AC9 File Offset: 0x00033CC9
		public PlayerData GetSecondActivePlayer
		{
			get
			{
				if (this.secondActivePlayerId != -1)
				{
					return this.playersInGame[this.secondActivePlayerId];
				}
				return null;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x00035AE7 File Offset: 0x00033CE7
		public PlayerData GetOwnerPlayer
		{
			get
			{
				return this.playersInGame[this.ownerPlayerId];
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x00035AFA File Offset: 0x00033CFA
		public int StartingPlayerClock
		{
			get
			{
				return this.startingPlayerclock;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x00035B02 File Offset: 0x00033D02
		public bool SpectatorMode
		{
			get
			{
				return this.gameManager != null && this.gameManager.SpectatorMode;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x00035B19 File Offset: 0x00033D19
		public bool IsMultiplayer
		{
			get
			{
				return this.gameManager != null && this.gameManager.IsMultiplayer;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x00035B30 File Offset: 0x00033D30
		public bool Asynchronous
		{
			get
			{
				return this.gameManager != null && this.gameManager.IsAsynchronous;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x00035B47 File Offset: 0x00033D47
		public bool Ranked
		{
			get
			{
				return this.gameManager != null && this.gameManager.IsRanked;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x00035B5E File Offset: 0x00033D5E
		public bool InLobby
		{
			get
			{
				return this.gameManager == null;
			}
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00035B69 File Offset: 0x00033D69
		private MultiplayerController()
		{
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0009B03C File Offset: 0x0009923C
		public void InitMultiplayer()
		{
			this.Clear();
			this.Disconnected = false;
			this.RunOutOfTime = false;
			this.ReturningToStartedGame = false;
			this.playersInGame.Clear();
			this.checkForLeaversTimer = new Timer(30000.0);
			this.checkForLeaversTimer.Elapsed += this.CheckForLeavers;
			this.checkForLeaversTimer.Start();
			this.sendMessageTimer = new Timer(1500.0);
			this.sendMessageTimer.Elapsed += this.SendMessage;
			this.sendMessageTimer.AutoReset = false;
			this.sendMessageTimer.Start();
			this.getUpdateTimer = new Timer(1500.0);
			this.getUpdateTimer.Elapsed += this.GetUpdate;
			this.getUpdateTimer.Start();
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0009B120 File Offset: 0x00099320
		public void StartMultiplayer(GameManager gameManager)
		{
			this.gameManager = gameManager;
			gameManager.IsMultiplayer = true;
			ConnectionProblem.Init();
			gameManager.GainedCombatCards += this.GetCombatCardsFromServer;
			gameManager.GainedFactoryCards += this.GainFactoryCards;
			gameManager.CardChoosen += this.ChoosenFactoryCard;
			gameManager.EncounterButtonClicked += this.GetEncounterCard;
			gameManager.ActionWasSent += this.SendActionToOtherPlayers;
			gameManager.ChangeTurn += this.NextTurn;
			gameManager.ActivePlayerChanged += this.SetCurrentPlayer;
			gameManager.SecondActivePlayerChanged += this.SetSecondActivePlayer;
			gameManager.ActivePlayerDisabled += this.DisableActivePlayer;
			this.secondActivePlayerId = -1;
			PlayerClock.InitTimer();
			this.updateTimer = new Timer(100.0);
			this.updateTimer.Elapsed += this.Update;
			this.updateTimer.AutoReset = false;
			this.updateTimer.Start();
			this.messageExecutor = new MessageExecutor();
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x00035B91 File Offset: 0x00033D91
		public void InitAfterReturnToGame(GameManager gameManager, int playerClock, int messageCounter)
		{
			this.StartMultiplayer(gameManager);
			this.AdjustCurrentPlayer();
			this.AdjustOwnerPlayer();
			this.SetStartingPlayerClock(playerClock);
			RequestController.SetCounter(messageCounter);
			this.ReturningToStartedGame = this.AllPlayersLoaded() || gameManager.GameStarted;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x00035BCA File Offset: 0x00033DCA
		public void InitObserverGame(GameManager gameManager, int messageCounter)
		{
			this.StartMultiplayer(gameManager);
			this.AdjustCurrentPlayer();
			RequestController.SetCounter(messageCounter);
			this.ReturningToStartedGame = true;
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x00035BE6 File Offset: 0x00033DE6
		public static MultiplayerController Instance
		{
			get
			{
				if (MultiplayerController.instance == null)
				{
					MultiplayerController.instance = new MultiplayerController();
				}
				return MultiplayerController.instance;
			}
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0009B23C File Offset: 0x0009943C
		private void Update(object sender, ElapsedEventArgs e)
		{
			try
			{
				if (!this.IsLoading && !this.IsFactionPresentationInProgress)
				{
					while (RequestController.serverUpdates.Count > 0 && !ShowEnemyMoves.Instance.MoreAnimations())
					{
						if (this.Disconnected)
						{
							RequestController.serverUpdates.Clear();
							break;
						}
						Scythe.Multiplayer.Messages.Message message = RequestController.serverUpdates.Dequeue();
						if (message is IExecutableMessage)
						{
							this.messageExecutor.ExecuteMessage(message as IExecutableMessage, this.gameManager);
						}
						if (!(message is ChatMessage))
						{
							this.ResetCheckForLeaversTimer();
						}
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (!this.Disconnected)
				{
					this.updateTimer.Start();
				}
			}
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x00035BFE File Offset: 0x00033DFE
		public void ReceivedChatMessage(ChatMessage message)
		{
			if (MultiplayerController.OnChatMessageReceived != null)
			{
				MultiplayerController.OnChatMessageReceived(message);
			}
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x00035C12 File Offset: 0x00033E12
		public void RestoredChatMessages(List<ChatMessage> messages)
		{
			if (MultiplayerController.OnChatMessageRestored != null)
			{
				MultiplayerController.OnChatMessageRestored(messages);
			}
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x00035C26 File Offset: 0x00033E26
		public void SendActionToOtherPlayers(Scythe.Multiplayer.Messages.Message message)
		{
			RequestController.AddAction(message);
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x00035C2E File Offset: 0x00033E2E
		public void SendMessageToOtherPlayers(ChatMessage message)
		{
			if (this.sendMessageTimer != null)
			{
				RequestController.AddAction(message);
			}
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x00035C3E File Offset: 0x00033E3E
		public void GetCombatCardsFromServer(short amount, GainCombatCard.CombatCardGainType type)
		{
			if (this.SpectatorMode)
			{
				return;
			}
			RequestController.AddAction(new GetCombatCardsMessage(amount, (int)type));
			this.WaitingForCombatCards = true;
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x00035C5C File Offset: 0x00033E5C
		public void CombatCardsReceived()
		{
			this.WaitingForCombatCards = false;
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x00035C65 File Offset: 0x00033E65
		public void GainFactoryCards()
		{
			RequestController.AddAction(new GetFactoryCardsMessage());
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x00035C71 File Offset: 0x00033E71
		public void ChoosenFactoryCard(int index)
		{
			RequestController.AddAction(new GetFactoryMessage(index));
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x00035C7E File Offset: 0x00033E7E
		public void GetEncounterCard()
		{
			RequestController.AddAction(new GetEncounterCardMessage());
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x00035C8A File Offset: 0x00033E8A
		public void GetServerGameState()
		{
			RequestController.AddAction(new GetGameStateMessage());
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x00035C96 File Offset: 0x00033E96
		public void UpdatePlayerClockOnServer()
		{
			if (this.SpectatorMode)
			{
				return;
			}
			RequestController.SendMessage<Scythe.Multiplayer.Data.Game>("Scythe/UpdateTime", new Scythe.Multiplayer.Data.Game());
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0009B2F8 File Offset: 0x000994F8
		private void CheckForLeavers(object sender, ElapsedEventArgs e)
		{
			if (this.gameManager == null)
			{
				LobbyRestAPI.CheckForLeavers(delegate(string response)
				{
				}, null);
				return;
			}
			if (this.SpectatorMode)
			{
				return;
			}
			RequestController.SendMessage<Scythe.Multiplayer.Data.Game>("Scythe/CheckForLeavers", new Scythe.Multiplayer.Data.Game());
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0009B34C File Offset: 0x0009954C
		private void ForceStart(object sender, ElapsedEventArgs e)
		{
			if (this.SpectatorMode)
			{
				return;
			}
			ForceStartMessage forceStartMessage = new ForceStartMessage();
			RequestController.AddAction(forceStartMessage);
			RequestController.serverUpdates.Enqueue(forceStartMessage);
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x00035CB0 File Offset: 0x00033EB0
		private void ResetCheckForLeaversTimer()
		{
			this.checkForLeaversTimer.Stop();
			this.checkForLeaversTimer.Start();
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0009B37C File Offset: 0x0009957C
		public void MyMapLoaded()
		{
			this.IsLoading = false;
			if (!this.ReturningToStartedGame)
			{
				RequestController.AddAction(new MapLoadedMessage());
				if (this.AllPlayersLoaded())
				{
					this.StartGame();
				}
			}
			else
			{
				this.StartGame();
				this.ReturningToStartedGame = false;
			}
			this.PlayerMapLoaded(PlayerInfo.me.PlayerStats.Id);
			PlayerInfo.me.MapLoaded = true;
			if (this.gameManager.IsAsynchronous && !this.AllPlayersLoaded())
			{
				this.forceStartTimer = new Timer(15000.0);
				this.forceStartTimer.Elapsed += this.ForceStart;
				this.forceStartTimer.AutoReset = false;
				this.forceStartTimer.Start();
			}
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x00035CC8 File Offset: 0x00033EC8
		private void StartGame()
		{
			if (!this.gameManager.GameStarted || this.ReturningToStartedGame)
			{
				PlayerClock.StartTimer();
				this.SetCurrentPlayer();
				this.gameManager.StartGame();
				this.PlayStartSoundForFirstPlayer();
			}
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x00035CFB File Offset: 0x00033EFB
		private void PlayStartSoundForFirstPlayer()
		{
			if (this.gameManager.IsMyTurn())
			{
				WorldSFXManager.PlaySound(SoundEnum.RoundStart1, AudioSourceType.StartTurnTheme);
			}
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0009B438 File Offset: 0x00099638
		public void ForceStart()
		{
			foreach (PlayerData playerData in this.playersInGame)
			{
				if (!playerData.MapLoaded)
				{
					this.PlayerMapLoaded(playerData.Id);
				}
			}
			this.StartGame();
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0009B4A0 File Offset: 0x000996A0
		public void PlayerMapLoaded(Guid playerId)
		{
			PlayerData playerData = this.playersInGame.Find((PlayerData player) => player.Id == playerId);
			this.PlayerMapLoaded(playerData);
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0009B4DC File Offset: 0x000996DC
		public void PlayerMapLoadedByFaction(int faction)
		{
			PlayerData playerData = this.playersInGame.Find((PlayerData player) => player.Faction == faction);
			this.PlayerMapLoaded(playerData);
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0009B518 File Offset: 0x00099718
		private void PlayerMapLoaded(PlayerData player)
		{
			if (player != null)
			{
				if (player.MapLoaded)
				{
					return;
				}
				player.MapLoaded = true;
			}
			if (this.AllPlayersLoaded() && !this.gameManager.GameStarted)
			{
				if (this.forceStartTimer != null)
				{
					this.forceStartTimer.Stop();
				}
				this.StartGame();
			}
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0009B568 File Offset: 0x00099768
		public bool AllPlayersLoaded()
		{
			using (List<PlayerData>.Enumerator enumerator = this.playersInGame.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.MapLoaded)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x00035D12 File Offset: 0x00033F12
		private void GetUpdate(object sender, ElapsedEventArgs e)
		{
			RequestController.GetUpdate();
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0009B5C4 File Offset: 0x000997C4
		private void SendMessage(object sender, ElapsedEventArgs e)
		{
			try
			{
				RequestController.SendMessage();
			}
			catch (WebException ex)
			{
				Debug.LogWarning(ex.ToString());
			}
			finally
			{
				if (this.sendMessageTimer != null)
				{
					this.sendMessageTimer.Start();
				}
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00035D19 File Offset: 0x00033F19
		public void CloseMultiplayer()
		{
			if (this.gameManager.SpectatorMode)
			{
				this.SendLeaveSpectatorModeRequest();
			}
			else if (!this.Disconnected)
			{
				RequestController.SendMessage<Scythe.Multiplayer.Data.Game>("Scythe/ExitGame", new Scythe.Multiplayer.Data.Game());
			}
			this.Clear();
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x00035D4D File Offset: 0x00033F4D
		public void ReturnToLobby()
		{
			if (this.gameManager.SpectatorMode)
			{
				this.SendLeaveSpectatorModeRequest();
			}
			else if (!this.Disconnected)
			{
				RequestController.SendMessage<Scythe.Multiplayer.Data.Game>("Scythe/LeaveGame", new Scythe.Multiplayer.Data.Game());
			}
			this.Clear();
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x00035D81 File Offset: 0x00033F81
		public void LeaveTimeoutedSyncGame()
		{
			RequestController.SendMessage<Scythe.Multiplayer.Data.Game>("Scythe/LeaveTimeoutedSyncGame", new Scythe.Multiplayer.Data.Game());
			this.Clear();
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x00035D98 File Offset: 0x00033F98
		public void LeaveGame()
		{
			if (this.gameManager.SpectatorMode)
			{
				this.SendLeaveSpectatorModeRequest();
			}
			else if (!this.Disconnected)
			{
				RequestController.SendMessage<Scythe.Multiplayer.Data.Game>("Scythe/LeaveGame", new Scythe.Multiplayer.Data.Game());
			}
			this.ClearDelegates();
			this.ClearTimers();
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x00035DD2 File Offset: 0x00033FD2
		public void Forfeit()
		{
			if (!this.Disconnected)
			{
				RequestController.SendMessage<Scythe.Multiplayer.Data.Game>("Scythe/ForfeitGame", new Scythe.Multiplayer.Data.Game());
			}
			this.Clear();
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x00035DF1 File Offset: 0x00033FF1
		public void TryToReconnect()
		{
			this.getUpdateTimer.Stop();
			RequestController.Reset();
			ReconnectManager.TryToReconnect();
			ConnectionProblem.HideErrorPanel();
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0009B614 File Offset: 0x00099814
		public void NextTurn()
		{
			int num = this.activePlayerId + 1;
			this.activePlayerId = num;
			this.activePlayerId = num % this.playersInGame.Count;
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x00035E0D File Offset: 0x0003400D
		public void SetStartingPlayerClock(int playerClock)
		{
			this.startingPlayerclock = playerClock;
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x00035E16 File Offset: 0x00034016
		public IEnumerable<PlayerData> GetPlayersInGame()
		{
			return this.playersInGame;
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0009B644 File Offset: 0x00099844
		public PlayerData GetPlayerData(int faction)
		{
			return this.playersInGame.Find((PlayerData x) => x.Faction == faction);
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0009B678 File Offset: 0x00099878
		public PlayerData GetPlayerDataById(Guid id)
		{
			return this.playersInGame.Find((PlayerData player) => player.Id == id);
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0009B6AC File Offset: 0x000998AC
		public PlayerData GetPlayerAtOffsetFromOwner(int offset)
		{
			if (this.ownerPlayerId == -1)
			{
				return this.playersInGame[offset];
			}
			int num = this.ownerPlayerId + offset;
			if (num >= this.playersInGame.Count)
			{
				num -= this.playersInGame.Count;
			}
			return this.playersInGame[num];
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x00035E1E File Offset: 0x0003401E
		public void AddPlayer(PlayerData player)
		{
			this.playersInGame.Add(player);
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x00035E2C File Offset: 0x0003402C
		public void AdjustOwnerPlayer()
		{
			this.ownerPlayerId = this.playersInGame.FindIndex((PlayerData player) => player.Faction == (int)this.gameManager.PlayerOwner.matFaction.faction);
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x00035E4B File Offset: 0x0003404B
		public void AdjustCurrentPlayer()
		{
			this.activePlayerId = this.playersInGame.FindIndex((PlayerData player) => player.HasTurn);
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0009B700 File Offset: 0x00099900
		public void SetCurrentPlayer(int faction)
		{
			this.activePlayerId = this.playersInGame.FindIndex((PlayerData player) => player.Faction == faction);
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0009B738 File Offset: 0x00099938
		public void SetSecondActivePlayer(int faction)
		{
			this.secondActivePlayerId = this.playersInGame.FindIndex((PlayerData player) => player.Faction == faction);
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0009B770 File Offset: 0x00099970
		public void UpdatePlayerTimeLeft(int faction, int timeLeft)
		{
			this.playersInGame.Find((PlayerData player) => player.Faction == faction).PlayerClock = timeLeft;
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0009B7A8 File Offset: 0x000999A8
		public void OverridePlayerWithAi(int faction)
		{
			PlayerData playerData = this.playersInGame.Find((PlayerData player) => player.Faction == faction);
			playerData.Id = PlayerData.LeaverId;
			playerData.PlayerClock = 0;
			if (!playerData.MapLoaded)
			{
				this.PlayerMapLoaded(playerData.Id);
			}
			if (this.GetOwnerPlayer == playerData)
			{
				ConnectionProblem.ShowDisconnectedPanel();
			}
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x0009B810 File Offset: 0x00099A10
		public void OverrideAiWithPlayer(string name, Guid id, int faction, int playerClock)
		{
			PlayerData playerData = this.playersInGame.Find((PlayerData p) => p.Faction == faction);
			playerData.Id = id;
			playerData.Name = name;
			playerData.Faction = faction;
			playerData.PlayerClock = playerClock;
			playerData.IsOnline = true;
			if (MultiplayerController.OnPlayerReconnected != null)
			{
				MultiplayerController.OnPlayerReconnected();
			}
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x00035E7D File Offset: 0x0003407D
		public int NumberOfPlayersOverridedByAi()
		{
			return this.playersInGame.FindAll((PlayerData player) => player.Id == PlayerData.LeaverId).Count;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x00035EAE File Offset: 0x000340AE
		public void DisableActivePlayer(int faction)
		{
			if (this.GetActivePlayer.Faction != faction)
			{
				this.secondActivePlayerId = -1;
				return;
			}
			if (this.secondActivePlayerId == -1)
			{
				this.SetCurrentPlayer();
				return;
			}
			this.activePlayerId = this.secondActivePlayerId;
			this.secondActivePlayerId = -1;
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x00035EE9 File Offset: 0x000340E9
		public void SetCurrentPlayer()
		{
			this.activePlayerId = this.playersInGame.FindIndex((PlayerData player) => player.Faction == (int)this.gameManager.PlayerCurrent.matFaction.faction);
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x00035F08 File Offset: 0x00034108
		public void ClearInLobby()
		{
			this.getUpdateTimer.Dispose();
			this.sendMessageTimer.Dispose();
			this.checkForLeaversTimer.Dispose();
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0009B87C File Offset: 0x00099A7C
		private void SendLeaveSpectatorModeRequest()
		{
			RequestController.RequestDeleteCall(string.Format("{0}Spectator?gameId={1}", Uri.EscapeDataString("Game/"), PlayerInfo.me.RoomId), delegate(string s)
			{
			}, delegate(Exception exception)
			{
			});
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0009B8EC File Offset: 0x00099AEC
		private void Clear()
		{
			if (this.gameManager == null)
			{
				return;
			}
			this.ClearDelegates();
			this.ClearTimers();
			ConnectionProblem.Init();
			ReconnectManager.Init();
			this.playersInGame.Clear();
			PlayerInfo.me.MapLoaded = false;
			this.gameManager = null;
			this.WaitingForCombatCards = false;
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0009B93C File Offset: 0x00099B3C
		private void ClearTimers()
		{
			this.updateTimer.Stop();
			this.getUpdateTimer.Stop();
			this.checkForLeaversTimer.Stop();
			this.sendMessageTimer.Stop();
			if (this.forceStartTimer != null)
			{
				this.forceStartTimer.Stop();
				this.forceStartTimer.Dispose();
			}
			this.sendMessageTimer.Dispose();
			this.updateTimer.Dispose();
			this.getUpdateTimer.Dispose();
			this.checkForLeaversTimer.Dispose();
			PlayerClock.RemoveTimer();
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0009B9C4 File Offset: 0x00099BC4
		private void ClearDelegates()
		{
			this.gameManager.ActionWasSent -= this.SendActionToOtherPlayers;
			this.gameManager.GainedCombatCards -= this.GetCombatCardsFromServer;
			this.gameManager.GainedFactoryCards -= this.GainFactoryCards;
			this.gameManager.CardChoosen -= this.ChoosenFactoryCard;
			this.gameManager.EncounterButtonClicked -= this.GetEncounterCard;
			this.gameManager.ChangeTurn -= this.NextTurn;
			this.gameManager.ActivePlayerChanged -= this.SetCurrentPlayer;
			this.gameManager.SecondActivePlayerChanged -= this.SetSecondActivePlayer;
			this.gameManager.ActivePlayerDisabled -= this.DisableActivePlayer;
		}

		// Token: 0x04000EEF RID: 3823
		private static MultiplayerController instance;

		// Token: 0x04000EF0 RID: 3824
		public const string LOGIN_SERIVCE = "Login/";

		// Token: 0x04000EF1 RID: 3825
		public const string LOBBY_SERVICE = "Lobby/";

		// Token: 0x04000EF2 RID: 3826
		public const string SCYTHE_SERVICE = "Scythe/";

		// Token: 0x04000EF3 RID: 3827
		public const string FRIENDS_SERVICE = "Friends/";

		// Token: 0x04000EF4 RID: 3828
		public const string ROOM_SERVICE = "Room/";

		// Token: 0x04000EF5 RID: 3829
		public const string STRESS_TEST_SERVICE = "StressTest/";

		// Token: 0x04000EF6 RID: 3830
		public const string GAME_SERVICE = "Game/";

		// Token: 0x04000EF7 RID: 3831
		public const string DATA_REMOVER_SERVICE = "DataRemover/";

		// Token: 0x04000EF8 RID: 3832
		public const string RECONNECT_SERVICE = "Reconnect/";

		// Token: 0x04000EF9 RID: 3833
		public const string REVIEW_SERVICE = "Reviews/";

		// Token: 0x04000EFA RID: 3834
		public const string RELATIONSHIP_SERVICE = "RelationshipService.svc/";

		// Token: 0x04000EFB RID: 3835
		private Timer updateTimer;

		// Token: 0x04000EFC RID: 3836
		private Timer getUpdateTimer;

		// Token: 0x04000EFD RID: 3837
		private Timer sendMessageTimer;

		// Token: 0x04000EFE RID: 3838
		private Timer checkForLeaversTimer;

		// Token: 0x04000EFF RID: 3839
		private Timer forceStartTimer;

		// Token: 0x04000F09 RID: 3849
		public readonly List<PlayerData> playersInGame = new List<PlayerData>();

		// Token: 0x04000F0A RID: 3850
		private int activePlayerId = -1;

		// Token: 0x04000F0B RID: 3851
		private int secondActivePlayerId = -1;

		// Token: 0x04000F0C RID: 3852
		private int ownerPlayerId = -1;

		// Token: 0x04000F0D RID: 3853
		private int startingPlayerclock;

		// Token: 0x04000F0E RID: 3854
		private GameManager gameManager;

		// Token: 0x04000F0F RID: 3855
		private MessageExecutor messageExecutor;

		// Token: 0x02000283 RID: 643
		// (Invoke) Token: 0x0600149A RID: 5274
		public delegate void ChatMessageReceived(ChatMessage message);

		// Token: 0x02000284 RID: 644
		// (Invoke) Token: 0x0600149E RID: 5278
		public delegate void ChatMessagesRestored(List<ChatMessage> messages);
	}
}

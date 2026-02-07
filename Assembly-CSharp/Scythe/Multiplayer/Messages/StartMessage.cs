using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;
using Scythe.UI;
using Scythe.Utilities;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x02000300 RID: 768
	public class StartMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x06001668 RID: 5736 RVA: 0x000375F2 File Offset: 0x000357F2
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			this.ExecuteLogic(lobby);
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x000375F2 File Offset: 0x000357F2
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			this.ExecuteLogic(lobby);
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x000375FB File Offset: 0x000357FB
		private void ExecuteLogic(Lobby lobby)
		{
			lobby.OnGameStarted();
			this.InitGame(GameSerializer.DeserializeObject<GameInit>(this.data));
			lobby.LoadGameScene();
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x0009F850 File Offset: 0x0009DA50
		private void InitGame(GameInit data)
		{
			Scythe.GameLogic.Game game = GameController.Game;
			game.CreateNewGameManager();
			game.GameManager.InitMultiplayer();
			data.Players.Sort((PlayerInit p1, PlayerInit p2) => p1.Faction.CompareTo(p2.Faction));
			foreach (PlayerInit playerInit in data.Players)
			{
				PlayerData playerData = new PlayerData(playerInit.Name, playerInit.Id, playerInit.Faction, data.PlayerClockTime);
				if (PlayerInfo.me.PlayerStats.Id.Equals(playerData.Id))
				{
					PlayerInfo.me.Faction = playerInit.Faction;
				}
				MultiplayerController.Instance.AddPlayer(playerData);
				bool flag = playerInit.Id != PlayerData.BotId;
				game.GameManager.InitPlayer(playerInit.Faction, playerInit.Mat, playerInit.CombatCards, playerInit.ObjectiveCards, flag ? 0 : 2, playerData.Name);
			}
			game.GameManager.SortPlayers();
			int num = game.GameManager.GetPlayersFactions().IndexOf((Faction)PlayerInfo.me.Faction);
			game.GameManager.DisableTestingMode();
			game.GameManager.StartMultiplayer(data.NumberOfPlayers, num, data.StructureBonusCard, data.IsRanked, data.IsAsynchronous, PlayerInfo.me.CurrentLobbyRoom.IsPrivate, PlayerInfo.me.CurrentLobbyRoom.PromoCardsEnabled);
			MultiplayerController.Instance.StartMultiplayer(game.GameManager);
			MultiplayerController.Instance.AdjustOwnerPlayer();
			MultiplayerController.Instance.SetStartingPlayerClock(data.PlayerClockTime);
			MultiplayerController.Instance.RestoredChatMessages(new List<ChatMessage>(0));
			RequestController.Reset();
		}

		// Token: 0x04001082 RID: 4226
		private string data;
	}
}

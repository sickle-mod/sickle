using System;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002E6 RID: 742
	public class GameInProgressMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x0600161E RID: 5662 RVA: 0x00037329 File Offset: 0x00035529
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			this.ExecuteLogic(lobby);
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00037329 File Offset: 0x00035529
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			this.ExecuteLogic(lobby);
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x0009F438 File Offset: 0x0009D638
		private void ExecuteLogic(Lobby lobby)
		{
			if (this.invadersFromAfar && !this.CanPlayerJoinIfaGames())
			{
				return;
			}
			if (this.gameType == GameType.Synchronous)
			{
				lobby.ShowPlayAndStayReconnectPanel(this.roomId, this.timeLeftInSeconds, this.gameType, this.rankedGame);
				return;
			}
			lobby.ShowPlayAndGoReconnectPanel(this.roomId, this.timeLeftInSeconds, this.gameType, this.rankedGame);
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00037332 File Offset: 0x00035532
		private bool CanPlayerJoinIfaGames()
		{
			return GameServiceController.Instance.InvadersFromAfarUnlocked() || !this.IsPlayerUsingIfaMat();
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x0003734B File Offset: 0x0003554B
		private bool IsPlayerUsingIfaMat()
		{
			return MatAndFactionSelection.IsDLCFaction(this.playerFaction) || MatAndFactionSelection.IsDLCPlayerMatType(this.playerMatType);
		}

		// Token: 0x04001059 RID: 4185
		private string roomId;

		// Token: 0x0400105A RID: 4186
		private int timeLeftInSeconds;

		// Token: 0x0400105B RID: 4187
		private GameType gameType;

		// Token: 0x0400105C RID: 4188
		private bool rankedGame;

		// Token: 0x0400105D RID: 4189
		private bool invadersFromAfar;

		// Token: 0x0400105E RID: 4190
		private Faction playerFaction;

		// Token: 0x0400105F RID: 4191
		private PlayerMatType playerMatType;
	}
}

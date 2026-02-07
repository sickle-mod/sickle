using System;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002F0 RID: 752
	public class TimeToReconnectPassedMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x06001636 RID: 5686 RVA: 0x00031720 File Offset: 0x0002F920
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x000373E6 File Offset: 0x000355E6
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			lobby.ShowGameInterruptedPanel(this.gameType, this.rankedGame, this.timeToReconnectInMinutes);
		}

		// Token: 0x0400106B RID: 4203
		private int timeToReconnectInMinutes;

		// Token: 0x0400106C RID: 4204
		private bool rankedGame;

		// Token: 0x0400106D RID: 4205
		private GameType gameType;
	}
}

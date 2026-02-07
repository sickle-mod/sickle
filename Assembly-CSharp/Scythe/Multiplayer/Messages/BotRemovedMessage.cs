using System;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002F2 RID: 754
	public class BotRemovedMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x0600163A RID: 5690 RVA: 0x00037400 File Offset: 0x00035600
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			gameRoom.RemoveBot(this.slot);
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x0003740F File Offset: 0x0003560F
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			gameRoomMobile.RemoveBot(this.slot);
		}

		// Token: 0x0400106E RID: 4206
		private int slot;
	}
}

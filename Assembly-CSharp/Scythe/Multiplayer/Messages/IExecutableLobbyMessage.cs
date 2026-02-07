using System;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002F3 RID: 755
	public interface IExecutableLobbyMessage
	{
		// Token: 0x0600163D RID: 5693
		void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom);

		// Token: 0x0600163E RID: 5694
		void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile);
	}
}

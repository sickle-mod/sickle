using System;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Messages;

namespace Assets.Scripts.Multiplayer.Messages.Lobby
{
	// Token: 0x020001AF RID: 431
	public class GetUpdateExceptionMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x06000C8D RID: 3213 RVA: 0x000305CB File Offset: 0x0002E7CB
		public GetUpdateExceptionMessage(Exception exception)
		{
			this.exception = exception;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x000305DA File Offset: 0x0002E7DA
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			lobby.GetUpdateException(this.exception);
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x000305DA File Offset: 0x0002E7DA
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			lobby.GetUpdateException(this.exception);
		}

		// Token: 0x040009E1 RID: 2529
		private Exception exception;
	}
}

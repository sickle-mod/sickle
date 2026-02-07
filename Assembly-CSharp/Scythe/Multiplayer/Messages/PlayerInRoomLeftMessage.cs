using System;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002FC RID: 764
	public class PlayerInRoomLeftMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x0600165B RID: 5723 RVA: 0x00037548 File Offset: 0x00035748
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			gameRoom.RemovePlayer(this.id);
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00037557 File Offset: 0x00035757
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			gameRoomMobile.RemovePlayer(this.id);
		}

		// Token: 0x0400107B RID: 4219
		private Guid id;
	}
}

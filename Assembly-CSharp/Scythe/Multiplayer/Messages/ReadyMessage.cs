using System;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002FD RID: 765
	public class ReadyMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x0600165E RID: 5726 RVA: 0x00037566 File Offset: 0x00035766
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			gameRoom.ChangeReadyState(this.id, this.isReady == 1);
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x0003757E File Offset: 0x0003577E
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			gameRoomMobile.SetReadyPlayer(this.id, this.isReady == 1);
		}

		// Token: 0x0400107C RID: 4220
		private Guid id;

		// Token: 0x0400107D RID: 4221
		private int isReady;
	}
}

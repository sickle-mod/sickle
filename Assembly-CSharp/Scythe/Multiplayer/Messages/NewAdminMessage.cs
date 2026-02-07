using System;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002F7 RID: 759
	public class NewAdminMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x0600164A RID: 5706 RVA: 0x000374A3 File Offset: 0x000356A3
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			gameRoom.PromoteNewAdmin(this.id);
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x000374B2 File Offset: 0x000356B2
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			gameRoomMobile.PromoteNewAdmin(this.id);
		}

		// Token: 0x04001074 RID: 4212
		private Guid id;
	}
}

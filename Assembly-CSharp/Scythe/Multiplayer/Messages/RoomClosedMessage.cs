using System;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002FE RID: 766
	public class RoomClosedMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x06001661 RID: 5729 RVA: 0x00037596 File Offset: 0x00035796
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			this.ExecuteLogic(gameListPanel);
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00037596 File Offset: 0x00035796
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			this.ExecuteLogic(gameListPanel);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x0003759F File Offset: 0x0003579F
		private void ExecuteLogic(GameListPanel gameListPanel)
		{
			gameListPanel.RemoveRoom(this.id);
		}

		// Token: 0x0400107E RID: 4222
		private string id;
	}
}

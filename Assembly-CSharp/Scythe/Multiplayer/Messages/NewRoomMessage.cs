using System;
using Scythe.Multiplayer.Data;
using Scythe.Utilities;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002FA RID: 762
	public class NewRoomMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x06001653 RID: 5715 RVA: 0x00037503 File Offset: 0x00035703
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			this.ExecuteLogic(gameListPanel);
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x00037503 File Offset: 0x00035703
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			this.ExecuteLogic(gameListPanel);
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x0009F830 File Offset: 0x0009DA30
		private void ExecuteLogic(GameListPanel gameListPanel)
		{
			LobbyRoom lobbyRoom = GameSerializer.DeserializeObject<LobbyRoom>(this.roomData);
			gameListPanel.AddNewRoom(lobbyRoom);
		}

		// Token: 0x04001079 RID: 4217
		private string roomData;
	}
}

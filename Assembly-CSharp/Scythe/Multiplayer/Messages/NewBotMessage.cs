using System;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002F8 RID: 760
	public class NewBotMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x0600164D RID: 5709 RVA: 0x000374C1 File Offset: 0x000356C1
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			gameRoom.AddBot(new Bot(this.slot, this.difficulty, this.name), false);
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x000374E2 File Offset: 0x000356E2
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			gameRoomMobile.AddBot(new Bot(this.slot, this.difficulty, this.name), false);
		}

		// Token: 0x04001075 RID: 4213
		private int slot;

		// Token: 0x04001076 RID: 4214
		private int difficulty;

		// Token: 0x04001077 RID: 4215
		private string name;
	}
}

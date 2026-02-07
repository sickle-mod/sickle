using System;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002F6 RID: 758
	public class MatAndFactionChoosenMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x06001647 RID: 5703 RVA: 0x0003746D File Offset: 0x0003566D
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			gameRoom.FactionAndMatChoosen(this.faction, this.playerMat, this.slot);
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x00037488 File Offset: 0x00035688
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			gameRoomMobile.FactionAndMatChoosen(this.faction, this.playerMat, this.slot);
		}

		// Token: 0x04001071 RID: 4209
		private int faction;

		// Token: 0x04001072 RID: 4210
		private int playerMat;

		// Token: 0x04001073 RID: 4211
		private int slot;
	}
}

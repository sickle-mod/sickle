using System;
using Scythe.Multiplayer.Data;
using Scythe.Utilities;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002F9 RID: 761
	public class NewPlayerMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x06001650 RID: 5712 RVA: 0x0009F7E8 File Offset: 0x0009D9E8
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			PlayerInfo playerInfo = GameSerializer.DeserializeObject<PlayerInfo>(this.playerInfo);
			gameRoom.AddPlayer(playerInfo, false);
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x0009F80C File Offset: 0x0009DA0C
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			PlayerInfo playerInfo = GameSerializer.DeserializeObject<PlayerInfo>(this.playerInfo);
			gameRoomMobile.AddPlayer(playerInfo, false);
		}

		// Token: 0x04001078 RID: 4216
		private string playerInfo;
	}
}

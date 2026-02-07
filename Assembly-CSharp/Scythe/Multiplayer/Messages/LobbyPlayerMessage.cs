using System;
using Scythe.Multiplayer.Data;
using Scythe.Utilities;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002F5 RID: 757
	public class LobbyPlayerMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x06001643 RID: 5699 RVA: 0x0003743A File Offset: 0x0003563A
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			this.ExecuteLogic(playerListPanel);
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00037443 File Offset: 0x00035643
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			this.ExecuteLogic(playerListPanel);
			if (gameRoomMobile)
			{
				gameRoomMobile.inviteBuddiesPanel.AddPlayer(GameSerializer.DeserializeObject<PlayerInfo>(this.playerInfo));
			}
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x0009F7C8 File Offset: 0x0009D9C8
		private void ExecuteLogic(PlayerListPanel playerListPanel)
		{
			PlayerInfo playerInfo = GameSerializer.DeserializeObject<PlayerInfo>(this.playerInfo);
			playerListPanel.AddNewPlayer(playerInfo);
		}

		// Token: 0x04001070 RID: 4208
		public string playerInfo;
	}
}

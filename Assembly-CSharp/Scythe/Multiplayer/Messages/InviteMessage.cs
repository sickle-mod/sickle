using System;
using Scythe.Multiplayer.Data;
using Scythe.Utilities;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002F4 RID: 756
	public class InviteMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x0600163F RID: 5695 RVA: 0x0003741E File Offset: 0x0003561E
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			this.ExecuteLogic(lobby);
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x0003741E File Offset: 0x0003561E
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			this.ExecuteLogic(lobby);
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x00037427 File Offset: 0x00035627
		private void ExecuteLogic(Lobby lobby)
		{
			lobby.ShowInvite(GameSerializer.DeserializeObject<InvitationReceived>(this.inviteData));
		}

		// Token: 0x0400106F RID: 4207
		private string inviteData;
	}
}

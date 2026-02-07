using System;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002FB RID: 763
	public class PlayerInLobbyLeftMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x06001657 RID: 5719 RVA: 0x0003750C File Offset: 0x0003570C
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			this.ExecuteLogic(playerListPanel);
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x00037515 File Offset: 0x00035715
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			this.ExecuteLogic(playerListPanel);
			if (gameRoomMobile)
			{
				gameRoomMobile.inviteBuddiesPanel.RemovePlayer(this.id);
			}
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0003753A File Offset: 0x0003573A
		private void ExecuteLogic(PlayerListPanel playerListPanel)
		{
			playerListPanel.RemovePlayer(this.id);
		}

		// Token: 0x0400107A RID: 4218
		private Guid id;
	}
}

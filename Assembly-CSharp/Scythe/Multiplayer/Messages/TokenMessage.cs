using System;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x02000302 RID: 770
	public class TokenMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x06001670 RID: 5744 RVA: 0x00037626 File Offset: 0x00035826
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			this.ExecuteLogic();
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00037626 File Offset: 0x00035826
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			this.ExecuteLogic();
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void ExecuteLogic()
		{
		}

		// Token: 0x04001085 RID: 4229
		private int id;

		// Token: 0x04001086 RID: 4230
		private string token;
	}
}

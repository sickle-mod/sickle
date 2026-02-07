using System;
using System.Collections.Generic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002FF RID: 767
	public class StartingOrderMessage : Message, IExecutableLobbyMessage
	{
		// Token: 0x06001665 RID: 5733 RVA: 0x000375AD File Offset: 0x000357AD
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			gameRoom.SetSlotsOrder(this.oldSlots, this.newSlots, this.ids);
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x000375C8 File Offset: 0x000357C8
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			gameRoomMobile.SetSlotsOrder(this.oldSlots, this.newSlots, this.ids);
			gameRoomMobile.SetGameRoomState(GameRoomMobile.GameRoomStateType.SELECT_FACTIONS_AND_MAT);
			gameRoomMobile.StartChoosing();
		}

		// Token: 0x0400107F RID: 4223
		private List<int> oldSlots;

		// Token: 0x04001080 RID: 4224
		private List<int> newSlots;

		// Token: 0x04001081 RID: 4225
		private List<Guid> ids;
	}
}

using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200031C RID: 796
	public enum JoinRoomErrorStatus
	{
		// Token: 0x040010D5 RID: 4309
		RoomDoesNotExists,
		// Token: 0x040010D6 RID: 4310
		GameAlreadyStarted,
		// Token: 0x040010D7 RID: 4311
		RoomIsFull,
		// Token: 0x040010D8 RID: 4312
		YouAreAlreadyInThisRoom,
		// Token: 0x040010D9 RID: 4313
		PlayersAreChoosingMats,
		// Token: 0x040010DA RID: 4314
		YourEloIsTooLow,
		// Token: 0x040010DB RID: 4315
		YourEloIsTooHigh,
		// Token: 0x040010DC RID: 4316
		UnknownError
	}
}

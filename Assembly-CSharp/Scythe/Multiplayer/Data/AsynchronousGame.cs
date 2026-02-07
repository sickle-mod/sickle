using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000316 RID: 790
	[Serializable]
	public class AsynchronousGame
	{
		// Token: 0x040010C0 RID: 4288
		public string GameId;

		// Token: 0x040010C1 RID: 4289
		public bool IsRanked;

		// Token: 0x040010C2 RID: 4290
		public bool IsPlayerTurn;

		// Token: 0x040010C3 RID: 4291
		public int PlayerClock;

		// Token: 0x040010C4 RID: 4292
		public int Players;

		// Token: 0x040010C5 RID: 4293
		public string Name;

		// Token: 0x040010C6 RID: 4294
		public int PlayerFaction;

		// Token: 0x040010C7 RID: 4295
		public int CurrentFaction;

		// Token: 0x040010C8 RID: 4296
		public bool InvadersFromAfar;

		// Token: 0x040010C9 RID: 4297
		public int PlayerMatType;
	}
}

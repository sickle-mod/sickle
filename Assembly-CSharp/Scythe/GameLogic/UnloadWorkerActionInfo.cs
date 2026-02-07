using System;

namespace Scythe.GameLogic
{
	// Token: 0x02000602 RID: 1538
	public class UnloadWorkerActionInfo : EnemyActionInfo
	{
		// Token: 0x04002120 RID: 8480
		public Unit worker;

		// Token: 0x04002121 RID: 8481
		public Unit mech;

		// Token: 0x04002122 RID: 8482
		public GameHex positionToUnload;

		// Token: 0x04002123 RID: 8483
		public bool unloadOnBattlefield;
	}
}

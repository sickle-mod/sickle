using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005FE RID: 1534
	public class ProduceEnemyActionInfo : EnemyActionInfo
	{
		// Token: 0x0400210B RID: 8459
		public bool payPower;

		// Token: 0x0400210C RID: 8460
		public bool payPopularity;

		// Token: 0x0400210D RID: 8461
		public bool payCoin;

		// Token: 0x0400210E RID: 8462
		public Dictionary<GameHex, int> Hexes = new Dictionary<GameHex, int>();

		// Token: 0x0400210F RID: 8463
		public GameHex hex;

		// Token: 0x04002110 RID: 8464
		public ResourceType resourceType;

		// Token: 0x04002111 RID: 8465
		public int amount;
	}
}

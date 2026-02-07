using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005FD RID: 1533
	public class EnemyPayResourceFromHexInfo : EnemyActionInfo
	{
		// Token: 0x04002106 RID: 8454
		public HashSet<GameHex> allHexes = new HashSet<GameHex>();

		// Token: 0x04002107 RID: 8455
		public bool showAllHexes;

		// Token: 0x04002108 RID: 8456
		public GameHex gameHex;

		// Token: 0x04002109 RID: 8457
		public ResourceType resourceType;

		// Token: 0x0400210A RID: 8458
		public int amount;
	}
}

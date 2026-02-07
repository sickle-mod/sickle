using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x02000603 RID: 1539
	public class MoveRetreatEnemyActionInfo : EnemyActionInfo
	{
		// Token: 0x04002124 RID: 8484
		public GameHex withdrawPositionHex;

		// Token: 0x04002125 RID: 8485
		public List<Unit> units = new List<Unit>();
	}
}

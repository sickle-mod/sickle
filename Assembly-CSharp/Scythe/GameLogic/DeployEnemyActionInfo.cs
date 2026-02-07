using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005FB RID: 1531
	public class DeployEnemyActionInfo : EnemyActionInfo
	{
		// Token: 0x040020FF RID: 8447
		public GameHex mechHex;

		// Token: 0x04002100 RID: 8448
		public Mech mechToDeploy;

		// Token: 0x04002101 RID: 8449
		public int skillIndex;

		// Token: 0x04002102 RID: 8450
		public List<Unit> allPlayerBattleUnits = new List<Unit>();

		// Token: 0x04002103 RID: 8451
		public HashSet<GameHex> allHexes = new HashSet<GameHex>();
	}
}

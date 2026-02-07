using System;
using System.Collections.Generic;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005F8 RID: 1528
	public class EnlistEnemyActionInfo : EnemyActionInfo
	{
		// Token: 0x040020F8 RID: 8440
		public DownActionType typeOfDownAction;

		// Token: 0x040020F9 RID: 8441
		public GainType oneTimeBonus;

		// Token: 0x040020FA RID: 8442
		public HashSet<GameHex> allHexes = new HashSet<GameHex>();

		// Token: 0x040020FB RID: 8443
		public int resourcesToGainAmount;
	}
}

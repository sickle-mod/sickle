using System;
using System.Collections.Generic;
using Scythe.GameLogic;

namespace Scythe.UI
{
	// Token: 0x0200042D RID: 1069
	public class BattleResult
	{
		// Token: 0x040016EC RID: 5868
		public GameHex battlefield;

		// Token: 0x040016ED RID: 5869
		public Player attacker;

		// Token: 0x040016EE RID: 5870
		public Player defender;

		// Token: 0x040016EF RID: 5871
		public bool attackerIsWinner;

		// Token: 0x040016F0 RID: 5872
		public Dictionary<Player, PowerSelected> usedPower;
	}
}

using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005FC RID: 1532
	public class TradeEnemyActionInfo : EnemyActionInfo
	{
		// Token: 0x04002104 RID: 8452
		public List<GameHex> hexes = new List<GameHex>();

		// Token: 0x04002105 RID: 8453
		public List<Dictionary<ResourceType, int>> resourcesToTrade = new List<Dictionary<ResourceType, int>>();
	}
}

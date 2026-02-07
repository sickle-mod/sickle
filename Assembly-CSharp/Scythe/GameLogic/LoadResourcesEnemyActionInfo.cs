using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x02000600 RID: 1536
	public class LoadResourcesEnemyActionInfo : EnemyActionInfo
	{
		// Token: 0x0400211A RID: 8474
		public Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>
		{
			{
				ResourceType.food,
				0
			},
			{
				ResourceType.metal,
				0
			},
			{
				ResourceType.oil,
				0
			},
			{
				ResourceType.wood,
				0
			}
		};

		// Token: 0x0400211B RID: 8475
		public Unit unit;

		// Token: 0x0400211C RID: 8476
		public GameHex hex;

		// Token: 0x0400211D RID: 8477
		public bool isUnload;
	}
}

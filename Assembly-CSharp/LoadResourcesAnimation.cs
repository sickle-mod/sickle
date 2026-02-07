using System;
using System.Collections.Generic;
using Scythe.GameLogic;

// Token: 0x020000BA RID: 186
public class LoadResourcesAnimation : EnemyActionAnimation
{
	// Token: 0x040004DD RID: 1245
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

	// Token: 0x040004DE RID: 1246
	public Unit unit;

	// Token: 0x040004DF RID: 1247
	public GameHex hex;
}

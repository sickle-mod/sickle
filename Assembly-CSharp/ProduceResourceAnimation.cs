using System;
using System.Collections.Generic;
using Scythe.GameLogic;

// Token: 0x020000C0 RID: 192
public class ProduceResourceAnimation : EnemyActionAnimation
{
	// Token: 0x040004EB RID: 1259
	public GameHex hex;

	// Token: 0x040004EC RID: 1260
	public ResourceType resourceType;

	// Token: 0x040004ED RID: 1261
	public int amountOfResources;

	// Token: 0x040004EE RID: 1262
	public List<Worker> workersToAnimation = new List<Worker>();
}

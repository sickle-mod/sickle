using System;
using System.Collections.Generic;
using Scythe.GameLogic;

// Token: 0x020000C1 RID: 193
public class ProduceWorkerAnimation : EnemyActionAnimation
{
	// Token: 0x040004EF RID: 1263
	public GameHex hex;

	// Token: 0x040004F0 RID: 1264
	public ResourceType resourceType;

	// Token: 0x040004F1 RID: 1265
	public int amountOfResources;

	// Token: 0x040004F2 RID: 1266
	public List<Worker> workersToAnimation = new List<Worker>();
}

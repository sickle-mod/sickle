using System;
using System.Collections.Generic;
using Scythe.GameLogic;

// Token: 0x020000BB RID: 187
public class MoveRetreatUnitAnimation : EnemyActionAnimation
{
	// Token: 0x040004E0 RID: 1248
	public List<Unit> allUnitsToRetreat = new List<Unit>();

	// Token: 0x040004E1 RID: 1249
	public GameHex positionBase;
}

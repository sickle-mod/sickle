using System;
using System.Collections.Generic;
using Scythe.GameLogic;

// Token: 0x020000C3 RID: 195
public class MechSkillAnimation : EnemyActionAnimation
{
	// Token: 0x040004F6 RID: 1270
	public List<Unit> allPlayerBattleUnits = new List<Unit>();

	// Token: 0x040004F7 RID: 1271
	public GameHex mechHex;

	// Token: 0x040004F8 RID: 1272
	public int skillIndex;

	// Token: 0x040004F9 RID: 1273
	public Mech mechToDeploy;
}

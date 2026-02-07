using System;
using System.Collections.Generic;
using Scythe.GameLogic;

// Token: 0x020000B6 RID: 182
public class MoveUnitAnimation : EnemyActionAnimation
{
	// Token: 0x040004CD RID: 1229
	public Unit unitToAnimate;

	// Token: 0x040004CE RID: 1230
	public Dictionary<GameHex, GameHex> possibleMoves = new Dictionary<GameHex, GameHex>();

	// Token: 0x040004CF RID: 1231
	public GameHex moveFromPosition;

	// Token: 0x040004D0 RID: 1232
	public GameHex from;

	// Token: 0x040004D1 RID: 1233
	public GameHex to;

	// Token: 0x040004D2 RID: 1234
	public bool destinationIsBattlefield;

	// Token: 0x040004D3 RID: 1235
	public bool takingOwnerPosition;

	// Token: 0x040004D4 RID: 1236
	public GameHex positionAfterFight;
}

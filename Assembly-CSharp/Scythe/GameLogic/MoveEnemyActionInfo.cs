using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005FF RID: 1535
	public class MoveEnemyActionInfo : EnemyActionInfo
	{
		// Token: 0x04002112 RID: 8466
		public Dictionary<GameHex, GameHex> possibleMoves = new Dictionary<GameHex, GameHex>();

		// Token: 0x04002113 RID: 8467
		public Unit unit;

		// Token: 0x04002114 RID: 8468
		public GameHex moveFromPosition;

		// Token: 0x04002115 RID: 8469
		public GameHex fromHex;

		// Token: 0x04002116 RID: 8470
		public GameHex toHex;

		// Token: 0x04002117 RID: 8471
		public bool destinationIsBattlefield;

		// Token: 0x04002118 RID: 8472
		public bool takingOwnerPosition;

		// Token: 0x04002119 RID: 8473
		public GameHex positionAfterFight;
	}
}

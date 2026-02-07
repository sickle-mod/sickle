using System;

namespace Scythe.GameLogic
{
	// Token: 0x020005F1 RID: 1521
	public class Character : Unit
	{
		// Token: 0x06003044 RID: 12356 RVA: 0x000462F9 File Offset: 0x000444F9
		public Character(GameManager gameManager, Player owner, short maxMoveCount = 1)
			: base(gameManager, owner, maxMoveCount)
		{
			base.UnitType = UnitType.Character;
			base.Id = 0;
		}
	}
}

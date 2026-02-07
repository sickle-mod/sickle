using System;

namespace Scythe.GameLogic
{
	// Token: 0x020005EE RID: 1518
	public abstract class StructureBonusCard : Card
	{
		// Token: 0x06003037 RID: 12343
		public abstract int CalculateBonus(Player player);

		// Token: 0x040020D7 RID: 8407
		public StructureBonus structureBonus;
	}
}

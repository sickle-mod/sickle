using System;
using System.Collections.Generic;
using System.Linq;

namespace Scythe.GameLogic
{
	// Token: 0x020005EA RID: 1514
	public class InlineStructuresCard : StructureBonusCard
	{
		// Token: 0x0600302E RID: 12334 RVA: 0x000461BE File Offset: 0x000443BE
		public InlineStructuresCard()
		{
			this.cardId = 5;
			this.structureBonus = new StructureBonus(new int[] { 2, 4, 6, 9 });
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x0012674C File Offset: 0x0012494C
		public override int CalculateBonus(Player player)
		{
			this.buildings = player.FieldsWithPlayerBuildings();
			int num = 0;
			for (int i = 0; i < this.buildings.Count; i++)
			{
				int num2 = 0;
				foreach (EdgeMask edgeMask in Enum.GetValues(typeof(EdgeMask)).Cast<EdgeMask>())
				{
					int num3 = this.MaxDistance(this.buildings.ElementAt(i), edgeMask);
					if (num2 < num3)
					{
						num2 = num3;
					}
				}
				if (num < num2)
				{
					num = num2;
				}
			}
			return this.structureBonus[num];
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x001267F8 File Offset: 0x001249F8
		private int MaxDistance(GameHex hex, EdgeMask dir)
		{
			int num = 1;
			if (this.buildings.Contains(hex.GetNeighbour(dir)))
			{
				num = this.MaxDistance(hex.GetNeighbour(dir), dir);
				num++;
			}
			return num;
		}

		// Token: 0x040020D6 RID: 8406
		private HashSet<GameHex> buildings;
	}
}

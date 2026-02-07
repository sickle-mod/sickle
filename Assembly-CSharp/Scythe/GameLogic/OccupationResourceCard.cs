using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005EC RID: 1516
	public class OccupationResourceCard : StructureBonusCard
	{
		// Token: 0x06003033 RID: 12339 RVA: 0x00046214 File Offset: 0x00044414
		public OccupationResourceCard()
		{
			this.cardId = 6;
			this.structureBonus = new StructureBonus(new int[] { 2, 4, 6, 9 });
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x001268EC File Offset: 0x00124AEC
		public override int CalculateBonus(Player player)
		{
			HashSet<GameHex> hashSet = player.FieldsWithPlayerBuildings();
			int num = 0;
			foreach (GameHex gameHex in hashSet)
			{
				if (gameHex.hexType == HexType.farm || gameHex.hexType == HexType.tundra)
				{
					num++;
				}
			}
			return this.structureBonus[num];
		}
	}
}

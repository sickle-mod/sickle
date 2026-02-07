using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005F0 RID: 1520
	public class TunnelNeighbourCard : StructureBonusCard
	{
		// Token: 0x06003042 RID: 12354 RVA: 0x000462CE File Offset: 0x000444CE
		public TunnelNeighbourCard()
		{
			this.cardId = 1;
			this.structureBonus = new StructureBonus(new int[] { 2, 4, 4, 6, 6, 9 });
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x00126CBC File Offset: 0x00124EBC
		public override int CalculateBonus(Player player)
		{
			HashSet<GameHex> hashSet = player.FieldsWithPlayerBuildings();
			HashSet<GameHex> hashSet2 = new HashSet<GameHex>();
			int num = 0;
			foreach (GameHex gameHex in hashSet)
			{
				foreach (GameHex gameHex2 in gameHex.GetNeighboursAll())
				{
					if (gameHex2.hasTunnel && !hashSet2.Contains(gameHex2))
					{
						hashSet2.Add(gameHex2);
						num++;
					}
				}
			}
			return this.structureBonus[num];
		}
	}
}

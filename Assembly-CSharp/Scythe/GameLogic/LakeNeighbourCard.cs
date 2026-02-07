using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005EB RID: 1515
	public class LakeNeighbourCard : StructureBonusCard
	{
		// Token: 0x06003031 RID: 12337 RVA: 0x000461E9 File Offset: 0x000443E9
		public LakeNeighbourCard()
		{
			this.cardId = 2;
			this.structureBonus = new StructureBonus(new int[] { 2, 4, 4, 6, 6, 9, 9 });
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x00126830 File Offset: 0x00124A30
		public override int CalculateBonus(Player player)
		{
			HashSet<GameHex> hashSet = player.FieldsWithPlayerBuildings();
			HashSet<GameHex> hashSet2 = new HashSet<GameHex>();
			int num = 0;
			foreach (GameHex gameHex in hashSet)
			{
				foreach (GameHex gameHex2 in gameHex.GetNeighboursAll())
				{
					if (gameHex2.hexType == HexType.lake && !hashSet2.Contains(gameHex2))
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

using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005E9 RID: 1513
	public class EncounterNeighbourCard : StructureBonusCard
	{
		// Token: 0x0600302C RID: 12332 RVA: 0x00046193 File Offset: 0x00044393
		public EncounterNeighbourCard()
		{
			this.cardId = 3;
			this.structureBonus = new StructureBonus(new int[] { 2, 4, 4, 6, 6, 9, 9 });
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x00126690 File Offset: 0x00124890
		public override int CalculateBonus(Player player)
		{
			HashSet<GameHex> hashSet = player.FieldsWithPlayerBuildings();
			HashSet<GameHex> hashSet2 = new HashSet<GameHex>();
			int num = 0;
			foreach (GameHex gameHex in hashSet)
			{
				foreach (GameHex gameHex2 in gameHex.GetNeighboursAll())
				{
					if (gameHex2.hasEncounter && !hashSet2.Contains(gameHex2))
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

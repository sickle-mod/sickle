using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005ED RID: 1517
	public class OccupationTunnelCard : StructureBonusCard
	{
		// Token: 0x06003035 RID: 12341 RVA: 0x0004623F File Offset: 0x0004443F
		public OccupationTunnelCard()
		{
			this.cardId = 4;
			this.structureBonus = new StructureBonus(new int[] { 2, 4, 6, 6 });
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x0012695C File Offset: 0x00124B5C
		public override int CalculateBonus(Player player)
		{
			HashSet<GameHex> hashSet = player.FieldsWithPlayerBuildings();
			int num = 0;
			using (HashSet<GameHex>.Enumerator enumerator = hashSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.hasTunnel)
					{
						num++;
					}
				}
			}
			return this.structureBonus[num];
		}
	}
}

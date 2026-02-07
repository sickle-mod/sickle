using System;

namespace Scythe.GameLogic
{
	// Token: 0x020005E8 RID: 1512
	public static class PopularityTrack
	{
		// Token: 0x06003022 RID: 12322 RVA: 0x00046088 File Offset: 0x00044288
		public static bool LowTier(int amount)
		{
			return amount >= 0 && amount <= 6;
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x00046097 File Offset: 0x00044297
		public static bool MediumTier(int amount)
		{
			return amount >= 7 && amount <= 12;
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x000460A7 File Offset: 0x000442A7
		public static bool HighTier(int amount)
		{
			return amount >= 13 && amount <= 18;
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x000460B8 File Offset: 0x000442B8
		public static int StarBonus(int numberOfStar, int popularity)
		{
			if (PopularityTrack.LowTier(popularity))
			{
				return numberOfStar * 3;
			}
			if (PopularityTrack.MediumTier(popularity))
			{
				return numberOfStar * 4;
			}
			if (PopularityTrack.HighTier(popularity))
			{
				return numberOfStar * 5;
			}
			return 0;
		}

		// Token: 0x06003026 RID: 12326 RVA: 0x000460DF File Offset: 0x000442DF
		public static int TerritoryBonus(int territories, int popularity)
		{
			if (PopularityTrack.LowTier(popularity))
			{
				return territories * 2;
			}
			if (PopularityTrack.MediumTier(popularity))
			{
				return territories * 3;
			}
			if (PopularityTrack.HighTier(popularity))
			{
				return territories * 4;
			}
			return 0;
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x00046106 File Offset: 0x00044306
		public static int ResourceBonus(int resources, int popularity)
		{
			resources /= 2;
			if (PopularityTrack.LowTier(popularity))
			{
				return resources;
			}
			if (PopularityTrack.MediumTier(popularity))
			{
				return resources * 2;
			}
			if (PopularityTrack.HighTier(popularity))
			{
				return resources * 3;
			}
			return 0;
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x00046130 File Offset: 0x00044330
		public static int StarsMultiplier(int popularity)
		{
			if (PopularityTrack.LowTier(popularity))
			{
				return 3;
			}
			if (PopularityTrack.MediumTier(popularity))
			{
				return 4;
			}
			if (PopularityTrack.HighTier(popularity))
			{
				return 5;
			}
			return 0;
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x00046151 File Offset: 0x00044351
		public static int TerritoryMultiplier(int popularity)
		{
			if (PopularityTrack.LowTier(popularity))
			{
				return 2;
			}
			if (PopularityTrack.MediumTier(popularity))
			{
				return 3;
			}
			if (PopularityTrack.HighTier(popularity))
			{
				return 4;
			}
			return 0;
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x00046172 File Offset: 0x00044372
		public static int ResourceMultiplier(int popularity)
		{
			if (PopularityTrack.LowTier(popularity))
			{
				return 1;
			}
			if (PopularityTrack.MediumTier(popularity))
			{
				return 2;
			}
			if (PopularityTrack.HighTier(popularity))
			{
				return 3;
			}
			return 0;
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x00046172 File Offset: 0x00044372
		public static int PopularityTier(int popularity)
		{
			if (PopularityTrack.LowTier(popularity))
			{
				return 1;
			}
			if (PopularityTrack.MediumTier(popularity))
			{
				return 2;
			}
			if (PopularityTrack.HighTier(popularity))
			{
				return 3;
			}
			return 0;
		}
	}
}

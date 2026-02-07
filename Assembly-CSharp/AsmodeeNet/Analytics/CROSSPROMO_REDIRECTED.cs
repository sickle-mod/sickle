using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200097F RID: 2431
	public struct CROSSPROMO_REDIRECTED
	{
		// Token: 0x040031A6 RID: 12710
		public string crosspromo_session_id;

		// Token: 0x040031A7 RID: 12711
		public string product_id;

		// Token: 0x040031A8 RID: 12712
		public string product_name;

		// Token: 0x02000980 RID: 2432
		public enum crosspromo_type
		{
			// Token: 0x040031AA RID: 12714
			more_games,
			// Token: 0x040031AB RID: 12715
			interstitial,
			// Token: 0x040031AC RID: 12716
			banner
		}

		// Token: 0x02000981 RID: 2433
		public enum more_game_category
		{
			// Token: 0x040031AE RID: 12718
			featured,
			// Token: 0x040031AF RID: 12719
			family,
			// Token: 0x040031B0 RID: 12720
			advanced,
			// Token: 0x040031B1 RID: 12721
			tabletop
		}

		// Token: 0x02000982 RID: 2434
		public enum product_type
		{
			// Token: 0x040031B3 RID: 12723
			digital,
			// Token: 0x040031B4 RID: 12724
			boardgame
		}
	}
}

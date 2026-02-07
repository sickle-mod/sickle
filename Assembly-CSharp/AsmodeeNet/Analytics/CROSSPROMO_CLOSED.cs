using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000983 RID: 2435
	public struct CROSSPROMO_CLOSED
	{
		// Token: 0x040031B5 RID: 12725
		public string crosspromo_session_id;

		// Token: 0x040031B6 RID: 12726
		public int time_active_sec;

		// Token: 0x02000984 RID: 2436
		public enum crosspromo_type
		{
			// Token: 0x040031B8 RID: 12728
			more_games,
			// Token: 0x040031B9 RID: 12729
			interstitial,
			// Token: 0x040031BA RID: 12730
			banner
		}
	}
}

using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000976 RID: 2422
	public struct CROSSPROMO_OPENED
	{
		// Token: 0x04003174 RID: 12660
		public string crosspromo_session_id;

		// Token: 0x04003175 RID: 12661
		public string api_version;

		// Token: 0x04003176 RID: 12662
		public bool is_automatic;

		// Token: 0x02000977 RID: 2423
		public enum crosspromo_type
		{
			// Token: 0x04003178 RID: 12664
			more_games,
			// Token: 0x04003179 RID: 12665
			interstitial,
			// Token: 0x0400317A RID: 12666
			banner
		}
	}
}

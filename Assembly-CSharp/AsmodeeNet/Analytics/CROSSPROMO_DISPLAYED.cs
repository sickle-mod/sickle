using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000974 RID: 2420
	public struct CROSSPROMO_DISPLAYED
	{
		// Token: 0x0400316D RID: 12653
		public string crosspromo_session_id;

		// Token: 0x0400316E RID: 12654
		public string api_version;

		// Token: 0x0400316F RID: 12655
		public string product_id;

		// Token: 0x04003170 RID: 12656
		public string product_name;

		// Token: 0x02000975 RID: 2421
		public enum crosspromo_type
		{
			// Token: 0x04003172 RID: 12658
			interstitial,
			// Token: 0x04003173 RID: 12659
			banner
		}
	}
}

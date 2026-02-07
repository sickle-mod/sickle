using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000986 RID: 2438
	public struct CONTENT_DL_STOP
	{
		// Token: 0x040031BD RID: 12733
		public string dl_session_id;

		// Token: 0x040031BE RID: 12734
		public string dl_content_id;

		// Token: 0x040031BF RID: 12735
		public bool dl_is_complete;

		// Token: 0x040031C0 RID: 12736
		public int dl_time;

		// Token: 0x02000987 RID: 2439
		public enum dl_end_reason
		{
			// Token: 0x040031C2 RID: 12738
			completed,
			// Token: 0x040031C3 RID: 12739
			aborted_by_user,
			// Token: 0x040031C4 RID: 12740
			network_issue
		}
	}
}

using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009A0 RID: 2464
	public struct TUTORIAL_STEP
	{
		// Token: 0x04003257 RID: 12887
		public string step_id;

		// Token: 0x04003258 RID: 12888
		public float step_sequence_number;

		// Token: 0x04003259 RID: 12889
		public int time_on_step;

		// Token: 0x0400325A RID: 12890
		public bool is_tuto_complete;

		// Token: 0x020009A1 RID: 2465
		public enum step_status
		{
			// Token: 0x0400325C RID: 12892
			completed,
			// Token: 0x0400325D RID: 12893
			failed,
			// Token: 0x0400325E RID: 12894
			quit
		}
	}
}

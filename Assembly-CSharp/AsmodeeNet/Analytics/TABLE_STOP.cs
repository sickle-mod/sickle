using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000992 RID: 2450
	public struct TABLE_STOP
	{
		// Token: 0x04003217 RID: 12823
		public string match_session_id;

		// Token: 0x04003218 RID: 12824
		public string end_reason;

		// Token: 0x04003219 RID: 12825
		public int player_count_slots;

		// Token: 0x0400321A RID: 12826
		public int player_count_human;

		// Token: 0x0400321B RID: 12827
		public int player_count_ai;

		// Token: 0x0400321C RID: 12828
		public int player_clock_sec;

		// Token: 0x0400321D RID: 12829
		public bool is_asynchronous;

		// Token: 0x0400321E RID: 12830
		public bool is_private;

		// Token: 0x0400321F RID: 12831
		public bool is_ranked;

		// Token: 0x04003220 RID: 12832
		public bool is_observable;

		// Token: 0x04003221 RID: 12833
		public bool obs_show_hidden_info;

		// Token: 0x04003222 RID: 12834
		public int time_active_sec;

		// Token: 0x02000993 RID: 2451
		public enum obs_access
		{
			// Token: 0x04003224 RID: 12836
			friends_only,
			// Token: 0x04003225 RID: 12837
			everyone
		}
	}
}

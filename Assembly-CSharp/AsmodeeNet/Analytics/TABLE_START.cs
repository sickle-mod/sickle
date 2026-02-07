using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200098F RID: 2447
	public struct TABLE_START
	{
		// Token: 0x04003204 RID: 12804
		public string lobby_session_id;

		// Token: 0x04003205 RID: 12805
		public string match_session_id;

		// Token: 0x04003206 RID: 12806
		public int player_count_slots;

		// Token: 0x04003207 RID: 12807
		public int player_count_human;

		// Token: 0x04003208 RID: 12808
		public int player_count_ai;

		// Token: 0x04003209 RID: 12809
		public int player_clock_sec;

		// Token: 0x0400320A RID: 12810
		public bool is_asynchronous;

		// Token: 0x0400320B RID: 12811
		public bool is_private;

		// Token: 0x0400320C RID: 12812
		public bool is_ranked;

		// Token: 0x0400320D RID: 12813
		public bool is_observable;

		// Token: 0x0400320E RID: 12814
		public bool obs_show_hidden_info;

		// Token: 0x02000990 RID: 2448
		public enum launch_method
		{
			// Token: 0x04003210 RID: 12816
			create,
			// Token: 0x04003211 RID: 12817
			create_automatch,
			// Token: 0x04003212 RID: 12818
			join,
			// Token: 0x04003213 RID: 12819
			invite_received
		}

		// Token: 0x02000991 RID: 2449
		public enum obs_access
		{
			// Token: 0x04003215 RID: 12821
			friends_only,
			// Token: 0x04003216 RID: 12822
			everyone
		}
	}
}

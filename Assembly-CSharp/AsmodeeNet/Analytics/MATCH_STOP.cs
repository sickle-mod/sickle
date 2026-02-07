using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200098A RID: 2442
	public struct MATCH_STOP
	{
		// Token: 0x040031DA RID: 12762
		public string match_session_id;

		// Token: 0x040031DB RID: 12763
		public string mode;

		// Token: 0x040031DC RID: 12764
		public string map_id;

		// Token: 0x040031DD RID: 12765
		public string activated_dlc;

		// Token: 0x040031DE RID: 12766
		public int player_count_human;

		// Token: 0x040031DF RID: 12767
		public int player_count_ai;

		// Token: 0x040031E0 RID: 12768
		public int? player_playorder;

		// Token: 0x040031E1 RID: 12769
		public int time_active_sec;

		// Token: 0x040031E2 RID: 12770
		public string end_reason;

		// Token: 0x040031E3 RID: 12771
		public int? player_clock_sec;

		// Token: 0x040031E4 RID: 12772
		public string difficulty;

		// Token: 0x040031E5 RID: 12773
		public bool is_online;

		// Token: 0x040031E6 RID: 12774
		public bool is_tutorial;

		// Token: 0x040031E7 RID: 12775
		public bool? is_asynchronous;

		// Token: 0x040031E8 RID: 12776
		public bool? is_private;

		// Token: 0x040031E9 RID: 12777
		public bool? is_ranked;

		// Token: 0x040031EA RID: 12778
		public bool? is_observable;

		// Token: 0x040031EB RID: 12779
		public bool? obs_show_hidden_info;

		// Token: 0x040031EC RID: 12780
		public int? turn_count;

		// Token: 0x0200098B RID: 2443
		public enum player_result
		{
			// Token: 0x040031EE RID: 12782
			victory,
			// Token: 0x040031EF RID: 12783
			defeat,
			// Token: 0x040031F0 RID: 12784
			draw
		}

		// Token: 0x0200098C RID: 2444
		public enum obs_access
		{
			// Token: 0x040031F2 RID: 12786
			friends_only,
			// Token: 0x040031F3 RID: 12787
			everyone
		}
	}
}

using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000988 RID: 2440
	public struct MATCH_START
	{
		// Token: 0x040031C5 RID: 12741
		public string match_session_id;

		// Token: 0x040031C6 RID: 12742
		public string lobby_session_id;

		// Token: 0x040031C7 RID: 12743
		public string mode;

		// Token: 0x040031C8 RID: 12744
		public string map_id;

		// Token: 0x040031C9 RID: 12745
		public string activated_dlc;

		// Token: 0x040031CA RID: 12746
		public int player_count_human;

		// Token: 0x040031CB RID: 12747
		public int player_count_ai;

		// Token: 0x040031CC RID: 12748
		public int? player_playorder;

		// Token: 0x040031CD RID: 12749
		public string launch_method;

		// Token: 0x040031CE RID: 12750
		public int? player_clock_sec;

		// Token: 0x040031CF RID: 12751
		public string difficulty;

		// Token: 0x040031D0 RID: 12752
		public bool is_online;

		// Token: 0x040031D1 RID: 12753
		public bool is_tutorial;

		// Token: 0x040031D2 RID: 12754
		public bool? is_asynchronous;

		// Token: 0x040031D3 RID: 12755
		public bool? is_private;

		// Token: 0x040031D4 RID: 12756
		public bool? is_ranked;

		// Token: 0x040031D5 RID: 12757
		public bool? is_observable;

		// Token: 0x040031D6 RID: 12758
		public bool? obs_show_hidden_info;

		// Token: 0x02000989 RID: 2441
		public enum obs_access
		{
			// Token: 0x040031D8 RID: 12760
			friends_only,
			// Token: 0x040031D9 RID: 12761
			everyone
		}
	}
}

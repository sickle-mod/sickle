using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200098E RID: 2446
	public struct LOBBY_STOP
	{
		// Token: 0x040031FB RID: 12795
		public string lobby_session_id;

		// Token: 0x040031FC RID: 12796
		public int online_player_count_connected;

		// Token: 0x040031FD RID: 12797
		public int online_player_count_lobbyortable;

		// Token: 0x040031FE RID: 12798
		public int online_player_count_table;

		// Token: 0x040031FF RID: 12799
		public int online_player_count_match;

		// Token: 0x04003200 RID: 12800
		public int online_open_table_count;

		// Token: 0x04003201 RID: 12801
		public int online_ongoing_match_count;

		// Token: 0x04003202 RID: 12802
		public int time_active_sec;

		// Token: 0x04003203 RID: 12803
		public string end_reason;
	}
}

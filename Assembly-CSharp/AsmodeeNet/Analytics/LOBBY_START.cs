using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200098D RID: 2445
	public struct LOBBY_START
	{
		// Token: 0x040031F4 RID: 12788
		public string lobby_session_id;

		// Token: 0x040031F5 RID: 12789
		public int online_player_count_connected;

		// Token: 0x040031F6 RID: 12790
		public int online_player_count_lobbyortable;

		// Token: 0x040031F7 RID: 12791
		public int online_player_count_table;

		// Token: 0x040031F8 RID: 12792
		public int online_player_count_match;

		// Token: 0x040031F9 RID: 12793
		public int online_open_table_count;

		// Token: 0x040031FA RID: 12794
		public int online_ongoing_match_count;
	}
}

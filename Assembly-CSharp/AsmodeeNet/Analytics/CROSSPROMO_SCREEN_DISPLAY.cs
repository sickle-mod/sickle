using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000978 RID: 2424
	public struct CROSSPROMO_SCREEN_DISPLAY
	{
		// Token: 0x0400317B RID: 12667
		public string crosspromo_session_id;

		// Token: 0x0400317C RID: 12668
		public int screen_count;

		// Token: 0x0400317D RID: 12669
		public int screen_previous_time_sec;

		// Token: 0x0400317E RID: 12670
		public string game_detail_product_id;

		// Token: 0x0400317F RID: 12671
		public string game_detail_product_name;

		// Token: 0x04003180 RID: 12672
		public string clicked_crosspromo_tile_size;

		// Token: 0x04003181 RID: 12673
		public string clicked_crosspromo_tile_position_xy;

		// Token: 0x02000979 RID: 2425
		public enum crosspromo_type
		{
			// Token: 0x04003183 RID: 12675
			more_games,
			// Token: 0x04003184 RID: 12676
			interstitial,
			// Token: 0x04003185 RID: 12677
			banner
		}

		// Token: 0x0200097A RID: 2426
		public enum screen_current
		{
			// Token: 0x04003187 RID: 12679
			more_games,
			// Token: 0x04003188 RID: 12680
			game_detail,
			// Token: 0x04003189 RID: 12681
			zoom_image,
			// Token: 0x0400318A RID: 12682
			interstitial,
			// Token: 0x0400318B RID: 12683
			banner
		}

		// Token: 0x0200097B RID: 2427
		public enum screen_previous
		{
			// Token: 0x0400318D RID: 12685
			more_games,
			// Token: 0x0400318E RID: 12686
			interstitial,
			// Token: 0x0400318F RID: 12687
			banner,
			// Token: 0x04003190 RID: 12688
			ingame
		}

		// Token: 0x0200097C RID: 2428
		public enum screen_previous_nav_action
		{
			// Token: 0x04003192 RID: 12690
			automatic,
			// Token: 0x04003193 RID: 12691
			click_image,
			// Token: 0x04003194 RID: 12692
			click_banner,
			// Token: 0x04003195 RID: 12693
			click_tile,
			// Token: 0x04003196 RID: 12694
			click_learn_more,
			// Token: 0x04003197 RID: 12695
			click_close,
			// Token: 0x04003198 RID: 12696
			click_board_game,
			// Token: 0x04003199 RID: 12697
			click_back,
			// Token: 0x0400319A RID: 12698
			click_filter_featured,
			// Token: 0x0400319B RID: 12699
			click_filter_gamer,
			// Token: 0x0400319C RID: 12700
			click_filter_family,
			// Token: 0x0400319D RID: 12701
			click_filter_boardgame
		}

		// Token: 0x0200097D RID: 2429
		public enum more_game_category
		{
			// Token: 0x0400319F RID: 12703
			featured,
			// Token: 0x040031A0 RID: 12704
			family,
			// Token: 0x040031A1 RID: 12705
			advanced,
			// Token: 0x040031A2 RID: 12706
			tabletop
		}

		// Token: 0x0200097E RID: 2430
		public enum game_detail_product_type
		{
			// Token: 0x040031A4 RID: 12708
			digital,
			// Token: 0x040031A5 RID: 12709
			boardgame
		}
	}
}

using System;

namespace Scythe.Analytics
{
	// Token: 0x02000659 RID: 1625
	public enum ScreenPreviousNavActions
	{
		// Token: 0x040022E2 RID: 8930
		none,
		// Token: 0x040022E3 RID: 8931
		esc,
		// Token: 0x040022E4 RID: 8932
		click_x_button,
		// Token: 0x040022E5 RID: 8933
		click_ok_button,
		// Token: 0x040022E6 RID: 8934
		click_apply_button,
		// Token: 0x040022E7 RID: 8935
		click_accept_button,
		// Token: 0x040022E8 RID: 8936
		click_reject_button,
		// Token: 0x040022E9 RID: 8937
		click_back_button,
		// Token: 0x040022EA RID: 8938
		click_exit_button,
		// Token: 0x040022EB RID: 8939
		click_yes_button,
		// Token: 0x040022EC RID: 8940
		click_no_button,
		// Token: 0x040022ED RID: 8941
		click_rules_button,
		// Token: 0x040022EE RID: 8942
		click_ifa_rules_button,
		// Token: 0x040022EF RID: 8943
		click_cancel_button,
		// Token: 0x040022F0 RID: 8944
		click_tutorial_button,
		// Token: 0x040022F1 RID: 8945
		click_play_local_button,
		// Token: 0x040022F2 RID: 8946
		click_hotseat_button,
		// Token: 0x040022F3 RID: 8947
		click_online_button,
		// Token: 0x040022F4 RID: 8948
		click_album_button,
		// Token: 0x040022F5 RID: 8949
		click_showroom_button,
		// Token: 0x040022F6 RID: 8950
		click_option_button,
		// Token: 0x040022F7 RID: 8951
		click_new_game_button,
		// Token: 0x040022F8 RID: 8952
		click_resume_button,
		// Token: 0x040022F9 RID: 8953
		click_load_game_window_button,
		// Token: 0x040022FA RID: 8954
		click_save_game_window_button,
		// Token: 0x040022FB RID: 8955
		click_load_game_button,
		// Token: 0x040022FC RID: 8956
		click_save_game_button,
		// Token: 0x040022FD RID: 8957
		click_credits_button,
		// Token: 0x040022FE RID: 8958
		click_extras_button,
		// Token: 0x040022FF RID: 8959
		click_more_games_button,
		// Token: 0x04002300 RID: 8960
		click_login_button,
		// Token: 0x04002301 RID: 8961
		click_account_settings,
		// Token: 0x04002302 RID: 8962
		click_new_account_button,
		// Token: 0x04002303 RID: 8963
		click_edit_profile_button,
		// Token: 0x04002304 RID: 8964
		click_reset_password_button,
		// Token: 0x04002305 RID: 8965
		click_support_button,
		// Token: 0x04002306 RID: 8966
		click_logout_button,
		// Token: 0x04002307 RID: 8967
		click_register_button,
		// Token: 0x04002308 RID: 8968
		click_privacy_policy_button,
		// Token: 0x04002309 RID: 8969
		click_play_button,
		// Token: 0x0400230A RID: 8970
		click_player_name,
		// Token: 0x0400230B RID: 8971
		click_player_icon,
		// Token: 0x0400230C RID: 8972
		click_map_icon,
		// Token: 0x0400230D RID: 8973
		click_faction_details,
		// Token: 0x0400230E RID: 8974
		click_player_mat_details,
		// Token: 0x0400230F RID: 8975
		click_dlc_check_now_invaders,
		// Token: 0x04002310 RID: 8976
		click_dlc_banner_invaders,
		// Token: 0x04002311 RID: 8977
		click_quick_play_button,
		// Token: 0x04002312 RID: 8978
		click_join_quick_play_button,
		// Token: 0x04002313 RID: 8979
		click_create_game_button,
		// Token: 0x04002314 RID: 8980
		click_create_game_room_button,
		// Token: 0x04002315 RID: 8981
		click_start_button,
		// Token: 0x04002316 RID: 8982
		click_show_player_stats,
		// Token: 0x04002317 RID: 8983
		click_more_stats_button,
		// Token: 0x04002318 RID: 8984
		click_stats_overview_button,
		// Token: 0x04002319 RID: 8985
		click_stats_score_button,
		// Token: 0x0400231A RID: 8986
		click_stats_places_button,
		// Token: 0x0400231B RID: 8987
		click_stats_combat_button,
		// Token: 0x0400231C RID: 8988
		click_stats_others_button,
		// Token: 0x0400231D RID: 8989
		click_spectate_button,
		// Token: 0x0400231E RID: 8990
		click_menu_with_tabs_button,
		// Token: 0x0400231F RID: 8991
		tab_enlist_toggle,
		// Token: 0x04002320 RID: 8992
		tab_mech_toggle,
		// Token: 0x04002321 RID: 8993
		tab_structure_toggle,
		// Token: 0x04002322 RID: 8994
		tab_stars_toggle,
		// Token: 0x04002323 RID: 8995
		tab_tracks_toggle,
		// Token: 0x04002324 RID: 8996
		tab_stats_toggle,
		// Token: 0x04002325 RID: 8997
		tab_objectives_toggle,
		// Token: 0x04002326 RID: 8998
		tab_bonus_inspection_toggle,
		// Token: 0x04002327 RID: 8999
		tab_score_toggle,
		// Token: 0x04002328 RID: 9000
		tab_faction_toggle,
		// Token: 0x04002329 RID: 9001
		click_ingame_menu_button,
		// Token: 0x0400232A RID: 9002
		click_objective_preview_button,
		// Token: 0x0400232B RID: 9003
		click_complete_objective_button,
		// Token: 0x0400232C RID: 9004
		ingame_event,
		// Token: 0x0400232D RID: 9005
		factory_card_selected,
		// Token: 0x0400232E RID: 9006
		click_surrender_button,
		// Token: 0x0400232F RID: 9007
		click_help_button,
		// Token: 0x04002330 RID: 9008
		click_mouse_button
	}
}

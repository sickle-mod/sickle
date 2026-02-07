using System;

namespace Scythe.Analytics
{
	// Token: 0x0200065A RID: 1626
	public enum StepIDs
	{
		// Token: 0x04002332 RID: 9010
		none,
		// Token: 0x04002333 RID: 9011
		tut1_00_introduction,
		// Token: 0x04002334 RID: 9012
		tut1_01_end_game_coins,
		// Token: 0x04002335 RID: 9013
		tut1_02_popularity,
		// Token: 0x04002336 RID: 9014
		tut1_02_b_structure_bonus,
		// Token: 0x04002337 RID: 9015
		tut1_03_stars,
		// Token: 0x04002338 RID: 9016
		tut1_04_star_types,
		// Token: 0x04002339 RID: 9017
		tut1_05_star_types_panel,
		// Token: 0x0400233A RID: 9018
		tut1_06_player_mat,
		// Token: 0x0400233B RID: 9019
		tut1_07_mat_section_actions,
		// Token: 0x0400233C RID: 9020
		tut1_08_gain_coin_action,
		// Token: 0x0400233D RID: 9021
		tut1_09_end_turn,
		// Token: 0x0400233E RID: 9022
		tut1_10_disabled_mat_section,
		// Token: 0x0400233F RID: 9023
		tut1_11_star_types_popup,
		// Token: 0x04002340 RID: 9024
		tut1_12_power_star,
		// Token: 0x04002341 RID: 9025
		tut1_13_bolster_power_action,
		// Token: 0x04002342 RID: 9026
		tut1_14_ending,
		// Token: 0x04002343 RID: 9027
		tut2_00_introduction,
		// Token: 0x04002344 RID: 9028
		tut2_01_territories,
		// Token: 0x04002345 RID: 9029
		tut2_02_tunnels,
		// Token: 0x04002346 RID: 9030
		tut2_03_territory_control,
		// Token: 0x04002347 RID: 9031
		tut2_04_home_base,
		// Token: 0x04002348 RID: 9032
		tut2_05_resources,
		// Token: 0x04002349 RID: 9033
		tut2_06_move_goal,
		// Token: 0x0400234A RID: 9034
		tut2_07_move_select,
		// Token: 0x0400234B RID: 9035
		tut2_07_b_player_mat_hint,
		// Token: 0x0400234C RID: 9036
		tut2_08_move_select_mech,
		// Token: 0x0400234D RID: 9037
		tut2_09_move_load_workers,
		// Token: 0x0400234E RID: 9038
		tut2_10_move_mech_to_farm,
		// Token: 0x0400234F RID: 9039
		tut2_11_move_select_character,
		// Token: 0x04002350 RID: 9040
		tut2_12_move_character_load_resources,
		// Token: 0x04002351 RID: 9041
		tut2_13_move_character_to_metal,
		// Token: 0x04002352 RID: 9042
		tut2_14_ending,
		// Token: 0x04002353 RID: 9043
		tut3_00_introduction,
		// Token: 0x04002354 RID: 9044
		tut3_01_combat_win_condition,
		// Token: 0x04002355 RID: 9045
		tut3_02_combat_winer_and_loser,
		// Token: 0x04002356 RID: 9046
		tut3_02_b_open_left_top_menu,
		// Token: 0x04002357 RID: 9047
		tut3_03_expand_stats,
		// Token: 0x04002358 RID: 9048
		tut3_04_stats_enemy,
		// Token: 0x04002359 RID: 9049
		tut3_05_stats_player,
		// Token: 0x0400235A RID: 9050
		tut3_06_bolster_select,
		// Token: 0x0400235B RID: 9051
		tut3_07_end_first_turn,
		// Token: 0x0400235C RID: 9052
		tut3_08_move_select,
		// Token: 0x0400235D RID: 9053
		tut3_09_move_select_mech,
		// Token: 0x0400235E RID: 9054
		tut3_10_move_mech,
		// Token: 0x0400235F RID: 9055
		tut3_11_move_select_second_unit,
		// Token: 0x04002360 RID: 9056
		tut3_12_move_second_unit,
		// Token: 0x04002361 RID: 9057
		tut3_13_battlefield_select,
		// Token: 0x04002362 RID: 9058
		tut3_14_combat_set_power,
		// Token: 0x04002363 RID: 9059
		tut3_15_combat_add_ammo,
		// Token: 0x04002364 RID: 9060
		tut3_16_combat_preparation_summary,
		// Token: 0x04002365 RID: 9061
		tut3_17_combat_result,
		// Token: 0x04002366 RID: 9062
		tut3_18_combat_consequences,
		// Token: 0x04002367 RID: 9063
		tut3_19_combat_card_draw,
		// Token: 0x04002368 RID: 9064
		tut3_20_ending,
		// Token: 0x04002369 RID: 9065
		tut4_00_introduction,
		// Token: 0x0400236A RID: 9066
		tut4_01_factory_hex,
		// Token: 0x0400236B RID: 9067
		tut4_02_move_select,
		// Token: 0x0400236C RID: 9068
		tut4_03_move_select_character,
		// Token: 0x0400236D RID: 9069
		tut4_04_move_to_factory,
		// Token: 0x0400236E RID: 9070
		tut4_05_move_end,
		// Token: 0x0400236F RID: 9071
		tut4_06_factory_card_select,
		// Token: 0x04002370 RID: 9072
		tut4_07_factory_card_info,
		// Token: 0x04002371 RID: 9073
		tut4_08_end_turn,
		// Token: 0x04002372 RID: 9074
		tut4_09_factory_top_select,
		// Token: 0x04002373 RID: 9075
		tut4_10_stats_update,
		// Token: 0x04002374 RID: 9076
		tut4_11_factory_bottom_select,
		// Token: 0x04002375 RID: 9077
		tut4_12_factory_move_select_character,
		// Token: 0x04002376 RID: 9078
		tut4_13_encounter_hex,
		// Token: 0x04002377 RID: 9079
		tut4_14_encounters_info,
		// Token: 0x04002378 RID: 9080
		tut4_15_factory_move_to_encounter,
		// Token: 0x04002379 RID: 9081
		tut4_16_encounter_reveal,
		// Token: 0x0400237A RID: 9082
		tut4_17_encounter_actions,
		// Token: 0x0400237B RID: 9083
		tut4_18_encounter_select_action,
		// Token: 0x0400237C RID: 9084
		tut4_19_ending,
		// Token: 0x0400237D RID: 9085
		tut5_00_introduction,
		// Token: 0x0400237E RID: 9086
		tut5_01_production_info,
		// Token: 0x0400237F RID: 9087
		tut5_02_actual_production,
		// Token: 0x04002380 RID: 9088
		tut5_03_production_select,
		// Token: 0x04002381 RID: 9089
		tut5_04_production_first_hex,
		// Token: 0x04002382 RID: 9090
		tut5_05_produced_resources_info,
		// Token: 0x04002383 RID: 9091
		tut5_06_production_second_hex,
		// Token: 0x04002384 RID: 9092
		tut5_07_end_turn,
		// Token: 0x04002385 RID: 9093
		tut5_08_trade_select,
		// Token: 0x04002386 RID: 9094
		tut5_09_trade_action_first,
		// Token: 0x04002387 RID: 9095
		tut5_10_trade_action_second,
		// Token: 0x04002388 RID: 9096
		tut5_11_trade_summary,
		// Token: 0x04002389 RID: 9097
		tut5_12_ending,
		// Token: 0x0400238A RID: 9098
		tut6_00_introduction,
		// Token: 0x0400238B RID: 9099
		tut6_00_b_upgrade_info,
		// Token: 0x0400238C RID: 9100
		tut6_01_upgrade_select,
		// Token: 0x0400238D RID: 9101
		tut6_02_upgrade_pay,
		// Token: 0x0400238E RID: 9102
		tut6_03_upgrade_move_action,
		// Token: 0x0400238F RID: 9103
		tut6_04_upgrade_deploy_action,
		// Token: 0x04002390 RID: 9104
		tut6_05_upgrade_star,
		// Token: 0x04002391 RID: 9105
		tut6_06_ending,
		// Token: 0x04002392 RID: 9106
		tut7_00_introduction,
		// Token: 0x04002393 RID: 9107
		tut7_00_b_deploy_info,
		// Token: 0x04002394 RID: 9108
		tut7_01_deploy_select,
		// Token: 0x04002395 RID: 9109
		tut7_02_deploy_pay_resources,
		// Token: 0x04002396 RID: 9110
		tut7_02_b_deploy_pay_resources,
		// Token: 0x04002397 RID: 9111
		tut7_03_deploy_select_mech,
		// Token: 0x04002398 RID: 9112
		tut7_04_deploy_select_hex,
		// Token: 0x04002399 RID: 9113
		tut7_05_end_turn,
		// Token: 0x0400239A RID: 9114
		tut7_06_move_select,
		// Token: 0x0400239B RID: 9115
		tut7_07_ability_testing,
		// Token: 0x0400239C RID: 9116
		tut7_08_move_mech,
		// Token: 0x0400239D RID: 9117
		tut7_09_ability_info_mechs,
		// Token: 0x0400239E RID: 9118
		tut7_10_ability_info_character,
		// Token: 0x0400239F RID: 9119
		tut7_11_wait_step,
		// Token: 0x040023A0 RID: 9120
		tut7_11_b_end_turn,
		// Token: 0x040023A1 RID: 9121
		tut7_12_ability_info_workers,
		// Token: 0x040023A2 RID: 9122
		tut7_13_ending,
		// Token: 0x040023A3 RID: 9123
		tut8_00_introduction,
		// Token: 0x040023A4 RID: 9124
		tut8_00_b_build_info,
		// Token: 0x040023A5 RID: 9125
		tut8_01_already_placed_buildings,
		// Token: 0x040023A6 RID: 9126
		tut8_02_move_select,
		// Token: 0x040023A7 RID: 9127
		tut8_03_move_select_character,
		// Token: 0x040023A8 RID: 9128
		tut8_04_move_character,
		// Token: 0x040023A9 RID: 9129
		tut8_05_move_end,
		// Token: 0x040023AA RID: 9130
		tut8_06_end_first_turn,
		// Token: 0x040023AB RID: 9131
		tut8_07_trade_popularity_select,
		// Token: 0x040023AC RID: 9132
		tut8_08_armory_bonus,
		// Token: 0x040023AD RID: 9133
		tut8_09_build_select,
		// Token: 0x040023AE RID: 9134
		tut8_10_build_pay,
		// Token: 0x040023AF RID: 9135
		tut8_11_build_select_mill,
		// Token: 0x040023B0 RID: 9136
		tut8_12_build_mill,
		// Token: 0x040023B1 RID: 9137
		tut8_13_end_second_turn,
		// Token: 0x040023B2 RID: 9138
		tut8_14_produce_select,
		// Token: 0x040023B3 RID: 9139
		tut8_15_mill_bonus_info,
		// Token: 0x040023B4 RID: 9140
		tut8_16_produce,
		// Token: 0x040023B5 RID: 9141
		tut8_17_ending,
		// Token: 0x040023B6 RID: 9142
		tut9_00_introduction,
		// Token: 0x040023B7 RID: 9143
		tut9_00_b_enlist_info,
		// Token: 0x040023B8 RID: 9144
		tut9_01_enlist_select,
		// Token: 0x040023B9 RID: 9145
		tut9_02_stats_panel,
		// Token: 0x040023BA RID: 9146
		tut9_03_pay_resources,
		// Token: 0x040023BB RID: 9147
		tut9_04_enlist_ongoing_bonus,
		// Token: 0x040023BC RID: 9148
		tut9_05_enlist_one_time_bonus,
		// Token: 0x040023BD RID: 9149
		tut9_05_b_enlist_accept,
		// Token: 0x040023BE RID: 9150
		tut9_06_enemy_bonus,
		// Token: 0x040023BF RID: 9151
		tut9_07_two_players_case,
		// Token: 0x040023C0 RID: 9152
		tut9_08_ending,
		// Token: 0x040023C1 RID: 9153
		tut10_00_introduction,
		// Token: 0x040023C2 RID: 9154
		tut10_01_click_objectives_toggle,
		// Token: 0x040023C3 RID: 9155
		tut10_02_objectives_description,
		// Token: 0x040023C4 RID: 9156
		tut10_03_click_faction_toggle,
		// Token: 0x040023C5 RID: 9157
		tut10_04_faction_description,
		// Token: 0x040023C6 RID: 9158
		tut10_05_move_select,
		// Token: 0x040023C7 RID: 9159
		tut10_06_move_workers,
		// Token: 0x040023C8 RID: 9160
		tut10_07_display_objectives,
		// Token: 0x040023C9 RID: 9161
		tut10_08_finish_objective,
		// Token: 0x040023CA RID: 9162
		tut10_09_ending,
		// Token: 0x040023CB RID: 9163
		tut11_00_introduction,
		// Token: 0x040023CC RID: 9164
		tut11_01_click_structure_toggle,
		// Token: 0x040023CD RID: 9165
		tut11_02_structure_bonus,
		// Token: 0x040023CE RID: 9166
		tut11_03_click_score_toggle,
		// Token: 0x040023CF RID: 9167
		tut11_04_polania_score,
		// Token: 0x040023D0 RID: 9168
		tut11_05_nords_score,
		// Token: 0x040023D1 RID: 9169
		tut11_06_score_comparition,
		// Token: 0x040023D2 RID: 9170
		tut11_07_move_select,
		// Token: 0x040023D3 RID: 9171
		tut11_08_move_worker_factory,
		// Token: 0x040023D4 RID: 9172
		tut11_09_move_worker_tunnel,
		// Token: 0x040023D5 RID: 9173
		tut11_10_build_select,
		// Token: 0x040023D6 RID: 9174
		tut11_11_build_pay,
		// Token: 0x040023D7 RID: 9175
		tut11_12_build_select_structure,
		// Token: 0x040023D8 RID: 9176
		tut11_13_build_structure,
		// Token: 0x040023D9 RID: 9177
		tut11_14_ending
	}
}

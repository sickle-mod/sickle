using System;
using System.Collections.Generic;
using System.Globalization;
using Scythe.GameLogic;
using Scythe.UI;

namespace Scythe.Analytics
{
	// Token: 0x0200063C RID: 1596
	public class AnalyticsEvent
	{
		// Token: 0x060032BA RID: 12986 RVA: 0x00048069 File Offset: 0x00046269
		private static string FactionName(Faction faction)
		{
			return Enum.GetName(typeof(Faction), faction).ToLower();
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x001301C0 File Offset: 0x0012E3C0
		public static void EventHeaderData(ref Dictionary<string, object> data, AnalyticsEventTypes type, bool appBoot = false)
		{
			data.Add("user_id", AnalyticsEventData.UserID());
			data.Add("device_id", AnalyticsEventData.DeviceID());
			data.Add("event_type", AnalyticsEventData.EventType(type));
			data.Add("client_event_time", AnalyticsEventData.ClientEventTime());
			data.Add("event_id", AnalyticsEventData.EventID());
			data.Add("insert_id", AnalyticsEventData.InsertID());
			data.Add("session_id", AnalyticsEventData.SessionID());
			data.Add("app_version", AnalyticsEventData.VersionName());
			data.Add("os_name", AnalyticsEventData.OSName());
			data.Add("os_version", AnalyticsEventData.OSVersion());
			data.Add("device_brand", AnalyticsEventData.DeviceBrand());
			data.Add("device_manufacturer", AnalyticsEventData.DeviceManufacturer());
			data.Add("device_family", AnalyticsEventData.DeviceFamily());
			data.Add("device_type", AnalyticsEventData.DeviceType());
			data.Add("device_carrier", AnalyticsEventData.DeviceCarrier());
			data.Add("ip", AnalyticsEventData.IPAddress());
			data.Add("language", AnalyticsEventData.Language());
			data.Add("platform", AnalyticsEventData.Platform());
			data.Add("library", AnalyticsEventData.Library());
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x0013031C File Offset: 0x0012E51C
		private static void EventHeaderUserProperties(ref Dictionary<string, object> userProperties, bool appBoot = false)
		{
			userProperties.Add("app_boot_session_id", AnalyticsEventData.AppBootSessionID());
			userProperties.Add("user_id_first_party", AnalyticsEventData.UserIDFirstParty());
			userProperties.Add("timezone_client", AnalyticsEventData.TimezoneClient());
			userProperties.Add("ab_test_group", AnalyticsEventData.ABTestGroup());
			userProperties.Add("karma", AnalyticsEventData.Karma());
			userProperties.Add("dlc_owned", AnalyticsEventData.DLCOwned());
			userProperties.Add("elo_rating", AnalyticsEventData.ELORating());
			if (!appBoot)
			{
				userProperties.Add("time_ltd", AnalyticsEventData.TimeLTD());
				userProperties.Add("time_ltd_gameplay", AnalyticsEventData.TimeLTDGameplay());
			}
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x001303E0 File Offset: 0x0012E5E0
		private static void EventHeaderEventProperties(ref Dictionary<string, object> eventProperties, bool appBoot = false)
		{
			eventProperties.Add("client_local_time", AnalyticsEventData.ClientLocalTime());
			eventProperties.Add("first_party", AnalyticsEventData.FirstParty());
			eventProperties.Add("environment", AnalyticsEventData.Environment());
			eventProperties.Add("screen_resolution", AnalyticsEventData.ScreenResolution());
			eventProperties.Add("connection_type", AnalyticsEventData.ConnectionType());
			eventProperties.Add("unity_sdk_version", AnalyticsEventData.UnitySDKVersion());
			if (!appBoot)
			{
				eventProperties.Add("time_session", AnalyticsEventData.TimeSession());
				eventProperties.Add("time_session_gameplay", AnalyticsEventData.TimeSessionGameplay());
			}
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x00130484 File Offset: 0x0012E684
		public static Dictionary<string, object> AppBoot(LaunchMethods method)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			AnalyticsEvent.EventHeaderEventProperties(ref dictionary, method == LaunchMethods.boot);
			dictionary.Add("launch_method", AnalyticsEventData.LaunchMethod(method));
			return dictionary;
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x001304B4 File Offset: 0x0012E6B4
		public static Dictionary<string, object> MatchStart()
		{
			AnalyticsEventData.SessionGamePlayTimeStart();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			AnalyticsEvent.EventHeaderEventProperties(ref dictionary, false);
			dictionary.Add("match_session_id", AnalyticsEventData.MatchSessionID());
			dictionary.Add("player_count_human", AnalyticsEventData.PlayerCountHuman());
			dictionary.Add("player_count_ai", AnalyticsEventData.PlayerCountAI());
			dictionary.Add("player_playorder", AnalyticsEventData.PlayerPlayOrder());
			dictionary.Add("launch_method", AnalyticsEventData.MatchLaunchMethod());
			dictionary.Add("is_online", AnalyticsEventData.IsOnline());
			dictionary.Add("mode", AnalyticsEventData.Mode());
			if (GameController.GameManager != null && GameController.GameManager.IsMultiplayer)
			{
				dictionary.Add("is_ranked", AnalyticsEventData.IsRanked());
				dictionary.Add("is_observable", AnalyticsEventData.IsObservable());
				dictionary.Add("obs_access", AnalyticsEventData.ObsAccess());
				dictionary.Add("obs_show_hidden_info", AnalyticsEventData.ObsShowHiddenInfo());
				dictionary.Add("is_observer", AnalyticsEventData.IsObserver());
			}
			dictionary.Add("player_faction", AnalyticsEventData.PlayerFaction());
			dictionary.Add("player_mat", AnalyticsEventData.PlayerMat());
			if (GameController.GameManager != null)
			{
				foreach (Player player in GameController.GameManager.players)
				{
					Faction faction = player.matFaction.faction;
					string text = string.Format("player_faction_{0}", AnalyticsEvent.FactionName(faction));
					string text2 = string.Format("player_faction_{0}_mat", AnalyticsEvent.FactionName(faction));
					dictionary.Add(text, AnalyticsEventData.PlayerForFaction(faction));
					dictionary.Add(text2, AnalyticsEventData.PlayerMatForFaction(faction));
				}
			}
			dictionary.Add("dlc_enabled", AnalyticsEventData.DLCEnabled());
			dictionary.Add("promo_cards_enabled", AnalyticsEventData.PromoCardsEnabled());
			if (GameController.GameManager.IsMultiplayer)
			{
				dictionary.Add("player_clock_duration", AnalyticsEventData.PlayerClockDuration());
			}
			return dictionary;
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x001306D4 File Offset: 0x0012E8D4
		public static Dictionary<string, object> MatchStop(EndReasons reason)
		{
			AnalyticsEventData.UpdateEndgameStats();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			AnalyticsEvent.EventHeaderEventProperties(ref dictionary, false);
			dictionary.Add("match_session_id", AnalyticsEventData.MatchSessionID());
			dictionary.Add("time_active_sec", AnalyticsEventData.TimeActiveSec());
			dictionary.Add("player_count_human", AnalyticsEventData.PlayerCountHuman());
			dictionary.Add("player_count_ai", AnalyticsEventData.PlayerCountAI());
			dictionary.Add("player_playorder", AnalyticsEventData.PlayerPlayOrder());
			dictionary.Add("end_reason", AnalyticsEventData.EndReason(reason));
			if (reason == EndReasons.game_completed)
			{
				dictionary.Add("player_result", AnalyticsEventData.IsObserver() ? "" : AnalyticsEventData.PlayerResult(reason));
			}
			dictionary.Add("is_online", AnalyticsEventData.IsOnline());
			dictionary.Add("mode", AnalyticsEventData.Mode());
			if (GameController.GameManager != null && GameController.GameManager.IsMultiplayer)
			{
				dictionary.Add("is_ranked", AnalyticsEventData.IsRanked());
				dictionary.Add("is_observable", AnalyticsEventData.IsObservable());
				dictionary.Add("is_observer", AnalyticsEventData.IsObserver());
				dictionary.Add("obs_access", AnalyticsEventData.ObsAccess());
				dictionary.Add("obs_show_hidden_info", AnalyticsEventData.ObsShowHiddenInfo());
			}
			dictionary.Add("turn_count", AnalyticsEventData.TurnCount());
			dictionary.Add("player_faction", AnalyticsEventData.PlayerFaction());
			dictionary.Add("player_mat", AnalyticsEventData.PlayerMat());
			if (GameController.GameManager != null)
			{
				foreach (Player player in GameController.GameManager.players)
				{
					Faction faction = player.matFaction.faction;
					string text = string.Format("player_faction_{0}", AnalyticsEvent.FactionName(faction));
					string text2 = string.Format("player_faction_{0}_mat", AnalyticsEvent.FactionName(faction));
					string text3 = string.Format("player_faction_{0}_score", AnalyticsEvent.FactionName(faction));
					dictionary.Add(text, AnalyticsEventData.PlayerForFaction(faction));
					dictionary.Add(text2, AnalyticsEventData.PlayerMatForFaction(faction));
					dictionary.Add(text3, AnalyticsEventData.PlayerScoreForFaction(faction));
				}
			}
			dictionary.Add("bot_swap_count", AnalyticsEventData.BotSwapCount());
			if (!AnalyticsEventData.IsObserver())
			{
				dictionary.Add("player_score_coins", AnalyticsEventData.PlayerScoreCoins());
				dictionary.Add("player_score_structures", AnalyticsEventData.PlayerScoreStructures());
				dictionary.Add("player_score_stars", AnalyticsEventData.PlayerScoreStars());
				dictionary.Add("player_score_hex", AnalyticsEventData.PlayerScoreHex());
				dictionary.Add("player_score_resource_points", AnalyticsEventData.PlayerScoreResourcePoints());
				dictionary.Add("player_score_global", AnalyticsEventData.PlayerScoreGlobal());
				dictionary.Add("player_stat_popularity", AnalyticsEventData.PlayerStatPopularity());
				dictionary.Add("player_stat_objective_count", AnalyticsEventData.PlayerStatObjectiveCount());
				dictionary.Add("player_stat_hex_count", AnalyticsEventData.PlayerStatHexCount());
				dictionary.Add("player_stat_resource_count", AnalyticsEventData.PlayerStatResourceCount());
				dictionary.Add("player_stat_battle_won_count", AnalyticsEventData.PlayerStatBattleWonCount());
				dictionary.Add("player_stat_battle_lost_count", AnalyticsEventData.PlayerStatBattleLostCount());
				dictionary.Add("player_stars_detail", AnalyticsEventData.PlayerStarsDetail());
				dictionary.Add("player_rank", AnalyticsEventData.PlayerRank());
			}
			dictionary.Add("dlc_enabled", AnalyticsEventData.DLCEnabled());
			dictionary.Add("promo_cards_enabled", AnalyticsEventData.PromoCardsEnabled());
			if (!AnalyticsEventData.IsObserver())
			{
				dictionary.Add("player_clock_duration", AnalyticsEventData.PlayerClockDuration());
				dictionary.Add("undo_count", AnalyticsEventData.UndoCount());
			}
			AnalyticsEventData.SessionGameplayTimeEnd();
			AnalyticsEventData.ResetUndoCounter();
			AnalyticsEventData.ResetMatchSessionID();
			return dictionary;
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x00130AD4 File Offset: 0x0012ECD4
		public static Dictionary<string, object> ScreenDisplay(Screens screen, Contexts context)
		{
			AnalyticsEventData.ScreenDisplayed(screen, context);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			AnalyticsEvent.EventHeaderEventProperties(ref dictionary, false);
			dictionary.Add("screen_previous", AnalyticsEventData.ScreenPrevious());
			dictionary.Add("screen_current", AnalyticsEventData.ScreenCurrent());
			dictionary.Add("screen_count", AnalyticsEventData.ScreenCount());
			dictionary.Add("context", AnalyticsEventData.Context());
			dictionary.Add("screen_previous_nav_action", AnalyticsEventData.ScreenPreviousNavAction());
			dictionary.Add("time_screen_previous_sec", AnalyticsEventData.TimeScreenPreviousSec().ToString(CultureInfo.InvariantCulture));
			return dictionary;
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x00130B6C File Offset: 0x0012ED6C
		public static Dictionary<string, object> ScreenDisplay(Screens screen, Screens previousScreen, Contexts context)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			AnalyticsEvent.EventHeaderEventProperties(ref dictionary, false);
			Screens screens = ((AnalyticsEventData.GetCurrentTab() != Screens.none) ? AnalyticsEventData.GetCurrentTab() : previousScreen);
			dictionary.Add("screen_previous", AnalyticsEventData.ScreenName(screens));
			dictionary.Add("screen_current", AnalyticsEventData.ScreenName(screen));
			dictionary.Add("screen_count", AnalyticsEventData.ScreenCount());
			dictionary.Add("context", AnalyticsEventData.Context());
			dictionary.Add("screen_previous_nav_action", AnalyticsEventData.ScreenPreviousNavAction());
			if ((previousScreen >= Screens.tab_enlist && previousScreen <= Screens.tab_faction) || AnalyticsEventData.GetCurrentTab() != Screens.none)
			{
				dictionary.Add("time_screen_previous_sec", AnalyticsEventData.TimeTabPreviousSec().ToString(CultureInfo.InvariantCulture));
				AnalyticsEventData.TabEnabled(false, previousScreen);
			}
			else
			{
				dictionary.Add("time_screen_previous_sec", AnalyticsEventData.TimeScreenPreviousSecForTabs().ToString(CultureInfo.InvariantCulture));
			}
			if (screen >= Screens.tab_enlist && screen <= Screens.tab_faction)
			{
				AnalyticsEventData.TabEnabled(true, screen);
			}
			return dictionary;
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x00130C54 File Offset: 0x0012EE54
		public static Dictionary<string, object> FriendManagement(ActionsOnFriend action, string friendUserID, int friendsCount)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			AnalyticsEvent.EventHeaderEventProperties(ref dictionary, false);
			dictionary.Add("action", AnalyticsEventData.Action(action));
			dictionary.Add("friend_user_id", friendUserID);
			dictionary.Add("friend_count", friendsCount);
			return dictionary;
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x00130CA0 File Offset: 0x0012EEA0
		public static Dictionary<string, object> CreateAsmodeeNetAccount(bool newsletter, SingupPaths path)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			AnalyticsEvent.EventHeaderEventProperties(ref dictionary, false);
			dictionary.Add("is_email_opt_in", AnalyticsEventData.IsEmailOptIn(newsletter));
			dictionary.Add("signup_path", AnalyticsEventData.SignupPath(path));
			return dictionary;
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x00130CE0 File Offset: 0x0012EEE0
		public static Dictionary<string, object> TutorialStep(StepStatuses status)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			AnalyticsEvent.EventHeaderEventProperties(ref dictionary, false);
			dictionary.Add("step_id", AnalyticsEventData.StepID());
			dictionary.Add("step_sequence_number", AnalyticsEventData.StepSequenceNumber());
			dictionary.Add("step_status", AnalyticsEventData.StepStatus(status));
			dictionary.Add("time_on_step", AnalyticsEventData.TimeOnStep().ToString("n3"));
			dictionary.Add("is_tuto_complete", AnalyticsEventData.IsTutoComplete());
			AnalyticsEventData.UpdateTutorialStep();
			return dictionary;
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x00130D6C File Offset: 0x0012EF6C
		public static Dictionary<string, object> AchievementUnlock(Achievements achievement)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			AnalyticsEvent.EventHeaderEventProperties(ref dictionary, false);
			dictionary.Add("achievement_id", AnalyticsEventData.AchievementID(achievement));
			return dictionary;
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x00130D9C File Offset: 0x0012EF9C
		public static Dictionary<string, object> ContentUnlock(Achievements achievement, string contentType, UnlockReasons unlockReason)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			AnalyticsEvent.EventHeaderEventProperties(ref dictionary, false);
			dictionary.Add("content_id", AnalyticsEventData.ContentID(achievement));
			dictionary.Add("content_type", contentType);
			dictionary.Add("unlock_reason", AnalyticsEventData.UnlockReason(unlockReason));
			return dictionary;
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x00048085 File Offset: 0x00046285
		public static Dictionary<string, object> CrossPromoOpened(string crossPromoType, Dictionary<string, object> eventProperties)
		{
			AnalyticsEvent.EventHeaderEventProperties(ref eventProperties, false);
			return eventProperties;
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x00130DE8 File Offset: 0x0012EFE8
		public static Dictionary<string, object> CrossPromoScreenDisplay(Dictionary<string, object> screenDisplayProperties)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			AnalyticsEvent.EventHeaderEventProperties(ref dictionary, false);
			dictionary.Add("crosspromo_session_id", AnalyticsEventData.CrossPromoSessionID());
			dictionary.Add("crosspromo_type", AnalyticsEventData.TryCastString(screenDisplayProperties["crosspromo_type"]));
			dictionary.Add("screen_count", AnalyticsEventData.TryCastInt(screenDisplayProperties["screen_count"]));
			dictionary.Add("screen_current", AnalyticsEventData.TryCastString(screenDisplayProperties["screen_current"]));
			dictionary.Add("screen_previous", AnalyticsEventData.TryCastString(screenDisplayProperties["screen_previous"]));
			dictionary.Add("screen_previous_time_sec", AnalyticsEventData.TryCastInt(screenDisplayProperties["screen_previous_time_sec"]));
			dictionary.Add("screen_previous_nav_action", AnalyticsEventData.TryCastString(screenDisplayProperties["screen_previous_nav_action"]));
			dictionary.Add("more_games_category", AnalyticsEventData.TryCastString(screenDisplayProperties["more_game_category"]));
			dictionary.Add("game_detail_product_id", AnalyticsEventData.TryCastString(screenDisplayProperties["game_detail_product_id"]));
			dictionary.Add("game_detail_product_name", AnalyticsEventData.TryCastString(screenDisplayProperties["game_detail_product_name"]));
			dictionary.Add("game_detail_product_type", AnalyticsEventData.TryCastString(screenDisplayProperties["game_detail_product_type"]));
			dictionary.Add("clicked_crosspromo_tile_size", AnalyticsEventData.TryCastString(screenDisplayProperties["clicked_crosspromo_tile_size"]));
			dictionary.Add("clicked_crosspromo_tile_position_xy", AnalyticsEventData.TryCastString(screenDisplayProperties["clicked_crosspromo_tile_position_xy"]));
			return dictionary;
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x00130F64 File Offset: 0x0012F164
		public static Dictionary<string, object> CrossPromoRedirected(Dictionary<string, object> screenDisplayProperties)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			AnalyticsEvent.EventHeaderEventProperties(ref dictionary, false);
			dictionary.Add("crosspromo_session_id", AnalyticsEventData.CrossPromoSessionID());
			dictionary.Add("crosspromo_type", AnalyticsEventData.TryCastString(screenDisplayProperties["crosspromo_type"]));
			dictionary.Add("more_games_category", AnalyticsEventData.TryCastString(screenDisplayProperties["more_game_category"]));
			dictionary.Add("product_id", AnalyticsEventData.TryCastString(screenDisplayProperties["product_id"]));
			dictionary.Add("product_name", AnalyticsEventData.TryCastString(screenDisplayProperties["product_name"]));
			dictionary.Add("product_type", AnalyticsEventData.TryCastString(screenDisplayProperties["product_type"]));
			return dictionary;
		}

		// Token: 0x060032CB RID: 13003 RVA: 0x00048090 File Offset: 0x00046290
		public static Dictionary<string, object> CrossPromoClosed(Dictionary<string, object> eventProperties)
		{
			AnalyticsEvent.EventHeaderEventProperties(ref eventProperties, false);
			return eventProperties;
		}

		// Token: 0x060032CC RID: 13004 RVA: 0x00131018 File Offset: 0x0012F218
		public static Dictionary<string, object> PusnNotificationReceived(PushTypes pushType, bool pushOpen, string title, string title2, string message)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			AnalyticsEvent.EventHeaderEventProperties(ref dictionary, false);
			dictionary.Add("push_type", AnalyticsEventData.TryCastString(pushType.ToString()));
			dictionary.Add("push_open", AnalyticsEventData.BoolToString(pushOpen));
			dictionary.Add("title", title);
			dictionary.Add("title2", title2);
			dictionary.Add("message", message);
			return dictionary;
		}

		// Token: 0x060032CD RID: 13005 RVA: 0x00131088 File Offset: 0x0012F288
		public static Dictionary<string, object> GetEventHeaderData(AnalyticsEventTypes type)
		{
			return new Dictionary<string, object>
			{
				{
					"user_id",
					AnalyticsEventData.UserID()
				},
				{
					"device_id",
					AnalyticsEventData.DeviceID()
				},
				{
					"event_type",
					AnalyticsEventData.EventType(type)
				},
				{
					"client_event_time",
					AnalyticsEventData.ClientEventTime()
				},
				{
					"event_id",
					AnalyticsEventData.EventID()
				},
				{
					"insert_id",
					AnalyticsEventData.InsertID()
				},
				{
					"session_id",
					AnalyticsEventData.SessionID()
				},
				{
					"app_version",
					AnalyticsEventData.VersionName()
				},
				{
					"os_name",
					AnalyticsEventData.OSName()
				},
				{
					"os_version",
					AnalyticsEventData.OSVersion()
				},
				{
					"device_brand",
					AnalyticsEventData.DeviceBrand()
				},
				{
					"device_manufacturer",
					AnalyticsEventData.DeviceManufacturer()
				},
				{
					"device_family",
					AnalyticsEventData.DeviceFamily()
				},
				{
					"device_type",
					AnalyticsEventData.DeviceType()
				},
				{
					"device_carrier",
					AnalyticsEventData.DeviceCarrier()
				},
				{
					"ip",
					AnalyticsEventData.IPAddress()
				},
				{
					"language",
					AnalyticsEventData.Language()
				},
				{
					"platform",
					AnalyticsEventData.Platform()
				},
				{
					"library",
					AnalyticsEventData.Library()
				}
			};
		}

		// Token: 0x060032CE RID: 13006 RVA: 0x001311D8 File Offset: 0x0012F3D8
		public static Dictionary<string, object> GetUserProperties(bool appBoot = false)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("app_boot_session_id", AnalyticsEventData.AppBootSessionID());
			dictionary.Add("push_platform", AnalyticsEventData.PushPlatform());
			dictionary.Add("push_user_id", AnalyticsEventData.PushUserID());
			dictionary.Add("user_id_first_party", AnalyticsEventData.UserIDFirstParty());
			dictionary.Add("timezone_client", AnalyticsEventData.TimezoneClient());
			dictionary.Add("ab_test_group", AnalyticsEventData.ABTestGroup());
			dictionary.Add("karma", AnalyticsEventData.Karma());
			dictionary.Add("dlc_owned", AnalyticsEventData.DLCOwned());
			dictionary.Add("elo_rating", AnalyticsEventData.ELORating());
			if (!appBoot)
			{
				dictionary.Add("time_ltd", AnalyticsEventData.TimeLTD());
				dictionary.Add("time_ltd_gameplay", AnalyticsEventData.TimeLTDGameplay());
			}
			return dictionary;
		}

		// Token: 0x060032CF RID: 13007 RVA: 0x001312B8 File Offset: 0x0012F4B8
		private static Dictionary<string, object> GetHeaderEventProperties(bool appBoot = false)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("client_local_time", AnalyticsEventData.ClientLocalTime());
			dictionary.Add("first_party", AnalyticsEventData.FirstParty());
			dictionary.Add("environment", AnalyticsEventData.Environment());
			dictionary.Add("screen_resolution", AnalyticsEventData.ScreenResolution());
			dictionary.Add("connection_type", AnalyticsEventData.ConnectionType());
			dictionary.Add("unity_sdk_version", AnalyticsEventData.UnitySDKVersion());
			if (!appBoot)
			{
				dictionary.Add("time_session", AnalyticsEventData.TimeSession());
				dictionary.Add("time_session_gameplay", AnalyticsEventData.TimeSessionGameplay());
			}
			return dictionary;
		}

		// Token: 0x040021E4 RID: 8676
		private const string EVENT_TYPE = "event_type";

		// Token: 0x040021E5 RID: 8677
		private const string USER_PROPERTIES = "user_properties";

		// Token: 0x040021E6 RID: 8678
		private const string EVENT_PROPERTIES = "event_properties";
	}
}

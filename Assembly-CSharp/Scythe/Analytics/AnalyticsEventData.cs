using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsmodeeNet.Foundation;
using Firebase;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using Scythe.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scythe.Analytics
{
	// Token: 0x02000640 RID: 1600
	public class AnalyticsEventData
	{
		// Token: 0x060032FA RID: 13050 RVA: 0x00131698 File Offset: 0x0012F898
		public static string AchievementID(Achievements achievement)
		{
			int num = (int)achievement;
			return num.ToString();
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x000482EE File Offset: 0x000464EE
		public static string LaunchMethod(LaunchMethods method)
		{
			return Enum.GetName(typeof(LaunchMethods), method);
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x00048305 File Offset: 0x00046505
		public static string ContentID(Achievements achievement)
		{
			return AnalyticsEventData.AchievementID(achievement);
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x00031720 File Offset: 0x0002F920
		public static string ContentType()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x0004830D File Offset: 0x0004650D
		public static string UnlockReason(UnlockReasons unlockReason)
		{
			return unlockReason.ToString();
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x0004831C File Offset: 0x0004651C
		public static string IsEmailOptIn(bool option)
		{
			return option.ToString();
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x00048325 File Offset: 0x00046525
		public static string SignupPath(SingupPaths path)
		{
			return Enum.GetName(typeof(SingupPaths), path);
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x0004833C File Offset: 0x0004653C
		public static void SetCrossPromoPreviousScreen(string previousScreen)
		{
			AnalyticsEventData.crossPromoPreviousScreen = previousScreen;
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x001316B0 File Offset: 0x0012F8B0
		public static void CreateCrossPromoSessionID()
		{
			AnalyticsEventData.crossPromoSessionID = Guid.NewGuid().ToString();
		}

		// Token: 0x06003303 RID: 13059 RVA: 0x00048344 File Offset: 0x00046544
		public static void CrossPromoWindowOpenedTimestamp()
		{
			AnalyticsEventData.crossPromoOpened = Time.time;
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x00048350 File Offset: 0x00046550
		public static string CrossPromoSessionID()
		{
			return AnalyticsEventData.crossPromoSessionID;
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x00048357 File Offset: 0x00046557
		public static string ApiVersion()
		{
			return AnalyticsEventData.UnitySDKVersion();
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x0004835E File Offset: 0x0004655E
		public static string IsAutomatic(bool isAutomatic)
		{
			if (!isAutomatic)
			{
				return "False";
			}
			return "True";
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x0004836E File Offset: 0x0004656E
		public static string TryCastString(object value)
		{
			if (value != null)
			{
				return value.ToString();
			}
			return string.Empty;
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x0004837F File Offset: 0x0004657F
		public static int TryCastInt(object value)
		{
			if (value != null)
			{
				return (int)value;
			}
			return -1;
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x0004838C File Offset: 0x0004658C
		public static int CrossPromoTimeActiveSec()
		{
			return (int)(Time.time - AnalyticsEventData.crossPromoOpened);
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x0004839A File Offset: 0x0004659A
		public static string CrossPromoPreviousScreen()
		{
			return AnalyticsEventData.crossPromoPreviousScreen;
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x0004835E File Offset: 0x0004655E
		public static string BoolToString(bool boolean)
		{
			if (!boolean)
			{
				return "False";
			}
			return "True";
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x000483A1 File Offset: 0x000465A1
		public static string Action(ActionsOnFriend action)
		{
			return Enum.GetName(typeof(ActionsOnFriend), action);
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x000483B8 File Offset: 0x000465B8
		public static void IncreaseEventID()
		{
			PlayerPrefs.SetInt("EventId", PlayerPrefs.GetInt("EventId", 1) + 1);
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x001316D8 File Offset: 0x0012F8D8
		public static void CreateAppBootSessionID()
		{
			AnalyticsEventData.appBootSessionID = Guid.NewGuid().ToString();
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x000483D1 File Offset: 0x000465D1
		public static void CleanSessionTimes()
		{
			AnalyticsEventData.appStopped = false;
			AnalyticsEventData.timeGameplaySeconds = 0L;
			PlayerPrefs.SetString("TimeSessionStart", "");
			PlayerPrefs.SetString("TimeSessionGameplayStart", "");
			PlayerPrefs.SetString("TimeSessionGameplay", "");
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x0004840D File Offset: 0x0004660D
		public static void SessionStop()
		{
			AnalyticsEventData.appStopped = true;
			AnalyticsEventData.timeStopDate = DateTime.UtcNow;
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x00131700 File Offset: 0x0012F900
		public static void SessionTimeStart()
		{
			AnalyticsEventData.UpdateSessionStart();
			PlayerPrefs.SetString("TimeSessionStart", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x00131730 File Offset: 0x0012F930
		public static void UpdateSessionStart()
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			AnalyticsEventData.sessionStart = (long)(DateTime.UtcNow - dateTime).TotalMilliseconds;
			AnalyticsEventData.timeSessionTimestamp = AnalyticsEventData.TimeSession();
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x0004841F File Offset: 0x0004661F
		public static void SessionTimeEnd(bool inGameplay = false)
		{
			AnalyticsEventData.UpdateLTDTime();
			if (inGameplay)
			{
				AnalyticsEventData.UpdateLTDGameplayTime();
			}
			AnalyticsEventData.CleanSessionTimes();
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x00131774 File Offset: 0x0012F974
		public static void SessionGamePlayTimeStart()
		{
			AnalyticsEventData.timeSessionGameplayTimestamp = AnalyticsEventData.TimeSessionGameplay();
			AnalyticsEventData.UpdateGamePlayTime();
			PlayerPrefs.SetString("TimeSessionGameplayStart", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x00048433 File Offset: 0x00046633
		public static void SessionGameplayTimeEnd()
		{
			AnalyticsEventData.UpdateSessionGameplayTime();
			PlayerPrefs.SetString("TimeSessionGameplayStart", "");
			AnalyticsEventData.timeGameplaySeconds = 0L;
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x001317AC File Offset: 0x0012F9AC
		public static void UpdateSessionGameplayTime()
		{
			PlayerPrefs.SetString("TimeSessionGameplay", AnalyticsEventData.TimeSessionGameplay().ToString());
			AnalyticsEventData.UpdateLTDGameplayTime();
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x001317D8 File Offset: 0x0012F9D8
		public static void UpdateGamePlayTime()
		{
			DateTime dateTime;
			if (DateTime.TryParse(PlayerPrefs.GetString("TimeSessionGameplayStart", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffff")), out dateTime))
			{
				if (AnalyticsEventData.appStopped)
				{
					AnalyticsEventData.timeGameplaySeconds += (long)(AnalyticsEventData.timeStopDate - dateTime).TotalSeconds;
				}
				else
				{
					AnalyticsEventData.timeGameplaySeconds += (long)(DateTime.UtcNow - dateTime).TotalSeconds;
				}
			}
			AnalyticsEventData.appStopped = false;
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x00048450 File Offset: 0x00046650
		public static void UpdateLTDTime()
		{
			AnalyticsEventData.TimeLTD();
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x00048458 File Offset: 0x00046658
		public static void UpdateLTDGameplayTime()
		{
			AnalyticsEventData.TimeLTDGameplay();
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x00048460 File Offset: 0x00046660
		public static void UpdateIP(string ip)
		{
			AnalyticsEventData.ip = ip.TrimEnd();
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x0004846D File Offset: 0x0004666D
		public static void UpdateEnvironmentType()
		{
			RuntimePlatform platform = Application.platform;
			AnalyticsEventData.SetEnvironnement(Environments.prod);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x0004847B File Offset: 0x0004667B
		private static void SetEnvironnement(Environments enviro)
		{
			AnalyticsEventData.environmentType = enviro;
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x00131858 File Offset: 0x0012FA58
		public static string DeviceID()
		{
			string text = "";
			RuntimePlatform platform = Application.platform;
			if (platform != RuntimePlatform.OSXEditor && platform != RuntimePlatform.WindowsEditor && platform != RuntimePlatform.LinuxEditor && PlayerPrefs.HasKey("DeviceId"))
			{
				text = PlayerPrefs.GetString("DeviceId");
			}
			if (text.Equals(""))
			{
				text = SystemInfo.deviceUniqueIdentifier;
				PlayerPrefs.SetString("DeviceId", text);
			}
			return text;
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x001318B4 File Offset: 0x0012FAB4
		public static string UserID()
		{
			string text = "";
			try
			{
				if (AsmodeeLogic.Instance.GetUser() != null)
				{
					text = AsmodeeLogic.Instance.GetUser().UserId.ToString();
				}
			}
			catch
			{
			}
			if (text == null)
			{
				return "";
			}
			return text;
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x0013190C File Offset: 0x0012FB0C
		public static string ClientEventTime()
		{
			return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffff");
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x0013190C File Offset: 0x0012FB0C
		public static string ClientUploadTime()
		{
			return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffff");
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x00048483 File Offset: 0x00046683
		public static int EventID()
		{
			int @int = PlayerPrefs.GetInt("EventId", 0);
			AnalyticsEventData.IncreaseEventID();
			return @int;
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x0013192C File Offset: 0x0012FB2C
		public static string InsertID()
		{
			return Guid.NewGuid().ToString();
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x00048495 File Offset: 0x00046695
		public static long SessionID()
		{
			return AnalyticsEventData.sessionStart;
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x0004849C File Offset: 0x0004669C
		public static string EventType(AnalyticsEventTypes type)
		{
			return Enum.GetName(typeof(AnalyticsEventTypes), type);
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x000484B3 File Offset: 0x000466B3
		public static string VersionName()
		{
			return BuildVersionUtility.GetBuildVersion();
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x000484BA File Offset: 0x000466BA
		public static string OSName()
		{
			return Enum.GetName(typeof(OperatingSystemFamily), SystemInfo.operatingSystemFamily);
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x0002E89F File Offset: 0x0002CA9F
		public static string OSVersion()
		{
			return SystemInfo.operatingSystem;
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x0002F5FA File Offset: 0x0002D7FA
		public static string DeviceBrand()
		{
			return "";
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x0002F5FA File Offset: 0x0002D7FA
		public static string DeviceManufacturer()
		{
			return "";
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x000484D5 File Offset: 0x000466D5
		public static string DeviceFamily()
		{
			if (PlatformManager.IsSteam)
			{
				return "PC";
			}
			return "";
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x000484E9 File Offset: 0x000466E9
		public static string DeviceType()
		{
			return Enum.GetName(typeof(DeviceType), SystemInfo.deviceType);
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x0002F5FA File Offset: 0x0002D7FA
		public static string DeviceCarrier()
		{
			return "";
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x00048504 File Offset: 0x00046704
		public static string IPAddress()
		{
			return AnalyticsEventData.ip;
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x0013194C File Offset: 0x0012FB4C
		public static string Language()
		{
			return Application.systemLanguage.ToString();
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x0004850B File Offset: 0x0004670B
		public static string Platform()
		{
			return Enum.GetName(typeof(RuntimePlatform), Application.platform);
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x00048526 File Offset: 0x00046726
		public static string Library()
		{
			if (PlatformManager.IsMobile && !Application.isEditor)
			{
				return "amplitude-unity-2.10.0";
			}
			return "http/1.0";
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x00048541 File Offset: 0x00046741
		public static string AppBootSessionID()
		{
			return AnalyticsEventData.appBootSessionID;
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x0013196C File Offset: 0x0012FB6C
		public static Guid BackendUserID()
		{
			Guid guid = Guid.Empty;
			if (PlayerInfo.me != null && PlayerInfo.me.PlayerStats != null)
			{
				guid = PlayerInfo.me.PlayerStats.Id;
			}
			return guid;
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x00048548 File Offset: 0x00046748
		public static string BackendPlatform()
		{
			return "Microsoft Azure";
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x0004854F File Offset: 0x0004674F
		public static string UAPlatform()
		{
			return "Asmodee Digital";
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x00048556 File Offset: 0x00046756
		public static string UAUserID()
		{
			return AnalyticsEventData.UserID();
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x0002F5FA File Offset: 0x0002D7FA
		public static string UAChannel()
		{
			return "";
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x0004855D File Offset: 0x0004675D
		public static string PushPlatform()
		{
			if (PlatformManager.IsMobile)
			{
				return "Firebase";
			}
			return "";
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x00048571 File Offset: 0x00046771
		public static string PushUserID()
		{
			if (!Application.isEditor && PlatformManager.IsMobile && AnalyticsEventData.IsNotificationSystemReady && FirebaseApp.DefaultInstance != null)
			{
				return FirebaseApp.DefaultInstance.Options.MessageSenderId;
			}
			return "";
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x001319A4 File Offset: 0x0012FBA4
		public static string UserIDFirstParty()
		{
			if (!PlatformManager.IsSteam)
			{
				return GameServiceController.Instance.PlayerId();
			}
			if (CoreApplication.Instance.OAuthGate != null && CoreApplication.Instance.OAuthGate.SteamManager != null && CoreApplication.Instance.OAuthGate.SteamManager.HasClient)
			{
				return CoreApplication.Instance.OAuthGate.SteamManager.Me.PartnerUser;
			}
			return string.Empty;
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x00131A18 File Offset: 0x0012FC18
		public static int TimezoneClient()
		{
			return DateTimeOffset.Now.Offset.Hours * 60 + DateTimeOffset.Now.Offset.Minutes;
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x00131A54 File Offset: 0x0012FC54
		public static string ClientLocalTime()
		{
			return DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffff");
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x00131A74 File Offset: 0x0012FC74
		public static string FirstParty()
		{
			if (PlatformManager.IsSteam)
			{
				return Enum.GetName(typeof(FirstParties), FirstParties.Steam);
			}
			if (PlatformManager.IsIPhone || PlatformManager.IsIPad)
			{
				return Enum.GetName(typeof(FirstParties), FirstParties.AppStore);
			}
			if (PlatformManager.IsAndroid)
			{
				return Enum.GetName(typeof(FirstParties), FirstParties.GooglePlay);
			}
			return Enum.GetName(typeof(FirstParties), FirstParties.Standalone);
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x000485A4 File Offset: 0x000467A4
		public static string Environment()
		{
			return Enum.GetName(typeof(Environments), AnalyticsEventData.environmentType);
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x00131AF4 File Offset: 0x0012FCF4
		public static long TimeSession()
		{
			DateTime dateTime;
			if (!DateTime.TryParse(PlayerPrefs.GetString("TimeSessionStart", ""), out dateTime))
			{
				return 0L;
			}
			return (long)(DateTime.Now - dateTime).TotalSeconds;
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x00131B30 File Offset: 0x0012FD30
		public static long TimeSessionGameplay()
		{
			long num = 0L;
			string text = PlayerPrefs.GetString("TimeSessionGameplay", "0");
			if (text.Equals(""))
			{
				text = "0";
			}
			long num2 = Convert.ToInt64(text);
			DateTime dateTime;
			if (DateTime.TryParse(PlayerPrefs.GetString("TimeSessionGameplayStart", ""), out dateTime))
			{
				num = (long)(DateTime.Now - dateTime).TotalSeconds;
			}
			return num2 + num;
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x00131B98 File Offset: 0x0012FD98
		public static long TimeLTD()
		{
			long num = Convert.ToInt64(PlayerPrefs.GetString("TimeLTD", "0"));
			long num2 = AnalyticsEventData.TimeSession();
			long num3 = num2 - AnalyticsEventData.timeSessionTimestamp;
			long num4 = num + num3;
			PlayerPrefs.SetString("TimeLTD", num4.ToString());
			AnalyticsEventData.timeSessionTimestamp = num2;
			return num4;
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x00131BE4 File Offset: 0x0012FDE4
		public static long TimeLTDGameplay()
		{
			long num = Convert.ToInt64(PlayerPrefs.GetString("TimeLTDGameplay", "0"));
			long num2 = AnalyticsEventData.TimeSessionGameplay();
			long num3 = num2 - AnalyticsEventData.timeSessionGameplayTimestamp;
			long num4 = num + num3;
			PlayerPrefs.SetString("TimeLTDGameplay", num4.ToString());
			AnalyticsEventData.timeSessionGameplayTimestamp = num2;
			return num4;
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x00131C30 File Offset: 0x0012FE30
		public static string ScreenResolution()
		{
			return string.Format("{0}*{1}", Screen.currentResolution.width, Screen.currentResolution.height);
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x00131C6C File Offset: 0x0012FE6C
		public static string ConnectionType()
		{
			switch (Application.internetReachability)
			{
			case NetworkReachability.ReachableViaCarrierDataNetwork:
				return Enum.GetName(typeof(ConnectionTypes), ConnectionTypes.carrier_data_network);
			case NetworkReachability.ReachableViaLocalAreaNetwork:
				return Enum.GetName(typeof(ConnectionTypes), ConnectionTypes.wifi);
			}
			return Enum.GetName(typeof(ConnectionTypes), ConnectionTypes.not_connected);
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x000485BF File Offset: 0x000467BF
		public static string UnitySDKVersion()
		{
			return SDKVersionManager.Version();
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x0002F5FA File Offset: 0x0002D7FA
		public static string ABTestGroup()
		{
			return "";
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x000485C6 File Offset: 0x000467C6
		public static int Karma()
		{
			if (PlayerInfo.me == null)
			{
				return -1;
			}
			return PlayerInfo.me.PlayerStats.Karma;
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x00131CD4 File Offset: 0x0012FED4
		public static string DLCOwned()
		{
			string text = "";
			if (GameServiceController.Instance.InvadersFromAfarUnlocked())
			{
				text += "invaders from afar";
			}
			return text;
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x000485E0 File Offset: 0x000467E0
		public static int ELORating()
		{
			if (PlayerInfo.me == null)
			{
				return -1;
			}
			return PlayerInfo.me.PlayerStats.ELO;
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x000485FA File Offset: 0x000467FA
		public static void IncreaseUndoCounter()
		{
			AnalyticsEventData.undoCounter++;
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x00048608 File Offset: 0x00046808
		public static void ResetUndoCounter()
		{
			AnalyticsEventData.undoCounter = 0;
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x00048610 File Offset: 0x00046810
		public static void SetMatchLaunchMethod(MatchLaunchMethods matchLaunchMethod)
		{
			AnalyticsEventData.matchLaunchMethod = matchLaunchMethod;
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x00048618 File Offset: 0x00046818
		public static void UpdateEndgameStats()
		{
			AnalyticsEventData.endgameStats = GameController.GameManager.CalculateStats();
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x00048629 File Offset: 0x00046829
		public static void UpdateMatchSessionID(string loadedMatchID)
		{
			if (AnalyticsEventData.saveTestLoading)
			{
				return;
			}
			AnalyticsEventData.matchSessionID = loadedMatchID;
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x00131D00 File Offset: 0x0012FF00
		public static void CreateMatchSessionID()
		{
			if (!GameController.GameManager.IsMultiplayer)
			{
				string text = AnalyticsEventData.DeviceID();
				byte[] bytes = BitConverter.GetBytes((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
				byte[] bytes2 = Encoding.ASCII.GetBytes(text.Substring(0, 12));
				byte[] array = new byte[bytes.Length + bytes2.Length];
				Array.Copy(bytes, array, bytes.Length);
				Array.Copy(bytes2, 0, array, bytes.Length, bytes2.Length);
				AnalyticsEventData.UpdateMatchSessionID(new Guid(array).ToString());
				return;
			}
			if (PlayerInfo.me != null)
			{
				AnalyticsEventData.UpdateMatchSessionID(PlayerInfo.me.RoomId);
			}
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x00048639 File Offset: 0x00046839
		public static void ResetMatchSessionID()
		{
			AnalyticsEventData.matchSessionID = "";
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x00048645 File Offset: 0x00046845
		public static void SetMatchSessionIDUpdateLockState(bool enable)
		{
			AnalyticsEventData.saveTestLoading = enable;
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x0004864D File Offset: 0x0004684D
		public static string MatchSessionID()
		{
			if (AnalyticsEventData.matchSessionID != null && AnalyticsEventData.matchSessionID.Length != 0)
			{
				return AnalyticsEventData.matchSessionID;
			}
			AnalyticsEventData.CreateMatchSessionID();
			return AnalyticsEventData.matchSessionID;
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x00048672 File Offset: 0x00046872
		public static int PlayerCountHuman()
		{
			return GameController.GameManager.GetPlayersWithoutAICount();
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x0004867E File Offset: 0x0004687E
		public static int PlayerCountAI()
		{
			return GameController.GameManager.GetAIPlayersCount();
		}

		// Token: 0x06003354 RID: 13140 RVA: 0x00131DB8 File Offset: 0x0012FFB8
		public static int PlayerPlayOrder()
		{
			Player player = null;
			if (GameController.GameManager.IsMultiplayer)
			{
				player = GameController.GameManager.PlayerOwner;
			}
			else if (GameController.GameManager.GetPlayersWithoutAICount() == 1)
			{
				player = GameController.GameManager.GetPlayersWithoutAI().ToList<Player>()[0];
			}
			if (player != null)
			{
				return GameController.GameManager.GetPlayerLocalId(player) + 1;
			}
			return -1;
		}

		// Token: 0x06003355 RID: 13141 RVA: 0x0004868A File Offset: 0x0004688A
		public static string MatchLaunchMethod()
		{
			return Enum.GetName(typeof(MatchLaunchMethods), AnalyticsEventData.matchLaunchMethod);
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x000486A5 File Offset: 0x000468A5
		public static bool IsOnline()
		{
			return GameController.GameManager.IsMultiplayer;
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x00131E18 File Offset: 0x00130018
		public static string Mode()
		{
			if (GameController.GameManager == null)
			{
				return "";
			}
			if (GameController.GameManager.IsHotSeat)
			{
				if (AnalyticsEventData.PlayerCountHuman() == 1)
				{
					return Enum.GetName(typeof(Modes), Modes.solo);
				}
				return Enum.GetName(typeof(Modes), Modes.pass_and_play);
			}
			else
			{
				if (GameController.GameManager.IsCampaign)
				{
					return Enum.GetName(typeof(Modes), Modes.tutorial);
				}
				if (!GameController.GameManager.IsMultiplayer)
				{
					return "";
				}
				if (GameController.GameManager.IsRanked)
				{
					if (GameController.GameManager.IsAsynchronous)
					{
						return Enum.GetName(typeof(Modes), Modes.play_and_go_ranked);
					}
					return Enum.GetName(typeof(Modes), Modes.play_and_stay_ranked);
				}
				else
				{
					if (GameController.GameManager.IsAsynchronous)
					{
						return Enum.GetName(typeof(Modes), Modes.play_and_go_unranked);
					}
					return Enum.GetName(typeof(Modes), Modes.play_and_stay_unranked);
				}
			}
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x000486B1 File Offset: 0x000468B1
		public static bool IsRanked()
		{
			return GameController.GameManager.IsRanked;
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x000486A5 File Offset: 0x000468A5
		public static bool IsObservable()
		{
			return GameController.GameManager.IsMultiplayer;
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x00131F24 File Offset: 0x00130124
		public static string ObsAccess()
		{
			if (!GameController.GameManager.IsPrivate)
			{
				return ObserverAccess.everyone.ToString();
			}
			return ObserverAccess.friends_only.ToString();
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public static bool ObsShowHiddenInfo()
		{
			return false;
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x000486BD File Offset: 0x000468BD
		public static bool IsObserver()
		{
			return GameController.GameManager.SpectatorMode;
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x00131F5C File Offset: 0x0013015C
		public static string PlayerFaction()
		{
			Faction faction;
			if (GameController.GameManager.IsMultiplayer)
			{
				faction = GameController.GameManager.PlayerOwner.matFaction.faction;
			}
			else
			{
				if (GameController.GameManager.GetPlayersWithoutAICount() != 1)
				{
					return "Unknown";
				}
				faction = GameController.GameManager.GetPlayersWithoutAI().ToList<Player>()[0].matFaction.faction;
			}
			return Enum.GetName(typeof(Faction), faction);
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x00131FD8 File Offset: 0x001301D8
		public static string PlayerMat()
		{
			PlayerMatType playerMatType;
			if (GameController.GameManager.IsMultiplayer)
			{
				playerMatType = GameController.GameManager.PlayerOwner.matPlayer.matType;
			}
			else
			{
				if (GameController.GameManager.GetPlayersWithoutAICount() != 1)
				{
					return "Unknown";
				}
				playerMatType = GameController.GameManager.GetPlayersWithoutAI().ToList<Player>()[0].matPlayer.matType;
			}
			return Enum.GetName(typeof(PlayerMatType), playerMatType);
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x00132054 File Offset: 0x00130254
		public static string PlayerForFaction(Faction faction)
		{
			Player player = GameController.GameManager.GetPlayerByFaction(faction);
			if (player == null)
			{
				return "";
			}
			if (player.IsHuman)
			{
				if (!AnalyticsEventData.IsOnline())
				{
					return Enum.GetName(typeof(PlayerTypes), PlayerTypes.human_local);
				}
				IEnumerable<PlayerData> playersInGame = MultiplayerController.Instance.GetPlayersInGame();
				if (playersInGame == null)
				{
					return "";
				}
				return playersInGame.FirstOrDefault((PlayerData p) => p.Faction == (int)player.matFaction.faction).Id.ToString();
			}
			else
			{
				switch (player.aiDifficulty)
				{
				case AIDifficulty.Easy:
					return Enum.GetName(typeof(PlayerTypes), PlayerTypes.ai_easy);
				case AIDifficulty.Medium:
					return Enum.GetName(typeof(PlayerTypes), PlayerTypes.ai_medium);
				case AIDifficulty.Hard:
					return Enum.GetName(typeof(PlayerTypes), PlayerTypes.ai_hard);
				default:
					return Enum.GetName(typeof(PlayerTypes), PlayerTypes.ai_medium);
				}
			}
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x00132164 File Offset: 0x00130364
		public static string PlayerMatForFaction(Faction faction)
		{
			Player playerByFaction = GameController.GameManager.GetPlayerByFaction(faction);
			if (playerByFaction == null)
			{
				return "";
			}
			return Enum.GetName(typeof(PlayerMatType), playerByFaction.matPlayer.matType);
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x001321A8 File Offset: 0x001303A8
		public static string DLCEnabled()
		{
			string text = "";
			if (GameServiceController.Instance.InvadersFromAfarUnlocked())
			{
				text += "Invaders from Afar";
				text += ",";
			}
			if (string.IsNullOrEmpty(text) && PlatformManager.IsMobile)
			{
				text = "none";
			}
			return text;
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x000486C9 File Offset: 0x000468C9
		public static string PromoCardsEnabled()
		{
			if (GameController.GameManager.PromoCardsEnabled())
			{
				return GameController.GameManager.GetUnlockedPromoCards();
			}
			return "";
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x000486E7 File Offset: 0x000468E7
		public static int PlayerClockDuration()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				return MultiplayerController.Instance.StartingPlayerClock;
			}
			return -1;
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x00048701 File Offset: 0x00046901
		public static long TimeActiveSec()
		{
			AnalyticsEventData.UpdateGamePlayTime();
			return AnalyticsEventData.timeGameplaySeconds;
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x0004870D File Offset: 0x0004690D
		public static string EndReason(EndReasons reason)
		{
			return Enum.GetName(typeof(EndReasons), reason);
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x001321F8 File Offset: 0x001303F8
		public static string PlayerResult(EndReasons reason)
		{
			if (GameController.GameManager == null)
			{
				return "";
			}
			if (AnalyticsEventData.PlayerRank() != 1 || reason != EndReasons.game_completed)
			{
				return Enum.GetName(typeof(PlayerResults), PlayerResults.defeat);
			}
			return Enum.GetName(typeof(PlayerResults), PlayerResults.victory);
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x00048724 File Offset: 0x00046924
		public static int TurnCount()
		{
			if (GameController.GameManager == null)
			{
				return 0;
			}
			return GameController.GameManager.TurnCount;
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x0013224C File Offset: 0x0013044C
		public static int PlayerScoreForFaction(Faction faction)
		{
			if (GameController.GameManager == null)
			{
				return -1;
			}
			PlayerEndGameStats playerEndGameStats = AnalyticsEventData.endgameStats.Find((PlayerEndGameStats entry) => entry.player.matFaction.faction == faction);
			if (playerEndGameStats == null)
			{
				return -1;
			}
			return playerEndGameStats.totalPoints;
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x00048739 File Offset: 0x00046939
		public static int BotSwapCount()
		{
			if (!GameController.GameManager.IsMultiplayer)
			{
				return 0;
			}
			return MultiplayerController.Instance.NumberOfPlayersOverridedByAi();
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x00132294 File Offset: 0x00130494
		public static int PlayerScoreCoins()
		{
			if (GameController.GameManager == null)
			{
				return -1;
			}
			PlayerEndGameStats stats = AnalyticsEventData.GetStats();
			if (stats == null)
			{
				return -1;
			}
			return stats.coinPoints;
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x001322BC File Offset: 0x001304BC
		public static int PlayerScoreStructures()
		{
			if (GameController.GameManager == null)
			{
				return -1;
			}
			PlayerEndGameStats stats = AnalyticsEventData.GetStats();
			if (stats == null)
			{
				return -1;
			}
			return stats.structurePoints;
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x001322E4 File Offset: 0x001304E4
		public static int PlayerScoreStars()
		{
			if (GameController.GameManager == null)
			{
				return -1;
			}
			PlayerEndGameStats stats = AnalyticsEventData.GetStats();
			if (stats == null)
			{
				return -1;
			}
			return stats.starPoints;
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x0013230C File Offset: 0x0013050C
		public static int PlayerScoreHex()
		{
			if (GameController.GameManager == null)
			{
				return -1;
			}
			PlayerEndGameStats stats = AnalyticsEventData.GetStats();
			if (stats == null)
			{
				return -1;
			}
			return stats.territoryPoints;
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x00132334 File Offset: 0x00130534
		public static int PlayerScoreResourcePoints()
		{
			if (GameController.GameManager == null)
			{
				return -1;
			}
			PlayerEndGameStats stats = AnalyticsEventData.GetStats();
			if (stats == null)
			{
				return -1;
			}
			return stats.resourcePoints;
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x0013235C File Offset: 0x0013055C
		public static int PlayerScoreGlobal()
		{
			if (GameController.GameManager == null)
			{
				return -1;
			}
			PlayerEndGameStats stats = AnalyticsEventData.GetStats();
			if (stats == null)
			{
				return -1;
			}
			return stats.totalPoints;
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x00132384 File Offset: 0x00130584
		public static int PlayerStatPopularity()
		{
			if (GameController.GameManager == null)
			{
				return -1;
			}
			PlayerEndGameStats stats = AnalyticsEventData.GetStats();
			if (stats == null)
			{
				return -1;
			}
			return stats.player.Popularity;
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x00048753 File Offset: 0x00046953
		public static int PlayerStatObjectiveCount()
		{
			if (GameController.GameManager == null)
			{
				return -1;
			}
			return AnalyticsEventData.GetStats().player.GetNumberOfStars();
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x001323B0 File Offset: 0x001305B0
		public static int PlayerStatHexCount()
		{
			if (GameController.GameManager == null)
			{
				return -1;
			}
			PlayerEndGameStats stats = AnalyticsEventData.GetStats();
			if (stats == null)
			{
				return -1;
			}
			return stats.territories;
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x001323D8 File Offset: 0x001305D8
		public static int PlayerStatResourceCount()
		{
			if (GameController.GameManager == null)
			{
				return -1;
			}
			PlayerEndGameStats stats = AnalyticsEventData.GetStats();
			if (stats == null)
			{
				return -1;
			}
			return stats.resources;
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x00132400 File Offset: 0x00130600
		public static int PlayerStatBattleWonCount()
		{
			if (GameController.GameManager == null)
			{
				return -1;
			}
			PlayerEndGameStats stats = AnalyticsEventData.GetStats();
			if (stats == null)
			{
				return -1;
			}
			return stats.player.CombatWon;
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x0013242C File Offset: 0x0013062C
		public static int PlayerStatBattleLostCount()
		{
			if (GameController.GameManager == null)
			{
				return -1;
			}
			PlayerEndGameStats stats = AnalyticsEventData.GetStats();
			if (stats == null)
			{
				return -1;
			}
			return stats.player.CombatLost;
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x00132458 File Offset: 0x00130658
		private static PlayerEndGameStats GetStats()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				return AnalyticsEventData.endgameStats.Find((PlayerEndGameStats entry) => entry.player == GameController.GameManager.PlayerOwner);
			}
			return AnalyticsEventData.endgameStats.Find((PlayerEndGameStats entry) => entry.player == GameController.GameManager.GetPlayersWithoutAI().ToList<Player>()[0]);
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x001324C4 File Offset: 0x001306C4
		public static string PlayerStarsDetail()
		{
			if (GameController.GameManager == null)
			{
				return ",,,,,,,,,,,,,,";
			}
			Player player = (GameController.GameManager.IsMultiplayer ? GameController.GameManager.PlayerOwner : GameController.GameManager.GetPlayersWithoutAI().ToList<Player>()[0]);
			return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", new object[]
			{
				(player.stars[StarType.Upgrades] > 0) ? "6upg" : "",
				(player.stars[StarType.Mechs] > 0) ? "4mech" : "",
				(player.stars[StarType.Structures] > 0) ? "4struct" : "",
				(player.stars[StarType.Recruits] > 0) ? "4recr" : "",
				(player.stars[StarType.Workers] > 0) ? "8work" : "",
				(player.stars[StarType.Objective] > 0) ? "1obj" : "",
				(player.stars[StarType.Objective] > 1) ? "2obj" : "",
				(player.stars[StarType.Combat] > 0) ? "1wincomb" : "",
				(player.stars[StarType.Combat] > 1) ? "2wincomb" : "",
				(player.stars[StarType.Combat] > 2) ? "3wincomb" : "",
				(player.stars[StarType.Combat] > 3) ? "4wincomb" : "",
				(player.stars[StarType.Combat] > 4) ? "5wincomb" : "",
				(player.stars[StarType.Combat] > 5) ? "6wincomb" : "",
				(player.stars[StarType.Popularity] > 0) ? "18pop" : "",
				(player.stars[StarType.Power] > 0) ? "16pow" : ""
			});
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x001326E8 File Offset: 0x001308E8
		public static int PlayerRank()
		{
			int num;
			if (GameController.GameManager.IsMultiplayer)
			{
				num = AnalyticsEventData.endgameStats.FindIndex((PlayerEndGameStats entry) => entry.player.matFaction.faction == GameController.GameManager.PlayerOwner.matFaction.faction);
			}
			else
			{
				num = AnalyticsEventData.endgameStats.FindIndex((PlayerEndGameStats entry) => entry.player.IsHuman);
			}
			return num + 1;
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x0004876D File Offset: 0x0004696D
		public static int UndoCount()
		{
			return AnalyticsEventData.undoCounter;
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x0600337A RID: 13178 RVA: 0x00048774 File Offset: 0x00046974
		// (set) Token: 0x0600337B RID: 13179 RVA: 0x0004877B File Offset: 0x0004697B
		public static bool IsNotificationSystemReady { get; protected set; }

		// Token: 0x0600337C RID: 13180 RVA: 0x00048783 File Offset: 0x00046983
		public static void NotificationSystemReady()
		{
			AnalyticsEventData.IsNotificationSystemReady = true;
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x0004878B File Offset: 0x0004698B
		public static void TabEnabled(bool enabled, Screens tab)
		{
			if (enabled)
			{
				AnalyticsEventData.currentTabVisibleStart = DateTime.Now;
				AnalyticsEventData.currentTab = tab;
				return;
			}
			AnalyticsEventData.currentTab = Screens.none;
			AnalyticsEventData.tabTime = 0f;
			AnalyticsEventData.currentTabVisibleStart = AnalyticsEventData.EPOCH;
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x000487BB File Offset: 0x000469BB
		public static Screens GetCurrentTab()
		{
			return AnalyticsEventData.currentTab;
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x000487C2 File Offset: 0x000469C2
		public static void OverrideCurrentScreen(Screens screen)
		{
			AnalyticsEventData.overridedScreen = AnalyticsEventData.currentScreen;
			AnalyticsEventData.currentScreen = screen;
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x000487D4 File Offset: 0x000469D4
		public static void RestoreCurrentScreen()
		{
			if (AnalyticsEventData.currentScreen >= Screens.tab_enlist && AnalyticsEventData.currentScreen <= Screens.tab_faction && AnalyticsEventData.overridedScreen >= Screens.tab_enlist && AnalyticsEventData.overridedScreen <= Screens.tab_faction)
			{
				AnalyticsEventData.currentScreen = Screens.in_game;
			}
			else
			{
				AnalyticsEventData.currentScreen = AnalyticsEventData.overridedScreen;
			}
			AnalyticsEventData.overridedScreen = Screens.none;
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x00048813 File Offset: 0x00046A13
		public static void ScreenDisplayed(Screens screen, Contexts context)
		{
			AnalyticsEventData.screenCount++;
			AnalyticsEventData.current_context = context;
			AnalyticsEventData.ScreenChanged(screen);
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x0013275C File Offset: 0x0013095C
		private static void ScreenChanged(Screens screen)
		{
			if (AnalyticsEventData.currentScreen != Screens.none)
			{
				AnalyticsEventData.previousScreen = AnalyticsEventData.currentScreen;
				AnalyticsEventData.previousScreenTime = AnalyticsEventData.currentScreenTime + (float)(DateTime.Now - AnalyticsEventData.currentScreenVisibleStart).TotalMilliseconds / 1000f;
				AnalyticsEventData.currentScreenTime = 0f;
			}
			AnalyticsEventData.currentScreen = screen;
			AnalyticsEventData.currentScreenVisibleStart = DateTime.Now;
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x001327C0 File Offset: 0x001309C0
		public static void ApplicationFocusChanged(bool hasFocus, bool newSessionId = false)
		{
			if (hasFocus)
			{
				AnalyticsEventData.currentScreenVisibleStart = DateTime.Now;
				if (AnalyticsEventData.currentTabVisibleStart != AnalyticsEventData.EPOCH)
				{
					AnalyticsEventData.currentTabVisibleStart = DateTime.Now;
				}
				if (newSessionId)
				{
					AnalyticsEventData.SessionTimeStart();
					if (SceneManager.GetActiveScene().name.Equals(SceneController.SCENE_MAIN_NAME) && !GameController.GameManager.GameFinished)
					{
						AnalyticsEventData.SessionGamePlayTimeStart();
						return;
					}
				}
			}
			else
			{
				AnalyticsEventData.SessionStop();
				AnalyticsEventData.currentScreenTime += (float)(DateTime.Now - AnalyticsEventData.currentScreenVisibleStart).TotalMilliseconds / 1000f;
				if (AnalyticsEventData.currentTabVisibleStart != AnalyticsEventData.EPOCH)
				{
					AnalyticsEventData.tabTime += (float)(DateTime.Now - AnalyticsEventData.currentTabVisibleStart).TotalMilliseconds / 1000f;
				}
			}
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x0004882D File Offset: 0x00046A2D
		public static void NavigationButtonClicked(ScreenPreviousNavActions action)
		{
			AnalyticsEventData.lastNavigationAction = action;
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x00048835 File Offset: 0x00046A35
		public static string ScreenPrevious()
		{
			if (AnalyticsEventData.previousScreen != Screens.none)
			{
				return AnalyticsEventData.ScreenName(AnalyticsEventData.previousScreen);
			}
			return "";
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x0004884E File Offset: 0x00046A4E
		public static string ScreenCurrent()
		{
			return AnalyticsEventData.ScreenName(AnalyticsEventData.currentScreen);
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x0004885A File Offset: 0x00046A5A
		public static string ScreenName(Screens screen)
		{
			return Enum.GetName(typeof(Screens), screen);
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x00048871 File Offset: 0x00046A71
		public static int ScreenCount()
		{
			return AnalyticsEventData.screenCount;
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x00048878 File Offset: 0x00046A78
		public static string Context()
		{
			return Enum.GetName(typeof(Contexts), AnalyticsEventData.current_context);
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x00048893 File Offset: 0x00046A93
		public static string ScreenPreviousNavAction()
		{
			return Enum.GetName(typeof(ScreenPreviousNavActions), AnalyticsEventData.lastNavigationAction);
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x000488AE File Offset: 0x00046AAE
		public static float TimeScreenPreviousSec()
		{
			return (float)Math.Round((double)AnalyticsEventData.previousScreenTime, 3);
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x00132894 File Offset: 0x00130A94
		public static float TimeTabPreviousSec()
		{
			return AnalyticsEventData.tabTime + (float)(DateTime.Now - AnalyticsEventData.currentTabVisibleStart).TotalMilliseconds / 1000f;
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x001328C8 File Offset: 0x00130AC8
		public static float TimeScreenPreviousSecForTabs()
		{
			return AnalyticsEventData.currentScreenTime + (float)(DateTime.Now - AnalyticsEventData.currentScreenVisibleStart).TotalMilliseconds / 1000f;
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x000488BD File Offset: 0x00046ABD
		public static bool IsCurrentTutorialSessionFinished()
		{
			return AnalyticsEventData.playerFinishedCurrentTutorialSession;
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x000488C4 File Offset: 0x00046AC4
		public static void TutorialStart(StepIDs stepId)
		{
			AnalyticsEventData.playerFinishedCurrentTutorialSession = false;
			AnalyticsEventData.currentStepSequence = 0;
			AnalyticsEventData.currentStep = stepId;
			AnalyticsEventData.TutorialStepStarted();
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x000488DD File Offset: 0x00046ADD
		public static void UpdateTutorialStep()
		{
			AnalyticsEventData.currentStepSequence++;
			if (AnalyticsEventData.currentStep != (StepIDs)Enum.GetValues(typeof(StepIDs)).Length)
			{
				AnalyticsEventData.currentStep++;
			}
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x00048912 File Offset: 0x00046B12
		public static void RevertTutorialStepBy(int steps)
		{
			AnalyticsEventData.currentStepSequence -= steps;
			AnalyticsEventData.currentStep -= steps;
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x0004892C File Offset: 0x00046B2C
		public static void TutorialStepStarted()
		{
			AnalyticsEventData.tutorialStartTimestamp = DateTime.Now;
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x001328FC File Offset: 0x00130AFC
		public static void TutorialStepStoped()
		{
			AnalyticsEventData.currentTime += (float)(DateTime.Now - AnalyticsEventData.tutorialStartTimestamp).TotalSeconds;
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x00048938 File Offset: 0x00046B38
		public static void ResetTutorialStepTimer()
		{
			AnalyticsEventData.tutorialStartTimestamp = DateTime.MinValue;
			AnalyticsEventData.currentTime = 0f;
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x0004894E File Offset: 0x00046B4E
		public static void SetCurrentStep(StepIDs newCurrentStepID)
		{
			AnalyticsEventData.currentStep = newCurrentStepID;
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x00048956 File Offset: 0x00046B56
		public static StepIDs CurrentTutorialStepId()
		{
			return AnalyticsEventData.currentStep;
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x0013292C File Offset: 0x00130B2C
		public static void TutorialFinished()
		{
			AnalyticsEventData.playerFinishedCurrentTutorialSession = true;
			int missionId = GameController.GameManager.missionId;
			if (missionId > Enum.GetValues(typeof(StepIDs)).Length || missionId < 0)
			{
				return;
			}
			int num = PlayerPrefs.GetInt("TutorialsFinishedVector", 0);
			num |= 1 << missionId;
			PlayerPrefs.SetInt("TutorialsFinishedVector", num);
			PlayerPrefs.Save();
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x0004895D File Offset: 0x00046B5D
		public static string StepID()
		{
			return Enum.GetName(typeof(StepIDs), AnalyticsEventData.currentStep);
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x00048978 File Offset: 0x00046B78
		public static float StepSequenceNumber()
		{
			return (float)AnalyticsEventData.currentStepSequence;
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x00048980 File Offset: 0x00046B80
		public static string StepStatus(StepStatuses status)
		{
			return Enum.GetName(typeof(StepStatuses), status);
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x0013298C File Offset: 0x00130B8C
		public static float TimeOnStep()
		{
			return AnalyticsEventData.currentTime + ((AnalyticsEventData.tutorialStartTimestamp != DateTime.MinValue) ? ((float)(DateTime.Now - AnalyticsEventData.tutorialStartTimestamp).TotalSeconds) : 0f);
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x001329D0 File Offset: 0x00130BD0
		public static bool IsTutoComplete()
		{
			int @int = PlayerPrefs.GetInt("TutorialsFinishedVector", 0);
			int num = 2047;
			return @int == num;
		}

		// Token: 0x040021F7 RID: 8695
		private static string crossPromoSessionID = "";

		// Token: 0x040021F8 RID: 8696
		private static float crossPromoOpened = 0f;

		// Token: 0x040021F9 RID: 8697
		private static string crossPromoPreviousScreen = "";

		// Token: 0x040021FA RID: 8698
		private const string DEVICE_ID_KEY = "DeviceId";

		// Token: 0x040021FB RID: 8699
		private const string EVENT_ID_KEY = "EventId";

		// Token: 0x040021FC RID: 8700
		private const string TIME_SESSION_START_KEY = "TimeSessionStart";

		// Token: 0x040021FD RID: 8701
		private const string TIME_SESSION_GAMEPLAY_START_KEY = "TimeSessionGameplayStart";

		// Token: 0x040021FE RID: 8702
		private const string TIME_SESSION_GAMEPLAY_KEY = "TimeSessionGameplay";

		// Token: 0x040021FF RID: 8703
		private const string TIME_LTD_KEY = "TimeLTD";

		// Token: 0x04002200 RID: 8704
		private const string TIME_LTD_GAMEPLAY_KEY = "TimeLTDGameplay";

		// Token: 0x04002201 RID: 8705
		private static Environments environmentType = Environments.dev;

		// Token: 0x04002202 RID: 8706
		private static long timeSessionTimestamp = 0L;

		// Token: 0x04002203 RID: 8707
		private static long timeSessionGameplayTimestamp = 0L;

		// Token: 0x04002204 RID: 8708
		private static long timeGameplaySeconds = 0L;

		// Token: 0x04002205 RID: 8709
		private static bool appStopped = false;

		// Token: 0x04002206 RID: 8710
		private static DateTime timeStopDate = DateTime.UtcNow;

		// Token: 0x04002207 RID: 8711
		private static long sessionStart = 0L;

		// Token: 0x04002208 RID: 8712
		private static string appBootSessionID = "";

		// Token: 0x04002209 RID: 8713
		private static string ip = "";

		// Token: 0x0400220A RID: 8714
		private static int undoCounter = 0;

		// Token: 0x0400220B RID: 8715
		private static List<PlayerEndGameStats> endgameStats;

		// Token: 0x0400220C RID: 8716
		private static string matchSessionID = "";

		// Token: 0x0400220D RID: 8717
		private static MatchLaunchMethods matchLaunchMethod = MatchLaunchMethods.main;

		// Token: 0x0400220E RID: 8718
		private static bool saveTestLoading = false;

		// Token: 0x04002210 RID: 8720
		private static int screenCount = 0;

		// Token: 0x04002211 RID: 8721
		private static Screens previousScreen = Screens.none;

		// Token: 0x04002212 RID: 8722
		private static Screens overridedScreen = Screens.none;

		// Token: 0x04002213 RID: 8723
		private static Screens currentScreen = Screens.none;

		// Token: 0x04002214 RID: 8724
		private static float previousScreenTime = -1f;

		// Token: 0x04002215 RID: 8725
		private static float currentScreenTime = 0f;

		// Token: 0x04002216 RID: 8726
		private static DateTime currentScreenVisibleStart;

		// Token: 0x04002217 RID: 8727
		private static Screens currentTab = Screens.none;

		// Token: 0x04002218 RID: 8728
		private static float tabTime = 0f;

		// Token: 0x04002219 RID: 8729
		private static DateTime currentTabVisibleStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x0400221A RID: 8730
		private static DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x0400221B RID: 8731
		private static ScreenPreviousNavActions lastNavigationAction = ScreenPreviousNavActions.none;

		// Token: 0x0400221C RID: 8732
		private static Contexts current_context;

		// Token: 0x0400221D RID: 8733
		private const string TUTORIALS_FINISHED_VECTOR = "TutorialsFinishedVector";

		// Token: 0x0400221E RID: 8734
		private static DateTime tutorialStartTimestamp = DateTime.Now;

		// Token: 0x0400221F RID: 8735
		private static float currentTime = 0f;

		// Token: 0x04002220 RID: 8736
		private static StepIDs currentStep = StepIDs.none;

		// Token: 0x04002221 RID: 8737
		private static int currentStepSequence = 0;

		// Token: 0x04002222 RID: 8738
		private static bool playerFinishedCurrentTutorialSession = false;
	}
}

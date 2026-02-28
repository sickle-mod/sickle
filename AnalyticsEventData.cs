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
	// Token: 0x0200064E RID: 1614
	public class AnalyticsEventData
	{
		// Token: 0x0600334D RID: 13133 RVA: 0x001397C0 File Offset: 0x001379C0
		public static string AchievementID(Achievements achievement)
		{
			int num = (int)achievement;
			return num.ToString();
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x00048348 File Offset: 0x00046548
		public static string LaunchMethod(LaunchMethods method)
		{
			return Enum.GetName(typeof(LaunchMethods), method);
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x0004835F File Offset: 0x0004655F
		public static string ContentID(Achievements achievement)
		{
			return AnalyticsEventData.AchievementID(achievement);
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x00031737 File Offset: 0x0002F937
		public static string ContentType()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x00048367 File Offset: 0x00046567
		public static string UnlockReason(UnlockReasons unlockReason)
		{
			return unlockReason.ToString();
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x00048376 File Offset: 0x00046576
		public static string IsEmailOptIn(bool option)
		{
			return option.ToString();
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x0004837F File Offset: 0x0004657F
		public static string SignupPath(SingupPaths path)
		{
			return Enum.GetName(typeof(SingupPaths), path);
		}

		// Token: 0x06003354 RID: 13140 RVA: 0x00048396 File Offset: 0x00046596
		public static void SetCrossPromoPreviousScreen(string previousScreen)
		{
			AnalyticsEventData.crossPromoPreviousScreen = previousScreen;
		}

		// Token: 0x06003355 RID: 13141 RVA: 0x001397D8 File Offset: 0x001379D8
		public static void CreateCrossPromoSessionID()
		{
			AnalyticsEventData.crossPromoSessionID = Guid.NewGuid().ToString();
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x0004839E File Offset: 0x0004659E
		public static void CrossPromoWindowOpenedTimestamp()
		{
			AnalyticsEventData.crossPromoOpened = Time.time;
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x000483AA File Offset: 0x000465AA
		public static string CrossPromoSessionID()
		{
			return AnalyticsEventData.crossPromoSessionID;
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x000483B1 File Offset: 0x000465B1
		public static string ApiVersion()
		{
			return AnalyticsEventData.UnitySDKVersion();
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x000483B8 File Offset: 0x000465B8
		public static string IsAutomatic(bool isAutomatic)
		{
			if (!isAutomatic)
			{
				return "False";
			}
			return "True";
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x000483C8 File Offset: 0x000465C8
		public static string TryCastString(object value)
		{
			if (value != null)
			{
				return value.ToString();
			}
			return string.Empty;
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x000483D9 File Offset: 0x000465D9
		public static int TryCastInt(object value)
		{
			if (value != null)
			{
				return (int)value;
			}
			return -1;
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x000483E6 File Offset: 0x000465E6
		public static int CrossPromoTimeActiveSec()
		{
			return (int)(Time.time - AnalyticsEventData.crossPromoOpened);
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x000483F4 File Offset: 0x000465F4
		public static string CrossPromoPreviousScreen()
		{
			return AnalyticsEventData.crossPromoPreviousScreen;
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x000483B8 File Offset: 0x000465B8
		public static string BoolToString(bool boolean)
		{
			if (!boolean)
			{
				return "False";
			}
			return "True";
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x000483FB File Offset: 0x000465FB
		public static string Action(ActionsOnFriend action)
		{
			return Enum.GetName(typeof(ActionsOnFriend), action);
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x00048412 File Offset: 0x00046612
		public static void IncreaseEventID()
		{
			PlayerPrefs.SetInt("EventId", PlayerPrefs.GetInt("EventId", 1) + 1);
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x00139800 File Offset: 0x00137A00
		public static void CreateAppBootSessionID()
		{
			AnalyticsEventData.appBootSessionID = Guid.NewGuid().ToString();
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x0004842B File Offset: 0x0004662B
		public static void CleanSessionTimes()
		{
			AnalyticsEventData.appStopped = false;
			AnalyticsEventData.timeGameplaySeconds = 0L;
			PlayerPrefs.SetString("TimeSessionStart", "");
			PlayerPrefs.SetString("TimeSessionGameplayStart", "");
			PlayerPrefs.SetString("TimeSessionGameplay", "");
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x00048467 File Offset: 0x00046667
		public static void SessionStop()
		{
			AnalyticsEventData.appStopped = true;
			AnalyticsEventData.timeStopDate = DateTime.UtcNow;
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x00139828 File Offset: 0x00137A28
		public static void SessionTimeStart()
		{
			AnalyticsEventData.UpdateSessionStart();
			PlayerPrefs.SetString("TimeSessionStart", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x00139858 File Offset: 0x00137A58
		public static void UpdateSessionStart()
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			AnalyticsEventData.sessionStart = (long)(DateTime.UtcNow - dateTime).TotalMilliseconds;
			AnalyticsEventData.timeSessionTimestamp = AnalyticsEventData.TimeSession();
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x00048479 File Offset: 0x00046679
		public static void SessionTimeEnd(bool inGameplay = false)
		{
			AnalyticsEventData.UpdateLTDTime();
			if (inGameplay)
			{
				AnalyticsEventData.UpdateLTDGameplayTime();
			}
			AnalyticsEventData.CleanSessionTimes();
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x0013989C File Offset: 0x00137A9C
		public static void SessionGamePlayTimeStart()
		{
			AnalyticsEventData.timeSessionGameplayTimestamp = AnalyticsEventData.TimeSessionGameplay();
			AnalyticsEventData.UpdateGamePlayTime();
			PlayerPrefs.SetString("TimeSessionGameplayStart", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x0004848D File Offset: 0x0004668D
		public static void SessionGameplayTimeEnd()
		{
			AnalyticsEventData.UpdateSessionGameplayTime();
			PlayerPrefs.SetString("TimeSessionGameplayStart", "");
			AnalyticsEventData.timeGameplaySeconds = 0L;
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x001398D4 File Offset: 0x00137AD4
		public static void UpdateSessionGameplayTime()
		{
			PlayerPrefs.SetString("TimeSessionGameplay", AnalyticsEventData.TimeSessionGameplay().ToString());
			AnalyticsEventData.UpdateLTDGameplayTime();
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x00139900 File Offset: 0x00137B00
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

		// Token: 0x0600336B RID: 13163 RVA: 0x000484AA File Offset: 0x000466AA
		public static void UpdateLTDTime()
		{
			AnalyticsEventData.TimeLTD();
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x000484B2 File Offset: 0x000466B2
		public static void UpdateLTDGameplayTime()
		{
			AnalyticsEventData.TimeLTDGameplay();
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x000484BA File Offset: 0x000466BA
		public static void UpdateIP(string ip)
		{
			AnalyticsEventData.ip = ip.TrimEnd();
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x000484C7 File Offset: 0x000466C7
		public static void UpdateEnvironmentType()
		{
			RuntimePlatform platform = Application.platform;
			AnalyticsEventData.SetEnvironnement(Environments.prod);
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x000484D5 File Offset: 0x000466D5
		private static void SetEnvironnement(Environments enviro)
		{
			AnalyticsEventData.environmentType = enviro;
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x00139980 File Offset: 0x00137B80
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

		// Token: 0x06003371 RID: 13169 RVA: 0x001399DC File Offset: 0x00137BDC
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

		// Token: 0x06003372 RID: 13170 RVA: 0x00139A34 File Offset: 0x00137C34
		public static string ClientEventTime()
		{
			return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffff");
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x00139A34 File Offset: 0x00137C34
		public static string ClientUploadTime()
		{
			return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffff");
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x000484DD File Offset: 0x000466DD
		public static int EventID()
		{
			int @int = PlayerPrefs.GetInt("EventId", 0);
			AnalyticsEventData.IncreaseEventID();
			return @int;
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x00139A54 File Offset: 0x00137C54
		public static string InsertID()
		{
			return Guid.NewGuid().ToString();
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x000484EF File Offset: 0x000466EF
		public static long SessionID()
		{
			return AnalyticsEventData.sessionStart;
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x000484F6 File Offset: 0x000466F6
		public static string EventType(AnalyticsEventTypes type)
		{
			return Enum.GetName(typeof(AnalyticsEventTypes), type);
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x0004850D File Offset: 0x0004670D
		public static string VersionName()
		{
			return BuildVersionUtility.GetBuildVersion();
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x00048514 File Offset: 0x00046714
		public static string OSName()
		{
			return Enum.GetName(typeof(OperatingSystemFamily), SystemInfo.operatingSystemFamily);
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x0002E8B6 File Offset: 0x0002CAB6
		public static string OSVersion()
		{
			return SystemInfo.operatingSystem;
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x0002F611 File Offset: 0x0002D811
		public static string DeviceBrand()
		{
			return "";
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x0002F611 File Offset: 0x0002D811
		public static string DeviceManufacturer()
		{
			return "";
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x0004852F File Offset: 0x0004672F
		public static string DeviceFamily()
		{
			if (PlatformManager.IsSteam)
			{
				return "PC";
			}
			return "";
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x00048543 File Offset: 0x00046743
		public static string DeviceType()
		{
			return Enum.GetName(typeof(DeviceType), SystemInfo.deviceType);
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x0002F611 File Offset: 0x0002D811
		public static string DeviceCarrier()
		{
			return "";
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x0004855E File Offset: 0x0004675E
		public static string IPAddress()
		{
			return AnalyticsEventData.ip;
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x00139A74 File Offset: 0x00137C74
		public static string Language()
		{
			return Application.systemLanguage.ToString();
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x00048565 File Offset: 0x00046765
		public static string Platform()
		{
			return Enum.GetName(typeof(RuntimePlatform), Application.platform);
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x00048580 File Offset: 0x00046780
		public static string Library()
		{
			if (PlatformManager.IsMobile && !Application.isEditor)
			{
				return "amplitude-unity-2.10.0";
			}
			return "http/1.0";
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x0004859B File Offset: 0x0004679B
		public static string AppBootSessionID()
		{
			return AnalyticsEventData.appBootSessionID;
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x00139A94 File Offset: 0x00137C94
		public static Guid BackendUserID()
		{
			Guid guid = Guid.Empty;
			if (PlayerInfo.me != null && PlayerInfo.me.PlayerStats != null)
			{
				guid = PlayerInfo.me.PlayerStats.Id;
			}
			return guid;
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x000485A2 File Offset: 0x000467A2
		public static string BackendPlatform()
		{
			return "Microsoft Azure";
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x000485A9 File Offset: 0x000467A9
		public static string UAPlatform()
		{
			return "Asmodee Digital";
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x000485B0 File Offset: 0x000467B0
		public static string UAUserID()
		{
			return AnalyticsEventData.UserID();
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x0002F611 File Offset: 0x0002D811
		public static string UAChannel()
		{
			return "";
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x000485B7 File Offset: 0x000467B7
		public static string PushPlatform()
		{
			if (PlatformManager.IsMobile)
			{
				return "Firebase";
			}
			return "";
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x000485CB File Offset: 0x000467CB
		public static string PushUserID()
		{
			if (!Application.isEditor && PlatformManager.IsMobile && AnalyticsEventData.IsNotificationSystemReady && FirebaseApp.DefaultInstance != null)
			{
				return FirebaseApp.DefaultInstance.Options.MessageSenderId;
			}
			return "";
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x00139ACC File Offset: 0x00137CCC
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

		// Token: 0x0600338D RID: 13197 RVA: 0x00139B40 File Offset: 0x00137D40
		public static int TimezoneClient()
		{
			return DateTimeOffset.Now.Offset.Hours * 60 + DateTimeOffset.Now.Offset.Minutes;
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x00139B7C File Offset: 0x00137D7C
		public static string ClientLocalTime()
		{
			return DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffff");
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x00139B9C File Offset: 0x00137D9C
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

		// Token: 0x06003390 RID: 13200 RVA: 0x000485FE File Offset: 0x000467FE
		public static string Environment()
		{
			return Enum.GetName(typeof(Environments), AnalyticsEventData.environmentType);
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x00139C1C File Offset: 0x00137E1C
		public static long TimeSession()
		{
			DateTime dateTime;
			if (!DateTime.TryParse(PlayerPrefs.GetString("TimeSessionStart", ""), out dateTime))
			{
				return 0L;
			}
			return (long)(DateTime.Now - dateTime).TotalSeconds;
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x00139C58 File Offset: 0x00137E58
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

		// Token: 0x06003393 RID: 13203 RVA: 0x00139CC0 File Offset: 0x00137EC0
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

		// Token: 0x06003394 RID: 13204 RVA: 0x00139D0C File Offset: 0x00137F0C
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

		// Token: 0x06003395 RID: 13205 RVA: 0x00139D58 File Offset: 0x00137F58
		public static string ScreenResolution()
		{
			return string.Format("{0}*{1}", Screen.currentResolution.width, Screen.currentResolution.height);
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x00139D94 File Offset: 0x00137F94
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

		// Token: 0x06003397 RID: 13207 RVA: 0x00048619 File Offset: 0x00046819
		public static string UnitySDKVersion()
		{
			return SDKVersionManager.Version();
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x0002F611 File Offset: 0x0002D811
		public static string ABTestGroup()
		{
			return "";
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x00048620 File Offset: 0x00046820
		public static int Karma()
		{
			if (PlayerInfo.me == null)
			{
				return -1;
			}
			return PlayerInfo.me.PlayerStats.Karma;
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x00139DFC File Offset: 0x00137FFC
		public static string DLCOwned()
		{
			string text = "";
			if (GameServiceController.Instance.InvadersFromAfarUnlocked())
			{
				text += "invaders from afar";
			}
			return text;
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x0004863A File Offset: 0x0004683A
		public static int ELORating()
		{
			if (PlayerInfo.me == null)
			{
				return -1;
			}
			return PlayerInfo.me.PlayerStats.ELO;
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x00048654 File Offset: 0x00046854
		public static void IncreaseUndoCounter()
		{
			AnalyticsEventData.undoCounter++;
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x00048662 File Offset: 0x00046862
		public static void ResetUndoCounter()
		{
			AnalyticsEventData.undoCounter = 0;
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x0004866A File Offset: 0x0004686A
		public static void SetMatchLaunchMethod(MatchLaunchMethods matchLaunchMethod)
		{
			AnalyticsEventData.matchLaunchMethod = matchLaunchMethod;
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x00048672 File Offset: 0x00046872
		public static void UpdateEndgameStats()
		{
			AnalyticsEventData.endgameStats = GameController.GameManager.CalculateStats();
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x00048683 File Offset: 0x00046883
		public static void UpdateMatchSessionID(string loadedMatchID)
		{
			if (AnalyticsEventData.saveTestLoading)
			{
				return;
			}
			AnalyticsEventData.matchSessionID = loadedMatchID;
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x00139E28 File Offset: 0x00138028
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

		// Token: 0x060033A2 RID: 13218 RVA: 0x00048693 File Offset: 0x00046893
		public static void ResetMatchSessionID()
		{
			AnalyticsEventData.matchSessionID = "";
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x0004869F File Offset: 0x0004689F
		public static void SetMatchSessionIDUpdateLockState(bool enable)
		{
			AnalyticsEventData.saveTestLoading = enable;
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x000486A7 File Offset: 0x000468A7
		public static string MatchSessionID()
		{
			if (AnalyticsEventData.matchSessionID != null && AnalyticsEventData.matchSessionID.Length != 0)
			{
				return AnalyticsEventData.matchSessionID;
			}
			AnalyticsEventData.CreateMatchSessionID();
			return AnalyticsEventData.matchSessionID;
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x000486CC File Offset: 0x000468CC
		public static int PlayerCountHuman()
		{
			return GameController.GameManager.GetPlayersWithoutAICount();
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x000486D8 File Offset: 0x000468D8
		public static int PlayerCountAI()
		{
			return GameController.GameManager.GetAIPlayersCount();
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x00139EE0 File Offset: 0x001380E0
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

		// Token: 0x060033A8 RID: 13224 RVA: 0x000486E4 File Offset: 0x000468E4
		public static string MatchLaunchMethod()
		{
			return Enum.GetName(typeof(MatchLaunchMethods), AnalyticsEventData.matchLaunchMethod);
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x000486FF File Offset: 0x000468FF
		public static bool IsOnline()
		{
			return GameController.GameManager.IsMultiplayer;
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x00139F40 File Offset: 0x00138140
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

		// Token: 0x060033AB RID: 13227 RVA: 0x0004870B File Offset: 0x0004690B
		public static bool IsRanked()
		{
			return GameController.GameManager.IsRanked;
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x000486FF File Offset: 0x000468FF
		public static bool IsObservable()
		{
			return GameController.GameManager.IsMultiplayer;
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x0013A04C File Offset: 0x0013824C
		public static string ObsAccess()
		{
			if (!GameController.GameManager.IsPrivate)
			{
				return ObserverAccess.everyone.ToString();
			}
			return ObserverAccess.friends_only.ToString();
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public static bool ObsShowHiddenInfo()
		{
			return false;
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x00048717 File Offset: 0x00046917
		public static bool IsObserver()
		{
			return GameController.GameManager.SpectatorMode;
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x0013A084 File Offset: 0x00138284
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

		// Token: 0x060033B1 RID: 13233 RVA: 0x0013A100 File Offset: 0x00138300
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

		// Token: 0x060033B2 RID: 13234 RVA: 0x0013A17C File Offset: 0x0013837C
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

		// Token: 0x060033B3 RID: 13235 RVA: 0x0013A28C File Offset: 0x0013848C
		public static string PlayerMatForFaction(Faction faction)
		{
			Player playerByFaction = GameController.GameManager.GetPlayerByFaction(faction);
			if (playerByFaction == null)
			{
				return "";
			}
			return Enum.GetName(typeof(PlayerMatType), playerByFaction.matPlayer.matType);
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x0013A2D0 File Offset: 0x001384D0
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

		// Token: 0x060033B5 RID: 13237 RVA: 0x00048723 File Offset: 0x00046923
		public static string PromoCardsEnabled()
		{
			if (GameController.GameManager.PromoCardsEnabled())
			{
				return GameController.GameManager.GetUnlockedPromoCards();
			}
			return "";
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x00048741 File Offset: 0x00046941
		public static int PlayerClockDuration()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				return MultiplayerController.Instance.StartingPlayerClock;
			}
			return -1;
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x0004875B File Offset: 0x0004695B
		public static long TimeActiveSec()
		{
			AnalyticsEventData.UpdateGamePlayTime();
			return AnalyticsEventData.timeGameplaySeconds;
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x00048767 File Offset: 0x00046967
		public static string EndReason(EndReasons reason)
		{
			return Enum.GetName(typeof(EndReasons), reason);
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x0013A320 File Offset: 0x00138520
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

		// Token: 0x060033BA RID: 13242 RVA: 0x0004877E File Offset: 0x0004697E
		public static int TurnCount()
		{
			if (GameController.GameManager == null)
			{
				return 0;
			}
			return GameController.GameManager.TurnCount;
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x0013A374 File Offset: 0x00138574
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

		// Token: 0x060033BC RID: 13244 RVA: 0x00048793 File Offset: 0x00046993
		public static int BotSwapCount()
		{
			if (!GameController.GameManager.IsMultiplayer)
			{
				return 0;
			}
			return MultiplayerController.Instance.NumberOfPlayersOverridedByAi();
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x0013A3BC File Offset: 0x001385BC
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

		// Token: 0x060033BE RID: 13246 RVA: 0x0013A3E4 File Offset: 0x001385E4
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

		// Token: 0x060033BF RID: 13247 RVA: 0x0013A40C File Offset: 0x0013860C
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

		// Token: 0x060033C0 RID: 13248 RVA: 0x0013A434 File Offset: 0x00138634
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

		// Token: 0x060033C1 RID: 13249 RVA: 0x0013A45C File Offset: 0x0013865C
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

		// Token: 0x060033C2 RID: 13250 RVA: 0x0013A484 File Offset: 0x00138684
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

		// Token: 0x060033C3 RID: 13251 RVA: 0x0013A4AC File Offset: 0x001386AC
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

		// Token: 0x060033C4 RID: 13252 RVA: 0x000487AD File Offset: 0x000469AD
		public static int PlayerStatObjectiveCount()
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
			return stats.player.GetNumberOfStars();
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x0013A4D8 File Offset: 0x001386D8
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

		// Token: 0x060033C6 RID: 13254 RVA: 0x0013A500 File Offset: 0x00138700
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

		// Token: 0x060033C7 RID: 13255 RVA: 0x0013A528 File Offset: 0x00138728
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

		// Token: 0x060033C8 RID: 13256 RVA: 0x0013A554 File Offset: 0x00138754
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

		// Token: 0x060033C9 RID: 13257 RVA: 0x0013A580 File Offset: 0x00138780
		private static PlayerEndGameStats GetStats()
		{
			Player player = null;
			if (GameController.GameManager.IsMultiplayer)
			{
				player = GameController.GameManager.PlayerOwner;
			}
			else
			{
				List<Player> list = GameController.GameManager.GetPlayersWithoutAI().ToList<Player>();
				if (list.Count > 0) player = list[0];
			}
			if (player == null) return null;
			return AnalyticsEventData.endgameStats.Find((PlayerEndGameStats entry) => entry.player == player);
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x0013A5EC File Offset: 0x001387EC
		public static string PlayerStarsDetail()
		{
			if (GameController.GameManager == null)
			{
				return ",,,,,,,,,,,,,,";
			}
			Player player = null;
			if (GameController.GameManager.IsMultiplayer)
			{
				player = GameController.GameManager.PlayerOwner;
			}
			else
			{
				List<Player> list = GameController.GameManager.GetPlayersWithoutAI().ToList<Player>();
				if (list.Count > 0) player = list[0];
			}
			if (player == null)
			{
				return ",,,,,,,,,,,,,,";
			}
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

		// Token: 0x060033CB RID: 13259 RVA: 0x0013A810 File Offset: 0x00138A10
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

		// Token: 0x060033CC RID: 13260 RVA: 0x000487C7 File Offset: 0x000469C7
		public static int UndoCount()
		{
			return AnalyticsEventData.undoCounter;
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060033CD RID: 13261 RVA: 0x000487CE File Offset: 0x000469CE
		// (set) Token: 0x060033CE RID: 13262 RVA: 0x000487D5 File Offset: 0x000469D5
		public static bool IsNotificationSystemReady { get; protected set; }

		// Token: 0x060033CF RID: 13263 RVA: 0x000487DD File Offset: 0x000469DD
		public static void NotificationSystemReady()
		{
			AnalyticsEventData.IsNotificationSystemReady = true;
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x000487E5 File Offset: 0x000469E5
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

		// Token: 0x060033D1 RID: 13265 RVA: 0x00048815 File Offset: 0x00046A15
		public static Screens GetCurrentTab()
		{
			return AnalyticsEventData.currentTab;
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x0004881C File Offset: 0x00046A1C
		public static void OverrideCurrentScreen(Screens screen)
		{
			AnalyticsEventData.overridedScreen = AnalyticsEventData.currentScreen;
			AnalyticsEventData.currentScreen = screen;
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x0004882E File Offset: 0x00046A2E
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

		// Token: 0x060033D4 RID: 13268 RVA: 0x0004886D File Offset: 0x00046A6D
		public static void ScreenDisplayed(Screens screen, Contexts context)
		{
			AnalyticsEventData.screenCount++;
			AnalyticsEventData.current_context = context;
			AnalyticsEventData.ScreenChanged(screen);
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x0013A884 File Offset: 0x00138A84
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

		// Token: 0x060033D6 RID: 13270 RVA: 0x0013A8E8 File Offset: 0x00138AE8
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

		// Token: 0x060033D7 RID: 13271 RVA: 0x00048887 File Offset: 0x00046A87
		public static void NavigationButtonClicked(ScreenPreviousNavActions action)
		{
			AnalyticsEventData.lastNavigationAction = action;
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x0004888F File Offset: 0x00046A8F
		public static string ScreenPrevious()
		{
			if (AnalyticsEventData.previousScreen != Screens.none)
			{
				return AnalyticsEventData.ScreenName(AnalyticsEventData.previousScreen);
			}
			return "";
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x000488A8 File Offset: 0x00046AA8
		public static string ScreenCurrent()
		{
			return AnalyticsEventData.ScreenName(AnalyticsEventData.currentScreen);
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x000488B4 File Offset: 0x00046AB4
		public static string ScreenName(Screens screen)
		{
			return Enum.GetName(typeof(Screens), screen);
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x000488CB File Offset: 0x00046ACB
		public static int ScreenCount()
		{
			return AnalyticsEventData.screenCount;
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x000488D2 File Offset: 0x00046AD2
		public static string Context()
		{
			return Enum.GetName(typeof(Contexts), AnalyticsEventData.current_context);
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x000488ED File Offset: 0x00046AED
		public static string ScreenPreviousNavAction()
		{
			return Enum.GetName(typeof(ScreenPreviousNavActions), AnalyticsEventData.lastNavigationAction);
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x00048908 File Offset: 0x00046B08
		public static float TimeScreenPreviousSec()
		{
			return (float)Math.Round((double)AnalyticsEventData.previousScreenTime, 3);
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x0013A9BC File Offset: 0x00138BBC
		public static float TimeTabPreviousSec()
		{
			return AnalyticsEventData.tabTime + (float)(DateTime.Now - AnalyticsEventData.currentTabVisibleStart).TotalMilliseconds / 1000f;
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x0013A9F0 File Offset: 0x00138BF0
		public static float TimeScreenPreviousSecForTabs()
		{
			return AnalyticsEventData.currentScreenTime + (float)(DateTime.Now - AnalyticsEventData.currentScreenVisibleStart).TotalMilliseconds / 1000f;
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x00048917 File Offset: 0x00046B17
		public static bool IsCurrentTutorialSessionFinished()
		{
			return AnalyticsEventData.playerFinishedCurrentTutorialSession;
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x0004891E File Offset: 0x00046B1E
		public static void TutorialStart(StepIDs stepId)
		{
			AnalyticsEventData.playerFinishedCurrentTutorialSession = false;
			AnalyticsEventData.currentStepSequence = 0;
			AnalyticsEventData.currentStep = stepId;
			AnalyticsEventData.TutorialStepStarted();
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x00048937 File Offset: 0x00046B37
		public static void UpdateTutorialStep()
		{
			AnalyticsEventData.currentStepSequence++;
			if (AnalyticsEventData.currentStep != (StepIDs)Enum.GetValues(typeof(StepIDs)).Length)
			{
				AnalyticsEventData.currentStep++;
			}
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x0004896C File Offset: 0x00046B6C
		public static void RevertTutorialStepBy(int steps)
		{
			AnalyticsEventData.currentStepSequence -= steps;
			AnalyticsEventData.currentStep -= steps;
		}

		// Token: 0x060033E5 RID: 13285 RVA: 0x00048986 File Offset: 0x00046B86
		public static void TutorialStepStarted()
		{
			AnalyticsEventData.tutorialStartTimestamp = DateTime.Now;
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x0013AA24 File Offset: 0x00138C24
		public static void TutorialStepStoped()
		{
			AnalyticsEventData.currentTime += (float)(DateTime.Now - AnalyticsEventData.tutorialStartTimestamp).TotalSeconds;
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x00048992 File Offset: 0x00046B92
		public static void ResetTutorialStepTimer()
		{
			AnalyticsEventData.tutorialStartTimestamp = DateTime.MinValue;
			AnalyticsEventData.currentTime = 0f;
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x000489A8 File Offset: 0x00046BA8
		public static void SetCurrentStep(StepIDs newCurrentStepID)
		{
			AnalyticsEventData.currentStep = newCurrentStepID;
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x000489B0 File Offset: 0x00046BB0
		public static StepIDs CurrentTutorialStepId()
		{
			return AnalyticsEventData.currentStep;
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x0013AA54 File Offset: 0x00138C54
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

		// Token: 0x060033EB RID: 13291 RVA: 0x000489B7 File Offset: 0x00046BB7
		public static string StepID()
		{
			return Enum.GetName(typeof(StepIDs), AnalyticsEventData.currentStep);
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x000489D2 File Offset: 0x00046BD2
		public static float StepSequenceNumber()
		{
			return (float)AnalyticsEventData.currentStepSequence;
		}

		// Token: 0x060033ED RID: 13293 RVA: 0x000489DA File Offset: 0x00046BDA
		public static string StepStatus(StepStatuses status)
		{
			return Enum.GetName(typeof(StepStatuses), status);
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x0013AAB4 File Offset: 0x00138CB4
		public static float TimeOnStep()
		{
			return AnalyticsEventData.currentTime + ((AnalyticsEventData.tutorialStartTimestamp != DateTime.MinValue) ? ((float)(DateTime.Now - AnalyticsEventData.tutorialStartTimestamp).TotalSeconds) : 0f);
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x0013AAF8 File Offset: 0x00138CF8
		public static bool IsTutoComplete()
		{
			int @int = PlayerPrefs.GetInt("TutorialsFinishedVector", 0);
			int num = 2047;
			return @int == num;
		}

		// Token: 0x04002219 RID: 8729
		private static string crossPromoSessionID = "";

		// Token: 0x0400221A RID: 8730
		private static float crossPromoOpened = 0f;

		// Token: 0x0400221B RID: 8731
		private static string crossPromoPreviousScreen = "";

		// Token: 0x0400221C RID: 8732
		private const string DEVICE_ID_KEY = "DeviceId";

		// Token: 0x0400221D RID: 8733
		private const string EVENT_ID_KEY = "EventId";

		// Token: 0x0400221E RID: 8734
		private const string TIME_SESSION_START_KEY = "TimeSessionStart";

		// Token: 0x0400221F RID: 8735
		private const string TIME_SESSION_GAMEPLAY_START_KEY = "TimeSessionGameplayStart";

		// Token: 0x04002220 RID: 8736
		private const string TIME_SESSION_GAMEPLAY_KEY = "TimeSessionGameplay";

		// Token: 0x04002221 RID: 8737
		private const string TIME_LTD_KEY = "TimeLTD";

		// Token: 0x04002222 RID: 8738
		private const string TIME_LTD_GAMEPLAY_KEY = "TimeLTDGameplay";

		// Token: 0x04002223 RID: 8739
		private static Environments environmentType = Environments.dev;

		// Token: 0x04002224 RID: 8740
		private static long timeSessionTimestamp = 0L;

		// Token: 0x04002225 RID: 8741
		private static long timeSessionGameplayTimestamp = 0L;

		// Token: 0x04002226 RID: 8742
		private static long timeGameplaySeconds = 0L;

		// Token: 0x04002227 RID: 8743
		private static bool appStopped = false;

		// Token: 0x04002228 RID: 8744
		private static DateTime timeStopDate = DateTime.UtcNow;

		// Token: 0x04002229 RID: 8745
		private static long sessionStart = 0L;

		// Token: 0x0400222A RID: 8746
		private static string appBootSessionID = "";

		// Token: 0x0400222B RID: 8747
		private static string ip = "";

		// Token: 0x0400222C RID: 8748
		private static int undoCounter = 0;

		// Token: 0x0400222D RID: 8749
		private static List<PlayerEndGameStats> endgameStats;

		// Token: 0x0400222E RID: 8750
		private static string matchSessionID = "";

		// Token: 0x0400222F RID: 8751
		private static MatchLaunchMethods matchLaunchMethod = MatchLaunchMethods.main;

		// Token: 0x04002230 RID: 8752
		private static bool saveTestLoading = false;

		// Token: 0x04002232 RID: 8754
		private static int screenCount = 0;

		// Token: 0x04002233 RID: 8755
		private static Screens previousScreen = Screens.none;

		// Token: 0x04002234 RID: 8756
		private static Screens overridedScreen = Screens.none;

		// Token: 0x04002235 RID: 8757
		private static Screens currentScreen = Screens.none;

		// Token: 0x04002236 RID: 8758
		private static float previousScreenTime = -1f;

		// Token: 0x04002237 RID: 8759
		private static float currentScreenTime = 0f;

		// Token: 0x04002238 RID: 8760
		private static DateTime currentScreenVisibleStart;

		// Token: 0x04002239 RID: 8761
		private static Screens currentTab = Screens.none;

		// Token: 0x0400223A RID: 8762
		private static float tabTime = 0f;

		// Token: 0x0400223B RID: 8763
		private static DateTime currentTabVisibleStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x0400223C RID: 8764
		private static DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x0400223D RID: 8765
		private static ScreenPreviousNavActions lastNavigationAction = ScreenPreviousNavActions.none;

		// Token: 0x0400223E RID: 8766
		private static Contexts current_context;

		// Token: 0x0400223F RID: 8767
		private const string TUTORIALS_FINISHED_VECTOR = "TutorialsFinishedVector";

		// Token: 0x04002240 RID: 8768
		private static DateTime tutorialStartTimestamp = DateTime.Now;

		// Token: 0x04002241 RID: 8769
		private static float currentTime = 0f;

		// Token: 0x04002242 RID: 8770
		private static StepIDs currentStep = StepIDs.none;

		// Token: 0x04002243 RID: 8771
		private static int currentStepSequence = 0;

		// Token: 0x04002244 RID: 8772
		private static bool playerFinishedCurrentTutorialSession = false;
	}
}

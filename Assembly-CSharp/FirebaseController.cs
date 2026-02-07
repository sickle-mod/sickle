using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsmodeeNet.Foundation;
using Firebase;
using Firebase.Crashlytics;
using Firebase.Extensions;
using Firebase.Messaging;
using I2.Loc;
using Newtonsoft.Json;
using Scythe.Analytics;
using Scythe.Multiplayer;
using Scythe.Multiplayer.AuthApi.Models;
using Scythe.Multiplayer.Notifications;
using Scythe.Utilities;
using TMPro;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class FirebaseController : MonoBehaviour
{
	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000870 RID: 2160 RVA: 0x0002DB23 File Offset: 0x0002BD23
	// (set) Token: 0x06000871 RID: 2161 RVA: 0x0002DB2B File Offset: 0x0002BD2B
	public bool IsReady { get; protected set; }

	// Token: 0x06000872 RID: 2162 RVA: 0x0002DB34 File Offset: 0x0002BD34
	private void Awake()
	{
		if (PlatformManager.IsStandalone)
		{
			return;
		}
		global::UnityEngine.Object.DontDestroyOnLoad(this);
		FirebaseApp.LogLevel = LogLevel.Debug;
		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(delegate(Task<DependencyStatus> task)
		{
			DependencyStatus result = task.Result;
			if (result == DependencyStatus.Available)
			{
				FirebaseApp defaultInstance = FirebaseApp.DefaultInstance;
				Crashlytics.IsCrashlyticsCollectionEnabled = true;
				this.IsReady = true;
				this.RegisterEvents();
				AnalyticsEventData.NotificationSystemReady();
				this.SetCrashlyticsData();
				return;
			}
			Debug.LogError(string.Format("[FirebaseController]: Could not resolve all Firebase dependencies: {0}", result));
		});
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x0002DB61 File Offset: 0x0002BD61
	private void RegisterEvents()
	{
		FirebaseMessaging.TokenReceived += this.OnTokenReceived;
		FirebaseMessaging.MessageReceived += this.OnMessageReceived;
		Singleton<LoginController>.Instance.OnPlayerIdChangeSuccess += this.LoginController_OnPlayerIdChanged;
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00078540 File Offset: 0x00076740
	private void LoginController_OnPlayerIdChanged(LoginResponse result)
	{
		Crashlytics.SetUserId(result.Id.ToString());
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00078568 File Offset: 0x00076768
	private void SetCrashlyticsData()
	{
		Crashlytics.SetCustomKey("Device Memory", SystemInfo.systemMemorySize.ToString() + "MB");
		Crashlytics.SetCustomKey("Graphics Memory", SystemInfo.graphicsMemorySize.ToString() + "MB");
		Crashlytics.SetCustomKey("Operating System", SystemInfo.operatingSystem);
		Crashlytics.SetCustomKey("Device Model", SystemInfo.deviceModel);
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x0002DB9B File Offset: 0x0002BD9B
	private void OnTokenReceived(object sender, TokenReceivedEventArgs token)
	{
		Singleton<LoginController>.Instance.UpdateFirebaseToken(token.Token);
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x000785D8 File Offset: 0x000767D8
	private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
	{
		Debug.Log(this.ExtractData(e));
		Notification notification = this.ExtractNotification(e);
		bool flag = false;
		if (notification != null && e.Message.NotificationOpened)
		{
			if (GenericSingletonClass<NotificationLogic>.Instance != null)
			{
				GenericSingletonClass<NotificationLogic>.Instance.NotificationClicked(notification);
				flag = true;
			}
			else
			{
				Debug.Log("<color=red>Error: </color> NotificationLogic Instance is null");
			}
		}
		else if (notification != null && this.notificationsDisplayManager != null && GenericSingletonClass<NotificationLogic>.Instance != null && GenericSingletonClass<NotificationLogic>.Instance.IsDisplayable(notification))
		{
			this.notificationsDisplayManager.EnqueueNotificationForDisplay(notification);
		}
		if (notification != null)
		{
			this.SendNotificationToAnalytics(notification, flag);
		}
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x00078678 File Offset: 0x00076878
	private void SendNotificationToAnalytics(Notification notification, bool notificationOpenedApp)
	{
		if (notification is YourTurnNotification)
		{
			YourTurnNotification yourTurnNotification = notification as YourTurnNotification;
			AnalyticsEventLogger.Instance.LogPushNotification(PushTypes.server_side, notificationOpenedApp, "YourTurnNotification", this.GetUntranslatedText("Notifications/YourTurnClick"), this.GetUntranslatedText("Notifications/YourTurnInfo").Replace("{[GAME_NAME]}", yourTurnNotification.GameName));
			return;
		}
		if (notification is GameOverNotification)
		{
			GameOverNotification gameOverNotification = notification as GameOverNotification;
			AnalyticsEventLogger.Instance.LogPushNotification(PushTypes.server_side, notificationOpenedApp, "GameOverNotification", this.GetUntranslatedText("Notifications/EndOfGameClick"), this.GetUntranslatedText("Notifications/EndOfGameInfo").Replace("{[GAME_NAME]}", gameOverNotification.GameName).Replace("{[PLAYER_PLACE]}", this.PlaceLocalizedString(gameOverNotification.PlayerPlace)));
			return;
		}
		if (notification is CombatNotification)
		{
			CombatNotification combatNotification = notification as CombatNotification;
			AnalyticsEventLogger.Instance.LogPushNotification(PushTypes.server_side, notificationOpenedApp, "CombatNotification", this.GetUntranslatedText("Notifications/CombatClick"), this.GetUntranslatedText("Notifications/CombatInfo").Replace("{[GAME_NAME]}", combatNotification.GameName));
		}
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x00078770 File Offset: 0x00076970
	private string GetUntranslatedText(string key)
	{
		string currentLanguage = LocalizationManager.CurrentLanguage;
		LocalizationManager.CurrentLanguage = "English";
		string text = ScriptLocalization.Get(key);
		LocalizationManager.CurrentLanguage = currentLanguage;
		return text;
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x0007879C File Offset: 0x0007699C
	private string PlaceLocalizedString(int place)
	{
		switch (place)
		{
		case 0:
			return "last place";
		case 1:
			return this.GetUntranslatedText("Statistics/1Place");
		case 2:
			return this.GetUntranslatedText("Statistics/2Place");
		case 3:
			return this.GetUntranslatedText("Statistics/3Place");
		case 4:
			return this.GetUntranslatedText("Statistics/4Place");
		case 5:
			return this.GetUntranslatedText("Statistics/5Place");
		case 6:
			return this.GetUntranslatedText("Statistics/6Place");
		case 7:
			return this.GetUntranslatedText("Statistics/7Place");
		default:
			throw new ArgumentOutOfRangeException("Not a valid place number. " + place.ToString());
		}
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00078844 File Offset: 0x00076A44
	private void HandleNotification(MessageReceivedEventArgs e)
	{
		string text = string.Empty;
		if (e.Message.Data.TryGetValue("notification", out text))
		{
			text = text.Replace("ScytheWebRole", "Assembly-CSharp");
			text = text.Replace("\\\"", "\"");
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Objects
			};
			Notification notification = JsonConvert.DeserializeObject<Notification>(text, jsonSerializerSettings);
			CoreApplication.Instance.NotificationCenter.PostNotification(notification);
		}
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x000788B8 File Offset: 0x00076AB8
	private Notification ExtractNotification(MessageReceivedEventArgs e)
	{
		string text = string.Empty;
		if (e.Message.Data.TryGetValue("notification", out text))
		{
			text = text.Replace("ScytheWebRole", "Assembly-CSharp");
			text = text.Replace("\\\"", "\"");
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Objects
			};
			return JsonConvert.DeserializeObject<Notification>(text, jsonSerializerSettings);
		}
		return null;
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x0002DBAD File Offset: 0x0002BDAD
	private void PrintToken(TokenReceivedEventArgs token)
	{
		if (this.textField != null)
		{
			TextMeshProUGUI textMeshProUGUI = this.textField;
			textMeshProUGUI.text = textMeshProUGUI.text + "\nToken Recived: " + token.Token;
		}
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x0007891C File Offset: 0x00076B1C
	private void PrintMessage(MessageReceivedEventArgs e)
	{
		if (this.textField != null)
		{
			TextMeshProUGUI textMeshProUGUI = this.textField;
			TMP_Text tmp_Text = textMeshProUGUI;
			string[] array = new string[35];
			array[0] = textMeshProUGUI.text;
			array[1] = "\nMessage Recived:  Tittle ";
			array[2] = e.Message.Notification.Title;
			array[3] = " Body ";
			array[4] = e.Message.Notification.Body;
			array[5] = " ClickAction ";
			array[6] = e.Message.Notification.ClickAction;
			array[7] = " ID ";
			array[8] = e.Message.MessageId;
			array[9] = " MessageType ";
			array[10] = e.Message.MessageType;
			array[11] = " RawData ";
			int num = 12;
			byte[] rawData = e.Message.RawData;
			array[num] = ((rawData != null) ? rawData.ToString() : null);
			array[13] = " Data ";
			array[14] = this.ExtractData(e);
			array[15] = " Notification ";
			int num2 = 16;
			FirebaseNotification notification = e.Message.Notification;
			array[num2] = ((notification != null) ? notification.ToString() : null);
			array[17] = " From ";
			array[18] = e.Message.From;
			array[19] = " To ";
			array[20] = e.Message.To;
			array[21] = " Opened ";
			array[22] = e.Message.NotificationOpened.ToString();
			array[23] = " TimeToLive ";
			array[24] = e.Message.TimeToLive.ToString();
			array[25] = " Priority ";
			array[26] = e.Message.Priority;
			array[27] = " CollapseKey ";
			array[28] = e.Message.CollapseKey;
			array[29] = " Link ";
			int num3 = 30;
			Uri link = e.Message.Link;
			array[num3] = ((link != null) ? link.ToString() : null);
			array[31] = " Error ";
			array[32] = e.Message.Error;
			array[33] = " ErrorDescription ";
			array[34] = e.Message.ErrorDescription;
			tmp_Text.text = string.Concat(array);
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00078B34 File Offset: 0x00076D34
	private string ExtractData(MessageReceivedEventArgs e)
	{
		string text = "";
		foreach (KeyValuePair<string, string> keyValuePair in e.Message.Data)
		{
			text = string.Concat(new string[] { text, "Key: ", keyValuePair.Key, " Value: ", keyValuePair.Value, "  " });
		}
		return text;
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x0002DBDE File Offset: 0x0002BDDE
	public void ClearLog_OnClick()
	{
		if (this.textField != null)
		{
			this.textField.text = "";
		}
	}

	// Token: 0x04000717 RID: 1815
	[SerializeField]
	private TextMeshProUGUI textField;

	// Token: 0x04000718 RID: 1816
	[SerializeField]
	private NotificationsDisplayManager notificationsDisplayManager;
}

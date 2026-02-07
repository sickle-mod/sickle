using System;
using System.Collections;
using System.Collections.Generic;
using Scythe.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scythe.Analytics
{
	// Token: 0x0200063D RID: 1597
	public class AnalyticsEventLogger : MonoBehaviour
	{
		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x060032D1 RID: 13009 RVA: 0x0004809B File Offset: 0x0004629B
		// (set) Token: 0x060032D2 RID: 13010 RVA: 0x000480C6 File Offset: 0x000462C6
		public static AnalyticsEventLogger Instance
		{
			get
			{
				if (AnalyticsEventLogger.instance == null)
				{
					GameObject gameObject = new GameObject("AmplitudeEventLogger");
					gameObject.AddComponent<AnalyticsEventLogger>();
					return gameObject.GetComponent<AnalyticsEventLogger>();
				}
				return AnalyticsEventLogger.instance;
			}
			private set
			{
				AnalyticsEventLogger.instance = value;
			}
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x0013135C File Offset: 0x0012F55C
		private void Awake()
		{
			if (AnalyticsEventLogger.instance == null)
			{
				this.GenerateAmplitudeService();
				AnalyticsEventLogger.instance = this;
				this.firstAppBootEvent = true;
				AnalyticsEventData.CleanSessionTimes();
				AnalyticsEventData.ResetMatchSessionID();
				AnalyticsEventData.UpdateEnvironmentType();
				AnalyticsEventData.TabEnabled(false, Screens.none);
				AnalyticsEventData.SessionTimeStart();
				AnalyticsEventData.CreateAppBootSessionID();
				if (PlatformManager.IsMobile)
				{
					this.delayedAppBootCoroutine = base.StartCoroutine(this.LogAppBootWithDelay());
				}
				else
				{
					this.LogAppBoot(LaunchMethods.boot);
				}
			}
			else if (AnalyticsEventLogger.instance != this)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x000480CE File Offset: 0x000462CE
		private IEnumerator LogAppBootWithDelay()
		{
			yield return new WaitForSeconds(2f);
			this.LogAppBoot(LaunchMethods.boot);
			this.delayedAppBootCoroutine = null;
			yield break;
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x001313F0 File Offset: 0x0012F5F0
		private void OnApplicationFocus(bool hasFocus)
		{
			if (!hasFocus)
			{
				AnalyticsEventData.ApplicationFocusChanged(hasFocus, false);
				return;
			}
			if (this.firstAppBootEvent)
			{
				this.firstAppBootEvent = false;
				return;
			}
			if (PlatformManager.IsMobile)
			{
				AnalyticsEventData.CreateAppBootSessionID();
				this.LogAppBoot(LaunchMethods.resume);
			}
			if (this.timeSpentOutOfFocus >= 300f)
			{
				AnalyticsEventData.ApplicationFocusChanged(hasFocus, true);
			}
			else
			{
				AnalyticsEventData.ApplicationFocusChanged(hasFocus, false);
			}
			this.timeSpentOutOfFocus = 0f;
			this.outOfFocusSessionEndSend = false;
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x000480DD File Offset: 0x000462DD
		private void OnApplicationQuit()
		{
			AnalyticsEventData.CleanSessionTimes();
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x0013145C File Offset: 0x0012F65C
		private void Update()
		{
			if (!Application.isFocused)
			{
				this.timeSpentOutOfFocus += Time.unscaledDeltaTime;
				if (this.timeSpentOutOfFocus >= 300f && !this.outOfFocusSessionEndSend)
				{
					AnalyticsEventData.SessionTimeEnd(SceneManager.GetActiveScene().name.Equals(SceneController.SCENE_MAIN_NAME) && !GameController.GameManager.GameFinished);
					this.outOfFocusSessionEndSend = true;
				}
			}
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x000480E4 File Offset: 0x000462E4
		public void Create()
		{
			if (AnalyticsEventLogger.instance == null)
			{
				new GameObject("AmplitudeEventLogger").AddComponent<AnalyticsEventLogger>();
			}
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x001314CC File Offset: 0x0012F6CC
		private void GenerateAmplitudeService()
		{
			AmplitudeHttp amplitudeHttp = base.gameObject.AddComponent<AmplitudeHttp>();
			amplitudeHttp.SetApiKey(AmplitudeApiKey.ApiKey());
			this.analyticsService = amplitudeHttp;
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x00048103 File Offset: 0x00046303
		public bool FirstAppBoot()
		{
			return this.firstAppBootEvent;
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x0004810B File Offset: 0x0004630B
		private bool CanAddData()
		{
			return this.sendData && Application.internetReachability > NetworkReachability.NotReachable;
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x0004811F File Offset: 0x0004631F
		public void LogAppBoot(LaunchMethods launchMethod)
		{
			if (!this.CanAddData())
			{
				return;
			}
			if (launchMethod == LaunchMethods.resume && this.firstAppBootEvent)
			{
				this.firstAppBootEvent = false;
				return;
			}
			this.LogEvent(AnalyticsEventTypes.APP_BOOT, AnalyticsEvent.AppBoot(launchMethod));
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x0004814B File Offset: 0x0004634B
		public void LogScreenDisplay(Screens screen, Screens previousScreen, Contexts context)
		{
			if (!this.CanAddData())
			{
				return;
			}
			this.LogEvent(AnalyticsEventTypes.SCREEN_DISPLAY, AnalyticsEvent.ScreenDisplay(screen, previousScreen, context));
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x00048165 File Offset: 0x00046365
		public void LogScreenDisplay(Screens screen, Contexts context)
		{
			if (!this.CanAddData())
			{
				return;
			}
			this.LogEvent(AnalyticsEventTypes.SCREEN_DISPLAY, AnalyticsEvent.ScreenDisplay(screen, context));
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x0004817E File Offset: 0x0004637E
		public void LogMatchStart()
		{
			if (!this.CanAddData())
			{
				return;
			}
			this.LogEvent(AnalyticsEventTypes.MATCH_START, AnalyticsEvent.MatchStart());
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x00048195 File Offset: 0x00046395
		public void LogMatchStop(EndReasons reason)
		{
			if (!this.CanAddData())
			{
				return;
			}
			this.LogEvent(AnalyticsEventTypes.MATCH_STOP, AnalyticsEvent.MatchStop(reason));
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x000481AD File Offset: 0x000463AD
		public void LogCreateAsmodeeNetAccount(bool newsletter, SingupPaths path)
		{
			if (!this.CanAddData())
			{
				return;
			}
			this.LogEvent(AnalyticsEventTypes.CREATE_ASMODEENET_ACCOUNT, AnalyticsEvent.CreateAsmodeeNetAccount(newsletter, path));
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x000481C7 File Offset: 0x000463C7
		public void LogFriendManagement(ActionsOnFriend action, string friendID, int friendsCount)
		{
			if (!this.CanAddData())
			{
				return;
			}
			this.LogEvent(AnalyticsEventTypes.FRIEND_MANAGEMENT, AnalyticsEvent.FriendManagement(action, friendID, friendsCount));
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x000481E2 File Offset: 0x000463E2
		public void LogTutorialStep(StepStatuses status)
		{
			if (!this.CanAddData())
			{
				return;
			}
			this.LogEvent(AnalyticsEventTypes.TUTORIAL_STEP, AnalyticsEvent.TutorialStep(status));
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x000481FB File Offset: 0x000463FB
		public void LogAchievementUnlocked(Achievements achievement)
		{
			if (!this.CanAddData())
			{
				return;
			}
			this.LogEvent(AnalyticsEventTypes.ACHIEVEMENT_UNLOCK, AnalyticsEvent.AchievementUnlock(achievement));
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x00048214 File Offset: 0x00046414
		public void LogContentUnlock(Achievements achievement, string contentType, UnlockReasons unlockReason)
		{
			if (!this.CanAddData())
			{
				return;
			}
			this.LogEvent(AnalyticsEventTypes.CONTENT_UNLOCK, AnalyticsEvent.ContentUnlock(achievement, contentType, unlockReason));
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x0004822F File Offset: 0x0004642F
		public void LogCrossPromoOpened(string crossPromoType, Dictionary<string, object> eventProperties)
		{
			if (!this.CanAddData())
			{
				return;
			}
			this.LogEvent(AnalyticsEventTypes.CROSSPROMO_OPENED, AnalyticsEvent.CrossPromoOpened(crossPromoType, eventProperties));
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x00048249 File Offset: 0x00046449
		public void LogCrossPromoScreenDisplay(Dictionary<string, object> screenDisplayProperties)
		{
			if (!this.CanAddData())
			{
				return;
			}
			this.LogEvent(AnalyticsEventTypes.CROSSPROMO_SCREEN_DISPLAY, AnalyticsEvent.CrossPromoScreenDisplay(screenDisplayProperties));
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x00048262 File Offset: 0x00046462
		public void LogCrossPromoRedirected(Dictionary<string, object> screenDisplayProperties)
		{
			if (!this.CanAddData())
			{
				return;
			}
			this.LogEvent(AnalyticsEventTypes.CROSSPROMO_REDIRECTED, AnalyticsEvent.CrossPromoRedirected(screenDisplayProperties));
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x0004827B File Offset: 0x0004647B
		public void LogCrossPromoClosed(Dictionary<string, object> screenDisplayProperties)
		{
			if (!this.CanAddData())
			{
				return;
			}
			this.LogEvent(AnalyticsEventTypes.CROSSPROMO_CLOSED, AnalyticsEvent.CrossPromoClosed(screenDisplayProperties));
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x001314F8 File Offset: 0x0012F6F8
		public void LogPushNotification(PushTypes pushType, bool pushOpen, string title, string title2, string message)
		{
			if (!this.CanAddData())
			{
				return;
			}
			if (pushOpen && this.delayedAppBootCoroutine != null)
			{
				base.StopCoroutine(this.delayedAppBootCoroutine);
				this.delayedAppBootCoroutine = null;
				this.LogAppBoot(LaunchMethods.notification);
			}
			this.LogEvent(AnalyticsEventTypes.PUSH_NOTIFICATION, AnalyticsEvent.PusnNotificationReceived(pushType, pushOpen, title, title2, message));
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x00131548 File Offset: 0x0012F748
		private void LogEvent(AnalyticsEventTypes eventType, Dictionary<string, object> eventProperties)
		{
			if (PlatformManager.IsMobile && this.delayedAppBootCoroutine != null && eventType != AnalyticsEventTypes.APP_BOOT)
			{
				base.StartCoroutine(this.DelayedLogEvent(eventType, eventProperties));
				return;
			}
			this.analyticsService.SetUserId(AnalyticsEventData.UserID());
			this.analyticsService.UpdateUserProperties(AnalyticsEvent.GetUserProperties(false));
			this.analyticsService.LogEvent(eventType, AnalyticsEvent.GetEventHeaderData(eventType), eventProperties);
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x00048294 File Offset: 0x00046494
		private IEnumerator DelayedLogEvent(AnalyticsEventTypes eventType, Dictionary<string, object> eventProperties)
		{
			yield return new WaitForSeconds(2f);
			this.analyticsService.SetUserId(AnalyticsEventData.UserID());
			this.analyticsService.UpdateUserProperties(AnalyticsEvent.GetUserProperties(false));
			this.analyticsService.LogEvent(eventType, AnalyticsEvent.GetEventHeaderData(eventType), eventProperties);
			yield break;
		}

		// Token: 0x040021E7 RID: 8679
		private static AnalyticsEventLogger instance;

		// Token: 0x040021E8 RID: 8680
		private IAnalyticsService analyticsService;

		// Token: 0x040021E9 RID: 8681
		private bool firstAppBootEvent;

		// Token: 0x040021EA RID: 8682
		private const float MAX_TIME_OUT_OF_FOCUS = 300f;

		// Token: 0x040021EB RID: 8683
		private float timeSpentOutOfFocus;

		// Token: 0x040021EC RID: 8684
		private bool outOfFocusSessionEndSend;

		// Token: 0x040021ED RID: 8685
		private bool sendData = true;

		// Token: 0x040021EE RID: 8686
		private Coroutine delayedAppBootCoroutine;
	}
}

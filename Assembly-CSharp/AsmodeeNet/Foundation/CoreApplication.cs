using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation.Localization;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.UserInterface;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x0200093E RID: 2366
	[RequireComponent(typeof(SceneTransitionManager))]
	[RequireComponent(typeof(EventManager))]
	[RequireComponent(typeof(UINavigationManager))]
	[RequireComponent(typeof(AnalyticsManager))]
	[RequireComponent(typeof(AvatarManager))]
	[RequireComponent(typeof(CommunityHubLauncher))]
	[RequireComponent(typeof(FocusableLayer))]
	public class CoreApplication : MonoBehaviour
	{
		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06003F7A RID: 16250 RVA: 0x00050B9C File Offset: 0x0004ED9C
		public static CoreApplication Instance
		{
			get
			{
				if (CoreApplication._instance == null)
				{
					AsmoLogger.Log(Application.isPlaying ? AsmoLogger.Severity.Error : AsmoLogger.Severity.Warning, "CoreApplication", "Missing CoreApplication Instance", null);
				}
				return CoreApplication._instance;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06003F7B RID: 16251 RVA: 0x00050BCB File Offset: 0x0004EDCB
		public static bool HasInstance
		{
			get
			{
				return CoreApplication._instance != null;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06003F7C RID: 16252 RVA: 0x00050BD8 File Offset: 0x0004EDD8
		public Channel Channel
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				return Channel.steam;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06003F7D RID: 16253 RVA: 0x00050BE1 File Offset: 0x0004EDE1
		public SceneTransitionManager SceneTransitionManager
		{
			get
			{
				return this._sceneTransitionManager;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06003F7E RID: 16254 RVA: 0x00050BE9 File Offset: 0x0004EDE9
		public EventManager EventManager
		{
			get
			{
				return this._eventManager;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06003F7F RID: 16255 RVA: 0x00050BF1 File Offset: 0x0004EDF1
		public UINavigationManager UINavigationManager
		{
			get
			{
				return this._uiNavigationManager;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06003F80 RID: 16256 RVA: 0x00050BF9 File Offset: 0x0004EDF9
		public AnalyticsManager AnalyticsManager
		{
			get
			{
				return this._analyticsManager;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06003F81 RID: 16257 RVA: 0x00050C01 File Offset: 0x0004EE01
		public CommunityHubLauncher CommunityHubLauncher
		{
			get
			{
				return this._communityHubLauncher;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06003F82 RID: 16258 RVA: 0x00050C09 File Offset: 0x0004EE09
		public CommunityHub CommunityHub
		{
			get
			{
				return this._communityHubLauncher.CommunityHub;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06003F83 RID: 16259 RVA: 0x00050C16 File Offset: 0x0004EE16
		public OAuthGate OAuthGate
		{
			get
			{
				return this._oauthGate;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06003F84 RID: 16260 RVA: 0x00050C1E File Offset: 0x0004EE1E
		public AvatarManager AvatarManager
		{
			get
			{
				return this._avatarManager;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06003F85 RID: 16261 RVA: 0x00050C26 File Offset: 0x0004EE26
		public Preferences Preferences
		{
			get
			{
				return this._preferences;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06003F86 RID: 16262 RVA: 0x00050C2E File Offset: 0x0004EE2E
		public LocalizationManager LocalizationManager
		{
			get
			{
				return this._localizationManager;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06003F87 RID: 16263 RVA: 0x00050C36 File Offset: 0x0004EE36
		// (set) Token: 0x06003F88 RID: 16264 RVA: 0x00050C3E File Offset: 0x0004EE3E
		public ISteamManager SteamManager { get; private set; }

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06003F89 RID: 16265 RVA: 0x00050C47 File Offset: 0x0004EE47
		public NotificationCenter NotificationCenter
		{
			get
			{
				if (this._notificationCenter == null)
				{
					this._notificationCenter = new NotificationCenter();
				}
				return this._notificationCenter;
			}
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x00050C62 File Offset: 0x0004EE62
		protected virtual void Awake()
		{
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			this.SetCultureInfo();
			if (CoreApplication._instance == null)
			{
				this._Initialize();
				return;
			}
			if (CoreApplication._instance != this)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x0015C6F8 File Offset: 0x0015A8F8
		private void _Initialize()
		{
			Hashtable hashtable = new Hashtable { 
			{
				"SDK Version",
				SDKVersionManager.Version()
			} };
			AsmoLogger.Info("CoreApplication", "Initialization", hashtable);
			CoreApplication._instance = this;
			this._sceneTransitionManager = base.GetComponent<SceneTransitionManager>();
			if (this._sceneTransitionManager == null)
			{
				AsmoLogger.Error("CoreApplication", "Missing SceneTransitionManager component", null);
			}
			this._eventManager = base.GetComponent<EventManager>();
			if (this._eventManager == null)
			{
				AsmoLogger.Error("CoreApplication", "Missing EventManager component", null);
			}
			this._uiNavigationManager = base.GetComponent<UINavigationManager>();
			if (this._uiNavigationManager == null)
			{
				this._uiNavigationManager = base.gameObject.AddComponent<UINavigationManager>();
			}
			if (base.GetComponent<FocusableLayer>() == null)
			{
				base.gameObject.AddComponent<FocusableLayer>();
			}
			this._analyticsManager = base.GetComponent<AnalyticsManager>();
			if (this._analyticsManager == null)
			{
				AsmoLogger.Error("CoreApplication", "Missing AnalyticsManager component", null);
			}
			this._avatarManager = base.GetComponent<AvatarManager>();
			if (this._avatarManager == null)
			{
				AsmoLogger.Error("CoreApplication", "Missing AvatarManager component", null);
			}
			this._communityHubLauncher = base.GetComponent<CommunityHubLauncher>();
			if (this._communityHubLauncher == null)
			{
				AsmoLogger.Error("CoreApplication", "Missing CommunityHubLauncher component", null);
			}
			KeyValueStore.Instance = new PlayerPrefsKeyValueStore();
			try
			{
				this.SteamManager = new SteamManagerTKOU(this.SteamGameId);
			}
			catch (Exception ex)
			{
				AsmoLogger.Error("CoreApplication.Steam", string.Format("Couldn't initialize Steam because: \"{0}\" (exception type = {1})", ex.Message, ex.GetType()), null);
			}
			if (this.NetworkParameters == null)
			{
				string text = "NetworkParameters must be set for CoreApplication to work";
				AsmoLogger.Error("CoreApplication", text, null);
				throw new ArgumentNullException(text);
			}
			AsmoLogger.Info("CoreApplication", "Instantiating OAuthGate", hashtable);
			this._oauthGate = new OAuthGate(this.NetworkParameters)
			{
				SteamManager = this.SteamManager
			};
			AsmoLogger.Info("CoreApplication", "OAuthGate instantiated correctly.", hashtable);
			if (this.InterfaceSkin == null)
			{
				AsmoLogger.Warning("CoreApplication", "Interface Skin not provided -> Fall back to default", null);
				this.InterfaceSkin = global::UnityEngine.Object.Instantiate(Resources.Load("InterfaceDefaultSkin", typeof(InterfaceSkin))) as InterfaceSkin;
			}
			AsmoLogger.Info("CoreApplication", "InterfaceSkin instantiated correctly.", hashtable);
			this._localizationManager = new LocalizationManager(this.supportedLanguages);
			this._preferences = new Preferences();
			AsmoLogger.Info("CoreApplication", "Initialized (Lite SDK)", hashtable);
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x00050CA1 File Offset: 0x0004EEA1
		private void Start()
		{
			this._localizationManager.Init();
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x00050CAE File Offset: 0x0004EEAE
		private void Update()
		{
			if (this.SteamManager != null)
			{
				this.SteamManager.Update();
			}
			this._preferences.Update();
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06003F8E RID: 16270 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public static bool IsFullSDK
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06003F8F RID: 16271 RVA: 0x00050CCE File Offset: 0x0004EECE
		// (set) Token: 0x06003F90 RID: 16272 RVA: 0x00050CD5 File Offset: 0x0004EED5
		public static bool IsQuitting { get; private set; }

		// Token: 0x06003F91 RID: 16273 RVA: 0x00050CDD File Offset: 0x0004EEDD
		private void OnApplicationQuit()
		{
			CoreApplication.IsQuitting = true;
			KeyValueStore.ResetInstance();
			if (this.SteamManager != null)
			{
				this.SteamManager.Dispose();
				this.SteamManager = null;
			}
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x00050D04 File Offset: 0x0004EF04
		private void OnApplicationPause(bool paused)
		{
			if (!paused && this._avatarManager != null)
			{
				this._avatarManager.ClearCache();
			}
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x0002F5B4 File Offset: 0x0002D7B4
		public static string GetUserAgent()
		{
			return string.Format("{0}/{1} {2}/ Unity/{3}", new object[]
			{
				Application.productName,
				Application.version,
				Application.platform,
				Application.unityVersion
			});
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x0015C97C File Offset: 0x0015AB7C
		private void SetCultureInfo()
		{
			CultureInfo cultureInfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
			cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		}

		// Token: 0x0400309B RID: 12443
		private const string _consoleModuleName = "CoreApplication";

		// Token: 0x0400309C RID: 12444
		public uint SteamGameId = 718560U;

		// Token: 0x0400309D RID: 12445
		private static CoreApplication _instance;

		// Token: 0x0400309E RID: 12446
		private SceneTransitionManager _sceneTransitionManager;

		// Token: 0x0400309F RID: 12447
		private EventManager _eventManager;

		// Token: 0x040030A0 RID: 12448
		private UINavigationManager _uiNavigationManager;

		// Token: 0x040030A1 RID: 12449
		private AnalyticsManager _analyticsManager;

		// Token: 0x040030A2 RID: 12450
		private CommunityHubLauncher _communityHubLauncher;

		// Token: 0x040030A3 RID: 12451
		public NetworkParameters NetworkParameters;

		// Token: 0x040030A4 RID: 12452
		public InterfaceSkin InterfaceSkin;

		// Token: 0x040030A5 RID: 12453
		private OAuthGate _oauthGate;

		// Token: 0x040030A6 RID: 12454
		private AvatarManager _avatarManager;

		// Token: 0x040030A7 RID: 12455
		private Preferences _preferences;

		// Token: 0x040030A8 RID: 12456
		private LocalizationManager _localizationManager;

		// Token: 0x040030A9 RID: 12457
		[Tooltip("You can manage localization in Asmodee.net/Localization")]
		public List<LocalizationManager.Language> supportedLanguages = new List<LocalizationManager.Language> { LocalizationManager.Language.en_US };

		// Token: 0x040030AB RID: 12459
		private NotificationCenter _notificationCenter;
	}
}

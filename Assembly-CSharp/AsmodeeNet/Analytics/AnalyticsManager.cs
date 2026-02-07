using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009CB RID: 2507
	public class AnalyticsManager : MonoBehaviour
	{
		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06004199 RID: 16793 RVA: 0x00051FCF File Offset: 0x000501CF
		private string _AmplitudeAPIKey
		{
			get
			{
				return this._amplitudeAPIKeys.prod;
			}
		}

		// Token: 0x0600419A RID: 16794 RVA: 0x00051FDC File Offset: 0x000501DC
		private void Start()
		{
			this._analyticsService = new AmplitudeHttp(this._AmplitudeAPIKey);
		}

		// Token: 0x0600419B RID: 16795 RVA: 0x00162084 File Offset: 0x00160284
		public void LogEvent(string eventType, IDictionary<string, object> eventProperties)
		{
			Dictionary<string, object> dictionary = eventProperties.Concat(this._EventProperties.Where((KeyValuePair<string, object> _eP) => !eventProperties.Keys.Contains(_eP.Key))).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			dictionary["client_local_time"] = (long)DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
			if (this.HasContext(typeof(ApplicationAnalyticsContext)))
			{
				ApplicationAnalyticsContext applicationAnalyticsContext = this.GetContext(typeof(ApplicationAnalyticsContext)) as ApplicationAnalyticsContext;
				dictionary["app_boot_session_id"] = applicationAnalyticsContext.AppBootSessionId;
				dictionary["time_session"] = applicationAnalyticsContext.TimeSession;
				dictionary["time_session_gameplay"] = applicationAnalyticsContext.TimeSessionGamePlay;
				dictionary["time_ltd"] = applicationAnalyticsContext.TimeLifeToDate;
				dictionary["time_ltd_gameplay"] = applicationAnalyticsContext.TimeLifeToDateGameplay;
			}
			IAnalyticsService analyticsService = this._analyticsService;
			if (analyticsService == null)
			{
				return;
			}
			analyticsService.LogEvent(eventType, dictionary);
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x001621E8 File Offset: 0x001603E8
		public void SetUserProperties(IDictionary<string, object> userProperties)
		{
			Dictionary<string, object> dictionary = userProperties.Concat(this._UserProperties.Where((KeyValuePair<string, object> _uP) => !userProperties.Keys.Contains(_uP.Key))).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			IAnalyticsService analyticsService = this._analyticsService;
			if (analyticsService == null)
			{
				return;
			}
			analyticsService.UpdateUserProperties(dictionary);
		}

		// Token: 0x17000622 RID: 1570
		// (set) Token: 0x0600419D RID: 16797 RVA: 0x00051FEF File Offset: 0x000501EF
		public string UserId
		{
			set
			{
				IAnalyticsService analyticsService = this._analyticsService;
				if (analyticsService == null)
				{
					return;
				}
				analyticsService.SetUserId(value);
			}
		}

		// Token: 0x0600419E RID: 16798 RVA: 0x00052002 File Offset: 0x00050202
		public void SetVersionBuildNumber(string value)
		{
			this._EventProperties["version_build_number"] = value;
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x00052015 File Offset: 0x00050215
		public void SetEnvironment(string value)
		{
			this._EventProperties["environment"] = value;
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x00052028 File Offset: 0x00050228
		public void SetScreenResolution(string value)
		{
			this._EventProperties["screen_resolution"] = value;
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x0005203B File Offset: 0x0005023B
		public void SetConnectionType(string value)
		{
			this._EventProperties["connection_type"] = value;
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x0005204E File Offset: 0x0005024E
		public void SetUnitySDKVersion(string value)
		{
			this._EventProperties["unity_sdk_version"] = value;
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060041A3 RID: 16803 RVA: 0x0016227C File Offset: 0x0016047C
		private Dictionary<string, object> _EventProperties
		{
			get
			{
				if (this._eventProperties == null)
				{
					this._eventProperties = new Dictionary<string, object>();
					this._eventProperties.Add("environment", Application.isEditor ? "dev" : "prod");
					this._eventProperties.Add("screen_resolution", string.Format("{0}*{1}", Screen.width, Screen.height));
					this._eventProperties.Add("connection_type", this._ConnectionType());
					this._eventProperties.Add("unity_sdk_version", SDKVersionManager.Version());
					this._eventProperties.Add("version_build_number", null);
					this._eventProperties.Add("app_boot_session_id", null);
					this._eventProperties.Add("client_local_time", null);
					this._eventProperties.Add("time_session", null);
					this._eventProperties.Add("time_session_gameplay", null);
				}
				return this._eventProperties;
			}
		}

		// Token: 0x060041A4 RID: 16804 RVA: 0x00162378 File Offset: 0x00160578
		private string _ConnectionType()
		{
			switch (Application.internetReachability)
			{
			case NetworkReachability.ReachableViaCarrierDataNetwork:
				return "carrier_data_network";
			case NetworkReachability.ReachableViaLocalAreaNetwork:
				return "local_area_network";
			}
			return "no_connection";
		}

		// Token: 0x060041A5 RID: 16805 RVA: 0x00052061 File Offset: 0x00050261
		public void SetBackendPlatform(string value)
		{
			this._UserProperties["backend_platform"] = value;
		}

		// Token: 0x060041A6 RID: 16806 RVA: 0x00052074 File Offset: 0x00050274
		public void SetBackendUserId(string value)
		{
			this._UserProperties["backend_user_id"] = value;
		}

		// Token: 0x060041A7 RID: 16807 RVA: 0x00052087 File Offset: 0x00050287
		public void SetUAPlatform(string value)
		{
			this._UserProperties["ua_platform"] = value;
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x0005209A File Offset: 0x0005029A
		public void SetUAUserId(string value)
		{
			this._UserProperties["ua_user_id"] = value;
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x000520AD File Offset: 0x000502AD
		public void SetUAChannel(string value)
		{
			this._UserProperties["ua_channel"] = value;
		}

		// Token: 0x060041AA RID: 16810 RVA: 0x000520C0 File Offset: 0x000502C0
		public void SetPushPlatform(string value)
		{
			this._UserProperties["push_platform"] = value;
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x000520D3 File Offset: 0x000502D3
		public void SetPushUserId(string value)
		{
			this._UserProperties["push_user_id"] = value;
		}

		// Token: 0x060041AC RID: 16812 RVA: 0x000520E6 File Offset: 0x000502E6
		public void SetFirstParty(string value)
		{
			this._UserProperties["first_party"] = value;
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x000520F9 File Offset: 0x000502F9
		public void SetUserIdFirstParty(string value)
		{
			this._UserProperties["user_id_first_party"] = value;
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x0005210C File Offset: 0x0005030C
		public void SetTimeLtd(long? value)
		{
			this._UserProperties["time_ltd"] = value;
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x00052124 File Offset: 0x00050324
		public void SetTimeLtdGameplay(long? value)
		{
			this._UserProperties["time_ltd_gameplay"] = value;
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x0005213C File Offset: 0x0005033C
		public void SetABTestGroup(string value)
		{
			this._UserProperties["ab_test_group"] = value;
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x0005214F File Offset: 0x0005034F
		public void SetKarma(int? value)
		{
			this._UserProperties["karma"] = value;
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x00052167 File Offset: 0x00050367
		public void SetEloRating(int? value)
		{
			this._UserProperties["elo_rating"] = value;
		}

		// Token: 0x060041B3 RID: 16819 RVA: 0x0005217F File Offset: 0x0005037F
		public void SetIsPayer(bool? value)
		{
			this._UserProperties["is_payer"] = value;
		}

		// Token: 0x060041B4 RID: 16820 RVA: 0x00052197 File Offset: 0x00050397
		public void SetTimezoneClient(int value)
		{
			this._UserProperties["timezone_client"] = value;
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x060041B5 RID: 16821 RVA: 0x001623B0 File Offset: 0x001605B0
		private Dictionary<string, object> _UserProperties
		{
			get
			{
				if (this._userProperties == null)
				{
					this._userProperties = new Dictionary<string, object>();
					this._userProperties.Add("timezone_client", TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalMinutes);
					this._userProperties.Add("backend_platform", null);
					this._userProperties.Add("backend_user_id", null);
					this._userProperties.Add("ua_platform", null);
					this._userProperties.Add("ua_user_id", null);
					this._userProperties.Add("ua_channel", null);
					this._userProperties.Add("push_platform", null);
					this._userProperties.Add("push_user_id", null);
					this._userProperties.Add("first_party", null);
					this._userProperties.Add("user_id_first_party", null);
					this._userProperties.Add("time_ltd", null);
					this._userProperties.Add("time_ltd_gameplay", null);
					this._userProperties.Add("ab_test_group", null);
					this._userProperties.Add("karma", null);
					this._userProperties.Add("elo_rating", null);
					this._userProperties.Add("is_payer", null);
				}
				return this._userProperties;
			}
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x00162504 File Offset: 0x00160704
		private void OnApplicationPause(bool pause)
		{
			foreach (KeyValuePair<Type, AnalyticsContext> keyValuePair in this._contextData)
			{
				if (pause)
				{
					keyValuePair.Value.Pause();
				}
				else
				{
					keyValuePair.Value.Resume();
				}
			}
		}

		// Token: 0x060041B7 RID: 16823 RVA: 0x000521AF File Offset: 0x000503AF
		private void OnApplicationFocus(bool focus)
		{
			this.OnApplicationPause(!focus);
		}

		// Token: 0x060041B8 RID: 16824 RVA: 0x00162570 File Offset: 0x00160770
		private void OnApplicationQuit()
		{
			foreach (KeyValuePair<Type, AnalyticsContext> keyValuePair in this._contextData)
			{
				keyValuePair.Value.Quit();
			}
		}

		// Token: 0x060041B9 RID: 16825 RVA: 0x000521BB File Offset: 0x000503BB
		public bool HasContext(Type contextType)
		{
			if (!contextType.IsSubclassOf(typeof(AnalyticsContext)))
			{
				throw new ArgumentException("Wrong context " + contextType.ToString() + ". Must be a subclass of AnalyticsContext");
			}
			return this._contextData.ContainsKey(contextType);
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x001625C8 File Offset: 0x001607C8
		public AnalyticsContext GetContext(Type contextType)
		{
			if (!contextType.IsSubclassOf(typeof(AnalyticsContext)))
			{
				throw new ArgumentException("Wrong context " + contextType.ToString() + ". Must be a subclass of AnalyticsContext");
			}
			if (this._contextData.ContainsKey(contextType))
			{
				return this._contextData[contextType];
			}
			AsmoLogger.Log(Application.isEditor ? AsmoLogger.Severity.Warning : AsmoLogger.Severity.Error, "AnalyticsManager", "The required context " + contextType.ToString() + " does not exist", null);
			return null;
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x0016264C File Offset: 0x0016084C
		public void AddContext(AnalyticsContext context)
		{
			Type type = context.GetType();
			if (this._contextData.ContainsKey(type))
			{
				AsmoLogger.Log(Application.isEditor ? AsmoLogger.Severity.Warning : AsmoLogger.Severity.Error, "AnalyticsManager", "Context " + type.ToString() + " already exist", null);
			}
			this._contextData[type] = context;
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x001626A8 File Offset: 0x001608A8
		public void RemoveContext(Type contextType)
		{
			if (!contextType.IsSubclassOf(typeof(AnalyticsContext)))
			{
				throw new ArgumentException("Wrong context " + contextType.ToString() + ". Must be a subclass of AnalyticsContext");
			}
			if (this._contextData.ContainsKey(contextType))
			{
				this._contextData.Remove(contextType);
				return;
			}
			AsmoLogger.Log(Application.isEditor ? AsmoLogger.Severity.Warning : AsmoLogger.Severity.Error, "AnalyticsManager", "Try to remove unknown context " + contextType.ToString(), null);
		}

		// Token: 0x040032D5 RID: 13013
		private const string _kModuleName = "AnalyticsManager";

		// Token: 0x040032D6 RID: 13014
		[SerializeField]
		private AnalyticsManager.AmplitudeApiKeys _amplitudeAPIKeys;

		// Token: 0x040032D7 RID: 13015
		private IAnalyticsService _analyticsService;

		// Token: 0x040032D8 RID: 13016
		private Dictionary<string, object> _eventProperties;

		// Token: 0x040032D9 RID: 13017
		private Dictionary<string, object> _userProperties;

		// Token: 0x040032DA RID: 13018
		private Dictionary<Type, AnalyticsContext> _contextData = new Dictionary<Type, AnalyticsContext>();

		// Token: 0x020009CC RID: 2508
		[Serializable]
		public struct AmplitudeApiKeys
		{
			// Token: 0x040032DB RID: 13019
			public string dev;

			// Token: 0x040032DC RID: 13020
			public string prod;
		}
	}
}

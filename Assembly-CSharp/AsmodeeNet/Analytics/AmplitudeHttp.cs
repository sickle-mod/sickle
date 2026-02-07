using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using BestHTTP;
using MiniJSON;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009D7 RID: 2519
	public class AmplitudeHttp : IAnalyticsService
	{
		// Token: 0x06004225 RID: 16933 RVA: 0x000525F6 File Offset: 0x000507F6
		public AmplitudeHttp(string apiKey)
		{
			this._api_key = apiKey;
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x00162B4C File Offset: 0x00160D4C
		public void LogEvent(string eventType, IDictionary<string, object> eventProperties)
		{
			HTTPRequest httprequest = this._GenerateAmplitudeHttpApiRequest();
			Dictionary<string, object> dictionary = this._GenerateDefaultProperties();
			dictionary.Add("event_type", eventType);
			dictionary.Add("event_properties", eventProperties);
			dictionary.Add("user_properties", this._userProperties);
			string json = Json.Serialize(dictionary);
			httprequest.AddField("event", json);
			httprequest.Send();
			AsmoLogger.Trace("AmplitudeHttp", () => "LogEvent " + json, null);
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x00162BD0 File Offset: 0x00160DD0
		public void UpdateUserProperties(IDictionary<string, object> userProperties)
		{
			this._userProperties = userProperties.Concat(this._userProperties.Where((KeyValuePair<string, object> _uP) => !userProperties.Keys.Contains(_uP.Key))).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x00052610 File Offset: 0x00050810
		public void SetUserId(string userId)
		{
			this._userId = userId;
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x00162C58 File Offset: 0x00160E58
		private HTTPRequest _GenerateAmplitudeHttpApiRequest()
		{
			HTTPRequest httprequest = new HTTPRequest(new Uri("https://api.amplitude.com/httpapi"), HTTPMethods.Post, delegate(HTTPRequest req, HTTPResponse resp)
			{
				HTTPRequestStates state = req.State;
				if (state - HTTPRequestStates.Error <= 3)
				{
					AsmoLogger.Error("AmplitudeHttp", string.Format("LogEvent Error {0} [{1}] {2}", req.State.ToString(), resp.StatusCode, resp.Message), null);
				}
			});
			httprequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
			httprequest.AddField("api_key", this._api_key);
			return httprequest;
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x0600422A RID: 16938 RVA: 0x00162CB8 File Offset: 0x00160EB8
		public Dictionary<string, object> FixedProperties
		{
			get
			{
				if (this._fixedProperties == null)
				{
					this._fixedProperties = new Dictionary<string, object>();
					this._fixedProperties.Add("device_id", SystemInfo.deviceUniqueIdentifier);
					this._fixedProperties.Add("app_version", Application.version);
					this._fixedProperties.Add("platform", SystemInfo.operatingSystemFamily.ToString());
					this._fixedProperties.Add("os_name", Environment.OSVersion.Platform.ToString());
					this._fixedProperties.Add("os_version", Environment.OSVersion.Version.ToString());
					this._fixedProperties.Add("device_model", SystemInfo.deviceModel);
					this._fixedProperties.Add("language", CoreApplication.Instance.LocalizationManager.CurrentLanguageCode);
					this._fixedProperties.Add("ip", this._IP());
					this._fixedProperties.Add("session_id", this._GenerateTimestamp());
				}
				return this._fixedProperties;
			}
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x00162DDC File Offset: 0x00160FDC
		private Dictionary<string, object> _GenerateDefaultProperties()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(this.FixedProperties);
			dictionary.Add("user_id", this._userId);
			dictionary.Add("time", this._GenerateTimestamp());
			string text = "event_id";
			int event_id = this._event_id;
			this._event_id = event_id + 1;
			dictionary.Add(text, event_id);
			return dictionary;
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x00162E3C File Offset: 0x0016103C
		private long _GenerateTimestamp()
		{
			return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x00162E70 File Offset: 0x00161070
		private string _IP()
		{
			List<string> list = new List<string>();
			foreach (IPAddress ipaddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
			{
				if (!ipaddress.IsIPv6Multicast && !ipaddress.IsIPv6Teredo && !ipaddress.IsIPv6LinkLocal && !ipaddress.IsIPv6SiteLocal)
				{
					if (ipaddress.AddressFamily == AddressFamily.InterNetwork)
					{
						list.Add(ipaddress.ToString());
					}
					else if (ipaddress.AddressFamily == AddressFamily.InterNetworkV6)
					{
						list.Insert(0, ipaddress.ToString());
					}
				}
			}
			if (list.Count != 0)
			{
				return list.First<string>();
			}
			return null;
		}

		// Token: 0x04003312 RID: 13074
		private const string _kModuleName = "AmplitudeHttp";

		// Token: 0x04003313 RID: 13075
		private string _api_key;

		// Token: 0x04003314 RID: 13076
		private int _event_id;

		// Token: 0x04003315 RID: 13077
		private IDictionary<string, object> _userProperties = new Dictionary<string, object>();

		// Token: 0x04003316 RID: 13078
		private string _userId;

		// Token: 0x04003317 RID: 13079
		private Dictionary<string, object> _fixedProperties;
	}
}

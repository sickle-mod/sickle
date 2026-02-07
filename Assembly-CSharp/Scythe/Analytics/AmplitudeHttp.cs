using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

namespace Scythe.Analytics
{
	// Token: 0x02000636 RID: 1590
	public class AmplitudeHttp : MonoBehaviour, IAnalyticsService
	{
		// Token: 0x0600328A RID: 12938 RVA: 0x00047EA8 File Offset: 0x000460A8
		private void Awake()
		{
			this.webClient.DownloadStringCompleted += this.OnDownloadStringCompleted;
			this.UpdateIP();
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x00047EC7 File Offset: 0x000460C7
		private void Update()
		{
			if (this.eventsQueue.Count != 0 && this.CanSendEvents())
			{
				base.StartCoroutine(this.SendEventData());
			}
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x00047EEB File Offset: 0x000460EB
		private void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus)
			{
				this.UpdateIP();
			}
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x0012FA40 File Offset: 0x0012DC40
		public void LogEvent(AnalyticsEventTypes eventType, IDictionary<string, object> headerProperties, IDictionary<string, object> eventProperties)
		{
			JSONObject jsonobject = this.Serialize(headerProperties);
			JSONObject jsonobject2 = this.Serialize(eventProperties);
			JSONObject jsonobject3 = this.Serialize(this.userProperties);
			jsonobject.Add("event_properties", jsonobject2);
			jsonobject.Add("user_properties", jsonobject3);
			this.AddEventToQueue(jsonobject);
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x0012FA8C File Offset: 0x0012DC8C
		private JSONObject Serialize(IDictionary<string, object> properties)
		{
			JSONObject jsonobject = new JSONObject();
			foreach (string text in properties.Keys)
			{
				if (properties[text] is int)
				{
					jsonobject.Add(text, (int)properties[text]);
				}
				else if (properties[text] is long)
				{
					jsonobject.Add(text, (float)((long)properties[text]));
				}
				else if (properties[text] is float)
				{
					jsonobject.Add(text, (float)properties[text]);
				}
				else if (properties[text] is string)
				{
					jsonobject.Add(text, (string)properties[text]);
				}
			}
			return jsonobject;
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x00047EF6 File Offset: 0x000460F6
		public void UpdateUserProperties(IDictionary<string, object> userProperties)
		{
			this.userProperties = userProperties;
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void SetUserId(string userId)
		{
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void SetApiKey(string apiKey)
		{
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x00047EFF File Offset: 0x000460FF
		public void UpdateIP()
		{
			if (this.obtainIPCoroutine == null)
			{
				this.obtainIPCoroutine = base.StartCoroutine(this.GetIP(this.ipUpdateNeeded));
				this.ipUpdateNeeded = false;
			}
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x00047F28 File Offset: 0x00046128
		public IEnumerator GetIP(bool waitBeforeObtainingIP = false)
		{
			if (waitBeforeObtainingIP)
			{
				yield return this.wait10Seconds;
			}
			if (Application.internetReachability != NetworkReachability.NotReachable)
			{
				try
				{
					Uri uri = new Uri("http://checkip.dyndns.org");
					this.webClient.DownloadStringAsync(uri);
					goto IL_00C9;
				}
				catch (Exception ex)
				{
					Debug.LogError("[Analytics Event Logger] " + ex.ToString());
					this.ipUpdateNeeded = true;
					goto IL_00C9;
				}
			}
			Debug.LogWarning("[Analytics Event Logger] Unable to get IP, trying again.");
			yield return this.wait15Seconds;
			this.obtainIPCoroutine = base.StartCoroutine(this.GetIP(false));
			IL_00C9:
			yield return null;
			yield break;
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x0012FB80 File Offset: 0x0012DD80
		private void OnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				string text = "[Analytics Event Logger] ";
				Exception error = e.Error;
				Debug.LogWarning(text + ((error != null) ? error.ToString() : null));
				this.ipUpdateNeeded = true;
				return;
			}
			string text2 = new Regex("\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}").Matches(e.Result)[0].ToString();
			AnalyticsEventData.UpdateIP(text2);
			this.obtainIPCoroutine = null;
			if (text2 == "")
			{
				Debug.LogWarning("[Analytics Event Logger] Unable to get IP, trying again.");
				this.ipUpdateNeeded = true;
			}
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x00047F3E File Offset: 0x0004613E
		private bool CanSendEvents()
		{
			return this.sendData && Application.internetReachability != NetworkReachability.NotReachable && this.eventsQueue.Count != 0 && !this.error429Recieved && !this.errorReported;
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x00047F6F File Offset: 0x0004616F
		private void AddEventToQueue(JSONObject data)
		{
			if (this.sendData && data != null)
			{
				this.eventsQueue.Enqueue(data);
			}
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x00047F8E File Offset: 0x0004618E
		private IEnumerator SendEventData()
		{
			if (this.CanSendEvents())
			{
				JSONArray multipleEvents = this.GetEventsToSend();
				WWWForm wwwform = new WWWForm();
				wwwform.AddField("api_key", AmplitudeApiKey.ApiKey());
				wwwform.AddField("event", multipleEvents.ToString());
				UnityWebRequest www = UnityWebRequest.Post("https://api.amplitude.com/httpapi", wwwform);
				yield return www.SendWebRequest();
				if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
				{
					Debug.LogWarning("Handling an error which occured while sending the event data to Amplitude. ");
					Debug.LogWarning("Response code: " + www.responseCode.ToString());
					Debug.LogWarning("Error description: " + www.downloadHandler.text);
					this.HandleErrors(www.responseCode, ref multipleEvents);
				}
				www.Dispose();
				multipleEvents = null;
				www = null;
			}
			yield return null;
			yield break;
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x0012FC08 File Offset: 0x0012DE08
		private JSONArray GetEventsToSend()
		{
			JSONArray jsonarray = new JSONArray();
			int num = 0;
			while (num < 5 && this.eventsQueue.Count != 0)
			{
				JSONObject jsonobject = this.eventsQueue.Dequeue();
				jsonobject.Add("client_upload_time", AnalyticsEventData.ClientUploadTime());
				if (this.printDebugData)
				{
					Debug.Log(jsonobject);
				}
				jsonarray.Add(jsonobject);
				num++;
			}
			return jsonarray;
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x0012FC6C File Offset: 0x0012DE6C
		private void HandleErrors(long responseCode, ref JSONArray multipleEvents)
		{
			if (responseCode <= 413L)
			{
				if (responseCode == 400L)
				{
					Debug.LogWarning(responseCode.ToString() + ": Your request is malformed.");
					goto IL_0126;
				}
				if (responseCode == 413L)
				{
					Debug.LogWarning(responseCode.ToString() + ": You had too many events in your request.");
					this.PutDataFromJSONArrayBackToQueue(ref multipleEvents);
					goto IL_0126;
				}
			}
			else
			{
				if (responseCode == 429L)
				{
					this.error429Recieved = true;
					Debug.LogWarning(responseCode.ToString() + ": Too many requests for a device.");
					this.PutDataFromJSONArrayBackToQueue(ref multipleEvents);
					base.StartCoroutine(this.ResetError429Blockade());
					goto IL_0126;
				}
				long num = responseCode - 500L;
				if (num <= 4L)
				{
					switch ((uint)num)
					{
					case 0U:
					case 2U:
					case 4U:
						Debug.LogWarning(responseCode.ToString() + ": We encountered an error while handling the request.");
						this.PutDataFromJSONArrayBackToQueue(ref multipleEvents);
						goto IL_0126;
					case 3U:
						Debug.LogWarning(responseCode.ToString() + ": Server error.");
						this.PutDataFromJSONArrayBackToQueue(ref multipleEvents);
						goto IL_0126;
					}
				}
			}
			Debug.LogWarning(responseCode.ToString() + ": undefined case - using default option (puting data back to the events queue)");
			this.PutDataFromJSONArrayBackToQueue(ref multipleEvents);
			IL_0126:
			if (Time.time - this.countingErrorsTimestamp <= 120f)
			{
				this.errorCounter++;
			}
			else
			{
				this.errorCounter = 0;
			}
			if (this.errorCounter >= 5)
			{
				Debug.LogWarning("Maximum number of errors recieved in the small time window. Shutting down analytics.");
				this.sendData = false;
				return;
			}
			this.errorReported = true;
			base.StartCoroutine(this.ResetErrorBlockade());
			this.countingErrorsTimestamp = Time.time;
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x0012FE04 File Offset: 0x0012E004
		private void PutDataFromJSONArrayBackToQueue(ref JSONArray multipleEvents)
		{
			foreach (object obj in multipleEvents)
			{
				JSONObject jsonobject = (JSONObject)obj;
				this.eventsQueue.Enqueue(jsonobject);
			}
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x00047F9D File Offset: 0x0004619D
		private IEnumerator ResetError429Blockade()
		{
			yield return new WaitForSeconds(30f);
			this.error429Recieved = false;
			yield break;
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x00047FAC File Offset: 0x000461AC
		private IEnumerator ResetErrorBlockade()
		{
			yield return new WaitForSeconds(15f);
			this.errorReported = false;
			yield break;
		}

		// Token: 0x040021BD RID: 8637
		private const string AMPLITUDE_ADDRESS = "https://api.amplitude.com/httpapi";

		// Token: 0x040021BE RID: 8638
		private const string CLIENT_UPLOAD_TIME = "client_upload_time";

		// Token: 0x040021BF RID: 8639
		private const string EVENT = "event";

		// Token: 0x040021C0 RID: 8640
		private const string API_KEY = "api_key";

		// Token: 0x040021C1 RID: 8641
		private const int MAX_EVENTS_PER_REQUST = 5;

		// Token: 0x040021C2 RID: 8642
		private const float TOO_MANY_REQUEST_PAUSE_PERIOD = 30f;

		// Token: 0x040021C3 RID: 8643
		private const int MAX_ERROR_COUNT = 5;

		// Token: 0x040021C4 RID: 8644
		private const float MAX_TIME_BEFORE_RESETING_EVENT_SENDING = 15f;

		// Token: 0x040021C5 RID: 8645
		private const float ERROR_COUNTING_TIME_WINDOW = 120f;

		// Token: 0x040021C6 RID: 8646
		private float countingErrorsTimestamp;

		// Token: 0x040021C7 RID: 8647
		private int errorCounter;

		// Token: 0x040021C8 RID: 8648
		private bool errorReported;

		// Token: 0x040021C9 RID: 8649
		private bool error429Recieved;

		// Token: 0x040021CA RID: 8650
		private Queue<JSONObject> eventsQueue = new Queue<JSONObject>();

		// Token: 0x040021CB RID: 8651
		private bool sendData = true;

		// Token: 0x040021CC RID: 8652
		private bool printDebugData;

		// Token: 0x040021CD RID: 8653
		private Coroutine obtainIPCoroutine;

		// Token: 0x040021CE RID: 8654
		private WebClient webClient = new WebClient();

		// Token: 0x040021CF RID: 8655
		private bool ipUpdateNeeded;

		// Token: 0x040021D0 RID: 8656
		private WaitForSeconds wait10Seconds = new WaitForSeconds(10f);

		// Token: 0x040021D1 RID: 8657
		private WaitForSeconds wait15Seconds = new WaitForSeconds(15f);

		// Token: 0x040021D2 RID: 8658
		private IDictionary<string, object> userProperties = new Dictionary<string, object>();

		// Token: 0x040021D3 RID: 8659
		private const string USER_PROPERTIES = "user_properties";

		// Token: 0x040021D4 RID: 8660
		private const string EVENT_PROPERTIES = "event_properties";
	}
}

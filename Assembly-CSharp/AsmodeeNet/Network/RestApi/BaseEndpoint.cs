using System;
using System.Collections;
using System.Text;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using BestHTTP;
using MiniJSON;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000903 RID: 2307
	public abstract class BaseEndpoint
	{
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06003EBD RID: 16061 RVA: 0x000505F3 File Offset: 0x0004E7F3
		// (set) Token: 0x06003EBE RID: 16062 RVA: 0x000505FB File Offset: 0x0004E7FB
		public string DebugModuleName { get; protected set; }

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06003EBF RID: 16063 RVA: 0x00050604 File Offset: 0x0004E804
		public int GetHTTPResponseStatus
		{
			get
			{
				if (this._HTTPResponse != null)
				{
					return this._HTTPResponse.StatusCode;
				}
				return -1;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06003EC0 RID: 16064 RVA: 0x0005061B File Offset: 0x0004E81B
		public int ApiResponseStatus
		{
			get
			{
				if (this._HTTPResponse != null)
				{
					return this._HTTPResponse.StatusCode;
				}
				return 0;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06003EC1 RID: 16065 RVA: 0x00050632 File Offset: 0x0004E832
		// (set) Token: 0x06003EC2 RID: 16066 RVA: 0x0005063A File Offset: 0x0004E83A
		public bool SilentFailure { get; set; }

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06003EC3 RID: 16067 RVA: 0x00050643 File Offset: 0x0004E843
		// (set) Token: 0x06003EC4 RID: 16068 RVA: 0x0005064B File Offset: 0x0004E84B
		private protected bool _MustUsePrivateScope { protected get; private set; }

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06003EC5 RID: 16069 RVA: 0x00050654 File Offset: 0x0004E854
		// (set) Token: 0x06003EC6 RID: 16070 RVA: 0x0005065C File Offset: 0x0004E85C
		protected OAuthGate _OAuthGate { get; set; }

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06003EC7 RID: 16071 RVA: 0x00050665 File Offset: 0x0004E865
		// (set) Token: 0x06003EC8 RID: 16072 RVA: 0x0005066D File Offset: 0x0004E86D
		protected NetworkParameters _NetworkParameters { get; set; }

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06003EC9 RID: 16073 RVA: 0x00050676 File Offset: 0x0004E876
		// (set) Token: 0x06003ECA RID: 16074 RVA: 0x0005067E File Offset: 0x0004E87E
		protected string _URL { get; set; }

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06003ECB RID: 16075 RVA: 0x00050687 File Offset: 0x0004E887
		// (set) Token: 0x06003ECC RID: 16076 RVA: 0x0005068F File Offset: 0x0004E88F
		protected Hashtable _Parameters { get; set; }

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06003ECD RID: 16077 RVA: 0x00050698 File Offset: 0x0004E898
		// (set) Token: 0x06003ECE RID: 16078 RVA: 0x000506A0 File Offset: 0x0004E8A0
		protected HTTPMethods _HttpMethod { get; set; }

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06003ECF RID: 16079 RVA: 0x000506A9 File Offset: 0x0004E8A9
		// (set) Token: 0x06003ED0 RID: 16080 RVA: 0x000506B1 File Offset: 0x0004E8B1
		protected HTTPRequest _HTTPRequest { get; set; }

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06003ED1 RID: 16081 RVA: 0x000506BA File Offset: 0x0004E8BA
		// (set) Token: 0x06003ED2 RID: 16082 RVA: 0x000506C2 File Offset: 0x0004E8C2
		protected HTTPResponse _HTTPResponse { get; set; }

		// Token: 0x06003ED3 RID: 16083 RVA: 0x00159D20 File Offset: 0x00157F20
		public BaseEndpoint(bool mustUsePrivateScope, OAuthGate oauthGate)
		{
			this.SilentFailure = false;
			this._OAuthGate = ((oauthGate == null && CoreApplication.Instance != null) ? CoreApplication.Instance.OAuthGate : oauthGate);
			if (this._OAuthGate == null)
			{
				throw new ArgumentException("'oauthGate' must not be null");
			}
			if (this._OAuthGate.NetworkParameters == null)
			{
				throw new ArgumentException("'networkParameters' must not be null");
			}
			this.DebugModuleName = "Endpoint";
			this._MustUsePrivateScope = mustUsePrivateScope;
			this._NetworkParameters = this._OAuthGate.NetworkParameters;
			this._URL = null;
			this._Parameters = null;
			this._HttpMethod = HTTPMethods.Get;
			this._HTTPRequest = null;
			this._HTTPResponse = null;
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x000506CB File Offset: 0x0004E8CB
		public bool Succeeded()
		{
			return this._HTTPResponse != null && this._HTTPResponse.StatusCode >= 200 && this._HTTPResponse.StatusCode < 300;
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x00159DD4 File Offset: 0x00157FD4
		protected Hashtable _GetLogDetails()
		{
			string text = "";
			if (!this._URL.StartsWith("http"))
			{
				text += this._NetworkParameters.GetApiBaseUrl();
			}
			text += this._URL;
			return new Hashtable
			{
				{ "url", text },
				{
					"method",
					this._HttpMethod.ToString().ToUpper()
				}
			};
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x00159E50 File Offset: 0x00158050
		protected Hashtable _GetCopyOfParameters()
		{
			if (this._Parameters == null)
			{
				return new Hashtable();
			}
			Hashtable hashtable = this._Parameters.Clone() as Hashtable;
			hashtable.Remove("password");
			hashtable.Remove("access_token");
			hashtable.Remove("client_secret");
			return hashtable;
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x00159E9C File Offset: 0x0015809C
		protected void _ExecuteCore()
		{
			if (!string.IsNullOrEmpty(this._OAuthGate.AccessToken))
			{
				this._HTTPRequest.AddHeader("Authorization", "bearer " + this._OAuthGate.AccessToken);
			}
			this._HTTPRequest.AddHeader("User-Agent", CoreApplication.GetUserAgent());
			if (this._HttpMethod == HTTPMethods.Post || this._HttpMethod == HTTPMethods.Put || this._HttpMethod == HTTPMethods.Delete || this._HttpMethod == HTTPMethods.Patch)
			{
				this._HTTPRequest.AddHeader("Cache-Control", "no-cache");
				if (this._usePutAsPatch && this._HttpMethod == HTTPMethods.Put)
				{
					this._HTTPRequest.AddHeader("Content-Type", "application/prs.dow-online-features-patch+json");
				}
				else
				{
					this._HTTPRequest.AddHeader("Content-Type", "application/json; charset=UTF-8");
				}
				this._SetRequestParameters();
			}
			this._HTTPRequest.CustomCertificateVerifyer = new CertificateVerifier(this._NetworkParameters.RestAPIPinPublicKeys);
			this._HTTPRequest.UseAlternateSSL = true;
			this._LogOnSending();
			this._HTTPRequest.Send();
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x00159FAC File Offset: 0x001581AC
		protected void _LogOnSending()
		{
			Hashtable hashtable = this._GetLogDetails();
			Hashtable hashtable2 = this._GetCopyOfParameters();
			hashtable.Add("parameters", hashtable2);
			AsmoLogger.Info(this.DebugModuleName + ".sender", "Sending Request", hashtable);
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x00159FF0 File Offset: 0x001581F0
		public void Abort()
		{
			if (this._getTokenCallbackId != null)
			{
				this._OAuthGate.CancelAccessTokenRequest(this._getTokenCallbackId.Value);
				this._getTokenCallbackId = null;
			}
			if (this._HTTPRequest != null && (this._HTTPRequest.State == HTTPRequestStates.Queued || this._HTTPRequest.State == HTTPRequestStates.Processing))
			{
				this._HTTPRequest.Abort();
			}
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x0015A05C File Offset: 0x0015825C
		protected void _LogOnCompletion(WebError error)
		{
			Hashtable hashtable = this._GetLogDetails();
			hashtable.Add("http_status", (this._HTTPResponse == null) ? "null" : this._HTTPResponse.StatusCode.ToString());
			if (this.Succeeded())
			{
				hashtable.Add("result", this._HTTPResponse.DataAsText);
				AsmoLogger.Info(this.DebugModuleName + ".receiver", "Request Success", hashtable);
				return;
			}
			hashtable.Add("error", error.ToString());
			AsmoLogger.Error(this.DebugModuleName + ".receiver", "Request Failure", hashtable);
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x000506FB File Offset: 0x0004E8FB
		protected virtual ApiResponseError _ParseError()
		{
			return JsonUtility.FromJson<ApiResponseError>(this._HTTPResponse.DataAsText);
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x0015A104 File Offset: 0x00158304
		protected virtual void _SetRequestParameters()
		{
			if (this._Parameters != null)
			{
				string text = Json.Serialize(this._Parameters);
				this._HTTPRequest.RawData = Encoding.UTF8.GetBytes(text);
			}
		}

		// Token: 0x04003041 RID: 12353
		public const string kRequestFailureError = "request_failure";

		// Token: 0x04003045 RID: 12357
		protected bool _usePutAsPatch;

		// Token: 0x0400304D RID: 12365
		protected int? _getTokenCallbackId;
	}
}

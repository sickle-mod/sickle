using System;
using System.Collections;
using AsmodeeNet.Utils;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000904 RID: 2308
	public abstract class Endpoint : BaseEndpoint
	{
		// Token: 0x06003EDD RID: 16093 RVA: 0x0005070D File Offset: 0x0004E90D
		public Endpoint(bool mustUsePrivateScope, OAuthGate oauthGate)
			: base(mustUsePrivateScope, oauthGate)
		{
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x0015A13C File Offset: 0x0015833C
		public void Execute(EndpointCallback onCompletion)
		{
			OAuthCallback oauthCallback = delegate(OAuthError authError)
			{
				this._getTokenCallbackId = null;
				if (authError != null)
				{
					if (onCompletion != null)
					{
						onCompletion(authError);
					}
					return;
				}
				this._Execute(this._NetworkParameters.GetApiBaseUrl() + this._URL, onCompletion);
			};
			if (base._MustUsePrivateScope)
			{
				this._getTokenCallbackId = base._OAuthGate.GetPrivateAccessToken(base.SilentFailure, oauthCallback);
				return;
			}
			this._getTokenCallbackId = base._OAuthGate.GetPublicAccessToken(oauthCallback);
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x0015A19C File Offset: 0x0015839C
		private void _Execute(string fullUrl, EndpointCallback onCompletion)
		{
			base._HTTPRequest = new HTTPRequest(new Uri(fullUrl), base._HttpMethod, delegate(HTTPRequest req, HTTPResponse response)
			{
				if (response != null)
				{
					this._HTTPResponse = response;
					WebError webError = null;
					if (!this.Succeeded())
					{
						if (string.IsNullOrEmpty(response.DataAsText))
						{
							webError = new WebError("request_failure", response.StatusCode);
						}
						else
						{
							ApiResponseError apiResponseError = this._ParseError();
							if (string.IsNullOrEmpty(apiResponseError.error))
							{
								webError = new WebError("request_failure", response.StatusCode);
							}
							else
							{
								webError = apiResponseError;
							}
						}
					}
					this._LogOnCompletion(webError);
					if (onCompletion != null)
					{
						onCompletion(webError);
					}
					return;
				}
				Hashtable hashtable = this._GetLogDetails();
				Hashtable hashtable2 = this._GetCopyOfParameters();
				hashtable.Add("parameters", hashtable2);
				if (req.State == HTTPRequestStates.Aborted)
				{
					AsmoLogger.Error(this.DebugModuleName + ".receiver", "Aborting", hashtable);
					return;
				}
				AsmoLogger.Error(this.DebugModuleName + ".receiver", "No response", hashtable);
				if (onCompletion != null)
				{
					onCompletion(WebError.MakeNoResponseError());
				}
			});
			base._ExecuteCore();
		}
	}
}

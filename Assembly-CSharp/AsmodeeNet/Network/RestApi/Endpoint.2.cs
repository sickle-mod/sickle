using System;
using System.Collections;
using AsmodeeNet.Utils;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000907 RID: 2311
	public abstract class Endpoint<ResultType> : BaseEndpoint where ResultType : class
	{
		// Token: 0x06003EE4 RID: 16100 RVA: 0x0005070D File Offset: 0x0004E90D
		public Endpoint(bool mustUsePrivateScope, OAuthGate oauthGate)
			: base(mustUsePrivateScope, oauthGate)
		{
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x0015A374 File Offset: 0x00158574
		public void Execute(Action<ResultType, WebError> onCompletion)
		{
			OAuthCallback oauthCallback = delegate(OAuthError authError)
			{
				this._getTokenCallbackId = null;
				if (authError != null)
				{
					if (onCompletion != null)
					{
						onCompletion(default(ResultType), authError);
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

		// Token: 0x06003EE6 RID: 16102 RVA: 0x0015A3D4 File Offset: 0x001585D4
		protected void _Execute(string fullUrl, Action<ResultType, WebError> onCompletion)
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
						this._LogOnCompletion(webError);
						if (onCompletion != null)
						{
							onCompletion(default(ResultType), webError);
							return;
						}
					}
					else
					{
						this._LogOnCompletion(webError);
						this.ProcessResponse(onCompletion);
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
					onCompletion(default(ResultType), WebError.MakeNoResponseError());
				}
			});
			base._ExecuteCore();
		}

		// Token: 0x06003EE7 RID: 16103
		protected abstract void ProcessResponse(Action<ResultType, WebError> onCompletion);
	}
}

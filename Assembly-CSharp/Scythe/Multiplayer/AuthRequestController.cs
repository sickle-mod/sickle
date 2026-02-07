using System;
using System.Text;
using BestHTTP;
using Multiplayer.AuthApi;
using Newtonsoft.Json;
using Scythe.Multiplayer.AuthApi.Models;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer
{
	// Token: 0x02000206 RID: 518
	public class AuthRequestController
	{
		// Token: 0x06000F79 RID: 3961 RVA: 0x0008D380 File Offset: 0x0008B580
		public static HTTPRequest RequestGetCall<TSuccess, TFailure>(string endpoint, Action<TSuccess> onSuccess, Action<TFailure> onFailure, Action<Exception> onError = null)
		{
			HTTPRequest httprequest = new HTTPRequest(AuthRequestController.GetApiUri(endpoint), HTTPMethods.Get, delegate(HTTPRequest req, HTTPResponse resp)
			{
				AuthRequestController.HandleResponse<TSuccess, TFailure>(onSuccess, onFailure, onError, req, resp);
			});
			httprequest.Send();
			AuthRequestController.AddAuthorizationToken(httprequest);
			return httprequest;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0008D3D0 File Offset: 0x0008B5D0
		public static HTTPRequest RequestPostCall<TSuccess, TFailure>(string endpoint, string message, Action<TSuccess> onSuccess, Action<TFailure> onFailure, Action<Exception> onError = null)
		{
			HTTPRequest httprequest = new HTTPRequest(AuthRequestController.GetApiUri(endpoint), HTTPMethods.Post, delegate(HTTPRequest req, HTTPResponse resp)
			{
				AuthRequestController.HandleResponse<TSuccess, TFailure>(onSuccess, onFailure, onError, req, resp);
			});
			AuthRequestController.AddAuthorizationToken(httprequest);
			httprequest.AddHeader("Content-Type", "application/json; charset=UTF-8");
			httprequest.RawData = Encoding.UTF8.GetBytes(message);
			httprequest.Send();
			return httprequest;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0008D440 File Offset: 0x0008B640
		public static HTTPRequest RequestPostCallWithAuthorization<TSuccess, TFailure>(string endpoint, string message, Action<TSuccess> onSuccess, Action<TFailure> onFailure, Action<Exception> onError = null)
		{
			HTTPRequest httprequest = new HTTPRequest(AuthRequestController.GetApiUri(endpoint), HTTPMethods.Post, delegate(HTTPRequest req, HTTPResponse resp)
			{
				AuthRequestController.HandleResponse<TSuccess, TFailure>(onSuccess, onFailure, onError, req, resp);
			});
			AuthRequestController.AddAuthorizationToken(httprequest);
			httprequest.AddHeader("Content-Type", "application/json; charset=UTF-8");
			httprequest.RawData = Encoding.UTF8.GetBytes(message);
			httprequest.Send();
			return httprequest;
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0008D4B0 File Offset: 0x0008B6B0
		public static HTTPRequest RequestDeleteCall<TSuccess, TFailure>(string endpoint, Action<TSuccess> onSuccess, Action<TFailure> onFailure, Action<Exception> onError = null)
		{
			HTTPRequest httprequest = new HTTPRequest(AuthRequestController.GetApiUri(endpoint), HTTPMethods.Delete, delegate(HTTPRequest req, HTTPResponse resp)
			{
				AuthRequestController.HandleResponse<TSuccess, TFailure>(onSuccess, onFailure, onError, req, resp);
			});
			httprequest.Send();
			return httprequest;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0008D4F8 File Offset: 0x0008B6F8
		private static void HandleResponse<TSuccess, TFailure>(Action<TSuccess> onSuccess, Action<TFailure> onFailure, Action<Exception> onError, HTTPRequest req, HTTPResponse resp)
		{
			if (AuthRequestController.RequestHasError(req))
			{
				DebugLog.LogError(req.Exception.Message);
				if (onError != null)
				{
					onError(req.Exception);
				}
				return;
			}
			if (AuthRequestController.RequestHasFailed(resp.StatusCode))
			{
				TFailure tfailure = (TFailure)((object)FailureProcessor.GetResponse(JsonConvert.DeserializeObject(resp.DataAsText, AuthRequestController.GetFailureType(resp.StatusCode))));
				if (onFailure != null)
				{
					onFailure(tfailure);
					return;
				}
			}
			else
			{
				TSuccess tsuccess = JsonConvert.DeserializeObject<TSuccess>(resp.DataAsText);
				if (onSuccess != null)
				{
					onSuccess(tsuccess);
				}
			}
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0003203B File Offset: 0x0003023B
		private static bool RequestHasError(HTTPRequest req)
		{
			return req.Exception != null;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00032046 File Offset: 0x00030246
		private static bool RequestHasFailed(int statusCode)
		{
			return statusCode != 200;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00032053 File Offset: 0x00030253
		private static Uri GetApiUri(string endpoint)
		{
			return new Uri(ServerEndpoints.AuthApiEndpoint + endpoint);
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x00032065 File Offset: 0x00030265
		private static void AddAuthorizationToken(HTTPRequest request)
		{
			if (!string.IsNullOrEmpty(PlayerInfo.me.Token))
			{
				request.AddHeader("AuthToken", PlayerInfo.me.Token);
			}
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x0003208D File Offset: 0x0003028D
		private static Type GetFailureType(int statusCode)
		{
			if (statusCode == 401)
			{
				return typeof(UnauthorizedResponse);
			}
			if (statusCode != 500)
			{
				return typeof(ErrorResponse);
			}
			return typeof(ServerErrorResponse);
		}
	}
}

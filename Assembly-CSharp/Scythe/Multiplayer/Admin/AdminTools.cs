using System;
using System.Text;
using System.Xml;
using BestHTTP;

namespace Scythe.Multiplayer.Admin
{
	// Token: 0x02000385 RID: 901
	public static class AdminTools
	{
		// Token: 0x06001A16 RID: 6678 RVA: 0x00039564 File Offset: 0x00037764
		public static void ClearStagingCache(Action<string> onSuccess, Action<Exception> onError = null)
		{
			AdminTools.RequestDeleteCall("AdminService.svc/ClearStagingCache", string.Empty, onSuccess, onError);
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x000A352C File Offset: 0x000A172C
		private static HTTPRequest RequestDeleteCall(string endpoint, string message, Action<string> onSuccess, Action<Exception> onError = null)
		{
			HTTPRequest httprequest = new HTTPRequest(AdminTools.GetApiUri(endpoint), HTTPMethods.Delete, delegate(HTTPRequest req, HTTPResponse resp)
			{
				AdminTools.HandleResponse(onSuccess, onError, req, resp);
			});
			httprequest.AddHeader("Content-Type", "application/xml; charset=UTF-8");
			httprequest.RawData = Encoding.UTF8.GetBytes(message);
			httprequest.Send();
			return httprequest;
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x00039578 File Offset: 0x00037778
		private static Uri GetApiUri(string endpoint)
		{
			return new Uri(string.Format("{0}{1}", "https://scythetesting.cloudapp.net/Services/", endpoint));
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x000A3590 File Offset: 0x000A1790
		private static void HandleResponse(Action<string> onSuccess, Action<Exception> onError, HTTPRequest req, HTTPResponse resp)
		{
			if (req.Exception != null && onError != null)
			{
				onError(req.Exception);
				return;
			}
			if (resp == null)
			{
				string text = "No response from ";
				Uri currentUri = req.CurrentUri;
				DebugLog.LogError(text + ((currentUri != null) ? currentUri.ToString() : null));
				if (onError != null)
				{
					onError(new Exception(string.Empty));
				}
				return;
			}
			if (resp.StatusCode == 200)
			{
				onSuccess(resp.DataAsText);
				return;
			}
			Exception ex;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(resp.DataAsText);
				if (resp.StatusCode == 408)
				{
					ex = new TimeoutException(xmlDocument.FirstChild.InnerText);
				}
				else
				{
					ex = new Exception(xmlDocument.FirstChild.InnerText);
				}
			}
			catch (XmlException)
			{
				ex = new Exception(resp.DataAsText);
			}
			if (onError != null)
			{
				onError(ex);
				return;
			}
			DebugLog.LogError(ex.Message);
		}

		// Token: 0x040012BF RID: 4799
		private const string ENDPOINT = "https://scythetesting.cloudapp.net/Services/";

		// Token: 0x040012C0 RID: 4800
		private const string ADMIN_SERVICE = "AdminService.svc/";

		// Token: 0x040012C1 RID: 4801
		private const string CLEAR_STAGING_ENDPOINT = "ClearStagingCache";
	}
}

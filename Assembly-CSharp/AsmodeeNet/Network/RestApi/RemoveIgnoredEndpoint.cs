using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000916 RID: 2326
	public class RemoveIgnoredEndpoint : Endpoint
	{
		// Token: 0x06003F0B RID: 16139 RVA: 0x0015ACB8 File Offset: 0x00158EB8
		public RemoveIgnoredEndpoint(int ignoredId, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.Ignore";
			base._URL = string.Format("/main/v1/user/me/ignore/{0}", ignoredId);
			base._HttpMethod = HTTPMethods.Delete;
			base._Parameters = new Hashtable { { "ignoredId", ignoredId } };
		}
	}
}

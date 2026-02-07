using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000913 RID: 2323
	public class AddIgnoredEndpoint : Endpoint
	{
		// Token: 0x06003F05 RID: 16133 RVA: 0x0015AAF4 File Offset: 0x00158CF4
		public AddIgnoredEndpoint(int ignoredId, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.Ignore";
			base._URL = string.Format("/main/v1/user/me/ignore/{0}", ignoredId);
			base._HttpMethod = HTTPMethods.Post;
			base._Parameters = new Hashtable { { "ignoredId", ignoredId } };
		}
	}
}

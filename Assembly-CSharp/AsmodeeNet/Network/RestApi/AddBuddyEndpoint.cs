using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000884 RID: 2180
	public class AddBuddyEndpoint : Endpoint
	{
		// Token: 0x06003D03 RID: 15619 RVA: 0x001575F8 File Offset: 0x001557F8
		public AddBuddyEndpoint(int buddyId, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.Buddies";
			base._URL = string.Format("/main/v1/user/me/buddies/{0}", buddyId);
			base._HttpMethod = HTTPMethods.Post;
			base._Parameters = new Hashtable { { "buddyId", buddyId } };
		}
	}
}

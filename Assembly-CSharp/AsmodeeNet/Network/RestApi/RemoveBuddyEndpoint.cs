using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000887 RID: 2183
	public class RemoveBuddyEndpoint : Endpoint
	{
		// Token: 0x06003D09 RID: 15625 RVA: 0x001577BC File Offset: 0x001559BC
		public RemoveBuddyEndpoint(int buddyId, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.Buddies";
			base._URL = string.Format("/main/v1/user/me/buddies/{0}", buddyId);
			base._HttpMethod = HTTPMethods.Delete;
			base._Parameters = new Hashtable { { "buddyId", buddyId } };
		}
	}
}

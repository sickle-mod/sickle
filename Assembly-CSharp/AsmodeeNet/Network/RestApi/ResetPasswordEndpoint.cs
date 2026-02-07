using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200093C RID: 2364
	public class ResetPasswordEndpoint : Endpoint
	{
		// Token: 0x06003F76 RID: 16246 RVA: 0x0015C5C4 File Offset: 0x0015A7C4
		public ResetPasswordEndpoint(int userId, OAuthGate oauthGate = null)
			: base(false, oauthGate)
		{
			base.DebugModuleName += ".User.ResetPassword";
			base._URL = string.Format("/main/v1/user/{0}/password", userId);
			base._Parameters = new Hashtable();
			base._HttpMethod = HTTPMethods.Delete;
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x00050B5F File Offset: 0x0004ED5F
		public ResetPasswordEndpoint(OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.ResetPassword";
			base._URL = "/main/v1/user/me/password";
			base._Parameters = new Hashtable();
			base._HttpMethod = HTTPMethods.Delete;
		}
	}
}

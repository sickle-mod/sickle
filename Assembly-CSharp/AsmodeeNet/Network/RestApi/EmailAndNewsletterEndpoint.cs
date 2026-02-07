using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000936 RID: 2358
	public abstract class EmailAndNewsletterEndpoint : Endpoint
	{
		// Token: 0x06003F6C RID: 16236 RVA: 0x0015C3C8 File Offset: 0x0015A5C8
		public EmailAndNewsletterEndpoint(string email, bool? receiveNewsletter, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.Mail";
			base._URL = "/main/v1/user/me/email";
			base._HttpMethod = HTTPMethods.Put;
			base._Parameters = new Hashtable();
			if (email != null)
			{
				base._Parameters.Add("email", email);
			}
			if (receiveNewsletter != null)
			{
				base._Parameters.Add("newsletter", receiveNewsletter);
			}
		}
	}
}

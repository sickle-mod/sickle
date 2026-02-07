using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000938 RID: 2360
	public class UpdateNewsletterEndpoint : EmailAndNewsletterEndpoint
	{
		// Token: 0x06003F6E RID: 16238 RVA: 0x00050AE5 File Offset: 0x0004ECE5
		public UpdateNewsletterEndpoint(bool receiveNewsletter, OAuthGate oauthGate = null)
			: base(null, new bool?(receiveNewsletter), oauthGate)
		{
		}
	}
}

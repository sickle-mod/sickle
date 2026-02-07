using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000939 RID: 2361
	public class UpdateEmailAndNewsletterEndpoint : EmailAndNewsletterEndpoint
	{
		// Token: 0x06003F6F RID: 16239 RVA: 0x00050AF5 File Offset: 0x0004ECF5
		public UpdateEmailAndNewsletterEndpoint(string email, bool receiveNewsletter, OAuthGate oauthGate = null)
			: base(email, new bool?(receiveNewsletter), oauthGate)
		{
			if (email == null)
			{
				throw new ArgumentException("\"email\" parameter cannot be null");
			}
		}
	}
}

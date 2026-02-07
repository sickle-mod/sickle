using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000937 RID: 2359
	public class UpdateEmailEndpoint : EmailAndNewsletterEndpoint
	{
		// Token: 0x06003F6D RID: 16237 RVA: 0x0015C444 File Offset: 0x0015A644
		public UpdateEmailEndpoint(string email, OAuthGate oauthGate = null)
			: base(email, null, oauthGate)
		{
			if (email == null)
			{
				throw new ArgumentException("\"email\" parameter cannot be null");
			}
		}
	}
}

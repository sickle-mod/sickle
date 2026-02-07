using System;

namespace AsmodeeNet.Network
{
	// Token: 0x0200086A RID: 2154
	[Serializable]
	public class OAuthError : WebError
	{
		// Token: 0x06003C7F RID: 15487 RVA: 0x0004F28A File Offset: 0x0004D48A
		public OAuthError()
		{
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x0004F292 File Offset: 0x0004D492
		public OAuthError(string errorName, string errorDescription, int status = -1)
			: base(errorName, status)
		{
			this.error_description = errorDescription;
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x0004F2A3 File Offset: 0x0004D4A3
		public new static OAuthError MakeNoResponseError()
		{
			return new OAuthError("no_response", "There was no response", -1);
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x0004F2B5 File Offset: 0x0004D4B5
		public new static OAuthError MakeTimeoutError()
		{
			return new OAuthError("timeout", "The request timed out", -1);
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x0004F2C7 File Offset: 0x0004D4C7
		public new static OAuthError MakePublicKeyPinningError()
		{
			return new OAuthError("public_key_pinning_error", "The signature of the server's SSL certificate doesn't match expectations", -1);
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x0004F2D9 File Offset: 0x0004D4D9
		public static OAuthError MakeSilentAuthError()
		{
			return new OAuthError("no_private_token", "The OAuthGate required a private token but silently failed.", -1);
		}

		// Token: 0x04002DDC RID: 11740
		public string error_description;
	}
}

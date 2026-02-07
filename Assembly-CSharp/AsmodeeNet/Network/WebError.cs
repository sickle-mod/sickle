using System;

namespace AsmodeeNet.Network
{
	// Token: 0x0200087B RID: 2171
	public class WebError
	{
		// Token: 0x06003CE5 RID: 15589 RVA: 0x0004F60C File Offset: 0x0004D80C
		protected WebError()
		{
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06003CE6 RID: 15590 RVA: 0x0004F626 File Offset: 0x0004D826
		public OAuthError GetOAuthError
		{
			get
			{
				return this as OAuthError;
			}
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x0004F62E File Offset: 0x0004D82E
		public WebError(string error, int status = -1)
		{
			this.error = error;
			this.status = status;
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x0004F656 File Offset: 0x0004D856
		public T ToChildError<T>() where T : WebError
		{
			return this as T;
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x0004F663 File Offset: 0x0004D863
		public static WebError MakeNoResponseError()
		{
			return new WebError("no_response", -1);
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x0004F670 File Offset: 0x0004D870
		public static WebError MakeTimeoutError()
		{
			return new WebError("timeout", -1);
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x0004F67D File Offset: 0x0004D87D
		public static WebError MakePublicKeyPinningError()
		{
			return new WebError("public_key_pinning_error", -1);
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x0004F68A File Offset: 0x0004D88A
		public override string ToString()
		{
			return string.Format("[{0}] {1}", this.status, this.error);
		}

		// Token: 0x04002E0E RID: 11790
		public const string kNoResponseError = "no_response";

		// Token: 0x04002E0F RID: 11791
		public const string kTimeoutError = "timeout";

		// Token: 0x04002E10 RID: 11792
		public const string kPublicKeyPinningError = "public_key_pinning_error";

		// Token: 0x04002E11 RID: 11793
		public string error = "";

		// Token: 0x04002E12 RID: 11794
		public int status = -1;
	}
}

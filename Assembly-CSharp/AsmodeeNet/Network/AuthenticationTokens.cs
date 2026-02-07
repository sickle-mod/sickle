using System;

namespace AsmodeeNet.Network
{
	// Token: 0x02000867 RID: 2151
	[Serializable]
	public class AuthenticationTokens
	{
		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06003C72 RID: 15474 RVA: 0x00155818 File Offset: 0x00153A18
		public bool HasExpired
		{
			get
			{
				return (this.expirationDate - DateTime.Now).TotalSeconds < 0.0;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06003C73 RID: 15475 RVA: 0x0004F191 File Offset: 0x0004D391
		public bool HasPublicToken
		{
			get
			{
				return !this.HasExpired && this.scope.Contains("public");
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06003C74 RID: 15476 RVA: 0x0004F1AD File Offset: 0x0004D3AD
		public bool HasPrivateToken
		{
			get
			{
				return !this.HasExpired && this.scope.Contains("private");
			}
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x00155848 File Offset: 0x00153A48
		public void InitExpiration()
		{
			this.expirationDate = DateTime.Now.AddSeconds((double)this.expires_in);
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x0004F1C9 File Offset: 0x0004D3C9
		public AuthenticationTokens()
		{
			this.InitExpiration();
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x0004F1E2 File Offset: 0x0004D3E2
		public AuthenticationTokens(string accessToken, string refreshToken)
		{
			this.InitExpiration();
			this.access_token = accessToken;
			this.refresh_token = refreshToken;
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x0004F209 File Offset: 0x0004D409
		public AuthenticationTokens(string accessToken, string refreshToken, int expiresIn, string scope)
		{
			this.access_token = accessToken;
			this.refresh_token = refreshToken;
			this.expires_in = expiresIn;
			this.scope = scope;
			this.InitExpiration();
		}

		// Token: 0x04002DCA RID: 11722
		public string access_token;

		// Token: 0x04002DCB RID: 11723
		public string refresh_token;

		// Token: 0x04002DCC RID: 11724
		public int expires_in;

		// Token: 0x04002DCD RID: 11725
		public string scope = "";

		// Token: 0x04002DCE RID: 11726
		private DateTime expirationDate;
	}
}

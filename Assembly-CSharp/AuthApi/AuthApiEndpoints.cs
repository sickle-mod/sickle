using System;

namespace AuthApi
{
	// Token: 0x020001C6 RID: 454
	public class AuthApiEndpoints
	{
		// Token: 0x06000D58 RID: 3416 RVA: 0x00030DB6 File Offset: 0x0002EFB6
		public AuthApiEndpoints()
		{
			this._get = new GET("https://auth.scythedigitaledition.com/api");
			this._post = new POST("https://auth.scythedigitaledition.com/api");
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x00030DDE File Offset: 0x0002EFDE
		// (set) Token: 0x06000D5A RID: 3418 RVA: 0x00030DE6 File Offset: 0x0002EFE6
		private GET _get { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000D5B RID: 3419 RVA: 0x00030DEF File Offset: 0x0002EFEF
		// (set) Token: 0x06000D5C RID: 3420 RVA: 0x00030DF7 File Offset: 0x0002EFF7
		private POST _post { get; set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000D5D RID: 3421 RVA: 0x00030E00 File Offset: 0x0002F000
		public GET GET
		{
			get
			{
				return this._get;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00030E08 File Offset: 0x0002F008
		public POST POST
		{
			get
			{
				return this._post;
			}
		}

		// Token: 0x04000AB4 RID: 2740
		public const string AuthApiBaseUrl = "https://auth.scythedigitaledition.com/api";
	}
}

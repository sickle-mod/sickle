using System;

namespace AuthApi
{
	// Token: 0x020001C9 RID: 457
	public class POST : EndpointCollection
	{
		// Token: 0x06000D64 RID: 3428 RVA: 0x00030E30 File Offset: 0x0002F030
		public POST(string baseUrl)
			: base(baseUrl)
		{
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000D65 RID: 3429 RVA: 0x00030E4C File Offset: 0x0002F04C
		public string Login
		{
			get
			{
				return base._baseUrl + "/Account/Login";
			}
		}
	}
}

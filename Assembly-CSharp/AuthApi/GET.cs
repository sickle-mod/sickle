using System;

namespace AuthApi
{
	// Token: 0x020001C8 RID: 456
	public class GET : EndpointCollection
	{
		// Token: 0x06000D62 RID: 3426 RVA: 0x00030E30 File Offset: 0x0002F030
		public GET(string baseUrl)
			: base(baseUrl)
		{
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00030E39 File Offset: 0x0002F039
		public string IsTokenValid(string token)
		{
			return base._baseUrl + "/Account/TokenValidate?token=" + token;
		}
	}
}

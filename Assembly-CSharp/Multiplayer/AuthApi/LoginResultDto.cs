using System;
using Scythe.Multiplayer.AuthApi;

namespace Multiplayer.AuthApi
{
	// Token: 0x020001A7 RID: 423
	[Serializable]
	public class LoginResultDto : BasicApiResultDto
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x00030460 File Offset: 0x0002E660
		// (set) Token: 0x06000C63 RID: 3171 RVA: 0x00030468 File Offset: 0x0002E668
		public bool IsSuccessful { get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x00030471 File Offset: 0x0002E671
		// (set) Token: 0x06000C65 RID: 3173 RVA: 0x00030479 File Offset: 0x0002E679
		public string AccessToken { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x00030482 File Offset: 0x0002E682
		// (set) Token: 0x06000C67 RID: 3175 RVA: 0x0003048A File Offset: 0x0002E68A
		public string RefreshToken { get; set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x00030493 File Offset: 0x0002E693
		// (set) Token: 0x06000C69 RID: 3177 RVA: 0x0003049B File Offset: 0x0002E69B
		public PlatformAccount[] LinkedAccounts { get; set; }
	}
}

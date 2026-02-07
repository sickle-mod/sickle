using System;
using Scythe.Multiplayer.AuthApi.Models;

namespace Multiplayer.AuthApi
{
	// Token: 0x020001A8 RID: 424
	[Serializable]
	public class PlatformAccount
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x000304AC File Offset: 0x0002E6AC
		// (set) Token: 0x06000C6C RID: 3180 RVA: 0x000304B4 File Offset: 0x0002E6B4
		public LoginPlatform Platform { get; set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x000304BD File Offset: 0x0002E6BD
		// (set) Token: 0x06000C6E RID: 3182 RVA: 0x000304C5 File Offset: 0x0002E6C5
		public string PlatformUserId { get; set; }
	}
}

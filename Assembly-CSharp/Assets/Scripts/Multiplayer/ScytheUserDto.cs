using System;
using Scythe.Multiplayer.AuthApi.Models;

namespace Assets.Scripts.Multiplayer
{
	// Token: 0x020001AB RID: 427
	[Serializable]
	public class ScytheUserDto
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x000304F0 File Offset: 0x0002E6F0
		// (set) Token: 0x06000C78 RID: 3192 RVA: 0x000304F8 File Offset: 0x0002E6F8
		public string Login { get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x00030501 File Offset: 0x0002E701
		// (set) Token: 0x06000C7A RID: 3194 RVA: 0x00030509 File Offset: 0x0002E709
		public string Password { get; set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x00030512 File Offset: 0x0002E712
		// (set) Token: 0x06000C7C RID: 3196 RVA: 0x0003051A File Offset: 0x0002E71A
		public string Email { get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x00030523 File Offset: 0x0002E723
		// (set) Token: 0x06000C7E RID: 3198 RVA: 0x0003052B File Offset: 0x0002E72B
		public LoginPlatform Platform { get; set; }
	}
}

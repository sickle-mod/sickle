using System;

namespace Assets.Scripts.Multiplayer
{
	// Token: 0x020001AA RID: 426
	[Serializable]
	public class PasswordResetDto
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x000304CE File Offset: 0x0002E6CE
		// (set) Token: 0x06000C73 RID: 3187 RVA: 0x000304D6 File Offset: 0x0002E6D6
		public string NewPassword { get; set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x000304DF File Offset: 0x0002E6DF
		// (set) Token: 0x06000C75 RID: 3189 RVA: 0x000304E7 File Offset: 0x0002E6E7
		public string Email { get; set; }
	}
}

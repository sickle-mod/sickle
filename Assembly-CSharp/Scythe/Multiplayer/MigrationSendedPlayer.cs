using System;

namespace Scythe.Multiplayer
{
	// Token: 0x0200027E RID: 638
	public struct MigrationSendedPlayer
	{
		// Token: 0x06001420 RID: 5152 RVA: 0x000358B8 File Offset: 0x00033AB8
		public MigrationSendedPlayer(string login, string newPassword, string oldEmail)
		{
			this.login = login;
			this.newPassword = newPassword;
			this.oldEmail = oldEmail;
		}

		// Token: 0x04000ECF RID: 3791
		public string login;

		// Token: 0x04000ED0 RID: 3792
		public string newPassword;

		// Token: 0x04000ED1 RID: 3793
		public string oldEmail;
	}
}

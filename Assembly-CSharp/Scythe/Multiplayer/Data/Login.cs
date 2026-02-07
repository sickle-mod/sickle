using System;
using Scythe.Multiplayer.AuthApi.Models;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000336 RID: 822
	public class Login
	{
		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06001791 RID: 6033 RVA: 0x0003813E File Offset: 0x0003633E
		// (set) Token: 0x06001792 RID: 6034 RVA: 0x00038146 File Offset: 0x00036346
		public Guid PlayerGuid { get; set; }

		// Token: 0x06001793 RID: 6035 RVA: 0x00027E56 File Offset: 0x00026056
		public Login()
		{
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x0003814F File Offset: 0x0003634F
		public Login(string name, string gameVersion, string email, bool ifaDlc, string thirdPartyPlayerId, LoginPlatform loginPlatform, string currentLanguage)
		{
			this.Name = name;
			this.GameVersion = gameVersion;
			this.Email = email;
			this.IfaDlc = ifaDlc;
			this.ThirdPartyPlayerId = thirdPartyPlayerId;
			this.LoginPlatform = (int)loginPlatform;
			this.CurrentLanguage = currentLanguage;
		}

		// Token: 0x04001150 RID: 4432
		public string Name;

		// Token: 0x04001151 RID: 4433
		public string GameVersion;

		// Token: 0x04001152 RID: 4434
		public string Email;

		// Token: 0x04001153 RID: 4435
		public bool IfaDlc;

		// Token: 0x04001154 RID: 4436
		public int LoginPlatform;

		// Token: 0x04001155 RID: 4437
		public string ThirdPartyPlayerId;

		// Token: 0x04001156 RID: 4438
		public string CurrentLanguage;
	}
}

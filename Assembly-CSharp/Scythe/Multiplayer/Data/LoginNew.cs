using System;
using Scythe.Multiplayer.AuthApi.Models;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000335 RID: 821
	public class LoginNew
	{
		// Token: 0x0600178F RID: 6031 RVA: 0x00027E56 File Offset: 0x00026056
		public LoginNew()
		{
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x00038109 File Offset: 0x00036309
		public LoginNew(string token, string gameVersion, bool ifaDlc, string thirdPartyPlayerId, LoginPlatform loginPlatform, string currentLanguage)
		{
			this.Token = token;
			this.GameVersion = gameVersion;
			this.IfaDlc = ifaDlc;
			this.ThirdPartyPlayerId = thirdPartyPlayerId;
			this.LoginPlatform = (int)loginPlatform;
			this.CurrentLanguage = currentLanguage;
		}

		// Token: 0x0400114A RID: 4426
		public string Token;

		// Token: 0x0400114B RID: 4427
		public string GameVersion;

		// Token: 0x0400114C RID: 4428
		public bool IfaDlc;

		// Token: 0x0400114D RID: 4429
		public int LoginPlatform;

		// Token: 0x0400114E RID: 4430
		public string ThirdPartyPlayerId;

		// Token: 0x0400114F RID: 4431
		public string CurrentLanguage;
	}
}

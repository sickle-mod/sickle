using System;
using Scythe.Multiplayer.AuthApi.Models;

namespace Assets.Scripts.LocalStore.Model
{
	// Token: 0x020001B3 RID: 435
	public class LocalLoggedUserData
	{
		// Token: 0x06000CA5 RID: 3237 RVA: 0x00080F84 File Offset: 0x0007F184
		public LocalLoggedUserData(LoginResponse loginResult, Guid? id, string token, string email, string login, string encryptedToken)
		{
			this.Id = id ?? Guid.Empty;
			this.Token = token;
			this.Email = email;
			this.Login = login;
			this.RefreshToken = encryptedToken;
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x0003063C File Offset: 0x0002E83C
		// (set) Token: 0x06000CA7 RID: 3239 RVA: 0x00030644 File Offset: 0x0002E844
		public Guid Id { get; set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x0003064D File Offset: 0x0002E84D
		// (set) Token: 0x06000CA9 RID: 3241 RVA: 0x00030655 File Offset: 0x0002E855
		public string Token { get; set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x0003065E File Offset: 0x0002E85E
		// (set) Token: 0x06000CAB RID: 3243 RVA: 0x00030666 File Offset: 0x0002E866
		public string Email { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x0003066F File Offset: 0x0002E86F
		// (set) Token: 0x06000CAD RID: 3245 RVA: 0x00030677 File Offset: 0x0002E877
		public string Login { get; set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x00030680 File Offset: 0x0002E880
		// (set) Token: 0x06000CAF RID: 3247 RVA: 0x00030688 File Offset: 0x0002E888
		public string RefreshToken { get; set; }
	}
}

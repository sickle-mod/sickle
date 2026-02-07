using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000344 RID: 836
	public class RegisterData
	{
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060017F5 RID: 6133 RVA: 0x0003842F File Offset: 0x0003662F
		// (set) Token: 0x060017F6 RID: 6134 RVA: 0x00038437 File Offset: 0x00036637
		public string Email { get; set; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060017F7 RID: 6135 RVA: 0x00038440 File Offset: 0x00036640
		// (set) Token: 0x060017F8 RID: 6136 RVA: 0x00038448 File Offset: 0x00036648
		public string Name { get; set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060017F9 RID: 6137 RVA: 0x00038451 File Offset: 0x00036651
		// (set) Token: 0x060017FA RID: 6138 RVA: 0x00038459 File Offset: 0x00036659
		public string Password { get; set; }

		// Token: 0x060017FB RID: 6139 RVA: 0x00027E56 File Offset: 0x00026056
		public RegisterData()
		{
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00038462 File Offset: 0x00036662
		public RegisterData(string nickname, string password, string email)
		{
			this.Name = nickname;
			this.Password = password;
			this.Email = email;
		}
	}
}

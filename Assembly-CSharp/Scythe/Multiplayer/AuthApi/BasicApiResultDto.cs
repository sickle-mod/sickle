using System;

namespace Scythe.Multiplayer.AuthApi
{
	// Token: 0x02000353 RID: 851
	[Serializable]
	public class BasicApiResultDto
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600185F RID: 6239 RVA: 0x000387CB File Offset: 0x000369CB
		// (set) Token: 0x06001860 RID: 6240 RVA: 0x000387D3 File Offset: 0x000369D3
		public bool IsSuccesful { get; set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06001861 RID: 6241 RVA: 0x000387DC File Offset: 0x000369DC
		// (set) Token: 0x06001862 RID: 6242 RVA: 0x000387E4 File Offset: 0x000369E4
		public Guid Id { get; set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06001863 RID: 6243 RVA: 0x000387ED File Offset: 0x000369ED
		// (set) Token: 0x06001864 RID: 6244 RVA: 0x000387F5 File Offset: 0x000369F5
		public string Message { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06001865 RID: 6245 RVA: 0x000387FE File Offset: 0x000369FE
		// (set) Token: 0x06001866 RID: 6246 RVA: 0x00038806 File Offset: 0x00036A06
		public string Token { get; set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06001867 RID: 6247 RVA: 0x0003880F File Offset: 0x00036A0F
		// (set) Token: 0x06001868 RID: 6248 RVA: 0x00038817 File Offset: 0x00036A17
		public string Login { get; set; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06001869 RID: 6249 RVA: 0x00038820 File Offset: 0x00036A20
		// (set) Token: 0x0600186A RID: 6250 RVA: 0x00038828 File Offset: 0x00036A28
		public string Email { get; set; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600186B RID: 6251 RVA: 0x00038831 File Offset: 0x00036A31
		// (set) Token: 0x0600186C RID: 6252 RVA: 0x00038839 File Offset: 0x00036A39
		public bool TransferNeeded { get; set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600186D RID: 6253 RVA: 0x00038842 File Offset: 0x00036A42
		// (set) Token: 0x0600186E RID: 6254 RVA: 0x0003884A File Offset: 0x00036A4A
		public Errors errors { get; set; }
	}
}

using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000312 RID: 786
	public class GameToSpectate
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060016CE RID: 5838 RVA: 0x000379D1 File Offset: 0x00035BD1
		// (set) Token: 0x060016CF RID: 5839 RVA: 0x000379D9 File Offset: 0x00035BD9
		public string Name { get; set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060016D0 RID: 5840 RVA: 0x000379E2 File Offset: 0x00035BE2
		// (set) Token: 0x060016D1 RID: 5841 RVA: 0x000379EA File Offset: 0x00035BEA
		public string Id { get; set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060016D2 RID: 5842 RVA: 0x000379F3 File Offset: 0x00035BF3
		// (set) Token: 0x060016D3 RID: 5843 RVA: 0x000379FB File Offset: 0x00035BFB
		public string PlayersAmount { get; set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060016D4 RID: 5844 RVA: 0x00037A04 File Offset: 0x00035C04
		// (set) Token: 0x060016D5 RID: 5845 RVA: 0x00037A0C File Offset: 0x00035C0C
		public string Turn { get; set; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060016D6 RID: 5846 RVA: 0x00037A15 File Offset: 0x00035C15
		// (set) Token: 0x060016D7 RID: 5847 RVA: 0x00037A1D File Offset: 0x00035C1D
		public string StartTime { get; set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060016D8 RID: 5848 RVA: 0x00037A26 File Offset: 0x00035C26
		// (set) Token: 0x060016D9 RID: 5849 RVA: 0x00037A2E File Offset: 0x00035C2E
		public string Type { get; set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060016DA RID: 5850 RVA: 0x00037A37 File Offset: 0x00035C37
		// (set) Token: 0x060016DB RID: 5851 RVA: 0x00037A3F File Offset: 0x00035C3F
		public bool Ranked { get; set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060016DC RID: 5852 RVA: 0x00037A48 File Offset: 0x00035C48
		// (set) Token: 0x060016DD RID: 5853 RVA: 0x00037A50 File Offset: 0x00035C50
		public bool InvadersFromAfar { get; set; }
	}
}

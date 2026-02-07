using System;
using Newtonsoft.Json;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200033F RID: 831
	[JsonObject]
	public class RankingStatByTopScore
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060017E0 RID: 6112 RVA: 0x000383A7 File Offset: 0x000365A7
		// (set) Token: 0x060017E1 RID: 6113 RVA: 0x000383AF File Offset: 0x000365AF
		public long RankingPosition { get; set; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060017E2 RID: 6114 RVA: 0x000383B8 File Offset: 0x000365B8
		// (set) Token: 0x060017E3 RID: 6115 RVA: 0x000383C0 File Offset: 0x000365C0
		public int Id { get; set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060017E4 RID: 6116 RVA: 0x000383C9 File Offset: 0x000365C9
		// (set) Token: 0x060017E5 RID: 6117 RVA: 0x000383D1 File Offset: 0x000365D1
		public int TopScore { get; set; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060017E6 RID: 6118 RVA: 0x000383DA File Offset: 0x000365DA
		// (set) Token: 0x060017E7 RID: 6119 RVA: 0x000383E2 File Offset: 0x000365E2
		public string Name { get; set; }
	}
}

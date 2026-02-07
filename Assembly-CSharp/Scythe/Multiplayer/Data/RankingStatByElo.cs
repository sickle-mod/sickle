using System;
using Newtonsoft.Json;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200033D RID: 829
	[JsonObject]
	public class RankingStatByElo
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x0003831F File Offset: 0x0003651F
		// (set) Token: 0x060017CF RID: 6095 RVA: 0x00038327 File Offset: 0x00036527
		public long RankingPosition { get; set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060017D0 RID: 6096 RVA: 0x00038330 File Offset: 0x00036530
		// (set) Token: 0x060017D1 RID: 6097 RVA: 0x00038338 File Offset: 0x00036538
		public int Id { get; set; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060017D2 RID: 6098 RVA: 0x00038341 File Offset: 0x00036541
		// (set) Token: 0x060017D3 RID: 6099 RVA: 0x00038349 File Offset: 0x00036549
		public int ELO { get; set; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060017D4 RID: 6100 RVA: 0x00038352 File Offset: 0x00036552
		// (set) Token: 0x060017D5 RID: 6101 RVA: 0x0003835A File Offset: 0x0003655A
		public string Name { get; set; }
	}
}

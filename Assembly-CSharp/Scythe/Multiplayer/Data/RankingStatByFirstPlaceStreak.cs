using System;
using Newtonsoft.Json;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200033E RID: 830
	[JsonObject]
	public class RankingStatByFirstPlaceStreak
	{
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060017D7 RID: 6103 RVA: 0x00038363 File Offset: 0x00036563
		// (set) Token: 0x060017D8 RID: 6104 RVA: 0x0003836B File Offset: 0x0003656B
		public long RankingPosition { get; set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060017D9 RID: 6105 RVA: 0x00038374 File Offset: 0x00036574
		// (set) Token: 0x060017DA RID: 6106 RVA: 0x0003837C File Offset: 0x0003657C
		public int Id { get; set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060017DB RID: 6107 RVA: 0x00038385 File Offset: 0x00036585
		// (set) Token: 0x060017DC RID: 6108 RVA: 0x0003838D File Offset: 0x0003658D
		public int FirstPlaceStreak { get; set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060017DD RID: 6109 RVA: 0x00038396 File Offset: 0x00036596
		// (set) Token: 0x060017DE RID: 6110 RVA: 0x0003839E File Offset: 0x0003659E
		public string Name { get; set; }
	}
}

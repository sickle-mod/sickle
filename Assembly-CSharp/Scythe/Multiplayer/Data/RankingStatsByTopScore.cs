using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000343 RID: 835
	[JsonObject]
	public class RankingStatsByTopScore
	{
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060017F2 RID: 6130 RVA: 0x0003841E File Offset: 0x0003661E
		// (set) Token: 0x060017F3 RID: 6131 RVA: 0x00038426 File Offset: 0x00036626
		public List<RankingStatByTopScore> RankingStatsByTopScoreList { get; set; }
	}
}

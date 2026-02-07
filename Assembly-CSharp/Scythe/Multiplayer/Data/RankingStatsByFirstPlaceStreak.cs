using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000342 RID: 834
	[JsonObject]
	public class RankingStatsByFirstPlaceStreak
	{
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060017EF RID: 6127 RVA: 0x0003840D File Offset: 0x0003660D
		// (set) Token: 0x060017F0 RID: 6128 RVA: 0x00038415 File Offset: 0x00036615
		public List<RankingStatByFirstPlaceStreak> RankingStatsByFirstPlaceStreakList { get; set; }
	}
}

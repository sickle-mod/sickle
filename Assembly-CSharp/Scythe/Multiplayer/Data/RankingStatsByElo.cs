using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000341 RID: 833
	[JsonObject]
	public class RankingStatsByElo
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060017EC RID: 6124 RVA: 0x000383FC File Offset: 0x000365FC
		// (set) Token: 0x060017ED RID: 6125 RVA: 0x00038404 File Offset: 0x00036604
		public List<RankingStatByElo> RankingStatsByEloList { get; set; }
	}
}

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000340 RID: 832
	[JsonObject]
	public class RankingStats
	{
		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060017E9 RID: 6121 RVA: 0x000383EB File Offset: 0x000365EB
		// (set) Token: 0x060017EA RID: 6122 RVA: 0x000383F3 File Offset: 0x000365F3
		public List<RankingStat> RankingStatsList { get; set; }
	}
}

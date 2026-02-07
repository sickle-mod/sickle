using System;
using Newtonsoft.Json;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200033C RID: 828
	[JsonObject]
	public class RankingStat
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060017BF RID: 6079 RVA: 0x000382A8 File Offset: 0x000364A8
		// (set) Token: 0x060017C0 RID: 6080 RVA: 0x000382B0 File Offset: 0x000364B0
		public long RankingPosition { get; set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x000382B9 File Offset: 0x000364B9
		// (set) Token: 0x060017C2 RID: 6082 RVA: 0x000382C1 File Offset: 0x000364C1
		public int Id { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060017C3 RID: 6083 RVA: 0x000382CA File Offset: 0x000364CA
		// (set) Token: 0x060017C4 RID: 6084 RVA: 0x000382D2 File Offset: 0x000364D2
		public int ELO { get; set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060017C5 RID: 6085 RVA: 0x000382DB File Offset: 0x000364DB
		// (set) Token: 0x060017C6 RID: 6086 RVA: 0x000382E3 File Offset: 0x000364E3
		public int TopScore { get; set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060017C7 RID: 6087 RVA: 0x000382EC File Offset: 0x000364EC
		// (set) Token: 0x060017C8 RID: 6088 RVA: 0x000382F4 File Offset: 0x000364F4
		public int FirstPlaceStreak { get; set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060017C9 RID: 6089 RVA: 0x000382FD File Offset: 0x000364FD
		// (set) Token: 0x060017CA RID: 6090 RVA: 0x00038305 File Offset: 0x00036505
		public int Karma { get; set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060017CB RID: 6091 RVA: 0x0003830E File Offset: 0x0003650E
		// (set) Token: 0x060017CC RID: 6092 RVA: 0x00038316 File Offset: 0x00036516
		public string Name { get; set; }
	}
}

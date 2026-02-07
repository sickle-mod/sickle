using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200033B RID: 827
	[Serializable]
	public class PlayerStatsOld : MonoBehaviour
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060017BC RID: 6076 RVA: 0x00038297 File Offset: 0x00036497
		// (set) Token: 0x060017BD RID: 6077 RVA: 0x0003829F File Offset: 0x0003649F
		public string GamesHistory { get; set; }

		// Token: 0x040011B6 RID: 4534
		public int Id;

		// Token: 0x040011B7 RID: 4535
		public string Name;

		// Token: 0x040011B8 RID: 4536
		public int ELO;

		// Token: 0x040011B9 RID: 4537
		public int Karma;

		// Token: 0x040011BA RID: 4538
		public long RankingPosition;

		// Token: 0x040011BB RID: 4539
		public int RankedGames;

		// Token: 0x040011BC RID: 4540
		public int RankedTopScore;

		// Token: 0x040011BD RID: 4541
		public int RankedFirstPlaceStreak;

		// Token: 0x040011BE RID: 4542
		public List<int> ELOList;

		// Token: 0x040011BF RID: 4543
		public List<int> GamesList;

		// Token: 0x040011C0 RID: 4544
		public PlayerFactionStats[][] PlayerFactionStats;

		// Token: 0x040011C1 RID: 4545
		public string ELOListString;

		// Token: 0x040011C2 RID: 4546
		public string GamesListString;

		// Token: 0x040011C3 RID: 4547
		public List<string> PlayerFactionStatsString;
	}
}

using System;
using System.Collections.Generic;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000349 RID: 841
	public class GameInit
	{
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06001818 RID: 6168 RVA: 0x00038569 File Offset: 0x00036769
		// (set) Token: 0x06001819 RID: 6169 RVA: 0x00038571 File Offset: 0x00036771
		public List<PlayerInit> Players { get; set; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600181A RID: 6170 RVA: 0x0003857A File Offset: 0x0003677A
		// (set) Token: 0x0600181B RID: 6171 RVA: 0x00038582 File Offset: 0x00036782
		public int NumberOfPlayers { get; set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600181C RID: 6172 RVA: 0x0003858B File Offset: 0x0003678B
		// (set) Token: 0x0600181D RID: 6173 RVA: 0x00038593 File Offset: 0x00036793
		public int StructureBonusCard { get; set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600181E RID: 6174 RVA: 0x0003859C File Offset: 0x0003679C
		// (set) Token: 0x0600181F RID: 6175 RVA: 0x000385A4 File Offset: 0x000367A4
		public int PlayerClockTime { get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06001820 RID: 6176 RVA: 0x000385AD File Offset: 0x000367AD
		// (set) Token: 0x06001821 RID: 6177 RVA: 0x000385B5 File Offset: 0x000367B5
		public bool IsRanked { get; set; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06001822 RID: 6178 RVA: 0x000385BE File Offset: 0x000367BE
		// (set) Token: 0x06001823 RID: 6179 RVA: 0x000385C6 File Offset: 0x000367C6
		public bool IsAsynchronous { get; set; }
	}
}

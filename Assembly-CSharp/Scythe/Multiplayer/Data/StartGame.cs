using System;
using System.Collections.Generic;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000350 RID: 848
	public class StartGame : Data
	{
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600184D RID: 6221 RVA: 0x00038728 File Offset: 0x00036928
		// (set) Token: 0x0600184E RID: 6222 RVA: 0x00038730 File Offset: 0x00036930
		public List<int> Slots { get; set; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x0600184F RID: 6223 RVA: 0x00038739 File Offset: 0x00036939
		// (set) Token: 0x06001850 RID: 6224 RVA: 0x00038741 File Offset: 0x00036941
		public List<int> Factions { get; set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06001851 RID: 6225 RVA: 0x0003874A File Offset: 0x0003694A
		// (set) Token: 0x06001852 RID: 6226 RVA: 0x00038752 File Offset: 0x00036952
		public List<int> PlayerMats { get; set; }

		// Token: 0x06001853 RID: 6227 RVA: 0x00037A7B File Offset: 0x00035C7B
		public StartGame()
		{
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x0003875B File Offset: 0x0003695B
		public StartGame(List<int> slots, List<int> factions, List<int> playerMats)
		{
			this.Slots = new List<int>(slots);
			this.Factions = new List<int>(factions);
			this.PlayerMats = new List<int>(playerMats);
		}
	}
}

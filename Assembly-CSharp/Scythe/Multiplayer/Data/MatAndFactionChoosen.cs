using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200034C RID: 844
	public class MatAndFactionChoosen : Data
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06001830 RID: 6192 RVA: 0x00038622 File Offset: 0x00036822
		// (set) Token: 0x06001831 RID: 6193 RVA: 0x0003862A File Offset: 0x0003682A
		public int Faction { get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06001832 RID: 6194 RVA: 0x00038633 File Offset: 0x00036833
		// (set) Token: 0x06001833 RID: 6195 RVA: 0x0003863B File Offset: 0x0003683B
		public int PlayerMat { get; set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06001834 RID: 6196 RVA: 0x00038644 File Offset: 0x00036844
		// (set) Token: 0x06001835 RID: 6197 RVA: 0x0003864C File Offset: 0x0003684C
		public int Slot { get; set; }

		// Token: 0x06001836 RID: 6198 RVA: 0x00037A7B File Offset: 0x00035C7B
		public MatAndFactionChoosen()
		{
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x00038655 File Offset: 0x00036855
		public MatAndFactionChoosen(int faction, int playerMat, int slot)
		{
			this.Faction = faction;
			this.PlayerMat = playerMat;
			this.Slot = slot;
		}
	}
}

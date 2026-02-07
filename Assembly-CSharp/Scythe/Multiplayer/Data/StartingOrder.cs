using System;
using System.Collections.Generic;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000331 RID: 817
	public class StartingOrder : Data
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600177B RID: 6011 RVA: 0x00037FE0 File Offset: 0x000361E0
		// (set) Token: 0x0600177C RID: 6012 RVA: 0x00037FE8 File Offset: 0x000361E8
		public List<int> OldSlots { get; set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600177D RID: 6013 RVA: 0x00037FF1 File Offset: 0x000361F1
		// (set) Token: 0x0600177E RID: 6014 RVA: 0x00037FF9 File Offset: 0x000361F9
		public List<int> NewSlots { get; set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600177F RID: 6015 RVA: 0x00038002 File Offset: 0x00036202
		// (set) Token: 0x06001780 RID: 6016 RVA: 0x0003800A File Offset: 0x0003620A
		public List<Guid> Ids { get; set; }

		// Token: 0x06001781 RID: 6017 RVA: 0x00038013 File Offset: 0x00036213
		public StartingOrder()
		{
			this.OldSlots = new List<int>();
			this.NewSlots = new List<int>();
			this.Ids = new List<Guid>();
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x0003803C File Offset: 0x0003623C
		public StartingOrder(IEnumerable<int> oldSlots, IEnumerable<int> newSlots, IEnumerable<Guid> ids)
		{
			this.OldSlots = new List<int>(oldSlots);
			this.NewSlots = new List<int>(newSlots);
			this.Ids = new List<Guid>(ids);
		}
	}
}

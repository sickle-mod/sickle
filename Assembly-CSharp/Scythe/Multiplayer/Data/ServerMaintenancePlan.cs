using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000352 RID: 850
	[Serializable]
	public class ServerMaintenancePlan
	{
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06001858 RID: 6232 RVA: 0x00038798 File Offset: 0x00036998
		// (set) Token: 0x06001859 RID: 6233 RVA: 0x000387A0 File Offset: 0x000369A0
		public bool IsPlanned { get; set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600185A RID: 6234 RVA: 0x000387A9 File Offset: 0x000369A9
		// (set) Token: 0x0600185B RID: 6235 RVA: 0x000387B1 File Offset: 0x000369B1
		public DateTime StartDateTime { get; set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600185C RID: 6236 RVA: 0x000387BA File Offset: 0x000369BA
		// (set) Token: 0x0600185D RID: 6237 RVA: 0x000387C2 File Offset: 0x000369C2
		public DateTime EndDateTime { get; set; }
	}
}

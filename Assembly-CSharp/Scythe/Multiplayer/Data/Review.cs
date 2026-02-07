using System;
using UnityEngine;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000347 RID: 839
	public class Review
	{
		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06001800 RID: 6144 RVA: 0x00038490 File Offset: 0x00036690
		// (set) Token: 0x06001801 RID: 6145 RVA: 0x00038498 File Offset: 0x00036698
		public string ReviewerId { get; set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06001802 RID: 6146 RVA: 0x000384A1 File Offset: 0x000366A1
		// (set) Token: 0x06001803 RID: 6147 RVA: 0x000384A9 File Offset: 0x000366A9
		public int StarsAmount { get; set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06001804 RID: 6148 RVA: 0x000384B2 File Offset: 0x000366B2
		// (set) Token: 0x06001805 RID: 6149 RVA: 0x000384BA File Offset: 0x000366BA
		public string Feedback { get; set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06001806 RID: 6150 RVA: 0x000384C3 File Offset: 0x000366C3
		// (set) Token: 0x06001807 RID: 6151 RVA: 0x000384CB File Offset: 0x000366CB
		public string Version { get; set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06001808 RID: 6152 RVA: 0x000384D4 File Offset: 0x000366D4
		// (set) Token: 0x06001809 RID: 6153 RVA: 0x000384DC File Offset: 0x000366DC
		public string ReviewerName { get; set; }

		// Token: 0x0600180A RID: 6154 RVA: 0x00027E56 File Offset: 0x00026056
		public Review()
		{
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x000A017C File Offset: 0x0009E37C
		public Review(int starsAmount, string feedback)
		{
			this.ReviewerId = GameServiceController.Instance.PlayerId();
			this.StarsAmount = starsAmount;
			this.Feedback = feedback;
			this.Version = Application.version;
			this.ReviewerName = GameServiceController.Instance.DisplayName();
		}
	}
}

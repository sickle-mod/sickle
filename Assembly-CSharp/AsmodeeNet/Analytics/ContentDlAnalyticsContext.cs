using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009D2 RID: 2514
	public class ContentDlAnalyticsContext : AnalyticsContext
	{
		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x060041DB RID: 16859 RVA: 0x00052338 File Offset: 0x00050538
		// (set) Token: 0x060041DC RID: 16860 RVA: 0x00052340 File Offset: 0x00050540
		public string DlSessionId { get; private set; }

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x060041DD RID: 16861 RVA: 0x00052349 File Offset: 0x00050549
		// (set) Token: 0x060041DE RID: 16862 RVA: 0x00052351 File Offset: 0x00050551
		public string DlContentId { get; private set; }

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x060041DF RID: 16863 RVA: 0x0005235A File Offset: 0x0005055A
		public int DlTime
		{
			get
			{
				return (int)(Time.unscaledTime - this._startDlTime);
			}
		}

		// Token: 0x060041E0 RID: 16864 RVA: 0x0016289C File Offset: 0x00160A9C
		public ContentDlAnalyticsContext(string dlContentId)
		{
			this.DlSessionId = Guid.NewGuid().ToString();
			this.DlContentId = dlContentId;
			this._startDlTime = Time.unscaledTime;
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x00052369 File Offset: 0x00050569
		public override void Resume()
		{
			base.Resume();
			this._startDlTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x040032F0 RID: 13040
		private float _startDlTime;
	}
}

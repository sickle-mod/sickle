using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009DD RID: 2525
	public class ShopAnalyticsContext : AnalyticsContext
	{
		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x0600423E RID: 16958 RVA: 0x00052674 File Offset: 0x00050874
		// (set) Token: 0x0600423F RID: 16959 RVA: 0x0005267C File Offset: 0x0005087C
		public string ShopSessionId { get; private set; }

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06004240 RID: 16960 RVA: 0x00052685 File Offset: 0x00050885
		// (set) Token: 0x06004241 RID: 16961 RVA: 0x0005268D File Offset: 0x0005088D
		public string EntryPoint { get; private set; }

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06004242 RID: 16962 RVA: 0x00052696 File Offset: 0x00050896
		// (set) Token: 0x06004243 RID: 16963 RVA: 0x0005269E File Offset: 0x0005089E
		public string DefaultItemId { get; private set; }

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06004244 RID: 16964 RVA: 0x000526A7 File Offset: 0x000508A7
		public int TimeActiveSec
		{
			get
			{
				return (int)(Time.unscaledTime - this._startShopTime);
			}
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x00162F58 File Offset: 0x00161158
		public ShopAnalyticsContext(string entryPoint, string itemId)
		{
			this.ShopSessionId = Guid.NewGuid().ToString();
			this.EntryPoint = entryPoint;
			this.DefaultItemId = itemId;
			this._startShopTime = Time.unscaledTime;
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x000526B6 File Offset: 0x000508B6
		public override void Resume()
		{
			base.Resume();
			this._startShopTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x04003321 RID: 13089
		public bool DidPurchase;

		// Token: 0x04003322 RID: 13090
		private float _startShopTime;
	}
}

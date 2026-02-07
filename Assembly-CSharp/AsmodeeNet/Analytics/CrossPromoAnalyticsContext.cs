using System;
using AsmodeeNet.Network.RestApi;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009D3 RID: 2515
	public class CrossPromoAnalyticsContext : AnalyticsContext
	{
		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x060041E2 RID: 16866 RVA: 0x0005238A File Offset: 0x0005058A
		// (set) Token: 0x060041E3 RID: 16867 RVA: 0x00052392 File Offset: 0x00050592
		public string CrossPromoSessionId { get; private set; }

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x060041E4 RID: 16868 RVA: 0x0005239B File Offset: 0x0005059B
		// (set) Token: 0x060041E5 RID: 16869 RVA: 0x000523A3 File Offset: 0x000505A3
		public string CrossPromoType { get; private set; }

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x060041E6 RID: 16870 RVA: 0x000523AC File Offset: 0x000505AC
		// (set) Token: 0x060041E7 RID: 16871 RVA: 0x000523B4 File Offset: 0x000505B4
		public string MoreGameCategory { get; set; }

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x060041E8 RID: 16872 RVA: 0x001628DC File Offset: 0x00160ADC
		public string ApiVersion
		{
			get
			{
				if (this.CrossPromoType.Equals(CROSSPROMO_OPENED.crosspromo_type.banner.ToString()))
				{
					return RequestBannerEndpoint.GetEndpointVersion().ToString();
				}
				if (this.CrossPromoType.Equals(CROSSPROMO_OPENED.crosspromo_type.interstitial.ToString()))
				{
					return RequestInterstitialEndpoint.GetEndpointVersion().ToString();
				}
				if (this.CrossPromoType.Equals(CROSSPROMO_OPENED.crosspromo_type.more_games.ToString()))
				{
					return RequestGamesEndpoint.GetEndpointVersion().ToString();
				}
				return null;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x060041E9 RID: 16873 RVA: 0x000523BD File Offset: 0x000505BD
		// (set) Token: 0x060041EA RID: 16874 RVA: 0x000523C5 File Offset: 0x000505C5
		public int ScreenCount { get; private set; }

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x060041EB RID: 16875 RVA: 0x000523CE File Offset: 0x000505CE
		// (set) Token: 0x060041EC RID: 16876 RVA: 0x000523D6 File Offset: 0x000505D6
		public string ScreenPrevious { get; private set; }

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x060041ED RID: 16877 RVA: 0x000523DF File Offset: 0x000505DF
		// (set) Token: 0x060041EE RID: 16878 RVA: 0x00162968 File Offset: 0x00160B68
		public string ScreenCurrent
		{
			get
			{
				return this._screenCurrent;
			}
			set
			{
				this.ScreenPrevious = this._screenCurrent;
				this._screenCurrent = value;
				int screenCount = this.ScreenCount;
				this.ScreenCount = screenCount + 1;
				this.ScreenPreviousTime = (int)(Time.unscaledTime - this._startScreenTime);
				this._startScreenTime = Time.unscaledTime;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x060041EF RID: 16879 RVA: 0x000523E7 File Offset: 0x000505E7
		// (set) Token: 0x060041F0 RID: 16880 RVA: 0x000523EF File Offset: 0x000505EF
		public int ScreenPreviousTime { get; private set; }

		// Token: 0x060041F1 RID: 16881 RVA: 0x001629B8 File Offset: 0x00160BB8
		public CrossPromoAnalyticsContext(string crossPromoType)
		{
			this.CrossPromoSessionId = Guid.NewGuid().ToString();
			this.CrossPromoType = crossPromoType;
			this._startScreenTime = Time.unscaledTime;
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x000523F8 File Offset: 0x000505F8
		public override void Resume()
		{
			base.Resume();
			this._startScreenTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x040032F6 RID: 13046
		protected string _screenCurrent;

		// Token: 0x040032F7 RID: 13047
		private float _startScreenTime;

		// Token: 0x040032F9 RID: 13049
		public bool CrossPromoOpenedEventLogged;
	}
}

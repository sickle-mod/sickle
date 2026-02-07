using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009D6 RID: 2518
	public class ScreenDisplayAnalyticsContext : AnalyticsContext
	{
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06004219 RID: 16921 RVA: 0x00052589 File Offset: 0x00050789
		// (set) Token: 0x0600421A RID: 16922 RVA: 0x00052591 File Offset: 0x00050791
		public int ScreenCount { get; private set; }

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x0600421B RID: 16923 RVA: 0x0005259A File Offset: 0x0005079A
		// (set) Token: 0x0600421C RID: 16924 RVA: 0x000525A2 File Offset: 0x000507A2
		public string ScreenPrevious { get; private set; }

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x0600421D RID: 16925 RVA: 0x000525AB File Offset: 0x000507AB
		// (set) Token: 0x0600421E RID: 16926 RVA: 0x000525B3 File Offset: 0x000507B3
		public string Context { get; set; }

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x0600421F RID: 16927 RVA: 0x000525BC File Offset: 0x000507BC
		// (set) Token: 0x06004220 RID: 16928 RVA: 0x00162AC4 File Offset: 0x00160CC4
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
				this.ScreenPreviousTime = new int?((int)(Time.unscaledTime - this._startScreenTime));
				this._startScreenTime = Time.unscaledTime;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06004221 RID: 16929 RVA: 0x000525C4 File Offset: 0x000507C4
		// (set) Token: 0x06004222 RID: 16930 RVA: 0x000525CC File Offset: 0x000507CC
		public int? ScreenPreviousTime { get; private set; }

		// Token: 0x06004223 RID: 16931 RVA: 0x00162B18 File Offset: 0x00160D18
		public ScreenDisplayAnalyticsContext(string screenCurrent)
		{
			this._startScreenTime = Time.unscaledTime;
			this.ScreenCurrent = screenCurrent;
			this.ScreenPreviousTime = null;
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x000525D5 File Offset: 0x000507D5
		public override void Resume()
		{
			base.Resume();
			this._startScreenTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x0400330F RID: 13071
		private string _screenCurrent;

		// Token: 0x04003310 RID: 13072
		private float _startScreenTime;
	}
}

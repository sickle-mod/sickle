using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009DE RID: 2526
	public class TableAnalyticsContext : AnalyticsContext
	{
		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06004247 RID: 16967 RVA: 0x000526D7 File Offset: 0x000508D7
		// (set) Token: 0x06004248 RID: 16968 RVA: 0x000526DF File Offset: 0x000508DF
		public string MatchSessionId { get; private set; }

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06004249 RID: 16969 RVA: 0x000526E8 File Offset: 0x000508E8
		// (set) Token: 0x0600424A RID: 16970 RVA: 0x000526F0 File Offset: 0x000508F0
		public int PlayerClockSec { get; private set; }

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x0600424B RID: 16971 RVA: 0x000526F9 File Offset: 0x000508F9
		// (set) Token: 0x0600424C RID: 16972 RVA: 0x00052701 File Offset: 0x00050901
		public bool IsAsynchronous { get; private set; }

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x0600424D RID: 16973 RVA: 0x0005270A File Offset: 0x0005090A
		// (set) Token: 0x0600424E RID: 16974 RVA: 0x00052712 File Offset: 0x00050912
		public bool IsPrivate { get; private set; }

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x0600424F RID: 16975 RVA: 0x0005271B File Offset: 0x0005091B
		// (set) Token: 0x06004250 RID: 16976 RVA: 0x00052723 File Offset: 0x00050923
		public bool IsRanked { get; private set; }

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06004251 RID: 16977 RVA: 0x0005272C File Offset: 0x0005092C
		// (set) Token: 0x06004252 RID: 16978 RVA: 0x00052734 File Offset: 0x00050934
		public bool IsObservable { get; private set; }

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06004253 RID: 16979 RVA: 0x0005273D File Offset: 0x0005093D
		// (set) Token: 0x06004254 RID: 16980 RVA: 0x00052745 File Offset: 0x00050945
		public TABLE_START.obs_access ObsAccess { get; private set; }

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06004255 RID: 16981 RVA: 0x0005274E File Offset: 0x0005094E
		// (set) Token: 0x06004256 RID: 16982 RVA: 0x00052756 File Offset: 0x00050956
		public bool ObsShowHiddenInfo { get; private set; }

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06004257 RID: 16983 RVA: 0x0005275F File Offset: 0x0005095F
		public int TimeActiveSec
		{
			get
			{
				return (int)(Time.unscaledTime - this._startLobbyTime);
			}
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x00162FA0 File Offset: 0x001611A0
		public TableAnalyticsContext(string matchSessionId, int playerClockSec, bool isAsynchronous, bool isPrivate, bool isRanked, bool isObservable, TABLE_START.obs_access obsAccess, bool obsShowHiddenInfo)
		{
			this.MatchSessionId = matchSessionId;
			this.PlayerClockSec = playerClockSec;
			this.IsAsynchronous = isAsynchronous;
			this.IsPrivate = isPrivate;
			this.IsRanked = isRanked;
			this.IsObservable = isObservable;
			this.ObsAccess = obsAccess;
			this.ObsShowHiddenInfo = obsShowHiddenInfo;
			this._startLobbyTime = Time.unscaledTime;
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x0005276E File Offset: 0x0005096E
		public override void Resume()
		{
			base.Resume();
			this._startLobbyTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x0400332B RID: 13099
		private float _startLobbyTime;
	}
}

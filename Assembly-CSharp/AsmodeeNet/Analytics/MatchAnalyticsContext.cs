using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009D5 RID: 2517
	public class MatchAnalyticsContext : AnalyticsContext
	{
		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x060041F8 RID: 16888 RVA: 0x0005245A File Offset: 0x0005065A
		// (set) Token: 0x060041F9 RID: 16889 RVA: 0x00052462 File Offset: 0x00050662
		public string MatchSessionId { get; private set; }

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060041FA RID: 16890 RVA: 0x0005246B File Offset: 0x0005066B
		// (set) Token: 0x060041FB RID: 16891 RVA: 0x00052473 File Offset: 0x00050673
		public string Mode { get; private set; }

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060041FC RID: 16892 RVA: 0x0005247C File Offset: 0x0005067C
		// (set) Token: 0x060041FD RID: 16893 RVA: 0x00052484 File Offset: 0x00050684
		public string MapId { get; private set; }

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060041FE RID: 16894 RVA: 0x0005248D File Offset: 0x0005068D
		// (set) Token: 0x060041FF RID: 16895 RVA: 0x00052495 File Offset: 0x00050695
		public string ActivatedDlc { get; private set; }

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06004200 RID: 16896 RVA: 0x0005249E File Offset: 0x0005069E
		// (set) Token: 0x06004201 RID: 16897 RVA: 0x000524A6 File Offset: 0x000506A6
		public int? PlayerPlayOrder { get; private set; }

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06004202 RID: 16898 RVA: 0x000524AF File Offset: 0x000506AF
		// (set) Token: 0x06004203 RID: 16899 RVA: 0x000524B7 File Offset: 0x000506B7
		public int? PlayerClockSec { get; private set; }

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06004204 RID: 16900 RVA: 0x000524C0 File Offset: 0x000506C0
		// (set) Token: 0x06004205 RID: 16901 RVA: 0x000524C8 File Offset: 0x000506C8
		public string Difficulty { get; private set; }

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06004206 RID: 16902 RVA: 0x000524D1 File Offset: 0x000506D1
		// (set) Token: 0x06004207 RID: 16903 RVA: 0x000524D9 File Offset: 0x000506D9
		public bool IsOnline { get; private set; }

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06004208 RID: 16904 RVA: 0x000524E2 File Offset: 0x000506E2
		// (set) Token: 0x06004209 RID: 16905 RVA: 0x000524EA File Offset: 0x000506EA
		public bool IsTutorial { get; private set; }

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x0600420A RID: 16906 RVA: 0x000524F3 File Offset: 0x000506F3
		// (set) Token: 0x0600420B RID: 16907 RVA: 0x000524FB File Offset: 0x000506FB
		public bool? IsAsynchronous { get; private set; }

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x0600420C RID: 16908 RVA: 0x00052504 File Offset: 0x00050704
		// (set) Token: 0x0600420D RID: 16909 RVA: 0x0005250C File Offset: 0x0005070C
		public bool? IsPrivate { get; private set; }

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x0600420E RID: 16910 RVA: 0x00052515 File Offset: 0x00050715
		// (set) Token: 0x0600420F RID: 16911 RVA: 0x0005251D File Offset: 0x0005071D
		public bool? IsRanked { get; private set; }

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06004210 RID: 16912 RVA: 0x00052526 File Offset: 0x00050726
		// (set) Token: 0x06004211 RID: 16913 RVA: 0x0005252E File Offset: 0x0005072E
		public bool? IsObservable { get; private set; }

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x00052537 File Offset: 0x00050737
		// (set) Token: 0x06004213 RID: 16915 RVA: 0x0005253F File Offset: 0x0005073F
		public MATCH_START.obs_access? ObsAccess { get; private set; }

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06004214 RID: 16916 RVA: 0x00052548 File Offset: 0x00050748
		// (set) Token: 0x06004215 RID: 16917 RVA: 0x00052550 File Offset: 0x00050750
		public bool? ObsShowHiddenInfo { get; private set; }

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06004216 RID: 16918 RVA: 0x00052559 File Offset: 0x00050759
		public int TimeActiveSec
		{
			get
			{
				return (int)(Time.unscaledTime - this._startLobbyTime);
			}
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x00162A30 File Offset: 0x00160C30
		public MatchAnalyticsContext(string matchSessionId, string mode, string mapId, string activatedDlc, int? playerPlayOrder, int? playerClockSec, string difficulty, bool isOnline, bool isTutorial, bool? isAsynchronous, bool? isPrivate, bool? isRanked, bool? isObservable, MATCH_START.obs_access? obsAccess, bool? obsShowHiddenInfo)
		{
			this.MatchSessionId = matchSessionId;
			this.Mode = mode;
			this.MapId = mapId;
			this.ActivatedDlc = activatedDlc;
			this.PlayerPlayOrder = playerPlayOrder;
			this.PlayerClockSec = playerClockSec;
			this.Difficulty = difficulty;
			this.IsOnline = isOnline;
			this.IsTutorial = isTutorial;
			this.IsAsynchronous = isAsynchronous;
			this.IsPrivate = isPrivate;
			this.IsRanked = isRanked;
			this.IsObservable = isObservable;
			this.ObsAccess = obsAccess;
			this.ObsShowHiddenInfo = obsShowHiddenInfo;
			this._startLobbyTime = Time.unscaledTime;
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x00052568 File Offset: 0x00050768
		public override void Resume()
		{
			base.Resume();
			this._startLobbyTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x0400330B RID: 13067
		private float _startLobbyTime;
	}
}

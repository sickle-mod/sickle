using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009D4 RID: 2516
	public class LobbyAnalyticsContext : AnalyticsContext
	{
		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060041F3 RID: 16883 RVA: 0x00052419 File Offset: 0x00050619
		// (set) Token: 0x060041F4 RID: 16884 RVA: 0x00052421 File Offset: 0x00050621
		public string LobbySessionId { get; private set; }

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x060041F5 RID: 16885 RVA: 0x0005242A File Offset: 0x0005062A
		public int TimeActiveSec
		{
			get
			{
				return (int)(Time.unscaledTime - this._startLobbyTime);
			}
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x001629F8 File Offset: 0x00160BF8
		public LobbyAnalyticsContext()
		{
			this.LobbySessionId = Guid.NewGuid().ToString();
			this._startLobbyTime = Time.unscaledTime;
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x00052439 File Offset: 0x00050639
		public override void Resume()
		{
			base.Resume();
			this._startLobbyTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x040032FB RID: 13051
		private float _startLobbyTime;
	}
}

using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200032D RID: 813
	public class ReconnectData : Data
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06001767 RID: 5991 RVA: 0x00037F0C File Offset: 0x0003610C
		// (set) Token: 0x06001768 RID: 5992 RVA: 0x00037F14 File Offset: 0x00036114
		public int TimeLeft { get; set; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06001769 RID: 5993 RVA: 0x00037F1D File Offset: 0x0003611D
		// (set) Token: 0x0600176A RID: 5994 RVA: 0x00037F25 File Offset: 0x00036125
		public bool RankedGame { get; set; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600176B RID: 5995 RVA: 0x00037F2E File Offset: 0x0003612E
		// (set) Token: 0x0600176C RID: 5996 RVA: 0x00037F36 File Offset: 0x00036136
		public GameType GameType { get; set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600176D RID: 5997 RVA: 0x00037F3F File Offset: 0x0003613F
		// (set) Token: 0x0600176E RID: 5998 RVA: 0x00037F47 File Offset: 0x00036147
		public bool IfaDlc { get; set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600176F RID: 5999 RVA: 0x00037F50 File Offset: 0x00036150
		// (set) Token: 0x06001770 RID: 6000 RVA: 0x00037F58 File Offset: 0x00036158
		public string ThirdPartyPlayerId { get; set; }

		// Token: 0x06001771 RID: 6001 RVA: 0x00037A7B File Offset: 0x00035C7B
		public ReconnectData()
		{
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x00037F61 File Offset: 0x00036161
		public ReconnectData(string gameId, int timeLeft, bool ranked, GameType gameType, string thirdPartyPlayerId)
			: base(gameId)
		{
			this.TimeLeft = timeLeft;
			this.RankedGame = ranked;
			this.GameType = gameType;
			this.IfaDlc = GameServiceController.Instance.InvadersFromAfarUnlocked();
			this.ThirdPartyPlayerId = thirdPartyPlayerId;
		}
	}
}

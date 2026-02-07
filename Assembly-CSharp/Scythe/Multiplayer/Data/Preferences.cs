using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200032C RID: 812
	public class Preferences
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x0600175B RID: 5979 RVA: 0x00037E7D File Offset: 0x0003607D
		// (set) Token: 0x0600175C RID: 5980 RVA: 0x00037E85 File Offset: 0x00036085
		public int NumberOfPlayers { get; set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600175D RID: 5981 RVA: 0x00037E8E File Offset: 0x0003608E
		// (set) Token: 0x0600175E RID: 5982 RVA: 0x00037E96 File Offset: 0x00036096
		public bool IsRanked { get; set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600175F RID: 5983 RVA: 0x00037E9F File Offset: 0x0003609F
		// (set) Token: 0x06001760 RID: 5984 RVA: 0x00037EA7 File Offset: 0x000360A7
		public bool IsAsynchronous { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06001761 RID: 5985 RVA: 0x00037EB0 File Offset: 0x000360B0
		// (set) Token: 0x06001762 RID: 5986 RVA: 0x00037EB8 File Offset: 0x000360B8
		public Guid PlayerId { get; set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06001763 RID: 5987 RVA: 0x00037EC1 File Offset: 0x000360C1
		// (set) Token: 0x06001764 RID: 5988 RVA: 0x00037EC9 File Offset: 0x000360C9
		public bool IncludeDLC { get; set; }

		// Token: 0x06001765 RID: 5989 RVA: 0x00027E56 File Offset: 0x00026056
		public Preferences()
		{
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x00037ED2 File Offset: 0x000360D2
		public Preferences(int numberOfPlayers, bool isRanked, bool isAsynchronous, bool includeDLC)
		{
			this.NumberOfPlayers = numberOfPlayers;
			this.IsRanked = isRanked;
			this.IsAsynchronous = isAsynchronous;
			this.PlayerId = PlayerInfo.me.PlayerStats.Id;
			this.IncludeDLC = includeDLC;
		}
	}
}

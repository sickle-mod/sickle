using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200032F RID: 815
	public class ReturnToGame : Data
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06001773 RID: 6003 RVA: 0x00037F98 File Offset: 0x00036198
		// (set) Token: 0x06001774 RID: 6004 RVA: 0x00037FA0 File Offset: 0x000361A0
		public bool IsRanked { get; set; }

		// Token: 0x06001775 RID: 6005 RVA: 0x00037A7B File Offset: 0x00035C7B
		public ReturnToGame()
		{
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00037FA9 File Offset: 0x000361A9
		public ReturnToGame(string roomId, bool isRanked)
		{
			base.RoomId = roomId;
			this.IsRanked = isRanked;
		}
	}
}

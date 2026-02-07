using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200034B RID: 843
	public class InviteSend : Data
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600182C RID: 6188 RVA: 0x00038602 File Offset: 0x00036802
		// (set) Token: 0x0600182D RID: 6189 RVA: 0x0003860A File Offset: 0x0003680A
		public Guid FriendId { get; set; }

		// Token: 0x0600182E RID: 6190 RVA: 0x00037A7B File Offset: 0x00035C7B
		public InviteSend()
		{
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x00038613 File Offset: 0x00036813
		public InviteSend(Guid friendId)
		{
			this.FriendId = friendId;
		}
	}
}

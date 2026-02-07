using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000325 RID: 805
	public class Invitation
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06001700 RID: 5888 RVA: 0x00037B79 File Offset: 0x00035D79
		// (set) Token: 0x06001701 RID: 5889 RVA: 0x00037B81 File Offset: 0x00035D81
		public InvitationStatus Status { get; set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06001702 RID: 5890 RVA: 0x00037B8A File Offset: 0x00035D8A
		// (set) Token: 0x06001703 RID: 5891 RVA: 0x00037B92 File Offset: 0x00035D92
		public DateTime Timestamp { get; set; }
	}
}

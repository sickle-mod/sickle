using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200034A RID: 842
	public class InvitationReceived : Data
	{
		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06001825 RID: 6181 RVA: 0x000385CF File Offset: 0x000367CF
		// (set) Token: 0x06001826 RID: 6182 RVA: 0x000385D7 File Offset: 0x000367D7
		public string Name { get; set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06001827 RID: 6183 RVA: 0x000385E0 File Offset: 0x000367E0
		// (set) Token: 0x06001828 RID: 6184 RVA: 0x000385E8 File Offset: 0x000367E8
		public string RoomName { get; set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06001829 RID: 6185 RVA: 0x000385F1 File Offset: 0x000367F1
		// (set) Token: 0x0600182A RID: 6186 RVA: 0x000385F9 File Offset: 0x000367F9
		public bool IfaDLC { get; set; }
	}
}

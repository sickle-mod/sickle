using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200030C RID: 780
	public class Data
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x0600169F RID: 5791 RVA: 0x000377E3 File Offset: 0x000359E3
		// (set) Token: 0x060016A0 RID: 5792 RVA: 0x000377EB File Offset: 0x000359EB
		public string RoomId { get; set; }

		// Token: 0x060016A1 RID: 5793 RVA: 0x000377F4 File Offset: 0x000359F4
		public Data()
		{
			this.RoomId = PlayerInfo.me.RoomId;
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x0003780C File Offset: 0x00035A0C
		public Data(string roomId)
		{
			this.RoomId = roomId;
		}
	}
}

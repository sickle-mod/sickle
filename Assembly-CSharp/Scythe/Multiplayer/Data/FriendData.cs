using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200030F RID: 783
	public class FriendData
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060016B0 RID: 5808 RVA: 0x00037895 File Offset: 0x00035A95
		// (set) Token: 0x060016B1 RID: 5809 RVA: 0x0003789D File Offset: 0x00035A9D
		public int Id { get; set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060016B2 RID: 5810 RVA: 0x000378A6 File Offset: 0x00035AA6
		// (set) Token: 0x060016B3 RID: 5811 RVA: 0x000378AE File Offset: 0x00035AAE
		public string Name { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060016B4 RID: 5812 RVA: 0x000378B7 File Offset: 0x00035AB7
		// (set) Token: 0x060016B5 RID: 5813 RVA: 0x000378BF File Offset: 0x00035ABF
		public bool IsOnline { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060016B6 RID: 5814 RVA: 0x000378C8 File Offset: 0x00035AC8
		// (set) Token: 0x060016B7 RID: 5815 RVA: 0x000378D0 File Offset: 0x00035AD0
		public string RoomName { get; set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060016B8 RID: 5816 RVA: 0x000378D9 File Offset: 0x00035AD9
		// (set) Token: 0x060016B9 RID: 5817 RVA: 0x000378E1 File Offset: 0x00035AE1
		public string RoomId { get; set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060016BA RID: 5818 RVA: 0x000378EA File Offset: 0x00035AEA
		// (set) Token: 0x060016BB RID: 5819 RVA: 0x000378F2 File Offset: 0x00035AF2
		public int PlayersInRoom { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060016BC RID: 5820 RVA: 0x000378FB File Offset: 0x00035AFB
		// (set) Token: 0x060016BD RID: 5821 RVA: 0x00037903 File Offset: 0x00035B03
		public int MaxPlayersInRoom { get; set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060016BE RID: 5822 RVA: 0x0003790C File Offset: 0x00035B0C
		// (set) Token: 0x060016BF RID: 5823 RVA: 0x00037914 File Offset: 0x00035B14
		public int Faction { get; set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060016C0 RID: 5824 RVA: 0x0003791D File Offset: 0x00035B1D
		// (set) Token: 0x060016C1 RID: 5825 RVA: 0x00037925 File Offset: 0x00035B25
		public bool Asynchronous { get; set; }
	}
}

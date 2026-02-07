using System;
using System.Collections.Generic;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000323 RID: 803
	public class GameData
	{
		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060016F2 RID: 5874 RVA: 0x00037ADD File Offset: 0x00035CDD
		// (set) Token: 0x060016F3 RID: 5875 RVA: 0x00037AE5 File Offset: 0x00035CE5
		public string GameState { get; set; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060016F4 RID: 5876 RVA: 0x00037AEE File Offset: 0x00035CEE
		// (set) Token: 0x060016F5 RID: 5877 RVA: 0x00037AF6 File Offset: 0x00035CF6
		public int MessageCounter { get; set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060016F6 RID: 5878 RVA: 0x00037AFF File Offset: 0x00035CFF
		// (set) Token: 0x060016F7 RID: 5879 RVA: 0x00037B07 File Offset: 0x00035D07
		public int PlayerClock { get; set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060016F8 RID: 5880 RVA: 0x00037B10 File Offset: 0x00035D10
		// (set) Token: 0x060016F9 RID: 5881 RVA: 0x00037B18 File Offset: 0x00035D18
		public List<PlayerData> Players { get; set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060016FA RID: 5882 RVA: 0x00037B21 File Offset: 0x00035D21
		// (set) Token: 0x060016FB RID: 5883 RVA: 0x00037B29 File Offset: 0x00035D29
		public string LeaversFactions { get; set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060016FC RID: 5884 RVA: 0x00037B32 File Offset: 0x00035D32
		// (set) Token: 0x060016FD RID: 5885 RVA: 0x00037B3A File Offset: 0x00035D3A
		public string ChatInJsonFormat { get; set; }
	}
}

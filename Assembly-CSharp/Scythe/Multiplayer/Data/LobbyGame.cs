using System;
using System.Collections.Generic;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000327 RID: 807
	public class LobbyGame : LobbyRoom
	{
		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x00037B9B File Offset: 0x00035D9B
		// (set) Token: 0x06001706 RID: 5894 RVA: 0x00037BA3 File Offset: 0x00035DA3
		public Guid AdminId { get; set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06001707 RID: 5895 RVA: 0x00037BAC File Offset: 0x00035DAC
		// (set) Token: 0x06001708 RID: 5896 RVA: 0x00037BB4 File Offset: 0x00035DB4
		public List<Bot> BotsList { get; set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06001709 RID: 5897 RVA: 0x00037BBD File Offset: 0x00035DBD
		// (set) Token: 0x0600170A RID: 5898 RVA: 0x00037BC5 File Offset: 0x00035DC5
		public List<PlayerInfo> PlayersList { get; set; }
	}
}

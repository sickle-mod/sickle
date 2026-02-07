using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200034F RID: 847
	public class RemovePlayer : Data
	{
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06001849 RID: 6217 RVA: 0x00038708 File Offset: 0x00036908
		// (set) Token: 0x0600184A RID: 6218 RVA: 0x00038710 File Offset: 0x00036910
		public Guid Id { get; set; }

		// Token: 0x0600184B RID: 6219 RVA: 0x00037A7B File Offset: 0x00035C7B
		public RemovePlayer()
		{
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x00038719 File Offset: 0x00036919
		public RemovePlayer(Guid playerId)
		{
			this.Id = playerId;
		}
	}
}

using System;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002DD RID: 733
	public class GetFactoryMessage : Message
	{
		// Token: 0x0600160E RID: 5646 RVA: 0x00037269 File Offset: 0x00035469
		public GetFactoryMessage(int index)
		{
			this.faction = PlayerInfo.me.Faction;
			this.index = index;
		}

		// Token: 0x0400104A RID: 4170
		private int faction;

		// Token: 0x0400104B RID: 4171
		private int index;
	}
}

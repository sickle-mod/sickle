using System;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002DC RID: 732
	public class GetFactoryCardsMessage : Message
	{
		// Token: 0x0600160D RID: 5645 RVA: 0x00037251 File Offset: 0x00035451
		public GetFactoryCardsMessage()
		{
			this.faction = PlayerInfo.me.Faction;
		}

		// Token: 0x04001049 RID: 4169
		private int faction;
	}
}

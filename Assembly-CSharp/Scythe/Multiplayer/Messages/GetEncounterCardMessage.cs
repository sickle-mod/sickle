using System;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002DB RID: 731
	public class GetEncounterCardMessage : Message
	{
		// Token: 0x0600160C RID: 5644 RVA: 0x00037239 File Offset: 0x00035439
		public GetEncounterCardMessage()
		{
			this.faction = PlayerInfo.me.Faction;
		}

		// Token: 0x04001048 RID: 4168
		private int faction;
	}
}

using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002CC RID: 716
	public class LosePopularityMessage : Message, IExecutableMessage
	{
		// Token: 0x060015ED RID: 5613 RVA: 0x00036FA3 File Offset: 0x000351A3
		public LosePopularityMessage(int faction, int amount)
		{
			this.faction = faction;
			this.amount = amount;
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x00036FB9 File Offset: 0x000351B9
		public void Execute(GameManager gameManager)
		{
			gameManager.GetPlayerByFaction((Faction)this.faction).Popularity -= this.amount;
		}

		// Token: 0x04001026 RID: 4134
		private int faction;

		// Token: 0x04001027 RID: 4135
		private int amount;
	}
}

using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002E5 RID: 741
	public class FactoryMessage : Message, IExecutableMessage
	{
		// Token: 0x0600161C RID: 5660 RVA: 0x000372ED File Offset: 0x000354ED
		public FactoryMessage(string factoryCard, int faction, int cardIndex)
		{
			this.factoryCard = factoryCard;
			this.faction = faction;
			this.cardIndex = cardIndex;
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x0003730A File Offset: 0x0003550A
		public void Execute(GameManager gameManager)
		{
			gameManager.AddFactoryCard(int.Parse(this.factoryCard), this.faction, this.cardIndex);
		}

		// Token: 0x04001056 RID: 4182
		private string factoryCard;

		// Token: 0x04001057 RID: 4183
		private int faction;

		// Token: 0x04001058 RID: 4184
		private int cardIndex;
	}
}

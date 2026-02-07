using System;
using System.Collections.Generic;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002E4 RID: 740
	public class FactoryCardsMessage : Message, IExecutableMessage
	{
		// Token: 0x0600161A RID: 5658 RVA: 0x000372DF File Offset: 0x000354DF
		public void Execute(GameManager gameManager)
		{
			gameManager.OnShowFactoryCards(this.cardsIds);
		}

		// Token: 0x04001055 RID: 4181
		private List<string> cardsIds;
	}
}

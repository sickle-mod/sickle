using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002E1 RID: 737
	public class EmptyCardsMessage : Message, IExecutableMessage
	{
		// Token: 0x06001614 RID: 5652 RVA: 0x00036AB3 File Offset: 0x00034CB3
		public EmptyCardsMessage(int count)
		{
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00037295 File Offset: 0x00035495
		public void Execute(GameManager gameManager)
		{
			gameManager.OnShowEmptyCards(this.cardsAmount);
		}

		// Token: 0x04001051 RID: 4177
		private int cardsAmount;
	}
}

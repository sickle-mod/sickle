using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002DF RID: 735
	public class BadMessage : Message, IExecutableMessage
	{
		// Token: 0x06001610 RID: 5648 RVA: 0x00037288 File Offset: 0x00035488
		public void Execute(GameManager gameManager)
		{
			RequestController.RemoveMessageFromQueue(this.messageCounter);
		}

		// Token: 0x0400104C RID: 4172
		private int messageCounter;
	}
}

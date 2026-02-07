using System;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x02000306 RID: 774
	public class PassCoinsMessage : Message, IExecutableMessage
	{
		// Token: 0x0600167B RID: 5755 RVA: 0x00037693 File Offset: 0x00035893
		public PassCoinsMessage(int senderFaction, int receiverFaction, int amount)
		{
			this.senderFaction = senderFaction;
			this.receiverFaction = receiverFaction;
			this.amount = amount;
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x0009FA58 File Offset: 0x0009DC58
		public void Execute(GameManager gameManager)
		{
			Player playerByFaction = gameManager.GetPlayerByFaction((Faction)this.senderFaction);
			Player playerByFaction2 = gameManager.GetPlayerByFaction((Faction)this.receiverFaction);
			PassCoins.PassCoinsBetweenPlayers(gameManager, playerByFaction, playerByFaction2, this.amount);
			gameManager.OnActionFinished();
		}

		// Token: 0x0400108B RID: 4235
		private int senderFaction;

		// Token: 0x0400108C RID: 4236
		private int receiverFaction;

		// Token: 0x0400108D RID: 4237
		private int amount;
	}
}

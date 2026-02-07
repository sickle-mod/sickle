using System;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002BE RID: 702
	public class PayPowerMessage : Message, IExecutableMessage
	{
		// Token: 0x060015BF RID: 5567 RVA: 0x00036D36 File Offset: 0x00034F36
		public PayPowerMessage(short amount)
		{
			this.amount = amount;
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x00036D45 File Offset: 0x00034F45
		public void Execute(GameManager gameManager)
		{
			(gameManager.actionManager.GetLastPayAction() as PayPower).SetPayed(true);
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04001006 RID: 4102
		private short amount;
	}
}

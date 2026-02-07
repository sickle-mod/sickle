using System;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002BA RID: 698
	public class PayCoinMessage : Message, IExecutableMessage
	{
		// Token: 0x060015B7 RID: 5559 RVA: 0x00036CB4 File Offset: 0x00034EB4
		public PayCoinMessage(short amount, bool encounter)
		{
			this.amount = amount;
			this.encounter = encounter;
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x00036CCA File Offset: 0x00034ECA
		public void Execute(GameManager gameManager)
		{
			(gameManager.actionManager.GetLastPayAction() as PayCoin).SetPayed(true);
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04001000 RID: 4096
		private short amount;

		// Token: 0x04001001 RID: 4097
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		private bool encounter;
	}
}

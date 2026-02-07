using System;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002BD RID: 701
	public class PayPopularityMessage : Message, IExecutableMessage
	{
		// Token: 0x060015BD RID: 5565 RVA: 0x00036CFD File Offset: 0x00034EFD
		public PayPopularityMessage(short amount, bool encounter)
		{
			this.amount = amount;
			this.encounter = encounter;
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x00036D13 File Offset: 0x00034F13
		public void Execute(GameManager gameManager)
		{
			(gameManager.actionManager.GetLastPayAction() as PayPopularity).SetPayed(true);
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04001004 RID: 4100
		private short amount;

		// Token: 0x04001005 RID: 4101
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		private bool encounter;
	}
}

using System;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002AF RID: 687
	public class GainPopularityMessage : Message, IExecutableMessage
	{
		// Token: 0x0600159E RID: 5534 RVA: 0x00036AF3 File Offset: 0x00034CF3
		public GainPopularityMessage(short amount, bool bonusAction, bool encounter)
		{
			this.amount = amount;
			this.bonusAction = bonusAction;
			this.encounter = encounter;
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x00036B10 File Offset: 0x00034D10
		public void Execute(GameManager gameManager)
		{
			(gameManager.actionManager.GetLastSelectedGainAction() as GainPopularity).SetPopularity(this.amount);
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04000FD1 RID: 4049
		private short amount;

		// Token: 0x04000FD2 RID: 4050
		private bool bonusAction;

		// Token: 0x04000FD3 RID: 4051
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		private bool encounter;
	}
}

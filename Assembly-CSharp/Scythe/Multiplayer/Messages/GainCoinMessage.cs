using System;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002AB RID: 683
	public class GainCoinMessage : Message, IExecutableMessage
	{
		// Token: 0x06001596 RID: 5526 RVA: 0x00036A39 File Offset: 0x00034C39
		public GainCoinMessage(short amount, bool encounter)
		{
			this.amount = amount;
			this.encounter = encounter;
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x00036A4F File Offset: 0x00034C4F
		public void Execute(GameManager gameManager)
		{
			(gameManager.actionManager.GetLastSelectedGainAction() as GainCoin).SetCoins(this.amount);
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04000FCA RID: 4042
		private short amount;

		// Token: 0x04000FCB RID: 4043
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		private bool encounter;
	}
}

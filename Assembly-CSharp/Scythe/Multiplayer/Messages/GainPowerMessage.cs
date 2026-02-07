using System;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002B0 RID: 688
	public class GainPowerMessage : Message, IExecutableMessage
	{
		// Token: 0x060015A0 RID: 5536 RVA: 0x00036B39 File Offset: 0x00034D39
		public GainPowerMessage(short amount, bool bonusAction, bool encounter)
		{
			this.amount = amount;
			this.bonusAction = bonusAction;
			this.encounter = encounter;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00036B56 File Offset: 0x00034D56
		public void Execute(GameManager gameManager)
		{
			(gameManager.actionManager.GetLastSelectedGainAction() as GainPower).SetPower(this.amount);
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04000FD4 RID: 4052
		private short amount;

		// Token: 0x04000FD5 RID: 4053
		private bool bonusAction;

		// Token: 0x04000FD6 RID: 4054
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		private bool encounter;
	}
}

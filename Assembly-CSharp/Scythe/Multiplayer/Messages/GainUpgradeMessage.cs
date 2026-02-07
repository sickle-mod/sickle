using System;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002B4 RID: 692
	public class GainUpgradeMessage : Message, IExecutableMessage
	{
		// Token: 0x060015A9 RID: 5545 RVA: 0x00036C0F File Offset: 0x00034E0F
		public GainUpgradeMessage(int topId, int downId, int actionIndex, bool encounter)
		{
			this.topId = topId;
			this.downId = downId;
			this.actionIndex = actionIndex;
			this.encounter = encounter;
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x0009E004 File Offset: 0x0009C204
		public void Execute(GameManager gameManager)
		{
			GainUpgrade gainUpgrade = gameManager.actionManager.GetLastSelectedGainAction() as GainUpgrade;
			GainAction gainAction = gameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(this.topId).ActionTop.GetGainAction(this.actionIndex);
			PayAction payAction = gameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(this.downId).ActionDown.GetPayAction(0);
			gainUpgrade.SetPayAndGainActions(gainAction, payAction);
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04000FE4 RID: 4068
		private int topId;

		// Token: 0x04000FE5 RID: 4069
		private int downId;

		// Token: 0x04000FE6 RID: 4070
		private int actionIndex;

		// Token: 0x04000FE7 RID: 4071
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		private bool encounter;
	}
}

using System;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002B5 RID: 693
	public class GainWorkerMessage : Message, IExecutableMessage
	{
		// Token: 0x060015AB RID: 5547 RVA: 0x00036C34 File Offset: 0x00034E34
		public GainWorkerMessage(GameHex hex, int workersAmount, bool encounter)
		{
			this.x = hex.posX;
			this.y = hex.posY;
			this.workersAmount = workersAmount;
			this.encounter = encounter;
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x0009E080 File Offset: 0x0009C280
		public void Execute(GameManager gameManager)
		{
			GainWorker gainWorker = gameManager.actionManager.GetLastSelectedGainAction() as GainWorker;
			GameHex gameHex = gameManager.gameBoard.hexMap[this.x, this.y];
			gainWorker.SetLocationAndWorkersAmount(gameHex, this.workersAmount);
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04000FE8 RID: 4072
		private int x;

		// Token: 0x04000FE9 RID: 4073
		private int y;

		// Token: 0x04000FEA RID: 4074
		private int workersAmount;

		// Token: 0x04000FEB RID: 4075
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		private bool encounter;
	}
}

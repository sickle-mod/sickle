using System;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002B1 RID: 689
	public class GainProduceMessage : Message, IExecutableMessage
	{
		// Token: 0x060015A2 RID: 5538 RVA: 0x00036B7F File Offset: 0x00034D7F
		public GainProduceMessage()
		{
			this.actionEnded = true;
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x00036B8E File Offset: 0x00034D8E
		public GainProduceMessage(GameHex hex, int amount, short hexesLeft)
		{
			this.x = hex.posX;
			this.y = hex.posY;
			this.amount = amount;
			this.hexesLeft = hexesLeft;
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0009DEA4 File Offset: 0x0009C0A4
		public void Execute(GameManager gameManager)
		{
			GainProduce gainProduce = gameManager.actionManager.GetLastSelectedGainAction() as GainProduce;
			gainProduce.SelectAction();
			if (this.actionEnded)
			{
				gameManager.actionManager.PrepareNextAction();
				return;
			}
			GameHex gameHex = gameManager.gameBoard.hexMap[this.x, this.y];
			gainProduce.ExecuteOnce(gameHex, this.amount);
		}

		// Token: 0x04000FD7 RID: 4055
		private int x;

		// Token: 0x04000FD8 RID: 4056
		private int y;

		// Token: 0x04000FD9 RID: 4057
		private int amount;

		// Token: 0x04000FDA RID: 4058
		private short hexesLeft;

		// Token: 0x04000FDB RID: 4059
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		private bool actionEnded;
	}
}

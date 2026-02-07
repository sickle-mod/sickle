using System;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002B3 RID: 691
	public class GainResourceMessage : Message, IExecutableMessage
	{
		// Token: 0x060015A7 RID: 5543 RVA: 0x00036BD9 File Offset: 0x00034DD9
		public GainResourceMessage(int resourceType, GameHex hex, short amount, bool encounter)
		{
			this.resourceType = resourceType;
			this.x = hex.posX;
			this.y = hex.posY;
			this.amount = amount;
			this.encounter = encounter;
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x0009DFB0 File Offset: 0x0009C1B0
		public void Execute(GameManager gameManager)
		{
			GainResource gainResource = gameManager.actionManager.GetLastSelectedGainAction() as GainResource;
			GameHex gameHex = gameManager.gameBoard.hexMap[this.x, this.y];
			gainResource.SetDestinationAmount(gameHex, this.amount);
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04000FDF RID: 4063
		private int resourceType;

		// Token: 0x04000FE0 RID: 4064
		private int x;

		// Token: 0x04000FE1 RID: 4065
		private int y;

		// Token: 0x04000FE2 RID: 4066
		private short amount;

		// Token: 0x04000FE3 RID: 4067
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		private bool encounter;
	}
}

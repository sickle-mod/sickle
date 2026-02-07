using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002D4 RID: 724
	public class TrapTokenArmedMessage : Message, IExecutableMessage
	{
		// Token: 0x060015FE RID: 5630 RVA: 0x000370E4 File Offset: 0x000352E4
		public TrapTokenArmedMessage(GameHex hex)
		{
			this.x = hex.posX;
			this.y = hex.posY;
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0009F230 File Offset: 0x0009D430
		public void Execute(GameManager gameManager)
		{
			GameHex gameHex = gameManager.gameBoard.hexMap[this.x, this.y];
			gameManager.tokenManager.ArmTrap(gameHex);
			gameManager.OnActionFinished();
		}

		// Token: 0x0400103A RID: 4154
		private int x;

		// Token: 0x0400103B RID: 4155
		private int y;
	}
}

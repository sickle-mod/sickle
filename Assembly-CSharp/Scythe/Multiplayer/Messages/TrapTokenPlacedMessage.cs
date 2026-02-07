using System;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002D5 RID: 725
	public class TrapTokenPlacedMessage : Message, IExecutableMessage
	{
		// Token: 0x06001600 RID: 5632 RVA: 0x00037104 File Offset: 0x00035304
		public TrapTokenPlacedMessage(GameHex hex, PayType penalty)
		{
			this.x = hex.posX;
			this.y = hex.posY;
			this.penalty = (int)penalty;
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0009F270 File Offset: 0x0009D470
		public void Execute(GameManager gameManager)
		{
			GameHex gameHex = gameManager.gameBoard.hexMap[this.x, this.y];
			gameManager.tokenManager.PlaceToken(gameHex, this.penalty);
			gameManager.OnActionFinished();
		}

		// Token: 0x0400103C RID: 4156
		private int x;

		// Token: 0x0400103D RID: 4157
		private int y;

		// Token: 0x0400103E RID: 4158
		private int penalty;
	}
}

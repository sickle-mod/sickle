using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002D3 RID: 723
	public class FlagTokenPlacedMessage : Message, IExecutableMessage
	{
		// Token: 0x060015FC RID: 5628 RVA: 0x000370C4 File Offset: 0x000352C4
		public FlagTokenPlacedMessage(GameHex hex)
		{
			this.x = hex.posX;
			this.y = hex.posY;
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x0009F1F0 File Offset: 0x0009D3F0
		public void Execute(GameManager gameManager)
		{
			GameHex gameHex = gameManager.gameBoard.hexMap[this.x, this.y];
			gameManager.tokenManager.PlaceToken(gameHex);
			gameManager.OnActionFinished();
		}

		// Token: 0x04001038 RID: 4152
		private int x;

		// Token: 0x04001039 RID: 4153
		private int y;
	}
}

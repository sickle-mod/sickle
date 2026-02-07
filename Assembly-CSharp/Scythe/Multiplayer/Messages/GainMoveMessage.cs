using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002AD RID: 685
	public class GainMoveMessage : Message, IExecutableMessage
	{
		// Token: 0x0600159A RID: 5530 RVA: 0x00036AA6 File Offset: 0x00034CA6
		public void Execute(GameManager gameManager)
		{
			gameManager.actionManager.PrepareNextAction();
		}
	}
}

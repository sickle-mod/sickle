using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002EA RID: 746
	public class OutOfTimeMessage : Message, IExecutableMessage
	{
		// Token: 0x0600162A RID: 5674 RVA: 0x000373BC File Offset: 0x000355BC
		public void Execute(GameManager gameManager)
		{
			PlayerClock.StopTimer();
			ConnectionProblem.ShowOutOfTimePanel();
		}
	}
}

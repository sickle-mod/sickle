using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002D6 RID: 726
	public class EncounterEndedMessage : Message, IExecutableMessage
	{
		// Token: 0x06001602 RID: 5634 RVA: 0x0003712B File Offset: 0x0003532B
		public EncounterEndedMessage(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x00037141 File Offset: 0x00035341
		public void Execute(GameManager gameManager)
		{
			gameManager.EndEncounter(this.x, this.y);
		}

		// Token: 0x0400103F RID: 4159
		private int x;

		// Token: 0x04001040 RID: 4160
		private int y;
	}
}

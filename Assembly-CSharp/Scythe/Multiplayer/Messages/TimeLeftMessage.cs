using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x02000309 RID: 777
	public class TimeLeftMessage : Message, IExecutableMessage
	{
		// Token: 0x06001681 RID: 5761 RVA: 0x000376D8 File Offset: 0x000358D8
		public TimeLeftMessage(int timeLeft, int playerFaction)
		{
			this.timeLeft = timeLeft;
			this.faction = playerFaction;
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x000376EE File Offset: 0x000358EE
		public void Execute(GameManager gameManager)
		{
			MultiplayerController.Instance.UpdatePlayerTimeLeft(this.faction, this.timeLeft);
		}

		// Token: 0x04001091 RID: 4241
		private int timeLeft;

		// Token: 0x04001092 RID: 4242
		private int faction;
	}
}

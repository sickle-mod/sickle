using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x02000305 RID: 773
	public class ObjectiveCompletedMessage : Message, IExecutableMessage
	{
		// Token: 0x06001679 RID: 5753 RVA: 0x00037669 File Offset: 0x00035869
		public ObjectiveCompletedMessage(int index, int faction)
		{
			this.index = index;
			this.faction = faction;
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x0003767F File Offset: 0x0003587F
		public void Execute(GameManager gameManager)
		{
			gameManager.CompleteObjective(this.index, (Faction)this.faction);
		}

		// Token: 0x04001089 RID: 4233
		private int index;

		// Token: 0x0400108A RID: 4234
		private int faction;
	}
}

using System;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002AE RID: 686
	public class GainPeekCombatCardsMessage : Message, IExecutableMessage
	{
		// Token: 0x0600159C RID: 5532 RVA: 0x00036ABB File Offset: 0x00034CBB
		public GainPeekCombatCardsMessage(Faction faction)
		{
			this.faction = (int)faction;
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x00036ACA File Offset: 0x00034CCA
		public void Execute(GameManager gameManager)
		{
			(gameManager.actionManager.GetLastSelectedGainAction() as GainPeekCombatCards).SetFaction((Faction)this.faction);
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04000FD0 RID: 4048
		private int faction;
	}
}

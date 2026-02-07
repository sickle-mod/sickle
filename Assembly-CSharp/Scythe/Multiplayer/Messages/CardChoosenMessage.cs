using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002CA RID: 714
	public class CardChoosenMessage : Message, IExecutableMessage
	{
		// Token: 0x060015E9 RID: 5609 RVA: 0x00036F31 File Offset: 0x00035131
		public CardChoosenMessage(int faction)
		{
			this.faction = faction;
			MultiplayerController.Instance.SetCurrentPlayer();
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x00036F4A File Offset: 0x0003514A
		public void Execute(GameManager gameManager)
		{
			MultiplayerController.Instance.SetCurrentPlayer();
			gameManager.combatManager.AddCombatCard(false);
			gameManager.combatManager.SwitchToNextStage();
		}

		// Token: 0x04001024 RID: 4132
		private int faction;
	}
}

using System;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002C6 RID: 710
	public class AbilityNotUsedMessage : Message, IExecutableMessage
	{
		// Token: 0x060015E1 RID: 5601 RVA: 0x00036ECF File Offset: 0x000350CF
		public AbilityNotUsedMessage()
		{
			this.faction = PlayerInfo.me.Faction;
			MultiplayerController.Instance.SetCurrentPlayer();
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x00036EF1 File Offset: 0x000350F1
		public void Execute(GameManager gameManager)
		{
			gameManager.combatManager.SwitchToNextStage();
			MultiplayerController.Instance.SetCurrentPlayer();
		}

		// Token: 0x0400101D RID: 4125
		private int faction;
	}
}

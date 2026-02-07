using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002CF RID: 719
	public class StartCombatMessage : Message, IExecutableMessage
	{
		// Token: 0x060015F3 RID: 5619 RVA: 0x00037050 File Offset: 0x00035250
		public StartCombatMessage(Faction playerFaction, int x, int y)
		{
			this.faction = (int)playerFaction;
			this.x = x;
			this.y = y;
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x0009F028 File Offset: 0x0009D228
		public void Execute(GameManager gameManager)
		{
			gameManager.combatManager.SetAttacker(gameManager.gameBoard.hexMap[this.x, this.y].Enemy);
			gameManager.combatManager.SelectBattlefield(gameManager.gameBoard.hexMap[this.x, this.y]);
			gameManager.SetBattlefieldEffect();
			gameManager.combatManager.SwitchToNextStage();
		}

		// Token: 0x0400102D RID: 4141
		private int faction;

		// Token: 0x0400102E RID: 4142
		private int x;

		// Token: 0x0400102F RID: 4143
		private int y;
	}
}

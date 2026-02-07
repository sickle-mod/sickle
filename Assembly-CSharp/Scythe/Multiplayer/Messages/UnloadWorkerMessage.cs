using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002B8 RID: 696
	public class UnloadWorkerMessage : Message, IExecutableMessage
	{
		// Token: 0x060015B1 RID: 5553 RVA: 0x00036C70 File Offset: 0x00034E70
		public UnloadWorkerMessage(GameHex position, int workerId)
		{
			this.x = position.posX;
			this.y = position.posY;
			this.workerId = workerId;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0009E324 File Offset: 0x0009C524
		public void Execute(GameManager gameManager)
		{
			Unit unit = gameManager.moveManager.GetUnit(UnitType.Worker, this.x, this.y, this.workerId);
			gameManager.moveManager.UnloadWorkerFromSelectedMech(unit);
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x00036C97 File Offset: 0x00034E97
		private Player GetProperPlayer(GameManager gameManager)
		{
			if (!this.NordicRetreat(gameManager))
			{
				return gameManager.PlayerCurrent;
			}
			return gameManager.combatManager.GetDefeated();
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0009E360 File Offset: 0x0009C560
		private bool NordicRetreat(GameManager gameManager)
		{
			return gameManager.combatManager.CombatAlreadyStarted() && gameManager.combatManager.GetDefeated() != null && gameManager.combatManager.GetDefeated().matFaction.faction == Faction.Nordic && gameManager.combatManager.GetDefeated().matFaction.SkillUnlocked[1];
		}

		// Token: 0x04000FF8 RID: 4088
		private int x;

		// Token: 0x04000FF9 RID: 4089
		private int y;

		// Token: 0x04000FFA RID: 4090
		private int workerId;
	}
}

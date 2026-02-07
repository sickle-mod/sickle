using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002CE RID: 718
	public class RemovePowerMessage : Message, IExecutableMessage
	{
		// Token: 0x060015F1 RID: 5617 RVA: 0x00037029 File Offset: 0x00035229
		public RemovePowerMessage(int cost, int playerUsing, int playerLosing)
		{
			this.cost = cost;
			this.playerUsing = playerUsing;
			this.playerLosing = playerLosing;
			MultiplayerController.Instance.SetCurrentPlayer();
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x0009EF98 File Offset: 0x0009D198
		public void Execute(GameManager gameManager)
		{
			Player playerByFaction = gameManager.GetPlayerByFaction((Faction)this.playerUsing);
			Player playerByFaction2 = gameManager.GetPlayerByFaction((Faction)this.playerLosing);
			playerByFaction.Power -= this.cost;
			playerByFaction2.Power -= 2;
			gameManager.OnActionFinished();
			gameManager.combatManager.SwitchToNextStage();
			if (this.cost == 0)
			{
				if (this.playerUsing == 6)
				{
					gameManager.EnemyUsedCombatAbility(AbilityPerk.Disarm);
				}
				else
				{
					gameManager.EnemyUsedCombatAbility(AbilityPerk.Sword);
				}
			}
			else
			{
				gameManager.EnemyUsedCombatAbility(AbilityPerk.Artillery);
			}
			MultiplayerController.Instance.SetCurrentPlayer();
		}

		// Token: 0x0400102A RID: 4138
		private int cost;

		// Token: 0x0400102B RID: 4139
		private int playerUsing;

		// Token: 0x0400102C RID: 4140
		private int playerLosing;
	}
}

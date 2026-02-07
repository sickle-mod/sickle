using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002C9 RID: 713
	public class BolsterBeforeCombatMessage : Message, IExecutableMessage
	{
		// Token: 0x060015E7 RID: 5607 RVA: 0x00036F18 File Offset: 0x00035118
		public BolsterBeforeCombatMessage(int playerUsing)
		{
			this.playerUsing = playerUsing;
			MultiplayerController.Instance.SetCurrentPlayer();
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x0009EF3C File Offset: 0x0009D13C
		public void Execute(GameManager gameManager)
		{
			gameManager.GetPlayerByFaction((Faction)this.playerUsing).Power += 2;
			gameManager.OnActionFinished();
			gameManager.combatManager.SwitchToNextStage();
			if (this.playerUsing == 1)
			{
				gameManager.EnemyUsedCombatAbility(AbilityPerk.Shield);
			}
			else
			{
				gameManager.EnemyUsedCombatAbility(AbilityPerk.Ronin);
			}
			MultiplayerController.Instance.SetCurrentPlayer();
		}

		// Token: 0x04001023 RID: 4131
		private int playerUsing;
	}
}

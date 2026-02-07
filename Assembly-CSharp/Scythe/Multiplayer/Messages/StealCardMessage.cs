using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002D0 RID: 720
	public class StealCardMessage : Message, IExecutableMessage
	{
		// Token: 0x060015F5 RID: 5621 RVA: 0x0003706D File Offset: 0x0003526D
		public StealCardMessage(int cardId, int playerUsing, int playerLosing)
		{
			this.cardId = cardId;
			this.playerUsing = playerUsing;
			this.playerLosing = playerLosing;
			MultiplayerController.Instance.SetCurrentPlayer();
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x0009F09C File Offset: 0x0009D29C
		public void Execute(GameManager gameManager)
		{
			Player playerByFaction = gameManager.GetPlayerByFaction((Faction)this.playerUsing);
			Player playerByFaction2 = gameManager.GetPlayerByFaction((Faction)this.playerLosing);
			CombatCard combatCard = playerByFaction2.combatCards.Find((CombatCard c) => c.CombatBonus == this.cardId);
			playerByFaction2.RemoveCombatCard(combatCard);
			playerByFaction.AddCombatCard(combatCard);
			gameManager.OnActionFinished();
			gameManager.combatManager.SwitchToNextStage();
			gameManager.EnemyUsedCombatAbility(AbilityPerk.Scout);
			MultiplayerController.Instance.SetCurrentPlayer();
		}

		// Token: 0x04001030 RID: 4144
		private int cardId;

		// Token: 0x04001031 RID: 4145
		private int playerUsing;

		// Token: 0x04001032 RID: 4146
		private int playerLosing;
	}
}

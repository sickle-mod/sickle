using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002CD RID: 717
	public class PolaniaAbilityMessage : Message, IExecutableMessage
	{
		// Token: 0x060015EF RID: 5615 RVA: 0x00036FD9 File Offset: 0x000351D9
		public PolaniaAbilityMessage(int faction, bool choice)
		{
			this.faction = faction;
			this.choice = choice;
			MultiplayerController.Instance.SetCurrentPlayer();
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x00036FF9 File Offset: 0x000351F9
		public void Execute(GameManager gameManager)
		{
			MultiplayerController.Instance.SetCurrentPlayer();
			gameManager.combatManager.PolaniaCamaraderieChoice(this.choice);
			gameManager.combatManager.SwitchToNextStage();
			gameManager.EnemyUsedCombatAbility(AbilityPerk.Camaraderie);
		}

		// Token: 0x04001028 RID: 4136
		private int faction;

		// Token: 0x04001029 RID: 4137
		private bool choice;
	}
}

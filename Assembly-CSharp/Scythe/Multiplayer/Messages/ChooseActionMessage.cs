using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002C4 RID: 708
	public class ChooseActionMessage : Message, IExecutableMessage
	{
		// Token: 0x060015DD RID: 5597 RVA: 0x00036E8E File Offset: 0x0003508E
		public ChooseActionMessage(Faction senderFaction, int actionType, int actionIndex)
		{
			this.faction = (int)senderFaction;
			this.actionType = actionType;
			this.actionIndex = actionIndex;
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x0009ED44 File Offset: 0x0009CF44
		public void Execute(GameManager gameManager)
		{
			if (this.actionType == 0)
			{
				gameManager.actionManager.BreakSectionAction(false);
				gameManager.actionManager.SetSectionAction(gameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(gameManager.PlayerCurrent.currentMatSection).ActionDown, null, this.actionIndex);
				return;
			}
			if (this.actionType == 1)
			{
				gameManager.ClearLastEncounterCard();
				gameManager.actionManager.BreakSectionAction(false);
				gameManager.actionManager.SetSectionAction(gameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(gameManager.PlayerCurrent.currentMatSection).ActionTop, null, this.actionIndex);
			}
		}

		// Token: 0x04001018 RID: 4120
		private int faction;

		// Token: 0x04001019 RID: 4121
		private int actionType;

		// Token: 0x0400101A RID: 4122
		private int actionIndex;
	}
}

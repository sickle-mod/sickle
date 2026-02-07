using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002DA RID: 730
	public class GetCombatCardsMessage : Message, IExecutableMessage
	{
		// Token: 0x0600160A RID: 5642 RVA: 0x0003720C File Offset: 0x0003540C
		public GetCombatCardsMessage(short amount, int type)
		{
			this.faction = PlayerInfo.me.Faction;
			this.amount = amount;
			this.type = type;
			this.combatCards = null;
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x0009F2B4 File Offset: 0x0009D4B4
		public void Execute(GameManager gameManager)
		{
			GainCombatCard gainCombatCard = gameManager.actionManager.GetLastSelectedGainAction() as GainCombatCard;
			if (gainCombatCard != null)
			{
				gainCombatCard.SetCards(gainCombatCard.Amount);
				gameManager.actionManager.PrepareNextAction();
			}
		}

		// Token: 0x04001044 RID: 4164
		private int faction;

		// Token: 0x04001045 RID: 4165
		private short amount;

		// Token: 0x04001046 RID: 4166
		private int type;

		// Token: 0x04001047 RID: 4167
		private List<CombatCard> combatCards;
	}
}

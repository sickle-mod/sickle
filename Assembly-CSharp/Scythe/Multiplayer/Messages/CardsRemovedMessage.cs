using System;
using System.Collections.Generic;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002D1 RID: 721
	public class CardsRemovedMessage : Message, IExecutableMessage
	{
		// Token: 0x060015F8 RID: 5624 RVA: 0x0009F108 File Offset: 0x0009D308
		public CardsRemovedMessage(List<CombatCard> combatCards, Player player)
		{
			this.playerFaction = (int)player.matFaction.faction;
			this.cards = new List<int>(combatCards.Count);
			for (int i = 0; i < combatCards.Count; i++)
			{
				this.cards.Add(combatCards[i].CombatBonus);
			}
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0009F168 File Offset: 0x0009D368
		public void Execute(GameManager gameManager)
		{
			Player playerByFaction = gameManager.GetPlayerByFaction((Faction)this.playerFaction);
			int i;
			Predicate<CombatCard> <>9__0;
			int num;
			for (i = 0; i < this.cards.Count; i = num)
			{
				List<CombatCard> combatCards = playerByFaction.combatCards;
				Predicate<CombatCard> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (CombatCard c) => c.CombatBonus == this.cards[i]);
				}
				CombatCard combatCard = combatCards.Find(predicate);
				playerByFaction.RemoveCombatCard(combatCard);
				num = i + 1;
			}
		}

		// Token: 0x04001033 RID: 4147
		private List<int> cards;

		// Token: 0x04001034 RID: 4148
		private int playerFaction;
	}
}

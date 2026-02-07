using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002BB RID: 699
	public class PayCombatCardMessage : Message, IExecutableMessage
	{
		// Token: 0x060015B9 RID: 5561 RVA: 0x0009E630 File Offset: 0x0009C830
		public PayCombatCardMessage(List<CombatCard> combatCards)
		{
			this.cards = new List<int>(combatCards.Count);
			foreach (CombatCard combatCard in combatCards)
			{
				this.cards.Add(combatCard.CombatBonus);
			}
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0009E6A0 File Offset: 0x0009C8A0
		public void Execute(GameManager gameManager)
		{
			PayCombatCard payCombatCard = gameManager.actionManager.GetLastPayAction() as PayCombatCard;
			List<CombatCard> list = new List<CombatCard>(this.cards.Count);
			List<CombatCard> combatCards = gameManager.PlayerCurrent.combatCards;
			using (List<int>.Enumerator enumerator = this.cards.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int cardId = enumerator.Current;
					CombatCard combatCard = combatCards.Find((CombatCard c) => c.CardId == cardId);
					combatCards.Remove(combatCard);
					list.Add(combatCard);
				}
			}
			payCombatCard.SetPayed(true);
			payCombatCard.SetCombatCards(list);
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04001002 RID: 4098
		private List<int> cards;
	}
}

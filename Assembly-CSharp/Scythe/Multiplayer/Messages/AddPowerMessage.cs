using System;
using System.Collections.Generic;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002C7 RID: 711
	public class AddPowerMessage : Message, IExecutableMessage
	{
		// Token: 0x060015E3 RID: 5603 RVA: 0x0009EDE8 File Offset: 0x0009CFE8
		public AddPowerMessage(int faction, int power, int cardsPower, List<CombatCard> cards)
		{
			this.faction = faction;
			this.power = power;
			this.cardsPower = cardsPower;
			this.combatCards = new List<int>();
			foreach (CombatCard combatCard in cards)
			{
				this.combatCards.Add(combatCard.CardId);
			}
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x0009EE68 File Offset: 0x0009D068
		public void Execute(GameManager gameManager)
		{
			MultiplayerController.Instance.DisableActivePlayer(this.faction);
			Player playerByFaction = gameManager.GetPlayerByFaction((Faction)this.faction);
			List<CombatCard> list = new List<CombatCard>();
			List<CombatCard> list2 = new List<CombatCard>(playerByFaction.combatCards);
			using (List<int>.Enumerator enumerator = this.combatCards.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int combatCard = enumerator.Current;
					CombatCard combatCard2 = list2.Find((CombatCard c) => c.CardId == combatCard);
					list.Add(combatCard2);
					list2.Remove(combatCard2);
				}
			}
			gameManager.AddPower(gameManager.GetPlayerByFaction((Faction)this.faction), this.power, this.cardsPower, list);
			gameManager.combatManager.SwitchToNextStage();
		}

		// Token: 0x0400101E RID: 4126
		private int faction;

		// Token: 0x0400101F RID: 4127
		private int power;

		// Token: 0x04001020 RID: 4128
		private int cardsPower;

		// Token: 0x04001021 RID: 4129
		private List<int> combatCards;
	}
}

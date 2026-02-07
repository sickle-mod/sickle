using System;

namespace Scythe.GameLogic
{
	// Token: 0x020005A8 RID: 1448
	public class CombatCard : Card, IComparable
	{
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06002DF1 RID: 11761 RVA: 0x00044972 File Offset: 0x00042B72
		public int CombatBonus
		{
			get
			{
				return this.cardId;
			}
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x00044A18 File Offset: 0x00042C18
		public CombatCard()
		{
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x00044A20 File Offset: 0x00042C20
		public CombatCard(int combatBonus)
		{
			this.cardId = combatBonus;
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x00044A2F File Offset: 0x00042C2F
		public int CompareTo(object c)
		{
			return this.cardId.CompareTo(((CombatCard)c).cardId);
		}
	}
}

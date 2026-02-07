using System;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005B9 RID: 1465
	public static class TokenSupplyFactory
	{
		// Token: 0x06002EB9 RID: 11961 RVA: 0x00045225 File Offset: 0x00043425
		public static TokenSupply GetTokenSupply(Faction faction, GameManager gameManager)
		{
			if (faction == Faction.Albion)
			{
				return TokenSupplyFactory.GetAlbionTokenSupply(gameManager);
			}
			if (faction != Faction.Togawa)
			{
				return null;
			}
			return TokenSupplyFactory.GetTogawaTokenSupply(gameManager);
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x0011BA80 File Offset: 0x00119C80
		private static TokenSupply GetAlbionTokenSupply(GameManager gameManager)
		{
			TokenSupply tokenSupply = new TokenSupply();
			for (int i = 0; i < 4; i++)
			{
				tokenSupply.AddToken(new FlagToken(gameManager));
			}
			return tokenSupply;
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x00045240 File Offset: 0x00043440
		private static TokenSupply GetTogawaTokenSupply(GameManager gameManager)
		{
			TokenSupply tokenSupply = new TokenSupply();
			tokenSupply.AddToken(new TrapToken(gameManager, PayType.Coin, 4));
			tokenSupply.AddToken(new TrapToken(gameManager, PayType.Popularity, 2));
			tokenSupply.AddToken(new TrapToken(gameManager, PayType.Power, 3));
			tokenSupply.AddToken(new TrapToken(gameManager, PayType.CombatCard, 2));
			return tokenSupply;
		}
	}
}

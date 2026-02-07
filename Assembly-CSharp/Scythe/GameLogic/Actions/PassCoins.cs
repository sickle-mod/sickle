using System;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000625 RID: 1573
	public class PassCoins
	{
		// Token: 0x060031ED RID: 12781 RVA: 0x0012DAEC File Offset: 0x0012BCEC
		public static void PassCoinsBetweenPlayers(GameManager gameManager, Player from, Player to, int amount)
		{
			to.Coins += amount;
			from.Coins -= amount;
			PassCoinLogInfo passCoinLogInfo = new PassCoinLogInfo(gameManager);
			passCoinLogInfo.PlayerAssigned = from.matFaction.faction;
			passCoinLogInfo.from = from.matFaction.faction;
			passCoinLogInfo.to = to.matFaction.faction;
			passCoinLogInfo.amount = amount;
			passCoinLogInfo.ActionPlacement = ActionPositionType.Other;
			gameManager.actionLog.LogInfoReported(passCoinLogInfo);
		}
	}
}

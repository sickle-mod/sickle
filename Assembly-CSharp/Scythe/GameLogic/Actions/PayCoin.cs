using System;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000628 RID: 1576
	public class PayCoin : PayAction
	{
		// Token: 0x06003200 RID: 12800 RVA: 0x0004784E File Offset: 0x00045A4E
		public PayCoin()
			: base(0, 0, false, false)
		{
			this.payType = PayType.Coin;
		}

		// Token: 0x06003201 RID: 12801 RVA: 0x00047861 File Offset: 0x00045A61
		public PayCoin(GameManager gameManager)
			: base(gameManager, 0, 0, false, false)
		{
			this.payType = PayType.Coin;
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x00047875 File Offset: 0x00045A75
		public PayCoin(GameManager gameManager, short amount, short maxUpgradeLevel = 0, bool payed = false, bool isEncounter = false)
			: base(gameManager, amount, maxUpgradeLevel, payed, isEncounter)
		{
			this.payType = PayType.Coin;
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x0004788B File Offset: 0x00045A8B
		public override int GetMissingResourceCount()
		{
			return Math.Max(0, (int)base.Amount - this.player.Coins);
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x000478A5 File Offset: 0x00045AA5
		public override bool CanExecute()
		{
			return this.player.Coins >= (int)base.Amount;
		}

		// Token: 0x06003205 RID: 12805 RVA: 0x0012DBAC File Offset: 0x0012BDAC
		public override void Execute()
		{
			this.player.Coins -= (int)base.Amount;
			base.Payed = true;
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new PayCoinMessage(base.Amount, base.IsEncounter));
			}
		}

		// Token: 0x06003206 RID: 12806 RVA: 0x0012DC3C File Offset: 0x0012BE3C
		public override LogInfo GetLogInfo()
		{
			return new PayNonboardResourceLogInfo(this.gameManager)
			{
				Type = LogInfoType.PayCoin,
				PlayerAssigned = this.player.matFaction.faction,
				Amount = base.Amount,
				Resource = PayType.Coin,
				IsEncounter = base.IsEncounter
			};
		}
	}
}

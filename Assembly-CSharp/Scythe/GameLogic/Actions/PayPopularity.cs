using System;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x0200062B RID: 1579
	public class PayPopularity : PayAction
	{
		// Token: 0x06003217 RID: 12823 RVA: 0x000479B0 File Offset: 0x00045BB0
		public PayPopularity()
			: base(0, 0, false, false)
		{
			this.payType = PayType.Popularity;
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x000479C3 File Offset: 0x00045BC3
		public PayPopularity(GameManager gameManager)
			: base(gameManager, 0, 0, false, false)
		{
			this.payType = PayType.Popularity;
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x000479D7 File Offset: 0x00045BD7
		public PayPopularity(GameManager gameManager, short amount, short maxUpgradeLevel = 0, bool payed = false, bool isEncounter = false)
			: base(gameManager, amount, maxUpgradeLevel, payed, isEncounter)
		{
			this.payType = PayType.Popularity;
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x000479ED File Offset: 0x00045BED
		public override int GetMissingResourceCount()
		{
			return Math.Max(0, (int)base.Amount - this.player.Popularity);
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x00047A07 File Offset: 0x00045C07
		public override bool CanExecute()
		{
			return this.player.Popularity >= (int)base.Amount;
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x0012DF20 File Offset: 0x0012C120
		public override void Execute()
		{
			this.player.Popularity -= (int)base.Amount;
			base.Payed = true;
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new PayPopularityMessage(base.Amount, base.IsEncounter));
			}
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x0012DFB0 File Offset: 0x0012C1B0
		public override LogInfo GetLogInfo()
		{
			return new PayNonboardResourceLogInfo(this.gameManager)
			{
				Type = LogInfoType.PayPopularity,
				PlayerAssigned = this.player.matFaction.faction,
				Amount = base.Amount,
				Resource = PayType.Popularity,
				IsEncounter = base.IsEncounter
			};
		}
	}
}

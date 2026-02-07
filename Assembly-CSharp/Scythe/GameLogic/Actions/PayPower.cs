using System;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x0200062C RID: 1580
	public class PayPower : PayAction
	{
		// Token: 0x0600321E RID: 12830 RVA: 0x00047A1F File Offset: 0x00045C1F
		public PayPower()
			: base(0, 0, false, false)
		{
			this.payType = PayType.Power;
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x00047A32 File Offset: 0x00045C32
		public PayPower(GameManager gameManager)
			: base(gameManager, 0, 0, false, false)
		{
			this.payType = PayType.Power;
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x00047A46 File Offset: 0x00045C46
		public PayPower(GameManager gameManager, short amount, short maxUpgradeLevel = 0, bool payed = false)
			: base(gameManager, amount, maxUpgradeLevel, payed, false)
		{
			this.payType = PayType.Power;
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x00047A5B File Offset: 0x00045C5B
		public override int GetMissingResourceCount()
		{
			return Math.Max(0, (int)base.Amount - this.player.Power);
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x00047A75 File Offset: 0x00045C75
		public override bool CanExecute()
		{
			return this.player.Power >= (int)base.Amount;
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x0012E008 File Offset: 0x0012C208
		public override void Execute()
		{
			this.player.Power -= (int)base.Amount;
			base.Payed = true;
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new PayPowerMessage(base.Amount));
			}
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x0012E094 File Offset: 0x0012C294
		public override LogInfo GetLogInfo()
		{
			return new PayNonboardResourceLogInfo(this.gameManager)
			{
				Type = LogInfoType.PayPower,
				PlayerAssigned = this.player.matFaction.faction,
				Amount = base.Amount,
				Resource = PayType.Power,
				IsEncounter = base.IsEncounter
			};
		}
	}
}

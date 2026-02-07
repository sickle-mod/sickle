using System;
using System.Xml;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000627 RID: 1575
	public abstract class PayAction : BaseAction
	{
		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060031EF RID: 12783 RVA: 0x0004775F File Offset: 0x0004595F
		// (set) Token: 0x060031F0 RID: 12784 RVA: 0x00047767 File Offset: 0x00045967
		public bool Payed { get; protected set; }

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060031F1 RID: 12785 RVA: 0x00047770 File Offset: 0x00045970
		// (set) Token: 0x060031F2 RID: 12786 RVA: 0x00047778 File Offset: 0x00045978
		public bool IsEncounter { get; protected set; }

		// Token: 0x060031F3 RID: 12787 RVA: 0x00047781 File Offset: 0x00045981
		public PayAction(short amount, short maxUpgradeLevel = 0, bool payed = false, bool isEncounter = false)
			: base(amount, maxUpgradeLevel)
		{
			base.ActionType = ActionType.Pay;
			if (base.Amount < maxUpgradeLevel)
			{
				base.Amount = maxUpgradeLevel;
			}
			this.Payed = payed;
			this.IsEncounter = isEncounter;
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x000477B1 File Offset: 0x000459B1
		public PayAction(GameManager gameManager, short amount, short maxUpgradeLevel = 0, bool payed = false, bool isEncounter = false)
			: base(gameManager, amount, maxUpgradeLevel)
		{
			base.ActionType = ActionType.Pay;
			if (base.Amount < maxUpgradeLevel)
			{
				base.Amount = maxUpgradeLevel;
			}
			this.Payed = payed;
			this.IsEncounter = isEncounter;
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x000477E3 File Offset: 0x000459E3
		public PayType GetPayType()
		{
			return this.payType;
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x000477EB File Offset: 0x000459EB
		public short GetAmount()
		{
			return base.Amount;
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x000477F3 File Offset: 0x000459F3
		public void SetPayed(bool payed)
		{
			this.Payed = payed;
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x000477FC File Offset: 0x000459FC
		public bool CanPlayerPay()
		{
			return this.GetMissingResourceCount() == 0;
		}

		// Token: 0x060031F9 RID: 12793
		public abstract int GetMissingResourceCount();

		// Token: 0x060031FA RID: 12794 RVA: 0x0012DB6C File Offset: 0x0012BD6C
		public override void Upgrade()
		{
			if (base.upgradeLevel < this.maxUpgradeLevel)
			{
				short num = base.Amount;
				base.Amount = num - 1;
				num = base.upgradeLevel;
				base.upgradeLevel = num + 1;
			}
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x00047807 File Offset: 0x00045A07
		public void Reset()
		{
			this.Payed = false;
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x00047807 File Offset: 0x00045A07
		public override void Clear()
		{
			this.Payed = false;
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x00047810 File Offset: 0x00045A10
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("Paid") != null)
			{
				this.Payed = true;
			}
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x0004782D File Offset: 0x00045A2D
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.Payed)
			{
				writer.WriteAttributeString("Paid", "");
			}
		}

		// Token: 0x060031FF RID: 12799
		public abstract LogInfo GetLogInfo();

		// Token: 0x04002196 RID: 8598
		protected PayType payType;
	}
}

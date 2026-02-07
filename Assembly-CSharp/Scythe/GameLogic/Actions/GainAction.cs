using System;
using System.Xml;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x0200060D RID: 1549
	public abstract class GainAction : BaseAction
	{
		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060030AB RID: 12459 RVA: 0x00046752 File Offset: 0x00044952
		// (set) Token: 0x060030AC RID: 12460 RVA: 0x0004675A File Offset: 0x0004495A
		public bool ActionSelected { get; protected set; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060030AD RID: 12461 RVA: 0x00046763 File Offset: 0x00044963
		// (set) Token: 0x060030AE RID: 12462 RVA: 0x0004676B File Offset: 0x0004496B
		public bool IsEncounter { get; protected set; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060030AF RID: 12463 RVA: 0x00046774 File Offset: 0x00044974
		// (set) Token: 0x060030B0 RID: 12464 RVA: 0x0004677C File Offset: 0x0004497C
		public bool Gained { get; protected set; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060030B1 RID: 12465 RVA: 0x00046785 File Offset: 0x00044985
		// (set) Token: 0x060030B2 RID: 12466 RVA: 0x0004678D File Offset: 0x0004498D
		public bool IsRecruit { get; protected set; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060030B3 RID: 12467 RVA: 0x00046796 File Offset: 0x00044996
		// (set) Token: 0x060030B4 RID: 12468 RVA: 0x0004679E File Offset: 0x0004499E
		public bool IsBonusAction { get; protected set; }

		// Token: 0x060030B5 RID: 12469 RVA: 0x000467A7 File Offset: 0x000449A7
		public GainAction(short amount, short maxLevelUpgrade = 0, bool isEncounter = false, bool isRecruit = false, bool isBonusAction = false)
			: base(amount, maxLevelUpgrade)
		{
			base.ActionType = ActionType.Gain;
			this.ActionSelected = false;
			this.IsRecruit = isRecruit;
			this.IsEncounter = isEncounter;
			this.IsBonusAction = isBonusAction;
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x000467D6 File Offset: 0x000449D6
		public GainAction(GameManager gameManager, short amount, short maxLevelUpgrade = 0, bool isEncounter = false, bool isRecruit = false, bool isBonusAction = false)
			: base(gameManager, amount, maxLevelUpgrade)
		{
			base.ActionType = ActionType.Gain;
			this.ActionSelected = false;
			this.IsRecruit = isRecruit;
			this.IsEncounter = isEncounter;
			this.IsBonusAction = isBonusAction;
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x00046807 File Offset: 0x00044A07
		public GainType GetGainType()
		{
			return this.gainType;
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x000283F8 File Offset: 0x000265F8
		public virtual bool GainAvaliable()
		{
			return true;
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public virtual bool IsMaxReached()
		{
			return false;
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x00127B00 File Offset: 0x00125D00
		public override void Upgrade()
		{
			if (base.upgradeLevel < this.maxUpgradeLevel)
			{
				short num = base.Amount;
				base.Amount = num + 1;
				num = base.upgradeLevel;
				base.upgradeLevel = num + 1;
			}
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x00127B40 File Offset: 0x00125D40
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.ActionSelected = reader.GetAttribute("ActionSelected") != null;
			if (!string.IsNullOrEmpty(reader.GetAttribute("IsEncounter")))
			{
				this.IsEncounter = Convert.ToBoolean(reader.GetAttribute("IsEncounter"));
			}
			this.IsBonusAction = reader.GetAttribute("BonusAction") != null;
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x00127BA4 File Offset: 0x00125DA4
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.ActionSelected)
			{
				writer.WriteAttributeString("ActionSelected", "");
			}
			if (this.IsEncounter)
			{
				writer.WriteAttributeString("IsEncounter", this.IsEncounter.ToString());
			}
			if (this.IsBonusAction)
			{
				writer.WriteAttributeString("BonusAction", "");
			}
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x0004680F File Offset: 0x00044A0F
		public override string ToString()
		{
			return base.GetType().Name;
		}

		// Token: 0x060030BE RID: 12478
		public abstract LogInfo GetLogInfo();

		// Token: 0x04002153 RID: 8531
		protected GainType gainType;
	}
}

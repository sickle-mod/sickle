using System;
using System.Xml;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x0200061F RID: 1567
	public class GainPower : GainAction
	{
		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06003188 RID: 12680 RVA: 0x000471F9 File Offset: 0x000453F9
		// (set) Token: 0x06003189 RID: 12681 RVA: 0x00047201 File Offset: 0x00045401
		public short AmountOfPower { get; private set; }

		// Token: 0x0600318A RID: 12682 RVA: 0x0004720A File Offset: 0x0004540A
		public GainPower()
			: base(0, 0, false, false, false)
		{
			this.gainType = GainType.Power;
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x0004721E File Offset: 0x0004541E
		public GainPower(GameManager gameManager)
			: base(gameManager, 0, 0, false, false, false)
		{
			this.gainType = GainType.Power;
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x00047233 File Offset: 0x00045433
		public GainPower(GameManager gameManager, short amount, short maxLevelUpgrade = 0, bool isEncounter = false, bool isRecruit = false, bool isBonusAction = false)
			: base(gameManager, amount, maxLevelUpgrade, isEncounter, isRecruit, isBonusAction)
		{
			this.gainType = GainType.Power;
			this.AmountOfPower = amount;
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x00047252 File Offset: 0x00045452
		public bool SetPower(short amount)
		{
			if (!this.CheckLogic(amount))
			{
				return false;
			}
			this.AmountOfPower = amount;
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x0004726E File Offset: 0x0004546E
		public override bool CanExecute()
		{
			return this.CheckLogic(this.AmountOfPower);
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x00046A8E File Offset: 0x00044C8E
		private bool CheckLogic(short amount)
		{
			return amount <= base.Amount;
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x0004727C File Offset: 0x0004547C
		public override bool GainAvaliable()
		{
			return this.player.Power < 16;
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x0013093C File Offset: 0x0012EB3C
		public override LogInfo GetLogInfo()
		{
			return new GainNonboardResourceLogInfo(this.gameManager)
			{
				Type = LogInfoType.GainPower,
				IsEncounter = base.IsEncounter,
				PlayerAssigned = this.player.matFaction.faction,
				Amount = (int)this.AmountOfPower,
				Gained = this.gainType
			};
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x00130998 File Offset: 0x0012EB98
		public override void Execute()
		{
			base.Gained = true;
			this.player.Power += (int)this.AmountOfPower;
			if (this.gameManager.IsMultiplayer && !base.IsRecruit && !base.IsBonusAction && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new GainPowerMessage(this.AmountOfPower, base.IsBonusAction, base.IsEncounter));
			}
			if (!base.IsBonusAction && ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman)))
			{
				GainTopStatsEnemyActionInfo gainTopStatsEnemyActionInfo = new GainTopStatsEnemyActionInfo();
				gainTopStatsEnemyActionInfo.actionOwner = this.player.matFaction.faction;
				gainTopStatsEnemyActionInfo.resourcesToGainAmount = (int)this.AmountOfPower;
				gainTopStatsEnemyActionInfo.gainType = GainType.Power;
				gainTopStatsEnemyActionInfo.actionType = this.LogInfoType;
				this.gameManager.EnemyGainTopStat(gainTopStatsEnemyActionInfo);
			}
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x0004728D File Offset: 0x0004548D
		public override void Clear()
		{
			base.Gained = false;
			this.AmountOfPower = base.Amount;
			base.ActionSelected = false;
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x00130AD8 File Offset: 0x0012ECD8
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			string attribute = reader.GetAttribute("Power");
			if (!string.IsNullOrEmpty(attribute))
			{
				this.AmountOfPower = short.Parse(attribute);
			}
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x00130B0C File Offset: 0x0012ED0C
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			writer.WriteAttributeString("Power", this.AmountOfPower.ToString());
		}
	}
}

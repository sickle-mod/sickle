using System;
using System.Xml;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x0200061E RID: 1566
	public class GainPopularity : GainAction
	{
		// Token: 0x06003174 RID: 12660 RVA: 0x0004712D File Offset: 0x0004532D
		public GainPopularity()
			: base(0, 0, false, false, false)
		{
			this.gainType = GainType.Popularity;
			this.AmountOfPopularity = 0;
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x00047148 File Offset: 0x00045348
		public GainPopularity(GameManager gameManager)
			: base(gameManager, 0, 0, false, false, false)
		{
			this.gainType = GainType.Popularity;
			this.AmountOfPopularity = 0;
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x00047164 File Offset: 0x00045364
		public GainPopularity(GameManager gameManager, short amount, short maxLevelUpgrade = 0, bool isEncounter = false, bool isRecruit = false, bool isBonusAction = false)
			: base(gameManager, amount, maxLevelUpgrade, isEncounter, isRecruit, isBonusAction)
		{
			this.gainType = GainType.Popularity;
			this.AmountOfPopularity = amount;
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x00047183 File Offset: 0x00045383
		public bool SetPopularity(short amount)
		{
			if (!this.CheckLogic(amount))
			{
				return false;
			}
			this.AmountOfPopularity = amount;
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x0004719F File Offset: 0x0004539F
		public override bool CanExecute()
		{
			return this.CheckLogic(this.AmountOfPopularity);
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x00046A8E File Offset: 0x00044C8E
		private bool CheckLogic(short amount)
		{
			return amount <= base.Amount;
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x000471AD File Offset: 0x000453AD
		public override bool GainAvaliable()
		{
			return this.player.Popularity < 18;
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x0012BD50 File Offset: 0x00129F50
		public override LogInfo GetLogInfo()
		{
			return new GainNonboardResourceLogInfo(this.gameManager)
			{
				Type = LogInfoType.GainPopularity,
				IsEncounter = base.IsEncounter,
				PlayerAssigned = this.player.matFaction.faction,
				Amount = (int)this.AmountOfPopularity,
				Gained = this.gainType
			};
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x0012BDAC File Offset: 0x00129FAC
		public override void Execute()
		{
			base.Gained = true;
			this.player.Popularity += (int)this.AmountOfPopularity;
			if (this.gameManager.IsMultiplayer && !base.IsRecruit && !base.IsBonusAction && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new GainPopularityMessage(this.AmountOfPopularity, base.IsBonusAction, base.IsEncounter));
			}
			if (!base.IsBonusAction && ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman)))
			{
				GainTopStatsEnemyActionInfo gainTopStatsEnemyActionInfo = new GainTopStatsEnemyActionInfo();
				gainTopStatsEnemyActionInfo.actionOwner = this.player.matFaction.faction;
				gainTopStatsEnemyActionInfo.fromEncounter = base.IsEncounter;
				gainTopStatsEnemyActionInfo.resourcesToGainAmount = (int)this.AmountOfPopularity;
				gainTopStatsEnemyActionInfo.gainType = GainType.Popularity;
				gainTopStatsEnemyActionInfo.actionType = this.LogInfoType;
				this.gameManager.EnemyGainTopStat(gainTopStatsEnemyActionInfo);
			}
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x000471BE File Offset: 0x000453BE
		public override void Clear()
		{
			base.Gained = false;
			this.AmountOfPopularity = base.Amount;
			base.ActionSelected = false;
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x0012BEF8 File Offset: 0x0012A0F8
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			string attribute = reader.GetAttribute("Popularity");
			if (!string.IsNullOrEmpty(attribute))
			{
				this.AmountOfPopularity = short.Parse(attribute);
			}
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x000471DA File Offset: 0x000453DA
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			writer.WriteAttributeString("Popularity", this.AmountOfPopularity.ToString());
		}

		// Token: 0x04002180 RID: 8576
		public short AmountOfPopularity;
	}
}

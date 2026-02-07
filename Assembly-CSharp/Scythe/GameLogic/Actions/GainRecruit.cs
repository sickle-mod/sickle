using System;
using System.Xml;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000621 RID: 1569
	public class GainRecruit : GainAction
	{
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x060031A8 RID: 12712 RVA: 0x000473DA File Offset: 0x000455DA
		// (set) Token: 0x060031A9 RID: 12713 RVA: 0x000473E2 File Offset: 0x000455E2
		public DownActionType TypeOfDownAction { get; private set; }

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x060031AA RID: 12714 RVA: 0x000473EB File Offset: 0x000455EB
		// (set) Token: 0x060031AB RID: 12715 RVA: 0x000473F3 File Offset: 0x000455F3
		public GainAction Bonus { get; private set; }

		// Token: 0x060031AC RID: 12716 RVA: 0x000473FC File Offset: 0x000455FC
		public GainRecruit()
			: base(0, 0, false, false, false)
		{
			this.gainType = GainType.Recruit;
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x00047411 File Offset: 0x00045611
		public GainRecruit(GameManager gameManager)
			: base(gameManager, 0, 0, false, false, false)
		{
			this.gainType = GainType.Recruit;
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x00047427 File Offset: 0x00045627
		public GainRecruit(GameManager gameManager, short amount, short maxLevelUpgrade = 0, bool isEncounter = false)
			: base(gameManager, amount, maxLevelUpgrade, isEncounter, false, false)
		{
			this.gainType = GainType.Recruit;
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x0004743E File Offset: 0x0004563E
		public bool SetSectionAndBonus(DownActionType downActionType, GainAction bonus)
		{
			if (!this.CheckLogic(downActionType, bonus))
			{
				return false;
			}
			this.TypeOfDownAction = downActionType;
			this.Bonus = bonus;
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x00047462 File Offset: 0x00045662
		public override bool CanExecute()
		{
			return this.CheckLogic(this.TypeOfDownAction, this.Bonus);
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x0012C954 File Offset: 0x0012AB54
		private bool CheckLogic(DownActionType downActionType, GainAction bonus)
		{
			return this.GainAvaliable() && !this.player.matPlayer.GetDownAction(downActionType).RecruitEnlisted() && !this.player.matFaction.OneTimeBonusUsed(bonus.GetGainType()) && bonus.CanExecute();
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x00047476 File Offset: 0x00045676
		public override bool GainAvaliable()
		{
			return this.player.matPlayer.RecruitsEnlisted != 4;
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x0004748E File Offset: 0x0004568E
		public override bool IsMaxReached()
		{
			return this.player.matPlayer.RecruitsEnlisted == 4;
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x0012C9AC File Offset: 0x0012ABAC
		public override LogInfo GetLogInfo()
		{
			EnlistLogInfo enlistLogInfo = new EnlistLogInfo(this.gameManager);
			enlistLogInfo.Type = LogInfoType.Enlist;
			enlistLogInfo.IsEncounter = base.IsEncounter;
			enlistLogInfo.PlayerAssigned = this.player.matFaction.faction;
			if (this.GainAvaliable() && this.Bonus != null)
			{
				enlistLogInfo.TypeOfDownAction = this.TypeOfDownAction;
				enlistLogInfo.OneTimeBonus = this.Bonus.GetGainType();
			}
			else
			{
				enlistLogInfo.TypeOfDownAction = DownActionType.Factory;
			}
			return enlistLogInfo;
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x0012CA28 File Offset: 0x0012AC28
		public override void Execute()
		{
			base.Gained = true;
			this.player.matPlayer.GetDownAction(this.TypeOfDownAction).EnlistRecruit();
			if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman))
			{
				EnlistEnemyActionInfo enlistEnemyActionInfo = new EnlistEnemyActionInfo();
				enlistEnemyActionInfo.actionType = LogInfoType.Enlist;
				enlistEnemyActionInfo.actionOwner = this.player.matFaction.faction;
				enlistEnemyActionInfo.fromEncounter = base.IsEncounter;
				enlistEnemyActionInfo.oneTimeBonus = this.Bonus.GetGainType();
				if (enlistEnemyActionInfo.oneTimeBonus == GainType.CombatCard)
				{
					if (this.gameManager.CombatCardsLeft() > 2)
					{
						enlistEnemyActionInfo.resourcesToGainAmount = 2;
					}
					else
					{
						enlistEnemyActionInfo.resourcesToGainAmount = this.gameManager.CombatCardsLeft();
					}
				}
				else
				{
					enlistEnemyActionInfo.resourcesToGainAmount = 2;
				}
				enlistEnemyActionInfo.allHexes = this.gameManager.PlayerCurrent.OwnedFields(false);
				enlistEnemyActionInfo.typeOfDownAction = this.TypeOfDownAction;
				this.gameManager.EnemyEnlist(enlistEnemyActionInfo);
			}
			if (!this.gameManager.IsMultiplayer || (this.gameManager.PlayerOwner != null && (this.gameManager.PlayerOwner == this.player || this.Bonus.GetGainType() != GainType.CombatCard)) || (this.gameManager.PlayerOwner == null && (!this.player.IsHuman || this.Bonus.GetGainType() != GainType.CombatCard)))
			{
				this.Bonus.Execute();
			}
			this.player.matFaction.OneTimeBonuses[this.Bonus.GetGainType()] = true;
			this.player.matPlayer.IncrementEnlistedRecruitsCounter();
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new GainRecruitMessage((int)this.Bonus.GetGainType(), (int)this.TypeOfDownAction, base.IsEncounter));
			}
			this.player.matFaction.CheckRecruitStar();
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x0004711D File Offset: 0x0004531D
		public override void Clear()
		{
			base.Gained = false;
			base.ActionSelected = false;
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x0012CC6C File Offset: 0x0012AE6C
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("Bonus") == null)
			{
				return;
			}
			switch (int.Parse(reader.GetAttribute("Bonus")))
			{
			case 0:
				this.Bonus = new GainCoin(this.gameManager, 2, 0, false, true, false);
				break;
			case 1:
				this.Bonus = new GainPopularity(this.gameManager, 2, 0, false, true, false);
				break;
			case 2:
				this.Bonus = new GainPower(this.gameManager, 2, 0, false, true, false);
				break;
			case 3:
				this.Bonus = new GainCombatCard(this.gameManager, 2, 0, false, true, false);
				break;
			}
			this.Bonus.SetPlayer(this.player);
			this.TypeOfDownAction = (DownActionType)int.Parse(reader.GetAttribute("DownType"));
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x0012CD3C File Offset: 0x0012AF3C
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.Bonus != null)
			{
				writer.WriteAttributeString("Bonus", ((int)this.Bonus.GetGainType()).ToString());
				writer.WriteAttributeString("DownType", ((int)this.TypeOfDownAction).ToString());
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Xml;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000611 RID: 1553
	public class GainCombatCard : GainAction
	{
		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060030F2 RID: 12530 RVA: 0x00046AE5 File Offset: 0x00044CE5
		// (set) Token: 0x060030F3 RID: 12531 RVA: 0x00046AED File Offset: 0x00044CED
		public short AmountOfCards { get; private set; }

		// Token: 0x060030F4 RID: 12532 RVA: 0x00046AF6 File Offset: 0x00044CF6
		public GainCombatCard()
			: base(0, 0, false, false, false)
		{
			this.gainType = GainType.CombatCard;
			this.AmountOfCards = 0;
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x00046B11 File Offset: 0x00044D11
		public GainCombatCard(GameManager gameManager)
			: base(gameManager, 0, 0, false, false, false)
		{
			this.gainType = GainType.CombatCard;
			this.AmountOfCards = 0;
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x00046B2D File Offset: 0x00044D2D
		public GainCombatCard(GameManager gameManager, short amount, short maxLevelUpgrade = 0, bool isEncounter = false, bool isRecruit = false, bool isBonusAction = false)
			: base(gameManager, amount, maxLevelUpgrade, isEncounter, isRecruit, isBonusAction)
		{
			this.gainType = GainType.CombatCard;
			this.AmountOfCards = amount;
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x00046B4C File Offset: 0x00044D4C
		public bool SetCards(short amount)
		{
			if (!this.CheckLogic(amount))
			{
				return false;
			}
			this.AmountOfCards = amount;
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x00046B68 File Offset: 0x00044D68
		public override bool CanExecute()
		{
			return this.CheckLogic(this.AmountOfCards);
		}

		// Token: 0x060030F9 RID: 12537 RVA: 0x00046B76 File Offset: 0x00044D76
		public override bool GainAvaliable()
		{
			return this.gameManager.CombatCardsLeft() > 0;
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x00046A8E File Offset: 0x00044C8E
		public bool CheckLogic(short amount)
		{
			return amount <= base.Amount;
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x00128BFC File Offset: 0x00126DFC
		public override LogInfo GetLogInfo()
		{
			return new GainNonboardResourceLogInfo(this.gameManager)
			{
				Type = LogInfoType.GainCombatCard,
				IsEncounter = base.IsEncounter,
				PlayerAssigned = this.player.matFaction.faction,
				Amount = (int)this.AmountOfCards,
				Gained = this.gainType
			};
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x00128C58 File Offset: 0x00126E58
		public override void Execute()
		{
			base.Gained = true;
			if (!this.gameManager.IsMultiplayer)
			{
				List<CombatCard> combatCards = this.gameManager.GetCombatCards((int)this.AmountOfCards);
				foreach (CombatCard combatCard in combatCards)
				{
					this.player.AddCombatCard(combatCard);
				}
				if (!base.IsBonusAction)
				{
					this.CreateEnemyActionInfo(combatCards.Count);
				}
				return;
			}
			GainCombatCard.CombatCardGainType combatCardGainType = GainCombatCard.CombatCardGainType.Bolster;
			if (base.IsRecruit)
			{
				combatCardGainType = GainCombatCard.CombatCardGainType.Recruit;
			}
			else if (base.IsBonusAction)
			{
				combatCardGainType = GainCombatCard.CombatCardGainType.OngoingBonus;
			}
			if (base.IsBonusAction)
			{
				this.gameManager.SendCards((int)this.AmountOfCards, (int)this.player.matFaction.faction, combatCardGainType);
				return;
			}
			if (this.player.IsHuman)
			{
				if (this.gameManager.PlayerOwner != null && this.player == this.gameManager.PlayerOwner)
				{
					this.gameManager.OnGainCombatCards(this.AmountOfCards, combatCardGainType);
				}
				return;
			}
			this.gameManager.SendCards((int)this.AmountOfCards, (int)this.player.matFaction.faction, combatCardGainType);
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x00128D90 File Offset: 0x00126F90
		private void CreateEnemyActionInfo(int collectedCardAmount)
		{
			if (this.gameManager.CombatCardsLeft() == 0)
			{
				return;
			}
			if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman))
			{
				GainTopStatsEnemyActionInfo gainTopStatsEnemyActionInfo = new GainTopStatsEnemyActionInfo();
				gainTopStatsEnemyActionInfo.actionOwner = this.player.matFaction.faction;
				gainTopStatsEnemyActionInfo.fromEncounter = base.IsEncounter;
				gainTopStatsEnemyActionInfo.resourcesToGainAmount = collectedCardAmount;
				gainTopStatsEnemyActionInfo.gainType = GainType.CombatCard;
				gainTopStatsEnemyActionInfo.actionType = this.LogInfoType;
				this.gameManager.EnemyGainTopStat(gainTopStatsEnemyActionInfo);
			}
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x00046B86 File Offset: 0x00044D86
		public override void Clear()
		{
			base.Gained = false;
			this.AmountOfCards = base.Amount;
			base.ActionSelected = false;
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x00046BA2 File Offset: 0x00044DA2
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.AmountOfCards = base.Amount;
		}

		// Token: 0x02000612 RID: 1554
		public enum CombatCardGainType
		{
			// Token: 0x0400215C RID: 8540
			Bolster,
			// Token: 0x0400215D RID: 8541
			Recruit,
			// Token: 0x0400215E RID: 8542
			OngoingBonus,
			// Token: 0x0400215F RID: 8543
			Combat
		}
	}
}

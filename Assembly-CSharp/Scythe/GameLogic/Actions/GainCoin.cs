using System;
using System.Xml;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000610 RID: 1552
	public class GainCoin : GainAction
	{
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060030E5 RID: 12517 RVA: 0x00046A0B File Offset: 0x00044C0B
		// (set) Token: 0x060030E6 RID: 12518 RVA: 0x00046A13 File Offset: 0x00044C13
		public short AmountOfCoins { get; private set; }

		// Token: 0x060030E7 RID: 12519 RVA: 0x00046A1C File Offset: 0x00044C1C
		public GainCoin()
			: base(0, 0, false, false, false)
		{
			this.gainType = GainType.Coin;
			this.AmountOfCoins = 0;
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x00046A37 File Offset: 0x00044C37
		public GainCoin(GameManager gameManager)
			: base(gameManager, 0, 0, false, false, false)
		{
			this.gainType = GainType.Coin;
			this.AmountOfCoins = 0;
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x00046A53 File Offset: 0x00044C53
		public GainCoin(GameManager gameManager, short amount, short maxLevelUpgrade = 0, bool isEncounter = false, bool isRecruit = false, bool isBonusAction = false)
			: base(gameManager, amount, maxLevelUpgrade, isEncounter, isRecruit, isBonusAction)
		{
			this.gainType = GainType.Coin;
			this.AmountOfCoins = amount;
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x00046A72 File Offset: 0x00044C72
		public bool SetCoins(short amount)
		{
			if (!this.CheckLogic(amount))
			{
				return false;
			}
			this.AmountOfCoins = amount;
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x00046A8E File Offset: 0x00044C8E
		private bool CheckLogic(short amount)
		{
			return amount <= base.Amount;
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x00046A9C File Offset: 0x00044C9C
		public override bool CanExecute()
		{
			return this.CheckLogic(this.AmountOfCoins);
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x00128A38 File Offset: 0x00126C38
		public override LogInfo GetLogInfo()
		{
			return new GainNonboardResourceLogInfo(this.gameManager)
			{
				Type = LogInfoType.GainCoin,
				IsEncounter = base.IsEncounter,
				PlayerAssigned = this.player.matFaction.faction,
				Amount = (int)this.AmountOfCoins,
				Gained = this.gainType
			};
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x00128A94 File Offset: 0x00126C94
		public override void Execute()
		{
			base.Gained = true;
			this.player.Coins += (int)this.AmountOfCoins;
			if (this.gameManager.IsMultiplayer && !base.IsRecruit && !base.IsBonusAction && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new GainCoinMessage(this.AmountOfCoins, base.IsEncounter));
			}
			if (!base.IsBonusAction && ((this.gameManager.IsMultiplayer && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman)))
			{
				GainTopStatsEnemyActionInfo gainTopStatsEnemyActionInfo = new GainTopStatsEnemyActionInfo();
				gainTopStatsEnemyActionInfo.actionOwner = this.player.matFaction.faction;
				gainTopStatsEnemyActionInfo.fromEncounter = base.IsEncounter;
				gainTopStatsEnemyActionInfo.resourcesToGainAmount = (int)this.AmountOfCoins;
				gainTopStatsEnemyActionInfo.gainType = GainType.Coin;
				gainTopStatsEnemyActionInfo.actionType = this.LogInfoType;
				this.gameManager.EnemyGainTopStat(gainTopStatsEnemyActionInfo);
			}
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x00046AAA File Offset: 0x00044CAA
		public override void Clear()
		{
			base.Gained = false;
			this.AmountOfCoins = base.Amount;
			base.ActionSelected = false;
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x00046AC6 File Offset: 0x00044CC6
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.AmountOfCoins = short.Parse(reader.GetAttribute("Coin"));
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x00128BCC File Offset: 0x00126DCC
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			writer.WriteAttributeString("Coin", this.AmountOfCoins.ToString());
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic
{
	// Token: 0x020005BA RID: 1466
	public class TrapToken : FactionAbilityToken, IXmlSerializable
	{
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06002EBC RID: 11964 RVA: 0x0004527F File Offset: 0x0004347F
		// (set) Token: 0x06002EBD RID: 11965 RVA: 0x00045287 File Offset: 0x00043487
		public bool Armed { get; private set; }

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06002EBE RID: 11966 RVA: 0x00045290 File Offset: 0x00043490
		// (set) Token: 0x06002EBF RID: 11967 RVA: 0x00045298 File Offset: 0x00043498
		public PayType Penalty { get; private set; }

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06002EC0 RID: 11968 RVA: 0x000452A1 File Offset: 0x000434A1
		// (set) Token: 0x06002EC1 RID: 11969 RVA: 0x000452A9 File Offset: 0x000434A9
		public int Amount { get; private set; }

		// Token: 0x06002EC2 RID: 11970 RVA: 0x00044F56 File Offset: 0x00043156
		public TrapToken(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x000452B2 File Offset: 0x000434B2
		public TrapToken(GameManager gameManager, PayType penalty, int amount)
			: base(gameManager)
		{
			this.Penalty = penalty;
			this.Amount = amount;
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x000452C9 File Offset: 0x000434C9
		public override void OnTokenPlaced()
		{
			this.Armed = true;
			if (this.gameManager.IsMyTurn() && !this.gameManager.GameLoading)
			{
				this.gameManager.OnActionSent(new TrapTokenPlacedMessage(base.Position, this.Penalty));
			}
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x00045308 File Offset: 0x00043508
		public void OnTokenTriggered(Unit initiator)
		{
			if (this.Armed && initiator.Owner != base.Owner)
			{
				this.ExecutePenalty(initiator.Owner);
				this.Armed = false;
				this.gameManager.tokenManager.TrapTriggered(this, initiator);
			}
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x00045345 File Offset: 0x00043545
		public void OnTokenArmed()
		{
			if (!this.Armed)
			{
				this.Armed = true;
				if (this.gameManager.IsMyTurn())
				{
					this.gameManager.OnActionSent(new TrapTokenArmedMessage(base.Position));
				}
			}
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x0011BAAC File Offset: 0x00119CAC
		private void ExecutePenalty(Player player)
		{
			switch (this.Penalty)
			{
			case PayType.Coin:
				this.RemoveCoins(player);
				return;
			case PayType.Popularity:
				this.RemovePopularity(player);
				return;
			case PayType.Power:
				this.RemovePower(player);
				return;
			case PayType.CombatCard:
				this.RemoveCards(player);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x00045379 File Offset: 0x00043579
		private void RemoveCoins(Player player)
		{
			player.Coins -= this.Amount;
			if (player.Coins < 0)
			{
				player.Coins = 0;
			}
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x0004539E File Offset: 0x0004359E
		private void RemovePopularity(Player player)
		{
			player.Popularity -= this.Amount;
			if (player.Popularity < 0)
			{
				player.Popularity = 0;
			}
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x000453C3 File Offset: 0x000435C3
		private void RemovePower(Player player)
		{
			player.Power -= this.Amount;
			if (player.Power < 0)
			{
				player.Power = 0;
			}
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x0011BAF8 File Offset: 0x00119CF8
		private void RemoveCards(Player player)
		{
			if (this.gameManager.IsMyTurn())
			{
				List<CombatCard> list = new List<CombatCard>(this.Amount);
				int num = 0;
				while (num < this.Amount && player.GetCombatCardsCount() != 0)
				{
					int num2 = this.gameManager.random.Next(0, player.GetCombatCardsCount());
					CombatCard combatCard = player.combatCards[num2];
					list.Add(combatCard);
					player.RemoveCombatCard(combatCard);
					this.gameManager.AddUsedCombatCard(combatCard);
					num++;
				}
				this.gameManager.OnActionSent(new CardsRemovedMessage(list, player));
			}
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x0011BB88 File Offset: 0x00119D88
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[TrapToken] ");
			stringBuilder.Append(base.ToString());
			stringBuilder.Append(" Armed: ");
			stringBuilder.Append(this.Armed.ToString());
			stringBuilder.Append(". Penalty: -");
			stringBuilder.Append(this.Amount);
			stringBuilder.Append(" ");
			stringBuilder.Append(this.Penalty);
			stringBuilder.Append(".");
			return stringBuilder.ToString();
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x0011B224 File Offset: 0x00119424
		public override LogInfo CreateLogInfo(TokenActionType actionType, Unit unit)
		{
			return new TokenActionLogInfo(this.gameManager)
			{
				Type = LogInfoType.TokenAction,
				PlayerAssigned = base.Owner.matFaction.faction,
				ActionPlacement = ActionPositionType.Other,
				token = this,
				action = actionType,
				unit = unit
			};
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public override XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x000453E8 File Offset: 0x000435E8
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.Armed = reader.GetAttribute("Armed") != null;
			this.Penalty = (PayType)int.Parse(reader["TrapType"]);
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x0011BC1C File Offset: 0x00119E1C
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("TrapToken");
			base.WriteXml(writer);
			if (this.Armed)
			{
				writer.WriteAttributeString("Armed", "");
			}
			writer.WriteAttributeString("TrapType", ((int)this.Penalty).ToString());
			writer.WriteEndElement();
		}
	}
}

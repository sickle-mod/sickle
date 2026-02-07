using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000629 RID: 1577
	public class PayCombatCard : PayAction
	{
		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06003207 RID: 12807 RVA: 0x000478BD File Offset: 0x00045ABD
		// (set) Token: 0x06003208 RID: 12808 RVA: 0x000478C5 File Offset: 0x00045AC5
		public List<CombatCard> Payment { get; private set; }

		// Token: 0x06003209 RID: 12809 RVA: 0x000478CE File Offset: 0x00045ACE
		public PayCombatCard()
			: base(0, 0, false, false)
		{
			this.payType = PayType.CombatCard;
			this.Payment = new List<CombatCard>();
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x000478EC File Offset: 0x00045AEC
		public PayCombatCard(GameManager gameManager)
			: base(gameManager, 0, 0, false, false)
		{
			this.payType = PayType.CombatCard;
			this.Payment = new List<CombatCard>();
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x0004790B File Offset: 0x00045B0B
		public PayCombatCard(GameManager gameManager, short amount, short maxUpgradeLevel = 0, bool payed = false, bool asResource = false)
			: base(gameManager, amount, maxUpgradeLevel, payed, false)
		{
			this.payType = PayType.CombatCard;
			this.Payment = new List<CombatCard>();
			this.payAsResource = asResource;
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x00047933 File Offset: 0x00045B33
		public override int GetMissingResourceCount()
		{
			return Math.Max(0, (int)base.Amount - this.player.combatCards.Count);
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x00047952 File Offset: 0x00045B52
		public bool SetCombatCards(List<CombatCard> cards)
		{
			if (!this.CheckLogic(cards))
			{
				return false;
			}
			this.Payment = cards;
			return true;
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x00047967 File Offset: 0x00045B67
		public override bool CanExecute()
		{
			return this.CheckLogic(this.Payment);
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x00047975 File Offset: 0x00045B75
		private bool CheckLogic(List<CombatCard> cards)
		{
			return cards != null && cards.Count == (int)base.Amount;
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x0012DC94 File Offset: 0x0012BE94
		public override void Execute()
		{
			foreach (CombatCard combatCard in this.Payment)
			{
				this.player.combatCards.Remove(combatCard);
				this.gameManager.AddUsedCombatCard(combatCard);
			}
			base.Payed = true;
			if (!this.payAsResource && this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new PayCombatCardMessage(this.Payment));
			}
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x0004798D File Offset: 0x00045B8D
		public override void Clear()
		{
			this.Payment.Clear();
			base.Clear();
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x0012DD6C File Offset: 0x0012BF6C
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("AsResource") != null)
			{
				this.payAsResource = true;
			}
			reader.ReadStartElement();
			if (reader.Name == "Cards")
			{
				string[] array = reader.ReadElementContentAsString().Split(' ', StringSplitOptions.None);
				if (array[0] == " ")
				{
					return;
				}
				List<CombatCard> list = new List<CombatCard>(this.player.combatCards);
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					if (text == "")
					{
						break;
					}
					int id = int.Parse(text);
					CombatCard combatCard = list.Find((CombatCard c) => c.CombatBonus == id);
					this.Payment.Add(combatCard);
					list.Remove(combatCard);
				}
			}
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x0012DE44 File Offset: 0x0012C044
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.payAsResource)
			{
				writer.WriteAttributeString("AsResource", "");
			}
			writer.WriteStartElement("Cards");
			foreach (CombatCard combatCard in this.Payment)
			{
				((IXmlSerializable)combatCard).WriteXml(writer);
			}
			writer.WriteEndElement();
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x0012DEC8 File Offset: 0x0012C0C8
		public override LogInfo GetLogInfo()
		{
			return new PayNonboardResourceLogInfo(this.gameManager)
			{
				Type = LogInfoType.PayCombatCard,
				PlayerAssigned = this.player.matFaction.faction,
				Amount = base.Amount,
				Resource = PayType.CombatCard,
				IsEncounter = base.IsEncounter
			};
		}

		// Token: 0x0400219A RID: 8602
		private bool payAsResource;
	}
}

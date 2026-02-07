using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scythe.GameLogic
{
	// Token: 0x020005A7 RID: 1447
	public class CardDeck<T> : IXmlSerializable where T : Card
	{
		// Token: 0x06002DE4 RID: 11748 RVA: 0x000449CD File Offset: 0x00042BCD
		public CardDeck(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000449E7 File Offset: 0x00042BE7
		public void AddCard(T card)
		{
			this.cards.Add(card);
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x000449F5 File Offset: 0x00042BF5
		public void AddCards(List<T> cards)
		{
			cards.AddRange(cards);
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000449FE File Offset: 0x00042BFE
		public void ClearDeck()
		{
			this.cards.Clear();
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x00044A0B File Offset: 0x00042C0B
		public int CardsLeft()
		{
			return this.cards.Count;
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x00115F7C File Offset: 0x0011417C
		public T GetCard()
		{
			if (this.cards.Count == 0)
			{
				return default(T);
			}
			T t = this.cards[0];
			this.cards.RemoveAt(0);
			return t;
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x00115FB8 File Offset: 0x001141B8
		public T LookupCard(int id)
		{
			if (this.cards.Count == 0 || id < 0 || id >= this.cards.Count)
			{
				return default(T);
			}
			return this.cards[id];
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x00115FFC File Offset: 0x001141FC
		public List<T> GetCards(int amount)
		{
			List<T> list = new List<T>();
			while (amount > 0)
			{
				T card = this.GetCard();
				if (card == null)
				{
					break;
				}
				list.Add(card);
				amount--;
			}
			return list;
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x00116034 File Offset: 0x00114234
		public void ShuffleDeck()
		{
			for (int i = 0; i < this.cards.Count; i++)
			{
				T t = this.cards[i];
				int num = this.gameManager.random.Next(i, this.cards.Count);
				this.cards[i] = this.cards[num];
				this.cards[num] = t;
			}
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x001160A8 File Offset: 0x001142A8
		public string Serialize()
		{
			string text = "";
			foreach (T t in this.cards)
			{
				Card card = t;
				text += card.Serialize();
			}
			return text;
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void ReadXml(XmlReader reader)
		{
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x00116110 File Offset: 0x00114310
		public void WriteXml(XmlWriter writer)
		{
			foreach (T t in this.cards)
			{
				t.WriteXml(writer);
			}
		}

		// Token: 0x04001F2F RID: 7983
		private List<T> cards = new List<T>();

		// Token: 0x04001F30 RID: 7984
		private GameManager gameManager;
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scythe.GameLogic
{
	// Token: 0x020005B8 RID: 1464
	public class TokenSupply : IXmlSerializable
	{
		// Token: 0x06002EAB RID: 11947 RVA: 0x0011B818 File Offset: 0x00119A18
		public void SetOwner(Player owner)
		{
			for (int i = 0; i < this.factionAbilityTokens.Count; i++)
			{
				this.factionAbilityTokens[i].SetOwner(owner);
			}
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x000451C9 File Offset: 0x000433C9
		public void AddToken(FactionAbilityToken token)
		{
			if (token != null)
			{
				this.factionAbilityTokens.Add(token);
			}
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x000451DA File Offset: 0x000433DA
		public int GetTokensCount()
		{
			return this.factionAbilityTokens.Count;
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x0011B850 File Offset: 0x00119A50
		public int GetPlacedTokensCount()
		{
			int num = 0;
			for (int i = 0; i < this.GetTokensCount(); i++)
			{
				if (this.GetToken(i).IsTokenPlaced())
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x000451E7 File Offset: 0x000433E7
		public int GetUnplacedTokensCount()
		{
			return this.GetTokensCount() - this.GetPlacedTokensCount();
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000451F6 File Offset: 0x000433F6
		public FactionAbilityToken GetToken(int id)
		{
			return this.factionAbilityTokens[id];
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x00045204 File Offset: 0x00043404
		public int GetTokenId(FactionAbilityToken token)
		{
			return this.factionAbilityTokens.IndexOf(token);
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x0011B884 File Offset: 0x00119A84
		public FactionAbilityToken GetFirstUnplacedToken()
		{
			for (int i = 0; i < this.GetTokensCount(); i++)
			{
				if (!this.GetToken(i).IsTokenPlaced())
				{
					return this.factionAbilityTokens[i];
				}
			}
			return null;
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x0011B8C0 File Offset: 0x00119AC0
		public List<FactionAbilityToken> GetPlacedTokens()
		{
			List<FactionAbilityToken> list = new List<FactionAbilityToken>();
			for (int i = 0; i < this.GetTokensCount(); i++)
			{
				if (this.GetToken(i).IsTokenPlaced())
				{
					list.Add(this.GetToken(i));
				}
			}
			return list;
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x0011B900 File Offset: 0x00119B00
		public override string ToString()
		{
			if (this.GetTokensCount() == 0)
			{
				return "[TokenSupply] Empty token supply.";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[TokenSupply] Token count: " + this.GetTokensCount().ToString());
			stringBuilder.Append(". Owner: ");
			stringBuilder.Append(this.GetToken(0).Owner.matFaction.faction.ToString());
			for (int i = 0; i < this.GetTokensCount(); i++)
			{
				stringBuilder.Append("\n");
				stringBuilder.Append(this.GetToken(i).ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x0011B9AC File Offset: 0x00119BAC
		public void ReadXml(XmlReader reader)
		{
			int num = int.Parse(reader["TokenCount"]);
			for (int i = 0; i < num; i++)
			{
				reader.Read();
				FactionAbilityToken token = this.GetToken(i);
				if (reader.Name == "TrapToken")
				{
					((IXmlSerializable)token).ReadXml(reader);
				}
				else if (reader.Name == "FlagToken")
				{
					((IXmlSerializable)token).ReadXml(reader);
				}
			}
			reader.Read();
			reader.ReadEndElement();
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x0011BA28 File Offset: 0x00119C28
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("TokenSupply");
			writer.WriteAttributeString("TokenCount", this.GetTokensCount().ToString());
			for (int i = 0; i < this.GetTokensCount(); i++)
			{
				((IXmlSerializable)this.GetToken(i)).WriteXml(writer);
			}
			writer.WriteEndElement();
		}

		// Token: 0x04001F68 RID: 8040
		private List<FactionAbilityToken> factionAbilityTokens = new List<FactionAbilityToken>();
	}
}

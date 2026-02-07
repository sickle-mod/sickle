using System;
using System.Xml;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005D3 RID: 1491
	public class GainNonboardResourceLogInfo : LogInfo
	{
		// Token: 0x06002F4E RID: 12110 RVA: 0x0004569B File Offset: 0x0004389B
		public GainNonboardResourceLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x0012043C File Offset: 0x0011E63C
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("Type") != null)
			{
				this.Type = (LogInfoType)int.Parse(reader.GetAttribute("Type"));
			}
			else
			{
				this.Type = LogInfoType.GainCoin;
			}
			this.Amount = int.Parse(reader.GetAttribute("Amount"));
			switch (this.Type)
			{
			case LogInfoType.GainCoin:
				this.Gained = GainType.Coin;
				break;
			case LogInfoType.GainPopularity:
			case LogInfoType.TradePopularity:
				this.Gained = GainType.Popularity;
				break;
			case LogInfoType.GainPower:
			case LogInfoType.BolsterPower:
				this.Gained = GainType.Power;
				break;
			case LogInfoType.GainCombatCard:
			case LogInfoType.BolsterCombatCard:
				this.Gained = GainType.CombatCard;
				break;
			}
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x001204F8 File Offset: 0x0011E6F8
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L9");
			base.WriteXml(writer);
			if (this.Type != LogInfoType.GainCoin)
			{
				string text = "Type";
				int type = (int)this.Type;
				writer.WriteAttributeString(text, type.ToString());
			}
			writer.WriteAttributeString("Amount", this.Amount.ToString());
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x04002011 RID: 8209
		public GainType Gained;

		// Token: 0x04002012 RID: 8210
		public int Amount;
	}
}

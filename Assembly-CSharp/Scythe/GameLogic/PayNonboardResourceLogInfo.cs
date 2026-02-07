using System;
using System.Xml;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005CB RID: 1483
	public class PayNonboardResourceLogInfo : LogInfo
	{
		// Token: 0x06002F36 RID: 12086 RVA: 0x0004569B File Offset: 0x0004389B
		public PayNonboardResourceLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x0011F1C0 File Offset: 0x0011D3C0
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("Type") != null)
			{
				this.Type = (LogInfoType)int.Parse(reader.GetAttribute("Type"));
			}
			else
			{
				this.Type = LogInfoType.PayCoin;
			}
			this.Amount = short.Parse(reader.GetAttribute("Amount"));
			switch (this.Type)
			{
			case LogInfoType.PayCoin:
				this.Resource = PayType.Coin;
				break;
			case LogInfoType.PayPopularity:
				this.Resource = PayType.Popularity;
				break;
			case LogInfoType.PayPower:
				this.Resource = PayType.Power;
				break;
			case LogInfoType.PayCombatCard:
				this.Resource = PayType.CombatCard;
				break;
			}
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x0011F264 File Offset: 0x0011D464
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L1");
			base.WriteXml(writer);
			if (this.Type != LogInfoType.PayCoin)
			{
				string text = "Type";
				int type = (int)this.Type;
				writer.WriteAttributeString(text, type.ToString());
			}
			writer.WriteAttributeString("Amount", this.Amount.ToString());
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x04001FF7 RID: 8183
		public PayType Resource;

		// Token: 0x04001FF8 RID: 8184
		public short Amount;
	}
}

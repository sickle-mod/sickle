using System;
using System.Xml;

namespace Scythe.GameLogic
{
	// Token: 0x020005D9 RID: 1497
	public class PassCoinLogInfo : LogInfo
	{
		// Token: 0x06002F60 RID: 12128 RVA: 0x00045724 File Offset: 0x00043924
		public PassCoinLogInfo(GameManager gameManager)
			: base(gameManager)
		{
			this.Type = LogInfoType.PassCoins;
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x00120B38 File Offset: 0x0011ED38
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.Type = LogInfoType.PassCoins;
			this.from = (Faction)int.Parse(reader.GetAttribute("From"));
			this.to = (Faction)int.Parse(reader.GetAttribute("To"));
			this.amount = int.Parse(reader.GetAttribute("Amount"));
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x00120BA0 File Offset: 0x0011EDA0
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L15");
			base.WriteXml(writer);
			string text = "From";
			int num = (int)this.from;
			writer.WriteAttributeString(text, num.ToString());
			string text2 = "To";
			num = (int)this.to;
			writer.WriteAttributeString(text2, num.ToString());
			writer.WriteAttributeString("Amount", this.amount.ToString());
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x0400201D RID: 8221
		public Faction from;

		// Token: 0x0400201E RID: 8222
		public Faction to;

		// Token: 0x0400201F RID: 8223
		public int amount;
	}
}

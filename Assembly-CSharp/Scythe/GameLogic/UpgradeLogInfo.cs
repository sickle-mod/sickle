using System;
using System.Xml;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005CE RID: 1486
	public class UpgradeLogInfo : LogInfo
	{
		// Token: 0x06002F3F RID: 12095 RVA: 0x0004569B File Offset: 0x0004389B
		public UpgradeLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x0011FE88 File Offset: 0x0011E088
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.Type = LogInfoType.Upgrade;
			this.DownAction = (PayType)int.Parse(reader.GetAttribute("DownAction"));
			if (this.DownAction != PayType.Coin)
			{
				this.TopAction = (GainType)int.Parse(reader.GetAttribute("TopAction"));
				this.Resource = (ResourceType)int.Parse(reader.GetAttribute("Resource"));
			}
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x0011FEF8 File Offset: 0x0011E0F8
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L4");
			base.WriteXml(writer);
			string text = "DownAction";
			int num = (int)this.DownAction;
			writer.WriteAttributeString(text, num.ToString());
			if (this.DownAction != PayType.Coin)
			{
				string text2 = "TopAction";
				num = (int)this.TopAction;
				writer.WriteAttributeString(text2, num.ToString());
				string text3 = "Resource";
				num = (int)this.Resource;
				writer.WriteAttributeString(text3, num.ToString());
			}
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x04002005 RID: 8197
		public GainType TopAction;

		// Token: 0x04002006 RID: 8198
		public PayType DownAction;

		// Token: 0x04002007 RID: 8199
		public ResourceType Resource;
	}
}

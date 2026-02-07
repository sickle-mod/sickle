using System;
using System.Xml;

namespace Scythe.GameLogic
{
	// Token: 0x020005D7 RID: 1495
	public class StarLogInfo : LogInfo
	{
		// Token: 0x06002F5A RID: 12122 RVA: 0x0004569B File Offset: 0x0004389B
		public StarLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x001207B4 File Offset: 0x0011E9B4
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.Type = LogInfoType.GainStar;
			this.GainedStar = (StarType)int.Parse(reader.GetAttribute("StarType"));
			this.starsUnlocked = int.Parse(reader.GetAttribute("Stars"));
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x00120804 File Offset: 0x0011EA04
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L13");
			base.WriteXml(writer);
			string text = "StarType";
			int gainedStar = (int)this.GainedStar;
			writer.WriteAttributeString(text, gainedStar.ToString());
			writer.WriteAttributeString("Stars", this.starsUnlocked.ToString());
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x04002018 RID: 8216
		public StarType GainedStar;

		// Token: 0x04002019 RID: 8217
		public int starsUnlocked;
	}
}

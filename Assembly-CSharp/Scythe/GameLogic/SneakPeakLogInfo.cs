using System;
using System.Xml;

namespace Scythe.GameLogic
{
	// Token: 0x020005D6 RID: 1494
	public class SneakPeakLogInfo : LogInfo
	{
		// Token: 0x06002F57 RID: 12119 RVA: 0x0004569B File Offset: 0x0004389B
		public SneakPeakLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x000456F6 File Offset: 0x000438F6
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.Type = LogInfoType.SpyPlayer;
			this.SpiedFaction = (Faction)int.Parse(reader.GetAttribute("Spied"));
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x0012076C File Offset: 0x0011E96C
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L12");
			base.WriteXml(writer);
			string text = "Spied";
			int spiedFaction = (int)this.SpiedFaction;
			writer.WriteAttributeString(text, spiedFaction.ToString());
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x04002017 RID: 8215
		public Faction SpiedFaction;
	}
}

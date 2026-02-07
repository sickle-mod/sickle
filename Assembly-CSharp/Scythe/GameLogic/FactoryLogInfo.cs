using System;
using System.Xml;

namespace Scythe.GameLogic
{
	// Token: 0x020005D4 RID: 1492
	public class FactoryLogInfo : LogInfo
	{
		// Token: 0x06002F51 RID: 12113 RVA: 0x0004569B File Offset: 0x0004389B
		public FactoryLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x0012055C File Offset: 0x0011E75C
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("Type") != null)
			{
				this.Type = (LogInfoType)int.Parse(reader.GetAttribute("Type"));
			}
			else
			{
				this.Type = LogInfoType.FactoryCardGain;
			}
			this.Character = this.gameManager.GetPlayerByFaction(this.PlayerAssigned).character;
			this.GainedFactoryCard = (FactoryCard)this.gameManager.GetPlayerByFaction(this.PlayerAssigned).matPlayer.GetPlayerMatSection(4);
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x001205E8 File Offset: 0x0011E7E8
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L10");
			base.WriteXml(writer);
			if (this.Type != LogInfoType.FactoryCardGain)
			{
				string text = "Type";
				int type = (int)this.Type;
				writer.WriteAttributeString(text, type.ToString());
			}
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x04002013 RID: 8211
		public Unit Character;

		// Token: 0x04002014 RID: 8212
		public FactoryCard GainedFactoryCard;
	}
}

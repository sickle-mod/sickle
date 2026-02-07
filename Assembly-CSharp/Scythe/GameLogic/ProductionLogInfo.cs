using System;
using System.Collections.Generic;
using System.Xml;

namespace Scythe.GameLogic
{
	// Token: 0x020005D2 RID: 1490
	public class ProductionLogInfo : LogInfo
	{
		// Token: 0x06002F4B RID: 12107 RVA: 0x000456E2 File Offset: 0x000438E2
		public ProductionLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x001202A4 File Offset: 0x0011E4A4
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.Type = LogInfoType.Produce;
			this.MillUsed = reader.GetAttribute("MillUsed") != null;
			reader.ReadStartElement();
			while (reader.Name == "Hex")
			{
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				int num3 = int.Parse(reader.GetAttribute("Amount"));
				this.Hexes.Add(this.gameManager.gameBoard.hexMap[num, num2], num3);
				reader.ReadStartElement();
			}
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x00120354 File Offset: 0x0011E554
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L8");
			if (this.MillUsed)
			{
				writer.WriteAttributeString("MillUsed", "'");
			}
			base.WriteXml(writer);
			foreach (KeyValuePair<GameHex, int> keyValuePair in this.Hexes)
			{
				writer.WriteStartElement("Hex");
				writer.WriteAttributeString("X", keyValuePair.Key.posX.ToString());
				writer.WriteAttributeString("Y", keyValuePair.Key.posY.ToString());
				writer.WriteAttributeString("Amount", keyValuePair.Value.ToString());
				writer.WriteEndElement();
			}
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x0400200F RID: 8207
		public Dictionary<GameHex, int> Hexes = new Dictionary<GameHex, int>();

		// Token: 0x04002010 RID: 8208
		public bool MillUsed;
	}
}

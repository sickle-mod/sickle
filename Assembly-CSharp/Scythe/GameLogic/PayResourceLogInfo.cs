using System;
using System.Collections.Generic;
using System.Xml;

namespace Scythe.GameLogic
{
	// Token: 0x020005CA RID: 1482
	public class PayResourceLogInfo : LogInfo
	{
		// Token: 0x06002F33 RID: 12083 RVA: 0x00045687 File Offset: 0x00043887
		public PayResourceLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x0011F028 File Offset: 0x0011D228
		public override void ReadXml(XmlReader reader)
		{
			this.Type = LogInfoType.PayResource;
			base.ReadXml(reader);
			reader.ReadStartElement();
			if (reader.Name == "Resources")
			{
				if (reader.GetAttribute("R0") != null)
				{
					this.Resources.Add(ResourceType.oil, (int)short.Parse(reader.GetAttribute("R0")));
				}
				if (reader.GetAttribute("R1") != null)
				{
					this.Resources.Add(ResourceType.metal, (int)short.Parse(reader.GetAttribute("R1")));
				}
				if (reader.GetAttribute("R2") != null)
				{
					this.Resources.Add(ResourceType.food, (int)short.Parse(reader.GetAttribute("R2")));
				}
				if (reader.GetAttribute("R3") != null)
				{
					this.Resources.Add(ResourceType.wood, (int)short.Parse(reader.GetAttribute("R3")));
				}
			}
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x0011F10C File Offset: 0x0011D30C
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L0");
			base.WriteXml(writer);
			writer.WriteStartElement("Resources");
			foreach (KeyValuePair<ResourceType, int> keyValuePair in this.Resources)
			{
				if (keyValuePair.Value != 0)
				{
					writer.WriteAttributeString("R" + ((int)keyValuePair.Key).ToString(), keyValuePair.Value.ToString());
				}
			}
			writer.WriteEndElement();
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x04001FF6 RID: 8182
		public Dictionary<ResourceType, int> Resources = new Dictionary<ResourceType, int>();
	}
}

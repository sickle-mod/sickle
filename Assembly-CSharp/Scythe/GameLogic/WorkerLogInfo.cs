using System;
using System.Xml;

namespace Scythe.GameLogic
{
	// Token: 0x020005D5 RID: 1493
	public class WorkerLogInfo : LogInfo
	{
		// Token: 0x06002F54 RID: 12116 RVA: 0x0004569B File Offset: 0x0004389B
		public WorkerLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x00120638 File Offset: 0x0011E838
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.Type = LogInfoType.GainWorker;
			if (reader.GetAttribute("X") != null && reader.GetAttribute("Y") != null)
			{
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				this.Position = this.gameManager.gameBoard.hexMap[num, num2];
			}
			if (reader.GetAttribute("Amount") != null)
			{
				this.WorkersAmount = int.Parse(reader.GetAttribute("Amount"));
			}
			else
			{
				this.WorkersAmount = 1;
			}
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x001206E0 File Offset: 0x0011E8E0
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L11");
			base.WriteXml(writer);
			if (this.Position != null)
			{
				writer.WriteAttributeString("X", this.Position.posX.ToString());
				writer.WriteAttributeString("Y", this.Position.posY.ToString());
			}
			if (this.WorkersAmount != 1)
			{
				writer.WriteAttributeString("Amount", this.WorkersAmount.ToString());
			}
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x04002015 RID: 8213
		public GameHex Position;

		// Token: 0x04002016 RID: 8214
		public int WorkersAmount;
	}
}

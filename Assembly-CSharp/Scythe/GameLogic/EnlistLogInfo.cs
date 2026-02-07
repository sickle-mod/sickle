using System;
using System.Xml;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005CF RID: 1487
	public class EnlistLogInfo : LogInfo
	{
		// Token: 0x06002F42 RID: 12098 RVA: 0x0004569B File Offset: 0x0004389B
		public EnlistLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x0011FF78 File Offset: 0x0011E178
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.Type = LogInfoType.Enlist;
			this.TypeOfDownAction = (DownActionType)int.Parse(reader.GetAttribute("DownAction"));
			if (this.TypeOfDownAction != DownActionType.Factory)
			{
				this.OneTimeBonus = (GainType)int.Parse(reader.GetAttribute("Bonus"));
			}
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x0011FFD0 File Offset: 0x0011E1D0
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L5");
			base.WriteXml(writer);
			string text = "DownAction";
			int num = (int)this.TypeOfDownAction;
			writer.WriteAttributeString(text, num.ToString());
			if (this.TypeOfDownAction != DownActionType.Factory)
			{
				string text2 = "Bonus";
				num = (int)this.OneTimeBonus;
				writer.WriteAttributeString(text2, num.ToString());
			}
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x04002008 RID: 8200
		public DownActionType TypeOfDownAction;

		// Token: 0x04002009 RID: 8201
		public GainType OneTimeBonus;
	}
}

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scythe.GameLogic
{
	// Token: 0x020005A6 RID: 1446
	public abstract class Card : IXmlSerializable
	{
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06002DDE RID: 11742 RVA: 0x00044972 File Offset: 0x00042B72
		public int CardId
		{
			get
			{
				return this.cardId;
			}
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x0004497A File Offset: 0x00042B7A
		public string Serialize()
		{
			return this.cardId.ToString();
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x00044987 File Offset: 0x00042B87
		public virtual void ReadXml(XmlReader reader)
		{
			reader.Read();
			this.cardId = int.Parse(reader.Value);
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x000449A1 File Offset: 0x00042BA1
		public virtual void WriteXml(XmlWriter writer)
		{
			writer.WriteValue(this.cardId.ToString() + " ");
		}

		// Token: 0x04001F2E RID: 7982
		protected int cardId = -1;
	}
}

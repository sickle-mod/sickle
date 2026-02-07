using System;
using System.Xml.Serialization;

namespace Scythe.UI
{
	// Token: 0x020004D6 RID: 1238
	public class Mission
	{
		// Token: 0x04001C4B RID: 7243
		[XmlAttribute("id")]
		public string id;

		// Token: 0x04001C4C RID: 7244
		[XmlElement(ElementName = "Title")]
		public string Title;

		// Token: 0x04001C4D RID: 7245
		[XmlElement(ElementName = "Description")]
		public string Description;

		// Token: 0x04001C4E RID: 7246
		[XmlElement(ElementName = "Objectives")]
		public string Objectives;
	}
}

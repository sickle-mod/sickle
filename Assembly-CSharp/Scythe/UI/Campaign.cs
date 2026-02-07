using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Scythe.UI
{
	// Token: 0x020004D7 RID: 1239
	public class Campaign
	{
		// Token: 0x04001C4F RID: 7247
		[XmlAttribute("id")]
		public string id;

		// Token: 0x04001C50 RID: 7248
		[XmlAttribute("label")]
		public string label;

		// Token: 0x04001C51 RID: 7249
		[XmlElement("Mission")]
		public List<Mission> Missions = new List<Mission>();
	}
}

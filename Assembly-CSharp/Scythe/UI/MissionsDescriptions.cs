using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Scythe.UI
{
	// Token: 0x020004D8 RID: 1240
	[XmlRoot("Root")]
	public class MissionsDescriptions
	{
		// Token: 0x04001C52 RID: 7250
		[XmlArray("MissionsDescriptions")]
		[XmlArrayItem("Campaign")]
		public List<Campaign> Campaigns = new List<Campaign>();
	}
}

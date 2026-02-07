using System;
using System.Xml;

namespace Scythe.GameLogic
{
	// Token: 0x020005D0 RID: 1488
	public class BuildLogInfo : LogInfo
	{
		// Token: 0x06002F45 RID: 12101 RVA: 0x0004569B File Offset: 0x0004389B
		public BuildLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x00120038 File Offset: 0x0011E238
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.Type = LogInfoType.Build;
			if (reader.GetAttribute("BuildingType") != null)
			{
				this.PlacedBuilding = this.gameManager.GetPlayerByFaction(this.PlayerAssigned).matPlayer.GetBuilding((BuildingType)int.Parse(reader.GetAttribute("BuildingType")));
				this.Position = this.PlacedBuilding.position;
			}
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x001200AC File Offset: 0x0011E2AC
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L6");
			base.WriteXml(writer);
			if (this.PlacedBuilding != null)
			{
				string text = "BuildingType";
				int buildingType = (int)this.PlacedBuilding.buildingType;
				writer.WriteAttributeString(text, buildingType.ToString());
			}
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x0400200A RID: 8202
		public Building PlacedBuilding;

		// Token: 0x0400200B RID: 8203
		public GameHex Position;
	}
}

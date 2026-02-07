using System;
using System.Xml;

namespace Scythe.GameLogic
{
	// Token: 0x020005D1 RID: 1489
	public class DeployLogInfo : LogInfo
	{
		// Token: 0x06002F48 RID: 12104 RVA: 0x0004569B File Offset: 0x0004389B
		public DeployLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x00120100 File Offset: 0x0011E300
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.Type = LogInfoType.Deploy;
			if (reader.GetAttribute("Id") != null)
			{
				int num = int.Parse(reader.GetAttribute("Id"));
				foreach (Mech mech in this.gameManager.GetPlayerByFaction(this.PlayerAssigned).matFaction.mechs)
				{
					if (mech.Id == num)
					{
						this.DeployedMech = mech;
						break;
					}
				}
				int num2 = int.Parse(reader.GetAttribute("X"));
				int num3 = int.Parse(reader.GetAttribute("Y"));
				this.Position = this.gameManager.gameBoard.hexMap[num2, num3];
				this.MechBonus = int.Parse(reader.GetAttribute("Bonus"));
			}
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x00120204 File Offset: 0x0011E404
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L7");
			base.WriteXml(writer);
			if (this.DeployedMech != null)
			{
				writer.WriteAttributeString("Id", this.DeployedMech.Id.ToString());
				writer.WriteAttributeString("Bonus", this.MechBonus.ToString());
				writer.WriteAttributeString("X", this.Position.posX.ToString());
				writer.WriteAttributeString("Y", this.Position.posY.ToString());
			}
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x0400200C RID: 8204
		public Mech DeployedMech;

		// Token: 0x0400200D RID: 8205
		public GameHex Position;

		// Token: 0x0400200E RID: 8206
		public int MechBonus;
	}
}

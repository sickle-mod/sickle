using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic
{
	// Token: 0x020005B3 RID: 1459
	public class FlagToken : FactionAbilityToken, IXmlSerializable
	{
		// Token: 0x06002E7A RID: 11898 RVA: 0x00044F56 File Offset: 0x00043156
		public FlagToken(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x00044F5F File Offset: 0x0004315F
		public override string ToString()
		{
			return "[FlagToken] " + base.ToString();
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x0011B224 File Offset: 0x00119424
		public override LogInfo CreateLogInfo(TokenActionType actionType, Unit unit)
		{
			return new TokenActionLogInfo(this.gameManager)
			{
				Type = LogInfoType.TokenAction,
				PlayerAssigned = base.Owner.matFaction.faction,
				ActionPlacement = ActionPositionType.Other,
				token = this,
				action = actionType,
				unit = unit
			};
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x00044F71 File Offset: 0x00043171
		public override void OnTokenPlaced()
		{
			if (this.gameManager.IsMyTurn() && !this.gameManager.GameLoading)
			{
				this.gameManager.OnActionSent(new FlagTokenPlacedMessage(base.Position));
			}
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public override XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x00044FA3 File Offset: 0x000431A3
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x00044FAC File Offset: 0x000431AC
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("FlagToken");
			base.WriteXml(writer);
			writer.WriteEndElement();
		}
	}
}

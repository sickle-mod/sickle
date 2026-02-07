using System;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scythe.GameLogic
{
	// Token: 0x020005B2 RID: 1458
	public class FactionAbilityToken : IXmlSerializable
	{
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06002E6B RID: 11883 RVA: 0x00044EA4 File Offset: 0x000430A4
		// (set) Token: 0x06002E6C RID: 11884 RVA: 0x00044EAC File Offset: 0x000430AC
		public Player Owner { get; protected set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06002E6D RID: 11885 RVA: 0x00044EB5 File Offset: 0x000430B5
		// (set) Token: 0x06002E6E RID: 11886 RVA: 0x00044EBD File Offset: 0x000430BD
		public GameHex Position { get; protected set; }

		// Token: 0x06002E6F RID: 11887 RVA: 0x00044EC6 File Offset: 0x000430C6
		public FactionAbilityToken(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x00044ED5 File Offset: 0x000430D5
		public void SetOwner(Player owner)
		{
			this.Owner = owner;
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x00044EDE File Offset: 0x000430DE
		public void PlaceToken(GameHex gameHex, bool placedOnSave = false)
		{
			if (this.CanTokenBePlaced(gameHex))
			{
				this.Position = gameHex;
				gameHex.Token = this;
				if (!placedOnSave)
				{
					this.OnTokenPlaced();
				}
			}
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x00044F00 File Offset: 0x00043100
		public bool CanTokenBePlaced(GameHex gameHex)
		{
			return gameHex.Token == null;
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x00044F0B File Offset: 0x0004310B
		public bool IsTokenPlaced()
		{
			return this.Position != null;
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void OnTokenPlaced()
		{
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x0011B08C File Offset: 0x0011928C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Owner: ");
			stringBuilder.Append((this.Owner != null) ? this.Owner.matFaction.faction.ToString() : "null");
			stringBuilder.Append(". Position on: ");
			if (this.Position != null)
			{
				stringBuilder.Append(this.Position.hexType.ToString());
				stringBuilder.Append("(X:");
				stringBuilder.Append(this.Position.posX);
				stringBuilder.Append(", Y:");
				stringBuilder.Append(this.Position.posY);
				stringBuilder.Append(").");
			}
			else
			{
				stringBuilder.Append("null.");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x0011B16C File Offset: 0x0011936C
		public virtual LogInfo CreateLogInfo(TokenActionType actionType, Unit unit)
		{
			return new TokenActionLogInfo(this.gameManager)
			{
				Type = LogInfoType.TokenAction,
				PlayerAssigned = unit.Owner.matFaction.faction,
				ActionPlacement = ActionPositionType.Other,
				token = this,
				action = actionType,
				unit = unit
			};
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public virtual XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x0011B1C0 File Offset: 0x001193C0
		public virtual void ReadXml(XmlReader reader)
		{
			if (reader["X"] != null)
			{
				int num = int.Parse(reader["X"]);
				int num2 = int.Parse(reader["Y"]);
				this.PlaceToken(this.gameManager.gameBoard.hexMap[num, num2], true);
				return;
			}
			this.Position = null;
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x00044F16 File Offset: 0x00043116
		public virtual void WriteXml(XmlWriter writer)
		{
			if (this.Position != null)
			{
				writer.WriteAttributeString("X", this.Position.posX.ToString());
				writer.WriteAttributeString("Y", this.Position.posY.ToString());
			}
		}

		// Token: 0x04001F5E RID: 8030
		protected GameManager gameManager;
	}
}

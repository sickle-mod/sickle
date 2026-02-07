using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005A5 RID: 1445
	public class Building : IXmlSerializable
	{
		// Token: 0x06002DD7 RID: 11735 RVA: 0x0004492D File Offset: 0x00042B2D
		public Building(GameManager gameManager)
		{
			this.buildingType = BuildingType.Armory;
			this.position = null;
			this.gameManager = gameManager;
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x0004494A File Offset: 0x00042B4A
		public Building(BuildingType type, GameManager gameManager)
		{
			this.buildingType = type;
			this.position = null;
			this.gameManager = gameManager;
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x00115E08 File Offset: 0x00114008
		public GainAction GetBonus(Player player)
		{
			GainAction gainAction = null;
			switch (this.buildingType)
			{
			case BuildingType.Monument:
				gainAction = new GainPopularity(this.gameManager, 1, 0, false, false, true);
				gainAction.SetPlayer(player);
				break;
			case BuildingType.Armory:
				gainAction = new GainPower(this.gameManager, 1, 0, false, false, true);
				gainAction.SetPlayer(player);
				break;
			case BuildingType.Mill:
				if (!this.IsOnMap())
				{
					TopAction topAction = player.matPlayer.GetTopAction(TopActionType.Produce);
					if (topAction != null)
					{
						(topAction.GetGainAction(0) as GainProduce).MillProduce = true;
					}
				}
				break;
			}
			return gainAction;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x00044967 File Offset: 0x00042B67
		public bool IsOnMap()
		{
			return this.position != null;
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x00115E94 File Offset: 0x00114094
		public void ReadXml(XmlReader reader)
		{
			reader.MoveToContent();
			this.buildingType = (BuildingType)int.Parse(reader.GetAttribute("Type"));
			if (reader.GetAttribute("X") != null)
			{
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				this.position = this.gameManager.gameBoard.hexMap[num, num2];
				this.position.Building = this;
			}
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x00115F18 File Offset: 0x00114118
		public void WriteXml(XmlWriter writer)
		{
			string text = "Type";
			int num = (int)this.buildingType;
			writer.WriteAttributeString(text, num.ToString());
			if (this.IsOnMap())
			{
				writer.WriteAttributeString("X", this.position.posX.ToString());
				writer.WriteAttributeString("Y", this.position.posY.ToString());
			}
		}

		// Token: 0x04001F29 RID: 7977
		public BuildingType buildingType;

		// Token: 0x04001F2A RID: 7978
		public GameHex position;

		// Token: 0x04001F2B RID: 7979
		public Player player;

		// Token: 0x04001F2C RID: 7980
		public bool enemySpawnAnimation;

		// Token: 0x04001F2D RID: 7981
		private GameManager gameManager;
	}
}

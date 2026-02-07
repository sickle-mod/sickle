using System;
using System.Collections.Generic;
using System.Xml;

namespace Scythe.GameLogic
{
	// Token: 0x020005CC RID: 1484
	public class HexUnitResourceLogInfo : LogInfo
	{
		// Token: 0x06002F39 RID: 12089 RVA: 0x000456A4 File Offset: 0x000438A4
		public HexUnitResourceLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x0011F2CC File Offset: 0x0011D4CC
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("Type") != null)
			{
				this.Type = (LogInfoType)int.Parse(reader.GetAttribute("Type"));
			}
			else
			{
				this.Type = LogInfoType.TradeResources;
			}
			reader.ReadStartElement();
			while (reader.Name == "Hex")
			{
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				this.Hexes.Add(this.gameManager.gameBoard.hexMap[num, num2]);
				reader.ReadStartElement();
			}
			while (reader.Name == "Unit")
			{
				int num3 = int.Parse(reader.GetAttribute("Id"));
				switch (int.Parse(reader.GetAttribute("Type")))
				{
				case 0:
					this.Units.Add(this.gameManager.GetPlayerByFaction(this.PlayerAssigned).character);
					break;
				case 1:
				{
					using (List<Mech>.Enumerator enumerator = this.gameManager.GetPlayerByFaction(this.PlayerAssigned).matFaction.mechs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Mech mech = enumerator.Current;
							if (mech.Id == num3)
							{
								this.Units.Add(mech);
								break;
							}
						}
						break;
					}
					goto IL_015A;
				}
				case 2:
					goto IL_015A;
				}
				IL_01B9:
				reader.ReadStartElement();
				continue;
				IL_015A:
				foreach (Worker worker in this.gameManager.GetPlayerByFaction(this.PlayerAssigned).matPlayer.workers)
				{
					if (worker.Id == num3)
					{
						this.Units.Add(worker);
						break;
					}
				}
				goto IL_01B9;
			}
			while (reader.Name == "Resources")
			{
				Dictionary<ResourceType, int> dictionary = new Dictionary<ResourceType, int>();
				if (reader.GetAttribute("R0") != null)
				{
					dictionary.Add(ResourceType.oil, int.Parse(reader.GetAttribute("R0")));
				}
				if (reader.GetAttribute("R1") != null)
				{
					dictionary.Add(ResourceType.metal, int.Parse(reader.GetAttribute("R1")));
				}
				if (reader.GetAttribute("R2") != null)
				{
					dictionary.Add(ResourceType.food, int.Parse(reader.GetAttribute("R2")));
				}
				if (reader.GetAttribute("R3") != null)
				{
					dictionary.Add(ResourceType.wood, int.Parse(reader.GetAttribute("R3")));
				}
				this.Resources.Add(dictionary);
				reader.ReadStartElement();
			}
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x0011F598 File Offset: 0x0011D798
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L2");
			base.WriteXml(writer);
			if (this.Type != LogInfoType.TradeResources)
			{
				string text = "Type";
				int num = (int)this.Type;
				writer.WriteAttributeString(text, num.ToString());
			}
			foreach (GameHex gameHex in this.Hexes)
			{
				writer.WriteStartElement("Hex");
				writer.WriteAttributeString("X", gameHex.posX.ToString());
				writer.WriteAttributeString("Y", gameHex.posY.ToString());
				writer.WriteEndElement();
			}
			foreach (Unit unit in this.Units)
			{
				writer.WriteStartElement("Unit");
				writer.WriteAttributeString("Id", unit.Id.ToString());
				writer.WriteAttributeString("Type", ((int)unit.UnitType).ToString());
				writer.WriteEndElement();
			}
			foreach (Dictionary<ResourceType, int> dictionary in this.Resources)
			{
				writer.WriteStartElement("Resources");
				foreach (ResourceType resourceType in dictionary.Keys)
				{
					string text2 = "R";
					int num = (int)resourceType;
					writer.WriteAttributeString(text2 + num.ToString(), dictionary[resourceType].ToString());
				}
				writer.WriteEndElement();
			}
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x04001FF9 RID: 8185
		public List<GameHex> Hexes = new List<GameHex>();

		// Token: 0x04001FFA RID: 8186
		public List<Unit> Units = new List<Unit>();

		// Token: 0x04001FFB RID: 8187
		public List<Dictionary<ResourceType, int>> Resources = new List<Dictionary<ResourceType, int>>();
	}
}

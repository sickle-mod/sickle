using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Scythe.GameLogic
{
	// Token: 0x020005CD RID: 1485
	public class CombatLogInfo : LogInfo
	{
		// Token: 0x06002F3C RID: 12092 RVA: 0x000456CE File Offset: 0x000438CE
		public CombatLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x0011F79C File Offset: 0x0011D99C
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("Type") != null)
			{
				this.Type = (LogInfoType)int.Parse(reader.GetAttribute("Type"));
			}
			else
			{
				this.Type = LogInfoType.Combat;
			}
			int num = int.Parse(reader.GetAttribute("X"));
			int num2 = int.Parse(reader.GetAttribute("Y"));
			this.Battlefield = this.gameManager.gameBoard.hexMap[num, num2];
			this.Winner = this.gameManager.GetPlayerByFaction((Faction)int.Parse(reader.GetAttribute("Winner")));
			this.Defeated = this.gameManager.GetPlayerByFaction((Faction)int.Parse(reader.GetAttribute("Defeated")));
			if (reader.GetAttribute("Popularity") != null)
			{
				this.LostPopularity = int.Parse(reader.GetAttribute("Popularity"));
			}
			this.WinnerAbilityUsed = reader.GetAttribute("WinnerAbility") != null;
			this.DefeatedAbilityUsed = reader.GetAttribute("DefeatedAbility") != null;
			reader.ReadStartElement();
			if (reader.Name == "WP")
			{
				this.WinnerPower.selectedPower = int.Parse(reader.GetAttribute("SPower"));
				this.WinnerPower.cardsPower = int.Parse(reader.GetAttribute("CPower"));
				this.WinnerPower.selectedCards = new List<CombatCard>();
				reader.ReadStartElement();
				if (reader.Name == "CC")
				{
					foreach (string text in reader.ReadElementContentAsString().Split(' ', StringSplitOptions.None))
					{
						if (text == "")
						{
							break;
						}
						this.WinnerPower.selectedCards.Add(new CombatCard(int.Parse(text)));
					}
					reader.ReadEndElement();
				}
			}
			if (reader.Name == "DP")
			{
				this.DefeatedPower.selectedPower = int.Parse(reader.GetAttribute("SPower"));
				this.DefeatedPower.cardsPower = int.Parse(reader.GetAttribute("CPower"));
				this.DefeatedPower.selectedCards = new List<CombatCard>();
				reader.ReadStartElement();
				if (reader.Name == "CC")
				{
					foreach (string text2 in reader.ReadElementContentAsString().Split(' ', StringSplitOptions.None))
					{
						if (text2 == "")
						{
							break;
						}
						this.DefeatedPower.selectedCards.Add(new CombatCard(int.Parse(text2)));
					}
					reader.ReadEndElement();
				}
			}
			while (reader.Name == "Unit")
			{
				Faction faction = (Faction)int.Parse(reader.GetAttribute("Owner"));
				Player playerByFaction = this.gameManager.GetPlayerByFaction(faction);
				int num3 = int.Parse(reader.GetAttribute("Id"));
				switch (int.Parse(reader.GetAttribute("Type")))
				{
				case 0:
					this.Units.Add(playerByFaction.character);
					break;
				case 1:
				{
					using (List<Mech>.Enumerator enumerator = playerByFaction.matFaction.mechs.GetEnumerator())
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
					goto IL_035D;
				}
				case 2:
					goto IL_035D;
				}
				IL_03AE:
				reader.ReadStartElement();
				continue;
				IL_035D:
				foreach (Worker worker in playerByFaction.matPlayer.workers)
				{
					if (worker.Id == num3)
					{
						this.Units.Add(worker);
						break;
					}
				}
				goto IL_03AE;
			}
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F3E RID: 12094 RVA: 0x0011FB98 File Offset: 0x0011DD98
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L3");
			base.WriteXml(writer);
			int num;
			if (this.Type != LogInfoType.Combat)
			{
				string text = "Type";
				num = (int)this.Type;
				writer.WriteAttributeString(text, num.ToString());
			}
			writer.WriteAttributeString("X", this.Battlefield.posX.ToString());
			writer.WriteAttributeString("Y", this.Battlefield.posY.ToString());
			string text2 = "Winner";
			num = (int)this.Winner.matFaction.faction;
			writer.WriteAttributeString(text2, num.ToString());
			string text3 = "Defeated";
			num = (int)this.Defeated.matFaction.faction;
			writer.WriteAttributeString(text3, num.ToString());
			if (this.LostPopularity != 0)
			{
				writer.WriteAttributeString("Popularity", this.LostPopularity.ToString());
			}
			if (this.WinnerAbilityUsed)
			{
				writer.WriteAttributeString("WinnerAbility", "");
			}
			if (this.DefeatedAbilityUsed)
			{
				writer.WriteAttributeString("DefeatedAbility", "");
			}
			writer.WriteStartElement("WP");
			writer.WriteAttributeString("SPower", this.WinnerPower.selectedPower.ToString());
			writer.WriteAttributeString("CPower", this.WinnerPower.cardsPower.ToString());
			if (this.WinnerPower.selectedCards != null)
			{
				writer.WriteStartElement("CC");
				for (int i = 0; i < this.WinnerPower.selectedCards.Count; i++)
				{
					((IXmlSerializable)this.WinnerPower.selectedCards[i]).WriteXml(writer);
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.WriteStartElement("DP");
			writer.WriteAttributeString("SPower", this.DefeatedPower.selectedPower.ToString());
			writer.WriteAttributeString("CPower", this.DefeatedPower.cardsPower.ToString());
			if (this.DefeatedPower.selectedCards != null)
			{
				writer.WriteStartElement("CC");
				for (int j = 0; j < this.DefeatedPower.selectedCards.Count; j++)
				{
					((IXmlSerializable)this.DefeatedPower.selectedCards[j]).WriteXml(writer);
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			foreach (Unit unit in this.Units)
			{
				writer.WriteStartElement("Unit");
				string text4 = "Owner";
				num = (int)unit.Owner.matFaction.faction;
				writer.WriteAttributeString(text4, num.ToString());
				writer.WriteAttributeString("Id", unit.Id.ToString());
				writer.WriteAttributeString("Type", ((int)unit.UnitType).ToString());
				writer.WriteEndElement();
			}
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x04001FFC RID: 8188
		public GameHex Battlefield;

		// Token: 0x04001FFD RID: 8189
		public List<Unit> Units = new List<Unit>();

		// Token: 0x04001FFE RID: 8190
		public Player Winner;

		// Token: 0x04001FFF RID: 8191
		public Player Defeated;

		// Token: 0x04002000 RID: 8192
		public PowerSelected WinnerPower;

		// Token: 0x04002001 RID: 8193
		public PowerSelected DefeatedPower;

		// Token: 0x04002002 RID: 8194
		public int LostPopularity;

		// Token: 0x04002003 RID: 8195
		public bool WinnerAbilityUsed;

		// Token: 0x04002004 RID: 8196
		public bool DefeatedAbilityUsed;
	}
}

using System;
using System.Collections.Generic;
using System.Xml;

namespace Scythe.GameLogic
{
	// Token: 0x020005D8 RID: 1496
	public class TokenActionLogInfo : LogInfo
	{
		// Token: 0x06002F5D RID: 12125 RVA: 0x0004569B File Offset: 0x0004389B
		public TokenActionLogInfo(GameManager gameManager)
			: base(gameManager)
		{
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x00120860 File Offset: 0x0011EA60
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.Type = LogInfoType.TokenAction;
			this.action = (TokenActionType)int.Parse(reader.GetAttribute("Action"));
			Faction faction = (Faction)int.Parse(reader.GetAttribute("TokenOwner"));
			int num = int.Parse(reader.GetAttribute("Token"));
			this.token = this.gameManager.GetPlayerByFaction(faction).matFaction.FactionTokens.GetToken(num);
			reader.ReadStartElement();
			if (reader.Name == "Unit")
			{
				Faction faction2 = (Faction)int.Parse(reader.GetAttribute("Owner"));
				Player playerByFaction = this.gameManager.GetPlayerByFaction(faction2);
				int num2 = int.Parse(reader.GetAttribute("Id"));
				switch (int.Parse(reader.GetAttribute("Type")))
				{
				case 0:
					this.unit = playerByFaction.character;
					goto IL_0185;
				case 1:
				{
					using (List<Mech>.Enumerator enumerator = playerByFaction.matFaction.mechs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Mech mech = enumerator.Current;
							if (mech.Id == num2)
							{
								this.unit = mech;
								break;
							}
						}
						goto IL_0185;
					}
					break;
				}
				case 2:
					break;
				default:
					goto IL_0185;
				}
				foreach (Worker worker in playerByFaction.matPlayer.workers)
				{
					if (worker.Id == num2)
					{
						this.unit = worker;
						break;
					}
				}
			}
			IL_0185:
			base.ReadAdditionalLogs(reader);
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x00120A18 File Offset: 0x0011EC18
		public override void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("L14");
			base.WriteXml(writer);
			string text = "Action";
			int num = (int)this.action;
			writer.WriteAttributeString(text, num.ToString());
			string text2 = "TokenOwner";
			num = (int)this.token.Owner.matFaction.faction;
			writer.WriteAttributeString(text2, num.ToString());
			writer.WriteAttributeString("Token", this.token.Owner.matFaction.FactionTokens.GetTokenId(this.token).ToString());
			if (this.unit != null)
			{
				writer.WriteStartElement("Unit");
				string text3 = "Owner";
				num = (int)this.unit.Owner.matFaction.faction;
				writer.WriteAttributeString(text3, num.ToString());
				writer.WriteAttributeString("Id", this.unit.Id.ToString());
				writer.WriteAttributeString("Type", ((int)this.unit.UnitType).ToString());
				writer.WriteEndElement();
			}
			this.WriteAdditionalLogs(writer);
			writer.WriteEndElement();
		}

		// Token: 0x0400201A RID: 8218
		public FactionAbilityToken token;

		// Token: 0x0400201B RID: 8219
		public TokenActionType action;

		// Token: 0x0400201C RID: 8220
		public Unit unit;
	}
}

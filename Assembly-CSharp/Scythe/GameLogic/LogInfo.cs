using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scythe.GameLogic
{
	// Token: 0x020005C9 RID: 1481
	public class LogInfo : IXmlSerializable
	{
		// Token: 0x06002F2D RID: 12077 RVA: 0x00045662 File Offset: 0x00043862
		public LogInfo(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x0011EC60 File Offset: 0x0011CE60
		public virtual void ReadXml(XmlReader reader)
		{
			if (reader.GetAttribute("Placement") != null)
			{
				this.ActionPlacement = (ActionPositionType)int.Parse(reader.GetAttribute("Placement"));
			}
			else
			{
				this.ActionPlacement = ActionPositionType.Top;
			}
			this.PlayerAssigned = (Faction)int.Parse(reader.GetAttribute("Player"));
			this.IsEncounter = reader.GetAttribute("Encounter") != null;
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x0011ECC4 File Offset: 0x0011CEC4
		protected void ReadAdditionalLogs(XmlReader reader)
		{
			if (reader.NodeType == XmlNodeType.EndElement)
			{
				reader.ReadEndElement();
				return;
			}
			if (reader.Name != "PayLog" && reader.Name != "GainLog")
			{
				reader.ReadStartElement();
			}
			if (reader.NodeType == XmlNodeType.EndElement)
			{
				reader.ReadEndElement();
				return;
			}
			while (reader.Name == "PayLog")
			{
				reader.ReadStartElement();
				int num;
				if (int.TryParse(reader.Name.Substring(1), out num))
				{
					LogInfo logInfo = null;
					if (num != 0)
					{
						if (num == 1)
						{
							logInfo = new PayNonboardResourceLogInfo(this.gameManager);
						}
					}
					else
					{
						logInfo = new PayResourceLogInfo(this.gameManager);
					}
					((IXmlSerializable)logInfo).ReadXml(reader);
					this.PayLogInfos.Add(logInfo);
					if (reader.NodeType == XmlNodeType.EndElement)
					{
						reader.ReadEndElement();
					}
				}
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					reader.ReadEndElement();
				}
			}
			while (reader.Name == "GainLog")
			{
				reader.ReadStartElement();
				int num2;
				if (int.TryParse(reader.Name.Substring(1), out num2))
				{
					LogInfo logInfo2 = null;
					switch (num2)
					{
					case 2:
						logInfo2 = new HexUnitResourceLogInfo(this.gameManager);
						break;
					case 3:
						logInfo2 = new CombatLogInfo(this.gameManager);
						break;
					case 4:
						logInfo2 = new UpgradeLogInfo(this.gameManager);
						break;
					case 5:
						logInfo2 = new EnlistLogInfo(this.gameManager);
						break;
					case 6:
						logInfo2 = new BuildLogInfo(this.gameManager);
						break;
					case 7:
						logInfo2 = new DeployLogInfo(this.gameManager);
						break;
					case 8:
						logInfo2 = new ProductionLogInfo(this.gameManager);
						break;
					case 9:
						logInfo2 = new GainNonboardResourceLogInfo(this.gameManager);
						break;
					case 11:
						logInfo2 = new WorkerLogInfo(this.gameManager);
						break;
					case 12:
						logInfo2 = new SneakPeakLogInfo(this.gameManager);
						break;
					case 14:
						logInfo2 = new TokenActionLogInfo(this.gameManager);
						break;
					case 15:
						logInfo2 = new PassCoinLogInfo(this.gameManager);
						break;
					}
					((IXmlSerializable)logInfo2).ReadXml(reader);
					this.AdditionalGain.Add(logInfo2);
					if (reader.NodeType == XmlNodeType.EndElement)
					{
						reader.ReadEndElement();
					}
				}
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					reader.ReadEndElement();
				}
			}
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x0011EF04 File Offset: 0x0011D104
		public virtual void WriteXml(XmlWriter writer)
		{
			int num;
			if (this.ActionPlacement != ActionPositionType.Top)
			{
				string text = "Placement";
				num = (int)this.ActionPlacement;
				writer.WriteAttributeString(text, num.ToString());
			}
			string text2 = "Player";
			num = (int)this.PlayerAssigned;
			writer.WriteAttributeString(text2, num.ToString());
			if (this.IsEncounter)
			{
				writer.WriteAttributeString("Encounter", "1");
			}
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x0011EF64 File Offset: 0x0011D164
		protected virtual void WriteAdditionalLogs(XmlWriter writer)
		{
			if (this.PayLogInfos != null)
			{
				foreach (IXmlSerializable xmlSerializable in this.PayLogInfos)
				{
					writer.WriteStartElement("PayLog");
					xmlSerializable.WriteXml(writer);
					writer.WriteEndElement();
				}
			}
			if (this.AdditionalGain != null)
			{
				foreach (IXmlSerializable xmlSerializable2 in this.AdditionalGain)
				{
					writer.WriteStartElement("GainLog");
					xmlSerializable2.WriteXml(writer);
					writer.WriteEndElement();
				}
			}
		}

		// Token: 0x04001FEF RID: 8175
		public LogInfoType Type;

		// Token: 0x04001FF0 RID: 8176
		public ActionPositionType ActionPlacement;

		// Token: 0x04001FF1 RID: 8177
		public Faction PlayerAssigned;

		// Token: 0x04001FF2 RID: 8178
		public List<LogInfo> PayLogInfos = new List<LogInfo>();

		// Token: 0x04001FF3 RID: 8179
		public List<LogInfo> AdditionalGain = new List<LogInfo>();

		// Token: 0x04001FF4 RID: 8180
		public bool IsEncounter;

		// Token: 0x04001FF5 RID: 8181
		protected GameManager gameManager;
	}
}

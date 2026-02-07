using System;
using System.Xml;

namespace Common.GameSaves
{
	// Token: 0x02000198 RID: 408
	public class GameSave
	{
		// Token: 0x06000BF7 RID: 3063 RVA: 0x00080098 File Offset: 0x0007E298
		public void ReadGameSave(string gameSaveData)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(gameSaveData);
			XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("SaveMetadata");
			if (xmlNodeList.Count > 0)
			{
				this.SaveMetadata = xmlNodeList.Item(0).OuterXml;
			}
			xmlNodeList = xmlDocument.GetElementsByTagName("GameManager");
			if (xmlNodeList.Count > 0)
			{
				this.GameManager = xmlNodeList.Item(0).OuterXml;
			}
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00080100 File Offset: 0x0007E300
		public string CreateGameSave(string saveMetadata, string gameManagerData)
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlNode xmlNode = xmlDocument.CreateElement("GameSave");
			xmlDocument.AppendChild(xmlNode);
			XmlDocument xmlDocument2 = new XmlDocument();
			xmlDocument2.LoadXml(saveMetadata);
			XmlDocument xmlDocument3 = new XmlDocument();
			xmlDocument3.LoadXml(gameManagerData);
			xmlNode.AppendChild(xmlNode.OwnerDocument.ImportNode(xmlDocument2.DocumentElement, true));
			xmlNode.AppendChild(xmlNode.OwnerDocument.ImportNode(xmlDocument3.DocumentElement, true));
			return xmlDocument.OuterXml;
		}

		// Token: 0x0400099E RID: 2462
		public string SaveMetadata;

		// Token: 0x0400099F RID: 2463
		public string GameManager;
	}
}

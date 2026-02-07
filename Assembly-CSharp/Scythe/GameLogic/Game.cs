using System;
using System.IO;
using System.Xml;
using Scythe.Utilities;

namespace Scythe.GameLogic
{
	// Token: 0x020005C0 RID: 1472
	public class Game
	{
		// Token: 0x06002EE6 RID: 12006 RVA: 0x000454A5 File Offset: 0x000436A5
		public void SetGameId(string gameId)
		{
			this.gameId = gameId;
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x000454AE File Offset: 0x000436AE
		public string GetGameId()
		{
			return this.gameId;
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x0011C6CC File Offset: 0x0011A8CC
		public void CreateNewGameId()
		{
			this.gameId = Guid.NewGuid().ToString();
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06002EE9 RID: 12009 RVA: 0x000454B6 File Offset: 0x000436B6
		public GameManager GameManager
		{
			get
			{
				return this.gameManager;
			}
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000454BE File Offset: 0x000436BE
		public void CreateNewGameManager()
		{
			this.gameManager = new GameManager();
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x000454CB File Offset: 0x000436CB
		public string SaveToString()
		{
			return GameSerializer.Serialize<GameManager>(this.GameManager);
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x000454D8 File Offset: 0x000436D8
		public void LoadGame(string fileName)
		{
			this.gameManager = GameSerializer.Deserialize<GameManager>(fileName);
			this.gameManager.AfterLoad();
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x000454F1 File Offset: 0x000436F1
		public void LoadFromString(string saveData)
		{
			this.gameManager = GameSerializer.DeserializeObject<GameManager>(saveData);
			this.gameManager.AfterLoad();
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x0004550A File Offset: 0x0004370A
		public static bool TestLoadSave(string saveData)
		{
			return GameSerializer.DeserializeObject<GameManager>(saveData) != null;
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x0011C6F4 File Offset: 0x0011A8F4
		public static string GetSaveName(string fileName)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(new FileStream(fileName, FileMode.Open));
			xmlTextReader.Read();
			xmlTextReader.MoveToContent();
			string attribute = xmlTextReader.GetAttribute("SaveName");
			xmlTextReader.Close();
			return attribute;
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x00045515 File Offset: 0x00043715
		public static string GetGameName(string saveGame)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(saveGame);
			return xmlDocument.FirstChild.Attributes["SaveName"].Value;
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x0011C730 File Offset: 0x0011A930
		public static string GetSaveHash(string fileName)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(new FileStream(fileName, FileMode.Open));
			xmlTextReader.Read();
			xmlTextReader.MoveToContent();
			string attribute = xmlTextReader.GetAttribute("SaveHash");
			xmlTextReader.Close();
			return attribute;
		}

		// Token: 0x04001F82 RID: 8066
		private GameManager gameManager;

		// Token: 0x04001F83 RID: 8067
		public const string GAME_VERSION = "1.65a";

		// Token: 0x04001F84 RID: 8068
		private string gameId = string.Empty;
	}
}

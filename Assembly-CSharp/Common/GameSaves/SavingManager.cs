using System;
using System.Text.RegularExpressions;
using System.Xml;
using Scythe.Analytics;
using Scythe.GameLogic;

namespace Common.GameSaves
{
	// Token: 0x0200019E RID: 414
	internal abstract class SavingManager
	{
		// Token: 0x06000C23 RID: 3107
		public abstract string SaveGame(string saveSlotName, int slotId, string saveName, string gameId);

		// Token: 0x06000C24 RID: 3108
		public abstract void UpdateSave(string saveSlotName, string data);

		// Token: 0x06000C25 RID: 3109
		public abstract void LoadGame(string saveSlotName);

		// Token: 0x06000C26 RID: 3110
		public abstract void DeleteGame(string saveSlotName);

		// Token: 0x06000C27 RID: 3111
		protected abstract string GetSaveData(string saveSlotName);

		// Token: 0x06000C28 RID: 3112
		public abstract bool IsSaveFileExisting(string saveSlotName);

		// Token: 0x06000C29 RID: 3113 RVA: 0x0008067C File Offset: 0x0007E87C
		public bool IsSaveValid(string saveSlotName)
		{
			string saveData = this.GetSaveData(saveSlotName);
			GameSave gameSave = new GameSave();
			gameSave.ReadGameSave(saveData);
			AnalyticsEventData.SetMatchSessionIDUpdateLockState(true);
			bool flag = Game.TestLoadSave(gameSave.GameManager);
			AnalyticsEventData.SetMatchSessionIDUpdateLockState(false);
			return flag;
		}

		// Token: 0x06000C2A RID: 3114
		public abstract SaveMetadata GetSaveMetadata(string saveSlotName);

		// Token: 0x06000C2B RID: 3115 RVA: 0x000806B4 File Offset: 0x0007E8B4
		public bool IsSaveSlotIFA(string saveSlotName)
		{
			string saveData = this.GetSaveData(saveSlotName);
			GameSave gameSave = new GameSave();
			gameSave.ReadGameSave(saveData);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(gameSave.GameManager);
			return xmlDocument.SelectSingleNode("/GameManager").Attributes["IFA"] != null;
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00080704 File Offset: 0x0007E904
		public SaveMetadata GenerateSaveMetadata(int slotId, string saveName, string gameId)
		{
			return new SaveMetadata
			{
				SaveSlotId = slotId,
				SaveName = saveName,
				SaveHash = Guid.NewGuid().ToString(),
				GameId = gameId,
				LastWriteDate = DateTime.UtcNow.ToLocalTime()
			};
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0003021D File Offset: 0x0002E41D
		public string GenerateSaveMetadataAsString(int slotId, string saveName, string gameId)
		{
			return SaveMetadata.Serialize(this.GenerateSaveMetadata(slotId, saveName, gameId));
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00080758 File Offset: 0x0007E958
		public DateTime GetLastWriteTimeFromData(string saveData)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(saveData);
			if (xmlDocument.FirstChild.Attributes["LastWriteTime"] != null)
			{
				return DateTime.Parse(xmlDocument.FirstChild.Attributes["LastWriteTime"].Value).ToLocalTime();
			}
			return default(DateTime);
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x000807BC File Offset: 0x0007E9BC
		public string GetSaveHashFromData(string saveData)
		{
			GameSave gameSave = new GameSave();
			gameSave.ReadGameSave(saveData);
			if (!string.IsNullOrEmpty(gameSave.SaveMetadata))
			{
				return SaveMetadata.Deserialize(gameSave.SaveMetadata).SaveHash;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(gameSave.GameManager);
			if (xmlDocument.FirstChild.Attributes["SaveHash"] != null)
			{
				return xmlDocument.FirstChild.Attributes["SaveHash"].Value;
			}
			return string.Empty;
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x00080840 File Offset: 0x0007EA40
		public string GetSaveNameFromData(string saveData)
		{
			GameSave gameSave = new GameSave();
			gameSave.ReadGameSave(saveData);
			if (!string.IsNullOrEmpty(gameSave.SaveMetadata))
			{
				return SaveMetadata.Deserialize(gameSave.SaveMetadata).SaveName;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(gameSave.GameManager);
			if (xmlDocument.FirstChild.Attributes["SaveName"] != null)
			{
				return xmlDocument.FirstChild.Attributes["SaveName"].Value;
			}
			return string.Empty;
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x000808C4 File Offset: 0x0007EAC4
		public string GetGameIdFromData(string saveData)
		{
			GameSave gameSave = new GameSave();
			gameSave.ReadGameSave(saveData);
			if (!string.IsNullOrEmpty(gameSave.SaveMetadata))
			{
				return SaveMetadata.Deserialize(gameSave.SaveMetadata).SaveHash;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(gameSave.GameManager);
			if (xmlDocument.FirstChild.Attributes["GameId"] != null)
			{
				return xmlDocument.FirstChild.Attributes["GameId"].Value;
			}
			return string.Empty;
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00080948 File Offset: 0x0007EB48
		public int GetSaveSlotIdFromSlotName(string saveSlotName)
		{
			Match match = new Regex("([1-5]|Tmp)").Match(saveSlotName);
			if (!match.Success)
			{
				return 5;
			}
			if (match.Value.Equals("Tmp"))
			{
				return 5;
			}
			return int.Parse(match.Value) - 1;
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00080994 File Offset: 0x0007EB94
		public string GetGameIdFromSlot(string saveSlotName)
		{
			string saveData = this.GetSaveData(saveSlotName);
			return this.GetGameIdFromData(saveData);
		}
	}
}

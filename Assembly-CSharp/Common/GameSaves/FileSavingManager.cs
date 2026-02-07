using System;
using System.IO;
using Scythe.UI;
using UnityEngine;

namespace Common.GameSaves
{
	// Token: 0x02000197 RID: 407
	internal class FileSavingManager : SavingManager
	{
		// Token: 0x06000BEC RID: 3052 RVA: 0x00030096 File Offset: 0x0002E296
		public string GetSaveFolderPath()
		{
			return Path.Combine(Application.persistentDataPath, "Saves");
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x000300A7 File Offset: 0x0002E2A7
		public string GetSaveFilePath(string saveSlotName)
		{
			return Path.Combine(this.GetSaveFolderPath(), saveSlotName);
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x0007FEA4 File Offset: 0x0007E0A4
		private void CreateSaveFolderIfNotPresent()
		{
			string saveFolderPath = this.GetSaveFolderPath();
			if (string.IsNullOrEmpty(saveFolderPath))
			{
				return;
			}
			if (!Directory.Exists(saveFolderPath))
			{
				Directory.CreateDirectory(saveFolderPath);
			}
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0007FED0 File Offset: 0x0007E0D0
		public override SaveMetadata GetSaveMetadata(string saveSlotName)
		{
			string text = Path.Combine(this.GetSaveFolderPath(), saveSlotName);
			SaveMetadata saveMetadata2;
			try
			{
				if (File.Exists(text))
				{
					string text2 = File.ReadAllText(text);
					GameSave gameSave = new GameSave();
					gameSave.ReadGameSave(text2);
					SaveMetadata saveMetadata;
					if (string.IsNullOrEmpty(gameSave.SaveMetadata) && !string.IsNullOrEmpty(gameSave.GameManager))
					{
						saveMetadata = new SaveMetadata();
						saveMetadata.SaveSlotId = base.GetSaveSlotIdFromSlotName(saveSlotName);
						saveMetadata.SaveName = base.GetSaveNameFromData(gameSave.GameManager);
						saveMetadata.SaveHash = base.GetSaveHashFromData(gameSave.GameManager);
						FileInfo fileInfo = new FileInfo(text);
						saveMetadata.LastWriteDate = fileInfo.LastWriteTime.ToLocalTime();
					}
					else
					{
						saveMetadata = SaveMetadata.Deserialize(gameSave.SaveMetadata);
					}
					saveMetadata2 = saveMetadata;
				}
				else
				{
					saveMetadata2 = null;
				}
			}
			catch
			{
				saveMetadata2 = null;
			}
			return saveMetadata2;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x000300B5 File Offset: 0x0002E2B5
		public override bool IsSaveFileExisting(string saveSlotName)
		{
			return File.Exists(this.GetSaveFilePath(saveSlotName));
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0007FFAC File Offset: 0x0007E1AC
		public override string SaveGame(string saveSlotName, int slotId, string saveName, string gameId)
		{
			this.CreateSaveFolderIfNotPresent();
			string saveFilePath = this.GetSaveFilePath(saveSlotName);
			string text = base.GenerateSaveMetadataAsString(slotId, saveName, gameId);
			string text2 = GameController.Game.SaveToString();
			string text3 = new GameSave().CreateGameSave(text, text2);
			File.WriteAllText(saveFilePath, text3);
			return text3;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x0007FFF0 File Offset: 0x0007E1F0
		public override void LoadGame(string saveSlotName)
		{
			this.CreateSaveFolderIfNotPresent();
			string saveFilePath = this.GetSaveFilePath(saveSlotName);
			if (File.Exists(saveFilePath))
			{
				string saveData = this.GetSaveData(saveSlotName);
				GameSave gameSave = new GameSave();
				gameSave.ReadGameSave(saveData);
				if (!string.IsNullOrEmpty(gameSave.SaveMetadata))
				{
					SaveMetadata saveMetadata = SaveMetadata.Deserialize(gameSave.SaveMetadata);
					GameController.Game.SetGameId(saveMetadata.GameId);
				}
				GameController.Game.LoadFromString(gameSave.GameManager);
				return;
			}
			Debug.LogError("Load game error - There is no such a file: " + saveFilePath);
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00080074 File Offset: 0x0007E274
		public override void DeleteGame(string saveSlotName)
		{
			string saveFilePath = this.GetSaveFilePath(saveSlotName);
			if (File.Exists(saveFilePath))
			{
				File.Delete(saveFilePath);
			}
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x000300C3 File Offset: 0x0002E2C3
		public override void UpdateSave(string saveSlotName, string saveData)
		{
			this.CreateSaveFolderIfNotPresent();
			File.WriteAllText(this.GetSaveFilePath(saveSlotName), saveData);
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x000300D8 File Offset: 0x0002E2D8
		protected override string GetSaveData(string saveSlotName)
		{
			if (this.IsSaveFileExisting(saveSlotName))
			{
				return File.ReadAllText(this.GetSaveFilePath(saveSlotName));
			}
			return string.Empty;
		}

		// Token: 0x0400099D RID: 2461
		private const string SAVE_FOLDER = "Saves";
	}
}

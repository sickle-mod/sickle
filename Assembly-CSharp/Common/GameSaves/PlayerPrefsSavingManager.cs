using System;
using Scythe.UI;
using UnityEngine;

namespace Common.GameSaves
{
	// Token: 0x0200019C RID: 412
	internal class PlayerPrefsSavingManager : SavingManager
	{
		// Token: 0x06000C18 RID: 3096 RVA: 0x00080448 File Offset: 0x0007E648
		public override SaveMetadata GetSaveMetadata(string saveSlotName)
		{
			if (!PlayerPrefs.HasKey(saveSlotName))
			{
				return null;
			}
			SaveMetadata saveMetadata = new SaveMetadata();
			string @string = PlayerPrefs.GetString(saveSlotName);
			GameSave gameSave = new GameSave();
			gameSave.ReadGameSave(@string);
			if (string.IsNullOrEmpty(gameSave.SaveMetadata) && !string.IsNullOrEmpty(gameSave.GameManager))
			{
				saveMetadata.SaveSlotId = base.GetSaveSlotIdFromSlotName(saveSlotName);
				saveMetadata.SaveName = base.GetSaveNameFromData(gameSave.GameManager);
				saveMetadata.SaveHash = base.GetSaveHashFromData(gameSave.GameManager);
				saveMetadata.LastWriteDate = base.GetLastWriteTimeFromData(gameSave.GameManager);
				return saveMetadata;
			}
			return SaveMetadata.Deserialize(gameSave.SaveMetadata);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0002F567 File Offset: 0x0002D767
		public override bool IsSaveFileExisting(string saveSlotName)
		{
			return PlayerPrefs.HasKey(saveSlotName);
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x000804E8 File Offset: 0x0007E6E8
		public override string SaveGame(string saveSlotName, int slotId, string saveName, string gameId)
		{
			string text = base.GenerateSaveMetadataAsString(slotId, saveName, gameId);
			string text2 = GameController.Game.SaveToString();
			string text3 = new GameSave().CreateGameSave(text, text2);
			PlayerPrefs.SetString(saveSlotName, text3);
			return text2;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00080520 File Offset: 0x0007E720
		public override void LoadGame(string saveSlotName)
		{
			string text = string.Empty;
			text = this.GetSaveData(saveSlotName);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			GameSave gameSave = new GameSave();
			gameSave.ReadGameSave(text);
			if (!string.IsNullOrEmpty(gameSave.SaveMetadata))
			{
				SaveMetadata saveMetadata = SaveMetadata.Deserialize(gameSave.SaveMetadata);
				GameController.Game.SetGameId(saveMetadata.GameId);
			}
			GameController.Game.LoadFromString(gameSave.GameManager);
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x000301F2 File Offset: 0x0002E3F2
		public override void DeleteGame(string saveSlotName)
		{
			if (PlayerPrefs.HasKey(saveSlotName))
			{
				PlayerPrefs.DeleteKey(saveSlotName);
			}
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0002F5A3 File Offset: 0x0002D7A3
		public override void UpdateSave(string saveSlotName, string saveData)
		{
			PlayerPrefs.SetString(saveSlotName, saveData);
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00030202 File Offset: 0x0002E402
		protected override string GetSaveData(string saveSlotName)
		{
			if (PlayerPrefs.HasKey(saveSlotName))
			{
				return PlayerPrefs.GetString(saveSlotName, string.Empty);
			}
			return string.Empty;
		}
	}
}

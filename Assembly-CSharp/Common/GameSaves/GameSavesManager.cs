using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common.GameSaves
{
	// Token: 0x02000199 RID: 409
	public static class GameSavesManager
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x000300FD File Offset: 0x0002E2FD
		private static SavingManager GameSavingManager
		{
			get
			{
				if (GameSavesManager.savingManager == null)
				{
					GameSavesManager.savingManager = new FileSavingManager();
				}
				return GameSavesManager.savingManager;
			}
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06000BFB RID: 3067 RVA: 0x00080178 File Offset: 0x0007E378
		// (remove) Token: 0x06000BFC RID: 3068 RVA: 0x000801AC File Offset: 0x0007E3AC
		public static event GameSavesManager.GameSaved OnGameSaved;

		// Token: 0x06000BFD RID: 3069 RVA: 0x000801E0 File Offset: 0x0007E3E0
		public static string GetSaveSlotName(int id)
		{
			if (id < 0 || id > 6)
			{
				return string.Empty;
			}
			if (id < 5)
			{
				return "ScytheSave" + (id + 1).ToString() + ".xml";
			}
			return "ScytheSaveTmp.xml";
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00030115 File Offset: 0x0002E315
		public static string GetSaveHashFromData(string saveData)
		{
			return GameSavesManager.GameSavingManager.GetSaveHashFromData(saveData);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00030122 File Offset: 0x0002E322
		public static string GetSaveNameFromData(string saveData)
		{
			return GameSavesManager.GameSavingManager.GetSaveNameFromData(saveData);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x0003012F File Offset: 0x0002E32F
		public static string GetGameIdFromSlot(int saveSlotId)
		{
			return GameSavesManager.GameSavingManager.GetGameIdFromSlot(GameSavesManager.GetSaveSlotName(saveSlotId));
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x0002FC16 File Offset: 0x0002DE16
		public static int GetAutomaticSaveSlotId()
		{
			return 5;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00080220 File Offset: 0x0007E420
		public static List<SaveMetadata> GetSaves()
		{
			List<SaveMetadata> list = new List<SaveMetadata>();
			for (int i = 0; i < 6; i++)
			{
				SaveMetadata saveMetadata = GameSavesManager.GameSavingManager.GetSaveMetadata(GameSavesManager.GetSaveSlotName(i));
				if (saveMetadata != null)
				{
					list.Add(saveMetadata);
				}
			}
			return list;
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00030141 File Offset: 0x0002E341
		public static int GetAutomaticSaveIndex()
		{
			return GameSavesManager.GetSaves().Count - 1;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x0008025C File Offset: 0x0007E45C
		public static bool IsSaveSlotAccessible(int saveSlotId)
		{
			string saveSlotName = GameSavesManager.GetSaveSlotName(saveSlotId);
			return GameSavesManager.IsSaveSlotValid(saveSlotId) && (!GameSavesManager.GameSavingManager.IsSaveSlotIFA(saveSlotName) || GameServiceController.Instance.InvadersFromAfarUnlocked());
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0003014F File Offset: 0x0002E34F
		public static bool SaveSlotExists(string saveSlotName)
		{
			return GameSavesManager.GameSavingManager.IsSaveFileExisting(saveSlotName);
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00080294 File Offset: 0x0007E494
		public static void SaveGame(int slotId, string saveName, string gameId)
		{
			string saveSlotName = GameSavesManager.GetSaveSlotName(slotId);
			string text = GameSavesManager.GameSavingManager.SaveGame(saveSlotName, slotId, saveName, gameId);
			if (GameSavesManager.OnGameSaved != null)
			{
				GameSavesManager.OnGameSaved(saveSlotName, text);
			}
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0003015C File Offset: 0x0002E35C
		public static void LoadGame(string fileName)
		{
			GameSavesManager.GameSavingManager.LoadGame(fileName);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00030169 File Offset: 0x0002E369
		public static void DeleteGame(string fileName)
		{
			GameSavesManager.GameSavingManager.DeleteGame(fileName);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00030176 File Offset: 0x0002E376
		public static void UpdateSave(string fileName, string data)
		{
			GameSavesManager.GameSavingManager.UpdateSave(fileName, data);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x000802CC File Offset: 0x0007E4CC
		public static bool IsSaveSlotValid(int saveSlotId)
		{
			string saveSlotName = GameSavesManager.GetSaveSlotName(saveSlotId);
			bool flag = true;
			try
			{
				GameSavesManager.GameSavingManager.IsSaveValid(saveSlotName);
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0008030C File Offset: 0x0007E50C
		public static List<SaveMetadata> GetWorkingManualSaves()
		{
			List<SaveMetadata> list = new List<SaveMetadata>();
			for (int i = 0; i < 5; i++)
			{
				SaveMetadata saveMetadata = GameSavesManager.GameSavingManager.GetSaveMetadata(GameSavesManager.GetSaveSlotName(i));
				if (saveMetadata != null && GameSavesManager.IsSaveSlotValid(i))
				{
					list.Add(saveMetadata);
				}
			}
			return list;
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00030184 File Offset: 0x0002E384
		public static string GetAutomaticSaveGameId()
		{
			return GameSavesManager.GetSaves()[GameSavesManager.GetAutomaticSaveSlotId()].GameId;
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00080350 File Offset: 0x0007E550
		public static List<SaveMetadata> GetManualSavesByAutomaticGameId()
		{
			List<SaveMetadata> workingManualSaves = GameSavesManager.GetWorkingManualSaves();
			List<SaveMetadata> list = new List<SaveMetadata>();
			string @string = PlayerPrefs.GetString(OptionsManager.PREFS_OFFLINE_GAME_ID, string.Empty);
			foreach (SaveMetadata saveMetadata in workingManualSaves)
			{
				if (saveMetadata.GameId == @string)
				{
					list.Add(saveMetadata);
				}
			}
			return GameSavesManager.SortSavesByDate(list);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000803CC File Offset: 0x0007E5CC
		public static List<SaveMetadata> GetManualSavesByGameId(string gameId)
		{
			List<SaveMetadata> workingManualSaves = GameSavesManager.GetWorkingManualSaves();
			List<SaveMetadata> list = new List<SaveMetadata>();
			PlayerPrefs.GetString(OptionsManager.PREFS_OFFLINE_GAME_ID, string.Empty);
			foreach (SaveMetadata saveMetadata in workingManualSaves)
			{
				if (saveMetadata.GameId == gameId)
				{
					list.Add(saveMetadata);
				}
			}
			return GameSavesManager.SortSavesByDate(list);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0003019A File Offset: 0x0002E39A
		public static List<SaveMetadata> SortSavesByDate(List<SaveMetadata> savesList)
		{
			new List<SaveMetadata>();
			return savesList.OrderByDescending((SaveMetadata save) => save.LastWriteDate).ToList<SaveMetadata>();
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x000301CC File Offset: 0x0002E3CC
		public static bool ManualWorkingSaveExist()
		{
			return GameSavesManager.GetManualSavesByAutomaticGameId().Count > 0;
		}

		// Token: 0x040009A0 RID: 2464
		private const string SAVE_FILE_NAME = "ScytheSave";

		// Token: 0x040009A1 RID: 2465
		public const string AUTOMATIC_SAVE_FILE = "ScytheSaveTmp.xml";

		// Token: 0x040009A2 RID: 2466
		public const int NUMBER_OF_SAVES = 6;

		// Token: 0x040009A3 RID: 2467
		private static SavingManager savingManager;

		// Token: 0x0200019A RID: 410
		// (Invoke) Token: 0x06000C12 RID: 3090
		public delegate void GameSaved(string fileName, string saveData);
	}
}

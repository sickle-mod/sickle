using System;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000942 RID: 2370
	public abstract class KeyValueStore
	{
		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06003FA2 RID: 16290 RVA: 0x00050D7F File Offset: 0x0004EF7F
		// (set) Token: 0x06003FA3 RID: 16291 RVA: 0x00050D86 File Offset: 0x0004EF86
		public static KeyValueStore Instance { get; set; }

		// Token: 0x06003FA4 RID: 16292 RVA: 0x00050D8E File Offset: 0x0004EF8E
		public static void ResetInstance()
		{
			KeyValueStore.Instance = null;
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x00050D96 File Offset: 0x0004EF96
		public static void DeleteAll()
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance != null)
			{
				KeyValueStore.Instance._DeleteAll();
			}
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x00050DAE File Offset: 0x0004EFAE
		public static void DeleteKey(string key)
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance != null)
			{
				KeyValueStore.Instance._DeleteKey(key);
			}
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x00050DC7 File Offset: 0x0004EFC7
		public static bool HasKey(string key)
		{
			KeyValueStore._CheckInstance();
			return KeyValueStore.Instance != null && KeyValueStore.Instance._HasKey(key);
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x00050DE2 File Offset: 0x0004EFE2
		public static void Save()
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance != null)
			{
				KeyValueStore.Instance._Save();
			}
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x00050DFA File Offset: 0x0004EFFA
		public static int GetInt(string key, int defaultValue = 0)
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance == null)
			{
				return defaultValue;
			}
			return KeyValueStore.Instance._GetInt(key, defaultValue);
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x00050E16 File Offset: 0x0004F016
		public static float GetFloat(string key, float defaultValue = 0f)
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance == null)
			{
				return defaultValue;
			}
			return KeyValueStore.Instance._GetFloat(key, defaultValue);
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x00050E32 File Offset: 0x0004F032
		public static string GetString(string key, string defaultValue = "")
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance == null)
			{
				return defaultValue;
			}
			return KeyValueStore.Instance._GetString(key, defaultValue);
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x00050E4E File Offset: 0x0004F04E
		public static void SetInt(string key, int value)
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance != null)
			{
				KeyValueStore.Instance._SetInt(key, value);
			}
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x00050E68 File Offset: 0x0004F068
		public static void SetFloat(string key, float value)
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance != null)
			{
				KeyValueStore.Instance._SetFloat(key, value);
			}
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x00050E82 File Offset: 0x0004F082
		public static void SetString(string key, string value)
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance != null)
			{
				KeyValueStore.Instance._SetString(key, value);
			}
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x00050E9C File Offset: 0x0004F09C
		private static void _CheckInstance()
		{
			if (KeyValueStore.Instance == null)
			{
				AsmoLogger.Trace("KeyValueStore", "KeyValueStore is not set", null);
			}
		}

		// Token: 0x06003FB0 RID: 16304
		protected abstract void _DeleteAll();

		// Token: 0x06003FB1 RID: 16305
		protected abstract void _DeleteKey(string key);

		// Token: 0x06003FB2 RID: 16306
		protected abstract bool _HasKey(string key);

		// Token: 0x06003FB3 RID: 16307
		protected abstract void _Save();

		// Token: 0x06003FB4 RID: 16308
		protected abstract int _GetInt(string key, int defaultValue);

		// Token: 0x06003FB5 RID: 16309
		protected abstract float _GetFloat(string key, float defaultValue);

		// Token: 0x06003FB6 RID: 16310
		protected abstract string _GetString(string key, string defaultValue);

		// Token: 0x06003FB7 RID: 16311
		protected abstract void _SetInt(string key, int value);

		// Token: 0x06003FB8 RID: 16312
		protected abstract void _SetFloat(string key, float value);

		// Token: 0x06003FB9 RID: 16313
		protected abstract void _SetString(string key, string value);

		// Token: 0x040030B3 RID: 12467
		private const string _kModuleName = "KeyValueStore";
	}
}

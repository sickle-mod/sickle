using System;
using UnityEngine;

namespace Utils
{
	// Token: 0x02000176 RID: 374
	public abstract class KeyValueStore
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x0002F437 File Offset: 0x0002D637
		// (set) Token: 0x06000A92 RID: 2706 RVA: 0x0002F43E File Offset: 0x0002D63E
		public static KeyValueStore Instance { get; set; }

		// Token: 0x06000A93 RID: 2707 RVA: 0x0002F446 File Offset: 0x0002D646
		public static void ResetInstance()
		{
			KeyValueStore.Instance = null;
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0002F44E File Offset: 0x0002D64E
		public static void DeleteAll()
		{
			KeyValueStore._CheckInstance();
			KeyValueStore instance = KeyValueStore.Instance;
			if (instance == null)
			{
				return;
			}
			instance._DeleteAll();
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0002F464 File Offset: 0x0002D664
		public static void DeleteKey(string key)
		{
			KeyValueStore._CheckInstance();
			KeyValueStore instance = KeyValueStore.Instance;
			if (instance == null)
			{
				return;
			}
			instance._DeleteKey(key);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0002F47B File Offset: 0x0002D67B
		public static bool HasKey(string key)
		{
			KeyValueStore._CheckInstance();
			KeyValueStore instance = KeyValueStore.Instance;
			return instance != null && instance._HasKey(key);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0002F493 File Offset: 0x0002D693
		public static void Save()
		{
			KeyValueStore._CheckInstance();
			KeyValueStore instance = KeyValueStore.Instance;
			if (instance == null)
			{
				return;
			}
			instance._Save();
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0002F4A9 File Offset: 0x0002D6A9
		public static int GetInt(string key, int defaultValue = 0)
		{
			KeyValueStore._CheckInstance();
			KeyValueStore instance = KeyValueStore.Instance;
			if (instance == null)
			{
				return defaultValue;
			}
			return instance._GetInt(key, defaultValue);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0002F4C2 File Offset: 0x0002D6C2
		public static float GetFloat(string key, float defaultValue = 0f)
		{
			KeyValueStore._CheckInstance();
			KeyValueStore instance = KeyValueStore.Instance;
			if (instance == null)
			{
				return defaultValue;
			}
			return instance._GetFloat(key, defaultValue);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0002F4DB File Offset: 0x0002D6DB
		public static string GetString(string key, string defaultValue = "")
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance == null)
			{
				return defaultValue;
			}
			return KeyValueStore.Instance._GetString(key, defaultValue);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0002F4F7 File Offset: 0x0002D6F7
		public static void SetInt(string key, int value)
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance != null)
			{
				KeyValueStore.Instance._SetInt(key, value);
			}
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0002F511 File Offset: 0x0002D711
		public static void SetFloat(string key, float value)
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance != null)
			{
				KeyValueStore.Instance._SetFloat(key, value);
			}
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0002F52B File Offset: 0x0002D72B
		public static void SetString(string key, string value)
		{
			KeyValueStore._CheckInstance();
			if (KeyValueStore.Instance != null)
			{
				KeyValueStore.Instance._SetString(key, value);
			}
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0002F545 File Offset: 0x0002D745
		private static void _CheckInstance()
		{
			if (KeyValueStore.Instance == null)
			{
				Debug.Log("KeyValueStore is not set");
			}
		}

		// Token: 0x06000A9F RID: 2719
		protected abstract void _DeleteAll();

		// Token: 0x06000AA0 RID: 2720
		protected abstract void _DeleteKey(string key);

		// Token: 0x06000AA1 RID: 2721
		protected abstract bool _HasKey(string key);

		// Token: 0x06000AA2 RID: 2722
		protected abstract void _Save();

		// Token: 0x06000AA3 RID: 2723
		protected abstract int _GetInt(string key, int defaultValue);

		// Token: 0x06000AA4 RID: 2724
		protected abstract float _GetFloat(string key, float defaultValue);

		// Token: 0x06000AA5 RID: 2725
		protected abstract string _GetString(string key, string defaultValue);

		// Token: 0x06000AA6 RID: 2726
		protected abstract void _SetInt(string key, int value);

		// Token: 0x06000AA7 RID: 2727
		protected abstract void _SetFloat(string key, float value);

		// Token: 0x06000AA8 RID: 2728
		protected abstract void _SetString(string key, string value);

		// Token: 0x0400093F RID: 2367
		private const string _kModuleName = "KeyValueStore";
	}
}

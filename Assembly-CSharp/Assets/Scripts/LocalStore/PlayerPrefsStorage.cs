using System;
using Assets.Scripts.LocalStore.AuthStorage;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.LocalStore
{
	// Token: 0x020001B1 RID: 433
	public class PlayerPrefsStorage : IKeyValueStorage
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x000305E8 File Offset: 0x0002E7E8
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x000305F0 File Offset: 0x0002E7F0
		private IAuthKeyValueStore _authPrefsStore { get; set; }

		// Token: 0x06000C9A RID: 3226 RVA: 0x000305F9 File Offset: 0x0002E7F9
		public PlayerPrefsStorage()
		{
			this._authPrefsStore = new AuthPrefsStorage();
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0003060C File Offset: 0x0002E80C
		public IAuthKeyValueStore AuthData
		{
			get
			{
				return this._authPrefsStore;
			}
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00080F24 File Offset: 0x0007F124
		public void SetValue(string key, object value)
		{
			string text = JsonConvert.SerializeObject(value);
			PlayerPrefs.SetString(key, text);
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0002F5A3 File Offset: 0x0002D7A3
		public void SetValue(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x00030614 File Offset: 0x0002E814
		public string GetValue(string key, Action<Exception> onError = null)
		{
			return PlayerPrefs.GetString(key);
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00080F40 File Offset: 0x0007F140
		public T GetValue<T>(string key, Action onError = null)
		{
			T t;
			try
			{
				t = JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(key));
			}
			catch (Exception)
			{
				if (onError == null)
				{
					throw;
				}
				onError();
				t = default(T);
			}
			return t;
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0002F558 File Offset: 0x0002D758
		public void ClearAllKeys()
		{
			PlayerPrefs.DeleteAll();
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0002F55F File Offset: 0x0002D75F
		public void DeleteKey(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0002F567 File Offset: 0x0002D767
		public bool HasKey(string key)
		{
			return PlayerPrefs.HasKey(key);
		}
	}
}

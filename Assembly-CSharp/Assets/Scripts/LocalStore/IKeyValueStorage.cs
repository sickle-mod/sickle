using System;
using Assets.Scripts.LocalStore.AuthStorage;

namespace Assets.Scripts.LocalStore
{
	// Token: 0x020001B0 RID: 432
	public interface IKeyValueStorage
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000C90 RID: 3216
		IAuthKeyValueStore AuthData { get; }

		// Token: 0x06000C91 RID: 3217
		void SetValue(string key, object value);

		// Token: 0x06000C92 RID: 3218
		void SetValue(string key, string value);

		// Token: 0x06000C93 RID: 3219
		string GetValue(string key, Action<Exception> OnError = null);

		// Token: 0x06000C94 RID: 3220
		T GetValue<T>(string key, Action onError = null);

		// Token: 0x06000C95 RID: 3221
		void ClearAllKeys();

		// Token: 0x06000C96 RID: 3222
		void DeleteKey(string key);

		// Token: 0x06000C97 RID: 3223
		bool HasKey(string key);
	}
}

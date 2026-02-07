using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LocalStore.AuthStorage
{
	// Token: 0x020001B4 RID: 436
	public class AuthPrefsStorage : IAuthKeyValueStore
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x00030691 File Offset: 0x0002E891
		// (set) Token: 0x06000CB1 RID: 3249 RVA: 0x00030699 File Offset: 0x0002E899
		private List<string> _usedAuthPrefsKeys { get; set; }

		// Token: 0x06000CB2 RID: 3250 RVA: 0x000306A2 File Offset: 0x0002E8A2
		public AuthPrefsStorage()
		{
			this._usedAuthPrefsKeys = new List<string>
			{
				PlayerPrefsStorageKeys.Auth_PlayerNameKey,
				PlayerPrefsStorageKeys.Auth_PlayerAuthTokenKey,
				PlayerPrefsStorageKeys.Auth_LocalLoggedUserDataObj
			};
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x000306D6 File Offset: 0x0002E8D6
		public void Clear()
		{
			this._usedAuthPrefsKeys.ForEach(delegate(string p)
			{
				PlayerPrefs.DeleteKey(p);
			});
		}
	}
}

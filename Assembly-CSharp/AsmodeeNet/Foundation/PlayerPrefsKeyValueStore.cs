using System;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000944 RID: 2372
	public class PlayerPrefsKeyValueStore : KeyValueStore
	{
		// Token: 0x06003FC6 RID: 16326 RVA: 0x0002F558 File Offset: 0x0002D758
		protected override void _DeleteAll()
		{
			PlayerPrefs.DeleteAll();
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x0002F55F File Offset: 0x0002D75F
		protected override void _DeleteKey(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x0002F567 File Offset: 0x0002D767
		protected override bool _HasKey(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x0002F56F File Offset: 0x0002D76F
		protected override void _Save()
		{
			PlayerPrefs.Save();
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x0002F576 File Offset: 0x0002D776
		protected override int _GetInt(string key, int defaultValue)
		{
			return PlayerPrefs.GetInt(key, defaultValue);
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x0002F57F File Offset: 0x0002D77F
		protected override float _GetFloat(string key, float defaultValue)
		{
			return PlayerPrefs.GetFloat(key, defaultValue);
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x0002F588 File Offset: 0x0002D788
		protected override string _GetString(string key, string defaultValue)
		{
			return PlayerPrefs.GetString(key, defaultValue);
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x0002F591 File Offset: 0x0002D791
		protected override void _SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
		}

		// Token: 0x06003FCE RID: 16334 RVA: 0x0002F59A File Offset: 0x0002D79A
		protected override void _SetFloat(string key, float value)
		{
			PlayerPrefs.SetFloat(key, value);
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x0002F5A3 File Offset: 0x0002D7A3
		protected override void _SetString(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}
	}
}

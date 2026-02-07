using System;
using UnityEngine;

namespace Utils
{
	// Token: 0x02000177 RID: 375
	public class PlayerPrefsKeyValueStore : KeyValueStore
	{
		// Token: 0x06000AAA RID: 2730 RVA: 0x0002F558 File Offset: 0x0002D758
		protected override void _DeleteAll()
		{
			PlayerPrefs.DeleteAll();
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0002F55F File Offset: 0x0002D75F
		protected override void _DeleteKey(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0002F567 File Offset: 0x0002D767
		protected override bool _HasKey(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0002F56F File Offset: 0x0002D76F
		protected override void _Save()
		{
			PlayerPrefs.Save();
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0002F576 File Offset: 0x0002D776
		protected override int _GetInt(string key, int defaultValue)
		{
			return PlayerPrefs.GetInt(key, defaultValue);
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0002F57F File Offset: 0x0002D77F
		protected override float _GetFloat(string key, float defaultValue)
		{
			return PlayerPrefs.GetFloat(key, defaultValue);
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0002F588 File Offset: 0x0002D788
		protected override string _GetString(string key, string defaultValue)
		{
			return PlayerPrefs.GetString(key, defaultValue);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0002F591 File Offset: 0x0002D791
		protected override void _SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0002F59A File Offset: 0x0002D79A
		protected override void _SetFloat(string key, float value)
		{
			PlayerPrefs.SetFloat(key, value);
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0002F5A3 File Offset: 0x0002D7A3
		protected override void _SetString(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}
	}
}

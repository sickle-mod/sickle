using System;
using System.Collections.Generic;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000943 RID: 2371
	public class NonPersistentKeyValueStore : KeyValueStore
	{
		// Token: 0x06003FBB RID: 16315 RVA: 0x00050EB5 File Offset: 0x0004F0B5
		protected override void _DeleteAll()
		{
			this._ints.Clear();
			this._floats.Clear();
			this._strings.Clear();
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x00050ED8 File Offset: 0x0004F0D8
		protected override void _DeleteKey(string key)
		{
			this._ints.Remove(key);
			this._floats.Remove(key);
			this._strings.Remove(key);
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x00050F01 File Offset: 0x0004F101
		protected override bool _HasKey(string key)
		{
			return this._ints.ContainsKey(key) || this._floats.ContainsKey(key) || this._strings.ContainsKey(key);
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected override void _Save()
		{
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x0015CC60 File Offset: 0x0015AE60
		protected override int _GetInt(string key, int defaultValue)
		{
			int num;
			if (this._ints.TryGetValue(key, out num))
			{
				return num;
			}
			return defaultValue;
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x0015CC80 File Offset: 0x0015AE80
		protected override float _GetFloat(string key, float defaultValue)
		{
			float num;
			if (this._floats.TryGetValue(key, out num))
			{
				return num;
			}
			return defaultValue;
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x0015CCA0 File Offset: 0x0015AEA0
		protected override string _GetString(string key, string defaultValue)
		{
			string text;
			if (this._strings.TryGetValue(key, out text))
			{
				return text;
			}
			return defaultValue;
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x00050F2D File Offset: 0x0004F12D
		protected override void _SetInt(string key, int value)
		{
			this._DeleteKey(key);
			this._ints.Add(key, value);
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x00050F43 File Offset: 0x0004F143
		protected override void _SetFloat(string key, float value)
		{
			this._DeleteKey(key);
			this._floats.Add(key, value);
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x00050F59 File Offset: 0x0004F159
		protected override void _SetString(string key, string value)
		{
			this._DeleteKey(key);
			this._strings.Add(key, value);
		}

		// Token: 0x040030B5 RID: 12469
		private Dictionary<string, int> _ints = new Dictionary<string, int>();

		// Token: 0x040030B6 RID: 12470
		private Dictionary<string, float> _floats = new Dictionary<string, float>();

		// Token: 0x040030B7 RID: 12471
		private Dictionary<string, string> _strings = new Dictionary<string, string>();
	}
}

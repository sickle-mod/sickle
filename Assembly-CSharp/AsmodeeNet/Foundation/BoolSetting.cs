using System;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000952 RID: 2386
	[Serializable]
	public class BoolSetting : BaseSetting<bool>
	{
		// Token: 0x06004027 RID: 16423 RVA: 0x000513FA File Offset: 0x0004F5FA
		public BoolSetting(string name)
			: base(name)
		{
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x00051403 File Offset: 0x0004F603
		protected override bool _ReadValue()
		{
			return this._IntToBool(KeyValueStore.GetInt(base._FullPath, this._BoolToInt(base.DefaultValue)));
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x00051422 File Offset: 0x0004F622
		protected override void _WriteValue(bool value)
		{
			KeyValueStore.SetInt(base._FullPath, this._BoolToInt(value));
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x00051436 File Offset: 0x0004F636
		private bool _IntToBool(int i)
		{
			return i > 0;
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x0005143C File Offset: 0x0004F63C
		private int _BoolToInt(bool b)
		{
			if (!b)
			{
				return 0;
			}
			return 1;
		}
	}
}

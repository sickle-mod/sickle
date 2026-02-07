using System;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000953 RID: 2387
	[Serializable]
	public class StringSetting : BaseSetting<string>
	{
		// Token: 0x0600402C RID: 16428 RVA: 0x00051444 File Offset: 0x0004F644
		public StringSetting(string name)
			: base(name)
		{
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x0005144D File Offset: 0x0004F64D
		protected override string _ReadValue()
		{
			return KeyValueStore.GetString(base._FullPath, base.DefaultValue);
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x00051460 File Offset: 0x0004F660
		protected override void _WriteValue(string value)
		{
			KeyValueStore.SetString(base._FullPath, value);
		}
	}
}

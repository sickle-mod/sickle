using System;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000951 RID: 2385
	[Serializable]
	public class IntSetting : BaseSetting<int>
	{
		// Token: 0x06004024 RID: 16420 RVA: 0x000513D0 File Offset: 0x0004F5D0
		public IntSetting(string name)
			: base(name)
		{
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x000513D9 File Offset: 0x0004F5D9
		protected override int _ReadValue()
		{
			return KeyValueStore.GetInt(base._FullPath, base.DefaultValue);
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x000513EC File Offset: 0x0004F5EC
		protected override void _WriteValue(int value)
		{
			KeyValueStore.SetInt(base._FullPath, value);
		}
	}
}

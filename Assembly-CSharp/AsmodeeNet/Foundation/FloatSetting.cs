using System;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000950 RID: 2384
	[Serializable]
	public class FloatSetting : BaseSetting<float>
	{
		// Token: 0x06004021 RID: 16417 RVA: 0x000513A6 File Offset: 0x0004F5A6
		public FloatSetting(string name)
			: base(name)
		{
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x000513AF File Offset: 0x0004F5AF
		protected override float _ReadValue()
		{
			return KeyValueStore.GetFloat(base._FullPath, base.DefaultValue);
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x000513C2 File Offset: 0x0004F5C2
		protected override void _WriteValue(float value)
		{
			KeyValueStore.SetFloat(base._FullPath, value);
		}
	}
}

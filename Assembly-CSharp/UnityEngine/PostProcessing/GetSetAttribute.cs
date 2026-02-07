using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200070F RID: 1807
	public sealed class GetSetAttribute : PropertyAttribute
	{
		// Token: 0x0600366B RID: 13931 RVA: 0x0004AE80 File Offset: 0x00049080
		public GetSetAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x040027CF RID: 10191
		public readonly string name;

		// Token: 0x040027D0 RID: 10192
		public bool dirty;
	}
}

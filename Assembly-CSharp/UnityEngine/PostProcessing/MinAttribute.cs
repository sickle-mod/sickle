using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000710 RID: 1808
	public sealed class MinAttribute : PropertyAttribute
	{
		// Token: 0x0600366C RID: 13932 RVA: 0x0004AE8F File Offset: 0x0004908F
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x040027D1 RID: 10193
		public readonly float min;
	}
}

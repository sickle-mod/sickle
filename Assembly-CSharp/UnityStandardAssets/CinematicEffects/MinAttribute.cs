using System;
using UnityEngine;

namespace UnityStandardAssets.CinematicEffects
{
	// Token: 0x020006E7 RID: 1767
	public sealed class MinAttribute : PropertyAttribute
	{
		// Token: 0x06003583 RID: 13699 RVA: 0x0004A0E3 File Offset: 0x000482E3
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x0400272B RID: 10027
		public readonly float min;
	}
}

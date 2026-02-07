using System;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200084D RID: 2125
	public struct MathUtils
	{
		// Token: 0x06003C03 RID: 15363 RVA: 0x00154554 File Offset: 0x00152754
		public static bool Approximately(float value1, float value2, float epsilon)
		{
			float num = value1 - value2;
			if (num >= 0f)
			{
				return num <= epsilon;
			}
			return num >= -epsilon;
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x0004EDA7 File Offset: 0x0004CFA7
		public static bool Approximately(Vector2 value1, Vector2 value2, float epsilon)
		{
			return MathUtils.Approximately(value1.x, value2.x, epsilon) && MathUtils.Approximately(value1.y, value2.y, epsilon);
		}
	}
}

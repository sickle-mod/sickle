using System;
using UnityEngine;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x02000864 RID: 2148
	public static class ScreenExtension
	{
		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06003C6E RID: 15470 RVA: 0x001554E0 File Offset: 0x001536E0
		public static float DiagonalLengthInch
		{
			get
			{
				float num = Screen.dpi;
				if (num < 25f || num > 1000f)
				{
					num = 150f;
				}
				return Mathf.Sqrt((float)(Screen.width * Screen.width + Screen.height * Screen.height)) / num;
			}
		}
	}
}

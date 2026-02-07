using System;
using UnityEngine;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x02000860 RID: 2144
	public static class ColorExtension
	{
		// Token: 0x06003C63 RID: 15459 RVA: 0x0015524C File Offset: 0x0015344C
		public static string ToHex(this Color32 color, bool includeAlpha = false)
		{
			string text = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
			if (includeAlpha)
			{
				text += color.a.ToString("X2");
			}
			return text;
		}

		// Token: 0x06003C64 RID: 15460 RVA: 0x0004F183 File Offset: 0x0004D383
		public static string ToHex(this Color color, bool includeAlpha = false)
		{
			return color.ToHex(includeAlpha);
		}
	}
}

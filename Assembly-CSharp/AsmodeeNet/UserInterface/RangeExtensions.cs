using System;
using UnityEngine.SocialPlatforms;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007D6 RID: 2006
	internal static class RangeExtensions
	{
		// Token: 0x06003977 RID: 14711 RVA: 0x0004CEA9 File Offset: 0x0004B0A9
		public static int Last(this UnityEngine.SocialPlatforms.Range range)
		{
			if (range.count == 0)
			{
				throw new InvalidOperationException("Empty range has no to()");
			}
			return range.from + range.count - 1;
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x0004CECD File Offset: 0x0004B0CD
		public static bool Contains(this UnityEngine.SocialPlatforms.Range range, int num)
		{
			return num >= range.from && num < range.from + range.count;
		}
	}
}

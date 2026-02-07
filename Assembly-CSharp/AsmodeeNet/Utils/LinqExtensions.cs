using System;
using System.Collections.Generic;
using System.Linq;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200084C RID: 2124
	public static class LinqExtensions
	{
		// Token: 0x06003C01 RID: 15361 RVA: 0x0004ED7B File Offset: 0x0004CF7B
		public static IEnumerable<T> Diff<T>(this IEnumerable<T> first, IEnumerable<T> second)
		{
			return first.Except(second).Union(second.Except(first));
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x0004ED90 File Offset: 0x0004CF90
		public static IEnumerable<T> Diff<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T> comparer)
		{
			return first.Except(second, comparer).Union(second.Except(first, comparer));
		}
	}
}

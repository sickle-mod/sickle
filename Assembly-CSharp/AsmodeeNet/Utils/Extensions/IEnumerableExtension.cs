using System;
using System.Collections.Generic;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x02000862 RID: 2146
	public static class IEnumerableExtension
	{
		// Token: 0x06003C66 RID: 15462 RVA: 0x001552E4 File Offset: 0x001534E4
		public static T First<T>(this IEnumerable<T> items)
		{
			T t;
			using (IEnumerator<T> enumerator = items.GetEnumerator())
			{
				enumerator.MoveNext();
				t = enumerator.Current;
			}
			return t;
		}
	}
}

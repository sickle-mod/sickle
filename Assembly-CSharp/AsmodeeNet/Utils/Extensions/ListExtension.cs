using System;
using System.Collections.Generic;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x02000863 RID: 2147
	public static class ListExtension
	{
		// Token: 0x06003C67 RID: 15463 RVA: 0x00155324 File Offset: 0x00153524
		public static T First<T>(this List<T> items)
		{
			if (items != null && items.Count > 0)
			{
				return items[0];
			}
			return default(T);
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x00155350 File Offset: 0x00153550
		public static T Last<T>(this List<T> items)
		{
			if (items != null && items.Count > 0)
			{
				return items[items.Count - 1];
			}
			return default(T);
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x00155384 File Offset: 0x00153584
		public static T RemoveFirst<T>(this List<T> items)
		{
			if (items != null && items.Count > 0)
			{
				T t = items[0];
				items.RemoveAt(0);
				return t;
			}
			return default(T);
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x001553B8 File Offset: 0x001535B8
		public static T RemoveLast<T>(this List<T> items)
		{
			if (items != null && items.Count > 0)
			{
				T t = items[items.Count - 1];
				items.RemoveAt(items.Count - 1);
				return t;
			}
			return default(T);
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x001553F8 File Offset: 0x001535F8
		public static int? Max(this List<int?> items)
		{
			int? num = null;
			if (items != null && items.Count > 0)
			{
				int i = 0;
				while (i < items.Count)
				{
					if (num == null)
					{
						goto IL_004D;
					}
					int? num2 = items[i];
					int? num3 = num;
					if ((num2.GetValueOrDefault() > num3.GetValueOrDefault()) & ((num2 != null) & (num3 != null)))
					{
						goto IL_004D;
					}
					IL_0055:
					i++;
					continue;
					IL_004D:
					num = items[i];
					goto IL_0055;
				}
			}
			return num;
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x00155468 File Offset: 0x00153668
		public static int Max(this List<int> items)
		{
			int num = -1;
			if (items != null && items.Count > 0)
			{
				for (int i = 0; i < items.Count; i++)
				{
					if (items[i] > num)
					{
						num = items[i];
					}
				}
			}
			return num;
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x001554A8 File Offset: 0x001536A8
		public static int Sum(this List<int> items)
		{
			int num = 0;
			if (items != null && items.Count > 0)
			{
				for (int i = 0; i < items.Count; i++)
				{
					num += items[i];
				}
			}
			return num;
		}
	}
}

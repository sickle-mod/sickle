using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005A2 RID: 1442
	public class InvertedComparer : IComparer<int>
	{
		// Token: 0x06002DC3 RID: 11715 RVA: 0x00110A6C File Offset: 0x0010EC6C
		public int Compare(int x, int y)
		{
			int num = y.CompareTo(x);
			if (num == 0)
			{
				return 1;
			}
			return num;
		}
	}
}

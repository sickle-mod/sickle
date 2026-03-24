using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005B0 RID: 1456
	public class InvertedComparer : IComparer<int>
	{
		// Token: 0x06002E13 RID: 11795 RVA: 0x00117718 File Offset: 0x00115918
		public int Compare(int x, int y)
		{
			int num = y.CompareTo(x);
			return num;
		}
	}
}

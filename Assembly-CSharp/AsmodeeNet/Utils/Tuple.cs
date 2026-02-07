using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200085A RID: 2138
	public class Tuple<T1, T2>
	{
		// Token: 0x06003C4C RID: 15436 RVA: 0x0004F0F9 File Offset: 0x0004D2F9
		public Tuple(T1 item1, T2 item2)
		{
			this.Item1 = item1;
			this.Item2 = item2;
		}

		// Token: 0x04002DB8 RID: 11704
		public T1 Item1;

		// Token: 0x04002DB9 RID: 11705
		public T2 Item2;
	}
}

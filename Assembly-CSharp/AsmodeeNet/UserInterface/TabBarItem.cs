using System;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007AD RID: 1965
	public struct TabBarItem
	{
		// Token: 0x060038A0 RID: 14496 RVA: 0x0004C664 File Offset: 0x0004A864
		public TabBarItem(int tag, Tab tab, RectTransform transform)
		{
			this.tag = tag;
			this.tab = tab;
			this.transform = transform;
		}

		// Token: 0x04002A8C RID: 10892
		public int tag;

		// Token: 0x04002A8D RID: 10893
		public Tab tab;

		// Token: 0x04002A8E RID: 10894
		public RectTransform transform;
	}
}

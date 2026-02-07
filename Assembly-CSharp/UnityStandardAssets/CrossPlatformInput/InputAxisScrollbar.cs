using System;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput
{
	// Token: 0x020006F6 RID: 1782
	public class InputAxisScrollbar : MonoBehaviour
	{
		// Token: 0x060035D0 RID: 13776 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void Update()
		{
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x0004A50F File Offset: 0x0004870F
		public void HandleInput(float value)
		{
			CrossPlatformInputManager.SetAxis(this.axis, value * 2f - 1f);
		}

		// Token: 0x04002763 RID: 10083
		public string axis;
	}
}

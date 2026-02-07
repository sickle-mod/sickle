using System;
using System.Collections.Generic;
using Scythe.Utilities;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020003C0 RID: 960
	public class CanvasHider : SingletonMono<CanvasHider>
	{
		// Token: 0x06001C00 RID: 7168 RVA: 0x0003A4DC File Offset: 0x000386DC
		public void RegisterCanvas(Canvas canvas)
		{
			this.canvases.Add(canvas);
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x000AFAB4 File Offset: 0x000ADCB4
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F11))
			{
				foreach (Canvas canvas in this.canvases)
				{
					this.ChangeCanvasEnabledState(canvas);
				}
			}
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x0003A4EA File Offset: 0x000386EA
		private void ChangeCanvasEnabledState(Canvas canvas)
		{
			canvas.enabled = !canvas.enabled;
		}

		// Token: 0x0400141A RID: 5146
		private List<Canvas> canvases = new List<Canvas>();
	}
}

using System;
using UnityEngine;

namespace HoneyFramework
{
	// Token: 0x020001B9 RID: 441
	public class CameraParams
	{
		// Token: 0x06000CE8 RID: 3304 RVA: 0x000307E4 File Offset: 0x0002E9E4
		public CameraParams()
		{
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00084428 File Offset: 0x00082628
		public CameraParams(CameraControler cam)
		{
			this.stickMinZoom = cam.stickMinZoom;
			this.stickMaxZoom = cam.stickMaxZoom;
			this.swivelMinZoom = cam.swivelMinZoom;
			this.swivelMaxZoom = cam.swivelMaxZoom;
			this.posXMin = cam.posXMin;
			this.posXMax = cam.posXMax;
			this.posYMin = cam.posYMin;
			this.posYMax = cam.posYMax;
			this.zoom = cam.zoom;
			this.swivel = cam.swivel;
			this.stick = cam.stick;
		}

		// Token: 0x04000A53 RID: 2643
		public float stickMinZoom;

		// Token: 0x04000A54 RID: 2644
		public float stickMaxZoom;

		// Token: 0x04000A55 RID: 2645
		public float swivelMinZoom;

		// Token: 0x04000A56 RID: 2646
		public float swivelMaxZoom;

		// Token: 0x04000A57 RID: 2647
		public float posXMin = -15f;

		// Token: 0x04000A58 RID: 2648
		public float posXMax = 15f;

		// Token: 0x04000A59 RID: 2649
		public float posYMin = -15f;

		// Token: 0x04000A5A RID: 2650
		public float posYMax = 15f;

		// Token: 0x04000A5B RID: 2651
		public float zoom = 0.5f;

		// Token: 0x04000A5C RID: 2652
		public Transform swivel;

		// Token: 0x04000A5D RID: 2653
		public Transform stick;
	}
}

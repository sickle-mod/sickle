using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006E3 RID: 1763
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Displacement/Vortex")]
	public class Vortex : ImageEffectBase
	{
		// Token: 0x06003573 RID: 13683 RVA: 0x00049F65 File Offset: 0x00048165
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
		}

		// Token: 0x0400271C RID: 10012
		public Vector2 radius = new Vector2(0.4f, 0.4f);

		// Token: 0x0400271D RID: 10013
		public float angle = 50f;

		// Token: 0x0400271E RID: 10014
		public Vector2 center = new Vector2(0.5f, 0.5f);
	}
}

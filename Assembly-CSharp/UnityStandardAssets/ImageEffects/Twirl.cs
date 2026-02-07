using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006E0 RID: 1760
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Displacement/Twirl")]
	public class Twirl : ImageEffectBase
	{
		// Token: 0x0600356E RID: 13678 RVA: 0x00049F07 File Offset: 0x00048107
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
		}

		// Token: 0x04002708 RID: 9992
		public Vector2 radius = new Vector2(0.3f, 0.3f);

		// Token: 0x04002709 RID: 9993
		[Range(0f, 360f)]
		public float angle = 50f;

		// Token: 0x0400270A RID: 9994
		public Vector2 center = new Vector2(0.5f, 0.5f);
	}
}

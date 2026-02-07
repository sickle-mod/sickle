using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C7 RID: 1735
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
	public class Grayscale : ImageEffectBase
	{
		// Token: 0x06003519 RID: 13593 RVA: 0x00049B56 File Offset: 0x00047D56
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.material.SetTexture("_RampTex", this.textureRamp);
			base.material.SetFloat("_RampOffset", this.rampOffset);
			Graphics.Blit(source, destination, base.material);
		}

		// Token: 0x04002672 RID: 9842
		public Texture textureRamp;

		// Token: 0x04002673 RID: 9843
		[Range(-1f, 1f)]
		public float rampOffset;
	}
}

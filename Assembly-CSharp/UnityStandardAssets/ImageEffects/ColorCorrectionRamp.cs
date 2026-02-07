using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006B6 RID: 1718
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Ramp)")]
	public class ColorCorrectionRamp : ImageEffectBase
	{
		// Token: 0x060034DE RID: 13534 RVA: 0x000497D6 File Offset: 0x000479D6
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.material.SetTexture("_RampTex", this.textureRamp);
			Graphics.Blit(source, destination, base.material);
		}

		// Token: 0x040025D3 RID: 9683
		public Texture textureRamp;
	}
}

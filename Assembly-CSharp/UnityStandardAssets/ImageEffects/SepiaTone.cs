using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006D5 RID: 1749
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Sepia Tone")]
	public class SepiaTone : ImageEffectBase
	{
		// Token: 0x0600355B RID: 13659 RVA: 0x00049E9D File Offset: 0x0004809D
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination, base.material);
		}
	}
}

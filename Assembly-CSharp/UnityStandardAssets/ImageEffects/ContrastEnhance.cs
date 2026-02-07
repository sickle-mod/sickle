using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006B7 RID: 1719
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Color Adjustments/Contrast Enhance (Unsharp Mask)")]
	public class ContrastEnhance : PostEffectsBase
	{
		// Token: 0x060034E0 RID: 13536 RVA: 0x00139A14 File Offset: 0x00137C14
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.contrastCompositeMaterial = base.CheckShaderAndCreateMaterial(this.contrastCompositeShader, this.contrastCompositeMaterial);
			this.separableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x00139A70 File Offset: 0x00137C70
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int width = source.width;
			int height = source.height;
			RenderTexture temporary = RenderTexture.GetTemporary(width / 2, height / 2, 0);
			Graphics.Blit(source, temporary);
			RenderTexture renderTexture = RenderTexture.GetTemporary(width / 4, height / 4, 0);
			Graphics.Blit(temporary, renderTexture);
			RenderTexture.ReleaseTemporary(temporary);
			this.separableBlurMaterial.SetVector("offsets", new Vector4(0f, this.blurSpread * 1f / (float)renderTexture.height, 0f, 0f));
			RenderTexture temporary2 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
			Graphics.Blit(renderTexture, temporary2, this.separableBlurMaterial);
			RenderTexture.ReleaseTemporary(renderTexture);
			this.separableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * 1f / (float)renderTexture.width, 0f, 0f, 0f));
			renderTexture = RenderTexture.GetTemporary(width / 4, height / 4, 0);
			Graphics.Blit(temporary2, renderTexture, this.separableBlurMaterial);
			RenderTexture.ReleaseTemporary(temporary2);
			this.contrastCompositeMaterial.SetTexture("_MainTexBlurred", renderTexture);
			this.contrastCompositeMaterial.SetFloat("intensity", this.intensity);
			this.contrastCompositeMaterial.SetFloat("threshold", this.threshold);
			Graphics.Blit(source, destination, this.contrastCompositeMaterial);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x040025D4 RID: 9684
		[Range(0f, 1f)]
		public float intensity = 0.5f;

		// Token: 0x040025D5 RID: 9685
		[Range(0f, 0.999f)]
		public float threshold;

		// Token: 0x040025D6 RID: 9686
		private Material separableBlurMaterial;

		// Token: 0x040025D7 RID: 9687
		private Material contrastCompositeMaterial;

		// Token: 0x040025D8 RID: 9688
		[Range(0f, 1f)]
		public float blurSpread = 1f;

		// Token: 0x040025D9 RID: 9689
		public Shader separableBlurShader;

		// Token: 0x040025DA RID: 9690
		public Shader contrastCompositeShader;
	}
}

using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006AF RID: 1711
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Blur/Blur (Optimized)")]
	public class BlurOptimized : PostEffectsBase
	{
		// Token: 0x060034BF RID: 13503 RVA: 0x0004962D File Offset: 0x0004782D
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x00049663 File Offset: 0x00047863
		public void OnDisable()
		{
			if (this.blurMaterial)
			{
				global::UnityEngine.Object.DestroyImmediate(this.blurMaterial);
			}
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x001381F4 File Offset: 0x001363F4
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			float num = 1f / (1f * (float)(1 << this.downsample));
			this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num, -this.blurSize * num, 0f, 0f));
			source.filterMode = FilterMode.Bilinear;
			int num2 = source.width >> this.downsample;
			int num3 = source.height >> this.downsample;
			RenderTexture renderTexture = RenderTexture.GetTemporary(num2, num3, 0, source.format);
			renderTexture.filterMode = FilterMode.Bilinear;
			Graphics.Blit(source, renderTexture, this.blurMaterial, 0);
			int num4 = ((this.blurType == BlurOptimized.BlurType.StandardGauss) ? 0 : 2);
			for (int i = 0; i < this.blurIterations; i++)
			{
				float num5 = (float)i * 1f;
				this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num + num5, -this.blurSize * num - num5, 0f, 0f));
				RenderTexture renderTexture2 = RenderTexture.GetTemporary(num2, num3, 0, source.format);
				renderTexture2.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, renderTexture2, this.blurMaterial, 1 + num4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = renderTexture2;
				renderTexture2 = RenderTexture.GetTemporary(num2, num3, 0, source.format);
				renderTexture2.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, renderTexture2, this.blurMaterial, 2 + num4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = renderTexture2;
			}
			Graphics.Blit(renderTexture, destination);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x04002585 RID: 9605
		[Range(0f, 2f)]
		public int downsample = 1;

		// Token: 0x04002586 RID: 9606
		[Range(0f, 10f)]
		public float blurSize = 3f;

		// Token: 0x04002587 RID: 9607
		[Range(1f, 4f)]
		public int blurIterations = 2;

		// Token: 0x04002588 RID: 9608
		public BlurOptimized.BlurType blurType;

		// Token: 0x04002589 RID: 9609
		public Shader blurShader;

		// Token: 0x0400258A RID: 9610
		private Material blurMaterial;

		// Token: 0x020006B0 RID: 1712
		public enum BlurType
		{
			// Token: 0x0400258C RID: 9612
			StandardGauss,
			// Token: 0x0400258D RID: 9613
			SgxGauss
		}
	}
}

using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006AB RID: 1707
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/Bloom (Optimized)")]
	public class BloomOptimized : PostEffectsBase
	{
		// Token: 0x060034B4 RID: 13492 RVA: 0x00049522 File Offset: 0x00047722
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.fastBloomMaterial = base.CheckShaderAndCreateMaterial(this.fastBloomShader, this.fastBloomMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x00049558 File Offset: 0x00047758
		private void OnDisable()
		{
			if (this.fastBloomMaterial)
			{
				global::UnityEngine.Object.DestroyImmediate(this.fastBloomMaterial);
			}
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x00137F08 File Offset: 0x00136108
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int num = ((this.resolution == BloomOptimized.Resolution.Low) ? 4 : 2);
			float num2 = ((this.resolution == BloomOptimized.Resolution.Low) ? 0.5f : 1f);
			this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2, 0f, this.threshold, this.intensity));
			source.filterMode = FilterMode.Bilinear;
			int num3 = source.width / num;
			int num4 = source.height / num;
			RenderTexture renderTexture = RenderTexture.GetTemporary(num3, num4, 0, source.format);
			renderTexture.filterMode = FilterMode.Bilinear;
			Graphics.Blit(source, renderTexture, this.fastBloomMaterial, 1);
			int num5 = ((this.blurType == BloomOptimized.BlurType.Standard) ? 0 : 2);
			for (int i = 0; i < this.blurIterations; i++)
			{
				this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2 + (float)i * 1f, 0f, this.threshold, this.intensity));
				RenderTexture renderTexture2 = RenderTexture.GetTemporary(num3, num4, 0, source.format);
				renderTexture2.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, renderTexture2, this.fastBloomMaterial, 2 + num5);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = renderTexture2;
				renderTexture2 = RenderTexture.GetTemporary(num3, num4, 0, source.format);
				renderTexture2.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, renderTexture2, this.fastBloomMaterial, 3 + num5);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = renderTexture2;
			}
			this.fastBloomMaterial.SetTexture("_Bloom", renderTexture);
			Graphics.Blit(source, destination, this.fastBloomMaterial, 0);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x04002572 RID: 9586
		[Range(0f, 1.5f)]
		public float threshold = 0.25f;

		// Token: 0x04002573 RID: 9587
		[Range(0f, 2.5f)]
		public float intensity = 0.75f;

		// Token: 0x04002574 RID: 9588
		[Range(0.25f, 5.5f)]
		public float blurSize = 1f;

		// Token: 0x04002575 RID: 9589
		private BloomOptimized.Resolution resolution;

		// Token: 0x04002576 RID: 9590
		[Range(1f, 4f)]
		public int blurIterations = 1;

		// Token: 0x04002577 RID: 9591
		public BloomOptimized.BlurType blurType;

		// Token: 0x04002578 RID: 9592
		public Shader fastBloomShader;

		// Token: 0x04002579 RID: 9593
		private Material fastBloomMaterial;

		// Token: 0x020006AC RID: 1708
		public enum Resolution
		{
			// Token: 0x0400257B RID: 9595
			Low,
			// Token: 0x0400257C RID: 9596
			High
		}

		// Token: 0x020006AD RID: 1709
		public enum BlurType
		{
			// Token: 0x0400257E RID: 9598
			Standard,
			// Token: 0x0400257F RID: 9599
			Sgx
		}
	}
}

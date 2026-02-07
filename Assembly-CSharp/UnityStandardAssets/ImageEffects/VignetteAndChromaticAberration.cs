using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006E1 RID: 1761
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Vignette and Chromatic Aberration")]
	public class VignetteAndChromaticAberration : PostEffectsBase
	{
		// Token: 0x06003570 RID: 13680 RVA: 0x0013F4E8 File Offset: 0x0013D6E8
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.m_VignetteMaterial = base.CheckShaderAndCreateMaterial(this.vignetteShader, this.m_VignetteMaterial);
			this.m_SeparableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.m_SeparableBlurMaterial);
			this.m_ChromAberrationMaterial = base.CheckShaderAndCreateMaterial(this.chromAberrationShader, this.m_ChromAberrationMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x0013F55C File Offset: 0x0013D75C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int width = source.width;
			int height = source.height;
			bool flag = Mathf.Abs(this.blur) > 0f || Mathf.Abs(this.intensity) > 0f;
			float num = 1f * (float)width / (1f * (float)height);
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			if (flag)
			{
				renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
				if (Mathf.Abs(this.blur) > 0f)
				{
					renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
					Graphics.Blit(source, renderTexture2, this.m_ChromAberrationMaterial, 0);
					for (int i = 0; i < 2; i++)
					{
						this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(0f, this.blurSpread * 0.001953125f, 0f, 0f));
						RenderTexture temporary = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
						Graphics.Blit(renderTexture2, temporary, this.m_SeparableBlurMaterial);
						RenderTexture.ReleaseTemporary(renderTexture2);
						this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * 0.001953125f / num, 0f, 0f, 0f));
						renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
						Graphics.Blit(temporary, renderTexture2, this.m_SeparableBlurMaterial);
						RenderTexture.ReleaseTemporary(temporary);
					}
				}
				this.m_VignetteMaterial.SetFloat("_Intensity", 1f / (1f - this.intensity) - 1f);
				this.m_VignetteMaterial.SetFloat("_Blur", 1f / (1f - this.blur) - 1f);
				this.m_VignetteMaterial.SetTexture("_VignetteTex", renderTexture2);
				Graphics.Blit(source, renderTexture, this.m_VignetteMaterial, 0);
			}
			this.m_ChromAberrationMaterial.SetFloat("_ChromaticAberration", this.chromaticAberration);
			this.m_ChromAberrationMaterial.SetFloat("_AxialAberration", this.axialAberration);
			this.m_ChromAberrationMaterial.SetVector("_BlurDistance", new Vector2(-this.blurDistance, this.blurDistance));
			this.m_ChromAberrationMaterial.SetFloat("_Luminance", 1f / Mathf.Max(Mathf.Epsilon, this.luminanceDependency));
			if (flag)
			{
				renderTexture.wrapMode = TextureWrapMode.Clamp;
			}
			else
			{
				source.wrapMode = TextureWrapMode.Clamp;
			}
			Graphics.Blit(flag ? renderTexture : source, destination, this.m_ChromAberrationMaterial, (this.mode == VignetteAndChromaticAberration.AberrationMode.Advanced) ? 2 : 1);
			RenderTexture.ReleaseTemporary(renderTexture);
			RenderTexture.ReleaseTemporary(renderTexture2);
		}

		// Token: 0x0400270B RID: 9995
		public VignetteAndChromaticAberration.AberrationMode mode;

		// Token: 0x0400270C RID: 9996
		public float intensity = 0.036f;

		// Token: 0x0400270D RID: 9997
		public float chromaticAberration = 0.2f;

		// Token: 0x0400270E RID: 9998
		public float axialAberration = 0.5f;

		// Token: 0x0400270F RID: 9999
		public float blur;

		// Token: 0x04002710 RID: 10000
		public float blurSpread = 0.75f;

		// Token: 0x04002711 RID: 10001
		public float luminanceDependency = 0.25f;

		// Token: 0x04002712 RID: 10002
		public float blurDistance = 2.5f;

		// Token: 0x04002713 RID: 10003
		public Shader vignetteShader;

		// Token: 0x04002714 RID: 10004
		public Shader separableBlurShader;

		// Token: 0x04002715 RID: 10005
		public Shader chromAberrationShader;

		// Token: 0x04002716 RID: 10006
		private Material m_VignetteMaterial;

		// Token: 0x04002717 RID: 10007
		private Material m_SeparableBlurMaterial;

		// Token: 0x04002718 RID: 10008
		private Material m_ChromAberrationMaterial;

		// Token: 0x020006E2 RID: 1762
		public enum AberrationMode
		{
			// Token: 0x0400271A RID: 10010
			Simple,
			// Token: 0x0400271B RID: 10011
			Advanced
		}
	}
}

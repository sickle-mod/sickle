using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006AA RID: 1706
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/BloomAndFlares (3.5, Deprecated)")]
	public class BloomAndFlares : PostEffectsBase
	{
		// Token: 0x060034AD RID: 13485 RVA: 0x001374F8 File Offset: 0x001356F8
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.screenBlend = base.CheckShaderAndCreateMaterial(this.screenBlendShader, this.screenBlend);
			this.lensFlareMaterial = base.CheckShaderAndCreateMaterial(this.lensFlareShader, this.lensFlareMaterial);
			this.vignetteMaterial = base.CheckShaderAndCreateMaterial(this.vignetteShader, this.vignetteMaterial);
			this.separableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
			this.addBrightStuffBlendOneOneMaterial = base.CheckShaderAndCreateMaterial(this.addBrightStuffOneOneShader, this.addBrightStuffBlendOneOneMaterial);
			this.hollywoodFlaresMaterial = base.CheckShaderAndCreateMaterial(this.hollywoodFlaresShader, this.hollywoodFlaresMaterial);
			this.brightPassFilterMaterial = base.CheckShaderAndCreateMaterial(this.brightPassFilterShader, this.brightPassFilterMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x001375CC File Offset: 0x001357CC
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.doHdr = false;
			if (this.hdr == HDRBloomMode.Auto)
			{
				this.doHdr = source.format == RenderTextureFormat.ARGBHalf && base.GetComponent<Camera>().allowHDR;
			}
			else
			{
				this.doHdr = this.hdr == HDRBloomMode.On;
			}
			this.doHdr = this.doHdr && this.supportHDRTextures;
			BloomScreenBlendMode bloomScreenBlendMode = this.screenBlendMode;
			if (this.doHdr)
			{
				bloomScreenBlendMode = BloomScreenBlendMode.Add;
			}
			RenderTextureFormat renderTextureFormat = (this.doHdr ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.Default);
			RenderTexture temporary = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, renderTextureFormat);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, renderTextureFormat);
			RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, renderTextureFormat);
			RenderTexture temporary4 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, renderTextureFormat);
			float num = 1f * (float)source.width / (1f * (float)source.height);
			float num2 = 0.001953125f;
			Graphics.Blit(source, temporary, this.screenBlend, 2);
			Graphics.Blit(temporary, temporary2, this.screenBlend, 2);
			RenderTexture.ReleaseTemporary(temporary);
			this.BrightFilter(this.bloomThreshold, this.useSrcAlphaAsMask, temporary2, temporary3);
			temporary2.DiscardContents();
			if (this.bloomBlurIterations < 1)
			{
				this.bloomBlurIterations = 1;
			}
			for (int i = 0; i < this.bloomBlurIterations; i++)
			{
				float num3 = (1f + (float)i * 0.5f) * this.sepBlurSpread;
				this.separableBlurMaterial.SetVector("offsets", new Vector4(0f, num3 * num2, 0f, 0f));
				RenderTexture renderTexture = ((i == 0) ? temporary3 : temporary2);
				Graphics.Blit(renderTexture, temporary4, this.separableBlurMaterial);
				renderTexture.DiscardContents();
				this.separableBlurMaterial.SetVector("offsets", new Vector4(num3 / num * num2, 0f, 0f, 0f));
				Graphics.Blit(temporary4, temporary2, this.separableBlurMaterial);
				temporary4.DiscardContents();
			}
			if (this.lensflares)
			{
				if (this.lensflareMode == LensflareStyle34.Ghosting)
				{
					this.BrightFilter(this.lensflareThreshold, 0f, temporary2, temporary4);
					temporary2.DiscardContents();
					this.Vignette(0.975f, temporary4, temporary3);
					temporary4.DiscardContents();
					this.BlendFlares(temporary3, temporary2);
					temporary3.DiscardContents();
				}
				else
				{
					this.hollywoodFlaresMaterial.SetVector("_threshold", new Vector4(this.lensflareThreshold, 1f / (1f - this.lensflareThreshold), 0f, 0f));
					this.hollywoodFlaresMaterial.SetVector("tintColor", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.flareColorA.a * this.lensflareIntensity);
					Graphics.Blit(temporary4, temporary3, this.hollywoodFlaresMaterial, 2);
					temporary4.DiscardContents();
					Graphics.Blit(temporary3, temporary4, this.hollywoodFlaresMaterial, 3);
					temporary3.DiscardContents();
					this.hollywoodFlaresMaterial.SetVector("offsets", new Vector4(this.sepBlurSpread * 1f / num * num2, 0f, 0f, 0f));
					this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth);
					Graphics.Blit(temporary4, temporary3, this.hollywoodFlaresMaterial, 1);
					temporary4.DiscardContents();
					this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth * 2f);
					Graphics.Blit(temporary3, temporary4, this.hollywoodFlaresMaterial, 1);
					temporary3.DiscardContents();
					this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth * 4f);
					Graphics.Blit(temporary4, temporary3, this.hollywoodFlaresMaterial, 1);
					temporary4.DiscardContents();
					if (this.lensflareMode == LensflareStyle34.Anamorphic)
					{
						for (int j = 0; j < this.hollywoodFlareBlurIterations; j++)
						{
							this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
							Graphics.Blit(temporary3, temporary4, this.separableBlurMaterial);
							temporary3.DiscardContents();
							this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
							Graphics.Blit(temporary4, temporary3, this.separableBlurMaterial);
							temporary4.DiscardContents();
						}
						this.AddTo(1f, temporary3, temporary2);
						temporary3.DiscardContents();
					}
					else
					{
						for (int k = 0; k < this.hollywoodFlareBlurIterations; k++)
						{
							this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
							Graphics.Blit(temporary3, temporary4, this.separableBlurMaterial);
							temporary3.DiscardContents();
							this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
							Graphics.Blit(temporary4, temporary3, this.separableBlurMaterial);
							temporary4.DiscardContents();
						}
						this.Vignette(1f, temporary3, temporary4);
						temporary3.DiscardContents();
						this.BlendFlares(temporary4, temporary3);
						temporary4.DiscardContents();
						this.AddTo(1f, temporary3, temporary2);
						temporary3.DiscardContents();
					}
				}
			}
			this.screenBlend.SetFloat("_Intensity", this.bloomIntensity);
			this.screenBlend.SetTexture("_ColorBuffer", source);
			Graphics.Blit(temporary2, destination, this.screenBlend, (int)bloomScreenBlendMode);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(temporary3);
			RenderTexture.ReleaseTemporary(temporary4);
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x00049502 File Offset: 0x00047702
		private void AddTo(float intensity_, RenderTexture from, RenderTexture to)
		{
			this.addBrightStuffBlendOneOneMaterial.SetFloat("_Intensity", intensity_);
			Graphics.Blit(from, to, this.addBrightStuffBlendOneOneMaterial);
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x00137BDC File Offset: 0x00135DDC
		private void BlendFlares(RenderTexture from, RenderTexture to)
		{
			this.lensFlareMaterial.SetVector("colorA", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorB", new Vector4(this.flareColorB.r, this.flareColorB.g, this.flareColorB.b, this.flareColorB.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorC", new Vector4(this.flareColorC.r, this.flareColorC.g, this.flareColorC.b, this.flareColorC.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorD", new Vector4(this.flareColorD.r, this.flareColorD.g, this.flareColorD.b, this.flareColorD.a) * this.lensflareIntensity);
			Graphics.Blit(from, to, this.lensFlareMaterial);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x00137D28 File Offset: 0x00135F28
		private void BrightFilter(float thresh, float useAlphaAsMask, RenderTexture from, RenderTexture to)
		{
			if (this.doHdr)
			{
				this.brightPassFilterMaterial.SetVector("threshold", new Vector4(thresh, 1f, 0f, 0f));
			}
			else
			{
				this.brightPassFilterMaterial.SetVector("threshold", new Vector4(thresh, 1f / (1f - thresh), 0f, 0f));
			}
			this.brightPassFilterMaterial.SetFloat("useSrcAlphaAsMask", useAlphaAsMask);
			Graphics.Blit(from, to, this.brightPassFilterMaterial);
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x00137DB0 File Offset: 0x00135FB0
		private void Vignette(float amount, RenderTexture from, RenderTexture to)
		{
			if (this.lensFlareVignetteMask)
			{
				this.screenBlend.SetTexture("_ColorBuffer", this.lensFlareVignetteMask);
				Graphics.Blit(from, to, this.screenBlend, 3);
				return;
			}
			this.vignetteMaterial.SetFloat("vignetteIntensity", amount);
			Graphics.Blit(from, to, this.vignetteMaterial);
		}

		// Token: 0x04002550 RID: 9552
		public TweakMode34 tweakMode;

		// Token: 0x04002551 RID: 9553
		public BloomScreenBlendMode screenBlendMode = BloomScreenBlendMode.Add;

		// Token: 0x04002552 RID: 9554
		public HDRBloomMode hdr;

		// Token: 0x04002553 RID: 9555
		private bool doHdr;

		// Token: 0x04002554 RID: 9556
		public float sepBlurSpread = 1.5f;

		// Token: 0x04002555 RID: 9557
		public float useSrcAlphaAsMask = 0.5f;

		// Token: 0x04002556 RID: 9558
		public float bloomIntensity = 1f;

		// Token: 0x04002557 RID: 9559
		public float bloomThreshold = 0.5f;

		// Token: 0x04002558 RID: 9560
		public int bloomBlurIterations = 2;

		// Token: 0x04002559 RID: 9561
		public bool lensflares;

		// Token: 0x0400255A RID: 9562
		public int hollywoodFlareBlurIterations = 2;

		// Token: 0x0400255B RID: 9563
		public LensflareStyle34 lensflareMode = LensflareStyle34.Anamorphic;

		// Token: 0x0400255C RID: 9564
		public float hollyStretchWidth = 3.5f;

		// Token: 0x0400255D RID: 9565
		public float lensflareIntensity = 1f;

		// Token: 0x0400255E RID: 9566
		public float lensflareThreshold = 0.3f;

		// Token: 0x0400255F RID: 9567
		public Color flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);

		// Token: 0x04002560 RID: 9568
		public Color flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);

		// Token: 0x04002561 RID: 9569
		public Color flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);

		// Token: 0x04002562 RID: 9570
		public Color flareColorD = new Color(0.8f, 0.4f, 0f, 0.75f);

		// Token: 0x04002563 RID: 9571
		public Texture2D lensFlareVignetteMask;

		// Token: 0x04002564 RID: 9572
		public Shader lensFlareShader;

		// Token: 0x04002565 RID: 9573
		private Material lensFlareMaterial;

		// Token: 0x04002566 RID: 9574
		public Shader vignetteShader;

		// Token: 0x04002567 RID: 9575
		private Material vignetteMaterial;

		// Token: 0x04002568 RID: 9576
		public Shader separableBlurShader;

		// Token: 0x04002569 RID: 9577
		private Material separableBlurMaterial;

		// Token: 0x0400256A RID: 9578
		public Shader addBrightStuffOneOneShader;

		// Token: 0x0400256B RID: 9579
		private Material addBrightStuffBlendOneOneMaterial;

		// Token: 0x0400256C RID: 9580
		public Shader screenBlendShader;

		// Token: 0x0400256D RID: 9581
		private Material screenBlend;

		// Token: 0x0400256E RID: 9582
		public Shader hollywoodFlaresShader;

		// Token: 0x0400256F RID: 9583
		private Material hollywoodFlaresMaterial;

		// Token: 0x04002570 RID: 9584
		public Shader brightPassFilterShader;

		// Token: 0x04002571 RID: 9585
		private Material brightPassFilterMaterial;
	}
}

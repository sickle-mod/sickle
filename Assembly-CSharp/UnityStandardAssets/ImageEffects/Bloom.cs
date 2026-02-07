using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006A0 RID: 1696
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/Bloom")]
	public class Bloom : PostEffectsBase
	{
		// Token: 0x060034A5 RID: 13477 RVA: 0x00136A38 File Offset: 0x00134C38
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.screenBlend = base.CheckShaderAndCreateMaterial(this.screenBlendShader, this.screenBlend);
			this.lensFlareMaterial = base.CheckShaderAndCreateMaterial(this.lensFlareShader, this.lensFlareMaterial);
			this.blurAndFlaresMaterial = base.CheckShaderAndCreateMaterial(this.blurAndFlaresShader, this.blurAndFlaresMaterial);
			this.brightPassFilterMaterial = base.CheckShaderAndCreateMaterial(this.brightPassFilterShader, this.brightPassFilterMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x00136AC4 File Offset: 0x00134CC4
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.doHdr = false;
			if (this.hdr == Bloom.HDRBloomMode.Auto)
			{
				this.doHdr = source.format == RenderTextureFormat.ARGBHalf && base.GetComponent<Camera>().allowHDR;
			}
			else
			{
				this.doHdr = this.hdr == Bloom.HDRBloomMode.On;
			}
			this.doHdr = this.doHdr && this.supportHDRTextures;
			Bloom.BloomScreenBlendMode bloomScreenBlendMode = this.screenBlendMode;
			if (this.doHdr)
			{
				bloomScreenBlendMode = Bloom.BloomScreenBlendMode.Add;
			}
			RenderTextureFormat renderTextureFormat = (this.doHdr ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.Default);
			int num = source.width / 2;
			int num2 = source.height / 2;
			int num3 = source.width / 4;
			int num4 = source.height / 4;
			float num5 = 1f * (float)source.width / (1f * (float)source.height);
			float num6 = 0.001953125f;
			RenderTexture temporary = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat);
			RenderTexture temporary2 = RenderTexture.GetTemporary(num, num2, 0, renderTextureFormat);
			if (this.quality > Bloom.BloomQuality.Cheap)
			{
				Graphics.Blit(source, temporary2, this.screenBlend, 2);
				RenderTexture temporary3 = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat);
				Graphics.Blit(temporary2, temporary3, this.screenBlend, 2);
				Graphics.Blit(temporary3, temporary, this.screenBlend, 6);
				RenderTexture.ReleaseTemporary(temporary3);
			}
			else
			{
				Graphics.Blit(source, temporary2);
				Graphics.Blit(temporary2, temporary, this.screenBlend, 6);
			}
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture renderTexture = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat);
			this.BrightFilter(this.bloomThreshold * this.bloomThresholdColor, temporary, renderTexture);
			if (this.bloomBlurIterations < 1)
			{
				this.bloomBlurIterations = 1;
			}
			else if (this.bloomBlurIterations > 10)
			{
				this.bloomBlurIterations = 10;
			}
			for (int i = 0; i < this.bloomBlurIterations; i++)
			{
				float num7 = (1f + (float)i * 0.25f) * this.sepBlurSpread;
				RenderTexture renderTexture2 = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat);
				this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0f, num7 * num6, 0f, 0f));
				Graphics.Blit(renderTexture, renderTexture2, this.blurAndFlaresMaterial, 4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = renderTexture2;
				renderTexture2 = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat);
				this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num7 / num5 * num6, 0f, 0f, 0f));
				Graphics.Blit(renderTexture, renderTexture2, this.blurAndFlaresMaterial, 4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = renderTexture2;
				if (this.quality > Bloom.BloomQuality.Cheap)
				{
					if (i == 0)
					{
						Graphics.SetRenderTarget(temporary);
						GL.Clear(false, true, Color.black);
						Graphics.Blit(renderTexture, temporary);
					}
					else
					{
						temporary.MarkRestoreExpected();
						Graphics.Blit(renderTexture, temporary, this.screenBlend, 10);
					}
				}
			}
			if (this.quality > Bloom.BloomQuality.Cheap)
			{
				Graphics.SetRenderTarget(renderTexture);
				GL.Clear(false, true, Color.black);
				Graphics.Blit(temporary, renderTexture, this.screenBlend, 6);
			}
			if (this.lensflareIntensity > Mathf.Epsilon)
			{
				RenderTexture temporary4 = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat);
				if (this.lensflareMode == Bloom.LensFlareStyle.Ghosting)
				{
					this.BrightFilter(this.lensflareThreshold, renderTexture, temporary4);
					if (this.quality > Bloom.BloomQuality.Cheap)
					{
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0f, 1.5f / (1f * (float)temporary.height), 0f, 0f));
						Graphics.SetRenderTarget(temporary);
						GL.Clear(false, true, Color.black);
						Graphics.Blit(temporary4, temporary, this.blurAndFlaresMaterial, 4);
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(1.5f / (1f * (float)temporary.width), 0f, 0f, 0f));
						Graphics.SetRenderTarget(temporary4);
						GL.Clear(false, true, Color.black);
						Graphics.Blit(temporary, temporary4, this.blurAndFlaresMaterial, 4);
					}
					this.Vignette(0.975f, temporary4, temporary4);
					this.BlendFlares(temporary4, renderTexture);
				}
				else
				{
					float num8 = 1f * Mathf.Cos(this.flareRotation);
					float num9 = 1f * Mathf.Sin(this.flareRotation);
					float num10 = this.hollyStretchWidth * 1f / num5 * num6;
					this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num8, num9, 0f, 0f));
					this.blurAndFlaresMaterial.SetVector("_Threshhold", new Vector4(this.lensflareThreshold, 1f, 0f, 0f));
					this.blurAndFlaresMaterial.SetVector("_TintColor", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.flareColorA.a * this.lensflareIntensity);
					this.blurAndFlaresMaterial.SetFloat("_Saturation", this.lensFlareSaturation);
					temporary.DiscardContents();
					Graphics.Blit(temporary4, temporary, this.blurAndFlaresMaterial, 2);
					temporary4.DiscardContents();
					Graphics.Blit(temporary, temporary4, this.blurAndFlaresMaterial, 3);
					this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num8 * num10, num9 * num10, 0f, 0f));
					this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth);
					temporary.DiscardContents();
					Graphics.Blit(temporary4, temporary, this.blurAndFlaresMaterial, 1);
					this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 2f);
					temporary4.DiscardContents();
					Graphics.Blit(temporary, temporary4, this.blurAndFlaresMaterial, 1);
					this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 4f);
					temporary.DiscardContents();
					Graphics.Blit(temporary4, temporary, this.blurAndFlaresMaterial, 1);
					for (int j = 0; j < this.hollywoodFlareBlurIterations; j++)
					{
						num10 = this.hollyStretchWidth * 2f / num5 * num6;
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num10 * num8, num10 * num9, 0f, 0f));
						temporary4.DiscardContents();
						Graphics.Blit(temporary, temporary4, this.blurAndFlaresMaterial, 4);
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num10 * num8, num10 * num9, 0f, 0f));
						temporary.DiscardContents();
						Graphics.Blit(temporary4, temporary, this.blurAndFlaresMaterial, 4);
					}
					if (this.lensflareMode == Bloom.LensFlareStyle.Anamorphic)
					{
						this.AddTo(1f, temporary, renderTexture);
					}
					else
					{
						this.Vignette(1f, temporary, temporary4);
						this.BlendFlares(temporary4, temporary);
						this.AddTo(1f, temporary, renderTexture);
					}
				}
				RenderTexture.ReleaseTemporary(temporary4);
			}
			int num11 = (int)bloomScreenBlendMode;
			this.screenBlend.SetFloat("_Intensity", this.bloomIntensity);
			this.screenBlend.SetTexture("_ColorBuffer", source);
			if (this.quality > Bloom.BloomQuality.Cheap)
			{
				RenderTexture temporary5 = RenderTexture.GetTemporary(num, num2, 0, renderTextureFormat);
				Graphics.Blit(renderTexture, temporary5);
				Graphics.Blit(temporary5, destination, this.screenBlend, num11);
				RenderTexture.ReleaseTemporary(temporary5);
			}
			else
			{
				Graphics.Blit(renderTexture, destination, this.screenBlend, num11);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x0004948B File Offset: 0x0004768B
		private void AddTo(float intensity_, RenderTexture from, RenderTexture to)
		{
			this.screenBlend.SetFloat("_Intensity", intensity_);
			to.MarkRestoreExpected();
			Graphics.Blit(from, to, this.screenBlend, 9);
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x00137228 File Offset: 0x00135428
		private void BlendFlares(RenderTexture from, RenderTexture to)
		{
			this.lensFlareMaterial.SetVector("colorA", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorB", new Vector4(this.flareColorB.r, this.flareColorB.g, this.flareColorB.b, this.flareColorB.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorC", new Vector4(this.flareColorC.r, this.flareColorC.g, this.flareColorC.b, this.flareColorC.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorD", new Vector4(this.flareColorD.r, this.flareColorD.g, this.flareColorD.b, this.flareColorD.a) * this.lensflareIntensity);
			to.MarkRestoreExpected();
			Graphics.Blit(from, to, this.lensFlareMaterial);
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x000494B3 File Offset: 0x000476B3
		private void BrightFilter(float thresh, RenderTexture from, RenderTexture to)
		{
			this.brightPassFilterMaterial.SetVector("_Threshhold", new Vector4(thresh, thresh, thresh, thresh));
			Graphics.Blit(from, to, this.brightPassFilterMaterial, 0);
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x000494DC File Offset: 0x000476DC
		private void BrightFilter(Color threshColor, RenderTexture from, RenderTexture to)
		{
			this.brightPassFilterMaterial.SetVector("_Threshhold", threshColor);
			Graphics.Blit(from, to, this.brightPassFilterMaterial, 1);
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x00137378 File Offset: 0x00135578
		private void Vignette(float amount, RenderTexture from, RenderTexture to)
		{
			if (this.lensFlareVignetteMask)
			{
				this.screenBlend.SetTexture("_ColorBuffer", this.lensFlareVignetteMask);
				to.MarkRestoreExpected();
				Graphics.Blit((from == to) ? null : from, to, this.screenBlend, (from == to) ? 7 : 3);
				return;
			}
			if (from != to)
			{
				Graphics.SetRenderTarget(to);
				GL.Clear(false, true, Color.black);
				Graphics.Blit(from, to);
			}
		}

		// Token: 0x04002513 RID: 9491
		public Bloom.TweakMode tweakMode;

		// Token: 0x04002514 RID: 9492
		public Bloom.BloomScreenBlendMode screenBlendMode = Bloom.BloomScreenBlendMode.Add;

		// Token: 0x04002515 RID: 9493
		public Bloom.HDRBloomMode hdr;

		// Token: 0x04002516 RID: 9494
		private bool doHdr;

		// Token: 0x04002517 RID: 9495
		public float sepBlurSpread = 2.5f;

		// Token: 0x04002518 RID: 9496
		public Bloom.BloomQuality quality = Bloom.BloomQuality.High;

		// Token: 0x04002519 RID: 9497
		public float bloomIntensity = 0.5f;

		// Token: 0x0400251A RID: 9498
		public float bloomThreshold = 0.5f;

		// Token: 0x0400251B RID: 9499
		public Color bloomThresholdColor = Color.white;

		// Token: 0x0400251C RID: 9500
		public int bloomBlurIterations = 2;

		// Token: 0x0400251D RID: 9501
		public int hollywoodFlareBlurIterations = 2;

		// Token: 0x0400251E RID: 9502
		public float flareRotation;

		// Token: 0x0400251F RID: 9503
		public Bloom.LensFlareStyle lensflareMode = Bloom.LensFlareStyle.Anamorphic;

		// Token: 0x04002520 RID: 9504
		public float hollyStretchWidth = 2.5f;

		// Token: 0x04002521 RID: 9505
		public float lensflareIntensity;

		// Token: 0x04002522 RID: 9506
		public float lensflareThreshold = 0.3f;

		// Token: 0x04002523 RID: 9507
		public float lensFlareSaturation = 0.75f;

		// Token: 0x04002524 RID: 9508
		public Color flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);

		// Token: 0x04002525 RID: 9509
		public Color flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);

		// Token: 0x04002526 RID: 9510
		public Color flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);

		// Token: 0x04002527 RID: 9511
		public Color flareColorD = new Color(0.8f, 0.4f, 0f, 0.75f);

		// Token: 0x04002528 RID: 9512
		public Texture2D lensFlareVignetteMask;

		// Token: 0x04002529 RID: 9513
		public Shader lensFlareShader;

		// Token: 0x0400252A RID: 9514
		private Material lensFlareMaterial;

		// Token: 0x0400252B RID: 9515
		public Shader screenBlendShader;

		// Token: 0x0400252C RID: 9516
		private Material screenBlend;

		// Token: 0x0400252D RID: 9517
		public Shader blurAndFlaresShader;

		// Token: 0x0400252E RID: 9518
		private Material blurAndFlaresMaterial;

		// Token: 0x0400252F RID: 9519
		public Shader brightPassFilterShader;

		// Token: 0x04002530 RID: 9520
		private Material brightPassFilterMaterial;

		// Token: 0x020006A1 RID: 1697
		public enum LensFlareStyle
		{
			// Token: 0x04002532 RID: 9522
			Ghosting,
			// Token: 0x04002533 RID: 9523
			Anamorphic,
			// Token: 0x04002534 RID: 9524
			Combined
		}

		// Token: 0x020006A2 RID: 1698
		public enum TweakMode
		{
			// Token: 0x04002536 RID: 9526
			Basic,
			// Token: 0x04002537 RID: 9527
			Complex
		}

		// Token: 0x020006A3 RID: 1699
		public enum HDRBloomMode
		{
			// Token: 0x04002539 RID: 9529
			Auto,
			// Token: 0x0400253A RID: 9530
			On,
			// Token: 0x0400253B RID: 9531
			Off
		}

		// Token: 0x020006A4 RID: 1700
		public enum BloomScreenBlendMode
		{
			// Token: 0x0400253D RID: 9533
			Screen,
			// Token: 0x0400253E RID: 9534
			Add
		}

		// Token: 0x020006A5 RID: 1701
		public enum BloomQuality
		{
			// Token: 0x04002540 RID: 9536
			Cheap,
			// Token: 0x04002541 RID: 9537
			High
		}
	}
}

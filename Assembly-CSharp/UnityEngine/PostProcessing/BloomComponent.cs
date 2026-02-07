using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000716 RID: 1814
	public sealed class BloomComponent : PostProcessingComponentRenderTexture<BloomModel>
	{
		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06003678 RID: 13944 RVA: 0x0004AF48 File Offset: 0x00049148
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.bloom.intensity > 0f && !this.context.interrupted;
			}
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x00141988 File Offset: 0x0013FB88
		public void Prepare(RenderTexture source, Material uberMaterial, Texture autoExposure)
		{
			BloomModel.BloomSettings bloom = base.model.settings.bloom;
			BloomModel.LensDirtSettings lensDirt = base.model.settings.lensDirt;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Bloom");
			material.shaderKeywords = null;
			material.SetTexture(BloomComponent.Uniforms._AutoExposure, autoExposure);
			int num = this.context.width / 2;
			int num2 = this.context.height / 2;
			RenderTextureFormat renderTextureFormat = (Application.isMobilePlatform ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);
			float num3 = Mathf.Log((float)num2, 2f) + bloom.radius - 8f;
			int num4 = (int)num3;
			int num5 = Mathf.Clamp(num4, 1, 16);
			float thresholdLinear = bloom.thresholdLinear;
			material.SetFloat(BloomComponent.Uniforms._Threshold, thresholdLinear);
			float num6 = thresholdLinear * bloom.softKnee + 1E-05f;
			Vector3 vector = new Vector3(thresholdLinear - num6, num6 * 2f, 0.25f / num6);
			material.SetVector(BloomComponent.Uniforms._Curve, vector);
			material.SetFloat(BloomComponent.Uniforms._PrefilterOffs, bloom.antiFlicker ? (-0.5f) : 0f);
			float num7 = 0.5f + num3 - (float)num4;
			material.SetFloat(BloomComponent.Uniforms._SampleScale, num7);
			if (bloom.antiFlicker)
			{
				material.EnableKeyword("ANTI_FLICKER");
			}
			RenderTexture renderTexture = this.context.renderTextureFactory.Get(num, num2, 0, renderTextureFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
			Graphics.Blit(source, renderTexture, material, 0);
			RenderTexture renderTexture2 = renderTexture;
			for (int i = 0; i < num5; i++)
			{
				this.m_BlurBuffer1[i] = this.context.renderTextureFactory.Get(renderTexture2.width / 2, renderTexture2.height / 2, 0, renderTextureFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
				int num8 = ((i == 0) ? 1 : 2);
				Graphics.Blit(renderTexture2, this.m_BlurBuffer1[i], material, num8);
				renderTexture2 = this.m_BlurBuffer1[i];
			}
			for (int j = num5 - 2; j >= 0; j--)
			{
				RenderTexture renderTexture3 = this.m_BlurBuffer1[j];
				material.SetTexture(BloomComponent.Uniforms._BaseTex, renderTexture3);
				this.m_BlurBuffer2[j] = this.context.renderTextureFactory.Get(renderTexture3.width, renderTexture3.height, 0, renderTextureFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
				Graphics.Blit(renderTexture2, this.m_BlurBuffer2[j], material, 3);
				renderTexture2 = this.m_BlurBuffer2[j];
			}
			RenderTexture renderTexture4 = renderTexture2;
			for (int k = 0; k < 16; k++)
			{
				if (this.m_BlurBuffer1[k] != null)
				{
					this.context.renderTextureFactory.Release(this.m_BlurBuffer1[k]);
				}
				if (this.m_BlurBuffer2[k] != null && this.m_BlurBuffer2[k] != renderTexture4)
				{
					this.context.renderTextureFactory.Release(this.m_BlurBuffer2[k]);
				}
				this.m_BlurBuffer1[k] = null;
				this.m_BlurBuffer2[k] = null;
			}
			this.context.renderTextureFactory.Release(renderTexture);
			uberMaterial.SetTexture(BloomComponent.Uniforms._BloomTex, renderTexture4);
			uberMaterial.SetVector(BloomComponent.Uniforms._Bloom_Settings, new Vector2(num7, bloom.intensity));
			if (lensDirt.intensity > 0f && lensDirt.texture != null)
			{
				uberMaterial.SetTexture(BloomComponent.Uniforms._Bloom_DirtTex, lensDirt.texture);
				uberMaterial.SetFloat(BloomComponent.Uniforms._Bloom_DirtIntensity, lensDirt.intensity);
				uberMaterial.EnableKeyword("BLOOM_LENS_DIRT");
				return;
			}
			uberMaterial.EnableKeyword("BLOOM");
		}

		// Token: 0x040027E4 RID: 10212
		private const int k_MaxPyramidBlurLevel = 16;

		// Token: 0x040027E5 RID: 10213
		private readonly RenderTexture[] m_BlurBuffer1 = new RenderTexture[16];

		// Token: 0x040027E6 RID: 10214
		private readonly RenderTexture[] m_BlurBuffer2 = new RenderTexture[16];

		// Token: 0x02000717 RID: 1815
		private static class Uniforms
		{
			// Token: 0x040027E7 RID: 10215
			internal static readonly int _AutoExposure = Shader.PropertyToID("_AutoExposure");

			// Token: 0x040027E8 RID: 10216
			internal static readonly int _Threshold = Shader.PropertyToID("_Threshold");

			// Token: 0x040027E9 RID: 10217
			internal static readonly int _Curve = Shader.PropertyToID("_Curve");

			// Token: 0x040027EA RID: 10218
			internal static readonly int _PrefilterOffs = Shader.PropertyToID("_PrefilterOffs");

			// Token: 0x040027EB RID: 10219
			internal static readonly int _SampleScale = Shader.PropertyToID("_SampleScale");

			// Token: 0x040027EC RID: 10220
			internal static readonly int _BaseTex = Shader.PropertyToID("_BaseTex");

			// Token: 0x040027ED RID: 10221
			internal static readonly int _BloomTex = Shader.PropertyToID("_BloomTex");

			// Token: 0x040027EE RID: 10222
			internal static readonly int _Bloom_Settings = Shader.PropertyToID("_Bloom_Settings");

			// Token: 0x040027EF RID: 10223
			internal static readonly int _Bloom_DirtTex = Shader.PropertyToID("_Bloom_DirtTex");

			// Token: 0x040027F0 RID: 10224
			internal static readonly int _Bloom_DirtIntensity = Shader.PropertyToID("_Bloom_DirtIntensity");
		}
	}
}

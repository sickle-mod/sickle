using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000720 RID: 1824
	public sealed class DepthOfFieldComponent : PostProcessingComponentRenderTexture<DepthOfFieldModel>
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060036AE RID: 13998 RVA: 0x0004B208 File Offset: 0x00049408
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x000283F8 File Offset: 0x000265F8
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x0014327C File Offset: 0x0014147C
		private float CalculateFocalLength()
		{
			DepthOfFieldModel.Settings settings = base.model.settings;
			if (!settings.useCameraFov)
			{
				return settings.focalLength / 1000f;
			}
			float num = this.context.camera.fieldOfView * 0.017453292f;
			return 0.012f / Mathf.Tan(0.5f * num);
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x001432D4 File Offset: 0x001414D4
		private float CalculateMaxCoCRadius(int screenHeight)
		{
			float num = (float)base.model.settings.kernelSize * 4f + 6f;
			return Mathf.Min(0.05f, num / (float)screenHeight);
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x0004B227 File Offset: 0x00049427
		private bool CheckHistory(int width, int height)
		{
			return this.m_CoCHistory != null && this.m_CoCHistory.IsCreated() && this.m_CoCHistory.width == width && this.m_CoCHistory.height == height;
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x0004B262 File Offset: 0x00049462
		private RenderTextureFormat SelectFormat(RenderTextureFormat primary, RenderTextureFormat secondary)
		{
			if (SystemInfo.SupportsRenderTextureFormat(primary))
			{
				return primary;
			}
			if (SystemInfo.SupportsRenderTextureFormat(secondary))
			{
				return secondary;
			}
			return RenderTextureFormat.Default;
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x00143310 File Offset: 0x00141510
		public void Prepare(RenderTexture source, Material uberMaterial, bool antialiasCoC, Vector2 taaJitter, float taaBlending)
		{
			DepthOfFieldModel.Settings settings = base.model.settings;
			RenderTextureFormat renderTextureFormat = RenderTextureFormat.DefaultHDR;
			RenderTextureFormat renderTextureFormat2 = this.SelectFormat(RenderTextureFormat.R8, RenderTextureFormat.RHalf);
			float num = this.CalculateFocalLength();
			float num2 = Mathf.Max(settings.focusDistance, num);
			float num3 = (float)source.width / (float)source.height;
			float num4 = num * num / (settings.aperture * (num2 - num) * 0.024f * 2f);
			float num5 = this.CalculateMaxCoCRadius(source.height);
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Depth Of Field");
			material.SetFloat(DepthOfFieldComponent.Uniforms._Distance, num2);
			material.SetFloat(DepthOfFieldComponent.Uniforms._LensCoeff, num4);
			material.SetFloat(DepthOfFieldComponent.Uniforms._MaxCoC, num5);
			material.SetFloat(DepthOfFieldComponent.Uniforms._RcpMaxCoC, 1f / num5);
			material.SetFloat(DepthOfFieldComponent.Uniforms._RcpAspect, 1f / num3);
			RenderTexture renderTexture = this.context.renderTextureFactory.Get(this.context.width, this.context.height, 0, renderTextureFormat2, RenderTextureReadWrite.Linear, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
			Graphics.Blit(null, renderTexture, material, 0);
			if (antialiasCoC)
			{
				material.SetTexture(DepthOfFieldComponent.Uniforms._CoCTex, renderTexture);
				float num6 = (this.CheckHistory(this.context.width, this.context.height) ? taaBlending : 0f);
				material.SetVector(DepthOfFieldComponent.Uniforms._TaaParams, new Vector3(taaJitter.x, taaJitter.y, num6));
				RenderTexture temporary = RenderTexture.GetTemporary(this.context.width, this.context.height, 0, renderTextureFormat2);
				Graphics.Blit(this.m_CoCHistory, temporary, material, 1);
				this.context.renderTextureFactory.Release(renderTexture);
				if (this.m_CoCHistory != null)
				{
					RenderTexture.ReleaseTemporary(this.m_CoCHistory);
				}
				renderTexture = (this.m_CoCHistory = temporary);
			}
			RenderTexture renderTexture2 = this.context.renderTextureFactory.Get(this.context.width / 2, this.context.height / 2, 0, renderTextureFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
			material.SetTexture(DepthOfFieldComponent.Uniforms._CoCTex, renderTexture);
			Graphics.Blit(source, renderTexture2, material, 2);
			RenderTexture renderTexture3 = this.context.renderTextureFactory.Get(this.context.width / 2, this.context.height / 2, 0, renderTextureFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
			Graphics.Blit(renderTexture2, renderTexture3, material, (int)(3 + settings.kernelSize));
			Graphics.Blit(renderTexture3, renderTexture2, material, 7);
			uberMaterial.SetVector(DepthOfFieldComponent.Uniforms._DepthOfFieldParams, new Vector3(num2, num4, num5));
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.FocusPlane))
			{
				uberMaterial.EnableKeyword("DEPTH_OF_FIELD_COC_VIEW");
				this.context.Interrupt();
			}
			else
			{
				uberMaterial.SetTexture(DepthOfFieldComponent.Uniforms._DepthOfFieldTex, renderTexture2);
				uberMaterial.SetTexture(DepthOfFieldComponent.Uniforms._DepthOfFieldCoCTex, renderTexture);
				uberMaterial.EnableKeyword("DEPTH_OF_FIELD");
			}
			this.context.renderTextureFactory.Release(renderTexture3);
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x0004B279 File Offset: 0x00049479
		public override void OnDisable()
		{
			if (this.m_CoCHistory != null)
			{
				RenderTexture.ReleaseTemporary(this.m_CoCHistory);
			}
			this.m_CoCHistory = null;
		}

		// Token: 0x0400281F RID: 10271
		private const string k_ShaderString = "Hidden/Post FX/Depth Of Field";

		// Token: 0x04002820 RID: 10272
		private RenderTexture m_CoCHistory;

		// Token: 0x04002821 RID: 10273
		private const float k_FilmHeight = 0.024f;

		// Token: 0x02000721 RID: 1825
		private static class Uniforms
		{
			// Token: 0x04002822 RID: 10274
			internal static readonly int _DepthOfFieldTex = Shader.PropertyToID("_DepthOfFieldTex");

			// Token: 0x04002823 RID: 10275
			internal static readonly int _DepthOfFieldCoCTex = Shader.PropertyToID("_DepthOfFieldCoCTex");

			// Token: 0x04002824 RID: 10276
			internal static readonly int _Distance = Shader.PropertyToID("_Distance");

			// Token: 0x04002825 RID: 10277
			internal static readonly int _LensCoeff = Shader.PropertyToID("_LensCoeff");

			// Token: 0x04002826 RID: 10278
			internal static readonly int _MaxCoC = Shader.PropertyToID("_MaxCoC");

			// Token: 0x04002827 RID: 10279
			internal static readonly int _RcpMaxCoC = Shader.PropertyToID("_RcpMaxCoC");

			// Token: 0x04002828 RID: 10280
			internal static readonly int _RcpAspect = Shader.PropertyToID("_RcpAspect");

			// Token: 0x04002829 RID: 10281
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x0400282A RID: 10282
			internal static readonly int _CoCTex = Shader.PropertyToID("_CoCTex");

			// Token: 0x0400282B RID: 10283
			internal static readonly int _TaaParams = Shader.PropertyToID("_TaaParams");

			// Token: 0x0400282C RID: 10284
			internal static readonly int _DepthOfFieldParams = Shader.PropertyToID("_DepthOfFieldParams");
		}
	}
}

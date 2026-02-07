using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000726 RID: 1830
	public sealed class FogComponent : PostProcessingComponentCommandBuffer<FogModel>
	{
		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060036C7 RID: 14023 RVA: 0x0004B33D File Offset: 0x0004953D
		public override bool active
		{
			get
			{
				return base.model.enabled && this.context.isGBufferAvailable && RenderSettings.fog && !this.context.interrupted;
			}
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x0004B370 File Offset: 0x00049570
		public override string GetName()
		{
			return "Fog";
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x000283F8 File Offset: 0x000265F8
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x0004B377 File Offset: 0x00049577
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.AfterImageEffectsOpaque;
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x00143D34 File Offset: 0x00141F34
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			FogModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Fog");
			material.shaderKeywords = null;
			Color color = (GraphicsUtils.isLinearColorSpace ? RenderSettings.fogColor.linear : RenderSettings.fogColor);
			material.SetColor(FogComponent.Uniforms._FogColor, color);
			material.SetFloat(FogComponent.Uniforms._Density, RenderSettings.fogDensity);
			material.SetFloat(FogComponent.Uniforms._Start, RenderSettings.fogStartDistance);
			material.SetFloat(FogComponent.Uniforms._End, RenderSettings.fogEndDistance);
			switch (RenderSettings.fogMode)
			{
			case FogMode.Linear:
				material.EnableKeyword("FOG_LINEAR");
				break;
			case FogMode.Exponential:
				material.EnableKeyword("FOG_EXP");
				break;
			case FogMode.ExponentialSquared:
				material.EnableKeyword("FOG_EXP2");
				break;
			}
			RenderTextureFormat renderTextureFormat = (this.context.isHdr ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default);
			cb.GetTemporaryRT(FogComponent.Uniforms._TempRT, this.context.width, this.context.height, 24, FilterMode.Bilinear, renderTextureFormat);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, FogComponent.Uniforms._TempRT);
			cb.Blit(FogComponent.Uniforms._TempRT, BuiltinRenderTextureType.CameraTarget, material, settings.excludeSkybox ? 1 : 0);
			cb.ReleaseTemporaryRT(FogComponent.Uniforms._TempRT);
		}

		// Token: 0x04002843 RID: 10307
		private const string k_ShaderString = "Hidden/Post FX/Fog";

		// Token: 0x02000727 RID: 1831
		private static class Uniforms
		{
			// Token: 0x04002844 RID: 10308
			internal static readonly int _FogColor = Shader.PropertyToID("_FogColor");

			// Token: 0x04002845 RID: 10309
			internal static readonly int _Density = Shader.PropertyToID("_Density");

			// Token: 0x04002846 RID: 10310
			internal static readonly int _Start = Shader.PropertyToID("_Start");

			// Token: 0x04002847 RID: 10311
			internal static readonly int _End = Shader.PropertyToID("_End");

			// Token: 0x04002848 RID: 10312
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}
	}
}

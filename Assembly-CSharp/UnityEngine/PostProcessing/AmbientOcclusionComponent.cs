using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000713 RID: 1811
	public sealed class AmbientOcclusionComponent : PostProcessingComponentCommandBuffer<AmbientOcclusionModel>
	{
		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x0600366F RID: 13935 RVA: 0x001414E4 File Offset: 0x0013F6E4
		private AmbientOcclusionComponent.OcclusionSource occlusionSource
		{
			get
			{
				if (this.context.isGBufferAvailable && !base.model.settings.forceForwardCompatibility)
				{
					return AmbientOcclusionComponent.OcclusionSource.GBuffer;
				}
				if (base.model.settings.highPrecision && (!this.context.isGBufferAvailable || base.model.settings.forceForwardCompatibility))
				{
					return AmbientOcclusionComponent.OcclusionSource.DepthTexture;
				}
				return AmbientOcclusionComponent.OcclusionSource.DepthNormalsTexture;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06003670 RID: 13936 RVA: 0x00141548 File Offset: 0x0013F748
		private bool ambientOnlySupported
		{
			get
			{
				return this.context.isHdr && base.model.settings.ambientOnly && this.context.isGBufferAvailable && !base.model.settings.forceForwardCompatibility;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06003671 RID: 13937 RVA: 0x0004AEB5 File Offset: 0x000490B5
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && !this.context.interrupted;
			}
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x00141598 File Offset: 0x0013F798
		public override DepthTextureMode GetCameraFlags()
		{
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			if (this.occlusionSource == AmbientOcclusionComponent.OcclusionSource.DepthTexture)
			{
				depthTextureMode |= DepthTextureMode.Depth;
			}
			if (this.occlusionSource != AmbientOcclusionComponent.OcclusionSource.GBuffer)
			{
				depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			return depthTextureMode;
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x0004AEEB File Offset: 0x000490EB
		public override string GetName()
		{
			return "Ambient Occlusion";
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x0004AEF2 File Offset: 0x000490F2
		public override CameraEvent GetCameraEvent()
		{
			if (!this.ambientOnlySupported || this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion))
			{
				return CameraEvent.BeforeImageEffectsOpaque;
			}
			return CameraEvent.BeforeReflections;
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x001415C4 File Offset: 0x0013F7C4
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			AmbientOcclusionModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Blit");
			Material material2 = this.context.materialFactory.Get("Hidden/Post FX/Ambient Occlusion");
			material2.shaderKeywords = null;
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Intensity, settings.intensity);
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Radius, settings.radius);
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Downsample, settings.downsampling ? 0.5f : 1f);
			material2.SetInt(AmbientOcclusionComponent.Uniforms._SampleCount, (int)settings.sampleCount);
			if (!this.context.isGBufferAvailable && RenderSettings.fog)
			{
				material2.SetVector(AmbientOcclusionComponent.Uniforms._FogParams, new Vector3(RenderSettings.fogDensity, RenderSettings.fogStartDistance, RenderSettings.fogEndDistance));
				switch (RenderSettings.fogMode)
				{
				case FogMode.Linear:
					material2.EnableKeyword("FOG_LINEAR");
					break;
				case FogMode.Exponential:
					material2.EnableKeyword("FOG_EXP");
					break;
				case FogMode.ExponentialSquared:
					material2.EnableKeyword("FOG_EXP2");
					break;
				}
			}
			else
			{
				material2.EnableKeyword("FOG_OFF");
			}
			int width = this.context.width;
			int height = this.context.height;
			int num = (settings.downsampling ? 2 : 1);
			int num2 = AmbientOcclusionComponent.Uniforms._OcclusionTexture1;
			cb.GetTemporaryRT(num2, width / num, height / num, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.Blit(null, num2, material2, (int)this.occlusionSource);
			int occlusionTexture = AmbientOcclusionComponent.Uniforms._OcclusionTexture2;
			cb.GetTemporaryRT(occlusionTexture, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, num2);
			cb.Blit(num2, occlusionTexture, material2, (this.occlusionSource == AmbientOcclusionComponent.OcclusionSource.GBuffer) ? 4 : 3);
			cb.ReleaseTemporaryRT(num2);
			num2 = AmbientOcclusionComponent.Uniforms._OcclusionTexture;
			cb.GetTemporaryRT(num2, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, occlusionTexture);
			cb.Blit(occlusionTexture, num2, material2, 5);
			cb.ReleaseTemporaryRT(occlusionTexture);
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion))
			{
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, num2);
				cb.Blit(num2, BuiltinRenderTextureType.CameraTarget, material2, 8);
				this.context.Interrupt();
			}
			else if (this.ambientOnlySupported)
			{
				cb.SetRenderTarget(this.m_MRT, BuiltinRenderTextureType.CameraTarget);
				cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material2, 0, 7);
			}
			else
			{
				RenderTextureFormat renderTextureFormat = (this.context.isHdr ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default);
				int tempRT = AmbientOcclusionComponent.Uniforms._TempRT;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, FilterMode.Bilinear, renderTextureFormat);
				cb.Blit(BuiltinRenderTextureType.CameraTarget, tempRT, material, 0);
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, tempRT);
				cb.Blit(tempRT, BuiltinRenderTextureType.CameraTarget, material2, 6);
				cb.ReleaseTemporaryRT(tempRT);
			}
			cb.ReleaseTemporaryRT(num2);
		}

		// Token: 0x040027D3 RID: 10195
		private const string k_BlitShaderString = "Hidden/Post FX/Blit";

		// Token: 0x040027D4 RID: 10196
		private const string k_ShaderString = "Hidden/Post FX/Ambient Occlusion";

		// Token: 0x040027D5 RID: 10197
		private readonly RenderTargetIdentifier[] m_MRT = new RenderTargetIdentifier[]
		{
			BuiltinRenderTextureType.GBuffer0,
			BuiltinRenderTextureType.CameraTarget
		};

		// Token: 0x02000714 RID: 1812
		private static class Uniforms
		{
			// Token: 0x040027D6 RID: 10198
			internal static readonly int _Intensity = Shader.PropertyToID("_Intensity");

			// Token: 0x040027D7 RID: 10199
			internal static readonly int _Radius = Shader.PropertyToID("_Radius");

			// Token: 0x040027D8 RID: 10200
			internal static readonly int _FogParams = Shader.PropertyToID("_FogParams");

			// Token: 0x040027D9 RID: 10201
			internal static readonly int _Downsample = Shader.PropertyToID("_Downsample");

			// Token: 0x040027DA RID: 10202
			internal static readonly int _SampleCount = Shader.PropertyToID("_SampleCount");

			// Token: 0x040027DB RID: 10203
			internal static readonly int _OcclusionTexture1 = Shader.PropertyToID("_OcclusionTexture1");

			// Token: 0x040027DC RID: 10204
			internal static readonly int _OcclusionTexture2 = Shader.PropertyToID("_OcclusionTexture2");

			// Token: 0x040027DD RID: 10205
			internal static readonly int _OcclusionTexture = Shader.PropertyToID("_OcclusionTexture");

			// Token: 0x040027DE RID: 10206
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x040027DF RID: 10207
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}

		// Token: 0x02000715 RID: 1813
		private enum OcclusionSource
		{
			// Token: 0x040027E1 RID: 10209
			DepthTexture,
			// Token: 0x040027E2 RID: 10210
			DepthNormalsTexture,
			// Token: 0x040027E3 RID: 10211
			GBuffer
		}
	}
}

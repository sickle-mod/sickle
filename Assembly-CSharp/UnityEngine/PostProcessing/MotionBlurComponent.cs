using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200072C RID: 1836
	public sealed class MotionBlurComponent : PostProcessingComponentCommandBuffer<MotionBlurModel>
	{
		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060036D7 RID: 14039 RVA: 0x0004B474 File Offset: 0x00049674
		public MotionBlurComponent.ReconstructionFilter reconstructionFilter
		{
			get
			{
				if (this.m_ReconstructionFilter == null)
				{
					this.m_ReconstructionFilter = new MotionBlurComponent.ReconstructionFilter();
				}
				return this.m_ReconstructionFilter;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060036D8 RID: 14040 RVA: 0x0004B48F File Offset: 0x0004968F
		public MotionBlurComponent.FrameBlendingFilter frameBlendingFilter
		{
			get
			{
				if (this.m_FrameBlendingFilter == null)
				{
					this.m_FrameBlendingFilter = new MotionBlurComponent.FrameBlendingFilter();
				}
				return this.m_FrameBlendingFilter;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060036D9 RID: 14041 RVA: 0x001440FC File Offset: 0x001422FC
		public override bool active
		{
			get
			{
				MotionBlurModel.Settings settings = base.model.settings;
				return base.model.enabled && ((settings.shutterAngle > 0f && this.reconstructionFilter.IsSupported()) || settings.frameBlending > 0f) && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2 && !this.context.interrupted;
			}
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x0004B4AA File Offset: 0x000496AA
		public override string GetName()
		{
			return "Motion Blur";
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x0004B4B1 File Offset: 0x000496B1
		public void ResetHistory()
		{
			if (this.m_FrameBlendingFilter != null)
			{
				this.m_FrameBlendingFilter.Dispose();
			}
			this.m_FrameBlendingFilter = null;
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x0002FC16 File Offset: 0x0002DE16
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x0004B4CD File Offset: 0x000496CD
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.BeforeImageEffects;
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x0004B4D1 File Offset: 0x000496D1
		public override void OnEnable()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x00144164 File Offset: 0x00142364
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			if (this.m_FirstFrame)
			{
				this.m_FirstFrame = false;
				return;
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Motion Blur");
			Material material2 = this.context.materialFactory.Get("Hidden/Post FX/Blit");
			MotionBlurModel.Settings settings = base.model.settings;
			RenderTextureFormat renderTextureFormat = (this.context.isHdr ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default);
			int tempRT = MotionBlurComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, FilterMode.Point, renderTextureFormat);
			if (settings.shutterAngle > 0f && settings.frameBlending > 0f)
			{
				this.reconstructionFilter.ProcessImage(this.context, cb, ref settings, BuiltinRenderTextureType.CameraTarget, tempRT, material);
				this.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, tempRT, BuiltinRenderTextureType.CameraTarget, material);
				this.frameBlendingFilter.PushFrame(cb, tempRT, this.context.width, this.context.height, material);
			}
			else if (settings.shutterAngle > 0f)
			{
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget);
				cb.Blit(BuiltinRenderTextureType.CameraTarget, tempRT, material2, 0);
				this.reconstructionFilter.ProcessImage(this.context, cb, ref settings, tempRT, BuiltinRenderTextureType.CameraTarget, material);
			}
			else if (settings.frameBlending > 0f)
			{
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget);
				cb.Blit(BuiltinRenderTextureType.CameraTarget, tempRT, material2, 0);
				this.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, tempRT, BuiltinRenderTextureType.CameraTarget, material);
				this.frameBlendingFilter.PushFrame(cb, tempRT, this.context.width, this.context.height, material);
			}
			cb.ReleaseTemporaryRT(tempRT);
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x0004B4DA File Offset: 0x000496DA
		public override void OnDisable()
		{
			if (this.m_FrameBlendingFilter != null)
			{
				this.m_FrameBlendingFilter.Dispose();
			}
		}

		// Token: 0x04002850 RID: 10320
		private MotionBlurComponent.ReconstructionFilter m_ReconstructionFilter;

		// Token: 0x04002851 RID: 10321
		private MotionBlurComponent.FrameBlendingFilter m_FrameBlendingFilter;

		// Token: 0x04002852 RID: 10322
		private bool m_FirstFrame = true;

		// Token: 0x0200072D RID: 1837
		private static class Uniforms
		{
			// Token: 0x04002853 RID: 10323
			internal static readonly int _VelocityScale = Shader.PropertyToID("_VelocityScale");

			// Token: 0x04002854 RID: 10324
			internal static readonly int _MaxBlurRadius = Shader.PropertyToID("_MaxBlurRadius");

			// Token: 0x04002855 RID: 10325
			internal static readonly int _RcpMaxBlurRadius = Shader.PropertyToID("_RcpMaxBlurRadius");

			// Token: 0x04002856 RID: 10326
			internal static readonly int _VelocityTex = Shader.PropertyToID("_VelocityTex");

			// Token: 0x04002857 RID: 10327
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04002858 RID: 10328
			internal static readonly int _Tile2RT = Shader.PropertyToID("_Tile2RT");

			// Token: 0x04002859 RID: 10329
			internal static readonly int _Tile4RT = Shader.PropertyToID("_Tile4RT");

			// Token: 0x0400285A RID: 10330
			internal static readonly int _Tile8RT = Shader.PropertyToID("_Tile8RT");

			// Token: 0x0400285B RID: 10331
			internal static readonly int _TileMaxOffs = Shader.PropertyToID("_TileMaxOffs");

			// Token: 0x0400285C RID: 10332
			internal static readonly int _TileMaxLoop = Shader.PropertyToID("_TileMaxLoop");

			// Token: 0x0400285D RID: 10333
			internal static readonly int _TileVRT = Shader.PropertyToID("_TileVRT");

			// Token: 0x0400285E RID: 10334
			internal static readonly int _NeighborMaxTex = Shader.PropertyToID("_NeighborMaxTex");

			// Token: 0x0400285F RID: 10335
			internal static readonly int _LoopCount = Shader.PropertyToID("_LoopCount");

			// Token: 0x04002860 RID: 10336
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			// Token: 0x04002861 RID: 10337
			internal static readonly int _History1LumaTex = Shader.PropertyToID("_History1LumaTex");

			// Token: 0x04002862 RID: 10338
			internal static readonly int _History2LumaTex = Shader.PropertyToID("_History2LumaTex");

			// Token: 0x04002863 RID: 10339
			internal static readonly int _History3LumaTex = Shader.PropertyToID("_History3LumaTex");

			// Token: 0x04002864 RID: 10340
			internal static readonly int _History4LumaTex = Shader.PropertyToID("_History4LumaTex");

			// Token: 0x04002865 RID: 10341
			internal static readonly int _History1ChromaTex = Shader.PropertyToID("_History1ChromaTex");

			// Token: 0x04002866 RID: 10342
			internal static readonly int _History2ChromaTex = Shader.PropertyToID("_History2ChromaTex");

			// Token: 0x04002867 RID: 10343
			internal static readonly int _History3ChromaTex = Shader.PropertyToID("_History3ChromaTex");

			// Token: 0x04002868 RID: 10344
			internal static readonly int _History4ChromaTex = Shader.PropertyToID("_History4ChromaTex");

			// Token: 0x04002869 RID: 10345
			internal static readonly int _History1Weight = Shader.PropertyToID("_History1Weight");

			// Token: 0x0400286A RID: 10346
			internal static readonly int _History2Weight = Shader.PropertyToID("_History2Weight");

			// Token: 0x0400286B RID: 10347
			internal static readonly int _History3Weight = Shader.PropertyToID("_History3Weight");

			// Token: 0x0400286C RID: 10348
			internal static readonly int _History4Weight = Shader.PropertyToID("_History4Weight");
		}

		// Token: 0x0200072E RID: 1838
		private enum Pass
		{
			// Token: 0x0400286E RID: 10350
			VelocitySetup,
			// Token: 0x0400286F RID: 10351
			TileMax1,
			// Token: 0x04002870 RID: 10352
			TileMax2,
			// Token: 0x04002871 RID: 10353
			TileMaxV,
			// Token: 0x04002872 RID: 10354
			NeighborMax,
			// Token: 0x04002873 RID: 10355
			Reconstruction,
			// Token: 0x04002874 RID: 10356
			FrameCompression,
			// Token: 0x04002875 RID: 10357
			FrameBlendingChroma,
			// Token: 0x04002876 RID: 10358
			FrameBlendingRaw
		}

		// Token: 0x0200072F RID: 1839
		public class ReconstructionFilter
		{
			// Token: 0x060036E3 RID: 14051 RVA: 0x0004B4FE File Offset: 0x000496FE
			public ReconstructionFilter()
			{
				this.CheckTextureFormatSupport();
			}

			// Token: 0x060036E4 RID: 14052 RVA: 0x0004B51B File Offset: 0x0004971B
			private void CheckTextureFormatSupport()
			{
				if (!SystemInfo.SupportsRenderTextureFormat(this.m_PackedRTFormat))
				{
					this.m_PackedRTFormat = RenderTextureFormat.ARGB32;
				}
			}

			// Token: 0x060036E5 RID: 14053 RVA: 0x0004B531 File Offset: 0x00049731
			public bool IsSupported()
			{
				return SystemInfo.supportsMotionVectors;
			}

			// Token: 0x060036E6 RID: 14054 RVA: 0x001444F0 File Offset: 0x001426F0
			public void ProcessImage(PostProcessingContext context, CommandBuffer cb, ref MotionBlurModel.Settings settings, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material)
			{
				int num = (int)(5f * (float)context.height / 100f);
				int num2 = ((num - 1) / 8 + 1) * 8;
				float num3 = settings.shutterAngle / 360f;
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._VelocityScale, num3);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._MaxBlurRadius, (float)num);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._RcpMaxBlurRadius, 1f / (float)num);
				int velocityTex = MotionBlurComponent.Uniforms._VelocityTex;
				cb.GetTemporaryRT(velocityTex, context.width, context.height, 0, FilterMode.Point, this.m_PackedRTFormat, RenderTextureReadWrite.Linear);
				cb.Blit(null, velocityTex, material, 0);
				int tile2RT = MotionBlurComponent.Uniforms._Tile2RT;
				cb.GetTemporaryRT(tile2RT, context.width / 2, context.height / 2, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, velocityTex);
				cb.Blit(velocityTex, tile2RT, material, 1);
				int tile4RT = MotionBlurComponent.Uniforms._Tile4RT;
				cb.GetTemporaryRT(tile4RT, context.width / 4, context.height / 4, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile2RT);
				cb.Blit(tile2RT, tile4RT, material, 2);
				cb.ReleaseTemporaryRT(tile2RT);
				int tile8RT = MotionBlurComponent.Uniforms._Tile8RT;
				cb.GetTemporaryRT(tile8RT, context.width / 8, context.height / 8, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile4RT);
				cb.Blit(tile4RT, tile8RT, material, 2);
				cb.ReleaseTemporaryRT(tile4RT);
				Vector2 vector = Vector2.one * ((float)num2 / 8f - 1f) * -0.5f;
				cb.SetGlobalVector(MotionBlurComponent.Uniforms._TileMaxOffs, vector);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._TileMaxLoop, (float)((int)((float)num2 / 8f)));
				int tileVRT = MotionBlurComponent.Uniforms._TileVRT;
				cb.GetTemporaryRT(tileVRT, context.width / num2, context.height / num2, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile8RT);
				cb.Blit(tile8RT, tileVRT, material, 3);
				cb.ReleaseTemporaryRT(tile8RT);
				int neighborMaxTex = MotionBlurComponent.Uniforms._NeighborMaxTex;
				int num4 = context.width / num2;
				int num5 = context.height / num2;
				cb.GetTemporaryRT(neighborMaxTex, num4, num5, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tileVRT);
				cb.Blit(tileVRT, neighborMaxTex, material, 4);
				cb.ReleaseTemporaryRT(tileVRT);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._LoopCount, (float)Mathf.Clamp(settings.sampleCount / 2, 1, 64));
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
				cb.Blit(source, destination, material, 5);
				cb.ReleaseTemporaryRT(velocityTex);
				cb.ReleaseTemporaryRT(neighborMaxTex);
			}

			// Token: 0x04002877 RID: 10359
			private RenderTextureFormat m_VectorRTFormat = RenderTextureFormat.RGHalf;

			// Token: 0x04002878 RID: 10360
			private RenderTextureFormat m_PackedRTFormat = RenderTextureFormat.ARGB2101010;
		}

		// Token: 0x02000730 RID: 1840
		public class FrameBlendingFilter
		{
			// Token: 0x060036E7 RID: 14055 RVA: 0x0004B538 File Offset: 0x00049738
			public FrameBlendingFilter()
			{
				this.m_UseCompression = MotionBlurComponent.FrameBlendingFilter.CheckSupportCompression();
				this.m_RawTextureFormat = MotionBlurComponent.FrameBlendingFilter.GetPreferredRenderTextureFormat();
				this.m_FrameList = new MotionBlurComponent.FrameBlendingFilter.Frame[4];
			}

			// Token: 0x060036E8 RID: 14056 RVA: 0x001447D4 File Offset: 0x001429D4
			public void Dispose()
			{
				foreach (MotionBlurComponent.FrameBlendingFilter.Frame frame in this.m_FrameList)
				{
					frame.Release();
				}
			}

			// Token: 0x060036E9 RID: 14057 RVA: 0x00144808 File Offset: 0x00142A08
			public void PushFrame(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, Material material)
			{
				int frameCount = Time.frameCount;
				if (frameCount == this.m_LastFrameCount)
				{
					return;
				}
				int num = frameCount % this.m_FrameList.Length;
				if (this.m_UseCompression)
				{
					this.m_FrameList[num].MakeRecord(cb, source, width, height, material);
				}
				else
				{
					this.m_FrameList[num].MakeRecordRaw(cb, source, width, height, this.m_RawTextureFormat);
				}
				this.m_LastFrameCount = frameCount;
			}

			// Token: 0x060036EA RID: 14058 RVA: 0x00144878 File Offset: 0x00142A78
			public void BlendFrames(CommandBuffer cb, float strength, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material)
			{
				float time = Time.time;
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative = this.GetFrameRelative(-1);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative2 = this.GetFrameRelative(-2);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative3 = this.GetFrameRelative(-3);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative4 = this.GetFrameRelative(-4);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1LumaTex, frameRelative.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2LumaTex, frameRelative2.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3LumaTex, frameRelative3.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4LumaTex, frameRelative4.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1ChromaTex, frameRelative.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2ChromaTex, frameRelative2.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3ChromaTex, frameRelative3.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4ChromaTex, frameRelative4.chromaTexture);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History1Weight, frameRelative.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History2Weight, frameRelative2.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History3Weight, frameRelative3.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History4Weight, frameRelative4.CalculateWeight(strength, time));
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
				cb.Blit(source, destination, material, this.m_UseCompression ? 7 : 8);
			}

			// Token: 0x060036EB RID: 14059 RVA: 0x0004B562 File Offset: 0x00049762
			private static bool CheckSupportCompression()
			{
				return SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8) && SystemInfo.supportedRenderTargetCount > 1;
			}

			// Token: 0x060036EC RID: 14060 RVA: 0x001449D4 File Offset: 0x00142BD4
			private static RenderTextureFormat GetPreferredRenderTextureFormat()
			{
				foreach (RenderTextureFormat renderTextureFormat in new RenderTextureFormat[]
				{
					RenderTextureFormat.RGB565,
					RenderTextureFormat.ARGB1555,
					RenderTextureFormat.ARGB4444
				})
				{
					if (SystemInfo.SupportsRenderTextureFormat(renderTextureFormat))
					{
						return renderTextureFormat;
					}
				}
				return RenderTextureFormat.Default;
			}

			// Token: 0x060036ED RID: 14061 RVA: 0x00144A10 File Offset: 0x00142C10
			private MotionBlurComponent.FrameBlendingFilter.Frame GetFrameRelative(int offset)
			{
				int num = (Time.frameCount + this.m_FrameList.Length + offset) % this.m_FrameList.Length;
				return this.m_FrameList[num];
			}

			// Token: 0x04002879 RID: 10361
			private bool m_UseCompression;

			// Token: 0x0400287A RID: 10362
			private RenderTextureFormat m_RawTextureFormat;

			// Token: 0x0400287B RID: 10363
			private MotionBlurComponent.FrameBlendingFilter.Frame[] m_FrameList;

			// Token: 0x0400287C RID: 10364
			private int m_LastFrameCount;

			// Token: 0x02000731 RID: 1841
			private struct Frame
			{
				// Token: 0x060036EE RID: 14062 RVA: 0x00144A44 File Offset: 0x00142C44
				public float CalculateWeight(float strength, float currentTime)
				{
					if (Mathf.Approximately(this.m_Time, 0f))
					{
						return 0f;
					}
					float num = Mathf.Lerp(80f, 16f, strength);
					return Mathf.Exp((this.m_Time - currentTime) * num);
				}

				// Token: 0x060036EF RID: 14063 RVA: 0x00144A8C File Offset: 0x00142C8C
				public void Release()
				{
					if (this.lumaTexture != null)
					{
						RenderTexture.ReleaseTemporary(this.lumaTexture);
					}
					if (this.chromaTexture != null)
					{
						RenderTexture.ReleaseTemporary(this.chromaTexture);
					}
					this.lumaTexture = null;
					this.chromaTexture = null;
				}

				// Token: 0x060036F0 RID: 14064 RVA: 0x00144ADC File Offset: 0x00142CDC
				public void MakeRecord(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, Material material)
				{
					this.Release();
					this.lumaTexture = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
					this.chromaTexture = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
					this.lumaTexture.filterMode = FilterMode.Point;
					this.chromaTexture.filterMode = FilterMode.Point;
					if (this.m_MRT == null)
					{
						this.m_MRT = new RenderTargetIdentifier[2];
					}
					this.m_MRT[0] = this.lumaTexture;
					this.m_MRT[1] = this.chromaTexture;
					cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
					cb.SetRenderTarget(this.m_MRT, this.lumaTexture);
					cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material, 0, 6);
					this.m_Time = Time.time;
				}

				// Token: 0x060036F1 RID: 14065 RVA: 0x00144BB0 File Offset: 0x00142DB0
				public void MakeRecordRaw(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, RenderTextureFormat format)
				{
					this.Release();
					this.lumaTexture = RenderTexture.GetTemporary(width, height, 0, format);
					this.lumaTexture.filterMode = FilterMode.Point;
					cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
					cb.Blit(source, this.lumaTexture);
					this.m_Time = Time.time;
				}

				// Token: 0x0400287D RID: 10365
				public RenderTexture lumaTexture;

				// Token: 0x0400287E RID: 10366
				public RenderTexture chromaTexture;

				// Token: 0x0400287F RID: 10367
				private float m_Time;

				// Token: 0x04002880 RID: 10368
				private RenderTargetIdentifier[] m_MRT;
			}
		}
	}
}

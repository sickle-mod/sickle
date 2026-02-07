using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000724 RID: 1828
	public sealed class EyeAdaptationComponent : PostProcessingComponentRenderTexture<EyeAdaptationModel>
	{
		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060036BE RID: 14014 RVA: 0x0004B2F3 File Offset: 0x000494F3
		public override bool active
		{
			get
			{
				return base.model.enabled && SystemInfo.supportsComputeShaders && !this.context.interrupted;
			}
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x0004B319 File Offset: 0x00049519
		public void ResetHistory()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x0004B319 File Offset: 0x00049519
		public override void OnEnable()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x001437BC File Offset: 0x001419BC
		public override void OnDisable()
		{
			RenderTexture[] autoExposurePool = this.m_AutoExposurePool;
			for (int i = 0; i < autoExposurePool.Length; i++)
			{
				GraphicsUtils.Destroy(autoExposurePool[i]);
			}
			if (this.m_HistogramBuffer != null)
			{
				this.m_HistogramBuffer.Release();
			}
			this.m_HistogramBuffer = null;
			if (this.m_DebugHistogram != null)
			{
				this.m_DebugHistogram.Release();
			}
			this.m_DebugHistogram = null;
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x00143820 File Offset: 0x00141A20
		private Vector4 GetHistogramScaleOffsetRes()
		{
			EyeAdaptationModel.Settings settings = base.model.settings;
			float num = (float)(settings.logMax - settings.logMin);
			float num2 = 1f / num;
			float num3 = (float)(-(float)settings.logMin) * num2;
			return new Vector4(num2, num3, Mathf.Floor((float)this.context.width / 2f), Mathf.Floor((float)this.context.height / 2f));
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x00143890 File Offset: 0x00141A90
		public Texture Prepare(RenderTexture source, Material uberMaterial)
		{
			EyeAdaptationModel.Settings settings = base.model.settings;
			if (this.m_EyeCompute == null)
			{
				this.m_EyeCompute = Resources.Load<ComputeShader>("Shaders/EyeHistogram");
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Eye Adaptation");
			material.shaderKeywords = null;
			if (this.m_HistogramBuffer == null)
			{
				this.m_HistogramBuffer = new ComputeBuffer(64, 4);
			}
			if (EyeAdaptationComponent.s_EmptyHistogramBuffer == null)
			{
				EyeAdaptationComponent.s_EmptyHistogramBuffer = new uint[64];
			}
			Vector4 histogramScaleOffsetRes = this.GetHistogramScaleOffsetRes();
			RenderTexture renderTexture = this.context.renderTextureFactory.Get((int)histogramScaleOffsetRes.z, (int)histogramScaleOffsetRes.w, 0, source.format, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
			Graphics.Blit(source, renderTexture);
			if (this.m_AutoExposurePool[0] == null || !this.m_AutoExposurePool[0].IsCreated())
			{
				this.m_AutoExposurePool[0] = new RenderTexture(1, 1, 0, RenderTextureFormat.RFloat);
			}
			if (this.m_AutoExposurePool[1] == null || !this.m_AutoExposurePool[1].IsCreated())
			{
				this.m_AutoExposurePool[1] = new RenderTexture(1, 1, 0, RenderTextureFormat.RFloat);
			}
			this.m_HistogramBuffer.SetData(EyeAdaptationComponent.s_EmptyHistogramBuffer);
			int num = this.m_EyeCompute.FindKernel("KEyeHistogram");
			this.m_EyeCompute.SetBuffer(num, "_Histogram", this.m_HistogramBuffer);
			this.m_EyeCompute.SetTexture(num, "_Source", renderTexture);
			this.m_EyeCompute.SetVector("_ScaleOffsetRes", histogramScaleOffsetRes);
			this.m_EyeCompute.Dispatch(num, Mathf.CeilToInt((float)renderTexture.width / 16f), Mathf.CeilToInt((float)renderTexture.height / 16f), 1);
			this.context.renderTextureFactory.Release(renderTexture);
			settings.highPercent = Mathf.Clamp(settings.highPercent, 1.01f, 99f);
			settings.lowPercent = Mathf.Clamp(settings.lowPercent, 1f, settings.highPercent - 0.01f);
			material.SetBuffer("_Histogram", this.m_HistogramBuffer);
			material.SetVector(EyeAdaptationComponent.Uniforms._Params, new Vector4(settings.lowPercent * 0.01f, settings.highPercent * 0.01f, Mathf.Exp(settings.minLuminance * 0.6931472f), Mathf.Exp(settings.maxLuminance * 0.6931472f)));
			material.SetVector(EyeAdaptationComponent.Uniforms._Speed, new Vector2(settings.speedDown, settings.speedUp));
			material.SetVector(EyeAdaptationComponent.Uniforms._ScaleOffsetRes, histogramScaleOffsetRes);
			material.SetFloat(EyeAdaptationComponent.Uniforms._ExposureCompensation, settings.keyValue);
			if (settings.dynamicKeyValue)
			{
				material.EnableKeyword("AUTO_KEY_VALUE");
			}
			if (this.m_FirstFrame || !Application.isPlaying)
			{
				this.m_CurrentAutoExposure = this.m_AutoExposurePool[0];
				Graphics.Blit(null, this.m_CurrentAutoExposure, material, 1);
				Graphics.Blit(this.m_AutoExposurePool[0], this.m_AutoExposurePool[1]);
			}
			else
			{
				int num2 = this.m_AutoExposurePingPing;
				Texture texture = this.m_AutoExposurePool[++num2 % 2];
				RenderTexture renderTexture2 = this.m_AutoExposurePool[++num2 % 2];
				Graphics.Blit(texture, renderTexture2, material, (int)settings.adaptationType);
				this.m_AutoExposurePingPing = (num2 + 1) % 2;
				this.m_CurrentAutoExposure = renderTexture2;
			}
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation))
			{
				if (this.m_DebugHistogram == null || !this.m_DebugHistogram.IsCreated())
				{
					this.m_DebugHistogram = new RenderTexture(256, 128, 0, RenderTextureFormat.ARGB32)
					{
						filterMode = FilterMode.Point,
						wrapMode = TextureWrapMode.Clamp
					};
				}
				material.SetFloat(EyeAdaptationComponent.Uniforms._DebugWidth, (float)this.m_DebugHistogram.width);
				Graphics.Blit(null, this.m_DebugHistogram, material, 2);
			}
			this.m_FirstFrame = false;
			return this.m_CurrentAutoExposure;
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x00143C54 File Offset: 0x00141E54
		public void OnGUI()
		{
			if (this.m_DebugHistogram == null || !this.m_DebugHistogram.IsCreated())
			{
				return;
			}
			GUI.DrawTexture(new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)this.m_DebugHistogram.width, (float)this.m_DebugHistogram.height), this.m_DebugHistogram);
		}

		// Token: 0x04002832 RID: 10290
		private ComputeShader m_EyeCompute;

		// Token: 0x04002833 RID: 10291
		private ComputeBuffer m_HistogramBuffer;

		// Token: 0x04002834 RID: 10292
		private readonly RenderTexture[] m_AutoExposurePool = new RenderTexture[2];

		// Token: 0x04002835 RID: 10293
		private int m_AutoExposurePingPing;

		// Token: 0x04002836 RID: 10294
		private RenderTexture m_CurrentAutoExposure;

		// Token: 0x04002837 RID: 10295
		private RenderTexture m_DebugHistogram;

		// Token: 0x04002838 RID: 10296
		private static uint[] s_EmptyHistogramBuffer;

		// Token: 0x04002839 RID: 10297
		private bool m_FirstFrame = true;

		// Token: 0x0400283A RID: 10298
		private const int k_HistogramBins = 64;

		// Token: 0x0400283B RID: 10299
		private const int k_HistogramThreadX = 16;

		// Token: 0x0400283C RID: 10300
		private const int k_HistogramThreadY = 16;

		// Token: 0x02000725 RID: 1829
		private static class Uniforms
		{
			// Token: 0x0400283D RID: 10301
			internal static readonly int _Params = Shader.PropertyToID("_Params");

			// Token: 0x0400283E RID: 10302
			internal static readonly int _Speed = Shader.PropertyToID("_Speed");

			// Token: 0x0400283F RID: 10303
			internal static readonly int _ScaleOffsetRes = Shader.PropertyToID("_ScaleOffsetRes");

			// Token: 0x04002840 RID: 10304
			internal static readonly int _ExposureCompensation = Shader.PropertyToID("_ExposureCompensation");

			// Token: 0x04002841 RID: 10305
			internal static readonly int _AutoExposure = Shader.PropertyToID("_AutoExposure");

			// Token: 0x04002842 RID: 10306
			internal static readonly int _DebugWidth = Shader.PropertyToID("_DebugWidth");
		}
	}
}

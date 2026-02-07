using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000735 RID: 1845
	public sealed class TaaComponent : PostProcessingComponentRenderTexture<AntialiasingModel>
	{
		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060036FA RID: 14074 RVA: 0x00145554 File Offset: 0x00143754
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.method == AntialiasingModel.Method.Taa && SystemInfo.supportsMotionVectors && SystemInfo.supportedRenderTargetCount >= 2 && !this.context.interrupted;
			}
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x0002FC16 File Offset: 0x0002DE16
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x060036FC RID: 14076 RVA: 0x0004B5D0 File Offset: 0x000497D0
		// (set) Token: 0x060036FD RID: 14077 RVA: 0x0004B5D8 File Offset: 0x000497D8
		public Vector2 jitterVector { get; private set; }

		// Token: 0x060036FE RID: 14078 RVA: 0x0004B5E1 File Offset: 0x000497E1
		public void ResetHistory()
		{
			this.m_ResetHistory = true;
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x001455A0 File Offset: 0x001437A0
		public void SetProjectionMatrix(Func<Vector2, Matrix4x4> jitteredFunc)
		{
			AntialiasingModel.TaaSettings taaSettings = base.model.settings.taaSettings;
			Vector2 vector = this.GenerateRandomOffset();
			vector *= taaSettings.jitterSpread;
			this.context.camera.nonJitteredProjectionMatrix = this.context.camera.projectionMatrix;
			if (jitteredFunc != null)
			{
				this.context.camera.projectionMatrix = jitteredFunc(vector);
			}
			else
			{
				this.context.camera.projectionMatrix = (this.context.camera.orthographic ? this.GetOrthographicProjectionMatrix(vector) : this.GetPerspectiveProjectionMatrix(vector));
			}
			this.context.camera.useJitteredProjectionMatrixForTransparentRendering = false;
			vector.x /= (float)this.context.width;
			vector.y /= (float)this.context.height;
			this.context.materialFactory.Get("Hidden/Post FX/Temporal Anti-aliasing").SetVector(TaaComponent.Uniforms._Jitter, vector);
			this.jitterVector = vector;
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x001456AC File Offset: 0x001438AC
		public void Render(RenderTexture source, RenderTexture destination)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Temporal Anti-aliasing");
			material.shaderKeywords = null;
			AntialiasingModel.TaaSettings taaSettings = base.model.settings.taaSettings;
			if (this.m_ResetHistory || this.m_HistoryTexture == null || this.m_HistoryTexture.width != source.width || this.m_HistoryTexture.height != source.height)
			{
				if (this.m_HistoryTexture)
				{
					RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
				}
				this.m_HistoryTexture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
				this.m_HistoryTexture.name = "TAA History";
				Graphics.Blit(source, this.m_HistoryTexture, material, 2);
			}
			material.SetVector(TaaComponent.Uniforms._SharpenParameters, new Vector4(taaSettings.sharpen, 0f, 0f, 0f));
			material.SetVector(TaaComponent.Uniforms._FinalBlendParameters, new Vector4(taaSettings.stationaryBlending, taaSettings.motionBlending, 6000f, 0f));
			material.SetTexture(TaaComponent.Uniforms._MainTex, source);
			material.SetTexture(TaaComponent.Uniforms._HistoryTex, this.m_HistoryTexture);
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
			temporary.name = "TAA History";
			this.m_MRT[0] = destination.colorBuffer;
			this.m_MRT[1] = temporary.colorBuffer;
			Graphics.SetRenderTarget(this.m_MRT, source.depthBuffer);
			GraphicsUtils.Blit(material, this.context.camera.orthographic ? 1 : 0);
			RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
			this.m_HistoryTexture = temporary;
			this.m_ResetHistory = false;
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x00145870 File Offset: 0x00143A70
		private float GetHaltonValue(int index, int radix)
		{
			float num = 0f;
			float num2 = 1f / (float)radix;
			while (index > 0)
			{
				num += (float)(index % radix) * num2;
				index /= radix;
				num2 /= (float)radix;
			}
			return num;
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x001458A8 File Offset: 0x00143AA8
		private Vector2 GenerateRandomOffset()
		{
			Vector2 vector = new Vector2(this.GetHaltonValue(this.m_SampleIndex & 1023, 2), this.GetHaltonValue(this.m_SampleIndex & 1023, 3));
			int num = this.m_SampleIndex + 1;
			this.m_SampleIndex = num;
			if (num >= 8)
			{
				this.m_SampleIndex = 0;
			}
			return vector;
		}

		// Token: 0x06003703 RID: 14083 RVA: 0x001458FC File Offset: 0x00143AFC
		private Matrix4x4 GetPerspectiveProjectionMatrix(Vector2 offset)
		{
			float num = Mathf.Tan(0.008726646f * this.context.camera.fieldOfView);
			float num2 = num * this.context.camera.aspect;
			offset.x *= num2 / (0.5f * (float)this.context.width);
			offset.y *= num / (0.5f * (float)this.context.height);
			float num3 = (offset.x - num2) * this.context.camera.nearClipPlane;
			float num4 = (offset.x + num2) * this.context.camera.nearClipPlane;
			float num5 = (offset.y + num) * this.context.camera.nearClipPlane;
			float num6 = (offset.y - num) * this.context.camera.nearClipPlane;
			Matrix4x4 matrix4x = default(Matrix4x4);
			matrix4x[0, 0] = 2f * this.context.camera.nearClipPlane / (num4 - num3);
			matrix4x[0, 1] = 0f;
			matrix4x[0, 2] = (num4 + num3) / (num4 - num3);
			matrix4x[0, 3] = 0f;
			matrix4x[1, 0] = 0f;
			matrix4x[1, 1] = 2f * this.context.camera.nearClipPlane / (num5 - num6);
			matrix4x[1, 2] = (num5 + num6) / (num5 - num6);
			matrix4x[1, 3] = 0f;
			matrix4x[2, 0] = 0f;
			matrix4x[2, 1] = 0f;
			matrix4x[2, 2] = -(this.context.camera.farClipPlane + this.context.camera.nearClipPlane) / (this.context.camera.farClipPlane - this.context.camera.nearClipPlane);
			matrix4x[2, 3] = -(2f * this.context.camera.farClipPlane * this.context.camera.nearClipPlane) / (this.context.camera.farClipPlane - this.context.camera.nearClipPlane);
			matrix4x[3, 0] = 0f;
			matrix4x[3, 1] = 0f;
			matrix4x[3, 2] = -1f;
			matrix4x[3, 3] = 0f;
			return matrix4x;
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x00145B84 File Offset: 0x00143D84
		private Matrix4x4 GetOrthographicProjectionMatrix(Vector2 offset)
		{
			float orthographicSize = this.context.camera.orthographicSize;
			float num = orthographicSize * this.context.camera.aspect;
			offset.x *= num / (0.5f * (float)this.context.width);
			offset.y *= orthographicSize / (0.5f * (float)this.context.height);
			float num2 = offset.x - num;
			float num3 = offset.x + num;
			float num4 = offset.y + orthographicSize;
			float num5 = offset.y - orthographicSize;
			return Matrix4x4.Ortho(num2, num3, num5, num4, this.context.camera.nearClipPlane, this.context.camera.farClipPlane);
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x0004B5EA File Offset: 0x000497EA
		public override void OnDisable()
		{
			if (this.m_HistoryTexture != null)
			{
				RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
			}
			this.m_HistoryTexture = null;
			this.m_SampleIndex = 0;
			this.ResetHistory();
		}

		// Token: 0x040028B3 RID: 10419
		private const string k_ShaderString = "Hidden/Post FX/Temporal Anti-aliasing";

		// Token: 0x040028B4 RID: 10420
		private const int k_SampleCount = 8;

		// Token: 0x040028B5 RID: 10421
		private readonly RenderBuffer[] m_MRT = new RenderBuffer[2];

		// Token: 0x040028B6 RID: 10422
		private int m_SampleIndex;

		// Token: 0x040028B7 RID: 10423
		private bool m_ResetHistory = true;

		// Token: 0x040028B8 RID: 10424
		private RenderTexture m_HistoryTexture;

		// Token: 0x02000736 RID: 1846
		private static class Uniforms
		{
			// Token: 0x040028BA RID: 10426
			internal static int _Jitter = Shader.PropertyToID("_Jitter");

			// Token: 0x040028BB RID: 10427
			internal static int _SharpenParameters = Shader.PropertyToID("_SharpenParameters");

			// Token: 0x040028BC RID: 10428
			internal static int _FinalBlendParameters = Shader.PropertyToID("_FinalBlendParameters");

			// Token: 0x040028BD RID: 10429
			internal static int _HistoryTex = Shader.PropertyToID("_HistoryTex");

			// Token: 0x040028BE RID: 10430
			internal static int _MainTex = Shader.PropertyToID("_MainTex");
		}
	}
}

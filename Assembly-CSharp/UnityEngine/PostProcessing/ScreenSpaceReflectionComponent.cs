using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000732 RID: 1842
	public sealed class ScreenSpaceReflectionComponent : PostProcessingComponentCommandBuffer<ScreenSpaceReflectionModel>
	{
		// Token: 0x060036F2 RID: 14066 RVA: 0x000283F8 File Offset: 0x000265F8
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060036F3 RID: 14067 RVA: 0x0004B577 File Offset: 0x00049777
		public override bool active
		{
			get
			{
				return base.model.enabled && this.context.isGBufferAvailable && !this.context.interrupted;
			}
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x00144C0C File Offset: 0x00142E0C
		public override void OnEnable()
		{
			this.m_ReflectionTextures[0] = Shader.PropertyToID("_ReflectionTexture0");
			this.m_ReflectionTextures[1] = Shader.PropertyToID("_ReflectionTexture1");
			this.m_ReflectionTextures[2] = Shader.PropertyToID("_ReflectionTexture2");
			this.m_ReflectionTextures[3] = Shader.PropertyToID("_ReflectionTexture3");
			this.m_ReflectionTextures[4] = Shader.PropertyToID("_ReflectionTexture4");
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x0004B5A3 File Offset: 0x000497A3
		public override string GetName()
		{
			return "Screen Space Reflection";
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x0004B5AA File Offset: 0x000497AA
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.AfterFinalPass;
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x00144C74 File Offset: 0x00142E74
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			ScreenSpaceReflectionModel.Settings settings = base.model.settings;
			Camera camera = this.context.camera;
			int num = ((settings.reflection.reflectionQuality == ScreenSpaceReflectionModel.SSRResolution.High) ? 1 : 2);
			int num2 = this.context.width / num;
			int num3 = this.context.height / num;
			float num4 = (float)this.context.width;
			float num5 = (float)this.context.height;
			float num6 = num4 / 2f;
			float num7 = num5 / 2f;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Screen Space Reflection");
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._RayStepSize, settings.reflection.stepSize);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._AdditiveReflection, (settings.reflection.blendType == ScreenSpaceReflectionModel.SSRReflectionBlendType.Additive) ? 1 : 0);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._BilateralUpsampling, this.k_BilateralUpsample ? 1 : 0);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._TreatBackfaceHitAsMiss, this.k_TreatBackfaceHitAsMiss ? 1 : 0);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._AllowBackwardsRays, settings.reflection.reflectBackfaces ? 1 : 0);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._TraceBehindObjects, this.k_TraceBehindObjects ? 1 : 0);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._MaxSteps, settings.reflection.iterationCount);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._FullResolutionFiltering, 0);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._HalfResolution, (settings.reflection.reflectionQuality != ScreenSpaceReflectionModel.SSRResolution.High) ? 1 : 0);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._HighlightSuppression, this.k_HighlightSuppression ? 1 : 0);
			float num8 = num4 / (-2f * Mathf.Tan(camera.fieldOfView / 180f * 3.1415927f * 0.5f));
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._PixelsPerMeterAtOneMeter, num8);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._ScreenEdgeFading, settings.screenEdgeMask.intensity);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._ReflectionBlur, settings.reflection.reflectionBlur);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._MaxRayTraceDistance, settings.reflection.maxDistance);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FadeDistance, settings.intensity.fadeDistance);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._LayerThickness, settings.reflection.widthModifier);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._SSRMultiplier, settings.intensity.reflectionMultiplier);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FresnelFade, settings.intensity.fresnelFade);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FresnelFadePower, settings.intensity.fresnelFadePower);
			Matrix4x4 projectionMatrix = camera.projectionMatrix;
			Vector4 vector = new Vector4(-2f / (num4 * projectionMatrix[0]), -2f / (num5 * projectionMatrix[5]), (1f - projectionMatrix[2]) / projectionMatrix[0], (1f + projectionMatrix[6]) / projectionMatrix[5]);
			Vector3 vector2 = (float.IsPositiveInfinity(camera.farClipPlane) ? new Vector3(camera.nearClipPlane, -1f, 1f) : new Vector3(camera.nearClipPlane * camera.farClipPlane, camera.nearClipPlane - camera.farClipPlane, camera.farClipPlane));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ReflectionBufferSize, new Vector2((float)num2, (float)num3));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ScreenSize, new Vector2(num4, num5));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._InvScreenSize, new Vector2(1f / num4, 1f / num5));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ProjInfo, vector);
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._CameraClipInfo, vector2);
			Matrix4x4 matrix4x = default(Matrix4x4);
			matrix4x.SetRow(0, new Vector4(num6, 0f, 0f, num6));
			matrix4x.SetRow(1, new Vector4(0f, num7, 0f, num7));
			matrix4x.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
			matrix4x.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
			Matrix4x4 matrix4x2 = matrix4x * projectionMatrix;
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._ProjectToPixelMatrix, matrix4x2);
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._WorldToCameraMatrix, camera.worldToCameraMatrix);
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._CameraToWorldMatrix, camera.worldToCameraMatrix.inverse);
			RenderTextureFormat renderTextureFormat = (this.context.isHdr ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32);
			int normalAndRoughnessTexture = ScreenSpaceReflectionComponent.Uniforms._NormalAndRoughnessTexture;
			int hitPointTexture = ScreenSpaceReflectionComponent.Uniforms._HitPointTexture;
			int blurTexture = ScreenSpaceReflectionComponent.Uniforms._BlurTexture;
			int filteredReflections = ScreenSpaceReflectionComponent.Uniforms._FilteredReflections;
			int finalReflectionTexture = ScreenSpaceReflectionComponent.Uniforms._FinalReflectionTexture;
			int tempTexture = ScreenSpaceReflectionComponent.Uniforms._TempTexture;
			cb.GetTemporaryRT(normalAndRoughnessTexture, -1, -1, 0, FilterMode.Point, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.GetTemporaryRT(hitPointTexture, num2, num3, 0, FilterMode.Bilinear, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
			for (int i = 0; i < 5; i++)
			{
				cb.GetTemporaryRT(this.m_ReflectionTextures[i], num2 >> i, num3 >> i, 0, FilterMode.Bilinear, renderTextureFormat);
			}
			cb.GetTemporaryRT(filteredReflections, num2, num3, 0, this.k_BilateralUpsample ? FilterMode.Point : FilterMode.Bilinear, renderTextureFormat);
			cb.GetTemporaryRT(finalReflectionTexture, num2, num3, 0, FilterMode.Point, renderTextureFormat);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, normalAndRoughnessTexture, material, 6);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, hitPointTexture, material, 0);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, filteredReflections, material, 5);
			cb.Blit(filteredReflections, this.m_ReflectionTextures[0], material, 8);
			for (int j = 1; j < 5; j++)
			{
				int num9 = this.m_ReflectionTextures[j - 1];
				int num10 = j;
				cb.GetTemporaryRT(blurTexture, num2 >> num10, num3 >> num10, 0, FilterMode.Bilinear, renderTextureFormat);
				cb.SetGlobalVector(ScreenSpaceReflectionComponent.Uniforms._Axis, new Vector4(1f, 0f, 0f, 0f));
				cb.SetGlobalFloat(ScreenSpaceReflectionComponent.Uniforms._CurrentMipLevel, (float)j - 1f);
				cb.Blit(num9, blurTexture, material, 2);
				cb.SetGlobalVector(ScreenSpaceReflectionComponent.Uniforms._Axis, new Vector4(0f, 1f, 0f, 0f));
				num9 = this.m_ReflectionTextures[j];
				cb.Blit(blurTexture, num9, material, 2);
				cb.ReleaseTemporaryRT(blurTexture);
			}
			cb.Blit(this.m_ReflectionTextures[0], finalReflectionTexture, material, 3);
			cb.GetTemporaryRT(tempTexture, camera.pixelWidth, camera.pixelHeight, 0, FilterMode.Bilinear, renderTextureFormat);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, tempTexture, material, 1);
			cb.Blit(tempTexture, BuiltinRenderTextureType.CameraTarget);
			cb.ReleaseTemporaryRT(tempTexture);
		}

		// Token: 0x04002881 RID: 10369
		private bool k_HighlightSuppression;

		// Token: 0x04002882 RID: 10370
		private bool k_TraceBehindObjects = true;

		// Token: 0x04002883 RID: 10371
		private bool k_TreatBackfaceHitAsMiss;

		// Token: 0x04002884 RID: 10372
		private bool k_BilateralUpsample = true;

		// Token: 0x04002885 RID: 10373
		private readonly int[] m_ReflectionTextures = new int[5];

		// Token: 0x02000733 RID: 1843
		private static class Uniforms
		{
			// Token: 0x04002886 RID: 10374
			internal static readonly int _RayStepSize = Shader.PropertyToID("_RayStepSize");

			// Token: 0x04002887 RID: 10375
			internal static readonly int _AdditiveReflection = Shader.PropertyToID("_AdditiveReflection");

			// Token: 0x04002888 RID: 10376
			internal static readonly int _BilateralUpsampling = Shader.PropertyToID("_BilateralUpsampling");

			// Token: 0x04002889 RID: 10377
			internal static readonly int _TreatBackfaceHitAsMiss = Shader.PropertyToID("_TreatBackfaceHitAsMiss");

			// Token: 0x0400288A RID: 10378
			internal static readonly int _AllowBackwardsRays = Shader.PropertyToID("_AllowBackwardsRays");

			// Token: 0x0400288B RID: 10379
			internal static readonly int _TraceBehindObjects = Shader.PropertyToID("_TraceBehindObjects");

			// Token: 0x0400288C RID: 10380
			internal static readonly int _MaxSteps = Shader.PropertyToID("_MaxSteps");

			// Token: 0x0400288D RID: 10381
			internal static readonly int _FullResolutionFiltering = Shader.PropertyToID("_FullResolutionFiltering");

			// Token: 0x0400288E RID: 10382
			internal static readonly int _HalfResolution = Shader.PropertyToID("_HalfResolution");

			// Token: 0x0400288F RID: 10383
			internal static readonly int _HighlightSuppression = Shader.PropertyToID("_HighlightSuppression");

			// Token: 0x04002890 RID: 10384
			internal static readonly int _PixelsPerMeterAtOneMeter = Shader.PropertyToID("_PixelsPerMeterAtOneMeter");

			// Token: 0x04002891 RID: 10385
			internal static readonly int _ScreenEdgeFading = Shader.PropertyToID("_ScreenEdgeFading");

			// Token: 0x04002892 RID: 10386
			internal static readonly int _ReflectionBlur = Shader.PropertyToID("_ReflectionBlur");

			// Token: 0x04002893 RID: 10387
			internal static readonly int _MaxRayTraceDistance = Shader.PropertyToID("_MaxRayTraceDistance");

			// Token: 0x04002894 RID: 10388
			internal static readonly int _FadeDistance = Shader.PropertyToID("_FadeDistance");

			// Token: 0x04002895 RID: 10389
			internal static readonly int _LayerThickness = Shader.PropertyToID("_LayerThickness");

			// Token: 0x04002896 RID: 10390
			internal static readonly int _SSRMultiplier = Shader.PropertyToID("_SSRMultiplier");

			// Token: 0x04002897 RID: 10391
			internal static readonly int _FresnelFade = Shader.PropertyToID("_FresnelFade");

			// Token: 0x04002898 RID: 10392
			internal static readonly int _FresnelFadePower = Shader.PropertyToID("_FresnelFadePower");

			// Token: 0x04002899 RID: 10393
			internal static readonly int _ReflectionBufferSize = Shader.PropertyToID("_ReflectionBufferSize");

			// Token: 0x0400289A RID: 10394
			internal static readonly int _ScreenSize = Shader.PropertyToID("_ScreenSize");

			// Token: 0x0400289B RID: 10395
			internal static readonly int _InvScreenSize = Shader.PropertyToID("_InvScreenSize");

			// Token: 0x0400289C RID: 10396
			internal static readonly int _ProjInfo = Shader.PropertyToID("_ProjInfo");

			// Token: 0x0400289D RID: 10397
			internal static readonly int _CameraClipInfo = Shader.PropertyToID("_CameraClipInfo");

			// Token: 0x0400289E RID: 10398
			internal static readonly int _ProjectToPixelMatrix = Shader.PropertyToID("_ProjectToPixelMatrix");

			// Token: 0x0400289F RID: 10399
			internal static readonly int _WorldToCameraMatrix = Shader.PropertyToID("_WorldToCameraMatrix");

			// Token: 0x040028A0 RID: 10400
			internal static readonly int _CameraToWorldMatrix = Shader.PropertyToID("_CameraToWorldMatrix");

			// Token: 0x040028A1 RID: 10401
			internal static readonly int _Axis = Shader.PropertyToID("_Axis");

			// Token: 0x040028A2 RID: 10402
			internal static readonly int _CurrentMipLevel = Shader.PropertyToID("_CurrentMipLevel");

			// Token: 0x040028A3 RID: 10403
			internal static readonly int _NormalAndRoughnessTexture = Shader.PropertyToID("_NormalAndRoughnessTexture");

			// Token: 0x040028A4 RID: 10404
			internal static readonly int _HitPointTexture = Shader.PropertyToID("_HitPointTexture");

			// Token: 0x040028A5 RID: 10405
			internal static readonly int _BlurTexture = Shader.PropertyToID("_BlurTexture");

			// Token: 0x040028A6 RID: 10406
			internal static readonly int _FilteredReflections = Shader.PropertyToID("_FilteredReflections");

			// Token: 0x040028A7 RID: 10407
			internal static readonly int _FinalReflectionTexture = Shader.PropertyToID("_FinalReflectionTexture");

			// Token: 0x040028A8 RID: 10408
			internal static readonly int _TempTexture = Shader.PropertyToID("_TempTexture");
		}

		// Token: 0x02000734 RID: 1844
		private enum PassIndex
		{
			// Token: 0x040028AA RID: 10410
			RayTraceStep,
			// Token: 0x040028AB RID: 10411
			CompositeFinal,
			// Token: 0x040028AC RID: 10412
			Blur,
			// Token: 0x040028AD RID: 10413
			CompositeSSR,
			// Token: 0x040028AE RID: 10414
			MinMipGeneration,
			// Token: 0x040028AF RID: 10415
			HitPointToReflections,
			// Token: 0x040028B0 RID: 10416
			BilateralKeyPack,
			// Token: 0x040028B1 RID: 10417
			BlitDepthAsCSZ,
			// Token: 0x040028B2 RID: 10418
			PoissonBlur
		}
	}
}

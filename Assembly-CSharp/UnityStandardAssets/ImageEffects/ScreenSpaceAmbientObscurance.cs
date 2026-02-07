using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006D2 RID: 1746
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Obscurance")]
	public class ScreenSpaceAmbientObscurance : PostEffectsBase
	{
		// Token: 0x0600354F RID: 13647 RVA: 0x00049DC7 File Offset: 0x00047FC7
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.aoMaterial = base.CheckShaderAndCreateMaterial(this.aoShader, this.aoMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x00049DFD File Offset: 0x00047FFD
		private void OnDisable()
		{
			if (this.aoMaterial)
			{
				global::UnityEngine.Object.DestroyImmediate(this.aoMaterial);
			}
			this.aoMaterial = null;
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x0013DF44 File Offset: 0x0013C144
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Camera component = base.GetComponent<Camera>();
			Matrix4x4 projectionMatrix = component.projectionMatrix;
			Matrix4x4 inverse = projectionMatrix.inverse;
			Vector4 vector = new Vector4(-2f / projectionMatrix[0, 0], -2f / projectionMatrix[1, 1], (1f - projectionMatrix[0, 2]) / projectionMatrix[0, 0], (1f + projectionMatrix[1, 2]) / projectionMatrix[1, 1]);
			if (component.stereoEnabled)
			{
				Matrix4x4 stereoProjectionMatrix = component.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
				Matrix4x4 stereoProjectionMatrix2 = component.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
				Vector4 vector2 = new Vector4(-2f / stereoProjectionMatrix[0, 0], -2f / stereoProjectionMatrix[1, 1], (1f - stereoProjectionMatrix[0, 2]) / stereoProjectionMatrix[0, 0], (1f + stereoProjectionMatrix[1, 2]) / stereoProjectionMatrix[1, 1]);
				Vector4 vector3 = new Vector4(-2f / stereoProjectionMatrix2[0, 0], -2f / stereoProjectionMatrix2[1, 1], (1f - stereoProjectionMatrix2[0, 2]) / stereoProjectionMatrix2[0, 0], (1f + stereoProjectionMatrix2[1, 2]) / stereoProjectionMatrix2[1, 1]);
				this.aoMaterial.SetVector("_ProjInfoLeft", vector2);
				this.aoMaterial.SetVector("_ProjInfoRight", vector3);
			}
			this.aoMaterial.SetVector("_ProjInfo", vector);
			this.aoMaterial.SetMatrix("_ProjectionInv", inverse);
			this.aoMaterial.SetTexture("_Rand", this.rand);
			this.aoMaterial.SetFloat("_Radius", this.radius);
			this.aoMaterial.SetFloat("_Radius2", this.radius * this.radius);
			this.aoMaterial.SetFloat("_Intensity", this.intensity);
			this.aoMaterial.SetFloat("_BlurFilterDistance", this.blurFilterDistance);
			int width = source.width;
			int height = source.height;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width >> this.downsample, height >> this.downsample);
			Graphics.Blit(source, renderTexture, this.aoMaterial, 0);
			if (this.downsample > 0)
			{
				RenderTexture renderTexture2 = RenderTexture.GetTemporary(width, height);
				Graphics.Blit(renderTexture, renderTexture2, this.aoMaterial, 4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = renderTexture2;
			}
			for (int i = 0; i < this.blurIterations; i++)
			{
				this.aoMaterial.SetVector("_Axis", new Vector2(1f, 0f));
				RenderTexture renderTexture2 = RenderTexture.GetTemporary(width, height);
				Graphics.Blit(renderTexture, renderTexture2, this.aoMaterial, 1);
				RenderTexture.ReleaseTemporary(renderTexture);
				this.aoMaterial.SetVector("_Axis", new Vector2(0f, 1f));
				renderTexture = RenderTexture.GetTemporary(width, height);
				Graphics.Blit(renderTexture2, renderTexture, this.aoMaterial, 1);
				RenderTexture.ReleaseTemporary(renderTexture2);
			}
			this.aoMaterial.SetTexture("_AOTex", renderTexture);
			Graphics.Blit(source, destination, this.aoMaterial, 2);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x040026AE RID: 9902
		[Range(0f, 3f)]
		public float intensity = 0.5f;

		// Token: 0x040026AF RID: 9903
		[Range(0.1f, 3f)]
		public float radius = 0.2f;

		// Token: 0x040026B0 RID: 9904
		[Range(0f, 3f)]
		public int blurIterations = 1;

		// Token: 0x040026B1 RID: 9905
		[Range(0f, 5f)]
		public float blurFilterDistance = 1.25f;

		// Token: 0x040026B2 RID: 9906
		[Range(0f, 1f)]
		public int downsample;

		// Token: 0x040026B3 RID: 9907
		public Texture2D rand;

		// Token: 0x040026B4 RID: 9908
		public Shader aoShader;

		// Token: 0x040026B5 RID: 9909
		private Material aoMaterial;
	}
}

using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006D3 RID: 1747
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Occlusion")]
	public class ScreenSpaceAmbientOcclusion : MonoBehaviour
	{
		// Token: 0x06003553 RID: 13651 RVA: 0x00049E4E File Offset: 0x0004804E
		private static Material CreateMaterial(Shader shader)
		{
			if (!shader)
			{
				return null;
			}
			return new Material(shader)
			{
				hideFlags = HideFlags.HideAndDontSave
			};
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x00049E68 File Offset: 0x00048068
		private static void DestroyMaterial(Material mat)
		{
			if (mat)
			{
				global::UnityEngine.Object.DestroyImmediate(mat);
				mat = null;
			}
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x00049E7B File Offset: 0x0004807B
		private void OnDisable()
		{
			ScreenSpaceAmbientOcclusion.DestroyMaterial(this.m_SSAOMaterial);
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x0013E290 File Offset: 0x0013C490
		private void Start()
		{
			if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				this.m_Supported = false;
				base.enabled = false;
				return;
			}
			this.CreateMaterials();
			if (!this.m_SSAOMaterial || this.m_SSAOMaterial.passCount != 5)
			{
				this.m_Supported = false;
				base.enabled = false;
				return;
			}
			this.m_Supported = true;
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x00049E88 File Offset: 0x00048088
		private void OnEnable()
		{
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x0013E2EC File Offset: 0x0013C4EC
		private void CreateMaterials()
		{
			if (!this.m_SSAOMaterial && this.m_SSAOShader.isSupported)
			{
				this.m_SSAOMaterial = ScreenSpaceAmbientOcclusion.CreateMaterial(this.m_SSAOShader);
				this.m_SSAOMaterial.SetTexture("_RandomTexture", this.m_RandomTexture);
			}
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x0013E33C File Offset: 0x0013C53C
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.m_Supported || !this.m_SSAOShader.isSupported)
			{
				base.enabled = false;
				return;
			}
			this.CreateMaterials();
			this.m_Downsampling = Mathf.Clamp(this.m_Downsampling, 1, 6);
			this.m_Radius = Mathf.Clamp(this.m_Radius, 0.05f, 1f);
			this.m_MinZ = Mathf.Clamp(this.m_MinZ, 1E-05f, 0.5f);
			this.m_OcclusionIntensity = Mathf.Clamp(this.m_OcclusionIntensity, 0.5f, 4f);
			this.m_OcclusionAttenuation = Mathf.Clamp(this.m_OcclusionAttenuation, 0.2f, 2f);
			this.m_Blur = Mathf.Clamp(this.m_Blur, 0, 4);
			RenderTexture renderTexture = RenderTexture.GetTemporary(source.width / this.m_Downsampling, source.height / this.m_Downsampling, 0);
			float fieldOfView = base.GetComponent<Camera>().fieldOfView;
			float farClipPlane = base.GetComponent<Camera>().farClipPlane;
			float num = Mathf.Tan(fieldOfView * 0.017453292f * 0.5f) * farClipPlane;
			float num2 = num * base.GetComponent<Camera>().aspect;
			this.m_SSAOMaterial.SetVector("_FarCorner", new Vector3(num2, num, farClipPlane));
			int num3;
			int num4;
			if (this.m_RandomTexture)
			{
				num3 = this.m_RandomTexture.width;
				num4 = this.m_RandomTexture.height;
			}
			else
			{
				num3 = 1;
				num4 = 1;
			}
			this.m_SSAOMaterial.SetVector("_NoiseScale", new Vector3((float)renderTexture.width / (float)num3, (float)renderTexture.height / (float)num4, 0f));
			this.m_SSAOMaterial.SetVector("_Params", new Vector4(this.m_Radius, this.m_MinZ, 1f / this.m_OcclusionAttenuation, this.m_OcclusionIntensity));
			bool flag = this.m_Blur > 0;
			Graphics.Blit(flag ? null : source, renderTexture, this.m_SSAOMaterial, (int)this.m_SampleCount);
			if (flag)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
				this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4((float)this.m_Blur / (float)source.width, 0f, 0f, 0f));
				this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
				Graphics.Blit(null, temporary, this.m_SSAOMaterial, 3);
				RenderTexture.ReleaseTemporary(renderTexture);
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
				this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(0f, (float)this.m_Blur / (float)source.height, 0f, 0f));
				this.m_SSAOMaterial.SetTexture("_SSAO", temporary);
				Graphics.Blit(source, temporary2, this.m_SSAOMaterial, 3);
				RenderTexture.ReleaseTemporary(temporary);
				renderTexture = temporary2;
			}
			this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
			Graphics.Blit(source, destination, this.m_SSAOMaterial, 4);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x040026B6 RID: 9910
		[Range(0.05f, 1f)]
		public float m_Radius = 0.4f;

		// Token: 0x040026B7 RID: 9911
		public ScreenSpaceAmbientOcclusion.SSAOSamples m_SampleCount = ScreenSpaceAmbientOcclusion.SSAOSamples.Medium;

		// Token: 0x040026B8 RID: 9912
		[Range(0.5f, 4f)]
		public float m_OcclusionIntensity = 1.5f;

		// Token: 0x040026B9 RID: 9913
		[Range(0f, 4f)]
		public int m_Blur = 2;

		// Token: 0x040026BA RID: 9914
		[Range(1f, 6f)]
		public int m_Downsampling = 2;

		// Token: 0x040026BB RID: 9915
		[Range(0.2f, 2f)]
		public float m_OcclusionAttenuation = 1f;

		// Token: 0x040026BC RID: 9916
		[Range(1E-05f, 0.5f)]
		public float m_MinZ = 0.01f;

		// Token: 0x040026BD RID: 9917
		public Shader m_SSAOShader;

		// Token: 0x040026BE RID: 9918
		private Material m_SSAOMaterial;

		// Token: 0x040026BF RID: 9919
		public Texture2D m_RandomTexture;

		// Token: 0x040026C0 RID: 9920
		private bool m_Supported;

		// Token: 0x020006D4 RID: 1748
		public enum SSAOSamples
		{
			// Token: 0x040026C2 RID: 9922
			Low,
			// Token: 0x040026C3 RID: 9923
			Medium,
			// Token: 0x040026C4 RID: 9924
			High
		}
	}
}

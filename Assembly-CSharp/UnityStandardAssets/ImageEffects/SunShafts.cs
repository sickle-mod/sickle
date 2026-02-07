using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006D6 RID: 1750
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Sun Shafts")]
	public class SunShafts : PostEffectsBase
	{
		// Token: 0x0600355D RID: 13661 RVA: 0x0013E68C File Offset: 0x0013C88C
		public override bool CheckResources()
		{
			base.CheckSupport(this.useDepthTexture);
			this.sunShaftsMaterial = base.CheckShaderAndCreateMaterial(this.sunShaftsShader, this.sunShaftsMaterial);
			this.simpleClearMaterial = base.CheckShaderAndCreateMaterial(this.simpleClearShader, this.simpleClearMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x0013E6EC File Offset: 0x0013C8EC
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.useDepthTexture)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			int num = 4;
			if (this.resolution == SunShafts.SunShaftsResolution.Normal)
			{
				num = 2;
			}
			else if (this.resolution == SunShafts.SunShaftsResolution.High)
			{
				num = 1;
			}
			Vector3 vector = Vector3.one * 0.5f;
			if (this.sunTransform)
			{
				vector = base.GetComponent<Camera>().WorldToViewportPoint(this.sunTransform.position);
			}
			else
			{
				vector = new Vector3(0.5f, 0.5f, 0f);
			}
			int num2 = source.width / num;
			int num3 = source.height / num;
			RenderTexture renderTexture = RenderTexture.GetTemporary(num2, num3, 0);
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, 0f, 0f) * this.sunShaftBlurRadius);
			this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
			this.sunShaftsMaterial.SetVector("_SunThreshold", this.sunThreshold);
			if (!this.useDepthTexture)
			{
				RenderTextureFormat renderTextureFormat = (base.GetComponent<Camera>().allowHDR ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default);
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, renderTextureFormat);
				RenderTexture.active = temporary;
				GL.ClearWithSkybox(false, base.GetComponent<Camera>());
				this.sunShaftsMaterial.SetTexture("_Skybox", temporary);
				Graphics.Blit(source, renderTexture, this.sunShaftsMaterial, 3);
				RenderTexture.ReleaseTemporary(temporary);
			}
			else
			{
				Graphics.Blit(source, renderTexture, this.sunShaftsMaterial, 2);
			}
			base.DrawBorder(renderTexture, this.simpleClearMaterial);
			this.radialBlurIterations = Mathf.Clamp(this.radialBlurIterations, 1, 4);
			float num4 = this.sunShaftBlurRadius * 0.0013020834f;
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num4, num4, 0f, 0f));
			this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
			for (int i = 0; i < this.radialBlurIterations; i++)
			{
				RenderTexture temporary2 = RenderTexture.GetTemporary(num2, num3, 0);
				Graphics.Blit(renderTexture, temporary2, this.sunShaftsMaterial, 1);
				RenderTexture.ReleaseTemporary(renderTexture);
				num4 = this.sunShaftBlurRadius * (((float)i * 2f + 1f) * 6f) / 768f;
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num4, num4, 0f, 0f));
				renderTexture = RenderTexture.GetTemporary(num2, num3, 0);
				Graphics.Blit(temporary2, renderTexture, this.sunShaftsMaterial, 1);
				RenderTexture.ReleaseTemporary(temporary2);
				num4 = this.sunShaftBlurRadius * (((float)i * 2f + 2f) * 6f) / 768f;
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num4, num4, 0f, 0f));
			}
			if (vector.z >= 0f)
			{
				this.sunShaftsMaterial.SetVector("_SunColor", new Vector4(this.sunColor.r, this.sunColor.g, this.sunColor.b, this.sunColor.a) * this.sunShaftIntensity);
			}
			else
			{
				this.sunShaftsMaterial.SetVector("_SunColor", Vector4.zero);
			}
			this.sunShaftsMaterial.SetTexture("_ColorBuffer", renderTexture);
			Graphics.Blit(source, destination, this.sunShaftsMaterial, (this.screenBlendMode == SunShafts.ShaftsScreenBlendMode.Screen) ? 0 : 4);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x040026C5 RID: 9925
		public SunShafts.SunShaftsResolution resolution = SunShafts.SunShaftsResolution.Normal;

		// Token: 0x040026C6 RID: 9926
		public SunShafts.ShaftsScreenBlendMode screenBlendMode;

		// Token: 0x040026C7 RID: 9927
		public Transform sunTransform;

		// Token: 0x040026C8 RID: 9928
		public int radialBlurIterations = 2;

		// Token: 0x040026C9 RID: 9929
		public Color sunColor = Color.white;

		// Token: 0x040026CA RID: 9930
		public Color sunThreshold = new Color(0.87f, 0.74f, 0.65f);

		// Token: 0x040026CB RID: 9931
		public float sunShaftBlurRadius = 2.5f;

		// Token: 0x040026CC RID: 9932
		public float sunShaftIntensity = 1.15f;

		// Token: 0x040026CD RID: 9933
		public float maxRadius = 0.75f;

		// Token: 0x040026CE RID: 9934
		public bool useDepthTexture = true;

		// Token: 0x040026CF RID: 9935
		public Shader sunShaftsShader;

		// Token: 0x040026D0 RID: 9936
		private Material sunShaftsMaterial;

		// Token: 0x040026D1 RID: 9937
		public Shader simpleClearShader;

		// Token: 0x040026D2 RID: 9938
		private Material simpleClearMaterial;

		// Token: 0x020006D7 RID: 1751
		public enum SunShaftsResolution
		{
			// Token: 0x040026D4 RID: 9940
			Low,
			// Token: 0x040026D5 RID: 9941
			Normal,
			// Token: 0x040026D6 RID: 9942
			High
		}

		// Token: 0x020006D8 RID: 1752
		public enum ShaftsScreenBlendMode
		{
			// Token: 0x040026D8 RID: 9944
			Screen,
			// Token: 0x040026D9 RID: 9945
			Add
		}
	}
}

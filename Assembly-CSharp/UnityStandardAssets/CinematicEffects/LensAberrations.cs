using System;
using UnityEngine;

namespace UnityStandardAssets.CinematicEffects
{
	// Token: 0x020006E9 RID: 1769
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Cinematic/Lens Aberrations")]
	public class LensAberrations : MonoBehaviour
	{
		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06003588 RID: 13704 RVA: 0x0004A105 File Offset: 0x00048305
		public Shader shader
		{
			get
			{
				if (this.m_Shader == null)
				{
					this.m_Shader = Shader.Find("Hidden/LensAberrations");
				}
				return this.m_Shader;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06003589 RID: 13705 RVA: 0x0004A12B File Offset: 0x0004832B
		public Material material
		{
			get
			{
				if (this.m_Material == null)
				{
					this.m_Material = ImageEffectHelper.CheckShaderAndCreateMaterial(this.shader);
				}
				return this.m_Material;
			}
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x0004A152 File Offset: 0x00048352
		private void OnEnable()
		{
			if (!ImageEffectHelper.IsSupported(this.shader, false, false, this))
			{
				base.enabled = false;
			}
			this.m_RTU = new RenderTextureUtility();
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x0004A176 File Offset: 0x00048376
		private void OnDisable()
		{
			if (this.m_Material != null)
			{
				global::UnityEngine.Object.DestroyImmediate(this.m_Material);
			}
			this.m_Material = null;
			this.m_RTU.ReleaseAllTemporaryRenderTextures();
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x0013FD10 File Offset: 0x0013DF10
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.vignette.enabled && !this.chromaticAberration.enabled && !this.distortion.enabled)
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.material.shaderKeywords = null;
			if (this.distortion.enabled)
			{
				float num = 1.6f * Math.Max(Mathf.Abs(this.distortion.amount), 1f);
				float num2 = 0.017453292f * Math.Min(160f, num);
				float num3 = 2f * Mathf.Tan(num2 * 0.5f);
				Vector4 vector = new Vector4(this.distortion.centerX, this.distortion.centerY, Mathf.Max(this.distortion.amountX, 0.0001f), Mathf.Max(this.distortion.amountY, 0.0001f));
				Vector3 vector2 = new Vector3((this.distortion.amount >= 0f) ? num2 : (1f / num2), num3, 1f / this.distortion.scale);
				this.material.EnableKeyword((this.distortion.amount >= 0f) ? "DISTORT" : "UNDISTORT");
				this.material.SetVector("_DistCenterScale", vector);
				this.material.SetVector("_DistAmount", vector2);
			}
			if (this.chromaticAberration.enabled)
			{
				this.material.EnableKeyword("CHROMATIC_ABERRATION");
				Vector4 vector3 = new Vector4(this.chromaticAberration.color.r, this.chromaticAberration.color.g, this.chromaticAberration.color.b, this.chromaticAberration.amount * 0.001f);
				this.material.SetVector("_ChromaticAberration", vector3);
			}
			if (this.vignette.enabled)
			{
				this.material.SetColor("_VignetteColor", this.vignette.color);
				if (this.vignette.blur > 0f)
				{
					int num4 = source.width / 2;
					int num5 = source.height / 2;
					RenderTexture temporaryRenderTexture = this.m_RTU.GetTemporaryRenderTexture(num4, num5, 0, source.format, FilterMode.Bilinear);
					RenderTexture temporaryRenderTexture2 = this.m_RTU.GetTemporaryRenderTexture(num4, num5, 0, source.format, FilterMode.Bilinear);
					this.material.SetVector("_BlurPass", new Vector2(1f / (float)num4, 0f));
					Graphics.Blit(source, temporaryRenderTexture, this.material, 0);
					if (this.distortion.enabled)
					{
						this.material.DisableKeyword("DISTORT");
						this.material.DisableKeyword("UNDISTORT");
					}
					this.material.SetVector("_BlurPass", new Vector2(0f, 1f / (float)num5));
					Graphics.Blit(temporaryRenderTexture, temporaryRenderTexture2, this.material, 0);
					this.material.SetVector("_BlurPass", new Vector2(1f / (float)num4, 0f));
					Graphics.Blit(temporaryRenderTexture2, temporaryRenderTexture, this.material, 0);
					this.material.SetVector("_BlurPass", new Vector2(0f, 1f / (float)num5));
					Graphics.Blit(temporaryRenderTexture, temporaryRenderTexture2, this.material, 0);
					this.material.SetTexture("_BlurTex", temporaryRenderTexture2);
					this.material.SetFloat("_VignetteBlur", this.vignette.blur * 3f);
					this.material.EnableKeyword("VIGNETTE_BLUR");
					if (this.distortion.enabled)
					{
						this.material.EnableKeyword((this.distortion.amount >= 0f) ? "DISTORT" : "UNDISTORT");
					}
				}
				if (this.vignette.desaturate > 0f)
				{
					this.material.EnableKeyword("VIGNETTE_DESAT");
					this.material.SetFloat("_VignetteDesat", 1f - this.vignette.desaturate);
				}
				this.material.SetVector("_VignetteCenter", this.vignette.center);
				if (Mathf.Approximately(this.vignette.roundness, 1f))
				{
					this.material.EnableKeyword("VIGNETTE_CLASSIC");
					this.material.SetVector("_VignetteSettings", new Vector2(this.vignette.intensity, this.vignette.smoothness));
				}
				else
				{
					this.material.EnableKeyword("VIGNETTE_FILMIC");
					float num6 = (1f - this.vignette.roundness) * 6f + this.vignette.roundness;
					this.material.SetVector("_VignetteSettings", new Vector3(this.vignette.intensity, this.vignette.smoothness, num6));
				}
			}
			int num7 = 0;
			if (this.vignette.enabled && this.chromaticAberration.enabled && this.distortion.enabled)
			{
				num7 = 7;
			}
			else if (this.vignette.enabled && this.chromaticAberration.enabled)
			{
				num7 = 5;
			}
			else if (this.vignette.enabled && this.distortion.enabled)
			{
				num7 = 6;
			}
			else if (this.chromaticAberration.enabled && this.distortion.enabled)
			{
				num7 = 4;
			}
			else if (this.vignette.enabled)
			{
				num7 = 3;
			}
			else if (this.chromaticAberration.enabled)
			{
				num7 = 1;
			}
			else if (this.distortion.enabled)
			{
				num7 = 2;
			}
			Graphics.Blit(source, destination, this.material, num7);
			this.m_RTU.ReleaseAllTemporaryRenderTextures();
		}

		// Token: 0x0400272D RID: 10029
		[LensAberrations.SettingsGroup]
		public LensAberrations.DistortionSettings distortion = LensAberrations.DistortionSettings.defaultSettings;

		// Token: 0x0400272E RID: 10030
		[LensAberrations.SettingsGroup]
		public LensAberrations.VignetteSettings vignette = LensAberrations.VignetteSettings.defaultSettings;

		// Token: 0x0400272F RID: 10031
		[LensAberrations.SettingsGroup]
		public LensAberrations.ChromaticAberrationSettings chromaticAberration = LensAberrations.ChromaticAberrationSettings.defaultSettings;

		// Token: 0x04002730 RID: 10032
		[SerializeField]
		private Shader m_Shader;

		// Token: 0x04002731 RID: 10033
		private Material m_Material;

		// Token: 0x04002732 RID: 10034
		private RenderTextureUtility m_RTU;

		// Token: 0x020006EA RID: 1770
		[AttributeUsage(AttributeTargets.Field)]
		public class SettingsGroup : Attribute
		{
		}

		// Token: 0x020006EB RID: 1771
		[AttributeUsage(AttributeTargets.Field)]
		public class AdvancedSetting : Attribute
		{
		}

		// Token: 0x020006EC RID: 1772
		[Serializable]
		public struct DistortionSettings
		{
			// Token: 0x170003ED RID: 1005
			// (get) Token: 0x06003590 RID: 13712 RVA: 0x001402F8 File Offset: 0x0013E4F8
			public static LensAberrations.DistortionSettings defaultSettings
			{
				get
				{
					return new LensAberrations.DistortionSettings
					{
						enabled = false,
						amount = 0f,
						centerX = 0f,
						centerY = 0f,
						amountX = 1f,
						amountY = 1f,
						scale = 1f
					};
				}
			}

			// Token: 0x04002733 RID: 10035
			public bool enabled;

			// Token: 0x04002734 RID: 10036
			[Range(-100f, 100f)]
			[Tooltip("Distortion amount.")]
			public float amount;

			// Token: 0x04002735 RID: 10037
			[Range(-1f, 1f)]
			[Tooltip("Distortion center point (X axis).")]
			public float centerX;

			// Token: 0x04002736 RID: 10038
			[Range(-1f, 1f)]
			[Tooltip("Distortion center point (Y axis).")]
			public float centerY;

			// Token: 0x04002737 RID: 10039
			[Range(0f, 1f)]
			[Tooltip("Amount multiplier on X axis. Set it to 0 to disable distortion on this axis.")]
			public float amountX;

			// Token: 0x04002738 RID: 10040
			[Range(0f, 1f)]
			[Tooltip("Amount multiplier on Y axis. Set it to 0 to disable distortion on this axis.")]
			public float amountY;

			// Token: 0x04002739 RID: 10041
			[Range(0.01f, 5f)]
			[Tooltip("Global screen scaling.")]
			public float scale;
		}

		// Token: 0x020006ED RID: 1773
		[Serializable]
		public struct VignetteSettings
		{
			// Token: 0x170003EE RID: 1006
			// (get) Token: 0x06003591 RID: 13713 RVA: 0x00140360 File Offset: 0x0013E560
			public static LensAberrations.VignetteSettings defaultSettings
			{
				get
				{
					return new LensAberrations.VignetteSettings
					{
						enabled = false,
						color = new Color(0f, 0f, 0f, 1f),
						center = new Vector2(0.5f, 0.5f),
						intensity = 1.4f,
						smoothness = 0.8f,
						roundness = 1f,
						blur = 0f,
						desaturate = 0f
					};
				}
			}

			// Token: 0x0400273A RID: 10042
			public bool enabled;

			// Token: 0x0400273B RID: 10043
			[ColorUsage(false)]
			[Tooltip("Vignette color. Use the alpha channel for transparency.")]
			public Color color;

			// Token: 0x0400273C RID: 10044
			[Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
			public Vector2 center;

			// Token: 0x0400273D RID: 10045
			[Range(0f, 3f)]
			[Tooltip("Amount of vignetting on screen.")]
			public float intensity;

			// Token: 0x0400273E RID: 10046
			[Range(0.01f, 3f)]
			[Tooltip("Smoothness of the vignette borders.")]
			public float smoothness;

			// Token: 0x0400273F RID: 10047
			[LensAberrations.AdvancedSetting]
			[Range(0f, 1f)]
			[Tooltip("Lower values will make a square-ish vignette.")]
			public float roundness;

			// Token: 0x04002740 RID: 10048
			[Range(0f, 1f)]
			[Tooltip("Blurs the corners of the screen. Leave this at 0 to disable it.")]
			public float blur;

			// Token: 0x04002741 RID: 10049
			[Range(0f, 1f)]
			[Tooltip("Desaturate the corners of the screen. Leave this to 0 to disable it.")]
			public float desaturate;
		}

		// Token: 0x020006EE RID: 1774
		[Serializable]
		public struct ChromaticAberrationSettings
		{
			// Token: 0x170003EF RID: 1007
			// (get) Token: 0x06003592 RID: 13714 RVA: 0x001403F0 File Offset: 0x0013E5F0
			public static LensAberrations.ChromaticAberrationSettings defaultSettings
			{
				get
				{
					return new LensAberrations.ChromaticAberrationSettings
					{
						enabled = false,
						color = Color.green,
						amount = 0f
					};
				}
			}

			// Token: 0x04002742 RID: 10050
			public bool enabled;

			// Token: 0x04002743 RID: 10051
			[ColorUsage(false)]
			[Tooltip("Channels to apply chromatic aberration to.")]
			public Color color;

			// Token: 0x04002744 RID: 10052
			[Range(-50f, 50f)]
			[Tooltip("Amount of tangential distortion.")]
			public float amount;
		}

		// Token: 0x020006EF RID: 1775
		private enum Pass
		{
			// Token: 0x04002746 RID: 10054
			BlurPrePass,
			// Token: 0x04002747 RID: 10055
			Chroma,
			// Token: 0x04002748 RID: 10056
			Distort,
			// Token: 0x04002749 RID: 10057
			Vignette,
			// Token: 0x0400274A RID: 10058
			ChromaDistort,
			// Token: 0x0400274B RID: 10059
			ChromaVignette,
			// Token: 0x0400274C RID: 10060
			DistortVignette,
			// Token: 0x0400274D RID: 10061
			ChromaDistortVignette
		}
	}
}

using System;
using UnityEngine;

namespace UnityStandardAssets.CinematicEffects
{
	// Token: 0x020006E4 RID: 1764
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Cinematic/Bloom")]
	[ImageEffectAllowedInSceneView]
	public class Bloom : MonoBehaviour
	{
		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06003575 RID: 13685 RVA: 0x00049FC3 File Offset: 0x000481C3
		public Shader shader
		{
			get
			{
				if (this.m_Shader == null)
				{
					this.m_Shader = Shader.Find("Hidden/Image Effects/Cinematic/Bloom");
				}
				return this.m_Shader;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06003576 RID: 13686 RVA: 0x00049FE9 File Offset: 0x000481E9
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

		// Token: 0x06003577 RID: 13687 RVA: 0x0004A010 File Offset: 0x00048210
		private void OnEnable()
		{
			if (!ImageEffectHelper.IsSupported(this.shader, true, false, this))
			{
				base.enabled = false;
			}
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x0004A029 File Offset: 0x00048229
		private void OnDisable()
		{
			if (this.m_Material != null)
			{
				global::UnityEngine.Object.DestroyImmediate(this.m_Material);
			}
			this.m_Material = null;
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x0013F868 File Offset: 0x0013DA68
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			bool isMobilePlatform = Application.isMobilePlatform;
			int num = source.width;
			int num2 = source.height;
			if (!this.settings.highQuality)
			{
				num /= 2;
				num2 /= 2;
			}
			RenderTextureFormat renderTextureFormat = (isMobilePlatform ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);
			float num3 = Mathf.Log((float)num2, 2f) + this.settings.radius - 8f;
			int num4 = (int)num3;
			int num5 = Mathf.Clamp(num4, 1, 16);
			float thresholdLinear = this.settings.thresholdLinear;
			this.material.SetFloat("_Threshold", thresholdLinear);
			float num6 = thresholdLinear * this.settings.softKnee + 1E-05f;
			Vector3 vector = new Vector3(thresholdLinear - num6, num6 * 2f, 0.25f / num6);
			this.material.SetVector("_Curve", vector);
			bool flag = !this.settings.highQuality && this.settings.antiFlicker;
			this.material.SetFloat("_PrefilterOffs", flag ? (-0.5f) : 0f);
			this.material.SetFloat("_SampleScale", 0.5f + num3 - (float)num4);
			this.material.SetFloat("_Intensity", Mathf.Max(0f, this.settings.intensity));
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 0, renderTextureFormat);
			Graphics.Blit(source, temporary, this.material, this.settings.antiFlicker ? 1 : 0);
			RenderTexture renderTexture = temporary;
			for (int i = 0; i < num5; i++)
			{
				this.m_blurBuffer1[i] = RenderTexture.GetTemporary(renderTexture.width / 2, renderTexture.height / 2, 0, renderTextureFormat);
				Graphics.Blit(renderTexture, this.m_blurBuffer1[i], this.material, (i == 0) ? (this.settings.antiFlicker ? 3 : 2) : 4);
				renderTexture = this.m_blurBuffer1[i];
			}
			for (int j = num5 - 2; j >= 0; j--)
			{
				RenderTexture renderTexture2 = this.m_blurBuffer1[j];
				this.material.SetTexture("_BaseTex", renderTexture2);
				this.m_blurBuffer2[j] = RenderTexture.GetTemporary(renderTexture2.width, renderTexture2.height, 0, renderTextureFormat);
				Graphics.Blit(renderTexture, this.m_blurBuffer2[j], this.material, this.settings.highQuality ? 6 : 5);
				renderTexture = this.m_blurBuffer2[j];
			}
			this.material.SetTexture("_BaseTex", source);
			Graphics.Blit(renderTexture, destination, this.material, this.settings.highQuality ? 8 : 7);
			for (int k = 0; k < 16; k++)
			{
				if (this.m_blurBuffer1[k] != null)
				{
					RenderTexture.ReleaseTemporary(this.m_blurBuffer1[k]);
				}
				if (this.m_blurBuffer2[k] != null)
				{
					RenderTexture.ReleaseTemporary(this.m_blurBuffer2[k]);
				}
				this.m_blurBuffer1[k] = null;
				this.m_blurBuffer2[k] = null;
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x0400271F RID: 10015
		[SerializeField]
		public Bloom.Settings settings = Bloom.Settings.defaultSettings;

		// Token: 0x04002720 RID: 10016
		[SerializeField]
		[HideInInspector]
		private Shader m_Shader;

		// Token: 0x04002721 RID: 10017
		private Material m_Material;

		// Token: 0x04002722 RID: 10018
		private const int kMaxIterations = 16;

		// Token: 0x04002723 RID: 10019
		private RenderTexture[] m_blurBuffer1 = new RenderTexture[16];

		// Token: 0x04002724 RID: 10020
		private RenderTexture[] m_blurBuffer2 = new RenderTexture[16];

		// Token: 0x020006E5 RID: 1765
		[Serializable]
		public struct Settings
		{
			// Token: 0x170003E7 RID: 999
			// (get) Token: 0x0600357C RID: 13692 RVA: 0x0004A081 File Offset: 0x00048281
			// (set) Token: 0x0600357B RID: 13691 RVA: 0x0004A078 File Offset: 0x00048278
			public float thresholdGamma
			{
				get
				{
					return Mathf.Max(0f, this.threshold);
				}
				set
				{
					this.threshold = value;
				}
			}

			// Token: 0x170003E8 RID: 1000
			// (get) Token: 0x0600357E RID: 13694 RVA: 0x0004A0A1 File Offset: 0x000482A1
			// (set) Token: 0x0600357D RID: 13693 RVA: 0x0004A093 File Offset: 0x00048293
			public float thresholdLinear
			{
				get
				{
					return Mathf.GammaToLinearSpace(this.thresholdGamma);
				}
				set
				{
					this.threshold = Mathf.LinearToGammaSpace(value);
				}
			}

			// Token: 0x170003E9 RID: 1001
			// (get) Token: 0x0600357F RID: 13695 RVA: 0x0013FB70 File Offset: 0x0013DD70
			public static Bloom.Settings defaultSettings
			{
				get
				{
					return new Bloom.Settings
					{
						threshold = 0.9f,
						softKnee = 0.5f,
						radius = 2f,
						intensity = 0.7f,
						highQuality = true,
						antiFlicker = false
					};
				}
			}

			// Token: 0x04002725 RID: 10021
			[SerializeField]
			[Tooltip("Filters out pixels under this level of brightness.")]
			public float threshold;

			// Token: 0x04002726 RID: 10022
			[SerializeField]
			[Range(0f, 1f)]
			[Tooltip("Makes transition between under/over-threshold gradual.")]
			public float softKnee;

			// Token: 0x04002727 RID: 10023
			[SerializeField]
			[Range(1f, 7f)]
			[Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
			public float radius;

			// Token: 0x04002728 RID: 10024
			[SerializeField]
			[Tooltip("Blend factor of the result image.")]
			public float intensity;

			// Token: 0x04002729 RID: 10025
			[SerializeField]
			[Tooltip("Controls filter quality and buffer resolution.")]
			public bool highQuality;

			// Token: 0x0400272A RID: 10026
			[SerializeField]
			[Tooltip("Reduces flashing noise with an additional filter.")]
			public bool antiFlicker;
		}
	}
}

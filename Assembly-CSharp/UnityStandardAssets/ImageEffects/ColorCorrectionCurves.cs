using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006B3 RID: 1715
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Curves, Saturation)")]
	public class ColorCorrectionCurves : PostEffectsBase
	{
		// Token: 0x060034CF RID: 13519 RVA: 0x00049705 File Offset: 0x00047905
		private new void Start()
		{
			base.Start();
			this.updateTexturesOnStartup = true;
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void Awake()
		{
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x001390C8 File Offset: 0x001372C8
		public override bool CheckResources()
		{
			base.CheckSupport(this.mode == ColorCorrectionCurves.ColorCorrectionMode.Advanced);
			this.ccMaterial = base.CheckShaderAndCreateMaterial(this.simpleColorCorrectionCurvesShader, this.ccMaterial);
			this.ccDepthMaterial = base.CheckShaderAndCreateMaterial(this.colorCorrectionCurvesShader, this.ccDepthMaterial);
			this.selectiveCcMaterial = base.CheckShaderAndCreateMaterial(this.colorCorrectionSelectiveShader, this.selectiveCcMaterial);
			if (!this.rgbChannelTex)
			{
				this.rgbChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
			}
			if (!this.rgbDepthChannelTex)
			{
				this.rgbDepthChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
			}
			if (!this.zCurveTex)
			{
				this.zCurveTex = new Texture2D(256, 1, TextureFormat.ARGB32, false, true);
			}
			this.rgbChannelTex.hideFlags = HideFlags.DontSave;
			this.rgbDepthChannelTex.hideFlags = HideFlags.DontSave;
			this.zCurveTex.hideFlags = HideFlags.DontSave;
			this.rgbChannelTex.wrapMode = TextureWrapMode.Clamp;
			this.rgbDepthChannelTex.wrapMode = TextureWrapMode.Clamp;
			this.zCurveTex.wrapMode = TextureWrapMode.Clamp;
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x001391F0 File Offset: 0x001373F0
		public void UpdateParameters()
		{
			this.CheckResources();
			if (this.redChannel != null && this.greenChannel != null && this.blueChannel != null)
			{
				for (float num = 0f; num <= 1f; num += 0.003921569f)
				{
					float num2 = Mathf.Clamp(this.redChannel.Evaluate(num), 0f, 1f);
					float num3 = Mathf.Clamp(this.greenChannel.Evaluate(num), 0f, 1f);
					float num4 = Mathf.Clamp(this.blueChannel.Evaluate(num), 0f, 1f);
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
					float num5 = Mathf.Clamp(this.zCurve.Evaluate(num), 0f, 1f);
					this.zCurveTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num5, num5, num5));
					num2 = Mathf.Clamp(this.depthRedChannel.Evaluate(num), 0f, 1f);
					num3 = Mathf.Clamp(this.depthGreenChannel.Evaluate(num), 0f, 1f);
					num4 = Mathf.Clamp(this.depthBlueChannel.Evaluate(num), 0f, 1f);
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
				}
				this.rgbChannelTex.Apply();
				this.rgbDepthChannelTex.Apply();
				this.zCurveTex.Apply();
			}
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x00049714 File Offset: 0x00047914
		private void UpdateTextures()
		{
			this.UpdateParameters();
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x00139414 File Offset: 0x00137614
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.updateTexturesOnStartup)
			{
				this.UpdateParameters();
				this.updateTexturesOnStartup = false;
			}
			if (this.useDepthCorrection)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			RenderTexture renderTexture = destination;
			if (this.selectiveCc)
			{
				renderTexture = RenderTexture.GetTemporary(source.width, source.height);
			}
			if (this.useDepthCorrection)
			{
				this.ccDepthMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
				this.ccDepthMaterial.SetTexture("_ZCurve", this.zCurveTex);
				this.ccDepthMaterial.SetTexture("_RgbDepthTex", this.rgbDepthChannelTex);
				this.ccDepthMaterial.SetFloat("_Saturation", this.saturation);
				Graphics.Blit(source, renderTexture, this.ccDepthMaterial);
			}
			else
			{
				this.ccMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
				this.ccMaterial.SetFloat("_Saturation", this.saturation);
				Graphics.Blit(source, renderTexture, this.ccMaterial);
			}
			if (this.selectiveCc)
			{
				this.selectiveCcMaterial.SetColor("selColor", this.selectiveFromColor);
				this.selectiveCcMaterial.SetColor("targetColor", this.selectiveToColor);
				Graphics.Blit(renderTexture, destination, this.selectiveCcMaterial);
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}

		// Token: 0x040025B4 RID: 9652
		public AnimationCurve redChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040025B5 RID: 9653
		public AnimationCurve greenChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040025B6 RID: 9654
		public AnimationCurve blueChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040025B7 RID: 9655
		public bool useDepthCorrection;

		// Token: 0x040025B8 RID: 9656
		public AnimationCurve zCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040025B9 RID: 9657
		public AnimationCurve depthRedChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040025BA RID: 9658
		public AnimationCurve depthGreenChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040025BB RID: 9659
		public AnimationCurve depthBlueChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040025BC RID: 9660
		private Material ccMaterial;

		// Token: 0x040025BD RID: 9661
		private Material ccDepthMaterial;

		// Token: 0x040025BE RID: 9662
		private Material selectiveCcMaterial;

		// Token: 0x040025BF RID: 9663
		private Texture2D rgbChannelTex;

		// Token: 0x040025C0 RID: 9664
		private Texture2D rgbDepthChannelTex;

		// Token: 0x040025C1 RID: 9665
		private Texture2D zCurveTex;

		// Token: 0x040025C2 RID: 9666
		public float saturation = 1f;

		// Token: 0x040025C3 RID: 9667
		public bool selectiveCc;

		// Token: 0x040025C4 RID: 9668
		public Color selectiveFromColor = Color.white;

		// Token: 0x040025C5 RID: 9669
		public Color selectiveToColor = Color.white;

		// Token: 0x040025C6 RID: 9670
		public ColorCorrectionCurves.ColorCorrectionMode mode;

		// Token: 0x040025C7 RID: 9671
		public bool updateTextures = true;

		// Token: 0x040025C8 RID: 9672
		public Shader colorCorrectionCurvesShader;

		// Token: 0x040025C9 RID: 9673
		public Shader simpleColorCorrectionCurvesShader;

		// Token: 0x040025CA RID: 9674
		public Shader colorCorrectionSelectiveShader;

		// Token: 0x040025CB RID: 9675
		private bool updateTexturesOnStartup = true;

		// Token: 0x020006B4 RID: 1716
		public enum ColorCorrectionMode
		{
			// Token: 0x040025CD RID: 9677
			Simple,
			// Token: 0x040025CE RID: 9678
			Advanced
		}
	}
}

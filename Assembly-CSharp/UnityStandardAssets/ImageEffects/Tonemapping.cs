using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006DC RID: 1756
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Color Adjustments/Tonemapping")]
	public class Tonemapping : PostEffectsBase
	{
		// Token: 0x06003563 RID: 13667 RVA: 0x0013EC24 File Offset: 0x0013CE24
		public override bool CheckResources()
		{
			base.CheckSupport(false, true);
			this.tonemapMaterial = base.CheckShaderAndCreateMaterial(this.tonemapper, this.tonemapMaterial);
			if (!this.curveTex && this.type == Tonemapping.TonemapperType.UserCurve)
			{
				this.curveTex = new Texture2D(256, 1, TextureFormat.ARGB32, false, true);
				this.curveTex.filterMode = FilterMode.Bilinear;
				this.curveTex.wrapMode = TextureWrapMode.Clamp;
				this.curveTex.hideFlags = HideFlags.DontSave;
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x0013ECB8 File Offset: 0x0013CEB8
		public float UpdateCurve()
		{
			float num = 1f;
			if (this.remapCurve.keys.Length < 1)
			{
				this.remapCurve = new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 0f),
					new Keyframe(2f, 1f)
				});
			}
			if (this.remapCurve != null)
			{
				if (this.remapCurve.length > 0)
				{
					num = this.remapCurve[this.remapCurve.length - 1].time;
				}
				for (float num2 = 0f; num2 <= 1f; num2 += 0.003921569f)
				{
					float num3 = this.remapCurve.Evaluate(num2 * 1f * num);
					this.curveTex.SetPixel((int)Mathf.Floor(num2 * 255f), 0, new Color(num3, num3, num3));
				}
				this.curveTex.Apply();
			}
			return 1f / num;
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x0013EDB4 File Offset: 0x0013CFB4
		private void OnDisable()
		{
			if (this.rt)
			{
				global::UnityEngine.Object.DestroyImmediate(this.rt);
				this.rt = null;
			}
			if (this.tonemapMaterial)
			{
				global::UnityEngine.Object.DestroyImmediate(this.tonemapMaterial);
				this.tonemapMaterial = null;
			}
			if (this.curveTex)
			{
				global::UnityEngine.Object.DestroyImmediate(this.curveTex);
				this.curveTex = null;
			}
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x0013EE20 File Offset: 0x0013D020
		private bool CreateInternalRenderTexture()
		{
			if (this.rt)
			{
				return false;
			}
			this.rtFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf) ? RenderTextureFormat.RGHalf : RenderTextureFormat.ARGBHalf);
			this.rt = new RenderTexture(1, 1, 0, this.rtFormat);
			this.rt.hideFlags = HideFlags.DontSave;
			return true;
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x0013EE74 File Offset: 0x0013D074
		[ImageEffectTransformsToLDR]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.exposureAdjustment = ((this.exposureAdjustment < 0.001f) ? 0.001f : this.exposureAdjustment);
			if (this.type == Tonemapping.TonemapperType.UserCurve)
			{
				float num = this.UpdateCurve();
				this.tonemapMaterial.SetFloat("_RangeScale", num);
				this.tonemapMaterial.SetTexture("_Curve", this.curveTex);
				Graphics.Blit(source, destination, this.tonemapMaterial, 4);
				return;
			}
			if (this.type == Tonemapping.TonemapperType.SimpleReinhard)
			{
				this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
				Graphics.Blit(source, destination, this.tonemapMaterial, 6);
				return;
			}
			if (this.type == Tonemapping.TonemapperType.Hable)
			{
				this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
				Graphics.Blit(source, destination, this.tonemapMaterial, 5);
				return;
			}
			if (this.type == Tonemapping.TonemapperType.Photographic)
			{
				this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
				Graphics.Blit(source, destination, this.tonemapMaterial, 8);
				return;
			}
			if (this.type == Tonemapping.TonemapperType.OptimizedHejiDawson)
			{
				this.tonemapMaterial.SetFloat("_ExposureAdjustment", 0.5f * this.exposureAdjustment);
				Graphics.Blit(source, destination, this.tonemapMaterial, 7);
				return;
			}
			bool flag = this.CreateInternalRenderTexture();
			RenderTexture temporary = RenderTexture.GetTemporary((int)this.adaptiveTextureSize, (int)this.adaptiveTextureSize, 0, this.rtFormat);
			Graphics.Blit(source, temporary);
			int num2 = (int)Mathf.Log((float)temporary.width * 1f, 2f);
			int num3 = 2;
			RenderTexture[] array = new RenderTexture[num2];
			for (int i = 0; i < num2; i++)
			{
				array[i] = RenderTexture.GetTemporary(temporary.width / num3, temporary.width / num3, 0, this.rtFormat);
				num3 *= 2;
			}
			RenderTexture renderTexture = array[num2 - 1];
			Graphics.Blit(temporary, array[0], this.tonemapMaterial, 1);
			if (this.type == Tonemapping.TonemapperType.AdaptiveReinhardAutoWhite)
			{
				for (int j = 0; j < num2 - 1; j++)
				{
					Graphics.Blit(array[j], array[j + 1], this.tonemapMaterial, 9);
					renderTexture = array[j + 1];
				}
			}
			else if (this.type == Tonemapping.TonemapperType.AdaptiveReinhard)
			{
				for (int k = 0; k < num2 - 1; k++)
				{
					Graphics.Blit(array[k], array[k + 1]);
					renderTexture = array[k + 1];
				}
			}
			this.adaptionSpeed = ((this.adaptionSpeed < 0.001f) ? 0.001f : this.adaptionSpeed);
			this.tonemapMaterial.SetFloat("_AdaptionSpeed", this.adaptionSpeed);
			this.rt.MarkRestoreExpected();
			Graphics.Blit(renderTexture, this.rt, this.tonemapMaterial, flag ? 3 : 2);
			this.middleGrey = ((this.middleGrey < 0.001f) ? 0.001f : this.middleGrey);
			this.tonemapMaterial.SetVector("_HdrParams", new Vector4(this.middleGrey, this.middleGrey, this.middleGrey, this.white * this.white));
			this.tonemapMaterial.SetTexture("_SmallTex", this.rt);
			if (this.type == Tonemapping.TonemapperType.AdaptiveReinhard)
			{
				Graphics.Blit(source, destination, this.tonemapMaterial, 0);
			}
			else if (this.type == Tonemapping.TonemapperType.AdaptiveReinhardAutoWhite)
			{
				Graphics.Blit(source, destination, this.tonemapMaterial, 10);
			}
			else
			{
				Debug.LogError("No valid adaptive tonemapper type found!");
				Graphics.Blit(source, destination);
			}
			for (int l = 0; l < num2; l++)
			{
				RenderTexture.ReleaseTemporary(array[l]);
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x040026E9 RID: 9961
		public Tonemapping.TonemapperType type = Tonemapping.TonemapperType.Photographic;

		// Token: 0x040026EA RID: 9962
		public Tonemapping.AdaptiveTexSize adaptiveTextureSize = Tonemapping.AdaptiveTexSize.Square256;

		// Token: 0x040026EB RID: 9963
		public AnimationCurve remapCurve;

		// Token: 0x040026EC RID: 9964
		private Texture2D curveTex;

		// Token: 0x040026ED RID: 9965
		public float exposureAdjustment = 1.5f;

		// Token: 0x040026EE RID: 9966
		public float middleGrey = 0.4f;

		// Token: 0x040026EF RID: 9967
		public float white = 2f;

		// Token: 0x040026F0 RID: 9968
		public float adaptionSpeed = 1.5f;

		// Token: 0x040026F1 RID: 9969
		public Shader tonemapper;

		// Token: 0x040026F2 RID: 9970
		public bool validRenderTextureFormat = true;

		// Token: 0x040026F3 RID: 9971
		private Material tonemapMaterial;

		// Token: 0x040026F4 RID: 9972
		private RenderTexture rt;

		// Token: 0x040026F5 RID: 9973
		private RenderTextureFormat rtFormat = RenderTextureFormat.ARGBHalf;

		// Token: 0x020006DD RID: 1757
		public enum TonemapperType
		{
			// Token: 0x040026F7 RID: 9975
			SimpleReinhard,
			// Token: 0x040026F8 RID: 9976
			UserCurve,
			// Token: 0x040026F9 RID: 9977
			Hable,
			// Token: 0x040026FA RID: 9978
			Photographic,
			// Token: 0x040026FB RID: 9979
			OptimizedHejiDawson,
			// Token: 0x040026FC RID: 9980
			AdaptiveReinhard,
			// Token: 0x040026FD RID: 9981
			AdaptiveReinhardAutoWhite
		}

		// Token: 0x020006DE RID: 1758
		public enum AdaptiveTexSize
		{
			// Token: 0x040026FF RID: 9983
			Square16 = 16,
			// Token: 0x04002700 RID: 9984
			Square32 = 32,
			// Token: 0x04002701 RID: 9985
			Square64 = 64,
			// Token: 0x04002702 RID: 9986
			Square128 = 128,
			// Token: 0x04002703 RID: 9987
			Square256 = 256,
			// Token: 0x04002704 RID: 9988
			Square512 = 512,
			// Token: 0x04002705 RID: 9989
			Square1024 = 1024
		}
	}
}

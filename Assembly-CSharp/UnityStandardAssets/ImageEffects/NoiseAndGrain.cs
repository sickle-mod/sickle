using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006CB RID: 1739
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Noise/Noise And Grain (Filmic)")]
	public class NoiseAndGrain : PostEffectsBase
	{
		// Token: 0x06003527 RID: 13607 RVA: 0x0013C864 File Offset: 0x0013AA64
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.noiseMaterial = base.CheckShaderAndCreateMaterial(this.noiseShader, this.noiseMaterial);
			if (this.dx11Grain && this.supportDX11)
			{
				this.dx11NoiseMaterial = base.CheckShaderAndCreateMaterial(this.dx11NoiseShader, this.dx11NoiseMaterial);
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x0013C8D0 File Offset: 0x0013AAD0
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources() || null == this.noiseTexture)
			{
				Graphics.Blit(source, destination);
				if (null == this.noiseTexture)
				{
					Debug.LogWarning("Noise & Grain effect failing as noise texture is not assigned. please assign.", base.transform);
				}
				return;
			}
			this.softness = Mathf.Clamp(this.softness, 0f, 0.99f);
			if (this.dx11Grain && this.supportDX11)
			{
				this.dx11NoiseMaterial.SetFloat("_DX11NoiseTime", (float)Time.frameCount);
				this.dx11NoiseMaterial.SetTexture("_NoiseTex", this.noiseTexture);
				this.dx11NoiseMaterial.SetVector("_NoisePerChannel", this.monochrome ? Vector3.one : this.intensities);
				this.dx11NoiseMaterial.SetVector("_MidGrey", new Vector3(this.midGrey, 1f / (1f - this.midGrey), -1f / this.midGrey));
				this.dx11NoiseMaterial.SetVector("_NoiseAmount", new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier);
				if (this.softness > Mathf.Epsilon)
				{
					RenderTexture temporary = RenderTexture.GetTemporary((int)((float)source.width * (1f - this.softness)), (int)((float)source.height * (1f - this.softness)));
					NoiseAndGrain.DrawNoiseQuadGrid(source, temporary, this.dx11NoiseMaterial, this.noiseTexture, this.monochrome ? 3 : 2);
					this.dx11NoiseMaterial.SetTexture("_NoiseTex", temporary);
					Graphics.Blit(source, destination, this.dx11NoiseMaterial, 4);
					RenderTexture.ReleaseTemporary(temporary);
					return;
				}
				NoiseAndGrain.DrawNoiseQuadGrid(source, destination, this.dx11NoiseMaterial, this.noiseTexture, this.monochrome ? 1 : 0);
				return;
			}
			else
			{
				if (this.noiseTexture)
				{
					this.noiseTexture.wrapMode = TextureWrapMode.Repeat;
					this.noiseTexture.filterMode = this.filterMode;
				}
				this.noiseMaterial.SetTexture("_NoiseTex", this.noiseTexture);
				this.noiseMaterial.SetVector("_NoisePerChannel", this.monochrome ? Vector3.one : this.intensities);
				this.noiseMaterial.SetVector("_NoiseTilingPerChannel", this.monochrome ? (Vector3.one * this.monochromeTiling) : this.tiling);
				this.noiseMaterial.SetVector("_MidGrey", new Vector3(this.midGrey, 1f / (1f - this.midGrey), -1f / this.midGrey));
				this.noiseMaterial.SetVector("_NoiseAmount", new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier);
				if (this.softness > Mathf.Epsilon)
				{
					RenderTexture temporary2 = RenderTexture.GetTemporary((int)((float)source.width * (1f - this.softness)), (int)((float)source.height * (1f - this.softness)));
					NoiseAndGrain.DrawNoiseQuadGrid(source, temporary2, this.noiseMaterial, this.noiseTexture, 2);
					this.noiseMaterial.SetTexture("_NoiseTex", temporary2);
					Graphics.Blit(source, destination, this.noiseMaterial, 1);
					RenderTexture.ReleaseTemporary(temporary2);
					return;
				}
				NoiseAndGrain.DrawNoiseQuadGrid(source, destination, this.noiseMaterial, this.noiseTexture, 0);
				return;
			}
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x0013CC58 File Offset: 0x0013AE58
		private static void DrawNoiseQuadGrid(RenderTexture source, RenderTexture dest, Material fxMaterial, Texture2D noise, int passNr)
		{
			RenderTexture.active = dest;
			float num = (float)noise.width * 1f;
			float num2 = 1f * (float)source.width / NoiseAndGrain.TILE_AMOUNT;
			fxMaterial.SetTexture("_MainTex", source);
			GL.PushMatrix();
			GL.LoadOrtho();
			float num3 = 1f * (float)source.width / (1f * (float)source.height);
			float num4 = 1f / num2;
			float num5 = num4 * num3;
			float num6 = num / ((float)noise.width * 1f);
			fxMaterial.SetPass(passNr);
			GL.Begin(7);
			for (float num7 = 0f; num7 < 1f; num7 += num4)
			{
				for (float num8 = 0f; num8 < 1f; num8 += num5)
				{
					float num9 = global::UnityEngine.Random.Range(0f, 1f);
					float num10 = global::UnityEngine.Random.Range(0f, 1f);
					num9 = Mathf.Floor(num9 * num) / num;
					num10 = Mathf.Floor(num10 * num) / num;
					float num11 = 1f / num;
					GL.MultiTexCoord2(0, num9, num10);
					GL.MultiTexCoord2(1, 0f, 0f);
					GL.Vertex3(num7, num8, 0.1f);
					GL.MultiTexCoord2(0, num9 + num6 * num11, num10);
					GL.MultiTexCoord2(1, 1f, 0f);
					GL.Vertex3(num7 + num4, num8, 0.1f);
					GL.MultiTexCoord2(0, num9 + num6 * num11, num10 + num6 * num11);
					GL.MultiTexCoord2(1, 1f, 1f);
					GL.Vertex3(num7 + num4, num8 + num5, 0.1f);
					GL.MultiTexCoord2(0, num9, num10 + num6 * num11);
					GL.MultiTexCoord2(1, 0f, 1f);
					GL.Vertex3(num7, num8 + num5, 0.1f);
				}
			}
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x04002679 RID: 9849
		public float intensityMultiplier = 0.25f;

		// Token: 0x0400267A RID: 9850
		public float generalIntensity = 0.5f;

		// Token: 0x0400267B RID: 9851
		public float blackIntensity = 1f;

		// Token: 0x0400267C RID: 9852
		public float whiteIntensity = 1f;

		// Token: 0x0400267D RID: 9853
		public float midGrey = 0.2f;

		// Token: 0x0400267E RID: 9854
		public bool dx11Grain;

		// Token: 0x0400267F RID: 9855
		public float softness;

		// Token: 0x04002680 RID: 9856
		public bool monochrome;

		// Token: 0x04002681 RID: 9857
		public Vector3 intensities = new Vector3(1f, 1f, 1f);

		// Token: 0x04002682 RID: 9858
		public Vector3 tiling = new Vector3(64f, 64f, 64f);

		// Token: 0x04002683 RID: 9859
		public float monochromeTiling = 64f;

		// Token: 0x04002684 RID: 9860
		public FilterMode filterMode = FilterMode.Bilinear;

		// Token: 0x04002685 RID: 9861
		public Texture2D noiseTexture;

		// Token: 0x04002686 RID: 9862
		public Shader noiseShader;

		// Token: 0x04002687 RID: 9863
		private Material noiseMaterial;

		// Token: 0x04002688 RID: 9864
		public Shader dx11NoiseShader;

		// Token: 0x04002689 RID: 9865
		private Material dx11NoiseMaterial;

		// Token: 0x0400268A RID: 9866
		private static float TILE_AMOUNT = 64f;
	}
}

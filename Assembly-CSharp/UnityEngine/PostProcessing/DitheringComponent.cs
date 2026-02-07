using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000722 RID: 1826
	public sealed class DitheringComponent : PostProcessingComponentRenderTexture<DitheringModel>
	{
		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060036B8 RID: 14008 RVA: 0x0004B2A3 File Offset: 0x000494A3
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x0004B2C2 File Offset: 0x000494C2
		public override void OnDisable()
		{
			this.noiseTextures = null;
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x001436D0 File Offset: 0x001418D0
		private void LoadNoiseTextures()
		{
			this.noiseTextures = new Texture2D[64];
			for (int i = 0; i < 64; i++)
			{
				this.noiseTextures[i] = Resources.Load<Texture2D>("Bluenoise64/LDR_LLL1_" + i.ToString());
			}
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x00143718 File Offset: 0x00141918
		public override void Prepare(Material uberMaterial)
		{
			int num = this.textureIndex + 1;
			this.textureIndex = num;
			if (num >= 64)
			{
				this.textureIndex = 0;
			}
			float value = Random.value;
			float value2 = Random.value;
			if (this.noiseTextures == null)
			{
				this.LoadNoiseTextures();
			}
			Texture2D texture2D = this.noiseTextures[this.textureIndex];
			uberMaterial.EnableKeyword("DITHERING");
			uberMaterial.SetTexture(DitheringComponent.Uniforms._DitheringTex, texture2D);
			uberMaterial.SetVector(DitheringComponent.Uniforms._DitheringCoords, new Vector4((float)this.context.width / (float)texture2D.width, (float)this.context.height / (float)texture2D.height, value, value2));
		}

		// Token: 0x0400282D RID: 10285
		private Texture2D[] noiseTextures;

		// Token: 0x0400282E RID: 10286
		private int textureIndex;

		// Token: 0x0400282F RID: 10287
		private const int k_TextureCount = 64;

		// Token: 0x02000723 RID: 1827
		private static class Uniforms
		{
			// Token: 0x04002830 RID: 10288
			internal static readonly int _DitheringTex = Shader.PropertyToID("_DitheringTex");

			// Token: 0x04002831 RID: 10289
			internal static readonly int _DitheringCoords = Shader.PropertyToID("_DitheringCoords");
		}
	}
}

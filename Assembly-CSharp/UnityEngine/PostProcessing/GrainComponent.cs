using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200072A RID: 1834
	public sealed class GrainComponent : PostProcessingComponentRenderTexture<GrainModel>
	{
		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060036D2 RID: 14034 RVA: 0x0004B3DC File Offset: 0x000495DC
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) && !this.context.interrupted;
			}
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x0004B41A File Offset: 0x0004961A
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_GrainLookupRT);
			this.m_GrainLookupRT = null;
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x00143F8C File Offset: 0x0014218C
		public override void Prepare(Material uberMaterial)
		{
			GrainModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("GRAIN");
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float value = Random.value;
			float value2 = Random.value;
			if (this.m_GrainLookupRT == null || !this.m_GrainLookupRT.IsCreated())
			{
				GraphicsUtils.Destroy(this.m_GrainLookupRT);
				this.m_GrainLookupRT = new RenderTexture(192, 192, 0, RenderTextureFormat.ARGBHalf)
				{
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Repeat,
					anisoLevel = 0,
					name = "Grain Lookup Texture"
				};
				this.m_GrainLookupRT.Create();
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Grain Generator");
			material.SetFloat(GrainComponent.Uniforms._Phase, realtimeSinceStartup / 20f);
			Graphics.Blit(null, this.m_GrainLookupRT, material, settings.colored ? 1 : 0);
			uberMaterial.SetTexture(GrainComponent.Uniforms._GrainTex, this.m_GrainLookupRT);
			uberMaterial.SetVector(GrainComponent.Uniforms._Grain_Params1, new Vector2(settings.luminanceContribution, settings.intensity * 20f));
			uberMaterial.SetVector(GrainComponent.Uniforms._Grain_Params2, new Vector4((float)this.context.width / (float)this.m_GrainLookupRT.width / settings.size, (float)this.context.height / (float)this.m_GrainLookupRT.height / settings.size, value, value2));
		}

		// Token: 0x0400284B RID: 10315
		private RenderTexture m_GrainLookupRT;

		// Token: 0x0200072B RID: 1835
		private static class Uniforms
		{
			// Token: 0x0400284C RID: 10316
			internal static readonly int _Grain_Params1 = Shader.PropertyToID("_Grain_Params1");

			// Token: 0x0400284D RID: 10317
			internal static readonly int _Grain_Params2 = Shader.PropertyToID("_Grain_Params2");

			// Token: 0x0400284E RID: 10318
			internal static readonly int _GrainTex = Shader.PropertyToID("_GrainTex");

			// Token: 0x0400284F RID: 10319
			internal static readonly int _Phase = Shader.PropertyToID("_Phase");
		}
	}
}

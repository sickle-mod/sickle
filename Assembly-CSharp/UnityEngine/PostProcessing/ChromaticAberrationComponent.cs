using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200071C RID: 1820
	public sealed class ChromaticAberrationComponent : PostProcessingComponentRenderTexture<ChromaticAberrationModel>
	{
		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06003691 RID: 13969 RVA: 0x0004B05D File Offset: 0x0004925D
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && !this.context.interrupted;
			}
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x0004B093 File Offset: 0x00049293
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_SpectrumLut);
			this.m_SpectrumLut = null;
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x001423AC File Offset: 0x001405AC
		public override void Prepare(Material uberMaterial)
		{
			ChromaticAberrationModel.Settings settings = base.model.settings;
			Texture2D texture2D = settings.spectralTexture;
			if (texture2D == null)
			{
				if (this.m_SpectrumLut == null)
				{
					this.m_SpectrumLut = new Texture2D(3, 1, TextureFormat.RGB24, false)
					{
						name = "Chromatic Aberration Spectrum Lookup",
						filterMode = FilterMode.Bilinear,
						wrapMode = TextureWrapMode.Clamp,
						anisoLevel = 0,
						hideFlags = HideFlags.DontSave
					};
					Color[] array = new Color[]
					{
						new Color(1f, 0f, 0f),
						new Color(0f, 1f, 0f),
						new Color(0f, 0f, 1f)
					};
					this.m_SpectrumLut.SetPixels(array);
					this.m_SpectrumLut.Apply();
				}
				texture2D = this.m_SpectrumLut;
			}
			uberMaterial.EnableKeyword("CHROMATIC_ABERRATION");
			uberMaterial.SetFloat(ChromaticAberrationComponent.Uniforms._ChromaticAberration_Amount, settings.intensity * 0.03f);
			uberMaterial.SetTexture(ChromaticAberrationComponent.Uniforms._ChromaticAberration_Spectrum, texture2D);
		}

		// Token: 0x04002803 RID: 10243
		private Texture2D m_SpectrumLut;

		// Token: 0x0200071D RID: 1821
		private static class Uniforms
		{
			// Token: 0x04002804 RID: 10244
			internal static readonly int _ChromaticAberration_Amount = Shader.PropertyToID("_ChromaticAberration_Amount");

			// Token: 0x04002805 RID: 10245
			internal static readonly int _ChromaticAberration_Spectrum = Shader.PropertyToID("_ChromaticAberration_Spectrum");
		}
	}
}

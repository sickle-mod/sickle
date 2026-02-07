using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000739 RID: 1849
	public sealed class VignetteComponent : PostProcessingComponentRenderTexture<VignetteModel>
	{
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600370D RID: 14093 RVA: 0x0004B65C File Offset: 0x0004985C
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x00145DF4 File Offset: 0x00143FF4
		public override void Prepare(Material uberMaterial)
		{
			VignetteModel.Settings settings = base.model.settings;
			uberMaterial.SetColor(VignetteComponent.Uniforms._Vignette_Color, settings.color);
			if (settings.mode == VignetteModel.Mode.Classic)
			{
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Center, settings.center);
				uberMaterial.EnableKeyword("VIGNETTE_CLASSIC");
				float num = (1f - settings.roundness) * 6f + settings.roundness;
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Settings, new Vector4(settings.intensity * 3f, settings.smoothness * 5f, num, settings.rounded ? 1f : 0f));
				return;
			}
			if (settings.mode == VignetteModel.Mode.Masked && settings.mask != null && settings.opacity > 0f)
			{
				uberMaterial.EnableKeyword("VIGNETTE_MASKED");
				uberMaterial.SetTexture(VignetteComponent.Uniforms._Vignette_Mask, settings.mask);
				uberMaterial.SetFloat(VignetteComponent.Uniforms._Vignette_Opacity, settings.opacity);
			}
		}

		// Token: 0x0200073A RID: 1850
		private static class Uniforms
		{
			// Token: 0x040028C1 RID: 10433
			internal static readonly int _Vignette_Color = Shader.PropertyToID("_Vignette_Color");

			// Token: 0x040028C2 RID: 10434
			internal static readonly int _Vignette_Center = Shader.PropertyToID("_Vignette_Center");

			// Token: 0x040028C3 RID: 10435
			internal static readonly int _Vignette_Settings = Shader.PropertyToID("_Vignette_Settings");

			// Token: 0x040028C4 RID: 10436
			internal static readonly int _Vignette_Mask = Shader.PropertyToID("_Vignette_Mask");

			// Token: 0x040028C5 RID: 10437
			internal static readonly int _Vignette_Opacity = Shader.PropertyToID("_Vignette_Opacity");
		}
	}
}

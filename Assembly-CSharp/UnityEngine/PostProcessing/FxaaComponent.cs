using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000728 RID: 1832
	public sealed class FxaaComponent : PostProcessingComponentRenderTexture<AntialiasingModel>
	{
		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060036CE RID: 14030 RVA: 0x0004B383 File Offset: 0x00049583
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.method == AntialiasingModel.Method.Fxaa && !this.context.interrupted;
			}
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x00143EDC File Offset: 0x001420DC
		public void Render(RenderTexture source, RenderTexture destination)
		{
			AntialiasingModel.FxaaSettings fxaaSettings = base.model.settings.fxaaSettings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/FXAA");
			AntialiasingModel.FxaaQualitySettings fxaaQualitySettings = AntialiasingModel.FxaaQualitySettings.presets[(int)fxaaSettings.preset];
			AntialiasingModel.FxaaConsoleSettings fxaaConsoleSettings = AntialiasingModel.FxaaConsoleSettings.presets[(int)fxaaSettings.preset];
			material.SetVector(FxaaComponent.Uniforms._QualitySettings, new Vector3(fxaaQualitySettings.subpixelAliasingRemovalAmount, fxaaQualitySettings.edgeDetectionThreshold, fxaaQualitySettings.minimumRequiredLuminance));
			material.SetVector(FxaaComponent.Uniforms._ConsoleSettings, new Vector4(fxaaConsoleSettings.subpixelSpreadAmount, fxaaConsoleSettings.edgeSharpnessAmount, fxaaConsoleSettings.edgeDetectionThreshold, fxaaConsoleSettings.minimumRequiredLuminance));
			Graphics.Blit(source, destination, material, 0);
		}

		// Token: 0x02000729 RID: 1833
		private static class Uniforms
		{
			// Token: 0x04002849 RID: 10313
			internal static readonly int _QualitySettings = Shader.PropertyToID("_QualitySettings");

			// Token: 0x0400284A RID: 10314
			internal static readonly int _ConsoleSettings = Shader.PropertyToID("_ConsoleSettings");
		}
	}
}

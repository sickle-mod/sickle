using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200077D RID: 1917
	public class PostProcessingProfile : ScriptableObject
	{
		// Token: 0x040029B9 RID: 10681
		public BuiltinDebugViewsModel debugViews = new BuiltinDebugViewsModel();

		// Token: 0x040029BA RID: 10682
		public FogModel fog = new FogModel();

		// Token: 0x040029BB RID: 10683
		public AntialiasingModel antialiasing = new AntialiasingModel();

		// Token: 0x040029BC RID: 10684
		public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();

		// Token: 0x040029BD RID: 10685
		public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();

		// Token: 0x040029BE RID: 10686
		public DepthOfFieldModel depthOfField = new DepthOfFieldModel();

		// Token: 0x040029BF RID: 10687
		public MotionBlurModel motionBlur = new MotionBlurModel();

		// Token: 0x040029C0 RID: 10688
		public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();

		// Token: 0x040029C1 RID: 10689
		public BloomModel bloom = new BloomModel();

		// Token: 0x040029C2 RID: 10690
		public ColorGradingModel colorGrading = new ColorGradingModel();

		// Token: 0x040029C3 RID: 10691
		public UserLutModel userLut = new UserLutModel();

		// Token: 0x040029C4 RID: 10692
		public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();

		// Token: 0x040029C5 RID: 10693
		public GrainModel grain = new GrainModel();

		// Token: 0x040029C6 RID: 10694
		public VignetteModel vignette = new VignetteModel();

		// Token: 0x040029C7 RID: 10695
		public DitheringModel dithering = new DitheringModel();
	}
}

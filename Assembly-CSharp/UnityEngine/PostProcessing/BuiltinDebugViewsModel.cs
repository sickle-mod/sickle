using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200074A RID: 1866
	[Serializable]
	public class BuiltinDebugViewsModel : PostProcessingModel
	{
		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06003728 RID: 14120 RVA: 0x0004B731 File Offset: 0x00049931
		// (set) Token: 0x06003729 RID: 14121 RVA: 0x0004B739 File Offset: 0x00049939
		public BuiltinDebugViewsModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x0600372A RID: 14122 RVA: 0x0004B742 File Offset: 0x00049942
		public bool willInterrupt
		{
			get
			{
				return !this.IsModeActive(BuiltinDebugViewsModel.Mode.None) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut);
			}
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x0004B775 File Offset: 0x00049975
		public override void Reset()
		{
			this.settings = BuiltinDebugViewsModel.Settings.defaultSettings;
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x0004B782 File Offset: 0x00049982
		public bool IsModeActive(BuiltinDebugViewsModel.Mode mode)
		{
			return this.m_Settings.mode == mode;
		}

		// Token: 0x040028F8 RID: 10488
		[SerializeField]
		private BuiltinDebugViewsModel.Settings m_Settings = BuiltinDebugViewsModel.Settings.defaultSettings;

		// Token: 0x0200074B RID: 1867
		[Serializable]
		public struct DepthSettings
		{
			// Token: 0x17000423 RID: 1059
			// (get) Token: 0x0600372E RID: 14126 RVA: 0x00146360 File Offset: 0x00144560
			public static BuiltinDebugViewsModel.DepthSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.DepthSettings
					{
						scale = 1f
					};
				}
			}

			// Token: 0x040028F9 RID: 10489
			[Range(0f, 1f)]
			[Tooltip("Scales the camera far plane before displaying the depth map.")]
			public float scale;
		}

		// Token: 0x0200074C RID: 1868
		[Serializable]
		public struct MotionVectorsSettings
		{
			// Token: 0x17000424 RID: 1060
			// (get) Token: 0x0600372F RID: 14127 RVA: 0x00146384 File Offset: 0x00144584
			public static BuiltinDebugViewsModel.MotionVectorsSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.MotionVectorsSettings
					{
						sourceOpacity = 1f,
						motionImageOpacity = 0f,
						motionImageAmplitude = 16f,
						motionVectorsOpacity = 1f,
						motionVectorsResolution = 24,
						motionVectorsAmplitude = 64f
					};
				}
			}

			// Token: 0x040028FA RID: 10490
			[Range(0f, 1f)]
			[Tooltip("Opacity of the source render.")]
			public float sourceOpacity;

			// Token: 0x040028FB RID: 10491
			[Range(0f, 1f)]
			[Tooltip("Opacity of the per-pixel motion vector colors.")]
			public float motionImageOpacity;

			// Token: 0x040028FC RID: 10492
			[Min(0f)]
			[Tooltip("Because motion vectors are mainly very small vectors, you can use this setting to make them more visible.")]
			public float motionImageAmplitude;

			// Token: 0x040028FD RID: 10493
			[Range(0f, 1f)]
			[Tooltip("Opacity for the motion vector arrows.")]
			public float motionVectorsOpacity;

			// Token: 0x040028FE RID: 10494
			[Range(8f, 64f)]
			[Tooltip("The arrow density on screen.")]
			public int motionVectorsResolution;

			// Token: 0x040028FF RID: 10495
			[Min(0f)]
			[Tooltip("Tweaks the arrows length.")]
			public float motionVectorsAmplitude;
		}

		// Token: 0x0200074D RID: 1869
		public enum Mode
		{
			// Token: 0x04002901 RID: 10497
			None,
			// Token: 0x04002902 RID: 10498
			Depth,
			// Token: 0x04002903 RID: 10499
			Normals,
			// Token: 0x04002904 RID: 10500
			MotionVectors,
			// Token: 0x04002905 RID: 10501
			AmbientOcclusion,
			// Token: 0x04002906 RID: 10502
			EyeAdaptation,
			// Token: 0x04002907 RID: 10503
			FocusPlane,
			// Token: 0x04002908 RID: 10504
			PreGradingLog,
			// Token: 0x04002909 RID: 10505
			LogLut,
			// Token: 0x0400290A RID: 10506
			UserLut
		}

		// Token: 0x0200074E RID: 1870
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000425 RID: 1061
			// (get) Token: 0x06003730 RID: 14128 RVA: 0x001463E0 File Offset: 0x001445E0
			public static BuiltinDebugViewsModel.Settings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.Settings
					{
						mode = BuiltinDebugViewsModel.Mode.None,
						depth = BuiltinDebugViewsModel.DepthSettings.defaultSettings,
						motionVectors = BuiltinDebugViewsModel.MotionVectorsSettings.defaultSettings
					};
				}
			}

			// Token: 0x0400290B RID: 10507
			public BuiltinDebugViewsModel.Mode mode;

			// Token: 0x0400290C RID: 10508
			public BuiltinDebugViewsModel.DepthSettings depth;

			// Token: 0x0400290D RID: 10509
			public BuiltinDebugViewsModel.MotionVectorsSettings motionVectors;
		}
	}
}

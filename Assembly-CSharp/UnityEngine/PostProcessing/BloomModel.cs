using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000746 RID: 1862
	[Serializable]
	public class BloomModel : PostProcessingModel
	{
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600371F RID: 14111 RVA: 0x0004B6E5 File Offset: 0x000498E5
		// (set) Token: 0x06003720 RID: 14112 RVA: 0x0004B6ED File Offset: 0x000498ED
		public BloomModel.Settings settings
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

		// Token: 0x06003721 RID: 14113 RVA: 0x0004B6F6 File Offset: 0x000498F6
		public override void Reset()
		{
			this.m_Settings = BloomModel.Settings.defaultSettings;
		}

		// Token: 0x040028EE RID: 10478
		[SerializeField]
		private BloomModel.Settings m_Settings = BloomModel.Settings.defaultSettings;

		// Token: 0x02000747 RID: 1863
		[Serializable]
		public struct BloomSettings
		{
			// Token: 0x1700041D RID: 1053
			// (get) Token: 0x06003724 RID: 14116 RVA: 0x0004B724 File Offset: 0x00049924
			// (set) Token: 0x06003723 RID: 14115 RVA: 0x0004B716 File Offset: 0x00049916
			public float thresholdLinear
			{
				get
				{
					return Mathf.GammaToLinearSpace(this.threshold);
				}
				set
				{
					this.threshold = Mathf.LinearToGammaSpace(value);
				}
			}

			// Token: 0x1700041E RID: 1054
			// (get) Token: 0x06003725 RID: 14117 RVA: 0x001462B4 File Offset: 0x001444B4
			public static BloomModel.BloomSettings defaultSettings
			{
				get
				{
					return new BloomModel.BloomSettings
					{
						intensity = 0.5f,
						threshold = 1.1f,
						softKnee = 0.5f,
						radius = 4f,
						antiFlicker = false
					};
				}
			}

			// Token: 0x040028EF RID: 10479
			[Min(0f)]
			[Tooltip("Strength of the bloom filter.")]
			public float intensity;

			// Token: 0x040028F0 RID: 10480
			[Min(0f)]
			[Tooltip("Filters out pixels under this level of brightness.")]
			public float threshold;

			// Token: 0x040028F1 RID: 10481
			[Range(0f, 1f)]
			[Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")]
			public float softKnee;

			// Token: 0x040028F2 RID: 10482
			[Range(1f, 7f)]
			[Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
			public float radius;

			// Token: 0x040028F3 RID: 10483
			[Tooltip("Reduces flashing noise with an additional filter.")]
			public bool antiFlicker;
		}

		// Token: 0x02000748 RID: 1864
		[Serializable]
		public struct LensDirtSettings
		{
			// Token: 0x1700041F RID: 1055
			// (get) Token: 0x06003726 RID: 14118 RVA: 0x00146304 File Offset: 0x00144504
			public static BloomModel.LensDirtSettings defaultSettings
			{
				get
				{
					return new BloomModel.LensDirtSettings
					{
						texture = null,
						intensity = 3f
					};
				}
			}

			// Token: 0x040028F4 RID: 10484
			[Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
			public Texture texture;

			// Token: 0x040028F5 RID: 10485
			[Min(0f)]
			[Tooltip("Amount of lens dirtiness.")]
			public float intensity;
		}

		// Token: 0x02000749 RID: 1865
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000420 RID: 1056
			// (get) Token: 0x06003727 RID: 14119 RVA: 0x00146330 File Offset: 0x00144530
			public static BloomModel.Settings defaultSettings
			{
				get
				{
					return new BloomModel.Settings
					{
						bloom = BloomModel.BloomSettings.defaultSettings,
						lensDirt = BloomModel.LensDirtSettings.defaultSettings
					};
				}
			}

			// Token: 0x040028F6 RID: 10486
			public BloomModel.BloomSettings bloom;

			// Token: 0x040028F7 RID: 10487
			public BloomModel.LensDirtSettings lensDirt;
		}
	}
}

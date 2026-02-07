using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200073E RID: 1854
	[Serializable]
	public class AntialiasingModel : PostProcessingModel
	{
		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06003716 RID: 14102 RVA: 0x0004B6B4 File Offset: 0x000498B4
		// (set) Token: 0x06003717 RID: 14103 RVA: 0x0004B6BC File Offset: 0x000498BC
		public AntialiasingModel.Settings settings
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

		// Token: 0x06003718 RID: 14104 RVA: 0x0004B6C5 File Offset: 0x000498C5
		public override void Reset()
		{
			this.m_Settings = AntialiasingModel.Settings.defaultSettings;
		}

		// Token: 0x040028D3 RID: 10451
		[SerializeField]
		private AntialiasingModel.Settings m_Settings = AntialiasingModel.Settings.defaultSettings;

		// Token: 0x0200073F RID: 1855
		public enum Method
		{
			// Token: 0x040028D5 RID: 10453
			Fxaa,
			// Token: 0x040028D6 RID: 10454
			Taa
		}

		// Token: 0x02000740 RID: 1856
		public enum FxaaPreset
		{
			// Token: 0x040028D8 RID: 10456
			ExtremePerformance,
			// Token: 0x040028D9 RID: 10457
			Performance,
			// Token: 0x040028DA RID: 10458
			Default,
			// Token: 0x040028DB RID: 10459
			Quality,
			// Token: 0x040028DC RID: 10460
			ExtremeQuality
		}

		// Token: 0x02000741 RID: 1857
		[Serializable]
		public struct FxaaQualitySettings
		{
			// Token: 0x040028DD RID: 10461
			[Tooltip("The amount of desired sub-pixel aliasing removal. Effects the sharpeness of the output.")]
			[Range(0f, 1f)]
			public float subpixelAliasingRemovalAmount;

			// Token: 0x040028DE RID: 10462
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.063f, 0.333f)]
			public float edgeDetectionThreshold;

			// Token: 0x040028DF RID: 10463
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0f, 0.0833f)]
			public float minimumRequiredLuminance;

			// Token: 0x040028E0 RID: 10464
			public static AntialiasingModel.FxaaQualitySettings[] presets = new AntialiasingModel.FxaaQualitySettings[]
			{
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0f,
					edgeDetectionThreshold = 0.333f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.25f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.75f,
					edgeDetectionThreshold = 0.166f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.0625f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.063f,
					minimumRequiredLuminance = 0.0312f
				}
			};
		}

		// Token: 0x02000742 RID: 1858
		[Serializable]
		public struct FxaaConsoleSettings
		{
			// Token: 0x040028E1 RID: 10465
			[Tooltip("The amount of spread applied to the sampling coordinates while sampling for subpixel information.")]
			[Range(0.33f, 0.5f)]
			public float subpixelSpreadAmount;

			// Token: 0x040028E2 RID: 10466
			[Tooltip("This value dictates how sharp the edges in the image are kept; a higher value implies sharper edges.")]
			[Range(2f, 8f)]
			public float edgeSharpnessAmount;

			// Token: 0x040028E3 RID: 10467
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.125f, 0.25f)]
			public float edgeDetectionThreshold;

			// Token: 0x040028E4 RID: 10468
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0.04f, 0.06f)]
			public float minimumRequiredLuminance;

			// Token: 0x040028E5 RID: 10469
			public static AntialiasingModel.FxaaConsoleSettings[] presets = new AntialiasingModel.FxaaConsoleSettings[]
			{
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.05f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 4f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 2f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				}
			};
		}

		// Token: 0x02000743 RID: 1859
		[Serializable]
		public struct FxaaSettings
		{
			// Token: 0x17000419 RID: 1049
			// (get) Token: 0x0600371C RID: 14108 RVA: 0x00146214 File Offset: 0x00144414
			public static AntialiasingModel.FxaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.FxaaSettings
					{
						preset = AntialiasingModel.FxaaPreset.Default
					};
				}
			}

			// Token: 0x040028E6 RID: 10470
			public AntialiasingModel.FxaaPreset preset;
		}

		// Token: 0x02000744 RID: 1860
		[Serializable]
		public struct TaaSettings
		{
			// Token: 0x1700041A RID: 1050
			// (get) Token: 0x0600371D RID: 14109 RVA: 0x00146234 File Offset: 0x00144434
			public static AntialiasingModel.TaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.TaaSettings
					{
						jitterSpread = 0.75f,
						sharpen = 0.3f,
						stationaryBlending = 0.95f,
						motionBlending = 0.85f
					};
				}
			}

			// Token: 0x040028E7 RID: 10471
			[Tooltip("The diameter (in texels) inside which jitter samples are spread. Smaller values result in crisper but more aliased output, while larger values result in more stable but blurrier output.")]
			[Range(0.1f, 1f)]
			public float jitterSpread;

			// Token: 0x040028E8 RID: 10472
			[Tooltip("Controls the amount of sharpening applied to the color buffer.")]
			[Range(0f, 3f)]
			public float sharpen;

			// Token: 0x040028E9 RID: 10473
			[Tooltip("The blend coefficient for a stationary fragment. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float stationaryBlending;

			// Token: 0x040028EA RID: 10474
			[Tooltip("The blend coefficient for a fragment with significant motion. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float motionBlending;
		}

		// Token: 0x02000745 RID: 1861
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700041B RID: 1051
			// (get) Token: 0x0600371E RID: 14110 RVA: 0x0014627C File Offset: 0x0014447C
			public static AntialiasingModel.Settings defaultSettings
			{
				get
				{
					return new AntialiasingModel.Settings
					{
						method = AntialiasingModel.Method.Fxaa,
						fxaaSettings = AntialiasingModel.FxaaSettings.defaultSettings,
						taaSettings = AntialiasingModel.TaaSettings.defaultSettings
					};
				}
			}

			// Token: 0x040028EB RID: 10475
			public AntialiasingModel.Method method;

			// Token: 0x040028EC RID: 10476
			public AntialiasingModel.FxaaSettings fxaaSettings;

			// Token: 0x040028ED RID: 10477
			public AntialiasingModel.TaaSettings taaSettings;
		}
	}
}

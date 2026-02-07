using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000751 RID: 1873
	[Serializable]
	public class ColorGradingModel : PostProcessingModel
	{
		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06003736 RID: 14134 RVA: 0x0004B7D6 File Offset: 0x000499D6
		// (set) Token: 0x06003737 RID: 14135 RVA: 0x0004B7DE File Offset: 0x000499DE
		public ColorGradingModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
				this.OnValidate();
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06003738 RID: 14136 RVA: 0x0004B7ED File Offset: 0x000499ED
		// (set) Token: 0x06003739 RID: 14137 RVA: 0x0004B7F5 File Offset: 0x000499F5
		public bool isDirty { get; internal set; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x0600373A RID: 14138 RVA: 0x0004B7FE File Offset: 0x000499FE
		// (set) Token: 0x0600373B RID: 14139 RVA: 0x0004B806 File Offset: 0x00049A06
		public RenderTexture bakedLut { get; internal set; }

		// Token: 0x0600373C RID: 14140 RVA: 0x0004B80F File Offset: 0x00049A0F
		public override void Reset()
		{
			this.m_Settings = ColorGradingModel.Settings.defaultSettings;
			this.OnValidate();
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x0004B822 File Offset: 0x00049A22
		public override void OnValidate()
		{
			this.isDirty = true;
		}

		// Token: 0x04002911 RID: 10513
		[SerializeField]
		private ColorGradingModel.Settings m_Settings = ColorGradingModel.Settings.defaultSettings;

		// Token: 0x02000752 RID: 1874
		public enum Tonemapper
		{
			// Token: 0x04002915 RID: 10517
			None,
			// Token: 0x04002916 RID: 10518
			ACES,
			// Token: 0x04002917 RID: 10519
			Neutral
		}

		// Token: 0x02000753 RID: 1875
		[Serializable]
		public struct TonemappingSettings
		{
			// Token: 0x1700042B RID: 1067
			// (get) Token: 0x0600373F RID: 14143 RVA: 0x00146444 File Offset: 0x00144644
			public static ColorGradingModel.TonemappingSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.TonemappingSettings
					{
						tonemapper = ColorGradingModel.Tonemapper.Neutral,
						neutralBlackIn = 0.02f,
						neutralWhiteIn = 10f,
						neutralBlackOut = 0f,
						neutralWhiteOut = 10f,
						neutralWhiteLevel = 5.3f,
						neutralWhiteClip = 10f
					};
				}
			}

			// Token: 0x04002918 RID: 10520
			[Tooltip("Tonemapping algorithm to use at the end of the color grading process. Use \"Neutral\" if you need a customizable tonemapper or \"Filmic\" to give a standard filmic look to your scenes.")]
			public ColorGradingModel.Tonemapper tonemapper;

			// Token: 0x04002919 RID: 10521
			[Range(-0.1f, 0.1f)]
			public float neutralBlackIn;

			// Token: 0x0400291A RID: 10522
			[Range(1f, 20f)]
			public float neutralWhiteIn;

			// Token: 0x0400291B RID: 10523
			[Range(-0.09f, 0.1f)]
			public float neutralBlackOut;

			// Token: 0x0400291C RID: 10524
			[Range(1f, 19f)]
			public float neutralWhiteOut;

			// Token: 0x0400291D RID: 10525
			[Range(0.1f, 20f)]
			public float neutralWhiteLevel;

			// Token: 0x0400291E RID: 10526
			[Range(1f, 10f)]
			public float neutralWhiteClip;
		}

		// Token: 0x02000754 RID: 1876
		[Serializable]
		public struct BasicSettings
		{
			// Token: 0x1700042C RID: 1068
			// (get) Token: 0x06003740 RID: 14144 RVA: 0x001464AC File Offset: 0x001446AC
			public static ColorGradingModel.BasicSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.BasicSettings
					{
						postExposure = 0f,
						temperature = 0f,
						tint = 0f,
						hueShift = 0f,
						saturation = 1f,
						contrast = 1f
					};
				}
			}

			// Token: 0x0400291F RID: 10527
			[Tooltip("Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")]
			public float postExposure;

			// Token: 0x04002920 RID: 10528
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to a custom color temperature.")]
			public float temperature;

			// Token: 0x04002921 RID: 10529
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
			public float tint;

			// Token: 0x04002922 RID: 10530
			[Range(-180f, 180f)]
			[Tooltip("Shift the hue of all colors.")]
			public float hueShift;

			// Token: 0x04002923 RID: 10531
			[Range(0f, 2f)]
			[Tooltip("Pushes the intensity of all colors.")]
			public float saturation;

			// Token: 0x04002924 RID: 10532
			[Range(0f, 2f)]
			[Tooltip("Expands or shrinks the overall range of tonal values.")]
			public float contrast;
		}

		// Token: 0x02000755 RID: 1877
		[Serializable]
		public struct ChannelMixerSettings
		{
			// Token: 0x1700042D RID: 1069
			// (get) Token: 0x06003741 RID: 14145 RVA: 0x0014650C File Offset: 0x0014470C
			public static ColorGradingModel.ChannelMixerSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ChannelMixerSettings
					{
						red = new Vector3(1f, 0f, 0f),
						green = new Vector3(0f, 1f, 0f),
						blue = new Vector3(0f, 0f, 1f),
						currentEditingChannel = 0
					};
				}
			}

			// Token: 0x04002925 RID: 10533
			public Vector3 red;

			// Token: 0x04002926 RID: 10534
			public Vector3 green;

			// Token: 0x04002927 RID: 10535
			public Vector3 blue;

			// Token: 0x04002928 RID: 10536
			[HideInInspector]
			public int currentEditingChannel;
		}

		// Token: 0x02000756 RID: 1878
		[Serializable]
		public struct LogWheelsSettings
		{
			// Token: 0x1700042E RID: 1070
			// (get) Token: 0x06003742 RID: 14146 RVA: 0x0014657C File Offset: 0x0014477C
			public static ColorGradingModel.LogWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LogWheelsSettings
					{
						slope = Color.clear,
						power = Color.clear,
						offset = Color.clear
					};
				}
			}

			// Token: 0x04002929 RID: 10537
			[Trackball("GetSlopeValue")]
			public Color slope;

			// Token: 0x0400292A RID: 10538
			[Trackball("GetPowerValue")]
			public Color power;

			// Token: 0x0400292B RID: 10539
			[Trackball("GetOffsetValue")]
			public Color offset;
		}

		// Token: 0x02000757 RID: 1879
		[Serializable]
		public struct LinearWheelsSettings
		{
			// Token: 0x1700042F RID: 1071
			// (get) Token: 0x06003743 RID: 14147 RVA: 0x001465B8 File Offset: 0x001447B8
			public static ColorGradingModel.LinearWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LinearWheelsSettings
					{
						lift = Color.clear,
						gamma = Color.clear,
						gain = Color.clear
					};
				}
			}

			// Token: 0x0400292C RID: 10540
			[Trackball("GetLiftValue")]
			public Color lift;

			// Token: 0x0400292D RID: 10541
			[Trackball("GetGammaValue")]
			public Color gamma;

			// Token: 0x0400292E RID: 10542
			[Trackball("GetGainValue")]
			public Color gain;
		}

		// Token: 0x02000758 RID: 1880
		public enum ColorWheelMode
		{
			// Token: 0x04002930 RID: 10544
			Linear,
			// Token: 0x04002931 RID: 10545
			Log
		}

		// Token: 0x02000759 RID: 1881
		[Serializable]
		public struct ColorWheelsSettings
		{
			// Token: 0x17000430 RID: 1072
			// (get) Token: 0x06003744 RID: 14148 RVA: 0x001465F4 File Offset: 0x001447F4
			public static ColorGradingModel.ColorWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ColorWheelsSettings
					{
						mode = ColorGradingModel.ColorWheelMode.Log,
						log = ColorGradingModel.LogWheelsSettings.defaultSettings,
						linear = ColorGradingModel.LinearWheelsSettings.defaultSettings
					};
				}
			}

			// Token: 0x04002932 RID: 10546
			public ColorGradingModel.ColorWheelMode mode;

			// Token: 0x04002933 RID: 10547
			[TrackballGroup]
			public ColorGradingModel.LogWheelsSettings log;

			// Token: 0x04002934 RID: 10548
			[TrackballGroup]
			public ColorGradingModel.LinearWheelsSettings linear;
		}

		// Token: 0x0200075A RID: 1882
		[Serializable]
		public struct CurvesSettings
		{
			// Token: 0x17000431 RID: 1073
			// (get) Token: 0x06003745 RID: 14149 RVA: 0x0014662C File Offset: 0x0014482C
			public static ColorGradingModel.CurvesSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.CurvesSettings
					{
						master = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						red = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						green = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						blue = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						hueVShue = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						hueVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						satVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						lumVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						e_CurrentEditingCurve = 0,
						e_CurveY = true,
						e_CurveR = false,
						e_CurveG = false,
						e_CurveB = false
					};
				}
			}

			// Token: 0x04002935 RID: 10549
			public ColorGradingCurve master;

			// Token: 0x04002936 RID: 10550
			public ColorGradingCurve red;

			// Token: 0x04002937 RID: 10551
			public ColorGradingCurve green;

			// Token: 0x04002938 RID: 10552
			public ColorGradingCurve blue;

			// Token: 0x04002939 RID: 10553
			public ColorGradingCurve hueVShue;

			// Token: 0x0400293A RID: 10554
			public ColorGradingCurve hueVSsat;

			// Token: 0x0400293B RID: 10555
			public ColorGradingCurve satVSsat;

			// Token: 0x0400293C RID: 10556
			public ColorGradingCurve lumVSsat;

			// Token: 0x0400293D RID: 10557
			[HideInInspector]
			public int e_CurrentEditingCurve;

			// Token: 0x0400293E RID: 10558
			[HideInInspector]
			public bool e_CurveY;

			// Token: 0x0400293F RID: 10559
			[HideInInspector]
			public bool e_CurveR;

			// Token: 0x04002940 RID: 10560
			[HideInInspector]
			public bool e_CurveG;

			// Token: 0x04002941 RID: 10561
			[HideInInspector]
			public bool e_CurveB;
		}

		// Token: 0x0200075B RID: 1883
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000432 RID: 1074
			// (get) Token: 0x06003746 RID: 14150 RVA: 0x001468B4 File Offset: 0x00144AB4
			public static ColorGradingModel.Settings defaultSettings
			{
				get
				{
					return new ColorGradingModel.Settings
					{
						tonemapping = ColorGradingModel.TonemappingSettings.defaultSettings,
						basic = ColorGradingModel.BasicSettings.defaultSettings,
						channelMixer = ColorGradingModel.ChannelMixerSettings.defaultSettings,
						colorWheels = ColorGradingModel.ColorWheelsSettings.defaultSettings,
						curves = ColorGradingModel.CurvesSettings.defaultSettings
					};
				}
			}

			// Token: 0x04002942 RID: 10562
			public ColorGradingModel.TonemappingSettings tonemapping;

			// Token: 0x04002943 RID: 10563
			public ColorGradingModel.BasicSettings basic;

			// Token: 0x04002944 RID: 10564
			public ColorGradingModel.ChannelMixerSettings channelMixer;

			// Token: 0x04002945 RID: 10565
			public ColorGradingModel.ColorWheelsSettings colorWheels;

			// Token: 0x04002946 RID: 10566
			public ColorGradingModel.CurvesSettings curves;
		}
	}
}

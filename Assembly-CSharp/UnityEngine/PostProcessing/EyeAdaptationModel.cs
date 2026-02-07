using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000761 RID: 1889
	[Serializable]
	public class EyeAdaptationModel : PostProcessingModel
	{
		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06003751 RID: 14161 RVA: 0x0004B8A0 File Offset: 0x00049AA0
		// (set) Token: 0x06003752 RID: 14162 RVA: 0x0004B8A8 File Offset: 0x00049AA8
		public EyeAdaptationModel.Settings settings
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

		// Token: 0x06003753 RID: 14163 RVA: 0x0004B8B1 File Offset: 0x00049AB1
		public override void Reset()
		{
			this.m_Settings = EyeAdaptationModel.Settings.defaultSettings;
		}

		// Token: 0x04002953 RID: 10579
		[SerializeField]
		private EyeAdaptationModel.Settings m_Settings = EyeAdaptationModel.Settings.defaultSettings;

		// Token: 0x02000762 RID: 1890
		public enum EyeAdaptationType
		{
			// Token: 0x04002955 RID: 10581
			Progressive,
			// Token: 0x04002956 RID: 10582
			Fixed
		}

		// Token: 0x02000763 RID: 1891
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000438 RID: 1080
			// (get) Token: 0x06003755 RID: 14165 RVA: 0x0014696C File Offset: 0x00144B6C
			public static EyeAdaptationModel.Settings defaultSettings
			{
				get
				{
					return new EyeAdaptationModel.Settings
					{
						lowPercent = 45f,
						highPercent = 95f,
						minLuminance = -5f,
						maxLuminance = 1f,
						keyValue = 0.25f,
						dynamicKeyValue = true,
						adaptationType = EyeAdaptationModel.EyeAdaptationType.Progressive,
						speedUp = 2f,
						speedDown = 1f,
						logMin = -8,
						logMax = 4
					};
				}
			}

			// Token: 0x04002957 RID: 10583
			[Range(1f, 99f)]
			[Tooltip("Filters the dark part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
			public float lowPercent;

			// Token: 0x04002958 RID: 10584
			[Range(1f, 99f)]
			[Tooltip("Filters the bright part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
			public float highPercent;

			// Token: 0x04002959 RID: 10585
			[Tooltip("Minimum average luminance to consider for auto exposure (in EV).")]
			public float minLuminance;

			// Token: 0x0400295A RID: 10586
			[Tooltip("Maximum average luminance to consider for auto exposure (in EV).")]
			public float maxLuminance;

			// Token: 0x0400295B RID: 10587
			[Min(0f)]
			[Tooltip("Exposure bias. Use this to offset the global exposure of the scene.")]
			public float keyValue;

			// Token: 0x0400295C RID: 10588
			[Tooltip("Set this to true to let Unity handle the key value automatically based on average luminance.")]
			public bool dynamicKeyValue;

			// Token: 0x0400295D RID: 10589
			[Tooltip("Use \"Progressive\" if you want the auto exposure to be animated. Use \"Fixed\" otherwise.")]
			public EyeAdaptationModel.EyeAdaptationType adaptationType;

			// Token: 0x0400295E RID: 10590
			[Min(0f)]
			[Tooltip("Adaptation speed from a dark to a light environment.")]
			public float speedUp;

			// Token: 0x0400295F RID: 10591
			[Min(0f)]
			[Tooltip("Adaptation speed from a light to a dark environment.")]
			public float speedDown;

			// Token: 0x04002960 RID: 10592
			[Range(-16f, -1f)]
			[Tooltip("Lower bound for the brightness range of the generated histogram (in EV). The bigger the spread between min & max, the lower the precision will be.")]
			public int logMin;

			// Token: 0x04002961 RID: 10593
			[Range(1f, 16f)]
			[Tooltip("Upper bound for the brightness range of the generated histogram (in EV). The bigger the spread between min & max, the lower the precision will be.")]
			public int logMax;
		}
	}
}

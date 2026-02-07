using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000766 RID: 1894
	[Serializable]
	public class GrainModel : PostProcessingModel
	{
		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x0600375B RID: 14171 RVA: 0x0004B902 File Offset: 0x00049B02
		// (set) Token: 0x0600375C RID: 14172 RVA: 0x0004B90A File Offset: 0x00049B0A
		public GrainModel.Settings settings
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

		// Token: 0x0600375D RID: 14173 RVA: 0x0004B913 File Offset: 0x00049B13
		public override void Reset()
		{
			this.m_Settings = GrainModel.Settings.defaultSettings;
		}

		// Token: 0x04002964 RID: 10596
		[SerializeField]
		private GrainModel.Settings m_Settings = GrainModel.Settings.defaultSettings;

		// Token: 0x02000767 RID: 1895
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700043C RID: 1084
			// (get) Token: 0x0600375F RID: 14175 RVA: 0x00146A18 File Offset: 0x00144C18
			public static GrainModel.Settings defaultSettings
			{
				get
				{
					return new GrainModel.Settings
					{
						colored = true,
						intensity = 0.5f,
						size = 1f,
						luminanceContribution = 0.8f
					};
				}
			}

			// Token: 0x04002965 RID: 10597
			[Tooltip("Enable the use of colored grain.")]
			public bool colored;

			// Token: 0x04002966 RID: 10598
			[Range(0f, 1f)]
			[Tooltip("Grain strength. Higher means more visible grain.")]
			public float intensity;

			// Token: 0x04002967 RID: 10599
			[Range(0.3f, 3f)]
			[Tooltip("Grain particle size.")]
			public float size;

			// Token: 0x04002968 RID: 10600
			[Range(0f, 1f)]
			[Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
			public float luminanceContribution;
		}
	}
}

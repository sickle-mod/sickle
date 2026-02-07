using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000768 RID: 1896
	[Serializable]
	public class MotionBlurModel : PostProcessingModel
	{
		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06003760 RID: 14176 RVA: 0x0004B933 File Offset: 0x00049B33
		// (set) Token: 0x06003761 RID: 14177 RVA: 0x0004B93B File Offset: 0x00049B3B
		public MotionBlurModel.Settings settings
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

		// Token: 0x06003762 RID: 14178 RVA: 0x0004B944 File Offset: 0x00049B44
		public override void Reset()
		{
			this.m_Settings = MotionBlurModel.Settings.defaultSettings;
		}

		// Token: 0x04002969 RID: 10601
		[SerializeField]
		private MotionBlurModel.Settings m_Settings = MotionBlurModel.Settings.defaultSettings;

		// Token: 0x02000769 RID: 1897
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700043E RID: 1086
			// (get) Token: 0x06003764 RID: 14180 RVA: 0x00146A5C File Offset: 0x00144C5C
			public static MotionBlurModel.Settings defaultSettings
			{
				get
				{
					return new MotionBlurModel.Settings
					{
						shutterAngle = 270f,
						sampleCount = 10,
						frameBlending = 0f
					};
				}
			}

			// Token: 0x0400296A RID: 10602
			[Range(0f, 360f)]
			[Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
			public float shutterAngle;

			// Token: 0x0400296B RID: 10603
			[Range(4f, 32f)]
			[Tooltip("The amount of sample points, which affects quality and performances.")]
			public int sampleCount;

			// Token: 0x0400296C RID: 10604
			[Range(0f, 1f)]
			[Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
			public float frameBlending;
		}
	}
}

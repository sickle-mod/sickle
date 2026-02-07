using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000771 RID: 1905
	[Serializable]
	public class UserLutModel : PostProcessingModel
	{
		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x0600376A RID: 14186 RVA: 0x0004B995 File Offset: 0x00049B95
		// (set) Token: 0x0600376B RID: 14187 RVA: 0x0004B99D File Offset: 0x00049B9D
		public UserLutModel.Settings settings
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

		// Token: 0x0600376C RID: 14188 RVA: 0x0004B9A6 File Offset: 0x00049BA6
		public override void Reset()
		{
			this.m_Settings = UserLutModel.Settings.defaultSettings;
		}

		// Token: 0x04002984 RID: 10628
		[SerializeField]
		private UserLutModel.Settings m_Settings = UserLutModel.Settings.defaultSettings;

		// Token: 0x02000772 RID: 1906
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000442 RID: 1090
			// (get) Token: 0x0600376E RID: 14190 RVA: 0x00146B68 File Offset: 0x00144D68
			public static UserLutModel.Settings defaultSettings
			{
				get
				{
					return new UserLutModel.Settings
					{
						lut = null,
						contribution = 1f
					};
				}
			}

			// Token: 0x04002985 RID: 10629
			[Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
			public Texture2D lut;

			// Token: 0x04002986 RID: 10630
			[Range(0f, 1f)]
			[Tooltip("Blending factor.")]
			public float contribution;
		}
	}
}

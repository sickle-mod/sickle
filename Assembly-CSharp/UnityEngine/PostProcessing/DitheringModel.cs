using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200075F RID: 1887
	[Serializable]
	public class DitheringModel : PostProcessingModel
	{
		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x0600374C RID: 14156 RVA: 0x0004B86F File Offset: 0x00049A6F
		// (set) Token: 0x0600374D RID: 14157 RVA: 0x0004B877 File Offset: 0x00049A77
		public DitheringModel.Settings settings
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

		// Token: 0x0600374E RID: 14158 RVA: 0x0004B880 File Offset: 0x00049A80
		public override void Reset()
		{
			this.m_Settings = DitheringModel.Settings.defaultSettings;
		}

		// Token: 0x04002952 RID: 10578
		[SerializeField]
		private DitheringModel.Settings m_Settings = DitheringModel.Settings.defaultSettings;

		// Token: 0x02000760 RID: 1888
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000436 RID: 1078
			// (get) Token: 0x06003750 RID: 14160 RVA: 0x00146954 File Offset: 0x00144B54
			public static DitheringModel.Settings defaultSettings
			{
				get
				{
					return default(DitheringModel.Settings);
				}
			}
		}
	}
}

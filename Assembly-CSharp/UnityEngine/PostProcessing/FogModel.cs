using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000764 RID: 1892
	[Serializable]
	public class FogModel : PostProcessingModel
	{
		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06003756 RID: 14166 RVA: 0x0004B8D1 File Offset: 0x00049AD1
		// (set) Token: 0x06003757 RID: 14167 RVA: 0x0004B8D9 File Offset: 0x00049AD9
		public FogModel.Settings settings
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

		// Token: 0x06003758 RID: 14168 RVA: 0x0004B8E2 File Offset: 0x00049AE2
		public override void Reset()
		{
			this.m_Settings = FogModel.Settings.defaultSettings;
		}

		// Token: 0x04002962 RID: 10594
		[SerializeField]
		private FogModel.Settings m_Settings = FogModel.Settings.defaultSettings;

		// Token: 0x02000765 RID: 1893
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700043A RID: 1082
			// (get) Token: 0x0600375A RID: 14170 RVA: 0x001469F8 File Offset: 0x00144BF8
			public static FogModel.Settings defaultSettings
			{
				get
				{
					return new FogModel.Settings
					{
						excludeSkybox = true
					};
				}
			}

			// Token: 0x04002963 RID: 10595
			[Tooltip("Should the fog affect the skybox?")]
			public bool excludeSkybox;
		}
	}
}

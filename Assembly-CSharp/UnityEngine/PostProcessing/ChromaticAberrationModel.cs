using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200074F RID: 1871
	[Serializable]
	public class ChromaticAberrationModel : PostProcessingModel
	{
		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06003731 RID: 14129 RVA: 0x0004B7A5 File Offset: 0x000499A5
		// (set) Token: 0x06003732 RID: 14130 RVA: 0x0004B7AD File Offset: 0x000499AD
		public ChromaticAberrationModel.Settings settings
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

		// Token: 0x06003733 RID: 14131 RVA: 0x0004B7B6 File Offset: 0x000499B6
		public override void Reset()
		{
			this.m_Settings = ChromaticAberrationModel.Settings.defaultSettings;
		}

		// Token: 0x0400290E RID: 10510
		[SerializeField]
		private ChromaticAberrationModel.Settings m_Settings = ChromaticAberrationModel.Settings.defaultSettings;

		// Token: 0x02000750 RID: 1872
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000427 RID: 1063
			// (get) Token: 0x06003735 RID: 14133 RVA: 0x00146418 File Offset: 0x00144618
			public static ChromaticAberrationModel.Settings defaultSettings
			{
				get
				{
					return new ChromaticAberrationModel.Settings
					{
						spectralTexture = null,
						intensity = 0.1f
					};
				}
			}

			// Token: 0x0400290F RID: 10511
			[Tooltip("Shift the hue of chromatic aberrations.")]
			public Texture2D spectralTexture;

			// Token: 0x04002910 RID: 10512
			[Range(0f, 1f)]
			[Tooltip("Amount of tangential distortion.")]
			public float intensity;
		}
	}
}

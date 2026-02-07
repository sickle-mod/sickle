using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000773 RID: 1907
	[Serializable]
	public class VignetteModel : PostProcessingModel
	{
		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x0600376F RID: 14191 RVA: 0x0004B9C6 File Offset: 0x00049BC6
		// (set) Token: 0x06003770 RID: 14192 RVA: 0x0004B9CE File Offset: 0x00049BCE
		public VignetteModel.Settings settings
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

		// Token: 0x06003771 RID: 14193 RVA: 0x0004B9D7 File Offset: 0x00049BD7
		public override void Reset()
		{
			this.m_Settings = VignetteModel.Settings.defaultSettings;
		}

		// Token: 0x04002987 RID: 10631
		[SerializeField]
		private VignetteModel.Settings m_Settings = VignetteModel.Settings.defaultSettings;

		// Token: 0x02000774 RID: 1908
		public enum Mode
		{
			// Token: 0x04002989 RID: 10633
			Classic,
			// Token: 0x0400298A RID: 10634
			Masked
		}

		// Token: 0x02000775 RID: 1909
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000444 RID: 1092
			// (get) Token: 0x06003773 RID: 14195 RVA: 0x00146B94 File Offset: 0x00144D94
			public static VignetteModel.Settings defaultSettings
			{
				get
				{
					return new VignetteModel.Settings
					{
						mode = VignetteModel.Mode.Classic,
						color = new Color(0f, 0f, 0f, 1f),
						center = new Vector2(0.5f, 0.5f),
						intensity = 0.45f,
						smoothness = 0.2f,
						roundness = 1f,
						mask = null,
						opacity = 1f,
						rounded = false
					};
				}
			}

			// Token: 0x0400298B RID: 10635
			[Tooltip("Use the \"Classic\" mode for parametric controls. Use the \"Masked\" mode to use your own texture mask.")]
			public VignetteModel.Mode mode;

			// Token: 0x0400298C RID: 10636
			[ColorUsage(false)]
			[Tooltip("Vignette color. Use the alpha channel for transparency.")]
			public Color color;

			// Token: 0x0400298D RID: 10637
			[Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
			public Vector2 center;

			// Token: 0x0400298E RID: 10638
			[Range(0f, 1f)]
			[Tooltip("Amount of vignetting on screen.")]
			public float intensity;

			// Token: 0x0400298F RID: 10639
			[Range(0.01f, 1f)]
			[Tooltip("Smoothness of the vignette borders.")]
			public float smoothness;

			// Token: 0x04002990 RID: 10640
			[Range(0f, 1f)]
			[Tooltip("Lower values will make a square-ish vignette.")]
			public float roundness;

			// Token: 0x04002991 RID: 10641
			[Tooltip("A black and white mask to use as a vignette.")]
			public Texture mask;

			// Token: 0x04002992 RID: 10642
			[Range(0f, 1f)]
			[Tooltip("Mask opacity.")]
			public float opacity;

			// Token: 0x04002993 RID: 10643
			[Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
			public bool rounded;
		}
	}
}

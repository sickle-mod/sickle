using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200073B RID: 1851
	[Serializable]
	public class AmbientOcclusionModel : PostProcessingModel
	{
		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06003711 RID: 14097 RVA: 0x0004B683 File Offset: 0x00049883
		// (set) Token: 0x06003712 RID: 14098 RVA: 0x0004B68B File Offset: 0x0004988B
		public AmbientOcclusionModel.Settings settings
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

		// Token: 0x06003713 RID: 14099 RVA: 0x0004B694 File Offset: 0x00049894
		public override void Reset()
		{
			this.m_Settings = AmbientOcclusionModel.Settings.defaultSettings;
		}

		// Token: 0x040028C6 RID: 10438
		[SerializeField]
		private AmbientOcclusionModel.Settings m_Settings = AmbientOcclusionModel.Settings.defaultSettings;

		// Token: 0x0200073C RID: 1852
		public enum SampleCount
		{
			// Token: 0x040028C8 RID: 10440
			Lowest = 3,
			// Token: 0x040028C9 RID: 10441
			Low = 6,
			// Token: 0x040028CA RID: 10442
			Medium = 10,
			// Token: 0x040028CB RID: 10443
			High = 16
		}

		// Token: 0x0200073D RID: 1853
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000417 RID: 1047
			// (get) Token: 0x06003715 RID: 14101 RVA: 0x00145F48 File Offset: 0x00144148
			public static AmbientOcclusionModel.Settings defaultSettings
			{
				get
				{
					return new AmbientOcclusionModel.Settings
					{
						intensity = 1f,
						radius = 0.3f,
						sampleCount = AmbientOcclusionModel.SampleCount.Medium,
						downsampling = true,
						forceForwardCompatibility = false,
						ambientOnly = false,
						highPrecision = false
					};
				}
			}

			// Token: 0x040028CC RID: 10444
			[Range(0f, 4f)]
			[Tooltip("Degree of darkness produced by the effect.")]
			public float intensity;

			// Token: 0x040028CD RID: 10445
			[Min(0.0001f)]
			[Tooltip("Radius of sample points, which affects extent of darkened areas.")]
			public float radius;

			// Token: 0x040028CE RID: 10446
			[Tooltip("Number of sample points, which affects quality and performance.")]
			public AmbientOcclusionModel.SampleCount sampleCount;

			// Token: 0x040028CF RID: 10447
			[Tooltip("Halves the resolution of the effect to increase performance at the cost of visual quality.")]
			public bool downsampling;

			// Token: 0x040028D0 RID: 10448
			[Tooltip("Forces compatibility with Forward rendered objects when working with the Deferred rendering path.")]
			public bool forceForwardCompatibility;

			// Token: 0x040028D1 RID: 10449
			[Tooltip("Enables the ambient-only mode in that the effect only affects ambient lighting. This mode is only available with the Deferred rendering path and HDR rendering.")]
			public bool ambientOnly;

			// Token: 0x040028D2 RID: 10450
			[Tooltip("Toggles the use of a higher precision depth texture with the forward rendering path (may impact performances). Has no effect with the deferred rendering path.")]
			public bool highPrecision;
		}
	}
}

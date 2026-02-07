using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200075C RID: 1884
	[Serializable]
	public class DepthOfFieldModel : PostProcessingModel
	{
		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06003747 RID: 14151 RVA: 0x0004B83E File Offset: 0x00049A3E
		// (set) Token: 0x06003748 RID: 14152 RVA: 0x0004B846 File Offset: 0x00049A46
		public DepthOfFieldModel.Settings settings
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

		// Token: 0x06003749 RID: 14153 RVA: 0x0004B84F File Offset: 0x00049A4F
		public override void Reset()
		{
			this.m_Settings = DepthOfFieldModel.Settings.defaultSettings;
		}

		// Token: 0x04002947 RID: 10567
		[SerializeField]
		private DepthOfFieldModel.Settings m_Settings = DepthOfFieldModel.Settings.defaultSettings;

		// Token: 0x0200075D RID: 1885
		public enum KernelSize
		{
			// Token: 0x04002949 RID: 10569
			Small,
			// Token: 0x0400294A RID: 10570
			Medium,
			// Token: 0x0400294B RID: 10571
			Large,
			// Token: 0x0400294C RID: 10572
			VeryLarge
		}

		// Token: 0x0200075E RID: 1886
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000434 RID: 1076
			// (get) Token: 0x0600374B RID: 14155 RVA: 0x00146908 File Offset: 0x00144B08
			public static DepthOfFieldModel.Settings defaultSettings
			{
				get
				{
					return new DepthOfFieldModel.Settings
					{
						focusDistance = 10f,
						aperture = 5.6f,
						focalLength = 50f,
						useCameraFov = false,
						kernelSize = DepthOfFieldModel.KernelSize.Medium
					};
				}
			}

			// Token: 0x0400294D RID: 10573
			[Min(0.1f)]
			[Tooltip("Distance to the point of focus.")]
			public float focusDistance;

			// Token: 0x0400294E RID: 10574
			[Range(0.05f, 32f)]
			[Tooltip("Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")]
			public float aperture;

			// Token: 0x0400294F RID: 10575
			[Range(1f, 300f)]
			[Tooltip("Distance between the lens and the film. The larger the value is, the shallower the depth of field is.")]
			public float focalLength;

			// Token: 0x04002950 RID: 10576
			[Tooltip("Calculate the focal length automatically from the field-of-view value set on the camera. Using this setting isn't recommended.")]
			public bool useCameraFov;

			// Token: 0x04002951 RID: 10577
			[Tooltip("Convolution kernel size of the bokeh filter, which determines the maximum radius of bokeh. It also affects the performance (the larger the kernel is, the longer the GPU time is required).")]
			public DepthOfFieldModel.KernelSize kernelSize;
		}
	}
}

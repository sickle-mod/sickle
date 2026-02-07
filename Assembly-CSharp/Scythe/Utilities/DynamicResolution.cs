using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Utilities
{
	// Token: 0x020001CF RID: 463
	public class DynamicResolution : MonoBehaviour
	{
		// Token: 0x06000D70 RID: 3440 RVA: 0x00030EAF File Offset: 0x0002F0AF
		private void Awake()
		{
			this.timeStamps = new List<float>();
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0008560C File Offset: 0x0008380C
		private void Start()
		{
			Mathf.Ceil(ScalableBufferManager.widthScaleFactor * (float)Screen.currentResolution.width);
			Mathf.Ceil(ScalableBufferManager.heightScaleFactor * (float)Screen.currentResolution.height);
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00085650 File Offset: 0x00083850
		private void Update()
		{
			float widthScale = this.m_widthScale;
			float heightScale = this.m_heightScale;
			this.timeStamps.Add(Time.unscaledTime);
			while (Time.unscaledTime - this.timeStamps[0] > 1f)
			{
				this.timeStamps.RemoveAt(0);
			}
			if (this.timeStamps.Count < 20)
			{
				this.m_heightScale = Mathf.Max(this.minResolutionHeightScale, this.m_heightScale - this.scaleHeightIncrement);
				this.m_widthScale = Mathf.Max(this.minResolutionWidthScale, this.m_widthScale - this.scaleWidthIncrement);
			}
			if (this.timeStamps.Count > 30)
			{
				this.m_heightScale = Mathf.Min(this.maxResolutionHeightScale, this.m_heightScale + this.scaleHeightIncrement);
				this.m_widthScale = Mathf.Min(this.maxResolutionWidthScale, this.m_widthScale + this.scaleWidthIncrement);
			}
			ScalableBufferManager.ResizeBuffers(0.1f, 0.1f);
			int num = (int)Mathf.Ceil(ScalableBufferManager.widthScaleFactor * (float)Screen.currentResolution.width);
			int num2 = (int)Mathf.Ceil(ScalableBufferManager.heightScaleFactor * (float)Screen.currentResolution.height);
			if (this.screenText != null)
			{
				this.screenText.text = string.Format("Scale: {0:F3}x{1:F3}\nResolution: {2}x{3}\nScaleFactor: {4:F3}x{5:F3}\nDeltaTime: {6:F3}", new object[]
				{
					this.m_widthScale,
					this.m_heightScale,
					num,
					num2,
					ScalableBufferManager.widthScaleFactor,
					ScalableBufferManager.heightScaleFactor,
					this.timeStamps.Count
				});
			}
		}

		// Token: 0x04000ABB RID: 2747
		public Text screenText;

		// Token: 0x04000ABC RID: 2748
		private FrameTiming[] frameTimings = new FrameTiming[3];

		// Token: 0x04000ABD RID: 2749
		public float maxResolutionWidthScale = 1f;

		// Token: 0x04000ABE RID: 2750
		public float maxResolutionHeightScale = 1f;

		// Token: 0x04000ABF RID: 2751
		public float minResolutionWidthScale = 0.5f;

		// Token: 0x04000AC0 RID: 2752
		public float minResolutionHeightScale = 0.5f;

		// Token: 0x04000AC1 RID: 2753
		public float scaleWidthIncrement = 0.1f;

		// Token: 0x04000AC2 RID: 2754
		public float scaleHeightIncrement = 0.1f;

		// Token: 0x04000AC3 RID: 2755
		private float m_widthScale = 1f;

		// Token: 0x04000AC4 RID: 2756
		private float m_heightScale = 1f;

		// Token: 0x04000AC5 RID: 2757
		private const uint kNumFrameTimings = 2U;

		// Token: 0x04000AC6 RID: 2758
		private double m_gpuFrameTime;

		// Token: 0x04000AC7 RID: 2759
		private double m_cpuFrameTime;

		// Token: 0x04000AC8 RID: 2760
		private List<float> timeStamps;
	}
}

using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
	// Token: 0x0200066D RID: 1645
	[RequireComponent(typeof(Text))]
	public class FPSCounter : MonoBehaviour
	{
		// Token: 0x060033E0 RID: 13280 RVA: 0x00048C3F File Offset: 0x00046E3F
		private void Start()
		{
			this.m_FpsNextPeriod = Time.realtimeSinceStartup + 0.5f;
			this.m_Text = base.GetComponent<Text>();
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x00133458 File Offset: 0x00131658
		private void Update()
		{
			this.m_FpsAccumulator++;
			if (Time.realtimeSinceStartup > this.m_FpsNextPeriod)
			{
				this.m_CurrentFps = (int)((float)this.m_FpsAccumulator / 0.5f);
				this.m_FpsAccumulator = 0;
				this.m_FpsNextPeriod += 0.5f;
				this.m_Text.text = string.Format("{0} FPS", this.m_CurrentFps);
			}
		}

		// Token: 0x0400242B RID: 9259
		private const float fpsMeasurePeriod = 0.5f;

		// Token: 0x0400242C RID: 9260
		private int m_FpsAccumulator;

		// Token: 0x0400242D RID: 9261
		private float m_FpsNextPeriod;

		// Token: 0x0400242E RID: 9262
		private int m_CurrentFps;

		// Token: 0x0400242F RID: 9263
		private const string display = "{0} FPS";

		// Token: 0x04002430 RID: 9264
		private Text m_Text;
	}
}

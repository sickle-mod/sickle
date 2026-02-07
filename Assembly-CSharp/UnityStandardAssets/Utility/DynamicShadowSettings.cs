using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000668 RID: 1640
	public class DynamicShadowSettings : MonoBehaviour
	{
		// Token: 0x060033C9 RID: 13257 RVA: 0x00048B24 File Offset: 0x00046D24
		private void Start()
		{
			this.m_OriginalStrength = this.sunLight.shadowStrength;
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x00133158 File Offset: 0x00131358
		private void Update()
		{
			Ray ray = new Ray(Camera.main.transform.position, -Vector3.up);
			float num = base.transform.position.y;
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit))
			{
				num = raycastHit.distance;
			}
			if (Mathf.Abs(num - this.m_SmoothHeight) > 1f)
			{
				this.m_SmoothHeight = Mathf.SmoothDamp(this.m_SmoothHeight, num, ref this.m_ChangeSpeed, this.adaptTime);
			}
			float num2 = Mathf.InverseLerp(this.minHeight, this.maxHeight, this.m_SmoothHeight);
			QualitySettings.shadowDistance = Mathf.Lerp(this.minShadowDistance, this.maxShadowDistance, num2);
			this.sunLight.shadowBias = Mathf.Lerp(this.minShadowBias, this.maxShadowBias, 1f - (1f - num2) * (1f - num2));
			this.sunLight.shadowStrength = Mathf.Lerp(this.m_OriginalStrength, 0f, num2);
		}

		// Token: 0x04002410 RID: 9232
		public Light sunLight;

		// Token: 0x04002411 RID: 9233
		public float minHeight = 10f;

		// Token: 0x04002412 RID: 9234
		public float minShadowDistance = 80f;

		// Token: 0x04002413 RID: 9235
		public float minShadowBias = 1f;

		// Token: 0x04002414 RID: 9236
		public float maxHeight = 1000f;

		// Token: 0x04002415 RID: 9237
		public float maxShadowDistance = 10000f;

		// Token: 0x04002416 RID: 9238
		public float maxShadowBias = 0.1f;

		// Token: 0x04002417 RID: 9239
		public float adaptTime = 1f;

		// Token: 0x04002418 RID: 9240
		private float m_SmoothHeight;

		// Token: 0x04002419 RID: 9241
		private float m_ChangeSpeed;

		// Token: 0x0400241A RID: 9242
		private float m_OriginalStrength = 1f;
	}
}

using System;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	// Token: 0x0200068E RID: 1678
	public class FireLight : MonoBehaviour
	{
		// Token: 0x06003466 RID: 13414 RVA: 0x00049110 File Offset: 0x00047310
		private void Start()
		{
			this.m_Rnd = global::UnityEngine.Random.value * 100f;
			this.m_Light = base.GetComponent<Light>();
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x00134E84 File Offset: 0x00133084
		private void Update()
		{
			if (this.m_Burning)
			{
				this.m_Light.intensity = 2f * Mathf.PerlinNoise(this.m_Rnd + Time.time, this.m_Rnd + 1f + Time.time * 1f);
				float num = Mathf.PerlinNoise(this.m_Rnd + 0f + Time.time * 2f, this.m_Rnd + 1f + Time.time * 2f) - 0.5f;
				float num2 = Mathf.PerlinNoise(this.m_Rnd + 2f + Time.time * 2f, this.m_Rnd + 3f + Time.time * 2f) - 0.5f;
				float num3 = Mathf.PerlinNoise(this.m_Rnd + 4f + Time.time * 2f, this.m_Rnd + 5f + Time.time * 2f) - 0.5f;
				base.transform.localPosition = Vector3.up + new Vector3(num, num2, num3) * 1f;
			}
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x0004912F File Offset: 0x0004732F
		public void Extinguish()
		{
			this.m_Burning = false;
			this.m_Light.enabled = false;
		}

		// Token: 0x040024BD RID: 9405
		private float m_Rnd;

		// Token: 0x040024BE RID: 9406
		private bool m_Burning = true;

		// Token: 0x040024BF RID: 9407
		private Light m_Light;
	}
}

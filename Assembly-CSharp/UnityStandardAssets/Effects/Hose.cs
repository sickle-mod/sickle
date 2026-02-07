using System;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	// Token: 0x0200068F RID: 1679
	public class Hose : MonoBehaviour
	{
		// Token: 0x0600346A RID: 13418 RVA: 0x00134FB4 File Offset: 0x001331B4
		private void Update()
		{
			this.m_Power = Mathf.Lerp(this.m_Power, Input.GetMouseButton(0) ? this.maxPower : this.minPower, Time.deltaTime * this.changeSpeed);
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				this.systemRenderer.enabled = !this.systemRenderer.enabled;
			}
			foreach (ParticleSystem particleSystem in this.hoseWaterSystems)
			{
				particleSystem.main.startSpeed = this.m_Power;
				particleSystem.emission.enabled = this.m_Power > this.minPower * 1.1f;
			}
		}

		// Token: 0x040024C0 RID: 9408
		public float maxPower = 20f;

		// Token: 0x040024C1 RID: 9409
		public float minPower = 5f;

		// Token: 0x040024C2 RID: 9410
		public float changeSpeed = 5f;

		// Token: 0x040024C3 RID: 9411
		public ParticleSystem[] hoseWaterSystems;

		// Token: 0x040024C4 RID: 9412
		public Renderer systemRenderer;

		// Token: 0x040024C5 RID: 9413
		private float m_Power;
	}
}

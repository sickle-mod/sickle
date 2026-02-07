using System;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	// Token: 0x02000690 RID: 1680
	public class ParticleSystemMultiplier : MonoBehaviour
	{
		// Token: 0x0600346C RID: 13420 RVA: 0x00135068 File Offset: 0x00133268
		private void Start()
		{
			foreach (ParticleSystem particleSystem in base.GetComponentsInChildren<ParticleSystem>())
			{
				ParticleSystem.MainModule main = particleSystem.main;
				main.startSizeMultiplier = this.multiplier;
				main.startSpeedMultiplier = this.multiplier;
				main.startLifetimeMultiplier = Mathf.Lerp(this.multiplier, 1f, 0.5f);
				particleSystem.Clear();
				particleSystem.Play();
			}
		}

		// Token: 0x040024C6 RID: 9414
		public float multiplier = 1f;
	}
}

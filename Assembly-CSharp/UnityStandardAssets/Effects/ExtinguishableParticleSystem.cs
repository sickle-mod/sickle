using System;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	// Token: 0x0200068D RID: 1677
	public class ExtinguishableParticleSystem : MonoBehaviour
	{
		// Token: 0x06003463 RID: 13411 RVA: 0x000490EF File Offset: 0x000472EF
		private void Start()
		{
			this.m_Systems = base.GetComponentsInChildren<ParticleSystem>();
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x00134E50 File Offset: 0x00133050
		public void Extinguish()
		{
			ParticleSystem[] systems = this.m_Systems;
			for (int i = 0; i < systems.Length; i++)
			{
				systems[i].emission.enabled = false;
			}
		}

		// Token: 0x040024BB RID: 9403
		public float multiplier = 1f;

		// Token: 0x040024BC RID: 9404
		private ParticleSystem[] m_Systems;
	}
}

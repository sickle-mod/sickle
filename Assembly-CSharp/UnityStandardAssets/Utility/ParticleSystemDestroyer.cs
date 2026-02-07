using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000672 RID: 1650
	public class ParticleSystemDestroyer : MonoBehaviour
	{
		// Token: 0x060033F6 RID: 13302 RVA: 0x00048CC9 File Offset: 0x00046EC9
		private IEnumerator Start()
		{
			ParticleSystem[] systems = base.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem particleSystem in systems)
			{
				this.m_MaxLifetime = Mathf.Max(particleSystem.main.startLifetime.constant, this.m_MaxLifetime);
			}
			float stopTime = Time.time + global::UnityEngine.Random.Range(this.minDuration, this.maxDuration);
			while (Time.time < stopTime || this.m_EarlyStop)
			{
				yield return null;
			}
			Debug.Log("stopping " + base.name);
			ParticleSystem[] array = systems;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].emission.enabled = false;
			}
			base.BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);
			yield return new WaitForSeconds(this.m_MaxLifetime);
			global::UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x00048CD8 File Offset: 0x00046ED8
		public void Stop()
		{
			this.m_EarlyStop = true;
		}

		// Token: 0x04002440 RID: 9280
		public float minDuration = 8f;

		// Token: 0x04002441 RID: 9281
		public float maxDuration = 10f;

		// Token: 0x04002442 RID: 9282
		private float m_MaxLifetime;

		// Token: 0x04002443 RID: 9283
		private bool m_EarlyStop;
	}
}

using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Effects
{
	// Token: 0x0200068B RID: 1675
	public class Explosive : MonoBehaviour
	{
		// Token: 0x06003459 RID: 13401 RVA: 0x0004907B File Offset: 0x0004727B
		private void Start()
		{
			this.m_ObjectResetter = base.GetComponent<ObjectResetter>();
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x00049089 File Offset: 0x00047289
		private IEnumerator OnCollisionEnter(Collision col)
		{
			if (base.enabled && col.contacts.Length != 0 && (Vector3.Project(col.relativeVelocity, col.contacts[0].normal).magnitude > this.detonationImpactVelocity || this.m_Exploded) && !this.m_Exploded)
			{
				global::UnityEngine.Object.Instantiate<Transform>(this.explosionPrefab, col.contacts[0].point, Quaternion.LookRotation(col.contacts[0].normal));
				this.m_Exploded = true;
				base.SendMessage("Immobilize");
				if (this.reset)
				{
					this.m_ObjectResetter.DelayedReset(this.resetTimeDelay);
				}
			}
			yield return null;
			yield break;
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x0004909F File Offset: 0x0004729F
		public void Reset()
		{
			this.m_Exploded = false;
		}

		// Token: 0x040024B0 RID: 9392
		public Transform explosionPrefab;

		// Token: 0x040024B1 RID: 9393
		public float detonationImpactVelocity = 10f;

		// Token: 0x040024B2 RID: 9394
		public float sizeMultiplier = 1f;

		// Token: 0x040024B3 RID: 9395
		public bool reset = true;

		// Token: 0x040024B4 RID: 9396
		public float resetTimeDelay = 10f;

		// Token: 0x040024B5 RID: 9397
		private bool m_Exploded;

		// Token: 0x040024B6 RID: 9398
		private ObjectResetter m_ObjectResetter;
	}
}

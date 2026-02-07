using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	// Token: 0x02000692 RID: 1682
	public class WaterHoseParticles : MonoBehaviour
	{
		// Token: 0x06003470 RID: 13424 RVA: 0x000491BC File Offset: 0x000473BC
		private void Start()
		{
			this.m_ParticleSystem = base.GetComponent<ParticleSystem>();
		}

		// Token: 0x06003471 RID: 13425 RVA: 0x001350D8 File Offset: 0x001332D8
		private void OnParticleCollision(GameObject other)
		{
			int collisionEvents = this.m_ParticleSystem.GetCollisionEvents(other, this.m_CollisionEvents);
			for (int i = 0; i < collisionEvents; i++)
			{
				if (Time.time > WaterHoseParticles.lastSoundTime + 0.2f)
				{
					WaterHoseParticles.lastSoundTime = Time.time;
				}
				Rigidbody component = this.m_CollisionEvents[i].colliderComponent.GetComponent<Rigidbody>();
				if (component != null)
				{
					Vector3 velocity = this.m_CollisionEvents[i].velocity;
					component.AddForce(velocity * this.force, ForceMode.Impulse);
				}
				other.BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x040024C8 RID: 9416
		public static float lastSoundTime;

		// Token: 0x040024C9 RID: 9417
		public float force = 1f;

		// Token: 0x040024CA RID: 9418
		private List<ParticleCollisionEvent> m_CollisionEvents = new List<ParticleCollisionEvent>();

		// Token: 0x040024CB RID: 9419
		private ParticleSystem m_ParticleSystem;
	}
}

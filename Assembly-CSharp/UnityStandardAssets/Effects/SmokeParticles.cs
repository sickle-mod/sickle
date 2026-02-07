using System;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	// Token: 0x02000691 RID: 1681
	public class SmokeParticles : MonoBehaviour
	{
		// Token: 0x0600346E RID: 13422 RVA: 0x0004918F File Offset: 0x0004738F
		private void Start()
		{
			base.GetComponent<AudioSource>().clip = this.extinguishSounds[global::UnityEngine.Random.Range(0, this.extinguishSounds.Length)];
			base.GetComponent<AudioSource>().Play();
		}

		// Token: 0x040024C7 RID: 9415
		public AudioClip[] extinguishSounds;
	}
}

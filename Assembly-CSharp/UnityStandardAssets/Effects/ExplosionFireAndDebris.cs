using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	// Token: 0x02000687 RID: 1671
	public class ExplosionFireAndDebris : MonoBehaviour
	{
		// Token: 0x06003448 RID: 13384 RVA: 0x00048FF0 File Offset: 0x000471F0
		private IEnumerator Start()
		{
			float multiplier = base.GetComponent<ParticleSystemMultiplier>().multiplier;
			int num = 0;
			while ((float)num < (float)this.numDebrisPieces * multiplier)
			{
				Transform transform = this.debrisPrefabs[global::UnityEngine.Random.Range(0, this.debrisPrefabs.Length)];
				Vector3 vector = base.transform.position + global::UnityEngine.Random.insideUnitSphere * 3f * multiplier;
				Quaternion rotation = global::UnityEngine.Random.rotation;
				global::UnityEngine.Object.Instantiate<Transform>(transform, vector, rotation);
				num++;
			}
			yield return null;
			float num2 = 10f * multiplier;
			foreach (Collider collider in Physics.OverlapSphere(base.transform.position, num2))
			{
				if (this.numFires > 0)
				{
					Ray ray = new Ray(base.transform.position, collider.transform.position - base.transform.position);
					RaycastHit raycastHit;
					if (collider.Raycast(ray, out raycastHit, num2))
					{
						this.AddFire(collider.transform, raycastHit.point, raycastHit.normal);
						this.numFires--;
					}
				}
			}
			float num3 = 0f;
			while (this.numFires > 0 && num3 < num2)
			{
				RaycastHit raycastHit2;
				if (Physics.Raycast(new Ray(base.transform.position + Vector3.up, global::UnityEngine.Random.onUnitSphere), out raycastHit2, num3))
				{
					this.AddFire(null, raycastHit2.point, raycastHit2.normal);
					this.numFires--;
				}
				num3 += num2 * 0.1f;
			}
			yield break;
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x00048FFF File Offset: 0x000471FF
		private void AddFire(Transform t, Vector3 pos, Vector3 normal)
		{
			pos += normal * 0.5f;
			global::UnityEngine.Object.Instantiate<Transform>(this.firePrefab, pos, Quaternion.identity).parent = t;
		}

		// Token: 0x040024A4 RID: 9380
		public Transform[] debrisPrefabs;

		// Token: 0x040024A5 RID: 9381
		public Transform firePrefab;

		// Token: 0x040024A6 RID: 9382
		public int numDebrisPieces;

		// Token: 0x040024A7 RID: 9383
		public int numFires;
	}
}

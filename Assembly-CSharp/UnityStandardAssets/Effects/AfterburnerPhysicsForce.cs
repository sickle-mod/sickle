using System;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	// Token: 0x02000686 RID: 1670
	[RequireComponent(typeof(SphereCollider))]
	public class AfterburnerPhysicsForce : MonoBehaviour
	{
		// Token: 0x06003444 RID: 13380 RVA: 0x00048FA9 File Offset: 0x000471A9
		private void OnEnable()
		{
			this.m_Sphere = base.GetComponent<Collider>() as SphereCollider;
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x00134720 File Offset: 0x00132920
		private void FixedUpdate()
		{
			this.m_Cols = Physics.OverlapSphere(base.transform.position + this.m_Sphere.center, this.m_Sphere.radius);
			for (int i = 0; i < this.m_Cols.Length; i++)
			{
				if (this.m_Cols[i].attachedRigidbody != null)
				{
					Vector3 vector = base.transform.InverseTransformPoint(this.m_Cols[i].transform.position);
					vector = Vector3.MoveTowards(vector, new Vector3(0f, 0f, vector.z), this.effectWidth * 0.5f);
					float num = Mathf.Abs(Mathf.Atan2(vector.x, vector.z) * 57.29578f);
					float num2 = Mathf.InverseLerp(this.effectDistance, 0f, vector.magnitude);
					num2 *= Mathf.InverseLerp(this.effectAngle, 0f, num);
					Vector3 vector2 = this.m_Cols[i].transform.position - base.transform.position;
					this.m_Cols[i].attachedRigidbody.AddForceAtPosition(vector2.normalized * this.force * num2, Vector3.Lerp(this.m_Cols[i].transform.position, base.transform.TransformPoint(0f, 0f, vector.z), 0.1f));
				}
			}
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x001348A4 File Offset: 0x00132AA4
		private void OnDrawGizmosSelected()
		{
			if (this.m_Sphere == null)
			{
				this.m_Sphere = base.GetComponent<Collider>() as SphereCollider;
			}
			this.m_Sphere.radius = this.effectDistance * 0.5f;
			this.m_Sphere.center = new Vector3(0f, 0f, this.effectDistance * 0.5f);
			Vector3[] array = new Vector3[]
			{
				Vector3.up,
				-Vector3.up,
				Vector3.right,
				-Vector3.right
			};
			Vector3[] array2 = new Vector3[]
			{
				-Vector3.right,
				Vector3.right,
				Vector3.up,
				-Vector3.up
			};
			Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
			for (int i = 0; i < 4; i++)
			{
				Vector3 vector = base.transform.position + base.transform.rotation * array[i] * this.effectWidth * 0.5f;
				Vector3 vector2 = base.transform.TransformDirection(Quaternion.AngleAxis(this.effectAngle, array2[i]) * Vector3.forward);
				Gizmos.DrawLine(vector, vector + vector2 * this.m_Sphere.radius * 2f);
			}
		}

		// Token: 0x0400249E RID: 9374
		public float effectAngle = 15f;

		// Token: 0x0400249F RID: 9375
		public float effectWidth = 1f;

		// Token: 0x040024A0 RID: 9376
		public float effectDistance = 10f;

		// Token: 0x040024A1 RID: 9377
		public float force = 10f;

		// Token: 0x040024A2 RID: 9378
		private Collider[] m_Cols;

		// Token: 0x040024A3 RID: 9379
		private SphereCollider m_Sphere;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000670 RID: 1648
	public class ObjectResetter : MonoBehaviour
	{
		// Token: 0x060033EC RID: 13292 RVA: 0x001335E8 File Offset: 0x001317E8
		private void Start()
		{
			this.originalStructure = new List<Transform>(base.GetComponentsInChildren<Transform>());
			this.originalPosition = base.transform.position;
			this.originalRotation = base.transform.rotation;
			this.Rigidbody = base.GetComponent<Rigidbody>();
		}

		// Token: 0x060033ED RID: 13293 RVA: 0x00048C8C File Offset: 0x00046E8C
		public void DelayedReset(float delay)
		{
			base.StartCoroutine(this.ResetCoroutine(delay));
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x00048C9C File Offset: 0x00046E9C
		public IEnumerator ResetCoroutine(float delay)
		{
			yield return new WaitForSeconds(delay);
			foreach (Transform transform in base.GetComponentsInChildren<Transform>())
			{
				if (!this.originalStructure.Contains(transform))
				{
					transform.parent = null;
				}
			}
			base.transform.position = this.originalPosition;
			base.transform.rotation = this.originalRotation;
			if (this.Rigidbody)
			{
				this.Rigidbody.velocity = Vector3.zero;
				this.Rigidbody.angularVelocity = Vector3.zero;
			}
			base.SendMessage("Reset");
			yield break;
		}

		// Token: 0x04002438 RID: 9272
		private Vector3 originalPosition;

		// Token: 0x04002439 RID: 9273
		private Quaternion originalRotation;

		// Token: 0x0400243A RID: 9274
		private List<Transform> originalStructure;

		// Token: 0x0400243B RID: 9275
		private Rigidbody Rigidbody;
	}
}

using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000677 RID: 1655
	public class SmoothFollow : MonoBehaviour
	{
		// Token: 0x06003406 RID: 13318 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void Start()
		{
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x00133C28 File Offset: 0x00131E28
		private void LateUpdate()
		{
			if (!this.target)
			{
				return;
			}
			float y = this.target.eulerAngles.y;
			float num = this.target.position.y + this.height;
			float num2 = base.transform.eulerAngles.y;
			float num3 = base.transform.position.y;
			num2 = Mathf.LerpAngle(num2, y, this.rotationDamping * Time.deltaTime);
			num3 = Mathf.Lerp(num3, num, this.heightDamping * Time.deltaTime);
			Quaternion quaternion = Quaternion.Euler(0f, num2, 0f);
			base.transform.position = this.target.position;
			base.transform.position -= quaternion * Vector3.forward * this.distance;
			base.transform.position = new Vector3(base.transform.position.x, num3, base.transform.position.z);
			base.transform.LookAt(this.target);
		}

		// Token: 0x0400245A RID: 9306
		[SerializeField]
		private Transform target;

		// Token: 0x0400245B RID: 9307
		[SerializeField]
		private float distance = 10f;

		// Token: 0x0400245C RID: 9308
		[SerializeField]
		private float height = 5f;

		// Token: 0x0400245D RID: 9309
		[SerializeField]
		private float rotationDamping;

		// Token: 0x0400245E RID: 9310
		[SerializeField]
		private float heightDamping;
	}
}

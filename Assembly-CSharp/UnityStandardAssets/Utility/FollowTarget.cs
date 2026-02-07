using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000669 RID: 1641
	public class FollowTarget : MonoBehaviour
	{
		// Token: 0x060033CC RID: 13260 RVA: 0x00048B37 File Offset: 0x00046D37
		private void LateUpdate()
		{
			base.transform.position = this.target.position + this.offset;
		}

		// Token: 0x0400241B RID: 9243
		public Transform target;

		// Token: 0x0400241C RID: 9244
		public Vector3 offset = new Vector3(0f, 7.5f, 0f);
	}
}

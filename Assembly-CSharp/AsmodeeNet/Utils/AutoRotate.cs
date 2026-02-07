using System;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000829 RID: 2089
	public class AutoRotate : MonoBehaviour
	{
		// Token: 0x06003B40 RID: 15168 RVA: 0x0004E55C File Offset: 0x0004C75C
		private void FixedUpdate()
		{
			base.transform.Rotate(0f, 0f, this.Speed);
		}

		// Token: 0x04002CE3 RID: 11491
		public float Speed = 1f;
	}
}

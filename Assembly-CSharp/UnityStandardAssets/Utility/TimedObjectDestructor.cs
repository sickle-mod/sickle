using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x0200067F RID: 1663
	public class TimedObjectDestructor : MonoBehaviour
	{
		// Token: 0x06003422 RID: 13346 RVA: 0x00048E0E File Offset: 0x0004700E
		private IEnumerator Start()
		{
			yield return new WaitForSeconds(this.m_TimeOut);
			this.DestroyNow();
			yield break;
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x00048E1D File Offset: 0x0004701D
		private void DestroyNow()
		{
			if (this.m_DetachChildren)
			{
				base.transform.DetachChildren();
			}
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04002473 RID: 9331
		[SerializeField]
		private float m_TimeOut = 1f;

		// Token: 0x04002474 RID: 9332
		[SerializeField]
		private bool m_DetachChildren;
	}
}

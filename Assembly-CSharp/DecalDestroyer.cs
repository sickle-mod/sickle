using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class DecalDestroyer : MonoBehaviour
{
	// Token: 0x0600001E RID: 30 RVA: 0x00027EBF File Offset: 0x000260BF
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(this.lifeTime);
		global::UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400000B RID: 11
	public float lifeTime = 5f;
}

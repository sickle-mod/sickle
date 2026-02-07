using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class Rotarion : MonoBehaviour
{
	// Token: 0x0600003B RID: 59 RVA: 0x00027EF0 File Offset: 0x000260F0
	private void Start()
	{
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00053054 File Offset: 0x00051254
	private void Update()
	{
		Quaternion quaternion = Quaternion.Euler(1f, 0f, 0f);
		base.transform.rotation *= quaternion;
	}
}

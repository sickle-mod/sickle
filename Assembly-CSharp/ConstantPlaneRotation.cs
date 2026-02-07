using System;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class ConstantPlaneRotation : MonoBehaviour
{
	// Token: 0x06000297 RID: 663 RVA: 0x00029937 File Offset: 0x00027B37
	private void Awake()
	{
		this.rotation = base.transform.rotation;
	}

	// Token: 0x06000298 RID: 664 RVA: 0x0002994A File Offset: 0x00027B4A
	private void LateUpdate()
	{
		base.transform.rotation = this.rotation;
	}

	// Token: 0x040001F4 RID: 500
	private Quaternion rotation;
}

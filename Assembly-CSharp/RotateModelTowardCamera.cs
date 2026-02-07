using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class RotateModelTowardCamera : MonoBehaviour
{
	// Token: 0x0600006B RID: 107 RVA: 0x00028367 File Offset: 0x00026567
	private void Update()
	{
		base.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up);
	}
}

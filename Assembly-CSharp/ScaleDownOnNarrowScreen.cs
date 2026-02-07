using System;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class ScaleDownOnNarrowScreen : MonoBehaviour
{
	// Token: 0x060009B2 RID: 2482 RVA: 0x0002E935 File Offset: 0x0002CB35
	private void Start()
	{
		if ((float)Screen.height / (float)Screen.width >= 0.74f)
		{
			base.transform.localScale = this.scale * Vector3.one;
		}
	}

	// Token: 0x040008C2 RID: 2242
	public float scale = 0.92f;
}

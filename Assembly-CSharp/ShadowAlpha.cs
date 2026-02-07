using System;
using UnityEngine;

// Token: 0x0200015F RID: 351
public class ShadowAlpha : MonoBehaviour
{
	// Token: 0x06000A4E RID: 2638 RVA: 0x0002F067 File Offset: 0x0002D267
	private void Start()
	{
		this.mat = base.GetComponent<MeshRenderer>().material;
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x0007CE28 File Offset: 0x0007B028
	private void Update()
	{
		float num = base.transform.position.y * 2f;
		if (num < 0f)
		{
			num = 0f;
		}
		if (num > 1f)
		{
			num = 1f;
		}
		num = 1f - num;
		if ((double)num > 0.875)
		{
			num = 0.875f;
		}
		this.col.a = num;
		this.mat.color = this.col;
	}

	// Token: 0x040008F5 RID: 2293
	private Material mat;

	// Token: 0x040008F6 RID: 2294
	private Color col = new Color(1f, 1f, 1f, 1f);
}

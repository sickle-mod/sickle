using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class GunAim : MonoBehaviour
{
	// Token: 0x06000026 RID: 38 RVA: 0x00027F01 File Offset: 0x00026101
	private void Start()
	{
		this.parentCamera = base.GetComponentInParent<Camera>();
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00052C88 File Offset: 0x00050E88
	private void Update()
	{
		float x = Input.mousePosition.x;
		float y = Input.mousePosition.y;
		if (x <= (float)this.borderLeft || x >= (float)(Screen.width - this.borderRight) || y <= (float)this.borderBottom || y >= (float)(Screen.height - this.borderTop))
		{
			this.isOutOfBounds = true;
		}
		else
		{
			this.isOutOfBounds = false;
		}
		if (!this.isOutOfBounds)
		{
			base.transform.LookAt(this.parentCamera.ScreenToWorldPoint(new Vector3(x, y, 5f)));
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00027F0F File Offset: 0x0002610F
	public bool GetIsOutOfBounds()
	{
		return this.isOutOfBounds;
	}

	// Token: 0x0400000F RID: 15
	public int borderLeft;

	// Token: 0x04000010 RID: 16
	public int borderRight;

	// Token: 0x04000011 RID: 17
	public int borderTop;

	// Token: 0x04000012 RID: 18
	public int borderBottom;

	// Token: 0x04000013 RID: 19
	private Camera parentCamera;

	// Token: 0x04000014 RID: 20
	private bool isOutOfBounds;
}

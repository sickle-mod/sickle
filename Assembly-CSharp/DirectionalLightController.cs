using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class DirectionalLightController : MonoBehaviour
{
	// Token: 0x0600029A RID: 666 RVA: 0x0002995D File Offset: 0x00027B5D
	private void Start()
	{
		this.initialRotation = -120f;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x0005D2EC File Offset: 0x0005B4EC
	private void Update()
	{
		if (!this.introductionPanel.activeSelf)
		{
			this.rotation = new Vector3(126f, this.initialRotation + this.swivel.transform.eulerAngles.y, -126f);
			base.transform.localRotation = Quaternion.Euler(this.rotation);
		}
	}

	// Token: 0x040001F5 RID: 501
	public GameObject swivel;

	// Token: 0x040001F6 RID: 502
	private float initialRotation;

	// Token: 0x040001F7 RID: 503
	private Vector3 rotation;

	// Token: 0x040001F8 RID: 504
	public GameObject introductionPanel;
}

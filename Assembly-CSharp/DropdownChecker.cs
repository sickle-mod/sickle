using System;
using HoneyFramework;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class DropdownChecker : MonoBehaviour
{
	// Token: 0x0600029D RID: 669 RVA: 0x0002996A File Offset: 0x00027B6A
	private void Start()
	{
		CameraControler.Instance.dropdowns.Add(this);
	}

	// Token: 0x0600029E RID: 670 RVA: 0x0002997C File Offset: 0x00027B7C
	private void Update()
	{
		if (base.transform.childCount != 3)
		{
			this.isDropdownDown = true;
			return;
		}
		this.isDropdownDown = false;
	}

	// Token: 0x040001F9 RID: 505
	public bool isDropdownDown;
}

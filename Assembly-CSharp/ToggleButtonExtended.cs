using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200016C RID: 364
public class ToggleButtonExtended : MonoBehaviour
{
	// Token: 0x06000A75 RID: 2677 RVA: 0x0007D818 File Offset: 0x0007BA18
	public void EnableToggleButtonImage(bool state)
	{
		if (this.toggle && this.toggle.targetGraphic)
		{
			this.toggle.targetGraphic.color = new Color(1f, 1f, 1f, state ? 0f : 1f);
		}
	}

	// Token: 0x04000928 RID: 2344
	[SerializeField]
	private Toggle toggle;
}

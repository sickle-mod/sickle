using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000107 RID: 263
public class ShowroomModelTypeButtonController : MonoBehaviour
{
	// Token: 0x0600088C RID: 2188 RVA: 0x0002DC90 File Offset: 0x0002BE90
	private void Start()
	{
		this.image = base.GetComponent<Image>();
		this.button = base.GetComponent<Button>();
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0002DCAA File Offset: 0x0002BEAA
	public void SwitchButtonSprite(bool selfClick = false)
	{
		if (this.button.interactable || selfClick)
		{
			this.button.interactable = false;
			return;
		}
		if (!this.button.interactable)
		{
			this.button.interactable = true;
		}
	}

	// Token: 0x0400071F RID: 1823
	private Image image;

	// Token: 0x04000720 RID: 1824
	private Button button;
}

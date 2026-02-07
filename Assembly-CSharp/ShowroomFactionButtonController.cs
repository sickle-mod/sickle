using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000105 RID: 261
public class ShowroomFactionButtonController : MonoBehaviour
{
	// Token: 0x06000886 RID: 2182 RVA: 0x0002DC76 File Offset: 0x0002BE76
	private void Awake()
	{
		this.button = base.GetComponent<Button>();
		this.buttonImage = base.GetComponent<Image>();
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00078C18 File Offset: 0x00076E18
	public void TurnOnAndOff(bool on)
	{
		if (on)
		{
			WorldSFXManager.PlaySound(SoundEnum.GuiUpperBeltClick, AudioSourceType.Buttons);
			this.button.interactable = false;
			this.buttonImage.material = null;
			return;
		}
		if (!this.deactivated && this.button != null)
		{
			this.button.interactable = true;
			this.buttonImage.material = this.uisepia;
		}
	}

	// Token: 0x0400071B RID: 1819
	private Button button;

	// Token: 0x0400071C RID: 1820
	private Image buttonImage;

	// Token: 0x0400071D RID: 1821
	public Material uisepia;

	// Token: 0x0400071E RID: 1822
	public bool deactivated;
}

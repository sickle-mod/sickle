using System;
using UnityEngine;

// Token: 0x020000F4 RID: 244
public class ExitGameMenuController : MonoBehaviour
{
	// Token: 0x060007FA RID: 2042 RVA: 0x0002D48B File Offset: 0x0002B68B
	public void YesButtonClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		Application.Quit();
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x0002D49E File Offset: 0x0002B69E
	public void NoButtonClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		base.gameObject.SetActive(false);
	}
}

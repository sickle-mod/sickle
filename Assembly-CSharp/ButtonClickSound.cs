using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200010A RID: 266
public class ButtonClickSound : MonoBehaviour
{
	// Token: 0x060008A3 RID: 2211 RVA: 0x00079C70 File Offset: 0x00077E70
	public void ClickSound()
	{
		if (SceneManager.GetActiveScene().name.Contains("main"))
		{
			WorldSFXManager.PlaySound(this.soundEnum, this.audioSourceType);
			return;
		}
		ButtonsSFXManager.Instance.PlaySound(this.soundEnum);
	}

	// Token: 0x04000766 RID: 1894
	[SerializeField]
	private SoundEnum soundEnum;

	// Token: 0x04000767 RID: 1895
	[SerializeField]
	private AudioSourceType audioSourceType;
}

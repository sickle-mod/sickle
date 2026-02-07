using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000114 RID: 276
public class ToggleGroupSound : MonoBehaviour
{
	// Token: 0x060008E5 RID: 2277 RVA: 0x0002E143 File Offset: 0x0002C343
	private void OnEnable()
	{
		this.currentToggle = this.GetCurrentActiveToggle();
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x0007A3E4 File Offset: 0x000785E4
	private Toggle GetCurrentActiveToggle()
	{
		foreach (Toggle toggle in this.toggles)
		{
			if (toggle.isOn)
			{
				return toggle;
			}
		}
		return null;
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0007A418 File Offset: 0x00078618
	public void PlaySound_OnToggleValueChanged()
	{
		Toggle currentActiveToggle = this.GetCurrentActiveToggle();
		if (this.currentToggle != currentActiveToggle)
		{
			this.currentToggle = currentActiveToggle;
			if (SceneManager.GetActiveScene().name == SceneController.SCENE_MAIN_NAME)
			{
				WorldSFXManager.PlaySound(this.soundEnum, this.audioSourceType);
				return;
			}
			ButtonsSFXManager.Instance.PlaySound(this.soundEnum);
		}
	}

	// Token: 0x04000811 RID: 2065
	[SerializeField]
	private SoundEnum soundEnum;

	// Token: 0x04000812 RID: 2066
	[SerializeField]
	private AudioSourceType audioSourceType;

	// Token: 0x04000813 RID: 2067
	[SerializeField]
	private Toggle[] toggles;

	// Token: 0x04000814 RID: 2068
	private Toggle currentToggle;
}

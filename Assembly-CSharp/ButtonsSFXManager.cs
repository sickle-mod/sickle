using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010B RID: 267
public class ButtonsSFXManager : MonoBehaviour
{
	// Token: 0x060008A5 RID: 2213 RVA: 0x0002DD34 File Offset: 0x0002BF34
	private void Awake()
	{
		ButtonsSFXManager.Instance = this;
		this.buttonsAudioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x00079CB8 File Offset: 0x00077EB8
	public void PlaySound(SoundEnum soundLabel)
	{
		AudioClip audioClip = null;
		foreach (SFXReference sfxreference in this.worldSFX.sfxSounds)
		{
			if (sfxreference.label == soundLabel.ToString())
			{
				audioClip = sfxreference.buildSoundToPlay;
			}
		}
		if (audioClip != null)
		{
			this.buttonsAudioSource.clip = audioClip;
			this.buttonsAudioSource.Stop();
			this.buttonsAudioSource.Play();
			return;
		}
		Debug.LogWarning("Missing sound: " + soundLabel.ToString());
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x00079D74 File Offset: 0x00077F74
	public void GetAllButtons()
	{
		this.buttonsForSound = Resources.FindObjectsOfTypeAll(typeof(Button)) as Button[];
		for (int i = 0; i < this.buttonsForSound.Length; i++)
		{
			if (this.buttonsForSound[i].GetComponents<PointerEventsController>().Length > 1)
			{
				this.allButtons.Add(this.buttonsForSound[i]);
			}
		}
	}

	// Token: 0x04000768 RID: 1896
	public static ButtonsSFXManager Instance;

	// Token: 0x04000769 RID: 1897
	public WorldSFX worldSFX;

	// Token: 0x0400076A RID: 1898
	public List<Button> allButtons;

	// Token: 0x0400076B RID: 1899
	private Button[] buttonsForSound;

	// Token: 0x0400076C RID: 1900
	[SerializeField]
	private AudioClip clickSoundToPlay;

	// Token: 0x0400076D RID: 1901
	[SerializeField]
	private AudioClip hooverSoundToPlay;

	// Token: 0x0400076E RID: 1902
	private AudioSource buttonsAudioSource;
}

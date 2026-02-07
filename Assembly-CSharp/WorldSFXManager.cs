using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000117 RID: 279
public class WorldSFXManager : MonoBehaviour
{
	// Token: 0x060008EA RID: 2282 RVA: 0x0002E164 File Offset: 0x0002C364
	private void Awake()
	{
		OptionsManager.UpdateAudioMixer(this.generalAudioMixer);
		this.sfxAudioSources = base.GetComponents<AudioSource>();
		this.sfxAudioSources[1].loop = true;
		WorldSFXManager.Instance = this;
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x0007A47C File Offset: 0x0007867C
	public static void PlaySound(SoundEnum soundLabel, AudioSourceType audioSourceType)
	{
		if (!OptionsManager.IsSoundsActive())
		{
			return;
		}
		AudioClip audioClip = null;
		foreach (SFXReference sfxreference in WorldSFXManager.Instance.GetComponent<SFXEnumSetup>().worldSFX.sfxSounds)
		{
			if (sfxreference.label == soundLabel.ToString())
			{
				audioClip = sfxreference.buildSoundToPlay;
			}
		}
		switch (audioSourceType)
		{
		case AudioSourceType.Buttons:
			WorldSFXManager.Instance.actualAudioSource = WorldSFXManager.Instance.sfxAudioSources[0];
			goto IL_0116;
		case AudioSourceType.Loops:
			WorldSFXManager.Instance.actualAudioSource = WorldSFXManager.Instance.sfxAudioSources[1];
			goto IL_0116;
		case AudioSourceType.StartTurnTheme:
			WorldSFXManager.Instance.actualAudioSource = WorldSFXManager.Instance.sfxAudioSources[2];
			goto IL_0116;
		case AudioSourceType.WorldSfx:
			WorldSFXManager.Instance.actualAudioSource = WorldSFXManager.Instance.sfxAudioSources[3];
			goto IL_0116;
		case AudioSourceType.WinTheme:
			WorldSFXManager.Instance.actualAudioSource = WorldSFXManager.Instance.sfxAudioSources[4];
			goto IL_0116;
		}
		WorldSFXManager.Instance.actualAudioSource = WorldSFXManager.Instance.sfxAudioSources[3];
		IL_0116:
		if (audioClip != null)
		{
			WorldSFXManager.Instance.actualAudioSource.clip = audioClip;
			WorldSFXManager.Instance.actualAudioSource.Stop();
			WorldSFXManager.Instance.actualAudioSource.Play();
			return;
		}
		Debug.LogWarning("Missing sound: " + soundLabel.ToString());
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x0007A604 File Offset: 0x00078804
	public static void PlaySound(SoundEnum soundLabel, AudioSourceType audioSourceType, SoundGroup soundGroup)
	{
		AudioClip audioClip = null;
		foreach (SFXReference sfxreference in WorldSFXManager.Instance.GetComponent<SFXEnumSetup>().worldSFX.sfxSounds)
		{
			if (sfxreference.label == soundLabel.ToString())
			{
				audioClip = sfxreference.buildSoundToPlay;
			}
		}
		switch (audioSourceType)
		{
		case AudioSourceType.Buttons:
			WorldSFXManager.Instance.actualAudioSource = WorldSFXManager.Instance.sfxAudioSources[0];
			break;
		case AudioSourceType.Loops:
			WorldSFXManager.Instance.actualAudioSource = WorldSFXManager.Instance.sfxAudioSources[1];
			break;
		case AudioSourceType.EnemyMove:
			WorldSFXManager.Instance.actualAudioSource = WorldSFXManager.Instance.sfxAudioSources[4];
			break;
		case AudioSourceType.StartTurnTheme:
			WorldSFXManager.Instance.actualAudioSource = WorldSFXManager.Instance.sfxAudioSources[2];
			break;
		case AudioSourceType.WorldSfx:
			WorldSFXManager.Instance.actualAudioSource = WorldSFXManager.Instance.sfxAudioSources[3];
			break;
		default:
			WorldSFXManager.Instance.actualAudioSource = WorldSFXManager.Instance.sfxAudioSources[3];
			break;
		}
		if (audioClip != null)
		{
			WorldSFXManager.Instance.actualAudioSource.clip = audioClip;
			WorldSFXManager.Instance.actualAudioSource.Stop();
			WorldSFXManager.Instance.actualAudioSource.Play();
			return;
		}
		Debug.LogWarning("Missing sound: " + soundLabel.ToString());
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0002E191 File Offset: 0x0002C391
	public static void StopLoopSFX()
	{
		WorldSFXManager.Instance.sfxAudioSources[1].DOFade(0f, 1.3f);
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x0007A780 File Offset: 0x00078980
	public static void OnUndo()
	{
		for (int i = 0; i < WorldSFXManager.Instance.sfxAudioSources.Length; i++)
		{
			if (WorldSFXManager.Instance.sfxAudioSources[i] != null)
			{
				WorldSFXManager.Instance.sfxAudioSources[i].Stop();
			}
		}
	}

	// Token: 0x04000818 RID: 2072
	public static WorldSFXManager Instance;

	// Token: 0x04000819 RID: 2073
	[SerializeField]
	private AudioMixer generalAudioMixer;

	// Token: 0x0400081A RID: 2074
	private AudioSource actualAudioSource;

	// Token: 0x0400081B RID: 2075
	[SerializeField]
	private AudioSource[] sfxAudioSources;
}

using System;
using System.Collections;
using HoneyFramework;
using I2.Loc;
using Reworked.Options;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Multiplayer.Notifications;
using Scythe.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200005B RID: 91
public class OptionsManager : MonoBehaviour
{
	// Token: 0x14000018 RID: 24
	// (add) Token: 0x060002D1 RID: 721 RVA: 0x000608A0 File Offset: 0x0005EAA0
	// (remove) Token: 0x060002D2 RID: 722 RVA: 0x000608D4 File Offset: 0x0005EAD4
	public static event OptionsManager.LanguageChanged OnLanguageChanged;

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x060002D3 RID: 723 RVA: 0x00060908 File Offset: 0x0005EB08
	// (remove) Token: 0x060002D4 RID: 724 RVA: 0x0006093C File Offset: 0x0005EB3C
	public static event Action OnNotificationsSettingChanged;

	// Token: 0x060002D5 RID: 725 RVA: 0x00060970 File Offset: 0x0005EB70
	private void Awake()
	{
		if (!PlatformManager.IsStandalone)
		{
			this.fastForward.onValueChanged.AddListener(new UnityAction<bool>(this.OnFastForwardToggleChanged));
			this.acceptGains.onValueChanged.AddListener(new UnityAction<bool>(this.OnAcceptGainsToggleChanged));
			this.notificationsDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnNotificationsSettingValueChanged));
		}
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x000609D8 File Offset: 0x0005EBD8
	private void OnEnable()
	{
		this.silence = true;
		this.LoadMapColorPrefs();
		if (PlatformManager.IsStandalone)
		{
			this.graphicSettingsPanel.Init();
			this.LoadMusicSlider(PlayerPrefs.GetFloat(OptionsManager.PREFS_KEY_MUSIC_VOLUME, 0.5f));
			this.LoadSoundsSlider(PlayerPrefs.GetFloat(OptionsManager.PREFS_KEY_SOUNDS_VOLUME, 0.5f));
			this.showTooltips.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_TOOLTIPS_ACTIVE, OptionsManager.TOOLTIPS_DEFAULT));
			this.dragAndDrop.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_DRAG_AND_DROP, 0));
			this.traditionalTracks.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_TRADITIONAL_TRACKS, 0));
			this.shortcutsEnabled.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_SHORTCUTS_ENABLED, 1));
			this.cameraAutorotation.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_CAMERA_ROTATION, 0));
			this.cameraAnimations.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_CAMERA_ANIMATIONS, 1));
		}
		else
		{
			this.acceptGains.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_ACCEPT_GAINS, 1));
			this.fastForward.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_FAST_FORWARD, 0));
			this.notificationsDropdown.value = PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_NOTIFICATIONS, 1);
			this.allowSleepMode.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_ALLOW_SLEEP_MODE, 1));
		}
		this.musicOn.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_MUSIC_ON, 1));
		this.soundsOn.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_SOUNDS_ON, 1));
		this.showWarnings.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_WARNINGS_ACTIVE, 1));
		this.confirmActions.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_CONFIRM_ACTIONS, 1));
		this.actionAssist.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_ACTION_ASSIST, 1));
		if (SceneManager.GetActiveScene().name.Contains("menu"))
		{
			if (PlatformManager.IsStandalone)
			{
				this.loginAccountsButton.gameObject.SetActive(true);
				this.undoPanel.SetActive(false);
			}
		}
		else
		{
			if (this.loginAccountsButton != null)
			{
				this.loginAccountsButton.gameObject.SetActive(false);
			}
			if (this.undoPanel != null)
			{
				this.undoPanel.SetActive(!GameController.GameManager.IsMultiplayer);
			}
			if (this.undoSelection != null)
			{
				this.undoSelection.value = GameController.GameManager.UndoType;
			}
		}
		this.UpdateLanguageRaw();
		this.silence = false;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00029C1C File Offset: 0x00027E1C
	public static bool IsActionAssist()
	{
		return OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_ACTION_ASSIST, 1)) || GameController.GameManager.IsCampaign;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00029C3C File Offset: 0x00027E3C
	public static bool IsTraditionalTracks()
	{
		return OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_TRADITIONAL_TRACKS, 0)) && !GameController.GameManager.IsCampaign;
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00029C5F File Offset: 0x00027E5F
	public static bool IsFastForward()
	{
		return OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_FAST_FORWARD, 0)) && !GameController.GameManager.IsCampaign && !GameController.GameManager.GameFinished;
	}

	// Token: 0x060002DA RID: 730 RVA: 0x00029C8E File Offset: 0x00027E8E
	public static bool IsConfirmActions()
	{
		return OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_CONFIRM_ACTIONS, 1)) && !GameController.GameManager.IsCampaign;
	}

	// Token: 0x060002DB RID: 731 RVA: 0x00029CB1 File Offset: 0x00027EB1
	public static bool IsAcceptGains()
	{
		return OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_ACCEPT_GAINS, 1));
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00029CC3 File Offset: 0x00027EC3
	public static bool IsWarningsActive()
	{
		return OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_WARNINGS_ACTIVE, 1)) && !GameController.GameManager.IsCampaign;
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00029CE6 File Offset: 0x00027EE6
	public static bool IsSoundsActive()
	{
		return OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_SOUNDS_ON, 1));
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00029CF8 File Offset: 0x00027EF8
	public static bool IsMusicActive()
	{
		return OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_MUSIC_ON, 1));
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00029D0A File Offset: 0x00027F0A
	public static bool IsShotcutsActive()
	{
		return !GameController.GameManager.IsCampaign;
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x00029D19 File Offset: 0x00027F19
	public static bool IsCameraAnimationsActive()
	{
		return OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_CAMERA_ANIMATIONS, 1));
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x00029D2B File Offset: 0x00027F2B
	public static bool IsDragAndDropActive()
	{
		return OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_DRAG_AND_DROP, 0)) && !GameController.GameManager.IsCampaign && PlatformManager.IsStandalone;
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x00029D52 File Offset: 0x00027F52
	public void UpdateLanguage()
	{
		base.StartCoroutine(this.UpdateLanguageTexts());
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x00029D61 File Offset: 0x00027F61
	private IEnumerator UpdateLanguageTexts()
	{
		yield return new WaitForEndOfFrame();
		this.UpdateLanguageRaw();
		yield break;
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x00060C80 File Offset: 0x0005EE80
	private void UpdateLanguageRaw()
	{
		this.mapColorPreset.options[0].text = ScriptLocalization.Get("Common/Default");
		if (this.mapColorPreset.value == 0)
		{
			this.mapColorPreset.captionText.text = ScriptLocalization.Get("Common/Default");
		}
		this.mapColorPreset.options[1].text = ScriptLocalization.Get("Options/PaletteDoomAndGloom");
		if (this.mapColorPreset.value == 1)
		{
			this.mapColorPreset.captionText.text = ScriptLocalization.Get("Options/PaletteDoomAndGloom");
		}
		this.mapColorPreset.options[2].text = ScriptLocalization.Get("Options/PaletteMoonlight");
		if (this.mapColorPreset.value == 2)
		{
			this.mapColorPreset.captionText.text = ScriptLocalization.Get("Options/PaletteMoonlight");
		}
		this.mapColorPreset.options[3].text = ScriptLocalization.Get("Options/PaletteBlackAndWhite");
		if (this.mapColorPreset.value == 3)
		{
			this.mapColorPreset.captionText.text = ScriptLocalization.Get("Options/PaletteBlackAndWhite");
		}
		this.mapColorPreset.options[4].text = ScriptLocalization.Get("Options/PaletteVintage1920");
		if (this.mapColorPreset.value == 4)
		{
			this.mapColorPreset.captionText.text = ScriptLocalization.Get("Options/PaletteVintage1920");
		}
		this.UpdateNotificationDropdown();
		if (OptionsManager.OnLanguageChanged != null)
		{
			OptionsManager.OnLanguageChanged();
		}
		if (this.undoSelection != null)
		{
			for (int i = 0; i < this.undoSelection.options.Count; i++)
			{
				this.undoSelection.options[i].text = ScriptLocalization.Get("Common/Undo" + Enum.GetName(typeof(UndoTypes), (UndoTypes)i));
			}
			this.undoSelection.RefreshShownValue();
		}
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00060E7C File Offset: 0x0005F07C
	private void UpdateNotificationDropdown()
	{
		if (PlatformManager.IsMobile)
		{
			for (int i = 0; i < this.notificationsDropdown.options.Count; i++)
			{
				this.notificationsDropdown.options[i].text = OptionsManager.GetNotificationsSettingLocalization((NotificationsSetting)i);
			}
			this.notificationsDropdown.captionText.text = OptionsManager.GetNotificationsSettingLocalization((NotificationsSetting)this.notificationsDropdown.value);
		}
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x00060EE8 File Offset: 0x0005F0E8
	private static string GetNotificationsSettingLocalization(NotificationsSetting notificationsSetting)
	{
		switch (notificationsSetting)
		{
		case NotificationsSetting.None:
			return ScriptLocalization.Get("Options/NotificationsNone");
		case NotificationsSetting.All:
			return ScriptLocalization.Get("Options/NotificationsAll");
		case NotificationsSetting.InvitesOnly:
			return ScriptLocalization.Get("Options/NotificationsInvitesOnly");
		default:
			Debug.LogError("Undefined NotificatiosSetting value " + notificationsSetting.ToString());
			return "Undefined";
		}
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x00060F4C File Offset: 0x0005F14C
	public void LoadMapColorPrefs()
	{
		this.mapColorPreset.value = PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_MAP_COLOR_PRESET, 4);
		if (PlatformManager.IsStandalone && this.mapColorPreset.value == 0)
		{
			this.mapColorPreset.transform.Find("Label").GetComponent<Text>().text = ScriptLocalization.Get("Common/Default");
		}
		this.mapColorDynamic.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_MAP_COLOR_DYNAMIC, 1));
		this.hexTypeColor.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_HEXTYPE_COLOR, 0));
		this.hexTypeMarker.isOn = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_HEXTYPE_MARKER, 1));
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x00061000 File Offset: 0x0005F200
	public void LoadOptionsPrefs()
	{
		if (OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_SOUNDS_ON, 1)))
		{
			this.generalAudioMixer.SetFloat(OptionsManager.MIXER_VAR_UI_VALUE, OptionsManager.LinearToDecibel(PlayerPrefs.GetFloat(OptionsManager.PREFS_KEY_SOUNDS_VOLUME, 0.5f)));
		}
		else
		{
			this.generalAudioMixer.SetFloat(OptionsManager.MIXER_VAR_UI_VALUE, OptionsManager.LinearToDecibel(0f));
		}
		if (OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_MUSIC_ON, 1)))
		{
			this.generalAudioMixer.SetFloat(OptionsManager.MIXER_VAR_MUSIC_VALUE, OptionsManager.LinearToDecibel(PlayerPrefs.GetFloat(OptionsManager.PREFS_KEY_MUSIC_VOLUME, 0.5f)));
		}
		else
		{
			this.generalAudioMixer.SetFloat(OptionsManager.MIXER_VAR_MUSIC_VALUE, OptionsManager.LinearToDecibel(0f));
		}
		this.ApplyMapPreset(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_MAP_COLOR_PRESET, 4));
		this.ApplyHexTypeColor();
		this.ApplyHexTypeMarker();
		this.ApplyTraditionalTracks();
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x00029D70 File Offset: 0x00027F70
	public void EnableOptionsPanel(bool enable)
	{
		if (!enable)
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_account_settings);
		}
		else
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.settings, Contexts.outgame);
		}
		this.OptionsPanel.SetActive(enable);
	}

	// Token: 0x060002EA RID: 746 RVA: 0x000610D8 File Offset: 0x0005F2D8
	private void LoadMusicSlider(float musicVolume)
	{
		this.musicSlider.value = musicVolume;
		if (OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_MUSIC_ON, 1)))
		{
			this.musicSlider.interactable = true;
			this.generalAudioMixer.SetFloat(OptionsManager.MIXER_VAR_MUSIC_VALUE, OptionsManager.LinearToDecibel(musicVolume));
			return;
		}
		this.musicSlider.interactable = false;
	}

	// Token: 0x060002EB RID: 747 RVA: 0x00061134 File Offset: 0x0005F334
	private void LoadSoundsSlider(float soundsVolume)
	{
		this.soundsSlider.value = soundsVolume;
		if (OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_SOUNDS_ON, 1)))
		{
			this.soundsSlider.interactable = true;
			this.generalAudioMixer.SetFloat(OptionsManager.MIXER_VAR_UI_VALUE, OptionsManager.LinearToDecibel(soundsVolume));
			return;
		}
		this.soundsSlider.interactable = false;
	}

	// Token: 0x060002EC RID: 748 RVA: 0x00029D97 File Offset: 0x00027F97
	public void OnMusicSliderChanged(float musicVolume)
	{
		PlayerPrefs.SetFloat(OptionsManager.PREFS_KEY_MUSIC_VOLUME, musicVolume);
		PlayerPrefs.Save();
		if (OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_MUSIC_ON, 1)))
		{
			this.generalAudioMixer.SetFloat(OptionsManager.MIXER_VAR_MUSIC_VALUE, OptionsManager.LinearToDecibel(musicVolume));
		}
	}

	// Token: 0x060002ED RID: 749 RVA: 0x00061190 File Offset: 0x0005F390
	public void OnSoundsSliderChanged(float soundsVolume)
	{
		PlayerPrefs.SetFloat(OptionsManager.PREFS_KEY_SOUNDS_VOLUME, soundsVolume);
		PlayerPrefs.Save();
		if (OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_SOUNDS_ON, 1)))
		{
			this.generalAudioMixer.SetFloat(OptionsManager.MIXER_VAR_UI_VALUE, OptionsManager.LinearToDecibel(soundsVolume));
			AudioSource component = base.GetComponent<AudioSource>();
			if (!component.isPlaying && !this.silence)
			{
				component.Play();
			}
		}
	}

	// Token: 0x060002EE RID: 750 RVA: 0x000611F4 File Offset: 0x0005F3F4
	public void CreditsOpen()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_credits_button);
		this.Credits.SetActive(true);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.credits, Contexts.outgame);
		if (PlatformManager.IsMobile)
		{
			base.StartCoroutine(this.CloseAllDropboxes());
		}
	}

	// Token: 0x060002EF RID: 751 RVA: 0x00029DD2 File Offset: 0x00027FD2
	public void CreditsExit()
	{
		this.Credits.SetActive(false);
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_exit_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.settings, Contexts.outgame);
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00061244 File Offset: 0x0005F444
	public void OnMusicToggleChanged(bool musicStateFromToggle)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_MUSIC_ON, OptionsManager.BoolToInt(musicStateFromToggle));
		PlayerPrefs.Save();
		if (this.musicOn.isOn)
		{
			if (PlatformManager.IsStandalone)
			{
				this.musicSlider.interactable = true;
			}
			this.generalAudioMixer.SetFloat(OptionsManager.MIXER_VAR_MUSIC_VALUE, OptionsManager.LinearToDecibel(PlayerPrefs.GetFloat(OptionsManager.PREFS_KEY_MUSIC_VOLUME, 0.5f)));
			return;
		}
		if (PlatformManager.IsStandalone)
		{
			this.musicSlider.interactable = false;
		}
		this.generalAudioMixer.SetFloat(OptionsManager.MIXER_VAR_MUSIC_VALUE, OptionsManager.LinearToDecibel(0f));
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x000612E8 File Offset: 0x0005F4E8
	public void OnSoundsToggleChanged(bool soundStateFromToggle)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_SOUNDS_ON, OptionsManager.BoolToInt(soundStateFromToggle));
		PlayerPrefs.Save();
		if (this.soundsOn.isOn)
		{
			if (PlatformManager.IsStandalone)
			{
				this.soundsSlider.interactable = true;
			}
			this.generalAudioMixer.SetFloat(OptionsManager.MIXER_VAR_UI_VALUE, OptionsManager.LinearToDecibel(PlayerPrefs.GetFloat(OptionsManager.PREFS_KEY_SOUNDS_VOLUME, 0.5f)));
			return;
		}
		if (PlatformManager.IsStandalone)
		{
			this.soundsSlider.interactable = false;
		}
		this.generalAudioMixer.SetFloat(OptionsManager.MIXER_VAR_UI_VALUE, OptionsManager.LinearToDecibel(0f));
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x00029DFF File Offset: 0x00027FFF
	public void OnAllowSleepModeToggleChanged(bool allowSleepModeState)
	{
		PlayerPrefs.SetInt(OptionsManager.PREFS_ALLOW_SLEEP_MODE, OptionsManager.BoolToInt(allowSleepModeState));
		PlayerPrefs.Save();
		if (allowSleepModeState)
		{
			Screen.sleepTimeout = -2;
			return;
		}
		Screen.sleepTimeout = -1;
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x0006138C File Offset: 0x0005F58C
	public static void UpdateAudioMixer(AudioMixer audioMixer)
	{
		bool flag = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_MUSIC_ON, 1));
		bool flag2 = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_SOUNDS_ON, 1));
		if (flag)
		{
			audioMixer.SetFloat(OptionsManager.MIXER_VAR_MUSIC_VALUE, OptionsManager.LinearToDecibel(PlayerPrefs.GetFloat(OptionsManager.PREFS_KEY_MUSIC_VOLUME, 0.5f)));
		}
		else
		{
			audioMixer.SetFloat(OptionsManager.MIXER_VAR_MUSIC_VALUE, OptionsManager.LinearToDecibel(0f));
		}
		if (flag2)
		{
			audioMixer.SetFloat(OptionsManager.MIXER_VAR_UI_VALUE, OptionsManager.LinearToDecibel(PlayerPrefs.GetFloat(OptionsManager.PREFS_KEY_SOUNDS_VOLUME, 0.5f)));
			return;
		}
		audioMixer.SetFloat(OptionsManager.MIXER_VAR_UI_VALUE, OptionsManager.LinearToDecibel(0f));
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00029E27 File Offset: 0x00028027
	public void OnConfirmActionsToggleChanged(bool confirmState)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_CONFIRM_ACTIONS, OptionsManager.BoolToInt(confirmState));
		PlayerPrefs.Save();
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x00029E4A File Offset: 0x0002804A
	public void OnFastForwardToggleChanged(bool confirmState)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_FAST_FORWARD, OptionsManager.BoolToInt(confirmState));
		PlayerPrefs.Save();
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x00029E6D File Offset: 0x0002806D
	public void OnAcceptGainsToggleChanged(bool confirmState)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_ACCEPT_GAINS, OptionsManager.BoolToInt(confirmState));
		PlayerPrefs.Save();
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00029E90 File Offset: 0x00028090
	private void OnNotificationsSettingValueChanged(int newValue)
	{
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_NOTIFICATIONS, newValue);
		PlayerPrefs.Save();
		if (OptionsManager.OnNotificationsSettingChanged != null)
		{
			OptionsManager.OnNotificationsSettingChanged();
		}
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00061430 File Offset: 0x0005F630
	public void OnDragAndDropToggleChanged(bool state)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_DRAG_AND_DROP, OptionsManager.BoolToInt(state));
		PlayerPrefs.Save();
		if (SceneManager.GetActiveScene().name.Contains("main") && !GameController.GameManager.IsCampaign && !GameController.GameManager.SpectatorMode)
		{
			GameController.Instance.SetDragAndDrop(state);
		}
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00029EB3 File Offset: 0x000280B3
	public void OnWarningsToggleChanged(bool warningsState)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_WARNINGS_ACTIVE, OptionsManager.BoolToInt(warningsState));
		PlayerPrefs.Save();
	}

	// Token: 0x060002FA RID: 762 RVA: 0x00029ED6 File Offset: 0x000280D6
	public void OnTooltipsToggleChanged(bool tooltipsState)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_TOOLTIPS_ACTIVE, OptionsManager.BoolToInt(tooltipsState));
		PlayerPrefs.Save();
	}

	// Token: 0x060002FB RID: 763 RVA: 0x00029EF9 File Offset: 0x000280F9
	public void OnMapColorLockToggleChanged(bool tooltipsState)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_MAP_COLOR_DYNAMIC, OptionsManager.BoolToInt(tooltipsState));
		PlayerPrefs.Save();
		this.ApplyMapColor();
	}

	// Token: 0x060002FC RID: 764 RVA: 0x00029F22 File Offset: 0x00028122
	public void OnHexTypeColorToggleChanged(bool tooltipsState)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_HEXTYPE_COLOR, OptionsManager.BoolToInt(tooltipsState));
		PlayerPrefs.Save();
		this.ApplyHexTypeColor();
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00029F4B File Offset: 0x0002814B
	public void OnHexTypeMarkerToggleChanged(bool tooltipsState)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_HEXTYPE_MARKER, OptionsManager.BoolToInt(tooltipsState));
		PlayerPrefs.Save();
		this.ApplyHexTypeMarker();
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00029F74 File Offset: 0x00028174
	public void OnTraditionalTracksToggleChanged(bool state)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_TRADITIONAL_TRACKS, OptionsManager.BoolToInt(state));
		PlayerPrefs.Save();
		this.ApplyTraditionalTracks();
		if (CameraControler.Instance != null)
		{
			CameraControler.Instance.RotateTracksToCamera();
		}
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0006149C File Offset: 0x0005F69C
	public void OnActionAssistToggleChanged(bool state)
	{
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_ACTION_ASSIST, OptionsManager.BoolToInt(state));
		PlayerPrefs.Save();
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		if (PlatformManager.IsStandalone && !state && GameController.Instance != null && GameController.Instance.matPlayer != null)
		{
			for (int i = 0; i < 4; i++)
			{
				GameController.Instance.matPlayer.matSection[i].GetComponent<Animator>().Play("PlayerMatSectionTopIconsHide");
			}
		}
	}

	// Token: 0x06000300 RID: 768 RVA: 0x00029FB4 File Offset: 0x000281B4
	public void OnEnableKeyboardShortcutsToggleChanged(bool state)
	{
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_SHORTCUTS_ENABLED, OptionsManager.BoolToInt(state));
		PlayerPrefs.Save();
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
	}

	// Token: 0x06000301 RID: 769 RVA: 0x00029FD7 File Offset: 0x000281D7
	public void OnEnableCameraAutorotationToggleChanged(bool state)
	{
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_CAMERA_ROTATION, OptionsManager.BoolToInt(state));
		PlayerPrefs.Save();
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
	}

	// Token: 0x06000302 RID: 770 RVA: 0x00029FFA File Offset: 0x000281FA
	public void OnEnableCameraAnimationsToggleChanged(bool state)
	{
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_CAMERA_ANIMATIONS, OptionsManager.BoolToInt(state));
		PlayerPrefs.Save();
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0002A01D File Offset: 0x0002821D
	public void OptionsApplyButtonClicked()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_apply_button);
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0002A025 File Offset: 0x00028225
	public void OnShowLoginSettingsClicked()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_account_settings);
		if (PlatformManager.IsMobile)
		{
			base.StartCoroutine(this.CloseAllDropboxes());
			return;
		}
		this.OptionsPanel.SetActive(false);
	}

	// Token: 0x06000305 RID: 773 RVA: 0x0002A04F File Offset: 0x0002824F
	public void ShowOptionsPanel()
	{
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.settings, Contexts.outgame);
		this.OptionsPanel.SetActive(true);
	}

	// Token: 0x06000306 RID: 774 RVA: 0x00029B95 File Offset: 0x00027D95
	public static int BoolToInt(bool val)
	{
		if (!val)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00029B9D File Offset: 0x00027D9D
	public static bool IntToBool(int val)
	{
		return val != 0;
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00061524 File Offset: 0x0005F724
	private static float LinearToDecibel(float linear)
	{
		float num;
		if (linear != 0f)
		{
			num = 20f * Mathf.Log10(linear);
		}
		else
		{
			num = -144f;
		}
		return num;
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0002A06A File Offset: 0x0002826A
	private static float DecibelToLinear(float dB)
	{
		return Mathf.Pow(10f, dB / 20f);
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0002A07D File Offset: 0x0002827D
	public void ApplyMapPreset(int id)
	{
		if (id < this.presets.Length)
		{
			PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_MAP_COLOR_PRESET, id);
			PlayerPrefs.Save();
			this.ApplyMapColor();
		}
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00061550 File Offset: 0x0005F750
	private void ApplyMapColor()
	{
		int @int = PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_MAP_COLOR_PRESET, 0);
		bool flag = OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_MAP_COLOR_DYNAMIC, 1));
		if (this.mapContainer != null)
		{
			foreach (MeshRenderer meshRenderer in this.mapContainer.GetComponentsInChildren<MeshRenderer>())
			{
				this.presets[@int].Apply(meshRenderer.material, flag);
			}
		}
		if (this.traditionalTracksParent != null)
		{
			foreach (SpriteRenderer spriteRenderer in this.tracks)
			{
				this.presets[@int].Apply(spriteRenderer.material, flag);
			}
		}
		if (this.logosForRotation != null)
		{
			foreach (SpriteRenderer spriteRenderer2 in this.logosForRotation.GetComponentsInChildren<SpriteRenderer>())
			{
				this.presets[@int].Apply(spriteRenderer2.material, flag);
			}
		}
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0002A0A0 File Offset: 0x000282A0
	private void ApplyHexTypeColor()
	{
		if (this.resourceTypeLayer != null)
		{
			this.resourceTypeLayer.SetHexTypeColorsActive(OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_HEXTYPE_COLOR, 1)));
		}
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0002A0CB File Offset: 0x000282CB
	private void ApplyHexTypeMarker()
	{
		if (this.resourceTypeLayer != null)
		{
			this.resourceTypeLayer.OnToggle(OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_HEXTYPE_MARKER, 1)));
		}
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0002A0F6 File Offset: 0x000282F6
	private void ApplyTraditionalTracks()
	{
		if (this.traditionalTracksParent != null)
		{
			this.traditionalTracksParent.SetActive(OptionsManager.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_TRADITIONAL_TRACKS, 0)));
		}
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00061644 File Offset: 0x0005F844
	public void OpenRulebook()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_rules_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.external_web_page, Contexts.outgame);
		Application.OpenURL(this.GetInstructionURL(GenericSingletonClass<DataAggregator>.Instance.instructionModelData.instructionURLDictionary));
		if (GameServiceController.Instance.InvadersFromAfarUnlocked())
		{
			this.OpenIFARulebook();
		}
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0002A121 File Offset: 0x00028321
	public void OpenIFARulebook()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_ifa_rules_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.external_web_page, Contexts.outgame);
		Application.OpenURL(this.GetInstructionURL(GenericSingletonClass<DataAggregator>.Instance.instructionDLCModelData.instructionURLDictionary));
	}

	// Token: 0x06000311 RID: 785 RVA: 0x000616A0 File Offset: 0x0005F8A0
	private string GetInstructionURL(EnumInstructionModelDictionary urlDictionary)
	{
		string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
		uint num = global::<PrivateImplementationDetails>.ComputeStringHash(currentLanguageCode);
		if (num <= 1195724803U)
		{
			if (num <= 1162757945U)
			{
				if (num != 637978675U)
				{
					if (num != 1092248970U)
					{
						if (num == 1162757945U)
						{
							if (currentLanguageCode == "pl")
							{
								return urlDictionary[InstructionLanguage.pl];
							}
						}
					}
					else if (currentLanguageCode == "en")
					{
						return urlDictionary[InstructionLanguage.en];
					}
				}
				else if (currentLanguageCode == "zh-CN")
				{
					return urlDictionary[InstructionLanguage.zhCN];
				}
			}
			else if (num != 1176137065U)
			{
				if (num != 1194886160U)
				{
					if (num == 1195724803U)
					{
						if (currentLanguageCode == "tr")
						{
							return urlDictionary[InstructionLanguage.tr];
						}
					}
				}
				else if (currentLanguageCode == "it")
				{
					return urlDictionary[InstructionLanguage.it];
				}
			}
			else if (currentLanguageCode == "es")
			{
				return urlDictionary[InstructionLanguage.es];
			}
		}
		else if (num <= 1545391778U)
		{
			if (num != 1213488160U)
			{
				if (num != 1461901041U)
				{
					if (num == 1545391778U)
					{
						if (currentLanguageCode == "de")
						{
							return urlDictionary[InstructionLanguage.de];
						}
					}
				}
				else if (currentLanguageCode == "fr")
				{
					return urlDictionary[InstructionLanguage.fr];
				}
			}
			else if (currentLanguageCode == "ru")
			{
				return urlDictionary[InstructionLanguage.ru];
			}
		}
		else if (num <= 1664981344U)
		{
			if (num != 1630957159U)
			{
				if (num == 1664981344U)
				{
					if (currentLanguageCode == "pt-BR")
					{
						return urlDictionary[InstructionLanguage.ptBR];
					}
				}
			}
			else if (currentLanguageCode == "nl")
			{
				return urlDictionary[InstructionLanguage.nl];
			}
		}
		else if (num != 1816099348U)
		{
			if (num == 3973517379U)
			{
				if (currentLanguageCode == "zh-TW")
				{
					return urlDictionary[InstructionLanguage.zhTW];
				}
			}
		}
		else if (currentLanguageCode == "ja")
		{
			return urlDictionary[InstructionLanguage.ja];
		}
		return urlDictionary[InstructionLanguage.en];
	}

	// Token: 0x06000312 RID: 786 RVA: 0x000618F4 File Offset: 0x0005FAF4
	public void OnUndoModeSelected()
	{
		if (SceneManager.GetActiveScene().name.Contains("main"))
		{
			GameController.GameManager.UndoType = this.undoSelection.value;
			GameController.Instance.undoController.UpdateUndoType();
		}
	}

	// Token: 0x06000313 RID: 787 RVA: 0x0002A151 File Offset: 0x00028351
	private IEnumerator CloseAllDropboxes()
	{
		yield return new WaitForEndOfFrame();
		if (this.languageSelection != null)
		{
			Debug.Log("language hidden");
			this.languageSelection.Hide();
		}
		if (this.mapColorPreset != null)
		{
			Debug.Log("map colour hidden");
			this.mapColorPreset.Hide();
		}
		if (this.notificationsDropdown != null)
		{
			this.notificationsDropdown.Hide();
		}
		yield break;
	}

	// Token: 0x040002D6 RID: 726
	public GameObject OptionsPanel;

	// Token: 0x040002D7 RID: 727
	public GameObject Credits;

	// Token: 0x040002D8 RID: 728
	[SerializeField]
	private Slider musicSlider;

	// Token: 0x040002D9 RID: 729
	[SerializeField]
	private Slider soundsSlider;

	// Token: 0x040002DA RID: 730
	[SerializeField]
	private Toggle musicOn;

	// Token: 0x040002DB RID: 731
	[SerializeField]
	private Toggle soundsOn;

	// Token: 0x040002DC RID: 732
	[SerializeField]
	private Toggle soundsAndMusicOnMobile;

	// Token: 0x040002DD RID: 733
	[SerializeField]
	private Toggle showWarnings;

	// Token: 0x040002DE RID: 734
	[SerializeField]
	private Toggle showTooltips;

	// Token: 0x040002DF RID: 735
	[SerializeField]
	private AudioMixer generalAudioMixer;

	// Token: 0x040002E0 RID: 736
	[SerializeField]
	private GameObject warningTooltip;

	// Token: 0x040002E1 RID: 737
	[SerializeField]
	private Button loginAccountsButton;

	// Token: 0x040002E2 RID: 738
	[SerializeField]
	private LoginAccounts loginAccounts;

	// Token: 0x040002E3 RID: 739
	[SerializeField]
	private Dropdown mapColorPreset;

	// Token: 0x040002E4 RID: 740
	[SerializeField]
	private Toggle mapColorDynamic;

	// Token: 0x040002E5 RID: 741
	[SerializeField]
	private Toggle hexTypeColor;

	// Token: 0x040002E6 RID: 742
	[SerializeField]
	private Toggle hexTypeMarker;

	// Token: 0x040002E7 RID: 743
	[SerializeField]
	private Toggle confirmActions;

	// Token: 0x040002E8 RID: 744
	[SerializeField]
	private GameObject undoPanel;

	// Token: 0x040002E9 RID: 745
	[SerializeField]
	private Dropdown undoSelection;

	// Token: 0x040002EA RID: 746
	[SerializeField]
	private Toggle fastForward;

	// Token: 0x040002EB RID: 747
	[SerializeField]
	private Toggle allowSleepMode;

	// Token: 0x040002EC RID: 748
	[SerializeField]
	private Toggle acceptGains;

	// Token: 0x040002ED RID: 749
	[SerializeField]
	private Dropdown notificationsDropdown;

	// Token: 0x040002EE RID: 750
	[SerializeField]
	private Toggle dragAndDrop;

	// Token: 0x040002EF RID: 751
	[SerializeField]
	private Toggle traditionalTracks;

	// Token: 0x040002F0 RID: 752
	[SerializeField]
	private Toggle traditionalResourceModels;

	// Token: 0x040002F1 RID: 753
	[SerializeField]
	private Toggle actionAssist;

	// Token: 0x040002F2 RID: 754
	[SerializeField]
	private Toggle shortcutsEnabled;

	// Token: 0x040002F3 RID: 755
	[SerializeField]
	private Toggle cameraAutorotation;

	// Token: 0x040002F4 RID: 756
	[SerializeField]
	private Toggle cameraAnimations;

	// Token: 0x040002F5 RID: 757
	[SerializeField]
	private Toggle fresnelOutline;

	// Token: 0x040002F6 RID: 758
	[SerializeField]
	private Toggle groundOutline;

	// Token: 0x040002F7 RID: 759
	[SerializeField]
	private Toggle max30FPS;

	// Token: 0x040002F8 RID: 760
	[SerializeField]
	private TMP_Text colorPresetLabel;

	// Token: 0x040002F9 RID: 761
	[SerializeField]
	public BoardShaderPreset[] presets;

	// Token: 0x040002FA RID: 762
	public Transform mapContainer;

	// Token: 0x040002FB RID: 763
	public ResourceTypeLayer resourceTypeLayer;

	// Token: 0x040002FC RID: 764
	public GameObject traditionalTracksParent;

	// Token: 0x040002FD RID: 765
	public SpriteRenderer[] tracks;

	// Token: 0x040002FE RID: 766
	public GameObject logosForRotation;

	// Token: 0x040002FF RID: 767
	[SerializeField]
	private GraphicSettingsPanel graphicSettingsPanel;

	// Token: 0x04000300 RID: 768
	public static string MIXER_VAR_MUSIC_VALUE = "Music";

	// Token: 0x04000301 RID: 769
	public static string MIXER_VAR_UI_VALUE = "UI";

	// Token: 0x04000302 RID: 770
	public static string PREFS_KEY_MUSIC_VOLUME = "MusicVolume";

	// Token: 0x04000303 RID: 771
	public static string PREFS_KEY_SOUNDS_VOLUME = "SoundsVolume";

	// Token: 0x04000304 RID: 772
	public static string PREFS_KEY_MUSIC_ON = "MusicOn";

	// Token: 0x04000305 RID: 773
	public static string PREFS_KEY_SOUNDS_ON = "SoundsOn";

	// Token: 0x04000306 RID: 774
	public static string PREFS_KEY_WARNINGS_ACTIVE = "WarningsActivate";

	// Token: 0x04000307 RID: 775
	public static string PREFS_KEY_TOOLTIPS_ACTIVE = "TooltipsActivate";

	// Token: 0x04000308 RID: 776
	public static string PREFS_KEY_MAP_COLOR_PRESET = "MapColorPreset";

	// Token: 0x04000309 RID: 777
	public static string PREFS_KEY_MAP_COLOR_DYNAMIC = "MapColorDynamic";

	// Token: 0x0400030A RID: 778
	public static string PREFS_KEY_HEXTYPE_COLOR = "HexTypeColor";

	// Token: 0x0400030B RID: 779
	public static string PREFS_KEY_HEXTYPE_MARKER = "HexTypeMarker";

	// Token: 0x0400030C RID: 780
	public static string PREFS_KEY_CONFIRM_ACTIONS = "ConfirmActions";

	// Token: 0x0400030D RID: 781
	public static string PREFS_KEY_FAST_FORWARD = "FastForward";

	// Token: 0x0400030E RID: 782
	public static string PREFS_KEY_ACCEPT_GAINS = "AcceptGains";

	// Token: 0x0400030F RID: 783
	public static string PREFS_KEY_NOTIFICATIONS = "Notifications";

	// Token: 0x04000310 RID: 784
	public static string PREFS_KEY_DRAG_AND_DROP = "DragAndDrop";

	// Token: 0x04000311 RID: 785
	public static string PREFS_KEY_TRADITIONAL_TRACKS = "TraditionalTracks";

	// Token: 0x04000312 RID: 786
	public static string PREFS_KEY_ACTION_ASSIST = "ActionAssist";

	// Token: 0x04000313 RID: 787
	public static string PREFS_KEY_SHORTCUTS_ENABLED = "ShortcutsEnabled";

	// Token: 0x04000314 RID: 788
	public static string PREFS_KEY_CAMERA_ROTATION = "CameraAutorotation";

	// Token: 0x04000315 RID: 789
	public static string PREFS_KEY_CAMERA_ANIMATIONS = "CameraAnimations";

	// Token: 0x04000316 RID: 790
	public static string PREFS_GAME_CLOSED_CORRECT = "GameClosedCorrect";

	// Token: 0x04000317 RID: 791
	public static string PREFS_OFFLINE_GAME_ID = "OfflineGameId";

	// Token: 0x04000318 RID: 792
	public static string PREFS_ALLOW_SLEEP_MODE = "AllowSleepMode";

	// Token: 0x04000319 RID: 793
	public static int TOOLTIPS_DEFAULT = (PlatformManager.IsStandalone ? 1 : 0);

	// Token: 0x0400031A RID: 794
	private const float DEFAULT_MUSIC_VOLUME = 0.5f;

	// Token: 0x0400031B RID: 795
	private const float DEFAULT_SOUND_VOLUME = 0.5f;

	// Token: 0x0400031C RID: 796
	public const NotificationsSetting DEFAULT_NOTIFICATION_SETTING = NotificationsSetting.All;

	// Token: 0x0400031F RID: 799
	private bool silence;

	// Token: 0x04000320 RID: 800
	[SerializeField]
	private Dropdown languageSelection;

	// Token: 0x0200005C RID: 92
	// (Invoke) Token: 0x06000317 RID: 791
	public delegate void LanguageChanged();
}

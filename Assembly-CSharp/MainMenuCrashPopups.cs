using System;
using Common.GameSaves;
using Scythe.Analytics;
using Scythe.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200005A RID: 90
public class MainMenuCrashPopups : MonoBehaviour
{
	// Token: 0x060002C7 RID: 711 RVA: 0x00029B3E File Offset: 0x00027D3E
	private void Start()
	{
		if (!MainMenuCrashPopups.IsGameClosedCorrect())
		{
			this.DisplayProperPopup();
		}
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x00029B4D File Offset: 0x00027D4D
	public static bool IsGameClosedCorrect()
	{
		return MainMenuCrashPopups.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_GAME_CLOSED_CORRECT, 1));
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x00060788 File Offset: 0x0005E988
	private void DisplayProperPopup()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
		if (GameSavesManager.IsSaveSlotValid(GameSavesManager.GetAutomaticSaveSlotId()))
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.return_to_last_game_popup, Contexts.outgame);
			this.offlineGameInProgress.SetActive(true);
			this.gameInProgressResumeButton.onClick.RemoveAllListeners();
			this.gameInProgressResumeButton.onClick.AddListener(new UnityAction(this.LoadAutomaticSave));
			return;
		}
		if (GameSavesManager.ManualWorkingSaveExist())
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.select_save_popup, Contexts.outgame);
			this.offlineGameInProgress.SetActive(true);
			this.gameInProgressResumeButton.onClick.RemoveAllListeners();
			this.gameInProgressResumeButton.onClick.AddListener(new UnityAction(this.LoadLastManualSave));
			return;
		}
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.corrupted_save_popup, Contexts.outgame);
		this.corruptedSavePopup.SetActive(true);
	}

	// Token: 0x060002CA RID: 714 RVA: 0x00029B5F File Offset: 0x00027D5F
	private void LoadAutomaticSave()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		PlayerPrefs.SetInt(OptionsManager.PREFS_GAME_CLOSED_CORRECT, OptionsManager.BoolToInt(true));
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_resume_button);
		this.loadSaveMenu.LoadGame(GameSavesManager.GetAutomaticSaveSlotId(), true);
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00060854 File Offset: 0x0005EA54
	private void LoadLastManualSave()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		PlayerPrefs.SetInt(OptionsManager.PREFS_GAME_CLOSED_CORRECT, OptionsManager.BoolToInt(true));
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_resume_button);
		this.loadSaveMenu.LoadGame(GameSavesManager.GetManualSavesByAutomaticGameId()[0].SaveSlotId, true);
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00029B95 File Offset: 0x00027D95
	private static int BoolToInt(bool val)
	{
		if (!val)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00029B9D File Offset: 0x00027D9D
	private static bool IntToBool(int val)
	{
		return val != 0;
	}

	// Token: 0x060002CE RID: 718 RVA: 0x00029BA3 File Offset: 0x00027DA3
	public void Ok_OnClick()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		PlayerPrefs.SetInt(OptionsManager.PREFS_GAME_CLOSED_CORRECT, OptionsManager.BoolToInt(true));
		this.corruptedSavePopup.SetActive(false);
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_ok_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
	}

	// Token: 0x060002CF RID: 719 RVA: 0x00029BDF File Offset: 0x00027DDF
	public void Cancel_OnClick()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		PlayerPrefs.SetInt(OptionsManager.PREFS_GAME_CLOSED_CORRECT, OptionsManager.BoolToInt(true));
		this.offlineGameInProgress.SetActive(false);
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_cancel_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
	}

	// Token: 0x040002D2 RID: 722
	[SerializeField]
	private GameObject corruptedSavePopup;

	// Token: 0x040002D3 RID: 723
	[SerializeField]
	private GameObject offlineGameInProgress;

	// Token: 0x040002D4 RID: 724
	[SerializeField]
	private Button gameInProgressResumeButton;

	// Token: 0x040002D5 RID: 725
	[SerializeField]
	private LoadSaveMenu loadSaveMenu;
}

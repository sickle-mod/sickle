using System;
using Scythe.Analytics;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class ExtrasPanelMobile : MonoBehaviour
{
	// Token: 0x060007FD RID: 2045 RVA: 0x0002D4B8 File Offset: 0x0002B6B8
	private void OnEnable()
	{
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.extras, Contexts.outgame);
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x0002920A File Offset: 0x0002740A
	public void SetActive(bool active)
	{
		base.gameObject.SetActive(active);
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x0002D4C7 File Offset: 0x0002B6C7
	private void PlayClickSound()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x0002D4D5 File Offset: 0x0002B6D5
	public void OnTutorialButtonClicked()
	{
		this.PlayClickSound();
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_tutorial_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.tutorial_setup, Contexts.outgame);
		this.soundAndOptions.SetActive(false);
		this.campaignMenu.SetActive(true);
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x0002D508 File Offset: 0x0002B708
	public void OnAlbumButtonClicked()
	{
		this.PlayClickSound();
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_album_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.album, Contexts.outgame);
		this.album.SetActive(true);
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x0002D530 File Offset: 0x0002B730
	public void OnShowroomButtonClicked()
	{
		this.PlayClickSound();
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_showroom_button);
		SceneController.Instance.LoadScene(SceneController.SCENE_SHOWROOM_NAME);
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x0002D54E File Offset: 0x0002B74E
	public void OnStatsButtonClicked()
	{
		this.PlayClickSound();
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_more_stats_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.album, Contexts.outgame);
		this.stats.SetActive(true);
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x0002D576 File Offset: 0x0002B776
	public void OnTutorialClosed()
	{
		this.PlayClickSound();
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
		this.soundAndOptions.SetActive(true);
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x0002D59C File Offset: 0x0002B79C
	public void OnAlbumClosed()
	{
		this.PlayClickSound();
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.extras, Contexts.outgame);
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x0002D59C File Offset: 0x0002B79C
	public void OnShowroomClosed()
	{
		this.PlayClickSound();
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.extras, Contexts.outgame);
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x0002D59C File Offset: 0x0002B79C
	public void OnStatsClosed()
	{
		this.PlayClickSound();
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.extras, Contexts.outgame);
	}

	// Token: 0x040006CC RID: 1740
	[SerializeField]
	private GameObject campaignMenu;

	// Token: 0x040006CD RID: 1741
	[SerializeField]
	private GameObject album;

	// Token: 0x040006CE RID: 1742
	[SerializeField]
	private GameObject stats;

	// Token: 0x040006CF RID: 1743
	[SerializeField]
	private GameObject soundAndOptions;
}

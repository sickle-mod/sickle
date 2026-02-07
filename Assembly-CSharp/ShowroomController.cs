using System;
using DG.Tweening;
using Scythe.Analytics;
using Scythe.UI;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class ShowroomController : MonoBehaviour
{
	// Token: 0x06000883 RID: 2179 RVA: 0x0002DBFE File Offset: 0x0002BDFE
	private void Start()
	{
		this.albionButton.SetActive(GameServiceController.Instance.InvadersFromAfarUnlocked());
		this.togawaButton.SetActive(GameServiceController.Instance.InvadersFromAfarUnlocked());
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.showroom, Contexts.outgame);
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x0002DC37 File Offset: 0x0002BE37
	public void ExitShowroom()
	{
		MainMenu.OpenExtrasOnLoad = true;
		WorldSFXManager.PlaySound(SoundEnum.CommonCheckBoxV1, AudioSourceType.Buttons);
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
		if (PlatformManager.IsStandalone)
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
		}
		DOTween.KillAll(true);
		SceneController.Instance.LoadScene(SceneController.SCENE_MENU_NAME);
	}

	// Token: 0x04000719 RID: 1817
	[SerializeField]
	private GameObject albionButton;

	// Token: 0x0400071A RID: 1818
	[SerializeField]
	private GameObject togawaButton;
}

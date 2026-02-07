using System;
using Scythe.Analytics;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000496 RID: 1174
	public class HelpScreen : MonoBehaviour
	{
		// Token: 0x0600253B RID: 9531 RVA: 0x0003F9DC File Offset: 0x0003DBDC
		private void Awake()
		{
			this.overlayButton.onClick.AddListener(new UnityAction(this.OnOverlayButtonClicked));
			this.closeButton.onClick.AddListener(new UnityAction(this.OnCloseButtonClicked));
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000DE2C0 File Offset: 0x000DC4C0
		public void Show()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_help_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.help_screen, Contexts.ingame);
			WorldSFXManager.PlaySound(SoundEnum.CommonCheckBoxV1, AudioSourceType.Buttons);
			base.gameObject.SetActive(true);
			SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.HiddenForHelp, 0.25f);
			this.matchHeightCanvas.enabled = false;
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x0003FA16 File Offset: 0x0003DC16
		private void OnOverlayButtonClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_mouse_button);
			this.Hide();
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x0003FA25 File Offset: 0x0003DC25
		private void OnCloseButtonClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
			this.Hide();
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x0003FA33 File Offset: 0x0003DC33
		public void Hide()
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
			base.gameObject.SetActive(false);
			SingletonMono<BottomBar>.Instance.RestorePreviousState();
			this.matchHeightCanvas.enabled = true;
		}

		// Token: 0x04001A25 RID: 6693
		[SerializeField]
		private Button overlayButton;

		// Token: 0x04001A26 RID: 6694
		[SerializeField]
		private Button closeButton;

		// Token: 0x04001A27 RID: 6695
		[SerializeField]
		private Canvas matchHeightCanvas;
	}
}

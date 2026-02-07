using System;
using I2.Loc;
using Scythe.Analytics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000268 RID: 616
	public class MainLobbyScreen : MonoBehaviour
	{
		// Token: 0x060012D3 RID: 4819 RVA: 0x000346EF File Offset: 0x000328EF
		public void CreateGame_OnClick()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_create_game_room_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.game_setup, Contexts.outgame);
			this.ChangeCreateGameActivity(true);
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x00034718 File Offset: 0x00032918
		public void CloseCreateGame_OnClick()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.lobby, Contexts.outgame);
			this.ChangeCreateGameActivity(false);
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00034734 File Offset: 0x00032934
		public void Activate()
		{
			base.gameObject.SetActive(true);
			this.lobbyTitle.text = ScriptLocalization.Get("MainMenu/Multiplayer");
			this.ActivateResumeJoinPanel();
			this.closeLobbyButton.SetActive(true);
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00034769 File Offset: 0x00032969
		public void ActivateResumeJoinPanel()
		{
			if (!this.resumeJoinToggle.isOn)
			{
				this.resumeJoinToggle.isOn = true;
				return;
			}
			this.gameListPanel.SwitchToResumeJoinRooms();
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00034790 File Offset: 0x00032990
		public void ChangeResumeJoinActivity(bool newState)
		{
			if (!newState)
			{
				this.gameListPanel.Deactivate();
				return;
			}
			this.gameListPanel.Activate();
			this.gameListPanel.SwitchToResumeJoinRooms();
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x000347C3 File Offset: 0x000329C3
		public void ChangeSpectateActivity(bool newState)
		{
			if (!newState)
			{
				this.gameListPanel.Deactivate();
				return;
			}
			this.gameListPanel.Activate();
			this.gameListPanel.SwitchToSpectateRooms();
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x000347F6 File Offset: 0x000329F6
		public void ChangeCreateGameActivity(bool newState)
		{
			if (!newState)
			{
				this.createGamePanel.Deactivate();
				return;
			}
			this.createGamePanel.Activate();
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x00029172 File Offset: 0x00027372
		public void Deactivate()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000E36 RID: 3638
		[SerializeField]
		private TextMeshProUGUI lobbyTitle;

		// Token: 0x04000E37 RID: 3639
		[SerializeField]
		private GameObject closeLobbyButton;

		// Token: 0x04000E38 RID: 3640
		[SerializeField]
		private Toggle createRoomToggle;

		// Token: 0x04000E39 RID: 3641
		[SerializeField]
		private Toggle resumeJoinToggle;

		// Token: 0x04000E3A RID: 3642
		[SerializeField]
		private Toggle spectateToggle;

		// Token: 0x04000E3B RID: 3643
		[SerializeField]
		private GameListPanel gameListPanel;

		// Token: 0x04000E3C RID: 3644
		[SerializeField]
		private CreateGamePanel createGamePanel;
	}
}

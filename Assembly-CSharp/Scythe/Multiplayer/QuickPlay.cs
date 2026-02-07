using System;
using Scythe.Analytics;
using Scythe.Multiplayer.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000271 RID: 625
	public class QuickPlay : MonoBehaviour
	{
		// Token: 0x0600133D RID: 4925 RVA: 0x00034D63 File Offset: 0x00032F63
		public void QuickGame_OnClick()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_quick_play_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.quick_game, Contexts.outgame);
			this.Activate();
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00034D8B File Offset: 0x00032F8B
		public void CloseQuickGame_OnClick()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.lobby, Contexts.outgame);
			this.Close();
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00034DB2 File Offset: 0x00032FB2
		public void Find_OnClick()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_join_quick_play_button);
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.matchmaking);
			this.Search();
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x00034DD3 File Offset: 0x00032FD3
		public void Activate()
		{
			this.playersAmount.Init(GameServiceController.Instance.InvadersFromAfarUnlocked());
			base.gameObject.SetActive(true);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00098474 File Offset: 0x00096674
		public void Search()
		{
			int num = this.playersAmount.GetPlayersAmount();
			bool isOn = this.asynchronousToggle.isOn;
			bool isOn2 = this.rankedToggle.isOn;
			Preferences preferences = new Preferences(num, isOn2, isOn, GameServiceController.Instance.InvadersFromAfarUnlocked());
			this.lobby.QuickPlay(preferences);
			this.Close();
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x0002CAD4 File Offset: 0x0002ACD4
		public void OnToggleChanged()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00034E02 File Offset: 0x00033002
		public void Close()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000E6E RID: 3694
		[SerializeField]
		private Lobby lobby;

		// Token: 0x04000E6F RID: 3695
		[SerializeField]
		private PlayersAmount playersAmount;

		// Token: 0x04000E70 RID: 3696
		[SerializeField]
		private Toggle asynchronousToggle;

		// Token: 0x04000E71 RID: 3697
		[SerializeField]
		private Toggle rankedToggle;
	}
}

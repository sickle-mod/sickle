using System;
using System.Linq;
using Scythe.Analytics;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x0200050B RID: 1291
	public class Tutorial00 : Tutorial
	{
		// Token: 0x06002954 RID: 10580 RVA: 0x00042DAF File Offset: 0x00040FAF
		protected override void OnStart()
		{
			AnalyticsEventData.TutorialStart(StepIDs.tut1_00_introduction);
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x000EC544 File Offset: 0x000EA744
		protected override void RegisterCustomCallbacks()
		{
			base.SetShow(3, new Action(this.OnShow03));
			base.SetHide(3, new Action(this.OnHide03));
			base.SetNextButton(5, this.hamburgerButton, HighlightController.HighlightStyle.Circle, 1);
			base.SetShow(6, new Action(this.OnShow06));
			base.SetHide(6, new Action(this.OnHide06));
			base.SetShow(7, new Action(this.OnShow07));
			base.SetNextButton(9, GameController.Instance.matPlayer.matSection[0].topActionPresenter.gainActionButton[1], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetEndTurn(10);
			base.SetShow(12, new Action(this.OnShow12));
			base.SetShow(13, new Action(this.OnShow13));
			base.SetNextButton(14, GameController.Instance.matPlayer.matSection[1].topActionPresenter.gainActionButton[0], HighlightController.HighlightStyle.Rectangle, 1);
			base.SetShow(15, new Action(this.OnShow15));
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x00042DB7 File Offset: 0x00040FB7
		private void OnShow03()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			SingletonMono<TopMenuPanelsManager>.Instance.ShowPanel(4);
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x00042DCB File Offset: 0x00040FCB
		private void OnHide03()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			SingletonMono<TopMenuPanelsManager>.Instance.SwitchPanel(0);
			SingletonMono<TopMenuPanelsManager>.Instance.Hide();
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x00042DE9 File Offset: 0x00040FE9
		private void OnShow06()
		{
			SingletonMono<TopMenuPanelsManager>.Instance.ShowPanel(0);
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x00042DF6 File Offset: 0x00040FF6
		private void OnHide06()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			SingletonMono<TopMenuPanelsManager>.Instance.Hide();
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x00042E09 File Offset: 0x00041009
		private void OnShow07()
		{
			SingletonMono<HighlightController>.Instance.HighlightUI(GameController.Instance.matPlayer, HighlightController.HighlightStyle.Rectangle);
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x00042E20 File Offset: 0x00041020
		private void OnShow12()
		{
			SingletonMono<HighlightController>.Instance.HighlightUI(this.peekHighlightAreas[0], HighlightController.HighlightStyle.Circle);
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x00042E35 File Offset: 0x00041035
		private void OnShow13()
		{
			SingletonMono<HighlightController>.Instance.HighlightUI(this.peekHighlightAreas[1], HighlightController.HighlightStyle.Circle);
			SingletonMono<HighlightController>.Instance.HighlightUI(this.powerStat, HighlightController.HighlightStyle.Rectangle);
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x00042E5B File Offset: 0x0004105B
		private void OnShow15()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
			SingletonMono<TopMenuPanelsManager>.Instance.ShowPanel(0);
			this.HighlightPlayerPowerStar();
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x000EC65C File Offset: 0x000EA85C
		private void HighlightPlayerPowerStar()
		{
			TopMenuStarsEntry topMenuStarsEntry = SingletonMono<TopMenuPanelsManager>.Instance.GetComponentsInChildren<TopMenuStarsEntry>().First<TopMenuStarsEntry>();
			SingletonMono<HighlightController>.Instance.HighlightUI(topMenuStarsEntry.PowerStar, HighlightController.HighlightStyle.Circle);
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x00042E75 File Offset: 0x00041075
		protected override bool SkipCurrentState(StepIDs step)
		{
			return base.IsDesktopBuild() && step == StepIDs.tut1_02_b_structure_bonus;
		}

		// Token: 0x04001D80 RID: 7552
		[SerializeField]
		private Button hamburgerButton;

		// Token: 0x04001D81 RID: 7553
		[SerializeField]
		private RectTransform powerStat;

		// Token: 0x04001D82 RID: 7554
		[SerializeField]
		private RectTransform[] peekHighlightAreas;
	}
}

using System;
using I2.Loc;
using Scythe.Analytics;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x0200051E RID: 1310
	public class TutorialEndScreen : SingletonMono<TutorialEndScreen>
	{
		// Token: 0x060029D0 RID: 10704 RVA: 0x000433CB File Offset: 0x000415CB
		private void Awake()
		{
			this.playNextButton.onClick.AddListener(new UnityAction(this.PlayNext));
			this.exitButton.onClick.AddListener(new UnityAction(this.Exit));
			this.Hide();
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x000ED998 File Offset: 0x000EBB98
		public void Show()
		{
			TutorialMissionSelection.MissionCompleted(GameController.GameManager.missionId);
			int num = GameController.GameManager.missionId + 1;
			if (SingletonMono<TutorialController>.Instance.TutorialCount > num)
			{
				this.Initialize(num);
				SingletonMono<InputBlockerController>.Instance.UnblockUI(this);
				this.content.gameObject.SetActive(true);
				return;
			}
			SingletonMono<InputBlockerController>.Instance.UnblockAllUI();
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x0004340B File Offset: 0x0004160B
		private void Hide()
		{
			this.content.gameObject.SetActive(false);
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x000ED9FC File Offset: 0x000EBBFC
		private void Initialize(int nextTutorialId)
		{
			this.popupTitleText.SetText(string.Format("{0} {1}<font=\"TCM_Silver\">/</font>{2}", ScriptLocalization.Get("Tutorials/Tutorials"), nextTutorialId + 1, SingletonMono<TutorialController>.Instance.TutorialCount), true);
			this.tutorialTitleText.text = ScriptLocalization.Get(string.Format("Missions/MissionTitle{0}", nextTutorialId + 1));
			this.tutorialDescriptionText.text = ScriptLocalization.Get(string.Format("Missions/MissionDescription{0}", nextTutorialId + 1));
			this.tutorialImage.sprite = this.tutorialSprites[nextTutorialId];
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x0004341E File Offset: 0x0004161E
		private void PlayNext()
		{
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.previousMap);
			this.OnLoadingNextTutorial();
			GameController.GameManager.InitCampaign(GameController.GameManager.missionId + 1, 0);
			SceneController.Instance.LoadScene(SceneController.SCENE_MAIN_NAME);
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x0003A583 File Offset: 0x00038783
		private void Exit()
		{
			GameController.Instance.ExitGame();
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x00043452 File Offset: 0x00041652
		private void OnLoadingNextTutorial()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_new_game_button);
			AnalyticsEventLogger.Instance.LogMatchStop(EndReasons.game_completed);
		}

		// Token: 0x04001DB6 RID: 7606
		[SerializeField]
		private RectTransform content;

		// Token: 0x04001DB7 RID: 7607
		[SerializeField]
		private TextMeshProUGUI popupTitleText;

		// Token: 0x04001DB8 RID: 7608
		[SerializeField]
		private TextMeshProUGUI tutorialTitleText;

		// Token: 0x04001DB9 RID: 7609
		[SerializeField]
		private TextMeshProUGUI tutorialDescriptionText;

		// Token: 0x04001DBA RID: 7610
		[SerializeField]
		private Image tutorialImage;

		// Token: 0x04001DBB RID: 7611
		[SerializeField]
		private Button playNextButton;

		// Token: 0x04001DBC RID: 7612
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001DBD RID: 7613
		[SerializeField]
		private Sprite[] tutorialSprites;
	}
}

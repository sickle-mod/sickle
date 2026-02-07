using System;
using System.Collections;
using System.Collections.Generic;
using Scythe.Analytics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004F2 RID: 1266
	public class TutorialMissionSelection : MonoBehaviour
	{
		// Token: 0x060028AC RID: 10412 RVA: 0x0004257E File Offset: 0x0004077E
		private void Awake()
		{
			this.GetMissionToggles();
			this.ReadProgress();
			this.UpdateCampaignView();
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x00042592 File Offset: 0x00040792
		private void OnEnable()
		{
			base.StartCoroutine(this.UpdateMissionDescriptionNextFrame());
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000425A1 File Offset: 0x000407A1
		private IEnumerator UpdateMissionDescriptionNextFrame()
		{
			yield return new WaitForEndOfFrame();
			this.UpdateMissionDescription();
			yield break;
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x000EB34C File Offset: 0x000E954C
		private void Start()
		{
			Vector3 position = this.SelectedMissionArrows.transform.position;
			position.y = 700f;
			this.SelectedMissionArrows.transform.position = position;
			this.scrollRect.verticalNormalizedPosition = 1f;
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x000EB398 File Offset: 0x000E9598
		private void GetMissionToggles()
		{
			if (this.SelectedMissionArrows == null)
			{
				Debug.LogWarning("SelectedMissionArrows object is not connected to CampaignPanel.");
			}
			if (this.missionToggles == null)
			{
				this.missionToggles = new List<GameObject>();
			}
			foreach (object obj in this.MissionsPanel.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.GetInstanceID() != this.SelectedMissionArrows.GetInstanceID())
				{
					this.missionToggles.Add(transform.gameObject);
				}
			}
			for (int i = 0; i < this.missionCheckmarks.Length; i++)
			{
				this.missionCheckmarks[i].enabled = TutorialMissionSelection.isMissionCompleted(i);
			}
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000425B0 File Offset: 0x000407B0
		private void ReadProgress()
		{
			this.campaignsProgress = PlayerPrefs.GetInt("CampaignsProgress");
			this.selectedMission = 0;
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x000425C9 File Offset: 0x000407C9
		private void UpdateCampaignView()
		{
			this.missionToggles[this.selectedMission].GetComponent<Toggle2>().isOn = true;
			this.MoveSelectedMissionArrowsToToggle(this.missionToggles[this.selectedMission]);
			this.UpdateMissionDescription();
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x000EB46C File Offset: 0x000E966C
		private void UpdateMissionDescription()
		{
			this.MissionTitle.text = this.missionTitles[this.selectedMission].text;
			this.MissionDescription.text = this.missionDescriptions[this.selectedMission].text;
			this.Objectives.text = this.missionObjectives[this.selectedMission].text;
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x000EB4D0 File Offset: 0x000E96D0
		private void MoveSelectedMissionArrowsToToggle(GameObject missionToggle)
		{
			Vector3 position = this.SelectedMissionArrows.transform.position;
			position.y = missionToggle.transform.position.y;
			this.SelectedMissionArrows.transform.position = position;
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x00042604 File Offset: 0x00040804
		public void ChangeMission(int missionId)
		{
			if (missionId == this.selectedMission)
			{
				return;
			}
			this.selectedMission = missionId;
			this.MoveSelectedMissionArrowsToToggle(this.missionToggles[this.selectedMission]);
			this.UpdateMissionDescription();
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiShowHideMapMarkers);
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x000EB518 File Offset: 0x000E9718
		public void OnStartSelectedMission()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_new_game_button);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			GameController.Game.CreateNewGameManager();
			GameController.GameManager.InitCampaign(this.selectedMission, 0);
			GameController.gameFromSave = false;
			this.mainMenu.StartGame();
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.main);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x00029B9D File Offset: 0x00027D9D
		private static bool IntToBool(int val)
		{
			return val != 0;
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x00029B95 File Offset: 0x00027D95
		private static int BoolToInt(bool val)
		{
			if (!val)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x00042640 File Offset: 0x00040840
		public static bool isMissionCompleted(int id)
		{
			return TutorialMissionSelection.IntToBool(PlayerPrefs.GetInt(TutorialMissionSelection.MISSION_COMPLETED + id.ToString(), 0));
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x0004265E File Offset: 0x0004085E
		public static void MissionCompleted(int id)
		{
			AchievementManager.UpdateAchievementTutorial();
			PlayerPrefs.SetInt(TutorialMissionSelection.MISSION_COMPLETED + id.ToString(), TutorialMissionSelection.BoolToInt(true));
			PlayerPrefs.Save();
		}

		// Token: 0x04001D26 RID: 7462
		public static string MISSION_COMPLETED = "MissionCompleted";

		// Token: 0x04001D27 RID: 7463
		public MainMenu mainMenu;

		// Token: 0x04001D28 RID: 7464
		public GameObject SelectedMissionArrows;

		// Token: 0x04001D29 RID: 7465
		public GameObject MissionsPanel;

		// Token: 0x04001D2A RID: 7466
		public ScrollRect scrollRect;

		// Token: 0x04001D2B RID: 7467
		public TextMeshProUGUI MissionTitle;

		// Token: 0x04001D2C RID: 7468
		public Text MissionDescription;

		// Token: 0x04001D2D RID: 7469
		public Text Objectives;

		// Token: 0x04001D2E RID: 7470
		public TextMeshProUGUI[] missionTitles;

		// Token: 0x04001D2F RID: 7471
		public Text[] missionDescriptions;

		// Token: 0x04001D30 RID: 7472
		public Text[] missionObjectives;

		// Token: 0x04001D31 RID: 7473
		public Image[] missionCheckmarks;

		// Token: 0x04001D32 RID: 7474
		private List<GameObject> missionToggles = new List<GameObject>();

		// Token: 0x04001D33 RID: 7475
		private int campaignsProgress;

		// Token: 0x04001D34 RID: 7476
		private int selectedMission;
	}
}

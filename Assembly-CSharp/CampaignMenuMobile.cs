using System;
using I2.Loc;
using Scythe.Analytics;
using Scythe.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000E6 RID: 230
public class CampaignMenuMobile : MonoBehaviour
{
	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060006BE RID: 1726 RVA: 0x0002C522 File Offset: 0x0002A722
	private int campaignProgress
	{
		get
		{
			return PlayerPrefs.GetInt("CampaignsProgress");
		}
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x00071BF8 File Offset: 0x0006FDF8
	private void Awake()
	{
		this.nextTutorialButton.onClick.AddListener(new UnityAction(this.OnNextTutorialButtonClicked));
		this.previousTutorialButton.onClick.AddListener(new UnityAction(this.OnPreviousTutorialButtonClicked));
		this.startTutorialButton.onClick.AddListener(new UnityAction(this.OnstartButtonClicked));
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x0002C52E File Offset: 0x0002A72E
	private void Start()
	{
		this.SetTutorialByCurrentIndex();
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x0002C536 File Offset: 0x0002A736
	private void OnEnable()
	{
		this.currentIndex = 0;
		this.SetTutorialByCurrentIndex();
		OptionsManager.OnLanguageChanged += this.SetTutorialByCurrentIndex;
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x0002C556 File Offset: 0x0002A756
	private void OnDisable()
	{
		OptionsManager.OnLanguageChanged -= this.SetTutorialByCurrentIndex;
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x0002C569 File Offset: 0x0002A769
	private void OnstartButtonClicked()
	{
		GameController.Game.CreateNewGameManager();
		GameController.GameManager.InitCampaign(this.currentIndex, 0);
		GameController.gameFromSave = false;
		AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.main);
		this.mainMenu.StartGame();
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x00071C5C File Offset: 0x0006FE5C
	private void OnNextTutorialButtonClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiShowHideMapMarkers);
		int num;
		if (this.currentIndex >= this.tutorialMissionSprites.Length - 1)
		{
			num = 0;
		}
		else
		{
			int num2 = this.currentIndex + 1;
			this.currentIndex = num2;
			num = num2;
		}
		this.currentIndex = num;
		this.SetTutorialByCurrentIndex();
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x00071CA8 File Offset: 0x0006FEA8
	private void OnPreviousTutorialButtonClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiShowHideMapMarkers);
		int num;
		if (this.currentIndex <= 0)
		{
			num = this.tutorialMissionSprites.Length - 1;
		}
		else
		{
			int num2 = this.currentIndex - 1;
			this.currentIndex = num2;
			num = num2;
		}
		this.currentIndex = num;
		this.SetTutorialByCurrentIndex();
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x00071CF4 File Offset: 0x0006FEF4
	private void SetTutorialByCurrentIndex()
	{
		string translation = LocalizationManager.GetTranslation("Missions/MissionTitle" + (this.currentIndex + 1).ToString(), true, 0, true, false, null, null);
		string translation2 = LocalizationManager.GetTranslation("Missions/MissionDescription" + (this.currentIndex + 1).ToString(), true, 0, true, false, null, null);
		this.campaignMenuTutorialBox.FillTutorialBox(this.currentIndex + 1, this.tutorialMissionSprites.Length, translation, translation2, this.tutorialMissionSprites[this.currentIndex], TutorialMissionSelection.isMissionCompleted(this.currentIndex));
	}

	// Token: 0x04000608 RID: 1544
	private const string missionTitleLocalizationPrefix = "Missions/MissionTitle";

	// Token: 0x04000609 RID: 1545
	private const string missionDescriptionLocalizationPrefix = "Missions/MissionDescription";

	// Token: 0x0400060A RID: 1546
	[SerializeField]
	private MainMenu mainMenu;

	// Token: 0x0400060B RID: 1547
	[SerializeField]
	private CampaignMenuTutorialBox campaignMenuTutorialBox;

	// Token: 0x0400060C RID: 1548
	[SerializeField]
	private Button nextTutorialButton;

	// Token: 0x0400060D RID: 1549
	[SerializeField]
	private Button previousTutorialButton;

	// Token: 0x0400060E RID: 1550
	[SerializeField]
	private Button startTutorialButton;

	// Token: 0x0400060F RID: 1551
	[SerializeField]
	private Sprite[] tutorialMissionSprites;

	// Token: 0x04000610 RID: 1552
	private int currentIndex;
}

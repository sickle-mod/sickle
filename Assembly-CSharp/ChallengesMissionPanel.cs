using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Scythe.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E8 RID: 232
public class ChallengesMissionPanel : MonoBehaviour
{
	// Token: 0x060006CB RID: 1739 RVA: 0x0002C5B0 File Offset: 0x0002A7B0
	private void Awake()
	{
		this.GetMissionToggles();
		this.ReadProgress();
		this.UpdateCampaignView();
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x0002C5C4 File Offset: 0x0002A7C4
	private void OnEnable()
	{
		base.StartCoroutine(this.UpdateMissionDescriptionNextFrame());
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x0002C5D3 File Offset: 0x0002A7D3
	private IEnumerator UpdateMissionDescriptionNextFrame()
	{
		yield return new WaitForEndOfFrame();
		this.UpdateMissionDescription();
		yield break;
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x00071DE8 File Offset: 0x0006FFE8
	private void Start()
	{
		Vector3 position = this.SelectedMissionArrows.transform.position;
		position.y = 700f;
		this.SelectedMissionArrows.transform.position = position;
		this.scrollRect.verticalNormalizedPosition = 1f;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00071E34 File Offset: 0x00070034
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
			this.missionCheckmarks[i].enabled = ChallengesMissionPanel.isMissionCompleted(i);
		}
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x0002C5E2 File Offset: 0x0002A7E2
	private void ReadProgress()
	{
		this.campaignsProgress = PlayerPrefs.GetInt("ChallengesProgress");
		this.selectedMission = 0;
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x0002C5FB File Offset: 0x0002A7FB
	private void UpdateCampaignView()
	{
		this.missionToggles[this.selectedMission].GetComponent<Toggle2>().isOn = true;
		this.MoveSelectedMissionArrowsToToggle(this.missionToggles[this.selectedMission]);
		this.UpdateMissionDescription();
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x00071F08 File Offset: 0x00070108
	private void UpdateMissionDescription()
	{
		this.MissionTitle.text = this.missionTitles[this.selectedMission].text;
		this.MissionDescription.text = ScriptLocalization.Get("ChallengesMenu/ChallengeDescription" + (this.selectedMission + 1).ToString());
		this.Objectives.text = this.missionObjectives[this.selectedMission].text;
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x00071F7C File Offset: 0x0007017C
	private void MoveSelectedMissionArrowsToToggle(GameObject missionToggle)
	{
		Vector3 position = this.SelectedMissionArrows.transform.position;
		position.y = missionToggle.transform.position.y;
		this.SelectedMissionArrows.transform.position = position;
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x0002C636 File Offset: 0x0002A836
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

	// Token: 0x060006D5 RID: 1749 RVA: 0x00071FC4 File Offset: 0x000701C4
	public void OnStartSelectedMission()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		GameController.GameManager.challengesLogicStarter.InitChallenge(GameController.GameManager, this.selectedMission, 0);
		GameController.gameFromSave = false;
		this.mainMenu.StartGame();
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x00029B9D File Offset: 0x00027D9D
	private static bool IntToBool(int val)
	{
		return val != 0;
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x00029B95 File Offset: 0x00027D95
	private static int BoolToInt(bool val)
	{
		if (!val)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x0002C672 File Offset: 0x0002A872
	public static bool isMissionCompleted(int id)
	{
		return ChallengesMissionPanel.IntToBool(PlayerPrefs.GetInt(ChallengesMissionPanel.CHALLENGE_COMPLETED + id.ToString(), 0));
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x0002C690 File Offset: 0x0002A890
	public static void MissionCompleted(int id)
	{
		PlayerPrefs.SetInt(ChallengesMissionPanel.CHALLENGE_COMPLETED + id.ToString(), ChallengesMissionPanel.BoolToInt(true));
		PlayerPrefs.Save();
	}

	// Token: 0x04000617 RID: 1559
	public static string CHALLENGE_COMPLETED = "ChallengeCompleted";

	// Token: 0x04000618 RID: 1560
	public MainMenu mainMenu;

	// Token: 0x04000619 RID: 1561
	public GameObject SelectedMissionArrows;

	// Token: 0x0400061A RID: 1562
	public GameObject MissionsPanel;

	// Token: 0x0400061B RID: 1563
	public ScrollRect scrollRect;

	// Token: 0x0400061C RID: 1564
	public TextMeshProUGUI MissionTitle;

	// Token: 0x0400061D RID: 1565
	public Text MissionDescription;

	// Token: 0x0400061E RID: 1566
	public Text Objectives;

	// Token: 0x0400061F RID: 1567
	public TextMeshProUGUI[] missionTitles;

	// Token: 0x04000620 RID: 1568
	public Text[] missionDescriptions;

	// Token: 0x04000621 RID: 1569
	public Text[] missionObjectives;

	// Token: 0x04000622 RID: 1570
	public Image[] missionCheckmarks;

	// Token: 0x04000623 RID: 1571
	private List<GameObject> missionToggles = new List<GameObject>();

	// Token: 0x04000624 RID: 1572
	private int campaignsProgress;

	// Token: 0x04000625 RID: 1573
	private int selectedMission;
}

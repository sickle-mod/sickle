using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004D5 RID: 1237
	public class CampaignMissionSelection : MonoBehaviour
	{
		// Token: 0x06002766 RID: 10086 RVA: 0x00041275 File Offset: 0x0003F475
		private void Awake()
		{
			this.GetCampaignToggles();
			this.GetMissionToggles();
			this.ReadMissionDescriptions();
			this.ReadProgress();
			this.UpdateCampaignView();
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000E8230 File Offset: 0x000E6430
		private void GetCampaignToggles()
		{
			if (this.campaignToggles == null)
			{
				this.campaignToggles = new List<GameObject>();
			}
			foreach (object obj in this.FactionsToggleGroup.transform)
			{
				Transform transform = (Transform)obj;
				this.campaignToggles.Add(transform.gameObject);
			}
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000E82AC File Offset: 0x000E64AC
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
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000E8358 File Offset: 0x000E6558
		private void ReadMissionDescriptions()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(MissionsDescriptions));
			this.missionsDescriptions = (MissionsDescriptions)xmlSerializer.Deserialize(new StreamReader(this.descriptionsFilePath));
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000E8394 File Offset: 0x000E6594
		private void ReadProgress()
		{
			this.campaignsProgress = PlayerPrefs.GetInt("CampaignsProgress");
			this.unlockedCampaigns = Mathf.FloorToInt((float)this.campaignsProgress / (float)this.missionsPerCampaign);
			this.selectedCampaign = this.unlockedCampaigns;
			this.selectedMission = this.GetLatestMissionIdForCampaign(this.unlockedCampaigns);
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000E83EC File Offset: 0x000E65EC
		private void UpdateCampaignView()
		{
			int latestMissionIdForCampaign = this.GetLatestMissionIdForCampaign(this.selectedCampaign);
			this.selectedMission = latestMissionIdForCampaign;
			for (int i = 0; i < this.missionsDescriptions.Campaigns[this.selectedCampaign].Missions.Count; i++)
			{
				TextMeshProUGUI component = this.missionToggles[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>();
				component.text = this.missionsDescriptions.Campaigns[this.selectedCampaign].Missions[i].Title;
				Color color = component.color;
				if (i <= latestMissionIdForCampaign)
				{
					this.missionToggles[i].GetComponent<Toggle2>().interactable = true;
					color.a = 1f;
				}
				else
				{
					this.missionToggles[i].GetComponent<Toggle2>().interactable = false;
					color.a = 0.5f;
				}
				component.color = color;
			}
			this.missionToggles[this.selectedMission].GetComponent<Toggle2>().isOn = true;
			this.MoveSelectedMissionArrowsToToggle(this.missionToggles[this.selectedMission]);
			this.UpdateMissionDescription();
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x00041295 File Offset: 0x0003F495
		private void UpdateActualFactionEmblem()
		{
			this.ActualFactionEmblem.sprite = this.campaignToggles[this.selectedCampaign].transform.Find("Emblem").GetComponent<Image>().sprite;
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000E8518 File Offset: 0x000E6718
		private void UpdateMissionDescription()
		{
			this.MissionTitle.text = this.missionsDescriptions.Campaigns[this.selectedCampaign].Missions[this.selectedMission].Title;
			this.MissionDescription.text = this.missionsDescriptions.Campaigns[this.selectedCampaign].Missions[this.selectedMission].Description;
			this.Objectives.text = this.missionsDescriptions.Campaigns[this.selectedCampaign].Missions[this.selectedMission].Objectives;
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000E85C8 File Offset: 0x000E67C8
		private void MoveSelectedMissionArrowsToToggle(GameObject missionToggle)
		{
			Vector3 position = this.SelectedMissionArrows.transform.position;
			position.y = missionToggle.transform.position.y;
			this.SelectedMissionArrows.transform.position = position;
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000E8610 File Offset: 0x000E6810
		private int GetLatestMissionIdForCampaign(int campaignId)
		{
			int num = this.campaignsProgress - this.missionsPerCampaign * campaignId;
			if (num > this.missionsPerCampaign - 1)
			{
				return 2;
			}
			return num;
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000E863C File Offset: 0x000E683C
		public void ChangeCampaign(int campaignId)
		{
			if (campaignId == this.selectedCampaign)
			{
				return;
			}
			this.campaignToggles[this.selectedCampaign].transform.GetChild(0).gameObject.SetActive(false);
			this.selectedCampaign = campaignId;
			this.campaignToggles[this.selectedCampaign].transform.GetChild(0).gameObject.SetActive(true);
			this.UpdateActualFactionEmblem();
			this.UpdateCampaignView();
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000412CC File Offset: 0x0003F4CC
		public void ChangeMission(int missionId)
		{
			if (missionId == this.selectedMission)
			{
				return;
			}
			this.selectedMission = missionId;
			this.MoveSelectedMissionArrowsToToggle(this.missionToggles[this.selectedMission]);
			this.UpdateMissionDescription();
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000412FC File Offset: 0x0003F4FC
		public void OnStartSelectedMission()
		{
			GameController.GameManager.InitCampaign(this.selectedMission, this.selectedCampaign);
			this.mainMenu.StartGame();
		}

		// Token: 0x04001C3A RID: 7226
		public MainMenu mainMenu;

		// Token: 0x04001C3B RID: 7227
		public GameObject FactionsToggleGroup;

		// Token: 0x04001C3C RID: 7228
		public GameObject SelectedMissionArrows;

		// Token: 0x04001C3D RID: 7229
		public GameObject MissionsPanel;

		// Token: 0x04001C3E RID: 7230
		public Image ActualFactionEmblem;

		// Token: 0x04001C3F RID: 7231
		public TextMeshProUGUI MissionTitle;

		// Token: 0x04001C40 RID: 7232
		public Text MissionDescription;

		// Token: 0x04001C41 RID: 7233
		public Text Objectives;

		// Token: 0x04001C42 RID: 7234
		private List<GameObject> campaignToggles = new List<GameObject>();

		// Token: 0x04001C43 RID: 7235
		private List<GameObject> missionToggles = new List<GameObject>();

		// Token: 0x04001C44 RID: 7236
		private int campaignsProgress;

		// Token: 0x04001C45 RID: 7237
		private int selectedCampaign;

		// Token: 0x04001C46 RID: 7238
		private int unlockedCampaigns;

		// Token: 0x04001C47 RID: 7239
		private int selectedMission;

		// Token: 0x04001C48 RID: 7240
		private int missionsPerCampaign = 3;

		// Token: 0x04001C49 RID: 7241
		private string descriptionsFilePath = "Assets/Resources/MissionsDescriptions.xml";

		// Token: 0x04001C4A RID: 7242
		private MissionsDescriptions missionsDescriptions;
	}
}

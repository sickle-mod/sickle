using System;
using I2.Loc;
using Scythe.UI;
using TMPro;
using UnityEngine;

namespace Scythe.GameLogic
{
	// Token: 0x0200055E RID: 1374
	public class ChallengesPresenter : MonoBehaviour
	{
		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06002BF9 RID: 11257 RVA: 0x000283FB File Offset: 0x000265FB
		private GameManager GameManager
		{
			get
			{
				return GameController.GameManager;
			}
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x000F4328 File Offset: 0x000F2528
		private void Start()
		{
			this.cameraMovementEffects = CameraMovementEffects.Instance;
			if (!this.GameManager.IsChallenge)
			{
				return;
			}
			int num = this.GameManager.challengesLogicStarter.GetCurrentChallengeId() + 1;
			this.centerPopupText.text = ScriptLocalization.Get("Challenges/FirstPopupChallenge" + num.ToString());
			this.goalsText1.text = ScriptLocalization.Get("Challenges/Challenge" + num.ToString() + this.goalText + "1");
			this.goalsText2.text = ScriptLocalization.Get("Challenges/Challenge" + num.ToString() + this.goalText + "2");
			this.goalsText3.text = ScriptLocalization.Get("Challenges/Challenge" + num.ToString() + this.goalText + "3");
			for (int i = 0; i < this.actionsCovers.Length; i++)
			{
				this.actionsCovers[i].SetActive(false);
			}
			this.cameraMovementEffects.OnFactionPresentationEnd += this.ActivateChallengeUI;
			switch (this.GameManager.challengesLogicStarter.GetCurrentChallengeId())
			{
			case 0:
				this.StartChallenge1();
				return;
			case 1:
				this.StartChallenge2();
				return;
			case 2:
				this.StartChallenge3();
				return;
			case 3:
				this.StartChallenge4();
				return;
			default:
				Debug.LogWarning("Challenge with this ID: " + this.GameManager.missionId.ToString() + " doesn't exist");
				return;
			}
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x0004403F File Offset: 0x0004223F
		public void DisablePopup()
		{
			this.centerPopup.SetActive(false);
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x0004404D File Offset: 0x0004224D
		private void OnDisable()
		{
			this.cameraMovementEffects.OnFactionPresentationEnd -= this.ActivateChallengeUI;
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x00044066 File Offset: 0x00042266
		private void ActivateChallengeUI()
		{
			this.cameraMovementEffects.OnFactionPresentationEnd -= this.ActivateChallengeUI;
			this.goalsPanel.SetActive(true);
			this.centerPopup.SetActive(true);
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x000F44A8 File Offset: 0x000F26A8
		private void StartChallenge1()
		{
			this.actionsCovers[0].SetActive(true);
			for (int i = 5; i < 10; i++)
			{
				this.actionsCovers[i].SetActive(true);
			}
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void StartChallenge2()
		{
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void StartChallenge3()
		{
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void StartChallenge4()
		{
		}

		// Token: 0x04001E48 RID: 7752
		[SerializeField]
		private GameObject[] actionsCovers;

		// Token: 0x04001E49 RID: 7753
		[SerializeField]
		private TextMeshProUGUI centerPopupText;

		// Token: 0x04001E4A RID: 7754
		[SerializeField]
		private GameObject darkenUI;

		// Token: 0x04001E4B RID: 7755
		[SerializeField]
		private GameObject goalsPanel;

		// Token: 0x04001E4C RID: 7756
		[SerializeField]
		private TextMeshProUGUI goalsText1;

		// Token: 0x04001E4D RID: 7757
		[SerializeField]
		private TextMeshProUGUI goalsText2;

		// Token: 0x04001E4E RID: 7758
		[SerializeField]
		private TextMeshProUGUI goalsText3;

		// Token: 0x04001E4F RID: 7759
		[SerializeField]
		private GameObject centerPopup;

		// Token: 0x04001E50 RID: 7760
		[SerializeField]
		private GameObject endPopup;

		// Token: 0x04001E51 RID: 7761
		private CameraMovementEffects cameraMovementEffects;

		// Token: 0x04001E52 RID: 7762
		private readonly string goalText = "Goal";
	}
}

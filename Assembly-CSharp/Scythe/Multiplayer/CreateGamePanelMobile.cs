using System;
using I2.Loc;
using Scythe.Analytics;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x020001F3 RID: 499
	public class CreateGamePanelMobile : CreateGamePanel
	{
		// Token: 0x06000E8F RID: 3727 RVA: 0x0003178C File Offset: 0x0002F98C
		public override void Activate()
		{
			this.currentStep = 1;
			this.HideAllSteps();
			this.ResetCurrentStep();
			this.OpenCurrentStep();
			base.gameObject.SetActive(true);
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00029172 File Offset: 0x00027372
		public override void Deactivate()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x000317B3 File Offset: 0x0002F9B3
		public void OnNexStepClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.currentStep++;
			this.HideAllSteps();
			this.ResetCurrentStep();
			this.OpenCurrentStep();
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x000317E1 File Offset: 0x0002F9E1
		public void OnBackStepCliked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.currentStep--;
			this.HideAllSteps();
			this.OpenCurrentStep();
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00031809 File Offset: 0x0002FA09
		private void HideAllSteps()
		{
			this.step1.SetActive(false);
			this.step2.SetActive(false);
			this.step3.SetActive(false);
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0008A5F8 File Offset: 0x000887F8
		private void ResetCurrentStep()
		{
			switch (this.currentStep)
			{
			case 1:
				this.ResetStep1();
				return;
			case 2:
				this.ResetStep2();
				return;
			case 3:
				this.ResetStep3();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0008A638 File Offset: 0x00088838
		private void OpenCurrentStep()
		{
			this.SetCurrentStepTextIndicator();
			switch (this.currentStep)
			{
			case 1:
				this.step1.SetActive(true);
				this.RefreshButtons();
				return;
			case 2:
				this.step2.SetActive(true);
				this.RefreshButtons();
				return;
			case 3:
				this.step3.SetActive(true);
				this.RefreshButtons();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x0008A6A0 File Offset: 0x000888A0
		private void RefreshButtons()
		{
			switch (this.currentStep)
			{
			case 1:
				this.backButton.gameObject.SetActive(false);
				this.nextButton.gameObject.SetActive(true);
				this.createButton.gameObject.SetActive(false);
				return;
			case 2:
				this.backButton.gameObject.SetActive(true);
				this.nextButton.gameObject.SetActive(true);
				this.createButton.gameObject.SetActive(false);
				return;
			case 3:
				this.backButton.gameObject.SetActive(true);
				this.nextButton.gameObject.SetActive(false);
				this.createButton.gameObject.SetActive(true);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x0003182F File Offset: 0x0002FA2F
		private void SetCurrentStepTextIndicator()
		{
			this.stepIndicatorText.text = this.currentStep.ToString() + "/3";
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0008A764 File Offset: 0x00088964
		private void ResetStep1()
		{
			this.synchronousToggleGroup.allowSwitchOff = true;
			foreach (Toggle toggle in this.synchronousToggles)
			{
				toggle.isOn = true;
				toggle.isOn = false;
			}
			this.nextButton.interactable = false;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00031851 File Offset: 0x0002FA51
		public void OnPlayAndStayClicked(bool toggle)
		{
			if (toggle)
			{
				this.synchronousGameType = true;
				this.synchronousToggleGroup.allowSwitchOff = false;
				this.nextButton.interactable = true;
			}
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00031875 File Offset: 0x0002FA75
		public void OnPlayAndGoClicked(bool toggle)
		{
			if (toggle)
			{
				this.synchronousGameType = false;
				this.synchronousToggleGroup.allowSwitchOff = false;
				this.nextButton.interactable = true;
			}
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0008A7B0 File Offset: 0x000889B0
		private void ResetStep2()
		{
			this.eloAmountScript.Reset();
			this.rankedGameToggle.isOn = true;
			this.normalGameToggle.isOn = false;
			this.rankedGameType = this.rankedGameToggle.isOn;
			this.synchronousGameDescriptionObject.SetActive(this.synchronousGameType);
			this.asynchronousGameDescriptionObject.SetActive(!this.synchronousGameType);
			this.CheckIFADLC();
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00031899 File Offset: 0x0002FA99
		private void CheckIFADLC()
		{
			this.ifaToggle.isOn = GameServiceController.Instance.InvadersFromAfarUnlocked();
			this.ifaToggle.gameObject.SetActive(GameServiceController.Instance.InvadersFromAfarUnlocked());
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x000318CA File Offset: 0x0002FACA
		public void OnRankedGameClicked(bool toggle)
		{
			if (toggle)
			{
				this.rankedGameType = true;
				this.eloAmountObject.SetActive(this.rankedGameType);
			}
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x000318E7 File Offset: 0x0002FAE7
		public void OnNormalGameClicked(bool toggle)
		{
			if (toggle)
			{
				this.rankedGameType = false;
				this.eloAmountObject.SetActive(this.rankedGameType);
			}
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0008A81C File Offset: 0x00088A1C
		private void ResetStep3()
		{
			this.playersAmountScript.Init(this.ifaToggle.isOn);
			this.playersAmountScript.SetDefaultPlayersAmount(this.ifaToggle.isOn);
			this.playerTimeScript.Init(!this.synchronousGameType);
			this.promoCardsToggle.isOn = true;
			this.matsChoicesToggle.isOn = true;
			this.privateGameToggle.isOn = false;
			this.promoCardsObject.SetActive(!this.rankedGameType);
			this.matsChoicesObject.SetActive(!this.rankedGameType);
			this.privateGameObject.SetActive(!this.rankedGameType);
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0008A8CC File Offset: 0x00088ACC
		protected string GetAutoGeneratedRoomName()
		{
			return string.Format("{0} {1} {2}", ScriptLocalization.Get("Common/Game"), PlayerInfo.me.PlayerStats.Name, DateTime.Now.ToShortDateString());
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0008A90C File Offset: 0x00088B0C
		public void OnCreateGameClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.create);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_create_game_button);
			if (this.rankedGameType)
			{
				LobbyRoom lobbyRoom = new LobbyRoom(this.GetAutoGeneratedRoomName(), this.playersAmountScript.GetPlayersAmount(), this.playerTimeScript.GetTimeInMinutes(), this.rankedGameType, this.eloAmountScript.GetMin(), this.eloAmountScript.GetMax(), true, !this.synchronousGameType, false, this.ifaToggle.isOn, true);
				this.lobbyScript.CreateRoom(lobbyRoom);
				return;
			}
			LobbyRoom lobbyRoom2 = new LobbyRoom(this.GetAutoGeneratedRoomName(), this.playersAmountScript.GetPlayersAmount(), this.playerTimeScript.GetTimeInMinutes(), this.rankedGameType, this.eloAmountScript.GetMin(), this.eloAmountScript.GetMax(), this.promoCardsToggle.isOn, !this.synchronousGameType, this.privateGameToggle.isOn, this.ifaToggle.isOn, !this.matsChoicesToggle.isOn);
			this.lobbyScript.CreateRoom(lobbyRoom2);
		}

		// Token: 0x04000B5C RID: 2908
		[Header("Mobile UI")]
		[SerializeField]
		private Lobby lobbyScript;

		// Token: 0x04000B5D RID: 2909
		[SerializeField]
		private GameObject step1;

		// Token: 0x04000B5E RID: 2910
		[SerializeField]
		private GameObject step2;

		// Token: 0x04000B5F RID: 2911
		[SerializeField]
		private GameObject step3;

		// Token: 0x04000B60 RID: 2912
		[SerializeField]
		private TextMeshProUGUI stepIndicatorText;

		// Token: 0x04000B61 RID: 2913
		[SerializeField]
		private Button backButton;

		// Token: 0x04000B62 RID: 2914
		[SerializeField]
		private Button nextButton;

		// Token: 0x04000B63 RID: 2915
		[SerializeField]
		private Button createButton;

		// Token: 0x04000B64 RID: 2916
		[SerializeField]
		private ToggleGroup synchronousToggleGroup;

		// Token: 0x04000B65 RID: 2917
		[SerializeField]
		private Toggle[] synchronousToggles;

		// Token: 0x04000B66 RID: 2918
		[SerializeField]
		private Toggle rankedGameToggle;

		// Token: 0x04000B67 RID: 2919
		[SerializeField]
		private Toggle normalGameToggle;

		// Token: 0x04000B68 RID: 2920
		[SerializeField]
		private GameObject synchronousGameDescriptionObject;

		// Token: 0x04000B69 RID: 2921
		[SerializeField]
		private GameObject asynchronousGameDescriptionObject;

		// Token: 0x04000B6A RID: 2922
		[SerializeField]
		private ELOAmount eloAmountScript;

		// Token: 0x04000B6B RID: 2923
		[SerializeField]
		private GameObject eloAmountObject;

		// Token: 0x04000B6C RID: 2924
		[SerializeField]
		private Toggle ifaToggle;

		// Token: 0x04000B6D RID: 2925
		[SerializeField]
		private PlayersAmount playersAmountScript;

		// Token: 0x04000B6E RID: 2926
		[SerializeField]
		private PlayerTimeMobile playerTimeScript;

		// Token: 0x04000B6F RID: 2927
		[SerializeField]
		private Toggle promoCardsToggle;

		// Token: 0x04000B70 RID: 2928
		[SerializeField]
		private GameObject promoCardsObject;

		// Token: 0x04000B71 RID: 2929
		[SerializeField]
		private Toggle matsChoicesToggle;

		// Token: 0x04000B72 RID: 2930
		[SerializeField]
		private GameObject matsChoicesObject;

		// Token: 0x04000B73 RID: 2931
		[SerializeField]
		private Toggle privateGameToggle;

		// Token: 0x04000B74 RID: 2932
		[SerializeField]
		private GameObject privateGameObject;

		// Token: 0x04000B75 RID: 2933
		private int currentStep = 1;

		// Token: 0x04000B76 RID: 2934
		private bool synchronousGameType;

		// Token: 0x04000B77 RID: 2935
		private bool rankedGameType;
	}
}

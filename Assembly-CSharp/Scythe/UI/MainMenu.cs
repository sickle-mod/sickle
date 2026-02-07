using System;
using System.Collections;
using System.Collections.Generic;
using Common.GameSaves;
using I2.Loc;
using Multiplayer.AuthApi;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.AuthApi;
using Scythe.Multiplayer.AuthApi.Models;
using Scythe.Multiplayer.Data;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Scythe.UI
{
	// Token: 0x020004DF RID: 1247
	public class MainMenu : SingletonMono<MainMenu>
	{
		// Token: 0x17000324 RID: 804
		// (get) Token: 0x060027A8 RID: 10152 RVA: 0x00041444 File Offset: 0x0003F644
		// (set) Token: 0x060027A9 RID: 10153 RVA: 0x0004144C File Offset: 0x0003F64C
		public GameObject LoginMenuParent { get; set; }

		// Token: 0x060027AA RID: 10154 RVA: 0x00041455 File Offset: 0x0003F655
		private void Awake()
		{
			if (!PlatformManager.IsStandalone)
			{
				this.PreloadScreens(true);
			}
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x000E90A0 File Offset: 0x000E72A0
		private void Start()
		{
			this.connectingPanel = SceneController.Instance.GetConnectionPanel();
			if (!AchievementManager.AchievementsListGenerated())
			{
				AchievementManager.GenerateAchievementList();
			}
			if (PlatformManager.IsMobile)
			{
				QualitySettings.vSyncCount = -1;
			}
			global::UnityEngine.Random.InitState(DateTime.Now.Millisecond);
			if ((GameController.Instance == null || (GameController.Instance != null && !GameController.Instance.GameIsLoaded)) && !MainMenu.OpenExtrasOnLoad)
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
			}
			if (MainMenu.loadState == MainMenu.State.Campaign)
			{
				this.CampaignMenu();
			}
			MainMenu.loadState = MainMenu.State.None;
			this.optionsMenu.GetComponent<OptionsManager>().LoadOptionsPrefs();
			base.StartCoroutine(Censor.Init());
			if (PlatformManager.IsStandalone)
			{
				GameObject[] array = this.menuButtons;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].GetComponent<PointerEventsController>().buttonHoover += this.MenuButtonsHoover;
				}
			}
			if (PlatformManager.IsMobile)
			{
				this.PreloadScreens(false);
				if (this.menuQuitButton != null)
				{
					this.menuQuitButton.SetActive(false);
				}
			}
			if (!string.IsNullOrEmpty(MainMenu.loadGameFile))
			{
				this.LoadGame(MainMenu.loadGameFile);
				MainMenu.AfterLoadingTheGame();
			}
			this.LaunchMobileOrPCMenu();
			this.CheckOwnedDLCs();
			if (MainMenu.OpenExtrasOnLoad)
			{
				if (this.extrasButton != null)
				{
					this.extrasButton.onClick.Invoke();
				}
				MainMenu.OpenExtrasOnLoad = false;
			}
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x000E9204 File Offset: 0x000E7404
		private void OnEnable()
		{
			Singleton<LoginController>.Instance.OnLoginAttempt += this.LoginController_OnLoginAttempt;
			Singleton<LoginController>.Instance.OnLoginSuccess += this.LoginController_OnLoginSuccess;
			Singleton<LoginController>.Instance.OnLoginFailure += this.LoginController_OnLoginFailure;
			Singleton<LoginController>.Instance.OnLoginError += this.LoginController_OnLoginError;
			Singleton<LoginController>.Instance.OnLogoutSuccess += this.LoginController_OnLogoutSuccess;
			Singleton<LoginController>.Instance.OnMigrationNeeded += this.LoginController_OnMigrationNeeded;
			Singleton<LoginController>.Instance.OnAccountMigrationSuccess += this.LoginController_OnMigrationSuccess;
			Singleton<LoginController>.Instance.OnAccountMigrationFailure += this.LoginController_OnMigrationFailure;
			Singleton<LoginController>.Instance.OnAccountMigrationError += this.LoginController_OnMigrationError;
			Singleton<LoginController>.Instance.OnRegisterSuccess += this.LoginController_OnRegisterSuccess;
			Singleton<LoginController>.Instance.OnRegisterFailure += this.LoginController_OnRegisterFailure;
			Singleton<LoginController>.Instance.OnRegisterError += this.LoginController_OnRegisterError;
			Singleton<LoginController>.Instance.OnConnectionError += this.ShowConnectionErrorWindow;
			Singleton<LoginController>.Instance.OnAuthenticationSuccess += this.LoginController_OnAuthenticationCompleted;
			Singleton<LoginController>.Instance.PasswordChangedEvent += this.OnChangePasswordSuccess;
			Singleton<LoginController>.Instance.OnPasswordChangeErrorEvent += this.OnChangePasswordError;
			Singleton<LoginController>.Instance.OnEmailChangeSuccess += this.LoginController_OnEmailChangeSuccess;
			Singleton<LoginController>.Instance.OnDeleteAccountSuccess += this.LoginController_OnDeleteAccountSuccess;
			Singleton<LoginController>.Instance.OnPasswordResetEvent += this.LoginController_OnResetPasswordSuccess;
			Singleton<LoginController>.Instance.OnPasswordResetErrorEvent += this.LoginController_OnResetPasswordError;
			Singleton<LoginController>.Instance.OnServerCallWaitPanel += this.ShowAuthenticatingWindow;
			Singleton<LoginController>.Instance.OnServerCallWaitPanelHide += this.HideAuthenticatingWindow;
			Singleton<LoginController>.Instance.OnActivationNeeded += this.LoginController_OnActivationNeeded;
			Singleton<LoginController>.Instance.OnRegisterEmailAlreadyInUse += this.LoginController_OnRegisterFailureEmailInUse;
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x000E9424 File Offset: 0x000E7624
		private void OnDisable()
		{
			Singleton<LoginController>.Instance.OnLoginAttempt -= this.LoginController_OnLoginAttempt;
			Singleton<LoginController>.Instance.OnLoginSuccess -= this.LoginController_OnLoginSuccess;
			Singleton<LoginController>.Instance.OnLoginFailure -= this.LoginController_OnLoginFailure;
			Singleton<LoginController>.Instance.OnLoginError -= this.LoginController_OnLoginError;
			Singleton<LoginController>.Instance.OnLogoutSuccess -= this.LoginController_OnLogoutSuccess;
			Singleton<LoginController>.Instance.OnMigrationNeeded -= this.LoginController_OnMigrationNeeded;
			Singleton<LoginController>.Instance.OnAccountMigrationSuccess -= this.LoginController_OnMigrationSuccess;
			Singleton<LoginController>.Instance.OnAccountMigrationFailure -= this.LoginController_OnMigrationFailure;
			Singleton<LoginController>.Instance.OnAccountMigrationError -= this.LoginController_OnMigrationError;
			Singleton<LoginController>.Instance.OnRegisterSuccess -= this.LoginController_OnRegisterSuccess;
			Singleton<LoginController>.Instance.OnRegisterFailure -= this.LoginController_OnRegisterFailure;
			Singleton<LoginController>.Instance.OnRegisterError -= this.LoginController_OnRegisterError;
			Singleton<LoginController>.Instance.OnConnectionError -= this.ShowConnectionErrorWindow;
			Singleton<LoginController>.Instance.OnAuthenticationSuccess -= this.LoginController_OnAuthenticationCompleted;
			Singleton<LoginController>.Instance.PasswordChangedEvent -= this.OnChangePasswordSuccess;
			Singleton<LoginController>.Instance.OnPasswordChangeErrorEvent -= this.OnChangePasswordError;
			Singleton<LoginController>.Instance.OnEmailChangeSuccess -= this.LoginController_OnEmailChangeSuccess;
			Singleton<LoginController>.Instance.OnDeleteAccountSuccess -= this.LoginController_OnDeleteAccountSuccess;
			Singleton<LoginController>.Instance.OnPasswordResetEvent -= this.LoginController_OnResetPasswordSuccess;
			Singleton<LoginController>.Instance.OnPasswordResetErrorEvent -= this.LoginController_OnResetPasswordError;
			Singleton<LoginController>.Instance.OnServerCallWaitPanel -= this.ShowAuthenticatingWindow;
			Singleton<LoginController>.Instance.OnServerCallWaitPanelHide += this.HideAuthenticatingWindow;
			Singleton<LoginController>.Instance.OnActivationNeeded -= this.LoginController_OnActivationNeeded;
			Singleton<LoginController>.Instance.OnRegisterEmailAlreadyInUse -= this.LoginController_OnRegisterFailureEmailInUse;
			AssetBundle.UnloadAllAssetBundles(true);
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x00041465 File Offset: 0x0003F665
		private void PreloadScreens(bool active)
		{
			if (this.factionMatSelectionWindow != null)
			{
				this.factionMatSelectionWindow.gameObject.SetActive(active);
			}
			if (this.playerMatSelectionWindow != null)
			{
				this.playerMatSelectionWindow.gameObject.SetActive(active);
			}
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x000414A5 File Offset: 0x0003F6A5
		private bool RegisterMenuActive()
		{
			return PlatformManager.IsStandalone && this.registerPanel.gameObject.activeSelf;
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x0003A890 File Offset: 0x00038A90
		public void StartGame()
		{
			SceneController.Instance.LoadScene(SceneController.SCENE_MAIN_NAME);
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x000E9648 File Offset: 0x000E7848
		private void InitGame()
		{
			List<MatAndFactionSelection.PlayerEntry> playersAndTheirSettings = this.hotseatMenu.GetComponent<HotseatPanelController>().GetPlayersAndTheirSettings();
			this.hotseatMenu.GetComponent<HotseatPanelController>().SaveHotseatPrefs(playersAndTheirSettings);
			Scythe.GameLogic.Game game = GameController.Game;
			game.CreateNewGameId();
			game.CreateNewGameManager();
			game.GameManager.IsMultiplayer = false;
			game.GameManager.UndoType = this.hotseatMenu.GetComponent<HotseatPanelController>().GetUndoType();
			game.GameManager.Init(playersAndTheirSettings, this.hotseatMenu.GetComponent<HotseatPanelController>().PromoCardsUnlocked(), this.hotseatMenu.GetComponent<HotseatPanelController>().InvadersFromAfarUnlocked(), this.hotseatMenu.GetComponent<HotseatPanelController>().Balanced());
		}

		// Token: 0x060027B2 RID: 10162 RVA: 0x000414C0 File Offset: 0x0003F6C0
		public void OnStartGame()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_new_game_button);
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.main);
			GameController.gameFromSave = false;
			this.InitGame();
			this.StartGame();
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x000414ED File Offset: 0x0003F6ED
		public void OnLoadGameClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_load_game_window_button);
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x000E96EC File Offset: 0x000E78EC
		public void LoadGame(string file)
		{
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.load);
			Scythe.GameLogic.Game game = GameController.Game;
			game.CreateNewGameManager();
			GameController.gameFromSave = true;
			game.GameManager.IsMultiplayer = false;
			game.GameManager.missionId = -1;
			GameSavesManager.LoadGame(file);
			game.GameManager.IsHotSeat = true;
			this.StartGame();
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x00041502 File Offset: 0x0003F702
		public static void AfterLoadingTheGame()
		{
			MainMenu.loadGameFile = string.Empty;
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x0004150E File Offset: 0x0003F70E
		public static void SetLoadGameFile(string gameFile)
		{
			MainMenu.loadGameFile = gameFile;
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x000E9740 File Offset: 0x000E7940
		public void ShowLoginSettings()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			this.LoginMenuParent = this.optionsMenu;
			if (Singleton<LoginController>.Instance.IsPlayerLoggedIn)
			{
				this.EnterLoginSettings(null);
				return;
			}
			if (Singleton<LoginController>.Instance.IsPlayerLoggedOut)
			{
				this.ShowDefaultMultiplayerMenu();
				return;
			}
			Singleton<LoginController>.Instance.OnLoginSuccess += this.EnterLoginSettings;
			Singleton<LoginController>.Instance.TryToAutoLogin(false);
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x00041516 File Offset: 0x0003F716
		public void ShowEditProfilePanel()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			this.editProfilePanel.gameObject.SetActive(true);
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x00041535 File Offset: 0x0003F735
		public void ShowDeleteAccountPanel()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			this.deleteAccountPanel.gameObject.SetActive(true);
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x00041554 File Offset: 0x0003F754
		public void ShowChangePasswordPanel()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			this.changePasswordPanel.gameObject.SetActive(true);
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x000E97B0 File Offset: 0x000E79B0
		private void NormalLogin()
		{
			this.connectingPanel.HideConnectionWindow();
			if (this.optionsMenu.activeSelf && PlatformManager.IsStandalone)
			{
				this.optionsMenu.GetComponent<OptionsManager>().OptionsPanel.SetActive(true);
				this.optionsMenu.SetActive(false);
			}
			this.multiplayerButton.interactable = true;
			this.ShowDefaultMultiplayerMenu();
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x00041573 File Offset: 0x0003F773
		private void OnEnterToMultiplayerMenu()
		{
			if (Singleton<LoginController>.Instance.IsPlayerLoggedOut)
			{
				this.NormalLogin();
				return;
			}
			Singleton<LoginController>.Instance.OnLoginSuccess -= this.EnterLoginSettings;
			Singleton<LoginController>.Instance.TryToAutoLogin(false);
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000E9810 File Offset: 0x000E7A10
		private void OnStandAlonePlayOnlineClick()
		{
			PlayerInfo.me.PlayerStats.Name = this.playerName.text;
			PlayerPrefs.SetString("PlayerName", this.playerName.text);
			Singleton<LoginController>.Instance.Login(this.playerName.text, this.password.text);
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x000E986C File Offset: 0x000E7A6C
		private void OnMobilePlayOnlineClick()
		{
			PlayerInfo.me.PlayerStats.Name = this.playerNameTMP.text;
			PlayerPrefs.SetString("PlayerName", this.playerNameTMP.text);
			Singleton<LoginController>.Instance.Login(this.playerNameTMP.text, this.passwordTMP.text);
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x000E98C8 File Offset: 0x000E7AC8
		public void OnPlayOnlineClick()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_login_button);
			this.loginButton.interactable = false;
			this.newAccountButton.interactable = false;
			this.backButton.interactable = false;
			this.resetPasswordMenuButton.interactable = false;
			if (PlatformManager.IsStandalone)
			{
				this.OnStandAlonePlayOnlineClick();
			}
			if (PlatformManager.IsMobile)
			{
				this.mobileLoginBackButton.interactable = false;
				this.mobileLoginResetButton.interactable = false;
				this.OnMobilePlayOnlineClick();
			}
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x000E994C File Offset: 0x000E7B4C
		private void EnterLoginSettings(LoginResponse result = null)
		{
			if (PlatformManager.IsStandalone)
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
				this.optionsMenu.GetComponent<OptionsManager>().OptionsPanel.SetActive(false);
			}
			Singleton<LoginController>.Instance.OnLoginSuccess -= this.EnterLoginSettings;
			this.optionsMenu.SetActive(true);
			this.loginSettings.SetActive(true);
			this.connectingPanel.SetActive(false);
			this.multiplayerMenu.SetActive(false);
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x000415A9 File Offset: 0x0003F7A9
		private IEnumerator LoadLobbyScene()
		{
			AsyncOperation async = SceneManager.LoadSceneAsync(2);
			while (!async.isDone)
			{
				yield return async;
			}
			yield break;
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x000415B1 File Offset: 0x0003F7B1
		public void OnMigrationSuccess()
		{
			this.migrationPanel.RegisterMigrationAccountSuccess();
			this.migrationSuccessPanel.gameObject.SetActive(true);
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000415CF File Offset: 0x0003F7CF
		public void OnMigrationError(BasicApiResultDto error)
		{
			this.migrationPanel.RegisterMigrationAccountError(error.Message);
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x000415E2 File Offset: 0x0003F7E2
		public void OnChangePasswordSuccess()
		{
			this.changePasswordPanel.ChangePasswordSuccess();
			this.changePasswordSuccessPanel.gameObject.SetActive(true);
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x00041600 File Offset: 0x0003F800
		public void OnChangePasswordError(string errorText)
		{
			this.changePasswordPanel.ChangePasswordError(errorText);
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x0004160E File Offset: 0x0003F80E
		public void OnAccountAlreadyLinkedError()
		{
			if (PlatformManager.IsStandalone)
			{
				this.accountAlreadyLinked.SetActive(true);
			}
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x00041623 File Offset: 0x0003F823
		public void LoginController_OnResetPasswordSuccess()
		{
			this.resetPasswordPanel.ResetPasswordSuccess();
			this.resetPasswordSuccess.gameObject.SetActive(true);
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x00041600 File Offset: 0x0003F800
		public void LoginController_OnResetPasswordError(string errorText)
		{
			this.changePasswordPanel.ChangePasswordError(errorText);
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x00041641 File Offset: 0x0003F841
		public void OnResetPasswordSuccessBackClicked()
		{
			this.ClearResetPasswordMenu();
			this.resetPasswordPanel.gameObject.SetActive(false);
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x000E99C8 File Offset: 0x000E7BC8
		public void OnLoginPanelBackClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_back_button);
			if (this.optionsMenu.activeInHierarchy)
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.settings, Contexts.outgame);
				return;
			}
			this.defaultMenu.SetActive(true);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x0004165A File Offset: 0x0003F85A
		public void OnPrivacyPolicyCliked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			Application.OpenURL("https://www.theknightsofunity.com/privacypolicy");
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x00041672 File Offset: 0x0003F872
		public void TutorialButtonClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_tutorial_button);
			this.CampaignMenu();
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x0004168D File Offset: 0x0003F88D
		public void CampaignMenu()
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.tutorial_setup, Contexts.outgame);
			this.defaultMenu.SetActive(false);
			this.campaignMenu.SetActive(true);
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x000E9A1C File Offset: 0x000E7C1C
		public void OnPlayLocalClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_play_local_button);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			LoadSaveMenu component = this.loadSaveWindow.transform.Find("LoadSaveWindow").GetComponent<LoadSaveMenu>();
			if (PlatformManager.IsStandalone)
			{
				if (this.resumeGameButton != null)
				{
					this.resumeGameButton.interactable = component.AutomaticSaveFileCanBeLoaded();
					return;
				}
			}
			else if (this.resumeGameButton != null && this.resumeDummy != null)
			{
				this.resumeGameButton.gameObject.SetActive(component.AutomaticSaveFileCanBeLoaded());
				this.resumeDummy.SetActive(!component.AutomaticSaveFileCanBeLoaded());
			}
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x000416B3 File Offset: 0x0003F8B3
		public void OnBackToMenuFromPlayLocalClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_back_button);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
			this.playLocal.SetActive(false);
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x000E9AC4 File Offset: 0x000E7CC4
		public void OnLoadSaveClosed()
		{
			LoadSaveMenu component = this.loadSaveWindow.transform.Find("LoadSaveWindow").GetComponent<LoadSaveMenu>();
			if (PlatformManager.IsStandalone)
			{
				if (this.resumeGameButton != null)
				{
					this.resumeGameButton.interactable = component.AutomaticSaveFileCanBeLoaded();
					return;
				}
			}
			else if (this.resumeGameButton != null && this.resumeDummy != null)
			{
				this.resumeGameButton.gameObject.SetActive(component.AutomaticSaveFileCanBeLoaded());
				this.resumeDummy.SetActive(!component.AutomaticSaveFileCanBeLoaded());
			}
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x000416DF File Offset: 0x0003F8DF
		public void LocalPlayWindow(bool show)
		{
			if (show)
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.play_local_window, Contexts.outgame);
				return;
			}
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x000E9B5C File Offset: 0x000E7D5C
		public void HotSeatMenu()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_hotseat_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.game_setup, Contexts.outgame);
			if (PlatformManager.IsStandalone)
			{
				this.multiplayerMenu.SetActive(false);
			}
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			this.hotseatMenu.SetActive(true);
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x000416FD File Offset: 0x0003F8FD
		public void MultiplayerMenu()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_online_button);
			this.LoginMenuParent = base.gameObject;
			this.OnEnterToMultiplayerMenu();
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x000E9BA8 File Offset: 0x000E7DA8
		public void ShowDefaultMultiplayerMenu()
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.login, Contexts.outgame);
			if (PlatformManager.IsStandalone)
			{
				this.playerName.text = PlayerPrefs.GetString("PlayerName", string.Empty);
			}
			if (PlatformManager.IsMobile)
			{
				this.mobileLoginBackButton.interactable = true;
				this.mobileLoginResetButton.interactable = true;
			}
			this.defaultMenu.SetActive(false);
			this.hotseatMenu.SetActive(false);
			this.multiplayerMenu.SetActive(true);
			if (PlatformManager.IsMobile)
			{
				this.playerNameTMP.Clear();
				this.passwordTMP.Clear();
				this.playerNameTMP.text = PlayerPrefs.GetString("PlayerName", string.Empty);
			}
			this.loginButton.interactable = true;
			this.newAccountButton.interactable = true;
			this.resetPasswordMenuButton.interactable = true;
			if (PlatformManager.IsStandalone)
			{
				this.optionsMenu.SetActive(false);
				this.loginErrorText.text = "";
				this.password.text = "";
			}
			if (PlatformManager.IsMobile)
			{
				this.loginErrorTextTMP.text = "";
				this.passwordTMP.text = "";
			}
			this.backButton.interactable = true;
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x00041724 File Offset: 0x0003F924
		public void ShowMultiplayerMenuForMigration()
		{
			this.ShowDefaultMultiplayerMenu();
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x0004172C File Offset: 0x0003F92C
		public void EnableLoginPanel(bool enabled)
		{
			this.multiplayerMenu.SetActive(enabled);
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x0004173A File Offset: 0x0003F93A
		public void Album()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_album_button);
			this.defaultMenu.SetActive(false);
			this.album.SetActive(true);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x000E9CE8 File Offset: 0x000E7EE8
		public void OptionsMenu()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_option_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.settings, Contexts.outgame);
			if (PlatformManager.IsStandalone)
			{
				this.defaultMenu.SetActive(false);
			}
			this.optionsMenu.SetActive(true);
			this.optionsMenu.GetComponent<OptionsManager>().ShowOptionsPanel();
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x00041767 File Offset: 0x0003F967
		public void ShowroomEnter()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_showroom_button);
			SceneController.Instance.LoadScene(SceneController.SCENE_SHOWROOM_NAME);
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x0004178B File Offset: 0x0003F98B
		public void XButtonClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x0004179F File Offset: 0x0003F99F
		public void ShowHotseatPanel()
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.pass_and_play_setup, Contexts.outgame);
			this.hotseatMenu.transform.Find("HotseatWindow").gameObject.SetActive(true);
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x000E9D44 File Offset: 0x000E7F44
		public void ShowStatsPanel()
		{
			PlayerStats localPlayerStats = LocalStats.GetLocalPlayerStats();
			localPlayerStats.Name = string.Empty;
			if (PlatformManager.IsStandalone)
			{
				this.playerName.text = localPlayerStats.Name;
			}
			if (PlatformManager.IsMobile)
			{
				this.playerNameTMP.text = localPlayerStats.Name;
			}
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_more_stats_button);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			this.statsPresenter.ShowStats(localPlayerStats, Screens.play_local_window, true);
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x000E9DB4 File Offset: 0x000E7FB4
		public void HideOptions()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
			if (this.extrasMenu != null && this.extrasMenu.activeInHierarchy)
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.extras, Contexts.outgame);
			}
			else if (this.playLocal.activeInHierarchy)
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.play_local_window, Contexts.outgame);
			}
			else
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
			}
			this.optionsMenu.SetActive(false);
			this.defaultMenu.SetActive(true);
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x000417CD File Offset: 0x0003F9CD
		public void OnRegisterMenuStateButtonClicked(bool enabled)
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			if (enabled)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_new_account_button);
			}
			else
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
			}
			this.RegisterMenu(enabled);
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x000417F4 File Offset: 0x0003F9F4
		public void RegisterMenu(bool show)
		{
			if (show)
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.register_menu, Contexts.outgame);
			}
			else
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.login, Contexts.outgame);
			}
			this.EnableLoginPanel(false);
			this.registerPanel.gameObject.SetActive(show);
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x0004182D File Offset: 0x0003FA2D
		public void CloseFactionMatSelection()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.esc);
			this.factionMatSelectionWindow.CloseWindow(true);
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x00041841 File Offset: 0x0003FA41
		private void ClosePlayerMatSelection()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.esc);
			this.playerMatSelectionWindow.CloseWindow(true);
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000E9E3C File Offset: 0x000E803C
		public void ResetPasswordMenu(bool show)
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			if (show)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_reset_password_button);
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.reset_password, Contexts.outgame);
			}
			else
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.login, Contexts.outgame);
			}
			this.EnableLoginPanel(!show);
			this.resetPasswordPanel.gameObject.SetActive(show);
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000E9E9C File Offset: 0x000E809C
		public void DefaultMenu()
		{
			if (PlatformManager.IsStandalone && (this.multiplayerMenu.activeInHierarchy || this.playLocal.activeInHierarchy || this.campaignMenu.activeInHierarchy || this.registerPanel.gameObject.activeInHierarchy || this.accountCreatedSuccess.gameObject.activeInHierarchy || this.optionsMenu.activeInHierarchy))
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
			}
			this.LaunchMobileOrPCMenu();
			this.playLocal.SetActive(false);
			this.LoginMenuParent = null;
			Singleton<LoginController>.Instance.OnLoginSuccess -= this.EnterLoginSettings;
			Singleton<LoginController>.Instance.IsPlayerLoggedOut = false;
			this.multiplayerMenu.SetActive(false);
			this.registerPanel.gameObject.SetActive(false);
			this.accountCreatedSuccess.gameObject.SetActive(false);
			this.campaignMenu.SetActive(false);
			this.album.SetActive(false);
			bool flag = false;
			foreach (Dropdown dropdown in this.optionsMenu.GetComponentsInChildren<Dropdown>())
			{
				if (dropdown.GetComponentsInChildren<ScrollRect>(false).Length != 0)
				{
					dropdown.Hide();
					flag = true;
				}
			}
			if (!flag)
			{
				this.optionsMenu.SetActive(false);
			}
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x00041855 File Offset: 0x0003FA55
		public void CheckOwnedDLCs()
		{
			if (PlatformManager.IsStandalone)
			{
				this.invadersFromAfarBanner.SetActive(GameServiceController.Instance.InvadersFromAfarUnlocked());
				return;
			}
			SingletonMono<ExpansionsAvailableController>.Instance.ShowAvailableExpansions();
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x0004187E File Offset: 0x0003FA7E
		public void OpenInvadersFromAfarStoreSite()
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.dlc_banner_opened_invaders, Contexts.outgame);
			this.OpenInvadersFromAfarSite();
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x00041893 File Offset: 0x0003FA93
		public void OpenInvadersFromAfarSite()
		{
			Application.OpenURL("https://store.steampowered.com/app/1021190");
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x0004189F File Offset: 0x0003FA9F
		public void OnDLCBannerInvadersClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_dlc_banner_invaders);
			this.OpenInvadersFromAfarStoreSite();
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x000418AE File Offset: 0x0003FAAE
		public void OnExtrasClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_extras_button);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			this.extrasMenu.SetActive(true);
			this.defaultMenu.SetActive(false);
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x000418DB File Offset: 0x0003FADB
		public void OnExtrasClosed()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_back_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			this.extrasMenu.SetActive(false);
			this.defaultMenu.SetActive(true);
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x000E9FD8 File Offset: 0x000E81D8
		public void SupportPanelOpen()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			this.supportPanel.OnServerCallWaitPanel += this.ShowAuthenticatingWindow;
			this.supportPanel.OnServerCallWaitPanelHide += this.HideAuthenticatingWindow;
			this.supportPanel.OnSupportMailSent += this.OnSupportMailSent;
			this.supportPanel.gameObject.SetActive(true);
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x000EA048 File Offset: 0x000E8248
		public void OnSupportPanelExit()
		{
			this.supportPanel.OnServerCallWaitPanel -= this.ShowAuthenticatingWindow;
			this.supportPanel.OnServerCallWaitPanelHide -= this.HideAuthenticatingWindow;
			this.supportPanel.OnSupportMailSent -= this.OnSupportMailSent;
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x000EA09C File Offset: 0x000E829C
		private void OnSupportMailSent()
		{
			this.supportPanel.OnServerCallWaitPanel -= this.ShowAuthenticatingWindow;
			this.supportPanel.OnServerCallWaitPanelHide -= this.HideAuthenticatingWindow;
			this.supportPanel.OnSupportMailSent -= this.OnSupportMailSent;
			this.supportPanel.gameObject.SetActive(false);
			this.supportMessageSent.gameObject.SetActive(true);
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x00041913 File Offset: 0x0003FB13
		private void LogErrorBasedOnSignupPath()
		{
			if (this.optionsMenu.activeInHierarchy)
			{
				Debug.LogError(SingupPaths.through_options);
				return;
			}
			Debug.LogError(SingupPaths.through_joining_lobby);
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x00041939 File Offset: 0x0003FB39
		private void ClearResetPasswordMenu()
		{
			this.backButton.interactable = true;
			this.ShowDefaultMultiplayerMenu();
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x0004194D File Offset: 0x0003FB4D
		private void MenuButtonsHoover()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.PlayersBoardShowEnemysboardRelease);
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x0004195B File Offset: 0x0003FB5B
		public void ShowSteamConnectionError(string error)
		{
			this.connectingPanel.HideConnectionWindow();
			if (!string.IsNullOrEmpty(error))
			{
				this.ShowLoginError(error);
			}
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x00041977 File Offset: 0x0003FB77
		public void HideSteamConnectionWindow()
		{
			this.connectingPanel.HideConnectionWindow();
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x00041984 File Offset: 0x0003FB84
		public void CloseConnectionPanel()
		{
			this.DefaultMenu();
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x000EA110 File Offset: 0x000E8310
		private void ShowConnectionErrorWindow()
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.connection_error_popup, Contexts.outgame);
			if (this.connectingPanel.gameObject.activeInHierarchy)
			{
				this.connectingPanel.HideConnectionWindow();
			}
			if (PlatformManager.IsStandalone)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/NoInternetConnection");
			}
			if (PlatformManager.IsMobile)
			{
				this.errorTextTMP.text = ScriptLocalization.Get("MainMenu/NoInternetConnection");
			}
			this.errorPanel.SetActive(true);
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x0004198C File Offset: 0x0003FB8C
		private void ShowLoginError(string error)
		{
			this.HandleGUIVisibility();
			error = this.GetLocalizedErrorText(error);
			this.SetLoginErrorText(error);
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x000419A4 File Offset: 0x0003FBA4
		public void CloseErrorPanel()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_ok_button);
			if (PlatformManager.IsStandalone)
			{
				this.errorPanel.SetActive(false);
			}
			this.optionsMenu.GetComponent<OptionsManager>().OptionsPanel.SetActive(true);
			this.DefaultMenu();
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x000419DB File Offset: 0x0003FBDB
		public void OnRemoveAchievementsClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			AchievementManager.RemoveGainedAchievements();
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x000419EE File Offset: 0x0003FBEE
		public void LaunchMobileOrPCMenu()
		{
			if (PlatformManager.IsStandalone)
			{
				this.logoContainer.SetActive(true);
				this.defaultMenu.SetActive(true);
				return;
			}
			this.defaultMenu.SetActive(true);
			this.optionButtonMobile.SetActive(true);
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x00041A28 File Offset: 0x0003FC28
		public void Exit()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			Application.Quit();
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x00041A3B File Offset: 0x0003FC3B
		public void EnableExitGamePopup()
		{
			this.exitGameDialog.SetActive(true);
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x00041A49 File Offset: 0x0003FC49
		private void HandleGUIVisibility()
		{
			if (!this.multiplayerMenu.activeInHierarchy)
			{
				this.ShowDefaultMultiplayerMenu();
			}
			if (this.connectingPanel.gameObject.activeInHierarchy)
			{
				this.connectingPanel.HideConnectionWindow();
			}
			this.ActivateLoginPanelButtons();
			this.ActivateLoginErrorObject();
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x000EA18C File Offset: 0x000E838C
		public void ActivateLoginPanelButtons()
		{
			this.loginButton.interactable = true;
			this.newAccountButton.interactable = true;
			this.backButton.interactable = true;
			this.multiplayerButton.interactable = true;
			if (PlatformManager.IsMobile)
			{
				this.mobileLoginBackButton.interactable = true;
				this.mobileLoginResetButton.interactable = true;
			}
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x00041A87 File Offset: 0x0003FC87
		private void ActivateLoginErrorObject()
		{
			if (PlatformManager.IsStandalone)
			{
				this.resetPasswordMenuButton.interactable = true;
				this.loginErrorText.gameObject.SetActive(true);
			}
			if (PlatformManager.IsMobile)
			{
				this.loginErrorTextTMP.gameObject.SetActive(true);
			}
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000EA1E8 File Offset: 0x000E83E8
		private void SetLoginErrorText(string error)
		{
			if (PlatformManager.IsStandalone)
			{
				this.loginErrorText.text = error;
				Debug.LogWarning("[MainMenu] " + this.loginErrorText.text);
			}
			if (PlatformManager.IsMobile)
			{
				this.loginErrorTextTMP.text = error;
				Debug.LogWarning("[MainMenu] " + this.loginErrorTextTMP.text);
			}
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x000EA250 File Offset: 0x000E8450
		private string GetLocalizedErrorText(string error)
		{
			if (error.StartsWith("invalid_grant"))
			{
				error = ScriptLocalization.Get("MainMenu/SteamError");
			}
			else if (error == string.Empty)
			{
				error = ScriptLocalization.Get("MainMenu/NoInternetConnection");
			}
			else if (error == "Couldn't find linked Steam account. Please log in with your Asmodee account.")
			{
				error = ScriptLocalization.Get("MainMenu/NoLinkedAccount");
			}
			else if (error.ToLower().StartsWith("invalid username"))
			{
				error = ScriptLocalization.Get("MainMenu/LoginError");
			}
			else if (error == "Game Center Problem")
			{
				error = ScriptLocalization.Get("MainMenu/GameCenterNoUser");
			}
			else if (error.ToLower().Contains("12501"))
			{
				error = ScriptLocalization.Get("MainMenu/NotInstalledGooglePlayOnline");
			}
			else if (error.ToLower().Equals("maintenance"))
			{
				error = ScriptLocalization.Get("MainMenu/ServerMaintenance");
			}
			else
			{
				error = ScriptLocalization.Get("ErrorMessages/GenericError");
			}
			return error;
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x00041AC5 File Offset: 0x0003FCC5
		private void LoginController_OnRegisterSuccess(RegisterResponse result)
		{
			this.registerPanel.RegisterAccountSuccess();
			this.accountCreatedSuccess.gameObject.SetActive(true);
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x00041AE3 File Offset: 0x0003FCE3
		public void LoginController_OnRegisterFailure(FailureResponse result)
		{
			this.LogErrorBasedOnSignupPath();
			this.registerPanel.RegisterAccountError(result.GetErrorsString());
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x00041AFC File Offset: 0x0003FCFC
		public void LoginController_OnRegisterError(string error)
		{
			this.registerPanel.RegisterAccountError(error);
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x00041B0A File Offset: 0x0003FD0A
		public void LoginController_OnRegisterFailureEmailInUse()
		{
			this.emailInUsePanel.gameObject.SetActive(true);
			this.registerPanel.gameObject.SetActive(false);
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x000415B1 File Offset: 0x0003F7B1
		public void LoginController_OnMigrationSuccess(RegisterResponse result)
		{
			this.migrationPanel.RegisterMigrationAccountSuccess();
			this.migrationSuccessPanel.gameObject.SetActive(true);
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x00041B2E File Offset: 0x0003FD2E
		private void LoginController_OnDeleteAccountSuccess()
		{
			this.editProfilePanel.gameObject.SetActive(false);
			this.deleteAccountPanel.gameObject.SetActive(false);
			this.accountDeletedPanel.gameObject.SetActive(true);
			Singleton<LoginController>.Instance.Logout();
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x00041B6D File Offset: 0x0003FD6D
		private void LoginController_OnEmailChangeSuccess(ChangeEmailResponse result)
		{
			this.editProfilePanel.gameObject.SetActive(false);
			this.emailChangedPanel.gameObject.SetActive(true);
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x00041B91 File Offset: 0x0003FD91
		private void LoginController_OnMigrationFailure(FailureResponse error)
		{
			this.migrationPanel.RegisterMigrationAccountFailure(error.GetErrorsString());
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x00041BA4 File Offset: 0x0003FDA4
		private void LoginController_OnLoginAttempt()
		{
			this.connectingPanel.ShowCallApiWindow();
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x00041BA4 File Offset: 0x0003FDA4
		private void ShowAuthenticatingWindow()
		{
			this.connectingPanel.ShowCallApiWindow();
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x00041977 File Offset: 0x0003FB77
		private void HideAuthenticatingWindow()
		{
			this.connectingPanel.HideConnectionWindow();
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x00041BB1 File Offset: 0x0003FDB1
		private void LoginController_OnLoginSuccess(LoginResponse result)
		{
			this.connectingPanel.ShowConnectingWindow();
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x00041BBE File Offset: 0x0003FDBE
		private void LoginController_OnLoginFailure(FailureResponse result)
		{
			this.ShowLoginError(result.GetErrorsString());
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x00041BCC File Offset: 0x0003FDCC
		private void LoginController_OnLoginError(string result)
		{
			this.ShowLoginError(result);
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x00041BD5 File Offset: 0x0003FDD5
		private void LoginController_OnLogoutSuccess()
		{
			this.ShowDefaultMultiplayerMenu();
			Singleton<LoginController>.Instance.OnLoginSuccess += this.EnterLoginSettings;
			Singleton<LoginController>.Instance.IsPlayerLoggedOut = true;
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x00041BFE File Offset: 0x0003FDFE
		private void LoginController_OnMigrationError(string error)
		{
			this.migrationPanel.RegisterMigrationAccountError(error);
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x00041C0C File Offset: 0x0003FE0C
		private void LoginController_OnAuthenticationCompleted(LoginResponse result)
		{
			Singleton<LoginController>.Instance.IsPlayerLoggedOut = false;
			if (this.LoginMenuParent != null && this.LoginMenuParent == base.gameObject)
			{
				SceneController.Instance.LoadScene(SceneController.SCENE_LOBBY_NAME);
			}
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x00041C49 File Offset: 0x0003FE49
		private void LoginController_OnMigrationNeeded()
		{
			this.loginSettings.SetActive(false);
			this.migrationPanel.gameObject.SetActive(true);
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x00041C68 File Offset: 0x0003FE68
		private void LoginController_OnActivationNeeded()
		{
			this.inactiveAccountPanel.gameObject.SetActive(true);
		}

		// Token: 0x04001C76 RID: 7286
		public static bool OpenExtrasOnLoad = false;

		// Token: 0x04001C77 RID: 7287
		public static MainMenu.State loadState = MainMenu.State.None;

		// Token: 0x04001C78 RID: 7288
		[Header("Login Fields")]
		public Text loginErrorText;

		// Token: 0x04001C79 RID: 7289
		public InputField playerName;

		// Token: 0x04001C7A RID: 7290
		public InputField password;

		// Token: 0x04001C7B RID: 7291
		public Button loginButton;

		// Token: 0x04001C7C RID: 7292
		public Button newAccountButton;

		// Token: 0x04001C7D RID: 7293
		[Header("Mobile login fields")]
		public TextMeshProUGUI loginErrorTextTMP;

		// Token: 0x04001C7E RID: 7294
		public TMP_InputField playerNameTMP;

		// Token: 0x04001C7F RID: 7295
		public TMP_InputField passwordTMP;

		// Token: 0x04001C80 RID: 7296
		[SerializeField]
		private Button mobileLoginResetButton;

		// Token: 0x04001C81 RID: 7297
		[SerializeField]
		private Button mobileLoginBackButton;

		// Token: 0x04001C82 RID: 7298
		[Header("Reset password Fields")]
		public Button resetPasswordMenuButton;

		// Token: 0x04001C83 RID: 7299
		[Header("Others")]
		public PlayerMatSelectionWindow playerMatSelectionWindow;

		// Token: 0x04001C84 RID: 7300
		public FactionSelectionWindow factionMatSelectionWindow;

		// Token: 0x04001C85 RID: 7301
		public GameObject logoContainer;

		// Token: 0x04001C86 RID: 7302
		public GameObject defaultMenu;

		// Token: 0x04001C87 RID: 7303
		public GameObject optionButtonMobile;

		// Token: 0x04001C88 RID: 7304
		public GameObject playLocal;

		// Token: 0x04001C89 RID: 7305
		public GameObject hotseatMenu;

		// Token: 0x04001C8A RID: 7306
		public GameObject loadSaveWindow;

		// Token: 0x04001C8B RID: 7307
		public GameObject multiplayerMenu;

		// Token: 0x04001C8C RID: 7308
		public GameObject campaignMenu;

		// Token: 0x04001C8D RID: 7309
		public GameObject album;

		// Token: 0x04001C8E RID: 7310
		public GameObject accountAlreadyLinked;

		// Token: 0x04001C8F RID: 7311
		public GameObject optionsMenu;

		// Token: 0x04001C90 RID: 7312
		public GameObject loginSettings;

		// Token: 0x04001C91 RID: 7313
		public PlayerStatsPresenter statsPresenter;

		// Token: 0x04001C92 RID: 7314
		public GameObject extrasMenu;

		// Token: 0x04001C93 RID: 7315
		public Button resumeGameButton;

		// Token: 0x04001C94 RID: 7316
		public Button multiplayerButton;

		// Token: 0x04001C95 RID: 7317
		public Button backButton;

		// Token: 0x04001C96 RID: 7318
		public Button extrasButton;

		// Token: 0x04001C97 RID: 7319
		public ConnectionPanel connectingPanel;

		// Token: 0x04001C98 RID: 7320
		public GameObject errorPanel;

		// Token: 0x04001C99 RID: 7321
		[SerializeField]
		private Text errorText;

		// Token: 0x04001C9A RID: 7322
		[SerializeField]
		private TextMeshProUGUI errorTextTMP;

		// Token: 0x04001C9B RID: 7323
		[SerializeField]
		private GameObject invadersFromAfarBanner;

		// Token: 0x04001C9C RID: 7324
		[SerializeField]
		private GameObject exitGameDialog;

		// Token: 0x04001C9D RID: 7325
		[SerializeField]
		private GameObject[] menuButtons;

		// Token: 0x04001C9E RID: 7326
		[SerializeField]
		[Tooltip("Duplicates reference from menuButtons, but this button is disabled on iOS / tvOS")]
		private GameObject menuQuitButton;

		// Token: 0x04001C9F RID: 7327
		[SerializeField]
		[Tooltip("Dummy replacement for resume button")]
		private GameObject resumeDummy;

		// Token: 0x04001CA0 RID: 7328
		[SerializeField]
		private GameObject gameCenterLogOutPopup;

		// Token: 0x04001CA1 RID: 7329
		[SerializeField]
		private TextMeshProUGUI logOutPopupText;

		// Token: 0x04001CA2 RID: 7330
		[SerializeField]
		private AccountMigrationPanel migrationPanel;

		// Token: 0x04001CA3 RID: 7331
		[SerializeField]
		private AccountCreatedSuccess migrationSuccessPanel;

		// Token: 0x04001CA4 RID: 7332
		[SerializeField]
		private ChangePasswordPanel changePasswordPanel;

		// Token: 0x04001CA5 RID: 7333
		[SerializeField]
		private ChangePasswordSuccess changePasswordSuccessPanel;

		// Token: 0x04001CA6 RID: 7334
		[SerializeField]
		private AccountDeletedPanel accountDeletedPanel;

		// Token: 0x04001CA7 RID: 7335
		[SerializeField]
		private EmailChangedPanel emailChangedPanel;

		// Token: 0x04001CA8 RID: 7336
		[SerializeField]
		private DeleteAccountPanel deleteAccountPanel;

		// Token: 0x04001CA9 RID: 7337
		[SerializeField]
		private EditProfilePanel editProfilePanel;

		// Token: 0x04001CAA RID: 7338
		[SerializeField]
		private ResetPasswordPanel resetPasswordPanel;

		// Token: 0x04001CAB RID: 7339
		[SerializeField]
		private ResetPasswordSuccess resetPasswordSuccess;

		// Token: 0x04001CAC RID: 7340
		[SerializeField]
		private RegisterPanel registerPanel;

		// Token: 0x04001CAD RID: 7341
		[SerializeField]
		private AccountCreatedSuccess accountCreatedSuccess;

		// Token: 0x04001CAE RID: 7342
		[SerializeField]
		private AccountCreatedSuccess inactiveAccountPanel;

		// Token: 0x04001CAF RID: 7343
		[SerializeField]
		private SupportPanel supportPanel;

		// Token: 0x04001CB0 RID: 7344
		[SerializeField]
		private SupportMessageSent supportMessageSent;

		// Token: 0x04001CB1 RID: 7345
		[SerializeField]
		private EmailInUsePanel emailInUsePanel;

		// Token: 0x04001CB3 RID: 7347
		private const string PLAYER_NAME_KEY = "PlayerName";

		// Token: 0x04001CB4 RID: 7348
		private static string loadGameFile = string.Empty;

		// Token: 0x04001CB5 RID: 7349
		private const string PRIVACY_POLICY_URL = "https://www.theknightsofunity.com/privacypolicy";

		// Token: 0x04001CB6 RID: 7350
		private const string GENERIC_ERROR = "ErrorMessages/GenericError";

		// Token: 0x04001CB7 RID: 7351
		private const string NOT_INSTALLED_GOOGLE_PLAY = "MainMenu/NotInstalledGooglePlayOnline";

		// Token: 0x04001CB8 RID: 7352
		private const string ERROR_12501 = "12501";

		// Token: 0x04001CB9 RID: 7353
		private const string GAMECENTER_NO_USER = "MainMenu/GameCenterNoUser";

		// Token: 0x04001CBA RID: 7354
		private const string GAMECENTER_PROBLEM = "Game Center Problem";

		// Token: 0x04001CBB RID: 7355
		private const string LOGIN_ERROR = "MainMenu/LoginError";

		// Token: 0x04001CBC RID: 7356
		private const string INVALID_USERNAME = "invalid username";

		// Token: 0x04001CBD RID: 7357
		private const string NO_LINKED_ACCOUNT = "MainMenu/NoLinkedAccount";

		// Token: 0x04001CBE RID: 7358
		private const string NO_INTERNET_CONNECTION = "MainMenu/NoInternetConnection";

		// Token: 0x04001CBF RID: 7359
		private const string INVALID_GRANT = "invalid_grant";

		// Token: 0x04001CC0 RID: 7360
		private const string STEAM_ERROR = "MainMenu/SteamError";

		// Token: 0x04001CC1 RID: 7361
		private const string MAINTENANCE = "maintenance";

		// Token: 0x04001CC2 RID: 7362
		private const string SERVER_MAINTENANCE = "MainMenu/ServerMaintenance";

		// Token: 0x04001CC3 RID: 7363
		private const string IFA_STANDALONE_SHOP_URL = "https://store.steampowered.com/app/1021190";

		// Token: 0x020004E0 RID: 1248
		public enum State
		{
			// Token: 0x04001CC5 RID: 7365
			None,
			// Token: 0x04001CC6 RID: 7366
			Campaign
		}
	}
}

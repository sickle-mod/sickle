using System;
using I2.Loc;
using Scythe.Multiplayer.Data;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VoxelBusters.EssentialKit;

namespace Scythe.UI
{
	// Token: 0x020004E9 RID: 1257
	public class RateGamePanel : SingletonMono<RateGamePanel>
	{
		// Token: 0x06002851 RID: 10321 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void Start()
		{
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x00042111 File Offset: 0x00040311
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (!this.HasPlayerRatedGame() && this.TimerAllowShowRateGame() && this.GetLastSceneName() == SceneController.SCENE_MAIN_NAME && this.IsLastGameWon())
			{
				this.ShowRateGamePanelIfReviewDoesntExistOnServer();
			}
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x00042143 File Offset: 0x00040343
		private void OnDisable()
		{
			this.RemoveButtonsListeneres();
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x0004214B File Offset: 0x0004034B
		private void RemoveButtonsListeneres()
		{
			this.rateButton.onClick.RemoveAllListeners();
			this.mailTextInputField.onValueChanged.RemoveAllListeners();
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x000EA6A0 File Offset: 0x000E88A0
		public void RateButtonClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			if (this.starsAmount > 3)
			{
				this.GameRatedSave();
				this.ShowNativeRateApp();
				this.SendReview(this.starsAmount);
			}
			else
			{
				this.ShowFeedbackPanel();
			}
			this.rateButton.interactable = false;
			this.rateButton.onClick.RemoveAllListeners();
			this.rateButton.onClick.AddListener(new UnityAction(this.SendButtonClicked));
			this.mailTextInputField.onValueChanged.AddListener(delegate
			{
				this.ActivateSendButton();
			});
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x000EA738 File Offset: 0x000E8938
		private void ShowFeedbackPanel()
		{
			this.buttonTitle.text = ScriptLocalization.Get("MainMenu/Send");
			this.panelTitle.text = ScriptLocalization.Get("MainMenu/HelpUs");
			this.starsPanel.SetActive(false);
			this.feedbackPanel.SetActive(true);
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x0004216D File Offset: 0x0004036D
		private void ShowNativeRateApp()
		{
			this.DisplayConditionsReset();
			if (RateMyApp.IsAllowedToRate())
			{
				RateMyApp.AskForReviewNow(false);
			}
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x00042182 File Offset: 0x00040382
		private void GameRatedSave()
		{
			PlayerPrefs.SetInt(RateGamePanel.PREFS_PLAYER_RATED_GAME, RateGamePanel.BoolToInt(true));
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x00042194 File Offset: 0x00040394
		private void SendReview(int starsAmount)
		{
			RateGameServerAPI.SendReview(starsAmount, new Action<string>(this.RateGameServerAPI_SendReview_OnSuccess), new Action<Exception>(this.RateGameServerAPI_SendReview_OnError));
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x000421B4 File Offset: 0x000403B4
		private void SendReview(int starsAmount, string feedback)
		{
			RateGameServerAPI.SendReview(starsAmount, feedback, new Action<string>(this.RateGameServerAPI_SendReview_OnSuccess), new Action<Exception>(this.RateGameServerAPI_SendReview_OnError));
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void RateGameServerAPI_SendReview_OnSuccess(string response)
		{
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x0003059A File Offset: 0x0002E79A
		private void RateGameServerAPI_SendReview_OnError(Exception exception)
		{
			Debug.LogError(exception);
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x000EA788 File Offset: 0x000E8988
		private void RatePanelEnable()
		{
			this.buttonTitle.text = ScriptLocalization.Get("MainMenu/Rate");
			this.panelTitle.text = ScriptLocalization.Get("MainMenu/LeaveFeedback");
			this.rateButton.interactable = false;
			this.rateButton.onClick.AddListener(new UnityAction(this.RateButtonClicked));
			this.rateGamePanel.SetActive(true);
			this.starsPanel.SetActive(true);
			this.feedbackPanel.SetActive(false);
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x000421D5 File Offset: 0x000403D5
		private void ActivateSendButton()
		{
			if (this.mailTextInputField.text.Length > 4)
			{
				this.rateButton.interactable = true;
			}
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x000EA80C File Offset: 0x000E8A0C
		public void SendButtonClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			this.rateGamePanel.SetActive(false);
			EmailFactory.SendMailGmail(this.mailTextInputField.text);
			this.GameRatedSave();
			this.SendReview(this.starsAmount, this.mailTextInputField.text);
			this.RemoveButtonsListeneres();
			this.DisplayConditionsReset();
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x000EA86C File Offset: 0x000E8A6C
		public void ChooseStarsAmount(int clickedStarsAmount)
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			this.starsAmount = clickedStarsAmount;
			for (int i = 0; i < this.stars.Length; i++)
			{
				if (i <= clickedStarsAmount)
				{
					this.stars[i].sprite = this.starImage;
				}
				else
				{
					this.stars[i].sprite = this.starShadow;
				}
			}
			this.rateButton.interactable = true;
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000421F6 File Offset: 0x000403F6
		public void ExitButton()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.ExitButton);
			this.rateGamePanel.SetActive(false);
			this.DisplayConditionsReset();
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x00029B95 File Offset: 0x00027D95
		public static int BoolToInt(bool val)
		{
			if (!val)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x00029B9D File Offset: 0x00027D9D
		public static bool IntToBool(int val)
		{
			return val != 0;
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x00042216 File Offset: 0x00040416
		private void DisplayConditionsReset()
		{
			RateGamePanel.SaveLastGameWon(false);
			CountTimeInGame.ResetGameTime();
			PlayerPrefs.SetInt(RateGamePanel.PREFS_FIRST_PANEL_SHOW, RateGamePanel.BoolToInt(false));
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x00042233 File Offset: 0x00040433
		public static void SaveLastGameWon(bool state)
		{
			PlayerPrefs.SetInt(RateGamePanel.PREFS_LAST_GAME_WON, RateGamePanel.BoolToInt(state));
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x00042245 File Offset: 0x00040445
		private bool IsLastGameWon()
		{
			return RateGamePanel.IntToBool(PlayerPrefs.GetInt(RateGamePanel.PREFS_LAST_GAME_WON, 0));
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x00042257 File Offset: 0x00040457
		private string GetLastSceneName()
		{
			return PlayerPrefs.GetString(SceneController.PREFS_LAST_SCENE_NAME);
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x00042263 File Offset: 0x00040463
		private bool HasPlayerRatedGame()
		{
			return RateGamePanel.IntToBool(PlayerPrefs.GetInt(RateGamePanel.PREFS_PLAYER_RATED_GAME));
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x00042274 File Offset: 0x00040474
		private bool TimerAllowShowRateGame()
		{
			if (this.IsFirstRatePanelShow())
			{
				return CountTimeInGame.minutes > 20;
			}
			return CountTimeInGame.minutes > 120;
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x00042291 File Offset: 0x00040491
		private bool IsFirstRatePanelShow()
		{
			return RateGamePanel.IntToBool(PlayerPrefs.GetInt(RateGamePanel.PREFS_FIRST_PANEL_SHOW, 1));
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x000422A3 File Offset: 0x000404A3
		private void ShowRateGamePanelIfReviewDoesntExistOnServer()
		{
			if (GameServiceController.Instance.IsPlayerSignedIn())
			{
				RateGameServerAPI.GetReview(new Action<Review>(this.RateGameServerAPI_GetReview_OnSuccess), new Action<Exception>(this.RateGameServerAPI_GetReview_OnError));
			}
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x000422CE File Offset: 0x000404CE
		private void RateGameServerAPI_GetReview_OnSuccess(Review review)
		{
			if (review == null)
			{
				this.RatePanelEnable();
				return;
			}
			PlayerPrefs.SetInt(RateGamePanel.PREFS_PLAYER_RATED_GAME, RateGamePanel.BoolToInt(true));
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x000422EA File Offset: 0x000404EA
		private void RateGameServerAPI_GetReview_OnError(Exception exception)
		{
			Debug.LogError(exception);
			this.RatePanelEnable();
		}

		// Token: 0x04001CE9 RID: 7401
		public static string PREFS_PLAYER_RATED_GAME = "PlayerRatedGame";

		// Token: 0x04001CEA RID: 7402
		public static string PREFS_FIRST_PANEL_SHOW = "FirstRatePanelShow";

		// Token: 0x04001CEB RID: 7403
		public static string PREFS_LAST_GAME_WON = "LastGameWon";

		// Token: 0x04001CEC RID: 7404
		[SerializeField]
		private TextMeshProUGUI panelTitle;

		// Token: 0x04001CED RID: 7405
		[SerializeField]
		private TextMeshProUGUI buttonTitle;

		// Token: 0x04001CEE RID: 7406
		[SerializeField]
		private GameObject starsPanel;

		// Token: 0x04001CEF RID: 7407
		[SerializeField]
		private GameObject feedbackPanel;

		// Token: 0x04001CF0 RID: 7408
		[SerializeField]
		private GameObject rateGamePanel;

		// Token: 0x04001CF1 RID: 7409
		[SerializeField]
		private TMP_InputField mailTextInputField;

		// Token: 0x04001CF2 RID: 7410
		[SerializeField]
		private Image[] stars;

		// Token: 0x04001CF3 RID: 7411
		[SerializeField]
		private Sprite starImage;

		// Token: 0x04001CF4 RID: 7412
		[SerializeField]
		private Sprite starShadow;

		// Token: 0x04001CF5 RID: 7413
		[SerializeField]
		private Button rateButton;

		// Token: 0x04001CF6 RID: 7414
		private int starsAmount;
	}
}

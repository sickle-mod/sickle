using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003CB RID: 971
	public class LoadingScreenPresenterMobile : MonoBehaviour
	{
		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06001C67 RID: 7271 RVA: 0x0003A58F File Offset: 0x0003878F
		private bool ShouldTimerRun
		{
			get
			{
				return MultiplayerController.Instance.IsMultiplayer && !MultiplayerController.Instance.SpectatorMode && !MultiplayerController.Instance.Asynchronous;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06001C68 RID: 7272 RVA: 0x0003A969 File Offset: 0x00038B69
		public bool IsInTimeoutCountdown
		{
			get
			{
				return this.timeoutCountdownCR != null;
			}
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x0003A974 File Offset: 0x00038B74
		private void Awake()
		{
			if (this.LoadingProgressSlider == null)
			{
				Debug.LogError("Progress Slider is not attached!");
			}
			if (this.CurrentStateInfo == null)
			{
				Debug.LogError("Current State Info is not attached!");
			}
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000B17F8 File Offset: 0x000AF9F8
		public void DoTimeoutCountdown(int timeToWaitInSeconds)
		{
			LoadingScreenPresenterMobile.<>c__DisplayClass44_0 CS$<>8__locals1 = new LoadingScreenPresenterMobile.<>c__DisplayClass44_0();
			CS$<>8__locals1.timeToWaitInSeconds = timeToWaitInSeconds;
			CS$<>8__locals1.<>4__this = this;
			if (this.IsInTimeoutCountdown)
			{
				Debug.LogError("Tried to do timeout countdown when it is already in progress!");
				return;
			}
			if (!this.ShouldTimerRun)
			{
				Debug.LogError("Can't run the timeout timer. Condition is not met!");
				return;
			}
			this.playAndStayConnectingTimer.transform.parent.gameObject.SetActive(true);
			this.tipsObject.gameObject.SetActive(false);
			this.timeoutCountdownCR = base.StartCoroutine(CS$<>8__locals1.<DoTimeoutCountdown>g__DoTimeoutCountdownIE|0());
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000B1880 File Offset: 0x000AFA80
		public void StopCountdown()
		{
			if (!this.IsInTimeoutCountdown)
			{
				Debug.LogError("Tried to stop timeout countdown when it is not running!");
				return;
			}
			this.UpdateTextTime(0f);
			this.playAndStayConnectingTimer.transform.parent.gameObject.SetActive(false);
			this.tipsObject.gameObject.SetActive(true);
			base.StopCoroutine(this.timeoutCountdownCR);
			this.timeoutCountdownCR = null;
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000B18EC File Offset: 0x000AFAEC
		private void UpdateTextTime(float timeInSeconds)
		{
			int num = Mathf.FloorToInt(timeInSeconds / 60f);
			int num2 = Mathf.FloorToInt(timeInSeconds - (float)(num * 60));
			this.playAndStayConnectingTimer.text = string.Format("{0:0}:{1:00}", num, num2);
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x00029172 File Offset: 0x00027372
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x0003A9A6 File Offset: 0x00038BA6
		public Sprite GetBackground(int id)
		{
			return AssetBundleManager.LoadAssetBundle("graphic_backgrounds").LoadAsset<Sprite>(this.BackgroundNames[id]);
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x000B1934 File Offset: 0x000AFB34
		public void Init()
		{
			this.readyOnceFlag = false;
			this.ContentHolder.SetActive(true);
			this.ObjectivePanel.gameObject.SetActive(true);
			this.StartButton.SetActive(false);
			this.MultiplayerInfo.SetActive(false);
			this.LoadingPlayerList.gameObject.SetActive(false);
			if (SceneManager.GetActiveScene().name == SceneController.SCENE_MAIN_NAME)
			{
				this.CurrentStateInfo.text = LoadingScreenPresenterMobile.tipString;
			}
			else
			{
				this.UpdateHint();
			}
			if (GameController.GameManager.IsMultiplayer)
			{
				this.FillMultiplayerInfo(GameController.GameManager.PlayerOwner);
				this.MultiplayerInfo.SetActive(true);
				this.FillPlayerListMultiplayer();
				this.LoadingPlayerList.gameObject.SetActive(true);
				this.FillStructureBonusInfo();
				this.StructureBonusPanel.SetActive(true);
				this.SetBackground(new Faction?(GameController.GameManager.PlayerOwner.matFaction.faction));
			}
			else if (GameController.GameManager.IsHotSeat || GameController.GameManager.IsCampaign)
			{
				List<Player> list = new List<Player>(GameController.GameManager.GetPlayersWithoutAI());
				Player player = null;
				if (list.Count != 0)
				{
					player = list[0];
				}
				this.FillPlayerListHotseat();
				this.LoadingPlayerList.gameObject.SetActive(true);
				this.FillStructureBonusInfo();
				this.StructureBonusPanel.SetActive(true);
				if (list.Count == 1 && player != null && (GameController.GameManager.IsAIHotSeat || GameController.GameManager.IsCampaign))
				{
					this.FillMultiplayerInfo(player);
					this.MultiplayerInfo.SetActive(true);
					this.SetBackground(new Faction?(player.matFaction.faction));
				}
				else
				{
					this.SetBackground(null);
					this.ChooseResumeLayout();
				}
			}
			if (GameController.GameManager.IsCampaign)
			{
				this.MultiplayerInfo.SetActive(true);
				this.StructureBonusPanel.SetActive(false);
				this.ObjectivePanel.SetActive(false);
				this.FactionAbilityPanel.SetActive(false);
				this.LoadingPlayerList.gameObject.SetActive(false);
			}
			this.ShowPressAnyKeyText();
			if (SceneManager.GetActiveScene().name.Contains(SceneController.SCENE_MAIN_NAME))
			{
				AnalyticsEventLogger.Instance.LogMatchStart();
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
				GameController.Instance.CheckDLCSanity();
			}
			if (SceneController.Instance.GetSceneToLoad().Contains(SceneController.SCENE_MAIN_NAME) && (GameController.GameManager.GetPlayersWithoutAICount() == 1 || GameController.GameManager.IsMultiplayer))
			{
				Player player2 = null;
				if (GameController.GameManager.IsMultiplayer)
				{
					player2 = GameController.GameManager.PlayerOwner;
				}
				else if (GameController.GameManager.IsHotSeat && GameController.GameManager.GetPlayersWithoutAICount() == 1)
				{
					player2 = new List<Player>(GameController.GameManager.GetPlayersWithoutAI())[0];
				}
				if (player2 != null)
				{
					string text;
					if (player2.matPlayer.matType < PlayerMatType.Campaign00)
					{
						text = ScriptLocalization.Get("PlayerMat/" + player2.matPlayer.matType.ToString()).ToUpper();
					}
					else
					{
						text = ScriptLocalization.Get("MainMenu/Tutorial").ToUpper();
					}
					this.matFactionName.text = ScriptLocalization.Get("FactionMat/" + player2.matFaction.faction.ToString()).ToUpper() + " - " + text;
					this.startPower.text = player2.Power.ToString();
					this.startAmmo.text = player2.combatCards.Count.ToString();
					this.startCoins.text = player2.Coins.ToString();
					this.startPopularity.text = player2.Popularity.ToString();
					if (GameController.gameFromSave)
					{
						this.ChooseResumeLayout();
						return;
					}
					this.gameReady.SetActive(true);
				}
			}
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x000B1D34 File Offset: 0x000AFF34
		public void UpdateHint()
		{
			string text = "LoadingScreenTips/Tip" + global::UnityEngine.Random.Range(1, this.tipsCount + 1).ToString().PadLeft(3, '0');
			this.CurrentStateInfo.text = (LoadingScreenPresenterMobile.tipString = ScriptLocalization.Get(text));
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x000B1D84 File Offset: 0x000AFF84
		public void OnStartButton()
		{
			WorldSFXManager.PlaySound(SoundEnum.CommonBgGreenButton, AudioSourceType.Buttons);
			if (!this.readyOnceFlag)
			{
				this.readyOnceFlag = true;
				bool loadingSave = LoadingScreenPresenterMobile.LoadingSave;
				LoadingScreenPresenterMobile.LoadingSave = false;
				this.StopCountdown();
				GameController.Instance.OnLoadingScreenStartButtonClicked(loadingSave);
				base.gameObject.SetActive(false);
				SceneController.Instance.DisableLoadingScreenCanvas();
			}
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x000B1DDC File Offset: 0x000AFFDC
		private void SetBackground(Faction? faction)
		{
			if (!LoadingScreenPresenterMobile.loadingContinuation || SceneManager.GetActiveScene().buildIndex == 0)
			{
				LoadingScreenPresenterMobile.selectedBackground = global::UnityEngine.Random.Range(0, 3) + 7;
				if (faction != null)
				{
					LoadingScreenPresenterMobile.selectedBackground = (int)faction.Value;
				}
				this.Background.sprite = this.GetBackground(LoadingScreenPresenterMobile.selectedBackground);
				AspectRatioFitter component = this.Background.gameObject.GetComponent<AspectRatioFitter>();
				if (component != null && this.Background != null && this.Background.sprite != null)
				{
					component.aspectRatio = this.Background.sprite.rect.width / this.Background.sprite.rect.height;
				}
				LoadingScreenPresenterMobile.loadingContinuation = true;
				return;
			}
			this.Background.sprite = this.GetBackground(LoadingScreenPresenterMobile.selectedBackground);
			LoadingScreenPresenterMobile.loadingContinuation = false;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000B1ED0 File Offset: 0x000B00D0
		private void ShowPressAnyKeyText()
		{
			if (SceneManager.GetActiveScene().name == SceneController.SCENE_MAIN_NAME)
			{
				this.LoadingProgressSlider.gameObject.SetActive(false);
				this.StartButton.SetActive(true);
			}
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x000B1F14 File Offset: 0x000B0114
		private void FillStructureBonusInfo()
		{
			this.StructureBonusTitle.text = GameController.GetStructureBonusName(GameController.GameManager.StructureBonus.CardId).ToUpper();
			this.StructureBonusDescription.text = GameController.GetStructureBonusDescription(GameController.GameManager.StructureBonus.CardId).Replace("|", " ");
			this.StructureBonusIcon.sprite = this.StructureBonusSymbols[GameController.GameManager.StructureBonus.CardId - 1];
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x000B1F98 File Offset: 0x000B0198
		private void FillMultiplayerInfo(Player owner)
		{
			if (owner != null)
			{
				Faction faction = owner.matFaction.faction;
				this.FactionEmblem.sprite = this.FactionLogos[(int)faction];
				string text = ScriptLocalization.Get(string.Format("FactionMat/{0}", faction)).ToUpper();
				string text2 = ScriptLocalization.Get(string.Format("PlayerMat/{0}", owner.matPlayer.matType)).ToUpper();
				this.FactionName.text = (string.IsNullOrEmpty(text2) ? text : (text + " - " + text2));
				if (faction == Faction.Polania && GameController.GameManager.PlayerCount >= 6)
				{
					this.FactionAbilityDescription.text = ScriptLocalization.Get("FactionMat/" + faction.ToString() + "FactionAbilityName").ToUpper() + Environment.NewLine + ScriptLocalization.Get("FactionMat/" + faction.ToString() + "FactionAbilityDescriptionA").Replace("|", " ");
				}
				else
				{
					this.FactionAbilityDescription.text = ScriptLocalization.Get("FactionMat/" + faction.ToString() + "FactionAbilityName").ToUpper() + Environment.NewLine + ScriptLocalization.Get("FactionMat/" + faction.ToString() + "FactionAbilityDescription").Replace("|", " ");
				}
				GameController.GameManager.factionBasicInfo = GameController.CreateArrayOfInfo();
				if (!GameController.GameManager.SpectatorMode && owner.objectiveCards.Count > 1)
				{
					this.ObjectiveTitles[0].text = GameController.GetObjectiveTitle(owner.objectiveCards[0].CardId).ToUpper();
					this.ObjectiveDescriptions[0].text = GameController.GetObjectiveDescription(owner.objectiveCards[0].CardId).Replace("|", " ");
					this.ObjectiveTitles[1].text = GameController.GetObjectiveTitle(owner.objectiveCards[1].CardId).ToUpper();
					this.ObjectiveDescriptions[1].text = GameController.GetObjectiveDescription(owner.objectiveCards[1].CardId).Replace("|", " ");
					return;
				}
				this.ObjectivePanel.SetActive(false);
			}
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x000B2204 File Offset: 0x000B0404
		private void FillPlayerListHotseat()
		{
			List<Player> players = GameController.GameManager.GetPlayers();
			for (int i = 0; i < GameController.GameManager.PlayerCount; i++)
			{
				bool isHuman = players[i].IsHuman;
				this.LoadingPlayerList.FillPlayerInfoList(players, this.FactionLogos);
			}
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x000B2250 File Offset: 0x000B0450
		private void FillPlayerListMultiplayer()
		{
			List<PlayerData> list = new List<PlayerData>(MultiplayerController.Instance.GetPlayersInGame());
			for (int i = 0; i < GameController.GameManager.PlayerCount; i++)
			{
				this.LoadingPlayerList.FillPlayerInfoList(list, this.FactionLogos);
			}
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x0003A9BF File Offset: 0x00038BBF
		private IEnumerator MapLoadedCountdown(int seconds = 180)
		{
			yield return new WaitForSecondsRealtime((float)seconds);
			this.DisconnectBrokenPlayer();
			yield break;
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x0003A615 File Offset: 0x00038815
		private void DisconnectBrokenPlayer()
		{
			if (!PlayerInfo.me.MapLoaded)
			{
				MultiplayerController.Instance.LeaveTimeoutedSyncGame();
				SceneController.Instance.LoadScene(SceneController.SCENE_LOBBY_NAME);
			}
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000B2294 File Offset: 0x000B0494
		public void SetLoadingAsyncOperation(AsyncOperation async)
		{
			base.gameObject.SetActive(true);
			this.StartButton.SetActive(false);
			this.LoadingProgressSlider.gameObject.SetActive(true);
			LoadingScreenPresenterMobile.currentDate = DateTime.UtcNow;
			this.DoTimeoutCountdown(180);
			this.Init();
			base.StartCoroutine(this.LoadMainScene(async));
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x0003A9D5 File Offset: 0x00038BD5
		private IEnumerator LoadMainScene(AsyncOperation async)
		{
			yield return new WaitForSeconds(0.2f);
			int counter = 0;
			while (!async.isDone)
			{
				this.LoadingProgressSlider.value = async.progress;
				int num = counter;
				counter = num + 1;
				if (counter == 100)
				{
					counter = 0;
					this.UpdateHint();
				}
				yield return new WaitForSeconds(0.1f);
			}
			this.Init();
			yield return null;
			yield break;
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x0003A9EB File Offset: 0x00038BEB
		private void ChooseResumeLayout()
		{
			this.ObjectivePanel.gameObject.SetActive(false);
			this.MultiplayerInfo.gameObject.SetActive(false);
		}

		// Token: 0x0400146C RID: 5228
		public static DateTime currentDate;

		// Token: 0x0400146D RID: 5229
		public static float loadTimeoutForDateTimer = 180f;

		// Token: 0x0400146E RID: 5230
		public static string tipString;

		// Token: 0x0400146F RID: 5231
		public static bool LoadingSave = false;

		// Token: 0x04001470 RID: 5232
		public Image Background;

		// Token: 0x04001471 RID: 5233
		public GameObject ContentHolder;

		// Token: 0x04001472 RID: 5234
		public GameObject MultiplayerInfo;

		// Token: 0x04001473 RID: 5235
		public LoadingPlayerList LoadingPlayerList;

		// Token: 0x04001474 RID: 5236
		public Sprite[] FactionLogos;

		// Token: 0x04001475 RID: 5237
		public Image StructureBonusIcon;

		// Token: 0x04001476 RID: 5238
		public Sprite[] StructureBonusSymbols;

		// Token: 0x04001477 RID: 5239
		public Image FactionEmblem;

		// Token: 0x04001478 RID: 5240
		public TMP_Text FactionName;

		// Token: 0x04001479 RID: 5241
		public GameObject StructureBonusPanel;

		// Token: 0x0400147A RID: 5242
		public GameObject ObjectivePanel;

		// Token: 0x0400147B RID: 5243
		public GameObject FactionAbilityPanel;

		// Token: 0x0400147C RID: 5244
		public TMP_Text FactionAbilityDescription;

		// Token: 0x0400147D RID: 5245
		public TMP_Text StructureBonusTitle;

		// Token: 0x0400147E RID: 5246
		public TMP_Text StructureBonusDescription;

		// Token: 0x0400147F RID: 5247
		public TMP_Text[] ObjectiveTitles;

		// Token: 0x04001480 RID: 5248
		public TMP_Text[] ObjectiveDescriptions;

		// Token: 0x04001481 RID: 5249
		public Slider LoadingProgressSlider;

		// Token: 0x04001482 RID: 5250
		public GameObject StartButton;

		// Token: 0x04001483 RID: 5251
		public TMP_Text CurrentStateInfo;

		// Token: 0x04001484 RID: 5252
		public GameObject gameReady;

		// Token: 0x04001485 RID: 5253
		public TMP_Text matFactionName;

		// Token: 0x04001486 RID: 5254
		public TMP_Text startCoins;

		// Token: 0x04001487 RID: 5255
		public TMP_Text startPopularity;

		// Token: 0x04001488 RID: 5256
		public TMP_Text startPower;

		// Token: 0x04001489 RID: 5257
		public TMP_Text startAmmo;

		// Token: 0x0400148A RID: 5258
		public int tipsCount = 70;

		// Token: 0x0400148B RID: 5259
		public TMP_Text playAndStayConnectingTimer;

		// Token: 0x0400148C RID: 5260
		public Transform tipsObject;

		// Token: 0x0400148D RID: 5261
		private static int selectedBackground = 0;

		// Token: 0x0400148E RID: 5262
		private static bool loadingContinuation = false;

		// Token: 0x0400148F RID: 5263
		private const int loadTimeoutInSeconds = 180;

		// Token: 0x04001490 RID: 5264
		private bool readyOnceFlag;

		// Token: 0x04001491 RID: 5265
		private Coroutine timeoutCountdownCR;

		// Token: 0x04001492 RID: 5266
		private string[] BackgroundNames = new string[] { "polania_loading", "albion_loading", "nordic_loading", "rusviet_loading", "togawa_loading", "crimea_loading", "saxony_loading", "Background_LoadingScreen_01", "Background_LoadingScreen_02", "Background_LoadingScreen_03" };
	}
}

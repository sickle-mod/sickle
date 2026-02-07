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
	// Token: 0x020003C2 RID: 962
	public class LoadingScreenPresenter : MonoBehaviour
	{
		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06001C0E RID: 7182 RVA: 0x0003A58F File Offset: 0x0003878F
		private bool ShouldTimerRun
		{
			get
			{
				return MultiplayerController.Instance.IsMultiplayer && !MultiplayerController.Instance.SpectatorMode && !MultiplayerController.Instance.Asynchronous;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06001C0F RID: 7183 RVA: 0x0003A5B8 File Offset: 0x000387B8
		public bool IsInTimeoutCountdown
		{
			get
			{
				return this.timeoutCountdownCR != null;
			}
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x0003A5C3 File Offset: 0x000387C3
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

		// Token: 0x06001C11 RID: 7185 RVA: 0x000AFCB4 File Offset: 0x000ADEB4
		public void DoTimeoutCountdown(int timeToWaitInSeconds)
		{
			LoadingScreenPresenter.<>c__DisplayClass41_0 CS$<>8__locals1 = new LoadingScreenPresenter.<>c__DisplayClass41_0();
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
			this.timeoutCountdownCR = base.StartCoroutine(CS$<>8__locals1.<DoTimeoutCountdown>g__DoTimeoutCountdownIE|0());
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x000AFD28 File Offset: 0x000ADF28
		public void StopCountdown()
		{
			if (!this.IsInTimeoutCountdown)
			{
				Debug.LogError("Tried to stop timeout countdown when it is not running!");
				return;
			}
			this.UpdateTextTime(0f);
			this.playAndStayConnectingTimer.transform.parent.gameObject.SetActive(false);
			base.StopCoroutine(this.timeoutCountdownCR);
			this.timeoutCountdownCR = null;
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x000AFD84 File Offset: 0x000ADF84
		private void UpdateTextTime(float timeInSeconds)
		{
			int num = Mathf.FloorToInt(timeInSeconds / 60f);
			int num2 = Mathf.FloorToInt(timeInSeconds - (float)(num * 60));
			this.playAndStayConnectingTimer.text = string.Format("{0:0}:{1:00}", num, num2);
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x0003A5F5 File Offset: 0x000387F5
		private void OnEnable()
		{
			LoadingScreenPresenter.currentDate = DateTime.UtcNow;
			this.DoTimeoutCountdown(180);
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x0003A60C File Offset: 0x0003880C
		private void Start()
		{
			this.Init(false);
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x0003A615 File Offset: 0x00038815
		private void DisconnectBrokenPlayer()
		{
			if (!PlayerInfo.me.MapLoaded)
			{
				MultiplayerController.Instance.LeaveTimeoutedSyncGame();
				SceneController.Instance.LoadScene(SceneController.SCENE_LOBBY_NAME);
			}
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x00029172 File Offset: 0x00027372
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x0003A63C File Offset: 0x0003883C
		public Sprite GetBackground(int id)
		{
			return AssetBundleManager.LoadAssetBundle("graphic_backgrounds").LoadAsset<Sprite>(this.BackgroundNames[id]);
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x000AFDCC File Offset: 0x000ADFCC
		public void Init(bool tutorialNextSceneInit = false)
		{
			this.readyOnceFlag = false;
			this.StartButton.SetActive(false);
			this.LoadingProgressSlider.gameObject.SetActive(true);
			this.MultiplayerInfo.SetActive(false);
			this.HotSeatInfo.SetActive(false);
			if (SceneManager.GetActiveScene().name == SceneController.SCENE_MAIN_NAME && !tutorialNextSceneInit)
			{
				this.CurrentStateInfo.text = LoadingScreenPresenter.tipString;
			}
			else
			{
				base.gameObject.SetActive(true);
				this.UpdateHint();
			}
			if (GameController.GameManager.IsMultiplayer)
			{
				this.FillMultiplayerInfo(GameController.GameManager.PlayerOwner);
				this.MultiplayerInfo.SetActive(true);
				this.FillPlayerListMultiplayer();
				this.HotSeatInfo.SetActive(true);
				this.FillStructureBonusInfo();
				this.StructureBonusPanel.SetActive(true);
				this.SetBackground(new Faction?(GameController.GameManager.PlayerOwner.matFaction.faction));
			}
			else if (GameController.GameManager.IsHotSeat || GameController.GameManager.IsCampaign)
			{
				List<Player> list = new List<Player>(GameController.GameManager.GetPlayersWithoutAI());
				Player player = null;
				using (List<Player>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						player = enumerator.Current;
					}
				}
				this.FillPlayerListHotseat();
				this.HotSeatInfo.SetActive(true);
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
				}
			}
			if (GameController.GameManager.IsCampaign)
			{
				this.MultiplayerInfo.SetActive(true);
				this.StructureBonusPanel.SetActive(false);
				this.ObjectivePanel.SetActive(false);
				this.FactionAbilityPanel.SetActive(false);
				this.HotSeatInfo.SetActive(false);
			}
			this.ShowPressAnyKeyText();
			if (SceneManager.GetActiveScene().name.Contains("main"))
			{
				if (!tutorialNextSceneInit)
				{
					AnalyticsEventLogger.Instance.LogMatchStart();
					AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
					GameController.Instance.CheckDLCSanity();
				}
				if (GameController.GameManager.GetPlayersWithoutAICount() == 1 || GameController.GameManager.IsMultiplayer)
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
						this.gameReady.SetActive(true);
						this.matFactionName.text = ScriptLocalization.Get("FactionMat/" + player2.matFaction.faction.ToString()).ToUpper();
						if (player2.matPlayer.matType < PlayerMatType.Campaign00)
						{
							this.matPlayerName.text = ScriptLocalization.Get("PlayerMat/" + player2.matPlayer.matType.ToString()).ToUpper();
						}
						else
						{
							this.matPlayerName.text = ScriptLocalization.Get("MainMenu/Tutorial").ToUpper();
						}
						this.startPower.text = player2.Power.ToString();
						this.startAmmo.text = player2.combatCards.Count.ToString();
						this.startCoins.text = player2.Coins.ToString();
						this.startPopularity.text = player2.Popularity.ToString();
						return;
					}
				}
			}
			else
			{
				this.gameReady.SetActive(false);
			}
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x000B01D4 File Offset: 0x000AE3D4
		public void UpdateHint()
		{
			string text = "LoadingScreenTips/Tip" + global::UnityEngine.Random.Range(1, this.tipsCount + 1).ToString().PadLeft(3, '0');
			this.CurrentStateInfo.text = (LoadingScreenPresenter.tipString = ScriptLocalization.Get(text));
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x000B0224 File Offset: 0x000AE424
		public void OnStartButton()
		{
			WorldSFXManager.PlaySound(SoundEnum.CommonBgGreenButton, AudioSourceType.Buttons);
			if (!this.readyOnceFlag)
			{
				this.readyOnceFlag = true;
				bool loadingSave = LoadingScreenPresenter.LoadingSave;
				LoadingScreenPresenter.LoadingSave = false;
				this.StopCountdown();
				GameController.Instance.OnLoadingScreenStartButtonClicked(loadingSave);
				base.gameObject.SetActive(false);
				SceneController.Instance.DisableLoadingScreenCanvas();
				if (PlatformManager.IsStandalone)
				{
					SceneController.Instance.DisableLoadingScreenPresenter();
					AssetBundleManager.UnloadAssetBundle("graphic_backgrounds", true);
				}
			}
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x000B0298 File Offset: 0x000AE498
		private void SetBackground(Faction? faction)
		{
			if (!LoadingScreenPresenter.loadingContinuation || SceneManager.GetActiveScene().buildIndex == 0)
			{
				LoadingScreenPresenter.selectedBackground = global::UnityEngine.Random.Range(0, 3) + 7;
				if (faction != null)
				{
					LoadingScreenPresenter.selectedBackground = (int)faction.Value;
				}
				this.Background.sprite = this.GetBackground(LoadingScreenPresenter.selectedBackground);
				LoadingScreenPresenter.loadingContinuation = true;
				return;
			}
			this.Background.sprite = this.GetBackground(LoadingScreenPresenter.selectedBackground);
			LoadingScreenPresenter.loadingContinuation = false;
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x000B0318 File Offset: 0x000AE518
		private void ShowPressAnyKeyText()
		{
			if (SceneManager.GetActiveScene().name == SceneController.SCENE_MAIN_NAME)
			{
				this.LoadingProgressSlider.gameObject.SetActive(false);
				this.StartButton.SetActive(true);
			}
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x000B035C File Offset: 0x000AE55C
		private void FillStructureBonusInfo()
		{
			this.StructureBonusDescription.text = GameController.GetStructureBonusName(GameController.GameManager.StructureBonus.CardId).ToUpper() + Environment.NewLine + GameController.GetStructureBonusDescription(GameController.GameManager.StructureBonus.CardId).Replace("|", " ");
			this.StructureBonusIcon.sprite = this.StructureBonusSymbols[GameController.GameManager.StructureBonus.CardId - 1];
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x000B03E0 File Offset: 0x000AE5E0
		private void FillMultiplayerInfo(Player owner)
		{
			if (owner != null)
			{
				Faction faction = owner.matFaction.faction;
				this.FactionEmblem.sprite = this.FactionLogos[(int)faction];
				this.FactionName.text = ScriptLocalization.Get("FactionMat/" + faction.ToString()).ToUpper();
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
					this.ObjectivesDescription.text = string.Format("{0}{1}\n\n{2}{3}", new object[]
					{
						GameController.GetObjectiveTitle(owner.objectiveCards[0].CardId).ToUpper() + Environment.NewLine,
						GameController.GetObjectiveDescription(owner.objectiveCards[0].CardId).Replace("|", " "),
						GameController.GetObjectiveTitle(owner.objectiveCards[1].CardId).ToUpper() + Environment.NewLine,
						GameController.GetObjectiveDescription(owner.objectiveCards[1].CardId).Replace("|", " ")
					});
					return;
				}
				this.ObjectivePanel.SetActive(false);
			}
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x000B0620 File Offset: 0x000AE820
		private void FillPlayerListHotseat()
		{
			List<Player> players = GameController.GameManager.GetPlayers();
			for (int i = 0; i < GameController.GameManager.PlayerCount; i++)
			{
				Transform child = this.HotSeatInfo.transform.GetChild(i + 1);
				child.GetChild(0).GetComponent<Text>().text = players[i].Name;
				child.GetChild(1).GetComponent<Image>().sprite = this.FactionLogos[(int)players[i].matFaction.faction];
				child.GetChild(2).GetComponent<Text>().text = ScriptLocalization.Get("FactionMat/" + players[i].matFaction.faction.ToString());
				child.gameObject.SetActive(true);
			}
			for (int j = GameController.GameManager.PlayerCount; j < 7; j++)
			{
				this.HotSeatInfo.transform.GetChild(j + 1).gameObject.SetActive(false);
			}
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x000B0728 File Offset: 0x000AE928
		private void FillPlayerListMultiplayer()
		{
			List<PlayerData> list = new List<PlayerData>(MultiplayerController.Instance.GetPlayersInGame());
			for (int i = 0; i < GameController.GameManager.PlayerCount; i++)
			{
				Transform child = this.HotSeatInfo.transform.GetChild(i + 1);
				child.GetChild(0).GetComponent<Text>().text = list[i].Name;
				child.GetChild(1).GetComponent<Image>().sprite = this.FactionLogos[list[i].Faction];
				child.GetChild(2).GetComponent<Text>().text = ScriptLocalization.Get("FactionMat/" + ((Faction)list[i].Faction).ToString());
				child.gameObject.SetActive(true);
			}
			for (int j = GameController.GameManager.PlayerCount; j < 7; j++)
			{
				this.HotSeatInfo.transform.GetChild(j + 1).gameObject.SetActive(false);
			}
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x000B082C File Offset: 0x000AEA2C
		public void SetLoadingAsyncOperation(AsyncOperation async)
		{
			base.gameObject.SetActive(true);
			this.StartButton.SetActive(false);
			this.LoadingProgressSlider.gameObject.SetActive(true);
			LoadingScreenPresenter.loadingContinuation = false;
			this.Init(false);
			base.StartCoroutine(this.LoadMainScene(async));
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x0003A655 File Offset: 0x00038855
		private IEnumerator LoadMainScene(AsyncOperation async)
		{
			yield return null;
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
			this.Init(false);
			yield return null;
			yield break;
		}

		// Token: 0x04001420 RID: 5152
		public Image Background;

		// Token: 0x04001421 RID: 5153
		private string[] BackgroundNames = new string[] { "Background_Mat_Faction_Polania", "Background_Mat_Faction_Albion", "Background_Mat_Faction_Nordic", "Background_Mat_Faction_Rusviet", "Background_Mat_Faction_Togawa", "Background_Mat_Faction_Crimea", "Background_Mat_Faction_Saxony", "Background_LoadingScreen_01", "Background_LoadingScreen_02", "Background_LoadingScreen_03" };

		// Token: 0x04001422 RID: 5154
		public GameObject MultiplayerInfo;

		// Token: 0x04001423 RID: 5155
		public GameObject HotSeatInfo;

		// Token: 0x04001424 RID: 5156
		public Sprite[] FactionLogos;

		// Token: 0x04001425 RID: 5157
		public Image StructureBonusIcon;

		// Token: 0x04001426 RID: 5158
		public Sprite[] StructureBonusSymbols;

		// Token: 0x04001427 RID: 5159
		public Image FactionEmblem;

		// Token: 0x04001428 RID: 5160
		public Text FactionName;

		// Token: 0x04001429 RID: 5161
		public GameObject StructureBonusPanel;

		// Token: 0x0400142A RID: 5162
		public GameObject ObjectivePanel;

		// Token: 0x0400142B RID: 5163
		public GameObject FactionAbilityPanel;

		// Token: 0x0400142C RID: 5164
		public Text FactionAbilityDescription;

		// Token: 0x0400142D RID: 5165
		public Text StructureBonusDescription;

		// Token: 0x0400142E RID: 5166
		public Text ObjectivesDescription;

		// Token: 0x0400142F RID: 5167
		public Slider LoadingProgressSlider;

		// Token: 0x04001430 RID: 5168
		public GameObject StartButton;

		// Token: 0x04001431 RID: 5169
		public Text CurrentStateInfo;

		// Token: 0x04001432 RID: 5170
		public GameObject gameReady;

		// Token: 0x04001433 RID: 5171
		public Text matFactionName;

		// Token: 0x04001434 RID: 5172
		public Text matPlayerName;

		// Token: 0x04001435 RID: 5173
		public Text startCoins;

		// Token: 0x04001436 RID: 5174
		public Text startPopularity;

		// Token: 0x04001437 RID: 5175
		public Text startPower;

		// Token: 0x04001438 RID: 5176
		public Text startAmmo;

		// Token: 0x04001439 RID: 5177
		public int tipsCount = 70;

		// Token: 0x0400143A RID: 5178
		public TMP_Text playAndStayConnectingTimer;

		// Token: 0x0400143B RID: 5179
		public static string tipString;

		// Token: 0x0400143C RID: 5180
		public static bool LoadingSave = false;

		// Token: 0x0400143D RID: 5181
		private const int loadTimeoutInSeconds = 180;

		// Token: 0x0400143E RID: 5182
		private bool readyOnceFlag;

		// Token: 0x0400143F RID: 5183
		private static int selectedBackground = 0;

		// Token: 0x04001440 RID: 5184
		private static bool loadingContinuation = false;

		// Token: 0x04001441 RID: 5185
		public static DateTime currentDate;

		// Token: 0x04001442 RID: 5186
		public static float loadTimeoutForDateTimer = 180f;

		// Token: 0x04001443 RID: 5187
		private Coroutine timeoutCountdownCR;
	}
}

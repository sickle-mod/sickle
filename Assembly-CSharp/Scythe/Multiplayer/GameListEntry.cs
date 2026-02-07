using System;
using System.Globalization;
using Assets.Scripts.Utilities;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000236 RID: 566
	public class GameListEntry : MonoBehaviour
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x00032F25 File Offset: 0x00031125
		// (set) Token: 0x060010E3 RID: 4323 RVA: 0x00032F2D File Offset: 0x0003112D
		public bool IsIFA { get; protected set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x00032F36 File Offset: 0x00031136
		// (set) Token: 0x060010E5 RID: 4325 RVA: 0x00032F3E File Offset: 0x0003113E
		public bool IsJoiningThisGameDisabledBecauseOfIFA { get; protected set; }

		// Token: 0x060010E6 RID: 4326 RVA: 0x0009116C File Offset: 0x0008F36C
		public void Init(LobbyRoom roomData, Lobby lobby)
		{
			this.ClearEntry();
			this.lobby = lobby;
			this.SetGameName(roomData.Name);
			this.SetGameType(roomData.IsAsynchronous);
			if (this.forfeitButton != null)
			{
				this.forfeitButton.gameObject.SetActive(false);
			}
			if (this.backgroundFrame != null)
			{
				this.backgroundFrame.gameObject.SetActive(false);
			}
			this.joinResumeButton.interactable = true;
			this.CheckRoomELO(roomData);
			this.CheckPlayers(roomData);
			this.AddGameTime(roomData);
			if (!roomData.IsRanked)
			{
				this.AddNewSetting(this.promoCardsImage, "Lobby/PromoCardsColon", roomData.PromoCardsEnabled ? ScriptLocalization.Get("Common/Yes") : ScriptLocalization.Get("Common/No"));
				if (!roomData.AllRandom)
				{
					this.AddNewSetting(this.matChoicesImage, "Lobby/MatsChoicesColon", ScriptLocalization.Get("Common/Yes"));
				}
			}
			if (roomData.InvadersFromAfar)
			{
				this.AddExpansionSign();
				this.IsIFA = true;
			}
			if (PlayerInfo.me.CurrentLobbyRoom != null && PlayerInfo.me.CurrentLobbyRoom.RoomId == roomData.RoomId)
			{
				this.SetButtonText("Common/Back");
				this.joinResumeButton.onClick.AddListener(delegate
				{
					this.ReturnToCurrentRoom();
				});
				this.joinResumeButton.interactable = true;
			}
			else
			{
				this.SetButtonText("Lobby/Join");
				this.joinResumeButton.onClick.AddListener(delegate
				{
					this.JoinRoom(roomData.RoomId);
				});
			}
			this.RefreshJoinResumeButtonActivity();
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0009133C File Offset: 0x0008F53C
		private void CheckRoomELO(LobbyRoom roomData)
		{
			if (roomData.IsRanked)
			{
				this.EnableRankedObject();
				if (roomData.MinELO > PlayerInfo.me.PlayerStats.ELO || roomData.MaxELO < PlayerInfo.me.PlayerStats.ELO)
				{
					this.AddUnavailableSetting(this.eloImage, "Lobby/ELOColon", string.Format("{0}-{1}", roomData.MinELO, roomData.MaxELO));
					this.joinResumeButton.interactable = false;
					return;
				}
				this.AddNewSetting(this.eloImage, "Lobby/ELOColon", string.Format("{0}-{1}", roomData.MinELO, roomData.MaxELO));
			}
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x000913F8 File Offset: 0x0008F5F8
		private void CheckPlayers(LobbyRoom roomData)
		{
			if (roomData.Players == roomData.MaxPlayers)
			{
				this.AddUnavailableSetting(this.playersImage, "Lobby/PlayersColon", string.Format("{0}/{1}", roomData.Players, roomData.MaxPlayers));
				this.joinResumeButton.interactable = false;
				return;
			}
			if (!GameServiceController.Instance.InvadersFromAfarUnlocked() && roomData.Players >= 5)
			{
				this.AddUnavailableSetting(this.playersImage, "Lobby/PlayersColon", string.Format("{0}/{1}", roomData.Players, roomData.MaxPlayers));
				this.joinResumeButton.interactable = false;
				return;
			}
			this.AddNewSetting(this.playersImage, "Lobby/PlayersColon", string.Format("{0}/{1}", roomData.Players, roomData.MaxPlayers));
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x000914D8 File Offset: 0x0008F6D8
		private void AddGameTime(LobbyRoom roomData)
		{
			if (!roomData.IsAsynchronous)
			{
				this.AddNewSetting(this.playerTimeImage, "Lobby/PlayerTimeColon", string.Format("{0} {1}", roomData.PlayerClockTime, ScriptLocalization.Get("Lobby/MinutesAbbreviation")));
				return;
			}
			if (roomData.PlayerClockTime / 1440 < 1)
			{
				this.AddNewSetting(this.playerTimeImage, "Lobby/PlayerTimeColon", string.Format("{0} {1}", roomData.PlayerClockTime / 60, ScriptLocalization.Get("Lobby/Hours")));
				return;
			}
			this.AddNewSetting(this.playerTimeImage, "Lobby/PlayerTimeColon", string.Format("{0} {1}", roomData.PlayerClockTime / 1440, ScriptLocalization.Get("Lobby/Days")));
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00091598 File Offset: 0x0008F798
		public void Init(AsynchronousGame gameData, Lobby lobby)
		{
			this.ClearEntry();
			this.lobby = lobby;
			this.SetGameName(gameData.Name);
			this.SetGameType(true);
			if (this.forfeitButton != null)
			{
				this.forfeitButton.gameObject.SetActive(true);
			}
			this.gameId = gameData.GameId;
			this.actualGameName = gameData.Name;
			this.isRanked = gameData.IsRanked;
			this.SetButtonText("Lobby/Resume");
			this.joinResumeButton.onClick.AddListener(delegate
			{
				this.ResumeGame(gameData.GameId, gameData.IsRanked);
			});
			if (gameData.IsRanked)
			{
				this.EnableRankedObject();
			}
			Sprite sprite = this.factionImages[gameData.PlayerFaction];
			string text = "Lobby/FactionColon";
			string text2 = "FactionMat/";
			Faction faction = (Faction)gameData.PlayerFaction;
			this.AddNewSetting(sprite, text, ScriptLocalization.Get(text2 + faction.ToString()));
			this.AddNewSetting(this.playersImage, "Lobby/PlayersColon", gameData.Players.ToString());
			this.AddNewSetting(this.playerTimeImage, "GameScene/TimeLeft", DateConverter.SecondsToMMHHmmssFormat(gameData.PlayerClock));
			if (gameData.IsPlayerTurn)
			{
				this.AddYourTurnSetting(this.turnImage, "Lobby/WhoseTurn", ScriptLocalization.Get("Lobby/YoursTurn"));
				if (this.backgroundFrame != null)
				{
					this.backgroundFrame.gameObject.SetActive(true);
				}
			}
			else
			{
				Sprite sprite2 = this.turnImage;
				string text3 = "Lobby/WhoseTurn";
				string text4 = "FactionMat/";
				faction = (Faction)gameData.CurrentFaction;
				this.AddNewSetting(sprite2, text3, ScriptLocalization.Get(text4 + faction.ToString()));
				if (this.backgroundFrame != null)
				{
					this.backgroundFrame.gameObject.SetActive(false);
				}
			}
			if (PlatformManager.IsStandalone && gameData.InvadersFromAfar)
			{
				this.AddExpansionSign();
				this.IsIFA = true;
				if (this.IsPlayerUsingIFAMat(gameData) && !GameServiceController.Instance.InvadersFromAfarUnlocked())
				{
					this.DLCRequiredPlate.SetActive(true);
					this.joinResumeButton.interactable = false;
				}
			}
			base.gameObject.SetActive(true);
			this.RefreshJoinResumeButtonActivity();
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x00032F47 File Offset: 0x00031147
		private bool IsPlayerUsingIFAMat(AsynchronousGame gameData)
		{
			return MatAndFactionSelection.IsDLCFaction((Faction)gameData.PlayerFaction) || MatAndFactionSelection.IsDLCPlayerMatType((PlayerMatType)gameData.PlayerMatType);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x000917F8 File Offset: 0x0008F9F8
		public void Init(GameToSpectate gameData, Lobby lobby)
		{
			this.ClearEntry();
			this.lobby = lobby;
			this.SetGameName(gameData.Name);
			this.SetGameType(false);
			if (this.forfeitButton != null)
			{
				this.forfeitButton.gameObject.SetActive(false);
			}
			if (this.backgroundFrame != null)
			{
				this.backgroundFrame.gameObject.SetActive(false);
			}
			this.SetButtonText("Lobby/Spectate");
			this.joinResumeButton.onClick.AddListener(delegate
			{
				this.SpectateGame(gameData.Id);
			});
			if (gameData.Ranked)
			{
				this.EnableRankedObject();
			}
			this.AddNewSetting(this.playersImage, "Lobby/PlayersColon", gameData.PlayersAmount);
			TimeSpan timeSpan = DateTime.UtcNow - DateTime.FromOADate(double.Parse(gameData.StartTime, NumberStyles.Any, CultureInfo.InvariantCulture));
			this.AddNewSetting(this.playerTimeImage, "Lobby/GameTimeColon", DateConverter.SecondsToMMHHmmssFormat((int)timeSpan.TotalSeconds));
			this.AddNewSetting(this.turnImage, "Lobby/TurnColon", (int.Parse(gameData.Turn) + 1).ToString());
			base.gameObject.SetActive(true);
			this.RefreshJoinResumeButtonActivity();
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00091958 File Offset: 0x0008FB58
		private void ClearEntry()
		{
			this.joinResumeButton.onClick.RemoveAllListeners();
			this.rankedGameObject.SetActive(false);
			while (this.content.childCount > 0)
			{
				Transform child = this.content.transform.GetChild(0);
				child.SetParent(null, false);
				global::UnityEngine.Object.Destroy(child.gameObject);
			}
			if (PlatformManager.IsStandalone)
			{
				this.DLCRequiredPlate.SetActive(false);
			}
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00032F63 File Offset: 0x00031163
		private void SetGameName(string name)
		{
			this.gameName.text = name;
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x000919C8 File Offset: 0x0008FBC8
		private void SetGameType(bool asynchronous)
		{
			if (asynchronous)
			{
				this.gameType.text = "<color=#479BDA>" + ScriptLocalization.Get("Lobby/AsynchronousGames") + "</color>";
				return;
			}
			this.gameType.text = "<color=#36AF60>" + ScriptLocalization.Get("Lobby/SynchronousGames") + "</color>";
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00032F71 File Offset: 0x00031171
		private void EnableRankedObject()
		{
			this.rankedGameObject.SetActive(true);
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00032F7F File Offset: 0x0003117F
		private void AddNewSetting(Sprite sprite, string title, string value)
		{
			global::UnityEngine.Object.Instantiate<SettingEntry>(this.settingPrefab, this.content).Init(sprite, ScriptLocalization.Get(title), value);
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00032F9F File Offset: 0x0003119F
		private void AddUnavailableSetting(Sprite sprite, string title, string value)
		{
			global::UnityEngine.Object.Instantiate<SettingEntry>(this.settingPrefab, this.content).InitUnavailable(sprite, ScriptLocalization.Get(title), value);
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00032FBF File Offset: 0x000311BF
		private void AddYourTurnSetting(Sprite sprite, string title, string value)
		{
			global::UnityEngine.Object.Instantiate<SettingEntry>(this.settingPrefab, this.content).InitYourTurnEntry(sprite, ScriptLocalization.Get(title), value);
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00032FDF File Offset: 0x000311DF
		private void SetButtonText(string textTerm)
		{
			this.joinResumeButtonText.text = ScriptLocalization.Get(textTerm);
			if (this.joinResumeButtonDisabledCopyText != null)
			{
				this.joinResumeButtonDisabledCopyText.text = ScriptLocalization.Get(textTerm);
			}
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00091A24 File Offset: 0x0008FC24
		private void RefreshJoinResumeButtonActivity()
		{
			if (this.joinResumeButtonDisabledCopy != null)
			{
				this.joinResumeButton.gameObject.SetActive(this.joinResumeButton.interactable);
				this.joinResumeButtonDisabledCopy.gameObject.SetActive(!this.joinResumeButton.interactable);
			}
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00033011 File Offset: 0x00031211
		private void AddExpansionSign()
		{
			global::UnityEngine.Object.Instantiate<GameObject>(this.expansionPrefab, this.content);
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00033025 File Offset: 0x00031225
		private void SpectateGame(string gameId)
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.lobby.SpectateGame(-1, gameId);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00033040 File Offset: 0x00031240
		private void JoinRoom(string roomId)
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.lobby.JoinRoom(roomId);
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0003305A File Offset: 0x0003125A
		private void ReturnToCurrentRoom()
		{
			this.lobby.ReturnToGameRoom();
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00033067 File Offset: 0x00031267
		private void ResumeGame(string gameId, bool ranked)
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.lobby.Reconnect(gameId, -1, ranked, GameType.Asynchronous);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00033084 File Offset: 0x00031284
		public bool IsButtonInteractable()
		{
			return this.joinResumeButton.interactable;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00033091 File Offset: 0x00031291
		public void ForfeitGame()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.lobby.ShowForfeitGamePanel(this.gameId, this.actualGameName, this.isRanked);
		}

		// Token: 0x04000CF5 RID: 3317
		[SerializeField]
		private GameObject rankedGameObject;

		// Token: 0x04000CF6 RID: 3318
		[SerializeField]
		private TextMeshProUGUI gameType;

		// Token: 0x04000CF7 RID: 3319
		[SerializeField]
		private Button forfeitButton;

		// Token: 0x04000CF8 RID: 3320
		[SerializeField]
		private TextMeshProUGUI gameName;

		// Token: 0x04000CF9 RID: 3321
		[SerializeField]
		private Sprite playersImage;

		// Token: 0x04000CFA RID: 3322
		[SerializeField]
		private Sprite playerTimeImage;

		// Token: 0x04000CFB RID: 3323
		[SerializeField]
		private Sprite promoCardsImage;

		// Token: 0x04000CFC RID: 3324
		[SerializeField]
		private Sprite eloImage;

		// Token: 0x04000CFD RID: 3325
		[SerializeField]
		private Sprite[] factionImages;

		// Token: 0x04000CFE RID: 3326
		[SerializeField]
		private Sprite turnImage;

		// Token: 0x04000CFF RID: 3327
		[SerializeField]
		private Sprite matChoicesImage;

		// Token: 0x04000D00 RID: 3328
		private const string eloTerm = "Lobby/ELOColon";

		// Token: 0x04000D01 RID: 3329
		private const string playersTerm = "Lobby/PlayersColon";

		// Token: 0x04000D02 RID: 3330
		private const string playersTimeTerm = "Lobby/PlayerTimeColon";

		// Token: 0x04000D03 RID: 3331
		private const string promoCardsTerm = "Lobby/PromoCardsColon";

		// Token: 0x04000D04 RID: 3332
		private const string gameTimeTerm = "Lobby/GameTimeColon";

		// Token: 0x04000D05 RID: 3333
		private const string turnTerm = "Lobby/TurnColon";

		// Token: 0x04000D06 RID: 3334
		private const string factionTerm = "Lobby/FactionColon";

		// Token: 0x04000D07 RID: 3335
		private const string timeLeftTerm = "GameScene/TimeLeft";

		// Token: 0x04000D08 RID: 3336
		private const string whoseTurnTerm = "Lobby/WhoseTurn";

		// Token: 0x04000D09 RID: 3337
		private const string matChoicesTerm = "Lobby/MatsChoicesColon";

		// Token: 0x04000D0A RID: 3338
		private const string playAndStayTerm = "Lobby/SynchronousGames";

		// Token: 0x04000D0B RID: 3339
		private const string playAndGoTerm = "Lobby/AsynchronousGames";

		// Token: 0x04000D0C RID: 3340
		private const string joinTerm = "Lobby/Join";

		// Token: 0x04000D0D RID: 3341
		private const string resumeTerm = "Lobby/Resume";

		// Token: 0x04000D0E RID: 3342
		private const string spectateTerm = "Lobby/Spectate";

		// Token: 0x04000D0F RID: 3343
		private const string backTerm = "Common/Back";

		// Token: 0x04000D10 RID: 3344
		[SerializeField]
		private GameObject expansionPrefab;

		// Token: 0x04000D11 RID: 3345
		[FormerlySerializedAs("button")]
		[SerializeField]
		private Button joinResumeButton;

		// Token: 0x04000D12 RID: 3346
		[FormerlySerializedAs("buttonText")]
		[SerializeField]
		private TextMeshProUGUI joinResumeButtonText;

		// Token: 0x04000D13 RID: 3347
		[SerializeField]
		private SettingEntry settingPrefab;

		// Token: 0x04000D14 RID: 3348
		[SerializeField]
		private Transform content;

		// Token: 0x04000D15 RID: 3349
		[SerializeField]
		private GameObject DLCRequiredPlate;

		// Token: 0x04000D16 RID: 3350
		[SerializeField]
		private Image backgroundFrame;

		// Token: 0x04000D17 RID: 3351
		private Lobby lobby;

		// Token: 0x04000D18 RID: 3352
		[Header("Join / Resume / Spectate Button Disabled Copy")]
		[SerializeField]
		private Image joinResumeButtonDisabledCopy;

		// Token: 0x04000D19 RID: 3353
		[SerializeField]
		private TextMeshProUGUI joinResumeButtonDisabledCopyText;

		// Token: 0x04000D1C RID: 3356
		private string gameId;

		// Token: 0x04000D1D RID: 3357
		private string actualGameName;

		// Token: 0x04000D1E RID: 3358
		private bool isRanked;
	}
}

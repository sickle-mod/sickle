using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.Multiplayer.AuthApi.Models;
using Scythe.Multiplayer.Data;
using Scythe.Multiplayer.Messages;
using Scythe.UI;
using Scythe.Utilities;

namespace Scythe.Multiplayer
{
	// Token: 0x0200021C RID: 540
	public class ReconnectManager
	{
		// Token: 0x14000052 RID: 82
		// (add) Token: 0x06000FF6 RID: 4086 RVA: 0x0008EBC0 File Offset: 0x0008CDC0
		// (remove) Token: 0x06000FF7 RID: 4087 RVA: 0x0008EBF4 File Offset: 0x0008CDF4
		public static event global::System.Action ShowReconnectPanel;

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x06000FF8 RID: 4088 RVA: 0x0008EC28 File Offset: 0x0008CE28
		// (remove) Token: 0x06000FF9 RID: 4089 RVA: 0x0008EC5C File Offset: 0x0008CE5C
		public static event global::System.Action HideReconnectPanel;

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x06000FFA RID: 4090 RVA: 0x0008EC90 File Offset: 0x0008CE90
		// (remove) Token: 0x06000FFB RID: 4091 RVA: 0x0008ECC4 File Offset: 0x0008CEC4
		public static event global::System.Action ShowReconnectError;

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000FFC RID: 4092 RVA: 0x000324B1 File Offset: 0x000306B1
		// (set) Token: 0x06000FFD RID: 4093 RVA: 0x000324B8 File Offset: 0x000306B8
		public static bool IsActive { get; private set; }

		// Token: 0x06000FFE RID: 4094 RVA: 0x000324C0 File Offset: 0x000306C0
		public static void Init()
		{
			ReconnectManager.IsActive = false;
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0008ECF8 File Offset: 0x0008CEF8
		public static void TryToReconnect()
		{
			if (ReconnectManager.IsActive)
			{
				return;
			}
			ReconnectManager.IsActive = true;
			global::System.Action showReconnectPanel = ReconnectManager.ShowReconnectPanel;
			if (showReconnectPanel != null)
			{
				showReconnectPanel();
			}
			Singleton<LoginController>.Instance.OnLoginSuccess += ReconnectManager.OnLoginSuccess;
			Singleton<LoginController>.Instance.OnLoginError += ReconnectManager.OnLoginError;
			ReconnectManager.SendLoginRequest();
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x000324C8 File Offset: 0x000306C8
		private static void SendLoginRequest()
		{
			Singleton<LoginController>.Instance.TryToAutoLogin(true);
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x000324D5 File Offset: 0x000306D5
		private static void OnLoginSuccess(LoginResponse result)
		{
			Singleton<LoginController>.Instance.OnLoginSuccess -= ReconnectManager.OnLoginSuccess;
			Singleton<LoginController>.Instance.OnLoginError -= ReconnectManager.OnLoginError;
			ReconnectManager.SendReconnectRequest();
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0008ED54 File Offset: 0x0008CF54
		private static void OnLoginError(string error)
		{
			DebugLog.LogError("Reconnect error on trying to login");
			PlayerInfo.me.Token = string.Empty;
			PlayerInfo.me.PlayerStats.Id = Guid.Empty;
			Singleton<LoginController>.Instance.OnLoginSuccess -= ReconnectManager.OnLoginSuccess;
			Singleton<LoginController>.Instance.OnLoginError -= ReconnectManager.OnLoginError;
			global::System.Action showReconnectError = ReconnectManager.ShowReconnectError;
			if (showReconnectError == null)
			{
				return;
			}
			showReconnectError();
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0008EDCC File Offset: 0x0008CFCC
		private static void SendReconnectRequest()
		{
			string text = "Reconnect/Reconnect";
			string text2 = RequestController.GenerateStringFromMessage<ReconnectData>(ReconnectManager.GenerateReconnectData());
			RequestController.RequestPostCall(text, text2, false, new Action<string>(ReconnectManager.OnReconnectSuccess), new Action<Exception>(ReconnectManager.OnReconnectError));
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0008EE0C File Offset: 0x0008D00C
		private static ReconnectData GenerateReconnectData()
		{
			GameType gameType = (MultiplayerController.Instance.Asynchronous ? GameType.Asynchronous : GameType.Synchronous);
			string text = GameServiceController.Instance.PlayerId();
			return new ReconnectData(PlayerInfo.me.RoomId, MultiplayerController.Instance.GetOwnerPlayer.PlayerClock, MultiplayerController.Instance.Ranked, gameType, text);
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0008EE60 File Offset: 0x0008D060
		private static void OnReconnectSuccess(string response)
		{
			global::System.Action hideReconnectPanel = ReconnectManager.HideReconnectPanel;
			if (hideReconnectPanel != null)
			{
				hideReconnectPanel();
			}
			ReconnectResponse reconnectResponse = GameSerializer.DeserializeObject<ReconnectResponse>(response);
			MultiplayerController.Instance.InitMultiplayer();
			ReconnectManager.InitGame(reconnectResponse.GameData);
			GameController.gameFromSave = false;
			MultiplayerController.Instance.IsLoading = true;
			SceneController.Instance.LoadScene(SceneController.SCENE_MAIN_NAME);
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0008EEB8 File Offset: 0x0008D0B8
		private static void InitGame(GameData gameData)
		{
			Scythe.GameLogic.Game game = GameController.Game;
			game.LoadFromString(gameData.GameState);
			game.GameManager.SetAmountOfCombatCardsLeft();
			List<int> list = new List<int>();
			if (!string.IsNullOrEmpty(gameData.LeaversFactions))
			{
				foreach (string text in gameData.LeaversFactions.Split(' ', StringSplitOptions.None))
				{
					list.Add(int.Parse(text));
				}
			}
			using (List<Player>.Enumerator enumerator = game.GameManager.GetPlayers().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Player player = enumerator.Current;
					if (!player.IsHuman)
					{
						PlayerData playerData = new PlayerData();
						playerData.PlayerClock = 0;
						playerData.Faction = (int)player.matFaction.faction;
						if (list.Contains(playerData.Faction))
						{
							playerData.Id = PlayerData.LeaverId;
						}
						else
						{
							playerData.Id = PlayerData.BotId;
						}
						playerData.HasTurn = false;
						playerData.MapLoaded = true;
						playerData.Name = "Bot";
						MultiplayerController.Instance.AddPlayer(playerData);
					}
					else
					{
						PlayerData playerData2 = gameData.Players.Find((PlayerData p) => p.Faction == (int)player.matFaction.faction);
						if (playerData2.Id == PlayerInfo.me.PlayerStats.Id)
						{
							game.GameManager.SetOwnerIdFromFaction((Faction)playerData2.Faction);
						}
						MultiplayerController.Instance.AddPlayer(playerData2);
					}
				}
			}
			MessageExecutor.ResetLastMessageCounter();
			MultiplayerController.Instance.InitAfterReturnToGame(game.GameManager, gameData.PlayerClock, gameData.MessageCounter);
			List<ChatMessage> list2 = JsonConvert.DeserializeObject<List<ChatMessage>>(gameData.ChatInJsonFormat);
			MultiplayerController.Instance.RestoredChatMessages(list2);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0008F0A4 File Offset: 0x0008D2A4
		private static void OnReconnectError(Exception exception)
		{
			try
			{
				DebugLog.LogError(string.Format("Error while reconnecting to the game: {0}", exception));
				GameSerializer.DeserializeObject<ReconnectErrorResponse>(exception.Message);
			}
			catch (Exception ex)
			{
				DebugLog.LogError(ex.ToString());
			}
			MessageExecutor.ResetLastMessageCounter();
			if (ReconnectManager.ShowReconnectError != null)
			{
				ReconnectManager.ShowReconnectError();
			}
		}

		// Token: 0x04000C49 RID: 3145
		public const string RECONNECT_ENDPOINT = "Reconnect";
	}
}

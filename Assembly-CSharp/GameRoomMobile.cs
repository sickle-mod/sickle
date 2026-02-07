using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Analytics;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using Scythe.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000032 RID: 50
public class GameRoomMobile : MonoBehaviour
{
	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600011E RID: 286 RVA: 0x00056800 File Offset: 0x00054A00
	// (remove) Token: 0x0600011F RID: 287 RVA: 0x00056838 File Offset: 0x00054A38
	public event global::System.Action PlayerKicked;

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000120 RID: 288 RVA: 0x00028AED File Offset: 0x00026CED
	// (set) Token: 0x06000121 RID: 289 RVA: 0x00028AF5 File Offset: 0x00026CF5
	public bool IsMinimized { get; private set; }

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000122 RID: 290 RVA: 0x00028AFE File Offset: 0x00026CFE
	// (set) Token: 0x06000123 RID: 291 RVA: 0x00028B06 File Offset: 0x00026D06
	public GameRoomMobile.GameRoomStateType CurrentGameRoomStateType { get; private set; }

	// Token: 0x06000124 RID: 292 RVA: 0x00028B0F File Offset: 0x00026D0F
	public void Show()
	{
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.game_room, Contexts.outgame);
		if (!this.IsMinimized)
		{
			this.SetGameRoomState(GameRoomMobile.GameRoomStateType.ADD_PLAYER);
		}
		else
		{
			this.IsMinimized = false;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00028B42 File Offset: 0x00026D42
	public void Restore()
	{
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.game_room, Contexts.outgame);
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00028B5D File Offset: 0x00026D5D
	public void Hide()
	{
		this.IsMinimized = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00028B72 File Offset: 0x00026D72
	public void Minimize()
	{
		if (!this.IsMinimized)
		{
			this.IsMinimized = true;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00056870 File Offset: 0x00054A70
	public void Init(LobbyGame lobbyGame)
	{
		Debug.LogError("ERROR JUST TO SHOW CONSOLE!");
		this.lobbyGame = lobbyGame;
		this.SetGameRoomSlots();
		this.SetGameRoomState(GameRoomMobile.GameRoomStateType.ADD_PLAYER);
		int num = lobbyGame.PlayersList.FindIndex((PlayerInfo player) => player.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id);
		PlayerInfo.me.Slot = lobbyGame.PlayersList[num].Slot;
		lobbyGame.PlayersList[num] = PlayerInfo.me;
		for (int i = 0; i < lobbyGame.PlayersList.Count; i++)
		{
			PlayerInfo playerInfo = lobbyGame.PlayersList[i];
			if (playerInfo != null)
			{
				this.AddPlayer(playerInfo, true);
			}
		}
		for (int j = 0; j < lobbyGame.BotsList.Count; j++)
		{
			Bot bot = lobbyGame.BotsList[j];
			if (bot != null)
			{
				this.AddBot(bot, true);
			}
		}
		this.gameRoomMessageLog.ClearAll();
		this.gamePropertiesPanel.Init(lobbyGame);
		this.RegisterMatAndFactionCarouselEvents();
		this.PromoteNewAdmin(lobbyGame.AdminId);
	}

	// Token: 0x06000129 RID: 297 RVA: 0x0005697C File Offset: 0x00054B7C
	public void SetGameRoomState(GameRoomMobile.GameRoomStateType gameRoomStateType)
	{
		this.CurrentGameRoomStateType = gameRoomStateType;
		this.factionsAndMatsSelected = false;
		GameRoomMobile.GameRoomStateType currentGameRoomStateType = this.CurrentGameRoomStateType;
		if (currentGameRoomStateType == GameRoomMobile.GameRoomStateType.ADD_PLAYER)
		{
			for (int i = 0; i < this.gameRoomSlots.Count; i++)
			{
				((SelectFactionAndMatGameRoomSlotState)this.gameRoomSlots[i].GetSlotState(GameRoomSlotState.GameRoomSlotStateType.SELECTING)).Reset();
				if (this.gameRoomSlots[i].PlayerData != null)
				{
					this.gameRoomSlots[i].SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.PLAYER);
				}
				else if (this.gameRoomSlots[i].BotData != null)
				{
					this.gameRoomSlots[i].SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.BOT);
				}
				else
				{
					this.gameRoomSlots[i].SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.EMPTY);
				}
			}
			if (PlayerInfo.me.IsAdmin)
			{
				this.SetInvitationStateOnFirstEmptySlot();
			}
			this.selectFactionAndMatPanel.gameObject.SetActive(false);
			this.gamePropertiesPanel.gameObject.SetActive(true);
			this.RefreshButtons();
			return;
		}
		if (currentGameRoomStateType != GameRoomMobile.GameRoomStateType.SELECT_FACTIONS_AND_MAT)
		{
			return;
		}
		for (int j = 0; j < this.gameRoomSlots.Count; j++)
		{
			if (this.gameRoomSlots[j].CurrentGameRoomSlotStateType == GameRoomSlotState.GameRoomSlotStateType.PLAYER || this.gameRoomSlots[j].CurrentGameRoomSlotStateType == GameRoomSlotState.GameRoomSlotStateType.BOT)
			{
				this.gameRoomSlots[j].SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.SELECTING);
			}
			else
			{
				this.gameRoomSlots[j].SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.EMPTY);
			}
		}
		this.gamePropertiesPanel.gameObject.SetActive(false);
		this.selectFactionAndMatPanel.gameObject.SetActive(true);
		this.RefreshButtons();
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00056B0C File Offset: 0x00054D0C
	public void SetSlotsOrder(List<int> oldSlotIDs, List<int> newSlotIDs, List<Guid> playerIDs)
	{
		GameRoomSlot[] array = new GameRoomSlot[this.gameRoomSlots.Count];
		for (int i = 0; i < playerIDs.Count; i++)
		{
			for (int j = 0; j < newSlotIDs.Count; j++)
			{
				if (newSlotIDs[j] == i)
				{
					if (PlayerInfo.me.Slot == oldSlotIDs[j])
					{
						PlayerInfo.me.Slot = newSlotIDs[j];
					}
					this.gameRoomSlots[oldSlotIDs[j]].transform.SetSiblingIndex(newSlotIDs[j]);
					array[newSlotIDs[j]] = this.gameRoomSlots[oldSlotIDs[j]];
					break;
				}
			}
		}
		for (int k = 0; k < this.gameRoomSlots.Count; k++)
		{
			if (this.gameRoomSlots[k].CurrentGameRoomSlotStateType != GameRoomSlotState.GameRoomSlotStateType.PLAYER && this.gameRoomSlots[k].CurrentGameRoomSlotStateType != GameRoomSlotState.GameRoomSlotStateType.BOT)
			{
				for (int l = 0; l < array.Length; l++)
				{
					if (array[l] == null)
					{
						array[l] = this.gameRoomSlots[k];
						break;
					}
				}
			}
		}
		this.gameRoomSlots = new List<GameRoomSlot>(array);
		for (int m = 0; m < this.gameRoomSlots.Count; m++)
		{
			if (this.gameRoomSlots[m].PlayerData != null)
			{
				this.gameRoomSlots[m].PlayerData.Slot = m;
			}
			if (this.gameRoomSlots[m].BotData != null)
			{
				this.gameRoomSlots[m].BotData.Slot = m;
			}
		}
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00028B8F File Offset: 0x00026D8F
	public void LockSlots()
	{
		this.gameRoomSlotsCanvasGroup.interactable = false;
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00028B9D File Offset: 0x00026D9D
	public void UnlockSlots()
	{
		this.gameRoomSlotsCanvasGroup.interactable = true;
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00028BAB File Offset: 0x00026DAB
	public void LockButtons()
	{
		this.gameRoomButtonsCanvasGroup.interactable = false;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00028BB9 File Offset: 0x00026DB9
	public void UnlockButtons()
	{
		this.gameRoomButtonsCanvasGroup.interactable = true;
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00028BC7 File Offset: 0x00026DC7
	public void StartGame()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		LobbyRestAPI.StartGame(new Action<string>(this.StartGame_OnSuccess), new Action<Exception>(this.StartGame_OnFailure));
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00056CB0 File Offset: 0x00054EB0
	public void LeaveRoom()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.lobby, Contexts.outgame);
		base.gameObject.SetActive(false);
		this.lobby.LeaveRoom();
		this.ResetGameRoomSlots();
		this.DeregisterMatAndFactionCarouselEvents();
		this.matChoiceTimer.Deactivate();
		this.matAndFactionChoose = null;
		this.UnlockButtons();
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00056D18 File Offset: 0x00054F18
	public void AddPlayer(PlayerInfo playerData, bool onInit = false)
	{
		Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej AddPlayer: " + ((playerData != null) ? playerData.ToString() : null));
		this.gameRoomSlots[playerData.Slot].SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.PLAYER);
		if (this.gameRoomSlots[playerData.Slot].BotData != null)
		{
			string[] array = new string[6];
			array[0] = "[";
			array[1] = Time.time.ToString();
			array[2] = "] Sir_Maciej AddPlayer BOT IS NOT NULL! slot: ";
			array[3] = playerData.Slot.ToString();
			array[4] = " bot: ";
			int num = 5;
			Bot botData = this.gameRoomSlots[playerData.Slot].BotData;
			array[num] = ((botData != null) ? botData.ToString() : null);
			Debug.Log(string.Concat(array));
			this.gameRoomSlots[playerData.Slot].BotData = null;
		}
		if (playerData.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id)
		{
			PlayerInfo.me.Slot = playerData.Slot;
			this.gameRoomSlots[playerData.Slot].PlayerData = PlayerInfo.me;
		}
		else
		{
			this.gameRoomSlots[playerData.Slot].PlayerData = playerData;
		}
		if (!onInit)
		{
			this.lobbyGame.PlayersList.Add(playerData);
			LobbyGame lobbyGame = this.lobbyGame;
			int players = lobbyGame.Players;
			lobbyGame.Players = players + 1;
			this.gamePropertiesPanel.Init(this.lobbyGame);
			this.gameRoomMessageLog.LogMessage(ScriptLocalization.Get("Lobby/SLSystem"), ScriptLocalization.Get("Lobby/SLPlayerJoinedGame").Replace("{[PLAYER_NAME]}", playerData.PlayerStats.Name));
			if (PlayerInfo.me.IsAdmin)
			{
				this.SetInvitationStateOnFirstEmptySlot();
			}
		}
		this.RefreshButtons();
		this.RefreshStatus();
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00056EFC File Offset: 0x000550FC
	public void RemovePlayer(Guid playerId)
	{
		PlayerInfo playerInfo2 = this.lobbyGame.PlayersList.Find((PlayerInfo playerInfo) => playerInfo.PlayerStats.Id == playerId);
		if (playerInfo2 == null)
		{
			return;
		}
		this.lobbyGame.PlayersList.Remove(playerInfo2);
		LobbyGame lobbyGame = this.lobbyGame;
		int players = lobbyGame.Players;
		lobbyGame.Players = players - 1;
		this.gamePropertiesPanel.Init(this.lobbyGame);
		GameRoomSlot gameRoomSlot = null;
		for (int i = 0; i < this.gameRoomSlots.Count; i++)
		{
			if (this.gameRoomSlots[i].PlayerData != null && this.gameRoomSlots[i].PlayerData.PlayerStats.Id == playerId)
			{
				gameRoomSlot = this.gameRoomSlots[i];
				break;
			}
		}
		if (gameRoomSlot != null)
		{
			string text = "[";
			string text2 = Time.time.ToString();
			string text3 = "] Sir_Maciej RemovePlayer: ";
			PlayerInfo playerData = gameRoomSlot.PlayerData;
			Debug.Log(text + text2 + text3 + ((playerData != null) ? playerData.ToString() : null));
			this.gameRoomMessageLog.LogMessage(ScriptLocalization.Get("Lobby/SLSystem"), ScriptLocalization.Get("Lobby/SLPlayerLeftGame").Replace("{[PLAYER_NAME]}", gameRoomSlot.PlayerData.PlayerStats.Name));
			if (gameRoomSlot.CurrentGameRoomSlotStateType == GameRoomSlotState.GameRoomSlotStateType.SELECTING)
			{
				SelectFactionAndMatGameRoomSlotState selectFactionAndMatGameRoomSlotState = (SelectFactionAndMatGameRoomSlotState)gameRoomSlot.CurrentGameRoomSlotState;
				if (selectFactionAndMatGameRoomSlotState.IsSelected)
				{
					this.matAndFactionChoose.AddLeaversMats(selectFactionAndMatGameRoomSlotState.SelectedFactionID, selectFactionAndMatGameRoomSlotState.SelectedMatID);
				}
			}
			gameRoomSlot.PlayerData = null;
			gameRoomSlot.SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.EMPTY);
		}
		if (PlayerInfo.me.PlayerStats.Id == playerId)
		{
			this.DeregisterMatAndFactionCarouselEvents();
			this.UnlockButtons();
			this.Hide();
			PlayerInfo.me.CurrentLobbyRoom = null;
			PlayerInfo.me.RoomId = string.Empty;
			UniversalInvocator.Event_Invocator(this.PlayerKicked);
		}
		else if (this.CurrentGameRoomStateType == GameRoomMobile.GameRoomStateType.ADD_PLAYER)
		{
			if (PlayerInfo.me.IsAdmin)
			{
				this.SetInvitationStateOnFirstEmptySlot();
			}
		}
		else if (this.GetNumberOfSlotsWithPlayerData() == 1)
		{
			this.SetGameRoomState(GameRoomMobile.GameRoomStateType.ADD_PLAYER);
			this.matAndFactionChoose = null;
			this.matChoiceTimer.Deactivate();
		}
		else if (this.matAndFactionChoose != null)
		{
			int currentSlot = this.matAndFactionChoose.GetCurrentSlot();
			if (gameRoomSlot.SlotID == currentSlot)
			{
				this.matChoiceTimer.Deactivate();
				this.MoveToNextPlayer();
			}
		}
		this.RefreshButtons();
		this.RefreshStatus();
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0005716C File Offset: 0x0005536C
	public void SetReadyPlayer(Guid playerId, bool value)
	{
		int i = 0;
		while (i < this.gameRoomSlots.Count)
		{
			if (this.gameRoomSlots[i].PlayerData != null && this.gameRoomSlots[i].PlayerData.PlayerStats.Id == playerId)
			{
				this.gameRoomSlots[i].PlayerData.IsReady = value;
				if (this.gameRoomSlots[i].CurrentGameRoomSlotStateType != GameRoomSlotState.GameRoomSlotStateType.PLAYER)
				{
					break;
				}
				((PlayerGameRoomSlotState)this.gameRoomSlots[i].CurrentGameRoomSlotState).SetActiveReadyPlate(value);
				if (value)
				{
					this.gameRoomMessageLog.LogMessage(ScriptLocalization.Get("Lobby/SLSystem"), ScriptLocalization.Get("Lobby/SLPlayerIsReady").Replace("{[PLAYER_NAME]}", this.gameRoomSlots[i].PlayerData.PlayerStats.Name));
					break;
				}
				break;
			}
			else
			{
				i++;
			}
		}
		this.RefreshButtons();
		this.RefreshStatus();
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00057270 File Offset: 0x00055470
	public void AddBot(Bot botData, bool onInit = false)
	{
		if (botData != null)
		{
			if (this.gameRoomSlots[botData.Slot].CurrentGameRoomSlotStateType != GameRoomSlotState.GameRoomSlotStateType.BOT && !onInit)
			{
				this.lobbyGame.BotsList.Add(botData);
				LobbyGame lobbyGame = this.lobbyGame;
				int players = lobbyGame.Players;
				lobbyGame.Players = players + 1;
				this.gameRoomMessageLog.LogMessage(ScriptLocalization.Get("Lobby/SLSystem"), ScriptLocalization.Get("Lobby/SLAdminAddedBot").Replace("{[BOT_NAME]}", botData.Name));
			}
			if (this.gameRoomSlots[botData.Slot].PlayerData != null)
			{
				this.gameRoomSlots[botData.Slot].PlayerData = null;
			}
			this.gameRoomSlots[botData.Slot].BotData = botData;
			this.gameRoomSlots[botData.Slot].SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.BOT);
			BotGameRoomSlotState botGameRoomSlotState = (BotGameRoomSlotState)this.gameRoomSlots[botData.Slot].CurrentGameRoomSlotState;
			botGameRoomSlotState.SetActiveKickBotButton(PlayerInfo.me.IsAdmin);
			botGameRoomSlotState.SetInteractableBotLevelButton(PlayerInfo.me.IsAdmin);
			this.gamePropertiesPanel.Init(this.lobbyGame);
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0005739C File Offset: 0x0005559C
	public void RemoveBot(int slotId)
	{
		if (this.gameRoomSlots != null && slotId >= 0 && slotId < this.gameRoomSlots.Count && this.gameRoomSlots[slotId].BotData != null)
		{
			string text = "[";
			string text2 = Time.time.ToString();
			string text3 = "] Sir_Maciej RemoveBot: ";
			Bot botData = this.gameRoomSlots[slotId].BotData;
			Debug.Log(text + text2 + text3 + ((botData != null) ? botData.ToString() : null));
			this.gameRoomSlots[slotId].SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.EMPTY);
			this.gameRoomMessageLog.LogMessage(ScriptLocalization.Get("Lobby/SLSystem"), ScriptLocalization.Get("Lobby/SLAdminRemovedBot").Replace("{[BOT_NAME]}", this.gameRoomSlots[slotId].BotData.Name));
			this.gameRoomSlots[slotId].BotData = null;
			this.lobbyGame.BotsList.RemoveAt(this.lobbyGame.BotsList.FindIndex((Bot bot) => bot.Slot == slotId));
			LobbyGame lobbyGame = this.lobbyGame;
			int players = lobbyGame.Players;
			lobbyGame.Players = players - 1;
			this.gamePropertiesPanel.Init(this.lobbyGame);
		}
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00057508 File Offset: 0x00055708
	public void PromoteNewAdmin(Guid newAdminID)
	{
		string text = "[";
		string text2 = Time.time.ToString();
		string text3 = "] Sir_Maciej PromoteNewAdmin ID: ";
		Guid guid = newAdminID;
		Debug.Log(text + text2 + text3 + guid.ToString());
		if (newAdminID == PlayerInfo.me.PlayerStats.Id)
		{
			PlayerInfo.me.IsAdmin = true;
			PlayerInfo.me.IsReady = true;
			if (this.CurrentGameRoomStateType == GameRoomMobile.GameRoomStateType.ADD_PLAYER)
			{
				for (int i = 0; i < this.gameRoomSlots.Count; i++)
				{
					if (this.gameRoomSlots[i].PlayerData != null)
					{
						PlayerGameRoomSlotState playerGameRoomSlotState = (PlayerGameRoomSlotState)this.gameRoomSlots[i].GetSlotState(GameRoomSlotState.GameRoomSlotStateType.PLAYER);
						playerGameRoomSlotState.SetActivePendingPlate(false);
						if (this.gameRoomSlots[i].PlayerData.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id)
						{
							playerGameRoomSlotState.SetActiveKickPlayerButton(false);
							playerGameRoomSlotState.SetActiveReadyPlate(false);
						}
						else
						{
							playerGameRoomSlotState.SetActiveKickPlayerButton(true);
							playerGameRoomSlotState.SetInteractableKickPlayerButton(true);
							playerGameRoomSlotState.SetActiveReadyPlate(this.gameRoomSlots[i].PlayerData.IsReady);
						}
					}
					if (this.gameRoomSlots[i].BotData != null)
					{
						BotGameRoomSlotState botGameRoomSlotState = (BotGameRoomSlotState)this.gameRoomSlots[i].GetSlotState(GameRoomSlotState.GameRoomSlotStateType.BOT);
						botGameRoomSlotState.SetActiveKickBotButton(true);
						botGameRoomSlotState.SetInteractableBotLevelButton(true);
						botGameRoomSlotState.SetActivePendingPlate(false);
						botGameRoomSlotState.SetActiveReadyPlate(false);
					}
				}
				this.SetInvitationStateOnFirstEmptySlot();
			}
		}
		else
		{
			for (int j = 0; j < this.gameRoomSlots.Count; j++)
			{
				if (this.gameRoomSlots[j].PlayerData != null)
				{
					PlayerGameRoomSlotState playerGameRoomSlotState2 = (PlayerGameRoomSlotState)this.gameRoomSlots[j].GetSlotState(GameRoomSlotState.GameRoomSlotStateType.PLAYER);
					playerGameRoomSlotState2.SetActivePendingPlate(false);
					if (this.gameRoomSlots[j].PlayerData.PlayerStats.Id == newAdminID)
					{
						playerGameRoomSlotState2.SetActiveReadyPlate(false);
					}
					else
					{
						playerGameRoomSlotState2.SetActiveReadyPlate(this.gameRoomSlots[j].PlayerData.IsReady);
					}
					playerGameRoomSlotState2.SetActiveKickPlayerButton(false);
				}
				if (this.gameRoomSlots[j].BotData != null)
				{
					BotGameRoomSlotState botGameRoomSlotState2 = (BotGameRoomSlotState)this.gameRoomSlots[j].GetSlotState(GameRoomSlotState.GameRoomSlotStateType.BOT);
					botGameRoomSlotState2.SetActiveKickBotButton(false);
					botGameRoomSlotState2.SetInteractableBotLevelButton(false);
				}
			}
		}
		this.RefreshButtons();
		this.RefreshStatus();
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0005777C File Offset: 0x0005597C
	public void StartChoosing()
	{
		this.matAndFactionChoose = new MatAndFactionChoose(this.lobbyGame.InvadersFromAfar);
		for (int i = 0; i < this.gameRoomSlots.Count; i++)
		{
			if (this.gameRoomSlots[i].CurrentGameRoomSlotStateType == GameRoomSlotState.GameRoomSlotStateType.SELECTING)
			{
				((SelectFactionAndMatGameRoomSlotState)this.gameRoomSlots[i].CurrentGameRoomSlotState).Reset();
			}
		}
		this.selectFactionAndMatPanel.Reset();
		this.MoveToNextPlayer();
	}

	// Token: 0x06000138 RID: 312 RVA: 0x000577F8 File Offset: 0x000559F8
	public void MoveToNextPlayer()
	{
		this.matAndFactionChoose.MoveToNextPlayer();
		int currentSlot = this.matAndFactionChoose.GetCurrentSlot();
		if (currentSlot >= this.gameRoomSlots.Count)
		{
			this.EndChoosing();
		}
		else if (this.gameRoomSlots[currentSlot].CurrentGameRoomSlotStateType == GameRoomSlotState.GameRoomSlotStateType.EMPTY)
		{
			this.MoveToNextPlayer();
		}
		else
		{
			for (int i = 0; i < this.gameRoomSlots.Count; i++)
			{
				SelectFactionAndMatGameRoomSlotState selectFactionAndMatGameRoomSlotState = (SelectFactionAndMatGameRoomSlotState)this.gameRoomSlots[i].GetSlotState(GameRoomSlotState.GameRoomSlotStateType.SELECTING);
				selectFactionAndMatGameRoomSlotState.SetActiveGoldFrame(i == currentSlot);
				selectFactionAndMatGameRoomSlotState.SetActiveTimer(i == currentSlot);
			}
			string text = ((this.gameRoomSlots[currentSlot].PlayerData != null) ? this.gameRoomSlots[currentSlot].PlayerData.PlayerStats.Name : this.gameRoomSlots[currentSlot].BotData.Name);
			GameRoomFactionAndMatSelectionCarousel.PlayerType playerType = GameRoomFactionAndMatSelectionCarousel.PlayerType.Player;
			bool flag = this.lobbyGame.InvadersFromAfar;
			if (flag && this.gameRoomSlots[currentSlot].PlayerData != null)
			{
				flag = this.gameRoomSlots[currentSlot].PlayerData.DLC;
			}
			if (this.gameRoomSlots[currentSlot].PlayerData == null)
			{
				playerType = (GameRoomFactionAndMatSelectionCarousel.PlayerType)this.gameRoomSlots[currentSlot].BotData.Difficulty;
			}
			this.selectFactionAndMatPanel.SetPlayerName(text);
			this.selectFactionAndMatPanel.SetPlayerType(playerType);
			this.selectFactionAndMatPanel.SetInvadersMatsUnlocked(flag);
			this.selectFactionAndMatPanel.SetFaction(-1);
			this.selectFactionAndMatPanel.SetMat(-1);
			this.matChoiceTimer.OnTimeChanged += this.MatChoiceTimer_OnTimeChanged;
			this.matChoiceTimer.OnTimePassed += this.MatChoiceTimer_OnTimePassed;
			this.matChoiceTimer.OnAdditionalTimePassed += this.MatChoiceTimer_OnAdditionalTimePassed;
			this.matChoiceTimer.Activate();
			if (this.gameRoomSlots[currentSlot].PlayerData != null)
			{
				if (this.gameRoomSlots[currentSlot].PlayerData.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id)
				{
					this.selectFactionAndMatPanel.SetActiveTimer(false);
					this.selectFactionAndMatPanel.SetFactionInteractable(true);
					this.selectFactionAndMatPanel.SetMatInteractable(true);
				}
				else
				{
					this.selectFactionAndMatPanel.SetActiveTimer(true);
					this.selectFactionAndMatPanel.SetFactionInteractable(false);
					this.selectFactionAndMatPanel.SetMatInteractable(false);
				}
			}
			else if (PlayerInfo.me.IsAdmin && !this.autoselectFactionAndMatForBot)
			{
				this.selectFactionAndMatPanel.SetActiveTimer(false);
				this.selectFactionAndMatPanel.SetFactionInteractable(true);
				this.selectFactionAndMatPanel.SetMatInteractable(true);
			}
			else
			{
				this.selectFactionAndMatPanel.SetActiveTimer(true);
				this.selectFactionAndMatPanel.SetFactionInteractable(false);
				this.selectFactionAndMatPanel.SetMatInteractable(false);
			}
		}
		this.RefreshButtons();
		this.RefreshStatus();
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00057AD4 File Offset: 0x00055CD4
	public void FactionAndMatChoosen(int factionID, int matID, int slotID)
	{
		if (this.gameRoomSlots[slotID].PlayerData != null && this.gameRoomSlots[slotID].PlayerData.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id)
		{
			this.selectModeReadyButton.interactable = false;
			this.selectFactionAndMatPanel.SetFactionInteractable(false);
			this.selectFactionAndMatPanel.SetMatInteractable(false);
		}
		this.matChoiceTimer.Deactivate();
		SelectFactionAndMatGameRoomSlotState selectFactionAndMatGameRoomSlotState = (SelectFactionAndMatGameRoomSlotState)this.gameRoomSlots[slotID].CurrentGameRoomSlotState;
		selectFactionAndMatGameRoomSlotState.SetPlayerFaction(factionID);
		selectFactionAndMatGameRoomSlotState.SetPlayerMat(matID);
		selectFactionAndMatGameRoomSlotState.IsSelected = true;
		this.matAndFactionChoose.RemoveData(factionID, matID);
		this.MoveToNextPlayer();
	}

	// Token: 0x0600013A RID: 314 RVA: 0x00028BF2 File Offset: 0x00026DF2
	public void StartGameButtonClick()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_start_button);
		this.LockButtons();
		this.StartGame();
	}

	// Token: 0x0600013B RID: 315 RVA: 0x00057B94 File Offset: 0x00055D94
	public void PlayerReadyButtonClick()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		bool flag = !PlayerInfo.me.IsReady;
		this.SetReadyPlayer(PlayerInfo.me.PlayerStats.Id, flag);
		LobbyRestAPI.UpdateReadyState(new Action<string>(this.PlayerReady_OnSuccess), new Action<Exception>(this.PlayerReady_OnFailure));
	}

	// Token: 0x0600013C RID: 316 RVA: 0x00028C07 File Offset: 0x00026E07
	public void PlayerMatSelectionButtonClick()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		this.matChoiceTimer.Deactivate();
		this.MatChoiceTimer_OnTimePassed();
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void LeaveGameRoomButtonClick()
	{
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void MinimizeGameRoomButtonClick()
	{
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void EditGameRoomButtonClick()
	{
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00057BF0 File Offset: 0x00055DF0
	public void SelectMatsButtonClick()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		this.selectMatsButton.interactable = false;
		int num = this.GetNumberOfSlots(GameRoomSlotState.GameRoomSlotStateType.PLAYER) + this.GetNumberOfSlots(GameRoomSlotState.GameRoomSlotStateType.BOT);
		List<int> list = new List<int>();
		for (int i = 0; i < num; i++)
		{
			list.Add(i);
		}
		this.startingOrder = new StartingOrder();
		for (int j = 0; j < this.gameRoomSlots.Count; j++)
		{
			if (this.gameRoomSlots[j].CurrentGameRoomSlotStateType == GameRoomSlotState.GameRoomSlotStateType.PLAYER || this.gameRoomSlots[j].CurrentGameRoomSlotStateType == GameRoomSlotState.GameRoomSlotStateType.BOT)
			{
				this.startingOrder.Ids.Add((this.gameRoomSlots[j].PlayerData != null) ? this.gameRoomSlots[j].PlayerData.PlayerStats.Id : Guid.Empty.BotGuid());
				this.startingOrder.OldSlots.Add(j);
				int num2 = global::UnityEngine.Random.Range(0, list.Count);
				int num3 = list[num2];
				list.RemoveAt(num2);
				this.startingOrder.NewSlots.Add(num3);
			}
		}
		LobbyRestAPI.StartSelectingMats(this.startingOrder, new Action<string>(this.StartSelectingMats_OnSuccess), new Action<Exception>(this.StartSelectingMats_OnFailure));
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00057D40 File Offset: 0x00055F40
	private void ResetGameRoomSlots()
	{
		if (this.gameRoomSlots == null)
		{
			this.gameRoomSlots = new List<GameRoomSlot>();
		}
		for (int i = 0; i < this.gameRoomSlots.Count; i++)
		{
			this.gameRoomSlots[i].Reset();
			this.gameRoomSlots[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00057DA0 File Offset: 0x00055FA0
	private void SetGameRoomSlots()
	{
		if (this.lobbyGame != null)
		{
			this.ResetGameRoomSlots();
			for (int i = 0; i < this.lobbyGame.MaxPlayers; i++)
			{
				GameRoomSlot gameRoomSlot;
				if (i < this.gameRoomSlots.Count)
				{
					gameRoomSlot = this.gameRoomSlots[i];
				}
				else
				{
					gameRoomSlot = global::UnityEngine.Object.Instantiate<GameObject>(this.gameRoomSlotPrefab, this.gameRoomSlotsParent).GetComponent<GameRoomSlot>();
					gameRoomSlot.SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.EMPTY);
				}
				gameRoomSlot.gameObject.SetActive(true);
				gameRoomSlot.OnInvitePlayerButtonClick += this.GameRoomSlot_OnInvitePlayerButtonClick;
				gameRoomSlot.OnKickPlayerButtonClick += this.GameRoomSlot_OnKickPlayerButtonClick;
				gameRoomSlot.OnAddBotButtonClick += this.GameRoomSlot_OnAddBotButtonClick;
				gameRoomSlot.OnKickBotButtonClick += this.GameRoomSlot_OnKickBotButtonClick;
				gameRoomSlot.OnBotLevelButtonClick += this.GameRoomSlot_OnBotLevelButtonClick;
				gameRoomSlot.OnKickPlayerSuccess += this.GameRoomSlot_OnKickPlayerSuccess;
				gameRoomSlot.OnKickPlayerFailure += this.GameRoomSlot_OnKickPlayerFailure;
				gameRoomSlot.OnAddBotSuccess += this.GameRoomSlot_OnAddBotSuccess;
				gameRoomSlot.OnAddBotFailure += this.GameRoomSlot_OnAddBotFailure;
				gameRoomSlot.OnKickBotSuccess += this.GameRoomSlot_OnKickBotSuccess;
				gameRoomSlot.OnKickBotFailure += this.GameRoomSlot_OnKickBotFailure;
				gameRoomSlot.OnUpdateBotSuccess += this.GameRoomSlot_OnUpdateBotSuccess;
				gameRoomSlot.OnUpdateBotFailure += this.GameRoomSlot_OnUpdateBotFailure;
				if (!this.gameRoomSlots.Contains(gameRoomSlot))
				{
					this.gameRoomSlots.Add(gameRoomSlot);
				}
			}
		}
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00057F28 File Offset: 0x00056128
	private void RefreshButtons()
	{
		this.editGameButton.gameObject.SetActive(false);
		this.startButton.gameObject.SetActive(false);
		this.selectMatsButton.gameObject.SetActive(false);
		this.addModeReadyButton.gameObject.SetActive(false);
		this.selectModeReadyButton.gameObject.SetActive(false);
		GameRoomMobile.GameRoomStateType currentGameRoomStateType = this.CurrentGameRoomStateType;
		if (currentGameRoomStateType != GameRoomMobile.GameRoomStateType.ADD_PLAYER)
		{
			if (currentGameRoomStateType != GameRoomMobile.GameRoomStateType.SELECT_FACTIONS_AND_MAT)
			{
				return;
			}
			if (!this.factionsAndMatsSelected)
			{
				this.selectModeReadyButton.gameObject.SetActive(true);
				SelectFactionAndMatGameRoomSlotState selectFactionAndMatGameRoomSlotState = this.gameRoomSlots[PlayerInfo.me.Slot].CurrentGameRoomSlotState as SelectFactionAndMatGameRoomSlotState;
				if (selectFactionAndMatGameRoomSlotState && selectFactionAndMatGameRoomSlotState.SelectedFactionID != -1)
				{
					this.selectModeReadyButtonCheckmarkImage.gameObject.SetActive(true);
				}
				else
				{
					this.selectModeReadyButtonCheckmarkImage.gameObject.SetActive(false);
				}
				if (this.matAndFactionChoose != null)
				{
					int currentSlot = this.matAndFactionChoose.GetCurrentSlot();
					if (this.gameRoomSlots[currentSlot].PlayerData != null)
					{
						if (this.gameRoomSlots[currentSlot].PlayerData.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id)
						{
							this.selectModeReadyButton.interactable = true;
							return;
						}
						this.selectModeReadyButton.interactable = false;
						return;
					}
					else if (this.gameRoomSlots[currentSlot].BotData != null)
					{
						if (PlayerInfo.me.IsAdmin && !this.autoselectFactionAndMatForBot)
						{
							this.selectModeReadyButton.interactable = true;
							return;
						}
						this.selectModeReadyButton.interactable = false;
						return;
					}
					else
					{
						this.selectModeReadyButton.interactable = false;
					}
				}
				return;
			}
			if (PlayerInfo.me.IsAdmin)
			{
				this.startButton.gameObject.SetActive(true);
				this.startButton.interactable = true;
				return;
			}
			this.selectModeReadyButton.gameObject.SetActive(true);
			this.selectModeReadyButton.interactable = false;
			this.selectModeReadyButtonCheckmarkImage.gameObject.SetActive(true);
			return;
		}
		else if (PlayerInfo.me.IsAdmin)
		{
			if (this.lobbyGame.AllRandom)
			{
				this.startButton.gameObject.SetActive(true);
				if (this.GetNumberOfSlots(GameRoomSlotState.GameRoomSlotStateType.PLAYER) > 1 && this.AllPlayersReady())
				{
					this.startButton.interactable = true;
					return;
				}
				this.startButton.interactable = false;
				return;
			}
			else
			{
				this.selectMatsButton.gameObject.SetActive(true);
				if (this.GetNumberOfSlots(GameRoomSlotState.GameRoomSlotStateType.PLAYER) > 1 && this.AllPlayersReady())
				{
					this.selectMatsButton.interactable = true;
					return;
				}
				this.selectMatsButton.interactable = false;
				return;
			}
		}
		else
		{
			this.addModeReadyButton.gameObject.SetActive(true);
			if (PlayerInfo.me.IsReady)
			{
				this.addModeReadyButton.interactable = true;
				this.addModeReadyButtonCheckmarkImage.gameObject.SetActive(true);
				return;
			}
			this.addModeReadyButton.interactable = true;
			this.addModeReadyButtonCheckmarkImage.gameObject.SetActive(false);
			return;
		}
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00058214 File Offset: 0x00056414
	private void RefreshStatus()
	{
		if (this.CurrentGameRoomStateType != GameRoomMobile.GameRoomStateType.ADD_PLAYER)
		{
			if (this.CurrentGameRoomStateType == GameRoomMobile.GameRoomStateType.SELECT_FACTIONS_AND_MAT)
			{
				if (!this.factionsAndMatsSelected)
				{
					if (this.gameRoomSlots[this.matAndFactionChoose.GetCurrentSlot()].PlayerData != null && this.gameRoomSlots[this.matAndFactionChoose.GetCurrentSlot()].PlayerData.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id)
					{
						this.gameRoomMessageLog.LogState(ScriptLocalization.Get("Lobby/SLSYourTurnSelecting"));
						return;
					}
					string text = "";
					if (this.gameRoomSlots[this.matAndFactionChoose.GetCurrentSlot()].PlayerData != null)
					{
						text = this.gameRoomSlots[this.matAndFactionChoose.GetCurrentSlot()].PlayerData.PlayerStats.Name;
					}
					else if (this.gameRoomSlots[this.matAndFactionChoose.GetCurrentSlot()].BotData != null)
					{
						text = this.gameRoomSlots[this.matAndFactionChoose.GetCurrentSlot()].BotData.Name;
					}
					this.gameRoomMessageLog.LogState(ScriptLocalization.Get("Lobby/SLSWaitForPlayerSelecting").Replace("{[PLAYER_NAME]}", text));
					return;
				}
				else
				{
					if (PlayerInfo.me.IsAdmin)
					{
						this.gameRoomMessageLog.LogState(ScriptLocalization.Get("Lobby/SLSAllReadySoStartGame"));
						return;
					}
					this.gameRoomMessageLog.LogState(ScriptLocalization.Get("Lobby/SLSWaitForAdmin"));
				}
			}
			return;
		}
		if (this.GetNumberOfSlots(GameRoomSlotState.GameRoomSlotStateType.PLAYER) == 1)
		{
			this.gameRoomMessageLog.LogState(ScriptLocalization.Get("Lobby/SLSWaitForAtLeastTwoPlayers"));
			return;
		}
		if (PlayerInfo.me.IsAdmin)
		{
			if (!this.AllPlayersReady())
			{
				if (this.lobbyGame.AllRandom)
				{
					this.gameRoomMessageLog.LogState(ScriptLocalization.Get("Lobby/SLSWaitForReadyThenStartGame"));
					return;
				}
				this.gameRoomMessageLog.LogState(ScriptLocalization.Get("Lobby/SLSWaitForReadyThenSelectMats"));
				return;
			}
			else
			{
				if (this.lobbyGame.AllRandom)
				{
					this.gameRoomMessageLog.LogState(ScriptLocalization.Get("Lobby/SLSAllReadySoStartGame"));
					return;
				}
				this.gameRoomMessageLog.LogState(ScriptLocalization.Get("Lobby/SLSAllReadySoSelectMats"));
				return;
			}
		}
		else
		{
			if (!PlayerInfo.me.IsReady)
			{
				this.gameRoomMessageLog.LogState(ScriptLocalization.Get("Lobby/SLSTapReady"));
				return;
			}
			if (!this.AllPlayersReady())
			{
				this.gameRoomMessageLog.LogState(ScriptLocalization.Get("Lobby/SLSWaitForOtherPlayers"));
				return;
			}
			this.gameRoomMessageLog.LogState(ScriptLocalization.Get("Lobby/SLSWaitForAdmin"));
			return;
		}
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00058490 File Offset: 0x00056690
	private int GetNumberOfSlots(GameRoomSlotState.GameRoomSlotStateType slotStateType)
	{
		int num = 0;
		for (int i = 0; i < this.gameRoomSlots.Count; i++)
		{
			if (this.gameRoomSlots[i].CurrentGameRoomSlotStateType == slotStateType)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000146 RID: 326 RVA: 0x000584D0 File Offset: 0x000566D0
	private int GetNumberOfSlotsWithPlayerData()
	{
		int num = 0;
		for (int i = 0; i < this.gameRoomSlots.Count; i++)
		{
			if (this.gameRoomSlots[i].PlayerData != null)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00058510 File Offset: 0x00056710
	private bool AllPlayersReady()
	{
		for (int i = 0; i < this.gameRoomSlots.Count; i++)
		{
			if (this.gameRoomSlots[i].CurrentGameRoomSlotStateType == GameRoomSlotState.GameRoomSlotStateType.PLAYER && !this.gameRoomSlots[i].PlayerData.IsReady)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00058564 File Offset: 0x00056764
	private void EndChoosing()
	{
		this.factionsAndMatsSelected = true;
		this.selectFactionAndMatPanel.SetActiveTimer(false);
		this.selectFactionAndMatPanel.SetFactionInteractable(false);
		this.selectFactionAndMatPanel.SetMatInteractable(false);
		for (int i = 0; i < this.gameRoomSlots.Count; i++)
		{
			if (this.gameRoomSlots[i].CurrentGameRoomSlotStateType == GameRoomSlotState.GameRoomSlotStateType.SELECTING)
			{
				SelectFactionAndMatGameRoomSlotState selectFactionAndMatGameRoomSlotState = (SelectFactionAndMatGameRoomSlotState)this.gameRoomSlots[i].CurrentGameRoomSlotState;
				selectFactionAndMatGameRoomSlotState.SetActiveGoldFrame(false);
				if (this.gameRoomSlots[i].PlayerData != null && this.gameRoomSlots[i].PlayerData.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id)
				{
					this.selectFactionAndMatPanel.SetPlayerName(this.gameRoomSlots[i].PlayerData.PlayerStats.Name);
					this.selectFactionAndMatPanel.SetPlayerType(GameRoomFactionAndMatSelectionCarousel.PlayerType.Player);
					this.selectFactionAndMatPanel.SetFaction(selectFactionAndMatGameRoomSlotState.SelectedFactionID);
					this.selectFactionAndMatPanel.SetMat(selectFactionAndMatGameRoomSlotState.SelectedMatID);
				}
			}
		}
	}

	// Token: 0x06000149 RID: 329 RVA: 0x00058688 File Offset: 0x00056888
	private InvitationGameRoomSlotState SetInvitationStateOnFirstEmptySlot()
	{
		for (int i = 0; i < this.gameRoomSlots.Count; i++)
		{
			if (this.gameRoomSlots[i].CurrentGameRoomSlotStateType == GameRoomSlotState.GameRoomSlotStateType.INVITATION)
			{
				this.gameRoomSlots[i].SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.EMPTY);
			}
		}
		InvitationGameRoomSlotState invitationGameRoomSlotState = null;
		for (int j = 0; j < this.gameRoomSlots.Count; j++)
		{
			if (this.gameRoomSlots[j].CurrentGameRoomSlotStateType == GameRoomSlotState.GameRoomSlotStateType.EMPTY)
			{
				this.gameRoomSlots[j].SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.INVITATION);
				invitationGameRoomSlotState = (InvitationGameRoomSlotState)this.gameRoomSlots[j].CurrentGameRoomSlotState;
				break;
			}
		}
		if (invitationGameRoomSlotState != null)
		{
			if (this.GetNumberOfSlots(GameRoomSlotState.GameRoomSlotStateType.EMPTY) == 0 && this.GetNumberOfSlots(GameRoomSlotState.GameRoomSlotStateType.PLAYER) == 1)
			{
				invitationGameRoomSlotState.SetInteractableAddBotButton(false);
			}
			else
			{
				invitationGameRoomSlotState.SetInteractableAddBotButton(true);
			}
		}
		return invitationGameRoomSlotState;
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00028C26 File Offset: 0x00026E26
	private void ChoosingFactionAndMatRequest(int slotID, int factionID, int matID)
	{
		LobbyRestAPI.MatsSelected(new MatAndFactionChoosen(factionID, matID, slotID), new Action<string>(this.SelectFactionAndMat_OnSuccess), new Action<Exception>(this.SelectFactionAndMat_OnFailure));
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00058754 File Offset: 0x00056954
	private void RegisterMatAndFactionCarouselEvents()
	{
		Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej RegisterMatAndFactionCarouselEvents");
		this.debugEventCount++;
		if (this.debugEventCount > 1)
		{
			Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej Doubled events!");
		}
		this.selectFactionAndMatPanel.OnFactionChangeNext += this.SelectFactionAndMatPanel_OnFactionChangeNext;
		this.selectFactionAndMatPanel.OnFactionChangePrevious += this.SelectFactionAndMatPanel_OnFactionChangePrevious;
		this.selectFactionAndMatPanel.OnMatChangeNext += this.SelectFactionAndMatPanel_OnMatChangeNext;
		this.selectFactionAndMatPanel.OnMatChangePrevious += this.SelectFactionAndMatPanel_OnMatChangePrevious;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00058818 File Offset: 0x00056A18
	private void DeregisterMatAndFactionCarouselEvents()
	{
		Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej DeregisterMatAndFactionCarouselEvents");
		this.debugEventCount--;
		this.selectFactionAndMatPanel.OnFactionChangeNext -= this.SelectFactionAndMatPanel_OnFactionChangeNext;
		this.selectFactionAndMatPanel.OnFactionChangePrevious -= this.SelectFactionAndMatPanel_OnFactionChangePrevious;
		this.selectFactionAndMatPanel.OnMatChangeNext -= this.SelectFactionAndMatPanel_OnMatChangeNext;
		this.selectFactionAndMatPanel.OnMatChangePrevious -= this.SelectFactionAndMatPanel_OnMatChangePrevious;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x00028C4D File Offset: 0x00026E4D
	private void GameRoomSlot_OnInvitePlayerButtonClick()
	{
		if (this.inviteBuddiesPanel)
		{
			this.inviteBuddiesPanel.Show();
		}
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00028C67 File Offset: 0x00026E67
	private void GameRoomSlot_OnKickPlayerButtonClick()
	{
		this.LockButtons();
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00028C67 File Offset: 0x00026E67
	private void GameRoomSlot_OnAddBotButtonClick(GameRoomSlot gameRoomSlot)
	{
		this.LockButtons();
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00028C67 File Offset: 0x00026E67
	private void GameRoomSlot_OnKickBotButtonClick(GameRoomSlot gameRoomSlot)
	{
		this.LockButtons();
	}

	// Token: 0x06000151 RID: 337 RVA: 0x00028C67 File Offset: 0x00026E67
	private void GameRoomSlot_OnBotLevelButtonClick(GameRoomSlot gameRoomSlot)
	{
		this.LockButtons();
	}

	// Token: 0x06000152 RID: 338 RVA: 0x000588B0 File Offset: 0x00056AB0
	private void GameRoomSlot_OnAddBotSuccess(Bot bot)
	{
		if (bot != null)
		{
			this.lobbyGame.BotsList.Add(bot);
			LobbyGame lobbyGame = this.lobbyGame;
			int players = lobbyGame.Players;
			lobbyGame.Players = players + 1;
			this.gamePropertiesPanel.Init(this.lobbyGame);
			this.gameRoomMessageLog.LogMessage(ScriptLocalization.Get("Lobby/SLSystem"), ScriptLocalization.Get("Lobby/SLAdminAddedBot").Replace("{[BOT_NAME]}", bot.Name));
			this.SetInvitationStateOnFirstEmptySlot();
			this.UnlockButtons();
			this.RefreshButtons();
			this.RefreshStatus();
			return;
		}
		Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej GameRoomSlot_OnAddBotSuccess(Bot) ERROR: Bot is null!");
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00028C6F File Offset: 0x00026E6F
	private void GameRoomSlot_OnAddBotFailure()
	{
		this.SetInvitationStateOnFirstEmptySlot();
		this.UnlockButtons();
		this.RefreshButtons();
		this.RefreshStatus();
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00058964 File Offset: 0x00056B64
	private void GameRoomSlot_OnKickBotSuccess(Bot bot)
	{
		this.lobbyGame.BotsList.Remove(bot);
		LobbyGame lobbyGame = this.lobbyGame;
		int players = lobbyGame.Players;
		lobbyGame.Players = players - 1;
		this.gamePropertiesPanel.Init(this.lobbyGame);
		this.gameRoomMessageLog.LogMessage(ScriptLocalization.Get("Lobby/SLSystem"), ScriptLocalization.Get("Lobby/SLAdminRemovedBot").Replace("{[BOT_NAME]}", bot.Name));
		this.SetInvitationStateOnFirstEmptySlot();
		this.UnlockButtons();
		this.RefreshButtons();
		this.RefreshStatus();
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00028C6F File Offset: 0x00026E6F
	private void GameRoomSlot_OnKickBotFailure(Bot bot)
	{
		this.SetInvitationStateOnFirstEmptySlot();
		this.UnlockButtons();
		this.RefreshButtons();
		this.RefreshStatus();
	}

	// Token: 0x06000156 RID: 342 RVA: 0x000589F4 File Offset: 0x00056BF4
	private void GameRoomSlot_OnKickPlayerSuccess(PlayerInfo playerInfo)
	{
		this.gameRoomMessageLog.LogMessage(ScriptLocalization.Get("Lobby/SLSystem"), ScriptLocalization.Get("Lobby/SLAdminKickedPlayer").Replace("{[PLAYER_NAME]}", playerInfo.PlayerStats.Name));
		this.SetInvitationStateOnFirstEmptySlot();
		this.UnlockButtons();
		this.RefreshButtons();
		this.RefreshStatus();
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00028C6F File Offset: 0x00026E6F
	private void GameRoomSlot_OnKickPlayerFailure(PlayerInfo playerInfo)
	{
		this.SetInvitationStateOnFirstEmptySlot();
		this.UnlockButtons();
		this.RefreshButtons();
		this.RefreshStatus();
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00028C8A File Offset: 0x00026E8A
	private void GameRoomSlot_OnUpdateBotSuccess(Bot bot)
	{
		this.UnlockButtons();
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00028C8A File Offset: 0x00026E8A
	private void GameRoomSlot_OnUpdateBotFailure(Bot bot)
	{
		this.UnlockButtons();
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00027EF0 File Offset: 0x000260F0
	private void StartGame_OnSuccess(string message)
	{
	}

	// Token: 0x0600015B RID: 347 RVA: 0x00028C8A File Offset: 0x00026E8A
	private void StartGame_OnFailure(Exception exception)
	{
		this.UnlockButtons();
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00027EF0 File Offset: 0x000260F0
	private void PlayerReady_OnSuccess(string message)
	{
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00058A50 File Offset: 0x00056C50
	private void PlayerReady_OnFailure(Exception exception)
	{
		bool flag = !PlayerInfo.me.IsReady;
		this.SetReadyPlayer(PlayerInfo.me.PlayerStats.Id, flag);
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00028C92 File Offset: 0x00026E92
	private void StartSelectingMats_OnSuccess(string message)
	{
		this.SetSlotsOrder(this.startingOrder.OldSlots, this.startingOrder.NewSlots, this.startingOrder.Ids);
		this.SetGameRoomState(GameRoomMobile.GameRoomStateType.SELECT_FACTIONS_AND_MAT);
		this.StartChoosing();
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00027EF0 File Offset: 0x000260F0
	private void StartSelectingMats_OnFailure(Exception exception)
	{
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00058A84 File Offset: 0x00056C84
	private void MatChoiceTimer_OnTimePassed()
	{
		int currentSlot = this.matAndFactionChoose.GetCurrentSlot();
		if (this.gameRoomSlots[currentSlot].PlayerData == null)
		{
			if (this.gameRoomSlots[currentSlot].BotData != null)
			{
				int num = this.matAndFactionChoose.GetCurrentFaction();
				if (num == 7)
				{
					this.matAndFactionChoose.SetRandomFaction();
					num = this.matAndFactionChoose.GetCurrentFaction();
				}
				int num2 = this.matAndFactionChoose.GetCurrentPlayerMat();
				if (num2 == 7)
				{
					this.matAndFactionChoose.SetRandomMat();
					num2 = this.matAndFactionChoose.GetCurrentPlayerMat();
				}
				this.ChoosingFactionAndMatRequest(currentSlot, num, num2);
			}
			return;
		}
		if (this.gameRoomSlots[currentSlot].PlayerData.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id)
		{
			this.selectModeReadyButton.interactable = false;
			this.selectFactionAndMatPanel.SetFactionInteractable(false);
			this.selectFactionAndMatPanel.SetMatInteractable(false);
			int num3 = this.matAndFactionChoose.GetCurrentFaction();
			if (num3 == 7)
			{
				this.matAndFactionChoose.SetRandomFaction();
				num3 = this.matAndFactionChoose.GetCurrentFaction();
			}
			int num4 = this.matAndFactionChoose.GetCurrentPlayerMat();
			if (num4 == 7)
			{
				this.matAndFactionChoose.SetRandomMat();
				num4 = this.matAndFactionChoose.GetCurrentPlayerMat();
			}
			this.ChoosingFactionAndMatRequest(currentSlot, num3, num4);
			return;
		}
		this.matChoiceTimer.ActivateAdditionalTimerForEnemy();
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00058BDC File Offset: 0x00056DDC
	private void MatChoiceTimer_OnAdditionalTimePassed()
	{
		int currentSlot = this.matAndFactionChoose.GetCurrentSlot();
		int num = this.matAndFactionChoose.GetCurrentFaction();
		if (num == 7)
		{
			this.matAndFactionChoose.SetRandomFaction();
			num = this.matAndFactionChoose.GetCurrentFaction();
		}
		int num2 = this.matAndFactionChoose.GetCurrentPlayerMat();
		if (num2 == 7)
		{
			this.matAndFactionChoose.SetRandomMat();
			num2 = this.matAndFactionChoose.GetCurrentPlayerMat();
		}
		this.ChoosingFactionAndMatRequest(currentSlot, num, num2);
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00058C4C File Offset: 0x00056E4C
	private void MatChoiceTimer_OnTimeChanged(int timeLeft)
	{
		if (this.matAndFactionChoose != null)
		{
			int currentSlot = this.matAndFactionChoose.GetCurrentSlot();
			((SelectFactionAndMatGameRoomSlotState)this.gameRoomSlots[currentSlot].CurrentGameRoomSlotState).SetValueTimer(timeLeft);
			this.selectFactionAndMatPanel.SetValueTimer(timeLeft);
			if (this.autoselectFactionAndMatForBot && timeLeft <= 57 && this.gameRoomSlots[currentSlot].BotData != null)
			{
				this.matChoiceTimer.Deactivate();
				this.MatChoiceTimer_OnTimePassed();
			}
		}
	}

	// Token: 0x06000163 RID: 355 RVA: 0x00028CC8 File Offset: 0x00026EC8
	private void SelectFactionAndMat_OnSuccess(string message)
	{
		this.selectModeReadyButtonCheckmarkImage.gameObject.SetActive(true);
	}

	// Token: 0x06000164 RID: 356 RVA: 0x00027EF0 File Offset: 0x000260F0
	private void SelectFactionAndMat_OnFailure(Exception exception)
	{
	}

	// Token: 0x06000165 RID: 357 RVA: 0x00058CC8 File Offset: 0x00056EC8
	private void SelectFactionAndMatPanel_OnFactionChangeNext()
	{
		if (this.matAndFactionChoose != null)
		{
			this.matAndFactionChoose.MoveToNextFaction();
			int num = this.matAndFactionChoose.GetCurrentFaction();
			if (num == 7)
			{
				num = -1;
			}
			this.selectFactionAndMatPanel.SetFaction(num);
		}
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00058D08 File Offset: 0x00056F08
	private void SelectFactionAndMatPanel_OnFactionChangePrevious()
	{
		if (this.matAndFactionChoose != null)
		{
			this.matAndFactionChoose.MoveToPreviousFaction();
			int num = this.matAndFactionChoose.GetCurrentFaction();
			if (num == 7)
			{
				num = -1;
			}
			this.selectFactionAndMatPanel.SetFaction(num);
		}
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00058D48 File Offset: 0x00056F48
	private void SelectFactionAndMatPanel_OnMatChangeNext()
	{
		if (this.matAndFactionChoose != null)
		{
			this.matAndFactionChoose.MoveToNextMat();
			int num = this.matAndFactionChoose.GetCurrentPlayerMat();
			if (num == 7)
			{
				num = -1;
			}
			this.selectFactionAndMatPanel.SetMat(num);
		}
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00058D88 File Offset: 0x00056F88
	private void SelectFactionAndMatPanel_OnMatChangePrevious()
	{
		if (this.matAndFactionChoose != null)
		{
			this.matAndFactionChoose.MoveToPreviousMat();
			int num = this.matAndFactionChoose.GetCurrentPlayerMat();
			if (num == 7)
			{
				num = -1;
			}
			this.selectFactionAndMatPanel.SetMat(num);
		}
	}

	// Token: 0x0400011C RID: 284
	[SerializeField]
	private Lobby lobby;

	// Token: 0x0400011D RID: 285
	[SerializeField]
	private GameObject gameRoomSlotPrefab;

	// Token: 0x0400011E RID: 286
	[SerializeField]
	private Transform gameRoomSlotsParent;

	// Token: 0x0400011F RID: 287
	public List<GameRoomSlot> gameRoomSlots;

	// Token: 0x04000120 RID: 288
	[SerializeField]
	private CanvasGroup gameRoomSlotsCanvasGroup;

	// Token: 0x04000121 RID: 289
	[SerializeField]
	private Button editGameButton;

	// Token: 0x04000122 RID: 290
	[SerializeField]
	private Button selectMatsButton;

	// Token: 0x04000123 RID: 291
	[SerializeField]
	private Button addModeReadyButton;

	// Token: 0x04000124 RID: 292
	[SerializeField]
	private Image addModeReadyButtonCheckmarkImage;

	// Token: 0x04000125 RID: 293
	[SerializeField]
	private Button selectModeReadyButton;

	// Token: 0x04000126 RID: 294
	[SerializeField]
	private Image selectModeReadyButtonCheckmarkImage;

	// Token: 0x04000127 RID: 295
	[SerializeField]
	private Button startButton;

	// Token: 0x04000128 RID: 296
	[SerializeField]
	private CanvasGroup gameRoomButtonsCanvasGroup;

	// Token: 0x04000129 RID: 297
	[SerializeField]
	private GamePropertiesPanel gamePropertiesPanel;

	// Token: 0x0400012A RID: 298
	[SerializeField]
	private GameRoomFactionAndMatSelectionCarousel selectFactionAndMatPanel;

	// Token: 0x0400012B RID: 299
	[SerializeField]
	private GameRoomMessageLog gameRoomMessageLog;

	// Token: 0x0400012C RID: 300
	[SerializeField]
	private bool autoselectFactionAndMatForBot = true;

	// Token: 0x0400012D RID: 301
	private LobbyGame lobbyGame;

	// Token: 0x0400012E RID: 302
	public InviteBuddiesPanelMobile inviteBuddiesPanel;

	// Token: 0x0400012F RID: 303
	public MatAndFactionChoose matAndFactionChoose;

	// Token: 0x04000130 RID: 304
	public MatChoiceTimer matChoiceTimer;

	// Token: 0x04000131 RID: 305
	private bool factionsAndMatsSelected;

	// Token: 0x04000132 RID: 306
	private StartingOrder startingOrder;

	// Token: 0x04000135 RID: 309
	private int debugEventCount;

	// Token: 0x02000033 RID: 51
	public enum GameRoomStateType
	{
		// Token: 0x04000137 RID: 311
		ADD_PLAYER,
		// Token: 0x04000138 RID: 312
		SELECT_FACTIONS_AND_MAT
	}
}

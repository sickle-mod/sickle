using System;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class GameRoomSlot : MonoBehaviour
{
	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000171 RID: 369 RVA: 0x00058DC8 File Offset: 0x00056FC8
	// (remove) Token: 0x06000172 RID: 370 RVA: 0x00058E00 File Offset: 0x00057000
	public event global::System.Action OnInvitePlayerButtonClick;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000173 RID: 371 RVA: 0x00058E38 File Offset: 0x00057038
	// (remove) Token: 0x06000174 RID: 372 RVA: 0x00058E70 File Offset: 0x00057070
	public event global::System.Action OnKickPlayerButtonClick;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000175 RID: 373 RVA: 0x00058EA8 File Offset: 0x000570A8
	// (remove) Token: 0x06000176 RID: 374 RVA: 0x00058EE0 File Offset: 0x000570E0
	public event Action<GameRoomSlot> OnAddBotButtonClick;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000177 RID: 375 RVA: 0x00058F18 File Offset: 0x00057118
	// (remove) Token: 0x06000178 RID: 376 RVA: 0x00058F50 File Offset: 0x00057150
	public event Action<GameRoomSlot> OnKickBotButtonClick;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000179 RID: 377 RVA: 0x00058F88 File Offset: 0x00057188
	// (remove) Token: 0x0600017A RID: 378 RVA: 0x00058FC0 File Offset: 0x000571C0
	public event Action<GameRoomSlot> OnBotLevelButtonClick;

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x0600017B RID: 379 RVA: 0x00058FF8 File Offset: 0x000571F8
	// (remove) Token: 0x0600017C RID: 380 RVA: 0x00059030 File Offset: 0x00057230
	public event Action<PlayerInfo> OnKickPlayerSuccess;

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x0600017D RID: 381 RVA: 0x00059068 File Offset: 0x00057268
	// (remove) Token: 0x0600017E RID: 382 RVA: 0x000590A0 File Offset: 0x000572A0
	public event Action<PlayerInfo> OnKickPlayerFailure;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x0600017F RID: 383 RVA: 0x000590D8 File Offset: 0x000572D8
	// (remove) Token: 0x06000180 RID: 384 RVA: 0x00059110 File Offset: 0x00057310
	public event Action<Bot> OnAddBotSuccess;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06000181 RID: 385 RVA: 0x00059148 File Offset: 0x00057348
	// (remove) Token: 0x06000182 RID: 386 RVA: 0x00059180 File Offset: 0x00057380
	public event global::System.Action OnAddBotFailure;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06000183 RID: 387 RVA: 0x000591B8 File Offset: 0x000573B8
	// (remove) Token: 0x06000184 RID: 388 RVA: 0x000591F0 File Offset: 0x000573F0
	public event Action<Bot> OnKickBotSuccess;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06000185 RID: 389 RVA: 0x00059228 File Offset: 0x00057428
	// (remove) Token: 0x06000186 RID: 390 RVA: 0x00059260 File Offset: 0x00057460
	public event Action<Bot> OnKickBotFailure;

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06000187 RID: 391 RVA: 0x00059298 File Offset: 0x00057498
	// (remove) Token: 0x06000188 RID: 392 RVA: 0x000592D0 File Offset: 0x000574D0
	public event Action<Bot> OnUpdateBotSuccess;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06000189 RID: 393 RVA: 0x00059308 File Offset: 0x00057508
	// (remove) Token: 0x0600018A RID: 394 RVA: 0x00059340 File Offset: 0x00057540
	public event Action<Bot> OnUpdateBotFailure;

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x0600018B RID: 395 RVA: 0x00028D3F File Offset: 0x00026F3F
	public int SlotID
	{
		get
		{
			return base.transform.GetSiblingIndex();
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x0600018C RID: 396 RVA: 0x00028D4C File Offset: 0x00026F4C
	// (set) Token: 0x0600018D RID: 397 RVA: 0x00028D54 File Offset: 0x00026F54
	public PlayerInfo PlayerData
	{
		get
		{
			return this.playerData;
		}
		set
		{
			this.SetPlayerData(value);
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x0600018E RID: 398 RVA: 0x00028D5D File Offset: 0x00026F5D
	// (set) Token: 0x0600018F RID: 399 RVA: 0x00028D65 File Offset: 0x00026F65
	public Bot BotData
	{
		get
		{
			return this.botData;
		}
		set
		{
			this.SetBotData(value);
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000190 RID: 400 RVA: 0x00028D6E File Offset: 0x00026F6E
	// (set) Token: 0x06000191 RID: 401 RVA: 0x00028D76 File Offset: 0x00026F76
	public GameRoomSlotState.GameRoomSlotStateType CurrentGameRoomSlotStateType { get; private set; }

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000192 RID: 402 RVA: 0x00028D7F File Offset: 0x00026F7F
	public GameRoomSlotState CurrentGameRoomSlotState
	{
		get
		{
			return this.GetSlotState(this.CurrentGameRoomSlotStateType);
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000193 RID: 403 RVA: 0x00028D8D File Offset: 0x00026F8D
	public bool IsReady
	{
		get
		{
			return this.PlayerData == null || this.PlayerData.IsReady;
		}
	}

	// Token: 0x06000194 RID: 404 RVA: 0x00059378 File Offset: 0x00057578
	public void Reset()
	{
		this.SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.EMPTY);
		this.PlayerData = null;
		this.BotData = null;
		this.OnInvitePlayerButtonClick = null;
		this.OnKickPlayerButtonClick = null;
		this.OnAddBotButtonClick = null;
		this.OnKickBotButtonClick = null;
		this.OnBotLevelButtonClick = null;
		this.OnKickPlayerSuccess = null;
		this.OnKickPlayerFailure = null;
		this.OnAddBotSuccess = null;
		this.OnAddBotFailure = null;
		this.OnKickBotSuccess = null;
		this.OnKickBotFailure = null;
		this.OnUpdateBotSuccess = null;
		this.OnUpdateBotFailure = null;
		this.botUpdateLevelPrevValue = -1;
		this.botUpdateLevelNewValue = -1;
	}

	// Token: 0x06000195 RID: 405 RVA: 0x00059404 File Offset: 0x00057604
	private void OnEnable()
	{
		this.invitationStatePanel.OnAddBotButtonClick += this.InvitationStatePanel_OnAddBotButtonClick;
		this.botStatePanel.OnKickBotButtonClick += this.BotStatePanel_OnKickBotButtonClick;
		this.botStatePanel.OnBotLevelButtonClick += this.BotStatePanel_OnBotLevelButtonClick;
		this.invitationStatePanel.OnInvitePlayerButtonClick += this.InvitationStatePanel_OnInvitePlayerButtonClick;
		this.playerStatePanel.OnKickPlayerButtonClick += this.PlayerStatePanel_OnKickPlayerButtonClick;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00059484 File Offset: 0x00057684
	private void OnDisable()
	{
		this.invitationStatePanel.OnAddBotButtonClick -= this.InvitationStatePanel_OnAddBotButtonClick;
		this.botStatePanel.OnKickBotButtonClick -= this.BotStatePanel_OnKickBotButtonClick;
		this.botStatePanel.OnBotLevelButtonClick -= this.BotStatePanel_OnBotLevelButtonClick;
		this.invitationStatePanel.OnInvitePlayerButtonClick -= this.InvitationStatePanel_OnInvitePlayerButtonClick;
		this.playerStatePanel.OnKickPlayerButtonClick -= this.PlayerStatePanel_OnKickPlayerButtonClick;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x00059504 File Offset: 0x00057704
	public GameRoomSlotState GetSlotState(GameRoomSlotState.GameRoomSlotStateType slotStateType)
	{
		switch (slotStateType)
		{
		case GameRoomSlotState.GameRoomSlotStateType.EMPTY:
			return this.emptyStatePanel;
		case GameRoomSlotState.GameRoomSlotStateType.PLAYER:
			return this.playerStatePanel;
		case GameRoomSlotState.GameRoomSlotStateType.BOT:
			return this.botStatePanel;
		case GameRoomSlotState.GameRoomSlotStateType.INVITATION:
			return this.invitationStatePanel;
		case GameRoomSlotState.GameRoomSlotStateType.SELECTING:
			return this.selectFactionAndMatGameRoomSlotState;
		default:
			return null;
		}
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00059554 File Offset: 0x00057754
	public void SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType slotStateType)
	{
		this.CurrentGameRoomSlotStateType = slotStateType;
		this.emptyStatePanel.gameObject.SetActive(slotStateType == GameRoomSlotState.GameRoomSlotStateType.EMPTY);
		this.playerStatePanel.gameObject.SetActive(slotStateType == GameRoomSlotState.GameRoomSlotStateType.PLAYER);
		this.botStatePanel.gameObject.SetActive(slotStateType == GameRoomSlotState.GameRoomSlotStateType.BOT);
		this.invitationStatePanel.gameObject.SetActive(slotStateType == GameRoomSlotState.GameRoomSlotStateType.INVITATION);
		this.selectFactionAndMatGameRoomSlotState.gameObject.SetActive(slotStateType == GameRoomSlotState.GameRoomSlotStateType.SELECTING);
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00028DA4 File Offset: 0x00026FA4
	public void LockSlot()
	{
		this.canvasGroup.interactable = false;
	}

	// Token: 0x0600019A RID: 410 RVA: 0x00028DB2 File Offset: 0x00026FB2
	public void UnlockSlot()
	{
		this.canvasGroup.interactable = true;
	}

	// Token: 0x0600019B RID: 411 RVA: 0x00028DC0 File Offset: 0x00026FC0
	private void InvitationStatePanel_OnInvitePlayerButtonClick()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		UniversalInvocator.Event_Invocator(this.OnInvitePlayerButtonClick);
	}

	// Token: 0x0600019C RID: 412 RVA: 0x000595CC File Offset: 0x000577CC
	private void InvitationStatePanel_OnAddBotButtonClick()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		string text = HotseatPanel.GenerateAIPlayerName(this.SlotID, 1);
		this.BotData = new Bot(this.SlotID, 1, text);
		this.SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.BOT);
		this.LockSlot();
		LobbyRestAPI.AddBot(this.BotData, new Action<string>(this.AddBotResponseSuccess), new Action<Exception>(this.AddBotResponseError));
		UniversalInvocator.Event_Invocator<GameRoomSlot>(this.OnAddBotButtonClick, new object[] { this });
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0005964C File Offset: 0x0005784C
	private void AddBotResponseSuccess(string message)
	{
		Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej SUCCESS AddBotResponseSuccess: " + message);
		this.botStatePanel.SetActiveKickBotButton(PlayerInfo.me.IsAdmin);
		this.botStatePanel.SetInteractableBotLevelButton(PlayerInfo.me.IsAdmin);
		this.UnlockSlot();
		UniversalInvocator.Event_Invocator<Bot>(this.OnAddBotSuccess, new object[] { this.BotData });
	}

	// Token: 0x0600019E RID: 414 RVA: 0x000596C8 File Offset: 0x000578C8
	private void AddBotResponseError(Exception exception)
	{
		Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej ERROR AddBotResponseError: " + ((exception != null) ? exception.ToString() : null));
		this.BotData = null;
		if (this.PlayerData != null)
		{
			this.SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.PLAYER);
		}
		else
		{
			this.SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.EMPTY);
		}
		this.UnlockSlot();
		UniversalInvocator.Event_Invocator(this.OnAddBotFailure);
	}

	// Token: 0x0600019F RID: 415 RVA: 0x00059734 File Offset: 0x00057934
	private void PlayerStatePanel_OnKickPlayerButtonClick()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		this.SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.EMPTY);
		this.LockSlot();
		LobbyRestAPI.RemovePlayer(this.PlayerData.PlayerStats.Id, new Action<string>(this.KickPlayerResponseSuccess), new Action<Exception>(this.KickPlayerResponseError));
		UniversalInvocator.Event_Invocator(this.OnKickPlayerButtonClick);
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x00059794 File Offset: 0x00057994
	private void KickPlayerResponseSuccess(string message)
	{
		Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej SUCCESS KickPlayerResponseSuccess: " + message);
		PlayerInfo playerInfo = this.PlayerData;
		this.PlayerData = null;
		this.UnlockSlot();
		UniversalInvocator.Event_Invocator<PlayerInfo>(this.OnKickPlayerSuccess, new object[] { playerInfo });
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x000597EC File Offset: 0x000579EC
	private void KickPlayerResponseError(Exception exception)
	{
		Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej ERROR KickPlayerResponseError: " + ((exception != null) ? exception.ToString() : null));
		this.SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.PLAYER);
		this.UnlockSlot();
		UniversalInvocator.Event_Invocator<PlayerInfo>(this.OnKickPlayerFailure, new object[] { this.PlayerData });
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00059850 File Offset: 0x00057A50
	private void BotStatePanel_OnKickBotButtonClick()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		this.SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.EMPTY);
		this.LockSlot();
		LobbyRestAPI.RemoveBot(this.botData, new Action<string>(this.KickBotResponseSuccess), new Action<Exception>(this.KickBotResponseError));
		UniversalInvocator.Event_Invocator<GameRoomSlot>(this.OnKickBotButtonClick, new object[] { this });
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x000598B0 File Offset: 0x00057AB0
	private void KickBotResponseSuccess(string message)
	{
		Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej SUCCESS KickBotResponseSuccess: " + message);
		Bot bot = this.BotData;
		this.BotData = null;
		this.UnlockSlot();
		UniversalInvocator.Event_Invocator<Bot>(this.OnKickBotSuccess, new object[] { bot });
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00059908 File Offset: 0x00057B08
	private void KickBotResponseError(Exception exception)
	{
		Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej ERROR KickBotResponseError: " + ((exception != null) ? exception.ToString() : null));
		this.SetCurrentSlotState(GameRoomSlotState.GameRoomSlotStateType.BOT);
		this.UnlockSlot();
		UniversalInvocator.Event_Invocator<Bot>(this.OnKickBotFailure, new object[] { this.BotData });
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0005996C File Offset: 0x00057B6C
	private void BotStatePanel_OnBotLevelButtonClick(int prevValue, int newValue)
	{
		this.botUpdateLevelPrevValue = prevValue;
		this.botUpdateLevelNewValue = newValue;
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		this.BotData.Name = HotseatPanel.GenerateAIPlayerName(this.SlotID, newValue);
		this.botStatePanel.SetBotName(this.BotData.Name);
		this.selectFactionAndMatGameRoomSlotState.SetPlayerName(this.BotData.Name);
		this.BotData.Difficulty = newValue;
		this.botStatePanel.SetBotLevel(this.BotData.Difficulty);
		this.LockSlot();
		LobbyRestAPI.UpdateBot(this.BotData, new Action<string>(this.UpdateBotResponseSuccess), new Action<Exception>(this.UpdateBotResponseError));
		UniversalInvocator.Event_Invocator<GameRoomSlot>(this.OnBotLevelButtonClick, new object[] { this });
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00059A38 File Offset: 0x00057C38
	private void UpdateBotResponseSuccess(string message)
	{
		Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej SUCCESS UpdateBotResponseSuccess: " + message);
		this.botUpdateLevelPrevValue = -1;
		this.botUpdateLevelNewValue = -1;
		this.UnlockSlot();
		UniversalInvocator.Event_Invocator<Bot>(this.OnUpdateBotSuccess, new object[] { this.BotData });
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00059A98 File Offset: 0x00057C98
	private void UpdateBotResponseError(Exception exception)
	{
		Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej ERROR UpdateBotResponseError: " + ((exception != null) ? exception.ToString() : null));
		this.BotData.Name = HotseatPanel.GenerateAIPlayerName(this.SlotID, this.botUpdateLevelPrevValue);
		this.botStatePanel.SetBotName(this.BotData.Name);
		this.selectFactionAndMatGameRoomSlotState.SetPlayerName(this.BotData.Name);
		this.BotData.Difficulty = this.botUpdateLevelPrevValue;
		this.botStatePanel.SetBotLevel(this.BotData.Difficulty);
		this.botUpdateLevelPrevValue = -1;
		this.botUpdateLevelNewValue = -1;
		this.UnlockSlot();
		UniversalInvocator.Event_Invocator<Bot>(this.OnUpdateBotFailure, new object[] { this.BotData });
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00059B70 File Offset: 0x00057D70
	private void SetPlayerData(PlayerInfo playerData)
	{
		this.playerData = playerData;
		if (playerData != null)
		{
			this.playerStatePanel.SetPlayerName(playerData.PlayerStats.Name);
			this.playerStatePanel.SetPlayerELO(playerData.PlayerStats.ELO);
			this.playerStatePanel.SetActivePendingPlate(false);
			this.playerStatePanel.SetActiveReadyPlate(playerData.IsReady);
			this.playerStatePanel.SetActiveKickPlayerButton(PlayerInfo.me.IsAdmin);
			this.playerStatePanel.SetInteractableKickPlayerButton(PlayerInfo.me.IsAdmin);
			this.selectFactionAndMatGameRoomSlotState.Reset();
			this.selectFactionAndMatGameRoomSlotState.SetPlayerName(playerData.PlayerStats.Name);
		}
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00059C20 File Offset: 0x00057E20
	private void SetBotData(Bot botData)
	{
		this.botData = botData;
		if (botData != null)
		{
			this.botStatePanel.SetBotName(botData.Name);
			this.botStatePanel.SetBotLevel(botData.Difficulty);
			this.botStatePanel.SetActivePendingPlate(false);
			this.botStatePanel.SetActiveReadyPlate(false);
			this.selectFactionAndMatGameRoomSlotState.Reset();
			this.selectFactionAndMatGameRoomSlotState.SetPlayerName(botData.Name);
		}
	}

	// Token: 0x0400014A RID: 330
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x0400014B RID: 331
	[SerializeField]
	private EmptyGameRoomSlotState emptyStatePanel;

	// Token: 0x0400014C RID: 332
	[SerializeField]
	private PlayerGameRoomSlotState playerStatePanel;

	// Token: 0x0400014D RID: 333
	[SerializeField]
	private BotGameRoomSlotState botStatePanel;

	// Token: 0x0400014E RID: 334
	[SerializeField]
	private InvitationGameRoomSlotState invitationStatePanel;

	// Token: 0x0400014F RID: 335
	[SerializeField]
	private SelectFactionAndMatGameRoomSlotState selectFactionAndMatGameRoomSlotState;

	// Token: 0x04000150 RID: 336
	private int botUpdateLevelPrevValue = -1;

	// Token: 0x04000151 RID: 337
	private int botUpdateLevelNewValue = -1;

	// Token: 0x04000152 RID: 338
	private PlayerInfo playerData;

	// Token: 0x04000153 RID: 339
	private Bot botData;
}

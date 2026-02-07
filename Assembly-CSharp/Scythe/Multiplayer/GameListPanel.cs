using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x0200023C RID: 572
	public class GameListPanel : MonoBehaviour
	{
		// Token: 0x14000078 RID: 120
		// (add) Token: 0x06001129 RID: 4393 RVA: 0x00092038 File Offset: 0x00090238
		// (remove) Token: 0x0600112A RID: 4394 RVA: 0x00092070 File Offset: 0x00090270
		public event global::System.Action RoomListLoaded;

		// Token: 0x0600112B RID: 4395 RVA: 0x000920A8 File Offset: 0x000902A8
		public virtual void Start()
		{
			this.gameListLogic.GameToSpectateFound += this.OnGameToSpectateFound;
			this.gameListLogic.GamesToSpectateLoaded += this.OnGamesToSpectateLoaded;
			this.gameListLogic.GamesInProgressLoaded += this.OnGamesInProgressLoaded;
			this.gameListLogic.RoomFound += this.OnRoomFound;
			this.gameListLogic.RoomRefreshed += this.OnRoomFound;
			this.gameListLogic.RoomListLoaded += this.OnRoomListLoaded;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00092140 File Offset: 0x00090340
		public virtual void OnDestroy()
		{
			this.gameListLogic.GameToSpectateFound -= this.OnGameToSpectateFound;
			this.gameListLogic.GamesToSpectateLoaded -= this.OnGamesToSpectateLoaded;
			this.gameListLogic.GamesInProgressLoaded -= this.OnGamesInProgressLoaded;
			this.gameListLogic.RoomFound -= this.OnRoomFound;
			this.gameListLogic.RoomRefreshed -= this.OnRoomFound;
			this.gameListLogic.RoomListLoaded -= this.OnRoomListLoaded;
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x00033276 File Offset: 0x00031476
		public virtual void Activate()
		{
			base.gameObject.SetActive(true);
			this.refreshListButton.interactable = true;
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00029172 File Offset: 0x00027372
		public virtual void Deactivate()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x00033290 File Offset: 0x00031490
		public void RefreshButtonClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.RefreshRoomsLists();
			this.refreshListButton.interactable = false;
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x000921D8 File Offset: 0x000903D8
		public virtual void RefreshRoomsLists()
		{
			this.RemoveAllEntries();
			this.gameListLogic.RefreshRoomList();
			this.yourTurnIndicator.SetActive(false);
			this.DisableAllNotFoundTexts();
			if (this.resumeJoinToggle.isOn)
			{
				this.resumeTextAnimation.StartAnimating();
				this.joinTextAnimation.StartAnimating();
				return;
			}
			this.spectateTextAnimation.StartAnimating();
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x000332B0 File Offset: 0x000314B0
		private void DisableAllNotFoundTexts()
		{
			this.noGamesToJoinText.gameObject.SetActive(false);
			this.noGamesToResumeText.gameObject.SetActive(false);
			this.noGamesToSpectate.gameObject.SetActive(false);
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x000332E5 File Offset: 0x000314E5
		private void DisableAllLoadingAnimations()
		{
			this.joinTextAnimation.StopAnimating();
			this.resumeTextAnimation.StopAnimating();
			this.spectateTextAnimation.StopAnimating();
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00033308 File Offset: 0x00031508
		private void RemoveAllEntries()
		{
			this.RemoveResumeGames();
			this.RemoveJoinGames();
			this.RemoveSpectateGames();
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x00092238 File Offset: 0x00090438
		private void RemoveResumeGames()
		{
			foreach (string text in this.resumeGames.Keys)
			{
				global::UnityEngine.Object.Destroy(this.resumeGames[text].gameObject);
			}
			this.resumeGames.Clear();
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x000922AC File Offset: 0x000904AC
		private void RemoveJoinGames()
		{
			foreach (string text in this.joinRooms.Keys)
			{
				global::UnityEngine.Object.Destroy(this.joinRooms[text].gameObject);
			}
			this.joinRooms.Clear();
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00092320 File Offset: 0x00090520
		private void RemoveSpectateGames()
		{
			foreach (string text in this.spectateGames.Keys)
			{
				global::UnityEngine.Object.Destroy(this.spectateGames[text].gameObject);
			}
			this.spectateGames.Clear();
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00092394 File Offset: 0x00090594
		public virtual void SwitchToResumeJoinRooms()
		{
			this.RemoveAllEntries();
			this.resumeText.SetActive(true);
			this.firstDividerImage.SetActive(true);
			this.joinText.SetActive(true);
			this.secondDividerImage.SetActive(true);
			this.DisableAllLoadingAnimations();
			this.DisableAllNotFoundTexts();
			this.RefreshResumeGames();
			this.RefreshJoinGames();
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x000923F0 File Offset: 0x000905F0
		private void RefreshResumeGames()
		{
			AsynchronousGame[] asynchronousGamesInProgress = this.gameListLogic.AsynchronousGamesInProgress;
			if (asynchronousGamesInProgress.Length == 0)
			{
				this.noGamesToResumeText.gameObject.SetActive(true);
				this.resumeGamesContent.gameObject.SetActive(false);
				return;
			}
			this.noGamesToResumeText.gameObject.SetActive(false);
			this.resumeGamesContent.gameObject.SetActive(true);
			foreach (AsynchronousGame asynchronousGame in asynchronousGamesInProgress)
			{
				this.CreateResumeGameEntry(asynchronousGame);
			}
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00092470 File Offset: 0x00090670
		private void RefreshJoinGames()
		{
			Dictionary<string, LobbyRoom>.ValueCollection values = this.gameListLogic.SynchronousGames.Values;
			Dictionary<string, LobbyRoom>.ValueCollection values2 = this.gameListLogic.AsynchronousGames.Values;
			if (values.Count + values2.Count == 0)
			{
				this.noGamesToJoinText.gameObject.SetActive(true);
				this.joinGamesContent.gameObject.SetActive(false);
				return;
			}
			this.noGamesToJoinText.gameObject.SetActive(false);
			this.joinGamesContent.gameObject.SetActive(true);
			foreach (LobbyRoom lobbyRoom in values)
			{
				this.CreateJoinRoomEntry(lobbyRoom);
			}
			foreach (LobbyRoom lobbyRoom2 in values2)
			{
				this.CreateJoinRoomEntry(lobbyRoom2);
			}
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00092574 File Offset: 0x00090774
		private void CreateResumeGameEntry(AsynchronousGame gameData)
		{
			GameListEntry gameListEntry = global::UnityEngine.Object.Instantiate<GameListEntry>(this.gameEntry, this.resumeGamesContent);
			gameListEntry.Init(gameData, this.lobby);
			this.resumeGames.Add(gameData.GameId, gameListEntry);
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0003331C File Offset: 0x0003151C
		public virtual void AddNewRoom(LobbyRoom roomData)
		{
			this.gameListLogic.AddRoom(roomData);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x000925B4 File Offset: 0x000907B4
		public virtual void RemoveRoom(string id)
		{
			this.gameListLogic.RemoveRoom(id);
			if (this.joinRooms.ContainsKey(id))
			{
				global::UnityEngine.Object.Destroy(this.joinRooms[id].gameObject);
				this.joinRooms.Remove(id);
				if (this.GetVisibleJoinEntriesCount() == 0 && this.resumeJoinToggle.isOn)
				{
					this.noGamesToJoinText.gameObject.SetActive(true);
					this.joinGamesContent.gameObject.SetActive(false);
					return;
				}
			}
			else if (this.resumeGames.ContainsKey(id))
			{
				global::UnityEngine.Object.Destroy(this.resumeGames[id].gameObject);
				this.resumeGames.Remove(id);
				if (this.resumeGames.Count == 0 && this.resumeJoinToggle.isOn)
				{
					this.noGamesToResumeText.gameObject.SetActive(true);
					this.resumeGamesContent.gameObject.SetActive(false);
					return;
				}
			}
			else if (this.spectateGames.ContainsKey(id) && this.spectateToggle.isOn)
			{
				global::UnityEngine.Object.Destroy(this.spectateGames[id].gameObject);
				this.spectateGames.Remove(id);
				if (this.spectateGames.Count == 0)
				{
					this.noGamesToSpectate.gameObject.SetActive(true);
					this.spectateGamesContent.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00092720 File Offset: 0x00090920
		private void CreateJoinRoomEntry(LobbyRoom roomData)
		{
			GameListEntry gameListEntry = global::UnityEngine.Object.Instantiate<GameListEntry>(this.gameEntry, this.joinGamesContent);
			gameListEntry.Init(roomData, this.lobby);
			this.joinRooms.Add(roomData.RoomId, gameListEntry);
			this.RefreshJoinEntryVisibility(gameListEntry);
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00092768 File Offset: 0x00090968
		private void OnGamesInProgressLoaded()
		{
			if (this.resumeJoinToggle.isOn)
			{
				this.RemoveResumeGames();
				this.resumeTextAnimation.StopAnimating();
				this.RefreshResumeGames();
			}
			int num = this.gameListLogic.AsynchronousGamesInProgress.Count((AsynchronousGame game) => game.IsPlayerTurn);
			this.yourTurnIndicator.SetActive(num > 0);
			this.numberOfYourTurns.text = num.ToString();
			if (this.gameListLogic.RoomsListsRefreshed())
			{
				this.refreshListButton.interactable = true;
			}
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x00092804 File Offset: 0x00090A04
		private void OnRoomFound(LobbyRoom roomData)
		{
			if (this.resumeJoinToggle.isOn)
			{
				this.joinTextAnimation.StopAnimating();
				if (this.joinRooms.Count == 0)
				{
					this.CreateJoinRoomEntry(roomData);
					bool flag = this.GetVisibleJoinEntriesCount() != 0;
					this.noGamesToJoinText.gameObject.SetActive(!flag);
					this.joinGamesContent.gameObject.SetActive(flag);
					return;
				}
				if (this.joinRooms.ContainsKey(roomData.RoomId))
				{
					this.joinRooms[roomData.RoomId].Init(roomData, this.lobby);
					this.RefreshJoinEntryVisibility(this.joinRooms[roomData.RoomId]);
					return;
				}
				this.CreateJoinRoomEntry(roomData);
			}
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x000928C0 File Offset: 0x00090AC0
		private int GetVisibleJoinEntriesCount()
		{
			int num = 0;
			using (Dictionary<string, GameListEntry>.ValueCollection.Enumerator enumerator = this.joinRooms.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.gameObject.activeSelf)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00031A74 File Offset: 0x0002FC74
		private void RefreshJoinEntryVisibility(GameListEntry joinEntry)
		{
			joinEntry.gameObject.SetActive(joinEntry.IsButtonInteractable());
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00092924 File Offset: 0x00090B24
		private void OnRoomListLoaded(bool more)
		{
			if (more)
			{
				this.gameListLogic.GetRoomList();
				return;
			}
			if (this.resumeJoinToggle.isOn)
			{
				this.joinTextAnimation.StopAnimating();
				if (this.GetVisibleJoinEntriesCount() == 0)
				{
					this.noGamesToJoinText.gameObject.SetActive(true);
				}
			}
			this.TryRaiseRoomListLoadedAction();
			if (this.gameListLogic.RoomsListsRefreshed())
			{
				this.refreshListButton.interactable = true;
			}
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x0003332A File Offset: 0x0003152A
		protected void TryRaiseRoomListLoadedAction()
		{
			if (this.RoomListLoaded != null)
			{
				this.RoomListLoaded();
			}
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00092990 File Offset: 0x00090B90
		public virtual void SwitchToSpectateRooms()
		{
			this.RemoveAllEntries();
			this.resumeText.SetActive(false);
			this.joinText.SetActive(false);
			this.firstDividerImage.SetActive(false);
			this.secondDividerImage.SetActive(false);
			this.resumeGamesContent.gameObject.SetActive(false);
			this.joinGamesContent.gameObject.SetActive(false);
			this.DisableAllNotFoundTexts();
			this.DisableAllLoadingAnimations();
			Dictionary<string, GameToSpectate>.ValueCollection values = this.gameListLogic.GamesToSpectate.Values;
			if (values.Count == 0)
			{
				this.noGamesToSpectate.gameObject.SetActive(true);
				this.spectateGamesContent.gameObject.SetActive(false);
				return;
			}
			this.spectateGamesContent.gameObject.SetActive(true);
			foreach (GameToSpectate gameToSpectate in values)
			{
				this.CreateSpectateGameEntry(gameToSpectate);
			}
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00092A90 File Offset: 0x00090C90
		private void CreateSpectateGameEntry(GameToSpectate gameData)
		{
			GameListEntry gameListEntry = global::UnityEngine.Object.Instantiate<GameListEntry>(this.gameEntry, this.spectateGamesContent);
			gameListEntry.Init(gameData, this.lobby);
			this.spectateGames.Add(gameData.Id, gameListEntry);
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x00092AD0 File Offset: 0x00090CD0
		private void OnGameToSpectateFound(GameToSpectate gameData)
		{
			if (this.spectateToggle.isOn)
			{
				if (this.spectateGames.Count == 0)
				{
					this.noGamesToSpectate.gameObject.SetActive(false);
					this.spectateGamesContent.gameObject.SetActive(true);
					this.CreateSpectateGameEntry(gameData);
					return;
				}
				if (this.spectateGames.ContainsKey(gameData.Id))
				{
					this.spectateGames[gameData.Id].Init(gameData, this.lobby);
					return;
				}
				this.CreateSpectateGameEntry(gameData);
			}
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00092B5C File Offset: 0x00090D5C
		private void OnGamesToSpectateLoaded(bool more)
		{
			if (more)
			{
				this.gameListLogic.GetGamesToSpectate();
				return;
			}
			if (this.spectateToggle.isOn)
			{
				this.spectateTextAnimation.StopAnimating();
				if (this.gameListLogic.GamesToSpectate.Count == 0)
				{
					this.noGamesToSpectate.gameObject.SetActive(true);
				}
			}
			if (this.gameListLogic.RoomsListsRefreshed())
			{
				this.refreshListButton.interactable = true;
			}
		}

		// Token: 0x04000D34 RID: 3380
		[SerializeField]
		private Lobby lobby;

		// Token: 0x04000D35 RID: 3381
		[SerializeField]
		private GameListEntry gameEntry;

		// Token: 0x04000D36 RID: 3382
		[SerializeField]
		private Toggle createToggle;

		// Token: 0x04000D37 RID: 3383
		[SerializeField]
		private Toggle resumeJoinToggle;

		// Token: 0x04000D38 RID: 3384
		[SerializeField]
		private Toggle spectateToggle;

		// Token: 0x04000D39 RID: 3385
		[SerializeField]
		private Button refreshListButton;

		// Token: 0x04000D3A RID: 3386
		[SerializeField]
		private GameObject yourTurnIndicator;

		// Token: 0x04000D3B RID: 3387
		[SerializeField]
		private TextMeshProUGUI numberOfYourTurns;

		// Token: 0x04000D3C RID: 3388
		[SerializeField]
		private Transform spectateGamesContent;

		// Token: 0x04000D3D RID: 3389
		[SerializeField]
		private Transform resumeGamesContent;

		// Token: 0x04000D3E RID: 3390
		[SerializeField]
		private Transform joinGamesContent;

		// Token: 0x04000D3F RID: 3391
		[SerializeField]
		private GameObject resumeText;

		// Token: 0x04000D40 RID: 3392
		[SerializeField]
		private GameObject joinText;

		// Token: 0x04000D41 RID: 3393
		[SerializeField]
		private LoadingTextAnimated spectateTextAnimation;

		// Token: 0x04000D42 RID: 3394
		[SerializeField]
		private LoadingTextAnimated resumeTextAnimation;

		// Token: 0x04000D43 RID: 3395
		[SerializeField]
		private LoadingTextAnimated joinTextAnimation;

		// Token: 0x04000D44 RID: 3396
		[SerializeField]
		private GameObject firstDividerImage;

		// Token: 0x04000D45 RID: 3397
		[SerializeField]
		private GameObject secondDividerImage;

		// Token: 0x04000D46 RID: 3398
		[SerializeField]
		private TextMeshProUGUI noGamesToSpectate;

		// Token: 0x04000D47 RID: 3399
		[SerializeField]
		private TextMeshProUGUI noGamesToJoinText;

		// Token: 0x04000D48 RID: 3400
		[SerializeField]
		private TextMeshProUGUI noGamesToResumeText;

		// Token: 0x04000D49 RID: 3401
		private Dictionary<string, GameListEntry> resumeGames = new Dictionary<string, GameListEntry>();

		// Token: 0x04000D4A RID: 3402
		private Dictionary<string, GameListEntry> joinRooms = new Dictionary<string, GameListEntry>();

		// Token: 0x04000D4B RID: 3403
		private Dictionary<string, GameListEntry> spectateGames = new Dictionary<string, GameListEntry>();

		// Token: 0x04000D4C RID: 3404
		private GameListLogic gameListLogic = new GameListLogic();
	}
}

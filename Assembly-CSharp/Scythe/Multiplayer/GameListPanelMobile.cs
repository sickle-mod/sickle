using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x020001F4 RID: 500
	public class GameListPanelMobile : GameListPanel
	{
		// Token: 0x06000EA3 RID: 3747 RVA: 0x0008AA20 File Offset: 0x00088C20
		public override void Start()
		{
			this.gameListLogicMobile = new GameListLogic();
			this.gameListLogicMobile.GameToSpectateFound += this.OnGameToSpectateFound;
			this.gameListLogicMobile.GamesToSpectateLoaded += this.OnGamesToSpectateLoaded;
			this.gameListLogicMobile.GamesInProgressLoaded += this.OnGamesInProgressLoaded;
			this.gameListLogicMobile.RoomFound += this.OnRoomFound;
			this.gameListLogicMobile.RoomRefreshed += this.OnRoomFound;
			this.gameListLogicMobile.RoomListLoaded += this.OnRoomListLoaded;
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0008AAC4 File Offset: 0x00088CC4
		public override void OnDestroy()
		{
			this.gameListLogicMobile.GameToSpectateFound -= this.OnGameToSpectateFound;
			this.gameListLogicMobile.GamesToSpectateLoaded -= this.OnGamesToSpectateLoaded;
			this.gameListLogicMobile.GamesInProgressLoaded -= this.OnGamesInProgressLoaded;
			this.gameListLogicMobile.RoomFound -= this.OnRoomFound;
			this.gameListLogicMobile.RoomRefreshed -= this.OnRoomFound;
			this.gameListLogicMobile.RoomListLoaded -= this.OnRoomListLoaded;
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00031913 File Offset: 0x0002FB13
		public override void AddNewRoom(LobbyRoom newLobbyRoom)
		{
			this.gameListLogicMobile.AddRoom(newLobbyRoom);
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00031921 File Offset: 0x0002FB21
		public override void RemoveRoom(string roomId)
		{
			this.gameListLogicMobile.RemoveRoom(roomId);
			this.OnRoomRemoved(roomId);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0008AB5C File Offset: 0x00088D5C
		private void OnGamesInProgressLoaded()
		{
			if (this.resumeTabToggle.isOn)
			{
				this.RefreshResumeTab();
			}
			int num = this.gameListLogicMobile.AsynchronousGamesInProgress.Count((AsynchronousGame game) => game.IsPlayerTurn);
			this.yourTurnIndicatorObject.SetActive(num > 0);
			this.numberOfYourTurnsLabel.text = num.ToString();
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00031936 File Offset: 0x0002FB36
		private void OnRoomListLoaded(bool loadMore)
		{
			if (loadMore)
			{
				this.gameListLogicMobile.GetRoomList();
				return;
			}
			base.TryRaiseRoomListLoadedAction();
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0003194D File Offset: 0x0002FB4D
		private void OnGamesToSpectateLoaded(bool loadMore)
		{
			if (loadMore)
			{
				this.gameListLogicMobile.GetGamesToSpectate();
			}
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0008ABD0 File Offset: 0x00088DD0
		private void OnRoomFound(LobbyRoom roomData)
		{
			if (this.joinTabToggle.isOn)
			{
				if (this.joinGameEntries.Count == 0)
				{
					this.RefreshJoinTab();
					return;
				}
				if (this.joinGameEntries.ContainsKey(roomData.RoomId))
				{
					this.joinGameEntries[roomData.RoomId].Init(roomData, this.lobbyScript);
					this.RefreshJoinEntryVisibility(this.joinGameEntries[roomData.RoomId]);
					return;
				}
				this.CreateJoinGameEntry(roomData);
			}
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0008AC50 File Offset: 0x00088E50
		private void OnGameToSpectateFound(GameToSpectate gameData)
		{
			if (this.spectateTabToggle.isOn)
			{
				if (this.spectateGameEntries.Count == 0)
				{
					this.RefreshSpectateTab();
					return;
				}
				if (this.spectateGameEntries.ContainsKey(gameData.Id))
				{
					this.spectateGameEntries[gameData.Id].Init(gameData, this.lobbyScript);
					return;
				}
				this.CreateSpectateGameEntry(gameData);
			}
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0008ACB8 File Offset: 0x00088EB8
		private void OnRoomRemoved(string roomId)
		{
			if (this.joinTabToggle.isOn && this.joinGameEntries.ContainsKey(roomId))
			{
				this.joinGameEntries[roomId].gameObject.SetActive(false);
				global::UnityEngine.Object.Destroy(this.joinGameEntries[roomId].gameObject);
				this.joinGameEntries.Remove(roomId);
				this.RefreshNoGameToJoinObject();
				return;
			}
			if (this.resumeTabToggle.isOn && this.resumeGameEntries.ContainsKey(roomId))
			{
				this.resumeGameEntries[roomId].gameObject.SetActive(false);
				global::UnityEngine.Object.Destroy(this.resumeGameEntries[roomId].gameObject);
				this.resumeGameEntries.Remove(roomId);
				this.RefreshNoGameToResumeObject();
				return;
			}
			if (this.spectateTabToggle.isOn && this.spectateGameEntries.ContainsKey(roomId))
			{
				this.spectateGameEntries[roomId].gameObject.SetActive(false);
				global::UnityEngine.Object.Destroy(this.spectateGameEntries[roomId].gameObject);
				this.spectateGameEntries.Remove(roomId);
				this.RefreshNoGameToSpectateObject();
			}
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0003195D File Offset: 0x0002FB5D
		public override void SwitchToResumeJoinRooms()
		{
			this.SwitchToJoinTab();
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00031965 File Offset: 0x0002FB65
		public override void SwitchToSpectateRooms()
		{
			this.SwitchToSpectateTab();
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0003196D File Offset: 0x0002FB6D
		public void SwitchToJoinTab()
		{
			this.CloseAllTabs();
			this.joinTab.SetActive(true);
			this.RefreshRoomsLists();
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00031987 File Offset: 0x0002FB87
		public void SwitchToCreateTab()
		{
			this.CloseAllTabs();
			this.createTab.SetActive(true);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0003199B File Offset: 0x0002FB9B
		public void SwitchToResumeTab()
		{
			this.CloseAllTabs();
			this.resumeTab.SetActive(true);
			this.RefreshRoomsLists();
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x000319B5 File Offset: 0x0002FBB5
		public void SwitchToSpectateTab()
		{
			this.CloseAllTabs();
			this.spectateTab.SetActive(true);
			this.RefreshRoomsLists();
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x000319CF File Offset: 0x0002FBCF
		private void CloseAllTabs()
		{
			this.joinTab.SetActive(false);
			this.createTab.SetActive(false);
			this.resumeTab.SetActive(false);
			this.spectateTab.SetActive(false);
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0008ADD8 File Offset: 0x00088FD8
		public override void RefreshRoomsLists()
		{
			if (this.gameListLogicMobile == null)
			{
				return;
			}
			this.gameListLogicMobile.RefreshRoomList();
			switch (PlatformManager.DeviceSize)
			{
			case PlatformManager.DeviceSizeType.UNKNOWN:
				this.joinContainer.constraintCount = 3;
				this.resumeContainer.constraintCount = 3;
				this.spectateContainer.constraintCount = 3;
				break;
			case PlatformManager.DeviceSizeType.XS:
				if (PlatformManager.IsIPad)
				{
					this.joinContainer.constraintCount = 3;
					this.resumeContainer.constraintCount = 3;
					this.spectateContainer.constraintCount = 3;
				}
				else if (PlatformManager.IsIPhone)
				{
					this.joinContainer.constraintCount = 2;
					this.resumeContainer.constraintCount = 2;
					this.spectateContainer.constraintCount = 2;
				}
				break;
			case PlatformManager.DeviceSizeType.S:
				this.joinContainer.constraintCount = 3;
				this.resumeContainer.constraintCount = 3;
				this.spectateContainer.constraintCount = 3;
				break;
			case PlatformManager.DeviceSizeType.M:
				if (PlatformManager.IsIPad)
				{
					this.joinContainer.constraintCount = 4;
					this.resumeContainer.constraintCount = 4;
					this.spectateContainer.constraintCount = 4;
				}
				else if (PlatformManager.IsIPhone)
				{
					this.joinContainer.constraintCount = 3;
					this.resumeContainer.constraintCount = 3;
					this.spectateContainer.constraintCount = 3;
				}
				break;
			case PlatformManager.DeviceSizeType.L:
				if (PlatformManager.IsIPad)
				{
					this.joinContainer.constraintCount = 4;
					this.resumeContainer.constraintCount = 4;
					this.spectateContainer.constraintCount = 4;
				}
				else if (PlatformManager.IsIPhone)
				{
					this.joinContainer.constraintCount = 3;
					this.resumeContainer.constraintCount = 3;
					this.spectateContainer.constraintCount = 3;
				}
				break;
			case PlatformManager.DeviceSizeType.XL:
				if (PlatformManager.IsIPad)
				{
					this.joinContainer.constraintCount = 4;
					this.resumeContainer.constraintCount = 4;
					this.spectateContainer.constraintCount = 4;
				}
				else if (PlatformManager.IsIPhone)
				{
					this.joinContainer.constraintCount = 3;
					this.resumeContainer.constraintCount = 3;
					this.spectateContainer.constraintCount = 3;
				}
				break;
			}
			if (this.joinTabToggle.isOn)
			{
				this.RefreshJoinTab();
				return;
			}
			if (this.resumeTabToggle.isOn)
			{
				this.RefreshResumeTab();
				return;
			}
			if (this.spectateTabToggle.isOn)
			{
				this.RefreshSpectateTab();
			}
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0008B02C File Offset: 0x0008922C
		private void RefreshJoinTab()
		{
			this.RemoveJoinEntries();
			Dictionary<string, LobbyRoom>.ValueCollection values = this.gameListLogicMobile.SynchronousGames.Values;
			Dictionary<string, LobbyRoom>.ValueCollection values2 = this.gameListLogicMobile.AsynchronousGames.Values;
			foreach (LobbyRoom lobbyRoom in values)
			{
				this.CreateJoinGameEntry(lobbyRoom);
			}
			foreach (LobbyRoom lobbyRoom2 in values2)
			{
				this.CreateJoinGameEntry(lobbyRoom2);
			}
			this.RefreshNoGameToJoinObject();
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0008B0E4 File Offset: 0x000892E4
		private void RefreshResumeTab()
		{
			this.RemoveResumeEntries();
			foreach (AsynchronousGame asynchronousGame in this.gameListLogicMobile.AsynchronousGamesInProgress)
			{
				this.CreateResumeGameEntry(asynchronousGame);
			}
			this.RefreshNoGameToResumeObject();
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0008B124 File Offset: 0x00089324
		private void RefreshSpectateTab()
		{
			this.RemoveSpectateEntries();
			foreach (GameToSpectate gameToSpectate in this.gameListLogicMobile.GamesToSpectate.Values)
			{
				this.CreateSpectateGameEntry(gameToSpectate);
			}
			this.RefreshNoGameToSpectateObject();
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00031A01 File Offset: 0x0002FC01
		private void RefreshNoGameToSpectateObject()
		{
			if (this.spectateGameEntries.Count == 0)
			{
				this.noGameToSpectateObject.SetActive(true);
				return;
			}
			this.noGameToSpectateObject.SetActive(false);
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0008B190 File Offset: 0x00089390
		private void CreateJoinGameEntry(LobbyRoom lobbyRoom)
		{
			GameListEntry gameListEntry = global::UnityEngine.Object.Instantiate<GameListEntry>(this.gameEntryPrefab, this.joinContainer.transform);
			gameListEntry.Init(lobbyRoom, this.lobbyScript);
			this.joinGameEntries.Add(lobbyRoom.RoomId, gameListEntry);
			this.RefreshJoinEntryVisibility(gameListEntry);
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0008B1DC File Offset: 0x000893DC
		private void CreateResumeGameEntry(AsynchronousGame asynchronousGame)
		{
			GameListEntry gameListEntry = global::UnityEngine.Object.Instantiate<GameListEntry>(this.gameEntryPrefab, this.resumeContainer.transform);
			gameListEntry.Init(asynchronousGame, this.lobbyScript);
			this.resumeGameEntries.Add(asynchronousGame.GameId, gameListEntry);
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x0008B220 File Offset: 0x00089420
		private void CreateSpectateGameEntry(GameToSpectate gameToSpectate)
		{
			GameListEntry gameListEntry = global::UnityEngine.Object.Instantiate<GameListEntry>(this.gameEntryPrefab, this.spectateContainer.transform);
			gameListEntry.Init(gameToSpectate, this.lobbyScript);
			this.spectateGameEntries.Add(gameToSpectate.Id, gameListEntry);
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0008B264 File Offset: 0x00089464
		private void RemoveJoinEntries()
		{
			foreach (GameListEntry gameListEntry in this.joinGameEntries.Values)
			{
				global::UnityEngine.Object.Destroy(gameListEntry.gameObject);
			}
			this.joinGameEntries.Clear();
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0008B2CC File Offset: 0x000894CC
		private void RemoveResumeEntries()
		{
			foreach (GameListEntry gameListEntry in this.resumeGameEntries.Values)
			{
				global::UnityEngine.Object.Destroy(gameListEntry.gameObject);
			}
			this.resumeGameEntries.Clear();
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0008B334 File Offset: 0x00089534
		private void RemoveSpectateEntries()
		{
			foreach (GameListEntry gameListEntry in this.spectateGameEntries.Values)
			{
				global::UnityEngine.Object.Destroy(gameListEntry.gameObject);
			}
			this.spectateGameEntries.Clear();
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00031A29 File Offset: 0x0002FC29
		private void RefreshNoGameToJoinObject()
		{
			if (this.GetVisibleJoinEntriesCount() == 0)
			{
				this.noGameToJoinObject.SetActive(true);
				return;
			}
			this.noGameToJoinObject.SetActive(false);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00031A4C File Offset: 0x0002FC4C
		private void RefreshNoGameToResumeObject()
		{
			if (this.resumeGameEntries.Count == 0)
			{
				this.noGameToResumeObject.SetActive(true);
				return;
			}
			this.noGameToResumeObject.SetActive(false);
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00031A74 File Offset: 0x0002FC74
		private void RefreshJoinEntryVisibility(GameListEntry joinEntry)
		{
			joinEntry.gameObject.SetActive(joinEntry.IsButtonInteractable());
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0008B39C File Offset: 0x0008959C
		private int GetVisibleJoinEntriesCount()
		{
			int num = 0;
			using (Dictionary<string, GameListEntry>.ValueCollection.Enumerator enumerator = this.joinGameEntries.Values.GetEnumerator())
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

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0008B400 File Offset: 0x00089600
		private void SortNewlyAddedGameEntry(GameListEntry newEntry, List<GameListEntry> allEntries)
		{
			if (newEntry.IsIFA)
			{
				newEntry.transform.SetAsLastSibling();
				return;
			}
			int count = allEntries.FindAll((GameListEntry entry) => !entry.IsIFA).Count;
			newEntry.transform.SetSiblingIndex(count - 1);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00031A87 File Offset: 0x0002FC87
		public void RefreshButton_OnClick()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.RefreshRoomsLists();
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00031A9B File Offset: 0x0002FC9B
		public void RefreshAllJoinEntriesVisibility_OnValueChanged()
		{
			this.RefreshJoinTab();
		}

		// Token: 0x04000B78 RID: 2936
		[Header("Mobile UI")]
		[SerializeField]
		private Lobby lobbyScript;

		// Token: 0x04000B79 RID: 2937
		[SerializeField]
		private GameListEntry gameEntryPrefab;

		// Token: 0x04000B7A RID: 2938
		[SerializeField]
		private Toggle joinTabToggle;

		// Token: 0x04000B7B RID: 2939
		[SerializeField]
		private Toggle createTabToggle;

		// Token: 0x04000B7C RID: 2940
		[SerializeField]
		private Toggle resumeTabToggle;

		// Token: 0x04000B7D RID: 2941
		[SerializeField]
		private Toggle spectateTabToggle;

		// Token: 0x04000B7E RID: 2942
		[SerializeField]
		private GameObject yourTurnIndicatorObject;

		// Token: 0x04000B7F RID: 2943
		[SerializeField]
		private TextMeshProUGUI numberOfYourTurnsLabel;

		// Token: 0x04000B80 RID: 2944
		[SerializeField]
		private GameObject joinTab;

		// Token: 0x04000B81 RID: 2945
		[SerializeField]
		private GameObject createTab;

		// Token: 0x04000B82 RID: 2946
		[SerializeField]
		private GameObject resumeTab;

		// Token: 0x04000B83 RID: 2947
		[SerializeField]
		private GameObject spectateTab;

		// Token: 0x04000B84 RID: 2948
		[SerializeField]
		private StretchyGridLayout joinContainer;

		// Token: 0x04000B85 RID: 2949
		[SerializeField]
		private StretchyGridLayout resumeContainer;

		// Token: 0x04000B86 RID: 2950
		[SerializeField]
		private StretchyGridLayout spectateContainer;

		// Token: 0x04000B87 RID: 2951
		[SerializeField]
		private GameObject noGameToJoinObject;

		// Token: 0x04000B88 RID: 2952
		[SerializeField]
		private GameObject noGameToResumeObject;

		// Token: 0x04000B89 RID: 2953
		[SerializeField]
		private GameObject noGameToSpectateObject;

		// Token: 0x04000B8A RID: 2954
		private const bool SHOW_GAMES_YOU_CANT_JOIN = false;

		// Token: 0x04000B8B RID: 2955
		private Dictionary<string, GameListEntry> joinGameEntries = new Dictionary<string, GameListEntry>();

		// Token: 0x04000B8C RID: 2956
		private Dictionary<string, GameListEntry> resumeGameEntries = new Dictionary<string, GameListEntry>();

		// Token: 0x04000B8D RID: 2957
		private Dictionary<string, GameListEntry> spectateGameEntries = new Dictionary<string, GameListEntry>();

		// Token: 0x04000B8E RID: 2958
		private GameListLogic gameListLogicMobile;
	}
}

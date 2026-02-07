using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Scythe.Multiplayer.Data;
using Scythe.Utilities;

namespace Scythe.Multiplayer
{
	// Token: 0x0200023A RID: 570
	public class GameListLogic
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x0003311C File Offset: 0x0003131C
		public Dictionary<string, LobbyRoom> SynchronousGames
		{
			get
			{
				return this.synchronousGames;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x00033124 File Offset: 0x00031324
		public Dictionary<string, LobbyRoom> AsynchronousGames
		{
			get
			{
				return this.asynchronousGames;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x0003312C File Offset: 0x0003132C
		public AsynchronousGame[] AsynchronousGamesInProgress
		{
			get
			{
				return this.asynchronousGamesInProgress;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06001108 RID: 4360 RVA: 0x00033134 File Offset: 0x00031334
		public Dictionary<string, GameToSpectate> GamesToSpectate
		{
			get
			{
				return this.gamesToSpectate;
			}
		}

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06001109 RID: 4361 RVA: 0x00091A78 File Offset: 0x0008FC78
		// (remove) Token: 0x0600110A RID: 4362 RVA: 0x00091AB0 File Offset: 0x0008FCB0
		public event Action<LobbyRoom> RoomFound;

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x0600110B RID: 4363 RVA: 0x00091AE8 File Offset: 0x0008FCE8
		// (remove) Token: 0x0600110C RID: 4364 RVA: 0x00091B20 File Offset: 0x0008FD20
		public event Action<LobbyRoom> RoomRefreshed;

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x0600110D RID: 4365 RVA: 0x00091B58 File Offset: 0x0008FD58
		// (remove) Token: 0x0600110E RID: 4366 RVA: 0x00091B90 File Offset: 0x0008FD90
		public event Action<GameToSpectate> GameToSpectateFound;

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x0600110F RID: 4367 RVA: 0x00091BC8 File Offset: 0x0008FDC8
		// (remove) Token: 0x06001110 RID: 4368 RVA: 0x00091C00 File Offset: 0x0008FE00
		public event Action<bool> RoomListLoaded;

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06001111 RID: 4369 RVA: 0x00091C38 File Offset: 0x0008FE38
		// (remove) Token: 0x06001112 RID: 4370 RVA: 0x00091C70 File Offset: 0x0008FE70
		public event Action<bool> GamesToSpectateLoaded;

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06001113 RID: 4371 RVA: 0x00091CA8 File Offset: 0x0008FEA8
		// (remove) Token: 0x06001114 RID: 4372 RVA: 0x00091CE0 File Offset: 0x0008FEE0
		public event global::System.Action GamesInProgressLoaded;

		// Token: 0x06001115 RID: 4373 RVA: 0x0003313C File Offset: 0x0003133C
		public bool RoomsListsRefreshed()
		{
			return this.roomListLoaded && this.gamesInProgressLoaded && this.gamesInProgressLoaded;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00033156 File Offset: 0x00031356
		public int AmountOfGamesToJoin()
		{
			return this.asynchronousGames.Count + this.synchronousGames.Count;
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x0003316F File Offset: 0x0003136F
		public void RefreshRoom(string roomId)
		{
			LobbyRestAPI.RefreshRoom(roomId, delegate(string response)
			{
				this.AddRoom(this.DeserializeLobbyRoom(response));
			});
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00033183 File Offset: 0x00031383
		public LobbyRoom DeserializeLobbyRoom(string data)
		{
			return GameSerializer.DeserializeObject<LobbyRoom>(data);
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00091D18 File Offset: 0x0008FF18
		public void RefreshRoomList()
		{
			this.roomListLoaded = false;
			this.gamesToSpectateLoaded = false;
			this.gamesInProgressLoaded = false;
			this.synchronousGames.Clear();
			this.asynchronousGames.Clear();
			this.gamesToSpectate.Clear();
			this.asynchronousGamesInProgress = new AsynchronousGame[0];
			if (PlayerInfo.me.CurrentLobbyRoom != null)
			{
				this.AddRoom(PlayerInfo.me.CurrentLobbyRoom);
			}
			this.GetRoomList();
			this.GetGamesToSpectate();
			this.GetGamesInProgress(PlayerInfo.me.PlayerStats.Id);
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0003318B File Offset: 0x0003138B
		public void GetRoomList()
		{
			LobbyRestAPI.GetOpenedRooms(delegate(string rooms)
			{
				List<LobbyRoom> list = GameSerializer.DeserializeObject<List<LobbyRoom>>(rooms);
				int num = 0;
				foreach (LobbyRoom lobbyRoom in list)
				{
					if (!this.synchronousGames.ContainsKey(lobbyRoom.RoomId) && !this.asynchronousGames.ContainsKey(lobbyRoom.RoomId))
					{
						this.AddRoom(lobbyRoom);
						num++;
					}
				}
				this.roomListLoaded = num < 3;
				if (this.RoomListLoaded != null)
				{
					this.RoomListLoaded(!this.roomListLoaded);
				}
			});
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0003319E File Offset: 0x0003139E
		public void GetGamesToSpectate()
		{
			LobbyRestAPI.GetGamesToSpectate(delegate(string games)
			{
				List<GameToSpectate> list = GameSerializer.DeserializeObject<List<GameToSpectate>>(games);
				int num = 0;
				foreach (GameToSpectate gameToSpectate in list)
				{
					if (!this.gamesToSpectate.ContainsKey(gameToSpectate.Id))
					{
						this.gamesToSpectate.Add(gameToSpectate.Id, gameToSpectate);
						if (this.GameToSpectateFound != null)
						{
							this.GameToSpectateFound(gameToSpectate);
						}
						num++;
					}
				}
				this.gamesToSpectateLoaded = num < 3;
				if (this.GamesToSpectateLoaded != null)
				{
					this.GamesToSpectateLoaded(!this.gamesToSpectateLoaded);
				}
			});
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x000331B1 File Offset: 0x000313B1
		public void GetGamesInProgress(Guid playerId)
		{
			LobbyRestAPI.GetGamesInProgress(playerId, delegate(string data)
			{
				this.asynchronousGamesInProgress = JsonConvert.DeserializeObject<AsynchronousGame[]>(data);
				this.asynchronousGamesInProgress = this.asynchronousGamesInProgress.OrderBy(delegate(AsynchronousGame game)
				{
					if (!game.IsPlayerTurn)
					{
						return 1;
					}
					return 0;
				}).ToArray<AsynchronousGame>();
				this.gamesInProgressLoaded = true;
				if (this.GamesInProgressLoaded != null)
				{
					this.GamesInProgressLoaded();
				}
			});
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x000331C5 File Offset: 0x000313C5
		public void AddRoom(LobbyRoom lobbyRoom)
		{
			if (lobbyRoom.IsAsynchronous)
			{
				this.AddRoomToList(this.asynchronousGames, lobbyRoom);
				return;
			}
			this.AddRoomToList(this.synchronousGames, lobbyRoom);
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00091DA4 File Offset: 0x0008FFA4
		private void AddRoomToList(Dictionary<string, LobbyRoom> roomList, LobbyRoom room)
		{
			if (room.IsPrivate)
			{
				return;
			}
			if (room.ChoosingMats)
			{
				return;
			}
			if (roomList.ContainsKey(room.RoomId))
			{
				roomList[room.RoomId] = room;
				if (this.RoomFound != null)
				{
					this.RoomFound(room);
				}
				return;
			}
			roomList.Add(room.RoomId, room);
			if (this.RoomRefreshed != null)
			{
				this.RoomRefreshed(room);
			}
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00091E14 File Offset: 0x00090014
		public void RemoveRoom(string roomId)
		{
			if (this.synchronousGames.ContainsKey(roomId))
			{
				this.synchronousGames.Remove(roomId);
				return;
			}
			if (this.asynchronousGames.ContainsKey(roomId))
			{
				this.asynchronousGames.Remove(roomId);
				return;
			}
			if (this.gamesToSpectate.ContainsKey(roomId))
			{
				this.gamesToSpectate.Remove(roomId);
			}
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x000331EA File Offset: 0x000313EA
		public void Clear()
		{
			this.synchronousGames.Clear();
			this.asynchronousGames.Clear();
			this.gamesToSpectate.Clear();
			this.asynchronousGamesInProgress = new AsynchronousGame[0];
		}

		// Token: 0x04000D25 RID: 3365
		private Dictionary<string, LobbyRoom> synchronousGames = new Dictionary<string, LobbyRoom>();

		// Token: 0x04000D26 RID: 3366
		private Dictionary<string, LobbyRoom> asynchronousGames = new Dictionary<string, LobbyRoom>();

		// Token: 0x04000D27 RID: 3367
		private AsynchronousGame[] asynchronousGamesInProgress = new AsynchronousGame[0];

		// Token: 0x04000D28 RID: 3368
		private Dictionary<string, GameToSpectate> gamesToSpectate = new Dictionary<string, GameToSpectate>();

		// Token: 0x04000D29 RID: 3369
		private bool roomListLoaded;

		// Token: 0x04000D2A RID: 3370
		private bool gamesToSpectateLoaded;

		// Token: 0x04000D2B RID: 3371
		private bool gamesInProgressLoaded;
	}
}

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer
{
	// Token: 0x02000266 RID: 614
	public class LobbyRestAPI
	{
		// Token: 0x060012A9 RID: 4777 RVA: 0x0003433C File Offset: 0x0003253C
		public static void FriendInvitationByName(string playerName, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("RelationshipService.svc/InvitationByName", JsonConvert.SerializeObject(playerName), true, onSuccess, onError);
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00034352 File Offset: 0x00032552
		public static void FriendInvitationById(int playerId, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("RelationshipService.svc/InvitationById", JsonConvert.SerializeObject(playerId), true, onSuccess, onError);
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x0003436D File Offset: 0x0003256D
		public static void FriendSetAsDisplayed(Guid playerId, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPutCall("RelationshipService.svc/InvitationStatus", JsonConvert.SerializeObject(playerId), true, onSuccess, onError);
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00034388 File Offset: 0x00032588
		public static void FriendAcceptInvitation(Guid playerId, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPutCall("RelationshipService.svc/Invitation", JsonConvert.SerializeObject(playerId), true, onSuccess, onError);
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x000343A3 File Offset: 0x000325A3
		public static void FriendDeclineInvitation(Guid playerId, Action<string> onSuccess, Action<Exception> onError)
		{
			LobbyRestAPI.FriendRemove(playerId, onSuccess, onError);
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x000343A3 File Offset: 0x000325A3
		public static void FriendCancelInvitation(Guid playerId, Action<string> onSuccess, Action<Exception> onError)
		{
			LobbyRestAPI.FriendRemove(playerId, onSuccess, onError);
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x000343AD File Offset: 0x000325AD
		public static void FriendRemove(Guid playerId, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestDeleteCall(string.Format("{0}Relationship?playerId={1}", "RelationshipService.svc/", playerId), onSuccess, onError);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x000343CC File Offset: 0x000325CC
		public static void GetRelationships(Action<string> onSuccess)
		{
			RequestController.RequestGetCall(string.Format("{0}Relationships", "RelationshipService.svc/"), onSuccess, null);
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x000975E4 File Offset: 0x000957E4
		public static void RefreshRoom(string roomId, Action<string> onSuccess)
		{
			RequestController.RequestGetCall(string.Format("{0}Room?roomId={1}", "Lobby/", roomId), delegate(string room)
			{
				onSuccess(room);
			}, null);
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x000343E5 File Offset: 0x000325E5
		public static void GetOpenedRooms(Action<string> onSuccess)
		{
			RequestController.RequestGetCall(string.Format("{0}RoomList?amount={1}", "Lobby/", "15"), onSuccess, null);
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00034403 File Offset: 0x00032603
		public static void GetGamesToSpectate(Action<string> onSuccess)
		{
			RequestController.RequestGetCall(string.Format("{0}GameList?amount={1}", "Lobby/", "15"), onSuccess, null);
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00034421 File Offset: 0x00032621
		public static void GetGamesInProgress(Guid playerId, Action<string> onSuccess)
		{
			RequestController.RequestGetCall(string.Format("{0}CurrentGames?playerId={1}", "Lobby/", playerId), onSuccess, null);
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00034440 File Offset: 0x00032640
		public static void GetPlayersInLobby(Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestGetCall(string.Format("{0}PlayerList?amount={1}", "Lobby/", "15"), onSuccess, onError);
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0003445E File Offset: 0x0003265E
		public static void GetFriendsData(string friendsIds, Action<string> onSuccess)
		{
			RequestController.RequestGetCall(string.Format("{0}FriendsData?friendsIds={1}", "Friends/", friendsIds), onSuccess, null);
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00034478 File Offset: 0x00032678
		public static void GetFriendsData(List<Guid> friendsIds, Action<string> onSuccess)
		{
			LobbyRestAPI.GetFriendsData(JsonConvert.SerializeObject(friendsIds), onSuccess);
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00034486 File Offset: 0x00032686
		public static void GetPlayerStats(Guid playerId, Action<string> onSuccess)
		{
			RequestController.RequestGetCall(string.Format("{0}Stats?playerId={1}", "Lobby/", playerId), onSuccess, null);
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x000344A5 File Offset: 0x000326A5
		public static void JoinLobby(Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Lobby/JoinLobby", RequestController.GenerateStringFromMessage<Data>(new Data()), false, onSuccess, onError);
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x000344BF File Offset: 0x000326BF
		public static void LeaveRoom(Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Lobby/LeaveRoom", RequestController.GenerateStringFromMessage<Data>(new Data()), false, onSuccess, onError);
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x000344D9 File Offset: 0x000326D9
		public static void LeaveRoomAndJoinLobby(Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Lobby/LeaveRoomAndJoinLobby", RequestController.GenerateStringFromMessage<Data>(new Data()), false, onSuccess, onError);
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x000344F3 File Offset: 0x000326F3
		public static void JoinRoom(string roomId, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Lobby/JoinRoom", RequestController.GenerateStringFromMessage<Data>(new Data(roomId)), false, onSuccess, onError);
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0003450E File Offset: 0x0003270E
		public static void CreateAndJoinRoom(LobbyRoom roomData, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Lobby/CreateAndJoinRoom", RequestController.GenerateStringFromMessage<LobbyRoom>(roomData), false, onSuccess, onError);
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00034524 File Offset: 0x00032724
		public static void QuickPlay(Preferences preferences, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Lobby/QuickPlay", RequestController.GenerateStringFromMessage<Preferences>(preferences), false, onSuccess, onError);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0003453A File Offset: 0x0003273A
		public static void SpectateGame(SpectateGame data, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Game/Spectate", RequestController.GenerateStringFromMessage<SpectateGame>(data), false, onSuccess, onError);
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00034550 File Offset: 0x00032750
		public static void Reconnect(ReconnectData data, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall(string.Format("{0}Reconnect", "Game/"), RequestController.GenerateStringFromMessage<ReconnectData>(data), false, onSuccess, onError);
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00034570 File Offset: 0x00032770
		public static void AbandonGame(string gameId, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestDeleteCall(string.Format("{0}Reconnect?gameId={1}", "Game/", gameId), onSuccess, onError);
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x0003458A File Offset: 0x0003278A
		public static void AddBot(Bot data, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Room/Bot", RequestController.GenerateStringFromMessage<Bot>(data), false, onSuccess, onError);
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x000345A0 File Offset: 0x000327A0
		public static void RemoveBot(Bot data, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestDeleteCall(string.Format("{0}Bot?roomId={1}&slot={2}", "Room/", data.RoomId, data.Slot), onSuccess, onError);
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x000345CA File Offset: 0x000327CA
		public static void UpdateBot(Bot data, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPutCall("Room/Bot", RequestController.GenerateStringFromMessage<Bot>(data), false, onSuccess, onError);
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x000345E0 File Offset: 0x000327E0
		public static void UpdateReadyState(Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Room/SetReady", RequestController.GenerateStringFromMessage<Data>(new Data()), false, onSuccess, onError);
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x000345FA File Offset: 0x000327FA
		public static void StartGame(Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Room/StartGame", RequestController.GenerateStringFromMessage<Data>(new Data()), false, onSuccess, onError);
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00034614 File Offset: 0x00032814
		public static void StartSelectingMats(StartingOrder data, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Room/StartSelectingMats", RequestController.GenerateStringFromMessage<StartingOrder>(data), false, onSuccess, onError);
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0003462A File Offset: 0x0003282A
		public static void MatsSelected(MatAndFactionChoosen data, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Room/MatsSelected", RequestController.GenerateStringFromMessage<MatAndFactionChoosen>(data), false, onSuccess, onError);
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00034640 File Offset: 0x00032840
		public static void StartTestingMode(Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("StressTest/QuickPlay", RequestController.GenerateStringFromMessage<Data>(new Data()), false, onSuccess, onError);
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0003465A File Offset: 0x0003285A
		public static void RemovePlayer(Guid playerId, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Room/RemovePlayer", RequestController.GenerateStringFromMessage<RemovePlayer>(new RemovePlayer(playerId)), false, onSuccess, onError);
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00034675 File Offset: 0x00032875
		public static void InvitePlayer(Guid playerId, Action<string> onSuccess)
		{
			RequestController.RequestPostCall("Room/InvitePlayer", RequestController.GenerateStringFromMessage<InviteSend>(new InviteSend(playerId)), false, onSuccess, null);
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00034690 File Offset: 0x00032890
		public static void CheckForLeavers(Action<string> onSuccess, Action<Exception> onError = null)
		{
			RequestController.RequestPostCall("Lobby/CheckForLeavers", JsonConvert.SerializeObject(PlayerInfo.me.RoomId), true, onSuccess, onError);
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x000346AF File Offset: 0x000328AF
		public static void GetAmountOfConnectedPlayers(Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestGetCall(string.Format("{0}AmountOfConnectedPlayers", "Scythe/"), onSuccess, onError);
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x000346C8 File Offset: 0x000328C8
		public static void GetAmountOfOngoingGames(Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestGetCall(string.Format("{0}AmountOfOngoingGames", "Scythe/"), onSuccess, onError);
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00097624 File Offset: 0x00095824
		public static void ForfeitGame(string gameId, bool ranked, Action<string> onSuccess, Action<Exception> onError)
		{
			Game game = new Game(gameId, ranked, true);
			RequestController.RequestPostCall("Scythe/ForfeitGame", RequestController.GenerateStringFromMessage<Game>(game), false, onSuccess, onError);
		}
	}
}

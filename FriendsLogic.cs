using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Scythe.Multiplayer.AuthApi.Models;
using Scythe.Multiplayer.Data;
using Scythe.Utilities;

namespace Scythe.Multiplayer
{
	// Token: 0x0200022E RID: 558
	public class FriendsLogic
	{
		// Token: 0x14000062 RID: 98
		// (add) Token: 0x06001093 RID: 4243
		// (remove) Token: 0x06001094 RID: 4244
		public event global::System.Action FriendsRefreshed;

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x06001095 RID: 4245
		// (remove) Token: 0x06001096 RID: 4246
		public event global::System.Action InvitationsSentRefreshed;

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x06001097 RID: 4247
		// (remove) Token: 0x06001098 RID: 4248
		public event global::System.Action InvitationsReceivedRefreshed;

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06001099 RID: 4249
		// (remove) Token: 0x0600109A RID: 4250
		public event global::System.Action SendInvitationSuccess;

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x0600109B RID: 4251
		// (remove) Token: 0x0600109C RID: 4252
		public event Action<SendInvitationErrorStatus> SendInvitationError;

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x0600109D RID: 4253
		// (remove) Token: 0x0600109E RID: 4254
		public event Action<RemoveFriendErrorStatus> FriendRemoveError;

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x0600109F RID: 4255
		// (remove) Token: 0x060010A0 RID: 4256
		public event global::System.Action FriendRemoveSuccess;

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x060010A1 RID: 4257
		// (remove) Token: 0x060010A2 RID: 4258
		public event global::System.Action AcceptInvitationSuccess;

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x060010A3 RID: 4259
		// (remove) Token: 0x060010A4 RID: 4260
		public event Action<AcceptInvitationErrorStatus> AcceptInvitationError;

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x060010A5 RID: 4261
		// (remove) Token: 0x060010A6 RID: 4262
		public event Action<int> FriendAddedSuccess;

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x060010A7 RID: 4263
		// (remove) Token: 0x060010A8 RID: 4264
		public event global::System.Action RejectInvitationSuccess;

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x060010A9 RID: 4265
		// (remove) Token: 0x060010AA RID: 4266
		public event Action<RejectInvitationErrorStatus> RejectInvitationError;

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x060010AB RID: 4267
		// (remove) Token: 0x060010AC RID: 4268
		public event global::System.Action SetAsDisplayedSuccess;

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x060010AD RID: 4269
		// (remove) Token: 0x060010AE RID: 4270
		public event Action<SetAsDisplayedErrorStatus> SetAsDisplayedError;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x060010AF RID: 4271
		// (remove) Token: 0x060010B0 RID: 4272
		public event global::System.Action CancelInvitationSuccess;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x060010B1 RID: 4273
		// (remove) Token: 0x060010B2 RID: 4274
		public event Action<CancelInvitationErrorStatus> CancelInvitationError;

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060010B3 RID: 4275
		// (set) Token: 0x060010B4 RID: 4276
		public List<PlayerInfo> Friends { get; private set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060010B5 RID: 4277
		// (set) Token: 0x060010B6 RID: 4278
		public List<PlayerInfo> InvitationsSent { get; private set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060010B7 RID: 4279
		// (set) Token: 0x060010B8 RID: 4280
		public List<PlayerInfo> InvitationsReceived { get; private set; }

		// Token: 0x060010B9 RID: 4281
		public FriendsLogic()
		{
			this.Friends = new List<PlayerInfo>();
			this.InvitationsReceived = new List<PlayerInfo>();
			this.InvitationsSent = new List<PlayerInfo>();
			PersistentSingleton<BuddiesController>.Instance.OnGetBuddiesSuccess += this.BuddiesController_OnGetBuddiesSuccess;
		}

		// Token: 0x060010BA RID: 4282
		public void GetAllRelationships()
		{
			if (this.waitingForResponseCounter > 0)
			{
				return;
			}
			if (this.Friends.Count > 0)
			{
				Guid firstFriendId = this.Friends[0].PlayerStats.Id;
				this.RemoveFriend(firstFriendId);
				return;
			}
			PersistentSingleton<BuddiesController>.Instance.GetBuddies();
		}

		// Token: 0x060010BB RID: 4283
		public void SendInvitation(Guid playerID)
		{
			PersistentSingleton<BuddiesController>.Instance.AddBuddy(playerID);
		}

		// Token: 0x060010BC RID: 4284
		public void SendInvitation(string playerName)
		{
			PersistentSingleton<BuddiesController>.Instance.AddBuddy(playerName);
		}

		// Token: 0x060010BD RID: 4285
		public void RemoveFriend(Guid playerID)
		{
			PlayerInfo playerInfo = this.Friends.Find((PlayerInfo player) => player.PlayerStats.Id == playerID);
			if (playerInfo != null)
			{
				this.Friends.Remove(playerInfo);
			}
			PersistentSingleton<BuddiesController>.Instance.RemoveBuddy(playerID);
		}

		// Token: 0x060010BE RID: 4286
		public void SetAsDisplayed(Guid playerID)
		{
			PlayerInfo friend = this.InvitationsReceived.Find((PlayerInfo player) => player.PlayerStats.Id == playerID);
			if (friend != null)
			{
				friend.Invitation.Status = InvitationStatus.Displayed;
			}
			this.waitingForResponseCounter++;
			LobbyRestAPI.FriendSetAsDisplayed(playerID, delegate(string response)
			{
				this.waitingForResponseCounter--;
				UniversalInvocator.Event_Invocator(this.SetAsDisplayedSuccess);
			}, delegate(Exception exception)
			{
				this.waitingForResponseCounter--;
				if (friend != null)
				{
					friend.Invitation.Status = InvitationStatus.NotDisplayed;
				}
				SetAsDisplayedErrorStatus errorStatusFromResponse = this.GetErrorStatusFromResponse<SetAsDisplayedErrorStatus>(exception.Message, SetAsDisplayedErrorStatus.UnknownError);
				UniversalInvocator.Event_Invocator<SetAsDisplayedErrorStatus>(this.SetAsDisplayedError, new object[] { errorStatusFromResponse });
			});
		}

		// Token: 0x060010BF RID: 4287
		public void AcceptInvitation(Guid playerID)
		{
			PlayerInfo invitation = this.InvitationsReceived.Find((PlayerInfo player) => player.PlayerStats.Id == playerID);
			if (invitation != null)
			{
				this.InvitationsReceived.Remove(invitation);
			}
			this.waitingForResponseCounter++;
			LobbyRestAPI.FriendAcceptInvitation(playerID, delegate(string response)
			{
				this.waitingForResponseCounter--;
				UniversalInvocator.Event_Invocator(this.AcceptInvitationSuccess);
			}, delegate(Exception exception)
			{
				this.waitingForResponseCounter--;
				if (invitation != null && !this.InvitationsReceived.Exists((PlayerInfo player) => player.PlayerStats.Id == playerID))
				{
					this.InvitationsReceived.Add(invitation);
				}
				AcceptInvitationErrorStatus errorStatusFromResponse = this.GetErrorStatusFromResponse<AcceptInvitationErrorStatus>(exception.Message, AcceptInvitationErrorStatus.UnknownError);
				UniversalInvocator.Event_Invocator<AcceptInvitationErrorStatus>(this.AcceptInvitationError, new object[] { errorStatusFromResponse });
			});
		}

		// Token: 0x060010C0 RID: 4288
		public void RejectInvitation(Guid playerID)
		{
			PlayerInfo invitation = this.InvitationsReceived.Find((PlayerInfo player) => player.PlayerStats.Id == playerID);
			if (invitation != null)
			{
				this.InvitationsReceived.Remove(invitation);
			}
			this.waitingForResponseCounter++;
			LobbyRestAPI.FriendDeclineInvitation(playerID, delegate(string response)
			{
				this.waitingForResponseCounter--;
				UniversalInvocator.Event_Invocator(this.RejectInvitationSuccess);
			}, delegate(Exception exception)
			{
				this.waitingForResponseCounter--;
				if (invitation != null && !this.InvitationsReceived.Exists((PlayerInfo player) => player.PlayerStats.Id == playerID))
				{
					this.InvitationsReceived.Add(invitation);
				}
				RejectInvitationErrorStatus errorStatusFromResponse = this.GetErrorStatusFromResponse<RejectInvitationErrorStatus>(exception.Message, RejectInvitationErrorStatus.UnknownError);
				UniversalInvocator.Event_Invocator<RejectInvitationErrorStatus>(this.RejectInvitationError, new object[] { errorStatusFromResponse });
			});
		}

		// Token: 0x060010C1 RID: 4289
		public void CancelInvitation(Guid playerID)
		{
			PlayerInfo invitation = this.InvitationsSent.Find((PlayerInfo player) => player.PlayerStats.Id == playerID);
			if (invitation != null)
			{
				this.InvitationsSent.Remove(invitation);
			}
			this.waitingForResponseCounter++;
			LobbyRestAPI.FriendCancelInvitation(playerID, delegate(string response)
			{
				this.waitingForResponseCounter--;
				UniversalInvocator.Event_Invocator(this.CancelInvitationSuccess);
			}, delegate(Exception exception)
			{
				this.waitingForResponseCounter--;
				if (invitation != null && !this.InvitationsSent.Exists((PlayerInfo player) => player.PlayerStats.Id == playerID))
				{
					this.InvitationsSent.Add(invitation);
				}
				CancelInvitationErrorStatus errorStatusFromResponse = this.GetErrorStatusFromResponse<CancelInvitationErrorStatus>(exception.Message, CancelInvitationErrorStatus.UnknownError);
				UniversalInvocator.Event_Invocator<CancelInvitationErrorStatus>(this.CancelInvitationError, new object[] { errorStatusFromResponse });
			});
		}

		// Token: 0x060010C2 RID: 4290
		public void Clear()
		{
			this.ClearData();
			this.ClearEvents();
		}

		// Token: 0x060010C3 RID: 4291
		private T GetErrorStatusFromResponse<T>(string response, T defaultStatus)
		{
			T t;
			try
			{
				t = JsonConvert.DeserializeObject<FriendResponseErrorStatus<T>>(response).ErrorStatus;
			}
			catch
			{
				t = defaultStatus;
			}
			return t;
		}

		// Token: 0x060010C4 RID: 4292
		private void ClearData()
		{
			this.Friends.Clear();
			this.InvitationsReceived.Clear();
			this.InvitationsSent.Clear();
		}

		// Token: 0x060010C5 RID: 4293
		private void ClearEvents()
		{
			this.FriendsRefreshed = null;
			this.InvitationsReceivedRefreshed = null;
			this.InvitationsSentRefreshed = null;
			this.SendInvitationSuccess = null;
			this.SendInvitationError = null;
			this.FriendRemoveSuccess = null;
			this.FriendRemoveError = null;
			this.AcceptInvitationSuccess = null;
			this.AcceptInvitationError = null;
			this.RejectInvitationSuccess = null;
			this.RejectInvitationError = null;
			this.SetAsDisplayedSuccess = null;
			this.SetAsDisplayedError = null;
			this.CancelInvitationSuccess = null;
			this.CancelInvitationError = null;
		}

		// Token: 0x060010C6 RID: 4294
		private void BuddiesController_OnGetBuddiesSuccess(GetBuddiesResponse response)
		{
			this.Friends.Clear();
			this.lastDownloadedBuddies = response.Buddies;
			List<Guid> list = new List<Guid>();
			foreach (BuddyDto buddyDto in this.lastDownloadedBuddies)
			{
				list.Add(buddyDto.Id);
			}
			LobbyRestAPI.GetFriendsData(list, new Action<string>(this.LobbyRestAPI_OnGetFriendDataSuccess));
		}

		// Token: 0x060010C7 RID: 4295
		private void LobbyRestAPI_OnGetFriendDataSuccess(string response)
		{
			using (List<PlayerInfo>.Enumerator enumerator = GameSerializer.DeserializeObject<PlayerInfo[]>(response).ToList<PlayerInfo>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PlayerInfo player = enumerator.Current;
					BuddyDto buddyDto = this.lastDownloadedBuddies.FirstOrDefault((BuddyDto b) => b.Id == player.PlayerStats.Id);
					if (buddyDto != null)
					{
						player.PlayerStats.Name = buddyDto.Name;
					}
					this.Friends.Add(player);
				}
			}
			this.Friends.Sort((PlayerInfo friend1, PlayerInfo friend2) => string.Compare(friend1.PlayerStats.Name, friend2.PlayerStats.Name, StringComparison.CurrentCultureIgnoreCase));
			UniversalInvocator.Event_Invocator(this.FriendsRefreshed);
		}

		// Token: 0x04000CDD RID: 3293
		private int waitingForResponseCounter;

		// Token: 0x04000CDE RID: 3294
		private List<BuddyDto> lastDownloadedBuddies;
	}
}

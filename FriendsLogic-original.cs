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
		// (add) Token: 0x06001093 RID: 4243 RVA: 0x0009037C File Offset: 0x0008E57C
		// (remove) Token: 0x06001094 RID: 4244 RVA: 0x000903B4 File Offset: 0x0008E5B4
		public event global::System.Action FriendsRefreshed;

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x06001095 RID: 4245 RVA: 0x000903EC File Offset: 0x0008E5EC
		// (remove) Token: 0x06001096 RID: 4246 RVA: 0x00090424 File Offset: 0x0008E624
		public event global::System.Action InvitationsSentRefreshed;

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x06001097 RID: 4247 RVA: 0x0009045C File Offset: 0x0008E65C
		// (remove) Token: 0x06001098 RID: 4248 RVA: 0x00090494 File Offset: 0x0008E694
		public event global::System.Action InvitationsReceivedRefreshed;

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06001099 RID: 4249 RVA: 0x000904CC File Offset: 0x0008E6CC
		// (remove) Token: 0x0600109A RID: 4250 RVA: 0x00090504 File Offset: 0x0008E704
		public event global::System.Action SendInvitationSuccess;

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x0600109B RID: 4251 RVA: 0x0009053C File Offset: 0x0008E73C
		// (remove) Token: 0x0600109C RID: 4252 RVA: 0x00090574 File Offset: 0x0008E774
		public event Action<SendInvitationErrorStatus> SendInvitationError;

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x0600109D RID: 4253 RVA: 0x000905AC File Offset: 0x0008E7AC
		// (remove) Token: 0x0600109E RID: 4254 RVA: 0x000905E4 File Offset: 0x0008E7E4
		public event Action<RemoveFriendErrorStatus> FriendRemoveError;

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x0600109F RID: 4255 RVA: 0x0009061C File Offset: 0x0008E81C
		// (remove) Token: 0x060010A0 RID: 4256 RVA: 0x00090654 File Offset: 0x0008E854
		public event global::System.Action FriendRemoveSuccess;

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x060010A1 RID: 4257 RVA: 0x0009068C File Offset: 0x0008E88C
		// (remove) Token: 0x060010A2 RID: 4258 RVA: 0x000906C4 File Offset: 0x0008E8C4
		public event global::System.Action AcceptInvitationSuccess;

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x060010A3 RID: 4259 RVA: 0x000906FC File Offset: 0x0008E8FC
		// (remove) Token: 0x060010A4 RID: 4260 RVA: 0x00090734 File Offset: 0x0008E934
		public event Action<AcceptInvitationErrorStatus> AcceptInvitationError;

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x060010A5 RID: 4261 RVA: 0x0009076C File Offset: 0x0008E96C
		// (remove) Token: 0x060010A6 RID: 4262 RVA: 0x000907A4 File Offset: 0x0008E9A4
		public event Action<int> FriendAddedSuccess;

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x060010A7 RID: 4263 RVA: 0x000907DC File Offset: 0x0008E9DC
		// (remove) Token: 0x060010A8 RID: 4264 RVA: 0x00090814 File Offset: 0x0008EA14
		public event global::System.Action RejectInvitationSuccess;

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x060010A9 RID: 4265 RVA: 0x0009084C File Offset: 0x0008EA4C
		// (remove) Token: 0x060010AA RID: 4266 RVA: 0x00090884 File Offset: 0x0008EA84
		public event Action<RejectInvitationErrorStatus> RejectInvitationError;

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x060010AB RID: 4267 RVA: 0x000908BC File Offset: 0x0008EABC
		// (remove) Token: 0x060010AC RID: 4268 RVA: 0x000908F4 File Offset: 0x0008EAF4
		public event global::System.Action SetAsDisplayedSuccess;

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x060010AD RID: 4269 RVA: 0x0009092C File Offset: 0x0008EB2C
		// (remove) Token: 0x060010AE RID: 4270 RVA: 0x00090964 File Offset: 0x0008EB64
		public event Action<SetAsDisplayedErrorStatus> SetAsDisplayedError;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x060010AF RID: 4271 RVA: 0x0009099C File Offset: 0x0008EB9C
		// (remove) Token: 0x060010B0 RID: 4272 RVA: 0x000909D4 File Offset: 0x0008EBD4
		public event global::System.Action CancelInvitationSuccess;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x060010B1 RID: 4273 RVA: 0x00090A0C File Offset: 0x0008EC0C
		// (remove) Token: 0x060010B2 RID: 4274 RVA: 0x00090A44 File Offset: 0x0008EC44
		public event Action<CancelInvitationErrorStatus> CancelInvitationError;

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x00032CEB File Offset: 0x00030EEB
		// (set) Token: 0x060010B4 RID: 4276 RVA: 0x00032CF3 File Offset: 0x00030EF3
		public List<PlayerInfo> Friends { get; private set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x00032CFC File Offset: 0x00030EFC
		// (set) Token: 0x060010B6 RID: 4278 RVA: 0x00032D04 File Offset: 0x00030F04
		public List<PlayerInfo> InvitationsSent { get; private set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x00032D0D File Offset: 0x00030F0D
		// (set) Token: 0x060010B8 RID: 4280 RVA: 0x00032D15 File Offset: 0x00030F15
		public List<PlayerInfo> InvitationsReceived { get; private set; }

		// Token: 0x060010B9 RID: 4281 RVA: 0x00032D1E File Offset: 0x00030F1E
		public FriendsLogic()
		{
			this.Friends = new List<PlayerInfo>();
			this.InvitationsReceived = new List<PlayerInfo>();
			this.InvitationsSent = new List<PlayerInfo>();
			PersistentSingleton<BuddiesController>.Instance.OnGetBuddiesSuccess += this.BuddiesController_OnGetBuddiesSuccess;
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00032D5D File Offset: 0x00030F5D
		public void GetAllRelationships()
		{
			if (this.waitingForResponseCounter > 0)
			{
				return;
			}
			PersistentSingleton<BuddiesController>.Instance.GetBuddies();
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00032D73 File Offset: 0x00030F73
		public void SendInvitation(Guid playerID)
		{
			PersistentSingleton<BuddiesController>.Instance.AddBuddy(playerID);
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x00032D80 File Offset: 0x00030F80
		public void SendInvitation(string playerName)
		{
			PersistentSingleton<BuddiesController>.Instance.AddBuddy(playerName);
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x00090A7C File Offset: 0x0008EC7C
		public void RemoveFriend(Guid playerID)
		{
			PlayerInfo playerInfo = this.Friends.Find((PlayerInfo player) => player.PlayerStats.Id == playerID);
			if (playerInfo != null)
			{
				this.Friends.Remove(playerInfo);
			}
			PersistentSingleton<BuddiesController>.Instance.RemoveBuddy(playerID);
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x00090AD0 File Offset: 0x0008ECD0
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

		// Token: 0x060010BF RID: 4287 RVA: 0x00090B58 File Offset: 0x0008ED58
		public void AcceptInvitation(Guid playerID)
		{
			PlayerInfo invitation = this.InvitationsReceived.Find((PlayerInfo player) => player.PlayerStats.Id == playerID);
			if (invitation != null)
			{
				this.InvitationsReceived.Remove(invitation);
			}
			this.waitingForResponseCounter++;
			Predicate<PlayerInfo> <>9__3;
			LobbyRestAPI.FriendAcceptInvitation(playerID, delegate(string response)
			{
				this.waitingForResponseCounter--;
				UniversalInvocator.Event_Invocator(this.AcceptInvitationSuccess);
			}, delegate(Exception exception)
			{
				this.waitingForResponseCounter--;
				if (invitation != null)
				{
					List<PlayerInfo> invitationsReceived = this.InvitationsReceived;
					Predicate<PlayerInfo> predicate;
					if ((predicate = <>9__3) == null)
					{
						predicate = (<>9__3 = (PlayerInfo player) => player.PlayerStats.Id == playerID);
					}
					if (!invitationsReceived.Exists(predicate))
					{
						this.InvitationsReceived.Add(invitation);
					}
				}
				AcceptInvitationErrorStatus errorStatusFromResponse = this.GetErrorStatusFromResponse<AcceptInvitationErrorStatus>(exception.Message, AcceptInvitationErrorStatus.UnknownError);
				UniversalInvocator.Event_Invocator<AcceptInvitationErrorStatus>(this.AcceptInvitationError, new object[] { errorStatusFromResponse });
			});
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x00090BE4 File Offset: 0x0008EDE4
		public void RejectInvitation(Guid playerID)
		{
			PlayerInfo invitation = this.InvitationsReceived.Find((PlayerInfo player) => player.PlayerStats.Id == playerID);
			if (invitation != null)
			{
				this.InvitationsReceived.Remove(invitation);
			}
			this.waitingForResponseCounter++;
			Predicate<PlayerInfo> <>9__3;
			LobbyRestAPI.FriendDeclineInvitation(playerID, delegate(string response)
			{
				this.waitingForResponseCounter--;
				UniversalInvocator.Event_Invocator(this.RejectInvitationSuccess);
			}, delegate(Exception exception)
			{
				this.waitingForResponseCounter--;
				if (invitation != null)
				{
					List<PlayerInfo> invitationsReceived = this.InvitationsReceived;
					Predicate<PlayerInfo> predicate;
					if ((predicate = <>9__3) == null)
					{
						predicate = (<>9__3 = (PlayerInfo player) => player.PlayerStats.Id == playerID);
					}
					if (!invitationsReceived.Exists(predicate))
					{
						this.InvitationsReceived.Add(invitation);
					}
				}
				RejectInvitationErrorStatus errorStatusFromResponse = this.GetErrorStatusFromResponse<RejectInvitationErrorStatus>(exception.Message, RejectInvitationErrorStatus.UnknownError);
				UniversalInvocator.Event_Invocator<RejectInvitationErrorStatus>(this.RejectInvitationError, new object[] { errorStatusFromResponse });
			});
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x00090C70 File Offset: 0x0008EE70
		public void CancelInvitation(Guid playerID)
		{
			PlayerInfo invitation = this.InvitationsSent.Find((PlayerInfo player) => player.PlayerStats.Id == playerID);
			if (invitation != null)
			{
				this.InvitationsSent.Remove(invitation);
			}
			this.waitingForResponseCounter++;
			Predicate<PlayerInfo> <>9__3;
			LobbyRestAPI.FriendCancelInvitation(playerID, delegate(string response)
			{
				this.waitingForResponseCounter--;
				UniversalInvocator.Event_Invocator(this.CancelInvitationSuccess);
			}, delegate(Exception exception)
			{
				this.waitingForResponseCounter--;
				if (invitation != null)
				{
					List<PlayerInfo> invitationsSent = this.InvitationsSent;
					Predicate<PlayerInfo> predicate;
					if ((predicate = <>9__3) == null)
					{
						predicate = (<>9__3 = (PlayerInfo player) => player.PlayerStats.Id == playerID);
					}
					if (!invitationsSent.Exists(predicate))
					{
						this.InvitationsSent.Add(invitation);
					}
				}
				CancelInvitationErrorStatus errorStatusFromResponse = this.GetErrorStatusFromResponse<CancelInvitationErrorStatus>(exception.Message, CancelInvitationErrorStatus.UnknownError);
				UniversalInvocator.Event_Invocator<CancelInvitationErrorStatus>(this.CancelInvitationError, new object[] { errorStatusFromResponse });
			});
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x00032D8D File Offset: 0x00030F8D
		public void Clear()
		{
			this.ClearData();
			this.ClearEvents();
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00090CFC File Offset: 0x0008EEFC
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

		// Token: 0x060010C4 RID: 4292 RVA: 0x00032D9B File Offset: 0x00030F9B
		private void ClearData()
		{
			this.Friends.Clear();
			this.InvitationsReceived.Clear();
			this.InvitationsSent.Clear();
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00090D30 File Offset: 0x0008EF30
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

		// Token: 0x060010C6 RID: 4294 RVA: 0x00090DA8 File Offset: 0x0008EFA8
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

		// Token: 0x060010C7 RID: 4295 RVA: 0x00090E30 File Offset: 0x0008F030
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

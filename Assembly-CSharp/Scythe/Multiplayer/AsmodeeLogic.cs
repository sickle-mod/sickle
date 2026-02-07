using System;
using System.Diagnostics;
using System.Linq;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using I2.Loc;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x020001FC RID: 508
	public class AsmodeeLogic
	{
		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06000F10 RID: 3856 RVA: 0x0008C388 File Offset: 0x0008A588
		// (remove) Token: 0x06000F11 RID: 3857 RVA: 0x0008C3C0 File Offset: 0x0008A5C0
		private event Action ConnectedEvent;

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06000F12 RID: 3858 RVA: 0x0008C3F8 File Offset: 0x0008A5F8
		// (remove) Token: 0x06000F13 RID: 3859 RVA: 0x0008C430 File Offset: 0x0008A630
		private event Action<string> AuthenticationErrorEvent;

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06000F14 RID: 3860 RVA: 0x0008C468 File Offset: 0x0008A668
		// (remove) Token: 0x06000F15 RID: 3861 RVA: 0x0008C4A0 File Offset: 0x0008A6A0
		public event Action AccountCreatedEvent;

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06000F16 RID: 3862 RVA: 0x0008C4D8 File Offset: 0x0008A6D8
		// (remove) Token: 0x06000F17 RID: 3863 RVA: 0x0008C510 File Offset: 0x0008A710
		public event Action<string> AccountCreationErrorEvent;

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06000F18 RID: 3864 RVA: 0x0008C548 File Offset: 0x0008A748
		// (remove) Token: 0x06000F19 RID: 3865 RVA: 0x0008C580 File Offset: 0x0008A780
		public event Action UpdateEmailSuccessEvent;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06000F1A RID: 3866 RVA: 0x0008C5B8 File Offset: 0x0008A7B8
		// (remove) Token: 0x06000F1B RID: 3867 RVA: 0x0008C5F0 File Offset: 0x0008A7F0
		public event Action<string> UpdateEmailErrorEvent;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06000F1C RID: 3868 RVA: 0x0008C628 File Offset: 0x0008A828
		// (remove) Token: 0x06000F1D RID: 3869 RVA: 0x0008C660 File Offset: 0x0008A860
		public event Action LostPasswordEmailSentEvent;

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06000F1E RID: 3870 RVA: 0x0008C698 File Offset: 0x0008A898
		// (remove) Token: 0x06000F1F RID: 3871 RVA: 0x0008C6D0 File Offset: 0x0008A8D0
		public event Action<string> LostPasswordErrorEvent;

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06000F20 RID: 3872 RVA: 0x0008C708 File Offset: 0x0008A908
		// (remove) Token: 0x06000F21 RID: 3873 RVA: 0x0008C740 File Offset: 0x0008A940
		public event Action<string> AddFriendErrorEvent;

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06000F22 RID: 3874 RVA: 0x0008C778 File Offset: 0x0008A978
		// (remove) Token: 0x06000F23 RID: 3875 RVA: 0x0008C7B0 File Offset: 0x0008A9B0
		public event Action<int> AddFriendSuccessEvent;

		// Token: 0x1400004D RID: 77
		// (add) Token: 0x06000F24 RID: 3876 RVA: 0x0008C7E8 File Offset: 0x0008A9E8
		// (remove) Token: 0x06000F25 RID: 3877 RVA: 0x0008C820 File Offset: 0x0008AA20
		public event Action<BuddyOrIgnored[]> GetBuddyEvent;

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x06000F26 RID: 3878 RVA: 0x0008C858 File Offset: 0x0008AA58
		// (remove) Token: 0x06000F27 RID: 3879 RVA: 0x0008C890 File Offset: 0x0008AA90
		public event Action RemoveBuddyEvent;

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x00031D7F File Offset: 0x0002FF7F
		private static OAuthGate _OAuthGate
		{
			get
			{
				if (CoreApplication.Instance != null)
				{
					return CoreApplication.Instance.OAuthGate;
				}
				return null;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x00031D9A File Offset: 0x0002FF9A
		private static NetworkParameters _NetworkParameters
		{
			get
			{
				if (CoreApplication.Instance != null)
				{
					return CoreApplication.Instance.NetworkParameters;
				}
				return null;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x00031DB5 File Offset: 0x0002FFB5
		public static bool UserAlreadyLogged
		{
			get
			{
				return AsmodeeLogic._OAuthGate.UserDetails != null || !string.IsNullOrEmpty(AsmodeeLogic._OAuthGate.RefreshToken);
			}
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00027E56 File Offset: 0x00026056
		private AsmodeeLogic()
		{
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x00031DD7 File Offset: 0x0002FFD7
		public static AsmodeeLogic Instance
		{
			get
			{
				if (AsmodeeLogic.instance == null)
				{
					AsmodeeLogic.instance = new AsmodeeLogic();
				}
				return AsmodeeLogic.instance;
			}
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00031DEF File Offset: 0x0002FFEF
		public void Connect()
		{
			if (!AsmodeeLogic._OAuthGate.IsRetrievingPublicToken)
			{
				AsmodeeLogic._OAuthGate.GetPublicAccessToken(delegate(OAuthError error)
				{
				});
			}
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0008C8C8 File Offset: 0x0008AAC8
		public void Connect(string login, string password, Action onPrivateToken, Action<string> onFailure)
		{
			this.ConnectedEvent = onPrivateToken;
			this.AuthenticationErrorEvent = onFailure;
			SSOHandler ssohandler = delegate(SSOAuthenticate authenticate, OnSSOSucceeded onSuccess, AbortSSO onError)
			{
				authenticate(login, password, delegate(OAuthError error)
				{
					if (error == null)
					{
						onSuccess();
						onPrivateToken();
						return;
					}
					onError();
					if (error.status == -1)
					{
						onFailure(string.Empty);
						return;
					}
					onFailure(error.error_description);
				});
			};
			AsmodeeLogic._OAuthGate.SetSSOHandler(ssohandler);
			AsmodeeLogic._OAuthGate.GetPrivateAccessToken(false, new OAuthCallback(this.RestAPI_AuthenticationEvent));
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0008C940 File Offset: 0x0008AB40
		public void ConnectWithRefreshToken(Action onPrivateToken, Action<string> onFailure)
		{
			this.ConnectedEvent = onPrivateToken;
			this.AuthenticationErrorEvent = onFailure;
			AsmodeeLogic._OAuthGate.SetSSOHandler(delegate(SSOAuthenticate authenticate, OnSSOSucceeded onSuccess, AbortSSO onError)
			{
				onError();
				onFailure(string.Empty);
			});
			AsmodeeLogic._OAuthGate.GetPrivateAccessToken(false, new OAuthCallback(this.RestAPI_AuthenticationEvent));
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0008C99C File Offset: 0x0008AB9C
		public void ConnectWithGameServiceAccount(Action onPrivateToken, Action<string> onFailure)
		{
			AsmodeeLogic.<>c__DisplayClass51_0 CS$<>8__locals1 = new AsmodeeLogic.<>c__DisplayClass51_0();
			CS$<>8__locals1.onPrivateToken = onPrivateToken;
			CS$<>8__locals1.onFailure = onFailure;
			this.ConnectedEvent = CS$<>8__locals1.onPrivateToken;
			this.AuthenticationErrorEvent = CS$<>8__locals1.onFailure;
			AsmodeeLogic._OAuthGate.SetSSOHandler(new SSOHandler(CS$<>8__locals1.<ConnectWithGameServiceAccount>g__GameServiceSSOHandler|0));
			AsmodeeLogic._OAuthGate.GetPrivateAccessToken(false, new OAuthCallback(this.RestAPI_AuthenticationEvent));
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00031E27 File Offset: 0x00030027
		public void Disconnect()
		{
			AsmodeeLogic._OAuthGate.LogOut();
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0008CA04 File Offset: 0x0008AC04
		public void SignUp(string login, string password, string email, bool subscribeNewsletter, Action onSuccess = null)
		{
			new UserSignUpEndpoint(login, password, email, subscribeNewsletter, AsmodeeLogic._OAuthGate).Execute(delegate(ApiSignUpResponse response, WebError error)
			{
				if (error == null)
				{
					if (onSuccess != null)
					{
						onSuccess();
						return;
					}
					this.AccountCreatedEvent();
					return;
				}
				else
				{
					if (error is ApiResponseError)
					{
						this.AccountCreationErrorEvent((error as ApiResponseError).error_description);
						return;
					}
					this.AccountCreationErrorEvent(error.error);
					return;
				}
			});
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00031E33 File Offset: 0x00030033
		public void EditProfile()
		{
			new Process
			{
				StartInfo = 
				{
					UseShellExecute = true,
					FileName = "https://account.asmodee.net/profile/identity"
				}
			}.Start();
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00031E5C File Offset: 0x0003005C
		public void UpdateEmailAndNewsletter(string email, bool subscribe)
		{
			new UpdateEmailAndNewsletterEndpoint(email, subscribe, AsmodeeLogic._OAuthGate).Execute(delegate(WebError error)
			{
				if (error == null)
				{
					if (this.UpdateEmailSuccessEvent != null)
					{
						this.UpdateEmailSuccessEvent();
						return;
					}
				}
				else
				{
					if (error is ApiResponseError)
					{
						this.UpdateEmailErrorEvent((error as ApiResponseError).error_description);
						return;
					}
					this.UpdateEmailErrorEvent(error.error);
				}
			});
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0008CA48 File Offset: 0x0008AC48
		public void LinkUnlinkMultipleAccounts(PartnerAccount[] add, PartnerAccount[] remove, Action onSuccess = null, Action<string> onError = null)
		{
			new LinkUnlinkMultipleEndpoint(add, remove, AsmodeeLogic._OAuthGate).Execute(delegate(WebError error)
			{
				if (error == null)
				{
					if (onSuccess != null)
					{
						onSuccess();
						return;
					}
				}
				else if (onError != null)
				{
					if (error is ApiResponseError)
					{
						onError((error as ApiResponseError).error_description);
						return;
					}
					onError(error.error);
				}
			});
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00031E7B File Offset: 0x0003007B
		public void ResetPassword(string name)
		{
			new SearchByLoginEndpoint(name, Extras.None, -1, 1, AsmodeeLogic._OAuthGate).Execute(delegate(PaginatedResult<UserSearchResult> response, WebError error)
			{
				if (error == null)
				{
					if (response.TotalElement == 0)
					{
						this.LostPasswordErrorEvent(ScriptLocalization.Get("Lobby/AddFriendError"));
						return;
					}
					new ResetPasswordEndpoint(response.Elements[0].UserId, AsmodeeLogic._OAuthGate).Execute(delegate(WebError resetError)
					{
						if (resetError == null)
						{
							this.LostPasswordEmailSentEvent();
							return;
						}
						if (resetError is ApiResponseError)
						{
							this.LostPasswordErrorEvent((resetError as ApiResponseError).error_description);
							return;
						}
						this.LostPasswordErrorEvent(resetError.error);
					});
					return;
				}
				else
				{
					if (error is ApiResponseError)
					{
						this.LostPasswordErrorEvent((error as ApiResponseError).error_description);
						return;
					}
					this.LostPasswordErrorEvent(error.error);
					return;
				}
			});
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00031E9C File Offset: 0x0003009C
		public void ResetPassword()
		{
			new ResetPasswordEndpoint(this.GetUser().UserId, AsmodeeLogic._OAuthGate).Execute(delegate(WebError resetError)
			{
				if (resetError == null)
				{
					this.LostPasswordEmailSentEvent();
					return;
				}
				if (resetError is ApiResponseError)
				{
					this.LostPasswordErrorEvent((resetError as ApiResponseError).error_description);
					return;
				}
				this.LostPasswordErrorEvent(resetError.error);
			});
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x0008CA88 File Offset: 0x0008AC88
		public void AddBuddy(string name)
		{
			SearchByLoginEndpoint searchByLoginEndpoint;
			try
			{
				searchByLoginEndpoint = new SearchByLoginEndpoint(name, Extras.None, -1, 1, AsmodeeLogic._OAuthGate);
			}
			catch (ArgumentException ex)
			{
				this.AddFriendErrorEvent(ex.Message);
				return;
			}
			searchByLoginEndpoint.Execute(delegate(PaginatedResult<UserSearchResult> response, WebError error)
			{
				if (error == null)
				{
					if (response.TotalElement == 0)
					{
						this.AddFriendErrorEvent(ScriptLocalization.Get("Lobby/AddFriendError"));
						return;
					}
					new AddBuddyEndpoint(response.Elements[0].UserId, AsmodeeLogic._OAuthGate).Execute(delegate(WebError resetError)
					{
						this.AddFriendSuccessEvent(response.Elements[0].UserId);
					});
					return;
				}
				else
				{
					if (error is ApiResponseError)
					{
						this.AddFriendErrorEvent((error as ApiResponseError).error_description);
						return;
					}
					this.AddFriendErrorEvent(error.error);
					return;
				}
			});
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00031720 File Offset: 0x0002F920
		public void AddBuddy(Guid userId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00031720 File Offset: 0x0002F920
		public void RemoveBuddy(Guid buddyId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00031EC4 File Offset: 0x000300C4
		public void GetBuddies()
		{
			new GetBuddyEndpoint(-1, -1, AsmodeeLogic._OAuthGate).Execute(delegate(PaginatedResult<BuddyOrIgnored> response, WebError error)
			{
				if (error == null && this.GetBuddyEvent != null)
				{
					BuddyOrIgnored[] array = (from friend in response.Elements
						group friend by friend.Id into friendsWithSameId
						select friendsWithSameId.First<BuddyOrIgnored>()).ToArray<BuddyOrIgnored>();
					this.GetBuddyEvent(array);
				}
			});
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00031EE3 File Offset: 0x000300E3
		public User GetUser()
		{
			return AsmodeeLogic._OAuthGate.UserDetails;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00031EEF File Offset: 0x000300EF
		public string GetPlayerName()
		{
			return CoreApplication.Instance.OAuthGate.SteamManager.PlayerName;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x00031F05 File Offset: 0x00030105
		public void AddAward(Award[] achievement)
		{
			new AddAwardsEndpoint(achievement, AsmodeeLogic._OAuthGate).Execute(delegate(WebError error)
			{
			});
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00031F36 File Offset: 0x00030136
		public void RemoveAwards(Award[] awards)
		{
			new RemoveAwardsEndpoint(awards, AsmodeeLogic._OAuthGate).Execute(delegate(WebError error)
			{
				if (error != null)
				{
					global::UnityEngine.Debug.LogError("[AsmodeeLogic] Unable to remove awards");
				}
			});
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0008CADC File Offset: 0x0008ACDC
		private void RestAPI_AuthenticationEvent(OAuthError error)
		{
			if (error == null)
			{
				if (this.ConnectedEvent != null)
				{
					this.ConnectedEvent();
					return;
				}
			}
			else
			{
				if (error.status != -1)
				{
					this.AuthenticationErrorEvent(error.error_description);
					return;
				}
				this.AuthenticationErrorEvent(string.Empty);
			}
		}

		// Token: 0x04000BC7 RID: 3015
		public const string CONNECTING_STEAM_ERROR = "Couldn't find linked Steam account. Please log in with your Asmodee account.";

		// Token: 0x04000BC8 RID: 3016
		private const string GAME_SHORT_NAME = "scte";

		// Token: 0x04000BC9 RID: 3017
		private static AsmodeeLogic instance;
	}
}

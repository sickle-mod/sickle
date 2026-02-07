using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using AuthApi;
using BestHTTP;
using Scythe.Multiplayer;
using UnityEngine;

namespace AsmodeeNet.Network
{
	// Token: 0x02000870 RID: 2160
	public class OAuthGate
	{
		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06003C99 RID: 15513 RVA: 0x0004F2EB File Offset: 0x0004D4EB
		// (set) Token: 0x06003C9A RID: 15514 RVA: 0x0004F2F3 File Offset: 0x0004D4F3
		public NetworkParameters NetworkParameters { get; protected set; }

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06003C9B RID: 15515 RVA: 0x0004F2FC File Offset: 0x0004D4FC
		// (set) Token: 0x06003C9C RID: 15516 RVA: 0x0004F304 File Offset: 0x0004D504
		public bool AutoConnect { get; set; }

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06003C9D RID: 15517 RVA: 0x0004F30D File Offset: 0x0004D50D
		// (set) Token: 0x06003C9E RID: 15518 RVA: 0x0004F315 File Offset: 0x0004D515
		protected AuthenticationTokens _PrivateAuthenticationTokens { get; set; }

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06003C9F RID: 15519 RVA: 0x0004F31E File Offset: 0x0004D51E
		// (set) Token: 0x06003CA0 RID: 15520 RVA: 0x0004F326 File Offset: 0x0004D526
		protected AuthenticationTokens _PublicAuthenticationTokens { get; set; }

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06003CA1 RID: 15521 RVA: 0x0004F32F File Offset: 0x0004D52F
		protected AuthenticationTokens _AuthenticationTokens
		{
			get
			{
				if (this._PrivateAuthenticationTokens != null)
				{
					return this._PrivateAuthenticationTokens;
				}
				return this._PublicAuthenticationTokens;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06003CA2 RID: 15522 RVA: 0x0004F346 File Offset: 0x0004D546
		// (set) Token: 0x06003CA3 RID: 15523 RVA: 0x0004F34E File Offset: 0x0004D54E
		public ISteamManager SteamManager { get; set; }

		// Token: 0x06003CA4 RID: 15524 RVA: 0x00155960 File Offset: 0x00153B60
		public OAuthGate(NetworkParameters networkParameters)
		{
			if (networkParameters == null)
			{
				throw new ArgumentNullException("networkParameters");
			}
			this.NetworkParameters = networkParameters;
			this._publicRequestSender = new OAuthGate.RequestSender(this.NetworkParameters);
			this._privateRequestSender = new OAuthGate.RequestSender(this.NetworkParameters);
			this.AutoConnect = true;
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06003CA5 RID: 15525 RVA: 0x0004F357 File Offset: 0x0004D557
		public string AccessToken
		{
			get
			{
				if (this._AuthenticationTokens != null)
				{
					return this._AuthenticationTokens.access_token;
				}
				return null;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06003CA6 RID: 15526 RVA: 0x001559D0 File Offset: 0x00153BD0
		public string RefreshToken
		{
			get
			{
				if (this._PrivateAuthenticationTokens != null && !string.IsNullOrEmpty(this._PrivateAuthenticationTokens.refresh_token))
				{
					return this._PrivateAuthenticationTokens.refresh_token;
				}
				if (!KeyValueStore.HasKey("RefreshToken"))
				{
					return null;
				}
				return StringCipher.Decrypt(KeyValueStore.GetString("RefreshToken", ""), UniqueId.GetUniqueId());
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06003CA7 RID: 15527 RVA: 0x0004F36E File Offset: 0x0004D56E
		public int TokenLifeSpan
		{
			get
			{
				if (this._AuthenticationTokens != null)
				{
					return this._AuthenticationTokens.expires_in;
				}
				return -1;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06003CA8 RID: 15528 RVA: 0x0004F385 File Offset: 0x0004D585
		public string TokenScope
		{
			get
			{
				if (this._AuthenticationTokens != null && !this._AuthenticationTokens.HasExpired)
				{
					return this._AuthenticationTokens.scope;
				}
				return null;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06003CA9 RID: 15529 RVA: 0x0004F3A9 File Offset: 0x0004D5A9
		public bool HasPublicToken
		{
			get
			{
				return this._AuthenticationTokens != null && this._AuthenticationTokens.HasPublicToken;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06003CAA RID: 15530 RVA: 0x0004F3C0 File Offset: 0x0004D5C0
		public bool IsRetrievingPublicToken
		{
			get
			{
				return this._publicRequestSender.request != null && (this._publicRequestSender.request.State == HTTPRequestStates.Queued || this._publicRequestSender.request.State == HTTPRequestStates.Processing);
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06003CAB RID: 15531 RVA: 0x0004F3F9 File Offset: 0x0004D5F9
		public bool HasPrivateToken
		{
			get
			{
				return this._AuthenticationTokens != null && this._AuthenticationTokens.HasPrivateToken;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06003CAC RID: 15532 RVA: 0x0004F410 File Offset: 0x0004D610
		public bool IsRetrievingPrivateToken
		{
			get
			{
				return this._privateRequestSender.request != null && (this._privateRequestSender.request.State == HTTPRequestStates.Queued || this._privateRequestSender.request.State == HTTPRequestStates.Processing);
			}
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x0004F449 File Offset: 0x0004D649
		public void LogOut()
		{
			this._PrivateAuthenticationTokens = null;
			this.UserDetails = null;
			this._DeleteRefreshTokenFromDisk();
			this.AutoConnect = false;
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x00155A2C File Offset: 0x00153C2C
		public bool CancelAccessTokenRequest(int callbackId)
		{
			if (this._callbacksForPrivateToken.ContainsKey(callbackId))
			{
				this._callbacksForPrivateToken.Remove(callbackId);
				if (this._callbacksForPrivateToken.Count == 0)
				{
					this._privateRequestSender.Reset();
				}
				return true;
			}
			if (this._callbacksForPublicToken.ContainsKey(callbackId))
			{
				this._callbacksForPublicToken.Remove(callbackId);
				if (this._callbacksForPublicToken.Count == 0)
				{
					this._publicRequestSender.Reset();
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003CAF RID: 15535 RVA: 0x0004F466 File Offset: 0x0004D666
		private void _WriteRefreshTokenToDisk()
		{
			if (!string.IsNullOrEmpty(this._PrivateAuthenticationTokens.refresh_token))
			{
				KeyValueStore.SetString("RefreshToken", StringCipher.Encrypt(this._PrivateAuthenticationTokens.refresh_token, UniqueId.GetUniqueId()));
				KeyValueStore.Save();
			}
		}

		// Token: 0x06003CB0 RID: 15536 RVA: 0x0004F49E File Offset: 0x0004D69E
		private void _DeleteRefreshTokenFromDisk()
		{
			KeyValueStore.DeleteKey("RefreshToken");
			KeyValueStore.Save();
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x0004F4AF File Offset: 0x0004D6AF
		private void _RequireNonNullSSOHandler()
		{
			if (this._ssoHandler == null)
			{
				throw new ArgumentException("SetSSOHandler() must be called first with a valid SSO starter function", "SetSSOHandler");
			}
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x00155AA4 File Offset: 0x00153CA4
		private void _ExecuteAndFlushCallbacks(Dictionary<int, OAuthCallback> callbacks, OAuthError authError)
		{
			if (this.isFlushingCallbacks)
			{
				throw new Exception("Recursive call to _ExecuteAndFlushCallbacks !! ABORTING !");
			}
			this.isFlushingCallbacks = true;
			foreach (KeyValuePair<int, OAuthCallback> keyValuePair in callbacks)
			{
				if (keyValuePair.Value != null)
				{
					try
					{
						keyValuePair.Value(authError);
					}
					catch (Exception ex)
					{
						AsmoLogger.Error("OAuthGate", "OAuthGate was executing callbacks (with attached error) when something bad happened" + ((authError != null) ? authError.ToString() : null), null);
						AsmoLogger.LogException(ex, "OAuthGate", AsmoLogger.Severity.Error);
					}
				}
			}
			callbacks.Clear();
			this.isFlushingCallbacks = false;
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x00155B68 File Offset: 0x00153D68
		public int? GetPublicAccessToken(OAuthCallback onComplete)
		{
			if (this.HasPublicToken)
			{
				if (onComplete != null)
				{
					onComplete(null);
				}
				return null;
			}
			bool flag = this._callbacksForPublicToken.Count >= 1;
			int num = OAuthGate.nextCallbackID++;
			this._callbacksForPublicToken[num] = onComplete;
			if (flag)
			{
				return new int?(num);
			}
			string text = this.NetworkParameters.GetApiBaseUrl() + "/main/v2/oauth/token";
			Hashtable hashtable = new Hashtable
			{
				{ "grant_type", "client_credentials" },
				{
					"client_id",
					this.NetworkParameters.ClientId
				}
			};
			Hashtable logParams = hashtable.Clone() as Hashtable;
			logParams.Add("url", text);
			hashtable.Add("client_secret", this.NetworkParameters.ClientSecret);
			AsmoLogger.Info("OAuthGate.sender", "Getting public access token", logParams);
			this._publicRequestSender.Reset();
			this._publicRequestSender.SendRequest(text, hashtable, delegate(OAuthError onRequestComplete)
			{
				if (onRequestComplete != null)
				{
					logParams.Add("status", onRequestComplete.status);
					logParams.Add("error", onRequestComplete.error);
					logParams.Add("error_description", onRequestComplete.error_description);
					AsmoLogger.Error("OAuthGate.receiver", "Public access token failure", logParams);
				}
				else
				{
					AuthenticationTokens authenticationTokens = JsonUtility.FromJson<AuthenticationTokens>(this._publicRequestSender.response.DataAsText);
					authenticationTokens.InitExpiration();
					this._PublicAuthenticationTokens = authenticationTokens;
					logParams.Add("expires_in", authenticationTokens.expires_in);
					logParams.Add("scope", authenticationTokens.scope);
					AsmoLogger.Info("OAuthGate.receiver", "Public access token success", logParams);
				}
				this._publicRequestSender.Reset();
				this._ExecuteAndFlushCallbacks(this._callbacksForPublicToken, onRequestComplete);
			});
			return new int?(num);
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x0004F4C9 File Offset: 0x0004D6C9
		public void SetSSOHandler(SSOHandler ssoHandler)
		{
			this._ssoHandler = ssoHandler;
			AsmoLogger.Info("OAuthGate.SSO", string.Format("Setting ssoHandler to {0}", (ssoHandler != null) ? ssoHandler.Method : null), null);
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x00155C90 File Offset: 0x00153E90
		protected void _AbortSSO()
		{
			OAuthError oauthError = new OAuthError();
			oauthError.status = -1;
			oauthError.error = "aborted_by_user";
			oauthError.error_description = "User didn't complete the SSO";
			Hashtable hashtable = new Hashtable();
			hashtable["status"] = oauthError.status;
			hashtable["error"] = oauthError.error;
			hashtable["error_description"] = oauthError.error_description;
			AsmoLogger.Warning("OAuthGate.receiver", "Private access token failure", hashtable);
			this._privateRequestSender.Reset();
			this._ExecuteAndFlushCallbacks(this._callbacksForPrivateToken, oauthError);
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06003CB6 RID: 15542 RVA: 0x0004F4F3 File Offset: 0x0004D6F3
		// (set) Token: 0x06003CB7 RID: 15543 RVA: 0x0004F4FB File Offset: 0x0004D6FB
		public User UserDetails { get; private set; }

		// Token: 0x06003CB8 RID: 15544 RVA: 0x00155D28 File Offset: 0x00153F28
		public void UpdateUserDetails(Action onComplete, bool checkEmailValidity = true)
		{
			new GetUserDetailsEndpoint((Extras)15, this).Execute(delegate(User result, WebError error)
			{
				if (error == null)
				{
					this.UserDetails = result;
					if (checkEmailValidity && this.ShouldUpdateEmail() && this.CanDisplayPopUp())
					{
						this.ShowUpdateEmailPopUp(onComplete);
						return;
					}
				}
				else
				{
					AsmoLogger.Error("OAuthGate.receiver", "Unable to retreive the UserDetails", null);
				}
				if (onComplete != null)
				{
					onComplete();
				}
			});
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x00155D6C File Offset: 0x00153F6C
		public bool ShouldUpdateEmail()
		{
			return this.UserDetails.EmailValid != null && !this.UserDetails.EmailValid.Value;
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x00155DA8 File Offset: 0x00153FA8
		public bool CanDisplayPopUp()
		{
			DateTime now = DateTime.Now;
			DateTime dateTime;
			return !KeyValueStore.HasKey("UpdateEmailPopUpDisplayDate") || !DateTime.TryParse(KeyValueStore.GetString("UpdateEmailPopUpDisplayDate", ""), out dateTime) || (now - dateTime).TotalDays >= 7.0;
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x00155E00 File Offset: 0x00154000
		public void ShowUpdateEmailPopUp(Action onComplete)
		{
			new GetUserDetailsEndpoint((Extras)15, this);
			KeyValueStore.SetString("UpdateEmailPopUpDisplayDate", DateTime.Now.ToString());
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x00155E30 File Offset: 0x00154030
		protected void _OnSSOSucceeded()
		{
			AuthenticationTokens authenticationTokens = JsonUtility.FromJson<AuthenticationTokens>(this._privateRequestSender.response.DataAsText);
			authenticationTokens.InitExpiration();
			this._PrivateAuthenticationTokens = authenticationTokens;
			this._WriteRefreshTokenToDisk();
			this._logParamsForPrivateToken["expires_in"] = authenticationTokens.expires_in;
			this._logParamsForPrivateToken["scope"] = authenticationTokens.scope;
			AsmoLogger.Info("OAuthGate.receiver", "Private access token success", this._logParamsForPrivateToken);
			this._AuthenticationPostProcess();
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x00155EB4 File Offset: 0x001540B4
		public int? GetPrivateAccessToken(bool silentFailure, OAuthCallback onComplete)
		{
			if (this.HasPrivateToken)
			{
				if (onComplete != null)
				{
					onComplete(null);
				}
				return null;
			}
			AsmoLogger.Info("OAuthGate", "Looking for a private access token...", null);
			bool flag = this._callbacksForPrivateToken.Count >= 1;
			int num = OAuthGate.nextCallbackID++;
			this._callbacksForPrivateToken[num] = onComplete;
			if (flag)
			{
				return new int?(num);
			}
			if (this.RefreshToken != null)
			{
				this._AuthenticateWithRefreshToken(silentFailure);
				return new int?(num);
			}
			if (this.AutoConnect && GameServiceController.Instance.IsPlayerSignedIn())
			{
				if (PlatformManager.IsSteam)
				{
					if (this.SteamManager != null)
					{
						this._AuthenticateWithSteam(silentFailure);
						return new int?(num);
					}
				}
				else
				{
					if (PlatformManager.IsIOS)
					{
						this._AuthenticateWithGameCenter(silentFailure);
						return new int?(num);
					}
					if (PlatformManager.IsAndroid)
					{
						this._AuthenticateWithGooglePlay(silentFailure);
						return new int?(num);
					}
				}
			}
			if (silentFailure)
			{
				AsmoLogger.Warning("OAuthGate", "Silently failed to find a private access token", null);
				OAuthError oauthError = OAuthError.MakeSilentAuthError();
				this._ExecuteAndFlushCallbacks(this._callbacksForPrivateToken, oauthError);
				return null;
			}
			this._RequireNonNullSSOHandler();
			this._ssoHandler(new SSOAuthenticate(this._AuthenticateUser), new OnSSOSucceeded(this._OnSSOSucceeded), new AbortSSO(this._AbortSSO));
			return new int?(num);
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x00156000 File Offset: 0x00154200
		protected void _AuthenticateUser(string login, string password, OAuthCallback onComplete)
		{
			OAuthGate.<>c__DisplayClass73_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.login = login;
			CS$<>8__locals1.password = password;
			CS$<>8__locals1.onComplete = onComplete;
			OAuthGate._authApi.SetCredentials(CS$<>8__locals1.login, CS$<>8__locals1.password);
			string token = OAuthGate._authApi.Token;
			this.<_AuthenticateUser>g___AuthenticateLegacy|73_0(ref CS$<>8__locals1);
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x00156058 File Offset: 0x00154258
		protected void _AuthenticateWithRefreshToken(bool silentFailure)
		{
			string refreshToken = this.RefreshToken;
			if (string.IsNullOrEmpty(refreshToken))
			{
				throw new Exception("Impossible to authenticate without a refresh token");
			}
			string text = this.NetworkParameters.GetApiBaseUrl() + "/main/v2/oauth/token";
			Hashtable hashtable = new Hashtable
			{
				{ "grant_type", "refresh_token" },
				{
					"client_id",
					this.NetworkParameters.ClientId
				}
			};
			Hashtable logParams = hashtable.Clone() as Hashtable;
			logParams.Add("url", text);
			AsmoLogger.Info("OAuthGate.sender", "Requesting an access token by refresh token", logParams);
			hashtable.Add("client_secret", this.NetworkParameters.ClientSecret);
			hashtable.Add("refresh_token", refreshToken);
			this._privateRequestSender.SendRequest(text, hashtable, delegate(OAuthError error)
			{
				if (error == null)
				{
					AuthenticationTokens authenticationTokens = JsonUtility.FromJson<AuthenticationTokens>(this._privateRequestSender.response.DataAsText);
					authenticationTokens.InitExpiration();
					this._PrivateAuthenticationTokens = authenticationTokens;
					this._WriteRefreshTokenToDisk();
					logParams.Add("expires_in", authenticationTokens.expires_in);
					logParams.Add("scope", authenticationTokens.scope);
					AsmoLogger.Info("OAuthGate.receiver", "Authentication successful", logParams);
					this._AuthenticationPostProcess();
					return;
				}
				logParams.Add("status", error.status);
				logParams.Add("error", error.error);
				logParams.Add("error_description", error.error_description);
				AsmoLogger.Error("OAuthGate.receiver", "Private access token failure", logParams);
				if (error.status >= 400)
				{
					this._DeleteRefreshTokenFromDisk();
				}
				if (silentFailure)
				{
					AsmoLogger.Warning("OAuthGate", "Silently failed to find a private access token", null);
					OAuthError oauthError = OAuthError.MakeSilentAuthError();
					this._ExecuteAndFlushCallbacks(this._callbacksForPrivateToken, oauthError);
					return;
				}
				this._RequireNonNullSSOHandler();
				this._ssoHandler(new SSOAuthenticate(this._AuthenticateUser), new OnSSOSucceeded(this._OnSSOSucceeded), new AbortSSO(this._AbortSSO));
			});
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x00156148 File Offset: 0x00154348
		private void _AuthenticateWithGooglePlay(bool silentFailure)
		{
			GameServiceController.Instance.GetIdentityVerificationSignature(delegate(object googlePlayToken)
			{
				Debug.Log("GooglePlay token: " + ((googlePlayToken != null) ? googlePlayToken.ToString() : null));
				if (string.IsNullOrEmpty((string)googlePlayToken))
				{
					AsmoLogger.Info("OAuthGate.GooglePlay", string.Format("Falling back on traditional SSO flow ({0})", "Google Play Id Token is empty"), null);
					this._RequireNonNullSSOHandler();
					this._ssoHandler(new SSOAuthenticate(this._AuthenticateUser), new OnSSOSucceeded(this._OnSSOSucceeded), new AbortSSO(this._AbortSSO));
					return;
				}
				Hashtable hashtable = new Hashtable
				{
					{ "grant_type", "googleplay_partner" },
					{
						"client_id",
						this.NetworkParameters.ClientId
					},
					{ "app_id", "250865451980-ur2sp7qks2vj0n34p0qan718ek12ercb.apps.googleusercontent.com" },
					{ "session_ticket", googlePlayToken }
				};
				this._AuthenticateUserWithPartnerAccount(hashtable, "GooglePlay", silentFailure);
			});
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x0004F504 File Offset: 0x0004D704
		private void _AuthenticateWithGameCenter(bool silentFailure)
		{
			GameServiceController.Instance.GetIdentityVerificationSignature(delegate(object signatureString)
			{
			});
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x00156180 File Offset: 0x00154380
		private void _AuthenticateWithSteam(bool silentFailure)
		{
			if (this.SteamManager != null && this.SteamManager.HasClient)
			{
				Hashtable hashtable = new Hashtable
				{
					{ "grant_type", "steam_partner" },
					{
						"client_id",
						this.NetworkParameters.ClientId
					},
					{
						"app_id",
						this.SteamManager.SteamAppID
					},
					{
						"session_ticket",
						this.SteamManager.SessionTicket
					}
				};
				this._AuthenticateUserWithPartnerAccount(hashtable, "Steam", silentFailure);
				return;
			}
			if (silentFailure)
			{
				AsmoLogger.Warning("OAuthGate.Steam", "Silently failed to find a private access token (no Steam client)", null);
				OAuthError oauthError = OAuthError.MakeSilentAuthError();
				this._ExecuteAndFlushCallbacks(this._callbacksForPrivateToken, oauthError);
				return;
			}
			AsmoLogger.Info("OAuthGate.Steam", "Falling back on traditional SSO flow (no Steam client)", null);
			this._RequireNonNullSSOHandler();
			this._ssoHandler(new SSOAuthenticate(this._AuthenticateUser), new OnSSOSucceeded(this._OnSSOSucceeded), new AbortSSO(this._AbortSSO));
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x0015627C File Offset: 0x0015447C
		private void _AuthenticateUserWithPartnerAccount(Hashtable parameters, string partner, bool silentFailure)
		{
			OAuthGate.<>c__DisplayClass78_0 CS$<>8__locals1 = new OAuthGate.<>c__DisplayClass78_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.partner = partner;
			CS$<>8__locals1.silentFailure = silentFailure;
			string text = this.NetworkParameters.GetApiBaseUrl() + "/main/v2/oauth/token";
			CS$<>8__locals1.logParams = parameters.Clone() as Hashtable;
			CS$<>8__locals1.logParams.Add("url", text);
			AsmoLogger.Info(string.Format("{0}.{1}.sender", "OAuthGate", CS$<>8__locals1.partner), string.Format("Requesting an access token by {0} partner", CS$<>8__locals1.partner), CS$<>8__locals1.logParams);
			parameters.Add("client_secret", this.NetworkParameters.ClientSecret);
			this._privateRequestSender.SendRequest(text, parameters, new OAuthCallback(CS$<>8__locals1.<_AuthenticateUserWithPartnerAccount>g__OnAuthenticationResponse|0));
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x0004F52F File Offset: 0x0004D72F
		private void _AuthenticationPostProcess()
		{
			this.UpdateUserDetails(delegate
			{
				if (this.UserDetails != null && this.AutoConnect)
				{
					if (PlatformManager.IsSteam)
					{
						if (this.SteamManager != null && this.SteamManager.HasClient && this.UserDetails.Partners.FirstOrDefault((PartnerAccount x) => x.Equals(this.SteamManager.Me)) == null)
						{
							this.SteamManager.LinkSteamAccount(this, new Action(this.<_AuthenticationPostProcess>g__OnLinkSteamAccountResponse|79_1));
							return;
						}
					}
					else
					{
						if (PlatformManager.IsIOS)
						{
							PartnerAccount partnerAccount = new PartnerAccount(11, GameServiceController.Instance.PlayerId(), null);
							this.LinkLoggedAccountWithPartner(partnerAccount);
							return;
						}
						if (PlatformManager.IsAndroid)
						{
							GameServiceController.Instance.GetIdentityVerificationSignature(delegate(object token)
							{
								if (!string.IsNullOrEmpty((string)token))
								{
									PartnerAccount partnerAccount2 = new PartnerAccount(16, GameServiceController.Instance.PlayerId(), (string)token, "250865451980-ur2sp7qks2vj0n34p0qan718ek12ercb.apps.googleusercontent.com", null);
									this.LinkLoggedAccountWithPartner(partnerAccount2);
								}
								this.OnLinkComplete();
							});
							return;
						}
					}
				}
				this.OnLinkComplete();
			}, true);
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x0015633C File Offset: 0x0015453C
		private void LinkLoggedAccountWithPartner(PartnerAccount account)
		{
			if (this.UserDetails.Partners.FirstOrDefault((PartnerAccount x) => x.Equals(account)) == null)
			{
				PartnerAccount[] array = new PartnerAccount[] { account };
				Action <>9__3;
				AsmodeeLogic.Instance.LinkUnlinkMultipleAccounts(array, null, delegate
				{
					OAuthGate <>4__this = this;
					Action action;
					if ((action = <>9__3) == null)
					{
						action = (<>9__3 = delegate
						{
							this.OnLinkComplete();
						});
					}
					<>4__this.UpdateUserDetails(action, true);
				}, delegate(string error)
				{
					this.OnLinkComplete();
				});
				return;
			}
			this.OnLinkComplete();
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x0004F544 File Offset: 0x0004D744
		private void OnLinkComplete()
		{
			this._privateRequestSender.Reset();
			this._ExecuteAndFlushCallbacks(this._callbacksForPrivateToken, null);
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x001563B8 File Offset: 0x001545B8
		[CompilerGenerated]
		private void <_AuthenticateUser>g___AuthenticateLegacy|73_0(ref OAuthGate.<>c__DisplayClass73_0 A_1)
		{
			string text = this.NetworkParameters.GetApiBaseUrl() + "/main/v2/oauth/token";
			Hashtable hashtable = new Hashtable
			{
				{ "grant_type", "password" },
				{
					"client_id",
					this.NetworkParameters.ClientId
				},
				{ "username", A_1.login }
			};
			this._logParamsForPrivateToken = hashtable.Clone() as Hashtable;
			this._logParamsForPrivateToken.Add("url", text);
			AsmoLogger.Info("OAuthGate.sender", "Getting private access token", this._logParamsForPrivateToken);
			hashtable.Add("client_secret", this.NetworkParameters.ClientSecret);
			hashtable.Add("password", A_1.password);
			this._privateRequestSender.Reset();
			this._privateRequestSender.SendRequest(text, hashtable, A_1.onComplete);
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x0004F583 File Offset: 0x0004D783
		[CompilerGenerated]
		private void <_AuthenticationPostProcess>g__OnLinkSteamAccountResponse|79_1()
		{
			this.UpdateUserDetails(new Action(this.<_AuthenticationPostProcess>g__OnUpdateUserDetails|79_2), true);
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x0004F598 File Offset: 0x0004D798
		[CompilerGenerated]
		private void <_AuthenticationPostProcess>g__OnUpdateUserDetails|79_2()
		{
			this.OnLinkComplete();
		}

		// Token: 0x04002DDD RID: 11741
		private static readonly AuthApiConnection _authApi = new AuthApiConnection();

		// Token: 0x04002DDE RID: 11742
		public const string kDebugModuleName = "OAuthGate";

		// Token: 0x04002DDF RID: 11743
		public const string kNoPrivateTokenError = "no_private_token";

		// Token: 0x04002DE0 RID: 11744
		public const string kAbortError = "aborted_by_user";

		// Token: 0x04002DE1 RID: 11745
		private const string _kRefreshTokenKeyNameInPlayerPref = "RefreshToken";

		// Token: 0x04002DE2 RID: 11746
		private const string _kUpdateEmailPopUpDisplayDate = "UpdateEmailPopUpDisplayDate";

		// Token: 0x04002DE3 RID: 11747
		private const string _kEndpointAuthentication = "/main/v2/oauth/token";

		// Token: 0x04002DE6 RID: 11750
		protected OAuthGate.RequestSender _publicRequestSender;

		// Token: 0x04002DE7 RID: 11751
		protected OAuthGate.RequestSender _privateRequestSender;

		// Token: 0x04002DE8 RID: 11752
		private static int nextCallbackID = 0;

		// Token: 0x04002DE9 RID: 11753
		public Dictionary<int, OAuthCallback> _callbacksForPrivateToken = new Dictionary<int, OAuthCallback>();

		// Token: 0x04002DEA RID: 11754
		public Dictionary<int, OAuthCallback> _callbacksForPublicToken = new Dictionary<int, OAuthCallback>();

		// Token: 0x04002DEB RID: 11755
		private Hashtable _logParamsForPrivateToken;

		// Token: 0x04002DEF RID: 11759
		private bool isFlushingCallbacks;

		// Token: 0x04002DF0 RID: 11760
		private SSOHandler _ssoHandler;

		// Token: 0x02000871 RID: 2161
		protected class RequestSender
		{
			// Token: 0x06003CCE RID: 15566 RVA: 0x0004F5A0 File Offset: 0x0004D7A0
			public RequestSender(NetworkParameters networkParameters)
			{
				this._restAPIPinPublicKeys = networkParameters.RestAPIPinPublicKeys;
			}

			// Token: 0x06003CCF RID: 15567 RVA: 0x0004F5B4 File Offset: 0x0004D7B4
			public void Reset()
			{
				if (this.request != null && this.request.State < HTTPRequestStates.Finished)
				{
					this.request.Abort();
				}
				this.request = null;
				this.response = null;
			}

			// Token: 0x06003CD0 RID: 15568 RVA: 0x001565AC File Offset: 0x001547AC
			public virtual void SendRequest(string url, Hashtable parameters, OAuthCallback onComplete)
			{
				if (onComplete == null)
				{
					throw new ArgumentNullException("onComplete");
				}
				HTTPMethods httpmethods = HTTPMethods.Post;
				Uri uri = new Uri(url);
				this.response = null;
				this.request = new HTTPRequest(uri, httpmethods, delegate(HTTPRequest req, HTTPResponse resp)
				{
					if (req.State == HTTPRequestStates.Aborted)
					{
						return;
					}
					if (resp == null)
					{
						OAuthError oauthError;
						if (req.State == HTTPRequestStates.TimedOut)
						{
							oauthError = OAuthError.MakeTimeoutError();
						}
						else
						{
							CertificateVerifier certificateVerifier = req.CustomCertificateVerifyer as CertificateVerifier;
							if (certificateVerifier != null && !certificateVerifier.isValid)
							{
								oauthError = OAuthError.MakePublicKeyPinningError();
							}
							else
							{
								oauthError = OAuthError.MakeNoResponseError();
							}
						}
						onComplete(oauthError);
						return;
					}
					OAuthError oauthError2 = JsonUtility.FromJson<OAuthError>(resp.DataAsText);
					if (oauthError2 != null && !string.IsNullOrEmpty(oauthError2.error))
					{
						onComplete(oauthError2);
						return;
					}
					this.response = resp;
					onComplete(null);
				});
				this.request.AddHeader("User-Agent", CoreApplication.GetUserAgent());
				this.request.AddHeader("Cache-Control", "no-cache");
				this.request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
				foreach (object obj in parameters.Keys)
				{
					string text = (string)obj;
					this.request.AddField(text, parameters[text].ToString());
				}
				this.request.CustomCertificateVerifyer = new CertificateVerifier(this._restAPIPinPublicKeys);
				this.request.UseAlternateSSL = true;
				this.request.Send();
			}

			// Token: 0x04002DF2 RID: 11762
			public HTTPRequest request;

			// Token: 0x04002DF3 RID: 11763
			public HTTPResponse response;

			// Token: 0x04002DF4 RID: 11764
			private string[] _restAPIPinPublicKeys;
		}
	}
}

using System;
using System.Collections;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000959 RID: 2393
	public class SteamManagerAsmodee : ISteamManager, IDisposable
	{
		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06004049 RID: 16457 RVA: 0x0005157B File Offset: 0x0004F77B
		public PartnerAccount Me
		{
			get
			{
				return this.baseImplementation.Me;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x0600404A RID: 16458 RVA: 0x00051588 File Offset: 0x0004F788
		public bool HasClient
		{
			get
			{
				return this.baseImplementation.HasClient;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x0600404B RID: 16459 RVA: 0x00051595 File Offset: 0x0004F795
		public bool IsLoggedOn
		{
			get
			{
				return this.baseImplementation.IsLoggedOn;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x0600404C RID: 16460 RVA: 0x000515A2 File Offset: 0x0004F7A2
		public uint SteamAppID
		{
			get
			{
				return this.baseImplementation.SteamAppID;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x0600404D RID: 16461 RVA: 0x000515AF File Offset: 0x0004F7AF
		public string PlayerName
		{
			get
			{
				return this.baseImplementation.PlayerName;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x0600404E RID: 16462 RVA: 0x000515BC File Offset: 0x0004F7BC
		public ulong PlayerID
		{
			get
			{
				return this.baseImplementation.PlayerID;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x0600404F RID: 16463 RVA: 0x000515C9 File Offset: 0x0004F7C9
		public string SessionTicket
		{
			get
			{
				return this.baseImplementation.SessionTicket;
			}
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x000515D6 File Offset: 0x0004F7D6
		public SteamManagerAsmodee(uint steamGameID)
		{
			this.baseImplementation = new SteamManagerBase(steamGameID);
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x000515EA File Offset: 0x0004F7EA
		public void Update()
		{
			this.baseImplementation.Update();
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x000515F7 File Offset: 0x0004F7F7
		public void Dispose()
		{
			this.baseImplementation.Dispose();
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x0015DD2C File Offset: 0x0015BF2C
		public void LinkSteamAccount(OAuthGate gate, Action onComplete)
		{
			AsmoLogger.Debug("SteamManagerAsmodee.sender", "Trying to link a steam account...", null);
			if (onComplete == null)
			{
				throw new Exception("onComplete must not be null");
			}
			if (!this.HasClient)
			{
				AsmoLogger.Error("SteamManagerAsmodee", "Steam Client not instantiated", null);
				onComplete();
				return;
			}
			PartnerAccount partner = this.Me;
			new LinkUnlinkMultipleEndpoint(new PartnerAccount[] { partner }, null, gate).Execute(delegate(WebError error)
			{
				if (error == null)
				{
					AsmoLogger.Info("SteamManagerAsmodee.receiver", "Account successfully linked to steam", Reflection.HashtableFromObject(partner, null, 30U));
					onComplete();
					return;
				}
				Hashtable hashtable = new Hashtable
				{
					{ "status", error.status },
					{ "error", error.error }
				};
				ApiResponseError apiResponseError = error.ToChildError<ApiResponseError>();
				if (apiResponseError != null)
				{
					hashtable.Add("description", apiResponseError.error_description);
					hashtable.Add("error code", apiResponseError.error_code);
				}
				ApiResponseLinkUnlinkMultipleError apiResponseLinkUnlinkMultipleError = error.ToChildError<ApiResponseLinkUnlinkMultipleError>();
				if (apiResponseLinkUnlinkMultipleError != null)
				{
					hashtable.Add("error_details", Reflection.HashtableFromObject(apiResponseLinkUnlinkMultipleError.error_details, null, 30U));
				}
				AsmoLogger.Error("SteamManagerAsmodee.receiver", "Unable to link the account to steam", hashtable);
				onComplete();
			});
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x00051604 File Offset: 0x0004F804
		public bool SetAchievement(string pchName)
		{
			return this.baseImplementation.SetAchievement(pchName);
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x00051612 File Offset: 0x0004F812
		public bool GetUserAchievement(string pchName)
		{
			return this.baseImplementation.GetUserAchievement(pchName);
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x00051620 File Offset: 0x0004F820
		public void ResetAllAchievements()
		{
			this.baseImplementation.ResetAllAchievements();
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x0005162D File Offset: 0x0004F82D
		public bool IsInstalled(uint appid)
		{
			return this.baseImplementation.IsInstalled(appid);
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x0005163B File Offset: 0x0004F83B
		public bool IsDlcInstalled(uint appid)
		{
			return this.baseImplementation.IsDlcInstalled(appid);
		}

		// Token: 0x040030FA RID: 12538
		private const string _kConsoleModuleName = "SteamManagerAsmodee";

		// Token: 0x040030FB RID: 12539
		private SteamManagerBase baseImplementation;
	}
}

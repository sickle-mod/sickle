using System;
using System.Runtime.CompilerServices;
using System.Text;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x0200095B RID: 2395
	public class SteamManagerBase : IDisposable, ISteamManager
	{
		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x0600405B RID: 16475 RVA: 0x0015DE90 File Offset: 0x0015C090
		public PartnerAccount Me
		{
			get
			{
				if (!SteamClient.IsValid)
				{
					return null;
				}
				return new PartnerAccount(12, SteamClient.SteamId.ToString(), null);
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x0600405C RID: 16476 RVA: 0x00051649 File Offset: 0x0004F849
		public bool HasClient
		{
			get
			{
				return SteamClient.IsValid;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x0600405D RID: 16477 RVA: 0x00051650 File Offset: 0x0004F850
		public bool IsLoggedOn
		{
			get
			{
				return this.HasClient && SteamClient.IsLoggedOn;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x0600405E RID: 16478 RVA: 0x00051661 File Offset: 0x0004F861
		public uint SteamAppID
		{
			get
			{
				return SteamClient.AppId;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x0600405F RID: 16479 RVA: 0x0005166D File Offset: 0x0004F86D
		public string PlayerName
		{
			get
			{
				if (!SteamClient.IsValid)
				{
					return string.Empty;
				}
				return SteamClient.Name;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06004060 RID: 16480 RVA: 0x00051681 File Offset: 0x0004F881
		public ulong PlayerID
		{
			get
			{
				if (!SteamClient.IsValid)
				{
					return 0UL;
				}
				return SteamClient.SteamId;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06004061 RID: 16481 RVA: 0x00051697 File Offset: 0x0004F897
		public string SessionTicket
		{
			get
			{
				return BitConverter.ToString(SteamUser.GetAuthSessionTicket().Data).Replace("-", string.Empty);
			}
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x0015DECC File Offset: 0x0015C0CC
		public SteamManagerBase(uint steamGameId)
		{
			AsmoLogger.Info("SteamManagerBase.Steam", "Instantiating a Steam client", null);
			if (SteamClient.RestartAppIfNecessary(steamGameId))
			{
				AsmoLogger.Info("SteamManagerBase.Steam", "Restarting app.", null);
				Application.Quit();
				return;
			}
			try
			{
				SteamClient.Init(steamGameId, true);
			}
			catch (Exception ex)
			{
				AsmoLogger.Error("SteamManagerBase.Steam", "Couldn't initialize a Steam client: " + ex.Message + "\n" + ex.StackTrace, null);
			}
			if (!SteamClient.IsValid)
			{
				throw new Exception("Couldn't initialize a Steam client");
			}
			AsmoLogger.Info("SteamManagerBase.Steam", "Steam client created", null);
			if (!SteamClient.IsValid)
			{
				AsmoLogger.Info("SteamManagerBase.Steam", "Steam client not valid.", null);
				SteamClient.Shutdown();
				throw new Exception("Steam client has not been correctly instantiated.");
			}
			SteamUserStats.OnAchievementProgress += this.<.ctor>g__OnAchievementsProgress|16_1;
			SteamUserStats.OnUserStatsReceived += this.<.ctor>g__OnUserStatsReceived|16_0;
			AsmoLogger.Info("SteamManagerBase.Steam", "SteamManager instantiated correctly.", null);
			this.IsNetworkReachable();
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x000516B7 File Offset: 0x0004F8B7
		private void IsNetworkReachable()
		{
			CoreApplication.Instance.StartCoroutine(WebChecker.WebRequest(delegate
			{
				this.CallOnSuccess();
			}, delegate
			{
				this.CallOnFailure();
			}, "https://www.google.com"));
		}

		// Token: 0x06004064 RID: 16484 RVA: 0x000516E6 File Offset: 0x0004F8E6
		protected void CallOnSuccess()
		{
			while (!this.isReady)
			{
				this.Update();
			}
			AsmoLogger.Info("SteamManagerBase.Steam", "SteamManager instantiated correctly.", null);
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x00051708 File Offset: 0x0004F908
		protected void CallOnFailure()
		{
			Debug.LogError("No internet connection");
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x00051714 File Offset: 0x0004F914
		public void Update()
		{
			if (SteamClient.IsValid)
			{
				SteamClient.RunCallbacks();
			}
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x00051722 File Offset: 0x0004F922
		public void Dispose()
		{
			if (SteamClient.IsValid)
			{
				SteamClient.Shutdown();
				AsmoLogger.Debug("SteamManagerBase.Steam", "Steam client destroyed", null);
			}
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x00051740 File Offset: 0x0004F940
		public void LinkSteamAccount(OAuthGate gate, Action onComplete)
		{
			AsmoLogger.Error("SteamManagerBase", "Linkig steam account is not implemented in the base implementation!", null);
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x0015DFD4 File Offset: 0x0015C1D4
		public bool SetAchievement(string pchName)
		{
			global::Steamworks.Data.Achievement achievement = new global::Steamworks.Data.Achievement(pchName);
			return achievement.Trigger(true);
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x0015DFF4 File Offset: 0x0015C1F4
		public bool GetUserAchievement(string pchName)
		{
			bool flag = false;
			bool flag2 = false;
			global::Steamworks.Data.Achievement achievement2;
			foreach (global::Steamworks.Data.Achievement achievement in SteamUserStats.Achievements)
			{
				flag2 = true;
				if (achievement.Identifier == pchName)
				{
					achievement2 = achievement;
					flag = true;
					break;
				}
			}
			if (!flag2)
			{
				Debug.LogWarning("No achievements found");
				return false;
			}
			if (!flag)
			{
				Debug.LogWarning("Unable to find " + pchName);
				return flag;
			}
			return achievement2.State;
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x0015E084 File Offset: 0x0015C284
		public void ResetAllAchievements()
		{
			foreach (global::Steamworks.Data.Achievement achievement in SteamUserStats.Achievements)
			{
				achievement.Clear();
			}
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x00051752 File Offset: 0x0004F952
		public bool IsInstalled(uint appid)
		{
			return SteamApps.IsAppInstalled(appid);
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0005175F File Offset: 0x0004F95F
		public bool IsDlcInstalled(uint appid)
		{
			return SteamClient.IsValid && SteamApps.IsDlcInstalled(appid);
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x00051775 File Offset: 0x0004F975
		[CompilerGenerated]
		private void <.ctor>g__OnUserStatsReceived|16_0(SteamId steamID, Result result)
		{
			AsmoLogger.Info("SteamManagerBase.Steam", string.Format("User stats received. SteamID:{0}, Result : {1}", steamID, result), null);
			this.isReady = true;
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x0015E0D4 File Offset: 0x0015C2D4
		[CompilerGenerated]
		private void <.ctor>g__OnAchievementsProgress|16_1(global::Steamworks.Data.Achievement achievement, int currentProgress, int maxProgress)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Steam achievement updated");
			stringBuilder.AppendLine(achievement.ToString());
			stringBuilder.AppendLine(achievement.ToString());
			stringBuilder.AppendLine(string.Format("{0}:{1}", "currentProgress", currentProgress));
			stringBuilder.AppendLine(string.Format("{0}:{1}", "maxProgress", maxProgress));
			AsmoLogger.Info("SteamManagerBase.Steam", stringBuilder.ToString(), null);
			this.isReady = true;
		}

		// Token: 0x040030FE RID: 12542
		private const string _kConsoleModuleName = "SteamManagerBase";

		// Token: 0x040030FF RID: 12543
		private bool isReady;
	}
}

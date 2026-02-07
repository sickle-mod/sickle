using System;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AuthApi;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x0200095D RID: 2397
	public class SteamManagerTKOU : ISteamManager, IDisposable
	{
		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06004080 RID: 16512 RVA: 0x000517AF File Offset: 0x0004F9AF
		public PartnerAccount Me
		{
			get
			{
				return this.baseImplementation.Me;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06004081 RID: 16513 RVA: 0x000517BC File Offset: 0x0004F9BC
		public bool HasClient
		{
			get
			{
				return this.baseImplementation.HasClient;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06004082 RID: 16514 RVA: 0x000517C9 File Offset: 0x0004F9C9
		public bool IsLoggedOn
		{
			get
			{
				return this.baseImplementation.IsLoggedOn;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06004083 RID: 16515 RVA: 0x000517D6 File Offset: 0x0004F9D6
		public uint SteamAppID
		{
			get
			{
				return this.baseImplementation.SteamAppID;
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06004084 RID: 16516 RVA: 0x000517E3 File Offset: 0x0004F9E3
		public string PlayerName
		{
			get
			{
				return this.baseImplementation.PlayerName;
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06004085 RID: 16517 RVA: 0x000517F0 File Offset: 0x0004F9F0
		public ulong PlayerID
		{
			get
			{
				return this.baseImplementation.PlayerID;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06004086 RID: 16518 RVA: 0x000517FD File Offset: 0x0004F9FD
		public string SessionTicket
		{
			get
			{
				return this.baseImplementation.SessionTicket;
			}
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x0005180A File Offset: 0x0004FA0A
		public SteamManagerTKOU(uint steamGameID)
		{
			this.baseImplementation = new SteamManagerBase(steamGameID);
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x0005181E File Offset: 0x0004FA1E
		public void Update()
		{
			this.baseImplementation.Update();
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x0005182B File Offset: 0x0004FA2B
		public void Dispose()
		{
			this.baseImplementation.Dispose();
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x0015E16C File Offset: 0x0015C36C
		public void LinkSteamAccount(OAuthGate gate, Action onComplete)
		{
			SteamManagerTKOU.<>c__DisplayClass18_0 CS$<>8__locals1 = new SteamManagerTKOU.<>c__DisplayClass18_0();
			CS$<>8__locals1.onComplete = onComplete;
			Debug.Log("Trying to link a steam account...");
			if (CS$<>8__locals1.onComplete == null)
			{
				throw new Exception("onComplete must not be null");
			}
			if (!this.HasClient)
			{
				Debug.LogError("Steam Client not instantiated");
				CS$<>8__locals1.onComplete();
				return;
			}
			new AuthApiConnection().TryToLinkPlatformAccount(new Action(CS$<>8__locals1.<LinkSteamAccount>g__OnSuccess|1), new Action(CS$<>8__locals1.<LinkSteamAccount>g__OnFailure|0));
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x00051838 File Offset: 0x0004FA38
		public bool SetAchievement(string pchName)
		{
			return this.baseImplementation.SetAchievement(pchName);
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x00051846 File Offset: 0x0004FA46
		public bool GetUserAchievement(string pchName)
		{
			return this.baseImplementation.GetUserAchievement(pchName);
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x00051854 File Offset: 0x0004FA54
		public void ResetAllAchievements()
		{
			this.baseImplementation.ResetAllAchievements();
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x00051861 File Offset: 0x0004FA61
		public bool IsInstalled(uint appid)
		{
			return this.baseImplementation.IsInstalled(appid);
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x0005186F File Offset: 0x0004FA6F
		public bool IsDlcInstalled(uint appid)
		{
			return this.baseImplementation.IsDlcInstalled(appid);
		}

		// Token: 0x04003100 RID: 12544
		private SteamManagerBase baseImplementation;
	}
}

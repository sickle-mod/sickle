using System;
using System.Collections.Generic;
using AsmodeeNet.Foundation;
using I2.Loc;
using UnityEngine;

namespace Scythe.Platforms
{
	// Token: 0x020001F2 RID: 498
	public class SteamService : IGameService
	{
		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06000E7C RID: 3708 RVA: 0x0008A474 File Offset: 0x00088674
		// (remove) Token: 0x06000E7D RID: 3709 RVA: 0x0008A4AC File Offset: 0x000886AC
		public event Action AchievementsStateRetrieved;

		// Token: 0x06000E7E RID: 3710 RVA: 0x0008A4E4 File Offset: 0x000886E4
		public void ConnectWithGameServiceAccount(Action onLoginSuccess, Action<string> onLoginFailure, Action<string> onLoginError)
		{
			if (this.steamManager != null && this.steamManager.IsLoggedOn && this.AchievementsStateRetrieved != null)
			{
				this.AchievementsStateRetrieved();
			}
			if (!Application.isEditor && Application.internetReachability == NetworkReachability.NotReachable)
			{
				if (onLoginError != null)
				{
					onLoginError("Error: No internet connection");
				}
				return;
			}
			if (this.steamManager == null)
			{
				if (onLoginError != null)
				{
					onLoginError("Steam is not initialized! Can't connect with game service.");
				}
				return;
			}
			if (onLoginSuccess != null)
			{
				onLoginSuccess();
			}
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00031686 File Offset: 0x0002F886
		public void GetIdentityVerificationSignature(Action<object> callback)
		{
			UniversalInvocator.Event_Invocator<object>(callback, new object[] { this.steamManager.SessionTicket });
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x000316A2 File Offset: 0x0002F8A2
		public bool IsPlayerSignedIn()
		{
			return this.steamManager != null && this.steamManager.IsLoggedOn;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0008A558 File Offset: 0x00088758
		public string PlayerId()
		{
			if (this.steamManager == null)
			{
				return string.Empty;
			}
			return this.steamManager.PlayerID.ToString();
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x000316B9 File Offset: 0x0002F8B9
		public string DisplayName()
		{
			if (this.steamManager == null)
			{
				return string.Empty;
			}
			return this.steamManager.PlayerName;
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x000316D4 File Offset: 0x0002F8D4
		public bool IsAchievementUnlocked(Achievements achievement)
		{
			return this.steamManager != null && this.steamManager.GetUserAchievement(achievement.ToString());
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x000316F8 File Offset: 0x0002F8F8
		public void SetAchievement(Achievements achievement)
		{
			this.steamManager.SetAchievement(achievement.ToString());
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x0008A588 File Offset: 0x00088788
		public void SetAchievements(IEnumerable<Achievements> achievements)
		{
			foreach (Achievements achievements2 in achievements)
			{
				this.SetAchievement(achievements2);
			}
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00031713 File Offset: 0x0002F913
		public void ResetAllAchievements()
		{
			this.steamManager.ResetAllAchievements();
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00031720 File Offset: 0x0002F920
		public void ShowAchievementsList()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x0008A5D0 File Offset: 0x000887D0
		public bool IsDlcInstalled(DLCs dlc)
		{
			uint dlcid = this.GetDLCID(dlc);
			return dlcid != 0U && this.steamManager.IsDlcInstalled(dlcid);
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00031727 File Offset: 0x0002F927
		private uint GetDLCID(DLCs dlc)
		{
			if (dlc == DLCs.InvadersFromAfar)
			{
				return 1021190U;
			}
			return 0U;
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00031733 File Offset: 0x0002F933
		public string GetDlcWebPage(DLCs dlc)
		{
			return "https://store.steampowered.com/app/1021190";
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x0003173A File Offset: 0x0002F93A
		public string GetCurrentConnectionError()
		{
			if (!this.IsPlayerSignedIn())
			{
				return ScriptLocalization.Get("MainMenu/AuthenticationNoSteamFound");
			}
			return string.Empty;
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00031754 File Offset: 0x0002F954
		public string SessionTicket()
		{
			if (this.steamManager == null)
			{
				return "Steam not running!";
			}
			return this.steamManager.SessionTicket;
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x000283F8 File Offset: 0x000265F8
		public bool IsGoogleGamePlayAvailable()
		{
			return true;
		}

		// Token: 0x04000B5A RID: 2906
		private ISteamManager steamManager = CoreApplication.Instance.OAuthGate.SteamManager;
	}
}

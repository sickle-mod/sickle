using System;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;

namespace AsmodeeNet.Foundation
{
	// Token: 0x0200095C RID: 2396
	public interface ISteamManager : IDisposable
	{
		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06004072 RID: 16498
		bool HasClient { get; }

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06004073 RID: 16499
		bool IsLoggedOn { get; }

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06004074 RID: 16500
		PartnerAccount Me { get; }

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06004075 RID: 16501
		ulong PlayerID { get; }

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06004076 RID: 16502
		string PlayerName { get; }

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06004077 RID: 16503
		string SessionTicket { get; }

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06004078 RID: 16504
		uint SteamAppID { get; }

		// Token: 0x06004079 RID: 16505
		bool GetUserAchievement(string pchName);

		// Token: 0x0600407A RID: 16506
		bool IsDlcInstalled(uint appid);

		// Token: 0x0600407B RID: 16507
		bool IsInstalled(uint appid);

		// Token: 0x0600407C RID: 16508
		void LinkSteamAccount(OAuthGate gate, Action onComplete);

		// Token: 0x0600407D RID: 16509
		void ResetAllAchievements();

		// Token: 0x0600407E RID: 16510
		bool SetAchievement(string pchName);

		// Token: 0x0600407F RID: 16511
		void Update();
	}
}

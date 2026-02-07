using System;
using System.Collections.Generic;

// Token: 0x02000065 RID: 101
public interface IGameService
{
	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06000352 RID: 850
	// (remove) Token: 0x06000353 RID: 851
	event Action AchievementsStateRetrieved;

	// Token: 0x06000354 RID: 852
	void ConnectWithGameServiceAccount(Action onLogicSuccess, Action<string> onLogicFailure, Action<string> onLogicError);

	// Token: 0x06000355 RID: 853
	void GetIdentityVerificationSignature(Action<object> callback);

	// Token: 0x06000356 RID: 854 RVA: 0x00027EF0 File Offset: 0x000260F0
	void ClearCallbacks()
	{
	}

	// Token: 0x06000357 RID: 855
	bool IsPlayerSignedIn();

	// Token: 0x06000358 RID: 856
	string PlayerId();

	// Token: 0x06000359 RID: 857
	string DisplayName();

	// Token: 0x0600035A RID: 858
	bool IsGoogleGamePlayAvailable();

	// Token: 0x0600035B RID: 859
	void SetAchievement(Achievements achievement);

	// Token: 0x0600035C RID: 860
	void SetAchievements(IEnumerable<Achievements> achievements);

	// Token: 0x0600035D RID: 861
	bool IsAchievementUnlocked(Achievements achievement);

	// Token: 0x0600035E RID: 862
	void ResetAllAchievements();

	// Token: 0x0600035F RID: 863
	void ShowAchievementsList();

	// Token: 0x06000360 RID: 864
	bool IsDlcInstalled(DLCs dlc);

	// Token: 0x06000361 RID: 865
	string GetDlcWebPage(DLCs dlc);

	// Token: 0x06000362 RID: 866
	string GetCurrentConnectionError();
}

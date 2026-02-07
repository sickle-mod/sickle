using System;
using System.Collections.Generic;

// Token: 0x02000060 RID: 96
public class DummyGameService : IGameService
{
	// Token: 0x1400001A RID: 26
	// (add) Token: 0x06000329 RID: 809 RVA: 0x00027EF0 File Offset: 0x000260F0
	// (remove) Token: 0x0600032A RID: 810 RVA: 0x00027EF0 File Offset: 0x000260F0
	public event Action AchievementsStateRetrieved
	{
		add
		{
		}
		remove
		{
		}
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0002A1AB File Offset: 0x000283AB
	public void ConnectWithGameServiceAccount(Action onLogicSuccess, Action<string> onLogicFailure, Action<string> onLogicError)
	{
		UniversalInvocator.Event_Invocator(onLogicSuccess);
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0002A1B3 File Offset: 0x000283B3
	public void GetIdentityVerificationSignature(Action<object> callback)
	{
		UniversalInvocator.Event_Invocator<object>(callback, new object[] { string.Empty });
	}

	// Token: 0x0600032D RID: 813 RVA: 0x000283F8 File Offset: 0x000265F8
	public bool IsPlayerSignedIn()
	{
		return true;
	}

	// Token: 0x0600032E RID: 814 RVA: 0x0002A1C9 File Offset: 0x000283C9
	public string PlayerId()
	{
		return this.playerId;
	}

	// Token: 0x0600032F RID: 815 RVA: 0x0002A1D1 File Offset: 0x000283D1
	public string DisplayName()
	{
		return this.playerName;
	}

	// Token: 0x06000330 RID: 816 RVA: 0x000283F8 File Offset: 0x000265F8
	public bool IsGoogleGamePlayAvailable()
	{
		return true;
	}

	// Token: 0x06000331 RID: 817 RVA: 0x000283F8 File Offset: 0x000265F8
	public bool IsAchievementUnlocked(Achievements achievement)
	{
		return true;
	}

	// Token: 0x06000332 RID: 818 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void ResetAllAchievements()
	{
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void SetAchievement(Achievements achievement)
	{
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void ShowAchievementsList()
	{
	}

	// Token: 0x06000335 RID: 821 RVA: 0x0002A1D9 File Offset: 0x000283D9
	public bool IsDlcInstalled(DLCs dlc)
	{
		return false;
	}

	// Token: 0x06000336 RID: 822 RVA: 0x0002A1DC File Offset: 0x000283DC
	public string GetDlcWebPage(DLCs dlc)
	{
		return string.Empty;
	}

	// Token: 0x06000337 RID: 823 RVA: 0x0002A1DC File Offset: 0x000283DC
	public string GetCurrentConnectionError()
	{
		return string.Empty;
	}

	// Token: 0x06000338 RID: 824 RVA: 0x0002A1DC File Offset: 0x000283DC
	public string SessionTicket()
	{
		return string.Empty;
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void SetAchievements(IEnumerable<Achievements> achievements)
	{
	}

	// Token: 0x04000329 RID: 809
	private string playerName = "DummyAccountName";

	// Token: 0x0400032A RID: 810
	private string playerId = "-0";
}

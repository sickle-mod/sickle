using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class GameServiceController : MonoBehaviour
{
	// Token: 0x17000031 RID: 49
	// (get) Token: 0x0600033B RID: 827 RVA: 0x0002A201 File Offset: 0x00028401
	// (set) Token: 0x0600033C RID: 828 RVA: 0x0002A208 File Offset: 0x00028408
	public static GameServiceController Instance
	{
		get
		{
			return GameServiceController.instance;
		}
		private set
		{
			GameServiceController.instance = value;
		}
	}

	// Token: 0x0600033D RID: 829 RVA: 0x00061B50 File Offset: 0x0005FD50
	protected void Awake()
	{
		if (GameServiceController.Instance == null)
		{
			GameServiceController.Instance = this;
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			this.gameService = GameServiceFactory.GetGameServiceForCurrentPlatform();
			this.gameService.AchievementsStateRetrieved += this.GameService_AchievementsStateRetrieved;
			return;
		}
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600033E RID: 830 RVA: 0x0002A210 File Offset: 0x00028410
	private void GameService_AchievementsStateRetrieved()
	{
		AchievementManager.GenerateGainedAwardsList();
	}

	// Token: 0x0600033F RID: 831 RVA: 0x0002A217 File Offset: 0x00028417
	public void ConnectWithGameServiceAccount(Action onLoginSuccess, Action<string> onLoginFailure, Action<string> onLoginError)
	{
		this.gameService.ConnectWithGameServiceAccount(onLoginSuccess, onLoginFailure, onLoginError);
	}

	// Token: 0x06000340 RID: 832 RVA: 0x0002A227 File Offset: 0x00028427
	public void GetIdentityVerificationSignature(Action<object> callback)
	{
		this.gameService.GetIdentityVerificationSignature(callback);
	}

	// Token: 0x06000341 RID: 833 RVA: 0x0002A235 File Offset: 0x00028435
	public bool IsPlayerSignedIn()
	{
		return this.gameService.IsPlayerSignedIn();
	}

	// Token: 0x06000342 RID: 834 RVA: 0x0002A242 File Offset: 0x00028442
	public string PlayerId()
	{
		return this.gameService.PlayerId();
	}

	// Token: 0x06000343 RID: 835 RVA: 0x0002A24F File Offset: 0x0002844F
	public string DisplayName()
	{
		return this.gameService.DisplayName();
	}

	// Token: 0x06000344 RID: 836 RVA: 0x0002A25C File Offset: 0x0002845C
	public void SetAchievement(Achievements achievement)
	{
		this.gameService.SetAchievement(achievement);
	}

	// Token: 0x06000345 RID: 837 RVA: 0x0002A26A File Offset: 0x0002846A
	public void SetAchievements(IEnumerable<Achievements> achievements)
	{
		this.gameService.SetAchievements(achievements);
	}

	// Token: 0x06000346 RID: 838 RVA: 0x0002A278 File Offset: 0x00028478
	public bool IsAchievementUnlocked(Achievements achievement)
	{
		return this.gameService.IsAchievementUnlocked(achievement);
	}

	// Token: 0x06000347 RID: 839 RVA: 0x0002A286 File Offset: 0x00028486
	public void ResetAllAchievements()
	{
		this.gameService.ResetAllAchievements();
	}

	// Token: 0x06000348 RID: 840 RVA: 0x0002A293 File Offset: 0x00028493
	public void ShowAchievementsList()
	{
		this.gameService.ShowAchievementsList();
	}

	// Token: 0x06000349 RID: 841 RVA: 0x0002A2A0 File Offset: 0x000284A0
	public bool InvadersFromAfarUnlocked()
	{
		return this.gameService.IsDlcInstalled(DLCs.InvadersFromAfar);
	}

	// Token: 0x0600034A RID: 842 RVA: 0x0002A2AE File Offset: 0x000284AE
	public string GetDlcWebPage(DLCs dlc)
	{
		return this.gameService.GetDlcWebPage(dlc);
	}

	// Token: 0x0600034B RID: 843 RVA: 0x0002A2BC File Offset: 0x000284BC
	public bool IsDummyGameService()
	{
		return this.gameService is DummyGameService;
	}

	// Token: 0x0600034C RID: 844 RVA: 0x0002A2CC File Offset: 0x000284CC
	public string GetCurrentConnectionError()
	{
		return this.gameService.GetCurrentConnectionError();
	}

	// Token: 0x0600034D RID: 845 RVA: 0x0002A2D9 File Offset: 0x000284D9
	public bool IsAndroidGoogleGamesAvailable()
	{
		return this.gameService.IsGoogleGamePlayAvailable();
	}

	// Token: 0x0600034E RID: 846 RVA: 0x0002A2E6 File Offset: 0x000284E6
	internal void ClearCallbacks()
	{
		this.gameService.ClearCallbacks();
	}

	// Token: 0x0400032B RID: 811
	public const string GOOGLE_PLAY_CLIENT_ID = "250865451980-ur2sp7qks2vj0n34p0qan718ek12ercb.apps.googleusercontent.com";

	// Token: 0x0400032C RID: 812
	private IGameService gameService;

	// Token: 0x0400032D RID: 813
	private static GameServiceController instance;
}

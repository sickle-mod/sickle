using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

// Token: 0x02000066 RID: 102
public class MobileGameServiceController : IGameService
{
	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06000363 RID: 867 RVA: 0x00061BAC File Offset: 0x0005FDAC
	// (remove) Token: 0x06000364 RID: 868 RVA: 0x00061BE4 File Offset: 0x0005FDE4
	public event Action AchievementsStateRetrieved;

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x06000365 RID: 869 RVA: 0x00061C1C File Offset: 0x0005FE1C
	// (remove) Token: 0x06000366 RID: 870 RVA: 0x00061C54 File Offset: 0x0005FE54
	private event Action OnAuthenticationSuccess;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x06000367 RID: 871 RVA: 0x00061C8C File Offset: 0x0005FE8C
	// (remove) Token: 0x06000368 RID: 872 RVA: 0x00061CC4 File Offset: 0x0005FEC4
	private event Action<string> OnAuthenticationFail;

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x06000369 RID: 873 RVA: 0x00061CFC File Offset: 0x0005FEFC
	// (remove) Token: 0x0600036A RID: 874 RVA: 0x00061D34 File Offset: 0x0005FF34
	private event Action<string> OnAuthenticationError;

	// Token: 0x0600036B RID: 875 RVA: 0x0002A2FA File Offset: 0x000284FA
	public void ConnectWithGameServiceAccount(Action onLogicSuccess, Action<string> onLogicFailure, Action<string> onLogicError)
	{
		this.OnAuthenticationSuccess = onLogicSuccess;
		this.OnAuthenticationFail = onLogicFailure;
		this.OnAuthenticationError = onLogicError;
		this.PerformAuthentication();
	}

	// Token: 0x0600036C RID: 876 RVA: 0x0002A317 File Offset: 0x00028517
	public void GetIdentityVerificationSignature(Action<object> callback)
	{
		if (!GameServices.IsAuthenticated)
		{
			callback(string.Empty);
			return;
		}
		GameServices.LocalPlayer.GetIdentityVerificationSignature(callback);
	}

	// Token: 0x0600036D RID: 877 RVA: 0x0002A337 File Offset: 0x00028537
	public bool IsPlayerSignedIn()
	{
		return GameServices.IsAuthenticated;
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0002A33E File Offset: 0x0002853E
	public string PlayerId()
	{
		if (this.IsPlayerSignedIn())
		{
			return GameServices.LocalPlayer.LegacyId;
		}
		return string.Empty;
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0002A358 File Offset: 0x00028558
	public string DisplayName()
	{
		if (this.IsPlayerSignedIn())
		{
			return GameServices.LocalPlayer.DisplayName;
		}
		return string.Empty;
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00061D6C File Offset: 0x0005FF6C
	public bool IsGoogleGamePlayAvailable()
	{
		if (PlatformManager.IsAndroid)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getPackageManager", Array.Empty<object>());
			AndroidJavaObject androidJavaObject2 = null;
			try
			{
				androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getLaunchIntentForPackage", new object[] { "com.google.android.play.games" });
			}
			catch (Exception ex)
			{
				Debug.Log("exception" + ex.Message);
				return false;
			}
			return androidJavaObject2 != null;
		}
		return true;
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00061DF4 File Offset: 0x0005FFF4
	public void SetAchievement(Achievements achievement)
	{
		if (this.IsPlayerSignedIn())
		{
			string text = this.PrepareAchievementId(achievement);
			Debug.Log("JACK achiv: " + text);
			if (!string.IsNullOrEmpty(text))
			{
				Debug.Log("Sir JAck one" + text);
				GameServices.ReportAchievementProgress(text, 100.0, new CompletionCallback(this.OnAchievementUnlockResult));
			}
		}
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00061E54 File Offset: 0x00060054
	public void SetAchievements(IEnumerable<Achievements> achievements)
	{
		if (this.IsPlayerSignedIn())
		{
			new List<string>();
			foreach (Achievements achievements2 in achievements)
			{
				string text = this.PrepareAchievementId(achievements2);
				Debug.Log("JACK achiv: " + text);
				if (!string.IsNullOrEmpty(text))
				{
					Debug.Log("Sir JAck List" + text);
					GameServices.ReportAchievementProgress(text, 100.0, new CompletionCallback(this.OnAchievementUnlockResult));
				}
			}
		}
	}

	// Token: 0x06000373 RID: 883 RVA: 0x00061EF0 File Offset: 0x000600F0
	public bool IsAchievementUnlocked(Achievements achievement)
	{
		if (this.achievementsState == null)
		{
			Debug.LogError("Achievements list is null!!");
			return false;
		}
		string achievementId = this.PrepareAchievementId(achievement);
		this.achievementStateList.AddRange(this.achievementsState);
		IAchievement achievement2 = this.achievementStateList.Find((IAchievement US_Achievement) => US_Achievement != null && US_Achievement.Id.Equals(achievementId));
		return achievement2 != null && achievement2.IsCompleted;
	}

	// Token: 0x06000374 RID: 884 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void ResetAllAchievements()
	{
	}

	// Token: 0x06000375 RID: 885 RVA: 0x00061F58 File Offset: 0x00060158
	public bool IsDlcInstalled(DLCs dlc)
	{
		string text = PaymentServiceController.Instance.DLCId(dlc);
		return PaymentServiceController.Instance.IsProductPurchased(text);
	}

	// Token: 0x06000376 RID: 886 RVA: 0x0002A372 File Offset: 0x00028572
	public string GetDlcWebPage(DLCs dlc)
	{
		return PaymentServiceController.Instance.DLCId(dlc);
	}

	// Token: 0x06000377 RID: 887 RVA: 0x0002A37F File Offset: 0x0002857F
	public string GetCurrentConnectionError()
	{
		if (!this.IsPlayerSignedIn())
		{
			return "Please login to GameCenter or Google Play.";
		}
		return string.Empty;
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0002A394 File Offset: 0x00028594
	public string SessionTicket()
	{
		throw new NotImplementedException("[MobileGameServiceController] SessionTicket not implemented!");
	}

	// Token: 0x06000379 RID: 889 RVA: 0x0002A3A0 File Offset: 0x000285A0
	private void PerformAuthenticationIfGooglePlayServicesAvailable(Error result)
	{
		if (GameServices.IsAvailable())
		{
			this.PerformAuthentication();
			return;
		}
		Debug.LogError("[MobileGameServiceController] Google Play Services not available. Killing application.");
		Application.Quit();
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00061F7C File Offset: 0x0006017C
	private void PerformAuthentication()
	{
		if (!Application.isEditor)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				this.OnAuthenticationFail("[MobileGameServiceController] Error: No internet connection");
				return;
			}
			if (!GameServices.IsAvailable())
			{
				this.OnAuthenticationFail("[MobileGameServiceController] Error: Game Services are not available. Player will not be signed in.");
				return;
			}
		}
		GameServices.OnAuthStatusChange += this.PrintPlayerInfo;
		GameServices.OnAuthStatusChange += this.ProcessAuthentication;
		GameServices.Authenticate();
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00061FE8 File Offset: 0x000601E8
	private void ProcessAuthentication(GameServicesAuthStatusChangeResult result, Error error)
	{
		if (result.AuthStatus == LocalPlayerAuthStatus.Authenticated)
		{
			PaymentServiceController.Instance.AuthenticatePaymentService(new Action(this.PaymentServiceController_OnAuthenticationSuccess), new Action<string>(this.PaymentServiceController_OnAuthenticationFail));
			this.FetchAchievementsState();
			return;
		}
		if (error != null)
		{
			Debug.Log(error.Description);
			UniversalInvocator.Event_Invocator<string>(this.OnAuthenticationFail, new object[] { error.Description });
			return;
		}
		Debug.LogError(string.Format("Authentication error is null with authentication status: {0}", result.AuthStatus));
		UniversalInvocator.Event_Invocator<string>(this.OnAuthenticationFail, new object[] { string.Format("Something went really wrong, Authentication error is null. Authentication status: {0}", result.AuthStatus) });
	}

	// Token: 0x0600037C RID: 892 RVA: 0x0002A3BF File Offset: 0x000285BF
	private void PaymentServiceController_OnAuthenticationSuccess()
	{
		Action onAuthenticationSuccess = this.OnAuthenticationSuccess;
		if (onAuthenticationSuccess == null)
		{
			return;
		}
		onAuthenticationSuccess();
	}

	// Token: 0x0600037D RID: 893 RVA: 0x0002A3D1 File Offset: 0x000285D1
	private void PaymentServiceController_OnAuthenticationFail(string error)
	{
		Action<string> onAuthenticationFail = this.OnAuthenticationFail;
		if (onAuthenticationFail == null)
		{
			return;
		}
		onAuthenticationFail(error);
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00062094 File Offset: 0x00060294
	private void PrintPlayerInfo(GameServicesAuthStatusChangeResult result, Error error)
	{
		if (result != null)
		{
			Debug.Log("Received auth status change event");
			Debug.Log("Auth status: " + result.AuthStatus.ToString());
			if (result.AuthStatus == LocalPlayerAuthStatus.Authenticated)
			{
				if (result.LocalPlayer != null)
				{
					string[] array = new string[8];
					array[0] = "player.State: ";
					array[1] = result.LocalPlayer.LegacyId;
					array[2] = "\n";
					array[3] = result.LocalPlayer.Alias;
					array[4] = "\n";
					array[5] = result.LocalPlayer.DisplayName;
					array[6] = "\n";
					int num = 7;
					ILocalPlayer localPlayer = result.LocalPlayer;
					array[num] = ((localPlayer != null) ? localPlayer.ToString() : null);
					Debug.Log(string.Concat(array));
					return;
				}
				Debug.Log("LocalPlayer is null");
				return;
			}
		}
		else
		{
			Debug.Log("PrintPlayerInfo GameServicesAuthStatusChangeResult result is null");
		}
	}

	// Token: 0x0600037F RID: 895 RVA: 0x0002A3E4 File Offset: 0x000285E4
	private void FetchAchievementsState()
	{
		GameServices.LoadAchievements(delegate(GameServicesLoadAchievementsResult result, Error error)
		{
			if (error == null)
			{
				this.achievementsState = result.Achievements;
				Debug.Log("Request to load achievements finished successfully.");
				Debug.Log("Total achievements fetched: " + this.achievementsState.Length.ToString());
				Debug.Log("Below are the available achievements:");
				for (int i = 0; i < this.achievementsState.Length; i++)
				{
					IAchievement achievement = this.achievementsState[i];
					Debug.Log(string.Format("[{0}]: {1}", i, achievement));
				}
				if (this.AchievementsStateRetrieved != null)
				{
					this.AchievementsStateRetrieved();
					return;
				}
			}
			else
			{
				Debug.LogError("Request to load achievements failed with error. Error: " + ((error != null) ? error.ToString() : null));
			}
		});
	}

	// Token: 0x06000380 RID: 896 RVA: 0x0002A3F7 File Offset: 0x000285F7
	private string PrepareAchievementId(Achievements achievement)
	{
		return achievement.ToString();
	}

	// Token: 0x06000381 RID: 897 RVA: 0x0002A406 File Offset: 0x00028606
	private void OnAchievementUnlockResult(bool success, Error error)
	{
		if (success)
		{
			this.FetchAchievementsState();
			Debug.Log("Achievement unlocked ");
			return;
		}
		Debug.Log("Failed to unlock " + error.Description);
	}

	// Token: 0x06000382 RID: 898 RVA: 0x0002A431 File Offset: 0x00028631
	public void ShowAchievementsList()
	{
		GameServices.ShowAchievements(delegate(GameServicesViewResult result, Error error)
		{
			this.LogViewClosed();
		});
	}

	// Token: 0x06000383 RID: 899 RVA: 0x0002A444 File Offset: 0x00028644
	private void LogViewClosed()
	{
		Debug.Log("View is closed");
	}

	// Token: 0x06000384 RID: 900 RVA: 0x0002A450 File Offset: 0x00028650
	public void ClearCallbacks()
	{
		Debug.Log("ClearCallbacks");
		this.OnAuthenticationSuccess = null;
		this.OnAuthenticationFail = null;
		this.OnAuthenticationError = null;
	}

	// Token: 0x04000336 RID: 822
	private IAchievement[] achievementsState;

	// Token: 0x04000337 RID: 823
	private const string GameCenterAchievementPrefix = "grp.scythe.";

	// Token: 0x0400033B RID: 827
	private List<IAchievement> achievementStateList = new List<IAchievement>();
}

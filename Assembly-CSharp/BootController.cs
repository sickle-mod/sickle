using System;
using Multiplayer.AuthApi;
using Newtonsoft.Json.Converters;
using Scythe.Analytics;
using Scythe.Multiplayer;
using Scythe.Multiplayer.AuthApi.Models;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

// Token: 0x02000013 RID: 19
public class BootController : SingletonMono<BootController>
{
	// Token: 0x0600003E RID: 62 RVA: 0x00027FE0 File Offset: 0x000261E0
	private void Awake()
	{
		new StringEnumConverter();
		SceneManager.activeSceneChanged += this.ActiveSceneChanged;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00027FF9 File Offset: 0x000261F9
	private void Start()
	{
		this.CreateAnalyticsEventLogger();
		KeyValueStore.Instance = new PlayerPrefsKeyValueStore();
	}

	// Token: 0x06000040 RID: 64 RVA: 0x0002800B File Offset: 0x0002620B
	private void OnDestroy()
	{
		SceneManager.activeSceneChanged -= this.ActiveSceneChanged;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x0002801E File Offset: 0x0002621E
	private void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus)
		{
			return;
		}
		if (PlatformManager.IsAndroid && this.currentState == BootController.ConnectionWithGameServiceState.ConnectFailed && !GameServiceController.Instance.IsPlayerSignedIn() && GameServiceController.Instance.IsAndroidGoogleGamesAvailable())
		{
			this.LogInToGameServices();
		}
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00053090 File Offset: 0x00051290
	private void Update()
	{
		if ((this.currentState == BootController.ConnectionWithGameServiceState.ConnectSuccessful || this.currentState == BootController.ConnectionWithGameServiceState.ConnectFailed) && this.menuSceneLoading != null && this.menuSceneLoading.isDone && this.splashscreenController.HasEnded())
		{
			SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
			base.StartCoroutine(AssetBundleManager.LoadAssetBundleAsync("graphic_backgrounds"));
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00028052 File Offset: 0x00026252
	private void ActiveSceneChanged(Scene current, Scene next)
	{
		if (next.name != "boot")
		{
			return;
		}
		this.LogInToGameServices();
	}

	// Token: 0x06000044 RID: 68 RVA: 0x0002806E File Offset: 0x0002626E
	private void LogInToGameServices()
	{
		if (this.currentState != BootController.ConnectionWithGameServiceState.Connecting)
		{
			this.currentState = BootController.ConnectionWithGameServiceState.Connecting;
			GameServiceController.Instance.ConnectWithGameServiceAccount(new Action(this.GameServiceController_OnPlatformLoginSuccess), new Action<string>(this.GameServiceController_OnPlatformLoginFailure), new Action<string>(this.GameServiceController_OnPlatformLoginError));
		}
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000280AE File Offset: 0x000262AE
	private void CreateAnalyticsEventLogger()
	{
		AnalyticsEventLogger.Instance.Create();
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000280BA File Offset: 0x000262BA
	private void AttachLoginEvents()
	{
		LoginController instance = Singleton<LoginController>.Instance;
		instance.OnLoginSuccess += this.LoginController_OnLoginSuccess;
		instance.OnLoginFailure += this.LoginController_OnLoginFailure;
		instance.OnLoginError += this.LoginController_OnLoginError;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000280F6 File Offset: 0x000262F6
	private void DetachLoginEvents()
	{
		LoginController instance = Singleton<LoginController>.Instance;
		instance.OnLoginSuccess -= this.LoginController_OnLoginSuccess;
		instance.OnLoginFailure -= this.LoginController_OnLoginFailure;
		instance.OnLoginError -= this.LoginController_OnLoginError;
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00028132 File Offset: 0x00026332
	private void GameServiceController_OnPlatformLoginSuccess()
	{
		Debug.Log("GameServiceController_OnPlatformLoginSuccess Login Successful. Attempting to login using the normal account.");
		GameServiceController.Instance.ClearCallbacks();
		this.AttachLoginEvents();
		Singleton<LoginController>.Instance.TryToAutoLogin(false);
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00028159 File Offset: 0x00026359
	private void GameServiceController_OnPlatformLoginFailure(string exception)
	{
		Debug.LogWarning("GameServiceController_OnPlatformLoginFailure " + exception + ". Attempting to login using the normal account.");
		GameServiceController.Instance.ClearCallbacks();
		this.AttachLoginEvents();
		Singleton<LoginController>.Instance.TryToAutoLogin(false);
	}

	// Token: 0x0600004A RID: 74 RVA: 0x0002818B File Offset: 0x0002638B
	private void GameServiceController_OnPlatformLoginError(string exception)
	{
		Debug.LogError("GameServiceController_OnPlatformLoginError " + exception + ". Login error! Attempting to use normal account.");
		GameServiceController.Instance.ClearCallbacks();
		this.AttachLoginEvents();
		Singleton<LoginController>.Instance.TryToAutoLogin(false);
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000281BD File Offset: 0x000263BD
	private void LoginController_OnLoginSuccess(LoginResponse dto)
	{
		this.DetachLoginEvents();
		Debug.Log(string.Format("{0} {1}. login successful", "LoginController_OnLoginSuccess", dto.Result));
		this.OnLoginFinished(BootController.ConnectionWithGameServiceState.ConnectSuccessful);
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000281EB File Offset: 0x000263EB
	private void LoginController_OnLoginFailure(FailureResponse response)
	{
		this.DetachLoginEvents();
		Debug.LogWarning("LoginController_OnLoginFailure " + response.GetErrorsString() + ". Failed to log in.");
		this.OnLoginFinished(BootController.ConnectionWithGameServiceState.ConnectFailed);
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00028214 File Offset: 0x00026414
	private void LoginController_OnLoginError(string message)
	{
		this.DetachLoginEvents();
		Debug.LogError("LoginController_OnLoginError " + message + ". Login error!");
		this.OnLoginFinished(BootController.ConnectionWithGameServiceState.ConnectFailed);
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00028238 File Offset: 0x00026438
	private void OnLoginFinished(BootController.ConnectionWithGameServiceState nextState)
	{
		this.currentState = nextState;
		this.menuSceneLoading = SceneManager.LoadSceneAsync(SceneController.SCENE_MENU_NAME, LoadSceneMode.Additive);
	}

	// Token: 0x0400003B RID: 59
	private BootController.ConnectionWithGameServiceState currentState;

	// Token: 0x0400003C RID: 60
	[SerializeField]
	private SplashscreenController splashscreenController;

	// Token: 0x0400003D RID: 61
	private AsyncOperation menuSceneLoading;

	// Token: 0x02000014 RID: 20
	public enum ConnectionWithGameServiceState
	{
		// Token: 0x0400003F RID: 63
		NotConnected,
		// Token: 0x04000040 RID: 64
		Connecting,
		// Token: 0x04000041 RID: 65
		ConnectFailed,
		// Token: 0x04000042 RID: 66
		ConnectSuccessful
	}
}

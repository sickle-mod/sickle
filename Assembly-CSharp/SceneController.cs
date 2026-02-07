using System;
using Common.GameSaves;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200014F RID: 335
public class SceneController : MonoBehaviour
{
	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0002EB2B File Offset: 0x0002CD2B
	public static SceneController Instance
	{
		get
		{
			return SceneController.instance;
		}
	}

	// Token: 0x1400003B RID: 59
	// (add) Token: 0x060009D9 RID: 2521 RVA: 0x0007C69C File Offset: 0x0007A89C
	// (remove) Token: 0x060009DA RID: 2522 RVA: 0x0007C6D0 File Offset: 0x0007A8D0
	public static event SceneController.GameSceneLoaded OnGameSceneLoaded;

	// Token: 0x060009DB RID: 2523 RVA: 0x0002EB32 File Offset: 0x0002CD32
	private void Awake()
	{
		if (SceneController.instance == null)
		{
			SceneController.instance = this;
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x0002EB5E File Offset: 0x0002CD5E
	public void EnableCanvas(bool active)
	{
		this.loadingScreenMainHierarchy.SetActive(active);
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x0007C704 File Offset: 0x0007A904
	public void LoadScene(string sceneName)
	{
		this.loadingScreenMainHierarchy.SetActive(true);
		this.previousScene = SceneManager.GetActiveScene().name;
		PlayerPrefs.SetString(SceneController.PREFS_LAST_SCENE_NAME, this.previousScene);
		this.sceneToLoad = sceneName;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		this.sceneLoadingOperation = SceneManager.LoadSceneAsync("loading");
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x0007C77C File Offset: 0x0007A97C
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name != this.sceneToLoad)
		{
			this.OnTemporarySceneLoaded();
		}
		else if (scene.name == this.sceneToLoad)
		{
			this.OnTargetSceneLoaded();
		}
		if (SceneController.OnGameSceneLoaded != null)
		{
			SceneController.OnGameSceneLoaded(scene.name);
		}
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x0007C7D8 File Offset: 0x0007A9D8
	private void OnTemporarySceneLoaded()
	{
		this.sceneLoadingOperation = SceneManager.LoadSceneAsync(this.sceneToLoad);
		this.loadingScreenMainHierarchy.SetActive(true);
		Scene sceneByName = SceneManager.GetSceneByName(this.sceneToLoad);
		if (sceneByName.name.Contains("main"))
		{
			this.ReloadMainSceneForLoad();
			if (PlatformManager.IsStandalone)
			{
				this.loadingScreenPresenter.SetLoadingAsyncOperation(this.sceneLoadingOperation);
				return;
			}
			this.loadingScreenPresenterMobile.SetLoadingAsyncOperation(this.sceneLoadingOperation);
			return;
		}
		else
		{
			if (sceneByName.name.Contains("menu") || sceneByName.name.Contains("showroom"))
			{
				this.exitingScreen.GetComponent<ExitScreenPresenter>().Show(this.previousScene.Contains(SceneController.SCENE_MAIN_NAME));
				if (PlatformManager.IsStandalone)
				{
					this.loadingScreenPresenter.gameObject.SetActive(false);
				}
				else
				{
					this.loadingScreenPresenterMobile.gameObject.SetActive(false);
				}
				this.loadingScreenLobby.gameObject.SetActive(false);
				return;
			}
			if (sceneByName.name.Contains("lobby"))
			{
				if (SceneManager.GetSceneByName(this.sceneToLoad).name.Contains("main"))
				{
					if (PlatformManager.IsStandalone)
					{
						this.loadingScreenPresenter.SetLoadingAsyncOperation(this.sceneLoadingOperation);
						return;
					}
					this.loadingScreenPresenterMobile.SetLoadingAsyncOperation(this.sceneLoadingOperation);
					return;
				}
				else
				{
					this.loadingScreenLobby.ShowCallApiWindow();
				}
			}
			return;
		}
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x0002EB6C File Offset: 0x0002CD6C
	public void ShowAuthenticatingWindow()
	{
		this.loadingScreenLobby.ShowCallApiWindow();
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x0007C940 File Offset: 0x0007AB40
	private void OnTargetSceneLoaded()
	{
		Scene sceneByName = SceneManager.GetSceneByName(this.sceneToLoad);
		if (sceneByName.name.Contains("menu") || sceneByName.name.Contains("showroom"))
		{
			this.exitingScreen.GetComponent<ExitScreenPresenter>().Hide();
			this.loadingScreenMainHierarchy.SetActive(false);
		}
		else if (sceneByName.name.Contains("lobby"))
		{
			this.loadingScreenLobby.HideConnectionWindow();
			this.loadingScreenMainHierarchy.SetActive(false);
		}
		else if (sceneByName.name.Contains("main"))
		{
			if (PlatformManager.IsStandalone)
			{
				GameController.Instance.GetComponent<KeyboardShortcuts>().startButton = this.loadingScreenPresenter.StartButton;
				GameController.Instance.GetComponent<KeyboardShortcuts>().keyInfo[71] = this.loadingScreenPresenter.StartButton.transform.GetChild(2).gameObject;
				GameController.Instance.cameraControler.tooltipBlockers[0] = this.loadingScreenPresenter.Background.gameObject;
			}
			else
			{
				GameController.Instance.cameraControler.tooltipBlockers[0] = this.loadingScreenPresenterMobile.Background.gameObject;
			}
		}
		this.sceneToLoad = string.Empty;
		this.sceneLoadingOperation = null;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x0002EB79 File Offset: 0x0002CD79
	public string GetSceneToLoad()
	{
		return this.sceneToLoad;
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x0002EB81 File Offset: 0x0002CD81
	private void ReloadMainSceneForLoad()
	{
		if (!string.IsNullOrEmpty(SceneController.loadGameFile))
		{
			this.LoadGame(SceneController.loadGameFile);
			SceneController.AfterLoadingTheGame();
		}
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x0007CA9C File Offset: 0x0007AC9C
	public void LoadGame(string file)
	{
		AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.load);
		Game game = GameController.Game;
		game.CreateNewGameManager();
		GameController.gameFromSave = true;
		game.GameManager.IsMultiplayer = false;
		game.GameManager.missionId = -1;
		GameSavesManager.LoadGame(file);
		game.GameManager.IsHotSeat = true;
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x0002EB9F File Offset: 0x0002CD9F
	public static void AfterLoadingTheGame()
	{
		SceneController.loadGameFile = string.Empty;
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x0002EBAB File Offset: 0x0002CDAB
	public static void SetLoadGameFile(string gameFile)
	{
		SceneController.loadGameFile = gameFile;
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x0002EBB3 File Offset: 0x0002CDB3
	public void DisableLoadingScreenCanvas()
	{
		this.loadingScreenMainHierarchy.SetActive(false);
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x0002EBC1 File Offset: 0x0002CDC1
	public void DisableLoadingScreenPresenter()
	{
		this.loadingScreenPresenter.gameObject.SetActive(false);
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x0002EBD4 File Offset: 0x0002CDD4
	public ConnectionPanel GetConnectionPanel()
	{
		return this.loadingScreenLobby;
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x0002EBDC File Offset: 0x0002CDDC
	public LoadingScreenPresenterMobile GetLoadingScreenPresenterMobile()
	{
		return this.loadingScreenPresenterMobile;
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x0002EBE4 File Offset: 0x0002CDE4
	public LoadingScreenPresenter GetLoadingScreenPresenterPC()
	{
		return this.loadingScreenPresenter;
	}

	// Token: 0x040008D8 RID: 2264
	public static string SCENE_MAIN_NAME = (PlatformManager.IsStandalone ? "main" : "main_mobile");

	// Token: 0x040008D9 RID: 2265
	public static string SCENE_MENU_NAME = (PlatformManager.IsStandalone ? "menu" : "menu_mobile");

	// Token: 0x040008DA RID: 2266
	public static string SCENE_LOBBY_NAME = (PlatformManager.IsStandalone ? "lobby_new" : "lobby_mobile");

	// Token: 0x040008DB RID: 2267
	public static string SCENE_SHOWROOM_NAME = (PlatformManager.IsStandalone ? "showroom" : "showroom_mobile");

	// Token: 0x040008DC RID: 2268
	public const string SCENE_BOOT_NAME = "boot";

	// Token: 0x040008DD RID: 2269
	public const string SCENE_LOADING_NAME = "loading";

	// Token: 0x040008DE RID: 2270
	public const string SCENE_ANDROID_LOADER = "android_loader";

	// Token: 0x040008DF RID: 2271
	public static string PREFS_LAST_SCENE_NAME = "LastScene";

	// Token: 0x040008E0 RID: 2272
	private static SceneController instance = null;

	// Token: 0x040008E2 RID: 2274
	[SerializeField]
	private GameObject loadingScreenMainHierarchy;

	// Token: 0x040008E3 RID: 2275
	[SerializeField]
	private LoadingScreenPresenter loadingScreenPresenter;

	// Token: 0x040008E4 RID: 2276
	[SerializeField]
	private LoadingScreenPresenterMobile loadingScreenPresenterMobile;

	// Token: 0x040008E5 RID: 2277
	[SerializeField]
	private ConnectionPanel loadingScreenLobby;

	// Token: 0x040008E6 RID: 2278
	[SerializeField]
	private GameObject exitingScreen;

	// Token: 0x040008E7 RID: 2279
	private string previousScene = string.Empty;

	// Token: 0x040008E8 RID: 2280
	private string sceneToLoad = string.Empty;

	// Token: 0x040008E9 RID: 2281
	private AsyncOperation sceneLoadingOperation;

	// Token: 0x040008EA RID: 2282
	private static string loadGameFile = string.Empty;

	// Token: 0x02000150 RID: 336
	// (Invoke) Token: 0x060009EF RID: 2543
	public delegate void GameSceneLoaded(string SceneName);
}

using System;
using Scythe.UI;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000136 RID: 310
public class LoadLobby : GenericSingletonClass<LoadLobby>
{
	// Token: 0x0600094A RID: 2378 RVA: 0x0002E5DD File Offset: 0x0002C7DD
	private void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			this.RedirectToLobby();
		}
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0002E5F1 File Offset: 0x0002C7F1
	private void OnEnable()
	{
		SceneManager.sceneLoaded += this.OnLevelFinishedLoading;
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x0002E604 File Offset: 0x0002C804
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.OnLevelFinishedLoading;
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0007B278 File Offset: 0x00079478
	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if (SceneManager.GetActiveScene().name == SceneController.SCENE_MENU_NAME && this.loadedFromNotification)
		{
			this.LoadLobbyScene();
		}
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0002E617 File Offset: 0x0002C817
	private void LoadLobbyScene()
	{
		this.loadedFromNotification = false;
		SingletonMono<MainMenu>.Instance.MultiplayerMenu();
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0002E62A File Offset: 0x0002C82A
	private void LoadMenuScene()
	{
		this.loadedFromNotification = true;
		SceneController.Instance.LoadScene(SceneController.SCENE_MENU_NAME);
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0007B2AC File Offset: 0x000794AC
	private void RedirectToLobby()
	{
		if (SceneManager.GetActiveScene().name != SceneController.SCENE_MENU_NAME)
		{
			this.LoadMenuScene();
			return;
		}
		SingletonMono<MainMenu>.Instance.MultiplayerMenu();
	}

	// Token: 0x04000881 RID: 2177
	[SerializeField]
	private bool loadedFromNotification;
}

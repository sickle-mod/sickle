using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200010C RID: 268
public class MenuLobbyMusic : MonoBehaviour
{
	// Token: 0x060008A9 RID: 2217 RVA: 0x00079DD4 File Offset: 0x00077FD4
	private void Awake()
	{
		if (MenuLobbyMusic.instance == null)
		{
			MenuLobbyMusic.instance = base.gameObject;
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			return;
		}
		if (MenuLobbyMusic.instance != null && MenuLobbyMusic.instance != base.gameObject)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0002DD48 File Offset: 0x0002BF48
	private void OnDestroy()
	{
		if (MenuLobbyMusic.instance == base.gameObject)
		{
			MenuLobbyMusic.instance = null;
		}
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x00079E40 File Offset: 0x00078040
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (SceneManager.GetActiveScene().name.Contains("main"))
		{
			base.GetComponent<AudioSource>().Stop();
			return;
		}
		if (!base.GetComponent<AudioSource>().isPlaying)
		{
			base.GetComponent<AudioSource>().Play();
		}
	}

	// Token: 0x0400076F RID: 1903
	private static GameObject instance;
}

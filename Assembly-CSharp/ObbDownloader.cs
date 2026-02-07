using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

// Token: 0x02000140 RID: 320
public class ObbDownloader : MonoBehaviour
{
	// Token: 0x06000975 RID: 2421 RVA: 0x0002E779 File Offset: 0x0002C979
	private void Start()
	{
		if (GooglePlayObbDownloadManager.IsDownloaderAvailable())
		{
			this.m_obbDownloader = GooglePlayObbDownloadManager.GetGooglePlayObbDownloader();
			this.m_obbDownloader.PublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAiwfW+TayuTFZLi1ZKbm6Lbu4eluwxvzBQHJvDfhxuNAuXdIZ0A5CZIIb3Zq1LgfA6FzB5YnSl2MFp6fKwGS5FnmBC6qttvbtQ1FeoVoJWT94fXstPyPUqlGgx3wId5koP+5WzcBKHT51ZnQBkjj2gV/b35Qrc6N9kwWd7xzj7QeWCLHEXWGuuu6BW3OMOVtw/mYo6Z3/ZzaGqStUVfrpLKBXzWOfM0VcUNpBR8YaH7dBGYqBXJz0SsiCzoD8QxrkvNYl2VNQ3MW1ko7U2Zfq8ehLFPUVqdt7CDASksUAfBKxFfMbEVQYr2RWqcg6rdIjoA7eMd3X1M0Pqkd6JW+MKwIDAQAB";
			this.DoTheJob();
			return;
		}
		base.StartCoroutine(this.LoadBootScene());
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x0002E7B1 File Offset: 0x0002C9B1
	private void DoTheJob()
	{
		if (this.NeedsToDownloadObb())
		{
			this.ShowDownloadObbPopup();
			return;
		}
		base.StartCoroutine(this.LoadBootScene());
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0002E7CF File Offset: 0x0002C9CF
	private bool NeedsToDownloadObb()
	{
		if (this.m_obbDownloader.GetExpansionFilePath() == null)
		{
			throw new NotSupportedException("External storage is not available");
		}
		bool mainOBBPath = this.m_obbDownloader.GetMainOBBPath() != null;
		this.m_obbDownloader.GetPatchOBBPath();
		return !mainOBBPath;
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x0002E805 File Offset: 0x0002CA05
	private void ShowDownloadObbPopup()
	{
		this.downloadObbPopup.SetActive(true);
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x0002E813 File Offset: 0x0002CA13
	public void DownloadObbButton_OnClick()
	{
		this.downloadObbPopup.SetActive(false);
		this.m_obbDownloader.FetchOBB();
		base.StartCoroutine(this.WaitForObbFile());
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x0002E839 File Offset: 0x0002CA39
	private IEnumerator WaitForObbFile()
	{
		string mainPath;
		do
		{
			yield return new WaitForSeconds(0.5f);
			mainPath = this.m_obbDownloader.GetMainOBBPath();
		}
		while (mainPath == null);
		string text = "file://" + mainPath;
		UnityWebRequest www = UnityWebRequest.Get(text);
		www.downloadHandler = new DownloadHandlerBuffer();
		www.SendWebRequest();
		while (!www.isDone)
		{
			yield return null;
		}
		if (www.error != null)
		{
			throw new FileLoadException("Loading obb file exception: " + www.error + ". mainPath: " + mainPath);
		}
		base.StartCoroutine(this.LoadBootScene());
		yield break;
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x0002E848 File Offset: 0x0002CA48
	private IEnumerator LoadBootScene()
	{
		AsyncOperation loadScene = SceneManager.LoadSceneAsync("boot", LoadSceneMode.Additive);
		while (!loadScene.isDone)
		{
			yield return null;
		}
		SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
		yield break;
	}

	// Token: 0x0400088E RID: 2190
	private IGooglePlayObbDownloader m_obbDownloader;

	// Token: 0x0400088F RID: 2191
	[SerializeField]
	private GameObject downloadObbPopup;
}

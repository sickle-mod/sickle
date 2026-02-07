using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class GooglePlayObbDownloadManager
{
	// Token: 0x06000964 RID: 2404 RVA: 0x0002E6BF File Offset: 0x0002C8BF
	public static IGooglePlayObbDownloader GetGooglePlayObbDownloader()
	{
		if (GooglePlayObbDownloadManager.m_Instance != null)
		{
			return GooglePlayObbDownloadManager.m_Instance;
		}
		if (!GooglePlayObbDownloadManager.IsDownloaderAvailable())
		{
			return null;
		}
		GooglePlayObbDownloadManager.m_Instance = new GooglePlayObbDownloader();
		return GooglePlayObbDownloadManager.m_Instance;
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x0002E6E6 File Offset: 0x0002C8E6
	public static bool IsDownloaderAvailable()
	{
		return GooglePlayObbDownloadManager.m_AndroidOSBuildClass.GetRawClass() != IntPtr.Zero;
	}

	// Token: 0x04000886 RID: 2182
	private static AndroidJavaClass m_AndroidOSBuildClass = new AndroidJavaClass("android.os.Build");

	// Token: 0x04000887 RID: 2183
	private static IGooglePlayObbDownloader m_Instance;
}

using System;
using System.IO;
using UnityEngine;

// Token: 0x0200013F RID: 319
internal class GooglePlayObbDownloader : IGooglePlayObbDownloader
{
	// Token: 0x1700006E RID: 110
	// (get) Token: 0x06000968 RID: 2408 RVA: 0x0002E70D File Offset: 0x0002C90D
	// (set) Token: 0x06000969 RID: 2409 RVA: 0x0002E715 File Offset: 0x0002C915
	public string PublicKey { get; set; }

	// Token: 0x0600096A RID: 2410 RVA: 0x0007B3DC File Offset: 0x000795DC
	private void ApplyPublicKey()
	{
		if (string.IsNullOrEmpty(this.PublicKey))
		{
			Debug.LogError("GooglePlayObbDownloader: The public key is not set - did you forget to set it in the script?\n");
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.plugin.downloader.UnityDownloaderService"))
		{
			androidJavaClass.SetStatic<string>("BASE64_PUBLIC_KEY", this.PublicKey);
			androidJavaClass.SetStatic<byte[]>("SALT", new byte[]
			{
				1, 43, 244, byte.MaxValue, 54, 98, 156, 244, 43, 2,
				248, 252, 9, 5, 150, 148, 223, 45, byte.MaxValue, 84
			});
		}
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x0007B458 File Offset: 0x00079658
	public void FetchOBB()
	{
		this.ApplyPublicKey();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent", new object[]
			{
				@static,
				new AndroidJavaClass("com.unity3d.plugin.downloader.UnityDownloaderActivity")
			});
			androidJavaObject.Call<AndroidJavaObject>("addFlags", new object[] { 65536 });
			androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[]
			{
				"unityplayer.Activity",
				@static.Call<AndroidJavaObject>("getClass", Array.Empty<object>()).Call<string>("getName", Array.Empty<object>())
			});
			try
			{
				@static.Call("startActivity", new object[] { androidJavaObject });
			}
			catch (Exception ex)
			{
				Debug.LogError("GooglePlayObbDownloader: Exception occurred while attempting to start DownloaderActivity - is the AndroidManifest.xml incorrect?\n" + ex.Message);
			}
		}
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0007B554 File Offset: 0x00079754
	public string GetExpansionFilePath()
	{
		if (GooglePlayObbDownloader.EnvironmentClass.CallStatic<string>("getExternalStorageState", Array.Empty<object>()) != "mounted")
		{
			this.m_ExpansionFilePath = null;
			return this.m_ExpansionFilePath;
		}
		if (string.IsNullOrEmpty(this.m_ExpansionFilePath))
		{
			using (AndroidJavaObject androidJavaObject = GooglePlayObbDownloader.EnvironmentClass.CallStatic<AndroidJavaObject>("getExternalStorageDirectory", Array.Empty<object>()))
			{
				string text = androidJavaObject.Call<string>("getPath", Array.Empty<object>());
				this.m_ExpansionFilePath = string.Format("{0}/{1}/{2}", text, "Android/obb", GooglePlayObbDownloader.ObbPackage);
			}
		}
		return this.m_ExpansionFilePath;
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0002E71E File Offset: 0x0002C91E
	public string GetMainOBBPath()
	{
		return GooglePlayObbDownloader.GetOBBPackagePath(this.GetExpansionFilePath(), "main");
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x0002E730 File Offset: 0x0002C930
	public string GetPatchOBBPath()
	{
		return GooglePlayObbDownloader.GetOBBPackagePath(this.GetExpansionFilePath(), "patch");
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x0007B600 File Offset: 0x00079800
	private static string GetOBBPackagePath(string expansionFilePath, string prefix)
	{
		if (string.IsNullOrEmpty(expansionFilePath))
		{
			return null;
		}
		string text = string.Format("{0}/{1}.{2}.{3}.obb", new object[]
		{
			expansionFilePath,
			prefix,
			GooglePlayObbDownloader.ObbVersion,
			GooglePlayObbDownloader.ObbPackage
		});
		if (!File.Exists(text))
		{
			return null;
		}
		return text;
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000970 RID: 2416 RVA: 0x0002E742 File Offset: 0x0002C942
	private static string ObbPackage
	{
		get
		{
			if (GooglePlayObbDownloader.m_ObbPackage == null)
			{
				GooglePlayObbDownloader.PopulateOBBProperties();
			}
			return GooglePlayObbDownloader.m_ObbPackage;
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000971 RID: 2417 RVA: 0x0002E755 File Offset: 0x0002C955
	private static int ObbVersion
	{
		get
		{
			if (GooglePlayObbDownloader.m_ObbVersion == 0)
			{
				GooglePlayObbDownloader.PopulateOBBProperties();
			}
			return GooglePlayObbDownloader.m_ObbVersion;
		}
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0007B650 File Offset: 0x00079850
	private static void PopulateOBBProperties()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			GooglePlayObbDownloader.m_ObbPackage = @static.Call<string>("getPackageName", Array.Empty<object>());
			GooglePlayObbDownloader.m_ObbVersion = @static.Call<AndroidJavaObject>("getPackageManager", Array.Empty<object>()).Call<AndroidJavaObject>("getPackageInfo", new object[]
			{
				GooglePlayObbDownloader.m_ObbPackage,
				0
			}).Get<int>("versionCode");
		}
	}

	// Token: 0x04000888 RID: 2184
	private static AndroidJavaClass EnvironmentClass = new AndroidJavaClass("android.os.Environment");

	// Token: 0x04000889 RID: 2185
	private const string Environment_MediaMounted = "mounted";

	// Token: 0x0400088B RID: 2187
	private string m_ExpansionFilePath;

	// Token: 0x0400088C RID: 2188
	private static string m_ObbPackage;

	// Token: 0x0400088D RID: 2189
	private static int m_ObbVersion;
}

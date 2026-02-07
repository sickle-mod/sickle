using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200011B RID: 283
public static class AssetBundleManager
{
	// Token: 0x060008F8 RID: 2296 RVA: 0x0007A8B8 File Offset: 0x00078AB8
	public static AssetBundle LoadAssetBundle(string assetBundleName)
	{
		foreach (AssetBundle assetBundle in AssetBundle.GetAllLoadedAssetBundles())
		{
			if (assetBundle.name == assetBundleName)
			{
				return assetBundle;
			}
		}
		return AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles", assetBundleName));
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0002E1E2 File Offset: 0x0002C3E2
	public static IEnumerator LoadAssetBundleAsync(string assetBundleName)
	{
		bool flag = false;
		using (IEnumerator<AssetBundle> enumerator = AssetBundle.GetAllLoadedAssetBundles().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.name == assetBundleName)
				{
					flag = true;
				}
			}
		}
		if (!flag)
		{
			yield return AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, "AssetBundles", assetBundleName));
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0007A928 File Offset: 0x00078B28
	public static bool UnloadAssetBundle(string assetBundleName, bool unloadAllLoadedObjects = true)
	{
		AssetBundle assetBundle = null;
		foreach (AssetBundle assetBundle2 in AssetBundle.GetAllLoadedAssetBundles())
		{
			if (assetBundle2.name == assetBundleName)
			{
				assetBundle = assetBundle2;
			}
		}
		if (assetBundle == null)
		{
			return false;
		}
		assetBundle.Unload(unloadAllLoadedObjects);
		return true;
	}

	// Token: 0x0400082E RID: 2094
	public const string BUNDLE_GRAPHIC_ENCOUNTERS = "graphic_encounters";

	// Token: 0x0400082F RID: 2095
	public const string BUNDLE_GRAPHIC_ENCOUNTERS_MOBILE = "graphic_encounters_mobile";

	// Token: 0x04000830 RID: 2096
	public const string BUNDLE_GRAPHIC_OBJECTIVES = "graphic_objectives";

	// Token: 0x04000831 RID: 2097
	public const string BUNDLE_GRAPHIC_OBJECTIVES_INGAME = "graphic_objectivesmobileingame";

	// Token: 0x04000832 RID: 2098
	public const string BUNDLE_GRAPHIC_FACTORY = "graphic_factory";

	// Token: 0x04000833 RID: 2099
	public const string BUNDLE_GRAPHIC_BACKGROUNDS = "graphic_backgrounds";

	// Token: 0x04000834 RID: 2100
	public const string BUNDLE_GRAPHIC_BACKGROUNDS_MENU = "graphic_backgrounds_menu";

	// Token: 0x04000835 RID: 2101
	public const string BUNDLE_MUSIC_MANAGER = "music_manager";
}

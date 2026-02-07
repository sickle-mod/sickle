using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020004CE RID: 1230
	public class AlbumBundlesLoader
	{
		// Token: 0x06002737 RID: 10039 RVA: 0x0004111F File Offset: 0x0003F31F
		public IEnumerator LoadBundle(TabType tabType)
		{
			if (this.loadedBundles.ContainsKey(tabType))
			{
				yield break;
			}
			foreach (AssetBundle assetBundle in AssetBundle.GetAllLoadedAssetBundles())
			{
				if (assetBundle.name == this.GetBundleName(tabType))
				{
					this.loadedBundles.Add(tabType, assetBundle);
					yield break;
				}
			}
			AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, "AssetBundles", this.GetBundleName(tabType)));
			yield return bundleLoadRequest;
			AssetBundle assetBundle2 = bundleLoadRequest.assetBundle;
			if (assetBundle2 == null)
			{
				Debug.LogError("Failed to load AssetBundle " + this.GetBundleName(tabType));
				yield break;
			}
			this.loadedBundles.Add(tabType, assetBundle2);
			yield return this.loadedBundles[tabType];
			yield break;
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x00041135 File Offset: 0x0003F335
		public IEnumerator LoadSprites(TabType tabType)
		{
			if (this.loadedSprites.ContainsKey(tabType))
			{
				yield break;
			}
			if (!this.loadedBundles.ContainsKey(tabType))
			{
				yield return this.LoadBundle(tabType);
			}
			AssetBundleRequest assetLoadRequest = this.loadedBundles[tabType].LoadAllAssetsAsync<Sprite>();
			yield return assetLoadRequest;
			List<Sprite> list = new List<Sprite>(assetLoadRequest.allAssets.Length);
			foreach (global::UnityEngine.Object @object in assetLoadRequest.allAssets)
			{
				list.Add(@object as Sprite);
			}
			if (!this.loadedSprites.ContainsKey(tabType))
			{
				this.loadedSprites.Add(tabType, list);
			}
			yield return this.loadedSprites[tabType];
			yield break;
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x0004114B File Offset: 0x0003F34B
		public IEnumerator LoadSprites(TabType tabType, List<string> spriteNames)
		{
			if (this.loadedSprites.ContainsKey(tabType))
			{
				yield break;
			}
			if (!this.loadedBundles.ContainsKey(tabType))
			{
				yield return this.LoadBundle(tabType);
			}
			List<Sprite> sprites = new List<Sprite>();
			foreach (string text in spriteNames)
			{
				AssetBundleRequest assetLoadRequest = this.loadedBundles[tabType].LoadAssetAsync<Sprite>(text);
				yield return assetLoadRequest;
				sprites.Add(assetLoadRequest.asset as Sprite);
				assetLoadRequest = null;
			}
			List<string>.Enumerator enumerator = default(List<string>.Enumerator);
			if (!this.loadedSprites.ContainsKey(tabType))
			{
				this.loadedSprites.Add(tabType, sprites);
			}
			yield return this.loadedSprites[tabType];
			yield break;
			yield break;
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x00041168 File Offset: 0x0003F368
		public List<Sprite> GetSprites(TabType tabType)
		{
			if (!this.loadedSprites.ContainsKey(tabType))
			{
				return new List<Sprite>();
			}
			return this.loadedSprites[tabType];
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x0004118A File Offset: 0x0003F38A
		public bool SpritesLoaded(TabType tabType)
		{
			return this.loadedSprites.ContainsKey(tabType);
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000E77FC File Offset: 0x000E59FC
		public void Clear()
		{
			foreach (AssetBundle assetBundle in this.loadedBundles.Values)
			{
				if (assetBundle != null)
				{
					assetBundle.Unload(true);
				}
			}
			this.loadedBundles.Clear();
			this.loadedSprites.Clear();
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x000E7874 File Offset: 0x000E5A74
		private string GetBundleName(TabType tabType)
		{
			switch (tabType)
			{
			case TabType.EncounterCards:
				return "graphic_encounters";
			case TabType.FactoryCards:
				return "graphic_factory";
			case TabType.ObjectiveCards:
				return "graphic_objectives";
			case TabType.Mats:
				return "graphic_backgrounds";
			case TabType.None:
				Debug.LogError("Invalid tab type: " + tabType.ToString() + " . Returning encounters");
				return "graphic_encounters";
			default:
				throw new ArgumentOutOfRangeException("tabType", tabType, null);
			}
		}

		// Token: 0x04001C10 RID: 7184
		private Dictionary<TabType, AssetBundle> loadedBundles = new Dictionary<TabType, AssetBundle>();

		// Token: 0x04001C11 RID: 7185
		private Dictionary<TabType, List<Sprite>> loadedSprites = new Dictionary<TabType, List<Sprite>>();
	}
}

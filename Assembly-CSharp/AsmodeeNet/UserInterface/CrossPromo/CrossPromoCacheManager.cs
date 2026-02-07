using System;
using System.Collections;
using System.IO;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000802 RID: 2050
	public static class CrossPromoCacheManager
	{
		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06003A7C RID: 14972 RVA: 0x0004DC7F File Offset: 0x0004BE7F
		private static string _LanguageCode
		{
			get
			{
				if (CoreApplication.Instance != null)
				{
					return CoreApplication.Instance.LocalizationManager.CurrentLanguageCode;
				}
				return null;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06003A7D RID: 14973 RVA: 0x0004DC9F File Offset: 0x0004BE9F
		private static Channel _Channel
		{
			get
			{
				return CoreApplication.Instance.Channel;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06003A7E RID: 14974 RVA: 0x0004DCAB File Offset: 0x0004BEAB
		private static string _BasePath
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "AsmodeeDigital/CrossPromo/Cache");
			}
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x00150C08 File Offset: 0x0014EE08
		public static void SaveGroupProductInCache(ShowcaseProduct[] groupProduct, GameProductTag? filter = null)
		{
			string text = string.Empty;
			if (filter == null)
			{
				text = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("interstitial.json", Array.Empty<object>()));
			}
			else
			{
				text = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("GroupProducts/{0}.json", filter.ToString()));
			}
			Directory.CreateDirectory(Path.GetDirectoryName(text));
			foreach (ShowcaseProduct showcaseProduct in groupProduct)
			{
				string text2 = Path.Combine(Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/", showcaseProduct.Id)), string.Format("product_{0}.json", showcaseProduct.Id));
				CrossPromoCacheManager.SaveProductInCache(showcaseProduct, text2);
			}
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x00150CC8 File Offset: 0x0014EEC8
		public static void SaveBannerInCache(ShowcaseProduct product)
		{
			string text = Path.Combine(CrossPromoCacheManager._BasePath, "banner.json");
			CrossPromoCacheManager.SaveProductInCache(product, text);
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x00150CEC File Offset: 0x0014EEEC
		private static void SaveProductInCache(ShowcaseProduct product, string pathProductJson)
		{
			string text = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/", product.Id));
			if (!CrossPromoCacheManager.FreshResourceInCache(pathProductJson))
			{
				Directory.CreateDirectory(text);
				Directory.CreateDirectory(Path.GetDirectoryName(pathProductJson));
				string text2 = product.ToJson();
				File.WriteAllText(pathProductJson, text2);
			}
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x00150D44 File Offset: 0x0014EF44
		public static void LoadMoreGame(GameProductTag? filter, Action<ShowcaseProduct[]> onSucceed, Action onCannotLoadMoreGames)
		{
			if (CrossPromoCacheManager._requestGamesEndpoint != null)
			{
				return;
			}
			try
			{
				CrossPromoCacheManager._requestGamesEndpoint = ((filter == null) ? new RequestGamesEndpoint(CrossPromoCacheManager._Channel, CrossPromoCacheManager._LanguageCode, null) : new RequestGamesEndpoint(CrossPromoCacheManager._Channel, CrossPromoCacheManager._LanguageCode, filter.Value, null));
			}
			catch
			{
				if (onCannotLoadMoreGames != null)
				{
					onCannotLoadMoreGames();
				}
				CrossPromoCacheManager._requestGamesEndpoint = null;
				return;
			}
			CrossPromoCacheManager._requestGamesEndpoint.Execute(delegate(ShowcaseProduct[] result, WebError webError)
			{
				if (webError == null)
				{
					CrossPromoCacheManager.SaveGroupProductInCache(result, filter);
					if (onSucceed != null)
					{
						onSucceed(result);
					}
				}
				else if (onCannotLoadMoreGames != null)
				{
					onCannotLoadMoreGames();
				}
				CrossPromoCacheManager._requestGamesEndpoint = null;
			});
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x0004DCBC File Offset: 0x0004BEBC
		public static void CancelLoadMoreGame()
		{
			if (CrossPromoCacheManager._requestGamesEndpoint != null)
			{
				CrossPromoCacheManager._requestGamesEndpoint = null;
				CrossPromoCacheManager._requestGamesEndpoint.Abort();
			}
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x00150DF8 File Offset: 0x0014EFF8
		public static void LoadInterstitial(Action<ShowcaseProduct[]> onSucceed, Action onCannotLoadInterstitial)
		{
			if (CrossPromoCacheManager._requestInterstitialEndpoint != null)
			{
				return;
			}
			try
			{
				CrossPromoCacheManager._requestInterstitialEndpoint = new RequestInterstitialEndpoint(CrossPromoCacheManager._Channel, CrossPromoCacheManager._LanguageCode, null);
			}
			catch
			{
				if (onCannotLoadInterstitial != null)
				{
					onCannotLoadInterstitial();
				}
				CrossPromoCacheManager._requestInterstitialEndpoint = null;
				return;
			}
			CrossPromoCacheManager._requestInterstitialEndpoint.Execute(delegate(ShowcaseProduct[] result, WebError webError)
			{
				if (webError == null)
				{
					CrossPromoCacheManager.SaveGroupProductInCache(result, null);
					if (onSucceed != null)
					{
						onSucceed(result);
					}
				}
				else if (onCannotLoadInterstitial != null)
				{
					onCannotLoadInterstitial();
				}
				CrossPromoCacheManager._requestInterstitialEndpoint = null;
			});
		}

		// Token: 0x06003A85 RID: 14981 RVA: 0x0004DCD5 File Offset: 0x0004BED5
		public static void CancelLoadInterstitial()
		{
			if (CrossPromoCacheManager._requestInterstitialEndpoint != null)
			{
				CrossPromoCacheManager._requestInterstitialEndpoint = null;
				CrossPromoCacheManager._requestInterstitialEndpoint.Abort();
			}
		}

		// Token: 0x06003A86 RID: 14982 RVA: 0x00150E7C File Offset: 0x0014F07C
		public static void LoadBanner(Action<ShowcaseProduct> onSucceed, Action onCannotLoadBanner)
		{
			if (CrossPromoCacheManager.requestBannerEndpoint != null)
			{
				return;
			}
			try
			{
				CrossPromoCacheManager.requestBannerEndpoint = new RequestBannerEndpoint(CrossPromoCacheManager._Channel, CrossPromoCacheManager._LanguageCode, null);
			}
			catch
			{
				if (onCannotLoadBanner != null)
				{
					onCannotLoadBanner();
				}
				CrossPromoCacheManager.requestBannerEndpoint = null;
				return;
			}
			CrossPromoCacheManager.requestBannerEndpoint.Execute(delegate(ShowcaseProduct result, WebError webError)
			{
				if (webError == null)
				{
					CrossPromoCacheManager.SaveBannerInCache(result);
					if (onSucceed != null)
					{
						onSucceed(result);
					}
				}
				else if (onCannotLoadBanner != null)
				{
					onCannotLoadBanner();
				}
				CrossPromoCacheManager.requestBannerEndpoint = null;
			});
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x0004DCEE File Offset: 0x0004BEEE
		public static void CancelLoadBanner()
		{
			if (CrossPromoCacheManager.requestBannerEndpoint != null)
			{
				CrossPromoCacheManager.requestBannerEndpoint.Abort();
				CrossPromoCacheManager.requestBannerEndpoint = null;
			}
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x0004DD07 File Offset: 0x0004BF07
		public static IEnumerator LoadProductTileImage(ShowcaseProduct product, RawImage image, Action<bool> afterLoading)
		{
			string text = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/tile_url_{1}x{2}{3}", new object[]
			{
				product.Id,
				product.Tile.Width,
				product.Tile.Height,
				Path.GetExtension(product.Tile.ImageUrl)
			}));
			yield return CrossPromoCacheManager.LoadTexture(product.Tile.ImageUrl, text, image, afterLoading);
			yield break;
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x0004DD24 File Offset: 0x0004BF24
		public static IEnumerator LoadProductIcon(ShowcaseProduct product, Image image, Action<bool> afterLoading)
		{
			string text = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/icon_url{1}", product.Id, Path.GetExtension(product.IconUrl)));
			yield return CrossPromoCacheManager.LoadTexture(product.IconUrl, text, image, afterLoading);
			yield break;
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x0004DD41 File Offset: 0x0004BF41
		public static IEnumerator LoadProductImage(ShowcaseProduct product, string pathImage, Image image, Action<bool> afterLoading)
		{
			string text = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/Images/{1}", product.Id, Path.GetFileName(pathImage)));
			yield return CrossPromoCacheManager.LoadTexture(pathImage, text, image, afterLoading);
			yield break;
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x0004DD65 File Offset: 0x0004BF65
		public static IEnumerator LoadProductAward(ShowcaseProduct product, string pathImage, Image image, Action<bool> afterLoading)
		{
			string text = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/Awards/{1}", product.Id, Path.GetFileName(pathImage)));
			yield return CrossPromoCacheManager.LoadTexture(pathImage, text, image, afterLoading);
			yield break;
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x0004DD89 File Offset: 0x0004BF89
		public static IEnumerator LoadBannerImage(ShowcaseProduct product, RawImage image, Action<bool> afterLoading)
		{
			string text = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/banner_url{1}", product.Id, Path.GetExtension(product.BannerUrl)));
			yield return CrossPromoCacheManager.LoadTexture(product.BannerUrl, text, image, afterLoading);
			yield break;
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x00150F00 File Offset: 0x0014F100
		private static bool FreshResourceInCache(string pathCache)
		{
			if (!string.IsNullOrEmpty(pathCache) && File.Exists(pathCache))
			{
				DateTime lastWriteTime = File.GetLastWriteTime(pathCache);
				return DateTime.Now.Subtract(lastWriteTime).TotalDays <= 1.0;
			}
			return false;
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x0004DDA6 File Offset: 0x0004BFA6
		private static IEnumerator LoadTexture(string path, string pathLocalCache, MaskableGraphic image, Action<bool> onComplete)
		{
			if (CrossPromoCacheManager.FreshResourceInCache(pathLocalCache))
			{
				yield return CrossPromoCacheManager.LoadAndWriteTexture(pathLocalCache, pathLocalCache, image, onComplete);
			}
			else
			{
				AsmoLogger.Warning("CrossPromoCacheManager", "Asset needs to be refreshed", new Hashtable { { "path", path } });
				yield return CrossPromoCacheManager.LoadAndWriteTexture(path, pathLocalCache, image, onComplete);
			}
			yield break;
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x0004DDCA File Offset: 0x0004BFCA
		private static IEnumerator LoadAndWriteTexture(string path, string pathLocalCache, MaskableGraphic image, Action<bool> OnComplete)
		{
			yield return TextureLoader.LoadTexture(path, image, delegate(bool success, byte[] bytes)
			{
				if (!success)
				{
					if (OnComplete != null)
					{
						OnComplete(false);
					}
					return;
				}
				if (OnComplete != null)
				{
					OnComplete(true);
				}
				if (path.StartsWith("http"))
				{
					if (!Directory.Exists(Path.GetDirectoryName(pathLocalCache)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(pathLocalCache));
					}
					File.WriteAllBytes(pathLocalCache, bytes);
				}
			});
			yield break;
		}

		// Token: 0x04002C18 RID: 11288
		private static RequestGamesEndpoint _requestGamesEndpoint;

		// Token: 0x04002C19 RID: 11289
		private static RequestInterstitialEndpoint _requestInterstitialEndpoint;

		// Token: 0x04002C1A RID: 11290
		private static RequestBannerEndpoint requestBannerEndpoint;
	}
}

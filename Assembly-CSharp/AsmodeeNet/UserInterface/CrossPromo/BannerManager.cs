using System;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x020007FE RID: 2046
	public class BannerManager : MonoBehaviour
	{
		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06003A60 RID: 14944 RVA: 0x0004DA24 File Offset: 0x0004BC24
		// (set) Token: 0x06003A61 RID: 14945 RVA: 0x0004DA31 File Offset: 0x0004BC31
		public BannerImageFitter.BannerPosition Position
		{
			get
			{
				return this._bannerImageFitter.Position;
			}
			set
			{
				this._bannerImageFitter.Position = value;
			}
		}

		// Token: 0x06003A62 RID: 14946 RVA: 0x0004DA3F File Offset: 0x0004BC3F
		private void Awake()
		{
			this._bannerImage = base.GetComponentInChildren<RawImage>(true);
		}

		// Token: 0x06003A63 RID: 14947 RVA: 0x0004DA4E File Offset: 0x0004BC4E
		private void Start()
		{
			CrossPromoCacheManager.LoadBanner(delegate(ShowcaseProduct product)
			{
				this._product = product;
				base.StartCoroutine(CrossPromoCacheManager.LoadBannerImage(product, this._bannerImage, delegate(bool success)
				{
					if (success)
					{
						AnalyticsEvents.LogCrossPromoDisplayedEvent(new ShowcaseProduct[] { this._product }, CROSSPROMO_DISPLAYED.crosspromo_type.banner, null);
						this._bannerImage.gameObject.SetActive(true);
						return;
					}
					this._CleanBanner();
				}));
			}, delegate
			{
				this._CleanBanner();
			});
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x00150910 File Offset: 0x0014EB10
		public static BannerManager InstantiateBanner()
		{
			BannerManager bannerManager = (BannerManager)global::UnityEngine.Object.FindObjectOfType(typeof(BannerManager));
			if (bannerManager == null)
			{
				bannerManager = global::UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.BannerPrefab).GetComponent<BannerManager>();
			}
			else
			{
				AsmoLogger.Error("BannerManager", "Try to InstantiateBanner twice", null);
			}
			return bannerManager;
		}

		// Token: 0x06003A65 RID: 14949 RVA: 0x0004DA6D File Offset: 0x0004BC6D
		public void Dismiss()
		{
			CoreApplication.Instance.AnalyticsManager.RemoveContext(typeof(CrossPromoAnalyticsContext));
			this._CleanBanner();
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x0004DA8E File Offset: 0x0004BC8E
		private void _CleanBanner()
		{
			CrossPromoCacheManager.CancelLoadBanner();
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x00150968 File Offset: 0x0014EB68
		public void OnBannerClicked()
		{
			AnalyticsEvents.LogCrossPromoOpenedEvent(null, null);
			AnalyticsEvents.LogCrossPromoScreenDisplayEvent(CROSSPROMO_SCREEN_DISPLAY.screen_current.game_detail, CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_banner, this._product, null, null);
			GameDetailsPopup.InstantiateGameDetails(this._product, null, null);
		}

		// Token: 0x04002C08 RID: 11272
		private RawImage _bannerImage;

		// Token: 0x04002C09 RID: 11273
		[SerializeField]
		private BannerImageFitter _bannerImageFitter;

		// Token: 0x04002C0A RID: 11274
		private ShowcaseProduct _product;

		// Token: 0x04002C0B RID: 11275
		private const string _kModuleName = "BannerManager";
	}
}

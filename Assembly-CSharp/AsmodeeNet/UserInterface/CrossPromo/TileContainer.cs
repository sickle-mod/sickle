using System;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Foundation.Localization;
using AsmodeeNet.Network.RestApi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000820 RID: 2080
	public class TileContainer : MonoBehaviour
	{
		// Token: 0x06003B0E RID: 15118 RVA: 0x001523D0 File Offset: 0x001505D0
		public Rect Init(ShowcaseProduct product, int xCellPositionInGrid, int yCellPositionInGrid, int cellSizeInPixels, int xSpacing, int ySpacing, Action closeDetailsAction)
		{
			this._product = product;
			this._cellPositionInGrid = new Vector2((float)xCellPositionInGrid, (float)yCellPositionInGrid);
			this._closeDetailsAction = closeDetailsAction;
			base.gameObject.SetActive(true);
			this.ui.Loading.gameObject.SetActive(true);
			this.ui.Image.gameObject.SetActive(false);
			this.ui.ImageButton.color = this.PublishedColor;
			LocalizationManager localizationManager = CoreApplication.Instance.LocalizationManager;
			if (product.Status == ProductStatus.soon)
			{
				this.ui.TextButton.text = localizationManager.GetLocalizedText("CrossPromo.button.comingsoon");
				this.ui.ImageButton.color = this.ComingSoonColor;
			}
			else
			{
				string text = (string.IsNullOrEmpty(this._product.ShopDigitalUrl) ? "CrossPromo.button.learnmore" : "CrossPromo.button.playnow");
				this.ui.TextButton.text = localizationManager.GetLocalizedText(text);
			}
			base.StartCoroutine(CrossPromoCacheManager.LoadProductTileImage(product, this.ui.Image, delegate(bool success)
			{
				this.ui.Loading.gameObject.SetActive(false);
				if (!success)
				{
					this.ui.Image.gameObject.SetActive(true);
					this.ui.Image.texture = Resources.Load(string.Format("DefaultTextures/DefaultTile{0}x{1}", product.Tile.Width, product.Tile.Height)) as Texture2D;
				}
			}));
			this.ui.RectTransform.localScale = Vector3.one;
			int num = (int)((float)cellSizeInPixels * 0.2f);
			int num2 = (cellSizeInPixels + xSpacing) * xCellPositionInGrid;
			int num3 = (cellSizeInPixels + num + ySpacing) * yCellPositionInGrid;
			int num4 = product.Tile.Width * cellSizeInPixels + (product.Tile.Width - 1) * xSpacing;
			int num5 = product.Tile.Height * (cellSizeInPixels + num) + (product.Tile.Height - 1) * ySpacing;
			this._position = new Vector3((float)num2, (float)(-(float)num3), 0f);
			this.ui.RectTransform.sizeDelta = new Vector2((float)num4, (float)num5);
			return new Rect((float)num2, (float)num3, (float)num4, (float)num5);
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x0004E35F File Offset: 0x0004C55F
		private void Update()
		{
			if (!this._needsUpdate)
			{
				return;
			}
			this._needsUpdate = false;
			base.transform.localPosition = this._position;
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x001525DC File Offset: 0x001507DC
		private void _ShowDetails(CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action action)
		{
			base.transform.root.gameObject.SetActive(false);
			CrossPromoAnalyticsContext crossPromoAnalyticsContext = CoreApplication.Instance.AnalyticsManager.GetContext(typeof(CrossPromoAnalyticsContext)) as CrossPromoAnalyticsContext;
			CROSSPROMO_SCREEN_DISPLAY.screen_current screen = (CROSSPROMO_SCREEN_DISPLAY.screen_current)Enum.Parse(typeof(CROSSPROMO_SCREEN_DISPLAY.screen_current), crossPromoAnalyticsContext.ScreenCurrent);
			AnalyticsEvents.LogCrossPromoScreenDisplayEvent(CROSSPROMO_SCREEN_DISPLAY.screen_current.game_detail, action, this._product, new Vector2?(this._cellPositionInGrid), null);
			GameDetailsPopup.InstantiateGameDetails(this._product, delegate(GameDetailsPopup p)
			{
				p.Dismiss();
				if (this._closeDetailsAction != null)
				{
					this._closeDetailsAction();
				}
			}, delegate(GameDetailsPopup p)
			{
				AnalyticsEvents.LogCrossPromoScreenDisplayEvent(screen, CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_back, this._product, new Vector2?(this._cellPositionInGrid), null);
				this.transform.root.gameObject.SetActive(true);
				p.Dismiss();
			});
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x0004E382 File Offset: 0x0004C582
		public void TileImage_Clicked()
		{
			this._ShowDetails(CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_tile);
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x0004E38B File Offset: 0x0004C58B
		public void TileButton_Clicked()
		{
			if (string.IsNullOrEmpty(this._product.ShopDigitalUrl))
			{
				this._ShowDetails(CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_learn_more);
				return;
			}
			AnalyticsEvents.LogCrossPromoRedirectedEvent(this._product, null);
			Application.OpenURL(this._product.ShopDigitalUrl);
		}

		// Token: 0x04002CC0 RID: 11456
		public TileContainer.UI ui;

		// Token: 0x04002CC1 RID: 11457
		public Color ComingSoonColor;

		// Token: 0x04002CC2 RID: 11458
		public Color PublishedColor;

		// Token: 0x04002CC3 RID: 11459
		public GameObject gameDetailsPopupPrefab;

		// Token: 0x04002CC4 RID: 11460
		private ShowcaseProduct _product;

		// Token: 0x04002CC5 RID: 11461
		private Vector2 _cellPositionInGrid;

		// Token: 0x04002CC6 RID: 11462
		private Action _closeDetailsAction;

		// Token: 0x04002CC7 RID: 11463
		private Vector3 _position;

		// Token: 0x04002CC8 RID: 11464
		private bool _needsUpdate = true;

		// Token: 0x02000821 RID: 2081
		[Serializable]
		public class UI
		{
			// Token: 0x04002CC9 RID: 11465
			public RectTransform RectTransform;

			// Token: 0x04002CCA RID: 11466
			public RawImage Image;

			// Token: 0x04002CCB RID: 11467
			public TextMeshProUGUI TextButton;

			// Token: 0x04002CCC RID: 11468
			public Transform Loading;

			// Token: 0x04002CCD RID: 11469
			public Image ImageButton;
		}
	}
}

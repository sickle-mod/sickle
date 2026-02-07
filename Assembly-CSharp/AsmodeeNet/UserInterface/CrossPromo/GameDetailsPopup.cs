using System;
using System.Collections.Generic;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using Scythe.Analytics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000814 RID: 2068
	public class GameDetailsPopup : CrossPromoBasePopup
	{
		// Token: 0x06003ACF RID: 15055 RVA: 0x0004DF9F File Offset: 0x0004C19F
		public void OnBackClicked()
		{
			if (this.ContainerZoomed != null)
			{
				this.Zoom(this.ContainerZoomed);
				return;
			}
			this.Close(this._onClickPreviousButton != null);
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x001515E8 File Offset: 0x0014F7E8
		public static GameDetailsPopup InstantiateGameDetails(ShowcaseProduct product, Action<GameDetailsPopup> onClickCloseButton = null, Action<GameDetailsPopup> onClickPreviousButton = null)
		{
			GameDetailsPopup gameDetailsPopup = (GameDetailsPopup)global::UnityEngine.Object.FindObjectOfType(typeof(GameDetailsPopup));
			if (gameDetailsPopup == null)
			{
				gameDetailsPopup = global::UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.GameDetailsPopupPrefab).GetComponent<GameDetailsPopup>();
				gameDetailsPopup._Init(product, onClickCloseButton, onClickPreviousButton);
			}
			else
			{
				AsmoLogger.Error("GameDetailsPopup", "Try to InstantiateMoreGames twice", null);
			}
			return gameDetailsPopup;
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x0004DFCB File Offset: 0x0004C1CB
		private void _Init(ShowcaseProduct product, Action<GameDetailsPopup> onClickCloseButton, Action<GameDetailsPopup> onClickPreviousButton)
		{
			this._onClickCloseButton = onClickCloseButton;
			this._onClickPreviousButton = onClickPreviousButton;
			this._product = product;
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x0015164C File Offset: 0x0014F84C
		private void Start()
		{
			if (this._product == null)
			{
				AsmoLogger.Error("", "Cannot show a GameDetailsPopup which has not been initialized. Consider calling Init() before", null);
				return;
			}
			this._CleanPopup();
			this.uiGameDetailsPopup.BackButton.gameObject.SetActive(this._onClickPreviousButton != null);
			this.uiGameDetailsPopup.Name.text = this._product.Name;
			this.uiGameDetailsPopup.Description.text = this._product.Description;
			this.uiGameDetailsPopup.DescriptionIcon.Init(this, -1);
			base.StartCoroutine(CrossPromoCacheManager.LoadProductIcon(this._product, this.uiGameDetailsPopup.DescriptionIcon.uiContainer.Image, delegate(bool success)
			{
				this.uiGameDetailsPopup.DescriptionIcon.HideLoading();
				if (!success)
				{
					this.uiGameDetailsPopup.DescriptionIcon.uiContainer.Image.gameObject.SetActive(true);
					Texture2D texture2D = Resources.Load("DefaultTextures/DefaultTile1x1") as Texture2D;
					this.uiGameDetailsPopup.DescriptionIcon.uiContainer.Image.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
				}
			}));
			this.uiGameDetailsPopup.BuyDigitalButton.gameObject.SetActive(!string.IsNullOrEmpty(this._product.ShopDigitalUrl));
			this.uiGameDetailsPopup.BuyPhysicalButton.gameObject.SetActive(!string.IsNullOrEmpty(this._product.ShopPhysicalUrl));
			int num = 0;
			foreach (ShowcaseImage showcaseImage in this._product.Images)
			{
				CrossPromoContainer crossPromoContainer = global::UnityEngine.Object.Instantiate<CrossPromoContainer>(this.ImageContainerPrefab, this.uiGameDetailsPopup.Content);
				crossPromoContainer.transform.localScale = Vector3.one;
				crossPromoContainer.GetComponent<LayoutElement>().preferredHeight = this.uiGameDetailsPopup.Content.rect.height;
				crossPromoContainer.GetComponent<LayoutElement>().preferredWidth = this.uiGameDetailsPopup.Content.rect.height;
				crossPromoContainer.Init(this, num++);
				crossPromoContainer.LoadThumbnail(this._product);
			}
			foreach (ShowcaseAward showcaseAward in this._product.Awards)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.AwardContainerPrefab, this.uiGameDetailsPopup.DescriptionLeftColumn);
				gameObject.transform.localScale = Vector3.one;
				this._awardContainers.Add(gameObject);
				base.StartCoroutine(CrossPromoCacheManager.LoadProductAward(this._product, showcaseAward.ImageUrl, gameObject.GetComponentInChildren<Image>(), null));
			}
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x00151884 File Offset: 0x0014FA84
		private void _CleanPopup()
		{
			base.StopAllCoroutines();
			if (this.ContainerZoomed != null)
			{
				global::UnityEngine.Object.Destroy(this.uiGameDetailsPopup.ZoomedImageContainer.GetChild(0).gameObject);
				this.uiGameDetailsPopup.BackgroundZoomContainer.color = new Color(0f, 0f, 0f, 0f);
				this.uiGameDetailsPopup.ImagesHorizontalLayoutGroup.enabled = true;
				this._zoomState = GameDetailsPopup.ZoomState.NoZoom;
				this.ContainerZoomed = null;
			}
			this.uiGameDetailsPopup.Content.anchoredPosition = Vector2.zero;
			this.uiGameDetailsPopup.DescriptionIcon.uiContainer.Image.sprite = null;
			for (int i = 0; i < this.uiGameDetailsPopup.Content.childCount; i++)
			{
				global::UnityEngine.Object.Destroy(this.uiGameDetailsPopup.Content.GetChild(i).gameObject);
			}
			foreach (GameObject gameObject in this._awardContainers)
			{
				global::UnityEngine.Object.Destroy(gameObject.gameObject);
			}
			this._awardContainers.Clear();
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x001519C4 File Offset: 0x0014FBC4
		public void Zoom(CrossPromoContainer container)
		{
			float num = 0.25f;
			Easer cubeInOut = Ease.CubeInOut;
			if (this._zoomState == GameDetailsPopup.ZoomState.Zoomed)
			{
				AnalyticsEvents.LogCrossPromoScreenDisplayEvent(CROSSPROMO_SCREEN_DISPLAY.screen_current.game_detail, CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_image, this._product, null, null);
				container.LoadThumbnail(this._product);
				this._zoomState = GameDetailsPopup.ZoomState.ZoomAnimation;
				int compteur4 = 4;
				Action endAnim = delegate
				{
					int compteur2 = compteur4;
					compteur4 = compteur2 - 1;
					if (compteur4 == 0)
					{
						this.ContainerZoomed.transform.SetParent(this.uiGameDetailsPopup.Content);
						this.ContainerZoomed.uiContainer.RectTransform.SetSiblingIndex(this._indexSibling);
						this.uiGameDetailsPopup.ImagesHorizontalLayoutGroup.enabled = true;
						this.ContainerZoomed = null;
						this._zoomState = GameDetailsPopup.ZoomState.NoZoom;
					}
				};
				base.StartCoroutine(Easing.EaseFromTo(0.75f, 0f, num, cubeInOut, delegate(float value)
				{
					this.uiGameDetailsPopup.BackgroundZoomContainer.color = new Color(0f, 0f, 0f, value);
				}, endAnim));
				base.StartCoroutine(Easing.EaseFromTo(0f, 1f, num + 0.15f, cubeInOut, delegate(float value)
				{
				}, delegate
				{
					this.uiGameDetailsPopup.ImagesScrollView.horizontal = true;
					endAnim();
				}));
				base.StartCoroutine(Easing.EaseFromTo(this.ContainerZoomed.uiContainer.RectTransform.offsetMin, this._prevOffsetMin, num, cubeInOut, delegate(Vector2 value)
				{
					this.ContainerZoomed.uiContainer.RectTransform.offsetMin = value;
				}, endAnim));
				base.StartCoroutine(Easing.EaseFromTo(this.ContainerZoomed.uiContainer.RectTransform.offsetMax, this._prevOffsetMax, num, cubeInOut, delegate(Vector2 value)
				{
					this.ContainerZoomed.uiContainer.RectTransform.offsetMax = value;
				}, endAnim));
				return;
			}
			if (this._zoomState == GameDetailsPopup.ZoomState.NoZoom)
			{
				AnalyticsEvents.LogCrossPromoScreenDisplayEvent(CROSSPROMO_SCREEN_DISPLAY.screen_current.zoom_image, CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_image, null, null, null);
				container.LoadImage(this._product);
				this._zoomState = GameDetailsPopup.ZoomState.ZoomAnimation;
				this.uiGameDetailsPopup.ImagesScrollView.horizontal = false;
				this.uiGameDetailsPopup.ImagesHorizontalLayoutGroup.enabled = false;
				this.ContainerZoomed = container;
				this._indexSibling = container.uiContainer.RectTransform.GetSiblingIndex();
				container.transform.SetParent(this.uiGameDetailsPopup.ZoomedImageContainer, true);
				Vector3 position = container.transform.position;
				Vector2 vector = new Vector2(0.5f, 0.5f);
				container.uiContainer.RectTransform.pivot = vector;
				container.uiContainer.RectTransform.anchorMin = new Vector2(0f, 0f);
				container.uiContainer.RectTransform.anchorMax = new Vector2(1f, 1f);
				container.uiContainer.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, container.uiContainer.Layout.preferredWidth);
				container.uiContainer.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, container.uiContainer.Layout.preferredHeight);
				container.transform.position = position;
				int compteur = 4;
				Action action = delegate
				{
					int compteur3 = compteur;
					compteur = compteur3 - 1;
					if (compteur == 0)
					{
						this._zoomState = GameDetailsPopup.ZoomState.Zoomed;
					}
				};
				this._prevOffsetMin = container.uiContainer.RectTransform.offsetMin;
				this._prevOffsetMax = container.uiContainer.RectTransform.offsetMax;
				base.StartCoroutine(Easing.EaseFromTo(0f, 0.75f, num, cubeInOut, delegate(float value)
				{
					this.uiGameDetailsPopup.BackgroundZoomContainer.color = new Color(0f, 0f, 0f, value);
				}, action));
				base.StartCoroutine(Easing.EaseFromTo(container.uiContainer.RectTransform.offsetMin, Vector2.zero, num, cubeInOut, delegate(Vector2 value)
				{
					container.uiContainer.RectTransform.offsetMin = value;
				}, action));
				base.StartCoroutine(Easing.EaseFromTo(container.uiContainer.RectTransform.offsetMax, Vector2.zero, num, cubeInOut, delegate(Vector2 value)
				{
					container.uiContainer.RectTransform.offsetMax = value;
				}, action));
				base.StartCoroutine(container.transform.MoveTo(Vector3.zero, num, cubeInOut, action));
			}
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x0004DFE2 File Offset: 0x0004C1E2
		public void DigitalShop()
		{
			AnalyticsEvents.LogCrossPromoRedirectedEvent(this._product, null);
			Application.OpenURL(this._product.ShopDigitalUrl);
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x0004E000 File Offset: 0x0004C200
		public void PhysicalShop()
		{
			AnalyticsEvents.LogCrossPromoRedirectedEvent(this._product, null);
			Application.OpenURL(this._product.ShopPhysicalUrl);
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x00151E64 File Offset: 0x00150064
		public void Close(bool goBackToPreviousPopup)
		{
			if (goBackToPreviousPopup)
			{
				this._onClickPreviousButton(this);
				return;
			}
			if (this._onClickCloseButton != null)
			{
				this._onClickCloseButton(this);
				return;
			}
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
			AnalyticsEvents.LogCrossPromoClosedEvent(null);
			this.Dismiss();
		}

		// Token: 0x04002C85 RID: 11397
		public GameDetailsPopup.UIGameDetailsPopup uiGameDetailsPopup;

		// Token: 0x04002C86 RID: 11398
		private Action<GameDetailsPopup> _onClickCloseButton;

		// Token: 0x04002C87 RID: 11399
		private Action<GameDetailsPopup> _onClickPreviousButton;

		// Token: 0x04002C88 RID: 11400
		private ShowcaseProduct _product;

		// Token: 0x04002C89 RID: 11401
		[Header("Prefabs")]
		public CrossPromoContainer ImageContainerPrefab;

		// Token: 0x04002C8A RID: 11402
		public GameObject AwardContainerPrefab;

		// Token: 0x04002C8B RID: 11403
		private List<GameObject> _awardContainers = new List<GameObject>();

		// Token: 0x04002C8C RID: 11404
		private CrossPromoContainer ContainerZoomed;

		// Token: 0x04002C8D RID: 11405
		private GameDetailsPopup.ZoomState _zoomState;

		// Token: 0x04002C8E RID: 11406
		private int _indexSibling;

		// Token: 0x04002C8F RID: 11407
		private Vector2 _prevOffsetMin;

		// Token: 0x04002C90 RID: 11408
		private Vector2 _prevOffsetMax;

		// Token: 0x04002C91 RID: 11409
		private const string _consoleModuleName = "GameDetailsPopup";

		// Token: 0x02000815 RID: 2069
		[Serializable]
		public class UIGameDetailsPopup
		{
			// Token: 0x04002C92 RID: 11410
			[Header("Header")]
			public TextMeshProUGUI Name;

			// Token: 0x04002C93 RID: 11411
			public Button BackButton;

			// Token: 0x04002C94 RID: 11412
			[Header("Main")]
			public RectTransform MainContent;

			// Token: 0x04002C95 RID: 11413
			[Header("Images and Videos")]
			public RectTransform Content;

			// Token: 0x04002C96 RID: 11414
			public ScrollRect ImagesScrollView;

			// Token: 0x04002C97 RID: 11415
			public HorizontalLayoutGroup ImagesHorizontalLayoutGroup;

			// Token: 0x04002C98 RID: 11416
			[Header("Description - Left")]
			public Transform DescriptionLeftColumn;

			// Token: 0x04002C99 RID: 11417
			public CrossPromoContainer DescriptionIcon;

			// Token: 0x04002C9A RID: 11418
			public Button BuyDigitalButton;

			// Token: 0x04002C9B RID: 11419
			public Button BuyPhysicalButton;

			// Token: 0x04002C9C RID: 11420
			[Header("Description - Right")]
			public TextMeshProUGUI Description;

			// Token: 0x04002C9D RID: 11421
			[Header("Zoomed Image")]
			public RectTransform ZoomedImageContainer;

			// Token: 0x04002C9E RID: 11422
			public Image BackgroundZoomContainer;
		}

		// Token: 0x02000816 RID: 2070
		public enum ZoomState
		{
			// Token: 0x04002CA0 RID: 11424
			NoZoom,
			// Token: 0x04002CA1 RID: 11425
			ZoomAnimation,
			// Token: 0x04002CA2 RID: 11426
			Zoomed
		}
	}
}

using System;
using AsmodeeNet.Network.RestApi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x0200080E RID: 2062
	public class CrossPromoContainer : UIBehaviour
	{
		// Token: 0x06003AC2 RID: 15042 RVA: 0x00151468 File Offset: 0x0014F668
		public virtual void Init(GameDetailsPopup popup, int imageIndex = -1)
		{
			this._popup = popup;
			this.uiContainer.Image.sprite = null;
			this.uiContainer.Image.gameObject.SetActive(false);
			this._imagesIndex = imageIndex;
			base.gameObject.SetActive(true);
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x0004DF09 File Offset: 0x0004C109
		public void LoadImage(ShowcaseProduct product)
		{
			this._DisplayImage(product, CrossPromoContainer.Imagetype.Screenshoot);
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x0004DF13 File Offset: 0x0004C113
		public void LoadThumbnail(ShowcaseProduct product)
		{
			this._DisplayImage(product, CrossPromoContainer.Imagetype.Thumbnail);
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x001514B8 File Offset: 0x0014F6B8
		private void _DisplayImage(ShowcaseProduct product, CrossPromoContainer.Imagetype type)
		{
			if (this._imagesIndex == -1)
			{
				return;
			}
			if (this._image[(int)type] == null || this._imageLoadFailed[(int)type])
			{
				this.uiContainer.Loading.gameObject.SetActive(true);
				base.StartCoroutine(CrossPromoCacheManager.LoadProductImage(product, (type == CrossPromoContainer.Imagetype.Thumbnail) ? product.Images[this._imagesIndex].ThumbUrl : product.Images[this._imagesIndex].ImageUrl, this.uiContainer.Image, delegate(bool success)
				{
					this._imageLoadFailed[(int)type] = !success;
					this._image[(int)type] = this.uiContainer.Image;
					this.uiContainer.Loading.gameObject.SetActive(false);
				}));
				return;
			}
			this.uiContainer.Image = this._image[(int)type];
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x0004DF1D File Offset: 0x0004C11D
		public void HideLoading()
		{
			this.uiContainer.Loading.gameObject.SetActive(false);
		}

		// Token: 0x06003AC7 RID: 15047 RVA: 0x0004DF35 File Offset: 0x0004C135
		public void ImageContainer_Clicked()
		{
			this._popup.Zoom(this);
		}

		// Token: 0x06003AC8 RID: 15048 RVA: 0x0004DF43 File Offset: 0x0004C143
		protected override void OnDisable()
		{
			base.OnDestroy();
			if (this.destroyOnDisable)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x04002C4C RID: 11340
		public bool destroyOnDisable;

		// Token: 0x04002C4D RID: 11341
		public CrossPromoContainer.UIContainer uiContainer;

		// Token: 0x04002C4E RID: 11342
		protected GameDetailsPopup _popup;

		// Token: 0x04002C4F RID: 11343
		protected ShowcaseProduct _showcaseProduct;

		// Token: 0x04002C50 RID: 11344
		protected int _imagesIndex = -1;

		// Token: 0x04002C51 RID: 11345
		private Image[] _image = new Image[2];

		// Token: 0x04002C52 RID: 11346
		private bool[] _imageLoadFailed = new bool[2];

		// Token: 0x04002C53 RID: 11347
		private CrossPromoContainer.Imagetype _imagetype;

		// Token: 0x0200080F RID: 2063
		[Serializable]
		public class UIContainer
		{
			// Token: 0x04002C54 RID: 11348
			public RectTransform RectTransform;

			// Token: 0x04002C55 RID: 11349
			public Transform Loading;

			// Token: 0x04002C56 RID: 11350
			public LayoutElement Layout;

			// Token: 0x04002C57 RID: 11351
			public Image Image;
		}

		// Token: 0x02000810 RID: 2064
		private enum Imagetype
		{
			// Token: 0x04002C59 RID: 11353
			Thumbnail,
			// Token: 0x04002C5A RID: 11354
			Screenshoot
		}
	}
}

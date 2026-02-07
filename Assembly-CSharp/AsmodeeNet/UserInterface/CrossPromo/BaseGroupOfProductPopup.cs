using System;
using System.Collections.Generic;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network.RestApi;
using Scythe.Analytics;
using UnityEngine;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x020007FF RID: 2047
	public abstract class BaseGroupOfProductPopup : CrossPromoBasePopup
	{
		// Token: 0x06003A6C RID: 14956 RVA: 0x0004DB03 File Offset: 0x0004BD03
		protected virtual void OnEnable()
		{
			CoreApplication.Instance.Preferences.AspectDidChange += this.SetNeedsUpdate;
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange += this.SetNeedsUpdate;
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x0004DB3B File Offset: 0x0004BD3B
		protected virtual void OnDisable()
		{
			if (!CoreApplication.IsQuitting)
			{
				CoreApplication.Instance.Preferences.AspectDidChange -= this.SetNeedsUpdate;
				CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange -= this.SetNeedsUpdate;
			}
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x0004DB7A File Offset: 0x0004BD7A
		private void SetNeedsUpdate()
		{
			this._needsUpdate = true;
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x0004DB83 File Offset: 0x0004BD83
		private void LateUpdate()
		{
			if (!this._needsUpdate)
			{
				return;
			}
			this.ReloadProducts(null);
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x0004DB95 File Offset: 0x0004BD95
		public void ReloadProducts(ShowcaseProduct[] products = null)
		{
			if (products != null)
			{
				this._products = products;
			}
			if (this._products == null)
			{
				return;
			}
			this._ShowGroupOfProducts(this._products);
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x001509AC File Offset: 0x0014EBAC
		protected virtual void _ShowGroupOfProducts(ShowcaseProduct[] products)
		{
			this._needsUpdate = false;
			this.content.anchoredPosition = Vector2.zero;
			this.content.sizeDelta = Vector2.zero;
			this._tilesUI.ForEach(delegate(TileContainer t)
			{
				global::UnityEngine.Object.Destroy(t.gameObject);
			});
			this._tilesUI.Clear();
			int num = (int)((this.content.rect.width - (float)((this.nbColumns - 1) * this.spacingX)) / (float)this.nbColumns);
			int num2 = this.nbColumns;
			int num3 = 100;
			bool[,] array = new bool[num2, num3];
			foreach (ShowcaseProduct showcaseProduct in products)
			{
				bool flag = false;
				int num4 = 0;
				int num5 = 0;
				while (num5 < num3 && !flag)
				{
					num4 = 0;
					while (num4 < num2 && !flag)
					{
						bool flag2 = true;
						int num6 = 0;
						while (num6 < showcaseProduct.Tile.Width && flag2)
						{
							int num7 = 0;
							while (num7 < showcaseProduct.Tile.Height && flag2)
							{
								if (num4 + num6 > num2 - 1 || num5 + num7 > num3 - 1 || array[num4 + num6, num5 + num7])
								{
									flag2 = false;
								}
								num7++;
							}
							num6++;
						}
						if (flag2)
						{
							flag = true;
						}
						num4++;
					}
					num5++;
				}
				if (flag)
				{
					num4--;
					num5--;
					for (int j = 0; j < showcaseProduct.Tile.Width; j++)
					{
						for (int k = 0; k < showcaseProduct.Tile.Height; k++)
						{
							array[num4 + j, num5 + k] = true;
						}
					}
					TileContainer tileContainer = global::UnityEngine.Object.Instantiate<TileContainer>(this.TilePrefab, this.content, false);
					this._tilesUI.Add(tileContainer);
					Rect rect = tileContainer.Init(showcaseProduct, num4, num5, num, this.spacingX, this.spacingY, delegate
					{
						this.Dismiss();
					});
					Vector2 vector = new Vector2(0f, Math.Max(rect.y + rect.height, this.content.sizeDelta.y));
					this.content.sizeDelta = vector;
				}
			}
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x0004DBB6 File Offset: 0x0004BDB6
		public override void Dismiss()
		{
			base.Dismiss();
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
			AnalyticsEvents.LogCrossPromoClosedEvent(null);
		}

		// Token: 0x04002C0C RID: 11276
		public RectTransform content;

		// Token: 0x04002C0D RID: 11277
		public int nbColumns = 3;

		// Token: 0x04002C0E RID: 11278
		public int spacingX = 10;

		// Token: 0x04002C0F RID: 11279
		public int spacingY = 10;

		// Token: 0x04002C10 RID: 11280
		public TileContainer TilePrefab;

		// Token: 0x04002C11 RID: 11281
		protected ShowcaseProduct[] _products;

		// Token: 0x04002C12 RID: 11282
		private List<TileContainer> _tilesUI = new List<TileContainer>();

		// Token: 0x04002C13 RID: 11283
		private bool _needsUpdate;
	}
}

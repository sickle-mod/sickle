using System;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x020007FC RID: 2044
	public class BannerImageFitter : MonoBehaviour
	{
		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06003A5A RID: 14938 RVA: 0x0004D9BB File Offset: 0x0004BBBB
		// (set) Token: 0x06003A5B RID: 14939 RVA: 0x0004D9C3 File Offset: 0x0004BBC3
		public BannerImageFitter.BannerPosition Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position == value)
				{
					return;
				}
				this._position = value;
				this._RefreshUI();
			}
		}

		// Token: 0x06003A5C RID: 14940 RVA: 0x00150804 File Offset: 0x0014EA04
		private void _RefreshUI()
		{
			RectTransform rectTransform = base.transform.parent as RectTransform;
			float num = Mathf.Max(rectTransform.rect.width, rectTransform.rect.height) / 8f;
			RectTransform rectTransform2 = base.transform as RectTransform;
			rectTransform2.sizeDelta = new Vector2(num * 5f, num);
			if (this.Position == BannerImageFitter.BannerPosition.Top)
			{
				rectTransform2.anchorMax = new Vector2(0.5f, 1f);
				rectTransform2.anchorMin = new Vector2(0.5f, 1f);
				rectTransform2.anchoredPosition = new Vector2(0f, -rectTransform2.rect.height / 2f);
				return;
			}
			rectTransform2.anchorMax = new Vector2(0.5f, 0f);
			rectTransform2.anchorMin = new Vector2(0.5f, 0f);
			rectTransform2.anchoredPosition = new Vector2(0f, rectTransform2.rect.height / 2f);
		}

		// Token: 0x06003A5D RID: 14941 RVA: 0x0004D9DC File Offset: 0x0004BBDC
		private void OnEnable()
		{
			CoreApplication.Instance.Preferences.AspectDidChange += this._RefreshUI;
			this._RefreshUI();
		}

		// Token: 0x06003A5E RID: 14942 RVA: 0x0004D9FF File Offset: 0x0004BBFF
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.Preferences.AspectDidChange -= this._RefreshUI;
		}

		// Token: 0x04002C04 RID: 11268
		private BannerImageFitter.BannerPosition _position;

		// Token: 0x020007FD RID: 2045
		public enum BannerPosition
		{
			// Token: 0x04002C06 RID: 11270
			Top,
			// Token: 0x04002C07 RID: 11271
			Bottom
		}
	}
}

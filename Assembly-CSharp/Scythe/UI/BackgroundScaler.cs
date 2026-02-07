using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004F8 RID: 1272
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Image))]
	public class BackgroundScaler : MonoBehaviour
	{
		// Token: 0x060028D0 RID: 10448 RVA: 0x00042765 File Offset: 0x00040965
		private void Start()
		{
			this.Update();
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x0004276D File Offset: 0x0004096D
		private void Update()
		{
			this.tracker.Clear();
			this.tracker.Add(this, this.RectTransform, DrivenTransformProperties.All);
			this.Scale();
		}

		// Token: 0x060028D2 RID: 10450 RVA: 0x000EB758 File Offset: 0x000E9958
		private void Scale()
		{
			RectTransform parent = this.GetParent();
			if (parent != null)
			{
				this.RectTransform.anchorMin = BackgroundScaler.anchor;
				this.RectTransform.anchorMax = BackgroundScaler.anchor;
				this.RectTransform.anchoredPosition = Vector2.zero;
				this.RectTransform.pivot = BackgroundScaler.pivot;
				this.RectTransform.localScale = Vector3.one;
				float num = ((this.Image.sprite != null) ? (this.Image.sprite.rect.width / this.Image.sprite.rect.height) : 1f);
				Vector2 size = parent.rect.size;
				if (size.y * num < size.x)
				{
					this.RectTransform.sizeDelta = new Vector2(size.x, size.x / num);
				}
				else
				{
					this.RectTransform.sizeDelta = new Vector2(size.y * num, size.y);
				}
				this.RectTransform.position = parent.position;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x060028D3 RID: 10451 RVA: 0x00042793 File Offset: 0x00040993
		private RectTransform RectTransform
		{
			get
			{
				if (this.rectTransform == null)
				{
					this.rectTransform = base.GetComponent<RectTransform>();
				}
				return this.rectTransform;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x060028D4 RID: 10452 RVA: 0x000427B5 File Offset: 0x000409B5
		private Image Image
		{
			get
			{
				if (this.image == null)
				{
					this.image = base.GetComponent<Image>();
				}
				return this.image;
			}
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x000427D7 File Offset: 0x000409D7
		private RectTransform GetParent()
		{
			return (this.useCanvasAsParent ? base.GetComponentInParent<Canvas>().transform : this.RectTransform.parent) as RectTransform;
		}

		// Token: 0x04001D44 RID: 7492
		private static readonly Vector2 pivot = new Vector2(0.5f, 0.5f);

		// Token: 0x04001D45 RID: 7493
		private static readonly Vector2 anchor = new Vector2(0.5f, 0.5f);

		// Token: 0x04001D46 RID: 7494
		[SerializeField]
		private bool useCanvasAsParent = true;

		// Token: 0x04001D47 RID: 7495
		private DrivenRectTransformTracker tracker;

		// Token: 0x04001D48 RID: 7496
		private RectTransform rectTransform;

		// Token: 0x04001D49 RID: 7497
		private Image image;
	}
}

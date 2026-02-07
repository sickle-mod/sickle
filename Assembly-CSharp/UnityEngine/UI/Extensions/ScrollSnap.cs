using System;
using System.Collections;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200070B RID: 1803
	[ExecuteInEditMode]
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/Scroll Snap")]
	public class ScrollSnap : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler, IScrollSnap
	{
		// Token: 0x1400013B RID: 315
		// (add) Token: 0x0600364C RID: 13900 RVA: 0x00140DEC File Offset: 0x0013EFEC
		// (remove) Token: 0x0600364D RID: 13901 RVA: 0x00140E24 File Offset: 0x0013F024
		public event ScrollSnap.PageSnapChange onPageChange;

		// Token: 0x0600364E RID: 13902 RVA: 0x00140E5C File Offset: 0x0013F05C
		private void Start()
		{
			this._lerp = false;
			this._scroll_rect = base.gameObject.GetComponent<ScrollRect>();
			this._scrollRectTransform = base.gameObject.GetComponent<RectTransform>();
			this._listContainerTransform = this._scroll_rect.content;
			this._listContainerRectTransform = this._listContainerTransform.GetComponent<RectTransform>();
			this.UpdateListItemsSize();
			this.UpdateListItemPositions();
			this.ChangePage(this.CurrentPage());
			if (this._scroll_rect.verticalScrollbar != null && this._scroll_rect.vertical)
			{
				this._scroll_rect.verticalScrollbar.gameObject.GetOrAddComponent<ScrollSnapScrollbarHelper>().ss = this;
			}
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x00140F08 File Offset: 0x0013F108
		public void UpdateListItemsSize()
		{
			float num = this._scrollRectTransform.rect.height / (float)this.ItemsVisibleAtOnce;
			float num2 = this._listContainerRectTransform.rect.height / (float)this._itemsCount;
			this._itemSize = num;
			if (this.LinkScrolrectScrollSensitivity)
			{
				this._scroll_rect.scrollSensitivity = this._itemSize;
			}
			if (this.AutoLayoutItems && num2 != num && this._itemsCount > 0)
			{
				foreach (object obj in this._listContainerTransform)
				{
					GameObject gameObject = ((Transform)obj).gameObject;
					if (gameObject.activeInHierarchy)
					{
						LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();
						if (layoutElement == null)
						{
							layoutElement = gameObject.AddComponent<LayoutElement>();
						}
						layoutElement.minHeight = this._itemSize;
					}
				}
			}
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x00141004 File Offset: 0x0013F204
		public void UpdateListItemPositions()
		{
			if (!this._listContainerRectTransform.rect.size.Equals(this._listContainerCachedSize))
			{
				int num = 0;
				using (IEnumerator enumerator = this._listContainerTransform.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((Transform)enumerator.Current).gameObject.activeInHierarchy)
						{
							num++;
						}
					}
				}
				this._itemsCount = 0;
				Array.Resize<Vector3>(ref this._pageAnchorPositions, num);
				if (num > 0)
				{
					this._pages = Mathf.Max(num - this.ItemsVisibleAtOnce + 1, 1);
					this._scroll_rect.verticalNormalizedPosition = 1f;
					this._listContainerMinPosition = this._listContainerTransform.localPosition.y;
					this._scroll_rect.verticalNormalizedPosition = 0f;
					this._listContainerMaxPosition = this._listContainerTransform.localPosition.y;
					this._listContainerSize = this._listContainerMaxPosition - this._listContainerMinPosition;
					for (int i = 0; i < this._pages; i++)
					{
						this._pageAnchorPositions[i] = new Vector3(this._listContainerTransform.localPosition.x, this._listContainerMinPosition + this._itemSize * (float)i, this._listContainerTransform.localPosition.z);
					}
					this.UpdateScrollbar(this.LinkScrolbarSteps);
					this._startingPage = Mathf.Min(this._startingPage, this._pages);
					this.ResetPage();
				}
				if (this._itemsCount != num)
				{
					this.PageChanged(this.CurrentPage());
				}
				this._itemsCount = num;
				this._listContainerCachedSize.Set(this._listContainerRectTransform.rect.size.x, this._listContainerRectTransform.rect.size.y);
			}
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x0004AD03 File Offset: 0x00048F03
		public void ResetPage()
		{
			this._scroll_rect.verticalNormalizedPosition = ((this._pages > 1) ? ((float)(this._pages - this._startingPage - 1) / (float)(this._pages - 1)) : 0f);
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x001411F8 File Offset: 0x0013F3F8
		private void UpdateScrollbar(bool linkSteps)
		{
			if (linkSteps)
			{
				if (this._scroll_rect.verticalScrollbar != null)
				{
					this._scroll_rect.verticalScrollbar.numberOfSteps = this._pages;
					return;
				}
			}
			else if (this._scroll_rect.verticalScrollbar != null)
			{
				this._scroll_rect.verticalScrollbar.numberOfSteps = 0;
			}
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x00141258 File Offset: 0x0013F458
		private void LateUpdate()
		{
			this.UpdateListItemsSize();
			this.UpdateListItemPositions();
			if (this._lerp)
			{
				this.UpdateScrollbar(false);
				this._listContainerTransform.localPosition = Vector3.Lerp(this._listContainerTransform.localPosition, this._lerpTarget, 7.5f * Time.deltaTime);
				if (Vector3.Distance(this._listContainerTransform.localPosition, this._lerpTarget) < 0.001f)
				{
					this._listContainerTransform.localPosition = this._lerpTarget;
					this._lerp = false;
					this.UpdateScrollbar(this.LinkScrolbarSteps);
				}
				if (Vector3.Distance(this._listContainerTransform.localPosition, this._lerpTarget) < 10f)
				{
					this.PageChanged(this.CurrentPage());
				}
			}
			if (this._fastSwipeTimer)
			{
				this._fastSwipeCounter++;
			}
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x00141330 File Offset: 0x0013F530
		private void NextScreenCommand()
		{
			if (this._pageOnDragStart < this._pages - 1)
			{
				int num = Mathf.Min(this._pages - 1, this._pageOnDragStart + this.ItemsVisibleAtOnce);
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[num];
				this.PageChanged(num);
			}
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x00141388 File Offset: 0x0013F588
		private void PrevScreenCommand()
		{
			if (this._pageOnDragStart > 0)
			{
				int num = Mathf.Max(0, this._pageOnDragStart - this.ItemsVisibleAtOnce);
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[num];
				this.PageChanged(num);
			}
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x001413D4 File Offset: 0x0013F5D4
		public int CurrentPage()
		{
			return Mathf.Clamp(Mathf.RoundToInt(Mathf.Clamp(this._listContainerTransform.localPosition.y - this._listContainerMinPosition, 0f, this._listContainerSize) / this._itemSize), 0, this._pages);
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x0004AD3A File Offset: 0x00048F3A
		public void SetLerp(bool value)
		{
			this._lerp = value;
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x0004AD43 File Offset: 0x00048F43
		public void ChangePage(int page)
		{
			if (0 <= page && page < this._pages)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[page];
				this.PageChanged(page);
			}
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x0004AD72 File Offset: 0x00048F72
		private void PageChanged(int currentPage)
		{
			this._startingPage = currentPage;
			if (this.onPageChange != null)
			{
				this.onPageChange(currentPage);
			}
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x0004AD8F File Offset: 0x00048F8F
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.UpdateScrollbar(false);
			this._fastSwipeCounter = 0;
			this._fastSwipeTimer = true;
			this._positionOnDragStart = eventData.position;
			this._pageOnDragStart = this.CurrentPage();
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x00141420 File Offset: 0x0013F620
		public void OnEndDrag(PointerEventData eventData)
		{
			this._startDrag = true;
			float num = -this._positionOnDragStart.y + eventData.position.y;
			if (!this.UseFastSwipe)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[this.CurrentPage()];
				return;
			}
			this.fastSwipe = false;
			this._fastSwipeTimer = false;
			if (this._fastSwipeCounter <= this._fastSwipeTarget && Math.Abs(num) > (float)this.FastSwipeThreshold)
			{
				this.fastSwipe = true;
			}
			if (!this.fastSwipe)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[this.CurrentPage()];
				return;
			}
			if (num > 0f)
			{
				this.NextScreenCommand();
				return;
			}
			this.PrevScreenCommand();
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x0004ADC3 File Offset: 0x00048FC3
		public void OnDrag(PointerEventData eventData)
		{
			this._lerp = false;
			if (this._startDrag)
			{
				this.OnBeginDrag(eventData);
				this._startDrag = false;
			}
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void StartScreenChange()
		{
		}

		// Token: 0x040027AD RID: 10157
		private ScrollRect _scroll_rect;

		// Token: 0x040027AE RID: 10158
		private RectTransform _scrollRectTransform;

		// Token: 0x040027AF RID: 10159
		private Transform _listContainerTransform;

		// Token: 0x040027B0 RID: 10160
		private int _pages;

		// Token: 0x040027B1 RID: 10161
		private int _startingPage;

		// Token: 0x040027B2 RID: 10162
		private Vector3[] _pageAnchorPositions;

		// Token: 0x040027B3 RID: 10163
		private Vector3 _lerpTarget;

		// Token: 0x040027B4 RID: 10164
		private bool _lerp;

		// Token: 0x040027B5 RID: 10165
		private float _listContainerMinPosition;

		// Token: 0x040027B6 RID: 10166
		private float _listContainerMaxPosition;

		// Token: 0x040027B7 RID: 10167
		private float _listContainerSize;

		// Token: 0x040027B8 RID: 10168
		private RectTransform _listContainerRectTransform;

		// Token: 0x040027B9 RID: 10169
		private Vector2 _listContainerCachedSize;

		// Token: 0x040027BA RID: 10170
		private float _itemSize;

		// Token: 0x040027BB RID: 10171
		private int _itemsCount;

		// Token: 0x040027BC RID: 10172
		private bool _startDrag = true;

		// Token: 0x040027BD RID: 10173
		private Vector3 _positionOnDragStart;

		// Token: 0x040027BE RID: 10174
		private int _pageOnDragStart;

		// Token: 0x040027BF RID: 10175
		private bool _fastSwipeTimer;

		// Token: 0x040027C0 RID: 10176
		private int _fastSwipeCounter;

		// Token: 0x040027C1 RID: 10177
		private int _fastSwipeTarget = 10;

		// Token: 0x040027C2 RID: 10178
		[Tooltip("Number of items visible in one page of scroll frame.")]
		[Range(1f, 100f)]
		public int ItemsVisibleAtOnce = 1;

		// Token: 0x040027C3 RID: 10179
		[Tooltip("Sets minimum width of list items to 1/itemsVisibleAtOnce.")]
		public bool AutoLayoutItems = true;

		// Token: 0x040027C4 RID: 10180
		[Tooltip("If you wish to update scrollbar numberOfSteps to number of active children on list.")]
		public bool LinkScrolbarSteps;

		// Token: 0x040027C5 RID: 10181
		[Tooltip("If you wish to update scrollrect sensitivity to size of list element.")]
		public bool LinkScrolrectScrollSensitivity;

		// Token: 0x040027C6 RID: 10182
		public bool UseFastSwipe = true;

		// Token: 0x040027C7 RID: 10183
		public int FastSwipeThreshold = 100;

		// Token: 0x040027C9 RID: 10185
		public ScrollSnap.ScrollDirection direction = ScrollSnap.ScrollDirection.Vertical;

		// Token: 0x040027CA RID: 10186
		private bool fastSwipe;

		// Token: 0x0200070C RID: 1804
		public enum ScrollDirection
		{
			// Token: 0x040027CC RID: 10188
			Horizontal,
			// Token: 0x040027CD RID: 10189
			Vertical
		}

		// Token: 0x0200070D RID: 1805
		// (Invoke) Token: 0x06003660 RID: 13920
		public delegate void PageSnapChange(int page);
	}
}

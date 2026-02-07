using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200070E RID: 1806
	public class ScrollSnapScrollbarHelper : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
	{
		// Token: 0x06003663 RID: 13923 RVA: 0x0004AE1D File Offset: 0x0004901D
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.OnScrollBarDown();
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x0004AE25 File Offset: 0x00049025
		public void OnDrag(PointerEventData eventData)
		{
			this.ss.CurrentPage();
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x0004AE33 File Offset: 0x00049033
		public void OnEndDrag(PointerEventData eventData)
		{
			this.OnScrollBarUp();
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x0004AE1D File Offset: 0x0004901D
		public void OnPointerDown(PointerEventData eventData)
		{
			this.OnScrollBarDown();
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x0004AE33 File Offset: 0x00049033
		public void OnPointerUp(PointerEventData eventData)
		{
			this.OnScrollBarUp();
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x0004AE3B File Offset: 0x0004903B
		private void OnScrollBarDown()
		{
			if (this.ss != null)
			{
				this.ss.SetLerp(false);
				this.ss.StartScreenChange();
			}
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x0004AE5C File Offset: 0x0004905C
		private void OnScrollBarUp()
		{
			this.ss.SetLerp(true);
			this.ss.ChangePage(this.ss.CurrentPage());
		}

		// Token: 0x040027CE RID: 10190
		internal IScrollSnap ss;
	}
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007D7 RID: 2007
	public class TableViewCell : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06003979 RID: 14713 RVA: 0x0004680F File Offset: 0x00044A0F
		public virtual string ReuseIdentifier
		{
			get
			{
				return base.GetType().Name;
			}
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void Clean()
		{
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x0600397B RID: 14715 RVA: 0x0004CEEA File Offset: 0x0004B0EA
		// (set) Token: 0x0600397C RID: 14716 RVA: 0x0004CEF2 File Offset: 0x0004B0F2
		public Action<TableViewCell> OnSelection { private get; set; }

		// Token: 0x0600397D RID: 14717 RVA: 0x0004CEFB File Offset: 0x0004B0FB
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.OnSelection != null)
			{
				this.OnSelection(this);
			}
		}
	}
}

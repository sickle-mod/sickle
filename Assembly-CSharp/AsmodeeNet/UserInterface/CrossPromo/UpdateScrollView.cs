using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000824 RID: 2084
	public class UpdateScrollView : MonoBehaviour
	{
		// Token: 0x06003B1A RID: 15130 RVA: 0x0004E3F7 File Offset: 0x0004C5F7
		private void Awake()
		{
			this.rt = base.transform as RectTransform;
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x0015277C File Offset: 0x0015097C
		private void Update()
		{
			if (!Mathf.Approximately(this.rt.sizeDelta.y, 0f))
			{
				this.rt.sizeDelta = Vector2.zero;
				for (int i = 0; i < this.rt.childCount; i++)
				{
					LayoutElement component = this.rt.GetChild(i).GetComponent<LayoutElement>();
					component.preferredHeight = this.rt.rect.height;
					component.preferredWidth = this.rt.rect.height;
				}
			}
		}

		// Token: 0x04002CD2 RID: 11474
		private RectTransform rt;
	}
}

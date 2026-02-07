using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000132 RID: 306
public class HorizontalPercentageLayoutGroup : HorizontalLayoutGroup
{
	// Token: 0x0600093D RID: 2365 RVA: 0x0002E513 File Offset: 0x0002C713
	public override void SetLayoutHorizontal()
	{
		this.RecalculateSpacing();
		this.RescaleChildren();
		base.SetLayoutHorizontal();
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0007B054 File Offset: 0x00079254
	protected void RescaleChildren()
	{
		foreach (object obj in base.transform)
		{
			RectTransform rectTransform = (RectTransform)obj;
			float num = base.rectTransform.rect.width * this.percentageWidth;
			float num2 = (this.expandHeight ? base.rectTransform.rect.height : rectTransform.sizeDelta.y);
			rectTransform.sizeDelta = new Vector2(num, num2);
		}
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x0007B0FC File Offset: 0x000792FC
	protected void RecalculateSpacing()
	{
		base.spacing = base.rectTransform.rect.width * this.percentageSpacing;
	}

	// Token: 0x04000874 RID: 2164
	[Range(0f, 1f)]
	public float percentageWidth = 0.3f;

	// Token: 0x04000875 RID: 2165
	[Range(0f, 1f)]
	public float percentageSpacing = 0.05f;

	// Token: 0x04000876 RID: 2166
	public bool expandHeight;
}

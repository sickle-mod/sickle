using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000171 RID: 369
public class VerticalPercentageLayoutGroup : VerticalLayoutGroup
{
	// Token: 0x06000A85 RID: 2693 RVA: 0x0002F3D7 File Offset: 0x0002D5D7
	public override void SetLayoutVertical()
	{
		this.RecalculateSpacing();
		this.RescaleChildren();
		base.SetLayoutVertical();
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x0007D878 File Offset: 0x0007BA78
	protected void RescaleChildren()
	{
		foreach (object obj in base.transform)
		{
			RectTransform rectTransform = (RectTransform)obj;
			float num = base.rectTransform.rect.height * this.percentageHeight;
			float num2 = (this.expandWidth ? base.rectTransform.rect.width : rectTransform.sizeDelta.x);
			rectTransform.sizeDelta = new Vector2(num2, num);
		}
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x0007D920 File Offset: 0x0007BB20
	protected void RecalculateSpacing()
	{
		base.spacing = base.rectTransform.rect.height * this.percentageSpacing;
	}

	// Token: 0x04000935 RID: 2357
	[Range(0f, 1f)]
	public float percentageHeight = 0.3f;

	// Token: 0x04000936 RID: 2358
	[Range(0f, 1f)]
	public float percentageSpacing = 0.05f;

	// Token: 0x04000937 RID: 2359
	public bool expandWidth;
}

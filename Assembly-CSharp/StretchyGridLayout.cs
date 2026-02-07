using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000167 RID: 359
public class StretchyGridLayout : LayoutGroup
{
	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0002F178 File Offset: 0x0002D378
	// (set) Token: 0x06000A62 RID: 2658 RVA: 0x0002F180 File Offset: 0x0002D380
	public StretchyGridLayout.Corner startCorner
	{
		get
		{
			return this.m_StartCorner;
		}
		set
		{
			base.SetProperty<StretchyGridLayout.Corner>(ref this.m_StartCorner, value);
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0002F18F File Offset: 0x0002D38F
	// (set) Token: 0x06000A64 RID: 2660 RVA: 0x0002F197 File Offset: 0x0002D397
	public StretchyGridLayout.Axis startAxis
	{
		get
		{
			return this.m_StartAxis;
		}
		set
		{
			base.SetProperty<StretchyGridLayout.Axis>(ref this.m_StartAxis, value);
		}
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0002F1A6 File Offset: 0x0002D3A6
	// (set) Token: 0x06000A66 RID: 2662 RVA: 0x0002F1AE File Offset: 0x0002D3AE
	public Vector2 cellSize
	{
		get
		{
			return this.m_CellSize;
		}
		set
		{
			base.SetProperty<Vector2>(ref this.m_CellSize, value);
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0002F1BD File Offset: 0x0002D3BD
	// (set) Token: 0x06000A68 RID: 2664 RVA: 0x0002F1C5 File Offset: 0x0002D3C5
	public Vector2 spacing
	{
		get
		{
			return this.m_Spacing;
		}
		set
		{
			base.SetProperty<Vector2>(ref this.m_Spacing, value);
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0002F1D4 File Offset: 0x0002D3D4
	// (set) Token: 0x06000A6A RID: 2666 RVA: 0x0002F1DC File Offset: 0x0002D3DC
	public StretchyGridLayout.Constraint constraint
	{
		get
		{
			return this.m_Constraint;
		}
		set
		{
			base.SetProperty<StretchyGridLayout.Constraint>(ref this.m_Constraint, value);
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0002F1EB File Offset: 0x0002D3EB
	// (set) Token: 0x06000A6C RID: 2668 RVA: 0x0002F1F3 File Offset: 0x0002D3F3
	public int constraintCount
	{
		get
		{
			return this.m_ConstraintCount;
		}
		set
		{
			base.SetProperty<int>(ref this.m_ConstraintCount, Mathf.Max(1, value));
		}
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0007D144 File Offset: 0x0007B344
	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();
		int num2;
		int num;
		if (this.m_Constraint == StretchyGridLayout.Constraint.FixedColumnCount)
		{
			num = (num2 = this.m_ConstraintCount);
		}
		else if (this.m_Constraint == StretchyGridLayout.Constraint.FixedRowCount)
		{
			num = (num2 = Mathf.CeilToInt((float)base.rectChildren.Count / (float)this.m_ConstraintCount - 0.001f));
		}
		else
		{
			num2 = 1;
			num = Mathf.CeilToInt(Mathf.Sqrt((float)base.rectChildren.Count));
		}
		base.SetLayoutInputForAxis((float)base.padding.horizontal + (this.cellSize.x + this.spacing.x) * (float)num2 - this.spacing.x, (float)base.padding.horizontal + (this.cellSize.x + this.spacing.x) * (float)num - this.spacing.x, -1f, 0);
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x0007D228 File Offset: 0x0007B428
	public override void CalculateLayoutInputVertical()
	{
		int num;
		if (this.m_Constraint == StretchyGridLayout.Constraint.FixedColumnCount)
		{
			num = Mathf.CeilToInt((float)base.rectChildren.Count / (float)this.m_ConstraintCount - 0.001f);
		}
		else if (this.m_Constraint == StretchyGridLayout.Constraint.FixedRowCount)
		{
			num = this.m_ConstraintCount;
		}
		else
		{
			float x = base.rectTransform.rect.size.x;
			int num2 = Mathf.Max(1, Mathf.FloorToInt((x - (float)base.padding.horizontal + this.spacing.x + 0.001f) / (this.cellSize.x + this.spacing.x)));
			num = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num2);
		}
		float num3 = (float)base.padding.vertical + (this.cellSize.y + this.spacing.y) * (float)num - this.spacing.y;
		base.SetLayoutInputForAxis(num3, num3, -1f, 1);
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x0002F208 File Offset: 0x0002D408
	public override void SetLayoutHorizontal()
	{
		this.ResizeCells();
		this.SetCellsAlongAxis(0);
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x0002F217 File Offset: 0x0002D417
	public override void SetLayoutVertical()
	{
		this.ResizeCells();
		this.SetCellsAlongAxis(1);
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0007D32C File Offset: 0x0007B52C
	protected void ResizeCells()
	{
		if (this.m_Constraint == StretchyGridLayout.Constraint.FixedColumnCount)
		{
			float num = (base.rectTransform.rect.size.x - this.spacing.x * (float)(this.m_ConstraintCount - 1) - (float)base.padding.left - (float)base.padding.right) / (float)this.m_ConstraintCount;
			float num2 = num / this.cellSize.x * this.cellSize.y;
			this.cellSize = new Vector2(num, num2);
			return;
		}
		if (this.m_Constraint == StretchyGridLayout.Constraint.FixedRowCount)
		{
			float num3 = (base.rectTransform.rect.size.y - this.spacing.y * (float)(this.m_ConstraintCount - 1) - (float)base.padding.top - (float)base.padding.bottom) / (float)this.m_ConstraintCount;
			float num4 = num3 / this.cellSize.y * this.cellSize.x;
			this.cellSize = new Vector2(num4, num3);
		}
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x0007D440 File Offset: 0x0007B640
	private void SetCellsAlongAxis(int axis)
	{
		if (axis == 0)
		{
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				RectTransform rectTransform = base.rectChildren[i];
				this.m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
				rectTransform.anchorMin = Vector2.up;
				rectTransform.anchorMax = Vector2.up;
				rectTransform.sizeDelta = this.cellSize;
			}
			return;
		}
		float x = base.rectTransform.rect.size.x;
		float y = base.rectTransform.rect.size.y;
		int num;
		int num2;
		if (this.m_Constraint == StretchyGridLayout.Constraint.FixedColumnCount)
		{
			num = this.m_ConstraintCount;
			num2 = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num - 0.001f);
		}
		else if (this.m_Constraint == StretchyGridLayout.Constraint.FixedRowCount)
		{
			num2 = this.m_ConstraintCount;
			num = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num2 - 0.001f);
		}
		else
		{
			if (this.cellSize.x + this.spacing.x <= 0f)
			{
				num = int.MaxValue;
			}
			else
			{
				num = Mathf.Max(1, Mathf.FloorToInt((x - (float)base.padding.horizontal + this.spacing.x + 0.001f) / (this.cellSize.x + this.spacing.x)));
			}
			if (this.cellSize.y + this.spacing.y <= 0f)
			{
				num2 = int.MaxValue;
			}
			else
			{
				num2 = Mathf.Max(1, Mathf.FloorToInt((y - (float)base.padding.vertical + this.spacing.y + 0.001f) / (this.cellSize.y + this.spacing.y)));
			}
		}
		int num3 = (int)(this.startCorner % StretchyGridLayout.Corner.LowerLeft);
		int num4 = (int)(this.startCorner / StretchyGridLayout.Corner.LowerLeft);
		int num5;
		int num6;
		int num7;
		if (this.startAxis == StretchyGridLayout.Axis.Horizontal)
		{
			num5 = num;
			num6 = Mathf.Clamp(num, 1, base.rectChildren.Count);
			num7 = Mathf.Clamp(num2, 1, Mathf.CeilToInt((float)base.rectChildren.Count / (float)num5));
		}
		else
		{
			num5 = num2;
			num7 = Mathf.Clamp(num2, 1, base.rectChildren.Count);
			num6 = Mathf.Clamp(num, 1, Mathf.CeilToInt((float)base.rectChildren.Count / (float)num5));
		}
		Vector2 vector = new Vector2((float)num6 * this.cellSize.x + (float)(num6 - 1) * this.spacing.x, (float)num7 * this.cellSize.y + (float)(num7 - 1) * this.spacing.y);
		Vector2 vector2 = new Vector2(base.GetStartOffset(0, vector.x), base.GetStartOffset(1, vector.y));
		for (int j = 0; j < base.rectChildren.Count; j++)
		{
			int num8;
			int num9;
			if (this.startAxis == StretchyGridLayout.Axis.Horizontal)
			{
				num8 = j % num5;
				num9 = j / num5;
			}
			else
			{
				num8 = j / num5;
				num9 = j % num5;
			}
			if (num3 == 1)
			{
				num8 = num6 - 1 - num8;
			}
			if (num4 == 1)
			{
				num9 = num7 - 1 - num9;
			}
			base.SetChildAlongAxis(base.rectChildren[j], 0, vector2.x + (this.cellSize[0] + this.spacing[0]) * (float)num8, this.cellSize[0]);
			base.SetChildAlongAxis(base.rectChildren[j], 1, vector2.y + (this.cellSize[1] + this.spacing[1]) * (float)num9, this.cellSize[1]);
		}
	}

	// Token: 0x04000917 RID: 2327
	[SerializeField]
	protected StretchyGridLayout.Corner m_StartCorner;

	// Token: 0x04000918 RID: 2328
	[SerializeField]
	protected StretchyGridLayout.Axis m_StartAxis;

	// Token: 0x04000919 RID: 2329
	[SerializeField]
	protected Vector2 m_CellSize = new Vector2(100f, 100f);

	// Token: 0x0400091A RID: 2330
	[SerializeField]
	protected Vector2 m_Spacing = Vector2.zero;

	// Token: 0x0400091B RID: 2331
	[SerializeField]
	protected StretchyGridLayout.Constraint m_Constraint = StretchyGridLayout.Constraint.FixedColumnCount;

	// Token: 0x0400091C RID: 2332
	[SerializeField]
	protected int m_ConstraintCount = 2;

	// Token: 0x02000168 RID: 360
	public enum Corner
	{
		// Token: 0x0400091E RID: 2334
		UpperLeft,
		// Token: 0x0400091F RID: 2335
		UpperRight,
		// Token: 0x04000920 RID: 2336
		LowerLeft,
		// Token: 0x04000921 RID: 2337
		LowerRight
	}

	// Token: 0x02000169 RID: 361
	public enum Axis
	{
		// Token: 0x04000923 RID: 2339
		Horizontal,
		// Token: 0x04000924 RID: 2340
		Vertical
	}

	// Token: 0x0200016A RID: 362
	public enum Constraint
	{
		// Token: 0x04000926 RID: 2342
		FixedColumnCount = 1,
		// Token: 0x04000927 RID: 2343
		FixedRowCount
	}
}

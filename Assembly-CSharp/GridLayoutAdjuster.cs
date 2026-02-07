using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000130 RID: 304
public class GridLayoutAdjuster : MonoBehaviour
{
	// Token: 0x06000933 RID: 2355 RVA: 0x0002E4DA File Offset: 0x0002C6DA
	private void Awake()
	{
		this.AdjustGridLayout();
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0002E4ED File Offset: 0x0002C6ED
	private IEnumerator DelayedAdjustGridLayout()
	{
		yield return new WaitForEndOfFrame();
		this.AdjustGridLayout();
		yield break;
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0007AE70 File Offset: 0x00079070
	private void AdjustGridLayout()
	{
		if (this.gridLayoutGroup == null)
		{
			this.gridLayoutGroup = base.transform.GetComponent<GridLayoutGroup>();
		}
		Vector2 vector = this.gridLayoutGroup.GetComponent<RectTransform>().rect.size;
		vector.x -= (float)this.padding.left;
		vector.x -= (float)this.padding.right;
		vector.y -= (float)this.padding.top;
		vector.y -= (float)this.padding.bottom;
		if (this.cellsCount.x <= 0 || this.cellsCount.y <= 0)
		{
			Debug.LogError("GridLayoutAdjuster] Cells count can't be less then 1");
			return;
		}
		Vector2 vector2;
		vector2.x = (float)(this.cellsCount.x - 1) * this.spacing.x;
		vector2.y = (float)(this.cellsCount.y - 1) * this.spacing.y;
		vector -= vector2;
		if (vector.x < 0f || vector.y < 0f)
		{
			Debug.LogError("[GridLayoutAdjuster] Too little size of grid layout to apply adjuster.");
			return;
		}
		this.gridLayoutGroup.padding = this.padding;
		this.gridLayoutGroup.cellSize = new Vector2(vector.x / (float)this.cellsCount.x, vector.y / (float)this.cellsCount.y);
		this.gridLayoutGroup.spacing = this.spacing;
	}

	// Token: 0x0400086D RID: 2157
	public RectOffset padding;

	// Token: 0x0400086E RID: 2158
	public Vector2Int cellsCount;

	// Token: 0x0400086F RID: 2159
	public Vector2 spacing;

	// Token: 0x04000870 RID: 2160
	private GridLayoutGroup gridLayoutGroup;
}

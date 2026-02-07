using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class MinimizingScreen : MonoBehaviour
{
	// Token: 0x14000033 RID: 51
	// (add) Token: 0x06000694 RID: 1684 RVA: 0x00070850 File Offset: 0x0006EA50
	// (remove) Token: 0x06000695 RID: 1685 RVA: 0x00070888 File Offset: 0x0006EA88
	public event MinimizingScreen.MaximizeEnd OnMaximizeEnd;

	// Token: 0x06000696 RID: 1686 RVA: 0x0002C37F File Offset: 0x0002A57F
	private void Awake()
	{
		this.targetRectTransform = base.GetComponent<RectTransform>();
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x000708C0 File Offset: 0x0006EAC0
	public void Minimize()
	{
		Vector2 vector;
		Vector2 vector2;
		MinimizingScreen.AnchorPresetToVectors(this.minimizedAnchor, out vector, out vector2);
		Vector2 vector3;
		MinimizingScreen.PivotPresetToVector(this.minimizedPivot, out vector3);
		this.targetRectTransform.DOAnchorMin(vector, this.minimalizationDuration, false);
		this.targetRectTransform.DOAnchorMax(vector2, this.minimalizationDuration, false);
		this.targetRectTransform.DOPivot(vector3, this.minimalizationDuration);
		this.targetRectTransform.DOScale(this.minimizedScale, this.minimalizationDuration);
		if (vector.x == vector2.x)
		{
			this.targetRectTransform.DOAnchorPosX(this.minimizedPositionX, this.minimalizationDuration, false);
			DOTween.To(delegate(float value)
			{
				this.targetRectTransform.sizeDelta = new Vector2(value, this.targetRectTransform.sizeDelta.y);
			}, this.targetRectTransform.sizeDelta.x, this.minimizedWidth, this.minimalizationDuration);
		}
		else
		{
			DOTween.To(delegate(float value)
			{
				this.targetRectTransform.offsetMin = new Vector2(value, this.targetRectTransform.offsetMin.y);
			}, this.targetRectTransform.offsetMin.x, this.minimizedLeftMargin, this.minimalizationDuration);
			DOTween.To(delegate(float value)
			{
				this.targetRectTransform.offsetMax = new Vector2(value, this.targetRectTransform.offsetMax.y);
			}, this.targetRectTransform.offsetMax.x, -this.minimizedRightMargin, this.minimalizationDuration);
		}
		if (vector.y == vector2.y)
		{
			this.targetRectTransform.DOAnchorPosY(this.minimizedPositionY, this.minimalizationDuration, false);
			DOTween.To(delegate(float value)
			{
				this.targetRectTransform.sizeDelta = new Vector2(this.targetRectTransform.sizeDelta.x, value);
			}, this.targetRectTransform.sizeDelta.y, this.minimizedHeight, this.minimalizationDuration);
			return;
		}
		DOTween.To(delegate(float value)
		{
			this.targetRectTransform.offsetMin = new Vector2(this.targetRectTransform.offsetMin.x, value);
		}, this.targetRectTransform.offsetMin.y, this.minimizedBottomMargin, this.minimalizationDuration);
		DOTween.To(delegate(float value)
		{
			this.targetRectTransform.offsetMax = new Vector2(this.targetRectTransform.offsetMax.x, value);
		}, this.targetRectTransform.offsetMax.y, -this.minimizedTopMargin, this.minimalizationDuration);
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x00070AA4 File Offset: 0x0006ECA4
	public void Maximize()
	{
		Vector2 vector;
		Vector2 vector2;
		MinimizingScreen.AnchorPresetToVectors(this.maximizedAnchor, out vector, out vector2);
		Vector2 vector3;
		MinimizingScreen.PivotPresetToVector(this.maximizedPivot, out vector3);
		this.targetRectTransform.DOAnchorMin(vector, this.maximalizationDuration, false);
		this.targetRectTransform.DOAnchorMax(vector2, this.maximalizationDuration, false);
		this.targetRectTransform.DOPivot(vector3, this.maximalizationDuration);
		this.targetRectTransform.DOScale(this.maximizedScale, this.maximalizationDuration);
		if (vector.x == vector2.x)
		{
			this.targetRectTransform.DOAnchorPosX(this.maximizedPositionX, this.maximalizationDuration, false);
			DOTween.To(delegate(float value)
			{
				this.targetRectTransform.sizeDelta = new Vector2(value, this.targetRectTransform.sizeDelta.y);
			}, this.targetRectTransform.sizeDelta.x, this.maximizedWidth, this.maximalizationDuration).OnComplete(new TweenCallback(this.MaximizeComplete));
		}
		else
		{
			DOTween.To(delegate(float value)
			{
				this.targetRectTransform.offsetMin = new Vector2(value, this.targetRectTransform.offsetMin.y);
			}, this.targetRectTransform.offsetMin.x, this.maximizedLeftMargin, this.maximalizationDuration).OnComplete(new TweenCallback(this.MaximizeComplete));
			DOTween.To(delegate(float value)
			{
				this.targetRectTransform.offsetMax = new Vector2(value, this.targetRectTransform.offsetMax.y);
			}, this.targetRectTransform.offsetMax.x, -this.maximizedRightMargin, this.maximalizationDuration).OnComplete(new TweenCallback(this.MaximizeComplete));
		}
		if (vector.y == vector2.y)
		{
			this.targetRectTransform.DOAnchorPosY(this.maximizedPositionY, this.maximalizationDuration, false);
			DOTween.To(delegate(float value)
			{
				this.targetRectTransform.sizeDelta = new Vector2(this.targetRectTransform.sizeDelta.x, value);
			}, this.targetRectTransform.sizeDelta.y, this.maximizedHeight, this.maximalizationDuration).OnComplete(new TweenCallback(this.MaximizeComplete));
			return;
		}
		DOTween.To(delegate(float value)
		{
			this.targetRectTransform.offsetMin = new Vector2(this.targetRectTransform.offsetMin.x, value);
		}, this.targetRectTransform.offsetMin.y, this.maximizedBottomMargin, this.maximalizationDuration).OnComplete(new TweenCallback(this.MaximizeComplete));
		DOTween.To(delegate(float value)
		{
			this.targetRectTransform.offsetMax = new Vector2(this.targetRectTransform.offsetMax.x, value);
		}, this.targetRectTransform.offsetMax.y, -this.maximizedTopMargin, this.maximalizationDuration).OnComplete(new TweenCallback(this.MaximizeComplete));
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x00070CF0 File Offset: 0x0006EEF0
	public void Reset()
	{
		Vector2 vector;
		Vector2 vector2;
		MinimizingScreen.AnchorPresetToVectors(this.maximizedAnchor, out vector, out vector2);
		Vector2 vector3;
		MinimizingScreen.PivotPresetToVector(this.maximizedPivot, out vector3);
		this.targetRectTransform.anchorMin = vector;
		this.targetRectTransform.anchorMax = vector2;
		this.targetRectTransform.pivot = vector3;
		this.targetRectTransform.localScale = this.maximizedScale;
		if (vector.x == vector2.x)
		{
			Vector2 anchoredPosition = this.targetRectTransform.anchoredPosition;
			anchoredPosition.x = this.maximizedPositionX;
			this.targetRectTransform.anchoredPosition = anchoredPosition;
			this.targetRectTransform.sizeDelta = new Vector2(this.maximizedWidth, this.targetRectTransform.sizeDelta.y);
		}
		else
		{
			this.targetRectTransform.offsetMin = new Vector2(this.maximizedLeftMargin, this.targetRectTransform.offsetMin.y);
			this.targetRectTransform.offsetMax = new Vector2(-this.maximizedRightMargin, this.targetRectTransform.offsetMax.y);
		}
		if (vector.y == vector2.y)
		{
			this.targetRectTransform.anchoredPosition = new Vector2(this.targetRectTransform.anchoredPosition.x, this.maximizedPositionY);
			this.targetRectTransform.sizeDelta = new Vector2(this.targetRectTransform.sizeDelta.x, this.maximizedHeight);
			return;
		}
		this.targetRectTransform.offsetMin = new Vector2(this.targetRectTransform.offsetMin.x, this.maximizedBottomMargin);
		this.targetRectTransform.offsetMax = new Vector2(this.targetRectTransform.offsetMax.x, -this.maximizedTopMargin);
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0002C38D File Offset: 0x0002A58D
	private void MaximizeComplete()
	{
		if (this.OnMaximizeEnd != null)
		{
			this.OnMaximizeEnd();
		}
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x00070EA0 File Offset: 0x0006F0A0
	private static void AnchorPresetToVectors(MinimizingScreen.AnchorPreset anchorPreset, out Vector2 anchorMin, out Vector2 anchorMax)
	{
		switch (anchorPreset)
		{
		case MinimizingScreen.AnchorPreset.TopLeft:
			anchorMin = new Vector2(0f, 1f);
			anchorMax = new Vector2(0f, 1f);
			return;
		case MinimizingScreen.AnchorPreset.TopCenter:
			anchorMin = new Vector2(0.5f, 1f);
			anchorMax = new Vector2(0.5f, 1f);
			return;
		case MinimizingScreen.AnchorPreset.TopRight:
			anchorMin = new Vector2(1f, 1f);
			anchorMax = new Vector2(1f, 1f);
			return;
		case MinimizingScreen.AnchorPreset.MiddleLeft:
			anchorMin = new Vector2(0f, 0.5f);
			anchorMax = new Vector2(0f, 0.5f);
			return;
		case MinimizingScreen.AnchorPreset.MiddleCenter:
			anchorMin = new Vector2(0.5f, 0.5f);
			anchorMax = new Vector2(0.5f, 0.5f);
			return;
		case MinimizingScreen.AnchorPreset.MiddleRight:
			anchorMin = new Vector2(1f, 0.5f);
			anchorMax = new Vector2(1f, 0.5f);
			return;
		case MinimizingScreen.AnchorPreset.BottomLeft:
			anchorMin = new Vector2(0f, 0f);
			anchorMax = new Vector2(0f, 0f);
			return;
		case MinimizingScreen.AnchorPreset.BottonCenter:
			anchorMin = new Vector2(0.5f, 0f);
			anchorMax = new Vector2(0.5f, 0f);
			return;
		case MinimizingScreen.AnchorPreset.BottomRight:
			anchorMin = new Vector2(1f, 0f);
			anchorMax = new Vector2(1f, 0f);
			return;
		case MinimizingScreen.AnchorPreset.VertStretchLeft:
			anchorMin = new Vector2(0f, 0f);
			anchorMax = new Vector2(0f, 1f);
			return;
		case MinimizingScreen.AnchorPreset.VertStretchRight:
			anchorMin = new Vector2(1f, 0f);
			anchorMax = new Vector2(1f, 1f);
			return;
		case MinimizingScreen.AnchorPreset.VertStretchCenter:
			anchorMin = new Vector2(0.5f, 0f);
			anchorMax = new Vector2(0.5f, 1f);
			return;
		case MinimizingScreen.AnchorPreset.HorStretchTop:
			anchorMin = new Vector2(0f, 1f);
			anchorMax = new Vector2(1f, 1f);
			return;
		case MinimizingScreen.AnchorPreset.HorStretchMiddle:
			anchorMin = new Vector2(0f, 0.5f);
			anchorMax = new Vector2(1f, 0.5f);
			return;
		case MinimizingScreen.AnchorPreset.HorStretchBottom:
			anchorMin = new Vector2(0f, 0f);
			anchorMax = new Vector2(1f, 0f);
			return;
		case MinimizingScreen.AnchorPreset.StretchAll:
			anchorMin = new Vector2(0f, 0f);
			anchorMax = new Vector2(1f, 1f);
			return;
		}
		throw new ArgumentOutOfRangeException("No defined anchors for this preset " + anchorPreset.ToString());
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x000711C8 File Offset: 0x0006F3C8
	private static void PivotPresetToVector(MinimizingScreen.PivotPreset pivotPreset, out Vector2 pivot)
	{
		switch (pivotPreset)
		{
		case MinimizingScreen.PivotPreset.TopLeft:
			pivot = new Vector2(0f, 1f);
			return;
		case MinimizingScreen.PivotPreset.TopCenter:
			pivot = new Vector2(0.5f, 1f);
			return;
		case MinimizingScreen.PivotPreset.TopRight:
			pivot = new Vector2(1f, 1f);
			return;
		case MinimizingScreen.PivotPreset.MiddleLeft:
			pivot = new Vector2(0f, 0.5f);
			return;
		case MinimizingScreen.PivotPreset.MiddleCenter:
			pivot = new Vector2(0.5f, 0.5f);
			return;
		case MinimizingScreen.PivotPreset.MiddleRight:
			pivot = new Vector2(1f, 0.5f);
			return;
		case MinimizingScreen.PivotPreset.BottomLeft:
			pivot = new Vector2(0f, 0f);
			return;
		case MinimizingScreen.PivotPreset.BottomCenter:
			pivot = new Vector2(0.5f, 0f);
			return;
		case MinimizingScreen.PivotPreset.BottomRight:
			pivot = new Vector2(1f, 0f);
			return;
		default:
			throw new ArgumentOutOfRangeException("No defined pivot for this preset " + pivotPreset.ToString());
		}
	}

	// Token: 0x040005BD RID: 1469
	[Header("Maximize")]
	[SerializeField]
	private MinimizingScreen.AnchorPreset maximizedAnchor;

	// Token: 0x040005BE RID: 1470
	[SerializeField]
	private MinimizingScreen.PivotPreset maximizedPivot;

	// Token: 0x040005BF RID: 1471
	[SerializeField]
	private float maximalizationDuration = 1f;

	// Token: 0x040005C0 RID: 1472
	[SerializeField]
	private Vector3 maximizedScale = Vector2.one;

	// Token: 0x040005C1 RID: 1473
	[Header("If stretched horizontally")]
	[SerializeField]
	private float maximizedLeftMargin;

	// Token: 0x040005C2 RID: 1474
	[SerializeField]
	private float maximizedRightMargin;

	// Token: 0x040005C3 RID: 1475
	[Header("If stretched vertically")]
	[SerializeField]
	private float maximizedTopMargin;

	// Token: 0x040005C4 RID: 1476
	[SerializeField]
	private float maximizedBottomMargin;

	// Token: 0x040005C5 RID: 1477
	[Header("If anchored horizontally")]
	[SerializeField]
	private float maximizedPositionX;

	// Token: 0x040005C6 RID: 1478
	[SerializeField]
	private float maximizedWidth;

	// Token: 0x040005C7 RID: 1479
	[Header("If anchored vertically")]
	[SerializeField]
	private float maximizedPositionY;

	// Token: 0x040005C8 RID: 1480
	[SerializeField]
	private float maximizedHeight;

	// Token: 0x040005C9 RID: 1481
	[Header("Minimize")]
	[SerializeField]
	private MinimizingScreen.AnchorPreset minimizedAnchor;

	// Token: 0x040005CA RID: 1482
	[SerializeField]
	private MinimizingScreen.PivotPreset minimizedPivot;

	// Token: 0x040005CB RID: 1483
	[SerializeField]
	private float minimalizationDuration = 1f;

	// Token: 0x040005CC RID: 1484
	[SerializeField]
	private Vector3 minimizedScale = Vector2.one;

	// Token: 0x040005CD RID: 1485
	[Header("If stretched horizontally")]
	[SerializeField]
	private float minimizedLeftMargin;

	// Token: 0x040005CE RID: 1486
	[SerializeField]
	private float minimizedRightMargin;

	// Token: 0x040005CF RID: 1487
	[Header("If stretched vertically")]
	[SerializeField]
	private float minimizedTopMargin;

	// Token: 0x040005D0 RID: 1488
	[SerializeField]
	private float minimizedBottomMargin;

	// Token: 0x040005D1 RID: 1489
	[Header("If anchored horizontally")]
	[SerializeField]
	private float minimizedPositionX;

	// Token: 0x040005D2 RID: 1490
	[SerializeField]
	private float minimizedWidth;

	// Token: 0x040005D3 RID: 1491
	[Header("If anchored vertically")]
	[SerializeField]
	private float minimizedPositionY;

	// Token: 0x040005D4 RID: 1492
	[SerializeField]
	private float minimizedHeight;

	// Token: 0x040005D5 RID: 1493
	[SerializeField]
	private RectTransform targetRectTransform;

	// Token: 0x020000E0 RID: 224
	public enum AnchorPreset
	{
		// Token: 0x040005D7 RID: 1495
		TopLeft,
		// Token: 0x040005D8 RID: 1496
		TopCenter,
		// Token: 0x040005D9 RID: 1497
		TopRight,
		// Token: 0x040005DA RID: 1498
		MiddleLeft,
		// Token: 0x040005DB RID: 1499
		MiddleCenter,
		// Token: 0x040005DC RID: 1500
		MiddleRight,
		// Token: 0x040005DD RID: 1501
		BottomLeft,
		// Token: 0x040005DE RID: 1502
		BottonCenter,
		// Token: 0x040005DF RID: 1503
		BottomRight,
		// Token: 0x040005E0 RID: 1504
		BottomStretch,
		// Token: 0x040005E1 RID: 1505
		VertStretchLeft,
		// Token: 0x040005E2 RID: 1506
		VertStretchRight,
		// Token: 0x040005E3 RID: 1507
		VertStretchCenter,
		// Token: 0x040005E4 RID: 1508
		HorStretchTop,
		// Token: 0x040005E5 RID: 1509
		HorStretchMiddle,
		// Token: 0x040005E6 RID: 1510
		HorStretchBottom,
		// Token: 0x040005E7 RID: 1511
		StretchAll
	}

	// Token: 0x020000E1 RID: 225
	public enum PivotPreset
	{
		// Token: 0x040005E9 RID: 1513
		TopLeft,
		// Token: 0x040005EA RID: 1514
		TopCenter,
		// Token: 0x040005EB RID: 1515
		TopRight,
		// Token: 0x040005EC RID: 1516
		MiddleLeft,
		// Token: 0x040005ED RID: 1517
		MiddleCenter,
		// Token: 0x040005EE RID: 1518
		MiddleRight,
		// Token: 0x040005EF RID: 1519
		BottomLeft,
		// Token: 0x040005F0 RID: 1520
		BottomCenter,
		// Token: 0x040005F1 RID: 1521
		BottomRight
	}

	// Token: 0x020000E2 RID: 226
	// (Invoke) Token: 0x060006AB RID: 1707
	public delegate void MaximizeEnd();
}

using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000149 RID: 329
[RequireComponent(typeof(RectTransform))]
public class AntiSafetyAreaScaler : MonoBehaviour
{
	// Token: 0x060009B4 RID: 2484 RVA: 0x0002E979 File Offset: 0x0002CB79
	private void Awake()
	{
		this.rectTransform = (RectTransform)base.gameObject.transform;
		this.canvasScaler = base.GetComponentInParent<CanvasScaler>();
		this.platformCanvasScaler = base.GetComponentInParent<PlatformCanvasScaler>();
		this.platformSafetyAreaScaler = base.GetComponentInParent<PlatformSafetyAreaScaler>();
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x0002E9B5 File Offset: 0x0002CBB5
	private void OnEnable()
	{
		if (this.platformCanvasScaler != null)
		{
			this.platformCanvasScaler.OnScaleFactorChanged += this.OnScaleFactorChanged;
		}
		this.CounterSafetyArea();
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x0002E9E2 File Offset: 0x0002CBE2
	private void OnDisable()
	{
		if (this.platformCanvasScaler != null)
		{
			this.platformCanvasScaler.OnScaleFactorChanged -= this.OnScaleFactorChanged;
		}
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x0002EA09 File Offset: 0x0002CC09
	private void OnScaleFactorChanged(float scaleFactor)
	{
		this.CounterSafetyArea();
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x0007C320 File Offset: 0x0007A520
	private void CounterSafetyArea()
	{
		if (this.rectTransform != null && this.platformSafetyAreaScaler != null && this.canvasScaler != null)
		{
			Rect safetyAreaUnits = PlatformManager.GetSafetyAreaUnits(this.canvasScaler.referenceResolution);
			this.rectTransform.offsetMin = new Vector2(this.counterLeft ? (-safetyAreaUnits.x) : 0f, this.counterBottom ? (-safetyAreaUnits.y) : 0f);
			this.rectTransform.offsetMax = new Vector2(this.counterRight ? safetyAreaUnits.x : 0f, 0f);
		}
	}

	// Token: 0x040008C3 RID: 2243
	private RectTransform rectTransform;

	// Token: 0x040008C4 RID: 2244
	private CanvasScaler canvasScaler;

	// Token: 0x040008C5 RID: 2245
	private PlatformCanvasScaler platformCanvasScaler;

	// Token: 0x040008C6 RID: 2246
	private PlatformSafetyAreaScaler platformSafetyAreaScaler;

	// Token: 0x040008C7 RID: 2247
	public bool counterBottom;

	// Token: 0x040008C8 RID: 2248
	public bool counterLeft;

	// Token: 0x040008C9 RID: 2249
	public bool counterRight;
}

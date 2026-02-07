using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200014D RID: 333
public class PlatformSafetyAreaScaler : MonoBehaviour
{
	// Token: 0x060009CC RID: 2508 RVA: 0x0007C590 File Offset: 0x0007A790
	private void Awake()
	{
		this.rectTransform = (RectTransform)base.gameObject.transform;
		this.canvasScaler = base.GetComponentInParent<CanvasScaler>();
		this.platformCanvasScaler = base.GetComponentInParent<PlatformCanvasScaler>();
		this.platformCanvasScaler.OnScaleFactorChanged += this.OnScaleFactorChanged;
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x0002EADD File Offset: 0x0002CCDD
	private void OnEnable()
	{
		base.StartCoroutine(this.RecalculateRectTransform());
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x0002EAEC File Offset: 0x0002CCEC
	private void OnDestroy()
	{
		this.platformCanvasScaler.OnScaleFactorChanged -= this.OnScaleFactorChanged;
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x0002EADD File Offset: 0x0002CCDD
	private void OnScaleFactorChanged(float scaleFactor)
	{
		base.StartCoroutine(this.RecalculateRectTransform());
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x0002EB05 File Offset: 0x0002CD05
	private IEnumerator RecalculateRectTransform()
	{
		while (!PlatformManager.IsSafeAreaInitialized)
		{
			yield return new WaitForEndOfFrame();
		}
		if (this.rectTransform && this.canvasScaler)
		{
			Rect safetyAreaUnits = PlatformManager.GetSafetyAreaUnits(this.canvasScaler.referenceResolution);
			this.rectTransform.offsetMin = new Vector2(safetyAreaUnits.x, safetyAreaUnits.y);
			this.rectTransform.offsetMax = new Vector2(-safetyAreaUnits.x, 0f);
		}
		yield break;
	}

	// Token: 0x040008D2 RID: 2258
	private RectTransform rectTransform;

	// Token: 0x040008D3 RID: 2259
	private CanvasScaler canvasScaler;

	// Token: 0x040008D4 RID: 2260
	private PlatformCanvasScaler platformCanvasScaler;
}

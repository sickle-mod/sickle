using System;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020004F9 RID: 1273
	public class PlatformScalingAdjustment : MonoBehaviour
	{
		// Token: 0x060028D8 RID: 10456 RVA: 0x00042837 File Offset: 0x00040A37
		private void Awake()
		{
			this.platformCanvasScaler = base.GetComponentInParent<PlatformCanvasScaler>();
			this.platformCanvasScaler.OnScaleFactorChanged += this.OnScaleFactorChanged;
			this.initialScale = base.transform.localScale;
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x0004286D File Offset: 0x00040A6D
		private void Start()
		{
			this.OnScaleFactorChanged(this.platformCanvasScaler.ScaleFactor);
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x00042880 File Offset: 0x00040A80
		private void OnDestroy()
		{
			this.platformCanvasScaler.OnScaleFactorChanged -= this.OnScaleFactorChanged;
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000EB884 File Offset: 0x000E9A84
		private void RecalculateScale(float scaleFactor)
		{
			float num = 1f + (scaleFactor - 1f) * this.scaleAdjustmentMultiplier;
			base.transform.localScale = this.initialScale * num;
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x00042899 File Offset: 0x00040A99
		private void OnScaleFactorChanged(float scaleFactor)
		{
			this.RecalculateScale(scaleFactor);
		}

		// Token: 0x04001D4A RID: 7498
		[SerializeField]
		private float scaleAdjustmentMultiplier;

		// Token: 0x04001D4B RID: 7499
		private PlatformCanvasScaler platformCanvasScaler;

		// Token: 0x04001D4C RID: 7500
		private Vector3 initialScale;
	}
}

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004FC RID: 1276
	public class SliderWrapper
	{
		// Token: 0x140000F7 RID: 247
		// (add) Token: 0x060028E1 RID: 10465 RVA: 0x000EB8C0 File Offset: 0x000E9AC0
		// (remove) Token: 0x060028E2 RID: 10466 RVA: 0x000EB8F8 File Offset: 0x000E9AF8
		public event Action ValueChanged = delegate
		{
		};

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x060028E3 RID: 10467 RVA: 0x000428DC File Offset: 0x00040ADC
		// (set) Token: 0x060028E4 RID: 10468 RVA: 0x000EB930 File Offset: 0x000E9B30
		public int Value
		{
			get
			{
				if (this.slider != null)
				{
					return (int)this.slider.value;
				}
				return this.value;
			}
			set
			{
				int num = Mathf.Min(value, (this.slider != null) ? ((int)this.slider.maxValue) : this.maxValue);
				if (this.slider != null)
				{
					this.slider.value = (float)num;
					this.ValueChanged();
					return;
				}
				this.value = num;
				this.ValueChanged();
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060028E5 RID: 10469 RVA: 0x000428FF File Offset: 0x00040AFF
		// (set) Token: 0x060028E6 RID: 10470 RVA: 0x00042922 File Offset: 0x00040B22
		public int MaxValue
		{
			get
			{
				if (this.slider != null)
				{
					return (int)this.slider.maxValue;
				}
				return this.maxValue;
			}
			set
			{
				if (this.slider != null)
				{
					this.slider.maxValue = (float)value;
					return;
				}
				this.maxValue = value;
			}
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x00042947 File Offset: 0x00040B47
		public SliderWrapper(Slider slider)
		{
			this.slider = slider;
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x0004297B File Offset: 0x00040B7B
		public void SetSliderInteractable(bool interactable)
		{
			if (this.slider != null)
			{
				this.slider.interactable = interactable;
			}
		}

		// Token: 0x04001D52 RID: 7506
		private Slider slider;

		// Token: 0x04001D53 RID: 7507
		private int value;

		// Token: 0x04001D54 RID: 7508
		private int maxValue;
	}
}

using System;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003E5 RID: 997
	public class ExchangeSlot
	{
		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06001D9F RID: 7583 RVA: 0x0003B31F File Offset: 0x0003951F
		// (set) Token: 0x06001DA0 RID: 7584 RVA: 0x0003B32C File Offset: 0x0003952C
		public int Value
		{
			get
			{
				return this.sliderWrapper.Value;
			}
			set
			{
				this.sliderWrapper.Value = value;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06001DA1 RID: 7585 RVA: 0x0003B33A File Offset: 0x0003953A
		// (set) Token: 0x06001DA2 RID: 7586 RVA: 0x0003B347 File Offset: 0x00039547
		public int MaxValue
		{
			get
			{
				return this.sliderWrapper.MaxValue;
			}
			set
			{
				this.sliderWrapper.MaxValue = value;
			}
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x0003B355 File Offset: 0x00039555
		public ExchangeSlot(SliderWrapper sliderWrapper)
		{
			this.sliderWrapper = sliderWrapper;
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x0003B364 File Offset: 0x00039564
		public void SetSliderValueChangedEvent(Action onValueChanged)
		{
			this.sliderWrapper.ValueChanged += onValueChanged;
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x0003B372 File Offset: 0x00039572
		public void SetSliderInteractable(bool interactable)
		{
			this.sliderWrapper.SetSliderInteractable(interactable);
		}

		// Token: 0x04001547 RID: 5447
		public Button[] buttons;

		// Token: 0x04001548 RID: 5448
		public Text textField;

		// Token: 0x04001549 RID: 5449
		public Text textUnit;

		// Token: 0x0400154A RID: 5450
		private readonly SliderWrapper sliderWrapper;
	}
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x0200081F RID: 2079
	[Serializable]
	public class TabToggleTMP : Toggle
	{
		// Token: 0x06003B06 RID: 15110 RVA: 0x0004E284 File Offset: 0x0004C484
		protected override void Start()
		{
			base.Start();
			this.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
			this._isHighlighted = base.isOn;
			this.RefreshShadowLine();
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x0004E2B5 File Offset: 0x0004C4B5
		private void OnValueChanged(bool value)
		{
			this._isHighlighted = value;
			this.RefreshShadowLine();
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x0004E2C4 File Offset: 0x0004C4C4
		private void RefreshShadowLine()
		{
			if (this.Text != null)
			{
				this.Text.color = (this._isHighlighted ? this.ColorOn : this.ColorOff);
			}
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x0004E2F5 File Offset: 0x0004C4F5
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this._isHighlighted = true;
			this.RefreshShadowLine();
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x0004E30B File Offset: 0x0004C50B
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this._isHighlighted = base.isOn;
			this.RefreshShadowLine();
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x0004E326 File Offset: 0x0004C526
		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			this._isHighlighted = true;
			this.RefreshShadowLine();
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x0004E33C File Offset: 0x0004C53C
		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			this._isHighlighted = base.isOn;
			this.RefreshShadowLine();
		}

		// Token: 0x04002CBC RID: 11452
		public Color ColorOn;

		// Token: 0x04002CBD RID: 11453
		public Color ColorOff;

		// Token: 0x04002CBE RID: 11454
		public TextMeshProUGUI Text;

		// Token: 0x04002CBF RID: 11455
		private bool _isHighlighted;
	}
}

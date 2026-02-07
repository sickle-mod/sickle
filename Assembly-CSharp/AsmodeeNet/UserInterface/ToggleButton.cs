using System;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007D8 RID: 2008
	public class ToggleButton : MonoBehaviour
	{
		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x0600397F RID: 14719 RVA: 0x0004CF11 File Offset: 0x0004B111
		// (set) Token: 0x06003980 RID: 14720 RVA: 0x0004CF19 File Offset: 0x0004B119
		public bool IsOn
		{
			get
			{
				return this._isOn;
			}
			set
			{
				this._isOn = value;
				this._UpdateUI();
			}
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x0014E40C File Offset: 0x0014C60C
		private void _UpdateUI()
		{
			if (this._ui.onLayer != null)
			{
				this._ui.onLayer.SetActive(this._isOn);
			}
			if (this._ui.offLayer != null)
			{
				this._ui.offLayer.SetActive(!this._isOn);
			}
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x0004CF28 File Offset: 0x0004B128
		private void OnEnable()
		{
			this._UpdateUI();
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x0004CF30 File Offset: 0x0004B130
		public void OnButtonClicked()
		{
			this.IsOn = !this.IsOn;
		}

		// Token: 0x04002B69 RID: 11113
		private const string _documentation = "<b>ToggleButton</b> requires a <b>Button</b> and displays <b>onLayer</b> or <b>offLayer</b> according to <b>IsOn</b> property";

		// Token: 0x04002B6A RID: 11114
		[SerializeField]
		private ToggleButton.UI _ui;

		// Token: 0x04002B6B RID: 11115
		private bool _isOn;

		// Token: 0x020007D9 RID: 2009
		[Serializable]
		public class UI
		{
			// Token: 0x04002B6C RID: 11116
			public GameObject onLayer;

			// Token: 0x04002B6D RID: 11117
			public GameObject offLayer;
		}
	}
}

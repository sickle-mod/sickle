using System;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x020007EC RID: 2028
	public class SSOLoginAndPasswordPanel : SSOBasePanel
	{
		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060039EA RID: 14826 RVA: 0x0004D3E4 File Offset: 0x0004B5E4
		// (set) Token: 0x060039EB RID: 14827 RVA: 0x0004D3F6 File Offset: 0x0004B5F6
		public string Text
		{
			get
			{
				return this._ui.inputField.text;
			}
			set
			{
				this._ui.inputField.text = value;
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060039EC RID: 14828 RVA: 0x0004D409 File Offset: 0x0004B609
		public bool AreRequirementsMet
		{
			get
			{
				return !string.IsNullOrEmpty(this.Text);
			}
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x0014F9C0 File Offset: 0x0014DBC0
		public void DisplayErrorMessage(string message)
		{
			if (string.IsNullOrEmpty(message))
			{
				this._ui.errorIndicator.gameObject.SetActive(false);
				return;
			}
			this._ui.errorIndicator.gameObject.SetActive(true);
			this._ui.errorMessage.text = message;
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x0004D419 File Offset: 0x0004B619
		private void Awake()
		{
			ModifyInputFieldOnEndEditBehaviour.ModifyBehaviour(this._ui.inputField);
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x0014FA14 File Offset: 0x0014DC14
		public void Show(bool resetLogin = true)
		{
			if (resetLogin)
			{
				this._ui.inputField.text = string.Empty;
			}
			this._ui.errorIndicator.gameObject.SetActive(false);
			if (this._ui.showPasswordToggle != null)
			{
				this._ui.showPasswordToggle.isOn = this._ui.inputField.contentType == TMP_InputField.ContentType.Password;
			}
			base.Show();
			this._UpdateUI();
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x0014FA94 File Offset: 0x0014DC94
		public void SelectInputFieldContent()
		{
			this._ui.inputField.Select();
			this._ui.inputField.ActivateInputField();
			this._ui.inputField.selectionFocusPosition = 0;
			this._ui.inputField.MoveTextEnd(false);
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x0004D42B File Offset: 0x0004B62B
		public void OnValueChanged(string value)
		{
			this._UpdateUI();
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x0004D433 File Offset: 0x0004B633
		private void _UpdateUI()
		{
			this._ui.errorIndicator.gameObject.SetActive(false);
			this._buttons[1].interactable = this.AreRequirementsMet;
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x0004D45E File Offset: 0x0004B65E
		public void ToggleInputFieldContentType()
		{
			this._ui.inputField.contentType = (this._ui.showPasswordToggle.isOn ? TMP_InputField.ContentType.Password : TMP_InputField.ContentType.Standard);
			this._ui.inputField.ForceLabelUpdate();
		}

		// Token: 0x04002BC7 RID: 11207
		[SerializeField]
		private SSOLoginAndPasswordPanel.UI _ui;

		// Token: 0x020007ED RID: 2029
		[Serializable]
		public class UI
		{
			// Token: 0x04002BC8 RID: 11208
			public TMP_InputField inputField;

			// Token: 0x04002BC9 RID: 11209
			public Toggle showPasswordToggle;

			// Token: 0x04002BCA RID: 11210
			public GameObject errorIndicator;

			// Token: 0x04002BCB RID: 11211
			public TextMeshProUGUI errorMessage;
		}
	}
}

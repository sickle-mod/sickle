using System;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x020007F7 RID: 2039
	public class SSOWelcomePanel : SSOBasePanel
	{
		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06003A42 RID: 14914 RVA: 0x0004D82F File Offset: 0x0004BA2F
		// (set) Token: 0x06003A43 RID: 14915 RVA: 0x0004D841 File Offset: 0x0004BA41
		public string Email
		{
			get
			{
				return this._ui.emailInputField.text;
			}
			set
			{
				this._ui.emailInputField.text = value;
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06003A44 RID: 14916 RVA: 0x0004D854 File Offset: 0x0004BA54
		// (set) Token: 0x06003A45 RID: 14917 RVA: 0x0004D866 File Offset: 0x0004BA66
		public bool SubscribeToNewsletter
		{
			get
			{
				return this._ui.subscribeToNewsletterButton.IsOn;
			}
			set
			{
				this._ui.subscribeToNewsletterButton.IsOn = value;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06003A46 RID: 14918 RVA: 0x0004D879 File Offset: 0x0004BA79
		public bool AreRequirementsMet
		{
			get
			{
				return EmailFormatValidator.IsValidEmail(this.Email);
			}
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x0004D886 File Offset: 0x0004BA86
		private void Awake()
		{
			ModifyInputFieldOnEndEditBehaviour.ModifyBehaviour(this._ui.emailInputField);
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x0004D898 File Offset: 0x0004BA98
		public void Show(bool resetEmail = true)
		{
			if (resetEmail)
			{
				this._ui.emailInputField.text = string.Empty;
			}
			base.Show();
			this._UpdateUI();
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x0004D8BE File Offset: 0x0004BABE
		public void OnValueChanged(string value)
		{
			this._UpdateUI();
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x0004D8C6 File Offset: 0x0004BAC6
		private void _UpdateUI()
		{
			this._buttons[1].interactable = this.AreRequirementsMet;
		}

		// Token: 0x04002BF8 RID: 11256
		[SerializeField]
		private SSOWelcomePanel.UI _ui;

		// Token: 0x020007F8 RID: 2040
		[Serializable]
		public class UI
		{
			// Token: 0x04002BF9 RID: 11257
			public TMP_InputField emailInputField;

			// Token: 0x04002BFA RID: 11258
			public ToggleButton subscribeToNewsletterButton;
		}
	}
}

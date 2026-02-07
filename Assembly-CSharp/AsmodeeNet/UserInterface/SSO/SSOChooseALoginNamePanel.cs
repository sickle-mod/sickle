using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x020007E9 RID: 2025
	public class SSOChooseALoginNamePanel : SSOBasePanel
	{
		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060039DD RID: 14813 RVA: 0x0004D2E6 File Offset: 0x0004B4E6
		// (set) Token: 0x060039DE RID: 14814 RVA: 0x0004D2F8 File Offset: 0x0004B4F8
		public string LoginName
		{
			get
			{
				return this._ui.loginNameInputField.text;
			}
			set
			{
				this._ui.loginNameInputField.text = value;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060039DF RID: 14815 RVA: 0x0004D30B File Offset: 0x0004B50B
		// (set) Token: 0x060039E0 RID: 14816 RVA: 0x0004D31D File Offset: 0x0004B51D
		public string Password
		{
			get
			{
				return this._ui.passwordInputField.text;
			}
			set
			{
				this._ui.passwordInputField.text = value;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060039E1 RID: 14817 RVA: 0x0004D330 File Offset: 0x0004B530
		public bool AreRequirementsMet
		{
			get
			{
				return this._ui.loginNameInputField.text.Length >= 4 && this._ui.passwordInputField.text.Length >= 1 && this._currentState == SSOChooseALoginNamePanel.LoginState.Available;
			}
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x0004D36D File Offset: 0x0004B56D
		private void Awake()
		{
			ModifyInputFieldOnEndEditBehaviour.ModifyBehaviour(this._ui.loginNameInputField);
			ModifyInputFieldOnEndEditBehaviour.ModifyBehaviour(this._ui.passwordInputField);
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x0014F628 File Offset: 0x0014D828
		public override void Show()
		{
			this._ui.loginNameInputField.text = string.Empty;
			this._ui.passwordInputField.text = string.Empty;
			this.ShowLoginNameState(SSOChooseALoginNamePanel.LoginState.Empty);
			this.ShowPasswordStrength(-1);
			if (this._ui.showPasswordToggle != null)
			{
				this._ui.showPasswordToggle.isOn = this._ui.passwordInputField.contentType == TMP_InputField.ContentType.Password;
			}
			base.Show();
			this._UpdateYesButtonState();
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x0004D38F File Offset: 0x0004B58F
		private void _UpdateYesButtonState()
		{
			this._buttons[1].interactable = this.AreRequirementsMet;
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x0014F6B0 File Offset: 0x0014D8B0
		public void ShowLoginNameState(SSOChooseALoginNamePanel.LoginState state)
		{
			this._currentState = state;
			switch (state)
			{
			default:
				this._ui.loginNameMessage.gameObject.SetActive(false);
				this._ui.searchingLoginNameSpinner.SetActive(false);
				this._ui.loginNameIcon.sprite = this._ui.placeholderLoginNameIcon;
				break;
			case SSOChooseALoginNamePanel.LoginState.Searching:
				this._ui.loginNameMessage.gameObject.SetActive(false);
				this._ui.searchingLoginNameSpinner.SetActive(true);
				this._ui.loginNameIcon.sprite = this._ui.placeholderLoginNameIcon;
				break;
			case SSOChooseALoginNamePanel.LoginState.Available:
				this._ui.loginNameMessage.gameObject.SetActive(true);
				this._ui.searchingLoginNameSpinner.SetActive(false);
				this._ui.loginNameIcon.sprite = this._ui.validLoginNameIcon;
				this._ui.loginNameMessage.color = this._ui.validLoginNameColor;
				this._ui.loginNameMessage.text = CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.ChooseALoginPanel.LoginNameAvailable");
				break;
			case SSOChooseALoginNamePanel.LoginState.Unavailable:
				this._ui.loginNameMessage.gameObject.SetActive(true);
				this._ui.searchingLoginNameSpinner.SetActive(false);
				this._ui.loginNameIcon.sprite = this._ui.invalidLoginNameIcon;
				this._ui.loginNameMessage.color = this._ui.invalidLoginNameColor;
				this._ui.loginNameMessage.text = CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.ChooseALoginPanel.LoginNameUnAvailable");
				break;
			}
			this._UpdateYesButtonState();
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x0014F878 File Offset: 0x0014DA78
		public void ShowPasswordStrength(int strength)
		{
			if (strength < 0 || strength > 4)
			{
				this._ui.passwordIcon.sprite = this._ui.placeholderPasswordIcon;
				this._ui.passwordMessage.gameObject.SetActive(false);
			}
			else
			{
				if (strength == 0 || strength == 1)
				{
					this._ui.passwordIcon.sprite = this._ui.weakPasswordIcon;
					this._ui.passwordMessage.color = this._ui.weakPasswordColor;
				}
				else if (strength == 2)
				{
					this._ui.passwordIcon.sprite = this._ui.mediumPasswordIcon;
					this._ui.passwordMessage.color = this._ui.mediumPasswordColor;
				}
				else
				{
					this._ui.passwordIcon.sprite = this._ui.strongPasswordIcon;
					this._ui.passwordMessage.color = this._ui.strongPasswordColor;
				}
				this._ui.passwordMessage.text = CoreApplication.Instance.LocalizationManager.GetLocalizedText(string.Format("SSO.ChooseALoginPanel.PasswordLevel{0}", strength));
				this._ui.passwordMessage.gameObject.SetActive(true);
			}
			this._UpdateYesButtonState();
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x0004D3A4 File Offset: 0x0004B5A4
		public void ToggleInputFieldContentType()
		{
			this._ui.passwordInputField.contentType = (this._ui.showPasswordToggle.isOn ? TMP_InputField.ContentType.Password : TMP_InputField.ContentType.Standard);
			this._ui.passwordInputField.ForceLabelUpdate();
		}

		// Token: 0x04002BAC RID: 11180
		[SerializeField]
		private SSOChooseALoginNamePanel.UI _ui;

		// Token: 0x04002BAD RID: 11181
		private SSOChooseALoginNamePanel.LoginState _currentState;

		// Token: 0x020007EA RID: 2026
		[Serializable]
		public class UI
		{
			// Token: 0x04002BAE RID: 11182
			public TMP_InputField loginNameInputField;

			// Token: 0x04002BAF RID: 11183
			public Image loginNameIcon;

			// Token: 0x04002BB0 RID: 11184
			public Sprite placeholderLoginNameIcon;

			// Token: 0x04002BB1 RID: 11185
			public Sprite validLoginNameIcon;

			// Token: 0x04002BB2 RID: 11186
			public Color validLoginNameColor;

			// Token: 0x04002BB3 RID: 11187
			public Sprite invalidLoginNameIcon;

			// Token: 0x04002BB4 RID: 11188
			public Color invalidLoginNameColor;

			// Token: 0x04002BB5 RID: 11189
			public GameObject searchingLoginNameSpinner;

			// Token: 0x04002BB6 RID: 11190
			public TextMeshProUGUI loginNameMessage;

			// Token: 0x04002BB7 RID: 11191
			public TMP_InputField passwordInputField;

			// Token: 0x04002BB8 RID: 11192
			public Toggle showPasswordToggle;

			// Token: 0x04002BB9 RID: 11193
			public Image passwordIcon;

			// Token: 0x04002BBA RID: 11194
			public Sprite placeholderPasswordIcon;

			// Token: 0x04002BBB RID: 11195
			public Sprite strongPasswordIcon;

			// Token: 0x04002BBC RID: 11196
			public Color strongPasswordColor;

			// Token: 0x04002BBD RID: 11197
			public Sprite mediumPasswordIcon;

			// Token: 0x04002BBE RID: 11198
			public Color mediumPasswordColor;

			// Token: 0x04002BBF RID: 11199
			public Sprite weakPasswordIcon;

			// Token: 0x04002BC0 RID: 11200
			public Color weakPasswordColor;

			// Token: 0x04002BC1 RID: 11201
			public TextMeshProUGUI passwordMessage;
		}

		// Token: 0x020007EB RID: 2027
		public enum LoginState
		{
			// Token: 0x04002BC3 RID: 11203
			Empty,
			// Token: 0x04002BC4 RID: 11204
			Searching,
			// Token: 0x04002BC5 RID: 11205
			Available,
			// Token: 0x04002BC6 RID: 11206
			Unavailable
		}
	}
}

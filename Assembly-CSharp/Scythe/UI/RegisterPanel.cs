using System;
using I2.Loc;
using Scythe.Analytics;
using Scythe.Multiplayer;
using Scythe.Multiplayer.AuthApi;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Scythe.UI
{
	// Token: 0x020004ED RID: 1261
	public class RegisterPanel : MonoBehaviour
	{
		// Token: 0x0600287B RID: 10363 RVA: 0x000EA960 File Offset: 0x000E8B60
		private void OnEnable()
		{
			this.errorText.text = "";
			this.registerButton.onClick.AddListener(new UnityAction(this.RegisterAccount));
			this.exitButton.onClick.AddListener(new UnityAction(this.ExitWindow));
			this.supportButton.onClick.AddListener(new UnityAction(this.Support));
			this.privacyPolicyToggle.isOn = false;
			this.loginInputField.Clear();
			this.passwordInputField.Clear();
			this.confirmPasswordInputField.Clear();
			this.emailInputField.Clear();
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x000EAA0C File Offset: 0x000E8C0C
		private void OnDisable()
		{
			this.registerButton.onClick.RemoveListener(new UnityAction(this.RegisterAccount));
			this.exitButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			this.supportButton.onClick.RemoveListener(new UnityAction(this.Support));
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x000423BF File Offset: 0x000405BF
		public void RegisterAccountError(string message)
		{
			this.errorText.text = message;
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x00029172 File Offset: 0x00027372
		public void RegisterAccountSuccess()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x00040E8D File Offset: 0x0003F08D
		private void ExitWindow()
		{
			base.gameObject.SetActive(false);
			SingletonMono<MainMenu>.Instance.ShowDefaultMultiplayerMenu();
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x000EAA70 File Offset: 0x000E8C70
		private void RegisterAccount()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			if (string.IsNullOrEmpty(this.loginInputField.text) || string.IsNullOrEmpty(this.passwordInputField.text) || string.IsNullOrEmpty(this.confirmPasswordInputField.text) || string.IsNullOrEmpty(this.emailInputField.text))
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/FillFields");
				return;
			}
			if (this.passwordInputField.text.Length < 5 || this.confirmPasswordInputField.text.Length < 5)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/PasswordRequirements");
				return;
			}
			if (this.passwordInputField.text != this.confirmPasswordInputField.text)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/PasswordMatch");
				return;
			}
			if (this.emailInputField.text.IndexOf('@') <= 0)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/IncorrectEmail");
				return;
			}
			if (!this.privacyPolicyToggle.isOn)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/AcceptPrivacyPolicy");
				return;
			}
			this.errorText.text = "";
			Singleton<LoginController>.Instance.Register(this.loginInputField.text, this.passwordInputField.text, this.emailInputField.text);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_register_button);
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000423CD File Offset: 0x000405CD
		public void RegisterAccountFailure(BasicApiResultDto result)
		{
			this.errorText.text = ScriptLocalization.Get("MainMenu/WrongData");
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x00040EC1 File Offset: 0x0003F0C1
		private void Support()
		{
			SingletonMono<MainMenu>.Instance.SupportPanelOpen();
		}

		// Token: 0x04001CF9 RID: 7417
		[SerializeField]
		private TMP_InputField loginInputField;

		// Token: 0x04001CFA RID: 7418
		[SerializeField]
		private TMP_InputField passwordInputField;

		// Token: 0x04001CFB RID: 7419
		[SerializeField]
		private TMP_InputField confirmPasswordInputField;

		// Token: 0x04001CFC RID: 7420
		[SerializeField]
		private TMP_InputField emailInputField;

		// Token: 0x04001CFD RID: 7421
		[SerializeField]
		private Button registerButton;

		// Token: 0x04001CFE RID: 7422
		[SerializeField]
		private Toggle privacyPolicyToggle;

		// Token: 0x04001CFF RID: 7423
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001D00 RID: 7424
		[SerializeField]
		private Button supportButton;

		// Token: 0x04001D01 RID: 7425
		[SerializeField]
		private TMP_Text errorText;

		// Token: 0x04001D02 RID: 7426
		private const string LOC_WRONG_DATA = "MainMenu/WrongData";

		// Token: 0x04001D03 RID: 7427
		private const string LOC_FILL_FIELDS = "MainMenu/FillFields";

		// Token: 0x04001D04 RID: 7428
		private const string LOC_PASSWORD_REQUIREMENTS = "MainMenu/PasswordRequirements";

		// Token: 0x04001D05 RID: 7429
		private const string LOC_ACCEPT_PRIVACY = "MainMenu/AcceptPrivacyPolicy";

		// Token: 0x04001D06 RID: 7430
		private const string LOC_PASSWORD_MATCH = "MainMenu/PasswordMatch";

		// Token: 0x04001D07 RID: 7431
		private const string LOC_INCORRECT_MAIL = "MainMenu/IncorrectEmail";
	}
}

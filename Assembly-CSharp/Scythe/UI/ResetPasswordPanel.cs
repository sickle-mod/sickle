using System;
using I2.Loc;
using Multiplayer.AuthApi;
using Scythe.Analytics;
using Scythe.Multiplayer;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Scythe.UI
{
	// Token: 0x020004EE RID: 1262
	public class ResetPasswordPanel : MonoBehaviour
	{
		// Token: 0x06002884 RID: 10372 RVA: 0x000EABE8 File Offset: 0x000E8DE8
		private void OnEnable()
		{
			this.resetPassword.onClick.AddListener(new UnityAction(this.ResetPassword));
			this.exitButton.onClick.AddListener(new UnityAction(this.ExitWindow));
			this.supportButton.onClick.AddListener(new UnityAction(this.Support));
			this.loginOrEmailInputField.Clear();
			this.newPasswordInputField.Clear();
			this.confirmPasswordInputField.Clear();
			Singleton<LoginController>.Instance.OnPasswordResetFailure += this.ResetPasswordFailure;
			this.errorText.text = "";
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x000EAC90 File Offset: 0x000E8E90
		private void OnDisable()
		{
			this.resetPassword.onClick.RemoveListener(new UnityAction(this.ResetPassword));
			this.exitButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			this.supportButton.onClick.RemoveListener(new UnityAction(this.Support));
			Singleton<LoginController>.Instance.OnPasswordResetFailure -= this.ResetPasswordFailure;
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000423E4 File Offset: 0x000405E4
		public void ResetPasswordError(string message)
		{
			this.errorText.text = message;
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x00029172 File Offset: 0x00027372
		public void ResetPasswordSuccess()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x000423F2 File Offset: 0x000405F2
		private void ExitWindow()
		{
			base.gameObject.SetActive(false);
			SingletonMono<MainMenu>.Instance.EnableLoginPanel(true);
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x000EAD08 File Offset: 0x000E8F08
		private void ResetPassword()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			if (string.IsNullOrEmpty(this.loginOrEmailInputField.text) || string.IsNullOrEmpty(this.newPasswordInputField.text) || string.IsNullOrEmpty(this.confirmPasswordInputField.text))
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/EmptyEmail");
				return;
			}
			if (this.newPasswordInputField.text.Length < 5 || this.confirmPasswordInputField.text.Length < 5)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/PasswordRequirements");
				return;
			}
			if (this.newPasswordInputField.text != this.confirmPasswordInputField.text)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/PasswordMatch");
				return;
			}
			this.errorText.text = "";
			Singleton<LoginController>.Instance.ResetPassword(this.loginOrEmailInputField.text, this.newPasswordInputField.text);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_reset_password_button);
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x0004240B File Offset: 0x0004060B
		private void ResetPasswordFailure(FailureResponse result)
		{
			this.errorText.text = result.GetErrorsString();
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x00040EC1 File Offset: 0x0003F0C1
		private void Support()
		{
			SingletonMono<MainMenu>.Instance.SupportPanelOpen();
		}

		// Token: 0x04001D08 RID: 7432
		[SerializeField]
		private TMP_InputField loginOrEmailInputField;

		// Token: 0x04001D09 RID: 7433
		[SerializeField]
		private TMP_InputField newPasswordInputField;

		// Token: 0x04001D0A RID: 7434
		[SerializeField]
		private TMP_InputField confirmPasswordInputField;

		// Token: 0x04001D0B RID: 7435
		[SerializeField]
		private Button resetPassword;

		// Token: 0x04001D0C RID: 7436
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001D0D RID: 7437
		[SerializeField]
		private Button supportButton;

		// Token: 0x04001D0E RID: 7438
		[SerializeField]
		private TMP_Text errorText;

		// Token: 0x04001D0F RID: 7439
		private const string EmptyEmail = "MainMenu/EmptyEmail";

		// Token: 0x04001D10 RID: 7440
		private const string PasswordRequirements = "MainMenu/PasswordRequirements";

		// Token: 0x04001D11 RID: 7441
		private const string PasswordMatch = "MainMenu/PasswordMatch";
	}
}

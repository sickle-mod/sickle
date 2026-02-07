using System;
using I2.Loc;
using Multiplayer.AuthApi;
using Scythe.Multiplayer;
using Scythe.Multiplayer.AuthApi.Models;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004EF RID: 1263
	public class ResetPasswordSuccess : MonoBehaviour
	{
		// Token: 0x0600288D RID: 10381 RVA: 0x000EAE14 File Offset: 0x000E9014
		private void OnEnable()
		{
			this.errorText.text = "";
			this.okButton.onClick.AddListener(new UnityAction(this.ExitWindow));
			this.resendEmailButton.onClick.AddListener(new UnityAction(this.ResendEmail));
			this.supportButton.onClick.AddListener(new UnityAction(this.Support));
			if (PlatformManager.IsStandalone)
			{
				this.exitButton.onClick.AddListener(new UnityAction(this.ExitWindow));
			}
			Singleton<LoginController>.Instance.OnResendEmailResetPasswordSuccess += this.LoginController_OnResendEmailChangeSuccess;
			Singleton<LoginController>.Instance.OnResendEmailResetPasswordFailure += this.LoginController_OnResendEmailChangeFailure;
			Singleton<LoginController>.Instance.OnResendEmailResetPasswordError += this.LoginController_OnResendEmailChangeError;
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x000EAEEC File Offset: 0x000E90EC
		private void OnDisable()
		{
			this.okButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			this.resendEmailButton.onClick.RemoveListener(new UnityAction(this.ResendEmail));
			this.supportButton.onClick.RemoveListener(new UnityAction(this.Support));
			if (PlatformManager.IsStandalone)
			{
				this.exitButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			}
			Singleton<LoginController>.Instance.OnResendEmailResetPasswordSuccess -= this.LoginController_OnResendEmailChangeSuccess;
			Singleton<LoginController>.Instance.OnResendEmailResetPasswordFailure -= this.LoginController_OnResendEmailChangeFailure;
			Singleton<LoginController>.Instance.OnResendEmailResetPasswordError -= this.LoginController_OnResendEmailChangeError;
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x0004241E File Offset: 0x0004061E
		public void EmailChangeFailure(string result)
		{
			this.errorText.text = result;
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x00040E6A File Offset: 0x0003F06A
		private void LoginController_OnResendEmailChangeSuccess(ResendPasswordResetEmailResponse basicApiResultDto)
		{
			Debug.Log("Email sent");
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x0004242C File Offset: 0x0004062C
		private void LoginController_OnResendEmailChangeFailure(FailureResponse basicApiResultDto)
		{
			this.errorText.text = ScriptLocalization.Get("MainMenu/OperationFail");
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x0004242C File Offset: 0x0004062C
		private void LoginController_OnResendEmailChangeError(string result)
		{
			this.errorText.text = ScriptLocalization.Get("MainMenu/OperationFail");
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x00040E8D File Offset: 0x0003F08D
		private void ExitWindow()
		{
			base.gameObject.SetActive(false);
			SingletonMono<MainMenu>.Instance.ShowDefaultMultiplayerMenu();
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x00042443 File Offset: 0x00040643
		private void ResendEmail()
		{
			this.errorText.text = "";
			Singleton<LoginController>.Instance.ResendEmailResetPassword();
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x00040EC1 File Offset: 0x0003F0C1
		private void Support()
		{
			SingletonMono<MainMenu>.Instance.SupportPanelOpen();
		}

		// Token: 0x04001D12 RID: 7442
		[SerializeField]
		private Button okButton;

		// Token: 0x04001D13 RID: 7443
		[SerializeField]
		private Button resendEmailButton;

		// Token: 0x04001D14 RID: 7444
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001D15 RID: 7445
		[SerializeField]
		private TMP_Text errorText;

		// Token: 0x04001D16 RID: 7446
		[SerializeField]
		private Button supportButton;
	}
}

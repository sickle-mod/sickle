using System;
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
	// Token: 0x020004DD RID: 1245
	public class EmailChangedPanel : MonoBehaviour
	{
		// Token: 0x06002795 RID: 10133 RVA: 0x000E8D44 File Offset: 0x000E6F44
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
			Singleton<LoginController>.Instance.OnResendEmailChangeSuccess += this.LoginController_OnResendEmailChangeSuccess;
			Singleton<LoginController>.Instance.OnResendEmailChangeFailure += this.LoginController_OnResendEmailChangeFailure;
			Singleton<LoginController>.Instance.OnResendEmailChangeError += this.LoginController_OnResendEmailChangeError;
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000E8E1C File Offset: 0x000E701C
		private void OnDisable()
		{
			this.okButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			this.resendEmailButton.onClick.RemoveListener(new UnityAction(this.ResendEmail));
			this.supportButton.onClick.RemoveListener(new UnityAction(this.Support));
			this.resendEmailButton.onClick.RemoveListener(new UnityAction(this.ResendEmail));
			if (PlatformManager.IsStandalone)
			{
				this.exitButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			}
			Singleton<LoginController>.Instance.OnResendEmailChangeSuccess -= this.LoginController_OnResendEmailChangeSuccess;
			Singleton<LoginController>.Instance.OnResendEmailChangeFailure -= this.LoginController_OnResendEmailChangeFailure;
			Singleton<LoginController>.Instance.OnResendEmailChangeError -= this.LoginController_OnResendEmailChangeError;
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000413D4 File Offset: 0x0003F5D4
		public void EmailChangeFailure(string result)
		{
			this.errorText.text = result;
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x00040E6A File Offset: 0x0003F06A
		private void LoginController_OnResendEmailChangeSuccess(ResendEmailChangeEmailResponse basicApiResultDto)
		{
			Debug.Log("Email sent");
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x000413E2 File Offset: 0x0003F5E2
		private void LoginController_OnResendEmailChangeFailure(FailureResponse response)
		{
			this.errorText.text = response.GetErrorsString();
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x000413D4 File Offset: 0x0003F5D4
		private void LoginController_OnResendEmailChangeError(string result)
		{
			this.errorText.text = result;
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x00029172 File Offset: 0x00027372
		private void ExitWindow()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000413F5 File Offset: 0x0003F5F5
		private void ResendEmail()
		{
			this.errorText.text = "";
			Singleton<LoginController>.Instance.ResendEmailChangeEmail();
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x00040EC1 File Offset: 0x0003F0C1
		private void Support()
		{
			SingletonMono<MainMenu>.Instance.SupportPanelOpen();
		}

		// Token: 0x04001C6A RID: 7274
		[SerializeField]
		private Button okButton;

		// Token: 0x04001C6B RID: 7275
		[SerializeField]
		private Button resendEmailButton;

		// Token: 0x04001C6C RID: 7276
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001C6D RID: 7277
		[SerializeField]
		private TMP_Text errorText;

		// Token: 0x04001C6E RID: 7278
		[SerializeField]
		private Button supportButton;
	}
}

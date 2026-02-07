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
	// Token: 0x020004C6 RID: 1222
	public class AccountCreatedSuccess : MonoBehaviour
	{
		// Token: 0x060026F8 RID: 9976 RVA: 0x000E6BF0 File Offset: 0x000E4DF0
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
			Singleton<LoginController>.Instance.OnResendEmailRegisterAccountSuccess += this.LoginController_OnResendEmailRegisterAcccountSuccess;
			Singleton<LoginController>.Instance.OnResendEmailRegisterAccountFailure += this.LoginController_OnResendEmailRegisterAcccountFailure;
			Singleton<LoginController>.Instance.OnResendEmailRegisterAccountError += this.LoginController_OnResendEmailRegisterAcccountError;
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x000E6CC8 File Offset: 0x000E4EC8
		private void OnDisable()
		{
			this.okButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			this.resendEmailButton.onClick.RemoveListener(new UnityAction(this.ResendEmail));
			this.supportButton.onClick.RemoveListener(new UnityAction(this.Support));
			if (PlatformManager.IsStandalone)
			{
				this.exitButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			}
			Singleton<LoginController>.Instance.OnResendEmailRegisterAccountSuccess -= this.LoginController_OnResendEmailRegisterAcccountSuccess;
			Singleton<LoginController>.Instance.OnResendEmailRegisterAccountFailure -= this.LoginController_OnResendEmailRegisterAcccountFailure;
			Singleton<LoginController>.Instance.OnResendEmailRegisterAccountError -= this.LoginController_OnResendEmailRegisterAcccountError;
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x00040E5C File Offset: 0x0003F05C
		public void EmailChangeFailure(string result)
		{
			this.errorText.text = result;
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x00040E6A File Offset: 0x0003F06A
		private void LoginController_OnResendEmailRegisterAcccountSuccess(ResendActivationEmailResponse basicApiResultDto)
		{
			Debug.Log("Email sent");
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x00040E76 File Offset: 0x0003F076
		private void LoginController_OnResendEmailRegisterAcccountFailure(FailureResponse failureResponse)
		{
			this.errorText.text = ScriptLocalization.Get("MainMenu/OperationFail");
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x00040E76 File Offset: 0x0003F076
		private void LoginController_OnResendEmailRegisterAcccountError(string result)
		{
			this.errorText.text = ScriptLocalization.Get("MainMenu/OperationFail");
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x00040E8D File Offset: 0x0003F08D
		private void ExitWindow()
		{
			base.gameObject.SetActive(false);
			SingletonMono<MainMenu>.Instance.ShowDefaultMultiplayerMenu();
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x00040EA5 File Offset: 0x0003F0A5
		private void ResendEmail()
		{
			this.errorText.text = "";
			Singleton<LoginController>.Instance.ResendActivationEmail();
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x00040EC1 File Offset: 0x0003F0C1
		private void Support()
		{
			SingletonMono<MainMenu>.Instance.SupportPanelOpen();
		}

		// Token: 0x04001BD9 RID: 7129
		[SerializeField]
		private Button okButton;

		// Token: 0x04001BDA RID: 7130
		[SerializeField]
		private Button resendEmailButton;

		// Token: 0x04001BDB RID: 7131
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001BDC RID: 7132
		[SerializeField]
		private TMP_Text errorText;

		// Token: 0x04001BDD RID: 7133
		[SerializeField]
		private Button supportButton;

		// Token: 0x04001BDE RID: 7134
		private const string LOC_OPERATION_FAIL = "MainMenu/OperationFail";

		// Token: 0x04001BDF RID: 7135
		private const string LOC_EMIAL_SENT = "Email sent";
	}
}

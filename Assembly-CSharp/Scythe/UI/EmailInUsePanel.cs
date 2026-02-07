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
	// Token: 0x020004DE RID: 1246
	public class EmailInUsePanel : MonoBehaviour
	{
		// Token: 0x0600279F RID: 10143 RVA: 0x000E8F00 File Offset: 0x000E7100
		private void OnEnable()
		{
			this.errorText.text = "";
			this.resendEmailButton.onClick.AddListener(new UnityAction(this.ResendEmail));
			this.supportButton.onClick.AddListener(new UnityAction(this.Support));
			this.backToLoginButton.onClick.AddListener(new UnityAction(this.ExitWindow));
			if (PlatformManager.IsStandalone)
			{
				this.exitButton.onClick.AddListener(new UnityAction(this.ExitWindow));
			}
			Singleton<LoginController>.Instance.OnResendEmailLoginReminderSuccess += this.LoginController_OnResendEmailLoginReminderSuccess;
			Singleton<LoginController>.Instance.OnResendEmailLoginReminderFailure += this.LoginController_OnResendEmailLoginReminderFailure;
			Singleton<LoginController>.Instance.OnResendEmailRegisterAccountError += this.LoginController_OnResendEmailLoginReminderError;
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x000E8FD8 File Offset: 0x000E71D8
		private void OnDisable()
		{
			this.resendEmailButton.onClick.RemoveListener(new UnityAction(this.ResendEmail));
			this.supportButton.onClick.RemoveListener(new UnityAction(this.Support));
			this.backToLoginButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			if (PlatformManager.IsStandalone)
			{
				this.exitButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			}
			Singleton<LoginController>.Instance.OnResendEmailLoginReminderSuccess -= this.LoginController_OnResendEmailLoginReminderSuccess;
			Singleton<LoginController>.Instance.OnResendEmailLoginReminderFailure -= this.LoginController_OnResendEmailLoginReminderFailure;
			Singleton<LoginController>.Instance.OnResendEmailLoginReminderError -= this.LoginController_OnResendEmailLoginReminderError;
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x00040E6A File Offset: 0x0003F06A
		private void LoginController_OnResendEmailLoginReminderSuccess(ResendLoginReminderEmailResponse basicApiResultDto)
		{
			Debug.Log("Email sent");
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x00041411 File Offset: 0x0003F611
		private void LoginController_OnResendEmailLoginReminderFailure(FailureResponse failureResponse)
		{
			this.errorText.text = ScriptLocalization.Get("MainMenu/OperationFail");
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x00041411 File Offset: 0x0003F611
		private void LoginController_OnResendEmailLoginReminderError(string result)
		{
			this.errorText.text = ScriptLocalization.Get("MainMenu/OperationFail");
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x00040E8D File Offset: 0x0003F08D
		private void ExitWindow()
		{
			base.gameObject.SetActive(false);
			SingletonMono<MainMenu>.Instance.ShowDefaultMultiplayerMenu();
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x00041428 File Offset: 0x0003F628
		private void ResendEmail()
		{
			this.errorText.text = "";
			Singleton<LoginController>.Instance.ResendEmailLoginReminder();
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x00040EC1 File Offset: 0x0003F0C1
		private void Support()
		{
			SingletonMono<MainMenu>.Instance.SupportPanelOpen();
		}

		// Token: 0x04001C6F RID: 7279
		[SerializeField]
		private Button backToLoginButton;

		// Token: 0x04001C70 RID: 7280
		[SerializeField]
		private Button resendEmailButton;

		// Token: 0x04001C71 RID: 7281
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001C72 RID: 7282
		[SerializeField]
		private TMP_Text errorText;

		// Token: 0x04001C73 RID: 7283
		[SerializeField]
		private Button supportButton;

		// Token: 0x04001C74 RID: 7284
		private const string LOC_OPERATION_FAIL = "MainMenu/OperationFail";

		// Token: 0x04001C75 RID: 7285
		private const string LOC_EMIAL_SENT = "Email sent";
	}
}

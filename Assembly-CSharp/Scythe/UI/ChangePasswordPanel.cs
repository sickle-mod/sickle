using System;
using I2.Loc;
using Multiplayer.AuthApi;
using Scythe.Multiplayer;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Scythe.UI
{
	// Token: 0x020004D9 RID: 1241
	public class ChangePasswordPanel : MonoBehaviour
	{
		// Token: 0x06002777 RID: 10103 RVA: 0x000E86B4 File Offset: 0x000E68B4
		private void OnEnable()
		{
			this.changePassword.onClick.AddListener(new UnityAction(this.ChangePassword));
			this.exitButton.onClick.AddListener(new UnityAction(this.ExitWindow));
			this.supportButton.onClick.AddListener(new UnityAction(this.Support));
			this.oldPasswordInputField.Clear();
			this.newPasswordInputField.Clear();
			this.confirmPasswordInputField.Clear();
			Singleton<LoginController>.Instance.OnPasswordChangeFailure += this.ChangePasswordFailure;
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000E874C File Offset: 0x000E694C
		private void OnDisable()
		{
			this.changePassword.onClick.RemoveListener(new UnityAction(this.ChangePassword));
			this.exitButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			this.supportButton.onClick.RemoveListener(new UnityAction(this.Support));
			Singleton<LoginController>.Instance.OnPasswordChangeFailure -= this.ChangePasswordFailure;
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x00041375 File Offset: 0x0003F575
		public void ChangePasswordError(string message)
		{
			this.errorText.text = message;
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x00029172 File Offset: 0x00027372
		public void ChangePasswordSuccess()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x00029172 File Offset: 0x00027372
		private void ExitWindow()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x000E87C4 File Offset: 0x000E69C4
		private void ChangePassword()
		{
			if (this.oldPasswordInputField.text.Length < 5 || this.newPasswordInputField.text.Length < 5 || this.confirmPasswordInputField.text.Length < 5)
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
			Singleton<LoginController>.Instance.ChangePassword(this.oldPasswordInputField.text, this.newPasswordInputField.text);
			Debug.LogError("ShowAuthenticatingWindow");
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x00040EC1 File Offset: 0x0003F0C1
		private void Support()
		{
			SingletonMono<MainMenu>.Instance.SupportPanelOpen();
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x00041383 File Offset: 0x0003F583
		public void ChangePasswordFailure(FailureResponse result)
		{
			this.errorText.text = result.GetErrorsString();
		}

		// Token: 0x04001C53 RID: 7251
		[SerializeField]
		private TMP_InputField oldPasswordInputField;

		// Token: 0x04001C54 RID: 7252
		[SerializeField]
		private TMP_InputField newPasswordInputField;

		// Token: 0x04001C55 RID: 7253
		[SerializeField]
		private TMP_InputField confirmPasswordInputField;

		// Token: 0x04001C56 RID: 7254
		[SerializeField]
		private Button changePassword;

		// Token: 0x04001C57 RID: 7255
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001C58 RID: 7256
		[SerializeField]
		private Button supportButton;

		// Token: 0x04001C59 RID: 7257
		[SerializeField]
		private TMP_Text errorText;
	}
}

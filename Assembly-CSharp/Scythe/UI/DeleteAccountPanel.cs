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
	// Token: 0x020004DB RID: 1243
	public class DeleteAccountPanel : MonoBehaviour
	{
		// Token: 0x06002785 RID: 10117 RVA: 0x000E8960 File Offset: 0x000E6B60
		private void OnEnable()
		{
			this.errorText.text = "";
			this.cancelButton.onClick.AddListener(new UnityAction(this.CancelButton));
			this.deleteAccount.onClick.AddListener(new UnityAction(this.DeleteAccount));
			if (PlatformManager.IsStandalone)
			{
				this.exitButton.onClick.AddListener(new UnityAction(this.ExitWindow));
			}
			Singleton<LoginController>.Instance.OnDeleteAccountFailure += this.DeleteAccountFailure;
			this.passwordInputField.Clear();
			this.supportButton.onClick.AddListener(new UnityAction(this.Support));
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000E8A18 File Offset: 0x000E6C18
		private void OnDisable()
		{
			this.cancelButton.onClick.RemoveListener(new UnityAction(this.CancelButton));
			this.deleteAccount.onClick.RemoveListener(new UnityAction(this.DeleteAccount));
			if (PlatformManager.IsStandalone)
			{
				this.exitButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			}
			Singleton<LoginController>.Instance.OnDeleteAccountFailure -= this.DeleteAccountFailure;
			this.supportButton.onClick.RemoveListener(new UnityAction(this.Support));
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x00029172 File Offset: 0x00027372
		private void ExitWindow()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x00029172 File Offset: 0x00027372
		private void CancelButton()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000E8AB4 File Offset: 0x000E6CB4
		private void DeleteAccount()
		{
			if (this.passwordInputField.text.Length < 5)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/PasswordRequirements");
				return;
			}
			if (string.IsNullOrEmpty(this.passwordInputField.text))
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/EmptyEmail");
				return;
			}
			Singleton<LoginController>.Instance.DeleteAccount(this.passwordInputField.text);
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x00040EC1 File Offset: 0x0003F0C1
		private void Support()
		{
			SingletonMono<MainMenu>.Instance.SupportPanelOpen();
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x00041396 File Offset: 0x0003F596
		private void DeleteAccountFailure(FailureResponse result)
		{
			this.errorText.text = result.GetErrorsString();
		}

		// Token: 0x04001C5D RID: 7261
		[SerializeField]
		private TMP_InputField passwordInputField;

		// Token: 0x04001C5E RID: 7262
		[SerializeField]
		private Button cancelButton;

		// Token: 0x04001C5F RID: 7263
		[SerializeField]
		private Button deleteAccount;

		// Token: 0x04001C60 RID: 7264
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001C61 RID: 7265
		[SerializeField]
		private TMP_Text errorText;

		// Token: 0x04001C62 RID: 7266
		[SerializeField]
		private Button supportButton;
	}
}

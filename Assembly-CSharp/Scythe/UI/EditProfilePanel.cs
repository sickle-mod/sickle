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
	// Token: 0x020004DC RID: 1244
	public class EditProfilePanel : MonoBehaviour
	{
		// Token: 0x0600278D RID: 10125 RVA: 0x000E8B28 File Offset: 0x000E6D28
		private void OnEnable()
		{
			this.errorText.text = "";
			this.saveChanges.onClick.AddListener(new UnityAction(this.SaveChanges));
			this.deleteAccount.onClick.AddListener(new UnityAction(this.DeleteAccount));
			this.exitButton.onClick.AddListener(new UnityAction(this.ExitWindow));
			this.supportButton.onClick.AddListener(new UnityAction(this.Support));
			Singleton<LoginController>.Instance.OnEmailChangeFailure += this.EmailChangeFailure;
			this.changeEmailInputField.Clear();
			this.enterPasswordInputField.Clear();
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x000E8BE4 File Offset: 0x000E6DE4
		private void OnDisable()
		{
			this.saveChanges.onClick.RemoveListener(new UnityAction(this.SaveChanges));
			this.deleteAccount.onClick.RemoveListener(new UnityAction(this.DeleteAccount));
			this.exitButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			this.supportButton.onClick.RemoveListener(new UnityAction(this.Support));
			Singleton<LoginController>.Instance.OnEmailChangeFailure -= this.EmailChangeFailure;
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x000413A9 File Offset: 0x0003F5A9
		public void EmailChangeFailure(FailureResponse result)
		{
			this.errorText.text = result.GetErrorsString();
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x00029172 File Offset: 0x00027372
		private void ExitWindow()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000E8C78 File Offset: 0x000E6E78
		private void SaveChanges()
		{
			if (string.IsNullOrEmpty(this.changeEmailInputField.text) || string.IsNullOrEmpty(this.enterPasswordInputField.text))
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/EmptyEmail");
				return;
			}
			if (this.changeEmailInputField.text.IndexOf('@') <= 0)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/IncorrectEmail");
				return;
			}
			if (this.enterPasswordInputField.text.Length < 5)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/PasswordRequirements");
				return;
			}
			this.errorText.text = "";
			Singleton<LoginController>.Instance.EmailChange(this.changeEmailInputField.text, this.enterPasswordInputField.text);
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x000413BC File Offset: 0x0003F5BC
		private void DeleteAccount()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			SingletonMono<MainMenu>.Instance.ShowDeleteAccountPanel();
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x00040EC1 File Offset: 0x0003F0C1
		private void Support()
		{
			SingletonMono<MainMenu>.Instance.SupportPanelOpen();
		}

		// Token: 0x04001C63 RID: 7267
		[SerializeField]
		private TMP_InputField changeEmailInputField;

		// Token: 0x04001C64 RID: 7268
		[SerializeField]
		private TMP_InputField enterPasswordInputField;

		// Token: 0x04001C65 RID: 7269
		[SerializeField]
		private Button saveChanges;

		// Token: 0x04001C66 RID: 7270
		[SerializeField]
		private Button deleteAccount;

		// Token: 0x04001C67 RID: 7271
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001C68 RID: 7272
		[SerializeField]
		private TMP_Text errorText;

		// Token: 0x04001C69 RID: 7273
		[SerializeField]
		private Button supportButton;
	}
}

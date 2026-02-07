using System;
using I2.Loc;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004C8 RID: 1224
	public class AccountMigrationPanel : MonoBehaviour
	{
		// Token: 0x06002707 RID: 9991 RVA: 0x000E6E80 File Offset: 0x000E5080
		private void OnEnable()
		{
			this.errorText.text = "";
			this.registerButton.onClick.AddListener(new UnityAction(this.RegisterLegacyPlayer));
			this.exitButton.onClick.AddListener(new UnityAction(this.ExitWindow));
			this.supportButton.onClick.AddListener(new UnityAction(this.Support));
			this.loginText.text = ScriptLocalization.Get("MainMenu/YourLogin") + " " + PlayerInfo.me.PlayerStats.Name;
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000E6F20 File Offset: 0x000E5120
		private void OnDisable()
		{
			this.registerButton.onClick.RemoveListener(new UnityAction(this.RegisterLegacyPlayer));
			this.exitButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			this.supportButton.onClick.RemoveListener(new UnityAction(this.Support));
			SingletonMono<MainMenu>.Instance.ActivateLoginPanelButtons();
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x00040ECD File Offset: 0x0003F0CD
		public void RegisterMigrationAccountError(string message)
		{
			this.errorText.text = message;
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x00040ECD File Offset: 0x0003F0CD
		public void RegisterMigrationAccountFailure(string message)
		{
			this.errorText.text = message;
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x00029172 File Offset: 0x00027372
		public void RegisterMigrationAccountSuccess()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x00040EDB File Offset: 0x0003F0DB
		private void ExitWindow()
		{
			base.gameObject.SetActive(false);
			SingletonMono<MainMenu>.Instance.ShowMultiplayerMenuForMigration();
			SingletonMono<MainMenu>.Instance.ShowDefaultMultiplayerMenu();
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x000E6F8C File Offset: 0x000E518C
		private void RegisterLegacyPlayer()
		{
			if (string.IsNullOrEmpty(this.passwordInputField.text) || string.IsNullOrEmpty(this.oldEmailInputField.text) || string.IsNullOrEmpty(this.confirmPasswordInputField.text))
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/EmptyEmail");
				return;
			}
			if (this.passwordInputField.text != this.confirmPasswordInputField.text)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/PasswordMatch");
				return;
			}
			if (this.oldEmailInputField.text.IndexOf('@') <= 0)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/IncorrectEmail");
				return;
			}
			if (this.passwordInputField.text.Length < 5 || this.confirmPasswordInputField.text.Length < 5)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/PasswordRequirements");
				return;
			}
			if (!this.privacyPolicyToggle.isOn)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/AcceptPrivacyPolicy");
				return;
			}
			this.errorText.text = "";
			MigrationSendedPlayer migrationSendedPlayer = new MigrationSendedPlayer(PlayerInfo.me.PlayerStats.Name, this.passwordInputField.text, this.oldEmailInputField.text);
			Singleton<LoginController>.Instance.RegisterLegacyUser(migrationSendedPlayer);
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x00040EC1 File Offset: 0x0003F0C1
		private void Support()
		{
			SingletonMono<MainMenu>.Instance.SupportPanelOpen();
		}

		// Token: 0x04001BE3 RID: 7139
		[SerializeField]
		private TMP_InputField passwordInputField;

		// Token: 0x04001BE4 RID: 7140
		[SerializeField]
		private TMP_InputField confirmPasswordInputField;

		// Token: 0x04001BE5 RID: 7141
		[SerializeField]
		private TMP_InputField oldEmailInputField;

		// Token: 0x04001BE6 RID: 7142
		[SerializeField]
		private Toggle privacyPolicyToggle;

		// Token: 0x04001BE7 RID: 7143
		[SerializeField]
		private Button registerButton;

		// Token: 0x04001BE8 RID: 7144
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001BE9 RID: 7145
		[SerializeField]
		private TMP_Text loginText;

		// Token: 0x04001BEA RID: 7146
		[SerializeField]
		private TMP_Text errorText;

		// Token: 0x04001BEB RID: 7147
		[SerializeField]
		private Button supportButton;
	}
}

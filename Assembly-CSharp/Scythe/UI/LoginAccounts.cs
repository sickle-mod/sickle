using System;
using System.Text;
using Assets.Scripts.LocalStore;
using Scythe.Analytics;
using Scythe.Multiplayer;
using Scythe.Multiplayer.AuthApi.Models;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003BF RID: 959
	public class LoginAccounts : MonoBehaviour
	{
		// Token: 0x06001BF5 RID: 7157 RVA: 0x000AF86C File Offset: 0x000ADA6C
		private void OnEnable()
		{
			this.UpdateControls();
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.account_settings, Contexts.outgame);
			Singleton<LoginController>.Instance.OnDeleteAccountSuccess += this.Logout;
			this.supportButton.onClick.AddListener(new UnityAction(this.Support));
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x0003A41E File Offset: 0x0003861E
		private void OnDisable()
		{
			Singleton<LoginController>.Instance.OnDeleteAccountSuccess -= this.Logout;
			this.supportButton.onClick.RemoveListener(new UnityAction(this.Support));
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x000AF8C0 File Offset: 0x000ADAC0
		public void UpdateControls()
		{
			RegisterRequest user = AuthRestAPI.GetUser();
			if (user != null)
			{
				this.playerName.SetText(user.Login, true);
				this.userMail.text = this.ValidateEmailSize(user.Email);
				this.resetPasswordButton.interactable = true;
				this.editMyProfileButton.interactable = true;
				this.logoutButton.interactable = true;
				return;
			}
			this.playerName.SetText("-", true);
			if (PlatformManager.IsStandalone)
			{
				this.userMail.text = "-";
			}
			if (PlatformManager.IsMobile)
			{
				this.userMail.text = "-";
				this.resetPasswordButton.interactable = false;
				this.editMyProfileButton.interactable = false;
				this.logoutButton.interactable = false;
			}
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x0003A452 File Offset: 0x00038652
		public bool CanShowLoginAccounts()
		{
			return AsmodeeLogic.Instance.GetUser() != null;
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x000AF988 File Offset: 0x000ADB88
		public void Logout()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_logout_button);
			this._prefsStorage.AuthData.Clear();
			Singleton<LoginController>.Instance.Logout();
			this.UpdateControls();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x0003A461 File Offset: 0x00038661
		public void EditMyProfile()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_edit_profile_button);
			this.mainMenu.ShowEditProfilePanel();
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x0003A481 File Offset: 0x00038681
		public void ChangePassword()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_reset_password_button);
			this.mainMenu.ShowChangePasswordPanel();
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x0003A4A1 File Offset: 0x000386A1
		public void Support()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_support_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.customer_support, Contexts.outgame);
			SingletonMono<MainMenu>.Instance.SupportPanelOpen();
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x000AF9D4 File Offset: 0x000ADBD4
		public void Close()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_exit_button);
			if (PlatformManager.IsStandalone)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_ok_button);
			}
			else if (PlatformManager.IsMobile)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_back_button);
				base.gameObject.SetActive(false);
			}
			base.gameObject.SetActive(false);
			this.mainMenu.optionsMenu.GetComponent<OptionsManager>().ShowOptionsPanel();
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000AFA3C File Offset: 0x000ADC3C
		private string ValidateEmailSize(string mail)
		{
			StringBuilder stringBuilder = new StringBuilder(mail);
			if (mail.Length < this.longEmailThreshold)
			{
				return stringBuilder.ToString();
			}
			string[] array = mail.Split('@', StringSplitOptions.None);
			stringBuilder.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.AppendLine(array[i] ?? "");
				if (i < array.Length - 1)
				{
					stringBuilder.Append("@");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001411 RID: 5137
		[SerializeField]
		private MainMenu mainMenu;

		// Token: 0x04001412 RID: 5138
		[SerializeField]
		private TextMeshProUGUI playerName;

		// Token: 0x04001413 RID: 5139
		[SerializeField]
		private TextMeshProUGUI userMail;

		// Token: 0x04001414 RID: 5140
		[SerializeField]
		private Button resetPasswordButton;

		// Token: 0x04001415 RID: 5141
		[SerializeField]
		private Button editMyProfileButton;

		// Token: 0x04001416 RID: 5142
		[SerializeField]
		private Button logoutButton;

		// Token: 0x04001417 RID: 5143
		[SerializeField]
		private Button supportButton;

		// Token: 0x04001418 RID: 5144
		private IKeyValueStorage _prefsStorage = new PlayerPrefsStorage();

		// Token: 0x04001419 RID: 5145
		private int longEmailThreshold = 40;
	}
}

using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Assets.Scripts.LocalStore;
using Assets.Scripts.LocalStore.Model;
using I2.Loc;
using Multiplayer.AuthApi;
using Newtonsoft.Json;
using Scythe.Multiplayer.AuthApi.Models;
using Scythe.Multiplayer.Data;
using Scythe.Multiplayer.Notifications;
using Scythe.Utilities;
using UnityEngine;
using Utils;

namespace Scythe.Multiplayer
{
	// Token: 0x02000279 RID: 633
	public class LoginController : Singleton<LoginController>
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x00034FAA File Offset: 0x000331AA
		// (set) Token: 0x0600136C RID: 4972 RVA: 0x00034FB2 File Offset: 0x000331B2
		public bool IsPlayerLoggedOut { get; set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x00034FBB File Offset: 0x000331BB
		// (set) Token: 0x0600136E RID: 4974 RVA: 0x00034FC3 File Offset: 0x000331C3
		public bool IsPlayerLoggedIn { get; private set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x00034FCC File Offset: 0x000331CC
		// (set) Token: 0x06001370 RID: 4976 RVA: 0x00034FD4 File Offset: 0x000331D4
		public bool IsAccountLinked { get; private set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x00034FDD File Offset: 0x000331DD
		public string ConnectionError
		{
			get
			{
				return ScriptLocalization.Get("MainMenu/ConnectionError");
			}
		}

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06001372 RID: 4978 RVA: 0x000988D4 File Offset: 0x00096AD4
		// (remove) Token: 0x06001373 RID: 4979 RVA: 0x0009890C File Offset: 0x00096B0C
		public event Action<RegisterResponse> OnRegisterSuccess;

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06001374 RID: 4980 RVA: 0x00098944 File Offset: 0x00096B44
		// (remove) Token: 0x06001375 RID: 4981 RVA: 0x0009897C File Offset: 0x00096B7C
		public event Action<FailureResponse> OnRegisterFailure;

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06001376 RID: 4982 RVA: 0x000989B4 File Offset: 0x00096BB4
		// (remove) Token: 0x06001377 RID: 4983 RVA: 0x000989EC File Offset: 0x00096BEC
		public event Action<string> OnRegisterError;

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06001378 RID: 4984 RVA: 0x00098A24 File Offset: 0x00096C24
		// (remove) Token: 0x06001379 RID: 4985 RVA: 0x00098A5C File Offset: 0x00096C5C
		public event global::System.Action OnRegisterEmailAlreadyInUse;

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x0600137A RID: 4986 RVA: 0x00098A94 File Offset: 0x00096C94
		// (remove) Token: 0x0600137B RID: 4987 RVA: 0x00098ACC File Offset: 0x00096CCC
		public event global::System.Action OnMigrationNeeded;

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x0600137C RID: 4988 RVA: 0x00098B04 File Offset: 0x00096D04
		// (remove) Token: 0x0600137D RID: 4989 RVA: 0x00098B3C File Offset: 0x00096D3C
		public event Action<RegisterResponse> OnAccountMigrationSuccess;

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x0600137E RID: 4990 RVA: 0x00098B74 File Offset: 0x00096D74
		// (remove) Token: 0x0600137F RID: 4991 RVA: 0x00098BAC File Offset: 0x00096DAC
		public event Action<FailureResponse> OnAccountMigrationFailure;

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x06001380 RID: 4992 RVA: 0x00098BE4 File Offset: 0x00096DE4
		// (remove) Token: 0x06001381 RID: 4993 RVA: 0x00098C1C File Offset: 0x00096E1C
		public event Action<string> OnAccountMigrationError;

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06001382 RID: 4994 RVA: 0x00098C54 File Offset: 0x00096E54
		// (remove) Token: 0x06001383 RID: 4995 RVA: 0x00098C8C File Offset: 0x00096E8C
		public event global::System.Action OnActivationNeeded;

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06001384 RID: 4996 RVA: 0x00098CC4 File Offset: 0x00096EC4
		// (remove) Token: 0x06001385 RID: 4997 RVA: 0x00098CFC File Offset: 0x00096EFC
		public event global::System.Action OnLoginAttempt;

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06001386 RID: 4998 RVA: 0x00034FE9 File Offset: 0x000331E9
		// (remove) Token: 0x06001387 RID: 4999 RVA: 0x00035019 File Offset: 0x00033219
		public event Action<LoginResponse> OnLoginSuccess
		{
			add
			{
				this.onLoginSuccess = (Action<LoginResponse>)Delegate.Remove(this.onLoginSuccess, value);
				this.onLoginSuccess = (Action<LoginResponse>)Delegate.Combine(this.onLoginSuccess, value);
			}
			remove
			{
				this.onLoginSuccess = (Action<LoginResponse>)Delegate.Remove(this.onLoginSuccess, value);
			}
		}

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x06001388 RID: 5000 RVA: 0x00098D34 File Offset: 0x00096F34
		// (remove) Token: 0x06001389 RID: 5001 RVA: 0x00098D6C File Offset: 0x00096F6C
		public event Action<FailureResponse> OnLoginFailure;

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x0600138A RID: 5002 RVA: 0x00098DA4 File Offset: 0x00096FA4
		// (remove) Token: 0x0600138B RID: 5003 RVA: 0x00098DDC File Offset: 0x00096FDC
		public event Action<string> OnLoginError;

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x0600138C RID: 5004 RVA: 0x00098E14 File Offset: 0x00097014
		// (remove) Token: 0x0600138D RID: 5005 RVA: 0x00098E4C File Offset: 0x0009704C
		public event global::System.Action OnLogoutSuccess;

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x0600138E RID: 5006 RVA: 0x00098E84 File Offset: 0x00097084
		// (remove) Token: 0x0600138F RID: 5007 RVA: 0x00098EBC File Offset: 0x000970BC
		public event global::System.Action OnLogoutError;

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x06001390 RID: 5008 RVA: 0x00098EF4 File Offset: 0x000970F4
		// (remove) Token: 0x06001391 RID: 5009 RVA: 0x00098F2C File Offset: 0x0009712C
		public event global::System.Action ShowChangePasswordPanel;

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06001392 RID: 5010 RVA: 0x00098F64 File Offset: 0x00097164
		// (remove) Token: 0x06001393 RID: 5011 RVA: 0x00098F9C File Offset: 0x0009719C
		public event global::System.Action PasswordChangedEvent;

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x06001394 RID: 5012 RVA: 0x00098FD4 File Offset: 0x000971D4
		// (remove) Token: 0x06001395 RID: 5013 RVA: 0x0009900C File Offset: 0x0009720C
		public event Action<FailureResponse> OnPasswordChangeFailure;

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06001396 RID: 5014 RVA: 0x00099044 File Offset: 0x00097244
		// (remove) Token: 0x06001397 RID: 5015 RVA: 0x0009907C File Offset: 0x0009727C
		public event Action<string> OnPasswordChangeErrorEvent;

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x06001398 RID: 5016 RVA: 0x000990B4 File Offset: 0x000972B4
		// (remove) Token: 0x06001399 RID: 5017 RVA: 0x000990EC File Offset: 0x000972EC
		public event Action<LoginResponse> OnAuthenticationSuccess;

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x0600139A RID: 5018 RVA: 0x00099124 File Offset: 0x00097324
		// (remove) Token: 0x0600139B RID: 5019 RVA: 0x0009915C File Offset: 0x0009735C
		public event Action<LoginResponse> OnPlayerIdChangeSuccess;

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x0600139C RID: 5020 RVA: 0x00099194 File Offset: 0x00097394
		// (remove) Token: 0x0600139D RID: 5021 RVA: 0x000991CC File Offset: 0x000973CC
		public event global::System.Action OnConnectionError;

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x0600139E RID: 5022 RVA: 0x00099204 File Offset: 0x00097404
		// (remove) Token: 0x0600139F RID: 5023 RVA: 0x0009923C File Offset: 0x0009743C
		public event Action<ChangeEmailResponse> OnEmailChangeSuccess;

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x060013A0 RID: 5024 RVA: 0x00099274 File Offset: 0x00097474
		// (remove) Token: 0x060013A1 RID: 5025 RVA: 0x000992AC File Offset: 0x000974AC
		public event Action<FailureResponse> OnEmailChangeFailure;

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x060013A2 RID: 5026 RVA: 0x000992E4 File Offset: 0x000974E4
		// (remove) Token: 0x060013A3 RID: 5027 RVA: 0x0009931C File Offset: 0x0009751C
		public event Action<string> OnEmailChangeError;

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x060013A4 RID: 5028 RVA: 0x00099354 File Offset: 0x00097554
		// (remove) Token: 0x060013A5 RID: 5029 RVA: 0x0009938C File Offset: 0x0009758C
		public event Action<ResendEmailChangeEmailResponse> OnResendEmailChangeSuccess;

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x060013A6 RID: 5030 RVA: 0x000993C4 File Offset: 0x000975C4
		// (remove) Token: 0x060013A7 RID: 5031 RVA: 0x000993FC File Offset: 0x000975FC
		public event Action<FailureResponse> OnResendEmailChangeFailure;

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x060013A8 RID: 5032 RVA: 0x00099434 File Offset: 0x00097634
		// (remove) Token: 0x060013A9 RID: 5033 RVA: 0x0009946C File Offset: 0x0009766C
		public event Action<string> OnResendEmailChangeError;

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x060013AA RID: 5034 RVA: 0x000994A4 File Offset: 0x000976A4
		// (remove) Token: 0x060013AB RID: 5035 RVA: 0x000994DC File Offset: 0x000976DC
		public event global::System.Action OnDeleteAccountSuccess;

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x060013AC RID: 5036 RVA: 0x00099514 File Offset: 0x00097714
		// (remove) Token: 0x060013AD RID: 5037 RVA: 0x0009954C File Offset: 0x0009774C
		public event Action<FailureResponse> OnDeleteAccountFailure;

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x060013AE RID: 5038 RVA: 0x00099584 File Offset: 0x00097784
		// (remove) Token: 0x060013AF RID: 5039 RVA: 0x000995BC File Offset: 0x000977BC
		public event Action<string> OnDeleteAccountError;

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x060013B0 RID: 5040 RVA: 0x000995F4 File Offset: 0x000977F4
		// (remove) Token: 0x060013B1 RID: 5041 RVA: 0x0009962C File Offset: 0x0009782C
		public event global::System.Action ShowResetPasswordPanel;

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x060013B2 RID: 5042 RVA: 0x00099664 File Offset: 0x00097864
		// (remove) Token: 0x060013B3 RID: 5043 RVA: 0x0009969C File Offset: 0x0009789C
		public event global::System.Action OnPasswordResetEvent;

		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x060013B4 RID: 5044 RVA: 0x000996D4 File Offset: 0x000978D4
		// (remove) Token: 0x060013B5 RID: 5045 RVA: 0x0009970C File Offset: 0x0009790C
		public event Action<FailureResponse> OnPasswordResetFailure;

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x060013B6 RID: 5046 RVA: 0x00099744 File Offset: 0x00097944
		// (remove) Token: 0x060013B7 RID: 5047 RVA: 0x0009977C File Offset: 0x0009797C
		public event Action<string> OnPasswordResetErrorEvent;

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x060013B8 RID: 5048 RVA: 0x000997B4 File Offset: 0x000979B4
		// (remove) Token: 0x060013B9 RID: 5049 RVA: 0x000997EC File Offset: 0x000979EC
		public event Action<ResendPasswordResetEmailResponse> OnResendEmailResetPasswordSuccess;

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x060013BA RID: 5050 RVA: 0x00099824 File Offset: 0x00097A24
		// (remove) Token: 0x060013BB RID: 5051 RVA: 0x0009985C File Offset: 0x00097A5C
		public event Action<FailureResponse> OnResendEmailResetPasswordFailure;

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x060013BC RID: 5052 RVA: 0x00099894 File Offset: 0x00097A94
		// (remove) Token: 0x060013BD RID: 5053 RVA: 0x000998CC File Offset: 0x00097ACC
		public event Action<string> OnResendEmailResetPasswordError;

		// Token: 0x140000AA RID: 170
		// (add) Token: 0x060013BE RID: 5054 RVA: 0x00099904 File Offset: 0x00097B04
		// (remove) Token: 0x060013BF RID: 5055 RVA: 0x0009993C File Offset: 0x00097B3C
		public event Action<ResendActivationEmailResponse> OnResendEmailRegisterAccountSuccess;

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x060013C0 RID: 5056 RVA: 0x00099974 File Offset: 0x00097B74
		// (remove) Token: 0x060013C1 RID: 5057 RVA: 0x000999AC File Offset: 0x00097BAC
		public event Action<FailureResponse> OnResendEmailRegisterAccountFailure;

		// Token: 0x140000AC RID: 172
		// (add) Token: 0x060013C2 RID: 5058 RVA: 0x000999E4 File Offset: 0x00097BE4
		// (remove) Token: 0x060013C3 RID: 5059 RVA: 0x00099A1C File Offset: 0x00097C1C
		public event Action<string> OnResendEmailRegisterAccountError;

		// Token: 0x140000AD RID: 173
		// (add) Token: 0x060013C4 RID: 5060 RVA: 0x00099A54 File Offset: 0x00097C54
		// (remove) Token: 0x060013C5 RID: 5061 RVA: 0x00099A8C File Offset: 0x00097C8C
		public event Action<ResendLoginReminderEmailResponse> OnResendEmailLoginReminderSuccess;

		// Token: 0x140000AE RID: 174
		// (add) Token: 0x060013C6 RID: 5062 RVA: 0x00099AC4 File Offset: 0x00097CC4
		// (remove) Token: 0x060013C7 RID: 5063 RVA: 0x00099AFC File Offset: 0x00097CFC
		public event Action<FailureResponse> OnResendEmailLoginReminderFailure;

		// Token: 0x140000AF RID: 175
		// (add) Token: 0x060013C8 RID: 5064 RVA: 0x00099B34 File Offset: 0x00097D34
		// (remove) Token: 0x060013C9 RID: 5065 RVA: 0x00099B6C File Offset: 0x00097D6C
		public event Action<string> OnResendEmailLoginReminderError;

		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x060013CA RID: 5066 RVA: 0x00099BA4 File Offset: 0x00097DA4
		// (remove) Token: 0x060013CB RID: 5067 RVA: 0x00099BDC File Offset: 0x00097DDC
		public event global::System.Action OnServerCallWaitPanel;

		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x060013CC RID: 5068 RVA: 0x00099C14 File Offset: 0x00097E14
		// (remove) Token: 0x060013CD RID: 5069 RVA: 0x00099C4C File Offset: 0x00097E4C
		public event global::System.Action OnServerCallWaitPanelHide;

		// Token: 0x060013CE RID: 5070 RVA: 0x00099C84 File Offset: 0x00097E84
		public void Register(string login, string password, string email)
		{
			global::System.Action onServerCallWaitPanel = this.OnServerCallWaitPanel;
			if (onServerCallWaitPanel != null)
			{
				onServerCallWaitPanel();
			}
			this.loginSended = login;
			this.registerEmailSended = email;
			AuthRestAPI.RegisterNewUser(login, password, email, new Action<RegisterResponse>(this.AuthRestAPI_OnRegisterSuccess), new Action<FailureResponse>(this.AuthRestAPI_OnRegisterFailure), new Action<Exception>(this.AuthRestAPI_OnRegisterError));
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00099CDC File Offset: 0x00097EDC
		public void RegisterLegacyUser(MigrationSendedPlayer migrationSendedPlayer)
		{
			global::System.Action onServerCallWaitPanel = this.OnServerCallWaitPanel;
			if (onServerCallWaitPanel != null)
			{
				onServerCallWaitPanel();
			}
			this.loginSended = migrationSendedPlayer.login;
			AuthRestAPI.RegisterLegacyUser(migrationSendedPlayer, new Action<RegisterResponse>(this.AuthRestAPI_OnMigrationAccountSuccess), new Action<FailureResponse>(this.AuthRestAPI_OnMigrationAccountFailure), new Action<Exception>(this.AuthRestAPI_OnRegisterError));
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00035032 File Offset: 0x00033232
		public void UpdateFirebaseToken(string token)
		{
			this.firebaseToken = token;
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0003503B File Offset: 0x0003323B
		public void EmailChange(string newEmail, string password)
		{
			global::System.Action onServerCallWaitPanel = this.OnServerCallWaitPanel;
			if (onServerCallWaitPanel != null)
			{
				onServerCallWaitPanel();
			}
			AuthRestAPI.ChangeEmail(newEmail, password, new Action<ChangeEmailResponse>(this.AuthRestAPI_OnEmailChangeSuccess), new Action<FailureResponse>(this.AuthRestAPI_OnEmailChangeFailure), new Action<Exception>(this.AuthRestAPI_OnEmailChangeError));
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x00099D30 File Offset: 0x00097F30
		public void ResendEmailLoginReminder()
		{
			global::System.Action onServerCallWaitPanel = this.OnServerCallWaitPanel;
			if (onServerCallWaitPanel != null)
			{
				onServerCallWaitPanel();
			}
			AuthRestAPI.ResendEmailLoginReminderEndpoint(this.registerEmailSended, new Action<ResendLoginReminderEmailResponse>(this.AuthRestAPI_OnResendEmailLoginReminderSuccess), new Action<FailureResponse>(this.AuthRestAPI_OnResendEmailLoginReminderFailure), new Action<Exception>(this.AuthRestAPI_OnResendEmailLoginReminderError));
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00035079 File Offset: 0x00033279
		public void ResendEmailChangeEmail()
		{
			global::System.Action onServerCallWaitPanel = this.OnServerCallWaitPanel;
			if (onServerCallWaitPanel != null)
			{
				onServerCallWaitPanel();
			}
			AuthRestAPI.ResendEmailChangeEmailEndpoint(new Action<ResendEmailChangeEmailResponse>(this.AuthRestAPI_OnResendEmailChangeEmailSuccess), new Action<FailureResponse>(this.AuthRestAPI_OnResendEmailChangeEmailFailure), new Action<Exception>(this.AuthRestAPI_OnResendEmailChangeEmailError));
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x000350B5 File Offset: 0x000332B5
		public void DeleteAccount(string password)
		{
			global::System.Action onServerCallWaitPanel = this.OnServerCallWaitPanel;
			if (onServerCallWaitPanel != null)
			{
				onServerCallWaitPanel();
			}
			AuthRestAPI.DeleteAccount(password, new Action<DeleteAccountResponse>(this.AuthRestAPI_OnDeleteAccountSuccess), new Action<FailureResponse>(this.AuthRestAPI_OnDeleteAccountFailure), new Action<Exception>(this.AuthRestAPI_OnDeleteAccountError));
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00099D80 File Offset: 0x00097F80
		public void ResetPassword(string loginOrEmail, string newPassword)
		{
			global::System.Action onServerCallWaitPanel = this.OnServerCallWaitPanel;
			if (onServerCallWaitPanel != null)
			{
				onServerCallWaitPanel();
			}
			this.resetPasswordLoginOrMailSended = loginOrEmail;
			AuthRestAPI.ResetPassword(loginOrEmail, newPassword, new Action<ResetPasswordResponse>(this.AuthRestAPI_ResetPasswordSuccess), new Action<FailureResponse>(this.AuthRestAPI_ResetPasswordFailure), new Action<Exception>(this.AuthRestAPI_ResetPasswordError));
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00099DD0 File Offset: 0x00097FD0
		public void ResendEmailResetPassword()
		{
			global::System.Action onServerCallWaitPanel = this.OnServerCallWaitPanel;
			if (onServerCallWaitPanel != null)
			{
				onServerCallWaitPanel();
			}
			AuthRestAPI.ResendEmailResetPasswordEndpoint(this.resetPasswordLoginOrMailSended, new Action<ResendPasswordResetEmailResponse>(this.AuthRestAPI_OnResendEmailResetPasswordSuccess), new Action<FailureResponse>(this.AuthRestAPI_OnResendEmailResetPasswordFailure), new Action<Exception>(this.AuthRestAPI_OnResendEmailResetPasswordError));
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00099E20 File Offset: 0x00098020
		public void ResendActivationEmail()
		{
			global::System.Action onServerCallWaitPanel = this.OnServerCallWaitPanel;
			if (onServerCallWaitPanel != null)
			{
				onServerCallWaitPanel();
			}
			AuthRestAPI.ResendEmailRegisterAccountEndpoint(this.loginSended, new Action<ResendActivationEmailResponse>(this.AuthRestAPI_OnResendEmailRegisterAccountSuccess), new Action<FailureResponse>(this.AuthRestAPI_OnResendEmailRegisterAccountFailure), new Action<Exception>(this.AuthRestAPI_OnResendEmailRegisterAccountError));
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00099E70 File Offset: 0x00098070
		public void Login(string login, string password)
		{
			global::System.Action onLoginAttempt = this.OnLoginAttempt;
			if (onLoginAttempt != null)
			{
				onLoginAttempt();
			}
			if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
			{
				this.loginSended = login;
				AuthRestAPI.LoginUser(login, password, new Action<LoginResponse>(this.AuthRestAPI_OnLoginSuccess), new Action<FailureResponse>(this.AuthRestAPI_OnLoginFailure), new Action<Exception>(this.AuthRestAPI_OnLoginError));
				return;
			}
			Action<string> onLoginError = this.OnLoginError;
			if (onLoginError == null)
			{
				return;
			}
			onLoginError("Login and/or password is empty!");
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x000350F2 File Offset: 0x000332F2
		private void LoginWithTicket(AuthGrantType authGrantType, object ticket)
		{
			global::System.Action onLoginAttempt = this.OnLoginAttempt;
			if (onLoginAttempt != null)
			{
				onLoginAttempt();
			}
			AuthRestAPI.LoginUserWithTicket(authGrantType, ticket, new Action<LoginResponse>(this.AuthRestAPI_OnLoginSuccess), new Action<FailureResponse>(this.AuthRestAPI_OnLoginFailure), new Action<Exception>(this.AuthRestAPI_OnLoginError));
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x00035130 File Offset: 0x00033330
		private void LoginWithVerificationSignature(AuthGrantType authGrantType, object verificationSignature)
		{
			global::System.Action onLoginAttempt = this.OnLoginAttempt;
			if (onLoginAttempt != null)
			{
				onLoginAttempt();
			}
			AuthRestAPI.LoginUserWithVerificationSignature(authGrantType, verificationSignature, new Action<LoginResponse>(this.AuthRestAPI_OnLoginSuccess), new Action<FailureResponse>(this.AuthRestAPI_OnLoginFailure), new Action<Exception>(this.AuthRestAPI_OnLoginError));
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x00099EE8 File Offset: 0x000980E8
		public void Logout()
		{
			if (!this.IsPlayerLoggedIn)
			{
				Debug.LogError("[LoginController Tried to logout when the player is not logged in!");
				global::System.Action onLogoutError = this.OnLogoutError;
				if (onLogoutError == null)
				{
					return;
				}
				onLogoutError();
				return;
			}
			else
			{
				this.RemoveRefreshToken();
				this.UnlinkAccount();
				this.IsPlayerLoggedIn = false;
				global::System.Action onLogoutSuccess = this.OnLogoutSuccess;
				if (onLogoutSuccess == null)
				{
					return;
				}
				onLogoutSuccess();
				return;
			}
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x00099F3C File Offset: 0x0009813C
		public void TryToAutoLogin(bool force = false)
		{
			if (this.IsPlayerLoggedOut)
			{
				return;
			}
			if (this.IsPlayerLoggedIn && !force)
			{
				this.PerformLogin(GameServiceController.Instance.PlayerId(), LoginPlatformManager.GetLoginPlatform(), null);
				return;
			}
			if (this._prefsStorage.HasKey(PlayerPrefsStorageKeys.Auth_LocalLoggedUserDataObj))
			{
				LocalLoggedUserData value = this._prefsStorage.GetValue<LocalLoggedUserData>(PlayerPrefsStorageKeys.Auth_LocalLoggedUserDataObj, null);
				if (!string.IsNullOrEmpty(value.RefreshToken))
				{
					this.LoginWithTicket(AuthGrantType.RefreshToken, this.DecryptToken(value.RefreshToken));
					return;
				}
			}
			GameServiceController.Instance.GetIdentityVerificationSignature(delegate(object serverAuthCode)
			{
				this.LoginWithVerificationSignature(LoginPlatformManager.GetLoginPlatform().ToAuthGrantType(), serverAuthCode);
			});
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0003516E File Offset: 0x0003336E
		public void ChangePassword(string oldPassword, string newPassword)
		{
			global::System.Action onServerCallWaitPanel = this.OnServerCallWaitPanel;
			if (onServerCallWaitPanel != null)
			{
				onServerCallWaitPanel();
			}
			AuthRestAPI.ChangePassword(oldPassword, newPassword, new Action<ChangePasswordResponse>(this.AuthRestAPI_ChangePasswordSuccess), new Action<FailureResponse>(this.AuthRestAPI_ChangePasswordFailure), new Action<Exception>(this.AuthRestAPI_ChangePasswordError));
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x00099FD0 File Offset: 0x000981D0
		private void PerformLogin(string thirdPartyPlayerId, LoginPlatform platform, global::System.Action onResponse = null)
		{
			LoginNew loginNew = new LoginNew(PlayerInfo.me.Token, "1.65a", GameServiceController.Instance.InvadersFromAfarUnlocked(), thirdPartyPlayerId, platform, LocalizationManager.CurrentLanguage);
			Action<string> <>9__2;
			RequestController.RequestPostCall("Login/SimpleLogin", JsonConvert.SerializeObject(loginNew), true, delegate(string response)
			{
				global::System.Action onResponse2 = onResponse;
				if (onResponse2 != null)
				{
					onResponse2();
				}
				LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
				this.RegisterClientForNotifications();
				RequestController.Reset();
				MessageExecutor.ResetLastMessageCounter();
				Action<LoginResponse> onAuthenticationSuccess = this.OnAuthenticationSuccess;
				if (onAuthenticationSuccess != null)
				{
					onAuthenticationSuccess(loginResponse);
				}
				Action<LoginResponse> onPlayerIdChangeSuccess = this.OnPlayerIdChangeSuccess;
				if (onPlayerIdChangeSuccess == null)
				{
					return;
				}
				onPlayerIdChangeSuccess(loginResponse);
			}, delegate(Exception e)
			{
				global::System.Action onResponse3 = onResponse;
				if (onResponse3 != null)
				{
					onResponse3();
				}
				Debug.LogError(e);
				if (e is SocketException || e is TimeoutException || e is ArgumentNullException || e is IOException || e.Message.StartsWith("<?"))
				{
					string text = "Maintenance?";
					Action<string> action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate(string response)
						{
							if (GameSerializer.JsonMessageDeserializerWithStringDeserialization<ServerMaintenance>(response).InMaintenance)
							{
								this.OnLoginError(ScriptLocalization.Get("MainMenu/ServerMaintenance"));
								return;
							}
							this.OnLoginError(string.Empty);
						});
					}
					RequestController.RequestGetCallForAzureFunction(text, action, delegate(Exception er)
					{
						Debug.LogError(er);
					});
					return;
				}
				this.OnLoginError(e.Message);
				Debug.LogError(e);
			});
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x000351AC File Offset: 0x000333AC
		private void RegisterClientForNotifications()
		{
			new RegisterClient().Register(this.firebaseToken);
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x000351BE File Offset: 0x000333BE
		private void DebugLogAuthApiError(Exception exception)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("AuthRestAPI exception:");
			stringBuilder.AppendLine(exception.Message);
			stringBuilder.AppendLine(exception.StackTrace);
			Debug.LogError(stringBuilder.ToString());
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x000351F5 File Offset: 0x000333F5
		private void LinkAccount()
		{
			GameServiceController.Instance.GetIdentityVerificationSignature(delegate(object serverAuthCode)
			{
				AuthRestAPI.LinkAccount(LoginPlatformManager.GetLoginPlatform(), this.GetServerAuthCode(serverAuthCode), new Action<LinkAccountResponse>(this.AuthRestAPI_OnLinkAccountSuccess), new Action<FailureResponse>(this.AuthRestAPI_OnLinkAccountFailure), new Action<Exception>(this.AuthRestAPI_OnLinkAccountError));
			});
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0003520D File Offset: 0x0003340D
		private void UnlinkAccount()
		{
			AuthRestAPI.UnlinkAccount(LoginPlatformManager.GetLoginPlatform(), new Action<UnlinkAccountResponse>(this.AuthRestAPI_OnUnlinkAccountSuccess), new Action<FailureResponse>(this.AuthRestAPI_OnUnlinkAccountFailure), new Action<Exception>(this.AuthRestAPI_OnUnlinkAccountError));
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0003523D File Offset: 0x0003343D
		private string GetServerAuthCode(object ticket)
		{
			return AuthRestAPI.TicketToString(ticket);
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x00035245 File Offset: 0x00033445
		private string EncryptToken(string token)
		{
			return StringCipher.Encrypt(token, UniqueId.GetUniqueId());
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00035252 File Offset: 0x00033452
		private string DecryptToken(string encryptedToken)
		{
			return StringCipher.Decrypt(encryptedToken, UniqueId.GetUniqueId());
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0009A044 File Offset: 0x00098244
		private void AuthRestAPI_OnLoginSuccess(LoginResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Guid? id = apiResult.Id;
			string token = apiResult.Token;
			string email = apiResult.Email;
			string login = apiResult.Login;
			string refreshToken = apiResult.RefreshToken;
			string text = this.EncryptToken(refreshToken);
			LocalLoggedUserData localLoggedUserData = new LocalLoggedUserData(apiResult, id, token, email, login, text);
			this._prefsStorage.SetValue(PlayerPrefsStorageKeys.Auth_LocalLoggedUserDataObj, localLoggedUserData);
			PlayerInfo.me.Token = localLoggedUserData.Token;
			PlayerInfo.me.PlayerStats.Id = localLoggedUserData.Id;
			PlayerInfo.me.PlayerStats.Name = localLoggedUserData.Login;
			AuthRestAPI.SetUser(localLoggedUserData.Login, localLoggedUserData.Email);
			this.IsAccountLinked = apiResult.LinkedAccounts.ToList<LinkedAccount>().Any((LinkedAccount a) => a.Platform == LoginPlatformManager.GetLoginPlatform());
			if (!this.IsAccountLinked)
			{
				this.LinkAccount();
			}
			this.IsPlayerLoggedIn = true;
			this.PerformLogin(GameServiceController.Instance.PlayerId(), LoginPlatformManager.GetLoginPlatform(), delegate
			{
				this.onLoginSuccess(apiResult);
			});
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0009A1A4 File Offset: 0x000983A4
		private void AuthRestAPI_OnLoginFailure(FailureResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			if (apiResult.Error == Error.LoginErrorTransferNeeded)
			{
				global::System.Action onMigrationNeeded = this.OnMigrationNeeded;
				if (onMigrationNeeded == null)
				{
					return;
				}
				onMigrationNeeded();
				return;
			}
			else if (apiResult.Error == Error.LoginErrorActivationNeeded)
			{
				global::System.Action onActivationNeeded = this.OnActivationNeeded;
				if (onActivationNeeded == null)
				{
					return;
				}
				onActivationNeeded();
				return;
			}
			else
			{
				Action<FailureResponse> onLoginFailure = this.OnLoginFailure;
				if (onLoginFailure == null)
				{
					return;
				}
				onLoginFailure(apiResult);
				return;
			}
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0003525F File Offset: 0x0003345F
		private void AuthRestAPI_OnRegisterSuccess(RegisterResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<RegisterResponse> onRegisterSuccess = this.OnRegisterSuccess;
			if (onRegisterSuccess == null)
			{
				return;
			}
			onRegisterSuccess(apiResult);
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x00035283 File Offset: 0x00033483
		private void AuthRestAPI_OnRegisterFailure(FailureResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			if (apiResult.Error == Error.RegisterErrorEmailAlreadyInUseTransferNeeded)
			{
				global::System.Action onRegisterEmailAlreadyInUse = this.OnRegisterEmailAlreadyInUse;
				if (onRegisterEmailAlreadyInUse != null)
				{
					onRegisterEmailAlreadyInUse();
				}
			}
			Action<FailureResponse> onRegisterFailure = this.OnRegisterFailure;
			if (onRegisterFailure == null)
			{
				return;
			}
			onRegisterFailure(apiResult);
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x000352C1 File Offset: 0x000334C1
		private void AuthRestAPI_OnRegisterError(Exception exception)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			this.DebugLogAuthApiError(exception);
			Action<string> onRegisterError = this.OnRegisterError;
			if (onRegisterError == null)
			{
				return;
			}
			onRegisterError(this.ConnectionError);
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x000352F1 File Offset: 0x000334F1
		private void AuthRestAPI_OnMigrationAccountSuccess(RegisterResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<RegisterResponse> onAccountMigrationSuccess = this.OnAccountMigrationSuccess;
			if (onAccountMigrationSuccess == null)
			{
				return;
			}
			onAccountMigrationSuccess(apiResult);
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x00035315 File Offset: 0x00033515
		private void AuthRestAPI_OnMigrationAccountFailure(FailureResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<FailureResponse> onAccountMigrationFailure = this.OnAccountMigrationFailure;
			if (onAccountMigrationFailure == null)
			{
				return;
			}
			onAccountMigrationFailure(apiResult);
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00035339 File Offset: 0x00033539
		private void AuthRestAPI_ChangePasswordSuccess(ChangePasswordResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			global::System.Action passwordChangedEvent = this.PasswordChangedEvent;
			if (passwordChangedEvent == null)
			{
				return;
			}
			passwordChangedEvent();
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0003535C File Offset: 0x0003355C
		private void AuthRestAPI_ChangePasswordFailure(FailureResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<FailureResponse> onPasswordChangeFailure = this.OnPasswordChangeFailure;
			if (onPasswordChangeFailure == null)
			{
				return;
			}
			onPasswordChangeFailure(apiResult);
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00035380 File Offset: 0x00033580
		private void AuthRestAPI_ChangePasswordError(Exception exception)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			this.DebugLogAuthApiError(exception);
			Action<string> onPasswordChangeErrorEvent = this.OnPasswordChangeErrorEvent;
			if (onPasswordChangeErrorEvent == null)
			{
				return;
			}
			onPasswordChangeErrorEvent(this.ConnectionError);
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x000353B0 File Offset: 0x000335B0
		private void AuthRestAPI_ResetPasswordSuccess(ResetPasswordResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			global::System.Action onPasswordResetEvent = this.OnPasswordResetEvent;
			if (onPasswordResetEvent == null)
			{
				return;
			}
			onPasswordResetEvent();
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x000353D3 File Offset: 0x000335D3
		private void AuthRestAPI_ResetPasswordFailure(FailureResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<FailureResponse> onPasswordResetFailure = this.OnPasswordResetFailure;
			if (onPasswordResetFailure == null)
			{
				return;
			}
			onPasswordResetFailure(apiResult);
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x000353F7 File Offset: 0x000335F7
		private void AuthRestAPI_ResetPasswordError(Exception exception)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			this.DebugLogAuthApiError(exception);
			Action<string> onPasswordResetErrorEvent = this.OnPasswordResetErrorEvent;
			if (onPasswordResetErrorEvent == null)
			{
				return;
			}
			onPasswordResetErrorEvent(this.ConnectionError);
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x00035427 File Offset: 0x00033627
		private void AuthRestAPI_OnResendEmailResetPasswordSuccess(ResendPasswordResetEmailResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<ResendPasswordResetEmailResponse> onResendEmailResetPasswordSuccess = this.OnResendEmailResetPasswordSuccess;
			if (onResendEmailResetPasswordSuccess == null)
			{
				return;
			}
			onResendEmailResetPasswordSuccess(apiResult);
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0003544B File Offset: 0x0003364B
		private void AuthRestAPI_OnResendEmailResetPasswordFailure(FailureResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<FailureResponse> onResendEmailResetPasswordFailure = this.OnResendEmailResetPasswordFailure;
			if (onResendEmailResetPasswordFailure == null)
			{
				return;
			}
			onResendEmailResetPasswordFailure(apiResult);
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0003546F File Offset: 0x0003366F
		private void AuthRestAPI_OnResendEmailResetPasswordError(Exception exception)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			this.DebugLogAuthApiError(exception);
			Action<string> onResendEmailResetPasswordError = this.OnResendEmailResetPasswordError;
			if (onResendEmailResetPasswordError == null)
			{
				return;
			}
			onResendEmailResetPasswordError(this.ConnectionError);
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0003549F File Offset: 0x0003369F
		private void AuthRestAPI_OnResendEmailRegisterAccountSuccess(ResendActivationEmailResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<ResendActivationEmailResponse> onResendEmailRegisterAccountSuccess = this.OnResendEmailRegisterAccountSuccess;
			if (onResendEmailRegisterAccountSuccess == null)
			{
				return;
			}
			onResendEmailRegisterAccountSuccess(apiResult);
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x000354C3 File Offset: 0x000336C3
		private void AuthRestAPI_OnResendEmailRegisterAccountFailure(FailureResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<FailureResponse> onResendEmailRegisterAccountFailure = this.OnResendEmailRegisterAccountFailure;
			if (onResendEmailRegisterAccountFailure == null)
			{
				return;
			}
			onResendEmailRegisterAccountFailure(apiResult);
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x000354E7 File Offset: 0x000336E7
		private void AuthRestAPI_OnResendEmailRegisterAccountError(Exception exception)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			this.DebugLogAuthApiError(exception);
			Action<string> onResendEmailRegisterAccountError = this.OnResendEmailRegisterAccountError;
			if (onResendEmailRegisterAccountError == null)
			{
				return;
			}
			onResendEmailRegisterAccountError(this.ConnectionError);
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x00035517 File Offset: 0x00033717
		private void AuthRestAPI_OnLoginError(Exception exception)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			this.DebugLogAuthApiError(exception);
			Action<string> onLoginError = this.OnLoginError;
			if (onLoginError == null)
			{
				return;
			}
			onLoginError(this.ConnectionError);
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x00035547 File Offset: 0x00033747
		private void AuthRestAPI_OnEmailChangeSuccess(ChangeEmailResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<ChangeEmailResponse> onEmailChangeSuccess = this.OnEmailChangeSuccess;
			if (onEmailChangeSuccess == null)
			{
				return;
			}
			onEmailChangeSuccess(apiResult);
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0003556B File Offset: 0x0003376B
		private void AuthRestAPI_OnEmailChangeFailure(FailureResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<FailureResponse> onEmailChangeFailure = this.OnEmailChangeFailure;
			if (onEmailChangeFailure == null)
			{
				return;
			}
			onEmailChangeFailure(apiResult);
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0003558F File Offset: 0x0003378F
		private void AuthRestAPI_OnEmailChangeError(Exception exception)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			this.DebugLogAuthApiError(exception);
			Action<string> onEmailChangeError = this.OnEmailChangeError;
			if (onEmailChangeError == null)
			{
				return;
			}
			onEmailChangeError(this.ConnectionError);
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x000355BF File Offset: 0x000337BF
		private void AuthRestAPI_OnResendEmailChangeEmailSuccess(ResendEmailChangeEmailResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<ResendEmailChangeEmailResponse> onResendEmailChangeSuccess = this.OnResendEmailChangeSuccess;
			if (onResendEmailChangeSuccess == null)
			{
				return;
			}
			onResendEmailChangeSuccess(apiResult);
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x000355E3 File Offset: 0x000337E3
		private void AuthRestAPI_OnResendEmailChangeEmailFailure(FailureResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<FailureResponse> onResendEmailLoginReminderFailure = this.OnResendEmailLoginReminderFailure;
			if (onResendEmailLoginReminderFailure == null)
			{
				return;
			}
			onResendEmailLoginReminderFailure(apiResult);
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x00035607 File Offset: 0x00033807
		private void AuthRestAPI_OnResendEmailChangeEmailError(Exception exception)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			this.DebugLogAuthApiError(exception);
			Action<string> onResendEmailLoginReminderError = this.OnResendEmailLoginReminderError;
			if (onResendEmailLoginReminderError == null)
			{
				return;
			}
			onResendEmailLoginReminderError(this.ConnectionError);
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x00035637 File Offset: 0x00033837
		private void AuthRestAPI_OnResendEmailLoginReminderSuccess(ResendLoginReminderEmailResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<ResendLoginReminderEmailResponse> onResendEmailLoginReminderSuccess = this.OnResendEmailLoginReminderSuccess;
			if (onResendEmailLoginReminderSuccess == null)
			{
				return;
			}
			onResendEmailLoginReminderSuccess(apiResult);
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0003565B File Offset: 0x0003385B
		private void AuthRestAPI_OnResendEmailLoginReminderFailure(FailureResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<FailureResponse> onResendEmailChangeFailure = this.OnResendEmailChangeFailure;
			if (onResendEmailChangeFailure == null)
			{
				return;
			}
			onResendEmailChangeFailure(apiResult);
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0003567F File Offset: 0x0003387F
		private void AuthRestAPI_OnResendEmailLoginReminderError(Exception exception)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			this.DebugLogAuthApiError(exception);
			Action<string> onResendEmailChangeError = this.OnResendEmailChangeError;
			if (onResendEmailChangeError == null)
			{
				return;
			}
			onResendEmailChangeError(this.ConnectionError);
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x000356AF File Offset: 0x000338AF
		private void AuthRestAPI_OnDeleteAccountSuccess(DeleteAccountResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			global::System.Action onDeleteAccountSuccess = this.OnDeleteAccountSuccess;
			if (onDeleteAccountSuccess == null)
			{
				return;
			}
			onDeleteAccountSuccess();
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x000356D2 File Offset: 0x000338D2
		private void AuthRestAPI_OnDeleteAccountFailure(FailureResponse apiResult)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			Action<FailureResponse> onDeleteAccountFailure = this.OnDeleteAccountFailure;
			if (onDeleteAccountFailure == null)
			{
				return;
			}
			onDeleteAccountFailure(apiResult);
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x000356F6 File Offset: 0x000338F6
		private void AuthRestAPI_OnDeleteAccountError(Exception exception)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			this.DebugLogAuthApiError(exception);
			Action<string> onDeleteAccountError = this.OnDeleteAccountError;
			if (onDeleteAccountError == null)
			{
				return;
			}
			onDeleteAccountError(this.ConnectionError);
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00035726 File Offset: 0x00033926
		private void AuthRestAPI_OnLinkAccountSuccess(LinkAccountResponse apiResult)
		{
			Debug.Log("Account Linked!");
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x00035732 File Offset: 0x00033932
		private void AuthRestAPI_OnLinkAccountFailure(FailureResponse apiResult)
		{
			Debug.LogError(string.Format("Account Link failure : {0}", apiResult.Error));
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0003574E File Offset: 0x0003394E
		private void AuthRestAPI_OnLinkAccountError(Exception exception)
		{
			this.DebugLogAuthApiError(exception);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x00035757 File Offset: 0x00033957
		private void AuthRestAPI_OnUnlinkAccountSuccess(UnlinkAccountResponse apiResult)
		{
			Debug.Log("Account unlinked!");
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00035763 File Offset: 0x00033963
		private void AuthRestAPI_OnUnlinkAccountFailure(FailureResponse apiResult)
		{
			Debug.LogError(string.Format("Account Unlink failure : {0}", apiResult.Error));
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0003574E File Offset: 0x0003394E
		private void AuthRestAPI_OnUnlinkAccountError(Exception exception)
		{
			this.DebugLogAuthApiError(exception);
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0003577F File Offset: 0x0003397F
		private void RemoveRefreshToken()
		{
			if (this._prefsStorage.HasKey(PlayerPrefsStorageKeys.Auth_LocalLoggedUserDataObj))
			{
				this._prefsStorage.DeleteKey(PlayerPrefsStorageKeys.Auth_LocalLoggedUserDataObj);
			}
		}

		// Token: 0x04000E90 RID: 3728
		private IKeyValueStorage _prefsStorage = new PlayerPrefsStorage();

		// Token: 0x04000E91 RID: 3729
		private string firebaseToken = string.Empty;

		// Token: 0x04000E92 RID: 3730
		private string resetPasswordLoginOrMailSended;

		// Token: 0x04000E93 RID: 3731
		private string loginSended;

		// Token: 0x04000E94 RID: 3732
		private string registerEmailSended;

		// Token: 0x04000EA6 RID: 3750
		private Action<LoginResponse> onLoginSuccess;
	}
}

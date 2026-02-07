using System;
using Multiplayer.AuthApi;
using Newtonsoft.Json;
using Scythe.Multiplayer.AuthApi.Models;
using UnityEngine;
using VoxelBusters.EssentialKit;

namespace Scythe.Multiplayer
{
	// Token: 0x02000205 RID: 517
	public class AuthRestAPI
	{
		// Token: 0x06000F5C RID: 3932 RVA: 0x00031FD2 File Offset: 0x000301D2
		public static void SetUser(string login, string email)
		{
			AuthRestAPI.user = new RegisterRequest(login, string.Empty, email);
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00031FE5 File Offset: 0x000301E5
		public static RegisterRequest GetUser()
		{
			return AuthRestAPI.user;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0008CF58 File Offset: 0x0008B158
		public static void RegisterNewUser(string login, string password, string email, Action<RegisterResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			RegisterRequest registerRequest = new RegisterRequest(login, password, email);
			AuthRequestController.RequestPostCall<RegisterResponse, FailureResponse>("/Account/Register", JsonConvert.SerializeObject(registerRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0008CF84 File Offset: 0x0008B184
		public static void RegisterLegacyUser(MigrationSendedPlayer migrationSendedPlayer, Action<RegisterResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			RegisterRequest registerRequest = new RegisterRequest(migrationSendedPlayer.login, migrationSendedPlayer.newPassword, migrationSendedPlayer.oldEmail);
			AuthRequestController.RequestPostCall<RegisterResponse, FailureResponse>("/Account/RegisterLegacyPlayer", JsonConvert.SerializeObject(registerRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0008CFC0 File Offset: 0x0008B1C0
		public static void ResetPassword(string loginOrEmail, string password, Action<ResetPasswordResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			ResetPasswordRequest resetPasswordRequest = new ResetPasswordRequest(loginOrEmail, password);
			AuthRequestController.RequestPostCall<ResetPasswordResponse, FailureResponse>("/Account/ResetPassword", JsonConvert.SerializeObject(resetPasswordRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0008CFEC File Offset: 0x0008B1EC
		public static void ResendEmailLoginReminderEndpoint(string email, Action<ResendLoginReminderEmailResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			ResendLoginReminderEmailRequest resendLoginReminderEmailRequest = new ResendLoginReminderEmailRequest(email);
			AuthRequestController.RequestPostCall<ResendLoginReminderEmailResponse, FailureResponse>("/Account/ResendLoginReminderEmail", JsonConvert.SerializeObject(resendLoginReminderEmailRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0008D014 File Offset: 0x0008B214
		public static void ResendEmailChangeEmailEndpoint(Action<ResendEmailChangeEmailResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			ResendEmailChangeEmailRequest resendEmailChangeEmailRequest = new ResendEmailChangeEmailRequest();
			AuthRequestController.RequestPostCall<ResendEmailChangeEmailResponse, FailureResponse>("/Account/ResendEmailChangeEmail", JsonConvert.SerializeObject(resendEmailChangeEmailRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0008D03C File Offset: 0x0008B23C
		public static void ChangePassword(string oldPassword, string newPassword, Action<ChangePasswordResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			ChangePasswordRequest changePasswordRequest = new ChangePasswordRequest(oldPassword, newPassword);
			AuthRequestController.RequestPostCall<ChangePasswordResponse, FailureResponse>("/Account/ChangePassword", JsonConvert.SerializeObject(changePasswordRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0008D068 File Offset: 0x0008B268
		public static void ChangeEmail(string newEmail, string password, Action<ChangeEmailResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			ChangeEmailRequest changeEmailRequest = new ChangeEmailRequest(newEmail, password);
			AuthRequestController.RequestPostCall<ChangeEmailResponse, FailureResponse>("/Account/ChangeEmail", JsonConvert.SerializeObject(changeEmailRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0008D094 File Offset: 0x0008B294
		public static void ResendEmailResetPasswordEndpoint(string loginOrEmail, Action<ResendPasswordResetEmailResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			ResendPasswordResetEmailRequest resendPasswordResetEmailRequest = new ResendPasswordResetEmailRequest(loginOrEmail);
			AuthRequestController.RequestPostCall<ResendPasswordResetEmailResponse, FailureResponse>("/Account/ResendPasswordResetEmail", JsonConvert.SerializeObject(resendPasswordResetEmailRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0008D0BC File Offset: 0x0008B2BC
		public static void ResendEmailRegisterAccountEndpoint(string login, Action<ResendActivationEmailResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			ResendActivationEmailRequest resendActivationEmailRequest = new ResendActivationEmailRequest(login);
			AuthRequestController.RequestPostCall<ResendActivationEmailResponse, FailureResponse>("/Account/ResendActivationEmail", JsonConvert.SerializeObject(resendActivationEmailRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0008D0E4 File Offset: 0x0008B2E4
		public static void DeleteAccount(string password, Action<DeleteAccountResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			DeleteAccountRequest deleteAccountRequest = new DeleteAccountRequest(password);
			AuthRequestController.RequestPostCall<DeleteAccountResponse, FailureResponse>("/Account/DeleteAccount", JsonConvert.SerializeObject(deleteAccountRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0008D10C File Offset: 0x0008B30C
		public static void LoginUser(string login, string password, Action<LoginResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			LoginRequest loginRequest = new LoginRequest(AuthGrantType.Password, new LoginParams(login, password, null, null, null, null, null, null, null));
			AuthRequestController.RequestPostCall<LoginResponse, FailureResponse>("/Account/Login", JsonConvert.SerializeObject(loginRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0008D14C File Offset: 0x0008B34C
		public static void LoginUserWithTicket(AuthGrantType authGrantType, object ticket, Action<LoginResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			LoginRequest loginRequest = AuthRestAPI.GetLoginRequest(authGrantType, AuthRestAPI.TicketToString(ticket));
			AuthRequestController.RequestPostCall<LoginResponse, FailureResponse>("/Account/Login", JsonConvert.SerializeObject(loginRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0008D17C File Offset: 0x0008B37C
		public static void LoginUserWithVerificationSignature(AuthGrantType authGrantType, object ticket, Action<LoginResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			LoginRequest loginRequest = ((authGrantType != AuthGrantType.GameCenter) ? AuthRestAPI.GetLoginRequest(authGrantType, AuthRestAPI.TicketToString(ticket)) : AuthRestAPI.GetLoginRequestIOS(ticket));
			AuthRequestController.RequestPostCall<LoginResponse, FailureResponse>("/Account/Login", JsonConvert.SerializeObject(loginRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0008D1B8 File Offset: 0x0008B3B8
		public static void LinkAccount(LoginPlatform loginPlatform, object ticket, Action<LinkAccountResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			LinkAccountRequest linkAccountRequest = ((loginPlatform != LoginPlatform.GameCenter) ? AuthRestAPI.GetLinkAccountRequest(loginPlatform, AuthRestAPI.TicketToString(ticket)) : AuthRestAPI.GetLinkAccountRequestIOS(ticket));
			AuthRequestController.RequestPostCallWithAuthorization<LinkAccountResponse, FailureResponse>("/Account/LinkAccount", JsonConvert.SerializeObject(linkAccountRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0008D1F4 File Offset: 0x0008B3F4
		public static void UnlinkAccount(LoginPlatform loginPlatform, Action<UnlinkAccountResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			UnlinkAccountRequest unlinkAccountRequest = AuthRestAPI.GetUnlinkAccountRequest(loginPlatform);
			AuthRequestController.RequestPostCallWithAuthorization<UnlinkAccountResponse, FailureResponse>("/Account/UnlinkAccount", JsonConvert.SerializeObject(unlinkAccountRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0008D21C File Offset: 0x0008B41C
		public static void GetBuddies(Action<GetBuddiesResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			GetBuddiesRequest getBuddiesRequest = new GetBuddiesRequest();
			AuthRequestController.RequestPostCallWithAuthorization<GetBuddiesResponse, FailureResponse>("/Buddy/GetBuddies", JsonConvert.SerializeObject(getBuddiesRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0008D244 File Offset: 0x0008B444
		public static void AddBuddy(Guid buddyId, Action<AddBuddyResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			AddBuddyRequest addBuddyRequest = new AddBuddyRequest(new Guid?(buddyId));
			AuthRequestController.RequestPostCallWithAuthorization<AddBuddyResponse, FailureResponse>("/Buddy/AddBuddy", JsonConvert.SerializeObject(addBuddyRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0008D274 File Offset: 0x0008B474
		public static void AddBuddyByName(string name, Action<AddBuddyByNameResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			AddBuddyByNameRequest addBuddyByNameRequest = new AddBuddyByNameRequest(name);
			AuthRequestController.RequestPostCallWithAuthorization<AddBuddyByNameResponse, FailureResponse>("/Buddy/AddBuddyByName", JsonConvert.SerializeObject(addBuddyByNameRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0008D29C File Offset: 0x0008B49C
		public static void RemoveBuddy(Guid buddyId, Action<RemoveBuddyResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			RemoveBuddyRequest removeBuddyRequest = new RemoveBuddyRequest(new Guid?(buddyId));
			AuthRequestController.RequestPostCallWithAuthorization<RemoveBuddyResponse, FailureResponse>("/Buddy/RemoveBuddy", JsonConvert.SerializeObject(removeBuddyRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x0008D2CC File Offset: 0x0008B4CC
		public static void SendSupportTicket(string authorEmail, string messageBody, Action<SendSupportTicketResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			SendSupportTicketRequest sendSupportTicketRequest = new SendSupportTicketRequest(authorEmail, messageBody);
			AuthRequestController.RequestPostCallWithAuthorization<SendSupportTicketResponse, FailureResponse>("/Account/SendSupportTicket", JsonConvert.SerializeObject(sendSupportTicketRequest), onSuccess, onFailure, onError);
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00031FEC File Offset: 0x000301EC
		public static string TicketToString(object ticket)
		{
			if (!string.IsNullOrEmpty((string)ticket))
			{
				return (string)ticket;
			}
			return string.Empty;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00032007 File Offset: 0x00030207
		private static LinkAccountRequest GetLinkAccountRequest(LoginPlatform loginPlatform, string ticket)
		{
			return new LinkAccountRequest(loginPlatform, new LinkAccountParams(ticket, null));
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00032016 File Offset: 0x00030216
		private static UnlinkAccountRequest GetUnlinkAccountRequest(LoginPlatform loginPlatform)
		{
			return new UnlinkAccountRequest(loginPlatform);
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0003201E File Offset: 0x0003021E
		private static LinkAccountRequest GetLinkAccountRequestIOS(object ticket)
		{
			return new LinkAccountRequest(LoginPlatform.GameCenter, new LinkAccountParams(AuthRestAPI.TicketToString(ticket), GameServiceController.Instance.PlayerId()));
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0008D2F8 File Offset: 0x0008B4F8
		private static LoginRequest GetLoginRequest(AuthGrantType authGrantType, string ticket)
		{
			return new LoginRequest(authGrantType, new LoginParams(null, null, ticket, null, null, null, null, null, null));
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0008D324 File Offset: 0x0008B524
		private static LoginRequest GetLoginRequestIOS(object ticket)
		{
			ServerCredentials.IosPlatformProperties iosPlatformProperties = (ServerCredentials.IosPlatformProperties)ticket;
			string text = Convert.ToBase64String(iosPlatformProperties.Signature);
			string text2 = Convert.ToBase64String(iosPlatformProperties.Salt);
			return new LoginRequest(AuthGrantType.GameCenter, new LoginParams(null, null, null, iosPlatformProperties.PublicKeyUrl, text, text2, GameServiceController.Instance.PlayerId(), new long?(iosPlatformProperties.Timestamp), Application.identifier));
		}

		// Token: 0x04000BE0 RID: 3040
		private const string REGISTER_NEW_USER_ENDPOINT = "/Account/Register";

		// Token: 0x04000BE1 RID: 3041
		private const string REGISTER_LEGACY_USER_ENDPOINT = "/Account/RegisterLegacyPlayer";

		// Token: 0x04000BE2 RID: 3042
		private const string RESET_PASSWORD_ENDPOINT = "/Account/ResetPassword";

		// Token: 0x04000BE3 RID: 3043
		private const string CHANGE_PASSWORD_ENDPOINT = "/Account/ChangePassword";

		// Token: 0x04000BE4 RID: 3044
		private const string LOGIN_USER_ENDPOINT = "/Account/Login";

		// Token: 0x04000BE5 RID: 3045
		private const string EMAIL_CHANGE_ENDPOINT = "/Account/ChangeEmail";

		// Token: 0x04000BE6 RID: 3046
		private const string DELETE_ACCOUNT_ENDPOINT = "/Account/DeleteAccount";

		// Token: 0x04000BE7 RID: 3047
		private const string RESEND_EMAIL_EMAIL_CHANGE_ENDPOINT = "/Account/ResendEmailChangeEmail";

		// Token: 0x04000BE8 RID: 3048
		private const string LINK_ACCOUNT_ENDPOINT = "/Account/LinkAccount";

		// Token: 0x04000BE9 RID: 3049
		private const string UNLINK_ACCOUNT_ENDPOINT = "/Account/UnlinkAccount";

		// Token: 0x04000BEA RID: 3050
		private const string RESEND_EMAIL_RESET_PASSWORD_ENDPOINT = "/Account/ResendPasswordResetEmail";

		// Token: 0x04000BEB RID: 3051
		private const string RESEND_EMAIL_REGISTER_ACCOUNT_ENDPOINT = "/Account/ResendActivationEmail";

		// Token: 0x04000BEC RID: 3052
		private const string GET_BUDDIES_ENDPOINT = "/Buddy/GetBuddies";

		// Token: 0x04000BED RID: 3053
		private const string ADD_BUDDY_ENDPOINT = "/Buddy/AddBuddy";

		// Token: 0x04000BEE RID: 3054
		private const string ADD_BUDDY_BY_NAME_ENDPOINT = "/Buddy/AddBuddyByName";

		// Token: 0x04000BEF RID: 3055
		private const string REMOVE_BUDDY_ENDPOINT = "/Buddy/RemoveBuddy";

		// Token: 0x04000BF0 RID: 3056
		private const string RESEND_EMAIL_LOGIN_REMINDER_ENDPOINT = "/Account/ResendLoginReminderEmail";

		// Token: 0x04000BF1 RID: 3057
		private const string SEND_SUPPORT_TICKET_ENDPOINT = "/Account/SendSupportTicket";

		// Token: 0x04000BF2 RID: 3058
		private static RegisterRequest user;
	}
}

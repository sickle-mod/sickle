using System;
using Scythe.Multiplayer.AuthApi.Models;

namespace Multiplayer.AuthApi
{
	// Token: 0x020001A2 RID: 418
	public static class ErrorHandler
	{
		// Token: 0x06000C51 RID: 3153 RVA: 0x00080B24 File Offset: 0x0007ED24
		public static string ErrorMessage(Error error)
		{
			switch (error)
			{
			case Error.None:
			case Error.ValidationError:
				return "ErrorMessages/GenericError";
			case Error.UnspecifiedError:
			case Error.RegisterLegacyErrorUserNotFound:
			case Error.RegisterLegacyErrorNotLegacyUser:
			case Error.RegisterLegacyErrorAlreadyTransferred:
			case Error.ResendActivationEmailErrorUserByLoginNotFound:
			case Error.ResendPasswordResetEmailErrorUserNotFound:
			case Error.LinkAccountErrorUnsupportedPlatform:
			case Error.LinkAccountErrorInvalidTicket:
			case Error.UnlinkAccountErrorUnsupportedPlatform:
				return "ErrorMessages/GenericError";
			case Error.RegisterErrorLoginAlreadyInUseTransferNeeded:
				return "ErrorMessages/RegisterError_LoginAlreadyInUse";
			case Error.RegisterErrorLoginAlreadyInUse:
				return "ErrorMessages/RegisterError_LoginAlreadyInUse";
			case Error.RegisterErrorEmailAlreadyInUse:
				return "ErrorMessages/RegisterError_EmailAlreadyInUse";
			case Error.RegisterLegacyErrorInvalidEmail:
				return "ErrorMessages/RegisterLegacyError_InvalidEmail";
			case Error.ResendActivationEmailErrorUserAlreadyActivated:
				return "ErrorMessages/ResendActivationEmailError_UserAlreadyActivated";
			case Error.LoginErrorValidationFailed:
			case Error.LinkAccountErrorAlreadyLinked:
			case Error.UnlinkAccountErrorNotLinked:
			case Error.AddBuddyErrorCantAddYourself:
			case Error.AddBuddyErrorBuddyUserNotFound:
			case Error.AddBuddyByNameErrorBuddyUserNotFound:
			case Error.AddBuddyByNameErrorCantAddYourself:
			case Error.RemoveBuddyErrorCantRemoveYourself:
			case Error.RemoveBuddyErrorBuddyUserNotFound:
			case Error.RemoveBuddyErrorNotOnBuddyList:
				return string.Empty;
			case Error.LoginErrorTransferNeeded:
			case Error.ResetPasswordErrorTransferNeeded:
				return "ErrorMessages/LoginError_TransferNeeded";
			case Error.LoginErrorAuthenticationFailed:
				return "ErrorMessages/LoginError_AuthenticationFailed";
			case Error.LoginErrorActivationNeeded:
				return "ErrorMessages/LoginError_ActivationNeeded";
			case Error.ResetPasswordErrorUserNotFound:
				return "ErrorMessages/ResetPasswordError_UserNotFound";
			case Error.ResetPasswordErrorSamePassword:
				return "ErrorMessages/ResetPasswordError_SamePassword";
			case Error.ChangePasswordErrorInvalidOldPassword:
				return "ErrorMessages/ChangePasswordError_InvalidOldPassword";
			case Error.ChangePasswordErrorSamePassword:
				return "ErrorMessages/ChangePasswordError_SamePassword";
			case Error.ChangeEmailErrorSameEmail:
				return "ErrorMessages/ChangeEmailError_SameEmail";
			case Error.ChangeEmailErrorEmailAlreadyInUse:
				return "ErrorMessages/RegisterError_EmailAlreadyInUse";
			case Error.ChangeEmailErrorInvalidPassword:
				return "ErrorMessages/ProvidedPasswordInvalid";
			case Error.DeleteAccountErrorInvalidPassword:
				return "ErrorMessages/ProvidedPasswordInvalid";
			case Error.AddBuddyErrorAlreadyOnBuddyList:
			case Error.AddBuddyByNameErrorAlreadyOnBuddyList:
				return "ErrorMessages/BuddyAlreadyOnList";
			}
			return "ErrorMessages/GenericError";
		}

		// Token: 0x040009B9 RID: 2489
		private const string GENERIC_ERROR = "ErrorMessages/GenericError";

		// Token: 0x040009BA RID: 2490
		private const string REGISTER_ERROR_TRANSFER_NEEDED = "ErrorMessages/RegisterError_TransferNeeded";

		// Token: 0x040009BB RID: 2491
		private const string REGISTER_ERROR_LOGIN_ALREADY_IN_USE = "ErrorMessages/RegisterError_LoginAlreadyInUse";

		// Token: 0x040009BC RID: 2492
		private const string REGISTER_ERROR_EMAIL_ALREADY_IN_USE = "ErrorMessages/RegisterError_EmailAlreadyInUse";

		// Token: 0x040009BD RID: 2493
		private const string REGISTER_LEGACY_ERROR_INVALID_EMAIL = "ErrorMessages/RegisterLegacyError_InvalidEmail";

		// Token: 0x040009BE RID: 2494
		private const string RESEND_ACTIVATION_EMAIL_ERROR_USER_ALREADY_ACTIVATED = "ErrorMessages/ResendActivationEmailError_UserAlreadyActivated";

		// Token: 0x040009BF RID: 2495
		private const string LOGIN_ERROR_AUTHENTICATION_FAILED = "ErrorMessages/LoginError_AuthenticationFailed";

		// Token: 0x040009C0 RID: 2496
		private const string LOGIN_ERROR_ACTIVATION_NEEDED = "ErrorMessages/LoginError_ActivationNeeded";

		// Token: 0x040009C1 RID: 2497
		private const string LOGIN_ERROR_TRANSFER_NEEDED = "ErrorMessages/LoginError_TransferNeeded";

		// Token: 0x040009C2 RID: 2498
		private const string RESET_PASSWORD_ERROR_USER_NOT_FOUND = "ErrorMessages/ResetPasswordError_UserNotFound";

		// Token: 0x040009C3 RID: 2499
		private const string RESET_PASSWORD_ERROR_SAME_PASSWORD = "ErrorMessages/ResetPasswordError_SamePassword";

		// Token: 0x040009C4 RID: 2500
		private const string CHANGE_PASSWORD_ERROR_INVALID_OLD_PASSWORD = "ErrorMessages/ChangePasswordError_InvalidOldPassword";

		// Token: 0x040009C5 RID: 2501
		private const string CHANGE_PASSWORD_ERROR_SAME_PASSWORD = "ErrorMessages/ChangePasswordError_SamePassword";

		// Token: 0x040009C6 RID: 2502
		private const string CHANGE_EMAIL_ERROR_SAME_EMAIL = "ErrorMessages/ChangeEmailError_SameEmail";

		// Token: 0x040009C7 RID: 2503
		private const string PROVIDED_PASSWORD_INVALID = "ErrorMessages/ProvidedPasswordInvalid";

		// Token: 0x040009C8 RID: 2504
		private const string BUDDY_ALREADY_ON_LIST = "ErrorMessages/BuddyAlreadyOnList";
	}
}

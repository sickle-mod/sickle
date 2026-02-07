using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000361 RID: 865
	[JsonConverter(typeof(StringEnumConverter))]
	[Preserve]
	public enum Error
	{
		// Token: 0x0400122B RID: 4651
		[EnumMember(Value = "None")]
		None,
		// Token: 0x0400122C RID: 4652
		[EnumMember(Value = "UnspecifiedError")]
		UnspecifiedError,
		// Token: 0x0400122D RID: 4653
		[EnumMember(Value = "ValidationError")]
		ValidationError,
		// Token: 0x0400122E RID: 4654
		[EnumMember(Value = "RegisterError_LoginAlreadyInUseTransferNeeded")]
		RegisterErrorLoginAlreadyInUseTransferNeeded,
		// Token: 0x0400122F RID: 4655
		[EnumMember(Value = "RegisterError_EmailAlreadyInUseTransferNeeded")]
		RegisterErrorEmailAlreadyInUseTransferNeeded,
		// Token: 0x04001230 RID: 4656
		[EnumMember(Value = "RegisterError_LoginAlreadyInUse")]
		RegisterErrorLoginAlreadyInUse,
		// Token: 0x04001231 RID: 4657
		[EnumMember(Value = "RegisterError_EmailAlreadyInUse")]
		RegisterErrorEmailAlreadyInUse,
		// Token: 0x04001232 RID: 4658
		[EnumMember(Value = "RegisterLegacyError_UserNotFound")]
		RegisterLegacyErrorUserNotFound,
		// Token: 0x04001233 RID: 4659
		[EnumMember(Value = "RegisterLegacyError_NotLegacyUser")]
		RegisterLegacyErrorNotLegacyUser,
		// Token: 0x04001234 RID: 4660
		[EnumMember(Value = "RegisterLegacyError_AlreadyTransferred")]
		RegisterLegacyErrorAlreadyTransferred,
		// Token: 0x04001235 RID: 4661
		[EnumMember(Value = "RegisterLegacyError_InvalidEmail")]
		RegisterLegacyErrorInvalidEmail,
		// Token: 0x04001236 RID: 4662
		[EnumMember(Value = "ResendActivationEmailError_UserByLoginNotFound")]
		ResendActivationEmailErrorUserByLoginNotFound,
		// Token: 0x04001237 RID: 4663
		[EnumMember(Value = "ResendActivationEmailError_UserAlreadyActivated")]
		ResendActivationEmailErrorUserAlreadyActivated,
		// Token: 0x04001238 RID: 4664
		[EnumMember(Value = "LoginError_ValidationFailed")]
		LoginErrorValidationFailed,
		// Token: 0x04001239 RID: 4665
		[EnumMember(Value = "LoginError_TransferNeeded")]
		LoginErrorTransferNeeded,
		// Token: 0x0400123A RID: 4666
		[EnumMember(Value = "LoginError_AuthenticationFailed")]
		LoginErrorAuthenticationFailed,
		// Token: 0x0400123B RID: 4667
		[EnumMember(Value = "LoginError_ActivationNeeded")]
		LoginErrorActivationNeeded,
		// Token: 0x0400123C RID: 4668
		[EnumMember(Value = "ResetPasswordError_UserNotFound")]
		ResetPasswordErrorUserNotFound,
		// Token: 0x0400123D RID: 4669
		[EnumMember(Value = "ResetPasswordError_TransferNeeded")]
		ResetPasswordErrorTransferNeeded,
		// Token: 0x0400123E RID: 4670
		[EnumMember(Value = "ResetPasswordError_SamePassword")]
		ResetPasswordErrorSamePassword,
		// Token: 0x0400123F RID: 4671
		[EnumMember(Value = "ResendPasswordResetEmailError_UserNotFound")]
		ResendPasswordResetEmailErrorUserNotFound,
		// Token: 0x04001240 RID: 4672
		[EnumMember(Value = "ChangePasswordError_InvalidOldPassword")]
		ChangePasswordErrorInvalidOldPassword,
		// Token: 0x04001241 RID: 4673
		[EnumMember(Value = "ChangePasswordError_SamePassword")]
		ChangePasswordErrorSamePassword,
		// Token: 0x04001242 RID: 4674
		[EnumMember(Value = "ChangeEmailError_SameEmail")]
		ChangeEmailErrorSameEmail,
		// Token: 0x04001243 RID: 4675
		[EnumMember(Value = "ChangeEmailError_EmailAlreadyInUse")]
		ChangeEmailErrorEmailAlreadyInUse,
		// Token: 0x04001244 RID: 4676
		[EnumMember(Value = "ChangeEmailError_InvalidPassword")]
		ChangeEmailErrorInvalidPassword,
		// Token: 0x04001245 RID: 4677
		[EnumMember(Value = "DeleteAccountError_InvalidPassword")]
		DeleteAccountErrorInvalidPassword,
		// Token: 0x04001246 RID: 4678
		[EnumMember(Value = "LinkAccountError_UnsupportedPlatform")]
		LinkAccountErrorUnsupportedPlatform,
		// Token: 0x04001247 RID: 4679
		[EnumMember(Value = "LinkAccountError_InvalidTicket")]
		LinkAccountErrorInvalidTicket,
		// Token: 0x04001248 RID: 4680
		[EnumMember(Value = "LinkAccountError_AlreadyLinked")]
		LinkAccountErrorAlreadyLinked,
		// Token: 0x04001249 RID: 4681
		[EnumMember(Value = "UnlinkAccountError_UnsupportedPlatform")]
		UnlinkAccountErrorUnsupportedPlatform,
		// Token: 0x0400124A RID: 4682
		[EnumMember(Value = "UnlinkAccountError_NotLinked")]
		UnlinkAccountErrorNotLinked,
		// Token: 0x0400124B RID: 4683
		[EnumMember(Value = "AddBuddyError_CantAddYourself")]
		AddBuddyErrorCantAddYourself,
		// Token: 0x0400124C RID: 4684
		[EnumMember(Value = "AddBuddyError_AlreadyOnBuddyList")]
		AddBuddyErrorAlreadyOnBuddyList,
		// Token: 0x0400124D RID: 4685
		[EnumMember(Value = "AddBuddyError_BuddyUserNotFound")]
		AddBuddyErrorBuddyUserNotFound,
		// Token: 0x0400124E RID: 4686
		[EnumMember(Value = "AddBuddyByNameError_BuddyUserNotFound")]
		AddBuddyByNameErrorBuddyUserNotFound,
		// Token: 0x0400124F RID: 4687
		[EnumMember(Value = "AddBuddyByNameError_CantAddYourself")]
		AddBuddyByNameErrorCantAddYourself,
		// Token: 0x04001250 RID: 4688
		[EnumMember(Value = "AddBuddyByNameError_AlreadyOnBuddyList")]
		AddBuddyByNameErrorAlreadyOnBuddyList,
		// Token: 0x04001251 RID: 4689
		[EnumMember(Value = "RemoveBuddyError_CantRemoveYourself")]
		RemoveBuddyErrorCantRemoveYourself,
		// Token: 0x04001252 RID: 4690
		[EnumMember(Value = "RemoveBuddyError_BuddyUserNotFound")]
		RemoveBuddyErrorBuddyUserNotFound,
		// Token: 0x04001253 RID: 4691
		[EnumMember(Value = "RemoveBuddyError_NotOnBuddyList")]
		RemoveBuddyErrorNotOnBuddyList
	}
}

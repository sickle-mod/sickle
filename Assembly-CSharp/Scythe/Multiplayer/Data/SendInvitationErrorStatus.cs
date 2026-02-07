using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000321 RID: 801
	public enum SendInvitationErrorStatus
	{
		// Token: 0x040010E7 RID: 4327
		InvitationAlreadySent,
		// Token: 0x040010E8 RID: 4328
		InvitationAlreadyReceived,
		// Token: 0x040010E9 RID: 4329
		InvitationAlreadyAccepted,
		// Token: 0x040010EA RID: 4330
		PlayerNotFound,
		// Token: 0x040010EB RID: 4331
		CannotAddYourself,
		// Token: 0x040010EC RID: 4332
		UnknownError
	}
}

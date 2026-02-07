using System;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007A7 RID: 1959
	public class NoOnlineConnectionStatus : RequiredOnlineStatus
	{
		// Token: 0x0600388B RID: 14475 RVA: 0x0004C58C File Offset: 0x0004A78C
		public override void MeetRequirements(Action onSuccess, Action onFailure)
		{
			this.SetCallbacks(onSuccess, onFailure);
			base.CallOnSuccess();
		}
	}
}

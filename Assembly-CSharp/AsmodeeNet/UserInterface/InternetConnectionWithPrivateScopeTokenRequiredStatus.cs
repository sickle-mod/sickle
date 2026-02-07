using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007A1 RID: 1953
	public class InternetConnectionWithPrivateScopeTokenRequiredStatus : RequiredOnlineStatus
	{
		// Token: 0x06003864 RID: 14436 RVA: 0x0014A7FC File Offset: 0x001489FC
		public override void MeetRequirements(Action onSuccess, Action onFailure)
		{
			this.SetCallbacks(onSuccess, onFailure);
			OAuthGate oauthGate = CoreApplication.Instance.OAuthGate;
			if (!CoreApplication.Instance.CommunityHub.IsNetworkReachable)
			{
				base.CallOnFailure();
				return;
			}
			if (oauthGate.HasPrivateToken)
			{
				base.CallOnSuccess();
				return;
			}
			oauthGate.GetPrivateAccessToken(false, delegate(OAuthError error)
			{
				if (error == null)
				{
					base.CallOnSuccess();
					return;
				}
				base.CallOnFailure();
			});
		}
	}
}

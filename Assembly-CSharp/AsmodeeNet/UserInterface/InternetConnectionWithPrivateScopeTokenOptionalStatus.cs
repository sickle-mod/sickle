using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007A0 RID: 1952
	public class InternetConnectionWithPrivateScopeTokenOptionalStatus : RequiredOnlineStatus
	{
		// Token: 0x06003861 RID: 14433 RVA: 0x0014A7A0 File Offset: 0x001489A0
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
			oauthGate.GetPrivateAccessToken(true, delegate(OAuthError error)
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

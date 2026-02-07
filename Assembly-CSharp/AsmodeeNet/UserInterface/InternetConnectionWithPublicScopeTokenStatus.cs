using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007A2 RID: 1954
	public class InternetConnectionWithPublicScopeTokenStatus : RequiredOnlineStatus
	{
		// Token: 0x06003867 RID: 14439 RVA: 0x0014A858 File Offset: 0x00148A58
		public override void MeetRequirements(Action onSuccess, Action onFailure)
		{
			this.SetCallbacks(onSuccess, onFailure);
			OAuthGate oauthGate = CoreApplication.Instance.OAuthGate;
			if (!CoreApplication.Instance.CommunityHub.IsNetworkReachable)
			{
				base.CallOnFailure();
				return;
			}
			if (oauthGate.HasPublicToken)
			{
				base.CallOnSuccess();
				return;
			}
			oauthGate.GetPublicAccessToken(delegate(OAuthError error)
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

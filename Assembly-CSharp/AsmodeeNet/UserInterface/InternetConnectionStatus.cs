using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200079E RID: 1950
	public class InternetConnectionStatus : RequiredOnlineStatus
	{
		// Token: 0x0600385D RID: 14429 RVA: 0x0014A730 File Offset: 0x00148930
		public override void MeetRequirements(Action onSuccess, Action onFailure)
		{
			this.SetCallbacks(onSuccess, onFailure);
			InternetConnectionStatus.Method method = this.method;
			if (method != InternetConnectionStatus.Method.ApplicationInternetReachability && method == InternetConnectionStatus.Method.HttpRequestToURL)
			{
				CoreApplication.Instance.StartCoroutine(WebChecker.WebRequest(delegate
				{
					base.CallOnSuccess();
				}, delegate
				{
					base.CallOnFailure();
				}, this.targetURL));
				return;
			}
			if (CoreApplication.Instance.CommunityHub.IsNetworkReachable)
			{
				base.CallOnSuccess();
				return;
			}
			base.CallOnFailure();
		}

		// Token: 0x04002A54 RID: 10836
		public InternetConnectionStatus.Method method;

		// Token: 0x04002A55 RID: 10837
		public string targetURL;

		// Token: 0x0200079F RID: 1951
		public enum Method
		{
			// Token: 0x04002A57 RID: 10839
			ApplicationInternetReachability,
			// Token: 0x04002A58 RID: 10840
			HttpRequestToURL
		}
	}
}

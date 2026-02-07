using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200091D RID: 2333
	public class OnlineFeatureAddRemoveMultipleEndpoint : Endpoint
	{
		// Token: 0x06003F1F RID: 16159 RVA: 0x00050818 File Offset: 0x0004EA18
		public OnlineFeatureAddRemoveMultipleEndpoint(int userId, string[] featuresToAdd, string[] featuresToRemove, OAuthGate oauthGate = null)
			: base(false, oauthGate)
		{
			base.DebugModuleName += ".User.OnlineFeatures";
			base._URL = string.Format("/main/v1/user/{0}/features", userId);
			this.CtorCore(featuresToAdd, featuresToRemove);
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x00050857 File Offset: 0x0004EA57
		public OnlineFeatureAddRemoveMultipleEndpoint(string[] featuresToAdd, string[] featuresToRemove, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.OnlineFeatures";
			base._URL = "/main/v1/user/me/features";
			this.CtorCore(featuresToAdd, featuresToRemove);
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x0015B1F4 File Offset: 0x001593F4
		private void CtorCore(string[] featuresToAdd, string[] featuresToRemove)
		{
			if ((featuresToAdd == null || featuresToAdd.Length == 0) && (featuresToRemove == null || featuresToRemove.Length == 0))
			{
				throw new ArgumentException("'featuresToAdd' and 'featuresToRemove' arguments are both optional, but both cannot be omitted");
			}
			base._Parameters = new Hashtable();
			if (featuresToAdd != null && featuresToAdd.Length != 0)
			{
				base._Parameters.Add("add", featuresToAdd);
			}
			if (featuresToRemove != null && featuresToRemove.Length != 0)
			{
				base._Parameters.Add("remove", featuresToRemove);
			}
			base._HttpMethod = HTTPMethods.Patch;
		}
	}
}

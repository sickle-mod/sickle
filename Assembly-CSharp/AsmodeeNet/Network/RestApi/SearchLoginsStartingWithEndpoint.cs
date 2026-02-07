using System;
using UnityEngine.Networking;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000928 RID: 2344
	public class SearchLoginsStartingWithEndpoint : BaseSearchByLoginEndpoint
	{
		// Token: 0x06003F44 RID: 16196 RVA: 0x0015B9E4 File Offset: 0x00159BE4
		public SearchLoginsStartingWithEndpoint(string loginStart, Extras extras, int offset = -1, int limit = -1, OAuthGate oauthGate = null)
			: base(oauthGate)
		{
			base.DebugModuleName += ".User.Search";
			if (string.IsNullOrEmpty(loginStart))
			{
				throw new ArgumentException(BaseSearchByLoginEndpoint.kNullOrEmptyLoginException);
			}
			if (loginStart.Length < 2)
			{
				throw new ArgumentException(string.Format(BaseSearchByLoginEndpoint.kLoginToShortException, 2));
			}
			loginStart += "%";
			base._URL = string.Format("/main/v1/users?login={0}", UnityWebRequest.EscapeURL(loginStart));
			base.CtorCore(extras, offset, limit);
		}

		// Token: 0x04003081 RID: 12417
		public const int kLoginMinimalLength = 2;
	}
}

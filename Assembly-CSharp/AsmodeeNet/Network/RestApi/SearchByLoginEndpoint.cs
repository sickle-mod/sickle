using System;
using System.Linq;
using UnityEngine.Networking;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000929 RID: 2345
	public class SearchByLoginEndpoint : BaseSearchByLoginEndpoint
	{
		// Token: 0x06003F45 RID: 16197 RVA: 0x0015BA70 File Offset: 0x00159C70
		public SearchByLoginEndpoint(string[] logins, Extras extras, int offset = -1, int limit = -1, OAuthGate oauthGate = null)
			: base(oauthGate)
		{
			base.DebugModuleName += ".User.Search";
			if (logins == null || logins.Length == 0)
			{
				throw new ArgumentException(BaseSearchByLoginEndpoint.kEmptyLoginArrayException);
			}
			if (logins.Any((string x) => string.IsNullOrEmpty(x)))
			{
				throw new ArgumentException(BaseSearchByLoginEndpoint.kNullOrEmptyLoginException);
			}
			if (logins.Any((string x) => x.Length < 4))
			{
				throw new ArgumentException(string.Format(BaseSearchByLoginEndpoint.kLoginToShortException, 4));
			}
			base._URL = string.Format("/main/v1/users?login={0}", string.Join(",", logins.Select((string x) => UnityWebRequest.EscapeURL(x)).ToArray<string>()));
			base.CtorCore(extras, offset, limit);
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x0015BB6C File Offset: 0x00159D6C
		public SearchByLoginEndpoint(string login, Extras extras, int offset = -1, int limit = -1, OAuthGate oauthGate = null)
			: base(oauthGate)
		{
			base.DebugModuleName += ".User.Search";
			if (string.IsNullOrEmpty(login))
			{
				throw new ArgumentException(BaseSearchByLoginEndpoint.kNullOrEmptyLoginException);
			}
			if (login.Length < 4)
			{
				throw new ArgumentException(string.Format(BaseSearchByLoginEndpoint.kLoginToShortException, 4));
			}
			base._URL = string.Format("/main/v1/users?login={0}", UnityWebRequest.EscapeURL(login));
			base.CtorCore(extras, offset, limit);
		}

		// Token: 0x04003082 RID: 12418
		public const int kLoginMinimalLength = 4;
	}
}

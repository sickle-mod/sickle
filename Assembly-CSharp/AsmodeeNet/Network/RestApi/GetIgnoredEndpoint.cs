using System;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000914 RID: 2324
	public class GetIgnoredEndpoint : EndpointWithPaginatedResponse<BuddyOrIgnored>
	{
		// Token: 0x06003F06 RID: 16134 RVA: 0x0015AB58 File Offset: 0x00158D58
		public GetIgnoredEndpoint(int offset = -1, int limit = -1, OAuthGate oauthGate = null)
			: base(oauthGate, true)
		{
			base.DebugModuleName += ".User.Ignore";
			base._URL = "/main/v1/user/me/ignore";
			string text = "?";
			if (offset > 0)
			{
				text = text + "offset=" + offset.ToString() + "&";
			}
			if (limit > 0)
			{
				text = text + "limit=" + limit.ToString() + "&";
			}
			text = text.Substring(0, text.Length - 1);
			base._URL += text;
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x0015ABF0 File Offset: 0x00158DF0
		protected override void ProcessResponse(Action<PaginatedResult<BuddyOrIgnored>, WebError> onCompletion)
		{
			ApiGetBuddiesOrIgnoredResponse apiGetBuddiesOrIgnoredResponse = JsonUtility.FromJson<ApiGetBuddiesOrIgnoredResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<BuddyOrIgnored> paginatedResult = new PaginatedResult<BuddyOrIgnored>(apiGetBuddiesOrIgnoredResponse.data.total, apiGetBuddiesOrIgnoredResponse.data.ignored.Select((ApiGetBuddiesOrIgnoredResponse.Data.BuddyOrIgnored x) => new BuddyOrIgnored(x.id, x.login_name)).ToArray<BuddyOrIgnored>(), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.next), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.prev), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.first), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.last));
			if (onCompletion != null)
			{
				onCompletion(paginatedResult, null);
			}
		}
	}
}

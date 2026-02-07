using System;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000885 RID: 2181
	public class GetBuddyEndpoint : EndpointWithPaginatedResponse<BuddyOrIgnored>
	{
		// Token: 0x06003D04 RID: 15620 RVA: 0x0015765C File Offset: 0x0015585C
		public GetBuddyEndpoint(int offset = -1, int limit = -1, OAuthGate oauthGate = null)
			: base(oauthGate, true)
		{
			base.DebugModuleName += ".User.Buddies";
			base._URL = "/main/v1/user/me/buddies";
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

		// Token: 0x06003D05 RID: 15621 RVA: 0x001576F4 File Offset: 0x001558F4
		protected override void ProcessResponse(Action<PaginatedResult<BuddyOrIgnored>, WebError> onCompletion)
		{
			ApiGetBuddiesOrIgnoredResponse apiGetBuddiesOrIgnoredResponse = JsonUtility.FromJson<ApiGetBuddiesOrIgnoredResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<BuddyOrIgnored> paginatedResult = new PaginatedResult<BuddyOrIgnored>(apiGetBuddiesOrIgnoredResponse.data.total, apiGetBuddiesOrIgnoredResponse.data.buddies.Select((ApiGetBuddiesOrIgnoredResponse.Data.BuddyOrIgnored x) => new BuddyOrIgnored(x.id, x.login_name)).ToArray<BuddyOrIgnored>(), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.next), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.prev), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.first), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.last));
			if (onCompletion != null)
			{
				onCompletion(paginatedResult, null);
			}
		}
	}
}

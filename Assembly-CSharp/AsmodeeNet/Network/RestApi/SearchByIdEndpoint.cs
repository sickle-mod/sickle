using System;
using System.Linq;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000924 RID: 2340
	public class SearchByIdEndpoint : EndpointWithPaginatedResponse<UserSearchResult>
	{
		// Token: 0x06003F37 RID: 16183 RVA: 0x0015B60C File Offset: 0x0015980C
		public SearchByIdEndpoint(int[] ids, Extras extras = Extras.None, int offset = -1, int limit = -1, OAuthGate oauthGate = null)
			: base(oauthGate, false)
		{
			base.DebugModuleName += ".User.Search";
			if (ids == null || ids.Length == 0)
			{
				throw new ArgumentException("'ids' parameter cannot be null or empty");
			}
			base._URL = "/main/v1/users?ids=" + string.Join(",", ids.Select((int x) => x.ToString()).ToArray<string>());
			string text = "&extras=avatar,";
			if (extras != Extras.None && extras != Extras.Partners)
			{
				if ((extras & Extras.Boardgames) != Extras.None)
				{
					text += "boardgames,";
				}
				if ((extras & Extras.Onlinegames) != Extras.None)
				{
					text += "onlinegames,";
				}
				if ((extras & Extras.Features) != Extras.None)
				{
					text += "features,";
				}
			}
			text = text.Substring(0, text.Length - 1);
			base._URL += text;
			if (offset > 0)
			{
				base._URL = base._URL + "&offset=" + offset.ToString();
			}
			if (limit > 0)
			{
				base._URL = base._URL + "&limit=" + limit.ToString();
			}
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x0015B73C File Offset: 0x0015993C
		protected override void ProcessResponse(Action<PaginatedResult<UserSearchResult>, WebError> onCompletion)
		{
			ApiSearchUserResponse apiSearchUserResponse = JsonUtility.FromJson<ApiSearchUserResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<UserSearchResult> paginatedResult = new PaginatedResult<UserSearchResult>(apiSearchUserResponse.data.total, apiSearchUserResponse.data.users.Select((ApiSearchUserResponse.Data.User x) => new UserSearchResult(x)).ToArray<UserSearchResult>(), base._LinkSetter(apiSearchUserResponse.data._links.next), base._LinkSetter(apiSearchUserResponse.data._links.prev), base._LinkSetter(apiSearchUserResponse.data._links.first), base._LinkSetter(apiSearchUserResponse.data._links.last));
			if (onCompletion != null)
			{
				onCompletion(paginatedResult, null);
			}
		}
	}
}

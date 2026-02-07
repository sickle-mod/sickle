using System;
using System.Linq;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200092B RID: 2347
	public class SearchByPartnerEndpoint : EndpointWithPaginatedResponse<UserSearchResult>
	{
		// Token: 0x06003F4C RID: 16204 RVA: 0x0015BBEC File Offset: 0x00159DEC
		public SearchByPartnerEndpoint(int partnerId, string partnerUser, Extras extras = Extras.None, OAuthGate oauthGate = null)
			: base(oauthGate, false)
		{
			base.DebugModuleName += ".User.Search";
			if (string.IsNullOrEmpty(partnerUser))
			{
				throw new ArgumentException("'partnerUser' cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/users?partner={0}&partner_user={1}", partnerId, partnerUser);
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
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x0015BCAC File Offset: 0x00159EAC
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
